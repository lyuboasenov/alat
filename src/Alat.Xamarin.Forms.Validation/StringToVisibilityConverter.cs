using Alat.Validation;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Alat.Xamarin.Forms.Validation {
   public class StringToVisibilityConverter : IValueConverter {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
         var validationContext = value as IValidationContext;
         var propertyName = parameter as string;

         return null != validationContext &&
            !string.IsNullOrEmpty(propertyName) &&
            validationContext.HasErrors((string)parameter);
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
         throw new NotImplementedException();
      }
   }
}
