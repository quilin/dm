using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace DM.Services.Search.Consumer.Interceptors;

internal class IdentityInterceptor : Interceptor
{
    private readonly IIdentitySetter identitySetter;

    public IdentityInterceptor(
        IIdentitySetter identitySetter)
    {
        this.identitySetter = identitySetter;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        identitySetter.Current = Identity.Guest();
        var response = await continuation.Invoke(request, context);
        return response;
    }
}