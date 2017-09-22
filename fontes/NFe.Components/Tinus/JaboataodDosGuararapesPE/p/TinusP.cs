using System;
using NFe.Components.Abstract;
using NFe.Components.br.com.fiorilli.avaresp.p;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace NFe.Components.Tinus.JaboataodDosGuararapesPE.p
{
    public class TinusP : EmiteNFSeBase
    {
        IssWebWS Service = new IssWebWS();
        public override string NameSpaces
        {
            get
            {
                return "http://www.abrasf.org.br/nfse.xsd";
            }
        }

        #region construtores
        public TinusP(TipoAmbiente tpAmb, string pastaRetorno, string proxyuser, string proxypass, string proxyserver, X509Certificate2 certificado)
            : base(tpAmb, pastaRetorno)
        {
            ServicePointManager.Expect100Continue = false;

            Service.ClientCertificates.Add(certificado);

            if (!String.IsNullOrEmpty(proxyuser))
            {
                NetworkCredential credentials = new NetworkCredential(proxyuser, proxypass, proxyserver);
                WebRequest.DefaultWebProxy.Credentials = credentials;

                Service.Proxy = WebRequest.DefaultWebProxy;
                Service.Proxy.Credentials = new NetworkCredential(proxyuser, proxypass);
                Service.Credentials = new NetworkCredential(proxyuser, proxypass);
            }
        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string strResult = string.Empty;
            switch (doc.DocumentElement.Name)
            {
                case "GerarNfseEnvio":
                    strResult = EnvioSincrono(file);
                    break;
                case "EnviarLoteRpsSincronoEnvio":
                    strResult = EnvioSincronoEmLote(file);
                    break;
            }

            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                              Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);

        }

        private string EnvioSincrono(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        private string EnvioSincronoEmLote(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        public override void CancelarNfse(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        public override void ConsultarLoteRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        public override void ConsultarNfse(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        public override void ConsultarNfsePorRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }
        #endregion
    }
}
