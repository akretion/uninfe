using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// Tag que deve ser assinada no XML, se o conteúdo estiver em branco é por que o XML não deve ser assinado
        /// </summary>
        public string TagLoteAssinatura { get; private set; }
        /// <summary>
        /// Tag que tem o atributo ID no XML
        /// </summary>
        public string TagLoteAtributoId { get; private set; }
        public string cArquivoSchema { get; private set; }
        public string TargetNameSpace { get; private set; }

        public TipoArquivoXML(string rotaArqXML, int UFCod)
        {
            DefinirTipoArq(rotaArqXML, UFCod);
        }

        private void DefinirTipoArq(string fullPathXML, int UFCod)
        {
            nRetornoTipoArq = 0;
            cRetornoTipoArq = string.Empty;
            cArquivoSchema = string.Empty;
            TagAssinatura = string.Empty;
            TagAtributoId = string.Empty;
            TagLoteAssinatura = string.Empty;
            TagLoteAtributoId = string.Empty;
            TargetNameSpace = string.Empty;
            
            string versaoXML = string.Empty;

            string padraoNFSe = string.Empty;
            if (Propriedade.TipoAplicativo == TipoAplicativo.Nfse)
                padraoNFSe = Functions.PadraoNFSe(UFCod).ToString() + "-";
            else
                padraoNFSe = string.Empty;

            try
            {
                if (File.Exists(fullPathXML))
                {
                    if (fullPathXML.EndsWith(".txt"))
                    {
                        this.nRetornoTipoArq = SchemaXML.MaxID + 104;
                        this.cRetornoTipoArq = "Arquivo '" + fullPathXML + " não pode ser um arquivo texto";
                        return;
                    }

                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(fullPathXML);
                        string nome = doc.DocumentElement.Name;
                        string versao = string.Empty;
                        if (((XmlElement)(XmlNode)doc.GetElementsByTagName(doc.DocumentElement.Name)[0]).Attributes["versao"] != null)
                            versao = ((XmlElement)(XmlNode)doc.GetElementsByTagName(doc.DocumentElement.Name)[0]).Attributes["versao"].Value;
                        else if (((XmlElement)(XmlNode)doc.GetElementsByTagName(doc.DocumentElement.FirstChild.Name)[0]).Attributes["versao"] != null)
                            versao = ((XmlElement)(XmlNode)doc.GetElementsByTagName(doc.DocumentElement.FirstChild.Name)[0]).Attributes["versao"].Value;

                        if (versao.Equals("3.10") && Propriedade.TipoAplicativo == TipoAplicativo.Nfe)
                            versaoXML = "-" + versao;
                        
                        InfSchema schema = null;
                        try
                        {
                            if (Propriedade.TipoAplicativo == TipoAplicativo.Nfe)
                            {
                                if (nome.Equals("envEvento") || nome.Equals("eventoCTe"))
                                {
                                    XmlElement cl = (XmlElement)doc.GetElementsByTagName("tpEvento")[0];
                                    if (cl != null)
                                    {
                                        string evento = cl.InnerText;
                                        switch (evento)
                                        {
                                            case "110110":  //XML de Evento da CCe
                                            case "110111":  //XML de Envio de evento de cancelamento
                                            case "110113":  //XML de Envio do evento de contingencia EPEC, CTe
                                            case "110160":  //XML de Envio do evento de Registro Multimodal, CTe
                                            case "110140":  //EPEC
                                                nome = nome + evento;
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
                                    XmlElement cl = (XmlElement)doc.GetElementsByTagName("tpEvento")[0];
                                    if (cl != null)
                                    {
                                        nome = "eventoMDFe" + cl.InnerText;
                                    }
                                }
                            }

                            schema = SchemaXML.InfSchemas[Propriedade.TipoAplicativo.ToString().ToUpper() + versaoXML + "-" + padraoNFSe + nome];
                        }
                        catch
                        {
                            throw new Exception("Não foi possível identificar o tipo do XML para ser validado, ou seja, o sistema não sabe se é um XML de NFe, consulta, etc. Por favor verifique se não existe algum erro de estrutura do XML que impede sua identificação.");
                        }

                        nRetornoTipoArq = schema.ID;
                        cRetornoTipoArq = schema.Descricao;
                        cArquivoSchema = schema.ArquivoXSD;
                        TagAssinatura = schema.TagAssinatura;
                        TagAtributoId = schema.TagAtributoId;
                        TagLoteAssinatura = schema.TagLoteAssinatura;
                        TagLoteAtributoId = schema.TagLoteAtributoId;
                        TargetNameSpace = schema.TargetNameSpace;
                    }
                    catch (Exception ex)
                    {
                        this.nRetornoTipoArq = SchemaXML.MaxID + 102;
                        this.cRetornoTipoArq = ex.Message;
                    }
                }
                else
                {
                    this.nRetornoTipoArq = SchemaXML.MaxID + 100;
                    this.cRetornoTipoArq = "Arquivo XML não foi encontrado";
                }
            }
            catch (Exception ex)
            {
                this.nRetornoTipoArq = SchemaXML.MaxID + 103;
                this.cRetornoTipoArq = ex.Message;
            }

            if (this.nRetornoTipoArq == 0)
            {
                this.nRetornoTipoArq = SchemaXML.MaxID + 101;
                this.cRetornoTipoArq = "Não foi possível identificar o arquivo XML";
            }
        }

    }
}
