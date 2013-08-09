namespace MTO.Practices.Common.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MTO.Practices.Common.Crypto;

    /// <summary>
    /// The crypto tests.
    /// </summary>
    [TestClass]
    public class CryptoTests
    {
        /// <summary>
        /// Gera chave, criptografa o texto e depois recupera o texto
        /// </summary>
        [TestMethod]
        public void AsymetricRoundTrip()
        {
            const string Data = "Hello World";
            string pub;
            string priv;

            AsymmetricEncryption.GenerateKeys(2048, out pub, out priv);

            var crypto = AsymmetricEncryption.EncryptText(Data, pub);
            var result = AsymmetricEncryption.DecryptText(crypto, priv);

            Assert.AreEqual(Data, result);
        }

        /// <summary>
        /// Teste simples de decriptação para validar se o sistema continua compatível com as chaves em uso.
        /// </summary>
        [TestMethod]
        public void AsymetricDecrypt()
        {
            var crypted = "JOYjKv63wJyBSDEFjUOdKw4h7LAkGDvZzKm1T6iv7PM5EH73i9r28eaeibkqhF3wMJpev3BL6p6dGZoY/GwTVHbLeuO8bxy4sgHmyJ1m+pgCphKxeSoxzvfdl10DQ/OoQP1ugf3y901J/JPjJR+5eqPRjJ4dhu9UKgm+6aXWXzFANr0bh+i374hNuQo3ggElIkjVnqgpYGRDGSEGSbXT1f60eROtV/CTSWeDzFCJ8hBvPCAvYKAnRfzBuxMHmdsLt9gz45CLQaEVdkYH2c/Yv97zggk3c4wJvWtalGJBx9zLbfkxbD20tfiUsjVgE+R2KLRzt46ksSaM3NlmOsqmRg==";
            var priv = "<RSAKeyValue><Modulus>qWpbvvQnDJ6T4ckee+EyrlyzGtv6db9M/nS+5nwvtWubzkbo5W2J78Tk/hG3z+XZo/+HEab/vcKBdiJPrUi4Cq9BJm9hcDvHUZKRpF5AX3YNNEvqJwNlZK64Yg8DRAWfHIvZZyXpqCKVynkzuZdV8UrtyiUeuAQzKsMPodWs/HAUe31H4DaVfvybm9pQeLywj3SKROg3E31ftnxAzXq1W+NJKrmHnh21Otj9ZFOPu9oic3QdrAUM+B3QVrdTYSPT8YAXqW/hA+agkvhxdZU15ZpSemRAD0/UQ1CaO3aKvZ93jYUdzebraMva5SuXY1DJClPTbj6G7pfTxtnA7YMTSQ==</Modulus><Exponent>AQAB</Exponent><P>03CwMNV9kaXOR8w6uRrPJtIXYTLXO874Xbj6dGTVSn0NMehZLr/gJyK+kcliso2R5fZjjUjYSVQFx/EuYmISZv5Fe4XF6uwil/AECw5efuld+q6ny4v3ov4PkJikkIrMwVHhtrP1CkPo8piXFFCsf9UQtGi67WBXtuosUZrB5hM=</P><Q>zR5p4Sz+hzx9i/41goNZUtTQfh3A/ssqryKPj4/1dHQxLWc3MbxHfIkyQfIjflSUYRRV7gH3IPOQ2/3lbIBW7Bu6V+x6lhzRLWM8DklSYBYLoiMJ/UC+TuJWasKLxbzJ91dJbpcIIgiIqdNl2bR2tC8xqvRzpiJCYV3smOlKfLM=</Q><DP>EkiFQ8PCNuzyeGzliwoRbZbvJJ2D1Q0hluEe7x+k/erwUCd6Rruaw2eSuQKEAVKSkiMLGrV41mZolZB6ZVF7q+JC2dqIcbb3itHbV6VncGv3j2y437X7tOFpMSSx3jnSWMkeznCjOL8Ejq25Kq2LLQacii0gNNSG4S9Ao9HRAHc=</DP><DQ>ajg/uD3mqP2oPiCscSO30+8k8MpTsR5gwkTxdvSjtWSeOtbifDz2AAXEIuZTfw4psIQpY5Fc+pFnCKVYc3GakbZiLznk2Wue3xQue2942w1PhO0ENUObyZnoCm0omIOFSzLGciCAM6+bpeY1LiIH/pMZ81+XrAa9vVI4PY9B5YU=</DQ><InverseQ>0juz4PoTGVQBDzysPoYT3/0IZnLrtTekNZlqkQrkMH82rG0cVef2UQwuZIcEFs/V1NRHxOatnzSww112FhleRooX7LreEU/8HL2rW/firryqLjRcpOEJrasR5f5Omgf8JjqtQnG69wbFcBADl1DPtA1SNYyARLJZr3a0XsAp/fA=</InverseQ><D>ArIth7t5pQmx2ukNpqLicxZR2eoSaDclNCqT4JE/gNQXoks537jNLO38kFftaPvEe9DZZw5wyZOjwaK6kyffvVRcN9fdM0718LG9D73HYFRiP41yk4vuyexQcrVSmHtOtLBgMPQZbeAvl+kOkuG6Sy7KJPN0IXqKfQ2mU/aenC/HyqCYHxFCKx4t7rlsX60/I+AcDcbU8ISWYj7gMAJMR813ndHfw1XzNEuh8zpiAqzA+wjAKOrdqWvuaOZhMHfzw16/G7skDqmmSo6oYOVmRg0UPv0WWzMv272MzdgpTyIgWmnLQ9c1tt0mNqYwqWA6dz4Kk3M4dhuzy+1f0vv4nw==</D></RSAKeyValue>";
            var result = AsymmetricEncryption.DecryptText(crypted, priv);

            Assert.AreEqual("Hello World", result);
        }
    }
}
