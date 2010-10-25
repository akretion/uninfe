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
using System.Threading;

namespace uninfe
{
    /// <summary>
    /// Classe responsável por ler os diversos XML´s utilizados na nota fiscal eletrônica
    /// e dispor as informações em propriedades para facilitar a leitura.
    /// </summary>
    public class LerXML : absLerXML
    {
        #region ConsCad()
        /// <summary>
        /// Faz a leitura do XML de consulta do cadastro do contribuinte e disponibiliza os valores de algumas tag´s
        /// </summary>
        /// <param name="cArquivoXML">Caminho e nome do arquivo XML da consulta do cadastro do contribuinte a ser lido</param>
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
        #endregion

        #region Nfe()
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

                    foreach (XmlNode ideNode in ideList)
                    {
                        XmlElement ideElemento = (XmlElement)ideNode;

                        this.oDadosNfe.dEmi = Convert.ToDateTime(ideElemento.GetElementsByTagName("dEmi")[0].InnerText);
                        this.oDadosNfe.tpEmis = ideElemento.GetElementsByTagName("tpEmis")[0].InnerText;
                        this.oDadosNfe.tpAmb = ideElemento.GetElementsByTagName("tpAmb")[0].InnerText;
                        this.oDadosNfe.serie = ideElemento.GetElementsByTagName("serie")[0].InnerText;
                        this.oDadosNfe.cUF = ideElemento.GetElementsByTagName("cUF")[0].InnerText;
                        this.oDadosNfe.cNF = ideElemento.GetElementsByTagName("cNF")[0].InnerText;
                        this.oDadosNfe.mod = ideElemento.GetElementsByTagName("mod")[0].InnerText;
                        this.oDadosNfe.nNF = ideElemento.GetElementsByTagName("nNF")[0].InnerText;
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
        #endregion

        #region PedCanc()
        /// <summary>
        /// PedCan(string cArquivoXML)
        /// </summary>
        /// <param name="cArquivoXML"></param>
        public override void PedCanc(string cArquivoXML)
        {
            int emp = Empresa.FindEmpresaThread(Thread.CurrentThread.Name);

            this.oDadosPedCanc.tpAmb = Empresa.Configuracoes[emp].tpAmb;
            this.oDadosPedCanc.tpEmis = Empresa.Configuracoes[emp].tpEmis;

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

                XmlNodeList infCancList = doc.GetElementsByTagName("infCanc");

                foreach (XmlNode infCancNode in infCancList)
                {
                    XmlElement infCancElemento = (XmlElement)infCancNode;

                    this.oDadosPedCanc.tpAmb = Convert.ToInt32("0" + infCancElemento.GetElementsByTagName("tpAmb")[0].InnerText);

                    if (infCancElemento.GetElementsByTagName("chNFe").Count != 0)
                        this.oDadosPedCanc.chNFe = infCancElemento.GetElementsByTagName("chNFe")[0].InnerText;

                    ///
                    /// danasa 12-9-2009
                    /// 
                    if (infCancElemento.GetElementsByTagName("tpEmis").Count != 0)
                    {
                        this.oDadosPedCanc.tpEmis = Convert.ToInt16(infCancElemento.GetElementsByTagName("tpEmis")[0].InnerText);
                        /// para que o validador não rejeite, excluo a tag <tpEmis>
                        doc.DocumentElement["infCanc"].RemoveChild(infCancElemento.GetElementsByTagName("tpEmis")[0]);

                        /// salvo o arquivo modificado
                        doc.Save(cArquivoXML);
                    }
                }
            }
        }
        #endregion

