using NFe.Components.Abstract;
using NFe.Components.br.gov.pr.curitiba.pilotoisscuritiba.h;
using System;
using System.Reflection;
using System.Xml;

namespace NFe.Components.EGoverne.CuritibaPR
{
    public class EGoverneSerialization : EmiteNFSeBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        string UsuarioProxy = "";
        string SenhaProxy = "";
        string DomainProxy = "";
        string WsMethod = "";

        public EGoverneSerialization(TipoAmbiente tpAmb, string pastaRetorno, string usuarioProxy, string senhaProxy, string domainProxy)
            : base(tpAmb, pastaRetorno)
        {
            UsuarioProxy = usuarioProxy;
            SenhaProxy = senhaProxy;
            DomainProxy = domainProxy;

            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(UsuarioProxy, SenhaProxy, DomainProxy);
            System.Net.WebRequest.DefaultWebProxy.Credentials = credentials;
        }

        public override void EmiteNF(string file)
        {
            throw new NotImplementedException();
        }

        public override void CancelarNfse(string file)
        {
            throw new NotImplementedException();
        }

        public override void ConsultarLoteRps(string file)
        {
            throw new NotImplementedException();
        }

        public override void ConsultarNfse(string file)
        {
            throw new NotImplementedException();
        }

        public override void ConsultarNfsePorRps(string file)
        {
            throw new NotImplementedException();
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            throw new NotImplementedException();
        }

        public T ReadXML<T>(string file)
             where T : new()
        {
            T result = new T();

            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNodeList nodes = doc.GetElementsByTagName(result.GetType().Name);
            object rps = result;
            string tagName = rps.GetType().Name;

            XmlNode node = nodes[0];
            WsMethod = result.GetType().Name;
            ReadXML(node, rps, tagName);

            return result;
        }

        private void SignObject(XmlNode nodes, object rps)
        {
            SignatureType sign = new SignatureType();

            //Grupo: Signature->SignedInfo
            sign.SignedInfo = new SignedInfoType();

            sign.SignedInfo.CanonicalizationMethod = new CanonicalizationMethodType();

            // <- Elemento foreach

            sign.SignedInfo.SignatureMethod = new SignatureMethodType();

            // <- Elemento foreach

            // Grupo: Signature->SignedInfo->Reference
            sign.SignedInfo.Reference = new ReferenceType[1];

            ReferenceType referenceType = new ReferenceType();

            // <- Elemento foreach
            referenceType.DigestMethod = new DigestMethodType();

            sign.SignedInfo.Reference[0] = referenceType;

            // Grupo: Signature->SignedInfo->Reference->Transforms
            sign.SignedInfo.Reference[0].Transforms = new TransformType[CountElements(nodes, "Transform")];

            // <- Elemento foreach

            //Tag: Signature->SignatureValue
            sign.SignatureValue = new SignatureValueType();

            // <- Elemento foreach

            //Grupo: Signature->KeyInfo
            sign.KeyInfo = new KeyInfoType();
            X509DataType x509 = new X509DataType();
            x509.Items = new object[1];

            // <- Elemento foreach
            x509.ItemsElementName = new ItemsChoiceType1[1] { ItemsChoiceType1.X509Certificate };

            sign.KeyInfo.Items = new object[1];
            sign.KeyInfo.Items[0] = x509;

            sign.KeyInfo.ItemsElementName = new ItemsChoiceType2[1] { ItemsChoiceType2.X509Data };

            PopulateSignature(sign, referenceType, x509, nodes);

            SetProperty(rps, "Signature", sign);
        }

