using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NFe.Components.Abstract;
using System.Net;
using System.Web.Services.Protocols;
using System.Security.Cryptography.X509Certificates;
using NFe.Components.br.srv.gp.www.coplan.camponovodoparecis.p;

namespace NFe.Components.Coplan.CampoNovoParecisMT.p
{
    public class CoplanP : EmiteNFSeBase
    {
        nfse_web_service Service = new nfse_web_service();
        input Input = new input();
        XmlDocument XmlDocument = new XmlDocument();

        public override string NameSpaces
        {
            get
            {
                return "http://www.abrasf.org.br/nfse.xsd";
            }
        }

        #region construtores
        public CoplanP(TipoAmbiente tpAmb, string pastaRetorno, string usuarioProxy, string senhaProxy, string domainProxy, X509Certificate certificado)
            : base(tpAmb, pastaRetorno)
        {
            Service.Proxy = WebRequest.DefaultWebProxy;
            Service.Proxy.Credentials = new NetworkCredential(usuarioProxy, senhaProxy);
            Service.Credentials = new NetworkCredential(senhaProxy, senhaProxy);
            Service.ClientCertificates.Add(certificado);

            ServicePointManager.Expect100Continue = false;

            Input.nfseCabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.01\"> <versaoDados>2.01</versaoDados></cabecalho>";

        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            XmlDocument.Load(file);
            Input.nfseDadosMsg = XmlDocument.InnerXml;

            string result = string.Empty;

            switch (XmlDocument.DocumentElement.Name)
            {
                case "EnviarLoteRpsEnvio":
                    result = Service.RECEPCIONARLOTERPS(Input).outputXML;
                    break;
                case "EnviarLoteRpsSincronoEnvio":
                    result = Service.RECEPCIONARLOTERPSSINCRONO(Input).outputXML;
                    break;
            }

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            XmlDocument.Load(file);
            Input.nfseDadosMsg = XmlDocument.InnerXml;

            string result = Service.CANCELARNFSE(Input)?.outputXML;

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        {
            /*
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = Service.RecepcionarXml("ConsultarLoteRps", doc.InnerXml);
            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
                                        */
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            /*
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = Service.RecepcionarXml("ConsultarSituacaoLoteRps", doc.InnerXml);
            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).EnvioXML,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).RetornoXML);
                                        */
        }

        public override void ConsultarNfse(string file)
        {
            /*
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = Service.RecepcionarXml("ConsultarNfse", doc.InnerXml);
            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
                                        */
        }

        public override void ConsultarNfsePorRps(string file)
        {
            /*
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = Service.RecepcionarXml("ConsultarNfsePorRps", doc.InnerXml);
            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML);
                                        */
        }
        #endregion
    }
}
