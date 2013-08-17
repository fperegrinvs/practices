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

%x TAG  // inicio de tag
%x TAG_ARG   //argumento
%x TAG_ARG_VALUE // valor da tag (pode ter subcomando e pa)
%x TAG_NAME   //nome da tag
%x COMMENT   //comentario
%x URL
%x COMMAND   // MTO Template Engine command
%x COMMAND_ARG // argumento de comando
%x COMMAND_ARG_NAME // nome de argumento de comando
%x COMMAND_ARG_VALUE // valor do argumetno do comando
%x COMMAND_CONTENT // conteúdo do comando (tipo em $url(/oi)$ é o /oi)

%%
%{
    // local variables
%}

<INITIAL>\<\!{Ident}+									{ this.AddToken(Tokens.Content, yytext); }
<INITIAL,TAG>{EOL}     									{ this.AddToken(Tokens.NewLine, yytext); }
<INITIAL,TAG>\$\{[A-Za-z_]+\}       					{ this.AddToken(Tokens.Property, yytext.Substring(2, yytext.Length -3)); }
<INITIAL,TAG>\{[^\}]+\}         						{ this.AddToken(Tokens.Content, yytext); }
<INITIAL,TAG>[^\$<>\{\}]+       						{ this.AddToken(Tokens.Content, yytext); }
<INITIAL,TAG>\$[ \t]*[0-9\.\(]+ 						{ this.AddToken(Tokens.Content, yytext); }
<INITIAL,TAG>\<mto:              						{ this.AddToken(Tokens.OpenMtoTag, yytext); yy_push_state(TAG); yy_push_state(TAG_NAME); }
<INITIAL,TAG>\<a[ \t]+[^h]*href[ \t]*=[\"\' ]? 			{ this.AddToken(Tokens.Content, yytext); yy_push_state(URL); }
<URL>\$\{[A-Z_]+\}				    					{ this.AddToken(Tokens.Property, yytext.Substring(2, yytext.Length -3)); yy_pop_state(); }
<URL>[^ \"\'\{\$\}]+									{ this.AddToken(Tokens.Url, yytext); yy_pop_state(); }
<TAG_NAME>[A-Za-z]+             						{ this.AddToken(Tokens.TagName, yytext); yy_pop_state(); yy_push_state(TAG_ARG); }
<TAG_ARG>[ \t]+                 						{  }
<TAG_ARG>{Ident}=[\"\']	        						{ var idx = yytext.IndexOf('='); this.AddToken(Tokens.TagArg, yytext.Remove(idx)); yy_push_state(TAG_ARG_VALUE); }
<TAG_ARG_VALUE>[^\'\"\\\$]+								{ this.AddToken(Tokens.Content, yytext); }
<TAG_ARG_VALUE>\\[^\$\'\"]								{ this.AddToken(Tokens.Content, yytext); }
<TAG_ARG_VALUE>\\[\$\'\"]								{ this.AddToken(Tokens.Content, yytext.Substring(1, 1)); }
<TAG_ARG_VALUE>[\'\"]		     						{ this.AddToken(Tokens.CloseTagArg, yytext); yy_pop_state(); }
<TAG_ARG>\>                     						{ this.AddToken(Tokens.TagCloseArg, yytext); yy_pop_state(); }
<TAG_ARG>/\>                    						{ this.AddToken(Tokens.CloseMtoTag, yytext); yy_pop_state(); yy_pop_state(); }
<TAG>\/mto:[A-Za-z]+(\>)?           					{ this.AddToken(Tokens.CloseMtoTag, "</" + yytext); yy_pop_state();}
<TAG>\<\/mto:[A-Za-z]+(\>)?         					{ this.AddToken(Tokens.CloseMtoTag, yytext); yy_pop_state();}
<COMMENT>\-\-\>                     					{ this.AddToken(Tokens.CommentEnd, yytext); yy_pop_state(); }
<COMMENT>[^\-]+                     					{ this.AddToken(Tokens.Content, yytext); }
<COMMENT>\-[^\-]+                   					{ this.AddToken(Tokens.Content, yytext); }
<INITIAL,TAG>\<\!\-\-           						{ this.AddToken(Tokens.CommentStart, yytext); yy_push_state(COMMENT); }
<INITIAL,TAG>\<(\/)?[A-Za-tv-z][A-Za-z]+  				{ this.AddToken(Tokens.Content, yytext); }
<INITIAL,TAG>\<(\/)?u[A-Za-rt-z][A-Za-z]* 				{ this.AddToken(Tokens.Content, yytext); }
<INITIAL,TAG>\<(\/)?[B-Zb-z][\t \>] 					{ this.AddToken(Tokens.Content, yytext); }
<INITIAL,TAG>\<\/[Aa][\t \>] 							{ this.AddToken(Tokens.Content, yytext); }
<INITIAL,TAG>(\/)?\>   									{ this.AddToken(Tokens.CloseMtoTag, yytext); }
<INITIAL,TAG,COMMAND,COMMAND_ARG_VALUE,COMMAND_CONTENT,TAG_ARG_VALUE>\$[a-zA-Z]+ { this.AddToken(Tokens.OpenCommand, yytext.Substring(1, yytext.Length -1)); yy_push_state(COMMAND); }
<COMMAND>\.                         					{ this.AddToken(Tokens.OpenCommandArg, yytext); yy_push_state(COMMAND_ARG); yy_push_state(COMMAND_ARG_NAME); }
<COMMAND>\$												{ this.AddToken(Tokens.CloseCommand, yytext); yy_pop_state(); }
<COMMAND>\(												{ this.AddToken(Tokens.OpenCommandContent, yytext); yy_push_state(COMMAND_CONTENT); }
<COMMAND_ARG_NAME>[a-zA-Z]+								{ this.AddToken(Tokens.Content, yytext); yy_pop_state(); }
<COMMAND_ARG>\(											{ this.AddToken(Tokens.OpenComandArgValue, yytext); yy_push_state(COMMAND_ARG_VALUE); }
<COMMAND_ARG_VALUE,COMMAND_CONTENT>[^\)\\\$]+			{ this.AddToken(Tokens.Content, yytext); }
<COMMAND_ARG_VALUE,COMMAND_CONTENT>\\[^\$\)]			{ this.AddToken(Tokens.Content, yytext); }
<COMMAND_ARG_VALUE,COMMAND_CONTENT>\\[\$\)]				{ this.AddToken(Tokens.Content, yytext.Substring(1, 1)); }
<COMMAND_ARG_VALUE>\)									{ this.AddToken(Tokens.CloseCommandContent, yytext); yy_pop_state(); yy_pop_state(); }
<COMMAND_CONTENT>\)										{ this.AddToken(Tokens.CloseCommandContent, yytext); yy_pop_state(); }
<<EOF>>                			 						{ this.AddToken(Tokens.EOF, yytext); }
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
