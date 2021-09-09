using Newtonsoft.Json;
using NFSe.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace NFe.Components.AGILI
{
    public class Token
    {
        #region Privte Properties

        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public double ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }

        #endregion Privte Properties

        #region Public Methods

        public static Token GerarToken(IWebProxy proxy, string usuario, string senha, string clientID, string clientSecret, string url)
        {
            string result = string.Empty;
            var tokenResult = new Token();
            senha = Criptografia.descriptografaSenha(senha);

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
                     {"password", Functions.GerarMD5(senha).ToUpper() },
                     {"client_id", clientID},
                     {"client_secret", clientSecret}
                }, autorizations);
                }

                var token = JsonConvert.DeserializeObject<Token>(result);

                if (token.AccessToken == null)
                    throw new Exception("O token informado é inválido");

                tokenResult = token;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tokenResult;
        }

        #endregion Public Methods
    }
}