using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading;
using NFe.Components;
using NFe.Settings;

namespace NFe.Service
{
    /// <summary>
    /// Classe responsável por ler os diversos XML´s utilizados na nota fiscal eletrônica
    /// e dispor as informações em propriedades para facilitar a leitura.
    /// </summary>
    public class LerXML
    {
        #region Classes

        #region Classe com os Dados do XML da Consulta Cadastro do Contribuinte
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta do cadastro do contribuinte
        /// </summary>
        public DadosConsCad oDadosConsCad = new DadosConsCad();
        #endregion

        #region Classe com os dados do XML da NFe
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da nota fiscal eletrônica
        /// </summary>
        public DadosNFeClass oDadosNfe = new DadosNFeClass();
        #endregion

        #region Classe com os dados do XML do pedido de consulta do recibo do lote de nfe enviado
        /// <summary>
        /// Esta Herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de consulta do recibo do lote de NFe enviado
        /// </summary>
        public DadosPedRecClass oDadosPedRec = new DadosPedRecClass();
        #endregion

        #region Classe com os dados do XML do retorno do envio do Lote de NFe
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do recibo do lote
        /// </summary>
        public DadosRecClass oDadosRec = new DadosRecClass();
        #endregion

        #region Classe com os dados do XML da consulta do pedido de cancelamento
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de cancelamento
        /// </summary>
        public DadosPedCanc oDadosPedCanc = new DadosPedCanc(new FindEmpresaThread(Thread.CurrentThread).Index);
        #endregion

        #region Classe com os dados do XML do pedido de inutilização de números de NF
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de inutilizacao
        /// </summary>
        public DadosPedInut oDadosPedInut = new DadosPedInut(new FindEmpresaThread(Thread.CurrentThread).Index);
        #endregion

        #region Classe com os dados do XML da pedido de consulta da situação da NFe
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta da situação da nota
        /// </summary>
        public DadosPedSit oDadosPedSit = new DadosPedSit();
        #endregion

        #region Classe com os dados do XML da consulta do status do serviço da NFe
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do status do serviço
        /// </summary>
        public DadosPedSta oDadosPedSta = new DadosPedSta();
        #endregion

        #region Classe com os dados do XML de registro do DPEC
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do registro do DPEC
        /// </summary>
        public DadosEnvDPEC dadosEnvDPEC = new DadosEnvDPEC();
        #endregion

        #region Classe com os dados do XML de consulta do registro do DPEC
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do registro do DPEC
        /// </summary>
        public DadosConsDPEC dadosConsDPEC = new DadosConsDPEC();
        #endregion

        #region Classe com os dados do XML do registro de eventos
        public DadosenvEvento oDadosEnvEvento = new DadosenvEvento();
        #endregion

        #region Objetos relacionados a NFS-e

        #region Objeto com os dados do XML da consulta lote rps
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta lote rps
        /// </summary>
        public DadosPedLoteRps oDadosPedLoteRps = new DadosPedLoteRps(new FindEmpresaThread(Thread.CurrentThread).Index);
        #endregion

        #region Objeto com os dados do XML da consulta nfse
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta nfse
        /// </summary>
        public DadosPedSitNfse oDadosPedSitNfse = new DadosPedSitNfse(new FindEmpresaThread(Thread.CurrentThread).Index);
        #endregion

        #region Objeto com os dados do XML da consulta nfse por RPS
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta nfse por rps
        /// </summary>
        public DadosPedSitNfseRps oDadosPedSitNfseRps = new DadosPedSitNfseRps(new FindEmpresaThread(Thread.CurrentThread).Index);
        #endregion

        #region Objeto com os dados do XML de cancelamento de NFS-e
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de cancelamento
        /// </summary>
        public DadosPedCanNfse oDadosPedCanNfse = new DadosPedCanNfse(new FindEmpresaThread(Thread.CurrentThread).Index);
        #endregion

