using Alat.Abstractions.Mapping;
using System;

namespace Alat.Infrastructure.Services.Mapping {
   public class Mapper : IMapper {
      private readonly AutoMapper.IMapper _mapper;

      public Mapper(AutoMapper.IMapper mapper) {
         _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      }

      public TDest Map<TDest>(object src) {
         return _mapper.Map<TDest>(src);
      }

      public TDest Map<TSrc, TDest>(TSrc src) {
         return Map<TDest>(src);
      }
   }
}
