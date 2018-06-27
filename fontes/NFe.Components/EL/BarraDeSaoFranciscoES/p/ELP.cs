using NFe.Components.Abstract;
using NFe.Components.PBarraDeSaoFranciscoES;
using System;
using System.Net;
using System.Xml;

namespace NFe.Components.EL.BarraDeSaoFranciscoES.p
{
    public class ELP : EmiteNFSeBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        RpsServiceService service = new RpsServiceService();
        NetworkCredential Credentials;
        string UsuarioWs = "";
        string SenhaWs = "";
        string UsuarioProxy = "";
        string SenhaProxy = "";
        string DomainProxy = "";

        #region construtores
        public ELP(TipoAmbiente tpAmb,
                         string pastaRetorno,
                         string usuarioWs,
                         string senhaWs,
                         string usuarioProxy,
                         string senhaProxy,
                         string domainProxy)
            : base(tpAmb, pastaRetorno)
        {
            UsuarioWs = usuarioWs;
            SenhaWs = senhaWs;
            UsuarioProxy = usuarioProxy;
            SenhaProxy = senhaProxy;
            DomainProxy = domainProxy;

            if (!String.IsNullOrEmpty(UsuarioProxy))
            {
                Credentials = new NetworkCredential(UsuarioProxy, SenhaProxy, DomainProxy);
                WebRequest.DefaultWebProxy.Credentials = Credentials;
                service.Proxy = WebRequest.DefaultWebProxy;
                service.Proxy.Credentials = new NetworkCredential(usuarioProxy, senhaProxy);
                service.Credentials = new NetworkCredential(senhaProxy, senhaProxy);
            }
        }
        #endregion construtores

        #region Métodos
        public override void EmiteNF(string file)
        {
            loteRpsResposta result = service.EnviarLoteRpsEnvio(UsuarioWs, SenhaWs, XMLtoString(file));

            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            string numeroNfse = GetValueXML(file, "IdentificacaoNfse", "Numero");
            nfseRpsResposta result = service.CancelarNfseEnvio(UsuarioWs, numeroNfse);
            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        {
            string numeroProtocolo = GetValueXML(file, "ConsultarLoteRpsEnvio", "Protocolo");
            nfseResposta result = service.ConsultarLoteRpsEnvio(UsuarioWs, numeroProtocolo);
            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            string numeroProtocolo = GetValueXML(file, "ConsultarSituacaoLoteRpsEnvio", "Protocolo");
            situacaoLoteRps result = service.ConsultarSituacaoLoteRpsEnvio(UsuarioWs, numeroProtocolo);
            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).RetornoXML);
        }

        public override void ConsultarNfse(string file)
        {
            string numeroNfse = GetValueXML(file, "IdentificacaoRps", "Numero");

            DateTime dataInicial = Convert.ToDateTime(GetValueXML(file, "PeriodoEmissao", "DataInicial"));
            bool dataInicialDef = String.IsNullOrEmpty(dataInicial.Date.ToString()) ? false : true;

            DateTime dataFinal = Convert.ToDateTime(GetValueXML(file, "PeriodoEmissao", "DataFinal"));
            bool dataFinalDef = String.IsNullOrEmpty(dataFinal.Date.ToString()) ? false : true; ;

            string identificacaoTomador = GetValueXML(file, "Tomador", "Cnpj");
            if (String.IsNullOrEmpty(identificacaoTomador))
                identificacaoTomador = GetValueXML(file, "Tomador", "Cpf");

            string identificacaoIntermediario = GetValueXML(file, "IntermediarioServico", "Cnpj");
            if (String.IsNullOrEmpty(identificacaoIntermediario))
                identificacaoIntermediario = GetValueXML(file, "IntermediarioServico", "Cpf");

            nfseResposta result = service.ConsultarNfseEnvio(UsuarioWs, numeroNfse, dataFinal, dataInicialDef, dataFinal, dataFinalDef, identificacaoTomador, identificacaoIntermediario);
            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            string identificacaoRps = GetValueXML(file, "IdentificacaoRps", "Numero");
            nfseRpsResposta result = service.ConsultarNfseRpsEnvio(identificacaoRps, UsuarioWs);
            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML);
        }

        private string XMLtoString(string arquivo)
        {
            XmlDocument docXML = new XmlDocument();
            docXML.Load(arquivo);
            return docXML.OuterXml;
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

        #endregion Métodos
    }
}