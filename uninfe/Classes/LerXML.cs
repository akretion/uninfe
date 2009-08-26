//------------------------------------------------------------------------------ 
// <copyright file="UniLerXMLClass.cs" company="Unimake"> 
// 
// Copyright (c) 2008 Unimake Softwares. All rights reserved.
//
// Programador: Wandrey Mundin Ferreira
// 
// </copyright> 
//------------------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using UniNFeLibrary;

namespace uninfe
{
    /// <summary>
    /// Classe responsável por ler os diversos XML´s utilizados na nota fiscal eletrônica
    /// e dispor as informações em propriedades para facilitar a leitura.
    /// </summary>
    public class LerXML : absLerXML
    {
        /// <summary>
        /// Faz a leitura do XML da nota fiscal eletrônica e disponibiliza os valores
        /// de algumas tag´s
        /// </summary>
        /// <param name="cArquivoXML">Caminho e nome do arquivo XML da NFe a ser lido</param>
        /// <example>
        /// UniLerXmlClass oLerXml = new UniLerXmlClass();
        /// oLerXml.Nfe( cPasta_Nome_ArquivoXML );
        /// DateTime dEmi = oLerXml.Nfe.oDadosNfe.dEmi;
        /// </example>
        /// <remarks>
        /// Gera exception em caso de problemas na leitura
        /// </remarks>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>10/01/2009</date>
        public override void Nfe(string cArquivoXML)
        {
            this.oDadosNfe.chavenfe = string.Empty;
            this.oDadosNfe.idLote = string.Empty;
            this.oDadosNfe.tpAmb = string.Empty;
            this.oDadosNfe.tpEmis = string.Empty;
            this.oDadosNfe.serie = string.Empty;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                XmlNodeList infNFeList = doc.GetElementsByTagName("infNFe");

                foreach (XmlNode infNFeNode in infNFeList)
                {
                    XmlElement infNFeElemento = (XmlElement)infNFeNode;

                    //Pegar a chave da NF-e
                    if (infNFeElemento.HasAttributes)
                    {
                        this.oDadosNfe.chavenfe = infNFeElemento.Attributes["Id"].InnerText;
                    }

                    //Montar lista de tag´s da tag <ide>
                    XmlNodeList ideList = infNFeElemento.GetElementsByTagName("ide");

                    //Pegar o conteúdo da tag <dEmi>
                    foreach (XmlNode ideNode in ideList)
                    {
                        XmlElement ideElemento = (XmlElement)ideNode;

                        this.oDadosNfe.dEmi = Convert.ToDateTime(ideElemento.GetElementsByTagName("dEmi")[0].InnerText);
                        this.oDadosNfe.tpEmis = ideElemento.GetElementsByTagName("tpEmis")[0].InnerText;
                        this.oDadosNfe.tpAmb = ideElemento.GetElementsByTagName("tpAmb")[0].InnerText;
                        this.oDadosNfe.serie = ideElemento.GetElementsByTagName("serie")[0].InnerText;
                    }
                }

                //Tentar detectar a tag de lote, se tiver vai atualizar o atributo do lote que a nota faz parte
                XmlNodeList enviNFeList = doc.GetElementsByTagName("enviNFe");

                foreach (XmlNode enviNFeNode in enviNFeList)
                {
                    XmlElement enviNFeElemento = (XmlElement)enviNFeNode;

                    this.oDadosNfe.idLote = enviNFeElemento.GetElementsByTagName("idLote")[0].InnerText;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Faz a leitura do XML do Recibo do lote enviado e disponibiliza os valores
        /// de algumas tag´s
        /// </summary>
        /// <param name="cArquivoXML">Caminho e nome do arquivo XML da NFe a ser lido</param>
        /// <param name="strXml">String contendo o XML</param>
        /// <example>
        /// UniLerXmlClass oLerXml = new UniLerXmlClass();
        /// oLerXml.Recibo( strXml );
        /// String nRec = oLerXml.oDadosRec.nRec;
        /// </example>
        /// <remarks>
        /// Gera exception em caso de problemas na leitura
        /// </remarks>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        public override void Recibo(string strXml)
        {
            MemoryStream memoryStream = Auxiliar.StringXmlToStream(strXml);

            this.oDadosRec.cStat = string.Empty;
            this.oDadosRec.nRec = string.Empty;
            this.oDadosRec.tMed = 0;

            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(memoryStream);

                XmlNodeList retEnviNFeList = xml.GetElementsByTagName("retEnviNFe");

                foreach (XmlNode retEnviNFeNode in retEnviNFeList)
                {
                    XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeNode;

                    this.oDadosRec.cStat = retEnviNFeElemento.GetElementsByTagName("cStat")[0].InnerText;

                    XmlNodeList infRecList = xml.GetElementsByTagName("infRec");

                    foreach (XmlNode infRecNode in infRecList)
                    {
                        XmlElement infRecElemento = (XmlElement)infRecNode;

                        this.oDadosRec.nRec = infRecElemento.GetElementsByTagName("nRec")[0].InnerText;
                        this.oDadosRec.tMed = Convert.ToInt32(infRecElemento.GetElementsByTagName("tMed")[0].InnerText);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Faz a leitura do XML de pedido do status de serviço
        /// </summary>
        /// <param name="cArquivoXml">Nome do XML a ser lido</param>
        /// <by>Wandrey Mundin Ferreira</by>
        public override void PedSta(string cArquivoXML)
        {
            this.oDadosPedSta.tpAmb = string.Empty;
            this.oDadosPedSta.cUF = string.Empty;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                XmlNodeList consStatServList = doc.GetElementsByTagName("consStatServ");

                foreach (XmlNode consStatServNode in consStatServList)
                {
                    XmlElement consStatServElemento = (XmlElement)consStatServNode;

                    this.oDadosPedSta.tpAmb = consStatServElemento.GetElementsByTagName("tpAmb")[0].InnerText;
                    this.oDadosPedSta.cUF = consStatServElemento.GetElementsByTagName("cUF")[0].InnerText;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Faz a leitura do XML de pedido de consulta da situação da NFe
        /// </summary>
        /// <param name="cArquivoXML">Nome do XML a ser lido</param>
        /// <by>Wandrey Mundin Ferreira</by>
        public override void PedSit(string cArquivoXML)
        {
            this.oDadosPedSit.tpAmb = string.Empty;
            this.oDadosPedSit.chNFe = string.Empty;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                XmlNodeList consSitNFeList = doc.GetElementsByTagName("consSitNFe");

                foreach (XmlNode consSitNFeNode in consSitNFeList)
                {
                    XmlElement consSitNFeElemento = (XmlElement)consSitNFeNode;

                    this.oDadosPedSit.tpAmb = consSitNFeElemento.GetElementsByTagName("tpAmb")[0].InnerText;
                    this.oDadosPedSit.chNFe = consSitNFeElemento.GetElementsByTagName("chNFe")[0].InnerText;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public override void ConsCad(string cArquivoXML)
        {
            oDadosConsCad.CNPJ = string.Empty;
            this.oDadosConsCad.IE = string.Empty;
            this.oDadosConsCad.UF = string.Empty;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                XmlNodeList ConsCadList = doc.GetElementsByTagName("ConsCad");
                foreach (XmlNode ConsCadNode in ConsCadList)
                {
                    XmlElement ConsCadElemento = (XmlElement)ConsCadNode;

                    XmlNodeList infConsList = ConsCadElemento.GetElementsByTagName("infCons");

                    foreach (XmlNode infConsNode in infConsList)
                    {
                        XmlElement infConsElemento = (XmlElement)infConsNode;

                        if (infConsElemento.GetElementsByTagName("CNPJ")[0] != null)
                        {
                            this.oDadosConsCad.CNPJ = infConsElemento.GetElementsByTagName("CNPJ")[0].InnerText;
                        }
                        if (infConsElemento.GetElementsByTagName("UF")[0] != null)
                        {
                            this.oDadosConsCad.UF = infConsElemento.GetElementsByTagName("UF")[0].InnerText;
                        }
                        if (infConsElemento.GetElementsByTagName("IE")[0] != null)
                        {
                            this.oDadosConsCad.IE = infConsElemento.GetElementsByTagName("IE")[0].InnerText;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}