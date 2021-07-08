using System;

namespace CleanArchitecture.Application.Interfaces
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
    }
}
