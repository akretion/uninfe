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

namespace UniNFeLibrary
{
    /// <summary>
    /// Classe responsável por ler os diversos XML´s utilizados na nota fiscal eletrônica
    /// e dispor as informações em propriedades para facilitar a leitura.
    /// </summary>
    public abstract class absLerXML
    {
        #region Classes

        public class DadosConsCad
        {
            private string mUF;

            public DadosConsCad()
            {
                this.tpAmb = TipoAmbiente.taProducao;// "1";
            }

            /// <summary>
            /// Unidade Federativa (UF) - Sigla
            /// </summary>
            public string UF
            {
                get
                {
                    return this.mUF;
                }
                set
                {
                    this.mUF = value;
                    this.cUF = 0;// string.Empty;

                    switch (this.mUF.ToUpper().Trim())
                    {
                        case "AC":
                            this.cUF = 12;
                            break;

                        case "AL":
                            this.cUF = 27;
                            break;

                        case "AP":
                            this.cUF = 16;
                            break;

                        case "AM":
                            this.cUF = 13;
                            break;

                        case "BA":
                            this.cUF = 29;
                            break;

                        case "CE":
                            this.cUF = 23;
                            break;

                        case "DF":
                            this.cUF = 53;
                            break;

                        case "ES":
                            this.cUF = 32;
                            break;

                        case "GO":
                            this.cUF = 52;
                            break;

                        case "MA":
                            this.cUF = 21;
                            break;

                        case "MG":
                            this.cUF = 31;
                            break;

                        case "MS":
                            this.cUF = 50;
                            break;

                        case "MT":
                            this.cUF = 51;
                            break;

                        case "PA":
                            this.cUF = 15;
                            break;

                        case "PB":
                            this.cUF = 25;
                            break;

                        case "PE":
                            this.cUF = 26;
                            break;

                        case "PI":
                            this.cUF = 22;
                            break;

                        case "PR":
                            this.cUF = 41;
                            break;

                        case "RJ":
                            this.cUF = 33;
                            break;

                        case "RN":
                            this.cUF = 24;
                            break;

                        case "RO":
                            this.cUF = 11;
                            break;

                        case "RR":
                            this.cUF = 14;
                            break;

                        case "RS":
                            this.cUF = 43;
                            break;

                        case "SC":
                            this.cUF = 42;
                            break;

                        case "SE":
                            this.cUF = 28;
                            break;

                        case "SP":
                            this.cUF = 35;
                            break;

                        case "TO":
                            this.cUF = 17;
                            break;
                    }
                }
            }
            /// <summary>
            /// CPF
            /// </summary>
            public string CPF { get; set; }
            /// <summary>
            /// CNPJ
            /// </summary>
            public string CNPJ { get; set; }
            /// <summary>
            /// Inscrição Estadual
            /// </summary>
            public string IE { get; set; }
            /// <summary>
            /// Unidade Federativa (UF) - Código
            /// </summary>
            public int cUF { get; private set; }
            /// <summary>
            /// Ambiente (2-Homologação 1-Produção)
            /// </summary>
            public int tpAmb { get; private set; }
        }
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta do cadastro do contribuinte
        /// </summary>
        public DadosConsCad oDadosConsCad = new DadosConsCad();

        /// <summary>
        /// Esta classe possui as propriedades que vai receber o conteúdo
        /// do XML da nota fiscal eletrônica
        /// </summary>
        public class DadosNFeClass
        {
            public string chavenfe { get; set; } //Chave da nota fiscal
            public DateTime dEmi { get; set; } //Data de emissão
            public string tpEmis { get; set; } //Tipo de emissão 1-Normal 2-Contigência em papel de segurança 3-Contigência SCAN
            public string tpAmb { get; set; } //Tipo de Ambiente 1-Produção 2-Homologação
            public string idLote { get; set; } //Lote que a NFe faz parte
            public string serie { get; set; } //Série da NFe

            public string cUF { get; set; } //UF do Emitente
            public string cNF { get; set; } //Número randomico da chave da nfe
            public string mod { get; set; } //Modelo da nota fiscal
            public string nNF { get; set; } //Número da nota fiscal
            public string cDV { get; set; } //Dígito verificador da chave da nfe
            public string CNPJ { get; set; } //CNPJ do emitente
        }
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da nota fiscal eletrônica
        /// </summary>
        public DadosNFeClass oDadosNfe = new DadosNFeClass();

