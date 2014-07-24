using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using NFe.Components;

namespace NFe.Components
{
    public class WebServiceNFSe
    {
        private static List<string> _Padroes = null;

        /// <summary>
        /// lista de padrões usados para preencher o datagrid e pesquisas internas
        /// </summary>
        public static string[] PadroesNFSeList
        {
            get
            {
                if (_Padroes == null)
                {
                    Array arr = Enum.GetValues(typeof(PadroesNFSe));
                    _Padroes = new List<string>();
                    foreach (PadroesNFSe type in arr)
                        _Padroes.Add(type.ToString());
                }
                return _Padroes.ToArray();
            }
        }

        private static string getURLs(string local, PadroesNFSe padrao, int idMunicipio)
        {
            /*
             * tenta ler as URL's do 'WebService.xml'
             * não encontrando, assume o ID='padrao' e UF='XX' e padrao='padrao'
             */
#if para_quando_o_xml_estiver_atualizado
            if (System.IO.File.Exists(Propriedade.NomeArqXMLWebService))
            {
                XElement axml = XElement.Load(Propriedade.NomeArqXMLWebService);
                var s = (from p in axml.Descendents(NFe.Components.NFeStrConstants.Estado)
                         where (string)p.Attribute(NFe.Components.NFeStrConstants.Padrao) == padrao.ToString() &&
                                (string)p.Attribute(NFe.Components.NFeStrConstants.ID) == idMunicipio.ToString()
                         select p);
                foreach (var item in s)
                {
                    if (item.Element(local) != null)
                        return local.Equals(NFe.Components.NFeStrConstants.LocalHomologacao) ?
                            item.FirstNode.ToString() : item.LastNode.ToString();
                }

                var xs = (from p in axml.Descendents(NFe.Components.NFeStrConstants.Estado)
                          where (string)p.Attribute(NFe.Components.NFeStrConstants.Padrao) == padrao.ToString() &&
                                 (string)p.Attribute(NFe.Components.NFeStrConstants.UF) == "XX" &&
                                 (string)p.Attribute(NFe.Components.NFeStrConstants.ID) == padrao.ToString()
                          select p);
                foreach (var item in xs)
                {
                    if (item.Element(local) != null)
                        return local.Equals(NFe.Components.NFeStrConstants.LocalHomologacao) ?
                            item.FirstNode.ToString() : item.LastNode.ToString();
                }
            }
#endif
            return "";
        }

