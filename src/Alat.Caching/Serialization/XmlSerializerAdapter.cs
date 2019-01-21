using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Alat.Caching.Serialization {
   internal class XmlSerializerAdapter : ISerializer {
      private readonly object poolLock = new object();

      private IDictionary<Type, XmlSerializer> SerializerPool { get; }

      public XmlSerializerAdapter() {
         SerializerPool = new Dictionary<Type, XmlSerializer>();
      }


      public TResult Deserialize<TResult>(Stream serializationStream) {
         var serializer = GetInternalSerializer(typeof(TResult));
         return (TResult)serializer.Deserialize(serializationStream);
      }

      public void Serialize(Stream serializationStream, object graph) {
         var serializer = GetInternalSerializer(graph.GetType());
         serializer.Serialize(serializationStream, graph);
      }

      private XmlSerializer GetInternalSerializer(Type type) {
         lock(poolLock) {
            if (!SerializerPool.ContainsKey(type)) {
               SerializerPool.Add(type, new XmlSerializer(type));
            }
         }
         
         return SerializerPool[type];
      }
   }
}
