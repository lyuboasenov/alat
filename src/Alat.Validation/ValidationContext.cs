using Alat.Validation.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Alat.Validation {
   public class ValidationContext : IValidationContext, IDisposable {
      private bool autoValidate = false;
      private bool disposedValue = false; // To detect redundant calls

      public bool IsValid { get; set; }
      public bool AutoValidate {
         get {
            return autoValidate;
         }
         set {
            AutoValidateChangeValue(value);
         }
      }

      private IValidatable Target { get; }
      private IDictionary<string, IEnumerable<string>> ValidationErrors { get; }
      private IDictionary<string, IEnumerable<IRule>> ValidationRules { get; }
      private IDictionary<string, PropertyInfo> ValueExtracters { get; }

      public ValidationContext(IValidatable target, bool autoValidate = false) {
         Target = target ?? throw new ArgumentNullException(nameof(target));
         AutoValidate = autoValidate;

         ValidationErrors = new Dictionary<string, IEnumerable<string>>();
         ValidationRules = new Dictionary<string, IEnumerable<IRule>>();
         ValueExtracters = new Dictionary<string, PropertyInfo>();
      }

      ~ValidationContext() {
         // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
         Dispose(false);
      }

      public void AddRule(string propertyName, params IRule[] rules) {
         if (string.IsNullOrEmpty(propertyName)) {
            throw new ArgumentNullException(nameof(propertyName));
         }

         if (null == rules || rules.Length == 0) {
            throw new ArgumentNullException(nameof(rules));
         }

         AddRule(GetPropertyInfo(propertyName), rules: rules);
      }

      public void AddRule<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda, params IRule[] rules) 
         where TSource : IValidatable  {
         if (null == propertyLambda) {
            throw new ArgumentNullException(nameof(propertyLambda));
         }

         if (!(propertyLambda.Body is MemberExpression)) {
            throw new ArgumentException($"Expression '{nameof(propertyLambda)}' refers to a method, not a property");
         }

         if (null == rules || rules.Length == 0) {
            throw new ArgumentNullException(nameof(rules));
         }

         AddRule(GetPropertyInfo(propertyLambda), rules);
      }

      public void Validate() {
         foreach(var key in ValidationRules.Keys) {
            ValidateProperty(key);
         }

         Target.OnValidationContextChanged();
      }

      public void Validate(string propertyName) {
         ValidateProperty(propertyName);

         Target.OnValidationContextChanged();
      }

      public bool HasErrors(string propertyName) {
         return ValidationErrors.ContainsKey(propertyName);
      }

      public IEnumerable<string> GetErrors(string propertyName) {
         IEnumerable<string> result = null;
         if (HasErrors(propertyName)) {
            result = ValidationErrors[propertyName];
         } else {
            result = Array.Empty<string>();
         }

         return result;
      }

      public void Dispose() {
         // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      private void ValidateProperty(string name) {
         var value = ValueExtracters[name].GetValue(Target);
         ValidateProperty(name, value);
      }

      private void ValidateProperty(string name, object value) {
         if (ValidationRules.Count == 0) {
            throw new ArgumentNullException(nameof(ValidationRules));
         }

         if (ValidationRules.ContainsKey(name)) {
            ValidationErrors.Remove(name);
            var errors = new List<string>();

            foreach (var validationRule in ValidationRules[name]) {
               validationRule.Validate(name, value);
               if (!validationRule.IsValid) {
                  errors.Add(validationRule.ErrorMessage);
               }
            }

            if (errors.Count > 0) {
               ValidationErrors[name] = errors;
            }
         }

         UpdateIsValid();
      }

      private void UpdateIsValid() {
         var result = false;
         if (null != ValidationRules) {
            var invalidRules = ValidationRules.Values.Sum(vrs => vrs.Count(vr => !vr.IsValid));

            result = invalidRules == 0;
         }

         IsValid = result;
      }

      private void AddRule(PropertyInfo propertyInfo, params IRule[] rules) {
         ValidationRules.Add(propertyInfo.Name, rules);
         ValueExtracters.Add(propertyInfo.Name, propertyInfo);
      }

      private static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
         where TSource : IValidatable {
         Type type = typeof(TSource);

         MemberExpression member = propertyLambda.Body as MemberExpression;
         if (member == null)
            throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property");

         PropertyInfo propInfo = member.Member as PropertyInfo;
         if (propInfo == null)
            throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");

         return propInfo;
      }

      private PropertyInfo GetPropertyInfo(string propertyName) {
         if (string.IsNullOrEmpty(propertyName)) {
            throw new ArgumentNullException(nameof(propertyName));
         }

         return Target.GetType().GetProperty(propertyName);
      }

      private void AutoValidateChangeValue(bool changeToValue) {
         if (changeToValue && !(Target is INotifyPropertyChanged)) {
            throw new ArgumentException($"{nameof(Target)} does not implement {nameof(INotifyPropertyChanged)}");
         }

         if (changeToValue) {
            WireValidationOnPropertyChanged();
         } else {
            UnwireValidationOnPropertyChanged();
         }

         autoValidate = changeToValue;
      }

      private void UnwireValidationOnPropertyChanged() {
         var notifiableTarget = Target as INotifyPropertyChanged;
         notifiableTarget.PropertyChanged -= TargetPropertyChanged;
      }

      private void WireValidationOnPropertyChanged() {
         var notifiableTarget = Target as INotifyPropertyChanged;
         notifiableTarget.PropertyChanged += TargetPropertyChanged;
      }

      protected virtual void Dispose(bool disposing) {
         if (!disposedValue) {
            if (disposing) {
               AutoValidate = false;
            }
            disposedValue = true;
         }
      }

      private void TargetPropertyChanged(object sender, PropertyChangedEventArgs e) {
         Validate(e.PropertyName);
      }
   }
}
