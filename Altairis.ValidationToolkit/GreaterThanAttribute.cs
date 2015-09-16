﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace Altairis.ValidationToolkit {

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class GreaterThanAttribute : ValidationAttribute {

        public GreaterThanAttribute(string otherPropertyName)
            : base("{0} must be greater than {1}") {
            this.OtherPropertyName = otherPropertyName;
        }

        public bool AllowEqual { get; set; }

        public string OtherPropertyDisplayName { get; set; }

        public string OtherPropertyName { get; private set; }

        public override bool RequiresValidationContext {
            get {
                return true;
            }
        }

        public override string FormatErrorMessage(string name) {
            return string.Format(this.ErrorMessageString, name, this.OtherPropertyDisplayName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            // Get values
            var comparableValue = value as IComparable;
            IComparable comparableOtherValue;
            try {
                comparableOtherValue = validationContext.GetPropertyValue<IComparable>(this.OtherPropertyName);
            }
            catch (ArgumentException aex) {
                throw new InvalidOperationException("Other property not found", aex);
            }

            // Empty or noncomparable values are valid - let others validate that
            if (comparableValue == null || comparableOtherValue == null) return ValidationResult.Success;

            var compareResult = comparableValue.CompareTo(comparableOtherValue);
            if (compareResult == 1 || (this.AllowEqual && compareResult == 0)) {
                // This property is greater than other property or equal when permitted
                return ValidationResult.Success;
            }
            else {
                // This property is smaller or equal to the other property
                if (string.IsNullOrWhiteSpace(this.OtherPropertyDisplayName)) {
                    this.OtherPropertyDisplayName = validationContext.GetPropertyDisplayName(this.OtherPropertyName);
                }
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }
        }

    }
}