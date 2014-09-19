﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Altairis.ValidationToolkit {

    internal static class ExtensionMethods {

        public static object GetPropertyValue(this ValidationContext validationContext, string propertyName) {
            var propertyInfo = validationContext.ObjectType.GetProperty(propertyName);
            if (propertyInfo != null) {
                return propertyInfo.GetValue(validationContext.ObjectInstance, null);
            }
            return null;
        }

        public static T GetPropertyValue<T>(this ValidationContext validationContext, string propertyName) where T : class {
            return validationContext.GetPropertyValue(propertyName) as T;
        }
    }
}