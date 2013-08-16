using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTO.Practices.Templating.Lexer.Tests
{
    using MTO.Practices.Templating.Lexer.StateMachine;
    using MTO.Practices.Templating.Lexer.Tests.Engine;

    [TestClass]
    public class TemplateEngineTests
    {
        [TestMethod]
        public void SimpleTemplate()
        {
            var template = "oi mundo";
            var parser = new Parser(Scanner.ParseString(template), new TestEngine());
            
            Assert.AreEqual(template, parser.ProcessTokenList());
        }

        [TestMethod]
        public void OneTag()
        {
            var template = "oi <mto:jaspion argumento=\"oi\" /> tchau";

            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi {""Name"":""jaspion"",""ElementStatus"":0,""Arguments"":[{""Key"":""argumento"",""Value"":""oi""}]} tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void OneCommand()
        {
            var template = "oi $jiraya(ServerName)$ tchau";
            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi {""Name"":""jiraya"",""ElementStatus"":0,""Arguments"":[{""Key"":""ServerName"",""Value"":null}]} tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void ManyArguments()
        {
            var template = "oi $bundleCss.Add(/oi.css).AddMini(teste.css).Add(jiban.css)$ tchau";
            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi {""Name"":""bundleCss"",""ElementStatus"":0,""Arguments"":[{""Key"":""Add"",""Value"":""jiban.css""},{""Key"":""AddMini"",""Value"":""teste.css""},{""Key"":""Add"",""Value"":""/oi.css""}]} tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void ManyArgumentsMultiLine()
        {
            var template = @"oi $bundleCss.Add(/oi.css)
                                            .AddMini(teste.css)
                                            .Add(jiban.css)$ tchau";
            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi {""Name"":""bundleCss"",""ElementStatus"":0,""Arguments"":[{""Key"":""Add"",""Value"":""jiban.css""},{""Key"":""AddMini"",""Value"":""teste.css""},{""Key"":""Add"",""Value"":""/oi.css""}]} tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void EscapedDollar()
        {
            var template = @"oi $bundleCss.Add(/\$oi.css)
                                            .AddMini(teste.css)
                                            .Add(jiban.css)$ tchau";
            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi {""Name"":""bundleCss"",""ElementStatus"":0,""Arguments"":[{""Key"":""Add"",""Value"":""jiban.css""},{""Key"":""AddMini"",""Value"":""teste.css""},{""Key"":""Add"",""Value"":""/$oi.css""}]} tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void NestedCommand()
        {
            var template = "oi $jiraya($manabu(ServerName)$)$ tchau";
            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi {""Name"":""jiraya"",""ElementStatus"":0,""Arguments"":[{""Key"":""{\""Name\"":\""manabu\"",\""ElementStatus\"":0,\""Arguments\"":[{\""Key\"":\""ServerName\"",\""Value\"":null}]}"",""Value"":null}]} tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void TagWithCommandInArgument()
        {
            var template = "oi <mto:jiraya whatever=\"$manabu(ServerName)$\" /> tchau";
            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi {""Name"":""jiraya"",""ElementStatus"":0,""Arguments"":[{""Key"":""whatever"",""Value"":""{\""Name\"":\""manabu\"",\""ElementStatus\"":0,\""Arguments\"":[{\""Key\"":\""ServerName\"",\""Value\"":null}]}""}]} tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void TagWithContent()
        {
            var template = "oi <mto:teste> bom dia </mto:teste> tchau";

            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi {""Name"":""teste"",""ElementStatus"":0,""Arguments"":[]} bom dia  tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void TagWithCommandInContent()
        {
            var template = "oi <mto:teste> $bom(dia)$ </mto:teste> tchau";

            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi {""Name"":""teste"",""ElementStatus"":0,""Arguments"":[]} {""Name"":""bom"",""ElementStatus"":0,""Arguments"":[{""Key"":""dia"",""Value"":null}]}  tchau", parser.ProcessTokenList());
        }
    }
}
