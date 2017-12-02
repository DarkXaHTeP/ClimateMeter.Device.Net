using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace ClimateMeter.Device.Net.SignalR
{
    public interface IHubAuthenticationBuilder
    {
        IHubAuthenticationBuilder WithTokenQueryStringParameterName(string queryStringParameterName);
        IHubConnectionBuilder WithUrl(string url);
        IHubConnectionBuilder WithUrl(Uri url);
    }
}
