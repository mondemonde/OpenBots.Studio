using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Steeroid.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace DevNoteHub
{
    public class JwtAuthentication : Attribute, IAuthenticationFilter
    {
        public string Realm { get; set; }
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            // checking request header value having required scheme "Bearer" or not.
            if (authorization == null || authorization.Scheme != "Bearer" || string.IsNullOrEmpty(authorization.Parameter))
            {
                //context.ErrorResult = new AuthFailureResult("JWT Token is Missing", request);
                return;
           }
            // Getting Token value from header values.
            var token = authorization.Parameter;           
            var principal = await AuthJwtToken(token);

            if (principal == null)
            {
                context.ErrorResult = new AuthFailureResult("Invalid JWT Token", request);
            }
            else
            {
                context.Principal = principal;
            }
        }
       
        
        
        private static bool ValidateToken(string token, out string username)
        {
            username = null;

            var simplePrinciple = JwtAuthManager.GetPrincipal(token);
            if (simplePrinciple == null)
                return false;
            var identity = simplePrinciple.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim?.Value;

            if (string.IsNullOrEmpty(username))
                return false;

            // You can implement more validation to check whether username exists in your DB or not or something else. 

            return true;
        }
        private static bool ValidateMacToken(string token,TableConfig settings,out MachineServer mac )//out string clientId)
        {
            string clientId = null;
            mac = new MachineServer();

            //step#2
            var simplePrinciple = JwtAuthManager.GetMacPrincipal(token,settings);
            if (simplePrinciple == null)
                return false;
            var identity = simplePrinciple.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            clientId = usernameClaim?.Value;

            var jsonMessage = identity.FindFirst(ClaimTypes.UserData)?.Value;        

            if (string.IsNullOrEmpty(clientId)
                ||string.IsNullOrEmpty(jsonMessage))
                return false;
            else
            {
                mac = JsonConvert.DeserializeObject<MachineServer>(jsonMessage);
            }

            // You can implement more validation to check whether username exists in your DB or not or something else. 
            if (mac.Id > 0)
                return true;
            else
                return false;
        }



        protected Task<IPrincipal> AuthJwtToken(string token)
        {
            string username;

            if (ValidateToken(token, out username))
            {
                //to get more information from DB in order to build local identity based on username 
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                    // you can add more claims if needed like Roles ( Admin or normal user or something else)
                };

                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }

       public MachineServer AuthMacToken(string token,TableConfig settings)
        {
            MachineServer mac;// = new MachineServer();

            if (ValidateMacToken(token,settings, out mac))
            {

                return mac;
            }

            return null;
        }


        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }
        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter = null;

            if (!string.IsNullOrEmpty(Realm))
                parameter = "realm=\"" + Realm + "\"";

           context.ChallengeWith("Bearer", parameter);
        }
    }
}