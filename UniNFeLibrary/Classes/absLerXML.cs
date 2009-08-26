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
        }

        /// <summary>
        /// Esta classe possui as propriedades que vai receber o conteúdo do XML do recibo do lote
        /// </summary>
        public class DadosRecClass
        {
            public string nRec { get; set; } //Recibo do lote de notas fiscais enviado
            public string cStat { get; set; } //Status do Lote
            public int tMed { get; set; } //Tempo médio de resposta em segundos
        }

        public class DadosPedSta
        {
            /// <summary>
            /// Ambiente (2-Homologação ou 1-Produção)
            /// </summary>
            public string tpAmb { get; set; }
            /// <summary>
            /// Código da Unidade Federativa (UF)
            /// </summary>
            public string cUF { get; set; }
        }

        public class DadosPedSit
        {
            private string mchNFe;

            /// <summary>
            /// Ambiente (2-Homologação ou 1-Produção)
            /// </summary>
            public string tpAmb { get; set; }
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
                    this.cUF = string.Empty;
                    this.serie = string.Empty;
                    if (this.mchNFe != string.Empty)
                    {
                        this.cUF = this.mchNFe.Substring(0, 2);
                        this.serie = this.mchNFe.Substring(23, 3);
                        if (Convert.ToInt32(this.serie) >= 900)
                        {
                            this.tpEmis = "3";
                        }
                        else
                        {
                            this.tpEmis = "1";
                        }
                    }
                }
            }
            /// <summary>
            /// Código da Unidade Federativa (UF)
            /// </summary>
            public string cUF { get; private set; }
            /// <summary>
            /// Série da NFe que está sendo consultada a situação
            /// </summary>
            public string serie { get; private set; }
            /// <summary>
            /// Tipo de emissão para saber para onde será enviado a consulta da situação da nota
            /// </summary>
            public string tpEmis { get; private set; }
        }

        public class DadosConsCad
        {
            private string mUF;

            public DadosConsCad()
            {
                this.tpAmb = "1";
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
                    this.cUF = string.Empty;

                    switch (this.mUF.ToUpper().Trim())
                    {
                        case "AC":
                            this.cUF = "12";
                            break;

                        case "AL":
                            this.cUF = "27";
                            break;

                        case "AP":
                            this.cUF = "16";
                            break;

                        case "AM":
                            this.cUF = "13";
                            break;

                        case "BA":
                            this.cUF = "29";
                            break;

                        case "CE":
                            this.cUF = "23";
                            break;

                        case "DF":
                            this.cUF = "53";
                            break;

                        case "ES":
                            this.cUF = "32";
                            break;

                        case "GO":
                            this.cUF = "52";
                            break;

                        case "MA":
                            this.cUF = "21";
                            break;

                        case "MG":
                            this.cUF = "31";
                            break;

                        case "MS":
                            this.cUF = "50";
                            break;

                        case "MT":
                            this.cUF = "51";
                            break;

                        case "PA":
                            this.cUF = "15";
                            break;

                        case "PB":
                            this.cUF = "25";
                            break;

                        case "PE":
                            this.cUF = "26";
                            break;

                        case "PI":
                            this.cUF = "22";
                            break;

                        case "PR":
                            this.cUF = "41";
                            break;

                        case "RJ":
                            this.cUF = "33";
                            break;

                        case "RN":
                            this.cUF = "24";
                            break;

                        case "RO":
                            this.cUF = "11";
                            break;

                        case "RR":
                            this.cUF = "14";
                            break;

                        case "RS":
                            this.cUF = "43";
                            break;

                        case "SC":
                            this.cUF = "42";
                            break;

                        case "SE":
                            this.cUF = "28";
                            break;

                        case "SP":
                            this.cUF = "35";
                            break;

                        case "TO":
                            this.cUF = "17";
                            break;
                    }
                }
            }

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
            public string cUF { get; private set; }
            /// <summary>
            /// Ambiente (2-Homologação 1-Produção)
            /// </summary>
            public string tpAmb { get; private set; }
        }

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da nota fiscal eletrônica
        /// </summary>
        public DadosNFeClass oDadosNfe = new DadosNFeClass();

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do recibo do lote
        /// </summary>
        public DadosRecClass oDadosRec = new DadosRecClass();

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do status do serviço
        /// </summary>
        public DadosPedSta oDadosPedSta = new DadosPedSta();

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta da situação da nota
        /// </summary>
        public DadosPedSit oDadosPedSit = new DadosPedSit();

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta do cadastro do contribuinte
        /// </summary>
        public DadosConsCad oDadosConsCad = new DadosConsCad();

        public abstract void Nfe(string cArquivoXML);

        public abstract void Recibo(string strXml);

        public abstract void PedSta(string cArquivoXML);

        public abstract void PedSit(string cArquivoXML);

        public abstract void ConsCad(string cArquivoXML);
    }
}