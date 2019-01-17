using global::Xamarin.Forms;

namespace Alat.Xamarin.Forms.Validation {
   public class ErrorLabel : Label {

      private static ValidationErrorConverter ValidationErrorConverter { get; } = new ValidationErrorConverter();
      private static StringToVisibilityConverter StringToVisibilityConverter { get; } = new StringToVisibilityConverter();

      private Binding TextBinding { get; set; }
      private Binding VisibilityBinding { get; set; }

      public static readonly BindableProperty ErrorKeyProperty =
         BindableProperty.Create(nameof(ErrorKey), typeof(string), typeof(ErrorLabel), null,
            propertyChanged: (b, o, n) => { ((ErrorLabel)b).OnErrorKeyChanged(); });

      public string ErrorKey {
         get { return (string)GetValue(ErrorKeyProperty); }
         set { SetValue(ErrorKeyProperty, value); }
      }

      private void OnErrorKeyChanged() {
         var textBinding = new Binding("ValidationContext", 
            BindingMode.OneWay, 
            converter: ValidationErrorConverter, 
            converterParameter: ErrorKey);

         SetBinding(TextProperty, textBinding);

         var visibilityBinding = new Binding("ValidationContext", 
            BindingMode.OneWay, 
            converter: StringToVisibilityConverter, 
            converterParameter: ErrorKey);

         SetBinding(IsVisibleProperty, visibilityBinding);
      }
   }
}
