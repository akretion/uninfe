using NFe.Components;
using NFe.Exceptions;
using NFe.Settings;
using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Unimake.Business.DFe.Security;

namespace NFe.Certificado
{
    public delegate void CryptographicExceptionHandler(object sender, EventArgs args);

    public enum AlgorithmType
    {
        Sha1,
        Sha256
    }

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
        public void Assinar(string arqXMLAssinar,
            string tagAssinatura,
            string tagAtributoId,
            X509Certificate2 x509Cert,
            int empresa,
            AlgorithmType algorithmType = AlgorithmType.Sha1, bool comURI = true) => Assinar(new XmlDocument(), arqXMLAssinar, tagAssinatura, tagAtributoId, x509Cert, empresa, algorithmType, comURI);

        private void Assinar(XmlDocument conteudoXML,
            string arqXMLAssinar,
            string tagAssinatura,
            string tagAtributoId,
            X509Certificate2 x509Cert,
            int empresa,
            AlgorithmType algorithmType = AlgorithmType.Sha1, bool comURI = true)
        {
            conteudoXML.PreserveWhitespace = true;

            if(string.IsNullOrEmpty(conteudoXML.InnerText))
            {
                conteudoXML = new XmlDocument();

                try
                {
                    conteudoXML.Load(arqXMLAssinar);
                }
                catch
                {
                    conteudoXML.LoadXml(File.ReadAllText(arqXMLAssinar, System.Text.Encoding.UTF8));
                }
            }

            Assinar(conteudoXML, tagAssinatura, tagAtributoId, x509Cert, empresa, algorithmType, comURI);

            try
            {
                // Atualizar a string do XML já assinada
                var StringXMLAssinado = conteudoXML.OuterXml;

                // Gravar o XML Assinado no HD
                var SW_2 = File.CreateText(arqXMLAssinar);
                SW_2.Write(StringXMLAssinado);
                SW_2.Close();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// O método assina digitalmente o arquivo XML passado por parâmetro e
        /// grava o XML assinado com o mesmo nome, sobreponto o XML informado por parâmetro.
        /// Disponibiliza também uma propriedade com uma string do xml assinado (this.vXmlStringAssinado)
        /// </summary>
        /// <param name="conteudoXML">Conteúdo do XML</param>
        /// <param name="tagAssinatura">Nome da tag onde é para ficar a assinatura</param>
        /// <param name="tagAtributoId">Nome da tag que tem o atributo ID, tag que vai ser assinada</param>
        /// <param name="x509Cert">Certificado a ser utilizado na assinatura</param>
        /// <param name="empresa">Índice da empresa que está solicitando a assinatura</param>
        /// <param name="algorithmType">Tipo de algoritimo para assinatura do XML.</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 04/06/2008
        /// </remarks>
        private void Assinar(XmlDocument conteudoXML,
            string tagAssinatura,
            string tagAtributoId,
            X509Certificate2 x509Cert,
            int empresa,
            AlgorithmType algorithmType,
            bool comURI)
        {
            #region São José dos Pinhais


            if((Empresas.Configuracoes[empresa].UnidadeFederativaCodigo == 4125506 ||
                Empresas.Configuracoes[empresa].UnidadeFederativaCodigo == 4303103 ||
                Empresas.Configuracoes[empresa].UnidadeFederativaCodigo == 4104808 ||
                Empresas.Configuracoes[empresa].UnidadeFederativaCodigo == 3523503 ||
                Empresas.Configuracoes[empresa].UnidadeFederativaCodigo == 4215000) &&
                tagAssinatura.Equals(tagAtributoId))
            {
                AssinarJSP(conteudoXML, tagAssinatura, tagAtributoId, x509Cert, empresa, algorithmType, comURI);

                return;
            }

            #endregion

            try
            {
                if(x509Cert == null)
                {
                    throw new ExceptionCertificadoDigital(ErroPadrao.CertificadoNaoEncontrado);
                }

                if(conteudoXML.GetElementsByTagName(tagAssinatura).Count == 0)
                {
                    throw new Exception("A tag de assinatura " + tagAssinatura.Trim() + " não existe no XML. (Código do Erro: 5)");
                }
                else if(conteudoXML.GetElementsByTagName(tagAtributoId).Count == 0)
                {
                    throw new Exception("A tag de assinatura " + tagAtributoId.Trim() + " não existe no XML. (Código do Erro: 4)");
                }
                // Existe mais de uma tag a ser assinada
                else
                {
                    var lists = conteudoXML.GetElementsByTagName(tagAssinatura);
                    XmlNode listRPS = null;


                    /// Esta condição foi feita especificamente para prefeitura de Governador Valadares pois o AtribudoID e o Elemento assinado devem possuir o mesmo nome.
                    /// Talvez tenha que ser reavaliado.
                    #region Governador Valadares

                    if(tagAssinatura.Equals(tagAtributoId) && (Empresas.Configuracoes[empresa].UnidadeFederativaCodigo == 3127701))
                    {
                        foreach(XmlNode item in lists)
                        {
                            if(listRPS == null)
                            {
                                listRPS = item;
                            }

                            if(item.Name.Equals(tagAssinatura))
                            {
                                lists = item.ChildNodes;
                                break;
                            }
                        }
                    }

                    #endregion Governador Valadares

                    foreach(XmlNode nodes in lists)
                    {
                        foreach(XmlNode childNodes in nodes.ChildNodes)
                        {
                            if(!childNodes.Name.Equals(tagAtributoId))
                            {
                                continue;
                            }

                            // Create a reference to be signed
                            var reference = new Reference
                            {
                                Uri = ""
                            };

                            // pega o uri que deve ser assinada
                            var childElemen = (XmlElement)childNodes;

                            if(comURI)
                            {
                                if(childElemen.GetAttributeNode("Id") != null)
                                {
                                    reference.Uri = "#" + childElemen.GetAttributeNode("Id").Value;
                                }
                                else if(childElemen.GetAttributeNode("id") != null)
                                {
                                    reference.Uri = "#" + childElemen.GetAttributeNode("id").Value;
                                }
                            }

                            // Create a SignedXml object.
                            var signedXml = new SignedXml(conteudoXML);

                            if(algorithmType.Equals(AlgorithmType.Sha256))
                            {
                                signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
                                signedXml.SigningKey = x509Cert.GetRSAPrivateKey();
                            }

                            if(algorithmType.Equals(AlgorithmType.Sha1))
                            {
                                signedXml.SigningKey = x509Cert.PrivateKey;
                                signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
                                reference.DigestMethod = "http://www.w3.org/2000/09/xmldsig#sha1";
                            }

                            // Add an enveloped transformation to the reference.
                            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
                            reference.AddTransform(new XmlDsigC14NTransform());

                            // Add the reference to the SignedXml object.
                            signedXml.AddReference(reference);

                            if(algorithmType.Equals(AlgorithmType.Sha256))
                            {
                                reference.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
                            }

                            // Create a new KeyInfo object
                            var keyInfo = new KeyInfo();

                            // Load the certificate into a KeyInfoX509Data object
                            // and add it to the KeyInfo object.
                            keyInfo.AddClause(new KeyInfoX509Data(x509Cert));

                            // Add the KeyInfo object to the SignedXml object.
                            signedXml.KeyInfo = keyInfo;
                            signedXml.ComputeSignature();

                            // Get the XML representation of the signature and save
                            // it to an XmlElement object.
                            var xmlDigitalSignature = signedXml.GetXml();

                            if(tagAssinatura.Equals(tagAtributoId) && Empresas.Configuracoes[empresa].UnidadeFederativaCodigo == 3127701)
                            {
                                ///Desenvolvido especificamente para prefeitura de governador valadares
                                listRPS.AppendChild(conteudoXML.ImportNode(xmlDigitalSignature, true));
                            }
                            else
                            {
                                // Gravar o elemento no documento XML
                                nodes.AppendChild(conteudoXML.ImportNode(xmlDigitalSignature, true));
                            }
                        }
                    }
                }
            }
            catch(CryptographicException ex)
            {
                #region #10316

                /*
                 * Solução para o problema do certificado do tipo A3
                 * Marcelo
                 * 29/07/2013
                 */

                AssinaturaValida = false;

                if(x509Cert.IsA3())
                {
                    x509Cert = Empresas.ResetCertificado(empresa);
                    if(!TesteCertificado)
                    {
                        throw new Exception("O certificado deverá ser reiniciado.\r\n Retire o certificado.\r\nAguarde o LED terminar de piscar.\r\n Recoloque o certificado e informe o PIN novamente.\r\n" + ex.ToString());// #12342 concatenar com a mensagem original
                    }
                }
                else
                {
                    throw;
                }

                #endregion #10316
            }
            catch
            {
                throw;
            }
        }

        public RSACryptoServiceProvider LerDispositivo(string PIN, int providerType, string provider)
        {
            var csp = new CspParameters(providerType, provider);

            var ss = new SecureString();
            var PINs = PIN.ToCharArray();
            foreach(var a in PINs)
            {
                ss.AppendChar(a);
            }
            csp.KeyPassword = ss;
            csp.KeyNumber = 1;
            csp.Flags = CspProviderFlags.NoPrompt;
            csp.KeyContainerName = "";

            // Initialize an RSACryptoServiceProvider object using
            // the CspParameters object.
            var rsa = new RSACryptoServiceProvider(csp)
            {
                //rsa.PersistKeyInCsp = false; //importante, senão ele lota o certificado de chaves!
                PersistKeyInCsp = false
            };
            rsa.ToXmlString(false);
            return rsa;
        }

        #region Assinar()

        /// <summary>
        /// Assina o XML sobrepondo-o
        /// </summary>
        /// <param name="arqXMLAssinar">Nome do arquivo XML a ser assinado</param>
        /// <param name="emp">Código da empresa</param>
        /// <param name="UFCod">Codigo da UF</param>
        /// <param name="algorithmType">Tipo de algoritimo para assinatura do XML.</param>
        public void Assinar(string arqXMLAssinar, int emp, int UFCod, AlgorithmType algorithmType = AlgorithmType.Sha1, bool comURI = true) => Assinar(new XmlDocument(), arqXMLAssinar, emp, UFCod, algorithmType, comURI);

        public void Assinar(XmlDocument conteudoXML, string arqXMLAssinar, int emp, int UFCod, AlgorithmType algorithmType = AlgorithmType.Sha1, bool comURI = true)
        {
            if(Empresas.Configuracoes[emp].UsaCertificado)
            {
                var v = new TipoArquivoXML(arqXMLAssinar, UFCod, false);

                if(!string.IsNullOrEmpty(v.TagAssinatura0))
                {
                    if(!Assinado(arqXMLAssinar, v.TagAssinatura0))
                    {
                        Assinar(conteudoXML, arqXMLAssinar, v.TagAssinatura0, v.TagAtributoId0, Empresas.Configuracoes[emp].X509Certificado, emp, algorithmType, comURI);
                    }
                }

                if(!string.IsNullOrEmpty(v.TagAssinatura))
                {
                    if(!Assinado(arqXMLAssinar, v.TagAssinatura))
                    {
                        Assinar(conteudoXML, arqXMLAssinar, v.TagAssinatura, v.TagAtributoId, Empresas.Configuracoes[emp].X509Certificado, emp, algorithmType, comURI);
                    }
                }

                //Assinar o lote
                if(!string.IsNullOrEmpty(v.TagLoteAssinatura))
                {
                    if(!Assinado(arqXMLAssinar, v.TagLoteAssinatura))
                    {
                        Assinar(conteudoXML, arqXMLAssinar, v.TagLoteAssinatura, v.TagLoteAtributoId, Empresas.Configuracoes[emp].X509Certificado, emp, algorithmType, comURI);
                    }
                }
            }
        }

        #endregion Assinar()

        #region Assinar()

        /// <summary>
        /// Assina o conteúdo do XML
        /// </summary>
        /// <param name="conteudoXML">Conteúdo do XML</param>
        /// <param name="emp">Código da empresa</param>
        /// <param name="UFCod">Codigo da UF</param>
        /// <param name="algorithmType">Tipo de algoritimo para assinatura do XML.</param>
        public void Assinar(XmlDocument conteudoXML, int emp, int UFCod, AlgorithmType algorithmType = AlgorithmType.Sha1, bool comURI = true)
        {
            if(Empresas.Configuracoes[emp].UsaCertificado)
            {
                var v = new TipoArquivoXML("", conteudoXML, UFCod, false);

                if(!string.IsNullOrEmpty(v.TagAssinatura))
                {
                    if(!Assinado(conteudoXML, v.TagAssinatura))
                    {
                        Assinar(conteudoXML, v.TagAssinatura, v.TagAtributoId, Empresas.Configuracoes[emp].X509Certificado, emp, algorithmType, comURI);
                    }
                }

                //Assinar o lote
                if(!string.IsNullOrEmpty(v.TagLoteAssinatura))
                {
                    if(!Assinado(conteudoXML, v.TagLoteAssinatura))
                    {
                        Assinar(conteudoXML, v.TagLoteAssinatura, v.TagLoteAtributoId, Empresas.Configuracoes[emp].X509Certificado, emp, algorithmType, comURI);
                    }
                }
            }
        }

        #region Assinar lote de eventos do eSocial

        /// <summary>
        /// Assinar lote de eventos do eSocial
        /// </summary>
        /// <param name="conteudoXML">Conteúdo do XML de lote do eSocial para ser assinado/param>
        /// <param name="emp">Código da empresa das configurações</param>
        public void AssinarLoteESocial(XmlDocument conteudoXML, int emp)
        {
            var envioLoteEventosNodeList = conteudoXML.GetElementsByTagName("envioLoteEventos");

            foreach(XmlNode envioLoteEventosNode in envioLoteEventosNodeList)
            {
                var envioLoteEventosElement = (XmlElement)envioLoteEventosNode;
                var eventosNodeList = conteudoXML.GetElementsByTagName("eventos");

                foreach(XmlNode eventosNode in eventosNodeList)
                {
                    var eventosNodeElement = (XmlElement)eventosNode;
                    var eventoNodeList = conteudoXML.GetElementsByTagName("evento");

                    foreach(XmlNode eventoNode in eventoNodeList)
                    {
                        var eventoElement = (XmlElement)eventoNode;
                        var eSocialNodeList = eventoElement.GetElementsByTagName("eSocial");

                        var xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(eSocialNodeList[0].OuterXml);

                        Assinar(xmlDoc, emp, 991, AlgorithmType.Sha256, false);

                        var newNode = xmlDoc.ChildNodes[0];
                        eventoNode.RemoveChild(eSocialNodeList[0]);
                        eventoNode.AppendChild(conteudoXML.ImportNode(xmlDoc.DocumentElement, true));
                    }
                }
            }
        }

        public void AssinarLoteESocial(string arquivoXML, int emp)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(arquivoXML);

                AssinarLoteESocial(doc, emp);

                // Atualizar a string do XML já assinada
                var StringXMLAssinado = doc.OuterXml;

                // Gravar o XML Assinado no HD
                var SW_2 = File.CreateText(arquivoXML);
                SW_2.Write(StringXMLAssinado);
                SW_2.Close();
            }
            catch
            {
                throw;
            }
        }

