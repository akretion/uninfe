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
        public DadosPedLoteRps oDadosPedLoteRps = new DadosPedLoteRps(Empresas.FindEmpresaByThread());
        #endregion

        #region Objeto com os dados do XML da consulta nfse por RPS
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta nfse por rps
        /// </summary>
        public DadosPedSitNfseRps oDadosPedSitNfseRps = new DadosPedSitNfseRps(Empresas.FindEmpresaByThread());
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
                    this.oDadosNfe.chavenfe = infNFeElemento.Attributes[TpcnResources.Id.ToString()].InnerText;
                    oDadosNfe.versao = infNFeElemento.Attributes[NFe.Components.TpcnResources.versao.ToString()].InnerText;
                }

                //Montar lista de tag´s da tag <ide>
                XmlNodeList ideList = infNFeElemento.GetElementsByTagName("ide");

                foreach (XmlNode ideNode in ideList)
                {
                    XmlElement ideElemento = (XmlElement)ideNode;

                    if (oDadosNfe.versao == "2.00")
                        oDadosNfe.dEmi = Convert.ToDateTime(Functions.LerTag(ideElemento, NFe.Components.TpcnResources.dEmi.ToString(), false));
                    else
                        oDadosNfe.dEmi = Convert.ToDateTime(Functions.LerTag(ideElemento, TpcnResources.dhEmi.ToString(), false));
                    oDadosNfe.cNF = Functions.LerTag(ideElemento, TpcnResources.cNF.ToString(), false);
                    oDadosNfe.nNF = Functions.LerTag(ideElemento, TpcnResources.nNF.ToString(), false);
                    oDadosNfe.tpEmis = Functions.LerTag(ideElemento, NFe.Components.TpcnResources.tpEmis.ToString(), false);
                    oDadosNfe.tpAmb = Functions.LerTag(ideElemento, TpcnResources.tpAmb.ToString(), false);
                    oDadosNfe.serie = Functions.LerTag(ideElemento, TpcnResources.serie.ToString(), false);
                    oDadosNfe.cUF = Functions.LerTag(ideElemento, NFe.Components.TpcnResources.cUF.ToString(), false);
                    oDadosNfe.mod = Functions.LerTag(ideElemento, TpcnResources.mod.ToString(), false);
                    oDadosNfe.cDV = Functions.LerTag(ideElemento, TpcnResources.cDV.ToString(), false);
                }

                //Montar lista de tag´s da tag <emit>
                XmlNodeList emitList = infNFeElemento.GetElementsByTagName("emit");

                foreach (XmlNode emitNode in emitList)
                {
                    XmlElement emitElemento = (XmlElement)emitNode;

                    this.oDadosNfe.CNPJ = Functions.LerTag(emitElemento, NFe.Components.TpcnResources.CNPJ.ToString(), false);
                }
            }

            //Tentar detectar a tag de lote, se tiver vai atualizar o atributo do lote que a nota faz parte
            XmlNodeList enviNFeList = doc.GetElementsByTagName("enviNFe");

            foreach (XmlNode enviNFeNode in enviNFeList)
            {
                XmlElement enviNFeElemento = (XmlElement)enviNFeNode;

                oDadosNfe.idLote = Functions.LerTag(enviNFeElemento, TpcnResources.idLote.ToString(), false);

                switch (Functions.LerTag(enviNFeElemento, TpcnResources.indSinc.ToString(), false))
                {
                    case "1": //Processo síncrono
                        oDadosNfe.indSinc = true;
                        break;

                    default:
                        oDadosNfe.indSinc = false;
                        break;
                }
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
                    this.oDadosNfe.chavenfe = infNFeElemento.Attributes[TpcnResources.Id.ToString()].InnerText;
                }

                //Montar lista de tag´s da tag <ide>
                XmlNodeList ideList = infNFeElemento.GetElementsByTagName("ide");

                foreach (XmlNode ideNode in ideList)
                {
                    XmlElement ideElemento = (XmlElement)ideNode;

                    this.oDadosNfe.dEmi = Convert.ToDateTime(Functions.LerTag(ideElemento, TpcnResources.dhEmi.ToString(), false));
                    this.oDadosNfe.cNF = Functions.LerTag(ideElemento, TpcnResources.cCT.ToString(), false);
                    this.oDadosNfe.nNF = Functions.LerTag(ideElemento, TpcnResources.nCT.ToString(), false);
                    this.oDadosNfe.tpEmis = Functions.LerTag(ideElemento, TpcnResources.tpEmis.ToString(), false);
                    this.oDadosNfe.tpAmb = Functions.LerTag(ideElemento, TpcnResources.tpAmb.ToString(), false);
                    this.oDadosNfe.serie = Functions.LerTag(ideElemento, TpcnResources.serie.ToString(), false);
                    this.oDadosNfe.cUF = Functions.LerTag(ideElemento, TpcnResources.cUF.ToString(), false);
                    this.oDadosNfe.mod = Functions.LerTag(ideElemento, TpcnResources.mod.ToString(), false);
                    this.oDadosNfe.cDV = Functions.LerTag(ideElemento, TpcnResources.cDV.ToString(), false);
                }

                //Montar lista de tag´s da tag <emit>
                XmlNodeList emitList = infNFeElemento.GetElementsByTagName("emit");

                foreach (XmlNode emitNode in emitList)
                {
                    XmlElement emitElemento = (XmlElement)emitNode;

                    this.oDadosNfe.CNPJ = Functions.LerTag(emitElemento, NFe.Components.TpcnResources.CNPJ.ToString(), false);
                }
            }

            //Tentar detectar a tag de lote, se tiver vai atualizar o atributo do lote que a nota faz parte
            XmlNodeList enviNFeList = doc.GetElementsByTagName("enviCTe");

            foreach (XmlNode enviNFeNode in enviNFeList)
            {
                XmlElement enviNFeElemento = (XmlElement)enviNFeNode;
                this.oDadosNfe.idLote = Functions.LerTag(enviNFeElemento, TpcnResources.idLote.ToString(), false);
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
                    this.oDadosNfe.chavenfe = infNFeElemento.Attributes[TpcnResources.Id.ToString()].InnerText;
                }

                //Montar lista de tag´s da tag <ide>
                XmlNodeList ideList = infNFeElemento.GetElementsByTagName("ide");

                foreach (XmlNode ideNode in ideList)
                {
                    XmlElement ideElemento = (XmlElement)ideNode;

                    this.oDadosNfe.dEmi = Convert.ToDateTime(Functions.LerTag(ideElemento, TpcnResources.dhEmi.ToString(), false));
                    this.oDadosNfe.cNF = Functions.LerTag(ideElemento, TpcnResources.cMDF.ToString(), false);
                    this.oDadosNfe.nNF = Functions.LerTag(ideElemento, TpcnResources.nMDF.ToString(), false);
                    this.oDadosNfe.tpEmis = Functions.LerTag(ideElemento, TpcnResources.tpEmis.ToString(), false);
                    this.oDadosNfe.tpAmb = Functions.LerTag(ideElemento, TpcnResources.tpAmb.ToString(), false);
                    this.oDadosNfe.serie = Functions.LerTag(ideElemento, TpcnResources.serie.ToString(), false);
                    this.oDadosNfe.cUF = Functions.LerTag(ideElemento, TpcnResources.cUF.ToString(), false);
                    this.oDadosNfe.mod = Functions.LerTag(ideElemento, TpcnResources.mod.ToString(), false);
                    this.oDadosNfe.cDV = Functions.LerTag(ideElemento, TpcnResources.cDV.ToString(), false);
                }

                //Montar lista de tag´s da tag <emit>
                XmlNodeList emitList = infNFeElemento.GetElementsByTagName("emit");

                foreach (XmlNode emitNode in emitList)
                {
                    XmlElement emitElemento = (XmlElement)emitNode;

                    this.oDadosNfe.CNPJ = Functions.LerTag(emitElemento, NFe.Components.TpcnResources.CNPJ.ToString(), false);
                }
            }

            //Tentar detectar a tag de lote, se tiver vai atualizar o atributo do lote que a nota faz parte
            XmlNodeList enviNFeList = doc.GetElementsByTagName("enviMDFe");

            foreach (XmlNode enviNFeNode in enviNFeList)
            {
                XmlElement enviNFeElemento = (XmlElement)enviNFeNode;
                this.oDadosNfe.idLote = Functions.LerTag(enviNFeElemento, TpcnResources.idLote.ToString(), false);
            }
        }
        #endregion

        #region ClearDados()
        /// <summary>
        /// Limpa conteúdo das propriedades do objeto oDadosNFe
        /// </summary>
        private void ClearDados()
        {
            oDadosNfe.chavenfe = string.Empty;
            oDadosNfe.idLote = string.Empty;
            oDadosNfe.tpAmb = string.Empty;
            oDadosNfe.tpEmis = string.Empty;
            oDadosNfe.serie = string.Empty;
            oDadosNfe.cUF = string.Empty;
            oDadosNfe.cNF = string.Empty;
            oDadosNfe.mod = string.Empty;
            oDadosNfe.nNF = string.Empty;
            oDadosNfe.cDV = string.Empty;
            oDadosNfe.CNPJ = string.Empty;
            oDadosNfe.versao = string.Empty;
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
            //int emp = Empresas.FindEmpresaByThread();

            XmlDocument doc = new XmlDocument();
            doc.Load(arquivoXML);

            XmlNodeList infConsList = doc.GetElementsByTagName("ConsultarLoteRpsEnvio");

            foreach (XmlNode infConsNode in infConsList)
            {
                XmlElement infConsElemento = (XmlElement)infConsNode;
                oDadosPedLoteRps.Protocolo = Functions.LerTag(infConsElemento, "Protocolo", false);

                XmlElement infPrestadorElemento = (XmlElement)infConsElemento.GetElementsByTagName("Prestador").Item(0);
                if (infPrestadorElemento.GetElementsByTagName("tipos:Cnpj")[0] != null)
                    oDadosPedLoteRps.Cnpj = Functions.LerTag(infPrestadorElemento, "tipos:Cnpj", false);
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
            //int emp = Empresas.FindEmpresaByThread();
        }
        #endregion

        #endregion
    }
}