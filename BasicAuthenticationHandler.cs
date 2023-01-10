using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;


namespace FunctionTestLegacyServer
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration
            ) : base (options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                Response.StatusCode = 401;
                return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }

            var headerValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var bytes = Convert.FromBase64String(headerValue.Parameter);
            var credentials = Encoding.UTF8.GetString(bytes);

            if (!string.IsNullOrWhiteSpace(credentials))
            {
                string[] array = credentials.Split(":");
                var username = array[0];
                var password = array[1];
               

                if (username == _configuration["User"] && password == _configuration["Password"])
                {
                    var claim = new[] { new Claim(ClaimTypes.Name, username) };
                    var identity = new ClaimsIdentity(claim, Scheme.Name);
                    var principle = new ClaimsPrincipal(identity);

                    return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principle, Scheme.Name)));
                }
            }
           
            return await Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
            
        }
    }
}