        private void PopulateSignature(SignatureType sign, ReferenceType referenceType, X509DataType x509, XmlNode nodes)
        {
            int transformCount = 0;

            foreach (XmlNode item in nodes.ChildNodes)
            {
                if (item.Name.Equals("CanonicalizationMethod"))
                    sign.SignedInfo.CanonicalizationMethod.Algorithm = item.Attributes["Algorithm"].Value;

                if (item.Name.Equals("SignatureMethod"))
                    sign.SignedInfo.SignatureMethod.Algorithm = item.Attributes["Algorithm"].Value;

                if (item.Name.Equals("Reference"))
                    referenceType.URI = item.Attributes["URI"].Value;

                if (item.Name.Equals("Transform"))
                {
                    TransformType transformType = new TransformType();
                    transformType.Algorithm = item.Attributes["Algorithm"].Value;
                    sign.SignedInfo.Reference[0].Transforms[transformCount] = transformType;
                    transformCount += 1;
                }

                if (item.Name.Equals("DigestMethod"))
                    referenceType.DigestMethod.Algorithm = item.Attributes["Algorithm"].Value;

                if (item.Name.Equals("DigestValue"))
                    referenceType.DigestValue = GetBytes(item.InnerText);

                if (item.Name.Equals("SignatureValue"))
                    sign.SignatureValue.Value = GetBytes(item.InnerText);

                if (item.Name.Equals("X509Certificate"))
                    x509.Items[0] = GetBytes(item.InnerText);

                if (item.HasChildNodes)
                {
                    PopulateSignature(sign, referenceType, x509, item);
                }
            }
        }

