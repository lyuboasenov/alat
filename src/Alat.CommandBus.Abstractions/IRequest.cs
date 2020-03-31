using System;

namespace Alat.CommandBus.Abstractions {
   public interface IRequest {
   }

   public interface IRequest<TResult> : IRequest {
   }

   public interface IProgressRequest<TProgress> : IRequest {
      IProgress<TProgress> Progress { get; }
   }

   public interface IProgressRequest<TResult, TProgress> : IRequest<TResult>, IProgressRequest<TProgress> {
   }
}
