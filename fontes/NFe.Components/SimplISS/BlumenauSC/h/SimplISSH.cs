using NFe.Components.Abstract;
using NFe.Components.HBlumenauSC;
using System;
using System.Net;

namespace NFe.Components.SimplISS.BlumenauSC.h
{
    public class SimplISSH : EmiteNFSeBase
    {
        #region Propriedades

        /// <summary>
        /// Objeto de conexão com o Webservice
        /// </summary>
        private readonly NfseService Service = new NfseService();

        /// <summary>
        /// Namespace utilizada para deserialização do objeto
        /// </summary>
        public override string NameSpaces => "http://www.abrasf.org.br/nfse.xsd";

        #endregion

        #region Construtores

        public SimplISSH(TipoAmbiente tpAmb, string pastaRetorno, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver)
            : base(tpAmb, pastaRetorno)
        {
            if (!string.IsNullOrEmpty(proxyuser))
            {
                var credentials = new System.Net.NetworkCredential(proxyuser, proxypass, proxyserver);
                System.Net.WebRequest.DefaultWebProxy.Credentials = credentials;

                Service.Proxy = WebRequest.DefaultWebProxy;
                Service.Proxy.Credentials = new NetworkCredential(proxyuser, proxypass);
                Service.Credentials = new NetworkCredential(proxyuser, proxypass);
            }

        }

        #endregion

        #region Métodos

        public override void EmiteNF(string file)
        {
            var envio = DeserializarObjeto<GerarNfseEnvio>(file);
            var strResult = SerializarObjeto(Service.GerarNfse(envio));

            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            var envio = DeserializarObjeto<CancelarNfseEnvio>(file);
            var strResult = SerializarObjeto(Service.CancelarNfse(envio));

            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        {
            var envio = DeserializarObjeto<ConsultarLoteRpsEnvio>(file);
            var strResult = SerializarObjeto(Service.ConsultarLoteRps(envio));
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            //var envio = DeserializarObjeto<ConsultarSituacaoLoteRpsEnvio>(file);
            //var strResult = SerializarObjeto(Service.ConsultarSituacaoLoteRps(envio));
            //GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
            //                              Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
        }

        public override void ConsultarNfse(string file)
        {
            //var envio = DeserializarObjeto<ConsultarNfseEnvio>(file);
            //var strResult = SerializarObjeto(Service.ConsultarNfse(envio));
            //GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
            //                              Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            var envio = DeserializarObjeto<ConsultarNfseRpsEnvio>(file);
            var strResult = SerializarObjeto(Service.ConsultarNfseRps(envio));
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML, System.Text.Encoding.UTF8);
        }

        #endregion
    }
}