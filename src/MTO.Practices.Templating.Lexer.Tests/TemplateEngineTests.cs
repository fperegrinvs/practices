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

            Assert.AreEqual(@"oi {""Content"":null,""Name"":""jaspion"",""IsActive"":true,""ElementStatus"":0,""Arguments"":[{""Key"":""argumento"",""Value"":""oi""}]} tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void OneCommand()
        {
            var template = "oi $jiraya(ServerName)$ tchau";
            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi {""Content"":null,""Name"":""jiraya"",""IsActive"":false,""ElementStatus"":0,""Arguments"":[{""Key"":""ServerName"",""Value"":null}]} tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void ManyArguments()
        {
            var template = "oi $bundleCss.Add(/oi.css).AddMini(teste.css).Add(jiban.css)$ tchau";
            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi {""Content"":null,""Name"":""bundleCss"",""IsActive"":false,""ElementStatus"":0,""Arguments"":[{""Key"":""Add"",""Value"":""jiban.css""},{""Key"":""AddMini"",""Value"":""teste.css""},{""Key"":""Add"",""Value"":""/oi.css""}]} tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void ManyArgumentsMultiLine()
        {
            var template = @"oi $bundleCss.Add(/oi.css)
                                            .AddMini(teste.css)
                                            .Add(jiban.css)$ tchau";
            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi {""Content"":null,""Name"":""bundleCss"",""IsActive"":false,""ElementStatus"":0,""Arguments"":[{""Key"":""Add"",""Value"":""jiban.css""},{""Key"":""AddMini"",""Value"":""teste.css""},{""Key"":""Add"",""Value"":""/oi.css""}]} tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void EscapedDollar()
        {
            var template = @"oi $bundleCss.Add(/\$oi.css)
                                            .AddMini(teste.css)
                                            .Add(jiban.css)$ tchau";
            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi {""Content"":null,""Name"":""bundleCss"",""IsActive"":false,""ElementStatus"":0,""Arguments"":[{""Key"":""Add"",""Value"":""jiban.css""},{""Key"":""AddMini"",""Value"":""teste.css""},{""Key"":""Add"",""Value"":""/$oi.css""}]} tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void NestedCommand()
        {
            var template = "oi $jiraya($manabu(ServerName)$)$ tchau";
            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi {""Content"":null,""Name"":""jiraya"",""IsActive"":false,""ElementStatus"":0,""Arguments"":[{""Key"":""{\""Content\"":null,\""Name\"":\""manabu\"",\""IsActive\"":false,\""ElementStatus\"":0,\""Arguments\"":[{\""Key\"":\""ServerName\"",\""Value\"":null}]}"",""Value"":null}]} tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void TagWithCommandInArgument()
        {
            var template = "oi <mto:jiraya whatever=\"$manabu(ServerName)$\" /> tchau";
            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi {""Content"":null,""Name"":""jiraya"",""IsActive"":true,""ElementStatus"":0,""Arguments"":[{""Key"":""whatever"",""Value"":""{\""Content\"":null,\""Name\"":\""manabu\"",\""IsActive\"":false,\""ElementStatus\"":0,\""Arguments\"":[{\""Key\"":\""ServerName\"",\""Value\"":null}]}""}]} tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void TagWithContent()
        {
            var template = "oi <mto:teste> bom dia </mto:teste> tchau";

            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi {""Content"":null,""Name"":""teste"",""IsActive"":true,""ElementStatus"":0,""Arguments"":[]} bom dia  tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void TagWithCommandInContent()
        {
            var template = "oi <mto:reverse> $bom(dia)$ </mto:reverse> tchau";

            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi  }]}llun:""eulaV"",""aid"":""yeK""{[:""stnemugrA"",0:""sutatStnemelE"",eslaf:""evitcAsI"",""mob"":""emaN"",llun:""tnetnoC""{  tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void TagContent()
        {
            var template = "oi <mto:reverse>Bom dia</mto:reverse> tchau";

            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi aid moB tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void NestedTag()
        {
            var template = "<mto:reverse>uahct <mto:reverse>Bom dia</mto:reverse> io</mto:reverse>";

            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi Bom dia tchau", parser.ProcessTokenList());
        }

        [TestMethod]
        public void NumberInsideTag()
        {
            var template = "<mto:upper>Computador R$ 3000</mto:upper>";

            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"COMPUTADOR R$ 3000", parser.ProcessTokenList());
        }

        [TestMethod]
        public void JqueryInsideTag()
        {
            var template = "<mto:upper>$('.classe')</mto:upper>.show();";

            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"$('.CLASSE').show();", parser.ProcessTokenList());
        }

        /// <summary>
        /// Teste da funcionalidade de skip
        /// </summary>
        [TestMethod]
        public void TagSkip()
        {
            var template = "oi <mto:skip>erro</mto:skip> tchau";

            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi  tchau", parser.ProcessTokenList());
        }

        /// <summary>
        /// Teste da funcionalidade de dump
        /// </summary>
        [TestMethod]
        public void TagDump()
        {
            var template = "oi <mto:dump><mto:upper>erro</mto:upper> bom dia</mto:dump> tchau";

            var parser = new Parser(Scanner.ParseString(template), new TestEngine());

            Assert.AreEqual(@"oi <mto:upper>erro</mto:upper> bom dia tchau", parser.ProcessTokenList());
        }

    }
}