        private int CountElements(XmlNode node, string tag)
        {
            int result = 0;
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.HasChildNodes)
                    result += CountElements(item, tag);
                else
                    if (item.Name.Equals(tag))
                    result++;
            }
            return result;
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
                if (n.Name == "Signature")
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        SignObject(n, value);
                    else
                        PSignObject(n, value);

                    continue;
                }

                if (n.HasChildNodes && n.FirstChild.NodeType == XmlNodeType.Element)
                {
                    string component = (tpAmb == TipoAmbiente.taHomologacao ? "NFe.Components.br.gov.egoverne.isscuritiba.curitiba.h." : "NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.");

                    Object instance =
                    System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(
                        component + this.GetNameObject(n.Name, WsMethod),
                        false,
                        BindingFlags.Default,
                        null,
                        new object[] { },
                        null,
                        null
                    );

                    if (tpAmb == TipoAmbiente.taHomologacao)
                        SetProperty(value, n.Name, ReadXML(n, instance, n.Name));
                    else
                        PSetProperty(value, n.Name, ReadXML(n, instance, n.Name));
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

            /// Como tem duas propriedades eu tenho que setar os valores da propriedades do RPS a assinatura e feita pelo metodo especifico
            if (propertyName.Equals("Rps"))
            {
                SetProperty(result, "InfRps", ((tcRps)value).InfRps);
                SetProperty(result, "Signature", ((tcRps)value).Signature);
            }
            else
                if (pi != null)
            {
                if (!String.IsNullOrEmpty(value.ToString()))
                {
                    Type t = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;

                    /// Melhorar a conversao
                    if (t.Name.Equals("Rps[]"))
                    {
                        Rps[] obj = new Rps[1];
                        obj[0] = ((Rps)value);
                        pi.SetValue(result, obj, null);
                    }
                    else
                    {
                        object safeValue = (value == null) ? null : Convert.ChangeType(value, t);
                        pi.SetValue(result, safeValue, null);
                    }
                }
            }
        }

        private string GetNameObject(string tag, string wsMethod)
        {
            string nameObject = "";

            switch (tag)
            {
                case "":
                    nameObject = "unknow";
                    break;

                case "ListaRps":
                    nameObject = "Rps";
                    break;

                case "Servico":
                    nameObject = "tcDadosServico";
                    break;

                case "Prestador":
                    nameObject = "tcIdentificacaoPrestador";
                    break;

                case "Tomador":
                    if (wsMethod.Equals("ConsultarNfseEnvio"))
                        nameObject = "tcIdentificacaoTomador";
                    else
                        nameObject = "tcDadosTomador";
                    break;

                case "Pedido":
                    nameObject = "tcPedidoCancelamento";
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

        private void PSetProperty(object result, string propertyName, object value)
        {
            PropertyInfo pi = result.GetType().GetProperty(propertyName);

            /// Como tem duas propriedades eu tenho que setar os valores da propriedades do RPS a assinatura e feita pelo metodo especifico
            if (propertyName.Equals("Rps"))
            {
                SetProperty(result, "InfRps", ((NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.tcRps)value).InfRps);
                SetProperty(result, "Signature", ((NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.tcRps)value).Signature);
            }
            else
                if (pi != null)
            {
                if (!String.IsNullOrEmpty(value.ToString()))
                {
                    Type t = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;

                    /// Melhorar a conversao
                    if (t.Name.Equals("Rps[]"))
                    {
                        NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.Rps[] obj = new NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.Rps[1];
                        obj[0] = ((NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.Rps)value);
                        pi.SetValue(result, obj, null);
                    }
                    else
                    {
                        object safeValue = (value == null) ? null : Convert.ChangeType(value, t);
                        pi.SetValue(result, safeValue, null);
                    }
                }
            }
        }

        private void PSignObject(XmlNode nodes, object rps)
        {
            NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.SignatureType sign = new NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.SignatureType();

            //Grupo: Signature->SignedInfo
            sign.SignedInfo = new NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.SignedInfoType();

            sign.SignedInfo.CanonicalizationMethod = new NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.CanonicalizationMethodType();

            // <- Elemento foreach

            sign.SignedInfo.SignatureMethod = new NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.SignatureMethodType();

            // <- Elemento foreach

            // Grupo: Signature->SignedInfo->Reference
            sign.SignedInfo.Reference = new NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.ReferenceType[1];

            NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.ReferenceType referenceType = new NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.ReferenceType();

            // <- Elemento foreach
            referenceType.DigestMethod = new NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.DigestMethodType();

            sign.SignedInfo.Reference[0] = referenceType;

            // Grupo: Signature->SignedInfo->Reference->Transforms
            sign.SignedInfo.Reference[0].Transforms = new NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.TransformType[CountElements(nodes, "Transform")];

            // <- Elemento foreach

            //Tag: Signature->SignatureValue
            sign.SignatureValue = new NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.SignatureValueType();

            // <- Elemento foreach

            //Grupo: Signature->KeyInfo
            sign.KeyInfo = new NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.KeyInfoType();
            NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.X509DataType x509 = new NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.X509DataType();
            x509.Items = new object[1];

            // <- Elemento foreach
            x509.ItemsElementName = new NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.ItemsChoiceType[1] { NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.ItemsChoiceType.X509Certificate };

            sign.KeyInfo.Items = new object[1];
            sign.KeyInfo.Items[0] = x509;

            sign.KeyInfo.ItemsElementName = new NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.ItemsChoiceType2[1] { NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.ItemsChoiceType2.X509Data };

            PPopulateSignature(sign, referenceType, x509, nodes);

            SetProperty(rps, "Signature", sign);
        }

        private void PPopulateSignature(NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.SignatureType sign, NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.ReferenceType referenceType, NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.X509DataType x509, XmlNode nodes)
        {
            int transformCount = 0;

            foreach (XmlNode item in nodes.ChildNodes)
            {
                if (item.Name.Equals("CanonicalizationMethod"))
                    sign.SignedInfo.CanonicalizationMethod.Algorithm = item.Attributes["Algorithm"].Value;

                if (item.Name.Equals("SignatureMethod"))
                    sign.SignedInfo.SignatureMethod.Algorithm = item.Attributes["Algorithm"].Value;

                if (item.Name.Equals("Reference"))
                    referenceType.URI = item.Attributes["URI"].Value;

                if (item.Name.Equals("Transform"))
                {
                    NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.TransformType transformType = new NFe.Components.br.gov.egoverne.isscuritiba.curitiba.p.TransformType();
                    transformType.Algorithm = item.Attributes["Algorithm"].Value;
                    sign.SignedInfo.Reference[0].Transforms[transformCount] = transformType;
                    transformCount += 1;
                }

                if (item.Name.Equals("DigestMethod"))
                    referenceType.DigestMethod.Algorithm = item.Attributes["Algorithm"].Value;

                if (item.Name.Equals("DigestValue"))
                    referenceType.DigestValue = GetBytes(item.InnerText);

                if (item.Name.Equals("SignatureValue"))
                    sign.SignatureValue.Value = GetBytes(item.InnerText);

                if (item.Name.Equals("X509Certificate"))
                    x509.Items[0] = GetBytes(item.InnerText);

                if (item.HasChildNodes)
                {
                    PPopulateSignature(sign, referenceType, x509, item);
                }
            }
        }
    }
}