        #endregion Assinar lote de eventos do eSocial

        #region Assinar lote de eventos do EFDReinf

        /// <summary>
        /// Assinar lote de eventos do EFD-Reinf
        /// </summary>
        /// <param name="conteudoXML">Conteúdo do XML de lote do EFDReinf para ser assinado</param>
        /// <param name="emp">Código da empresa das configurações</param>
        public void AssinarLoteEFDReinf(XmlDocument conteudoXML, int emp)
        {
            var loteNodeList = conteudoXML.GetElementsByTagName("loteEventos");

            foreach(XmlNode loteEventosNode in loteNodeList)
            {
                var loteEventosElement = (XmlElement)loteEventosNode;
                var eventoNodeList = conteudoXML.GetElementsByTagName("evento");

                foreach(XmlNode eventoNode in eventoNodeList)
                {
                    var eventoElement = (XmlElement)eventoNode;
                    var reinfNodeList = eventoElement.GetElementsByTagName("Reinf");

                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(reinfNodeList[0].OuterXml);

                    Assinar(xmlDoc, emp, 991, AlgorithmType.Sha256, true);

                    var newNode = xmlDoc.ChildNodes[0];
                    eventoNode.RemoveChild(reinfNodeList[0]);
                    eventoNode.AppendChild(conteudoXML.ImportNode(xmlDoc.DocumentElement, true));
                }
            }
        }

