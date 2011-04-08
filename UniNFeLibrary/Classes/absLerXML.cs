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
using System.Threading;

namespace UniNFeLibrary
{
    /// <summary>
    /// Classe responsável por ler os diversos XML´s utilizados na nota fiscal eletrônica
    /// e dispor as informações em propriedades para facilitar a leitura.
    /// </summary>
    public abstract class absLerXML
    {
        #region Classes

        #region Classe com os Dados do XML da Consulta Cadastro do Contribuinte
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
        #endregion

        #region Classe com os dados do XML da NFe
        /// <summary>
        /// Esta classe possui as propriedades que vai receber o conteúdo
        /// do XML da nota fiscal eletrônica
        /// </summary>
        public class DadosNFeClass
        {
            /// <summary>
            /// Chave da nota fisca
            /// </summary>
            public string chavenfe { get; set; }
            /// <summary>
            /// Data de emissão
            /// </summary>
            public DateTime dEmi { get; set; }
            /// <summary>
            /// Tipo de emissão 1-Normal 2-Contigência em papel de segurança 3-Contigência SCAN
            /// </summary>
            public string tpEmis { get; set; }
            /// <summary>
            /// Tipo de Ambiente 1-Produção 2-Homologação
            /// </summary>
            public string tpAmb { get; set; }
            /// <summary>
            /// Lote que a NFe faz parte
            /// </summary>
            public string idLote { get; set; } 
            /// <summary>
            /// Série da NFe
            /// </summary>
            public string serie { get; set; } 
            /// <summary>
            /// UF do Emitente
            /// </summary>
            public string cUF { get; set; } 
            /// <summary>
            /// Número randomico da chave da nfe
            /// </summary>
            public string cNF { get; set; } 
            /// <summary>
            /// Modelo da nota fiscal
            /// </summary>
            public string mod { get; set; } 
            /// <summary>
            /// Número da nota fiscal
            /// </summary>
            public string nNF { get; set; } 
            /// <summary>
            /// Dígito verificador da chave da nfe
            /// </summary>
            public string cDV { get; set; } 
            /// <summary>
            /// CNPJ do emitente
            /// </summary>
            public string CNPJ { get; set; } 
        }
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da nota fiscal eletrônica
        /// </summary>
        public DadosNFeClass oDadosNfe = new DadosNFeClass();
        #endregion

        #region Classe com os dados do XML do pedido de consulta do recibo do lote de nfe enviado
        /// <summary>
        /// Classe com os dados do XML do pedido de consulta do recibo do lote de nfe enviado
        /// </summary>
        public class DadosPedRecClass
        {
            /// <summary>
            /// Tipo de ambiente: 1-Produção 2-Homologação
            /// </summary>
            public int tpAmb { get; set; }
            /// <summary>
            /// Número do recibo do lote de NFe enviado
            /// </summary>
            public string nRec { get; set; }
            /// <summary>
            /// Tipo de Emissão: 1-Normal 2-Contingência FS 3-Contingência SCAN 4-Contingência DEPEC 5-Contingência FS-DA
            /// </summary>
            public int tpEmis { get; set; }
            /// <summary>
            /// Código da Unidade Federativa (UF)
            /// </summary>
            public int cUF { get; set; }
        }
        /// <summary>
        /// Esta Herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de consulta do recibo do lote de NFe enviado
        /// </summary>
        public DadosPedRecClass oDadosPedRec = new DadosPedRecClass();
        #endregion

        #region Classe com os dados do XML do retorno do envio do Lote de NFe
        /// <summary>
        /// Esta classe possui as propriedades que vai receber o conteúdo do XML do recibo do lote
        /// </summary>
        public class DadosRecClass
        {
            /// <summary>
            /// Recibo do lote de notas fiscais enviado
            /// </summary>
            public string nRec { get; set; } 
            /// <summary>
            /// Status do Lote
            /// </summary>
            public string cStat { get; set; } 
            /// <summary>
            /// Tempo médio de resposta em segundos
            /// </summary>
            public int tMed { get; set; } 
        }
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do recibo do lote
        /// </summary>
        public DadosRecClass oDadosRec = new DadosRecClass();
        #endregion

        #region Classe com os dados do XML da consulta do pedido de cancelamento
        /// <summary>
        /// Classe com os dados do XML da consulta do pedido de cancelamento
        /// </summary>
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
                int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
                this.tpEmis = Empresa.Configuracoes[emp].tpEmis;
            }
        }
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de cancelamento
        /// </summary>
        public DadosPedCanc oDadosPedCanc = new DadosPedCanc();
        #endregion

        #region Classe com os dados do XML do pedido de inutilização de números de NF
        /// <summary>
        /// Classe com os dados do XML do pedido de inutilização de números de NF
        /// </summary>
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
                int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
                this.tpEmis = Empresa.Configuracoes[emp].tpEmis;
            }
        }
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de inutilizacao
        /// </summary>
        public DadosPedInut oDadosPedInut = new DadosPedInut();
        #endregion

        #region Classe com os dados do XML da pedido de consulta da situação da NFe
        /// <summary>
        /// Classe com os dados do XML da pedido de consulta da situação da NFe
        /// </summary>
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
        #endregion

        #region Classe com os dados do XML da consulta do status do serviço da NFe
        /// <summary>
        /// Classe com os dados do XML da consulta do status do serviço da NFe
        /// </summary>
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

        #region Classe com os dados do XML de registro do DPEC
        /// <summary>
        /// Classe com os dados do XML de registro do DPEC
        /// </summary>
        public class DadosEnvDPEC
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

            public string CNPJ { get; set; }
            public string IE { get; set; }
            public string verProc { get; set; }
            public string chNFe { get; set; }
            public string CNPJCPF { get; set; }
            public string UF { get; set; }
            public string vNF { get; set; }
            public string vICMS { get; set; }
            public string vST { get; set; }
        }
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do registro do DPEC
        /// </summary>
        public DadosEnvDPEC dadosEnvDPEC = new DadosEnvDPEC();
        #endregion

        #region Classe com os dados do XML de consulta do registro do DPEC
        /// <summary>
        /// Classe com os dados do XML de registro do DPEC
        /// </summary>
        public class DadosConsDPEC
        {
            /// <summary>
            /// Ambiente (2-Homologação ou 1-Produção)
            /// </summary>
            public int tpAmb { get; set; }
            /// <summary>
            /// Código da Unidade Federativa (UF)
            /// </summary>
            //public int cUF { get; set; }
            /// <summary>
            /// Tipo de Emissao (1-Normal, 2-Contingencia, 3-SCAN, ...
            /// </summary>
            public int tpEmis { get; set; }

            public string chNFe { get; set; }
            public string nRegDPEC { get; set; }
            public string verAplic { get; set; }
        }
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do registro do DPEC
        /// </summary>
        public DadosConsDPEC dadosConsDPEC = new DadosConsDPEC();
        #endregion

        #endregion

        #region Metodos abstratos

        public abstract void ConsCad(string cArquivoXML);
        public abstract void Nfe(string cArquivoXML);
        public abstract void PedCanc(string cArquivoXML);
        public abstract void PedInut(string cArquivoXML);
        public abstract void PedSit(string cArquivoXML);
        public abstract void PedSta(string cArquivoXML);
        public abstract void Recibo(string strXml);
        public abstract void PedRec(string cArquivoXML);
        public abstract void EnvDPEC(int emp, string arquivoXML);
        public abstract void ConsDPEC(int emp, string arquivoXML);

        #endregion
    }
}