        #region Objeto com os dados do XML de consulta situação do lote rps
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de consulta da situação do lote rps
        /// </summary>
        public DadosPedSitLoteRps oDadosPedSitLoteRps = new DadosPedSitLoteRps(new FindEmpresaThread(Thread.CurrentThread).Index);
        #endregion

        #region Objeto com os dados do XML de lote rps
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do lote rps
        /// </summary>
        public DadosEnvLoteRps oDadosEnvLoteRps = new DadosEnvLoteRps(new FindEmpresaThread(Thread.CurrentThread).Index);
        #endregion

        #endregion

        #endregion

        #region Metodos

        #region ConsCad()
        /// <summary>
        /// Faz a leitura do XML de consulta do cadastro do contribuinte e disponibiliza os valores de algumas tag´s
        /// </summary>
        /// <param name="cArquivoXML">Caminho e nome do arquivo XML da consulta do cadastro do contribuinte a ser lido</param>
        public void ConsCad(string cArquivoXML)
        {
            this.oDadosConsCad.CNPJ = string.Empty;
            this.oDadosConsCad.IE = string.Empty;
            this.oDadosConsCad.UF = string.Empty;

            try
            {
                if (Path.GetExtension(cArquivoXML).ToLower() == ".txt")
                {
                    List<string> cLinhas = Functions.LerArquivo(cArquivoXML);
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
        public void Nfe(string cArquivoXML)
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

                XmlNodeList infNFeList = null;
                switch (Propriedade.TipoAplicativo)
                {
                    case TipoAplicativo.Cte:
                        infNFeList = doc.GetElementsByTagName("infCte");
                        break;

                    case TipoAplicativo.Nfe:
                        infNFeList = doc.GetElementsByTagName("infNFe");
                        break;

                    default:
                        break;
                }

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

                        switch (Propriedade.TipoAplicativo)
                        {
                            case TipoAplicativo.Cte:
                                this.oDadosNfe.dEmi = Convert.ToDateTime(ideElemento.GetElementsByTagName("dhEmi")[0].InnerText);
                                this.oDadosNfe.cNF = ideElemento.GetElementsByTagName("cCT")[0].InnerText;
                                this.oDadosNfe.nNF = ideElemento.GetElementsByTagName("nCT")[0].InnerText;
                                goto default;

                            case TipoAplicativo.Nfe:
                                this.oDadosNfe.dEmi = Convert.ToDateTime(ideElemento.GetElementsByTagName("dEmi")[0].InnerText);
                                this.oDadosNfe.cNF = ideElemento.GetElementsByTagName("cNF")[0].InnerText;
                                this.oDadosNfe.nNF = ideElemento.GetElementsByTagName("nNF")[0].InnerText;
                                goto default;

                            default:
                                this.oDadosNfe.tpEmis = ideElemento.GetElementsByTagName("tpEmis")[0].InnerText;
                                this.oDadosNfe.tpAmb = ideElemento.GetElementsByTagName("tpAmb")[0].InnerText;
                                this.oDadosNfe.serie = ideElemento.GetElementsByTagName("serie")[0].InnerText;
                                this.oDadosNfe.cUF = ideElemento.GetElementsByTagName("cUF")[0].InnerText;
                                this.oDadosNfe.mod = ideElemento.GetElementsByTagName("mod")[0].InnerText;
                                this.oDadosNfe.cDV = ideElemento.GetElementsByTagName("cDV")[0].InnerText;
                                break;
                        }
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
                XmlNodeList enviNFeList = null;

                switch (Propriedade.TipoAplicativo)
                {
                    case TipoAplicativo.Cte:
                        enviNFeList = doc.GetElementsByTagName("enviCTe");
                        break;

                    case TipoAplicativo.Nfe:
                        enviNFeList = doc.GetElementsByTagName("enviNFe");
                        break;

                    default:
                        break;
                }

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
        public void PedCanc(string cArquivoXML)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            this.oDadosPedCanc.tpAmb = Empresa.Configuracoes[emp].tpAmb;
            this.oDadosPedCanc.tpEmis = Empresa.Configuracoes[emp].tpEmis;

            if (Path.GetExtension(cArquivoXML).ToLower() == ".txt")
            {
                //      tpAmb|2
                //      chNFe|35080699999090910270550000000000011234567890
                //      nProt|135080000000001
                //      xJust|Teste do WS de Cancelamento
                //      tpEmis|1                                    <<< opcional >>>
                List<string> cLinhas = Functions.LerArquivo(cArquivoXML);
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

                    switch (Propriedade.TipoAplicativo)
                    {
                        case TipoAplicativo.Cte:
                            if (infCancElemento.GetElementsByTagName("chCTe").Count != 0)
                                this.oDadosPedCanc.chNFe = infCancElemento.GetElementsByTagName("chCTe")[0].InnerText;
                            break;

                        case TipoAplicativo.Nfe:
                            if (infCancElemento.GetElementsByTagName("chNFe").Count != 0)
                                this.oDadosPedCanc.chNFe = infCancElemento.GetElementsByTagName("chNFe")[0].InnerText;
                            break;

                        default:
                            break;
                    }

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
        public void PedInut(string cArquivoXML)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

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
                    List<string> cLinhas = Functions.LerArquivo(cArquivoXML);
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

                    XmlNodeList InutNFeList = null;

                    switch (Propriedade.TipoAplicativo)
                    {
                        case TipoAplicativo.Cte:
                            InutNFeList = doc.GetElementsByTagName("inutCTe");
                            break;

                        case TipoAplicativo.Nfe:
                            InutNFeList = doc.GetElementsByTagName("inutNFe");
                            break;

                        default:
                            break;
                    }

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

                            switch (Propriedade.TipoAplicativo)
                            {
                                case TipoAplicativo.Cte:
                                    if (infInutElemento.GetElementsByTagName("nCTIni")[0] != null)
                                        this.oDadosPedInut.nNFIni = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("nCTIni")[0].InnerText);

                                    if (infInutElemento.GetElementsByTagName("nCTFin")[0] != null)
                                        this.oDadosPedInut.nNFFin = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("nCTFin")[0].InnerText);
                                    break;

                                case TipoAplicativo.Nfe:
                                    if (infInutElemento.GetElementsByTagName("nNFIni")[0] != null)
                                        this.oDadosPedInut.nNFIni = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("nNFIni")[0].InnerText);

                                    if (infInutElemento.GetElementsByTagName("nNFFin")[0] != null)
                                        this.oDadosPedInut.nNFFin = Convert.ToInt32("0" + infInutElemento.GetElementsByTagName("nNFFin")[0].InnerText);
                                    break;

                                default:
                                    break;
                            }

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
        public void PedSit(string cArquivoXML)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            this.oDadosPedSit.tpAmb = Empresa.Configuracoes[emp].tpAmb;// string.Empty;
            this.oDadosPedSit.chNFe = string.Empty;

            try
            {
                if (Path.GetExtension(cArquivoXML).ToLower() == ".txt")
                {
                    switch (Propriedade.TipoAplicativo)
                    {
                        case TipoAplicativo.Cte:
                            break;

                        case TipoAplicativo.Nfe:
                            //      tpAmb|2
                            //      tpEmis|1                <<< opcional >>>
                            //      chNFe|35080600000000000000550000000000010000000000
                            List<string> cLinhas = Functions.LerArquivo(cArquivoXML);
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
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(cArquivoXML);

                    XmlNodeList consSitNFeList = null;

                    switch (Propriedade.TipoAplicativo)
                    {
                        case TipoAplicativo.Cte:
                            consSitNFeList = doc.GetElementsByTagName("consSitCTe");
                            break;

                        case TipoAplicativo.Nfe:
                            consSitNFeList = doc.GetElementsByTagName("consSitNFe");
                            break;

                        default:
                            break;
                    }

                    foreach (XmlNode consSitNFeNode in consSitNFeList)
                    {
                        XmlElement consSitNFeElemento = (XmlElement)consSitNFeNode;

                        this.oDadosPedSit.tpAmb = Convert.ToInt32("0" + consSitNFeElemento.GetElementsByTagName("tpAmb")[0].InnerText);
                        switch (Propriedade.TipoAplicativo)
                        {
                            case TipoAplicativo.Cte:
                                this.oDadosPedSit.chNFe = consSitNFeElemento.GetElementsByTagName("chCTe")[0].InnerText;
                                break;

                            case TipoAplicativo.Nfe:
                                this.oDadosPedSit.chNFe = consSitNFeElemento.GetElementsByTagName("chNFe")[0].InnerText;

                                //Definir a versão do XML para resolver o problema do Estado do Paraná e Goiás que não migrou o banco de dados
                                //da versão 1.0 para a 2.0, sendo assim a consulta situação de notas fiscais tem que ser feita cada uma em seu 
                                //ambiente. Wandrey 23/03/2011
                                if (consSitNFeElemento.GetAttribute("versao") == "1.07" && (this.oDadosPedSit.cUF == 41 || this.oDadosPedSit.cUF == 52))
                                    this.oDadosPedSit.versaoNFe = 1;
                                else
                                {
                                    this.oDadosPedSit.versaoNFe = 2;

                                    bool _temCCe = ConfiguracaoApp.TemCCe(this.oDadosPedSit.chNFe.Substring(0, 2), this.oDadosPedSit.tpAmb, this.oDadosPedSit.tpEmis);

                                    if ((consSitNFeElemento.GetAttribute("versao") == "2.00" && _temCCe) ||
                                        (consSitNFeElemento.GetAttribute("versao") == "2.01" && !_temCCe))
                                    {
                                        //if (_temCCe && oDadosPedSit.cUF != 31)
                                        if (_temCCe)
                                            this.oDadosPedSit.versaoNFe = 201;

                                        //consSitNFeElemento.Attributes["versao"].InnerText = _temCCe && oDadosPedSit.cUF != 31 ? "2.01" : ConfiguracaoApp.VersaoXMLPedSit;
                                        consSitNFeElemento.Attributes["versao"].InnerText = _temCCe ? "2.01" : ConfiguracaoApp.VersaoXMLPedSit;
                                        doc.Save(cArquivoXML);
                                    }
                                    else
                                        if (consSitNFeElemento.GetAttribute("versao") == "2.01")
                                            this.oDadosPedSit.versaoNFe = 201;
                                }
                                break;

                            default:
                                break;
                        }

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
        public void PedSta(string cArquivoXML)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

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
                    switch (Propriedade.TipoAplicativo)
                    {
                        case TipoAplicativo.Cte:
                            break;

                        case TipoAplicativo.Nfe:
                            // tpEmis|1						<<< opcional >>>
                            // tpAmb|1
                            // cUF|35
                            List<string> cLinhas = Functions.LerArquivo(cArquivoXML);
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
                            break;

                        default:
                            break;
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

                    XmlNodeList consStatServList = null;

                    switch (Propriedade.TipoAplicativo)
                    {
                        case TipoAplicativo.Cte:
                            consStatServList = doc.GetElementsByTagName("consStatServCte");
                            break;

                        case TipoAplicativo.Nfe:
                            consStatServList = doc.GetElementsByTagName("consStatServ");
                            break;

                        default:
                            break;
                    }

                    foreach (XmlNode consStatServNode in consStatServList)
                    {
                        XmlElement consStatServElemento = (XmlElement)consStatServNode;

                        this.oDadosPedSta.tpAmb = Convert.ToInt32("0" + consStatServElemento.GetElementsByTagName("tpAmb")[0].InnerText);

                        if (consStatServElemento.GetElementsByTagName("cUF").Count != 0)
                        {
                            this.oDadosPedSta.cUF = Convert.ToInt32("0" + consStatServElemento.GetElementsByTagName("cUF")[0].InnerText);

                            //Se for o UniCTe tem que remover a tag da UF
                            if (Propriedade.TipoAplicativo == TipoAplicativo.Cte)
                            {
                                doc.DocumentElement.RemoveChild(consStatServElemento.GetElementsByTagName("cUF")[0]);
                            }
                        }

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
        public void Recibo(string strXml)
        {
            MemoryStream memoryStream = Functions.StringXmlToStream(strXml);

            this.oDadosRec.cStat = string.Empty;
            this.oDadosRec.nRec = string.Empty;
            this.oDadosRec.tMed = 0;

            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(memoryStream);

                XmlNodeList retEnviNFeList = null;

                switch (Propriedade.TipoAplicativo)
                {
                    case TipoAplicativo.Cte:
                        retEnviNFeList = xml.GetElementsByTagName("retEnviCte");
                        break;

                    case TipoAplicativo.Nfe:
                        retEnviNFeList = xml.GetElementsByTagName("retEnviNFe");
                        break;

                    default:
                        break;
                }


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

        #region PedRec()
        /// <summary>
        /// Faz a leitura do XML de pedido da consulta do recibo do lote de notas enviadas
        /// </summary>
        /// <param name="cArquivoXml">Nome do XML a ser lido</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 16/03/2010
        /// </remarks>
        public void PedRec(string cArquivoXML)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            this.oDadosPedRec.tpAmb = 0;
            this.oDadosPedRec.tpEmis = Empresa.Configuracoes[emp].tpEmis;
            this.oDadosPedRec.cUF = Empresa.Configuracoes[emp].UFCod;
            this.oDadosPedRec.nRec = string.Empty;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                XmlNodeList consReciNFeList = null;

                switch (Propriedade.TipoAplicativo)
                {
                    case TipoAplicativo.Cte:
                        consReciNFeList = doc.GetElementsByTagName("consReciCTe");
                        break;

                    case TipoAplicativo.Nfe:
                        consReciNFeList = doc.GetElementsByTagName("consReciNFe");
                        break;

                    default:
                        break;
                }

                foreach (XmlNode consReciNFeNode in consReciNFeList)
                {
                    XmlElement consReciNFeElemento = (XmlElement)consReciNFeNode;

                    oDadosPedRec.tpAmb = Convert.ToInt32("0" + consReciNFeElemento.GetElementsByTagName("tpAmb")[0].InnerText);
                    oDadosPedRec.nRec = consReciNFeElemento.GetElementsByTagName("nRec")[0].InnerText;
                    oDadosPedRec.cUF = Convert.ToInt32(oDadosPedRec.nRec.Substring(0, 2));

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
        public void EnvDPEC(int emp, string arquivoXML)
        {
            //int emp = Empresa.FindEmpresaThread(Thread.CurrentThread);

            this.dadosEnvDPEC.tpAmb = Empresa.Configuracoes[emp].tpAmb;
            this.dadosEnvDPEC.tpEmis = Propriedade.TipoEmissao.teDPEC;
            this.dadosEnvDPEC.cUF = Empresa.Configuracoes[emp].UFCod;

            ///
            /// danasa 21/10/2010
            /// 
            if (Path.GetExtension(arquivoXML).ToLower() == ".txt")
            {
                switch (Propriedade.TipoAplicativo)
                {
                    case TipoAplicativo.Cte:
                        break;

                    case TipoAplicativo.Nfe:
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
                        List<string> cLinhas = Functions.LerArquivo(arquivoXML);
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
                                    this.dadosEnvDPEC.CNPJ = (string)Functions.OnlyNumbers(dados[1].Trim());
                                    break;
                                case "ie":
                                    this.dadosEnvDPEC.IE = (string)Functions.OnlyNumbers(dados[1].Trim());
                                    break;
                                case "chnfe":
                                    this.dadosEnvDPEC.chNFe = dados[1].Trim();
                                    break;
                                case "cnpjcpf":
                                    this.dadosEnvDPEC.CNPJCPF = (string)Functions.OnlyNumbers(dados[1].Trim());
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
        public void ConsDPEC(int emp, string arquivoXML)
        {
            this.dadosConsDPEC.tpAmb = Empresa.Configuracoes[emp].tpAmb;
            this.dadosConsDPEC.tpEmis = Propriedade.TipoEmissao.teDPEC;

            ///
            /// danasa 21/10/2010
            /// 
            if (Path.GetExtension(arquivoXML).ToLower() == ".txt")
            {
                switch (Propriedade.TipoAplicativo)
                {
                    case TipoAplicativo.Cte:
                        break;

                    case TipoAplicativo.Nfe:
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
                        List<string> cLinhas = Functions.LerArquivo(arquivoXML);
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

        #region EnvEvento()
        public void EnvEvento(int emp, string arquivoXML)
        {
            ///
            /// danasa 6/2011
            /// 
            if (Path.GetExtension(arquivoXML).ToLower() == ".txt")
            {
                switch (Propriedade.TipoAplicativo)
                {
                    case TipoAplicativo.Cte:
                        break;

                    case TipoAplicativo.Nfe:
                        ///idLote|000000000015255
                        ///evento|1
                        ///Id|ID1101103511031029073900013955001000000001105112804101                    <<opcional
                        ///cOrgao|35
                        ///tpAmb|2
                        ///CNPJ|10290739000139 
                        ///    ou
                        ///CPF|80531385800
                        ///chNFe|35110310290739000139550010000000011051128041
                        ///dhEvento|2011-03-03T08:06:00
                        ///tpEvento|110110
                        ///nSeqEvento|1
                        ///verEvento|1.00
                        ///descEvento|Carta de Correção                                                 <<opcional
                        ///xCorrecao|Texto de teste para Carta de Correção. Conteúdo do campo xCorrecao.
                        ///xCondUso|A Carta de Correção é disciplinada pelo § 1º-A do art. ..........   <<opcional
                        ///evento|2
                        ///Id|ID1101103511031029073900013955001000000001105112804102
                        ///...
                        ///evento|20    <<MAXIMO
                        ///Id|ID1101103511031029073900013955001000000001105112804103
                        ///...
                        List<string> cLinhas = Functions.LerArquivo(arquivoXML);
                        foreach (string cTexto in cLinhas)
                        {
                            string[] dados = cTexto.Split('|');
                            if (dados.GetLength(0) == 1) continue;

                            switch (dados[0].ToLower())
                            {
                                case "idlote":
                                    this.oDadosEnvEvento.idLote = dados[1].Trim();
                                    break;
                                case "evento":
                                    this.oDadosEnvEvento.eventos.Add(new Evento());
                                    break;
                                case "id":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].Id = dados[1].Trim();
                                    break;
                                case "corgao":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].cOrgao = Convert.ToInt32("0" + dados[1].Trim());
                                    break;
                                case "tpamb":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].tpAmb = Convert.ToInt32("0" + dados[1].Trim());
                                    break;
                                case "cnpj":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].CNPJ = dados[1].Trim();
                                    break;
                                case "cpf":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].CPF = dados[1].Trim();
                                    break;
                                case "chnfe":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].chNFe = dados[1].Trim();
                                    break;
                                case "dhevento":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].dhEvento = dados[1].Trim();
                                    break;
                                case "tpevento":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].tpEvento = dados[1].Trim();
                                    break;
                                case "nseqevento":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].nSeqEvento = Convert.ToInt32("0" + dados[1].Trim());
                                    break;
                                case "verevento":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].verEvento = dados[1].Trim();
                                    break;
                                case "descevento":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].descEvento = dados[1].Trim();
                                    break;
                                case "xcorrecao":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].xCorrecao = dados[1].Trim();
                                    break;
                                case "xconduso":
                                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].xCondUso = dados[1].Trim();
                                    break;
                            }
                        }
                        foreach (Evento evento in this.oDadosEnvEvento.eventos)
                        {
                            if (string.IsNullOrEmpty(evento.xCondUso))
                                evento.xCondUso = "A Carta de Correcao e disciplinada pelo paragrafo 1o-A do art. 7o do Convenio S/N, " +
                                    "de 15 de dezembro de 1970 e pode ser utilizada para regularizacao de erro ocorrido na emissao de " +
                                    "documento fiscal, desde que o erro nao esteja relacionado com: I - as variaveis que determinam o " +
                                    "valor do imposto tais como: base de calculo, aliquota, diferenca de preco, quantidade, valor da " +
                                    "operacao ou da prestacao; II - a correcao de dados cadastrais que implique mudanca do remetente " +
                                    "ou do destinatario; III - a data de emissao ou de saida.";
                            if (string.IsNullOrEmpty(evento.descEvento))
                                evento.descEvento = "Carta de Correcao";
                            if (string.IsNullOrEmpty(evento.Id))
                                evento.Id = "ID" + evento.tpEvento + evento.chNFe + evento.nSeqEvento.ToString("00");
                        }
                        break;

                    default:
                        break;
                }
            }
            else
            {
                //<?xml version="1.0" encoding="UTF-8"?>
                //<envEvento versao="1.00" xmlns="http://www.portalfiscal.inf.br/nfe">
                //  <idLote>000000000015255</idLote>
                //  <evento versao="1.00" xmlns="http://www.portalfiscal.inf.br/nfe">
                //    <infEvento Id="ID1101103511031029073900013955001000000001105112804108">
                //      <cOrgao>35</cOrgao>
                //      <tpAmb>2</tpAmb>
                //      <CNPJ>10290739000139</CNPJ>
                //      <chNFe>35110310290739000139550010000000011051128041</chNFe>
                //      <dhEvento>2011-03-03T08:06:00-03:00</dhEvento>
                //      <tpEvento>110110</tpEvento>
                //      <nSeqEvento>8</nSeqEvento>
                //      <verEvento>1.00</verEvento>
                //      <detEvento versao="1.00">
                //          <descEvento>Carta de Correção</descEvento>
                //          <xCorrecao>Texto de teste para Carta de Correção. Conteúdo do campo xCorrecao.</xCorrecao>
                //          <xCondUso>A Carta de Correção é disciplinada pelo § 1º-A do art. 7º do Convênio S/N, de 15 de dezembro de 1970 e pode ser utilizada para regularização de erro ocorrido na emissão de documento fiscal, desde que o erro não esteja relacionado com: I - as variáveis que determinam o valor do imposto tais como: base de cálculo, alíquota, diferença de preço, quantidade, valor da operação ou da prestação; II - a correção de dados cadastrais que implique mudança do remetente ou do destinatário; III - a data de emissão ou de saída.</xCondUso>
                //      </detEvento>
                //    </infEvento>
                //  </evento>
                //</envEvento>

                XmlDocument doc = new XmlDocument();
                doc.Load(arquivoXML);

                XmlNodeList envEventoList = doc.GetElementsByTagName("infEvento");

                foreach (XmlNode envEventoNode in envEventoList)
                {
                    XmlElement envEventoElemento = (XmlElement)envEventoNode;

                    this.oDadosEnvEvento.eventos.Add(new Evento());
                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].tpAmb = Convert.ToInt32("0" + envEventoElemento.GetElementsByTagName("tpAmb")[0].InnerText);
                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].cOrgao = Convert.ToInt32("0" + envEventoElemento.GetElementsByTagName("cOrgao")[0].InnerText);
                    this.oDadosEnvEvento.eventos[this.oDadosEnvEvento.eventos.Count - 1].chNFe = envEventoElemento.GetElementsByTagName("chNFe")[0].InnerText;
                }
            }
        }
        #endregion

