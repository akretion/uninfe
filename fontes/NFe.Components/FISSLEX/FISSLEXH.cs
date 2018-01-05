using NFe.Components.Abstract;
using NFe.Components.br.com.fisslex.demo.aws_consultaloterps.h;
using NFe.Components.br.com.fisslex.demo.aws_consultanfse.h;
using NFe.Components.br.com.fisslex.demo.aws_consultanfseporrps.h;
using NFe.Components.br.com.fisslex.demo.aws_consultarsituacaoloterps.h;
using System;
using System.Net;
using System.Reflection;
using System.Xml;

namespace NFe.Components.FISSLEX.h
{
    public class FISSLEXH : EmiteNFSeBase
    {
        public override string NameSpaces
        {
            get
            {
                return "FISS-LEX";
            }
        }

        private string ProxyUser = "";
        private string ProxyPass = "";
        private string ProxyServer = "";
        private TipoAmbiente Ambiente;

        private WS_ConsultaLoteRps ServiceConsultaLoteRps = new WS_ConsultaLoteRps();
        private WS_ConsultarSituacaoLoteRps ServiceConsultarSituacaoLoteRps = new WS_ConsultarSituacaoLoteRps();
        private WS_ConsultaNfse ServiceConsultaNfse = new WS_ConsultaNfse();
        private WS_ConsultaNfsePorRps ServiceConsultaNfsePorRps = new WS_ConsultaNfsePorRps();

        #region construtores

        public FISSLEXH(TipoAmbiente tpAmb, string pastaRetorno, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver)
            : base(tpAmb, pastaRetorno)
        {
            Ambiente = tpAmb;

            #region Definições de proxy

            if (!String.IsNullOrEmpty(proxyuser))
            {
                ProxyUser = proxyuser;
                ProxyPass = proxypass;
                ProxyServer = proxyserver;

                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(proxyuser, proxypass, proxyserver);
                System.Net.WebRequest.DefaultWebProxy.Credentials = credentials;
            }

            #endregion Definições de proxy
        }

        #endregion construtores

        #region Métodos

