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
using NFe.Components.br.gov.pr.curitiba.pilotoisscuritiba.h;

namespace NFe.Components.EGoverne.CuritibaPR.h
{
    public class EGoverneH: EGoverneSerialization
    {
        WSNFSeV1001 service = new WSNFSeV1001();

        #region construtores
        public EGoverneH(TipoAmbiente tpAmb, string pastaRetorno, string usuarioProxy, string senhaProxy, string domainProxy, X509Certificate certificado)
            : base(tpAmb, pastaRetorno, usuarioProxy, senhaProxy, domainProxy)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyCertHandler;
            service.Proxy = WebRequest.DefaultWebProxy;
            service.Proxy.Credentials = new NetworkCredential(usuarioProxy, senhaProxy);
            service.Credentials = new NetworkCredential(senhaProxy, senhaProxy);
            service.ClientCertificates.Add(certificado);
        }

        /// <summary>
        /// Indentificamos falha no certificado o do servidor, entao temos que ignorar os erros
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
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = service.RecepcionarXml("RecepcionarLoteRps", doc.InnerXml);
            GerarRetorno(file, result, Propriedade.ExtEnvio.EnvLoteRps, Propriedade.ExtRetorno.LoteRps);
        }

        public override void CancelarNfse(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = service.RecepcionarXml("CancelarNfse", doc.InnerXml);
            GerarRetorno(file, result, Propriedade.ExtEnvio.PedCanNfse, Propriedade.ExtRetorno.retCancelamento_XML);
        }

        public override void ConsultarLoteRps(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = service.RecepcionarXml("ConsultarLoteRps", doc.InnerXml);
            GerarRetorno(file, result, Propriedade.ExtEnvio.PedLoteRps, Propriedade.ExtRetorno.RetLoteRps);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = service.RecepcionarXml("ConsultarSituacaoLoteRps", doc.InnerXml);
            GerarRetorno(file, result, Propriedade.ExtEnvio.PedLoteRps, Propriedade.ExtRetorno.RetLoteRps);
        }

        public override void ConsultarNfse(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = service.RecepcionarXml("ConsultarNfse", doc.InnerXml);
            GerarRetorno(file, result, Propriedade.ExtEnvio.PedSitNfse, Propriedade.ExtRetorno.SitNfse);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string result = service.RecepcionarXml("ConsultarNfsePorRps", doc.InnerXml);            
            GerarRetorno(file, result, Propriedade.ExtEnvio.PedSitNfseRps, Propriedade.ExtRetorno.SitNfseRps);
        }
        #endregion
    }
}