        #endregion

        #region Métodos para leitura dos XML´s da NFS-e (Nota Fiscal de Serviços Eletrônica)

        #region PedLoteRps()
        /// <summary>
        /// Fazer a leitura do conteúdo do XML de consulta lote rps e disponibilizar conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        public void PedLoteRps(string arquivoXML)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            XmlDocument doc = new XmlDocument();
            doc.Load(arquivoXML);

            XmlNodeList infConsList = doc.GetElementsByTagName("ConsultarLoteRpsEnvio");

            foreach (XmlNode infConsNode in infConsList)
            {
                XmlElement infConsElemento = (XmlElement)infConsNode;
                oDadosPedLoteRps.Protocolo = infConsElemento.GetElementsByTagName("Protocolo")[0].InnerText;

                XmlElement infPrestadorElemento = (XmlElement)infConsElemento.GetElementsByTagName("Prestador").Item(0);
                if (infPrestadorElemento.GetElementsByTagName("tipos:Cnpj")[0] != null)
                    oDadosPedLoteRps.Cnpj = infPrestadorElemento.GetElementsByTagName("tipos:Cnpj")[0].InnerText;
                else if (infPrestadorElemento.GetElementsByTagName("Cnpj")[0] != null)
                    oDadosPedLoteRps.Cnpj = infPrestadorElemento.GetElementsByTagName("Cnpj")[0].InnerText;

                if (infPrestadorElemento.GetElementsByTagName("tipos:InscricaoMunicipal")[0] != null)
                    oDadosPedLoteRps.InscricaoMunicipal = infPrestadorElemento.GetElementsByTagName("tipos:InscricaoMunicipal")[0].InnerText;
                else if (infPrestadorElemento.GetElementsByTagName("InscricaoMunicipal")[0] != null)
                    oDadosPedLoteRps.InscricaoMunicipal = infPrestadorElemento.GetElementsByTagName("InscricaoMunicipal")[0].InnerText;
            }
        }
        #endregion

