using Alat.Validation.Rules;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Alat.Validation {
   public interface IValidationContext {
      bool IsValid { get; set; }

      void AddRule(string propertyName, params IRule[] rules);
      void AddRule<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda, params IRule[] rules)
         where TSource : IValidatable;

      bool HasErrors(string propertyName);
      IEnumerable<string> GetErrors(string propertyName);

      void Validate();
   }
}