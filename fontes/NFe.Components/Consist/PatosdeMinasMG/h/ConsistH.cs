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
using NFe.Components.br.com.consist.patosdeminas.h;
using System.Net;

namespace NFe.Components.Consist.PatosdeMinasMG.h
{
    public class ConsistH : EmiteNFSeBase
    {
        string UsuarioWs = "";
        string SenhaWs = "";
        eISSWService service = new eISSWService();
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region construtores
        public ConsistH(TipoAmbiente tpAmb, string pastaRetorno, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver)
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

            UsuarioWs = usuario;
            SenhaWs = senhaWs;
        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            eISSWService_RetornoEmiteNFe result = null;
            result = service.EmiteNFe(Convert.ToInt32(UsuarioWs),
                                      SenhaWs,
                                      ReadXML<eISSWService_NFeInfo>(file));

            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            eISSWService_RetornoCancelaNFe result = null;
            result = service.CancelaNFe(Convert.ToInt32(UsuarioWs),
                                        SenhaWs,
                                        Convert.ToInt32(GetValueXML(file, "CancelaNFe", "numNota")),
                                        GetValueXML(file, "CancelaNFe", "motivo"));

            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        public override void ConsultarNfse(string file)
        {
            eISSWService_RetornoConsultaNFe result = null;
            result = service.ConsultaNFe(Convert.ToInt32(UsuarioWs),
                                         SenhaWs,
                                         Convert.ToInt32(GetValueXML(file, "ConsultaNFe", "numNota")),
                                         Convert.ToInt32(GetValueXML(file, "ConsultaNFe", "numRPS")));

            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        private T ReadXML<T>(string file)
            where T : new()
        {
            T result = new T();
            result = (T)ReadXML2(file, result, GetNameProperty(result.GetType().Name.Substring(2)));
            return result;
        }

        private object ReadXML2(string file, object value, string tag)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNodeList nodes = doc.GetElementsByTagName(tag);
            XmlNode node = nodes[0];

            foreach (XmlNode n in node)
            {
                if (n.NodeType == XmlNodeType.Element)
                {
                    SetProperrty(value, n.Name, n.InnerXml);
                }
            }

            return value;
        }

        private void SetProperrty(object result, string propertyName, object value)
        {
            PropertyInfo pi = result.GetType().GetProperty(propertyName);

            if (propertyName.Equals("descServicos"))
                value = new String[] { value.ToString() };
            else
                value = Convert.ChangeType(value, pi.PropertyType);

            if (pi != null)
                pi.SetValue(result, value, null);
        }

        private string GetNameProperty(string classname)
        {
            string result = classname;

            switch (classname)
            {
                case "SSWService_NFeInfo":
                    result = "aNFeInfo";
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

        #endregion
    }
}
