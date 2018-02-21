﻿using System;
using System.IO;
using System.Xml;

namespace NFe.Components
{
    public class TipoArquivoXML
    {
        public int nRetornoTipoArq { get; private set; }
        public string cRetornoTipoArq { get; private set; }

        /// <summary>
        /// Tag que deve ser assinada no XML, se o conteúdo estiver em branco é por que o XML não deve ser assinado
        /// </summary>
        public string TagAssinatura { get; private set; }

        /// <summary>
        /// Tag que tem o atributo ID no XML
        /// </summary>
        public string TagAtributoId { get; private set; }

        /// <summary>
        /// Nome da tag do XML que será assinada (uma segunda tag que tem que ser assinada ex. SubstituirNfse Pelotas-RS)
        /// </summary>
        public string TagAssinatura0 { get; private set; }

        /// <summary>
        /// Nome da tag que tem o atributo ID que será assinada, faz consunto com a TagAssinatura0
        /// </summary>
        public string TagAtributoId0 { get; private set; }

        /// <summary>
        /// Tag que deve ser assinada no XML, se o conteúdo estiver em branco é por que o XML não deve ser assinado
        /// </summary>
        public string TagLoteAssinatura { get; private set; }

        /// <summary>
        /// Tag que tem o atributo ID no XML
        /// </summary>
        public string TagLoteAtributoId { get; private set; }

        public string cArquivoSchema { get; private set; }
        public string TargetNameSpace { get; private set; }

        public TipoArquivoXML(string rotaArqXML, int UFCod, bool soValidar)
        {
            XmlDocument conteudoXML = new XmlDocument();
            conteudoXML.Load(rotaArqXML);

            DefinirTipoArq(rotaArqXML, conteudoXML, UFCod, soValidar);
        }

        public TipoArquivoXML(string rotaArqXML, XmlDocument conteudoXML, int UFCod, bool soValidar)
        {
            DefinirTipoArq(rotaArqXML, conteudoXML, UFCod, soValidar);
        }

