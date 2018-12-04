using System;
using NFe.Components.Abstract;
using NFe.Components.PCamboriuSC;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.Simple.CamboriuSC.p
{
    public class SimpleP : EmiteNFSeBase
    {
        public override string NameSpaces
        {
            get
            {
                return "http://tempuri.org";
            }
        }

        NFSePrefeituraCamboriu service = new NFSePrefeituraCamboriu();

        #region Construtores

        public SimpleP(TipoAmbiente tpAmb, string pastaRetorno, string proxyuser, string proxypass, string proxyserver, X509Certificate2 certificado)
            : base(tpAmb, pastaRetorno)
        {
            if (!String.IsNullOrEmpty(proxyuser))
            {
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(proxyuser, proxypass, proxyserver);
                System.Net.WebRequest.DefaultWebProxy.Credentials = credentials;

                service.Proxy = WebRequest.DefaultWebProxy;
                service.Proxy.Credentials = new NetworkCredential(proxyuser, proxypass);
                service.Credentials = new NetworkCredential(proxyuser, proxypass);
            }
            service.ClientCertificates.Add(certificado);
        }

        #endregion

        #region Métodos

        public override void EmiteNF(string file)
        {
            LeRPSeGravaNota oLeRPSeGravaNota = new LeRPSeGravaNota();
            oLeRPSeGravaNota = DeserializarObjeto<LeRPSeGravaNota>(file);
            Nota[] NotaAux = { oLeRPSeGravaNota.tNota.Nota };

            string strResult = SerializarObjeto(service.LeRPSeGravaNota(NotaAux, oLeRPSeGravaNota.iCMC, oLeRPSeGravaNota.sLogin, oLeRPSeGravaNota.sSenha)[0]);

            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);

        }

        public override void CancelarNfse(string file)
        {
            CancelarNota oCancelamento = new CancelarNota();
            oCancelamento = DeserializarObjeto<CancelarNota>(file);
            CancelamentoNota[] cancelNota = { oCancelamento.tCancelamentoNota.CancelamentoNota };

            string strResult = SerializarObjeto(service.CancelarNota(cancelNota, oCancelamento.iCMC, oCancelamento.sLogin, oCancelamento.sSenha)[0]);

            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        {
            throw new NotImplementedException();
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            throw new NotImplementedException();
        }

        public override void ConsultarNfse(string file)
        {
            ConsultaNota oFile = new ConsultaNota();
            oFile = DeserializarObjeto<ConsultaNota>(file);

            string strResult = SerializarObjeto(service.ConsultaNota(oFile.iCMC, oFile.sLogin, oFile.sSenha, oFile.iNota, oFile.sCPFCNPJ)[0]);

            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            ConsultaNotaporRPS oFile = new ConsultaNotaporRPS();
            oFile = DeserializarObjeto<ConsultaNotaporRPS>(file);

            string strResult = SerializarObjeto(service.ConsultaNotaporRPS(oFile.iCMC, oFile.sLogin, oFile.sSenha, oFile.iRPS, oFile.sCPFCNPJ, oFile.dDataRecibo)[0]);

            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
        }


        #endregion

    }
}