        public override void ConsultarSituacaoLoteRps(string file)
        {
            #region Definições de proxy

            if (!String.IsNullOrEmpty(ProxyUser))
            {
                ServiceConsultarSituacaoLoteRps.Proxy = WebRequest.DefaultWebProxy;
                ServiceConsultarSituacaoLoteRps.Proxy.Credentials = new NetworkCredential(ProxyUser, ProxyPass);
                ServiceConsultarSituacaoLoteRps.Credentials = new NetworkCredential(ProxyUser, ProxyPass);
            }

            #endregion Definições de proxy

            ConsultarSituacaoLoteRpsEnvio envio = DeserializarObjeto<ConsultarSituacaoLoteRpsEnvio>(file);
            string strResult = SerializarObjeto(ServiceConsultarSituacaoLoteRps.Execute(envio));

            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedSitLoteRps).RetornoXML);
        }

        public override void EmiteNF(string file)
        {
            //Envio de NFSe não é por referencia, utilizamos a forma de envio mais simples. Wandrey
        }

        public override void CancelarNfse(string file)
        {
            //Envio de cancelamento não é por referencia, utilizamos a forma de envio mais simples. Wandrey
        }

        public override void ConsultarLoteRps(string file)
        {
            #region Definições de proxy

            if (!String.IsNullOrEmpty(ProxyUser))
            {
                ServiceConsultaLoteRps.Proxy = WebRequest.DefaultWebProxy;
                ServiceConsultaLoteRps.Proxy.Credentials = new NetworkCredential(ProxyUser, ProxyPass);
                ServiceConsultaLoteRps.Credentials = new NetworkCredential(ProxyUser, ProxyPass);
            }

            #endregion Definições de proxy

            ConsultarLoteRpsEnvio oTcDadosConsultaLote = ReadXML<ConsultarLoteRpsEnvio>(file);
            NFe.Components.br.com.fisslex.demo.aws_consultaloterps.h.tcMensagemRetorno[] result = null;

            ServiceConsultaLoteRps.Execute(oTcDadosConsultaLote, out result);

            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
        }

        public override void ConsultarNfse(string file)
        {
            #region Definições de proxy

            if (!String.IsNullOrEmpty(ProxyUser))
            {
                ServiceConsultaNfse.Proxy = WebRequest.DefaultWebProxy;
                ServiceConsultaNfse.Proxy.Credentials = new NetworkCredential(ProxyUser, ProxyPass);
                ServiceConsultaNfse.Credentials = new NetworkCredential(ProxyUser, ProxyPass);
            }

            #endregion Definições de proxy

            ConsultarNfseEnvio oTcDadosConsultaNfse = ReadXML<ConsultarNfseEnvio>(file);
            NFe.Components.br.com.fisslex.demo.aws_consultanfse.h.tcMensagemRetorno[] result = null;
            string strResult = "";

            strResult = ServiceConsultaNfse.Execute(oTcDadosConsultaNfse, out result);

            strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            #region Definições de proxy

            if (!String.IsNullOrEmpty(ProxyUser))
            {
                ServiceConsultaNfsePorRps.Proxy = WebRequest.DefaultWebProxy;
                ServiceConsultaNfsePorRps.Proxy.Credentials = new NetworkCredential(ProxyUser, ProxyPass);
                ServiceConsultaNfsePorRps.Credentials = new NetworkCredential(ProxyUser, ProxyPass);
            }

            #endregion Definições de proxy

            ConsultarNfseRpsEnvio oTcDadosConsultaNfse = ReadXML<ConsultarNfseRpsEnvio>(file);
            NFe.Components.br.com.fisslex.demo.aws_consultanfseporrps.h.tcMensagemRetorno[] result = null;

            string xResult = ServiceConsultaNfsePorRps.Execute(oTcDadosConsultaNfse, out result);

            string strResult = string.Empty;
            if (result.Length != 0)
                strResult = base.CreateXML(result);
            else
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xResult);
                strResult = xmlDoc.OuterXml;
            }

            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML);
        }

        private T ReadXML<T>(string file)
            where T : new()
        {
            T result = new T();

            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNodeList nodes = doc.GetElementsByTagName(result.GetType().Name);
            object rps = result;
            string tagName = rps.GetType().Name;

            XmlNode node = nodes[0];
            ReadXML(node, rps, tagName);

            return result;
        }

        private object ReadXML(XmlNode node, object value, string tag)
        {
            try
            {
                foreach (XmlNode n in node.ChildNodes)
                {
                    if (node.Name == "Signature") continue;

                    if (n.HasChildNodes && n.FirstChild.NodeType == XmlNodeType.Element)
                    {
                        Object instance = null;

                        if (tag.Equals("Tomador") && n.Name.Equals("CpfCnpj"))
                        {
                            NFe.Components.br.com.fisslex.demo.aws_consultanfse.h.tcIdentificacaoTomador instances = new NFe.Components.br.com.fisslex.demo.aws_consultanfse.h.tcIdentificacaoTomador();
                            instance = instances.CpfCnpj = new tcCpfCnpj();
                        }
                        else if (tag.Equals("IntermediarioServico") && n.Name.Equals("CpfCnpj"))
                        {
                            NFe.Components.br.com.fisslex.demo.aws_consultanfse.h.tcIdentificacaoIntermediarioServico instances = new NFe.Components.br.com.fisslex.demo.aws_consultanfse.h.tcIdentificacaoIntermediarioServico();
                            instance = instances.CpfCnpj = new tcCpfCnpj();
                        }
                        else
                            instance =
                            System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(
                                this.GetNameClass(tag) + this.GetNameObject(n.Name),
                                false,
                                BindingFlags.Default,
                                null,
                                new object[] { },
                                null,
                                null
                            );

                        SetProperty(value, n.Name, ReadXML(n, instance, n.Name));
                    }
                    else
                    {
                        if (n.NodeType == XmlNodeType.Element)
                        {
                            SetProperty(value, GetNameProperty(n.Name), n.InnerXml);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return value;
        }

        private void SetProperty(object result, string propertyName, object value)
        {
            PropertyInfo pi = result.GetType().GetProperty(propertyName);

            if (pi != null)
            {
                value = ChangeType(value, pi.PropertyType);

                pi.SetValue(result, value, null);
            }
        }

        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }

        private string GetNameObject(string tag)
        {
            string nameObject = "";

            switch (tag)
            {
                case "Prestador":
                    nameObject = "tcIdentificacaoPrestador";
                    break;

                case "PeriodoEmissao":
                    nameObject = "ABRASFConsultarNfseEnvioPeriodoEmissao";
                    break;

                case "Tomador":
                    nameObject = "tcIdentificacaoTomador";
                    break;

                case "IntermediarioServico":
                    nameObject = "tcIdentificacaoIntermediarioServico";
                    break;

                default:
                    nameObject = "tc" + tag;
                    break;
            }

            return nameObject;
        }

        private string GetNameProperty(string name)
        {
            string result = name;

            switch (name)
            {
                case "PeriodoEmissao":
                    result = "ABRASFConsultarNfseEnvioPeriodoEmissao";
                    break;

                default:
                    break;
            }

            return result;
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

        private string GetNameClass(string tag)
        {
            string result;
            string munURL = (Ambiente == TipoAmbiente.taProducao ? "sinop" : "demo");
            string ambURL = (Ambiente == TipoAmbiente.taProducao ? "p" : "h");

            switch (tag)
            {
                case "ConsultarLoteRpsEnvio":
                    result = "NFe.Components.br.com.fisslex." + munURL + ".aws_consultaloterps." + ambURL + ".";
                    break;

                case "ConsultarSituacaoLoteRpsEnvio":
                    result = "NFe.Components.br.com.fisslex." + munURL + ".aws_consultarsituacaoloterps." + ambURL + ".";
                    break;

                case "ConsultarNfseEnvio":
                    result = "NFe.Components.br.com.fisslex." + munURL + ".aws_consultanfse." + ambURL + ".";
                    break;

                case "ConsultarNfseRpsEnvio":
                    result = "NFe.Components.br.com.fisslex." + munURL + ".aws_consultanfseporrps." + ambURL + ".";
                    break;

                default:
                    result = "";
                    break;
            }

            return result;
        }

        #endregion Métodos
    }
}