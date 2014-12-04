using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace NFe.Components
{
    /// <summary>
    /// Propriedades publicas
    /// </summary>
    public class Propriedade
    {
        public static Assembly AssemblyEXE;

        public static bool ExecutandoPeloUniNFe = true;

        public static string[] ServiceName = new string[] { "UniNFeServico", "UniNFSeServico" };

        /// <summary>
        /// Nome para a pasta dos XML assinados
        /// </summary>
        public const string NomePastaXMLAssinado = "\\Assinado";
        public const string NomeArqERRUniNFe = "UniNFeErro_{0}.err";
        /// <summary>
        /// Nome do arquivo XML de configurações
        /// </summary>
        public const string NomeArqConfig = "UniNfeConfig.xml";
        /// <summary>
        /// Nome do arquivo XML de configurações da tela de sobre
        /// </summary>
        public const string NomeArqConfigSobre = "UniNfeSobre.xml";
        /// <summary>
        /// Nome do arquivo XML que é gravado as empresas cadastradas
        /// </summary>
        public static readonly string NomeArqEmpresas = Propriedade.PastaExecutavel + "\\UniNfeEmpresa.xml";
        /// <summary>     
        /// Nome do arquivo para controle da numeração sequencial do lote.
        /// </summary>
        public const string NomeArqXmlLote = "UniNfeLote.xml";
        /// <summary>
        /// Nome do arquivo 1 de backup de segurança do arquivo de controle da numeração sequencial do lote
        /// </summary>
        public const string NomeArqXmlLoteBkp1 = "Bkp1_UniNfeLote.xml";
        /// <summary>
        /// Nome do arquivo 2 de backup de segurança do arquivo de controle da numeração sequencial do lote
        /// </summary>
        public const string NomeArqXmlLoteBkp2 = "Bkp2_UniNfeLote.xml";
        /// <summary>
        /// Nome do arquivo 3 de backup de segurança do arquivo de controle da numeração sequencial do lote
        /// </summary>
        public const string NomeArqXmlLoteBkp3 = "Bkp3_UniNfeLote.xml";
        /// <summary>
        /// Nome do arquivo que grava as notas fiscais em fluxo de envio
        /// </summary>
        public const string NomeArqXmlFluxoNfe = "fluxonfe.xml";

        /// <summary>
        /// Retorna o nome do XML dos municipios
        /// </summary>
        public static string NomeArqXMLMunicipios
        {
            get { return Propriedade.PastaExecutavel + "\\UniNFeMunic.xml"; }
        }

        /// <summary>
        /// Retorna a pasta do executável
        /// </summary>
        /// <returns>Retorna a pasta onde está o executável</returns>
        public static string PastaExecutavel
        {
            get { return System.IO.Path.GetDirectoryName(Application.ExecutablePath); }
        }

        #region Pastas de comunicação geral do ERP com o UniNFe
        /// <summary>
        /// Pasta de comunicação geral do ERP com o UniNFe (Envio)
        /// </summary>
        public static string PastaGeral
        {
            get { return Propriedade.PastaExecutavel + "\\Geral"; }
        }

        /// <summary>
        /// Pasta de comunicação geral do ERP com o UniNFe (Temporária)
        /// </summary>
        public static string PastaGeralTemporaria
        {
            get { return Propriedade.PastaGeral + "\\Temp"; }
        }

        /// <summary>
        /// Pasta de comunicação geral do ERP com o UniNFe (Retornos)
        /// </summary>
        public static string PastaGeralRetorno
        {
            get { return Propriedade.PastaGeral + "\\Retorno"; }
        }
        #endregion

        /// <summary>
        /// Retorna o XML contendos as definicoes dos webservices
        /// </summary>
        public static string NomeArqXMLWebService_NFe
        {
            get 
            {
                return Propriedade.PastaExecutavel + "\\NFe\\WSDL\\Webservice.xml"; 
            }
        }
        public static string NomeArqXMLWebService_NFSe
        {
            get
            {
                return Propriedade.PastaExecutavel + "\\NFse\\WSDL\\Webservice.xml";
            }
        }

        public static TipoAplicativo TipoAplicativo { get; set; }

        public static List<Municipio> Municipios { get; set; }

        public static string[,] CodigosEstados
        {
            get
            {
                return new string[,] 
                {
                    { "12", "AC - Acre" },
                    { "27", "AL - Alagoas" },
                    { "16", "AP - Amapá" },
                    { "13", "AM - Amazonas" },
                    { "29", "BA - Bahia" },
                    { "23", "CE - Ceará" },
                    { "53", "DF - Distrito Federal" },
                    { "32", "ES - Espirito Santo" },
                    { "52", "GO - Goiás" },
                    { "21", "MA - Maranhão" },
                    { "51", "MT - Mato Grosso" },
                    { "50", "MS - Mato Grosso do Sul" },
                    { "31", "MG - Minas Gerais" },
                    { "15", "PA - Pará" },
                    { "25", "PB - Paraiba" },
                    { "41", "PR - Paraná" },
                    { "26", "PE - Pernambuco" },
                    { "22", "PI - Piauí" },
                    { "33", "RJ - Rio de Janeiro" },
                    { "24", "RN - Rio Grande do Norte" },
                    { "43", "RS - Rio Grande do Sul" },
                    { "11", "RO - Rondonia" },
                    { "14", "RR - Roraima" },
                    { "42", "SC - Santa Catarina" },
                    { "35", "SP - São Paulo" },
                    { "28", "SE - Sergipe" },
                    { "17", "TO - Tocantins" }
                };
            }
        }

        /// <summary>
        /// Retorna a pasta onde são gravados os log´s do UniNFe
        /// </summary>
        /// <returns>Pasta de log</returns>
        public static string PastaLog
        {
            get { return PastaExecutavel + "\\log"; }
        }

        /// <summary>
        /// Retorna a pasta dos schemas para validar os XML´s
        /// </summary>
        /// <returns></returns>
        public static string PastaSchemas
        {
            get { return PastaExecutavel + "\\schemas"; }
        }

        /// <summary>
        /// Namespace URI associado (Endereço http dos schemas de XML)
        /// </summary>
        public static string nsURI_nfe { get; set; }

        #region Propriedades com as extensões dos XML ou TXT de envio
        /// <summary>
        /// Classe com as propriedades com as extensões dos XML ou TXT de envio
        /// </summary>
        public class ExtEnvio
        {
            #region Extensões gerais
            public const string ConsCertificado = "-cons-certificado.xml";
            /// <summary>
            /// -alt-con.xml
            /// </summary>
            public const string AltCon_XML = "-alt-con.xml";
            /// <summary>
            /// -alt-con.txt
            /// </summary>
            public const string AltCon_TXT = "-alt-con.txt";
            /// <summary>
            /// -cons-inf.xml
            /// </summary>
            public const string ConsInf_XML = "-cons-inf.xml";
            /// <summary>
            /// -cons-inf.txt
            /// </summary>
            public const string ConsInf_TXT = "-cons-inf.txt";
            #endregion

            #region Extensões da NFe
            /// <summary>
            /// -nfe.xml
            /// </summary>
            public const string Nfe = "-nfe.xml";
            /// <summary>
            /// -nfe.txt
            /// </summary>
            public const string Nfe_TXT = "-nfe.txt";
            /// <summary>
            /// -env-cce.xml
            /// </summary>
            public const string EnvCCe_XML = "-env-cce.xml";
            /// <summary>
            /// -env-cce.txt
            /// </summary>
            public const string EnvCCe_TXT = "-env-cce.txt";
            /// <summary>
            /// -env-manif.xml
            /// </summary>
            public const string EnvManifestacao_XML = "-env-manif.xml";
            /// <summary>
            /// -env-manif.txt
            /// </summary>
            public const string EnvManifestacao_TXT = "-env-manif.txt";
            /// <summary>
            /// -env-canc.xml
            /// </summary>
            public const string EnvCancelamento_XML = "-env-canc.xml";
            /// <summary>
            /// -env-canc.txt
            /// </summary>
            public const string EnvCancelamento_TXT = "-env-canc.txt";
            /// <summary>
            /// -gerar-chave.xml
            /// </summary>
            public const string GerarChaveNFe_XML = "-gerar-chave.xml";
            /// <summary>
            /// -gerar-chave.txt
            /// </summary>
            public const string GerarChaveNFe_TXT = "-gerar-chave.txt";
            /// <summary>
            /// -env-dpec.xml
            /// </summary>
            public const string EnvDPEC_XML = "-env-dpec.xml";
            /// <summary>
            /// -env-dpec.txt
            /// </summary>
            public const string EnvDPEC_TXT = "-env-dpec.txt";
            /// <summary>
            /// -cons-dpec.xml"
            /// </summary>
            public const string ConsDPEC_XML = "-cons-dpec.xml";
            /// <summary>
            /// -cons-dpec.txt
            /// </summary>
            public const string ConsDPEC_TXT = "-cons-dpec.txt";
            /// <summary>
            /// -down-nfe.xml
            /// </summary>
            public const string EnvDownload_XML = "-nfe-down.xml";
            /// <summary>
            /// -down-nfe.txt
            /// </summary>
            public const string EnvDownload_TXT = "-nfe-down.txt";
            /// <summary>
            /// -cons-nfe-dest.xml
            /// </summary>
            public const string ConsNFeDest_XML = "-cons-nfe-dest.xml";
            /// <summary>
            /// -cons-nfe-dest.txt
            /// </summary>
            public const string ConsNFeDest_TXT = "-cons-nfe-dest.txt";
#if nao
            /// <summary>
            /// -env-regs.xml
            /// </summary>
            public const string EnvRegistroDeSaida_XML = "-env-regs.xml";
            /// <summary>
            /// -env-regs.txt
            /// </summary>
            public const string EnvRegistroDeSaida_TXT = "-env-regs.txt";
            /// <summary>
            /// -env-canc-regs.xml
            /// </summary>
            public const string EnvCancRegistroDeSaida_XML = "-env-canc-regs.xml";
            /// <summary>
            /// -env-canc-regs.txt
            /// </summary>
            public const string EnvCancRegistroDeSaida_TXT = "-env-canc-regs.txt";
#endif
            #endregion

            #region Extensões do CTe
            /// <summary>
            /// -cte.xml
            /// </summary>
            public const string Cte = "-cte.xml";
            #endregion

            #region Extensões da NFS-e
            /// <summary>
            /// -env-loterps.xml X -ret-loterps.xml
            /// </summary>
            public const string EnvLoteRps = "-env-loterps.xml";
            /// <summary>
            /// -ped-cannfse.xml
            /// </summary>
            public const string PedCanNfse = "-ped-cannfse.xml";
            /// <summary>
            /// -ped-loterps.xml
            /// </summary>
            public const string PedLoteRps = "-ped-loterps.xml";
            /// <summary>
            /// -ped-sitloterps.xml
            /// </summary>
            public const string PedSitLoteRps = "-ped-sitloterps.xml";
            /// <summary>
            /// -ped-sitnfserps.xml
            /// </summary>
            public const string PedSitNfseRps = "-ped-sitnfserps.xml";
            /// <summary>
            /// -ped-sitnfse.xml
            /// </summary>
            public const string PedSitNfse = "-ped-sitnfse.xml";
            /// <summary>
            /// -ped-urlnfse.xml x -urlnfse.xml
            /// </summary>
            public const string PedURLNfse = "-ped-urlnfse.xml";
            /// <summary>
            /// -ped-urlnfseserie.xml x -urlnfseserie.xml
            /// </summary>
            public const string PedURLNfseSerie = "-ped-urlnfseserie.xml";
            #endregion

            #region Extensões MDF-e
            /// <summary>
            /// -mdfe.xml
            /// </summary>
            public static string MDFe = "-mdfe.xml";
            #endregion

            #region Extensões em comum entre NFe, CTe e MDF-e

            /// <summary>
            /// -ped-eve.xml
            /// </summary>
            public const string PedEve = "-ped-eve.xml";
            /// <summary>
            /// -ped-eve.txt
            /// </summary>
            public const string PedEve_TXT = "-ped-eve.txt";
            /// <summary>
            /// -montar-lote.xml
            /// </summary>
            public const string MontarLote = "-montar-lote.xml";
            /// <summary>
            /// -montar-lote.txt
            /// </summary>
            public const string MontarLote_TXT = "-montar-lote.txt";
            /// <summary>
            /// -cons-cad.xml
            /// </summary>
            public const string ConsCad_XML = "-cons-cad.xml";
            /// <summary>
            /// -cons-cad.txt
            /// </summary>
            public const string ConsCad_TXT = "-cons-cad.txt";
            /// <summary>
            /// -env-lot.xml
            /// </summary>
            public const string EnvLot = "-env-lot.xml";
            /// <summary>
            /// -ped-inu.xml
            /// </summary>
            public const string PedInu_XML = "-ped-inu.xml";
            /// <summary>
            /// -ped-inu.txt
            /// </summary>
            public const string PedInu_TXT = "-ped-inu.txt";
            /// <summary>
            /// -ped-rec.xml
            /// </summary>
            public const string PedRec_XML = "-ped-rec.xml";
            /// <summary>
            /// -ped-rec.xml
            /// </summary>
            public const string PedSit_XML = "-ped-sit.xml";
            /// <summary>
            /// -ped-sit.txt
            /// </summary>
            public const string PedSit_TXT = "-ped-sit.txt";
            /// <summary>
            /// -ped-sta.xml
            /// </summary>
            public const string PedSta_XML = "-ped-sta.xml";
            /// <summary>
            /// -ped-sta.txt
            /// </summary>
            public const string PedSta_TXT = "-ped-sta.txt";
            #endregion

            #region Extensões usadas na pesquisa de se um serviço existe para um determinado estado (Producao/Homologacao)
            public const string EnvWSExiste_XML = "-env-ws.xml";
            public const string EnvWSExiste_TXT = "-env-ws.txt";
            #endregion

            public const string EnvImpressaoDanfe_XML = "-env-danfe.xml";
            public const string EnvImpressaoDanfe_TXT = "-env-danfe.txt";
            public const string EnvDanfeReport_XML = "-env-danfe-report.xml";
            public const string EnvDanfeReport_TXT = "-env-danfe-report.txt";

            #region Extensoes de DFe
            public const string EnvDFe_XML = "-con-dist-dfe.xml";
            public const string EnvDFe_TXT = "-con-dist-dfe.txt";
            #endregion
        }
        #endregion

        #region Propriedades com as extensões dos XML ou TXT de retorno
        /// <summary>
        /// Classe com as propriedades com as extensões dos XML ou TXT de retorno
        /// </summary>
        public class ExtRetorno
        {
            #region Extensões gerais

            #endregion

            #region Extensões NFe
            /// <summary>
            /// -procnfe.xml
            /// </summary>
            public const string ProcNFe = "-procNFe.xml"; //Não deixar tudo minusculo para evitar problemas com Linux configurado para Case Sensitive. Wandrey 23/06/2011
            /// <summary>
            /// -den.xml
            /// </summary>
            public const string Den = "-den.xml";
            /// <summary>
            /// -ret-env-cce.xml
            /// </summary>
            public const string retEnvCCe_XML = "-ret-env-cce.xml";
            /// <summary>
            /// -ret-env-cce.err
            /// </summary>
            public const string retEnvCCe_ERR = "-ret-env-cce.err";
            /// <summary>
            /// -ret-canc.xml
            /// </summary>
            public const string retCancelamento_XML = "-ret-env-canc.xml";
            /// <summary>
            /// -ret-canc.err
            /// </summary>
            public const string retCancelamento_ERR = "-ret-env-canc.err";
            /// <summary>
            /// -procEventoNFe.xml
            /// </summary>
            public const string ProcEventoNFe = "-procEventoNFe.xml";
            /// <summary>
            /// -procinutnfe.xml
            /// </summary>
            public const string ProcInutNFe = "-procInutNFe.xml"; //Não deixar tudo minusculo para evitar problemas com Linux configurado para Case Sensitive. Wandrey 23/06/2011
            /// <summary>
            /// -nfe.err
            /// </summary>
            public const string Nfe_ERR = "-nfe.err";
            /// <summary>
            /// -ret-dpec.xml
            /// </summary>
            public const string retDPEC_XML = "-ret-dpec.xml";
            /// <summary>
            /// -ret-dpec.err
            /// </summary>
            public const string retDPEC_ERR = "-ret-dpec.err";
            /// <summary>
            /// -ret-cons-dpec.xml
            /// </summary>
            public const string retConsDPEC_XML = "-ret-cons-dpec.xml";
            /// <summary>
            /// -ret-cons-dpec.err
            /// </summary>
            public const string retConsDPEC_ERR = "-ret-cons-dpec.err";
            /// <summary>
            /// -ret-down-nfe.xml
            /// </summary>
            public const string retDownload_XML = "-ret-nfe-down.xml";
            /// <summary>
            /// -ret-down-nfe.err
            /// </summary>
            public const string retDownload_ERR = "-ret-nfe-down.err";
            /// <summary>
            /// -ret-cons-nfe-dest.xml
            /// </summary>
            public const string retConsNFeDest_XML = "-ret-cons-nfe-dest.xml";
            /// <summary>
            /// -ret-cons-nfe-dest.err
            /// </summary>
            public const string retConsNFeDest_ERR = "-ret-cons-nfe-dest.err";
            /// <summary>
            /// -ret-manif.xml
            /// </summary>
            public const string retManifestacao_XML = "-ret-env-manif.xml";
            /// <summary>
            /// -ret-manif.err
            /// </summary>
            public const string retManifestacao_ERR = "-ret-env-manif.err";
#if nao
            /// <summary>
            /// -ret-env-regsaida.xml
            /// </summary>
            public const string retRegistroDeSaida_XML = "-ret-env-regs.xml";
            /// <summary>
            /// -ret-env-regsaida.txt
            /// </summary>
            public const string retRegistroDeSaida_TXT = "-ret-env-regs.txt";
            /// <summary>
            /// -ret-env-regsaida.xml
            /// </summary>
            public const string retCancRegistroDeSaida_XML = "-ret-env-canc-regs.xml";
            /// <summary>
            /// -ret-env-regsaida.txt
            /// </summary>
            public const string retCancRegistroDeSaida_TXT = "-ret-env-canc-regs.txt";
#endif
            #endregion

            #region Extensões CTe
            /// <summary>
            /// -procCTe.xml
            /// </summary>
            public const string ProcCTe = "-procCTe.xml"; //Não deixar tudo minusculo para evitar problemas com Linux configurado para Case Sensitive. Wandrey 23/06/2011
            /// <summary>
            /// -procEventoCTe.xml
            /// </summary>
            public const string ProcEventoCTe = "-procEventoCTe.xml";
            /// <summary>
            /// -procInutCTe.xml
            /// </summary>
            public static string ProcInutCTe = "-procInutCTe.xml"; //Não deixar tudo minusculo para evitar problemas com Linux configurado para Case Sensitive. Wandrey 23/06/2011
            /// <summary>
            /// -cte.err
            /// </summary>
            public static string Cte_ERR = "-cte.err";
            #endregion

            #region Extensões MDFe
            /// <summary>
            /// -procMDFe.xml
            /// </summary>
            public const string ProcMDFe = "-procMDFe.xml"; //Não deixar tudo minusculo para evitar problemas com Linux configurado para Case Sensitive. Wandrey 23/06/2011
            /// <summary>
            /// -procEventoMDFe.xml
            /// </summary>
            public const string ProcEventoMDFe = "-procEventoMDFe.xml";
            /// <summary>
            /// -mdfe.err
            /// </summary>
            public const string MDFe_ERR = "-mdfe.err";
            #endregion

            #region Extensões em comum entre NFe, CTe e MDF-e
            /// <summary>
            /// -eve.xml
            /// </summary>
            public const string Eve = "-eve.xml";
            /// <summary>
            /// -eve.err
            /// </summary>
            public const string Eve_ERR = "-eve.err";
            /// <summary>
            /// -montar-lote.err
            /// </summary>
            public const string MontarLote_ERR = "-montar-lote.err";
            /// <summary>
            /// -ret-cons-cad.xml
            /// </summary>
            public const string ConsCad_XML = "-ret-cons-cad.xml";
            /// <summary>
            /// -ret-cons-cad.err
            /// </summary>
            public const string ConsCad_ERR = "-ret-cons-cad.err";
            /// <summary>
            /// -sit.xml
            /// </summary>
            public const string Sit_XML = "-sit.xml";
            /// <summary>
            /// -sit.err
            /// </summary>
            public const string Sit_ERR = "-sit.err";
            /// <summary>
            /// -pro-rec.xml
            /// </summary>
            public const string ProRec_XML = "-pro-rec.xml";
            /// <summary>
            /// -pro-rec.err
            /// </summary>
            public const string ProRec_ERR = "-pro-rec.err";
            /// <summary>
            /// -sta.xml
            /// </summary>
            public const string Sta_XML = "-sta.xml";
            /// <summary>
            /// -sta.err
            /// </summary>
            public const string Sta_ERR = "-sta.err";
            /// <summary>
            /// -inu.err
            /// </summary>
            public const string Inu_ERR = "-inu.err";
            /// <summary>
            /// -rec.xml
            /// </summary>
            public const string Rec = "-rec.xml";
            /// <summary>
            /// -rec.err
            /// </summary>
            public const string Rec_ERR = "-rec.err";
            #endregion

            #region Extensões NFSe
            /// <summary>
            /// -ret-loterps.xml X -env-loterps.xml
            /// </summary>
            public const string RetLoteRps = "-ret-loterps.xml";
            /// <summary>
            /// -ret-loterps.err X -env-loterps.xml
            /// </summary>
            public const string RetLoteRps_ERR = "-ret-loterps.err";
            /// <summary>
            /// -cannfse.xml
            /// </summary>
            public const string CanNfse = "-cannfse.xml";
            /// <summary>
            /// -cannfse.err
            /// </summary>
            public const string CanNfse_ERR = "-cannfse.err";
            /// <summary>
            /// -loterps.xml
            /// </summary>
            public const string LoteRps = "-loterps.xml";
            /// <summary>
            /// -lotrps.err
            /// </summary>
            public const string LoteRps_ERR = "-loterps.err";
            /// <summary>
            /// -sitloterps.xml
            /// </summary>
            public const string SitLoteRps = "-sitloterps.xml";
            /// <summary>
            /// -sitloterps.err
            /// </summary>
            public const string SitLoteRps_ERR = "-sitloterps.err";
            /// <summary>
            /// -sitnfse.xml
            /// </summary>
            public const string SitNfse = "-sitnfse.xml";
            /// <summary>
            /// -sitnfse.err
            /// </summary>
            public const string SitNfse_ERR = "-sitnfse.err";
            /// <summary>
            /// -sitnfserps.xml
            /// </summary>
            public const string SitNfseRps = "-sitnfserps.xml";
            /// <summary>
            /// -sitnfserps.err
            /// </summary>
            public const string SitNfseRps_ERR = "-sitnfserps.err";
            /// <summary>
            /// -urlnfse.xml x -ped-urlnfse.xml
            /// </summary>
            public const string Urlnfse = "-urlnfse.xml";
            /// <summary>
            /// -urlnfse.xml x -ped-urlnfse.xml x -urlnfse.err
            /// </summary>
            public const string Urlnfse_ERR = "-urlnfse.err";
            #endregion

            #region Extensões usadas para retorno da pesquisa de se um serviço exista para um determinado estado (Producao/Homologacao)
            public const string retWSExiste_XML = "-ret-env-ws.xml";
            public const string retWSExiste_TXT = "-ret-env-ws.txt";
            #endregion

            public const string RetImpressaoDanfe_XML = "-ret-danfe.xml";
            public const string RetImpressaoDanfe_TXT = "-ret-danfe.txt";
            public const string RetDanfeReport_XML = "-ret-env-danfe-report.xml";
            public const string RetDanfeReport_TXT = "-ret-env-danfe-report.txt";

            #region Extensoes de DFe
            public const string retEnvDFe_XML = "-dist-dfe.xml";
            public const string retEnvDFe_TXT = "-dist-dfe.txt";
            public const string retEnvDFe_ERR = "-con-dist-dfe.err";
            #endregion
        }
        #endregion

        #region NomeAplicacao
        /// <summary>
        /// Retorna o nome do aplicativo 
        /// </summary>
        /// <param name="oAssembly">Passar sempre: Assembly.GetExecutingAssembly() pois ele vai pegar o Assembly do EXE ou DLL de onde está sendo chamado o método</param>
        /// <returns>string contendo o nome do Aplicativo</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>31/07/2009</date>
        public static string NomeAplicacao
        {
            get
            {
                //Montar o nome da aplicação
                string Produto = string.Empty;

                foreach (Attribute attr in Attribute.GetCustomAttributes(AssemblyEXE))
                {
                    if (attr.GetType() == typeof(AssemblyProductAttribute))
                    {
                        Produto = ((AssemblyProductAttribute)attr).Product;
                        break;
                    }
                }

                return Produto;
            }
        }
        #endregion

        #region Versao
        /// <summary>
        /// Retorna a versão do aplicativo 
        /// </summary>
        public static string Versao
        {
            get
            {
                //Montar a versão do programa
                string versao;

                Assembly _assembly = AssemblyEXE;
                if (!ExecutandoPeloUniNFe)
                    //danasa 22/7/2011
                    //se o servico está sendo executado, pega a versão do 'uninfe.exe'
                    _assembly = System.Reflection.Assembly.LoadFile(Path.Combine(PastaExecutavel, "uninfe.exe"));

                foreach (Attribute attr in Attribute.GetCustomAttributes(_assembly))
                {
                    if (attr.GetType() == typeof(AssemblyVersionAttribute))
                    {
                        versao = ((AssemblyVersionAttribute)attr).Version;
                        break;
                    }
                }
                string delimStr = ",=";
                char[] delimiter = delimStr.ToCharArray();
                string[] strAssembly = _assembly.ToString().Split(delimiter);
                versao = strAssembly[2];

                return versao;
            }
        }
        #endregion

        public static bool ServicoRodando   //danasa 22/7/2011
        {
            get
            {
                try
                {
                    if (ServiceProcess.IsServiceInstalled(Propriedade.ServiceName[Propriedade.TipoAplicativo == Components.TipoAplicativo.Nfe ? 0 : 1]))
                        return ServiceProcess.StatusService(Propriedade.ServiceName[Propriedade.TipoAplicativo == Components.TipoAplicativo.Nfe ? 0 : 1]) == System.ServiceProcess.ServiceControllerStatus.Running;
                    else
                        return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Retorna o XML para salvar os parametros das telas
        /// </summary>
        public static string NomeArqXMLParams
        {
            get
            {
                return PastaExecutavel + "\\UniNFeParams.xml";
            }
        }

        #region Atributos
        /// <summary>
        /// Se pode encerrar a aplicação ou não
        /// </summary>
        public static bool EncerrarApp = false;
        #endregion

        #region DescricaoAplicacao
        /// <summary>
        /// Retorna a descrição da aplicação
        /// </summary>
        /// <returns>Descrição da aplicação</returns>
        public static string DescricaoAplicacao
        {
            get
            {
                //Montar o nome da aplicação
                string descricao = string.Empty;

                foreach (Attribute attr in Attribute.GetCustomAttributes(AssemblyEXE))
                {
                    if (attr.GetType() == typeof(AssemblyDescriptionAttribute))
                    {
                        descricao = ((AssemblyDescriptionAttribute)attr).Description;
                        break;
                    }
                }

                return descricao;
            }
        }
        #endregion
    }

    public class NFeStrConstants
    {
        public static string proxyError = "Especifique o nome do servidor/usuário/senha e porta para conectar do servidor proxy";
        public static string versaoError = "Defina a versão";
        public static string Nome = "Nome";
        public static string ID = "ID";
        public static string Padrao = "Padrao";
        public static string UF = "UF";
        public static string Estado = "Estado";
        public static string Registro = "Registro";
        public static string Servico = "Servico";
        public static string CNPJ = "CNPJ";
        public static string LocalHomologacao = "LocalHomologacao";
        public static string LocalProducao = "LocalProducao";


        public static string nfe_configuracoes = "nfe_configuracoes";
        public static string PastaXmlAssinado = "PastaXmlAssinado";
        public static string PastaXmlEnvio = "PastaXmlEnvio";
        public static string PastaXmlEmLote = "PastaXmlEmLote";
        public static string PastaXmlRetorno = "PastaXmlRetorno";
        public static string PastaXmlEnviado = "PastaXmlEnviado";
        public static string PastaXmlErro = "PastaXmlErro";
        public static string PastaBackup = "PastaBackup";
        public static string PastaValidar = "PastaValidar";
        public static string PastaDownloadNFeDest = "PastaDownloadNFeDest";
        public static string ConfiguracaoDanfe = "ConfiguracaoDanfe";
        public static string ConfiguracaoCCe = "ConfiguracaoCCe";
        public static string PastaExeUniDanfe = "PastaExeUniDanfe";
        public static string PastaConfigUniDanfe = "PastaConfigUniDanfe";
        public static string PastaDanfeMon = "PastaDanfeMon";
        public static string XMLDanfeMonNFe = "XMLDanfeMonNFe";
        public static string XMLDanfeMonProcNFe = "XMLDanfeMonProcNFe";
        public static string XMLDanfeMonDenegadaNFe = "XMLDanfeMonDenegadaNFe";

        public static string UsuarioWS = "UsuarioWS";
        public static string SenhaWS = "SenhaWS";

        public static string FTPAtivo = "FTPAtivo";
        public static string FTPGravaXMLPastaUnica = "FTPGravaXMLPastaUnica";
        public static string FTPSenha = "FTPSenha";
        public static string FTPPastaAutorizados = "FTPPastaAutorizados";
        public static string FTPPastaRetornos = "FTPPastaRetornos";
        public static string FTPNomeDoUsuario = "FTPNomeDoUsuario";
        public static string FTPNomeDoServidor = "FTPNomeDoServidor";
        public static string FTPPorta = "FTPPorta";

        public static string CertificadoDigital = "CertificadoDigital";
        public static string CertificadoDigitalThumbPrint = "CertificadoDigitalThumbPrint";
        public static string CertificadoInstalado = "CertificadoInstalado";
        public static string CertificadoArquivo = "CertificadoArquivo";
        public static string CertificadoSenha = "CertificadoSenha";
        public static string CertificadoPIN = "CertificadoPIN";
        public static string ProviderCertificado = "ProviderCertificado";
        public static string ProviderTypeCertificado = "ProviderTypeCertificado";

        public static string AmbienteCodigo = "AmbienteCodigo";
        public static string DiasLimpeza = "DiasLimpeza";
        public static string DiretorioSalvarComo = "DiretorioSalvarComo";
        public static string GravarRetornoTXTNFe = "GravarRetornoTXTNFe";
        public static string GravarEventosNaPastaEnviadosNFe = "GravarEventosNaPastaEnviadosNFe";
        public static string GravarEventosCancelamentoNaPastaEnviadosNFe = "GravarEventosCancelamentoNaPastaEnviadosNFe";
        public static string GravarEventosDeTerceiros = "GravarEventosDeTerceiros";
        public static string CompactarNfe = "CompactarNfe";
        public static string TempoConsulta = "TempoConsulta";
        public static string tpEmis = "tpEmis";
        public static string tpAmb = "tpAmb";
        public static string UnidadeFederativaCodigo = "UnidadeFederativaCodigo";
        public static string IndSinc = "IndSinc";
    }
}
