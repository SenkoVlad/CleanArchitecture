using CleanArchitecture.Application.Interfaces;
using MediatR;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Behaviors
{
    public class LoginBehavior<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        ICurrentUserService currentUserService;
        public LoginBehavior(ICurrentUserService currentUser) =>
            this.currentUserService = currentUser;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = typeof(TRequest).Name;
            var userId = currentUserService.UserId;
            Log.Information("Notes Request: {Name} {@UserId} {@Request}", requestName, userId, request);
            var response = await next();

            return response;
        }
    }
}