        public static string WebServicesHomologacao(PadroesNFSe padrao, int idMunicipio = 0)
        {
            string result = getURLs(NFe.Components.NFeStrConstants.LocalHomologacao, padrao, idMunicipio);
            if (result != "")
                return result;

            switch (padrao)
            {
                #region THEMA
                case PadroesNFSe.THEMA:
                    switch (idMunicipio)
                    {
                        case 4312401: //Monte Negro - RS 
                            return "<LocalHomologacao>" +
                                   @"<RecepcionarLoteRps>wsdl\homologacao\HMonteNegroRSRemessaNFSE.wsdl</RecepcionarLoteRps>" +
                                   @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HMonteNegroRSConsultarNFSE.wsdl</ConsultarSituacaoLoteRps>" +
                                   @"<ConsultarNfsePorRps>wsdl\homologacao\HMonteNegroRSConsultarNFSE.wsdl</ConsultarNfsePorRps>" +
                                   @"<ConsultarNfse>wsdl\homologacao\HMonteNegroRSConsultarNFSE.wsdl</ConsultarNfse>" +
                                   @"<ConsultarLoteRps>wsdl\homologacao\HMonteNegroRSConsultarNFSE.wsdl</ConsultarLoteRps>" +
                                   @"<CancelarNfse>wsdl\homologacao\HMonteNegroRSCancelarNFSE.wsdl</CancelarNfse>" +
                                   "</LocalHomologacao>";

                        case 4303103: // Cachoeirinha - RS
                            return "<LocalHomologacao>" +
                                   @"<RecepcionarLoteRps>wsdl\homologacao\HThemaCachoerinhaRSRemessa.wsdl</RecepcionarLoteRps>" +
                                   @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HThemaCachoerinhaRSConsulta.wsdl</ConsultarSituacaoLoteRps>" +
                                   @"<ConsultarNfsePorRps>wsdl\homologacao\HThemaCachoerinhaRSConsulta.wsdl</ConsultarNfsePorRps>" +
                                   @"<ConsultarNfse>wsdl\homologacao\HThemaCachoerinhaRSConsulta.wsdl</ConsultarNfse>" +
                                   @"<ConsultarLoteRps>wsdl\homologacao\HThemaCachoerinhaRSConsulta.wsdl</ConsultarLoteRps>" +
                                   @"<CancelarNfse>wsdl\homologacao\HThemaCachoerinhaRSCancelamento.wsdl</CancelarNfse>" +
                                   "</LocalHomologacao>";

                        case 4311403: //Lajeado - RS 
                            return "<LocalHomologacao>" +
                                   @"<RecepcionarLoteRps>wsdl\homologacao\HLajeadoRSRemessaNFSE.wsdl</RecepcionarLoteRps>" +
                                   @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HLajeadoRSConsultarNFSE.wsdl</ConsultarSituacaoLoteRps>" +
                                   @"<ConsultarNfsePorRps>wsdl\homologacao\HLajeadoRSConsultarNFSE.wsdl</ConsultarNfsePorRps>" +
                                   @"<ConsultarNfse>wsdl\homologacao\HLajeadoRSConsultarNFSE.wsdl</ConsultarNfse>" +
                                   @"<ConsultarLoteRps>wsdl\homologacao\HLajeadoRSConsultarNFSE.wsdl</ConsultarLoteRps>" +
                                   @"<CancelarNfse>wsdl\homologacao\HLajeadoRSCancelarNFSE.wsdl</CancelarNfse>" +
                                   "</LocalHomologacao>";

                        case 4321204: //Taquara - RS 
                            return "<LocalHomologacao>" +
                                   @"<RecepcionarLoteRps>wsdl\homologacao\HThemaTaquaraRSRemessa.wsdl</RecepcionarLoteRps>" +
                                   @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HThemaTaquaraRSConsulta.wsdl</ConsultarSituacaoLoteRps>" +
                                   @"<ConsultarNfsePorRps>wsdl\homologacao\HThemaTaquaraRSConsulta.wsdl</ConsultarNfsePorRps>" +
                                   @"<ConsultarNfse>wsdl\homologacao\HThemaTaquaraRSConsulta.wsdl</ConsultarNfse>" +
                                   @"<ConsultarLoteRps>wsdl\homologacao\HThemaTaquaraRSConsulta.wsdl</ConsultarLoteRps>" +
                                   @"<CancelarNfse>wsdl\homologacao\HThemaTaquaraRSCancelamento.wsdl</CancelarNfse>" +
                                   "</LocalHomologacao>";

                        case 4307708: //Esteio - RS 
                            return "<LocalHomologacao>" +
                                   @"<RecepcionarLoteRps>wsdl\homologacao\HThemaEsteioRSRemessa.wsdl</RecepcionarLoteRps>" +
                                   @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HThemaEsteioRSConsulta.wsdl</ConsultarSituacaoLoteRps>" +
                                   @"<ConsultarNfsePorRps>wsdl\homologacao\HThemaEsteioRSConsulta.wsdl</ConsultarNfsePorRps>" +
                                   @"<ConsultarNfse>wsdl\homologacao\HThemaEsteioRSConsulta.wsdl</ConsultarNfse>" +
                                   @"<ConsultarLoteRps>wsdl\homologacao\HThemaEsteioRSConsulta.wsdl</ConsultarLoteRps>" +
                                   @"<CancelarNfse>wsdl\homologacao\HThemaEsteioRSCancelamento.wsdl</CancelarNfse>" +
                                   "</LocalHomologacao>";

                        default:
                            return "<LocalHomologacao>" +
                                   @"<RecepcionarLoteRps>wsdl\homologacao\HThemaRemessa.wsdl</RecepcionarLoteRps>" +
                                   @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HThemaConsulta.wsdl</ConsultarSituacaoLoteRps>" +
                                   @"<ConsultarNfsePorRps>wsdl\homologacao\HThemaConsulta.wsdl</ConsultarNfsePorRps>" +
                                   @"<ConsultarNfse>wsdl\homologacao\HThemaConsulta.wsdl</ConsultarNfse>" +
                                   @"<ConsultarLoteRps>wsdl\homologacao\HThemaConsulta.wsdl</ConsultarLoteRps>" +
                                   @"<CancelarNfse>wsdl\homologacao\HThemaCancelamento.wsdl</CancelarNfse>" +
                                   "</LocalHomologacao>";
                    }

                #endregion

                #region GINFES
                case PadroesNFSe.GINFES:
                    return "<LocalHomologacao>" +
                            @"<RecepcionarLoteRps>wsdl\homologacao\hginfes.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\homologacao\hginfes.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\homologacao\hginfes.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\homologacao\hginfes.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\homologacao\hginfes.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\homologacao\hginfes.wsdl</CancelarNfse>" +
                            "</LocalHomologacao>";
                #endregion

                #region BETHA
                case PadroesNFSe.BETHA:
                    return "<LocalHomologacao>" +
                            @"<RecepcionarLoteRps>wsdl\homologacao\HBethaRecepcionarLoteRps.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HBethaConsultarSituacaoLoteRPS.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\homologacao\HBethaConsultarNFSePorRPS.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\homologacao\HBethaConsultarNFSe.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\homologacao\HBethaConsultarLoteRPS.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\homologacao\HBethaCancelarNFSe.wsdl</CancelarNfse>" +
                            "</LocalHomologacao>";
                #endregion

                #region SALVADOR_BA
                case PadroesNFSe.SALVADOR_BA:
                    return "<LocalHomologacao>" +
                            @"<RecepcionarLoteRps>wsdl\homologacao\HSalvadorBAEnvioLoteRPS.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HSalvadorBAConsultaSituacaoLoteRPS.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\homologacao\HSalvadorBAConsultaNfseRPS.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\homologacao\HSalvadorBAConsultaNfse.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\homologacao\HSalvadorBAConsultaLoteRPS.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\homologacao\HSalvadorBA.wsdl</CancelarNfse>" +
                            "</LocalHomologacao>";
                #endregion

                #region CANOAS_RS
                case PadroesNFSe.CANOAS_RS:
                    return "<LocalHomologacao>" +
                            @"<RecepcionarLoteRps>wsdl\homologacao\HCanoasRSRecepcionarLoteRps.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HCanoasRSConsultarSituacaoLoteRps.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\homologacao\HCanoasRSConsultarNfsePorRps.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\homologacao\HCanoasRSConsultarNfse.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\homologacao\HCanoasRSConsultarLoteRps.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\homologacao\HCanoasRSCancelarNfse.wsdl</CancelarNfse>" +
                            "</LocalHomologacao>";
                #endregion

                #region ISSNET
                case PadroesNFSe.ISSNET: // ISSNet fornece um unico WebService de homologacao para todos os municipios
                    return "<LocalHomologacao>" +
                            @"<RecepcionarLoteRps>wsdl\homologacao\HISSNet.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HISSNet.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\homologacao\HISSNet.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\homologacao\HISSNet.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\homologacao\HISSNet.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\homologacao\HISSNet.wsdl</CancelarNfse>" +
                            @"<ConsultarURLNfse>wsdl\homologacao\HISSNet.wsdl</ConsultarURLNfse>" +
                            "</LocalHomologacao>";


                #endregion

                #region ISSONLINE
                case PadroesNFSe.ISSONLINE:
                    switch (idMunicipio)
                    {
                        case 3502804: //Aracatuba - SP
                            return "<LocalHomologacao>" +
                                    @"<RecepcionarLoteRps>wsdl\homologacao\HISSOnLineAracatubaSP.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HISSOnLineAracatubaSP.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\homologacao\HISSOnLineAracatubaSP.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\homologacao\HISSOnLineAracatubaSP.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\homologacao\HISSOnLineAracatubaSP.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\homologacao\HISSOnLineAracatubaSP.wsdl</CancelarNfse>" +
                                    "</LocalHomologacao>";

                        default: //Apucarana - PR
                            return "<LocalHomologacao>" +
                                    @"<RecepcionarLoteRps>wsdl\homologacao\HISSOnLineApucarana.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HISSOnLineApucarana.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\homologacao\HISSOnLineApucarana.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\homologacao\HISSOnLineApucarana.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\homologacao\HISSOnLineApucarana.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\homologacao\HISSOnLineApucarana.wsdl</CancelarNfse>" +
                                    "</LocalHomologacao>";
                    }
                #endregion

                #region BLUMENAU_SC
                case PadroesNFSe.BLUMENAU_SC:
                    return "<LocalHomologacao>" +
                            @"<RecepcionarLoteRps>wsdl\homologacao\HBlumenauSC.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HBlumenauSC.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\homologacao\HBlumenauSC.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\homologacao\HBlumenauSC.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\homologacao\HBlumenauSC.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\homologacao\HBlumenauSC.wsdl</CancelarNfse>" +
                            "</LocalHomologacao>";
                #endregion

                #region BHISS
                case PadroesNFSe.BHISS:
                    if (idMunicipio == 3106200) //Belo Horizonte - MG
                    {
                        return "<LocalHomologacao>" +
                                @"<RecepcionarLoteRps>wsdl\homologacao\HBeloHorizonteMG-BHISS.wsdl</RecepcionarLoteRps>" +
                                @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HBeloHorizonteMG-BHISS.wsdl</ConsultarSituacaoLoteRps>" +
                                @"<ConsultarNfsePorRps>wsdl\homologacao\HBeloHorizonteMG-BHISS.wsdl</ConsultarNfsePorRps>" +
                                @"<ConsultarNfse>wsdl\homologacao\HBeloHorizonteMG-BHISS.wsdl</ConsultarNfse>" +
                                @"<ConsultarLoteRps>wsdl\homologacao\HBeloHorizonteMG-BHISS.wsdl</ConsultarLoteRps>" +
                                @"<CancelarNfse>wsdl\homologacao\HBeloHorizonteMG-BHISS.wsdl</CancelarNfse>" +
                                "</LocalHomologacao>";
                    }
                    else //Juiz de Fora - MG
                    {
                        return "<LocalHomologacao>" +
                                @"<RecepcionarLoteRps>wsdl\homologacao\HJuizdeForaMG.wsdl</RecepcionarLoteRps>" +
                                @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HJuizdeForaMG.wsdl</ConsultarSituacaoLoteRps>" +
                                @"<ConsultarNfsePorRps>wsdl\homologacao\HJuizdeForaMG.wsdl</ConsultarNfsePorRps>" +
                                @"<ConsultarNfse>wsdl\homologacao\HJuizdeForaMG.wsdl</ConsultarNfse>" +
                                @"<ConsultarLoteRps>wsdl\homologacao\HJuizdeForaMG.wsdl</ConsultarLoteRps>" +
                                @"<CancelarNfse>wsdl\homologacao\HJuizdeForaMG.wsdl</CancelarNfse>" +
                                "</LocalHomologacao>";
                    }
                #endregion

                #region GIF
                case PadroesNFSe.GIF:
                    if (idMunicipio == 4314050) // Parobé - RS
                    {
                        return "<LocalHomologacao>" +
                                @"<RecepcionarLoteRps>wsdl\homologacao\HParobeRSGIFServicos.wsdl</RecepcionarLoteRps>" +
                                @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HParobeRSGIFServicos.wsdl</ConsultarSituacaoLoteRps>" +
                                @"<ConsultarNfsePorRps>wsdl\homologacao\HParobeRSGIFServicos.wsdl</ConsultarNfsePorRps>" +
                                @"<ConsultarNfse>wsdl\homologacao\HParobeRSGIFServicos.wsdl</ConsultarNfse>" +
                                @"<ConsultarLoteRps>wsdl\homologacao\HParobeRSGIFServicos.wsdl</ConsultarLoteRps>" +
                                @"<CancelarNfse>wsdl\homologacao\HParobeRSGIFServicos.wsdl</CancelarNfse>" +
                                @"<ConsultarURLNfse>wsdl\homologacao\HParobeRSGIFServicos.wsdl</ConsultarURLNfse>" +
                                @"</LocalHomologacao>";
                    }
                    else
                    {
                        return "<LocalHomologacao>" +
                                @"<RecepcionarLoteRps>wsdl\homologacao\HCampoBomRS.wsdl</RecepcionarLoteRps>" +
                                @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HCampoBomRS.wsdl</ConsultarSituacaoLoteRps>" +
                                @"<ConsultarNfsePorRps>wsdl\homologacao\HCampoBomRS.wsdl</ConsultarNfsePorRps>" +
                                @"<ConsultarNfse>wsdl\homologacao\HCampoBomRS.wsdl</ConsultarNfse>" +
                                @"<ConsultarLoteRps>wsdl\homologacao\HCampoBomRS.wsdl</ConsultarLoteRps>" +
                                @"<CancelarNfse>wsdl\homologacao\HCampoBomRS.wsdl</CancelarNfse>" +
                                @"<ConsultarURLNfse>wsdl\homologacao\HCampoBomRS.wsdl</ConsultarURLNfse>" +
                                @"</LocalHomologacao>";
                    }
                #endregion

                #region DUETO
                case PadroesNFSe.DUETO:
                    switch (idMunicipio)
                    {
                        case 4310207: // Ijuí - RS
                            return "<LocalHomologacao>" +
                                    @"<RecepcionarLoteRps>wsdl\homologacao\HIjuiRS-Dueto.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HIjuiRS-Dueto.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\homologacao\HIjuiRS-Dueto.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\homologacao\HIjuiRS-Dueto.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\homologacao\HIjuiRS-Dueto.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\homologacao\HIjuiRS-Dueto.wsdl</CancelarNfse>" +
                                    @"</LocalHomologacao>";

                        case 4321709: // Tres Coroas - RS
                            return "<LocalHomologacao>" +
                                    @"<RecepcionarLoteRps>wsdl\homologacao\HTresCoroasRS-Dueto.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HTresCoroasRS-Dueto.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\homologacao\HTresCoroasRS-Dueto.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\homologacao\HTresCoroasRS-Dueto.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\homologacao\HTresCoroasRS-Dueto.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\homologacao\HTresCoroasRS-Dueto.wsdl</CancelarNfse>" +
                                    @"</LocalHomologacao>";

                        case 4302808: // Caçapava do Sul - RS
                            return "<LocalHomologacao>" +
                                    @"<RecepcionarLoteRps>wsdl\homologacao\HCacapavaRS-DUETOServices.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HCacapavaRS-DUETOServices.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\homologacao\HCacapavaRS-DUETOServices.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\homologacao\HCacapavaRS-DUETOServices.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\homologacao\HCacapavaRS-DUETOServices.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\homologacao\HCacapavaRS-DUETOServices.wsdl</CancelarNfse>" +
                                    @"</LocalHomologacao>";

                        case 4322400: // Uruguaiana - RS
                            return "<LocalHomologacao>" +
                                    @"<RecepcionarLoteRps>wsdl\homologacao\HUruguaianaRS-Dueto.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HUruguaianaRS-Dueto.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\homologacao\HUruguaianaRS-Dueto.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\homologacao\HUruguaianaRS-Dueto.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\homologacao\HUruguaianaRS-Dueto.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\homologacao\HUruguaianaRS-Dueto.wsdl</CancelarNfse>" +
                                    @"</LocalHomologacao>";


                        default: // Nova Santa Rita - RS
                            return "<LocalHomologacao>" +
                                    @"<RecepcionarLoteRps>wsdl\homologacao\HNovaSantaRitaRS-Dueto.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HNovaSantaRitaRS-Dueto.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\homologacao\HNovaSantaRitaRS-Dueto.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\homologacao\HNovaSantaRitaRS-Dueto.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\homologacao\HNovaSantaRitaRS-Dueto.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\homologacao\HNovaSantaRitaRS-Dueto.wsdl</CancelarNfse>" +
                                    @"</LocalHomologacao>";
                    }

                #endregion

                #region WEBISS
                case PadroesNFSe.WEBISS: // Feira de Santana - BA
                    return "<LocalHomologacao>" +
                            @"<RecepcionarLoteRps>wsdl\homologacao\HFeiradeSantanaBA_WebISS.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HFeiradeSantanaBA_WebISS.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\homologacao\HFeiradeSantanaBA_WebISS.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\homologacao\HFeiradeSantanaBA_WebISS.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\homologacao\HFeiradeSantanaBA_WebISS.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\homologacao\HFeiradeSantanaBA_WebISS.wsdl</CancelarNfse>" +
                            @"<ConsultarURLNfse>wsdl\homologacao\HFeiradeSantanaBA_WebISS.wsdl</ConsultarURLNfse>" +
                            @"</LocalHomologacao>";
                #endregion

                #region PAULISTANA
                case PadroesNFSe.PAULISTANA: // São Paulo - SP
                    return "<LocalHomologacao>" +
                            @"<RecepcionarLoteRps>wsdl\homologacao\HSaoPauloSP-PAULISTANA.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HSaoPauloSP-PAULISTANA.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\homologacao\HSaoPauloSP-PAULISTANA.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\homologacao\HSaoPauloSP-PAULISTANA.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\homologacao\HSaoPauloSP-PAULISTANA.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\homologacao\HSaoPauloSP-PAULISTANA.wsdl</CancelarNfse>" +
                            @"<ConsultarURLNfse>wsdl\homologacao\HSaoPauloSP-PAULISTANA.wsdl</ConsultarURLNfse>" +
                            @"</LocalHomologacao>";
                #endregion

                #region PORTOVELHENSE
                case PadroesNFSe.PORTOVELHENSE: // Porto Velho - RO
                    return "<LocalHomologacao>" +
                            @"<RecepcionarLoteRps>wsdl\homologacao\HPortoVelhoPO.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HPortoVelhoPO.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\homologacao\HPortoVelhoPO.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\homologacao\HPortoVelhoPO.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\homologacao\HPortoVelhoPO.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\homologacao\HPortoVelhoPO.wsdl</CancelarNfse>" +
                            @"<ConsultarURLNfse>wsdl\homologacao\HPortoVelhoPO.wsdl</ConsultarURLNfse>" +
                            @"</LocalHomologacao>";
                #endregion

                #region PRONIN
                case PadroesNFSe.PRONIN:
                    switch (idMunicipio)
                    {
                        case 3535804: // Paranapanema - SP
                            return "<LocalProducao>" +
                                    @"<RecepcionarLoteRps>wsdl\homologacao\HParanapanamaSP-PRONINServices.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\homologacao\HParanapanamaSP-PRONINServices.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\homologacao\HParanapanamaSP-PRONINServices.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\homologacao\HParanapanamaSP-PRONINServices.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\homologacao\HParanapanamaSP-PRONINServices.wsdl</CancelarNfse>" +
                                    @"</LocalProducao>";

                        default: // Mirassol - SP
                            return "<LocalHomologacao>" +
                                    @"<RecepcionarLoteRps>wsdl\homologacao\HMirassolSP.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\homologacao\HMirassolSP.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\homologacao\HMirassolSP.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\homologacao\HMirassolSP.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\homologacao\HMirassolSP.wsdl</CancelarNfse>" +
                                    @"</LocalHomologacao>";
                    }
                #endregion

                #region ISSONLINE4R (4R Sistemas)
                case PadroesNFSe.ISSONLINE4R: //Governador Valadares - MG
                    return "<LocalHomologacao>" +
                            @"<RecepcionarLoteRps>wsdl\homologacao\HGovernadorValadaresRecepcionarLoteRpsSincrono.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\homologacao\HGovernadorValadaresConsultarNfsePorRps.wsdl</ConsultarNfsePorRps>" +
                            @"<CancelarNfse>wsdl\homologacao\HGovernadorValadaresCancelarNfse.wsdl</CancelarNfse>" +
                            "</LocalHomologacao>";
                #endregion

                #region DSF
                case PadroesNFSe.DSF:
                    switch (idMunicipio)
                    {
                        case 3170206: // Urberlandia - MG
                            return "<LocalHomologacao>" +
                                   @"<RecepcionarLoteRps>wsdl\homologacao\HUberlandiaMGDFSLoteRps.wsdl</RecepcionarLoteRps>" +
                                   @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HUberlandiaMGDFSLoteRps.wsdl</ConsultarSituacaoLoteRps>" +
                                   @"<ConsultarNfsePorRps>wsdl\homologacao\HUberlandiaMGDFSLoteRps.wsdl</ConsultarNfsePorRps>" +
                                   @"<ConsultarNfse>wsdl\homologacao\HUberlandiaMGDFSLoteRps.wsdl</ConsultarNfse>" +
                                   @"<ConsultarLoteRps>wsdl\homologacao\HUberlandiaMGDFSLoteRps.wsdl</ConsultarLoteRps>" +
                                   @"<CancelarNfse>wsdl\homologacao\HUberlandiaMGDFSLoteRps.wsdl</CancelarNfse>" +
                                   @"</LocalHomologacao>";

                        case 3552205: // Sorocaba - SP
                            return "<LocalHomologacao>" +
                                   @"<RecepcionarLoteRps>wsdl\homologacao\HSorocabaSP-DFSLoteRps.wsdl</RecepcionarLoteRps>" +
                                   @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HSorocabaSP-DFSLoteRps.wsdl</ConsultarSituacaoLoteRps>" +
                                   @"<ConsultarNfsePorRps>wsdl\homologacao\HSorocabaSP-DFSLoteRps.wsdl</ConsultarNfsePorRps>" +
                                   @"<ConsultarNfse>wsdl\homologacao\HSorocabaSP-DFSLoteRps.wsdl</ConsultarNfse>" +
                                   @"<ConsultarLoteRps>wsdl\homologacao\HSorocabaSP-DFSLoteRps.wsdl</ConsultarLoteRps>" +
                                   @"<CancelarNfse>wsdl\homologacao\HSorocabaSP-DFSLoteRps.wsdl</CancelarNfse>" +
                                   @"</LocalHomologacao>";

                        case 3303500: //Nova Iguaçu - RJ
                            return "<LocalHomologacao>" +
                                   @"<RecepcionarLoteRps>wsdl\homologacao\HNovaIguacuRJ-DFSLoteRps.wsdl</RecepcionarLoteRps>" +
                                   @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HNovaIguacuRJ-DFSLoteRps.wsdl</ConsultarSituacaoLoteRps>" +
                                   @"<ConsultarNfsePorRps>wsdl\homologacao\HNovaIguacuRJ-DFSLoteRps.wsdl</ConsultarNfsePorRps>" +
                                   @"<ConsultarNfse>wsdl\homologacao\HNovaIguacuRJ-DFSLoteRps.wsdl</ConsultarNfse>" +
                                   @"<ConsultarLoteRps>wsdl\homologacao\HNovaIguacuRJ-DFSLoteRps.wsdl</ConsultarLoteRps>" +
                                   @"<CancelarNfse>wsdl\homologacao\HNovaIguacuRJ-DFSLoteRps.wsdl</CancelarNfse>" +
                                   @"</LocalHomologacao>";


                        default: // Campinas - SP
                            return "<LocalHomologacao>" +
                                    @"<RecepcionarLoteRps>wsdl\homologacao\HCampinasSP-DFSLoteRps.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HCampinasSP-DFSLoteRps.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\homologacao\HCampinasSP-DFSLoteRps.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\homologacao\HCampinasSP-DFSLoteRps.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\homologacao\HCampinasSP-DFSLoteRps.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\homologacao\HCampinasSP-DFSLoteRps.wsdl</CancelarNfse>" +
                                    @"</LocalHomologacao>";
                    }
                #endregion

                #region TENCNOSISTEMAS
                case PadroesNFSe.TECNOSISTEMAS: // Portão - RS
                    return "<LocalHomologacao>" +
                           @"<RecepcionarLoteRps>wsdl\homologacao\HPortaoRSEnvioLoteRPSSincrono.wsdl</RecepcionarLoteRps>" +
                           @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HPortaoRSConsultaNFSePorFaixa.wsdl</ConsultarSituacaoLoteRps>" +
                           @"<ConsultarNfsePorRps>wsdl\homologacao\HPortaoRSConsultaNFSePorRPS.wsdl</ConsultarNfsePorRps>" +
                           @"<ConsultarNfse>wsdl\homologacao\HPortaoRSConsultaNFSeServicosPrestados.wsdl</ConsultarNfse>" +
                           @"<ConsultarLoteRps>wsdl\homologacao\HPortaoRSConsultaLoteRPS.wsdl</ConsultarLoteRps>" +
                           @"<CancelarNfse>wsdl\homologacao\HPortaoRSCancelamentoNFSe.wsdl</CancelarNfse>" +
                           @"</LocalHomologacao>";
                #endregion

                #region SYSTEMPRO
                case PadroesNFSe.SYSTEMPRO: // Erechim - RS
                    return "<LocalHomologacao>" +
                           @"<RecepcionarLoteRps>wsdl\homologacao\HErechimRS-SystemPro.wsdl</RecepcionarLoteRps>" +
                           @"<ConsultarNfse>wsdl\homologacao\HErechimRS-SystemPro.wsdl</ConsultarNfse>" +
                           @"<CancelarNfse>wsdl\homologacao\HErechimRS-SystemPro.wsdl</CancelarNfse>" +
                           @"</LocalHomologacao>";
                #endregion

                default:
                    return "<LocalHomologacao></LocalHomologacao>";
            }
        }

