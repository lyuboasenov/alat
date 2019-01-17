using System;
using System.Linq;
using Xunit;

namespace Alat.Logging.Tests.ExceptionDataConverter {
   public class ExceptionDataConverterTests {
      private DataConverters.ExceptionDataConverter DataConverter { get; }

      public ExceptionDataConverterTests() {
         DataConverter = new DataConverters.ExceptionDataConverter();
      }

      [Fact]
      public void ConvertNullObject() {
         Assert.Throws<ArgumentNullException>(() => DataConverter.Convert(null));
      }

      [Fact]
      public void ConvertNotExceptionObject() {
         Assert.Throws<ArgumentException>(() => DataConverter.Convert("dummy"));
      }

      [Fact]
      public void ConvertExceptionObject() {
         var message = "dummy";
         var exception = new Exception(message);
         var data = DataConverter.Convert(exception);

         Assert.Equal(message, data.Message);
         Assert.NotEmpty(data.Properties);
      }

      [Fact]
      public void ConvertNestedExceptionObject() {
         var message = "dummy";
         var exception = new Exception(message, new Exception(message));
         var data = DataConverter.Convert(exception);

         Assert.Equal(message, data.Message);
         Assert.NotEmpty(data.Properties);
         Assert.Contains(data.Properties, (prop) => 
            prop.Name == "InnerException" && null != prop.Children && prop.Children.Any());
      }


   }
}