        private void DefinirTipoArq(string fullPathXML, XmlDocument conteudoXML, int UFCod, bool soValidar)
        {
            bool nfse = (UFCod.ToString().Length == 7) || (UFCod == 202);
            bool reinf = (conteudoXML.DocumentElement.Name == "Reinf");
            bool esocial = (conteudoXML.DocumentElement.Name == "eSocial");

            nRetornoTipoArq = 0;

            cRetornoTipoArq =
                cArquivoSchema =
                TagAssinatura =
                TagAtributoId =
                TagLoteAssinatura =
                TagLoteAtributoId =
                TargetNameSpace = string.Empty;

            string versaoXML = string.Empty;
            string padraoNFSe = string.Empty;

            try
            {
                if (!reinf && !esocial)
                {
                    #region Definir padrão NFSe

                    if (nfse)
                    {
                        switch (UFCod)
                        {
                            case 4314050: //Parobé
                                padraoNFSe = Functions.PadraoNFSe(UFCod).ToString() + "-4314050-";
                                break;

                            case 4320008: //Sapucaia do Sul - RS
                                padraoNFSe = Functions.PadraoNFSe(UFCod).ToString() + "-4320008-";
                                break;

                            case 4125506: //São José dos Pinhais-PR (GINFES)
                                padraoNFSe = Functions.PadraoNFSe(UFCod).ToString() + "-4125506-";
                                break;

                            case 2304400: //Fortaleza - CE
                                padraoNFSe = Functions.PadraoNFSe(UFCod).ToString() + "-2304400-";
                                break;

                            case 4113700: //Londrina-PR (SIGCORP_SIGISS)
                                padraoNFSe = Functions.PadraoNFSe(UFCod).ToString() + "-4113700-";
                                break;

                            case 4109401: //Guarapuava-PR
                                padraoNFSe = Functions.PadraoNFSe(UFCod).ToString() + "-4109401-";
                                break;

                            case 4303004: //Cachoeira do Sul-RS
                                padraoNFSe = Functions.PadraoNFSe(UFCod).ToString() + "-4303004-";
                                break;

                            case 202: //BETHA 2.02
                                padraoNFSe = PadroesNFSe.BETHA.ToString() + "-202-";
                                break;

                            case 4322509: //Vacari-RS
                                padraoNFSe = Functions.PadraoNFSe(UFCod).ToString() + "-4322509-";
                                break;

                            default:
                                padraoNFSe = Functions.PadraoNFSe(UFCod).ToString() + "-";
                                break;
                        }
                    }

                    #endregion Definir padrão NFSe
                }

                InfSchema schema = null;
                string chave = "";
                string versao = string.Empty;

                try
                {
                    string nome = conteudoXML.DocumentElement.Name;

                    switch (nome)
                    {
                        #region EFDReinf

                        case "Reinf":
                            if (!string.IsNullOrEmpty(conteudoXML.DocumentElement.NamespaceURI))
                                versao = conteudoXML.DocumentElement.NamespaceURI.Substring(conteudoXML.DocumentElement.NamespaceURI.Length - 7);

                            chave = nome + "-" + conteudoXML.DocumentElement.FirstChild.Name;

                            break;

                        #endregion EFDReinf

                        #region eSocial

                        case "eSocial":
                            chave = nome + "-" + conteudoXML.DocumentElement.FirstChild.Name;

                            break;

                        #endregion eSocial

                        #region NFe, NFCe, CTe, MDFe e NFSe

                        default:
                            if (((XmlElement)conteudoXML.GetElementsByTagName(conteudoXML.DocumentElement.Name)[0]).Attributes[TpcnResources.versao.ToString()] != null)
                                versao = ((XmlElement)conteudoXML.GetElementsByTagName(conteudoXML.DocumentElement.Name)[0]).Attributes[TpcnResources.versao.ToString()].Value;
                            else if (((XmlElement)conteudoXML.GetElementsByTagName(conteudoXML.DocumentElement.FirstChild.Name)[0]).Attributes[TpcnResources.versao.ToString()] != null)
                                versao = ((XmlElement)conteudoXML.GetElementsByTagName(conteudoXML.DocumentElement.FirstChild.Name)[0]).Attributes[TpcnResources.versao.ToString()].Value;
                            else if (((XmlElement)conteudoXML.GetElementsByTagName(conteudoXML.DocumentElement.Name)[0]).Attributes["versaoModal"] != null)
                                versao = ((XmlElement)conteudoXML.GetElementsByTagName(conteudoXML.DocumentElement.Name)[0]).Attributes["versaoModal"].Value;

                            if (nfse)
                            {
                                if (Functions.PadraoNFSe(UFCod) == PadroesNFSe.GINFES)
                                {
                                    if (conteudoXML.DocumentElement.Name == "e:CancelarNfseEnvio" && conteudoXML.DocumentElement.FirstChild.Name == "Pedido")
                                    {
                                        versaoXML = "-3";
                                    }
                                }
                            }
                            else if (conteudoXML.DocumentElement.Name.Equals("distDFeInt") && versao.Equals("1.01"))
                                versaoXML = "-" + versao;

                            if (string.IsNullOrEmpty(padraoNFSe))
                            {
                                if (nome.Equals("envEvento") || nome.Equals("eventoCTe"))
                                {
                                    XmlElement cl = (XmlElement)conteudoXML.GetElementsByTagName(TpcnResources.tpEvento.ToString())[0];
                                    if (cl != null)
                                    {
                                        string evento = cl.InnerText;
                                        switch (evento)
                                        {
                                            case "110110":  //XML de Evento da CCe
                                            case "110111":  //XML de Envio de evento de cancelamento
                                            case "110113":  //XML de Envio do evento de contingencia EPEC, CTe
                                            case "110160":  //XML de Envio do evento de Registro Multimodal, CTe
                                            case "111500":  //Evento pedido de prorrogação 1º. prazo
                                            case "111501":  //Evento pedido de prorrogação 2º. prazo
                                            case "111502":  //Evento Cancelamento de Pedido de Prorrogação 1º. Prazo
                                            case "111503":  //Evento Cancelamento de Pedido de Prorrogação 2º. Prazo
                                            case "411500":  //Evento Fisco Resposta ao Pedido de Prorrogação 1º prazo
                                            case "411501":  //Evento Fisco Resposta ao Pedido de Prorrogação 2º prazo
                                            case "411502":  //Evento Fisco Resposta ao Cancelamento de Prorrogação 1º prazo
                                            case "411503":  //Evento Fisco Resposta ao Cancelamento de Prorrogação 2º prazo
                                            case "610110":  //CTe Prestação de Serviços em Desacordo
                                                nome = nome + evento;
                                                break;

                                            case "110140":  //EPEC
                                                string mod = string.Empty;
                                                if (((XmlElement)conteudoXML.GetElementsByTagName("infEvento")[0]).Attributes[TpcnResources.Id.ToString()] != null)
                                                {
                                                    mod = "-" + ((XmlElement)conteudoXML.GetElementsByTagName("infEvento")[0]).Attributes[TpcnResources.Id.ToString()].Value.Substring(28, 2) + "-";
                                                    if (!mod.Equals("-65-"))
                                                        mod = string.Empty;
                                                }

                                                nome = nome + mod + evento;
                                                break;

                                            case "210200":  //XML Evento de manifestação do destinatário
                                            case "210210":  //XML Evento de manifestação do destinatário
                                            case "210220":  //XML Evento de manifestação do destinatário
                                            case "210240":  //XML Evento de manifestação do destinatário
                                                nome = "envConfRecebto";
                                                break;
                                        }
                                    }
                                }
                                else if (nome.Equals("eventoMDFe"))
                                {
                                    XmlElement cl = (XmlElement)conteudoXML.GetElementsByTagName(TpcnResources.tpEvento.ToString())[0];
                                    if (cl != null)
                                    {
                                        nome = "eventoMDFe" + cl.InnerText;
                                    }
                                }
                                else if (nome.Equals("distDFeInt"))
                                {
                                    if (conteudoXML.DocumentElement.NamespaceURI.ToLower().Equals("http://www.portalfiscal.inf.br/cte"))
                                    {
                                        nome = nome + "CTe";
                                    }
                                }
                                else if (nome.Equals("infModal"))
                                {
                                    if (conteudoXML.DocumentElement.NamespaceURI.ToLower().Equals("http://www.portalfiscal.inf.br/cte"))
                                    {
                                        nome = conteudoXML.FirstChild.FirstChild.Name + "-CTe";
                                    }
                                    if (conteudoXML.DocumentElement.NamespaceURI.ToLower().Equals("http://www.portalfiscal.inf.br/mdfe"))
                                    {
                                        nome = conteudoXML.FirstChild.FirstChild.Name + "-MDFe";
                                    }
                                }
                            }

                            if (!nfse)
                                chave = TipoAplicativo.Nfe.ToString().ToUpper() + versaoXML + "-" + nome;
                            else
                                chave = TipoAplicativo.Nfse.ToString().ToUpper() + versaoXML + "-" + padraoNFSe + nome;

                            break;

                            #endregion NFe, NFCe, CTe, MDFe e NFSe
                    }

                    schema = SchemaXML.InfSchemas[chave];
                }
                catch
                {
                    if (soValidar && chave.StartsWith(TipoAplicativo.Nfse.ToString().ToUpper() + versaoXML + "-"))
                    {
                        cRetornoTipoArq = fullPathXML;
                        nRetornoTipoArq = -1;
                        return;
                    }
                    throw new Exception(string.Format("Não foi possível identificar o tipo do XML para ser validado, ou seja, o sistema não sabe se é um XML de {0}, consulta, etc. ", string.IsNullOrEmpty(padraoNFSe) ? "NF-e/NFC-e/CT-e/MDF-e" : "NFS-e") +
                        "Por favor verifique se não existe algum erro de estrutura do XML que impede sua identificação. (Chave: " + chave + ")");
                }

                nRetornoTipoArq = schema.ID;
                cRetornoTipoArq = schema.Descricao;
                TagAssinatura = schema.TagAssinatura;
                TagAtributoId = schema.TagAtributoId;
                TagAssinatura0 = schema.TagAssinatura0;
                TagAtributoId0 = schema.TagAtributoId0;
                TagLoteAssinatura = schema.TagLoteAssinatura;
                TagLoteAtributoId = schema.TagLoteAtributoId;
                TargetNameSpace = schema.TargetNameSpace;

                if (!string.IsNullOrEmpty(schema.ArquivoXSD))
                {
                    if (nfse)
                        cArquivoSchema = Path.Combine(Propriedade.PastaExecutavel, "NFse\\schemas\\" + schema.ArquivoXSD);
                    else
                        cArquivoSchema = Path.Combine(Propriedade.PastaExecutavel, "NFe\\schemas\\" + string.Format(schema.ArquivoXSD, versao));
                }

                if (!string.IsNullOrEmpty(schema.TargetNameSpace) && reinf)
                {
                    TargetNameSpace = string.Format(schema.TargetNameSpace, versao);
                }

            }
            catch (Exception ex)
            {
                nRetornoTipoArq = SchemaXML.MaxID + 102;
                cRetornoTipoArq = ex.Message;
            }

            if (nRetornoTipoArq == 0)
            {
                nRetornoTipoArq = SchemaXML.MaxID + 101;
                cRetornoTipoArq = "Não foi possível identificar o arquivo XML";
            }
        }
    }
}