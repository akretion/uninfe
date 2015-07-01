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
using NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p;
using System.Net;
using System.Web.Services.Protocols;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.EGoverne.CuritibaPR.p
{
    public class EGoverneP : EGoverneSerialization
    {
        WSNFSeV1001 service = new WSNFSeV1001();

        #region construtores
        public EGoverneP(TipoAmbiente tpAmb, string pastaRetorno, string usuarioProxy, string senhaProxy, string domainProxy, X509Certificate certificado)
            : base(tpAmb, pastaRetorno, usuarioProxy, senhaProxy, domainProxy)
        {
            service.Proxy = WebRequest.DefaultWebProxy;
            service.Proxy.Credentials = new NetworkCredential(usuarioProxy, senhaProxy);
            service.Credentials = new NetworkCredential(senhaProxy, senhaProxy);
            service.ClientCertificates.Add(certificado);
        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            EnviarLoteRpsEnvio loterpsenvio = ReadXML<EnviarLoteRpsEnvio>(file);
            EnviarLoteRpsResposta result = service.RecepcionarLoteRps(loterpsenvio);
            GerarRetorno(file, base.CreateXML(result), Propriedade.ExtEnvio.EnvLoteRps, Propriedade.ExtRetorno.LoteRps);
        }

        public override void CancelarNfse(string file)
        {
            CancelarNfseEnvio cancelarnfseenvio = ReadXML<CancelarNfseEnvio>(file);
            CancelarNfseResposta result = service.CancelarNfse(cancelarnfseenvio);
            GerarRetorno(file, base.CreateXML(result), Propriedade.ExtEnvio.PedCanNfse, Propriedade.ExtRetorno.retCancelamento_XML);

        }

        public override void ConsultarLoteRps(string file)
        {
            ConsultarLoteRpsEnvio consultarloterps = ReadXML<ConsultarLoteRpsEnvio>(file);
            ConsultarLoteRpsResposta result = service.ConsultarLoteRps(consultarloterps);
            GerarRetorno(file, base.CreateXML(result), Propriedade.ExtEnvio.PedLoteRps, Propriedade.ExtRetorno.RetLoteRps);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            ConsultarSituacaoLoteRpsEnvio consultarsituacaoloterps = ReadXML<ConsultarSituacaoLoteRpsEnvio>(file);
            ConsultarSituacaoLoteRpsResposta result = service.ConsultarSituacaoLoteRps(consultarsituacaoloterps);
            GerarRetorno(file, base.CreateXML(result), Propriedade.ExtEnvio.PedLoteRps, Propriedade.ExtRetorno.RetLoteRps);
        }

        public override void ConsultarNfse(string file)
        {
            ConsultarNfseEnvio consultarnfseenvio = ReadXML<ConsultarNfseEnvio>(file);
            ConsultarNfseResposta result = service.ConsultarNfse(consultarnfseenvio);
            GerarRetorno(file, base.CreateXML(result), Propriedade.ExtEnvio.PedSitNfse, Propriedade.ExtRetorno.SitNfse);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            ConsultarNfseRpsEnvio consultarnfserps = ReadXML<ConsultarNfseRpsEnvio>(file);
            ConsultarNfseRpsResposta result = service.ConsultarNfsePorRps(consultarnfserps);
            GerarRetorno(file, base.CreateXML(result), Propriedade.ExtEnvio.PedSitNfseRps, Propriedade.ExtRetorno.SitNfseRps);
        }
        #endregion
    }
}