        public static string WebServicesProducao(NFe.Components.PadroesNFSe padrao, int idMunicipio = 0)
        {
            string result = getURLs(NFe.Components.NFeStrConstants.LocalProducao, padrao, idMunicipio);
            if (result != "")
                return result;

            switch (padrao)
            {
                #region THEMA
                case PadroesNFSe.THEMA:
                    switch (idMunicipio)
                    {
                        case 4312401: // Monte Negro - RS
                            return "<LocalProducao>" +
                                   @"<RecepcionarLoteRps>wsdl\producao\PMonteNegroRSRemessaNFSE.wsdl</RecepcionarLoteRps>" +
                                   @"<ConsultarSituacaoLoteRps>wsdl\producao\PMonteNegroRSConsultarNFSE.wsdl</ConsultarSituacaoLoteRps>" +
                                   @"<ConsultarNfsePorRps>wsdl\producao\PMonteNegroRSConsultarNFSE.wsdl</ConsultarNfsePorRps>" +
                                   @"<ConsultarNfse>wsdl\producao\PMonteNegroRSConsultarNFSE.wsdl</ConsultarNfse>" +
                                   @"<ConsultarLoteRps>wsdl\producao\PMonteNegroRSConsultarNFSE.wsdl</ConsultarLoteRps>" +
                                   @"<CancelarNfse>wsdl\producao\PMonteNegroRSCancelarNFSE.wsdl</CancelarNfse>" +
                                   "</LocalProducao>";

                        case 4303103: // Cachoeirinha - RS
                            return "<LocalProducao>" +
                                   @"<RecepcionarLoteRps>wsdl\producao\PThemaCachoerinhaRSRemessa.wsdl</RecepcionarLoteRps>" +
                                   @"<ConsultarSituacaoLoteRps>wsdl\producao\PThemaCachoerinhaRSConsulta.wsdl</ConsultarSituacaoLoteRps>" +
                                   @"<ConsultarNfsePorRps>wsdl\producao\PThemaCachoerinhaRSConsulta.wsdl</ConsultarNfsePorRps>" +
                                   @"<ConsultarNfse>wsdl\producao\PThemaCachoerinhaRSConsulta.wsdl</ConsultarNfse>" +
                                   @"<ConsultarLoteRps>wsdl\producao\PThemaCachoerinhaRSConsulta.wsdl</ConsultarLoteRps>" +
                                   @"<CancelarNfse>wsdl\producao\PThemaCachoerinhaRSCancelamento.wsdl</CancelarNfse>" +
                                   "</LocalProducao>";

                        case 4311403: // Lajeado - RS
                            return "<LocalProducao>" +
                                   @"<RecepcionarLoteRps>wsdl\producao\PLajeadoRSRemessaNFSE.wsdl</RecepcionarLoteRps>" +
                                   @"<ConsultarSituacaoLoteRps>wsdl\producao\PLajeadoRSConsultarNFSE.wsdl</ConsultarSituacaoLoteRps>" +
                                   @"<ConsultarNfsePorRps>wsdl\producao\PLajeadoRSConsultarNFSE.wsdl</ConsultarNfsePorRps>" +
                                   @"<ConsultarNfse>wsdl\producao\PLajeadoRSConsultarNFSE.wsdl</ConsultarNfse>" +
                                   @"<ConsultarLoteRps>wsdl\producao\PLajeadoRSConsultarNFSE.wsdl</ConsultarLoteRps>" +
                                   @"<CancelarNfse>wsdl\producao\PLajeadoRSCancelarNFSE.wsdl</CancelarNfse>" +
                                   "</LocalProducao>";

                        case 4321204: // Taquara - RS
                            return "<LocalProducao>" +
                                   @"<RecepcionarLoteRps>wsdl\producao\PThemaTaquaraRSRemessa.wsdl</RecepcionarLoteRps>" +
                                   @"<ConsultarSituacaoLoteRps>wsdl\producao\PThemaTaquaraRSConsulta.wsdl</ConsultarSituacaoLoteRps>" +
                                   @"<ConsultarNfsePorRps>wsdl\producao\PThemaTaquaraRSConsulta.wsdl</ConsultarNfsePorRps>" +
                                   @"<ConsultarNfse>wsdl\producao\PThemaTaquaraRSConsulta.wsdl</ConsultarNfse>" +
                                   @"<ConsultarLoteRps>wsdl\producao\PThemaTaquaraRSConsulta.wsdl</ConsultarLoteRps>" +
                                   @"<CancelarNfse>wsdl\producao\PThemaTaquaraRSCancelamento.wsdl</CancelarNfse>" +
                                   "</LocalProducao>";

                        case 4307708: // Esteio - RS
                            return "<LocalProducao>" +
                                   @"<RecepcionarLoteRps>wsdl\producao\PThemaEsteioRSRemessa.wsdl</RecepcionarLoteRps>" +
                                   @"<ConsultarSituacaoLoteRps>wsdl\producao\PThemaEsteioRSConsulta.wsdl</ConsultarSituacaoLoteRps>" +
                                   @"<ConsultarNfsePorRps>wsdl\producao\PThemaEsteioRSConsulta.wsdl</ConsultarNfsePorRps>" +
                                   @"<ConsultarNfse>wsdl\producao\PThemaEsteioRSConsulta.wsdl</ConsultarNfse>" +
                                   @"<ConsultarLoteRps>wsdl\producao\PThemaEsteioRSConsulta.wsdl</ConsultarLoteRps>" +
                                   @"<CancelarNfse>wsdl\producao\PThemaEsteioRSCancelamento.wsdl</CancelarNfse>" +
                                   "</LocalProducao>";
                        default:
                            return "<LocalProducao>" +
                                   @"<RecepcionarLoteRps>wsdl\producao\PThemaRemessa.wsdl</RecepcionarLoteRps>" +
                                   @"<ConsultarSituacaoLoteRps>wsdl\producao\PThemaConsulta.wsdl</ConsultarSituacaoLoteRps>" +
                                   @"<ConsultarNfsePorRps>wsdl\producao\PThemaConsulta.wsdl</ConsultarNfsePorRps>" +
                                   @"<ConsultarNfse>wsdl\producao\PThemaConsulta.wsdl</ConsultarNfse>" +
                                   @"<ConsultarLoteRps>wsdl\producao\PThemaConsulta.wsdl</ConsultarLoteRps>" +
                                   @"<CancelarNfse>wsdl\producao\PThemaCancelamento.wsdl</CancelarNfse>" +
                                   "</LocalProducao>";
                    }
                #endregion

                #region GINFES
                case PadroesNFSe.GINFES:
                    return "<LocalProducao>" +
                            @"<RecepcionarLoteRps>wsdl\producao\pginfes.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\producao\pginfes.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\producao\pginfes.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\producao\pginfes.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\producao\pginfes.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\producao\pginfes.wsdl</CancelarNfse>" +
                            "</LocalProducao>";
                #endregion

                #region BETHA
                case PadroesNFSe.BETHA:
                    return "<LocalProducao>" +
                            @"<RecepcionarLoteRps>wsdl\producao\PBethaRecepcionarLoteRPS.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\producao\PBethaConsultarSituacaoLoteRPS.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\producao\PBethaConsultarNFSePorRPS.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\producao\PBethaConsultarNFSe.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\producao\PBethaConsultarLoteRPS.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\producao\PBethaCancelarNFSe.wsdl</CancelarNfse>" +
                            "</LocalProducao>";
                #endregion

                #region SALVADOR_BA
                case PadroesNFSe.SALVADOR_BA:
                    return "<LocalProducao>" +
                            @"<RecepcionarLoteRps>wsdl\producao\PSalvadorBAEnvioLoteRPS.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\producao\PSalvadorBAConsultaSituacaoLoteRPS.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\producao\PSalvadorBAConsultaNfseRPS.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\producao\PSalvadorBAConsultaNfse.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\producao\PSalvadorBAConsultaLoteRPS.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\producao\PSalvadorBA.wsdl</CancelarNfse>" +
                            "</LocalProducao>";
                #endregion

                #region CANOAS_RS
                case PadroesNFSe.CANOAS_RS:
                    return "<LocalProducao>" +
                            @"<RecepcionarLoteRps>wsdl\producao\PCanoasRSRecepcionarLoteRps.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\producao\PCanoasRSConsultarSituacaoLoteRps.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\producao\PCanoasRSConsultarNfsePorRps.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\producao\PCanoasRSConsultarNfse.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\producao\PCanoasRSConsultarLoteRps.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\producao\PCanoasRSCancelarNfse.wsdl</CancelarNfse>" +
                            "</LocalProducao>";
                #endregion

                #region ISSNET
                case PadroesNFSe.ISSNET:
                    switch (idMunicipio)
                    {
                        case 5201108: // Anapolis - GO
                            return "<LocalProducao>" +
                                @"<RecepcionarLoteRps>wsdl\producao\PISSNetAnapolis.wsdl</RecepcionarLoteRps>" +
                                @"<ConsultarSituacaoLoteRps>wsdl\producao\PISSNetAnapolis.wsdl</ConsultarSituacaoLoteRps>" +
                                @"<ConsultarNfsePorRps>wsdl\producao\PISSNetAnapolis.wsdl</ConsultarNfsePorRps>" +
                                @"<ConsultarNfse>wsdl\producao\PISSNetAnapolis.wsdl</ConsultarNfse>" +
                                @"<ConsultarLoteRps>wsdl\producao\PISSNetAnapolis.wsdl</ConsultarLoteRps>" +
                                @"<CancelarNfse>wsdl\producao\PISSNetAnapolis.wsdl</CancelarNfse>" +
                                @"<ConsultarURLNfse>wsdl\producao\PISSNetAnapolis.wsdl</ConsultarURLNfse>" +
                                "</LocalProducao>";

                        case 4316907: // Santa Maria - RS
                            return "<LocalProducao>" +
                                @"<RecepcionarLoteRps>wsdl\producao\PSantaMariaRS-ISSNet.wsdl</RecepcionarLoteRps>" +
                                @"<ConsultarSituacaoLoteRps>wsdl\producao\PSantaMariaRS-ISSNet.wsdl</ConsultarSituacaoLoteRps>" +
                                @"<ConsultarNfsePorRps>wsdl\producao\PSantaMariaRS-ISSNet.wsdl</ConsultarNfsePorRps>" +
                                @"<ConsultarNfse>wsdl\producao\PSantaMariaRS-ISSNet.wsdl</ConsultarNfse>" +
                                @"<ConsultarLoteRps>wsdl\producao\PSantaMariaRS-ISSNet.wsdl</ConsultarLoteRps>" +
                                @"<CancelarNfse>wsdl\producao\PSantaMariaRS-ISSNet.wsdl</CancelarNfse>" +
                                @"<ConsultarURLNfse>wsdl\producao\PSantaMariaRS-ISSNet.wsdl</ConsultarURLNfse>" +
                                "</LocalProducao>";

                        case 5103403: // Cuiabá - MT
                            return "<LocalProducao>" +
                                    @"<RecepcionarLoteRps>wsdl\producao\PCuiaba-MTISSNet.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\producao\PCuiaba-MTISSNet.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\producao\PCuiaba-MTISSNet.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\producao\PCuiaba-MTISSNet.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\producao\PCuiaba-MTISSNet.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\producao\PCuiaba-MTISSNet.wsdl</CancelarNfse>" +
                                    @"<ConsultarURLNfse>wsdl\producao\PCuiaba-MTISSNet.wsdl</ConsultarURLNfse>" +
                                "</LocalProducao>";

                        default: // Novo Hamburgo - RS (Default)
                            return "<LocalProducao>" +
                                    @"<RecepcionarLoteRps>wsdl\producao\PISSNetNovoHamburgo.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\producao\PISSNetNovoHamburgo.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\producao\PISSNetNovoHamburgo.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\producao\PISSNetNovoHamburgo.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\producao\PISSNetNovoHamburgo.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\producao\PISSNetNovoHamburgo.wsdl</CancelarNfse>" +
                                    @"<ConsultarURLNfse>wsdl\producao\PISSNetNovoHamburgo.wsdl</ConsultarURLNfse>" +
                                    "</LocalProducao>";
                    }
                #endregion

                #region ISSONLINE
                case PadroesNFSe.ISSONLINE:
                    switch (idMunicipio)
                    {
                        case 3502804: //Aracatuba - SP
                            return "<LocalProducao>" +
                                    @"<RecepcionarLoteRps>wsdl\producao\PISSOnLineAracatubaSP.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\producao\PISSOnLineAracatubaSP.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\producao\PISSOnLineAracatubaSP.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\producao\PISSOnLineAracatubaSP.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\producao\PISSOnLineAracatubaSP.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\producao\PISSOnLineAracatubaSP.wsdl</CancelarNfse>" +
                                    "</LocalProducao>";

                        case 3537305: //Penapolis - SP
                            return "<LocalProducao>" +
                                    @"<RecepcionarLoteRps>wsdl\producao\PPenapoisSPIssOnLine.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\producao\PPenapoisSPIssOnLine.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\producao\PPenapoisSPIssOnLine.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\producao\PPenapoisSPIssOnLine.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\producao\PPenapoisSPIssOnLine.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\producao\PPenapoisSPIssOnLine.wsdl</CancelarNfse>" +
                                    "</LocalProducao>";

                        default: //Apucarana - PR
                            return "<LocalProducao>" +
                                    @"<RecepcionarLoteRps>wsdl\producao\PISSOnLineApucarana.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\producao\PISSOnLineApucarana.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\producao\PISSOnLineApucarana.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\producao\PISSOnLineApucarana.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\producao\PISSOnLineApucarana.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\producao\PISSOnLineApucarana.wsdl</CancelarNfse>" +
                                    "</LocalProducao>";

                    }


                #endregion

                #region BLUMENAU_SC
                case PadroesNFSe.BLUMENAU_SC:
                    return "<LocalProducao>" +
                            @"<RecepcionarLoteRps>wsdl\producao\PBlumenauSC.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\producao\PBlumenauSC.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\producao\PBlumenauSC.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\producao\PBlumenauSC.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\producao\PBlumenauSC.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\producao\PBlumenauSC.wsdl</CancelarNfse>" +
                            "</LocalProducao>";
                #endregion

                #region BHISS
                case PadroesNFSe.BHISS:
                    if (idMunicipio == 3106200) //Belo Horizonte - MG
                    {
                        return "<LocalProducao>" +
                                @"<RecepcionarLoteRps>wsdl\producao\PBeloHorizonteMG-BHISS.wsdl</RecepcionarLoteRps>" +
                                @"<ConsultarSituacaoLoteRps>wsdl\producao\PBeloHorizonteMG-BHISS.wsdl</ConsultarSituacaoLoteRps>" +
                                @"<ConsultarNfsePorRps>wsdl\producao\PBeloHorizonteMG-BHISS.wsdl</ConsultarNfsePorRps>" +
                                @"<ConsultarNfse>wsdl\producao\PBeloHorizonteMG-BHISS.wsdl</ConsultarNfse>" +
                                @"<ConsultarLoteRps>wsdl\producao\PBeloHorizonteMG-BHISS.wsdl</ConsultarLoteRps>" +
                                @"<CancelarNfse>wsdl\producao\PBeloHorizonteMG-BHISS.wsdl</CancelarNfse>" +
                                "</LocalProducao>";
                    }
                    else //Juiz de Fora - MG
                    {
                        return "<LocalProducao>" +
                                @"<RecepcionarLoteRps>wsdl\producao\PJuizdeForaMG.wsdl</RecepcionarLoteRps>" +
                                @"<ConsultarSituacaoLoteRps>wsdl\producao\PJuizdeForaMG.wsdl</ConsultarSituacaoLoteRps>" +
                                @"<ConsultarNfsePorRps>wsdl\producao\PJuizdeForaMG.wsdl</ConsultarNfsePorRps>" +
                                @"<ConsultarNfse>wsdl\producao\PJuizdeForaMG.wsdl</ConsultarNfse>" +
                                @"<ConsultarLoteRps>wsdl\producao\PJuizdeForaMG.wsdl</ConsultarLoteRps>" +
                                @"<CancelarNfse>wsdl\producao\PJuizdeForaMG.wsdl</CancelarNfse>" +
                                "</LocalProducao>";
                    }
                #endregion

                #region GIF
                case PadroesNFSe.GIF:
                    if (idMunicipio == 4314050) // Parobé - RS
                    {
                        return "<LocalProducao>" +
                                @"<RecepcionarLoteRps>wsdl\producao\PParobeRSGIFServicos.wsdl</RecepcionarLoteRps>" +
                                @"<ConsultarSituacaoLoteRps>wsdl\producao\PParobeRSGIFServicos.wsdl</ConsultarSituacaoLoteRps>" +
                                @"<ConsultarNfsePorRps>wsdl\producao\PParobeRSGIFServicos.wsdl</ConsultarNfsePorRps>" +
                                @"<ConsultarNfse>wsdl\producao\PParobeRSGIFServicos.wsdl</ConsultarNfse>" +
                                @"<ConsultarLoteRps>wsdl\producao\PParobeRSGIFServicos.wsdl</ConsultarLoteRps>" +
                                @"<CancelarNfse>wsdl\producao\PParobeRSGIFServicos.wsdl</CancelarNfse>" +
                                @"<ConsultarURLNfse>wsdl\producao\PParobeRSGIFServicos.wsdl</ConsultarURLNfse>" +
                                @"</LocalProducao>";
                    }
                    else
                    {
                        return "<LocalProducao>" +
                                @"<RecepcionarLoteRps>wsdl\producao\PCampoBomRS.wsdl</RecepcionarLoteRps>" +
                                @"<ConsultarSituacaoLoteRps>wsdl\producao\PCampoBomRS.wsdl</ConsultarSituacaoLoteRps>" +
                                @"<ConsultarNfsePorRps>wsdl\producao\PCampoBomRS.wsdl</ConsultarNfsePorRps>" +
                                @"<ConsultarNfse>wsdl\producao\PCampoBomRS.wsdl</ConsultarNfse>" +
                                @"<ConsultarLoteRps>wsdl\producao\PCampoBomRS.wsdl</ConsultarLoteRps>" +
                                @"<CancelarNfse>wsdl\producao\PCampoBomRS.wsdl</CancelarNfse>" +
                                @"<ConsultarURLNfse>wsdl\producao\PCampoBomRS.wsdl</ConsultarURLNfse>" +
                                @"</LocalProducao>";
                    }
                #endregion

                #region DUETO
                case PadroesNFSe.DUETO:
                    switch (idMunicipio)
                    {
                        case 4310207: // Ijuí - RS
                            return "<LocalProducao>" +
                                    @"<RecepcionarLoteRps>wsdl\producao\PIjuiRS-Dueto.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\producao\PIjuiRS-Dueto.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\producao\PIjuiRS-Dueto.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\producao\PIjuiRS-Dueto.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\producao\PIjuiRS-Dueto.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\producao\PIjuiRS-Dueto.wsdl</CancelarNfse>" +
                                    @"</LocalProducao>";    //danasa 9-2013

                        case 4321709: // Tres Coroas - RS
                            return "<LocalProducao>" +
                                    @"<RecepcionarLoteRps>wsdl\producao\PTresCoroasRS-Dueto.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\producao\PTresCoroasRS-Dueto.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\producao\PTresCoroasRS-Dueto.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\producao\PTresCoroasRS-Dueto.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\producao\PTresCoroasRS-Dueto.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\producao\PTresCoroasRS-Dueto.wsdl</CancelarNfse>" +
                                    @"</LocalProducao>";

                        case 4322400: // Uruguaiana - RS
                            return "<LocalProducao>" +
                                    @"<RecepcionarLoteRps>wsdl\producao\PUruguaianaRS-Dueto.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\producao\PUruguaianaRS-Dueto.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\producao\PUruguaianaRS-Dueto.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\producao\PUruguaianaRS-Dueto.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\producao\PUruguaianaRS-Dueto.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\producao\PUruguaianaRS-Dueto.wsdl</CancelarNfse>" +
                                    @"</LocalProducao>";

                        case 4302808: // Caçapava do Sul - RS
                            return "<LocalProducao>" +
                                    @"<RecepcionarLoteRps>wsdl\producao\PCacapavaRS-DUETOServices.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\producao\PCacapavaRS-DUETOServices.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\producao\PCacapavaRS-DUETOServices.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\producao\PCacapavaRS-DUETOServices.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\producao\PCacapavaRS-DUETOServices.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\producao\PCacapavaRS-DUETOServices.wsdl</CancelarNfse>" +
                                    @"</LocalProducao>";

                        default: // Nova Santa Rita - RS
                            return "<LocalProducao>" +
                                    @"<RecepcionarLoteRps>wsdl\producao\PNovaSantaRitaRS-Dueto.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\producao\PNovaSantaRitaRS-Dueto.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\producao\PNovaSantaRitaRS-Dueto.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\producao\PNovaSantaRitaRS-Dueto.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\producao\PNovaSantaRitaRS-Dueto.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\producao\PNovaSantaRitaRS-Dueto.wsdl</CancelarNfse>" +
                                    @"</LocalProducao>";
                    }
                #endregion

                #region WEBISS
                case PadroesNFSe.WEBISS: // Feira de Santana - BA
                    return "<LocalProducao>" +
                            @"<RecepcionarLoteRps>wsdl\producao\PFeiradeSantanaBA_WebISS.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\producao\PFeiradeSantanaBA_WebISS.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\producao\PFeiradeSantanaBA_WebISS.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\producao\PFeiradeSantanaBA_WebISS.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\producao\PFeiradeSantanaBA_WebISS.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\producao\PFeiradeSantanaBA_WebISS.wsdl</CancelarNfse>" +
                            @"<ConsultarURLNfse>wsdl\producao\PFeiradeSantanaBA_WebISS.wsdl</ConsultarURLNfse>" +
                            @"</LocalProducao>";
                #endregion

                #region PAULISTANA
                case PadroesNFSe.PAULISTANA: // São Paulo - SP
                    return "<LocalProducao>" +
                            @"<RecepcionarLoteRps>wsdl\producao\PSaoPauloSP-PAULISTANA.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\producao\PSaoPauloSP-PAULISTANA.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\producao\PSaoPauloSP-PAULISTANA.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\producao\PSaoPauloSP-PAULISTANA.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\producao\PSaoPauloSP-PAULISTANA.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\producao\PSaoPauloSP-PAULISTANA.wsdl</CancelarNfse>" +
                            @"<ConsultarURLNfse>wsdl\producao\PSaoPauloSP-PAULISTANA.wsdl</ConsultarURLNfse>" +
                            @"</LocalProducao>";
                #endregion

                #region PORTOVELHENSE
                case PadroesNFSe.PORTOVELHENSE: // Porto Velho - RO
                    return "<LocalProducao>" +
                            @"<RecepcionarLoteRps>wsdl\producao\PPortoVelhoPO.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\producao\PPortoVelhoPO.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\producao\PPortoVelhoPO.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\producao\PPortoVelhoPO.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\producao\PPortoVelhoPO.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\producao\PPortoVelhoPO.wsdl</CancelarNfse>" +
                            @"<ConsultarURLNfse>wsdl\producao\PPortoVelhoPO.wsdl</ConsultarURLNfse>" +
                            @"</LocalProducao>";
                #endregion

                #region PRONIN
                case PadroesNFSe.PRONIN:
                    switch (idMunicipio)
                    {
                        case 3535804: // Paranapanema - SP
                            return "<LocalProducao>" +
                                    @"<RecepcionarLoteRps>wsdl\producao\PParanapanamaSP-PRONINServices.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\producao\PParanapanamaSP-PRONINServices.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\producao\PParanapanamaSP-PRONINServices.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\producao\PParanapanamaSP-PRONINServices.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\producao\PParanapanamaSP-PRONINServices.wsdl</CancelarNfse>" +
                                    @"</LocalProducao>";

                        default:  // Mirassol - SP
                            return "<LocalProducao>" +
                                    @"<RecepcionarLoteRps>wsdl\producao\PMirassolSP.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\producao\PMirassolSP.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\producao\PMirassolSP.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\producao\PMirassolSP.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\producao\PMirassolSP.wsdl</CancelarNfse>" +
                                    @"</LocalProducao>";

                    }
                #endregion

                #region ISSONLINE4R (4R Sistemas)
                case PadroesNFSe.ISSONLINE4R: //Governador Valadares - MG
                    return "<LocalProducao>" +
                            @"<RecepcionarLoteRps>wsdl\producao\PGovernadorValadaresRecepcionarLoteRpsSincrono.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\producao\PGovernadorValadaresConsultarNfsePorRps.wsdl</ConsultarNfsePorRps>" +
                            @"<CancelarNfse>wsdl\producao\PGovernadorValadaresCancelarNfse.wsdl</CancelarNfse>" +
                            "</LocalProducao>";
                #endregion

                #region DSF
                case PadroesNFSe.DSF:
                    switch (idMunicipio)
                    {
                        case 3170206: // Urberlandia - MG
                            return "<LocalProducao>" +
                                    @"<RecepcionarLoteRps>wsdl\producao\PUberlandiaMGDFSLoteRps.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\producao\PUberlandiaMGDFSLoteRps.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\producao\PUberlandiaMGDFSLoteRps.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\producao\PUberlandiaMGDFSLoteRps.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\producao\PUberlandiaMGDFSLoteRps.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\producao\PUberlandiaMGDFSLoteRps.wsdl</CancelarNfse>" +
                                    @"</LocalProducao>";

                        case 3552205: // Sorocaba - SP 
                            return "<LocalProducao>" +
                                    @"<RecepcionarLoteRps>wsdl\producao\PSorocabaSP-DFSLoteRps.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\producao\PSorocabaSP-DFSLoteRps.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\producao\PSorocabaSP-DFSLoteRps.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\producao\PSorocabaSP-DFSLoteRps.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\producao\PSorocabaSP-DFSLoteRps.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\producao\PSorocabaSP-DFSLoteRps.wsdl</CancelarNfse>" +
                                    @"</LocalProducao>";

                        case 3303500: //Nova Iguaçu - RS
                            return "<LocalProducao>" +
                                    @"<RecepcionarLoteRps>wsdl\producao\PNovaIguacuRJ-DFSLoteRps.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\producao\PNovaIguacuRJ-DFSLoteRps.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\producao\PNovaIguacuRJ-DFSLoteRps.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\producao\PNovaIguacuRJ-DFSLoteRps.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\producao\PNovaIguacuRJ-DFSLoteRps.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\producao\PNovaIguacuRJ-DFSLoteRps.wsdl</CancelarNfse>" +
                                    @"</LocalProducao>";

                        default: // Campinas - SP
                            return "<LocalProducao>" +
                                    @"<RecepcionarLoteRps>wsdl\producao\PCampinasSP-DFSLoteRps.wsdl</RecepcionarLoteRps>" +
                                    @"<ConsultarSituacaoLoteRps>wsdl\producao\PCampinasSP-DFSLoteRps.wsdl</ConsultarSituacaoLoteRps>" +
                                    @"<ConsultarNfsePorRps>wsdl\producao\PCampinasSP-DFSLoteRps.wsdl</ConsultarNfsePorRps>" +
                                    @"<ConsultarNfse>wsdl\producao\PCampinasSP-DFSLoteRps.wsdl</ConsultarNfse>" +
                                    @"<ConsultarLoteRps>wsdl\producao\PCampinasSP-DFSLoteRps.wsdl</ConsultarLoteRps>" +
                                    @"<CancelarNfse>wsdl\producao\PCampinasSP-DFSLoteRps.wsdl</CancelarNfse>" +
                                    @"</LocalProducao>";
                    }
                #endregion

                #region TENCNOSISTEMAS
                case PadroesNFSe.TECNOSISTEMAS: // Portão - RS
                    return "<LocalProducao>" +
                           @"<RecepcionarLoteRps>wsdl\producao\PPortaoRSEnvioLoteRPSSincrono.wsdl</RecepcionarLoteRps>" +
                           @"<ConsultarSituacaoLoteRps>wsdl\producao\PPortaoRSConsultaNFSePorFaixa.wsdl</ConsultarSituacaoLoteRps>" +
                           @"<ConsultarNfsePorRps>wsdl\producao\PPortaoRSConsultaNFSePorRPS.wsdl</ConsultarNfsePorRps>" +
                           @"<ConsultarNfse>wsdl\producao\PPortaoRSConsultaNFSeServicosPrestados.wsdl</ConsultarNfse>" +
                           @"<ConsultarLoteRps>wsdl\producao\PPortaoRSConsultaLoteRPS.wsdl</ConsultarLoteRps>" +
                           @"<CancelarNfse>wsdl\producao\PPortaoRSCancelamentoNFSe.wsdl</CancelarNfse>" +
                           @"</LocalProducao>";
                #endregion

                #region SYSTEMPRO
                case PadroesNFSe.SYSTEMPRO: // Erechim - RS
                    return "<LocalProducao>" +
                           @"<RecepcionarLoteRps>wsdl\producao\PErechimRS-SystemPro.wsdl</RecepcionarLoteRps>" +
                           @"<ConsultarNfse>wsdl\producao\PErechimRS-SystemPro.wsdl</ConsultarNfse>" +
                           @"<CancelarNfse>wsdl\producao\PErechimRS-SystemPro.wsdl</CancelarNfse>" +
                           @"</LocalProducao>";
                #endregion

                default:
                    return "<LocalProducao></LocalProducao>";
            }
        }

