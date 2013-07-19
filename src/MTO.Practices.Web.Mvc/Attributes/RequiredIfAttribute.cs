namespace MTO.Practices.Web.Mvc.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RequiredIfAttribute : RequiredAttribute
    {
        private String PropertyName { get; set; }
        private Object DesiredValue { get; set; }

        public RequiredIfAttribute(String propertyName, Object desiredvalue, String errormessage)
        {
            this.PropertyName = propertyName;
            this.DesiredValue = desiredvalue;
            this.ErrorMessage = errormessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            Object instance = context.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty(this.PropertyName).GetValue(instance, null);
            if (proprtyvalue != null && proprtyvalue.ToString() == this.DesiredValue.ToString())
            {
                return base.IsValid(value, context);
            }

            return ValidationResult.Success;
        }
    }
}