        #region PedInut()
        /// <summary>
        /// PedInut(string cArquivoXML)
        /// </summary>
        /// <param name="cArquivoXML"></param>
        public override void PedInut(string cArquivoXML)
        {
            int emp = Empresa.FindEmpresaThread(Thread.CurrentThread.Name);

            this.oDadosPedInut.tpAmb = Empresa.Configuracoes[emp].tpAmb;
            this.oDadosPedInut.tpEmis = Empresa.Configuracoes[emp].tpEmis;

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

                    XmlNodeList InutNFeList = doc.GetElementsByTagName("inutNFe");
                    foreach (XmlNode InutNFeNode in InutNFeList)
                    {
                        XmlElement InutNFeElemento = (XmlElement)InutNFeNode;

                        XmlNodeList infInutList = InutNFeElemento.GetElementsByTagName("infInut");

                        foreach (XmlNode infInutNode in infInutList)
                        {
                            XmlElement infInutElemento = (XmlElement)infInutNode;

                            if (infInutElemento.GetElementsByTagName("tpAmb")[0] != null)
                                this.oDadosPedInut.tpAmb = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("tpAmb")[0].InnerText);

                            if (infInutElemento.GetElementsByTagName("cUF")[0] != null)
                                this.oDadosPedInut.cUF = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("cUF")[0].InnerText);

                            if (infInutElemento.GetElementsByTagName("ano")[0] != null)
                                this.oDadosPedInut.ano = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("ano")[0].InnerText);

                            if (infInutElemento.GetElementsByTagName("CNPJ")[0] != null)
                                this.oDadosPedInut.CNPJ = infInutElemento.GetElementsByTagName("CNPJ")[0].InnerText;

                            if (infInutElemento.GetElementsByTagName("mod")[0] != null)
                                this.oDadosPedInut.mod = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("mod")[0].InnerText);

