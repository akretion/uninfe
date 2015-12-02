#if false
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
using NFe.Components.br.com.etransparencia.nfe.p;

namespace NFe.Components.Conam.VarginhaMG.p
{
    public class ConamP : EmiteNFSeBase
    {
        ws_nfe service = new ws_nfe();
        string UsuarioWs = "";
        string SenhaWs = "";

        #region construtores
        public ConamP(TipoAmbiente tpAmb, string pastaRetorno, string usuario, string senhaWs)
            : base(tpAmb, pastaRetorno)
        {
            UsuarioWs = usuario;
            SenhaWs = senhaWs;
        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            Sdt_ProcessarpsIn oProcessaRpsIn = ReadXML<Sdt_ProcessarpsIn>(file);
            Sdt_ProcessarpsOut result = service.PROCESSARPS(oProcessaRpsIn);

            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.EnvLoteRps, Propriedade.ExtRetorno.LoteRps);

        }

        public override void CancelarNfse(string file)
        {
            Sdt_CancelaNFE oCancelaNFE = ReadXML<Sdt_CancelaNFE>(file);
            Sdt_RetornoCancelaNFE result = service.CANCELANOTAELETRONICA(oCancelaNFE);

            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.PedCanNfse, Propriedade.ExtRetorno.retCancelamento_XML);
        }

        public override void ConsultarLoteRps(string file)
        {
            SDT_ConsultaProtocoloIn oConsultaLoteRps = ReadXML<SDT_ConsultaProtocoloIn>(file);
            SDT_ConsultaNotasProtocoloOut result = service.CONSULTANOTASPROTOCOLO(oConsultaLoteRps);

            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.PedLoteRps, Propriedade.ExtRetorno.RetLoteRps);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        public override void ConsultarNfse(string file)
        {
            SDT_ConsultaProtocoloIn oConsultaProtocolo = ReadXML<SDT_ConsultaProtocoloIn>(file);
            SDT_ConsultaProtocoloOut result = service.CONSULTAPROTOCOLO(oConsultaProtocolo);

            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.PedSitNfse, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        private T ReadXML<T>(string file)
            where T : new()
        {
            T result = new T();

            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNodeList nodes = doc.GetElementsByTagName("nfe:" + result.GetType().Name);
            object rps = result;
            string tagName = "nfe:" + rps.GetType().Name;

            XmlNode node = nodes[0];
            ReadXML(node, rps, tagName);

            return result;
        }

        private object ReadXML(XmlNode node, object value, string tag)
        {
            if (value is IList)
            {
                IList<object> result = new List<object>();

                foreach (XmlNode item in node.ChildNodes)
                {
                    if (item.HasChildNodes && item.FirstChild.NodeType == XmlNodeType.Element)
                    {
                        Object instance =
                            System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(
                                "NFe.Components.br.com.etransparencia.nfehomologacao.h." + this.GetNameObject(item.Name, ""),
                                false,
                                BindingFlags.Default,
                                null,
                                new object[] { },
                                null,
                                null
                            );

                        SetProperty(result, "Add", ReadXML(item, instance, tag));
                    }
                    else
                        if (item.NodeType == XmlNodeType.Element)
                        {
                            SetProperty(value, item.Name, item.InnerXml);
                        }
                }
                value = result.ToArray();
            }
            else
                foreach (XmlNode n in node.ChildNodes)
                {
                    if (n.HasChildNodes && n.FirstChild.NodeType == XmlNodeType.Element)
                    {
                        string nomeObjeto = value.GetType().Name;

                        Object instance =
                        System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(
                            "NFe.Components.br.com.etransparencia.nfehomologacao.h." + this.GetNameObject(n.Name, nomeObjeto),
                            false,
                            BindingFlags.Default,
                            null,
                            new object[] { },
                            null,
                            null
                        );

                        if (n.Name.Equals("nfe:Reg20"))
                        {
                            instance = new List<NFe.Components.br.com.etransparencia.nfehomologacao.h.Sdt_ProcessarpsInSDTRPSReg20Item>();
                        }

                        if (n.Name.Equals("nfe:Reg30"))
                        {
                            instance = new List<NFe.Components.br.com.etransparencia.nfehomologacao.h.Sdt_ProcessarpsInSDTRPSReg20ItemReg30Item>();
                        }

                        SetProperty(value, n.Name, ReadXML(n, instance, n.Name));
                    }
                    else
                    {
                        if (n.NodeType == XmlNodeType.Element)
                        {
                            SetProperty(value, n.Name, n.InnerXml);
                        }
                    }
                }

            return value;
        }

        private void SetProperty(object result, string propertyName, object value)
        {
            if (result is IList)
            {
                MethodInfo mi = result.GetType().GetMethod("Add");
                mi.Invoke(result, new object[] { value });
            }
            else
            {
                PropertyInfo pi = result.GetType().GetProperty(GetNameProperty(propertyName));

                if (pi != null)
                {
                    if (value.GetType().IsArray)
                    {
                        Array arr = (Array)value;
                        Type arrayType = arr.Length == 0 ? arr.GetType().GetElementType() : arr.GetValue(0).GetType();
                        Array arrV = Array.CreateInstance(arrayType, arr.Length);
                        Array.Copy(arr, arrV, arr.Length);
                        value = arrV;
                    }
                    else
                    {
                        value = Convert.ChangeType(value, pi.PropertyType);
                    }
                    pi.SetValue(result, value, null);
                }
            }
        }

        private string GetNameObject(string tag, string parentTag)
        {
            string nameObject = "";

            switch (tag)
            {
                case "":
                    nameObject = "Não existe";
                    break;

                case "nfe:Reg20":
                    nameObject = parentTag + tag.Replace("nfe:", "") + "Item";
                    break;

                case "nfe:Reg30":
                    nameObject = parentTag + tag.Replace("nfe:", "") + "Item";
                    break;

                case "nfe:Reg20Item":
                    nameObject = "Sdt_ProcessarpsInSDTRPSReg20Item";
                    break;

                case "nfe:Reg30Item":
                    nameObject = "Sdt_ProcessarpsInSDTRPSReg20ItemReg30Item";
                    break;

                default:
                    nameObject = parentTag + tag.Replace("nfe:", "");
                    break;
            }

            return nameObject;
        }

        private string GetNameProperty(string nameTag)
        {
            return nameTag.Replace("nfe:", "");
        }
        #endregion
    }
}
#endif