using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Windows.Forms;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;
using NFe.Settings;
using NFe.Components;
using System.Security.Cryptography;
using System.Collections;
using System.Security;

namespace NFe.Certificado
{
    public delegate void CryptographicExceptionHandler(object sender, EventArgs args);


    public class AssinaturaDigital
    {
        public bool AssinaturaValida { get; private set; }
        public bool TesteCertificado { get; set; }

        /// <summary>
        /// O método assina digitalmente o arquivo XML passado por parâmetro e 
        /// grava o XML assinado com o mesmo nome, sobreponto o XML informado por parâmetro.
        /// Disponibiliza também uma propriedade com uma string do xml assinado (this.vXmlStringAssinado)
        /// </summary>
        /// <param name="arqXMLAssinar">Nome do arquivo XML a ser assinado</param>
        /// <param name="tagAssinatura">Nome da tag onde é para ficar a assinatura</param>
        /// <param name="tagAtributoId">Nome da tag que tem o atributo ID, tag que vai ser assinada</param>
        /// <param name="x509Cert">Certificado a ser utilizado na assinatura</param>
        /// <param name="empresa">Índice da empresa que está solicitando a assinatura</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 04/06/2008
        /// </remarks>
        private void Assinar(string arqXMLAssinar,
            string tagAssinatura,
            string tagAtributoId,
            X509Certificate2 x509Cert,
            int empresa)
        {

            StreamReader SR = null;

            try
            {
                //Abrir o arquivo XML a ser assinado e ler o seu conteúdo
                SR = File.OpenText(arqXMLAssinar);
                string xmlString = SR.ReadToEnd();
                SR.Close();
                SR = null;

                // Create a new XML document.
                XmlDocument doc = new XmlDocument();

                // Format the document to ignore white spaces.
                doc.PreserveWhitespace = false;

                // Load the passed XML file using it’s name.
                doc.LoadXml(xmlString);

                if (doc.GetElementsByTagName(tagAssinatura).Count == 0)
                {
                    throw new Exception("A tag de assinatura " + tagAssinatura.Trim() + " não existe no XML. (Código do Erro: 5)");
                }
                else if (doc.GetElementsByTagName(tagAtributoId).Count == 0)
                {
                    throw new Exception("A tag de assinatura " + tagAtributoId.Trim() + " não existe no XML. (Código do Erro: 4)");
                }
                // Existe mais de uma tag a ser assinada
                else
                {
                    XmlDocument XMLDoc;

                    XmlNodeList lists = doc.GetElementsByTagName(tagAssinatura);
                    XmlNode listRPS = null;

                    /// Esta condição foi feita especificamente para prefeitura de Governador Valadares pois o AtribudoID e o Elemento assinado devem possuir o mesmo nome.
                    /// Talvez tenha que ser reavaliado.
                    #region Governador Valadares
                    if (tagAssinatura.Equals(tagAtributoId) && Empresas.Configuracoes[empresa].UnidadeFederativaCodigo == 3127701)
                    {
                        foreach (XmlNode item in lists)
                        {
                            if (listRPS == null)
                            {
                                listRPS = item;
                            }

                            if (item.Name.Equals(tagAssinatura))
                            {
                                lists = item.ChildNodes;
                                break;
                            }
                        }
                    }
                    #endregion

                    foreach (XmlNode nodes in lists)
                    {
                        foreach (XmlNode childNodes in nodes.ChildNodes)
                        {
                            if (!childNodes.Name.Equals(tagAtributoId))
                                continue;

                            // Create a reference to be signed
                            Reference reference = new Reference();
                            reference.Uri = "";

                            // pega o uri que deve ser assinada                                       
                            XmlElement childElemen = (XmlElement)childNodes;
                            if (childElemen.GetAttributeNode("Id") != null)
                            {
                                reference.Uri = "#" + childElemen.GetAttributeNode("Id").Value;
                            }
                            else if (childElemen.GetAttributeNode("id") != null)
                            {
                                reference.Uri = "#" + childElemen.GetAttributeNode("id").Value;
                            }

                            // Create a SignedXml object.
                            SignedXml signedXml = new SignedXml(doc);

                            //A3
                            if (!String.IsNullOrEmpty(Empresas.Configuracoes[empresa].CertificadoPIN) && clsX509Certificate2Extension.IsA3(x509Cert))
                            {
                                signedXml.SigningKey = LerDispositivo(Empresas.Configuracoes[empresa].CertificadoPIN,
                                                                      Convert.ToInt32("0" + Empresas.Configuracoes[empresa].ProviderTypeCertificado),
                                                                      Empresas.Configuracoes[empresa].ProviderCertificado);
                            }
                            else
                                signedXml.SigningKey = x509Cert.PrivateKey;

                            // Add an enveloped transformation to the reference.
                            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                            reference.AddTransform(env);

                            XmlDsigC14NTransform c14 = new XmlDsigC14NTransform();
                            reference.AddTransform(c14);

                            // Add the reference to the SignedXml object.
                            signedXml.AddReference(reference);

                            // Create a new KeyInfo object
                            KeyInfo keyInfo = new KeyInfo();

                            // Load the certificate into a KeyInfoX509Data object
                            // and add it to the KeyInfo object.
                            keyInfo.AddClause(new KeyInfoX509Data(x509Cert));

                            // Add the KeyInfo object to the SignedXml object.
                            signedXml.KeyInfo = keyInfo;
                            signedXml.ComputeSignature();

                            // Get the XML representation of the signature and save
                            // it to an XmlElement object.
                            XmlElement xmlDigitalSignature = signedXml.GetXml();

                            if (tagAssinatura.Equals(tagAtributoId) && Empresas.Configuracoes[empresa].UnidadeFederativaCodigo == 3127701)
                            {
                                ///Desenvolvido especificamente para prefeitura de governador valadares
                                listRPS.AppendChild(doc.ImportNode(xmlDigitalSignature, true));
                            }
                            else
                            {
                                // Gravar o elemento no documento XML
                                nodes.AppendChild(doc.ImportNode(xmlDigitalSignature, true));
                            }
                        }
                    }

                    XMLDoc = new XmlDocument();
                    XMLDoc.PreserveWhitespace = false;
                    XMLDoc = doc;

                    // Atualizar a string do XML já assinada
                    string StringXMLAssinado = XMLDoc.OuterXml;

                    // Gravar o XML Assinado no HD
                    StreamWriter SW_2 = File.CreateText(arqXMLAssinar);
                    SW_2.Write(StringXMLAssinado);
                    SW_2.Close();
                }
            }
            catch (System.Security.Cryptography.CryptographicException ex)
            {
                #region #10316
                /*
                 * Solução para o problema do certificado do tipo A3
                 * Marcelo
                 * 29/07/2013
                 */

                AssinaturaValida = false;
#if DEBUG
                Debug.WriteLine("O erro CryptographicException foi lançado");
#endif
                x509Cert = Empresas.ResetCertificado(empresa);
                if (!TesteCertificado)
                    throw new Exception("O certificado deverá ser reiniciado.\r\n Retire o certificado.\r\nAguarde o LED terminar de piscar.\r\n Recoloque o certificado e informe o PIN novamente.\r\n" + ex.ToString());// #12342 concatenar com a mensagem original
                #endregion
            }
            catch
            {
                throw;
            }
            finally
            {
                if (SR != null)
                    SR.Close();
            }
        }

