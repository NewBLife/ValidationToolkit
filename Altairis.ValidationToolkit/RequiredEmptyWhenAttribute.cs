﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Altairis.ValidationToolkit {

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredEmptyWhenAttribute : ValidationAttribute {

        public RequiredEmptyWhenAttribute(string otherPropertyName, object otherPropertyValue)
            : base("Field {0} is required to be empty") {
            this.OtherPropertyName = otherPropertyName;
            this.OtherPropertyValue = otherPropertyValue;
        }

        public bool NegateCondition { get; set; }

        public string OtherPropertyName { get; set; }

        public object OtherPropertyValue { get; set; }

        public override bool RequiresValidationContext {
            get { return true; }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            // Always succeed on empty value
            if (value == null) return ValidationResult.Success;

            // Get other property value
            var currentOtherValue = validationContext.GetPropertyValue(this.OtherPropertyName);

            // Compare to it
            if (object.Equals(this.OtherPropertyValue, currentOtherValue) == this.NegateCondition) return ValidationResult.Success;

            // Return error
            return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
        }
    }
}