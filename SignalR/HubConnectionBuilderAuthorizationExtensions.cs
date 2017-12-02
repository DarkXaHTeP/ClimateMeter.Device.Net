using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace ClimateMeter.Device.Net.SignalR
{
    public static class HubConnectionBuilderAuthorizationExtensions
    {
        public static IHubAuthenticationBuilder WithJWTAuthentication(this IHubConnectionBuilder builder, Func<string> accessTokenProvider)
        {
            return new HubAuthenticationBuilder(accessTokenProvider, builder);
        }
        
        public static IHubAuthenticationBuilder WithJWTAuthentication(this IHubConnectionBuilder builder, Func<Task<string>> accessTokenProvider)
        {
            return new HubAuthenticationBuilder(() => accessTokenProvider().GetAwaiter().GetResult(), builder);
        }
    }
}
