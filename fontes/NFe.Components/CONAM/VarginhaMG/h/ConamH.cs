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
using NFe.Components.br.com.etransparencia.nfehomologacao.h;

namespace NFe.Components.Conam.VarginhaMG.h
{
    public class ConamH : EmiteNFSeBase
    {
        string UsuarioWs = "";
        string SenhaWs = "";
        int codigoMun = 0;
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region construtores
        public ConamH(TipoAmbiente tpAmb, string pastaRetorno, string usuario, string senhaWs, int CodigoMun)
            : base(tpAmb, pastaRetorno)
        {
            UsuarioWs = usuario;
            SenhaWs = senhaWs;
            codigoMun = CodigoMun;
        }
        #endregion

        #region Métodos
        private string Url(string cidade)
        {
            if (tpAmb == TipoAmbiente.taHomologacao)
                return "https://nfehomologacao.etransparencia.com.br/" + cidade + "/webservice/aws_nfe.aspx";
            else
                return "https://nfe.etransparencia.com.br/" + cidade + "/webservice/aws_nfe.aspx";

        }
        private ws_nfe _service = null;
        private ws_nfe service
        {
            get
            {
                if (_service == null)
                {
                    _service = new ws_nfe();
                }
                switch (codigoMun)
                {
                    case 3507001:
                        _service.Url = Url("sp.boituva");
                        break;
                    case 3509007:
                        _service.Url = Url("sp.caieiras");
                        break;
                    case 3522208:
                        _service.Url = Url("sp.itapecericadaserra");
                        break;
                    case 3525300:
                        _service.Url = Url("sp.jahu");
                        break;
                    case 3526902:
                        _service.Url = Url("sp.limeira");
                        break;
                    case 3528502:
                        _service.Url = Url("sp.mairipora");
                        break;
                    case 3539806:
                        _service.Url = Url("sp.poa");
                        break;
                    case 3552809:
                        _service.Url = Url("sp.taboaodaserra");
                        break;
                    case 3554102:
                        _service.Url = Url("sp.taubate");
                        break;
                    case 3170701:
                        _service.Url = Url("mg.varginha");
                        break;
                    case 3506102:
                        _service.Url = Url("sp.bebedouro");
                        break;
                }
                return _service;
            }
        }

        public override void EmiteNF(string file)
        {
            Sdt_ProcessarpsIn oProcessaRpsIn = ReadXML<Sdt_ProcessarpsIn>(file);
            Sdt_ProcessarpsOut result = service.PROCESSARPS(oProcessaRpsIn);
            
            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult,   Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML, 
                                            Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            Sdt_CancelaNFE oCancelaNFE = ReadXML<Sdt_CancelaNFE>(file);
            Sdt_RetornoCancelaNFE result = service.CANCELANOTAELETRONICA(oCancelaNFE);
            
            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult,   Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML, 
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        {
            SDT_ConsultaProtocoloIn oConsultaLoteRps = ReadXML<SDT_ConsultaProtocoloIn>(file);
            SDT_ConsultaNotasProtocoloOut result = service.CONSULTANOTASPROTOCOLO(oConsultaLoteRps);

            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult,   Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML, 
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
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
            GerarRetorno(file, strResult,   Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML, 
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

            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNodeList nodes = doc.GetElementsByTagName((tpAmb == TipoAmbiente.taProducao?"nfe:":"") + result.GetType().Name);

            if (nodes.Count == 0)
                nodes = doc.GetElementsByTagName(result.GetType().Name);

            object rps = result;
            string tagName = rps.GetType().Name;
            if (nodes[0] == null) throw new Exception("Tag <" + tagName + "> não encontrada");

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

                        switch(n.Name)
                        {
                            case "Reg20":
                            case "nfe:Reg20":
                                instance = new List<NFe.Components.br.com.etransparencia.nfehomologacao.h.Sdt_ProcessarpsInSDTRPSReg20Item>();
                                break;
                            case "Reg30":
                            case "nfe:Reg30":
                                instance = new List<NFe.Components.br.com.etransparencia.nfehomologacao.h.Sdt_ProcessarpsInSDTRPSReg20ItemReg30Item>();
                                break;
                        }
                        //if (n.Name.Equals((tpAmb == TipoAmbiente.taProducao?"nfe:":"") + "Reg20"))
                        //{
                        //    instance = new List<NFe.Components.br.com.etransparencia.nfehomologacao.h.Sdt_ProcessarpsInSDTRPSReg20Item>();
                        //}

                        //if (n.Name.Equals((tpAmb == TipoAmbiente.taProducao?"nfe:":"")+"Reg30"))
                        //{
                        //    instance = new List<NFe.Components.br.com.etransparencia.nfehomologacao.h.Sdt_ProcessarpsInSDTRPSReg20ItemReg30Item>();
                        //}

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
                        Type arrayType = arr.Length == 0?arr.GetType().GetElementType():arr.GetValue(0).GetType() ;
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

                case "Reg20":
                case "nfe:Reg20":
                    nameObject = parentTag + GetNameProperty(tag) /* tag.Replace("nfe:", "")*/ + "Item";
                    break;

                case "Reg30":
                case "nfe:Reg30":
                    nameObject = parentTag + GetNameProperty(tag) /* tag.Replace("nfe:", "")*/ + "Item";
                    break;

                case "Reg20Item":
                case "nfe:Reg20Item":
                    nameObject = "Sdt_ProcessarpsInSDTRPSReg20Item";
                    break;

                case "Reg30Item":
                case "nfe:Reg30Item":
                    nameObject = "Sdt_ProcessarpsInSDTRPSReg20ItemReg30Item";
                    break;

                default:
                    nameObject = parentTag + GetNameProperty(tag);// tag.Replace("nfe:", "");
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