        #region PedCanNfse()
        /// <summary>
        /// Fazer a leitura do conteúdo do XML de cancelamento de NFS-e e disponibilizar conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        public void PedCanNfse(string arquivoXML)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            XmlDocument doc = new XmlDocument();
            doc.Load(arquivoXML);

            XmlNodeList infCancList = doc.GetElementsByTagName("CancelarNfseEnvio");

            foreach (XmlNode infCancNode in infCancList)
            {
                XmlElement infCancElemento = (XmlElement)infCancNode;
            }
        }
        #endregion

        #region PedSitLoteRps()
        /// <summary>
        /// Fazer a leitura do conteúdo do XML de consulta situação do lote rps e disponibilizar conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        public void PedSitLoteRps(string arquivoXML)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            XmlDocument doc = new XmlDocument();
            doc.Load(arquivoXML);

            XmlNodeList infConsList = doc.GetElementsByTagName("ConsultarSituacaoLoteRpsEnvio");

            foreach (XmlNode infConsNode in infConsList)
            {
                XmlElement infConsElemento = (XmlElement)infConsNode;
            }
        }
        #endregion

        #region PedSitNfse()
        /// <summary>
        /// Fazer a leitura do conteúdo do XML de consulta nfse por numero e disponibiliza conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        public void PedSitNfse(string arquivoXML)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
        }
        #endregion

        #region PedSitNfseRps()
        /// <summary>
        /// Fazer a leitura do conteúdo do XML de consulta nfse por rps e disponibiliza conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        public void PedSitNfseRps(string arquivoXML)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
        }
        #endregion

        #region EnvLoteRps()
        /// <summary>
        /// Fazer a leitura do conteúdo do XML de lote rps e disponibiliza o conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        public void EnvLoteRps(string arquivoXML)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            XmlDocument doc = new XmlDocument();
            doc.Load(arquivoXML);

            XmlNodeList infEnvioList = doc.GetElementsByTagName("EnviarLoteRpsEnvio");

            foreach (XmlNode infEnvioNode in infEnvioList)
            {
                XmlElement infEnvioElemento = (XmlElement)infEnvioNode;
            }
        }
        #endregion

        #endregion
    }
}