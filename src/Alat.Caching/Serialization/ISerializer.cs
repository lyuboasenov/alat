using System.IO;

namespace Alat.Caching.Serialization {
   public interface ISerializer {
      void Serialize(Stream serializationStream, object graph);
      TResult Deserialize<TResult>(Stream serializationStream);
   }
}
