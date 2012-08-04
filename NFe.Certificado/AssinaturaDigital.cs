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

namespace NFe.Certificado
{
    public class AssinaturaDigital
    {
        /// <summary>
        /// O método assina digitalmente o arquivo XML passado por parâmetro e 
        /// grava o XML assinado com o mesmo nome, sobreponto o XML informado por parâmetro.
        /// Disponibiliza também uma propriedade com uma string do xml assinado (this.vXmlStringAssinado)
        /// </summary>
        /// <param name="arqXMLAssinar">Nome do arquivo XML a ser assinado</param>
        /// <param name="tagAssinatura">Nome da tag onde é para ficar a assinatura</param>
        /// <param name="tagAtributoId">Nome da tag que tem o atributo ID, tag que vai ser assinada</param>
        /// <param name="x509Cert">Certificado a ser utilizado na assinatura</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 04/06/2008
        /// </remarks>
        private void Assinar(string arqXMLAssinar, string tagAssinatura, string tagAtributoId, X509Certificate2 x509Cert)
        {
            if (String.IsNullOrEmpty(tagAssinatura))
                return;

            StreamReader SR = null;

            try
            {
                //Abrir o arquivo XML a ser assinado e ler o seu conteúdo
                SR = File.OpenText(arqXMLAssinar);
                string xmlString = SR.ReadToEnd();
                SR.Close();
                SR = null;

                try
                {
                    // Create a new XML document.
                    XmlDocument doc = new XmlDocument();

                    // Format the document to ignore white spaces.
                    doc.PreserveWhitespace = false;

                    // Load the passed XML file using it’s name.
                    try
                    {
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
                            try
                            {
                                XmlDocument XMLDoc;

                                XmlNodeList lists = doc.GetElementsByTagName(tagAssinatura);
                                foreach (XmlNode nodes in lists)
                                {
                                    foreach (XmlNode childNodes in nodes.ChildNodes)
                                    {
                                        if (!childNodes.Name.Equals(tagAtributoId))
                                            continue;

                                        if (childNodes.NextSibling != null && childNodes.NextSibling.Name.Equals("Signature"))
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
                                        /*
                                        XmlAttributeCollection _Uri = childElemen.GetElementsByTagName(tagAtributoId).Item(0).Attributes;

                                        if (_Uri.Count > 0)
                                            foreach (XmlAttribute _atributo in _Uri)
                                            {
                                                if (_atributo.Name == "Id" || _atributo.Name == "id")
                                                {
                                                    reference.Uri = "#" + _atributo.InnerText;
                                                }
                                            }
                                        */

                                        // Create a SignedXml object.
                                        SignedXml signedXml = new SignedXml(doc);

                                        // Add the key to the SignedXml document
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

                                        // Gravar o elemento no documento XML
                                        nodes.AppendChild(doc.ImportNode(xmlDigitalSignature, true));
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
                            catch (Exception caught)
                            {
                                throw (caught);
                            }
                        }
                    }
                    catch (Exception caught)
                    {
                        throw (caught);
                    }
                }
                catch (Exception caught)
                {
                    throw (caught);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (SR != null)
                    SR.Close();
            }
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
            try
            {
                TipoArquivoXML v = new TipoArquivoXML(arqXMLAssinar, UFCod);

                if (!String.IsNullOrEmpty(v.TagAssinatura))
                    this.Assinar(arqXMLAssinar, v.TagAssinatura, v.TagAtributoId, Empresa.Configuracoes[emp].X509Certificado);

                //Assinar o lote
                if (!String.IsNullOrEmpty(v.TagLoteAssinatura))
                    this.Assinar(arqXMLAssinar, v.TagLoteAssinatura, v.TagLoteAtributoId, Empresa.Configuracoes[emp].X509Certificado);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
