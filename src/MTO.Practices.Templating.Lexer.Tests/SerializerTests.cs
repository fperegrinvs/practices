namespace MTO.Practices.Templating.Lexer.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MTO.Practices.Templating.Lexer.StateMachine;
    using MTO.Practices.Templating.Lexer.Tests.Engine;

    [TestClass]
    public class SerializerTests
    {
        [TestMethod]
        public void RoundTripTest()
        {
            var template = @"oi $bundleCss.Add(/\$oi.css)
                                            .AddMini(teste.css)
                                            .Add(jiban.css)$ tchau";
            var tokenList = Scanner.ParseString(template);
            var parsedOriginal = new Parser(tokenList, new TestEngine()).ProcessTokenList();

            var binary = tokenList.Serialize();
            var restored = binary.Deserialize();

            var parsedRestored = new Parser(restored, new TestEngine()).ProcessTokenList();

            Assert.AreEqual(parsedOriginal, parsedRestored);
        }
    }
}
