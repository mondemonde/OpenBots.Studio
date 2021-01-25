using System;
using System.IdentityModel.Tokens.Jwt;
//using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
//using DevNote.Interface.Models;
//using DevNoteHub.Models;
using Microsoft.IdentityModel.Tokens;
using Steeroid.Models;

namespace DevNoteHub
{
    public static class JwtAuthManager
    {
        //public const string decoded = "ne longe fias a me quoniam tribulatio proxima est quoniam non est adiutor";
       
        //encoded version
        public const string SecretKey = "bmUgbG9uZ2UgZmlhcyBhIG1lIHF1b25pYW0gdHJpYnVsYXRpbyBwcm94aW1hIGVzdCBxdW9uaWFtIG5vbiBlc3QgYWRpdXRvcg==";

        public static string GenerateJWTToken(MachineServer app)
        {
            var symmetric_Key = Convert.FromBase64String(SecretKey);
            var token_Handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            DateTime? expiry = null;
            if (app.ExpirationDate.HasValue && app.ExpirationDate.Value > DateTime.MinValue)
            {
                expiry = app.ExpirationDate.Value;
            }

            var securitytokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Name, app.DomainName),
                    new Claim(ClaimTypes.UserData, app.ToString())
                }),
                NotBefore = DateTime.Now.AddDays(-1),
                Issuer = "www.blastasia.com",
                Expires = expiry,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetric_Key), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = token_Handler.CreateToken(securitytokenDescriptor);
            var token = token_Handler.WriteToken(stoken);

            return token;
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtTokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(SecretKey);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = jwtTokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }
            catch (Exception e)
            {
                return null;
            }
        }
       
        public static ClaimsPrincipal GetMacPrincipal(string token, TableConfig settings)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtTokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(SecretKey);

              

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = settings.Enable_JWTExpiration,//false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = jwtTokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }
            catch (Exception e)
            {
                
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static string GenerateJWTToken(string field,MachineServer app)
        {
            var symmetric_Key = Convert.FromBase64String(SecretKey);
            var token_Handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

            //var now = DateTime.UtcNow;
            DateTime? expiry = null;
           

            var securitytokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                    {
                            new Claim(ClaimTypes.Name, app.ClientId),
                            new Claim(ClaimTypes.UserData, app.ToString())
                     }),
                Expires = expiry,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetric_Key), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = token_Handler.CreateToken(securitytokenDescriptor);
            var token = token_Handler.WriteToken(stoken);

            return token;
        }


        public static string GenerateJWTToken(string username, int expire_in_Minutes = 30)
        {
            var symmetric_Key = Convert.FromBase64String(SecretKey);
            var token_Handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var securitytokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, username)
                        }),
                Expires = now.AddMinutes(Convert.ToInt32(expire_in_Minutes)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetric_Key), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = token_Handler.CreateToken(securitytokenDescriptor);
            var token = token_Handler.WriteToken(stoken);

            return token;
        }




        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string CreateSecretKeyForJwt()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            GuidString = GuidString.Replace("//", "x");
            GuidString = GuidString.Replace("\\", "z");         

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(GuidString);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Create128Key()
        {
            var key = CreateSecretKeyForJwt();
            var key128 = Extract128(key);
            return key128;
        }

        public static string Create192Key()
        {
            var key = CreateSecretKeyForJwt();
            var key192 = Extract192(key);
            return key192;
        }

        private static string Extract128(string longText)
        {
            string newText = null;

            foreach (char c in longText)
            {
                newText += c;
                var Count = (UTF8Encoding.UTF8.GetByteCount(newText) * 8);
                if (Count == 128)
                {
                    return newText;
                }

            }

            return newText;
        }

        public static string Extract192(string longText)
        {
            string newText = null;
            foreach (char c in longText)
            {
                newText += c;
                var Count = (UTF8Encoding.UTF8.GetByteCount(newText) * 8);
                if (Count == 192)
                {
                    return newText;
                }

            }
            return newText;
        }

        public static string GetDefaultLocalKey()
        {
            var longText = SecretKey;

            string newText = null;
            foreach (char c in longText)
            {
                newText += c;
                var Count = (UTF8Encoding.UTF8.GetByteCount(newText) * 8);
                if (Count == 192)
                {
                    return newText;
                }

            }
            return newText;
        }


    }
}