                            if (infInutElemento.GetElementsByTagName("serie")[0] != null)
                                this.oDadosPedInut.serie = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("serie")[0].InnerText);

                            if (infInutElemento.GetElementsByTagName("nNFIni")[0] != null)
                                this.oDadosPedInut.nNFIni = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("nNFIni")[0].InnerText);

                            if (infInutElemento.GetElementsByTagName("nNFFin")[0] != null)
                                this.oDadosPedInut.nNFFin = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("nNFFin")[0].InnerText);

                            ///
                            /// danasa 12-9-2009
                            /// 
                            if (infInutElemento.GetElementsByTagName("tpEmis").Count != 0)
                            {
                                this.oDadosPedInut.tpEmis = Convert.ToInt16(infInutElemento.GetElementsByTagName("tpEmis")[0].InnerText);
                                /// para que o validador não rejeite, excluo a tag <tpEmis>
                                doc.DocumentElement["infInut"].RemoveChild(infInutElemento.GetElementsByTagName("tpEmis")[0]);
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
        #endregion

        #region PedSit()
        /// <summary>
        /// Faz a leitura do XML de pedido de consulta da situação da NFe
        /// </summary>
        /// <param name="cArquivoXML">Nome do XML a ser lido</param>
        /// <by>Wandrey Mundin Ferreira</by>
        public override void PedSit(string cArquivoXML)
        {
            int emp = Empresa.FindEmpresaThread(Thread.CurrentThread.Name);

            this.oDadosPedSit.tpAmb = Empresa.Configuracoes[emp].tpAmb;// string.Empty;
            this.oDadosPedSit.chNFe = string.Empty;

            try
            {
                if (Path.GetExtension(cArquivoXML).ToLower() == ".txt")
                {
                    //      tpAmb|2
                    //      tpEmis|1                <<< opcional >>>
                    //      chNFe|35080600000000000000550000000000010000000000
                    List<string> cLinhas = new Auxiliar().LerArquivo(cArquivoXML);
                    foreach (string cTexto in cLinhas)
                    {
                        string[] dados = cTexto.Split('|');
                        switch (dados[0].ToLower())
                        {
                            case "tpamb":
                                this.oDadosPedSit.tpAmb = Convert.ToInt32("0" + dados[1].Trim());
                                break;
                            case "tpemis":
                                this.oDadosPedSit.tpEmis = Convert.ToInt32("0" + dados[1].Trim());
                                break;
                            case "chnfe":
                                this.oDadosPedSit.chNFe = dados[1].Trim();
                                break;
                        }
                    }
                }
                else
                {
                    //<?xml version="1.0" encoding="UTF-8"?>
                    //<consSitNFe xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.07">
                    //  <tpAmb>2</tpAmb>
                    //  <tpEmis>1</tpEmis>                          <<< opcional >>>
                    //  <xServ>CONSULTAR</xServ>
                    //  <chNFe>35080600000000000000550000000000010000000000</chNFe>
                    //</consSitNFe>                  
                    XmlDocument doc = new XmlDocument();
                    doc.Load(cArquivoXML);

                    XmlNodeList consSitNFeList = doc.GetElementsByTagName("consSitNFe");

                    foreach (XmlNode consSitNFeNode in consSitNFeList)
                    {
                        XmlElement consSitNFeElemento = (XmlElement)consSitNFeNode;

                        this.oDadosPedSit.tpAmb = Convert.ToInt32("0" + consSitNFeElemento.GetElementsByTagName("tpAmb")[0].InnerText);
                        this.oDadosPedSit.chNFe = consSitNFeElemento.GetElementsByTagName("chNFe")[0].InnerText;
                        ///
                        /// danasa 12-9-2009
                        /// 
                        if (consSitNFeElemento.GetElementsByTagName("tpEmis").Count != 0)
                        {
                            this.oDadosPedSit.tpEmis = Convert.ToInt16(consSitNFeElemento.GetElementsByTagName("tpEmis")[0].InnerText);
                            /// para que o validador não rejeite, excluo a tag <tpEmis>
                            doc.DocumentElement.RemoveChild(consSitNFeElemento.GetElementsByTagName("tpEmis")[0]);
                            /// salvo o arquivo modificado
                            doc.Save(cArquivoXML);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region PedSta()
        /// <summary>
        /// Faz a leitura do XML de pedido do status de serviço
        /// </summary>
        /// <param name="cArquivoXml">Nome do XML a ser lido</param>
        /// <by>Wandrey Mundin Ferreira</by>
        public override void PedSta(string cArquivoXML)
        {
            int emp = Empresa.FindEmpresaThread(Thread.CurrentThread.Name);

            this.oDadosPedSta.tpAmb = 0;
            this.oDadosPedSta.cUF = Empresa.Configuracoes[emp].UFCod;
            ///
            /// danasa 9-2009
            /// Assume o que está na configuracao
            /// 
            this.oDadosPedSta.tpEmis = Empresa.Configuracoes[emp].tpEmis;

            try
            {
                ///
                /// danasa 12-9-2009
                /// 
                if (Path.GetExtension(cArquivoXML).ToLower() == ".txt")
                {
                    // tpEmis|1						<<< opcional >>>
                    // tpAmb|1
                    // cUF|35
                    List<string> cLinhas = new Auxiliar().LerArquivo(cArquivoXML);
                    foreach (string cTexto in cLinhas)
                    {
                        string[] dados = cTexto.Split('|');
                        switch (dados[0].ToLower())
                        {
                            case "tpamb":
                                this.oDadosPedSta.tpAmb = Convert.ToInt32("0" + dados[1].Trim());
                                break;
                            case "cuf":
                                this.oDadosPedSta.cUF = Convert.ToInt32("0" + dados[1].Trim());
                                break;
                            case "tpemis":
                                this.oDadosPedSta.tpEmis = Convert.ToInt32("0" + dados[1].Trim());
                                break;
                        }
                    }
                }
                else
                {
                    //<?xml version="1.0" encoding="UTF-8"?>
                    //<consStatServ xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.07">
                    //  <tpAmb>2</tpAmb>
                    //  <cUF>35</cUF>
                    //  <xServ>STATUS</xServ>
                    //</consStatServ>                    

                    XmlDocument doc = new XmlDocument();
                    doc.Load(cArquivoXML);

                    XmlNodeList consStatServList = doc.GetElementsByTagName("consStatServ");

                    foreach (XmlNode consStatServNode in consStatServList)
                    {
                        XmlElement consStatServElemento = (XmlElement)consStatServNode;

                        this.oDadosPedSta.tpAmb = Convert.ToInt32("0" + consStatServElemento.GetElementsByTagName("tpAmb")[0].InnerText);

                        if (consStatServElemento.GetElementsByTagName("cUF").Count != 0)
                        {
                            this.oDadosPedSta.cUF = Convert.ToInt32("0" + consStatServElemento.GetElementsByTagName("cUF")[0].InnerText);
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
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region Recibo
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
        #endregion

        #region PedSta()
        /// <summary>
        /// Faz a leitura do XML de pedido da consulta do recibo do lote de notas enviadas
        /// </summary>
        /// <param name="cArquivoXml">Nome do XML a ser lido</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 16/03/2010
        /// </remarks>
        public override void PedRec(string cArquivoXML)
        {
            int emp = Empresa.FindEmpresaThread(Thread.CurrentThread.Name);

            this.oDadosPedRec.tpAmb = 0;
            this.oDadosPedRec.tpEmis = Empresa.Configuracoes[emp].tpEmis;
            this.oDadosPedRec.cUF = Empresa.Configuracoes[emp].UFCod;
            this.oDadosPedRec.nRec = string.Empty;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                XmlNodeList consReciNFeList = doc.GetElementsByTagName("consReciNFe");

                foreach (XmlNode consReciNFeNode in consReciNFeList)
                {
                    XmlElement consReciNFeElemento = (XmlElement)consReciNFeNode;

                    this.oDadosPedRec.tpAmb = Convert.ToInt32("0" + consReciNFeElemento.GetElementsByTagName("tpAmb")[0].InnerText);
                    this.oDadosPedRec.nRec = consReciNFeElemento.GetElementsByTagName("nRec")[0].InnerText;

                    if (consReciNFeElemento.GetElementsByTagName("cUF").Count != 0)
                    {
                        this.oDadosPedRec.cUF = Convert.ToInt32("0" + consReciNFeElemento.GetElementsByTagName("cUF")[0].InnerText);
                        /// Para que o validador não rejeite, excluo a tag <cUF>
                        doc.DocumentElement.RemoveChild(consReciNFeElemento.GetElementsByTagName("cUF")[0]);
                        /// Salvo o arquivo modificado
                        doc.Save(cArquivoXML);
                    }
                    if (consReciNFeElemento.GetElementsByTagName("tpEmis").Count != 0)
                    {
                        this.oDadosPedRec.tpEmis = Convert.ToInt16(consReciNFeElemento.GetElementsByTagName("tpEmis")[0].InnerText);
                        /// Para que o validador não rejeite, excluo a tag <tpEmis>
                        doc.DocumentElement.RemoveChild(consReciNFeElemento.GetElementsByTagName("tpEmis")[0]);
                        /// Salvo o arquivo modificado
                        doc.Save(cArquivoXML);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region EnvDPEC()
        /// <summary>
        /// Efetua a leitura do XML de registro do DPEC
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML de registro do DPEC</param>
        public override void EnvDPEC(int emp, string arquivoXML)
        {
            //int emp = Empresa.FindEmpresaThread(Thread.CurrentThread.Name);

            this.dadosEnvDPEC.tpAmb = Empresa.Configuracoes[emp].tpAmb;
            this.dadosEnvDPEC.tpEmis = TipoEmissao.teDPEC;
            this.dadosEnvDPEC.cUF = Empresa.Configuracoes[emp].UFCod;

            ///
            /// danasa 21/10/2010
            /// 
            if (Path.GetExtension(arquivoXML).ToLower() == ".txt")
            {
                switch (ConfiguracaoApp.TipoAplicativo)
                {
                    case UniNFeLibrary.Enums.TipoAplicativo.Cte:
                        break;

                    case UniNFeLibrary.Enums.TipoAplicativo.Nfe:
                        ///cUF|31                   |
                        ///tpAmb|2                  | opcional
                        ///verProc|1.0.0
                        ///CNPJ|10238568000360
                        ///IE|148230665114
                        ///------
                        ///chNFe|31101010238568000360550010000001011000001011
                        ///CNPJCPF|05481336000137   | se UF=EX->Branco
                        ///UF|SP
                        ///vNF|123456.00
                        ///vICMS|18.00
                        ///vST|121.99
                        List<string> cLinhas = new Auxiliar().LerArquivo(arquivoXML);
                        foreach (string cTexto in cLinhas)
                        {
                            string[] dados = cTexto.Split('|');
                            if (dados.GetLength(0) == 1) continue;

                            switch (dados[0].ToLower())
                            {
                                case "tpamb":
                                    this.dadosEnvDPEC.tpAmb = Convert.ToInt32("0" + dados[1].Trim());
                                    break;
                                case "cuf":
                                    this.dadosEnvDPEC.cUF = Convert.ToInt32("0" + dados[1].Trim());
                                    break;
                                case "verproc":
                                    this.dadosEnvDPEC.verProc = dados[1].Trim();
                                    break;
                                case "cnpj":
                                    this.dadosEnvDPEC.CNPJ = (string)Auxiliar.OnlyNumbers(dados[1].Trim());
                                    break;
                                case "ie":
                                    this.dadosEnvDPEC.IE = (string)Auxiliar.OnlyNumbers(dados[1].Trim());
                                    break;
                                case "chnfe":
                                    this.dadosEnvDPEC.chNFe = dados[1].Trim();
                                    break;
                                case "cnpjcpf":
                                    this.dadosEnvDPEC.CNPJCPF = (string)Auxiliar.OnlyNumbers(dados[1].Trim());
                                    break;
                                case "uf":
                                    this.dadosEnvDPEC.UF = dados[1].Trim();
                                    break;
                                case "vicms":
                                    this.dadosEnvDPEC.vICMS = dados[1].Trim();
                                    break;
                                case "vst":
                                    this.dadosEnvDPEC.vST = dados[1].Trim();
                                    break;
                                case "vnf":
                                    this.dadosEnvDPEC.vNF = dados[1].Trim();
                                    break;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(arquivoXML);

                XmlNodeList infDPECList = doc.GetElementsByTagName("infDPEC");

                foreach (XmlNode infDPECNode in infDPECList)
                {
                    XmlElement infDPECElemento = (XmlElement)infDPECNode;

                    this.dadosEnvDPEC.tpAmb = Convert.ToInt32("0" + infDPECElemento.GetElementsByTagName("tpAmb")[0].InnerText);
                    this.dadosEnvDPEC.cUF = Convert.ToInt32("0" + infDPECElemento.GetElementsByTagName("cUF")[0].InnerText);
                }
            }
        }
        #endregion

        #region ConsDPEC()
        public override void ConsDPEC(int emp, string arquivoXML)
        {
            this.dadosConsDPEC.tpAmb = Empresa.Configuracoes[emp].tpAmb;
            this.dadosConsDPEC.tpEmis = TipoEmissao.teDPEC;

            ///
            /// danasa 21/10/2010
            /// 
            if (Path.GetExtension(arquivoXML).ToLower() == ".txt")
            {
                switch (ConfiguracaoApp.TipoAplicativo)
                {
                    case UniNFeLibrary.Enums.TipoAplicativo.Cte:
                        break;

                    case UniNFeLibrary.Enums.TipoAplicativo.Nfe:
                        ///cUF|31                   |
                        ///tpAmb|2                  | opcional
                        ///verProc|1.0.0
                        ///CNPJ|10238568000360
                        ///IE|148230665114
                        ///------
                        ///chNFe|31101010238568000360550010000001011000001011
                        ///CNPJCPF|05481336000137   | se UF=EX->Branco
                        ///UF|SP
                        ///vNF|123456.00
                        ///vICMS|18.00
                        ///vST|121.99
                        List<string> cLinhas = new Auxiliar().LerArquivo(arquivoXML);
                        foreach (string cTexto in cLinhas)
                        {
                            string[] dados = cTexto.Split('|');
                            if (dados.GetLength(0) == 1) continue;

                            switch (dados[0].ToLower())
                            {
                                case "tpamb":
                                    this.dadosConsDPEC.tpAmb = Convert.ToInt32("0" + dados[1].Trim());
                                    break;
                                case "veraplic":
                                    this.dadosConsDPEC.verAplic = dados[1].Trim();
                                    break;
                                case "chnfe":
                                    this.dadosConsDPEC.chNFe = dados[1].Trim();
                                    break;
                                case "nregdpec":
                                    this.dadosConsDPEC.nRegDPEC = dados[1].Trim();
                                    break;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(arquivoXML);

                XmlNodeList consDPECList = doc.GetElementsByTagName("consDPEC");

                foreach (XmlNode consDPECNode in consDPECList)
                {
                    XmlElement consDPECElemento = (XmlElement)consDPECNode;

                    this.dadosConsDPEC.tpAmb = Convert.ToInt32("0" + consDPECElemento.GetElementsByTagName("tpAmb")[0].InnerText);
                }
            }
        }
        #endregion

    }
}