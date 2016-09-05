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
using NFe.Components.br.com.elotech.quatrobarras.hp;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.EloTech.QuatroBarrasPR
{
    public class EloTechHP : EmiteNFSeBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        NfsePortService service = new NfsePortService();
        X509Certificate2 Certificado;

        #region construtores
        public EloTechHP(TipoAmbiente tpAmb, string pastaRetorno, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver, X509Certificate2 certificado)
            : base(tpAmb, pastaRetorno)
        {
            if (!String.IsNullOrEmpty(proxyuser))
            {
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(proxyuser, proxypass, proxyserver);
                System.Net.WebRequest.DefaultWebProxy.Credentials = credentials;

                service.Proxy = WebRequest.DefaultWebProxy;
                service.Proxy.Credentials = new NetworkCredential(proxyuser, proxypass);
                service.Credentials = new NetworkCredential(proxyuser, proxypass);
                Certificado = certificado;
                AddClientCertificates();
            }
        }
        #endregion

        #region Métodos
        private void AddClientCertificates()
        {
            X509CertificateCollection certificates = null;
            Type t = service.GetType();
            PropertyInfo pi = t.GetProperty("ClientCertificates");
            certificates = pi.GetValue(service, null) as X509CertificateCollection;
            certificates.Add(Certificado);
        }

        public override void EmiteNF(string file)
        {
            EnviarLoteRpsEnvio oEnviarLoteRpsEnvio = ReadXML<EnviarLoteRpsEnvio>(file);            
            EnviarLoteRpsResposta result = service.EnviarLoteRps(oEnviarLoteRpsEnvio);

            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult,   Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            CancelarNfseEnvio oCancelarNfseEnvio = ReadXML<CancelarNfseEnvio>(file);
            CancelarNfseResposta result = service.CancelarNfse(oCancelarNfseEnvio);

            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult,   Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);

            /*
            object erros = new object[1];

            CancelarNfseEnvio oTcDadosCancela = ReadXML<CancelarNfseEnvio>(file);
            CancelarNfseResposta result = service.CancelarNfse(oTcDadosCancela, dadosConexao);
            */
        }

        public override void ConsultarLoteRps(string file)
        {
            /*
            object erros = new object[1];

            ConsultarLoteRpsEnvio oTcDadosConsultaNota = ReadXML<ConsultarLoteRpsEnvio>(file);
            ConsultarLoteRpsResposta result = service.ConsultarLoteRps(oTcDadosConsultaNota, dadosConexao);
            string strResult = base.CreateXML(result, erros);
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.PedLoteRps[0], Propriedade.ExtRetorno.RetLoteRps);
            */
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            /*
            object erros = new object[1];

            ConsultarSituacaoLoteRpsEnvio oTcDadosConsultaNota = ReadXML<ConsultarSituacaoLoteRpsEnvio>(file);
            ConsultarSituacaoLoteRpsResposta result = service.ConsultarSituacaoLoteRps(oTcDadosConsultaNota, dadosConexao);
            string strResult = base.CreateXML(result, erros);
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.PedLoteRps[], Propriedade.ExtRetorno.RetLoteRps);
            */
        }

        public override void ConsultarNfse(string file)
        {
            /*
            object erros = new object[1];

            ConsultarNfseEnvio oTcDadosPrestador = ReadXML<ConsultarNfseEnvio>(file);
            ConsultarNfseResposta result = service.ConsultarNfse(oTcDadosPrestador, dadosConexao);
            string strResult = base.CreateXML(result, erros);
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.PedSitNfse[0], Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
            */
        }

        public override void ConsultarNfsePorRps(string file)
        {
            /*
            object erros = new object[1];

            ConsultarNfseRpsEnvio oTcConsultarNfseRpsEnvio = ReadXML<ConsultarNfseRpsEnvio>(file);
            ConsultarNfseRpsResposta result = service.ConsultarNfsePorRps(oTcConsultarNfseRpsEnvio, dadosConexao);
            string strResult = base.CreateXML(result, erros);
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.PedSitNfseRps[0], Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML);
            */
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
            if (node == null) throw new Exception("Tag <" + result.GetType().Name + "> não encontrada");
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

                        if (n.Name.Equals("ListaItensServico"))
                        {
                            instance = new tcItemServico[1];
                        }
                        else if (n.Name.Equals("ListaRps"))
                        {
                            instance = new tcDeclaracaoPrestacaoServico[1];                            
                        }
                        else
                        {
                            instance = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(
                                "NFe.Components.br.com.elotech.quatrobarras.hp." + this.GetNameObject(n.Name),
                                false,
                                BindingFlags.Default,
                                null,
                                new object[] { },
                                null,
                                null
                            );
                        }

                        SetProperty(value, GetNameProperty(n.Name), ReadXML(n, instance, n.Name));
                    }
                    else
                    {
                        if (n.NodeType == XmlNodeType.Element)
                        {
                            SetProperty(value, n.Name, n.InnerXml);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private void SetProperty(object result, string propertyName, object value)
        {
            PropertyInfo pi = result.GetType().GetProperty(propertyName);


            if (result.GetType().IsArray && propertyName.Equals("ItensServico"))
            {
                ((tcItemServico[])result).SetValue(value, 0);
            }
            else if (result.GetType().IsArray && propertyName.Equals("DeclaracaoPrestacaoServico"))
            {                
                ((tcDeclaracaoPrestacaoServico[])result).SetValue(value, 0);                                                
            }
            else
            {
                if (pi != null)
                {
                    value = Convert.ChangeType(value, pi.PropertyType);
                    pi.SetValue(result, value, null);
                }
            }
        }

        private string GetNameObject(string tag)
        {
            string nameObject = "";

            switch (tag)
            {
                case "Rps":
                    nameObject = "tcInfRps";
                    break;

                case "ConstrucaoCivil":
                case "Tomador":
                case "Servico":
                    nameObject = "tcDados" + tag;
                    break;

                case "Valores":
                    nameObject = "tcValoresDeclaracaoServico";
                    break;

                case "Pedido":  
                    nameObject = "tcPedidoCancelamento";
                    break;

                default:
                    nameObject = "tc" + tag;
                    break;
            }

            return nameObject;
        }

        private string GetNameProperty(string property)
        {
            string nameProperty = "";

            switch (property)
            {
                case "ItemServico":
                    nameProperty = "ItensServico";
                    break;

                default:
                    nameProperty = property;
                    break;
            }

            return nameProperty;
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
