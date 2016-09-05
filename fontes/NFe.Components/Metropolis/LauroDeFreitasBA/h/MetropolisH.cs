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
using NFe.Components.br.com.metropolisweb.lfhomologacao.h;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.Metropolis.LauroDeFreitasBA.h
{
    public class MetropolisH : EmiteNFSeBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }

        }
        Nfse service = new Nfse();
        private string CabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"1.00\"><versaoDados >1.00</versaoDados ></cabecalho>";

        #region construtores
        public MetropolisH(TipoAmbiente tpAmb, string pastaRetorno, string proxyuser, string proxypass, string proxyserver, X509Certificate2 certificado)
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
            input objInput = new input();
            objInput.nfseCabecMsg = CabecMsg;
            objInput.nfseDadosMsg = ReadXML(file);                        
            GerarRetorno(file, service.RecepcionarLoteRps(objInput).outputXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                                                               Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            input objInput = new input();
            objInput.nfseCabecMsg = CabecMsg;
            objInput.nfseDadosMsg = ReadXML(file);
            GerarRetorno(file, service.CancelarNfse(objInput).outputXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                                                                         Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        {
            input objInput = new input();
            objInput.nfseCabecMsg = CabecMsg;
            objInput.nfseDadosMsg = ReadXML(file);
            GerarRetorno(file, service.ConsultarLoteRps(objInput).outputXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
                                                                             Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            input objInput = new input();
            objInput.nfseCabecMsg = CabecMsg;
            objInput.nfseDadosMsg = ReadXML(file);
            GerarRetorno(file, service.ConsultarSituacaoLoteRps(objInput).outputXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).EnvioXML,
                                                                                     Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).RetornoXML);
        }

        public override void ConsultarNfse(string file)
        {
            input objInput = new input();
            objInput.nfseCabecMsg = CabecMsg;
            objInput.nfseDadosMsg = ReadXML(file);
            GerarRetorno(file, service.ConsultarNfse(objInput).outputXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                                                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            input objInput = new input();
            objInput.nfseCabecMsg = CabecMsg;
            objInput.nfseDadosMsg = ReadXML(file);
            GerarRetorno(file, service.ConsultarNfsePorRps(objInput).outputXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML,
                                                                                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML);       
        }

        private string ReadXML(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            return doc.InnerXml;
        }
        #endregion
    }
}
