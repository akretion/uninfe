﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Utility
{
    /// <summary>
    /// Utilitários diversos para trabalhar com XML
    /// </summary>
    public static class XMLUtility
    {
        #region Public Structs

        /// <summary>
        /// Estrutura para recuperar o conteúdo separadamento da chave do DFe (NFe, CTe, NFCe, MDfe, etc...)
        /// </summary>
        public struct ConteudoChaveDFe
        {
            #region Public Properties

            /// <summary>
            /// Ano de emissão do documento fiscal
            /// </summary>
            public string AnoEmissao { get; set; }

            /// <summary>
            /// CNPJ do emissor do documento fiscal
            /// </summary>
            public string CNPJEmissor { get; set; }

            /// <summary>
            /// Código numérido do documento fiscal
            /// </summary>
            public string CodigoNumerico { get; set; }

            /// <summary>
            /// Digito verificador da chave do documento fiscal
            /// </summary>
            public int DigitoVerificador { get; set; }

            /// <summary>
            /// Mês de emissão do documento fiscal
            /// </summary>
            public string MesEmissao { get; set; }

            /// <summary>
            /// Modelo do documento fiscal
            /// </summary>
            public ModeloDFe Modelo { get; set; }

            /// <summary>
            /// Número do documento fiscal
            /// </summary>
            public int NumeroDoctoFiscal { get; set; }

            /// <summary>
            /// Série do documento fiscal
            /// </summary>
            public int Serie { get; set; }

            /// <summary>
            /// Tipo de emissão do documento fiscal
            /// </summary>
            public TipoEmissao TipoEmissao { get; set; }

            /// <summary>
            /// UF do emissor do documento fiscal
            /// </summary>
            public UFBrasil UFEmissor { get; set; }

            #endregion Public Properties
        }

        #endregion Public Structs

        #region Public Classes

        /// <summary>
        /// Tipo Namespace
        /// </summary>
        public class TNameSpace
        {
            #region Public Properties

            /// <summary>
            /// Conteúdo do Namespace
            /// </summary>
            public string NS { get; set; }

            /// <summary>
            /// Prefixo do Namespace
            /// </summary>
            public string Prefix { get; set; }

            #endregion Public Properties
        }

        /// <summary>
        /// Implementa um StringWriter para gravar informações em uma cadeia de caracteres. As informações são armazenadas em um StringBuilder subjacente.
        /// </summary>
        public class Utf8StringWriter: StringWriter
        {
            #region Public Properties

            /// <summary>
            /// Sobrecrever o Encoding para deixar como padrão o UTF8
            /// </summary>
            public override Encoding Encoding => Encoding.UTF8;

            #endregion Public Properties
        }

        #endregion Public Classes

        #region Public Methods

        /// <summary>
        /// Gerar o dígito da chave da NFe, CTe, MDFe ou NFCe
        /// </summary>
        /// <param name="chave">Chave do DFe (sem o dígito) que deve ser calculado o dígito verificador.</param>
        /// <returns>Dígito verificador</returns>
        public static int CalcularDVChave(string chave)
        {
            if(chave is null)
            {
                throw new ArgumentNullException(nameof(chave));
            }

            int i, j, Digito;
            const string PESO = "4329876543298765432987654329876543298765432";

            chave = chave.Replace("NFe", "").Replace("CTe", "").Replace("MDFe", "");

            if(chave.Length != 43)
            {
                throw new Exception(string.Format("Erro na composição da chave [{0}] para obter o dígito verificador.", chave) + Environment.NewLine);
            }
            else
            {
                j = 0;
                try
                {
                    for(i = 0; i < 43; ++i)
                    {
                        j += Convert.ToInt32(chave.Substring(i, 1)) * Convert.ToInt32(PESO.Substring(i, 1));
                    }

                    Digito = 11 - (j % 11);
                    if((j % 11) < 2)
                    {
                        Digito = 0;
                    }
                }
                catch
                {
                    Digito = -1;
                }

                if(Digito == -1)
                {
                    throw new Exception(string.Format("Erro no cálculo do dígito verificador da chave [{0}].", chave) + Environment.NewLine);
                }

                return Digito;
            }
        }

        /// <summary>
        /// Executa uma verificação simples para garantir que a chave do DFe (NFe, CTe, MDfe, NFCe, CTeOS) é valida, se tiver erros retorna exceção.
        /// </summary>
        /// <param name="chave">Chave do DFe a ser verificada</param>
        /// <example>
        /// try
        /// {
        ///     XMLUtility.ChecarChaveDFe("41201280568835000181570010000004841004185096");
        /// }
        /// catch(Exception ex)
        /// {
        ///     //Se chave tiver algum erro, vai retornar uma exceção.
        ///     MessageBox.Show(ex.Message);
        /// }
        /// </example>
        public static void ChecarChaveDFe(string chave)
        {
            #region Verificar o tamanho da chave

            if(chave.Length != 44)
            {
                throw new Exception("Tamanho da chave do documento fiscal eletrônico está diferente de 44 dígitos. Chave deve ter exatamente 44 dígitos.");
            }

            #endregion Verificar o tamanho da chave

            #region Verificar se o mês da emissão da nota da chave é válida

            var mes = Convert.ToInt32(chave.Substring(4, 2));

            if(mes < 1 || mes > 12)
            {
                throw new Exception("Mês da data de emissão, do documento fiscal eletrônico, que compõe a chave, está incorreto. Mês informado: " + mes.ToString() + ". Meses permitidos: 01 a 12.");
            }

            #endregion Verificar se o mês da emissão da nota da chave é válida

            #region Verificar se o modelo da chave é válido

            var modeloDFe = Convert.ToInt32(chave.Substring(20, 2));
            var tipoEnum = typeof(ModeloDFe);
            if(!Enum.IsDefined(tipoEnum, modeloDFe))
            {
                var modeloPermitido = string.Empty;

                foreach(var item in tipoEnum.GetFields())
                {
                    if(Attribute.GetCustomAttribute(item, typeof(XmlEnumAttribute)) is XmlEnumAttribute attribute)
                    {
                        if(!string.IsNullOrEmpty(modeloPermitido))
                        {
                            modeloPermitido += ", ";
                        }

                        modeloPermitido += attribute.Name;
                    }
                }

                throw new Exception("Modelo, do documento fiscal eletrônico, que compõe a chave, está incorreto. Modelo informado: " + modeloDFe.ToString() + ". Modelos permitidos: " + modeloPermitido + ".");
            }

            #endregion Verificar se o modelo da chave é válido

            #region Verificar se o tipo de emissão da chave é válido

            var tpEmis = Convert.ToInt32(chave.Substring(34, 1));
            tipoEnum = typeof(TipoEmissao);
            if(!Enum.IsDefined(tipoEnum, tpEmis))
            {
                var tipoPermitido = string.Empty;

                foreach(var item in tipoEnum.GetFields())
                {
                    if(Attribute.GetCustomAttribute(item, typeof(XmlEnumAttribute)) is XmlEnumAttribute attribute)
                    {
                        if(!string.IsNullOrEmpty(tipoPermitido))
                        {
                            tipoPermitido += ", ";
                        }

                        tipoPermitido += attribute.Name;
                    }
                }

                throw new Exception("Tipo de emissão, do documento fiscal eletrônico, que compõe a chave, está incorreto. Tipo informado: " + tpEmis.ToString() + ". Tipos permitidos: " + tipoPermitido + ".");
            }

            #endregion Verificar se o tipo de emissão da chave é válido

            #region Verificar se a UF da chave é válida

            var cUF = Convert.ToInt32(chave.Substring(0, 2));
            tipoEnum = typeof(UFBrasil);

            if(!Enum.IsDefined(tipoEnum, cUF) || cUF >= 90 || cUF == 0)
            {
                var cufPermitido = string.Empty;

                foreach(var item in Enum.GetValues(tipoEnum))
                {
                    var ufBrasil = (UFBrasil)Enum.Parse(typeof(UFBrasil), item.ToString());
                    var uf = (int)ufBrasil;

                    if(uf > 0 && uf < 90)
                    {
                        if(!string.IsNullOrEmpty(cufPermitido))
                        {
                            cufPermitido += ", ";
                        }

                        cufPermitido += uf.ToString();
                    }
                }

                throw new Exception("Código da UF, do documento fiscal eletrônico, que compõe a chave, está incorreto. Código informado: " + cUF.ToString() + ". Códigos permitidos: " + cufPermitido + ".");
            }

            #endregion Verificar se a UF da chave é válida

            #region Verificar se o dígito verificador está correto

            var digitoCalc = CalcularDVChave(chave.Substring(0, 43));
            var digitoInf = chave.Substring(43, 1);
            if(digitoCalc != Convert.ToInt32(digitoInf))
            {
                throw new Exception("Dígito verificador, do documento fiscal eletrônico, que compõe a chave, está incorreto. Dígito informado: " + digitoInf + ". Dígito calculado: " + digitoCalc.ToString() + ".");
            }

            #endregion Verificar se o dígito verificador está correto
        }

        /// <summary>
        /// Deserializar XML (Converte o XML para um objeto)
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="xml">XML a ser deserilizado</param>
        /// <returns>Retorna o objeto com o conteúdo do XML deserializado</returns>
        public static T Deserializar<T>(string xml)
            where T : new()
        {
            var serializer = new XmlSerializer(typeof(T));
            var stream = new StringReader(xml);

            var result = (T)serializer.Deserialize(stream);

            if(result is Contract.Serialization.IXmlSerializable serializable)
            {
                serializable.ProcessReader(XmlReader.Create(new StringReader(xml)));
            }

            return result;
        }

        /// <summary>
        /// Deserializar XML (Converte o XML para um objeto)
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="doc">Conteúdo do XML a ser deserilizado</param>
        /// <returns>Retorna o objeto com o conteúdo do XML deserializado</returns>
        public static T Deserializar<T>(XmlDocument doc)
            where T : new() => Deserializar<T>(doc.OuterXml);

        /// <summary>
        /// Detectar qual o tipo de documento fiscal eletrônico do XML
        /// </summary>
        /// <param name="xml">XML a ser analisado</param>
        /// <returns>Retorna o tipo do documento eletrônico</returns>
        public static TipoDFe DetectDFeType(XmlDocument xml) => DetectDFeType(xml.OuterXml);

        /// <summary>
        /// Detectar qual o tipo de documento fiscal eletrônico do XML
        /// </summary>
        /// <param name="xml">XML a ser analisado</param>
        /// <returns>Retorna o tipo do documento eletrônico</returns>
        public static TipoDFe DetectDFeType(string xml)
        {
            var tipoDFe = TipoDFe.Desconhecido;

            if(xml.Contains("<mod>55</mod>"))
            {
                tipoDFe = TipoDFe.NFe;
            }
            else if(xml.Contains("<mod>65</mod>"))
            {
                tipoDFe = TipoDFe.NFCe;
            }
            else if(xml.Contains("<mod>57</mod>"))
            {
                tipoDFe = TipoDFe.CTe;
            }
            else if(xml.Contains("<mod>67</mod>"))
            {
                tipoDFe = TipoDFe.CTeOS;
            }
            else if(xml.Contains("infMDFe"))
            {
                tipoDFe = TipoDFe.MDFe;
            }
            else if(xml.Contains("infCFe"))
            {
                tipoDFe = TipoDFe.CFe;
            }

            return tipoDFe;
        }

        /// <summary>
        /// Retorna o tipo de documento fiscal com base no XML de evento.
        /// </summary>
        /// <param name="xml">XML válido de evento.</param>
        /// <returns></returns>
        public static TipoDFe DetectEventByDFeType(string xml)
        {
            var tipoDFe = TipoDFe.Desconhecido;
            var tagId = "<infEvento Id=\"ID";

            if(!xml.Contains(tagId))
            {
                return tipoDFe;
            }

            var pos = xml.IndexOf(tagId);
            pos += tagId.Length + 26;
            var modelo = xml.Substring(pos, 2);

            switch(modelo)
            {
                case "55":
                    tipoDFe = TipoDFe.NFe;
                    break;

                case "57":
                    tipoDFe = TipoDFe.CTe;
                    break;

                case "58":
                    tipoDFe = TipoDFe.MDFe;
                    break;

                case "65":
                    tipoDFe = TipoDFe.NFCe;
                    break;

                case "67":
                    tipoDFe = TipoDFe.CTeOS;
                    break;
            }

            return tipoDFe;
        }

        /// <summary>
        /// Detectar qual o tipo de evento do CT-e.
        /// </summary>
        /// <param name="xml">XML a ser analisado</param>
        /// <returns>Retorna o tipo do evento do CT-e</returns>
        public static TipoEventoCTe DetectEventoCTeType(XmlDocument xml) => DetectEventoCTeType(xml.OuterXml);

        /// <summary>
        /// Detectar qual o tipo de evento do CT-e.
        /// </summary>
        /// <param name="xml">XML a ser analisado</param>
        /// <returns>Retorna o tipo do evento do CT-e</returns>
        public static TipoEventoCTe DetectEventoCTeType(string xml)
        {
            var tipoEventoCTe = TipoEventoCTe.Desconhecido;

            if(DetectEventByDFeType(xml) == TipoDFe.Desconhecido)
            {
                return tipoEventoCTe;
            }

            if(xml.Contains("<tpEvento>110110</tpEvento>"))
            {
                tipoEventoCTe = TipoEventoCTe.CartaCorrecao;
            }
            else if(xml.Contains("<tpEvento>110111</tpEvento>"))
            {
                tipoEventoCTe = TipoEventoCTe.Cancelamento;
            }
            else if(xml.Contains("<tpEvento>110180</tpEvento>"))
            {
                tipoEventoCTe = TipoEventoCTe.ComprovanteEntrega;
            }
            else if(xml.Contains("<tpEvento>110181</tpEvento>"))
            {
                tipoEventoCTe = TipoEventoCTe.CancelamentoComprovanteEntrega;
            }
            else if(xml.Contains("<tpEvento>610110</tpEvento>"))
            {
                tipoEventoCTe = TipoEventoCTe.PrestDesacordo;
            }
            else if(xml.Contains("<tpEvento>310620</tpEvento>"))
            {
                tipoEventoCTe = TipoEventoCTe.RegistroPassagem;
            }
            return tipoEventoCTe;
        }

        /// <summary>
        /// Detectar qual o tipo de evento do MDF-e.
        /// </summary>
        /// <param name="xml">XML a ser analisado</param>
        /// <returns>Retorna o tipo do evento do MDF-e</returns>
        public static TipoEventoMDFe DetectEventoMDFeType(XmlDocument xml) => DetectEventoMDFeType(xml.OuterXml);

        /// <summary>
        /// Detectar qual o tipo de evento do MDF-e.
        /// </summary>
        /// <param name="xml">XML a ser analisado</param>
        /// <returns>Retorna o tipo do evento do MDF-e</returns>
        public static TipoEventoMDFe DetectEventoMDFeType(string xml)
        {
            var tipoEventoMDFe = TipoEventoMDFe.Desconhecido;

            if(DetectEventByDFeType(xml) == TipoDFe.Desconhecido)
            {
                return tipoEventoMDFe;
            }

            if(xml.Contains("<tpEvento>110111</tpEvento>"))
            {
                tipoEventoMDFe = TipoEventoMDFe.Cancelamento;
            }
            else if(xml.Contains("<tpEvento>110112</tpEvento>"))
            {
                tipoEventoMDFe = TipoEventoMDFe.Encerramento;
            }
            else if(xml.Contains("<tpEvento>110114</tpEvento>"))
            {
                tipoEventoMDFe = TipoEventoMDFe.InclusaoCondutor;
            }
            else if(xml.Contains("<tpEvento>110115</tpEvento>"))
            {
                tipoEventoMDFe = TipoEventoMDFe.InclusaoDFe;
            }

            return tipoEventoMDFe;
        }

        /// <summary>
        /// Detectar qual o tipo de evento do dodocumento fiscal eletrônico do XML
        /// </summary>
        /// <param name="xml">XML a ser analisado</param>
        /// <returns>Retorna o tipo do evento do documento eletrônico</returns>
        public static TipoEventoNFe DetectEventoNFeType(XmlDocument xml) => DetectEventoNFeType(xml.OuterXml);

        /// <summary>
        /// Detectar qual o tipo de evento do documento fiscal eletrônico do XML
        /// </summary>
        /// <param name="xml">XML a ser analisado</param>
        /// <returns>Retorna o tipo do evento do documento eletrônico</returns>
        public static TipoEventoNFe DetectEventoNFeType(string xml)
        {
            var tipoEventoNFe = TipoEventoNFe.Desconhecido;

            if(DetectEventByDFeType(xml) == TipoDFe.Desconhecido)
            {
                return tipoEventoNFe;
            }

            if(xml.Contains("<tpEvento>110110</tpEvento>"))
            {
                tipoEventoNFe = TipoEventoNFe.CartaCorrecao;
            }
            else if(xml.Contains("<tpEvento>110111</tpEvento>"))
            {
                tipoEventoNFe = TipoEventoNFe.Cancelamento;
            }
            else if(xml.Contains("<tpEvento>110112</tpEvento>"))
            {
                tipoEventoNFe = TipoEventoNFe.CancelamentoPorSubstituicao;
            }
            else if(xml.Contains("<tpEvento>110140</tpEvento>"))
            {
                tipoEventoNFe = TipoEventoNFe.EPEC;
            }
            else if(xml.Contains("<tpEvento>111500</tpEvento>"))
            {
                tipoEventoNFe = TipoEventoNFe.PedidoProrrogacao;
            }
            else if(xml.Contains("<tpEvento>210200</tpEvento>"))
            {
                tipoEventoNFe = TipoEventoNFe.ManifestacaoConfirmacaoOperacao;
            }
            else if(xml.Contains("<tpEvento>210210</tpEvento>"))
            {
                tipoEventoNFe = TipoEventoNFe.ManifestacaoCienciaOperacao;
            }
            else if(xml.Contains("<tpEvento>210220</tpEvento>"))
            {
                tipoEventoNFe = TipoEventoNFe.ManifestacaoDesconhecimentoOperacao;
            }
            else if(xml.Contains("<tpEvento>210240</tpEvento>"))
            {
                tipoEventoNFe = TipoEventoNFe.ManifestacaoOperacaoNaoRealizada;
            }

            return tipoEventoNFe;
        }

        /// <summary>
        /// Extrair conteúdo da chave do documento fiscal eletrônico (NFe, NFCe, CTe, MDFe, etc...) com elementos separados.
        /// </summary>
        /// <param name="chave">Chave do DFe para extrair o conteúdo</param>
        /// <returns>Estrutura contendo o valor de cada elemento que compõe a chave do DFe</returns>
        /// <example>
        ///
        /// var conteudo = XMLUtility.ExtrairConteudoChaveDFe("41210212345678000112650110000000069123456787");
        ///
        /// Console.WriteLine(conteudo.UFEmissor); //Output: PR
        /// Console.WriteLine(conteudo.AnoEmissao); //Output: 21
        /// Console.WriteLine(conteudo.MesEmissao); //Output: 02
        /// Console.WriteLine(conteudo.CNPJEmissor); //Output: 12345678000112
        /// Console.WriteLine(conteudo.Modelo); //Output: NFCe
        /// Console.WriteLine(conteudo.Serie); //Output: 11
        /// Console.WriteLine(conteudo.NumeroDoctoFiscal); //Output: 6
        /// Console.WriteLine(conteudo.TipoEmissao); //Output: ContingenciaOffLine
        /// Console.WriteLine(conteudo.CodigoNumerico); //Output: 12345678
        /// Console.WriteLine(conteudo.DigitoVerificador); //Output: 7
        ///
        /// </example>
        public static ConteudoChaveDFe ExtrairConteudoChaveDFe(string chave)
        {
            var conteudo = new ConteudoChaveDFe
            {
                UFEmissor = (UFBrasil)Convert.ToInt32(chave.Substring(0, 2)),
                AnoEmissao = chave.Substring(2, 2),
                MesEmissao = chave.Substring(4, 2),
                CNPJEmissor = chave.Substring(6, 14),
                Modelo = (ModeloDFe)Convert.ToInt32(chave.Substring(20, 2)),
                Serie = Convert.ToInt32(chave.Substring(22, 3)),
                NumeroDoctoFiscal = Convert.ToInt32(chave.Substring(25, 9)),
                TipoEmissao = (TipoEmissao)Convert.ToInt32(chave.Substring(34, 1)),
                CodigoNumerico = chave.Substring(35, 8),
                DigitoVerificador = Convert.ToInt32(chave.Substring(43, 1))
            };

            return conteudo;
        }

        /// <summary>
        /// Gera um número randômico para ser utilizado no Codigo Numérico da NFe, NFCe, CTe, MDFe, etc...
        /// </summary>
        /// <param name="numeroNF">Número da NF, CT ou MDF</param>
        /// <returns>Código numérico</returns>
        public static int GerarCodigoNumerico(int numeroNF)
        {
            var retorno = 0;

            while(retorno == 0)
            {
                var rnd = new Random(numeroNF);

                retorno = Convert.ToInt32(rnd.Next(1, 99999999).ToString("00000000"));
            }

            return retorno;
        }

        /// <summary>
        /// Busca o número da chave do Documento Fiscal Eletrônico no XML do Documento Fiscal Eletrônico
        /// </summary>
        /// <param name="xml">Conteúdo do XML para busca da chave</param>
        /// <returns>Chave do DFe (Documento Fiscal Eletrônico = NFe, NFCe, CTe, etc...)</returns>
        public static string GetChaveDFe(string xml) => GetChaveDFe(xml, DetectDFeType(xml));

        /// <summary>
        /// Busca o número da chave do Documento Fiscal Eletrônico no XML do Documento Fiscal Eletrônico
        /// </summary>
        /// <param name="xml">Conteúdo do XML para busca da chave</param>
        /// <param name="typeDFe">Tipo do DFe</param>
        /// <returns>Chave do DFe (Documento Fiscal Eletrônico = NFe, NFCe, CTe, etc...)</returns>
        public static string GetChaveDFe(string xml, TipoDFe typeDFe)
        {
            var typeString = "";

            switch(typeDFe)
            {
                case TipoDFe.NFe:
                case TipoDFe.NFCe:
                    typeString = "NFe";
                    break;

                case TipoDFe.CTe:
                case TipoDFe.CTeOS:
                    typeString = "CTe";
                    break;

                case TipoDFe.MDFe:
                    typeString = "MDFe";
                    break;

                case TipoDFe.CFe:
                    typeString = "CFe";
                    break;
            }

            var pedacinhos = xml.Split(new string[] { $"Id=\"{typeString}" }, StringSplitOptions.None);

            if(pedacinhos.Length < 1)
            {
                return default;
            }

            return pedacinhos[1].Substring(0, 44);
        }

        /// <summary>
        /// Busca o número da chave do do evento do CT-e
        /// </summary>
        /// <param name="xml">Conteúdo do XML para busca da chave</param>
        /// <returns>Chave do evento do CT-e</returns>
        public static string GetChaveEventoCTe(string xml) => GetChaveEventoCTe(xml, DetectEventoCTeType(xml));

        /// <summary>
        /// Busca o número da chave do do evento do CT-e
        /// </summary>
        /// <param name="xml">Conteúdo do XML para busca da chave</param>
        /// <returns>Chave do evento do CT-e</returns>
        public static string GetChaveEventoCTe(string xml, TipoEventoCTe typeEventoCTe)
        {
            var typeString = "";

            switch(typeEventoCTe)
            {
                case TipoEventoCTe.CartaCorrecao:
                    typeString = "110110";
                    break;

                case TipoEventoCTe.Cancelamento:
                    typeString = "110111";
                    break;

                case TipoEventoCTe.ComprovanteEntrega:
                    typeString = "110180";
                    break;

                case TipoEventoCTe.CancelamentoComprovanteEntrega:
                    typeString = "110181";
                    break;

                case TipoEventoCTe.PrestDesacordo:
                    typeString = "610110";
                    break;

                case TipoEventoCTe.RegistroPassagem:
                    typeString = "310620";
                    break;
            }

            var pedacinhos = xml.Split(new string[] { $"Id=\"ID{typeString}" }, StringSplitOptions.None);

            if(pedacinhos.Length < 1)
            {
                return default;
            }

            return pedacinhos[1].Substring(0, 44);
        }

        /// <summary>
        /// Busca o número da chave do do evento do MDF-e
        /// </summary>
        /// <param name="xml">Conteúdo do XML para busca da chave</param>
        /// <returns>Chave do evento do MDF-e</returns>
        public static string GetChaveEventoMDFe(string xml) => GetChaveEventoMDFe(xml, DetectEventoMDFeType(xml));

        /// <summary>
        /// Busca o número da chave do do evento do MDF-e
        /// </summary>
        /// <param name="xml">Conteúdo do XML para busca da chave</param>
        /// <returns>Chave do evento do MDF-e</returns>
        public static string GetChaveEventoMDFe(string xml, TipoEventoMDFe typeEventoMDFe)
        {
            var typeString = "";

            switch(typeEventoMDFe)
            {
                case TipoEventoMDFe.Cancelamento:
                    typeString = "110111";
                    break;

                case TipoEventoMDFe.Encerramento:
                    typeString = "110112";
                    break;

                case TipoEventoMDFe.InclusaoCondutor:
                    typeString = "110114";
                    break;

                case TipoEventoMDFe.InclusaoDFe:
                    typeString = "110115";
                    break;
            }

            var pedacinhos = xml.Split(new string[] { $"Id=\"ID{typeString}" }, StringSplitOptions.None);

            if(pedacinhos.Length < 1)
            {
                return default;
            }

            return pedacinhos[1].Substring(0, 44);
        }

        /// <summary>
        /// Busca o número da chave do Documento Fiscal Eletrônico no XML do Documento Fiscal Eletrônico
        /// </summary>
        /// <param name="xml">Conteúdo do XML para busca da chave</param>
        /// <returns>Chave do DFe (Documento Fiscal Eletrônico = NFe, NFCe, CTe, etc...)</returns>
        public static string GetChaveEventoNFe(string xml) => GetChaveEventoNFe(xml, DetectEventoNFeType(xml));

        /// <summary>
        /// Busca o número da chave do Documento Fiscal Eletrônico no XML do Documento Fiscal Eletrônico
        /// </summary>
        /// <param name="xml">Conteúdo do XML para busca da chave</param>
        /// <param name="typeEventoDFe">Tipo do Evento DFe</param>
        /// <returns>Chave do evento do DFe (Documento Fiscal Eletrônico = NFe, NFCe, CTe, etc...)</returns>
        public static string GetChaveEventoNFe(string xml, TipoEventoNFe typeEventoDFe)
        {
            var typeString = "";

            switch(typeEventoDFe)
            {
                case TipoEventoNFe.CartaCorrecao:
                    typeString = "110110";
                    break;

                case TipoEventoNFe.Cancelamento:
                    typeString = "110111";
                    break;

                case TipoEventoNFe.CancelamentoPorSubstituicao:
                    typeString = "110112";
                    break;

                case TipoEventoNFe.EPEC:
                    typeString = "110140";
                    break;

                case TipoEventoNFe.PedidoProrrogacao:
                    typeString = "111500";
                    break;

                case TipoEventoNFe.ManifestacaoConfirmacaoOperacao:
                    typeString = "210200";
                    break;

                case TipoEventoNFe.ManifestacaoCienciaOperacao:
                    typeString = "210210";
                    break;

                case TipoEventoNFe.ManifestacaoDesconhecimentoOperacao:
                    typeString = "210220";
                    break;

                case TipoEventoNFe.ManifestacaoOperacaoNaoRealizada:
                    typeString = "210240";
                    break;
            }

            var pedacinhos = xml.Split(new string[] { $"Id=\"ID{typeString}" }, StringSplitOptions.None);

            if(pedacinhos.Length < 1)
            {
                return default;
            }

            return pedacinhos[1].Substring(0, 44);
        }

        /// <summary>
        /// Serializar o objeto (Converte o objeto para XML)
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="objeto">Objeto a ser serializado</param>
        /// <param name="nameSpaces">Namespaces a serem adicionados no XML</param>
        /// <returns>XML</returns>
        public static XmlDocument Serializar<T>(T objeto, List<TNameSpace> nameSpaces = null)
            where T : new() => Serializar((object)objeto, nameSpaces);

        /// <summary>
        /// Serializar o objeto (Converte o objeto para XML)
        /// </summary>
        /// <param name="objeto">Objeto a ser serializado</param>
        /// <param name="nameSpaces">Namespaces a serem adicionados no XML</param>
        /// <returns>XML</returns>
        public static XmlDocument Serializar(object objeto, List<TNameSpace> nameSpaces = null)
        {
            if(objeto is null)
            {
                throw new ArgumentNullException(nameof(objeto));
            }

            var ns = new XmlSerializerNamespaces();
            if(nameSpaces != null)
            {
                for(var i = 0; i < nameSpaces.Count; i++)
                {
                    ns.Add(nameSpaces[i].Prefix, nameSpaces[i].NS);
                }
            }

            var xmlSerializer = new XmlSerializer(objeto.GetType());
            var doc = new XmlDocument();
            using(StringWriter textWriter = new Utf8StringWriter())
            {
                xmlSerializer.Serialize(textWriter, objeto, ns);
                doc.LoadXml(textWriter.ToString());
            }

            return doc;
        }

        /// <summary>
        /// Busca o nome de uma determinada TAG em um Elemento do XML para ver se existe, se existir retorna seu conteúdo da TAG.
        /// </summary>
        /// <param name="xmlElement">Elemento do XML onde será pesquisado o Nome da TAG</param>
        /// <param name="tagName">Nome da Tag que será pesquisado</param>
        /// <returns>Conteúdo da tag</returns>
        public static bool TagExist(XmlElement xmlElement, string tagName)
        {
            if(xmlElement is null)
            {
                throw new ArgumentNullException(nameof(xmlElement));
            }

            return xmlElement.GetElementsByTagName(tagName).Count != 0;
        }

        /// <summary>
        /// Busca o nome de uma determinada TAG em um Elemento do XML para ver se existe, se existir retorna seu conteúdo da TAG.
        /// </summary>
        /// <param name="xmlElement">Elemento do XML onde será pesquisado o Nome da TAG</param>
        /// <param name="tagName">Nome da Tag que será pesquisado</param>
        /// <returns>Conteúdo da tag</returns>
        public static string TagRead(XmlElement xmlElement, string tagName)
        {
            if(xmlElement is null)
            {
                throw new ArgumentNullException(nameof(xmlElement));
            }

            var content = string.Empty;

            if(xmlElement.GetElementsByTagName(tagName).Count != 0)
            {
                content = xmlElement.GetElementsByTagName(tagName)[0].InnerText;
            }

            return content;
        }

        #endregion Public Methods
    }
}