using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace NFe.Components
{
    /// <summary>
    /// Propriedades publicas
    /// </summary>
    public class Propriedade
    {
        public static Assembly AssemblyEXE;

        public static bool ExecutandoPeloUniNFe = true;

        public static string ServiceName = "UniNFeServico";

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
        public static string NomeArqEmpresas { get { return Propriedade.PastaExecutavel + "\\UniNfeEmpresa.xml"; } }
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
        private static string _PastaExecutavel = string.Empty;
        public static string PastaExecutavel
        {
            get
            {
                if (string.IsNullOrEmpty(_PastaExecutavel))
                    return System.IO.Path.GetDirectoryName(Application.ExecutablePath);

                return _PastaExecutavel;
            }
            set
            {
                _PastaExecutavel = value;
            }
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
        private static string _NomeArqXMLWebService_NFe = "";
        public static string NomeArqXMLWebService_NFe
        {
            get
            {
                if (string.IsNullOrEmpty(_NomeArqXMLWebService_NFe))
                    return Propriedade.PastaExecutavel + "\\NFe\\WSDL\\Webservice.xml";

                return _NomeArqXMLWebService_NFe;
            }
            set
            {
                _NomeArqXMLWebService_NFe = value;
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

        private static List<Municipio> _Estados = null;
        public static List<Municipio> Estados
        {
            get
            {
                if (_Estados == null)
                {
                    _Estados = new List<Components.Municipio>();

                    if (File.Exists(NomeArqXMLWebService_NFe))
                    {
                        XElement axml = XElement.Load(NomeArqXMLWebService_NFe);
                        var s = (from p in axml.Descendants(NFe.Components.NFeStrConstants.Estado)
                                 where (Int32)p.Attribute(NFe.Components.TpcnResources.ID.ToString()) < 900
                                 orderby p.Attribute(NFe.Components.NFeStrConstants.Nome).Value
                                 select new
                                {
                                    Nome = (string)p.Attribute(NFeStrConstants.Nome),
                                    ID = (Int32)p.Attribute(TpcnResources.ID.ToString()),
                                    UF = (string)p.Attribute(TpcnResources.UF.ToString()),
                                    SVC = (string)p.Attribute(NFeStrConstants.SVC)
                                });
                        foreach (var item in s)
                        {
                            _Estados.Add(new Municipio
                            {
                                CodigoMunicipio = item.ID,
                                Nome = item.Nome,
                                UF = item.UF,
                                svc = NFe.Components.EnumHelper.StringToEnum<TipoEmissao>(item.SVC)
                            });
                        }
                    }
                }
                return _Estados;
            }
            set { _Estados = value; }
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
        //public static string nsURI_nfe { get; set; }

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
            /// <summary>
            /// -ped-nfsepng.xml x -nfsepng.xml
            /// </summary>
            public const string PedNFSePNG = "-ped-nfsepng.xml";
            /// <summary>
            /// -ped-inunfse.xml x -inunfse.xml
            /// </summary>
            public const string PedInuNfse = "-ped-inunfse.xml";
            /// <summary>
            /// -ped-nfsepdf.xml x -nfsepdf.xml
            /// </summary>
            public const string PedNFSePDF = "-ped-nfsepdf.xml";
            #endregion

            #region Extensões MDF-e
            /// <summary>
            /// -mdfe.xml
            /// </summary>
            public static string MDFe = "-mdfe.xml";
            /// <summary>
            /// -ped-cons-mdfe-naoenc.xml
            /// </summary>
            public static string MDFeConsNaoEnc = "-ped-cons-mdfe-naoenc.xml";
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

            #region Extensões LMC
            /// <summary>
            /// "-lmc.xml"
            /// </summary>
            public static string LMC = "-lmc.xml";
            #endregion

            #region Extensões só para resolver um problema de compatibilidade de um usuário, com o tempo poderemos excluir. Wandrey 10/12/2014
            public const string cce_XML = "-cce.xml";
            public const string cancel_XML = "-cancel.xml";
            #endregion
        }
        #endregion

        #region Propriedades com as extensões dos XML ou TXT de retorno
        /// <summary>
        /// Classe com as propriedades com as extensões dos XML ou TXT de retorno
        /// </summary>
        public class ExtRetorno
        {
            //public const string RetAltCon_XML = "-ret-alt-con.xml";
            //public const string RetAltCon_TXT = "-ret-alt-con.txt";
            public const string RetGerarChaveNFe_XML = "-ret-gerar-chave.xml";
            public const string RetGerarChaveNFe_TXT = "-ret-gerar-chave.txt";
            public const string RetConsInf_XML = "-ret-cons-inf.xml";
            public const string RetConsInf_TXT = "-ret-cons-inf.txt";

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
            /// <summary>
            /// -ret-consmdfenaoenc.xml"
            /// </summary>
            public static string MDFeConsNaoEnc = "-ret-cons-mdfe-naoenc.xml";
            /// <summary>
            /// -ret-consmdfenaoenc.err
            /// </summary>
            public static string MDFeConsNaoEnc_ERR = "-ret-cons-mdfe-naoenc.err";
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
            /// -inu.xml
            /// </summary>
            public const string Inu_XML = "-inu.xml";
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
            /// <summary>
            /// -ped-nfsepng.xml x -nfsepng.xml
            /// </summary>
            public const string NFSePNG = "-nfsepng.xml";
            /// <summary>
            /// -ped-nfsepng.xml x -nfsepng.xml x nfsepng.err
            /// </summary>
            public const string NFSePNG_ERR = "-nfsepng.err";
            /// <summary>
            /// -ped-inunfse.xml x -inunfse.xml
            /// </summary>
            public const string InuNfse = "-inunfse.xml";
            /// <summary>
            /// -ped-inunfse.xml x -inunfse.xml x -inunfse.err
            /// </summary>
            public const string InuNfse_ERR = "-inunfse.err";
            /// <summary>
            /// -ped-nfsepng.xml x -nfsepng.xml
            /// </summary>
            public const string NFSePDF = "-nfsepdf.xml";
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

            #region Extensões do LMC
            /// <summary>
            /// -ret-lmc.xml
            /// </summary>
            public static string LMCRet = "-ret-lmc.xml";
            /// <summary>
            /// -ret-lmc.err
            /// </summary>
            public static string LMCRet_ERR = "-ret-lmc.err";
            /// <summary>
            /// -procLMC.xml
            /// </summary>
            public static string ProcLMC = "-procLMC.xml";
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
                    if (ServiceProcess.IsServiceInstalled(Propriedade.ServiceName))
                        return ServiceProcess.StatusService(Propriedade.ServiceName) == System.ServiceProcess.ServiceControllerStatus.Running;
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

        public static void VerificaArquivos(out bool error, out string msg)
        {
            switch (NFe.Components.Propriedade.TipoAplicativo)
            {
                case TipoAplicativo.MDFe:
                case TipoAplicativo.Cte:
                case TipoAplicativo.NFCe:
                case TipoAplicativo.Nfe:
                    error = !System.IO.File.Exists(Propriedade.NomeArqXMLWebService_NFe);
                    msg = "Arquivo '" + Propriedade.NomeArqXMLWebService_NFe + "' não encontrado";
                    break;
                case TipoAplicativo.Nfse:
                    error = !System.IO.File.Exists(Propriedade.NomeArqXMLMunicipios) || !System.IO.File.Exists(Propriedade.NomeArqXMLWebService_NFSe);
                    msg = "Arquivos '" + Propriedade.NomeArqXMLMunicipios + "' e/ou '" + Propriedade.NomeArqXMLWebService_NFSe + "' não encontrados";
                    break;
                default:
                    error = !System.IO.File.Exists(Propriedade.NomeArqXMLMunicipios) || !System.IO.File.Exists(Propriedade.NomeArqXMLWebService_NFSe) || !System.IO.File.Exists(Propriedade.NomeArqXMLWebService_NFe);
                    msg = "Arquivos '" + Propriedade.NomeArqXMLMunicipios + "', '" + Propriedade.NomeArqXMLWebService_NFSe + "' e/ou '" + Propriedade.NomeArqXMLWebService_NFe + "' não encontrados";
                    break;
            }
        }
    }
}
