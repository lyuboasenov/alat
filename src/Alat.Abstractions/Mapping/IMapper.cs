namespace Alat.Abstractions.Mapping {
   public interface IMapper {
      TDest Map<TDest>(object src);
      TDest Map<TSrc, TDest>(TSrc src);
   }
}
