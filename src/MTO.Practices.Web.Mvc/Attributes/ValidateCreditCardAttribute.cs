﻿namespace MTO.Practices.Web.Mvc.Attributes
{
    using System.ComponentModel.DataAnnotations;

    using MTO.Practices.Common.Helper;

    public class ValidateCreditCardAttribute :  ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (null == value)
            {
                return true;
            }

            var str = (string)value;
            return CreditCardValidation.ValidateCard(str);
        }
    }
}
