using NFe.Components.Abstract;
using NFe.Components.PViamaoRS;
using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace NFe.Components.Pronin.ViamaoRS.p
{
    public class ProninP : EmiteNFSeBase
    {
        private BasicHttpBinding_INFSEGeracao ServiceGeracao = new BasicHttpBinding_INFSEGeracao();
        private BasicHttpBinding_INFSEConsultas ServiceConsultas = new BasicHttpBinding_INFSEConsultas();

        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region construtores

        public ProninP(TipoAmbiente tpAmb, string pastaRetorno, string usuarioProxy, string senhaProxy, string domainProxy, X509Certificate certificado)
            : base(tpAmb, pastaRetorno)
        {
            ServiceGeracao.Proxy = WebRequest.DefaultWebProxy;
            ServiceGeracao.Proxy.Credentials = new NetworkCredential(usuarioProxy, senhaProxy);
            ServiceGeracao.Credentials = new NetworkCredential(senhaProxy, senhaProxy);
            ServiceGeracao.ClientCertificates.Add(certificado);

            ServiceConsultas.Proxy = WebRequest.DefaultWebProxy;
            ServiceConsultas.Proxy.Credentials = new NetworkCredential(usuarioProxy, senhaProxy);
            ServiceConsultas.Credentials = new NetworkCredential(senhaProxy, senhaProxy);
            ServiceConsultas.ClientCertificates.Add(certificado);

            Cabecalho cabecMsg = new Cabecalho();
            cabecMsg.versao = "2.02";
            cabecMsg.versaoDados = "2.02";

            ServiceGeracao.cabecalho = cabecMsg;
            ServiceConsultas.cabecalho = cabecMsg;
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
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            string result = "";

            switch (doc.DocumentElement.Name)
            {
                case "GerarNfseEnvio":
                    result = ServiceGeracao.GerarNfse(doc.InnerXml);
                    break;
                case "EnviarLoteRpsSincronoEnvio":
                    result = ServiceGeracao.EnviarLoteRpsSincrono(doc.InnerXml);
                    break;
                case "EnviarLoteRpsEnvio":
                    result = ServiceGeracao.RecepcionarLoteRps(doc.InnerXml);
                    break;
            }

            //string result = ServiceGeracao.RecepcionarLoteRps(doc.InnerXml);

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = ServiceGeracao.CancelarNfse(doc.InnerXml);
            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void SubstituirNfse(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = ServiceGeracao.SubstituirNfse(doc.InnerXml);
            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSubstNfse).EnvioXML,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSubstNfse).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = ServiceConsultas.ConsultarLoteRps(doc.InnerXml);
            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = ServiceConsultas.ConsultarSituacaoLoteRps(doc.InnerXml);
            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).EnvioXML,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).RetornoXML);
        }

        public override void ConsultarNfse(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = ServiceConsultas.ConsultarNfse(doc.InnerXml);
            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = ServiceConsultas.ConsultarNfsePorRps(doc.InnerXml);
            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML);
        }

        #endregion Métodos
    }
}