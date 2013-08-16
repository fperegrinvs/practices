/*
   Using LEX to implement a simple case of the unix "wc" utility.
 */

%using UpStore.Practices.Parser; 
%namespace UpStore.Practices.Parser.StateMachine.Slot
%option codePage:utf-8, noEmbedBuffers, verbose, stack

%{
%}

Space           [ \t]
EOL				\n|\r\n?
Ident           [a-zA-Z_][a-zA-Z0-9_]*
StringDQ		\"([^\"\\]+(\\\")?)+\"        
StringSQ        \'([^\'\\]+(\\\')?)+\'

%x COMMAND  //UpStore command
%x COMMAND_ARG   //UpStore command
%x COMMAND_NAME   //UpStore command
%x COMMENT   //UpStore command
%x URL

%%
%{
    // local variables
%}


<INITIAL>\<\!{Ident}+				{ this.AddToken(Tokens.Content, yytext); }
<INITIAL,COMMAND>{EOL}     			{ this.AddToken(Tokens.NewLine, yytext); }
<INITIAL,COMMAND>\$\{[A-Za-z_]+\}       { this.AddToken(Tokens.Property, yytext.Substring(2, yytext.Length -3)); }
<INITIAL,COMMAND>\{[^\}]+\}         { this.AddToken(Tokens.Content, yytext); }
<INITIAL,COMMAND>[^\$<>\{\}]+       { this.AddToken(Tokens.Content, yytext); }
<INITIAL,COMMAND>\$                 { this.AddToken(Tokens.Content, yytext); }
<INITIAL,COMMAND>\<us:              { this.AddToken(Tokens.CommandOpenTag, yytext); yy_push_state(COMMAND); yy_push_state(COMMAND_NAME); }
<INITIAL,COMMAND>\<a[ \t]+[^h]*href[ \t]*=[\"\' ]? { this.AddToken(Tokens.Content, yytext); yy_push_state(URL); }
<URL>\$\{[A-Z_]+\}				    { this.AddToken(Tokens.Property, yytext.Substring(2, yytext.Length -3)); yy_pop_state(); }
<URL>[^ \"\'\{\$\}]+				{ this.AddToken(Tokens.Url, yytext); yy_pop_state(); }
<COMMAND_NAME>[A-Za-z]+             { this.AddToken(Tokens.CommandName, yytext); yy_pop_state(); yy_push_state(COMMAND_ARG); }
<COMMAND_ARG>[ \t]+                 {  }
<COMMAND_ARG>{Ident}={Ident}        { this.AddToken(Tokens.CommandArg, yytext); }
<COMMAND_ARG>{Ident}={StringSQ}     { this.AddToken(Tokens.CommandArg, yytext); }
<COMMAND_ARG>{Ident}={StringDQ}     { this.AddToken(Tokens.CommandArg, yytext); }
<COMMAND_ARG>\>                     { this.AddToken(Tokens.CommandCloseArg, yytext); yy_pop_state(); }
<COMMAND_ARG>/\>                    { this.AddToken(Tokens.CommandCloseTag, yytext); yy_pop_state(); yy_pop_state(); }
<COMMAND>\/us:[A-Za-z]+(\>)?            { this.AddToken(Tokens.CommandCloseTag, yytext.Substring(4, yytext.Length - 5)); yy_pop_state();}
<COMMAND>\<\/us:[A-Za-z]+(\>)?            { this.AddToken(Tokens.CommandCloseTag, yytext.Substring(5, yytext.Length - 6)); yy_pop_state();}
<COMMENT>\-\-\>                     { this.AddToken(Tokens.CommentEnd, yytext); yy_pop_state(); }
<COMMENT>[^\-]+                     { this.AddToken(Tokens.Content, yytext); }
<COMMENT>\-[^\-]+                   { this.AddToken(Tokens.Content, yytext); }
<INITIAL,COMMAND>\<\!\-\-           { this.AddToken(Tokens.CommentStart, yytext); yy_push_state(COMMENT); }
<INITIAL,COMMAND>\<(\/)?[A-Za-tv-z][A-Za-z]+  { this.AddToken(Tokens.Content, yytext); }
<INITIAL,COMMAND>\<(\/)?u[A-Za-rt-z][A-Za-z]* { this.AddToken(Tokens.Content, yytext); }
<INITIAL,COMMAND>\<(\/)?[B-Zb-z][\t \>] { this.AddToken(Tokens.Content, yytext); }
<INITIAL,COMMAND>\<\/[Aa][\t \>] { this.AddToken(Tokens.Content, yytext); }
<INITIAL,COMMAND>(\/)?\>   			{ this.AddToken(Tokens.CloseTag, yytext); }
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
