using NFe.Components.Abstract;
using NFe.Components.WSLoandaPR;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace NFe.Components.Elotech
{
    public class LoandaPR : EmiteNFSeBase
    {
        private readonly NfsePortService Servico = new NfsePortService();

        public override string NameSpaces => "http://shad.elotech.com.br/schemas/iss/nfse_v2_03.xsd";

        #region construtores

        public LoandaPR(TipoAmbiente tpAmb, string pastaRetorno, string usuarioProxy, string senhaProxy, string domainProxy, X509Certificate certificado)
            : base(tpAmb, pastaRetorno)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyCertHandler;
            Servico.Proxy = WebRequest.DefaultWebProxy;
            Servico.Proxy.Credentials = new NetworkCredential(usuarioProxy, senhaProxy);
            Servico.Credentials = new NetworkCredential(senhaProxy, senhaProxy);
            Servico.ClientCertificates.Add(certificado);
        }

        /// <summary>
        /// Identificamos falha no certificado o do servidor, então temos que ignorar os erros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private bool MyCertHandler(object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        #endregion construtores

        #region Métodos

        public override void EmiteNF(string file)
        {
            var strResult = string.Empty;

            var doc = new XmlDocument();
            doc.Load(file);

            switch (doc.DocumentElement.Name)
            {
                case "EnviarLoteRpsEnvio":
                    strResult = EnviarLoteRps(file);
                    break;

                case "EnviarLoteRpsSincronoEnvio":
                    strResult = EnviarLoteRpsSincrono(file);
                    break;
            }

            GerarRetorno(file, strResult,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML,
                Encoding.UTF8);
        }

        private string EnviarLoteRps(string file)
        {
            var enviarLoteRpsEnvio = new EnviarLoteRpsEnvio();
            enviarLoteRpsEnvio = DeserializarObjeto<EnviarLoteRpsEnvio>(file);

            var enviarLoteRpsResposta = Servico.EnviarLoteRps(enviarLoteRpsEnvio);
            var strResult = SerializarObjeto(enviarLoteRpsResposta);

            return strResult;
        }

        private string EnviarLoteRpsSincrono(string file)
        {
            var enviarLoteRpsSincronoEnvio = new EnviarLoteRpsSincronoEnvio();
            enviarLoteRpsSincronoEnvio = DeserializarObjeto<EnviarLoteRpsSincronoEnvio>(file);

            var enviarLoteRpsSincronoResposta = Servico.EnviarLoteRpsSincrono(enviarLoteRpsSincronoEnvio);
            var strResult = SerializarObjeto(enviarLoteRpsSincronoResposta);

            return strResult;
        }

        public override void CancelarNfse(string file)
        {
            var cancelarNfseEnvio = new CancelarNfseEnvio();
            cancelarNfseEnvio = DeserializarObjeto<CancelarNfseEnvio>(file);

            var cancelarNfseResposta = Servico.CancelarNfse(cancelarNfseEnvio);
            var strResult = SerializarObjeto(cancelarNfseResposta);

            GerarRetorno(file, strResult,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML,
                Encoding.UTF8);
        }

        public override void ConsultarLoteRps(string file)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.Load(file);
            //string result = ServiceConsultas.ConsultarLoteRps(doc.InnerXml);
            //GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
            //                            Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.Load(file);
            //string result = ServiceConsultas.ConsultarSituacaoLoteRps(doc.InnerXml);
            //GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).EnvioXML,
            //                            Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).RetornoXML);
        }

        public override void ConsultarNfse(string file)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.Load(file);
            //string result = ServiceConsultas.ConsultarNfse(doc.InnerXml);
            //GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
            //                            Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.Load(file);
            //string result = ServiceConsultas.ConsultarNfsePorRps(doc.InnerXml);
            //GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML,
            //                            Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML);
        }

        #endregion Métodos
    }
}