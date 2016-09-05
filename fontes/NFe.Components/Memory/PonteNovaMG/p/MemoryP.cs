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
using NFe.Components.br.com.memory.pontenovamg.p;
using System.Net;

namespace NFe.Components.Memory.PonteNovaMG.p
{
    public class MemoryP : EmiteNFSeBase
    {
        loterpswsdl service = new loterpswsdl();
        string CodigoMun = "";
        string HashWs = "";

        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        #region construtores
        public MemoryP(TipoAmbiente tpAmb, string pastaRetorno, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver, int codMun)
            : base(tpAmb, pastaRetorno)
        {
            CodigoMun = codMun.ToString();
            HashWs = senhaWs;

            if (!String.IsNullOrEmpty(proxyuser))
            {
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(proxyuser, proxypass, proxyserver);
                System.Net.WebRequest.DefaultWebProxy.Credentials = credentials;

                service.Proxy = WebRequest.DefaultWebProxy;
                service.Proxy.Credentials = new NetworkCredential(proxyuser, proxypass);
                service.Credentials = new NetworkCredential(proxyuser, proxypass);
            }

        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            string result = service.tm_lote_rps_serviceimportarLoteRPS(ReadXML(file),
                                                                       CodigoMun,
                                                                       GetValueXML(file, "LoteRps", "Cnpj"),
                                                                       HashWs);

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);

        }

        public override void CancelarNfse(string file)
        {
            string result = service.tm_lote_rps_servicecancelarNFSE(GetValueXML(file, "cancelarNFSE", "numeroNFSE"),
                                                                    CodigoMun,
                                                                    GetValueXML(file, "cancelarNFSE", "cnpjPrestador"),
                                                                    HashWs);

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);

        }

        public override void ConsultarLoteRps(string file)
        {
            string result = service.tm_lote_rps_serviceconsultarLoteRPS(GetValueXML(file, "consultarLoteRPS", "protocolo"),
                                                                        CodigoMun,
                                                                        GetValueXML(file, "consultarLoteRPS", "cnpjPrestador"),
                                                                        HashWs);

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
        }
        
        public override void ConsultarSituacaoLoteRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        public override void ConsultarNfse(string file)
        {
            string result = service.tm_lote_rps_serviceconsultarNFSE(GetValueXML(file, "consultarNFSE", "numeroNFSE"),
                                                                     CodigoMun,
                                                                     GetValueXML(file, "consultarNFSE", "cnpjPrestador"),
                                                                     HashWs);

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            string result = service.tm_lote_rps_serviceconsultarRPS(GetValueXML(file, "consultarRPS", "numeroRPS"),
                                                                    CodigoMun,
                                                                    GetValueXML(file, "consultarRPS", "cnpjPrestador"),
                                                                    HashWs);
            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML);
        }

        private string ReadXML(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            return doc.InnerXml;
        }

        private string GetValueXML(string file, string elementTag, string valueTag)
        {
            string value = "";
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNodeList nodes = doc.GetElementsByTagName(elementTag);
            XmlNode node = nodes[0];

            foreach (XmlNode n in node)
            {
                if (n.NodeType == XmlNodeType.Element)
                {
                    if (n.Name.Equals(valueTag))
                    {
                        value = n.InnerText;
                        break;
                    }
                }
            }

            return value;
        }

        #endregion
    }
}
