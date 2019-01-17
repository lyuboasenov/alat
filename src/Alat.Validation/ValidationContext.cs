using Alat.Validation.Rules;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Alat.Validation {
   public interface ValidationContext {
      bool IsValid { get; set; }

      void AddRule(string propertyName, params Rule[] rules);
      void AddRule<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda, params Rule[] rules)
         where TSource : Validatable;

      bool HasErrors(string propertyName);
      IEnumerable<string> GetErrors(string propertyName);

      void Validate();
   }
}