﻿using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Unimake.Security.Exceptions;

namespace Unimake.Business.DFe.Security
{
    public delegate void CryptographicExceptionHandler(object sender, EventArgs args);

    public enum AlgorithmType
    {
        Sha1,
        Sha256
    }

    /// <summary>
    /// Classe para fazer assinatura digital de XMLs
    /// </summary>
    public class AssinaturaDigital
    {
        /// <summary>
        /// Assinar digitalmente o XML
        /// </summary>
        /// <param name="conteudoXML">XML a ser assinado</param>
        /// <param name="tagAssinatura">Nome da tag a ser assinada</param>
        /// <param name="tagAtributoId">Nome da tag que possui o ID para referencia na URI da assinatura</param>
        /// <param name="x509Cert">Certificado digital a ser utilizado na assinatura</param>
        /// <param name="algorithmType">Tipo de algorítimo a ser utilizado na assinatura</param>
        /// <param name="definirURI">Define o Reference.URI na assinatura</param>
        /// <param name="pinCertificado">PIN do certificado digital, quando do tipo A3</param>
        /// <param name="idAttributeName">Nome do atributo que tem o ID para assinatura. Se nada for passado o sistema vai tentar buscar o nome Id ou id, se não encontrar, não vai criar a URI Reference na assinatura com ID.</param>
        public void Assinar(XmlDocument conteudoXML,
            string tagAssinatura,
            string tagAtributoId,
            X509Certificate2 x509Cert,
            AlgorithmType algorithmType = AlgorithmType.Sha1,
            bool definirURI = true,
            string pinCertificado = "",
            string idAttributeName = "")
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
                                if (string.IsNullOrEmpty(idAttributeName))
                                {
                                    if (childElemen.GetAttributeNode("Id") != null)
                                    {
                                        idAttributeName = "Id";
                                    }
                                    else if (childElemen.GetAttributeNode("id") != null)
                                    {
                                        idAttributeName = "id";
                                    }
                                }

                                if (!string.IsNullOrEmpty(idAttributeName))
                                {
                                    reference.Uri = "#" + childElemen.GetAttributeNode(idAttributeName).Value;
                                }
                            }

                            SignedXml signedXml = new SignedXml(conteudoXML);

                            if (!string.IsNullOrWhiteSpace(pinCertificado))
                            {
                                x509Cert.SetPinPrivateKey(pinCertificado);
                            }

                            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
                            reference.AddTransform(new XmlDsigC14NTransform());

                            switch (algorithmType)
                            {
                                case AlgorithmType.Sha256:
                                    signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
                                    signedXml.SigningKey = x509Cert.GetRSAPrivateKey();
                                    signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
                                    reference.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
                                    break;

                                default:
                                    signedXml.SigningKey = x509Cert.PrivateKey;
                                    signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
                                    reference.DigestMethod = "http://www.w3.org/2000/09/xmldsig#sha1";
                                    break;
                            }

                            signedXml.AddReference(reference);

                            KeyInfo keyInfo = new KeyInfo();
                            keyInfo.AddClause(new KeyInfoX509Data(x509Cert));
                            signedXml.KeyInfo = keyInfo;
                            signedXml.ComputeSignature();

                            XmlElement xmlDigitalSignature = signedXml.GetXml();

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
