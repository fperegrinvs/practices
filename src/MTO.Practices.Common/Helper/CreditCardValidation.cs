namespace MTO.Practices.Common.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MTO.Practices.Common.Enumerators;

    /// <summary>
    /// Valida cartões de crédito
    /// </summary>
    public class CreditCardValidation
    {
        /// <summary>
        /// Dicionário com regras para validar cada cartão
        /// </summary>
        private static readonly Dictionary<CreditCardTypeEnum, Func<string, bool>> CardValidationRules;

        /// <summary>
        /// evita multiplicar o número por 2 e somar os dígitos
        /// </summary>
        private static readonly int[] Odd = new[] { 0, 2, 4, 6, 8, 1, 3, 5, 7, 9 };

        /// <summary>
        /// Initializes static members of the <see cref="CreditCardValidation"/> class.
        /// </summary>
        static CreditCardValidation()
        {
            CardValidationRules = new Dictionary<CreditCardTypeEnum, Func<string, bool>>
                {
                    { CreditCardTypeEnum.Visa, number => ValidateCardLuhn(number, new[] { 13, 16 }) },
                    { CreditCardTypeEnum.Master, number => ValidateCardLuhn(number, new[] { 16 }) },
                    { CreditCardTypeEnum.Amex, number => ValidateCardLuhn(number, new[] { 15 }) },
                    { CreditCardTypeEnum.Diners, number => ValidateCardLuhn(number, new[] { 14, 16 }) },
                    { CreditCardTypeEnum.Discover, number => ValidateCardLuhn(number, new[] { 16 }) }
                };
        }

        /// <summary>
        /// Valida um tipo específico de cartao
        /// </summary>
        /// <param name="cardNumber">Número do cartão</param>
        /// <returns>true caso seja válido, false em caso contrário</returns>
        public static bool ValidateCard(string cardNumber)
        {
            var cardType = IdentifyCreditCardType(cardNumber);
            return CardValidationRules[cardType].Invoke(cardNumber);
        }

        /// <summary>
        /// Valida um tipo específico de cartao
        /// </summary>
        /// <param name="cardType">Bandeira do cartão de crédito</param>
        /// <param name="cardNumber">Número do cartão</param>
        /// <returns>true caso seja válido, false em caso contrário</returns>
        public static bool ValidateCard(CreditCardTypeEnum cardType, string cardNumber)
        {
            return IdentifyCreditCardType(cardNumber) == cardType && CardValidationRules[cardType].Invoke(cardNumber);
        }

        /// <summary>
        /// Valida um cartão de crédito qualquer (sem informar o tipo de cartão
        /// </summary>
        /// <param name="cardNumber">número do cartão</param>
        /// <param name="cardType">tipo do cartão (saida)</param>
        /// <returns>true caso o cartão seja válido, false em caso contrário</returns>
        public static bool ValidateGenercCreditCard(string cardNumber, out CreditCardTypeEnum cardType)
        {
            cardNumber = RemoveSeparators(cardNumber);
            cardType = IdentifyCreditCardType(cardNumber, true);
            return CardValidationRules[cardType].Invoke(cardNumber);
        }

        /// <summary>
        /// Identifica a bandeira do cartão de crédito
        /// </summary>
        /// <param name="cardNumber">número do cartão</param>
        /// <param name="isStringProcessed">indica se a string já foi processada ou nào</param>
        /// <returns>Tipo de cartao de crédito encotrado.</returns>
        public static CreditCardTypeEnum IdentifyCreditCardType(string cardNumber, bool isStringProcessed = false)
        {
            if (!isStringProcessed)
            {
                cardNumber = RemoveSeparators(cardNumber);
            }

            if (cardNumber.StartsWith("4", StringComparison.Ordinal))
            {
                return CreditCardTypeEnum.Visa;
            }

            if (cardNumber.StartsWith("36", StringComparison.Ordinal) || cardNumber.StartsWith("38", StringComparison.Ordinal) || (cardNumber.StartsWith("30", StringComparison.Ordinal) && cardNumber[2] >= '0' && cardNumber[2] < '6'))
            {
                return CreditCardTypeEnum.Diners;
            }

            if (cardNumber.StartsWith("34", StringComparison.Ordinal) || cardNumber.StartsWith("37", StringComparison.Ordinal))
            {
                return CreditCardTypeEnum.Amex;
            }

            // 51 a 55
            if (cardNumber.StartsWith("5", StringComparison.Ordinal) && cardNumber[1] > '0' && cardNumber[1] < '6' && cardNumber.Length == 16)
            {
                return CreditCardTypeEnum.Master;
            }

            if (cardNumber.StartsWith("6011", StringComparison.Ordinal))
            {
                return CreditCardTypeEnum.Discover;
            }

            return CreditCardTypeEnum.Desconhecido;
        }

        /// <summary>
        /// Aplica módulo de 10 no cartão
        /// </summary>
        /// <param name="number">número do cartão</param>
        /// <param name="lengths">tamanhos permitidos</param>
        /// <returns>true caso o cartão seja válido, false em caso contrário</returns>
        private static bool ValidateCardLuhn(string number, int[] lengths)
        {
            if (!lengths.Any(length => length == number.Length))
            {
                return false;
            }

            var sum = 0;

            // soma índices pares
            for (var i = number.Length - 1; i >= 0; i -= 2)
            {
                // neste caso , xor vai ser equivalente a subtração number[i] - '0' = numero
                sum += number[i] ^ '0';
            }

            // soma do dobro dos impares (quando o resultado tem 2 dígitos, eles devem ser somados).
            // Sintetizando a conta em um aray para acelerar o cálculo.
            for (var i = number.Length - 2; i >= 0; i -= 2)
            {
                sum += Odd[number[i] ^ '0'];
            }

            return sum % 10 == 0;
        }

        /// <summary>
        /// Remove os agrupadores
        /// </summary>
        /// <param name="number">número do cartão</param>
        /// <returns>número sem os agrupadores / separadores</returns>
        private static string RemoveSeparators(string number)
        {
            return number.Replace(".", "").Replace(" ", "").Replace("-", "");
        }
    }
}
