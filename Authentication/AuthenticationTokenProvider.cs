using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace ClimateMeter.Device.Net.Authentication
{
    public class AuthenticationTokenProvider
    {
        private readonly ILogger<AuthenticationTokenProvider> _log;
        private readonly AuthenticationSettings _settings;

        private string _accessToken;
        private DateTimeOffset _expiresOn;

        public AuthenticationTokenProvider(IOptions<AuthenticationSettings> options, ILogger<AuthenticationTokenProvider> log)
        {
            _log = log;
            _settings = options.Value;
        }
        
        public string ObtainJwtToken()
        {
            return GetCachedToken() ?? GetNewToken();
        }

        private string GetCachedToken()
        {
            if (_accessToken != null && _expiresOn > DateTimeOffset.UtcNow)
            {
                return _accessToken;
            }

            return null;
        }
        
        private string GetNewToken()
        {
            var credential = new ClientCredential(_settings.ClientId, _settings.ClientSecret);
            
            var authenticationContext = new AuthenticationContext($"{_settings.Instance}/{_settings.TenantId}", false);

            AuthenticationResult result = authenticationContext
                .AcquireTokenAsync(_settings.Resource, credential)
                .GetAwaiter()
                .GetResult();
            
            _log.LogInformation("Acquired new access token");

            _accessToken = result.AccessToken;
            _expiresOn = result.ExpiresOn.AddMinutes(-1);

            return _accessToken;
        }
    }
}