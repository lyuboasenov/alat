using Alat.Validation;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Alat.Xamarin.Forms.Validation {
   public class ValidationErrorConverter : IValueConverter {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
         var validationContext = value as IValidationContext;
         var propertyName = parameter as string;

         string result = null;
         
         if (null != validationContext &&
            !string.IsNullOrEmpty(propertyName) &&
            validationContext.HasErrors(propertyName)) {
            result = string.Join(Environment.NewLine, validationContext.GetErrors(propertyName));
         }

         return result;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
         throw new NotImplementedException();
      }
   }
}
