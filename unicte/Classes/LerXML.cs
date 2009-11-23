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

namespace unicte
{
    /// <summary>
    /// Classe responsável por ler os diversos XML´s utilizados na nota fiscal eletrônica
    /// e dispor as informações em propriedades para facilitar a leitura.
    /// </summary>
    public class LerXML : absLerXML
    {
        public override void ConsCad(string cArquivoXML)
        {
            this.oDadosConsCad.CNPJ = string.Empty;
            this.oDadosConsCad.IE = string.Empty;
            this.oDadosConsCad.UF = string.Empty;

            try
            {
                if (Path.GetExtension(cArquivoXML).ToLower() == ".txt")
                {
                    List<string> cLinhas = new Auxiliar().LerArquivo(cArquivoXML);
                    foreach (string cTexto in cLinhas)
                    {
                        string[] dados = cTexto.Split('|');
                        switch (dados[0].ToLower())
                        {
                            case "cnpj":
                                this.oDadosConsCad.CNPJ = dados[1].Trim();
                                break;
                            case "cpf":
                                this.oDadosConsCad.CPF = dados[1].Trim();
                                break;
                            case "ie":
                                this.oDadosConsCad.IE = dados[1].Trim();
                                break;
                            case "uf":
                                this.oDadosConsCad.UF = dados[1].Trim();
                                break;
                        }
                    }
                }
                else
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
                            if (infConsElemento.GetElementsByTagName("CPF")[0] != null)
                            {
                                this.oDadosConsCad.CPF = infConsElemento.GetElementsByTagName("CPF")[0].InnerText;
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
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

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
            this.oDadosNfe.cUF = string.Empty;
            this.oDadosNfe.cNF = string.Empty;
            this.oDadosNfe.mod = string.Empty;
            this.oDadosNfe.nNF = string.Empty;
            this.oDadosNfe.cDV = string.Empty;
            this.oDadosNfe.CNPJ = string.Empty;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                XmlNodeList infNFeList = doc.GetElementsByTagName("infCte");

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

                    foreach (XmlNode ideNode in ideList)
                    {
                        XmlElement ideElemento = (XmlElement)ideNode;
                        
                        this.oDadosNfe.dEmi = Convert.ToDateTime(ideElemento.GetElementsByTagName("dhEmi")[0].InnerText);
                        this.oDadosNfe.tpEmis = ideElemento.GetElementsByTagName("tpEmis")[0].InnerText;
                        this.oDadosNfe.tpAmb = ideElemento.GetElementsByTagName("tpAmb")[0].InnerText;
                        this.oDadosNfe.serie = ideElemento.GetElementsByTagName("serie")[0].InnerText;
                        this.oDadosNfe.cUF = ideElemento.GetElementsByTagName("cUF")[0].InnerText;
                        this.oDadosNfe.cNF = ideElemento.GetElementsByTagName("cCT")[0].InnerText;
                        this.oDadosNfe.mod = ideElemento.GetElementsByTagName("mod")[0].InnerText;
                        this.oDadosNfe.nNF = ideElemento.GetElementsByTagName("nCT")[0].InnerText;
                        this.oDadosNfe.cDV = ideElemento.GetElementsByTagName("cDV")[0].InnerText;
                    }

                    //Montar lista de tag´s da tag <emit>
                    XmlNodeList emitList = infNFeElemento.GetElementsByTagName("emit");

                    foreach (XmlNode emitNode in emitList)
                    {
                        XmlElement emitElemento = (XmlElement)emitNode;

                        this.oDadosNfe.CNPJ = emitElemento.GetElementsByTagName("CNPJ")[0].InnerText;
                    }
                }

                //Tentar detectar a tag de lote, se tiver vai atualizar o atributo do lote que a nota faz parte
                XmlNodeList enviNFeList = doc.GetElementsByTagName("enviCTe");

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
        /// PedCan(string cArquivoXML)
        /// </summary>
        /// <param name="cArquivoXML"></param>
        public override void PedCanc(string cArquivoXML)
        {
            this.oDadosPedCanc.tpAmb = ConfiguracaoApp.tpAmb;
            this.oDadosPedCanc.tpEmis = ConfiguracaoApp.tpEmis;

            if (Path.GetExtension(cArquivoXML).ToLower() == ".txt")
            {
                //      tpAmb|2
                //      chNFe|35080699999090910270550000000000011234567890
                //      nProt|135080000000001
                //      xJust|Teste do WS de Cancelamento
                //      tpEmis|1                                    <<< opcional >>>
                List<string> cLinhas = new Auxiliar().LerArquivo(cArquivoXML);
                foreach (string cTexto in cLinhas)
                {
                    string[] dados = cTexto.Split('|');
                    switch (dados[0].ToLower())
                    {
                        case "tpamb":
                            this.oDadosPedCanc.tpAmb = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                        case "chnfe":
                            this.oDadosPedCanc.chNFe = dados[1].Trim();
                            break;
                        case "nprot":
                            this.oDadosPedCanc.nProt = dados[1].Trim();
                            break;
                        case "xjust":
                            this.oDadosPedCanc.xJust = dados[1].Trim();
                            break;
                        case "tpemis":
                            this.oDadosPedCanc.tpEmis = Convert.ToInt32("0" + dados[1].Trim());
                            break;
                    }
                }
            }
            else
            {
                //<?xml version="1.0" encoding="UTF-8"?>
                //<cancNFe xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.07">
                //  <infCanc Id="ID35080699999090910270550000000000011234567890">
                //      <tpAmb>2</tpAmb>
                //      <xServ>CANCELAR</xServ>
                //      <chNFe>35080699999090910270550000000000011234567890</chNFe>
                //      <nProt>135080000000001</nProt>
                //      <xJust>Teste do WS de Cancelamento</xJust>
                //      <tpEmis>1</tpEmis>                                      <<< opcional >>>
                //  </infCanc>}
                //</cancNFe>
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                XmlNodeList consStatServList = doc.GetElementsByTagName("infCanc");

                foreach (XmlNode consStatServNode in consStatServList)
                {
                    XmlElement consStatServElemento = (XmlElement)consStatServNode;

                    this.oDadosPedCanc.tpAmb = Convert.ToInt32("0" + consStatServElemento.GetElementsByTagName("tpAmb")[0].InnerText);

                    if (consStatServElemento.GetElementsByTagName("chCTe").Count != 0)
                        this.oDadosPedCanc.chNFe = consStatServElemento.GetElementsByTagName("chCTe")[0].InnerText;
                    /*
                    if (consStatServElemento.GetElementsByTagName("nProt").Count != 0)
                        this.oDadosPedCanc.nProt = consStatServElemento.GetElementsByTagName("nProt")[0].InnerText;

                    if (consStatServElemento.GetElementsByTagName("xJust").Count != 0)
                        this.oDadosPedCanc.xJust = consStatServElemento.GetElementsByTagName("xJust")[0].InnerText;
                    */
                    ///
                    /// danasa 12-9-2009
                    /// 
                    if (consStatServElemento.GetElementsByTagName("tpEmis").Count != 0)
                    {
                        this.oDadosPedCanc.tpEmis = Convert.ToInt16(consStatServElemento.GetElementsByTagName("tpEmis")[0].InnerText);
                        /// para que o validador não rejeite, excluo a tag <tpEmis>
                        doc.DocumentElement.RemoveChild(consStatServElemento.GetElementsByTagName("tpEmis")[0]);
                        /// salvo o arquivo modificado
                        doc.Save(cArquivoXML);
                    }
                }
            }
        }

        /// <summary>
        /// PedInut(string cArquivoXML)
        /// </summary>
        /// <param name="cArquivoXML"></param>
        public override void PedInut(string cArquivoXML)
        {
            this.oDadosPedInut.tpAmb = ConfiguracaoApp.tpAmb;
            this.oDadosPedInut.tpEmis = ConfiguracaoApp.tpEmis;

            try
            {
                if (Path.GetExtension(cArquivoXML).ToLower() == ".txt")
                {
                    //      tpAmb|2
                    //      tpEmis|1                <<< opcional >>>
                    //      cUF|35
                    //      ano|08
                    //      CNPJ|99999090910270
                    //      mod|55
                    //      serie|0
                    //      nNFIni|1
                    //      nNFFin|1
                    //      xJust|Teste do WS de Inutilizacao
                    List<string> cLinhas = new Auxiliar().LerArquivo(cArquivoXML);
                    foreach (string cTexto in cLinhas)
                    {
                        string[] dados = cTexto.Split('|');
                        switch (dados[0].ToLower())
                        {
                            case "tpamb":
                                this.oDadosPedInut.tpAmb = Convert.ToInt32("0" + dados[1].Trim());
                                break;
                            case "tpemis":
                                this.oDadosPedInut.tpEmis = Convert.ToInt32("0" + dados[1].Trim());
                                break;
                            case "cuf":
                                this.oDadosPedInut.cUF = Convert.ToInt32("0" + dados[1].Trim());
                                break;
                            case "ano":
                                this.oDadosPedInut.ano = Convert.ToInt32("0" + dados[1].Trim());
                                break;
                            case "cnpj":
                                this.oDadosPedInut.CNPJ = dados[1].Trim();
                                break;
                            case "mod":
                                this.oDadosPedInut.mod = Convert.ToInt32("0" + dados[1].Trim());
                                break;
                            case "serie":
                                this.oDadosPedInut.serie = Convert.ToInt32("0" + dados[1].Trim());
                                break;
                            case "nnfini":
                                this.oDadosPedInut.nNFIni = Convert.ToInt32("0" + dados[1].Trim());
                                break;
                            case "nnffin":
                                this.oDadosPedInut.nNFFin = Convert.ToInt32("0" + dados[1].Trim());
                                break;
                            case "xjust":
                                this.oDadosPedInut.xJust = dados[1].Trim();
                                break;
                        }
                    }
                }
                else
                {
                    //<?xml version="1.0" encoding="UTF-8"?>
                    //<inutNFe xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.07">
                    //  <infInut Id="ID359999909091027055000000000001000000001">
                    //      <tpAmb>2</tpAmb>
                    //      <tpEmis>1</tpEmis>                  <<< opcional >>>
                    //      <xServ>INUTILIZAR</xServ>
                    //      <cUF>35</cUF>
                    //      <ano>08</ano>
                    //      <CNPJ>99999090910270</CNPJ>
                    //      <mod>55</mod>
                    //      <serie>0</serie>
                    //      <nNFIni>1</nNFIni>
                    //      <nNFFin>1</nNFFin>
                    //      <xJust>Teste do WS de InutilizaÃ§Ã£o</xJust>
                    //  </infInut>
                    //</inutNFe>
                    XmlDocument doc = new XmlDocument();
                    doc.Load(cArquivoXML);

                    XmlNodeList ConsCadList = doc.GetElementsByTagName("inutCTe");
                    foreach (XmlNode ConsCadNode in ConsCadList)
                    {
                        XmlElement ConsCadElemento = (XmlElement)ConsCadNode;

                        XmlNodeList infConsList = ConsCadElemento.GetElementsByTagName("infInut");

                        foreach (XmlNode infConsNode in infConsList)
                        {
                            XmlElement infConsElemento = (XmlElement)infConsNode;

                            if (infConsElemento.GetElementsByTagName("tpAmb")[0] != null)
                                this.oDadosPedInut.tpAmb = Convert.ToInt32("0" + infConsElemento.GetElementsByTagName("tpAmb")[0].InnerText);

                            if (infConsElemento.GetElementsByTagName("cUF")[0] != null)
                                this.oDadosPedInut.cUF = Convert.ToInt32("0" + infConsElemento.GetElementsByTagName("cUF")[0].InnerText);

                            if (infConsElemento.GetElementsByTagName("ano")[0] != null)
                                this.oDadosPedInut.ano = Convert.ToInt32("0" + infConsElemento.GetElementsByTagName("ano")[0].InnerText);

                            if (infConsElemento.GetElementsByTagName("CNPJ")[0] != null)
                                this.oDadosPedInut.CNPJ = infConsElemento.GetElementsByTagName("CNPJ")[0].InnerText;

                            if (infConsElemento.GetElementsByTagName("mod")[0] != null)
                                this.oDadosPedInut.mod = Convert.ToInt32("0" + infConsElemento.GetElementsByTagName("mod")[0].InnerText);

                            if (infConsElemento.GetElementsByTagName("serie")[0] != null)
                                this.oDadosPedInut.serie = Convert.ToInt32("0" + infConsElemento.GetElementsByTagName("serie")[0].InnerText);

                            if (infConsElemento.GetElementsByTagName("nCTIni")[0] != null)
                                this.oDadosPedInut.nNFIni = Convert.ToInt32("0" + infConsElemento.GetElementsByTagName("nCTIni")[0].InnerText);

                            if (infConsElemento.GetElementsByTagName("nCTFin")[0] != null)
                                this.oDadosPedInut.nNFFin = Convert.ToInt32("0" + infConsElemento.GetElementsByTagName("nCTFin")[0].InnerText);

                            ///
                            /// danasa 12-9-2009
                            /// 
                            if (infConsElemento.GetElementsByTagName("tpEmis").Count != 0)
                            {
                                this.oDadosPedInut.tpEmis = Convert.ToInt16(infConsElemento.GetElementsByTagName("tpEmis")[0].InnerText);
                                /// para que o validador não rejeite, excluo a tag <tpEmis>
                                doc.DocumentElement.RemoveChild(infConsElemento.GetElementsByTagName("tpEmis")[0]);
                                /// salvo o arquivo modificado
                                doc.Save(cArquivoXML);
                            }
                        }
                    }
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
            this.oDadosPedSit.tpAmb = ConfiguracaoApp.tpAmb;// string.Empty;
            this.oDadosPedSit.chNFe = string.Empty;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                XmlNodeList consSitNFeList = doc.GetElementsByTagName("consSitCTe");

                foreach (XmlNode consSitNFeNode in consSitNFeList)
                {
                    XmlElement consSitNFeElemento = (XmlElement)consSitNFeNode;

                    this.oDadosPedSit.tpAmb = Convert.ToInt16(consSitNFeElemento.GetElementsByTagName("tpAmb")[0].InnerText);
                    this.oDadosPedSit.chNFe = consSitNFeElemento.GetElementsByTagName("chCTe")[0].InnerText;
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
            this.oDadosPedSta.tpAmb = 0;
            this.oDadosPedSta.cUF = ConfiguracaoApp.UFCod;
            ///
            /// danasa 9-2009
            /// Assume o que está na configuracao
            /// 
            this.oDadosPedSta.tpEmis = ConfiguracaoApp.tpEmis;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                XmlNodeList consStatServList = doc.GetElementsByTagName("consStatServCte");

                foreach (XmlNode consStatServNode in consStatServList)
                {
                    XmlElement consStatServElemento = (XmlElement)consStatServNode;

                    this.oDadosPedSta.tpAmb = Convert.ToInt16(consStatServElemento.GetElementsByTagName("tpAmb")[0].InnerText);

                    if (consStatServElemento.GetElementsByTagName("cUF").Count != 0)
                    {
                        this.oDadosPedSta.cUF = Convert.ToInt32(consStatServElemento.GetElementsByTagName("cUF")[0].InnerText);
                        /// para que o validador não rejeite, excluo a tag <cUF>
                        doc.DocumentElement.RemoveChild(consStatServElemento.GetElementsByTagName("cUF")[0]);
                        /// salvo o arquivo modificado
                        doc.Save(cArquivoXML);
                    }

                    ///
                    /// danasa 9-2009
                    /// 
                    if (consStatServElemento.GetElementsByTagName("tpEmis").Count != 0)
                    {
                        this.oDadosPedSta.tpEmis = Convert.ToInt16(consStatServElemento.GetElementsByTagName("tpEmis")[0].InnerText);
                        /// para que o validador não rejeite, excluo a tag <tpEmis>
                        doc.DocumentElement.RemoveChild(consStatServElemento.GetElementsByTagName("tpEmis")[0]);
                        /// salvo o arquivo modificado
                        doc.Save(cArquivoXML);
                    }
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

                XmlNodeList retEnviNFeList = xml.GetElementsByTagName("retEnviCte");

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
    }
}