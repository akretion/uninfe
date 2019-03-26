using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Unimake.DFe.Exceptions.Unimake.DFe.Exceptions.CertificadoDigital;

namespace Unimake.DFe.Security
{
    public delegate void CryptographicExceptionHandler(object sender, EventArgs args);

    public enum AlgorithmType
    {
        Sha1,
        Sha256
    }

    public class AssinaturaDigital
    {
        private void Assinar(XmlDocument conteudoXML,
            string tagAssinatura,
            string tagAtributoId,
            X509Certificate2 x509Cert,
            AlgorithmType algorithmType,
            bool definirURI,
            string pinCertificado)
        {
            try
            {
                if (x509Cert == null)
                {
                    throw new ExceptionCertificadoDigital();
                }

                if (conteudoXML.GetElementsByTagName(tagAssinatura).Count == 0)
                {
                    throw new Exception("A tag de assinatura " + tagAssinatura.Trim() + " não existe no XML. (Código do Erro: 5)");
                }
                else if (conteudoXML.GetElementsByTagName(tagAtributoId).Count == 0)
                {
                    throw new Exception("A tag de assinatura " + tagAtributoId.Trim() + " não existe no XML. (Código do Erro: 4)");
                }
                else
                {
                    XmlNodeList lists = conteudoXML.GetElementsByTagName(tagAssinatura);

                    foreach (XmlNode nodes in lists)
                    {
                        foreach (XmlNode childNodes in nodes.ChildNodes)
                        {
                            if (!childNodes.Name.Equals(tagAtributoId))
                            {
                                continue;
                            }

                            // Create a reference to be signed
                            Reference reference = new Reference
                            {
                                Uri = ""
                            };

                            // pega o uri que deve ser assinada
                            XmlElement childElemen = (XmlElement)childNodes;

                            if (definirURI)
                            {
                                if (childElemen.GetAttributeNode("Id") != null)
                                {
                                    reference.Uri = "#" + childElemen.GetAttributeNode("Id").Value;
                                }
                                else if (childElemen.GetAttributeNode("id") != null)
                                {
                                    reference.Uri = "#" + childElemen.GetAttributeNode("id").Value;
                                }
                            }

                            // Create a SignedXml object.
                            SignedXml signedXml = new SignedXml(conteudoXML);

                            //Para certificado A3, vamos carregar o PIN para não ficar solicitando.
                            if (!string.IsNullOrWhiteSpace(pinCertificado))
                            {
                                x509Cert.SetPinPrivateKey(pinCertificado);
                            }

                            if (algorithmType.Equals(AlgorithmType.Sha256))
                            {
                                signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
                                signedXml.SigningKey = x509Cert.GetRSAPrivateKey();
                            }

                            if (algorithmType.Equals(AlgorithmType.Sha1))
                            {
                                signedXml.SigningKey = x509Cert.PrivateKey;
                            }

                            // Add an enveloped transformation to the reference.
                            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
                            reference.AddTransform(new XmlDsigC14NTransform());

                            // Add the reference to the SignedXml object.
                            signedXml.AddReference(reference);

                            if (algorithmType.Equals(AlgorithmType.Sha256))
                            {
                                reference.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
                            }

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
                            nodes.AppendChild(conteudoXML.ImportNode(xmlDigitalSignature, true));
                        }
                    }
                }
            }
            catch (CryptographicException ex)
            {
                if (x509Cert.IsA3())
                {
                    throw new Exception("O certificado deverá ser reiniciado.\r\n Retire o certificado.\r\nAguarde o LED terminar de piscar.\r\n Recoloque o certificado e informe o PIN novamente.\r\n" + ex.ToString());// #12342 concatenar com a mensagem original
                }
                else
                {
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
