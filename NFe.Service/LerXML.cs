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

        #region Classe com os dados do XML da NFe
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da nota fiscal eletrônica
        /// </summary>
        public DadosNFeClass oDadosNfe = new DadosNFeClass();
        #endregion

        #region Objetos relacionados a NFS-e

        #region Objeto com os dados do XML da consulta lote rps
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta lote rps
        /// </summary>
        public DadosPedLoteRps oDadosPedLoteRps = new DadosPedLoteRps(Functions.FindEmpresaByThread());
        #endregion

        #region Objeto com os dados do XML da consulta nfse por RPS
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta nfse por rps
        /// </summary>
        public DadosPedSitNfseRps oDadosPedSitNfseRps = new DadosPedSitNfseRps(Functions.FindEmpresaByThread());
        #endregion

        #endregion

        #endregion

        #region Metodos

        #region Nfe()
        /// <summary>
        /// Faz a leitura do XML da nota fiscal eletrônica e disponibiliza os valores de algumas tag´s
        /// </summary>
        /// <param name="arquivoXML">Caminho e nome do arquivo XML da NFe a ser lido</param>
        public void Nfe(string arquivoXML)
        {
            ClearDados();

            XmlDocument doc = new XmlDocument();
            doc.Load(arquivoXML);

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

                    oDadosNfe.dEmi = Convert.ToDateTime(ideElemento.GetElementsByTagName("dEmi")[0].InnerText);
                    oDadosNfe.cNF = ideElemento.GetElementsByTagName("cNF")[0].InnerText;
                    oDadosNfe.nNF = ideElemento.GetElementsByTagName("nNF")[0].InnerText;
                    oDadosNfe.tpEmis = ideElemento.GetElementsByTagName("tpEmis")[0].InnerText;
                    oDadosNfe.tpAmb = ideElemento.GetElementsByTagName("tpAmb")[0].InnerText;
                    oDadosNfe.serie = ideElemento.GetElementsByTagName("serie")[0].InnerText;
                    oDadosNfe.cUF = ideElemento.GetElementsByTagName("cUF")[0].InnerText;
                    oDadosNfe.mod = ideElemento.GetElementsByTagName("mod")[0].InnerText;
                    oDadosNfe.cDV = ideElemento.GetElementsByTagName("cDV")[0].InnerText;
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
        #endregion

        #region Cte()
        /// <summary>
        /// Faz a leitura do XML da nota fiscal eletrônica e disponibiliza os valores de algumas tag´s
        /// </summary>
        /// <param name="arquivoXML">Caminho e nome do arquivo XML da NFe a ser lido</param>
        public void Cte(string arquivoXML)
        {
            ClearDados();

            XmlDocument doc = new XmlDocument();
            doc.Load(arquivoXML);

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
                    this.oDadosNfe.cNF = ideElemento.GetElementsByTagName("cCT")[0].InnerText;
                    this.oDadosNfe.nNF = ideElemento.GetElementsByTagName("nCT")[0].InnerText;
                    this.oDadosNfe.tpEmis = ideElemento.GetElementsByTagName("tpEmis")[0].InnerText;
                    this.oDadosNfe.tpAmb = ideElemento.GetElementsByTagName("tpAmb")[0].InnerText;
                    this.oDadosNfe.serie = ideElemento.GetElementsByTagName("serie")[0].InnerText;
                    this.oDadosNfe.cUF = ideElemento.GetElementsByTagName("cUF")[0].InnerText;
                    this.oDadosNfe.mod = ideElemento.GetElementsByTagName("mod")[0].InnerText;
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
        #endregion

        #region Mdfe()
        /// <summary>
        /// Faz a leitura do XML da nota fiscal eletrônica e disponibiliza os valores de algumas tag´s
        /// </summary>
        /// <param name="arquivoXML">Caminho e nome do arquivo XML da NFe a ser lido</param>
        public void Mdfe(string arquivoXML)
        {
            ClearDados();

            XmlDocument doc = new XmlDocument();
            doc.Load(arquivoXML);

            XmlNodeList infNFeList = doc.GetElementsByTagName("infMDFe");

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
                    this.oDadosNfe.cNF = ideElemento.GetElementsByTagName("cMDF")[0].InnerText;
                    this.oDadosNfe.nNF = ideElemento.GetElementsByTagName("nMDF")[0].InnerText;
                    this.oDadosNfe.tpEmis = ideElemento.GetElementsByTagName("tpEmis")[0].InnerText;
                    this.oDadosNfe.tpAmb = ideElemento.GetElementsByTagName("tpAmb")[0].InnerText;
                    this.oDadosNfe.serie = ideElemento.GetElementsByTagName("serie")[0].InnerText;
                    this.oDadosNfe.cUF = ideElemento.GetElementsByTagName("cUF")[0].InnerText;
                    this.oDadosNfe.mod = ideElemento.GetElementsByTagName("mod")[0].InnerText;
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
            XmlNodeList enviNFeList = doc.GetElementsByTagName("enviMDFe");

            foreach (XmlNode enviNFeNode in enviNFeList)
            {
                XmlElement enviNFeElemento = (XmlElement)enviNFeNode;
                this.oDadosNfe.idLote = enviNFeElemento.GetElementsByTagName("idLote")[0].InnerText;
            }
        }
        #endregion

        #region ClearDados()
        /// <summary>
        /// Limpa conteúdo das propriedades do objeto oDadosNFe
        /// </summary>
        private void ClearDados()
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
            int emp = Functions.FindEmpresaByThread();

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


        #region PedSitNfseRps()
        /// <summary>
        /// Fazer a leitura do conteúdo do XML de consulta nfse por rps e disponibiliza conteúdo em um objeto para analise
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML que é para efetuar a leitura</param>
        public void PedSitNfseRps(string arquivoXML)
        {
            int emp = Functions.FindEmpresaByThread();
        }
        #endregion

        #endregion
    }
}