using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Windows.Forms;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;

namespace uninfe
{
    public class UniAssinaturaDigitalClass
    {
        public int vResultado { get; private set; }
        public string vResultadoString { get; private set; }
        public string vXMLStringAssinado { get; private set; }
        private XmlDocument XMLDoc;

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 04/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: O método assina digitalmente o arquivo XML passado por 
         *              parâmetro
         *              O método grava o XML assinado com o mesmo nome, sobreponto o 
         *              XML informado por parâmetro
         *              O método também disponibiliza uma propriedade com uma string
         *              do xml assinado (this.vXmlStringAssinado)
         *              
         * ------------------------------------------------------------------------------
         * Definição..: Assinar( string, string, X509Certificate2 )
         * Parâmetros.: pArqXMLAssinar - Nome do arquivo XML a ser assinado
         *              pUri           - URI (TAG) a ser assinada
         *              pCertificado   - Certificado a ser utilizado na assinatura
         *
         * ------------------------------------------------------------------------------
         * Retorno....: - Atualiza a propriedade this.vXMLStringAssinado com a string de
         *                xml já assinada
         *              - Grava o XML sobreponto o informado para o método com o conteúdo
         *                já assinado
         *                
         *              - Atualiza as propriedades this.vResultado e 
         *                this.vResultadoString com os seguintes valores:
         *                
         *                0 - Assinatura realizada com sucesso
         *                1 - Erro: Problema ao acessar o certificado digital - %exceção%
         *                2 - Problemas no certificado digital
         *                3 - XML mal formado + %exceção%
         *                4 - A tag de assinatura %pUri% não existe 
         *                5 - A tag de assinatura %pUri% não é unica
         *                6 - Erro ao assinar o documento - %exceção%
         *              
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * ------------------------------------------------------------------------------
         * Notas......: 
         *              
         * ==============================================================================         
         */
        public void Assinar(string pArqXMLAssinar, string pUri, X509Certificate2 pCertificado)
        {

            //Abrir o arquivo XML a ser assinado e ler o seu conteúdo
            StreamReader SR = File.OpenText(pArqXMLAssinar);
            string vXMLString = SR.ReadToEnd();
            SR.Close();

            //Atualizar atributos de retorno com conteúdo padrão
            this.vResultado = 0;
            this.vResultadoString = "Assinatura realizada com sucesso";

            try
            {
                // Verifica o certificado a ser utilizado na assinatura
                string _xnome = "";
                if (pCertificado != null)
                {
                    _xnome = pCertificado.Subject.ToString();
                }

                X509Certificate2 _X509Cert = new X509Certificate2();
                X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
                X509Certificate2Collection collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindBySubjectDistinguishedName, _xnome, false);

                if (collection1.Count == 0)
                {
                    this.vResultado = 2;
                    this.vResultadoString = "Problemas no certificado digital";
                }
                else
                {
                    // certificado ok
                    _X509Cert = collection1[0];
                    string x;
                    x = _X509Cert.GetKeyAlgorithm().ToString();
                    
                    // Create a new XML document.
                    XmlDocument doc = new XmlDocument();

                    // Format the document to ignore white spaces.
                    doc.PreserveWhitespace = false;

                    // Load the passed XML file using it’s name.
                    try
                    {
                        doc.LoadXml(vXMLString);

                        // Verifica se a tag a ser assinada existe é única
                        int qtdeRefUri = doc.GetElementsByTagName(pUri).Count;

                        if (qtdeRefUri == 0)
                        {
                            // a URI indicada não existe
                            this.vResultado = 4;
                            this.vResultadoString = "A tag de assinatura " + pUri.Trim() + " não existe";
                        }
                        // Exsiste mais de uma tag a ser assinada
                        else
                        {
                            if (qtdeRefUri > 1)
                            {
                                // existe mais de uma URI indicada
                                this.vResultado = 5;
                                this.vResultadoString = "A tag de assinatura " + pUri.Trim() + " não é unica";
                            }
                            else
                            {
                                try
                                {
                                    // Create a SignedXml object.
                                    SignedXml signedXml = new SignedXml(doc);

                                    // Add the key to the SignedXml document
                                    signedXml.SigningKey = _X509Cert.PrivateKey;

                                    // Create a reference to be signed
                                    Reference reference = new Reference();

                                    // pega o uri que deve ser assinada
                                    XmlAttributeCollection _Uri = doc.GetElementsByTagName(pUri).Item(0).Attributes;
                                    foreach (XmlAttribute _atributo in _Uri)
                                    {
                                        if (_atributo.Name == "Id")
                                        {
                                            reference.Uri = "#" + _atributo.InnerText;
                                        }
                                    }

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
                                    keyInfo.AddClause(new KeyInfoX509Data(_X509Cert));

                                    // Add the KeyInfo object to the SignedXml object.
                                    signedXml.KeyInfo = keyInfo;
                                    signedXml.ComputeSignature();

                                    // Get the XML representation of the signature and save
                                    // it to an XmlElement object.
                                    XmlElement xmlDigitalSignature = signedXml.GetXml();

                                    // Gravar o elemento no documento XML
                                    doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));
                                    XMLDoc = new XmlDocument();
                                    XMLDoc.PreserveWhitespace = false;
                                    XMLDoc = doc;

                                    // Atualizar a string do XML já assinada
                                    this.vXMLStringAssinado = XMLDoc.OuterXml;

                                    // Gravar o XML no HD
                                    StreamWriter SW_2 = File.CreateText(pArqXMLAssinar);
                                    SW_2.Write(this.vXMLStringAssinado);
                                    SW_2.Close();
                                }
                                catch (Exception caught)
                                {
                                    this.vResultado = 6;
                                    this.vResultadoString = "Erro ao assinar o documento - " + caught.Message;
                                }
                            }
                        }
                    }
                    catch (Exception caught)
                    {
                        this.vResultado = 3;
                        this.vResultadoString = "XML mal formado - " + caught.Message;
                    }
                }
            }
            catch (Exception caught)
            {
                this.vResultado = 1;
                this.vResultadoString = "Problema ao acessar o certificado digital" + caught.Message;
            }
        }
    }
}
