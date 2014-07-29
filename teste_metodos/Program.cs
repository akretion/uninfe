using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using NFe.Components;

namespace teste_metodos
{
    class Program
    {
        static void Main(string[] args)
        {
            NFe.Components.Servicos servico = NFe.Components.Servicos.Nulo;
            NFe.Components.PadroesNFSe padrao = PadroesNFSe.NaoIdentificado;
#if nfes
            XElement axml = XElement.Load(@"E:\Usr\NFe\uninfe\a_uninfe\NFe.Components.Wsdl\NFse\WSDL\Webservice.xml");
            var s = (from p in axml.Descendants(NFe.Components.NFeStrConstants.Estado)
                        where (string)p.Attribute(NFe.Components.NFeStrConstants.UF) != "XX"
                        select p);
            foreach (var item in s)
            {
                //Console.WriteLine(item.Element(NFe.Components.NFeStrConstants.LocalHomologacao).ToString());

                taHomologacao = true;
                var x = XElement.Parse(item.Element(NFe.Components.NFeStrConstants.LocalHomologacao).ToString()).Elements();
                foreach (var xa in x)
                {
                    if (!string.IsNullOrEmpty(xa.Value))
                    {
                        padrao = NFe.Components.EnumHelper.StringToEnum<NFe.Components.PadroesNFSe>(item.Attribute("Padrao").Value);

                        if (padrao == PadroesNFSe.SYSTEMPRO || padrao == PadroesNFSe.IPM || padrao == PadroesNFSe.BETHA)
                            continue;

                        switch(xa.Name.ToString())
                        {
                            case "RecepcionarLoteRps":
                                servico = NFe.Components.Servicos.RecepcionarLoteRps;
                                break;
                            case "ConsultarSituacaoLoteRps":
                                servico = NFe.Components.Servicos.ConsultarSituacaoLoteRps;
                                break;
                            case "ConsultarNfsePorRps":
                                servico = NFe.Components.Servicos.ConsultarNfsePorRps;
                                break;
                            case "ConsultarNfse":
                                servico = NFe.Components.Servicos.ConsultarNfse;
                                break;
                            case "ConsultarLoteRps":
                                servico = NFe.Components.Servicos.ConsultarLoteRps;
                                break;
                            case "CancelarNfse":
                                servico = NFe.Components.Servicos.CancelarNfse;
                                break;
                            case "ConsultarURLNfseSerie":
                                servico = Servicos.ConsultarURLNfseSerie;
                                break;
                            case "ConsultarURLNfse":
                                servico = Servicos.ConsultarURLNfse;
                                break;
                            default:
                                Console.WriteLine("====>(" + xa.Name.ToString()+")");
                                break;
                        }
                        if (servico == Servicos.Nulo)
                        {
                            Console.WriteLine("=========================================="
                                + " => " + item.Attribute("ID").Value
                                                    + "=>" + item.Attribute("Padrao").Value
                                                    + "=>" + xa.Name
                                                    + "=>" + xa.Value);
                        }
                        else
                        {
                            try
                            {
                                var proxy = new NFe.Components.WebServiceProxy(@"E:\Usr\NFe\uninfe\a_uninfe\NFe.Components.Wsdl\NFse\" + xa.Value, 
                                    null, padrao, taHomologacao, 0, "", servico);

                                if (NomeClasseWSNFSe(servico, padrao) != proxy.NomeClasseWS)
                                    Console.WriteLine("..."+servico.ToString()
                                                        + " => " + item.Attribute("ID").Value
                                                        + "=>" + item.Attribute("Padrao").Value
                                                        + "=>" + xa.Name
                                                        + "=>" + xa.Value
                                                        + "\r\n==>" + proxy.NomeClasseWS + " X " + NomeClasseWSNFSe(servico, padrao) + "---------");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("====\r\n" + xa.Value + "\r\n" + ex.Message);
                            }
                            //Console.WriteLine(NomeClasseWSNFSe(servico, padrao) + "==>" + proxy.NomeClasseWS);
                        }
                    }
                }
            }
#else
            XElement axml = XElement.Load(@"E:\Usr\NFe\uninfe\a_uninfe\NFe.Components.Wsdl\NFe\WSDL\Webservice.xml");
            var s = (from p in axml.Descendants(NFe.Components.NFeStrConstants.Estado)
                        where (string)p.Attribute(NFe.Components.NFeStrConstants.UF) != "XX"
                        select p);
            foreach (var item in s)
            {
                //Console.WriteLine(item.Element(NFe.Components.NFeStrConstants.LocalHomologacao).ToString());
                var x = XElement.Parse(item.Element(NFe.Components.NFeStrConstants.LocalHomologacao).ToString()).Elements();
                foreach (var xa in x)
                {
                    if (!string.IsNullOrEmpty(xa.Value))
                    {
                        //Console.WriteLine(xa.Name + " => " + xa.Value);
                        servico = Servicos.Nulo;
                        string versao = "2.00";
                        switch (xa.Name.ToString())
                        {
                            case "CTeRecepcaoEvento":
                                servico = Servicos.RecepcaoEventoCTe;
                                versao = NFe.ConvertTxt.versoes.VersaoXMLCTeEvento;
                                break;
                            case "CTeRecepcao":
                                servico = Servicos.EnviarLoteCTe;
                                versao = NFe.ConvertTxt.versoes.VersaoXMLCTe;
                                break;
                            case "CTeRetRecepcao":
                                servico = Servicos.PedidoSituacaoLoteCTe;
                                break;
                            case "CTeInutilizacao":
                                servico = Servicos.InutilizarNumerosCTe;
                                versao = NFe.ConvertTxt.versoes.VersaoXMLCTeInut;
                                break;
                            case "CTeConsulta":
                                servico = Servicos.PedidoConsultaSituacaoCTe;
                                break;
                            case "CTeStatusServico":
                                servico = Servicos.ConsultaStatusServicoCTe;
                                versao = NFe.ConvertTxt.versoes.VersaoXMLCTeStatusServico;
                                break;
                            case "CTeConsultaCadastro": break;

                            case "NFeRecepcao": 
                                servico = Servicos.EnviarLoteNfe; break;
                            case "NFeRetRecepcao": servico = Servicos.PedidoSituacaoLoteNFe; break;
                            case "NFeInutilizacao":
                                versao = NFe.ConvertTxt.versoes.VersaoXMLInut;
                                servico = Servicos.InutilizarNumerosNFe; break;
                            case "NFeConsulta": servico = Servicos.PedidoConsultaSituacaoNFe; break;
                            case "NFeStatusServico": 
                                servico = Servicos.ConsultaStatusServicoNFe; 
                                versao = NFe.ConvertTxt.versoes.VersaoXMLStatusServico; 
                                break;
                            case "NFeConsultaCadastro": 
                                servico = Servicos.ConsultaCadastroContribuinte;
                                versao = NFe.ConvertTxt.versoes.VersaoXMLConsCad;
                                break;
                            case "NFeRecepcaoEvento": 
                                servico = Servicos.RecepcaoEvento; 
                                versao = NFe.ConvertTxt.versoes.VersaoXMLEvento; 
                                break;
                            case "NFeConsultaNFeDest":
                                versao = NFe.ConvertTxt.versoes.VersaoXMLEnvConsultaNFeDest;
                                servico = Servicos.ConsultaNFDest; 
                                break;
                            case "NFeDownload": servico = Servicos.DownloadNFe; break;
                            case "NFeManifDest": 
                                servico = Servicos.EnviarManifDest; 
                                versao = NFe.ConvertTxt.versoes.VersaoXMLEvento; 
                                break;
                            case "NFeAutorizacao": servico = Servicos.EnviarLoteNfeZip2; break;
                            case "NFeRetAutorizacao": servico = Servicos.PedidoSituacaoLoteNFe2; break;

                            case "MDFeRecepcao": servico = Servicos.EnviarLoteMDFe;
                                versao = NFe.ConvertTxt.versoes.VersaoXMLMDFe;
                                break;
                            case "MDFeRetRecepcao": 
                                servico = Servicos.PedidoSituacaoLoteMDFe;
                                versao = NFe.ConvertTxt.versoes.VersaoXMLMDFe;
                                break;
                            case "MDFeConsulta": 
                                servico = Servicos.PedidoConsultaSituacaoMDFe;
                                versao = NFe.ConvertTxt.versoes.VersaoXMLMDFe;
                                break;
                            case "MDFeStatusServico": 
                                servico = Servicos.ConsultaStatusServicoMDFe;
                                versao = NFe.ConvertTxt.versoes.VersaoXMLMDFeStatusServico;
                                break;
                            case "MDFeRecepcaoEvento": 
                                servico = Servicos.RecepcaoEventoMDFe;
                                versao = NFe.ConvertTxt.versoes.VersaoXMLMDFeEvento;
                                break;

                            default:
                                servico = Servicos.Nulo;
                                Console.WriteLine("====>(" + xa.Name.ToString()+")");
                                break;
                        }
                        if (servico == Servicos.Nulo) continue;

                        try
                        {
                            if (xa.Value.Contains("\\DPEC\\"))
                                if (xa.Value.Contains("DEPCSCEConsultaRFB"))
                                    servico = Servicos.ConsultarDPEC;
                                else
                                    servico = Servicos.EnviarDPEC;

                            var proxy = new NFe.Components.WebServiceProxy(@"E:\Usr\NFe\uninfe\a_uninfe\NFe.Components.Wsdl\NFe\" + xa.Value,
                                null, padrao, taHomologacao, 
                                Convert.ToInt16(item.Attribute("ID").Value),
                                versao,
                                servico);


                            var n = NomeClasseWSNFe(servico, Convert.ToInt16(item.Attribute("ID").Value), versao);
                            if (n != proxy.NomeClasseWS)
                            {
                                Console.WriteLine(versao +": "+n + " X " + proxy.NomeClasseWS+ " ("+xa.Value+") "+xa.Name);
                                n = NomeClasseWSNFe(servico, Convert.ToInt16(item.Attribute("ID").Value), "3.10");
                                if (n != proxy.NomeClasseWS)
                                {
                                    n = NomeClasseWSNFe(servico, Convert.ToInt16(item.Attribute("ID").Value), "1.00");
                                    if (n != proxy.NomeClasseWS)
                                    {
                                        //Console.WriteLine("1.00: " + n + " X " + proxy.NomeClasseWS);
                                    }
                                    //Console.WriteLine("3.10: " + n + " X " + proxy.NomeClasseWS);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine("====\r\n" + xa.Value + "\r\n" + ex.Message);
                        }
                    }
                }
            }
#endif
        }

        private static string NomeClasseWSNFe(Servicos servico, int cUF, string versao)
        {
            string retorna = string.Empty;
            string cUFeVersao = cUF.ToString().Trim() + "|" + versao.Trim();

            switch (servico)
            {
                case Servicos.ConsultarDPEC:
                    retorna = "SCEConsultaRFB";
                    break;
                case Servicos.EnviarDPEC:
                    retorna = "SCERecepcaoRFB";
                    break;
                case Servicos.RecepcaoEvento:
                case Servicos.EnviarEventoCancelamento:
                case Servicos.EnviarManifDest:
                case Servicos.EnviarCCe:    //danasa 2/7/2011
                case Servicos.EnviarEPEC:
                    retorna = "RecepcaoEvento";break;
                case Servicos.ConsultaNFDest:
                    retorna = "NFeConsultaDest";break;
                case Servicos.DownloadNFe:
                    retorna = "NfeDownloadNF";break;
                #region NF-e
                case Servicos.InutilizarNumerosNFe:
                    retorna = "NfeInutilizacao2";
                    break;
                case Servicos.PedidoConsultaSituacaoNFe:
                    retorna = "NfeConsulta2";
                    break;
                case Servicos.ConsultaStatusServicoNFe:
                    switch (cUFeVersao)
                    {
                        case "29|3.10": //Bahia - XML versão 3.10
                            retorna = "NfeStatusServico";
                            break;
                        default:
                            retorna = "NfeStatusServico2";
                            break;
                    }
                    break;
                case Servicos.PedidoSituacaoLoteNFe:
                    retorna = "NfeRetRecepcao2";
                    break;
                case Servicos.PedidoSituacaoLoteNFe2:
                    retorna = "NfeRetAutorizacao";
                    break;
                case Servicos.ConsultaCadastroContribuinte:
                    retorna = "CadConsultaCadastro2";
                    break;
                case Servicos.EnviarLoteNfe:
                    retorna = "NfeRecepcao2";
                    break;
                case Servicos.EnviarLoteNfe2:
                    retorna = "NfeAutorizacao";
                    break;
                case Servicos.EnviarLoteNfeZip2:
                    retorna = "NfeAutorizacao";
                    break;

                #endregion

                #region MDF-e
                case Servicos.ConsultaStatusServicoMDFe:
                    retorna = "MDFeStatusServico";
                    break;
                case Servicos.EnviarLoteMDFe:
                    retorna = "MDFeRecepcao";
                    break;
                case Servicos.PedidoSituacaoLoteMDFe:
                    retorna = "MDFeRetRecepcao";
                    break;
                case Servicos.PedidoConsultaSituacaoMDFe:
                    retorna = "MDFeConsulta";
                    break;
                case Servicos.RecepcaoEventoMDFe:
                    retorna = "MDFeRecepcaoEvento";
                    break;
                #endregion

                #region CT-e
                case Servicos.ConsultaStatusServicoCTe:
                    retorna = "CteStatusServico";
                    break;
                case Servicos.EnviarLoteCTe:
                    retorna = "CteRecepcao";
                    break;
                case Servicos.PedidoSituacaoLoteCTe:
                    retorna = "CteRetRecepcao";
                    break;
                case Servicos.PedidoConsultaSituacaoCTe:
                    retorna = "CteConsulta";
                    break;
                case Servicos.InutilizarNumerosCTe:
                    retorna = "CteInutilizacao";
                    break;
                case Servicos.RecepcaoEventoCTe:
                    if (cUF == 31) //Minas Gerais (MG)
                        retorna = "RecepcaoEvento";
                    else
                        retorna = "CteRecepcaoEvento";
                    break;
                #endregion
            }

            return retorna;
        }

        private static bool taHomologacao;
        private static string NomeClasseWSNFSe(Servicos servico, PadroesNFSe p)// int cMunicipio)
        {
            string retorna = string.Empty;
            //bool taHomologacao = true;// (Empresas.Configuracoes[Empresas.FindEmpresaByThread()].AmbienteCodigo == (int)NFe.Components.TipoAmbiente.taHomologacao);

            switch (p)//Functions.PadraoNFSe(cMunicipio))
            {
                #region GINFES
                case PadroesNFSe.GINFES:
                    retorna = "ServiceGinfesImplService";
                    break;
                #endregion

                #region THEMA
                case PadroesNFSe.THEMA:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "NFSEconsulta";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "NFSEconsulta";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "NFSEconsulta";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "NFSEconsulta";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "NFSEcancelamento";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "NFSEremessa";
                            break;
                    }
                    break;
                #endregion

                #region BETHA
                case PadroesNFSe.BETHA:
                    switch (servico)
                    {
                        case Servicos.ConsultarNfse:
                            retorna = "";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "";
                            break;
                        case Servicos.ConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region CANOAS-RS (ABACO)
                case PadroesNFSe.CANOAS_RS:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "ConsultarNfse";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "RecepcionarLoteRPS";
                            break;
                    }
                    break;
                #endregion

                #region ISSNet
                case PadroesNFSe.ISSNET:
                    retorna = "Servicos";
                    break;
                #endregion

                #region ISSNet
                case PadroesNFSe.ISSONLINE:
                    retorna = "Nfse";
                    break;
                #endregion

                #region Blumenau-SC
                case PadroesNFSe.BLUMENAU_SC:
                    retorna = "LoteNFe";
                    break;
                #endregion

                #region BHISS
                case PadroesNFSe.BHISS:
                    retorna = "NfseWSService";
                    break;

                #endregion

                #region GIF
                case PadroesNFSe.GIF:
                    retorna = "ServicosService";
                    break;

                #endregion

                #region DUETO
                case PadroesNFSe.DUETO:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "basic_INFSEConsultas";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "basic_INFSEConsultas";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "basic_INFSEConsultas";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "basic_INFSEConsultas";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "basic_INFSEGeracao";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "basic_INFSEGeracao";
                            break;
                    }
                    break;
                #endregion

                #region WEBISS
                case PadroesNFSe.WEBISS:
                    retorna = "NfseServices";
                    break;

                #endregion

                #region PAULISTANA
                case PadroesNFSe.PAULISTANA:
                    retorna = "LoteNFe";
                    break;

                #endregion

                #region SALVADOR_BA
                case PadroesNFSe.SALVADOR_BA:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "ConsultaLoteRPS";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "ConsultaNfse";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "ConsultaNfseRPS";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "ConsultaSituacaoLoteRPS";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "EnvioLoteRPS";
                            break;
                    }
                    break;
                #endregion

                #region PORTOVELHENSE
                case PadroesNFSe.PORTOVELHENSE:
                    retorna = "NfseWSService";
                    break;

                #endregion

                #region PRONIN
                case PadroesNFSe.PRONIN:
                    switch (servico)
                    {
                        case Servicos.CancelarNfse:
                            retorna = "basic_INFSEGeracao";
                            break;

                        case Servicos.RecepcionarLoteRps:
                            retorna = "basic_INFSEGeracao";
                            break;

                        default:
                            retorna = "basic_INFSEConsultas";
                            break;
                    }
                    break;
                #endregion

                #region ISSONLINE4R (4R Sistemas)
                case PadroesNFSe.ISSONLINE4R:
                    switch (servico)
                    {
                        case Servicos.ConsultarNfsePorRps:
                            if (taHomologacao)
                                retorna = "hConsultarNfsePorRps";
                            else
                                retorna = "ConsultarNfsePorRps";
                            break;

                        case Servicos.CancelarNfse:
                            if (taHomologacao)
                                retorna = "hCancelarNfse";
                            else
                                retorna = NFe.Components.Servicos.CancelarNfse.ToString();
                            break;

                        case Servicos.RecepcionarLoteRps:
                            if (taHomologacao)
                                retorna = "hRecepcionarLoteRpsSincrono";
                            else
                                retorna = "RecepcionarLoteRpsSincrono";
                            break;
                    }
                    break;
                #endregion

                #region DSF
                case PadroesNFSe.DSF:
                    retorna = "LoteRpsService";
                    break;

                #endregion

                #region TECNOSISTEMAS
                case PadroesNFSe.TECNOSISTEMAS:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "ConsultaLoteRPS";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "ConsultaNFSeServicosPrestados";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "ConsultaNFSePorRPS";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "ConsultaNFSePorFaixa";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "CancelamentoNFSe";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "EnvioLoteRPSSincrono";
                            break;
                    }
                    break;
                #endregion
            }

            return retorna;
        }

    }
}