        public void AssinarLoteEFDReinf(string arquivoXML, int emp)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(arquivoXML);

                AssinarLoteEFDReinf(doc, emp);

                // Atualizar a string do XML já assinada
                var StringXMLAssinado = doc.OuterXml;

                // Gravar o XML Assinado no HD
                var SW_2 = File.CreateText(arquivoXML);
                SW_2.Write(StringXMLAssinado);
                SW_2.Close();
            }
            catch
            {
                throw;
            }
        }

        #endregion Assinar lote de eventos do EFDReinf

        #endregion Assinar()

        #region AssinarNew

        //**IMPORTANTE**
        //O Método AssinarNew vai substituir de futuro todos os demais, conforme os demais ficarem obsoletos vamos apagando.
        //Isso vai ocorrer pelo fato de estarmos migrando para uso da DLL do UniNFe
        //16/06/2021 - Wandrey

        /// <summary>
        /// Assina o conteúdo do XML com o certificado digital
        /// </summary>
        /// <param name="conteudoXML">XML a ser assinado</param>
        /// <param name="emp">Código da empresa de onde deve buscar informações para assinatura</param>
        /// <param name="UFCodOrMun">Codigo da UF ou município</param>
        /// <param name="algorithmType">Tipo de algorítimo a ser utilizado na assinatura</param>
        /// <param name="definirURI">Define o Reference.URI na assinatura</param>
        /// <param name="idAttributeName">Nome do atributo que tem o ID para assinatura. Se nada for passado o sistema vai tentar buscar o nome Id ou id, se não encontrar, não vai criar a URI Reference na assinatura com ID.</param>
        public void AssinarNew(XmlDocument conteudoXML, int emp, int UFCodOrMun, Unimake.Business.DFe.Security.AlgorithmType algorithmType = Unimake.Business.DFe.Security.AlgorithmType.Sha1, bool definirURI = true, string idAttributeName = "Id")
        {
            if(Empresas.Configuracoes[emp].UsaCertificado)
            {
                var v = new TipoArquivoXML("", conteudoXML, UFCodOrMun, false);

                //Assinar o XML
                Unimake.Business.DFe.Security.AssinaturaDigital.Assinar(conteudoXML, v.TagAssinatura, v.TagAtributoId, Empresas.Configuracoes[emp].X509Certificado, algorithmType, definirURI, idAttributeName, true);

                //Assinar o lote
                Unimake.Business.DFe.Security.AssinaturaDigital.Assinar(conteudoXML, v.TagLoteAssinatura, v.TagAtributoId, Empresas.Configuracoes[emp].X509Certificado, algorithmType, definirURI, idAttributeName, true);
            }
        }

        #endregion

        public void CarregarPIN(int emp, string arqXML, Servicos servico)
        {
            if(Empresas.Configuracoes[emp].UsaCertificado)
            {
                if(!string.IsNullOrEmpty(Empresas.Configuracoes[emp].CertificadoPIN) &&
                    Empresas.Configuracoes[emp].X509Certificado.IsA3() &&
                    !Empresas.Configuracoes[emp].CertificadoPINCarregado)
                {
                    var tempFile = "";

                    switch(servico)
                    {
                        case Servicos.ConsultaCadastroContribuinte:
                            tempFile = Functions.ExtraiPastaNomeArq(arqXML, Propriedade.Extensao(Propriedade.TipoEnvio.ConsCad).EnvioXML) + "__" + Propriedade.Extensao(Propriedade.TipoEnvio.ConsCad).EnvioXML;
                            File.Copy(arqXML, tempFile, true);
                            Assinar(tempFile, "ConsCad", "infCons", Empresas.Configuracoes[emp].X509Certificado, emp);
                            break;

                        case Servicos.NFeConsultaStatusServico:
                            tempFile = Functions.ExtraiPastaNomeArq(arqXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML) + "__" + Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML;
                            File.Copy(arqXML, tempFile, true);
                            Assinar(tempFile, "consStatServ", "xServ", Empresas.Configuracoes[emp].X509Certificado, emp);
                            break;

                        case Servicos.NFePedidoConsultaSituacao:
                            tempFile = Functions.ExtraiPastaNomeArq(arqXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML) + "__" + Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML;
                            File.Copy(arqXML, tempFile, true);
                            Assinar(tempFile, "consSitNFe", "xServ", Empresas.Configuracoes[emp].X509Certificado, emp);
                            break;

                        case Servicos.DFeEnviar:
                            tempFile = Functions.ExtraiPastaNomeArq(arqXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvDFe).EnvioXML) + "__" + Propriedade.Extensao(Propriedade.TipoEnvio.EnvDFe).EnvioXML;
                            File.Copy(arqXML, tempFile, true);
                            Assinar(tempFile, "distDFeInt", "CNPJ", Empresas.Configuracoes[emp].X509Certificado, emp);
                            break;

                        case Servicos.CTeDistribuicaoDFe:
                            tempFile = Functions.ExtraiPastaNomeArq(arqXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvDFeCTe).EnvioXML) + "__" + Propriedade.Extensao(Propriedade.TipoEnvio.EnvDFeCTe).EnvioXML;
                            File.Copy(arqXML, tempFile, true);
                            Assinar(tempFile, "distDFeInt", "CNPJ", Empresas.Configuracoes[emp].X509Certificado, emp);
                            break;

                        default:
                            break;
                    }

                    if(tempFile != "" && File.Exists(tempFile))
                    {
                        File.Delete(tempFile);
                    }
                }
            }
        }

        public bool TestarProviderCertificado(string tempFile,
            string tagAssinatura,
            string tagAtributo,
            X509Certificate2 certificado,
            int codEmp,
            string pin)
        {
            var _pin = pin;

            AssinaturaValida = true;
            try
            {
                Assinar(tempFile, tagAssinatura, tagAtributo, certificado, codEmp);
            }
            finally
            {
                Empresas.Configuracoes[codEmp].CertificadoPIN = _pin;
            }
            return AssinaturaValida;
        }

        #region Assinado()

        /// <summary>
        /// Verificar se o XML já tem assinatura
        /// </summary>
        /// <param name="arqXML">Arquivo XML a ser verificado se tem assinatura</param>
        /// <param name="tagAssinatura">Tag de assinatura onde vamos pesquisar</param>
        /// <returns>true = Já está assinado</returns>
        private bool Assinado(string arqXML, string tagAssinatura)
        {
            var retorno = false;
            try
            {
                var doc = new XmlDocument();
                doc.Load(arqXML);

                retorno = Assinado(doc, tagAssinatura);
            }
            catch { }

            return retorno;
        }

        #endregion Assinado()

        #region Assinado()

        /// <summary>
        /// Verificar se o XML já tem assinatura
        /// </summary>
        /// <param name="conteudoXML">Conteúdo do XML</param>
        /// <param name="tagAssinatura">Tag de assinatura onde vamos pesquisar</param>
        /// <returns>true = Já está assinado</returns>
        private bool Assinado(XmlDocument conteudoXML, string tagAssinatura)
        {
            var retorno = false;

            try
            {
                if(conteudoXML.GetElementsByTagName(tagAssinatura)[0].LastChild.Name == "Signature")
                {
                    retorno = true;
                }
            }
            catch { }

            return retorno;
        }

        #endregion Assinado()

        #region Assinar XML de consulta. Específico para município de São José dos Pinhais

        private void AssinarJSP(XmlDocument conteudoXML,
            string tagAssinatura,
            string tagAtributoId,
            X509Certificate2 x509Cert,
            int empresa,
            AlgorithmType algorithmType,
            bool comURI)
        {
            try
            {
                if(x509Cert == null)
                {
                    throw new ExceptionCertificadoDigital(ErroPadrao.CertificadoNaoEncontrado);
                }

                if(conteudoXML.GetElementsByTagName(tagAssinatura).Count == 0)
                {
                    throw new Exception("A tag de assinatura " + tagAssinatura.Trim() + " não existe no XML. (Código do Erro: 5)");
                }
                else if(conteudoXML.GetElementsByTagName(tagAtributoId).Count == 0)
                {
                    throw new Exception("A tag de assinatura " + tagAtributoId.Trim() + " não existe no XML. (Código do Erro: 4)");
                }
                // Existe mais de uma tag a ser assinada
                else
                {
                    XmlNode nodes = conteudoXML.GetElementsByTagName(tagAssinatura)[0];
                    XmlNode childNodes = nodes;

                    // Create a reference to be signed
                    var reference = new Reference
                    {
                        Uri = ""
                    };

                    // pega o uri que deve ser assinada
                    var childElemen = (XmlElement)childNodes;

                    if(comURI)
                    {
                        if(childElemen.GetAttributeNode("Id") != null)
                        {
                            reference.Uri = "#" + childElemen.GetAttributeNode("Id").Value;
                        }
                        else if(childElemen.GetAttributeNode("id") != null)
                        {
                            reference.Uri = "#" + childElemen.GetAttributeNode("id").Value;
                        }
                    }

                    // Create a SignedXml object.
                    var signedXml = new SignedXml(conteudoXML);

                    if(algorithmType.Equals(AlgorithmType.Sha256))
                    {
                        signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
                        signedXml.SigningKey = x509Cert.GetRSAPrivateKey();
                    }

                    if(algorithmType.Equals(AlgorithmType.Sha1))
                    {
                        signedXml.SigningKey = x509Cert.PrivateKey;
                        signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
                        reference.DigestMethod = "http://www.w3.org/2000/09/xmldsig#sha1";
                    }

                    // Add an enveloped transformation to the reference.
                    reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
                    reference.AddTransform(new XmlDsigC14NTransform());

                    // Add the reference to the SignedXml object.
                    signedXml.AddReference(reference);

                    if(algorithmType.Equals(AlgorithmType.Sha256))
                    {
                        reference.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
                    }

                    // Create a new KeyInfo object
                    var keyInfo = new KeyInfo();

                    // Load the certificate into a KeyInfoX509Data object
                    // and add it to the KeyInfo object.
                    keyInfo.AddClause(new KeyInfoX509Data(x509Cert));

                    // Add the KeyInfo object to the SignedXml object.
                    signedXml.KeyInfo = keyInfo;
                    signedXml.ComputeSignature();

                    // Get the XML representation of the signature and save
                    // it to an XmlElement object.
                    var xmlDigitalSignature = signedXml.GetXml();

                    // Gravar o elemento no documento XML
                    nodes.AppendChild(conteudoXML.ImportNode(xmlDigitalSignature, true));
                }
            }
            catch(CryptographicException ex)
            {
                #region #10316

                /*
                 * Solução para o problema do certificado do tipo A3
                 * Marcelo
                 * 29/07/2013
                 */

                AssinaturaValida = false;

                if(x509Cert.IsA3())
                {
                    x509Cert = Empresas.ResetCertificado(empresa);
                    if(!TesteCertificado)
                    {
                        throw new Exception("O certificado deverá ser reiniciado.\r\n Retire o certificado.\r\nAguarde o LED terminar de piscar.\r\n Recoloque o certificado e informe o PIN novamente.\r\n" + ex.ToString());// #12342 concatenar com a mensagem original
                    }
                }
                else
                {
                    throw;
                }

                #endregion #10316
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}