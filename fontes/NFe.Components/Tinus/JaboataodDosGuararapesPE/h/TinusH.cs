using NFe.Components.Abstract;
using NFe.Components.HJaboataoDosGuararapesPE_TINUS_CancelarNfse;
using NFe.Components.HJaboataoDosGuararapesPE_TINUS_RecepcionarLoteRps;
using NFe.Components.HJaboataoDosGuararapesPE_TINUS_ConsultarNfse;
using NFe.Components.HJaboataoDosGuararapesPE_TINUS_ConsultarLoteRps;
using NFe.Components.HJaboataoDosGuararapesPE_TINUS_ConsultarNfsePorRps;
using NFe.Components.HJaboataoDosGuararapesPE_TINUS_ConsultarSituacaoLoteRps;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.Tinus.JaboataodDosGuararapesPE.h
{
    public class TinusH : EmiteNFSeBase
    {
        private System.Web.Services.Protocols.SoapHttpClientProtocol Service;
        private X509Certificate2 Certificado;

        public override string NameSpaces
        {
            get
            {
                return "http://www2.tinus.com.br";
            }
        }

        #region construtores

        public TinusH(TipoAmbiente tpAmb, string pastaRetorno, string proxyuser, string proxypass, string proxyserver, X509Certificate2 certificado)
            : base(tpAmb, pastaRetorno)
        {
            ServicePointManager.Expect100Continue = false;
            Certificado = certificado;
        }

        #endregion construtores

        #region Métodos

        public override void EmiteNF(string file)
        {
            RecepcionarLoteRps Service = new RecepcionarLoteRps();
            Service.ClientCertificates.Add(Certificado);
            DefinirProxy<RecepcionarLoteRps>(Service);

            EnviarLoteRpsEnvio envio = DeserializarObjeto<EnviarLoteRpsEnvio>(file);
            string strResult = SerializarObjeto(Service.CallRecepcionarLoteRps(envio));

            GerarRetorno(file,
                strResult,
                Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            CancelarNfse Service = new CancelarNfse();
            Service.ClientCertificates.Add(Certificado);
            DefinirProxy<CancelarNfse>(Service);

            CancelarNfseEnvio envio = DeserializarObjeto<CancelarNfseEnvio>(file);
            string strResult = SerializarObjeto(Service.CallCancelarNfse(envio));

            GerarRetorno(file,
                strResult,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        {
            ConsultarLoteRps Service = new ConsultarLoteRps();
            Service.ClientCertificates.Add(Certificado);
            DefinirProxy<ConsultarLoteRps>(Service);

            ConsultarLoteRpsEnvio envio = DeserializarObjeto<ConsultarLoteRpsEnvio>(file);
            string strResult = SerializarObjeto(Service.CallConsultarLoteRps(envio));

            GerarRetorno(file,
                strResult,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            ConsultarSituacaoLoteRps Service = new ConsultarSituacaoLoteRps();
            Service.ClientCertificates.Add(Certificado);
            DefinirProxy<ConsultarSituacaoLoteRps>(Service);

            ConsultarSituacaoLoteRpsEnvio envio = DeserializarObjeto<ConsultarSituacaoLoteRpsEnvio>(file);
            string strResult = SerializarObjeto(Service.CallConsultarSituacaoLoteRps(envio));

            GerarRetorno(file,
                strResult,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).RetornoXML);
        }

        public override void ConsultarNfse(string file)
        {
            ConsultarNfse Service = new ConsultarNfse();
            Service.ClientCertificates.Add(Certificado);
            DefinirProxy<ConsultarNfse>(Service);

            ConsultarNfseEnvio envio = DeserializarObjeto<ConsultarNfseEnvio>(file);
            string strResult = SerializarObjeto(Service.CallConsultarNfse(envio));

            GerarRetorno(file,
                strResult,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            ConsultarNfsePorRps Service = new ConsultarNfsePorRps();
            Service.ClientCertificates.Add(Certificado);
            DefinirProxy<ConsultarNfsePorRps>(Service);

            ConsultarNfseRpsEnvio envio = DeserializarObjeto<ConsultarNfseRpsEnvio>(file);
            string strResult = SerializarObjeto(Service.CallConsultarNfsePorRps(envio));

            GerarRetorno(file,
                strResult,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML);
        }

        #endregion Métodos
    }
}