        public RSACryptoServiceProvider LerDispositivo(string PIN, int providerType, string provider)
        {
            CspParameters csp = new CspParameters(providerType, provider);
            
            SecureString ss = new SecureString();
            char[] PINs = PIN.ToCharArray();
            foreach (char a in PINs)
            {
                ss.AppendChar(a);
            }
            csp.KeyPassword = ss;
            csp.KeyNumber = 1;
            csp.Flags = CspProviderFlags.NoPrompt;
            csp.KeyContainerName = "SecurityBin2";
            
            // Initialize an RSACryptoServiceProvider object using
            // the CspParameters object.
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);
            //rsa.PersistKeyInCsp = false; //importante, senão ele lota o certificado de chaves!
            rsa.PersistKeyInCsp = false;
            rsa.ToXmlString(false);            
            return rsa;
        }
    
        /// <summary>
        /// Assina o XML sobrepondo-o
        /// </summary>
        /// <param name="arqXMLAssinar">Nome do arquivo XML a ser assinado</param>
        /// <param name="x509Certificado">Certificado a ser utilizado na assinatura</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>16/04/2009</date>
        public void Assinar(string arqXMLAssinar, int emp, int UFCod)
        {
            if (Empresas.Configuracoes[emp].UsaCertificado)
            {
                TipoArquivoXML v = new TipoArquivoXML(arqXMLAssinar, UFCod);

                if (!String.IsNullOrEmpty(v.TagAssinatura))
                {
                    if (!Assinado(arqXMLAssinar, v.TagAssinatura))
                        this.Assinar(arqXMLAssinar, v.TagAssinatura, v.TagAtributoId, Empresas.Configuracoes[emp].X509Certificado, emp);
                }

                //Assinar o lote
                if (!String.IsNullOrEmpty(v.TagLoteAssinatura))
                    if (!Assinado(arqXMLAssinar, v.TagLoteAssinatura))
                        this.Assinar(arqXMLAssinar, v.TagLoteAssinatura, v.TagLoteAtributoId, Empresas.Configuracoes[emp].X509Certificado, emp);
            }
        }