        public static NFe.Components.PadroesNFSe GetPadraoFromString(string padrao)
        {
            try
            {
                return (PadroesNFSe)EnumHelper.StringToEnum<PadroesNFSe>(padrao);
            }
            catch
            {
                return PadroesNFSe.NaoIdentificado;
            }
            /*
            Array arr = Enum.GetValues(typeof(PadroesNFSe));
            foreach (PadroesNFSe type in arr)
                if (padrao.ToLower() == type.ToString().ToLower())
                    return type;

            return PadroesNFSe.NaoIdentificado;*/
        }

        public static void SavePadrao(string uf, string cidade, int codigomunicipio, string padrao, bool forcaAtualizacao)
        {
            try
            {
                if (uf != null)
                {
                    Municipio mun = null;
                    for (int i = 0; i < Propriedade.Municipios.Count; ++i)
                        if (Propriedade.Municipios[i].CodigoMunicipio == codigomunicipio)
                        {
                            mun = Propriedade.Municipios[i];
                            break;
                        }

                    if (padrao == PadroesNFSe.NaoIdentificado.ToString() && mun != null)
                        Propriedade.Municipios.Remove(mun);

                    if (padrao != PadroesNFSe.NaoIdentificado.ToString())
                    {
                        if (mun != null)
                        {
                            ///
                            /// é o mesmo padrão definido?
                            /// o parametro "forcaAtualizacao" é "true" somente quando vier da aba "Municipios definidos"
                            /// desde que o datagrid atualiza automaticamente o membro "padrao" da classe "Municipio" quando ele é alterado.
                            if (mun.PadraoStr == padrao && !forcaAtualizacao)
                                return;

                            mun.Padrao = GetPadraoFromString(padrao);
                            mun.PadraoStr = padrao;
                        }
                        else
                            Propriedade.Municipios.Add(new Municipio(codigomunicipio, uf, cidade.Trim(), GetPadraoFromString(padrao)));
                    }
                }
                if (System.IO.File.Exists(Propriedade.NomeArqXMLMunicipios))
                {
                    ///
                    /// faz uma copia por segurança
                    if (System.IO.File.Exists(Propriedade.NomeArqXMLMunicipios + ".bck"))
                        System.IO.File.Delete(Propriedade.NomeArqXMLMunicipios + ".bck");
                    System.IO.File.Copy(Propriedade.NomeArqXMLMunicipios, Propriedade.NomeArqXMLMunicipios + ".bck");
                }

                /*
                <nfes_municipios>
                    <Registro ID="4125506" Nome="São José dos Pinais - PR" Padrao="GINFES" />
                </nfes_municipios>
                 */

                var xml = new XDocument(new XDeclaration("1.0", "utf-8", null));
                XElement elementos = new XElement("nfes_municipios");
                foreach (Municipio item in Propriedade.Municipios)
                {
                    elementos.Add(new XElement(NFe.Components.NFeStrConstants.Registro,
                                    new XAttribute(NFe.Components.NFeStrConstants.ID, item.CodigoMunicipio.ToString()),
                                    new XAttribute(NFe.Components.NFeStrConstants.Nome, item.Nome.Trim()),
                                    new XAttribute(NFe.Components.NFeStrConstants.Padrao, item.PadraoStr)));
                }
                xml.Add(elementos);
                xml.Save(Propriedade.NomeArqXMLMunicipios);
#if false
                XmlWriter oXmlGravar = null;
                XmlWriterSettings oSettings = new XmlWriterSettings();
                UTF8Encoding c = new UTF8Encoding(true);
                oSettings.Encoding = c;
                oSettings.Indent = true;
                oSettings.IndentChars = "";
                oSettings.NewLineOnAttributes = false;
                oSettings.OmitXmlDeclaration = false;
                oXmlGravar = XmlWriter.Create(Propriedade.NomeArqXMLMunicipios, oSettings);

                //Agora vamos gravar os dados
                oXmlGravar.WriteStartDocument();
                oXmlGravar.WriteStartElement("nfes_municipios");
                {
                    foreach (Municipio item in Propriedade.Municipios)
                    {
                        oXmlGravar.WriteStartElement(NFe.Components.NFeStrConstants.Registro);
                        {
                            oXmlGravar.WriteStartAttribute(NFe.Components.NFeStrConstants.ID);
                            oXmlGravar.WriteString(item.CodigoMunicipio.ToString());
                            oXmlGravar.WriteEndAttribute();

                            oXmlGravar.WriteStartAttribute(NFe.Components.NFeStrConstants.Nome);
                            oXmlGravar.WriteString(item.Nome);
                            oXmlGravar.WriteEndAttribute();

                            oXmlGravar.WriteStartAttribute(NFe.Components.NFeStrConstants.Padrao);
                            oXmlGravar.WriteString(item.PadraoStr);
                            oXmlGravar.WriteEndAttribute();
                        }
                        oXmlGravar.WriteEndElement();   //Registro
                    }
                }
                oXmlGravar.WriteEndElement(); //nfes_municipios
                oXmlGravar.WriteEndDocument();
                oXmlGravar.Flush();
                oXmlGravar.Close();
#endif
            }
            catch (Exception ex)
            {
                //recupera a copia feita se houve erro na criacao do XML de municipios
                if (System.IO.File.Exists(Propriedade.NomeArqXMLMunicipios + ".bck"))
                    Functions.Move(Propriedade.NomeArqXMLMunicipios + ".bck", Propriedade.NomeArqXMLMunicipios);
                throw ex;
            }
        }

