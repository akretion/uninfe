#if _fw46

using Newtonsoft.Json;
using NFSe.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace NFe.Components.SOFTPLAN
{
    public class Token
    {
        #region Privte Properties

        [JsonProperty(PropertyName = "access_token")]
        private string AccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        private string TokenType { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        private string RefreshToken { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        private int ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "scope")]
        private string Scope { get; set; }

        #endregion Privte Properties

        #region Public Methods

        public static string GerarToken(IWebProxy proxy, string usuario, string senha, string clientID, string clientSecret, string url)
        {
            string result = string.Empty;

            try
            {
                using (POSTRequest post = new POSTRequest
                {
                    Proxy = proxy
                })
                {
                    string autorization = Functions.Base64Encode($"{clientID}:{clientSecret}");

                    IList<string> autorizations = new List<string>()
                {
                    $"Authorization: Basic {autorization}"
                };

                    result = post.PostForm(Path.Combine(url, "autenticacao/oauth/token"), new Dictionary<string, string> {
                     {"grant_type", "password" },
                     {"username", usuario },
                     {"password", senha },
                     {"client_id", clientID},
                     {"client_secret", clientSecret}
                }, autorizations);
                }

                var token = JsonConvert.DeserializeObject<Token>(result);

                if (token.AccessToken == null)
                    throw new Exception("O token informado é inválido");

                result = token.AccessToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion Public Methods
    }
}

#endif