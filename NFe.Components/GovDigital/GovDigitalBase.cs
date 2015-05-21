using NFe.Components.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NFe.Components.GovDigital
{
    public abstract class GovDigitalBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        object govDigitalService;
        protected object GovDigitalService
        {
            get
            {
                if (govDigitalService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        govDigitalService = new br.com.govdigital.homolog1.NfseServiceImplDivService();
                    else
                        govDigitalService = new br.com.govdigital.ws.NfseServiceImplDivService();

                    AddClientCertificates();
                }
                return govDigitalService;
            }
        }

        private void AddClientCertificates()
        {
            X509CertificateCollection certificates = null;
            Type t = govDigitalService.GetType();
            PropertyInfo pi = t.GetProperty("ClientCertificates");
            certificates = pi.GetValue(govDigitalService, null) as X509CertificateCollection;
            certificates.Add(Certificate);
        }
        #endregion

        #region propriedades
        public X509Certificate Certificate { get; private set; }
        private string NfseCabecMsg = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><ns2:cabecalho xmlns=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:ns2=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.00\"><ns2:versaoDados>2.00</ns2:versaoDados></ns2:cabecalho>";
        #endregion

        #region Construtores
        public GovDigitalBase(TipoAmbiente tpAmb, string pastaRetorno, X509Certificate certificate)
            : base(tpAmb, pastaRetorno)
        {
            Certificate = certificate;
        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            string strResult = Invoke("GerarNfse", new[] { NfseCabecMsg, ReaderXML(file) });
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.EnvLoteRps, Propriedade.ExtRetorno.RetLoteRps);
        }

        public override void CancelarNfse(string file)
        {
            string strResult = Invoke("CancelarNfse", new[] { NfseCabecMsg, ReaderXML(file) });
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.PedCanNfse, Propriedade.ExtRetorno.CanNfse);
        }

        public override void ConsultarLoteRps(string file)
        {
            string strResult = Invoke("ConsultarLoteRps", new[] { NfseCabecMsg, ReaderXML(file) });
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.PedLoteRps, Propriedade.ExtRetorno.LoteRps);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        public override void ConsultarNfse(string file)
        {
            string strResult = Invoke("ConsultarNfsePorFaixa", new[] { NfseCabecMsg, ReaderXML(file) });
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.PedSitNfse, Propriedade.ExtRetorno.SitNfse);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            string strResult = Invoke("ConsultarNfsePorRps", new[] { NfseCabecMsg, ReaderXML(file) });
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.PedSitNfseRps, Propriedade.ExtRetorno.SitNfseRps);
        }
        #endregion

        #region invoke
        string Invoke(string methodName, params object[] _params)
        {
            object result = "";
            Type t = GovDigitalService.GetType();
            MethodInfo mi = t.GetMethod(methodName);
            result = mi.Invoke(GovDigitalService, _params);
            return result.ToString();
        }
        #endregion

        #region ReaderXML
        private string ReaderXML(string file)
        {
            string result = "";

            using (StreamReader reader = new StreamReader(file))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    result += line;
                }
            }
            return result;
        }
        #endregion
    }
}
