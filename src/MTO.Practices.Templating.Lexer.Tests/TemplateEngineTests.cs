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

            Assert.AreEqual(@"oi {""Name"":""jaspion"",""ElementStatus"":0,""Arguments"":{""argumento"":""oi""}} tchau", parser.ProcessTokenList());
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

            Assert.AreEqual(@"oi tchau", parser.ProcessTokenList());
        }
    }
}