        /// <summary>
        /// Esta classe possui as propriedades que vai receber o conteúdo do XML do recibo do lote
        /// </summary>
        public class DadosRecClass
        {
            public string nRec { get; set; } //Recibo do lote de notas fiscais enviado
            public string cStat { get; set; } //Status do Lote
            public int tMed { get; set; } //Tempo médio de resposta em segundos
        }
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do recibo do lote
        /// </summary>
        public DadosRecClass oDadosRec = new DadosRecClass();

        public class DadosPedCanc
        {
            private string mchNFe;

            public int tpAmb { get; set; }
            public int tpEmis { get; set; }
            public int cUF { get; private set; }
            public string chNFe
            {
                get
                {
                    return this.mchNFe;
                }
                set
                {
                    this.mchNFe = value;
                    string serie = this.mchNFe.Substring(22, 3);
                    this.tpEmis = (Convert.ToInt32(serie) >= 900 ? TipoEmissao.teSCAN : this.tpEmis);
                    this.cUF = Convert.ToInt32(this.mchNFe.Substring(0,2));
                }
            }
            public string nProt { get; set; }
            public string xJust { get; set; }

            public DadosPedCanc()
            {
                this.tpEmis = ConfiguracaoApp.tpEmis;
            }
        }
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de cancelamento
        /// </summary>
        public DadosPedCanc oDadosPedCanc = new DadosPedCanc();

        public class DadosPedInut
        {
            private int mSerie;
            public int tpAmb { get; set; }
            public int tpEmis { get; set; }
            public int cUF { get; set; }
            public int ano { get; set; }
            public string CNPJ { get; set; }
            public int mod { get; set; }
            public int serie 
            { 
                get
                {
                    return this.mSerie;
                }
                set
                {
                    this.mSerie = value;
                    this.tpEmis = (value >= 900 ? TipoEmissao.teSCAN : this.tpEmis);
                }
            }
            public int nNFIni { get; set; }
            public int nNFFin { get; set; }
            public string xJust { get; set; }

            public DadosPedInut()
            {
                this.tpEmis = ConfiguracaoApp.tpEmis;
            }
        }
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de inutilizacao
        /// </summary>
        public DadosPedInut oDadosPedInut = new DadosPedInut();

        public class DadosPedSit
        {
            private string mchNFe;

            /// <summary>
            /// Ambiente (2-Homologação ou 1-Produção)
            /// </summary>
            public int tpAmb { get; set; }
            /// <summary>
            /// Chave do documento fiscal
            /// </summary>
            public string chNFe
            {
                get
                {
                    return this.mchNFe;
                }
                set
                {
                    this.mchNFe = value;
                    if (this.mchNFe != string.Empty)
                    {
                        this.cUF = Convert.ToInt32(this.mchNFe.Substring(0, 2));
                        int serie = Convert.ToInt32(this.mchNFe.Substring(22, 3));
                        this.tpEmis = (serie >= 900 ? TipoEmissao.teSCAN : this.tpEmis);
                    }
                }
            }
            /// <summary>
            /// Código da Unidade Federativa (UF)
            /// </summary>
            public int cUF { get; private set; }
            /// <summary>
            /// Série da NFe que está sendo consultada a situação
            /// </summary>
//            public string serie { get; private set; }
            /// <summary>
            /// Tipo de emissão para saber para onde será enviado a consulta da situação da nota
            /// </summary>
            public int tpEmis { get; set; }

            public DadosPedSit()
            {
                this.cUF = 0;
                //this.serie = string.Empty;
                this.tpEmis = TipoEmissao.teNormal;// ConfiguracaoApp.tpEmis;
            }
        }
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta da situação da nota
        /// </summary>
        public DadosPedSit oDadosPedSit = new DadosPedSit();

        public class DadosPedSta
        {
            /// <summary>
            /// Ambiente (2-Homologação ou 1-Produção)
            /// </summary>
            public int tpAmb { get; set; }
            /// <summary>
            /// Código da Unidade Federativa (UF)
            /// </summary>
            public int cUF { get; set; }
            /// <summary>
            /// Tipo de Emissao (1-Normal, 2-Contingencia, 3-SCAN, ...
            /// </summary>
            public int tpEmis { get; set; }
        }
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do status do serviço
        /// </summary>
        public DadosPedSta oDadosPedSta = new DadosPedSta();

        #endregion

        #region Metodos abstratos

        public abstract void ConsCad(string cArquivoXML);
        public abstract void Nfe(string cArquivoXML);
        public abstract void PedCanc(string cArquivoXML);
        public abstract void PedInut(string cArquivoXML);
        public abstract void PedSit(string cArquivoXML);
        public abstract void PedSta(string cArquivoXML);
        public abstract void Recibo(string strXml);

        #endregion
    }
}