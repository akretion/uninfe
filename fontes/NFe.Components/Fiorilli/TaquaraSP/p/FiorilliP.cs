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
using NFe.Components.br.net.taquarituba.fiorilli.p;

namespace NFe.Components.Fiorilli.TaquaraSP.p
{
    public class FiorilliP : EmiteNFSeBase
    {
        IssWebWS service = new IssWebWS();
        string UsuarioWs = "";
        string SenhaWs = "";

        #region construtores
        public FiorilliP(TipoAmbiente tpAmb, string pastaRetorno, string usuario, string senhaWs)
            : base(tpAmb, pastaRetorno)
        {
            System.Net.ServicePointManager.Expect100Continue = false;

            UsuarioWs = usuario;
            SenhaWs = senhaWs;
        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            object erros = new object[1];

            GerarNfseEnvio oGerarNfseEnvio = ReadXML<GerarNfseEnvio>(file);
            GerarNfseResposta result = service.gerarNfse(oGerarNfseEnvio, UsuarioWs, SenhaWs);
            string strResult = base.CreateXML(result, erros);
            GerarRetorno(file, strResult,   Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML, 
                                            Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            object erros = new object[1];

            CancelarNfseEnvio oTcDadosCancela = ReadXML<CancelarNfseEnvio>(file);
            CancelarNfseResposta result = service.cancelarNfse(oTcDadosCancela, UsuarioWs, SenhaWs);
            string strResult = base.CreateXML(result, erros);
            GerarRetorno(file, strResult,   Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML, 
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        {
            object erros = new object[1];

            ConsultarLoteRpsEnvio oTcDadosConsultaNota = ReadXML<ConsultarLoteRpsEnvio>(file);
            ConsultarLoteRpsResposta result = service.consultarLoteRps(oTcDadosConsultaNota, UsuarioWs, SenhaWs);
            string strResult = base.CreateXML(result, erros);
            GerarRetorno(file, strResult,   Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML, 
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        public override void ConsultarNfse(string file)
        {
            throw new Exceptions.ServicoInexistenteException();

            /*
             * Não foi possivel identificar como funciona este serviço. Renan 06/11/2014
             * 
            
            object erros = new object[1];

            ConsultarNfseFaixaEnvio oTcDadosPrestador = ReadXML<ConsultarNfseFaixaEnvio>(file);
            ConsultarNfseFaixaResposta result = service.consultarNfsePorFaixa(oTcDadosPrestador, UsuarioWs, SenhaWs);
            string strResult = base.CreateXML(result, erros);
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.PedSitNfse[], Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
            */
        }

        public override void ConsultarNfsePorRps(string file)
        {
            object erros = new object[1];

            ConsultarNfseRpsEnvio oTcConsultarNfseRpsEnvio = ReadXML<ConsultarNfseRpsEnvio>(file);
            ConsultarNfseRpsResposta result = service.consultarNfsePorRps(oTcConsultarNfseRpsEnvio, UsuarioWs, SenhaWs);
            string strResult = base.CreateXML(result, erros);
            GerarRetorno(file, strResult,   Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML, 
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

            if (typeof(T).FullName == typeof(GerarNfseEnvio).FullName)
            {
                ((GerarNfseEnvio)(object)result).Rps = new tcDeclaracaoPrestacaoServico();
                nodes = doc.GetElementsByTagName("Rps");
                if (nodes[0] == null) throw new Exception("Tag <Rps> não encontrada");
                rps = ((GerarNfseEnvio)(object)result).Rps;
                tagName = "InfDeclaracaoPrestacaoServico";
            }
            else
                if (typeof(T).FullName == typeof(CancelarNfseEnvio).FullName)
                {
                    ((CancelarNfseEnvio)(object)result).Pedido = new tcPedidoCancelamento();
                    nodes = doc.GetElementsByTagName("Pedido");
                    if (nodes[0] == null) throw new Exception("Tag <Pedido> não encontrada");
                    rps = ((CancelarNfseEnvio)(object)result).Pedido;
                    tagName = "InfPedidoCancelamento";
                }else
                    if (nodes[0] == null) throw new Exception("Tag <" + result.GetType().Name + "> não encontrada");

            XmlNode node = nodes[0];
            ReadXML(node, rps, tagName);

            SignObject(doc, rps);

            return result;
        }

        private void SignObject(XmlDocument doc, object rps)
        {
            XmlNodeList nodes = doc.GetElementsByTagName("Signature");
            
            if (nodes.Count > 0)
            {
                SignatureType sign = new SignatureType();

                //Grupo: Signature->SignedInfo
                sign.SignedInfo = new SignedInfoType();

                sign.SignedInfo.CanonicalizationMethod = new CanonicalizationMethodType();
                sign.SignedInfo.CanonicalizationMethod.Algorithm = doc.GetElementsByTagName("CanonicalizationMethod")[0].Attributes[0].Value; // Tag: CanonicalizationMethod

                sign.SignedInfo.SignatureMethod = new SignatureMethodType();
                sign.SignedInfo.SignatureMethod.Algorithm = doc.GetElementsByTagName("SignatureMethod")[0].Attributes[0].Value; // Tag: SignatureMethod

                // Grupo: Signature->SignedInfo->Reference
                sign.SignedInfo.Reference = new ReferenceType[1];

                ReferenceType teste = new ReferenceType();

                teste.URI = doc.GetElementsByTagName("Reference")[0].Attributes[0].Value;
                teste.DigestMethod = new DigestMethodType();
                teste.DigestMethod.Algorithm = doc.GetElementsByTagName("DigestMethod")[0].Attributes[0].Value;
                teste.DigestValue = GetBytes(doc.GetElementsByTagName("DigestValue")[0].InnerText);
                sign.SignedInfo.Reference[0] = teste;

                // Grupo: Signature->SignedInfo->Reference->Transforms
                XmlNodeList transforms = doc.GetElementsByTagName("Transform");
                sign.SignedInfo.Reference[0].Transforms = new TransformType[transforms.Count];

                int run = 0;
                foreach (XmlNode item in transforms)
                {
                    TransformType qq = new TransformType();
                    qq.Algorithm = item.Attributes[0].Value;
                    sign.SignedInfo.Reference[0].Transforms[run] = qq;
                    run += 1;
                }

                //Tag: Signature->SignatureValue
                sign.SignatureValue = new SignatureValueType();
                sign.SignatureValue.Value = GetBytes(doc.GetElementsByTagName("SignatureValue")[0].InnerText);

                //Grupo: Signature->KeyInfo
                sign.KeyInfo = new KeyInfoType();
                X509DataType x509 = new X509DataType();
                x509.Items = new object[1];
                x509.Items[0] = GetBytes(doc.GetElementsByTagName("X509Certificate")[0].InnerText);
                x509.ItemsElementName = new ItemsChoiceType[1] { ItemsChoiceType.X509Certificate };

                sign.KeyInfo.Items = new object[1];
                sign.KeyInfo.Items[0] = x509;

                sign.KeyInfo.ItemsElementName = new ItemsChoiceType2[1] { ItemsChoiceType2.X509Data };

                SetProperty(rps, "Signature", sign);
            }
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private object ReadXML(XmlNode node, object value, string tag)
        {

            foreach (XmlNode n in node.ChildNodes)
            {
                if (node.Name == "Signature") continue;

                if (n.HasChildNodes && n.FirstChild.NodeType == XmlNodeType.Element)
                {
                    Object instance =
                    System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(
                        "NFe.Components.br.net.taquarituba.fiorilli.h." + this.GetNameObject(n.Name),
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
                        SetProperty(value, n.Name, n.InnerXml);
                    }
                }
            }

            return value;
        }

        private void SetProperty(object result, string propertyName, object value)
        {
            PropertyInfo pi = result.GetType().GetProperty(propertyName);

            if (pi != null)
            {
                value = Convert.ChangeType(value, pi.PropertyType);
                pi.SetValue(result, value, null);
            }
        }

        private string GetNameObject(string tag)
        {
            string nameObject = "";

            switch (tag)
            {

                case "Pedido":
                    nameObject = "tcPedidoCancelamento";
                    break;

                case "Rps":
                    nameObject = "tcInfRps";
                    break;

                case "IdentificacaoRps":
                    nameObject = "tcIdentificacaoRps";
                    break;

                case "Servico":
                    nameObject = "tcDadosServico";
                    break;

                case "Valores":
                    nameObject = "tcValoresDeclaracaoServico";
                    break;

                case "Prestador":
                    nameObject = "tcIdentificacaoPrestador";
                    break;

                case "Tomador":
                    nameObject = "tcDadosTomador";
                    break;

                case "CpfCnpj":
                    nameObject = "tcCpfCnpj";
                    break;

                case "InfDeclaracaoPrestacaoServico":
                    nameObject = "tcInfDeclaracaoPrestacaoServico";
                    break;

                case "IdentificacaoTomador":
                    nameObject = "tcIdentificacaoTomador";
                    break;

                case "Endereco":
                    nameObject = "tcEndereco";
                    break;

                case "Contato":
                    nameObject = "tcContato";
                    break;

                default:
                    nameObject = "tc" + tag;
                    break;
            }

            return nameObject;
        }
        #endregion
    }
}