        /// <summary>
        /// Responsavel pela gravacao do arquivo de municipios, caso nao exista
        /// </summary>
        public static void Start()
        {
            if (!System.IO.File.Exists(Propriedade.NomeArqXMLMunicipios) && 
                System.IO.File.Exists(Propriedade.NomeArqXMLWebService))
            {
                var xml = new XDocument(new XDeclaration("1.0", "utf-8", null));
                XElement elementos = new XElement("nfes_municipios");

                XElement axml = XElement.Load(Propriedade.NomeArqXMLWebService);
                var s = (from p in axml.Descendants(NFeStrConstants.Estado)
                         where  (string)p.Attribute(NFe.Components.NFeStrConstants.UF) != "XX"
                         select p);
                foreach (var item in s)
                {
                    string padrao = PadroesNFSe.NaoIdentificado.ToString();
                    if (item.Attribute(NFe.Components.NFeStrConstants.Padrao) != null)
                        padrao = item.Attribute(NFe.Components.NFeStrConstants.Padrao).Value;

                    /*
                    XmlNodeList urlList = estadoElemento.GetElementsByTagName(NFe.Components.NFeStrConstants.LocalHomologacao);
                    if (urlList.Count > 0)
                        ///
                        /// verifica qual o padrao com base nas url's
                        foreach (string p0 in PadroesNFSeList)
                            if (p0.ToLower() != PadroesNFSe.NaoIdentificado.ToString().ToLower())
                                if (urlList[0].ChildNodes[0].InnerText.ToLower().IndexOf(p0.ToLower()) >= 0)
                                {
                                    padrao = p0;
                                    break;
                                }
                    */
                    if (padrao != PadroesNFSe.NaoIdentificado.ToString())
                    {
                        string ID = item.Attribute(NFe.Components.NFeStrConstants.ID).Value;
                        string Nome = item.Attribute(NFe.Components.NFeStrConstants.Nome).Value;
                        string UF = item.Attribute(NFe.Components.NFeStrConstants.UF).Value;

                        elementos.Add(new XElement(NFe.Components.NFeStrConstants.Registro,
                                        new XAttribute(NFe.Components.NFeStrConstants.ID, ID),
                                        new XAttribute(NFe.Components.NFeStrConstants.Nome, Nome.Trim()),
                                        new XAttribute(NFe.Components.NFeStrConstants.Padrao, padrao)));
                    }
                }
                if (!elementos.IsEmpty)
                {
                    xml.Add(elementos);
                    xml.Save(Propriedade.NomeArqXMLMunicipios);
                }



#if false
                XmlWriter oXmlGravar = null;
                XmlWriterSettings oSettings = new XmlWriterSettings();
                UTF8Encoding c = new UTF8Encoding(true);
                oSettings.Encoding = c;
                oSettings.Indent = true;
                oSettings.IndentChars = "";
                oSettings.NewLineOnAttributes = false;
                oSettings.OmitXmlDeclaration = false;
                oXmlGravar = XmlWriter.Create(Propriedade.NomeArqXMLMunicipios, oSettings);
                //Agora vamos gravar os dados
                oXmlGravar.WriteStartDocument();
                oXmlGravar.WriteStartElement("nfes_municipios");

                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(Propriedade.NomeArqXMLWebService);
                    XmlNodeList estadoList = doc.GetElementsByTagName(NFeStrConstants.Estado);
                    foreach (XmlNode estadoNode in estadoList)
                    {
                        XmlElement estadoElemento = (XmlElement)estadoNode;
                        if (estadoElemento.Attributes.Count > 0)
                        {
                            if (estadoElemento.Attributes[2].Value != "XX")
                            {
                                int ID = Convert.ToInt32(estadoElemento.Attributes[0].Value);
                                string Nome = estadoElemento.Attributes[1].Value;
                                string UF = estadoElemento.Attributes[2].Value;

                                string padrao = PadroesNFSe.NaoIdentificado.ToString();
                                XmlNodeList urlList = estadoElemento.GetElementsByTagName(NFe.Components.NFeStrConstants.LocalHomologacao);
                                if (urlList.Count > 0)
                                    ///
                                    /// verifica qual o padrao com base nas url's
                                    foreach (string p0 in PadroesNFSeList)
                                        if (p0.ToLower() != PadroesNFSe.NaoIdentificado.ToString().ToLower())
                                            if (urlList[0].ChildNodes[0].InnerText.ToLower().IndexOf(p0.ToLower()) >= 0)
                                            {
                                                padrao = p0;
                                                break;
                                            }

                                if (padrao != PadroesNFSe.NaoIdentificado.ToString())
                                {
                                    oXmlGravar.WriteStartElement(NFe.Components.NFeStrConstants.Registro);
                                    {
                                        oXmlGravar.WriteStartAttribute(NFe.Components.NFeStrConstants.ID);
                                        oXmlGravar.WriteString(ID.ToString());
                                        oXmlGravar.WriteEndAttribute();

                                        oXmlGravar.WriteStartAttribute(NFe.Components.NFeStrConstants.Nome);
                                        oXmlGravar.WriteString(Nome);
                                        oXmlGravar.WriteEndAttribute();

                                        oXmlGravar.WriteStartAttribute(NFe.Components.NFeStrConstants.Padrao);
                                        oXmlGravar.WriteString(padrao);
                                        oXmlGravar.WriteEndAttribute();
                                    }
                                    oXmlGravar.WriteEndElement();   //Registro
                                }
                            }
                        }
                    }
                    oXmlGravar.WriteEndElement(); //nfes_municipios
                    oXmlGravar.WriteEndDocument();
                    oXmlGravar.Flush();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (oXmlGravar != null)
                        oXmlGravar.Close();
                }
#endif
            }
        }
    }
}
