/*
   Using LEX to implement a simple case of the unix "wc" utility.
 */

%using MTO.Practices.Templating.Lexer; 
%namespace MTO.Practices.Templating.Lexer.StateMachine
%option codePage:utf-8, noEmbedBuffers, verbose, stack

%{
%}

Space           [ \t]
EOL				\n|\r\n?
Ident           [a-zA-Z_][a-zA-Z0-9_]*
StringDQ		\"([^\"\\]+(\\\")?)+\"        
StringSQ        \'([^\'\\]+(\\\')?)+\'

%x TAG  //MTO Template Engine tag
%x TAG_ARG   //MTO Template Engine tag
%x TAG_NAME   //MTO Template Engine tag
%x COMMENT   //MTO Template Engine tag
%x URL

%%
%{
    // local variables
%}

<INITIAL>\<\!{Ident}+				{ this.AddToken(Tokens.Content, yytext); }
<INITIAL,TAG>{EOL}     			{ this.AddToken(Tokens.NewLine, yytext); }
<INITIAL,TAG>\$\{[A-Za-z_]+\}       { this.AddToken(Tokens.Property, yytext.Substring(2, yytext.Length -3)); }
<INITIAL,TAG>\{[^\}]+\}         { this.AddToken(Tokens.Content, yytext); }
<INITIAL,TAG>[^\$<>\{\}]+       { this.AddToken(Tokens.Content, yytext); }
<INITIAL,TAG>\$                 { this.AddToken(Tokens.Content, yytext); }
<INITIAL,TAG>\<mto:              { this.AddToken(Tokens.OpenMtoTag, yytext); yy_push_state(TAG); yy_push_state(TAG_NAME); }
<INITIAL,TAG>\<a[ \t]+[^h]*href[ \t]*=[\"\' ]? { this.AddToken(Tokens.Content, yytext); yy_push_state(URL); }
<URL>\$\{[A-Z_]+\}				    { this.AddToken(Tokens.Property, yytext.Substring(2, yytext.Length -3)); yy_pop_state(); }
<URL>[^ \"\'\{\$\}]+				{ this.AddToken(Tokens.Url, yytext); yy_pop_state(); }
<TAG_NAME>[A-Za-z]+             { this.AddToken(Tokens.TagName, yytext); yy_pop_state(); yy_push_state(TAG_ARG); }
<TAG_ARG>[ \t]+                 {  }
<TAG_ARG>{Ident}={Ident}        { this.AddToken(Tokens.TagArg, yytext); }
<TAG_ARG>{Ident}={StringSQ}     { this.AddToken(Tokens.TagArg, yytext); }
<TAG_ARG>{Ident}={StringDQ}     { this.AddToken(Tokens.TagArg, yytext); }
<TAG_ARG>\>                     { this.AddToken(Tokens.TagCloseArg, yytext); yy_pop_state(); }
<TAG_ARG>/\>                    { this.AddToken(Tokens.CloseMtoTag, yytext); yy_pop_state(); yy_pop_state(); }
<TAG>\/mto:[A-Za-z]+(\>)?            { this.AddToken(Tokens.CloseMtoTag, yytext.Substring(5, yytext.Length - 6)); yy_pop_state();}
<TAG>\<\/mto:[A-Za-z]+(\>)?            { this.AddToken(Tokens.CloseMtoTag, yytext.Substring(6, yytext.Length - 7)); yy_pop_state();}
<COMMENT>\-\-\>                     { this.AddToken(Tokens.TagEnd, yytext); yy_pop_state(); }
<COMMENT>[^\-]+                     { this.AddToken(Tokens.Content, yytext); }
<COMMENT>\-[^\-]+                   { this.AddToken(Tokens.Content, yytext); }
<INITIAL,TAG>\<\!\-\-           { this.AddToken(Tokens.CommentStart, yytext); yy_push_state(COMMENT); }
<INITIAL,TAG>\<(\/)?[A-Za-tv-z][A-Za-z]+  { this.AddToken(Tokens.Content, yytext); }
<INITIAL,TAG>\<(\/)?u[A-Za-rt-z][A-Za-z]* { this.AddToken(Tokens.Content, yytext); }
<INITIAL,TAG>\<(\/)?[B-Zb-z][\t \>] { this.AddToken(Tokens.Content, yytext); }
<INITIAL,TAG>\<\/[Aa][\t \>] { this.AddToken(Tokens.Content, yytext); }
<INITIAL,TAG>(\/)?\>   			{ this.AddToken(Tokens.CloseTag, yytext); }
<<EOF>>                			 	{ this.AddToken(Tokens.EOF, yytext); }
%%

     /// <summary>
     /// Posição do scanner
     /// </summary>
     public override int CurrentPosition 
	 { 
		get { return yypos; }
     }

     /// <summary>
     /// Coluna do scanner
     /// </summary>
     public override int CurrentColumn 
	 { 
		get { return yycol; }
     }

     /// <summary>
     /// Coluna atual do scanner
     /// </summary>
     public override int CurrentLine 
	 { 
	    get { return yyline; }
     }

     /// <summary>
     /// Estado atual do scanner
     /// </summary>
    public override StartEnum CurrentStart 
	 { 
	   get { return (StartEnum)YY_START; }
     }


     /// <summary>
     /// Processa conteudo
     /// </summary> 	 
	public static TokenList ParseString(string content)
	{
	    var scnr = new Scanner();
	    scnr.SetSource(content, 0);
	    while (scnr.yylex() != (int)Tokens.EOF)
	    {
	    }

	    return scnr.TokenOutput;
	}
