using Newtonsoft.Json;
using NFe.Components.Abstract;
using NFSe.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace NFe.Components.AGILI
{
    public class AGILI : EmiteNFSeBase, IEmiteNFSeAGILI
    {
        #region Public Properties

        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string ClientID { get; set; }

        public string ClientSecret { get; set; }

        public string Token { get; set; }

        public DateTime TokenExpire { get; set; }

        public string URLAPIBase
        {
            get
            {
                    return @"http://agiliblue.agilicloud.com.br/api/";
            }
        }

        #endregion Public Properties

        #region Public Construstor

        public AGILI(TipoAmbiente tpAmb, string pastaRetorno, string token, DateTime tokenExpire, string usuario, string senha, string clienteID, string clientSecret)
            : base(tpAmb, pastaRetorno)
        {
            Token = token;
            TokenExpire = tokenExpire;
            Usuario = usuario;
            Senha = senha;
            ClientID = clienteID;
            ClientSecret = clientSecret;
        }

        #endregion Public Construstor

        #region Public Methods

        public override void CancelarNfse(string file)
        {
            string result = "";

            TokenTimeExpire();

            if (String.IsNullOrEmpty(Token))
                throw new Exception("Token inválido");

            using (POSTRequest post = new POSTRequest
            {
                Proxy = Proxy
            })
            {
                IList<string> autorizations = new List<string>()
                {
                    $"Authorization: bearer {Token}"
                };

                result = post.PostForm(Path.Combine(URLAPIBase, "CancelarNfse"),
                    new Dictionary<string, string>
                    {
                        {"f1", file}
                    },
                    autorizations);
            }

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        { 
        
        }

        public override void ConsultarNfse(string file)
        {

        }

        public override void ConsultarNfsePorRps(string file)
        {
            string result = "";

            XmlDocument xmlConsulta = new XmlDocument();
            xmlConsulta.Load(file);

            using (GetRequest get = new GetRequest
            {
                Proxy = Proxy
            })
            {
                result = get.GetForm($"{Path.Combine(URLAPIBase, "ConsultarNfseRps")}");
            }

            result = result?.Replace("{", "");
            result = "{" + "\"nota\":{" + result + "}";

            XmlDocument consultaResult = JsonConvert.DeserializeXmlNode(result);

            GerarRetorno(file, consultaResult.InnerXml, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        { }

        public override void EmiteNF(string file)
        {
            string result = "";

            TokenTimeExpire();

            if (String.IsNullOrEmpty(Token))
                throw new Exception("Token inválido");

            using (POSTRequest post = new POSTRequest
            {
                Proxy = Proxy
            })
            {
                IList<string> autorizations = new List<string>()
                {
                    $"Authorization: bearer {Token}"
                };

                result = post.PostForm(Path.Combine(URLAPIBase, "GerarNfse"),
                    new Dictionary<string, string>
                    {
                        { "f1", file}
                    },
                    autorizations);
            }

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void GerarRetorno(string file, string result, string extEnvio, string extRetorno)
        {
            FileInfo fi = new FileInfo(file);
            string nomearq = PastaRetorno + "\\" + fi.Name.Replace(extEnvio, extRetorno);

            Encoding iso = Encoding.GetEncoding("UTF-8");
            StreamWriter write = new StreamWriter(nomearq, false, iso);
            write.Write(result);
            write.Flush();
            write.Close();
            write.Dispose();
        }

        private void TokenTimeExpire()
        {
            if (TokenExpire < DateTime.Now)
            {
                var token = Components.SOFTPLAN.Token.GerarToken(Proxy, Usuario, Senha, ClientID, ClientSecret, URLAPIBase);

                Token = token.AccessToken;
                TokenExpire = DateTime.Now.AddSeconds(token.ExpiresIn);
            }
        }

        #endregion Public Methods
    }
}