        public void CarregarPIN(int emp, string arqXML, Servicos servico)
        {
            X509Certificate2 x509Cert = new X509Certificate2(Empresas.Configuracoes[emp].X509Certificado);
            if (Empresas.Configuracoes[emp].UsaCertificado && clsX509Certificate2Extension.IsA3(x509Cert))
            {
                string tempFile = "";

                switch (servico)
                {
                    case Servicos.ConsultaCadastroContribuinte:
                        tempFile = Functions.ExtraiPastaNomeArq(arqXML, Propriedade.ExtEnvio.ConsCad_XML) + "__" + Propriedade.ExtEnvio.ConsCad_XML;
                        File.Copy(arqXML, tempFile);
                        Assinar(tempFile, "ConsCad", "infCons", x509Cert, emp);
                        break;

                    case Servicos.NFeConsultaStatusServico:
                        tempFile = Functions.ExtraiPastaNomeArq(arqXML, Propriedade.ExtEnvio.PedSta_XML) + "__" + Propriedade.ExtEnvio.PedSta_XML;
                        File.Copy(arqXML, tempFile);
                        Assinar(tempFile, "consStatServ", "xServ", x509Cert, emp);
                        break;

                    case Servicos.NFePedidoConsultaSituacao:
                        tempFile = Functions.ExtraiPastaNomeArq(arqXML, Propriedade.ExtEnvio.PedSta_XML) + "__" + Propriedade.ExtEnvio.PedSta_XML;
                        File.Copy(arqXML, tempFile);
                        Assinar(tempFile, "consSitNFe", "xServ", x509Cert, emp);
                        break;

                    default:
                        break;
                }
                if (tempFile != "" && File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        public bool TestarProviderCertificado(string tempFile,
            string tagAssinatura,
            string tagAtributo,
            X509Certificate2 certificado,
            int codEmp,
            string pin,
            string provider,
            string type)
        {
            string _pin = Empresas.Configuracoes[codEmp].CertificadoPIN;
            string _provider = Empresas.Configuracoes[codEmp].ProviderCertificado;
            string _type = Empresas.Configuracoes[codEmp].ProviderTypeCertificado;

            Empresas.Configuracoes[codEmp].CertificadoPIN = pin;
            Empresas.Configuracoes[codEmp].ProviderTypeCertificado = type;
            Empresas.Configuracoes[codEmp].ProviderCertificado = provider;
            AssinaturaValida = true;
            try
            {
                Assinar(tempFile, tagAssinatura, tagAtributo, certificado, codEmp);
            }
            finally
            {
                Empresas.Configuracoes[codEmp].CertificadoPIN = _pin;
                Empresas.Configuracoes[codEmp].ProviderTypeCertificado = _type;
                Empresas.Configuracoes[codEmp].ProviderCertificado = _provider;
            }
            return AssinaturaValida;
        }

        /// <summary>
        /// Verificar se o XML já tem assinatura
        /// </summary>
        /// <param name="arqXML">Arquivo XML a ser verificado se tem assinatura</param>
        /// <param name="tagAssinatura">Tag de assinatura onde vamos pesquisar</param>
        /// <returns>true = Já está assinado</returns>
        private bool Assinado(string arqXML, string tagAssinatura)
        {
            bool retorno = false;
            try
            {

                XmlDocument doc = new XmlDocument();
                doc.Load(arqXML);

                if (doc.GetElementsByTagName(tagAssinatura)[0].LastChild.Name == "Signature")
                    retorno = true;
            }
            catch { }

            return retorno;
        }
    }
}

