using NFe.Components.Abstract;
using NFe.Components.HGeisWeb;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace NFe.Components.GeisWeb.ItatingaSP.h
{
    public class GeisWebH : EmiteNFSeBase
    {
        private System.Web.Services.Protocols.SoapHttpClientProtocol Service;
        private X509Certificate2 Certificado;

        public override string NameSpaces
        {
            get
            {
                return "";
            }
        }

        #region construtores

        public GeisWebH(TipoAmbiente tpAmb, string pastaRetorno, string proxyuser, string proxypass, string proxyserver, X509Certificate2 certificado)
            : base(tpAmb, pastaRetorno)
        {
            ServicePointManager.Expect100Continue = false;
            Certificado = certificado;
        }

        #endregion construtores

        #region Métodos

        public override void EmiteNF(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            GeisWebService Service = new GeisWebService();
            Service.ClientCertificates.Add(Certificado);
            string strResult = Service.EnviaLoteRps(doc.OuterXml);

            GerarRetorno(file,
                strResult,
                Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            GeisWebService Service = new GeisWebService();
            Service.ClientCertificates.Add(Certificado);
            string strResult = Service.CancelaNfse(doc.OuterXml);

            GerarRetorno(file,
                strResult,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            GeisWebService Service = new GeisWebService();
            Service.ClientCertificates.Add(Certificado);
            string strResult = Service.ConsultaLoteRps(doc.OuterXml);

            GerarRetorno(file,
                strResult,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
        }

        public override void ConsultarNfse(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            GeisWebService Service = new GeisWebService();
            Service.ClientCertificates.Add(Certificado);
            string strResult = Service.ConsultaNfse(doc.OuterXml);

            GerarRetorno(file,
                strResult,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            throw new System.NotImplementedException();
        }

        public override void ConsultarNfsePorRps(string file)
        {
            throw new System.NotImplementedException();
        }

        #endregion Métodos
    }
}