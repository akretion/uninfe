using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Xml;
using NFe.Components;
using NFe.Settings;
using NFe.Certificado;
using NFe.Exceptions;
using NFe.Validate;
using NFe.Components.QRCode;

namespace NFe.Service
{
    public abstract class TaskAbst
    {
        #region Objetos
        protected Auxiliar oAux = new Auxiliar();
        protected InvocarObjeto oInvocarObj = new InvocarObjeto();
        protected GerarXML oGerarXML = new GerarXML(Thread.CurrentThread);
        #endregion

        #region Propriedades

        /// <summary>
        /// Conteúdo do XML de retorno do serviço, ou seja, para cada serviço invocado a classe seta neste atributo a string do XML Retornado pelo serviço
        /// </summary>
        public string vStrXmlRetorno { get; set; }

        /// <summary>
        /// Pasta/Nome do arquivo XML contendo os dados a serem enviados (Nota Fiscal, Pedido de Status, Cancelamento, etc...)
        /// </summary>
        private string mNomeArquivoXML;
        public string NomeArquivoXML
        {
            get
            {
                return this.mNomeArquivoXML;
            }
            set
            {
                this.mNomeArquivoXML = value;
                oGerarXML.NomeXMLDadosMsg = value;
            }
        }

        /// <summary>
        /// Serviço que está sendo executado (Envio de Nota, Cancelamento, Consulta, etc...)
        /// </summary>
        private Servicos mServico;
        public Servicos Servico
        {
            get
            {
                return this.mServico;
            }
            protected set
            {
                this.mServico = value;
                oGerarXML.Servico = value;
            }
        }

        /// <summary>
        /// Se o vXmlNFeDadosMsg é um XML
        /// </summary>
        public bool vXmlNfeDadosMsgEhXML    //danasa 12-9-2009
        {
            get { return Path.GetExtension(NomeArquivoXML).ToLower() == ".xml"; }
        }

        #endregion

        public abstract void Execute();

        #region Métodos para definição dos nomes das classes e métodos da NFe, CTe, NFSe e MDFe
#if DEBUG
        #region NomeClasseWS()
        /// <summary>
        /// Retorna o nome da classe do serviço passado por parâmetro do WebService do SEFAZ - CTe
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cUF">Código da UF</param>
        /// <param name="versao">Versão do XML</param>
        /// <returns>Nome da classe</returns>
        protected string xNomeClasseWS(Servicos servico, int cUF, string versao)
        {
            string retorna = string.Empty;

            switch (Propriedade.TipoAplicativo)
            {
                case TipoAplicativo.Nfe:
                    retorna = NomeClasseWSNFe(servico, cUF, versao);
                    break;
                case TipoAplicativo.Nfse:
                    retorna = NomeClasseWSNFSe(servico, cUF);
                    break;
            }

            return retorna;
        }

        protected string NomeClasseWS(Servicos servico, int cUF)
        {
            return xNomeClasseWS(servico, cUF, "");
        }
        #endregion

        #region NomeClasseWSNFe()
        /// <summary>
        /// Retorna o nome da classe do serviço passado por parâmetro do WebService do SEFAZ - NFe
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cUF">Código da UF</param>
        /// <param name="versao">Versão do XML</param>
        /// <returns>Nome da classe</returns>
        private string NomeClasseWSNFe(Servicos servico, int cUF, string versao)
        {
            string retorna = string.Empty;
            string cUFeVersao = cUF.ToString().Trim() + "|" + versao.Trim();

            switch (servico)
            {
                #region NF-e
                case Servicos.NFeInutilizarNumeros:
                    retorna = "NfeInutilizacao2";
                    break;
                case Servicos.NFePedidoConsultaSituacao:
                    retorna = "NfeConsulta2";
                    break;
                case Servicos.NFeConsultaStatusServico:
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
                case Servicos.NFePedidoSituacaoLote:
                    retorna = "NfeRetRecepcao2";
                    break;
                case Servicos.NFePedidoSituacaoLote2:
                    retorna = "NfeRetAutorizacao";
                    break;
                case Servicos.ConsultaCadastroContribuinte:
                    retorna = "CadConsultaCadastro2";
                    break;
                case Servicos.NFeEnviarLote:
                    retorna = "NfeRecepcao2";
                    break;
                case Servicos.NFeEnviarLote2:
                    retorna = "NfeAutorizacao";
                    break;
                case Servicos.NFeEnviarLoteZip2:
                    retorna = "NfeAutorizacao";
                    break;

                case Servicos.EventoRecepcao:
                case Servicos.EventoCancelamento:
                case Servicos.EventoManifestacaoDest:
                case Servicos.EventoCCe:    //danasa 2/7/2011
                case Servicos.EventoEPEC:
                    retorna = "RecepcaoEvento";
                    break;
                case Servicos.NFeConsultaNFDest:
                    retorna = "NFeConsultaDest";
                    break;
                case Servicos.NFeDownload:
                    retorna = "NfeDownloadNF";
                    break;
                #endregion

                #region MDF-e
                case Servicos.MDFeConsultaStatusServico:
                    retorna = "MDFeStatusServico";
                    break;
                case Servicos.MDFeEnviarLote:
                    retorna = "MDFeRecepcao";
                    break;
                case Servicos.MDFePedidoSituacaoLote:
                    retorna = "MDFeRetRecepcao";
                    break;
                case Servicos.MDFePedidoConsultaSituacao:
                    retorna = "MDFeConsulta";
                    break;
                case Servicos.MDFeRecepcaoEvento:
                    retorna = "MDFeRecepcaoEvento";
                    break;
                #endregion

                #region CT-e
                case Servicos.CTeConsultaStatusServico:
                    retorna = "CteStatusServico";
                    break;
                case Servicos.CTeEnviarLote:
                    retorna = "CteRecepcao";
                    break;
                case Servicos.CTePedidoSituacaoLote:
                    retorna = "CteRetRecepcao";
                    break;
                case Servicos.CTePedidoConsultaSituacao:
                    retorna = "CteConsulta";
                    break;
                case Servicos.CTeInutilizarNumeros:
                    retorna = "CteInutilizacao";
                    break;
                case Servicos.CTeRecepcaoEvento:
                    if (cUF == 31) //Minas Gerais (MG)
                        retorna = "RecepcaoEvento";
                    else
                        retorna = "CteRecepcaoEvento";
                    break;
                    #endregion
            }

            return retorna;
        }
        #endregion

        #region NomeClasseWSNFSe()
        /// <summary>
        /// Retorna o nome da classe do serviço passado por parâmetro do WebService do SEFAZ - CTe
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cMunicipio">Código do Municipio UF</param>
        /// <returns>Nome da classe</returns>
        private string NomeClasseWSNFSe(Servicos servico, int cMunicipio)
        {
            string retorna = string.Empty;
            bool taHomologacao = (Empresas.Configuracoes[Empresas.FindEmpresaByThread()].AmbienteCodigo == (int)NFe.Components.TipoAmbiente.taHomologacao);

            switch (Functions.PadraoNFSe(cMunicipio))
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
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "NFSEconsulta";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "NFSEconsulta";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "NFSEconsulta";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "NFSEconsulta";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "NFSEcancelamento";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "NFSEremessa";
                            break;
                    }
                    break;
                #endregion

                #region BETHA
                case PadroesNFSe.BETHA:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultar:
                            retorna = "";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "";
                            break;
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region CANOAS-RS (ABACO)
                case PadroesNFSe.CANOAS_RS:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfse";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
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
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "basic_INFSEConsultas";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "basic_INFSEConsultas";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "basic_INFSEConsultas";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "basic_INFSEConsultas";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "basic_INFSEGeracao";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
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
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultaLoteRPS";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultaNfse";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultaNfseRPS";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultaSituacaoLoteRPS";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
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
                        case Servicos.NFSeCancelar:
                            retorna = "basic_INFSEGeracao";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
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
                        case Servicos.NFSeConsultarPorRps:
                            if (taHomologacao)
                                retorna = "hConsultarNfsePorRps";
                            else
                                retorna = "ConsultarNfsePorRps";
                            break;

                        case Servicos.NFSeCancelar:
                            if (taHomologacao)
                                retorna = "hCancelarNfse";
                            else
                                retorna = NFe.Components.Servicos.NFSeCancelar.ToString();
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
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
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultaLoteRPS";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultaNFSeServicosPrestados";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultaNFSePorRPS";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultaNFSePorFaixa";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelamentoNFSe";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "EnvioLoteRPSSincrono";
                            break;
                    }
                    break;
                #endregion

                #region TIPLAN
                case PadroesNFSe.TIPLAN:
                    retorna = "Nfse";
                    break;
                #endregion

                #region CARIOCA
                case PadroesNFSe.CARIOCA:
                    retorna = "Nfse";
                    break;
                #endregion

                #region GOIANIA
                case PadroesNFSe.GOIANIA:
                    retorna = "Nfse";
                    break;
                #endregion

                #region SMARAPD
                case PadroesNFSe.SMARAPD:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultaLoteRPS";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultaNFSeServicosPrestados";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultaNFSePorRPS";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultaNFSePorFaixa";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelamentoNFSe";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "nfdEntrada";
                            break;
                    }
                    break;
                #endregion

                #region E-GOVERNE
                case PadroesNFSe.EGOVERNE:
                    retorna = "WSNFSeV1001";
                    break;
                    #endregion

            }

            return retorna;
        }
        #endregion
#endif

        #region NomeClasseCabecWS()
        /// <summary>
        /// Retorna o nome da classe de cabecalho do serviço
        /// </summary>
        /// <param name="cUF">Código da UF</param>
        /// <param name="servico">Serviço que está sendo executado</param>
        /// <returns>Nome da classe de cabecalho</returns>
        protected string NomeClasseCabecWS(int cUF, Servicos servico)
        {
            return NomeClasseCabecWS(cUF, servico, (int)TipoEmissao.teNormal);
        }
        #endregion

        #region NomeClasseCabecWS()
        /// <summary>
        /// Retorna o nome da classe de cabecalho do serviço de NFe
        /// </summary>
        /// <param name="cUF">Código da UF</param>
        /// <param name="servico">Serviço que está sendo executado</param>
        /// <param name="tpEmis">Tipo de emissão</param>
        /// <returns>nome da classe de cabecalho do serviço de NFe</returns>
        protected string NomeClasseCabecWS(int cUF, Servicos servico, int tpEmis)
        {
            string retorna = string.Empty;

            switch (servico)
            {
                #region MDFe
                case Servicos.MDFePedidoConsultaSituacao:
                case Servicos.MDFePedidoSituacaoLote:
                case Servicos.MDFeEnviarLote:
                case Servicos.MDFeConsultaStatusServico:
                case Servicos.MDFeRecepcaoEvento:
                case Servicos.MDFeConsultaNaoEncerrado:
                    retorna = "mdfeCabecMsg";
                    break;
                #endregion

                #region CTe
                case Servicos.CTeInutilizarNumeros:
                case Servicos.CTePedidoConsultaSituacao:
                case Servicos.CTePedidoSituacaoLote:
                case Servicos.CTeEnviarLote:
                case Servicos.CTeConsultaStatusServico:
                case Servicos.CTeRecepcaoEvento:
                    retorna = "cteCabecMsg";

                    if (cUF == 50 && tpEmis == (int)TipoEmissao.teNormal)
                        retorna = "CTeCabecMsg";

                    break;
                #endregion

                #region NFe
                default:
                    retorna = "nfeCabecMsg";
                    break;
                    #endregion
            }

            return retorna;
        }
        #endregion

        #region NomeMetodoWS()
        /// <summary>
        /// Retorna o nome do método da classe de serviço
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cUF">Código da UF</param>
        /// <param name="versao">Versão do XML</param>
        /// <returns>nome do método da classe de serviço</returns>
        protected string NomeMetodoWS(Servicos servico, int cUF, string versao)
        {
            string retorna = string.Empty;

            retorna = NomeMetodoWSNFSe(servico, cUF);

            if (retorna == string.Empty) //nem seria necessário, porque estamos obtendo do wsdl
                retorna = NomeMetodoWSNFe(servico, cUF, versao);

            return retorna;
        }

        protected string NomeMetodoWS(Servicos servico, int cUF)
        {
            return NomeMetodoWS(servico, cUF, "");
        }
        #endregion

        #region NomeMetodoWSNFe()
        /// <summary>
        /// Retorna o nome do método da classe de serviço - NFe
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cUF">Código da UF</param>
        /// <param name="versao">Versão do XML</param>
        /// <returns>nome do método da classe de serviço</returns>
        private string NomeMetodoWSNFe(Servicos servico, int cUF, string versao)
        {
            string retorna = string.Empty;
            string cUFeVersao = cUF.ToString().Trim() + "|" + versao;

            switch (servico)
            {
                #region NF-e
                case Servicos.NFeInutilizarNumeros:
                    retorna = "nfeInutilizacaoNF2";
                    break;
                case Servicos.NFePedidoConsultaSituacao:
                    retorna = "nfeConsultaNF2";
                    break;
                case Servicos.NFeConsultaStatusServico:
                    switch (cUFeVersao)
                    {
                        case "29|3.10": //Bahia - XML versão 3.10
                            retorna = "NfeStatusServicoNF";
                            break;
                        default:
                            retorna = "nfeStatusServicoNF2";
                            break;
                    }
                    break;
                case Servicos.NFePedidoSituacaoLote:
                    retorna = "nfeRetRecepcao2";
                    break;
                case Servicos.NFePedidoSituacaoLote2:
                    switch (cUFeVersao)
                    {
                        case "29|3.10": //Bahia - XML versão 3.10
                            retorna = "NfeRetAutorizacaoLote";
                            break;
                        default:
                            retorna = "nfeRetAutorizacaoLote";
                            break;
                    }
                    break;
                case Servicos.ConsultaCadastroContribuinte:
                    retorna = "consultaCadastro2";
                    break;
                case Servicos.NFeEnviarLote2:
                    switch (cUFeVersao)
                    {
                        case "29|3.10": //Bahia - XML versão 3.10
                            retorna = "NfeAutorizacaoLote";
                            break;
                        default:
                            retorna = "nfeAutorizacaoLote";
                            break;
                    }
                    break;
                case Servicos.NFeEnviarLoteZip2:
                    switch (cUFeVersao)
                    {
                        case "29|3.10": //Bahia - XML versão 3.10
                            retorna = "NfeAutorizacaoLoteZip";
                            break;
                        default:
                            retorna = "nfeAutorizacaoLoteZip";
                            break;
                    }
                    break;
                case Servicos.NFeEnviarLote:
                    retorna = "nfeRecepcaoLote2";
                    break;
                case Servicos.EventoCCe:
                case Servicos.EventoCancelamento:
                case Servicos.EventoManifestacaoDest:
                case Servicos.EventoRecepcao:
                    retorna = "nfeRecepcaoEvento";
                    break;
                #endregion

                #region MDF-e
                case Servicos.MDFeConsultaStatusServico:
                    retorna = "mdfeStatusServicoMDF";
                    break;
                case Servicos.MDFeEnviarLote:
                    retorna = "mdfeRecepcaoLote";
                    break;
                case Servicos.MDFePedidoSituacaoLote:
                    retorna = "mdfeRetRecepcao";
                    break;
                case Servicos.MDFePedidoConsultaSituacao:
                    retorna = "mdfeConsultaMDF";
                    break;
                case Servicos.MDFeRecepcaoEvento:
                    retorna = "mdfeRecepcaoEvento";
                    break;
                #endregion

                #region CT-e
                case Servicos.CTeConsultaStatusServico:
                    retorna = "cteStatusServicoCT";
                    break;
                case Servicos.CTeEnviarLote:
                    retorna = "cteRecepcaoLote";
                    break;
                case Servicos.CTePedidoSituacaoLote:
                    retorna = "cteRetRecepcao";
                    break;
                case Servicos.CTePedidoConsultaSituacao:
                    retorna = "cteConsultaCT";
                    break;
                case Servicos.CTeInutilizarNumeros:
                    retorna = "cteInutilizacaoCT";
                    break;
                case Servicos.CTeRecepcaoEvento:
                    retorna = "cteRecepcaoEvento";
                    break;

                #endregion

                default:
                    break;
            }

            return retorna;
        }
        #endregion

        #region NomeMetodoWSNFSe()
        /// <summary>
        /// Retorna o nome da classe do serviço passado por parâmetro do WebService do SEFAZ - CTe
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cMunicipio">Código do Municipio UF</param>
        /// <returns>Nome da classe</returns>
        private string NomeMetodoWSNFSe(Servicos servico, int cMunicipio)
        {
            string retorna = string.Empty;
            bool taHomologacao = (Empresas.Configuracoes[Empresas.FindEmpresaByThread()].AmbienteCodigo == (int)NFe.Components.TipoAmbiente.taHomologacao);

            switch (Functions.PadraoNFSe(cMunicipio))
            {
                #region ISSWEB
                case PadroesNFSe.ISSWEB:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNotaFiscal";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNotaFiscalEletronica";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = taHomologacao ? "EnviarLoteNotaFiscalDeTeste" : "EnviarLoteNotaFiscal";
                            break;
                    }
                    break;
                #endregion

                #region GINFES
                case PadroesNFSe.GINFES:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRpsV3";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfseV3";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRpsV3";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRpsV3";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRpsV3";
                            break;
                    }
                    break;
                #endregion

                #region THEMA
                case PadroesNFSe.THEMA:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "consultarLoteRps";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "consultarNfse";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "consultarNfsePorRps";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "consultarSituacaoLoteRps";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "cancelarNfse";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "recepcionarLoteRpsLimitado"; //"recepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region BETHA
                case PadroesNFSe.BETHA:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfse";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region GOIANIA
                case PadroesNFSe.GOIANIA:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfseRps";
                            break;
                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;
                    }
                    break;
                #endregion

                #region CANOAS - RS (ABACO)
                case PadroesNFSe.ABACO:
                case PadroesNFSe.CANOAS_RS:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "Execute";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "Execute";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "Execute";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "Execute";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "Execute";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "Execute";
                            break;
                    }
                    break;
                #endregion

                #region ISSNET
                case PadroesNFSe.ISSNET:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfse";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultaNFSePorRPS";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultaSituacaoLoteRPS";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                        case Servicos.NFSeConsultarURL:
                            retorna = "ConsultarUrlVisualizacaoNfse";
                            break;
                        case Servicos.NFSeConsultarURLSerie:
                            retorna = "ConsultarUrlVisualizacaoNfseSerie";
                            break;
                    }
                    break;
                #endregion

                #region ISSONLINE
                case PadroesNFSe.ISSONLINE:
                    retorna = "Execute";
                    break;
                #endregion

                #region Blumenau-SC
                case PadroesNFSe.BLUMENAU_SC:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultaLote";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultaNFeEmitidas";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultaNFe";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultaInformacoesLote";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelamentoNFe";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            if (Empresas.Configuracoes[Empresas.FindEmpresaByThread()].AmbienteCodigo == (int)NFe.Components.TipoAmbiente.taHomologacao)
                                retorna = "TesteEnvioLoteRPS";
                            else
                                retorna = "EnvioLoteRPS";
                            break;
                    }
                    break;
                #endregion

                #region BHISS
                case PadroesNFSe.BHISS:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfse";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "GerarNfse";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region GIF
                case PadroesNFSe.GIF:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "consultarNotaFiscal";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "obterCriticaLote";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "cancelarNotaFiscal";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "enviarLoteNotas";
                            break;
                        case Servicos.NFSeConsultarNFSePNG:
                            retorna = "obterNotasEmPNG";
                            break;
                        case Servicos.NFSeInutilizarNFSe:
                            retorna = "inutilizacao";
                            break;
                        case Servicos.NFSeConsultarNFSePDF:
                            retorna = "obterNotasEmPDF";
                            break;
                        case Servicos.NFSeObterNotaFiscal:
                            retorna = "obterNotaFiscal";
                            break;
                    }
                    break;
                #endregion

                #region DUETO
                case PadroesNFSe.DUETO:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfse";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                        case Servicos.NFSeConsultarURL:
                            retorna = "";
                            break;
                    }
                    break;
                #endregion

                #region WEBISS
                case PadroesNFSe.WEBISS:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfse";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region PAULISTANA
                case PadroesNFSe.PAULISTANA:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultaLote";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultaNFeEmitidas";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultaNFe";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultaInformacoesLote";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelamentoNFe";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            if (Empresas.Configuracoes[Empresas.FindEmpresaByThread()].AmbienteCodigo == (int)NFe.Components.TipoAmbiente.taHomologacao)
                                retorna = "TesteEnvioLoteRPS";
                            else
                                retorna = "EnvioLoteRPS";
                            break;
                    }
                    break;
                #endregion

                #region SALVADOR_BA
                case PadroesNFSe.SALVADOR_BA:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRPS";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfse";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfseRPS";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRPS";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "EnviarLoteRPS";
                            break;
                    }
                    break;
                #endregion

                #region PORTOVELHENSE
                case PadroesNFSe.PORTOVELHENSE:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfsePorFaixa";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = ""; //Ainda não implmentado pelo municipio somenete pelo Site - Renan 
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "GerarNfse";
                            break;
                    }
                    break;


                #endregion

                #region PRONIN
                case PadroesNFSe.PRONIN:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;

                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfse";
                            break;

                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;

                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;

                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region ISSONLINE4R (4R Sistemas)
                case PadroesNFSe.ISSONLINE4R:
                    retorna = "Execute";
                    break;
                #endregion

                #region DSF
                case PadroesNFSe.DSF:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            if (taHomologacao)
                                throw new NFe.Components.Exceptions.ServicoInexistenteHomologacaoException(servico);
                            else
                                retorna = "consultarLote";
                            break;

                        case Servicos.NFSeConsultar:
                            if (taHomologacao)
                                throw new NFe.Components.Exceptions.ServicoInexistenteHomologacaoException(servico);
                            else
                                retorna = "consultarNota";
                            break;

                        case Servicos.NFSeConsultarPorRps:
                            if (taHomologacao)
                                throw new NFe.Components.Exceptions.ServicoInexistenteHomologacaoException(servico);
                            else
                                retorna = "consultarNFSeRps";
                            break;

                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            if (taHomologacao)
                                throw new NFe.Components.Exceptions.ServicoInexistenteHomologacaoException(servico);
                            else
                                retorna = "consultarSequencialRps";
                            break;

                        case Servicos.NFSeCancelar:
                            if (taHomologacao)
                                throw new NFe.Components.Exceptions.ServicoInexistenteHomologacaoException(servico);
                            else
                                retorna = "cancelar";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            if (taHomologacao)
                                if (cMunicipio.ToString().Equals("5002704")) // Campo grande - MS não tem web service de teste
                                    throw new NFe.Components.Exceptions.ServicoInexistenteHomologacaoException(servico);
                                else
                                    retorna = "testeEnviar";
                            else
                                retorna = "enviar";
                            break;

                        default:
                            throw new NFe.Components.Exceptions.ServicoInexistenteException();
                    }
                    break;
                #endregion

                #region TECNOSISTEMAS
                case PadroesNFSe.TECNOSISTEMAS:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "mConsultaLoteRPS";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "mConsultaNFSeServicosPrestados";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "mConsultaNFSePorRPS";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "mConsultaNFSePorFaixa";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "mCancelamentoNFSe";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "mEnvioLoteRPSSincrono";
                            break;
                    }
                    break;
                #endregion

                #region TIPLAN
                case PadroesNFSe.TIPLAN:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            if (cMunicipio.Equals(3304201) || cMunicipio.Equals(3301702))
                                retorna = "ConsultarLoteRps";
                            else
                                retorna = "ConsultarLoteRPS";
                            break;

                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfse";
                            break;

                        case Servicos.NFSeConsultarPorRps:
                            if (cMunicipio.Equals(3304201) || cMunicipio.Equals(3301702))
                                retorna = "ConsultarNfsePorRps";
                            else
                                retorna = "ConsultarNfseRPS";

                            break;

                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            if (cMunicipio.Equals(3304201) || cMunicipio.Equals(3301702))
                                retorna = "ConsultarSituacaoLoteRps";
                            else
                                retorna = "ConsultarSituacaoLoteRPS";

                            break;

                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region CARIOCA
                case PadroesNFSe.CARIOCA:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfse";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region SIGCORP_SIGISS
                case PadroesNFSe.SIGCORP_SIGISS:
                    switch (servico)
                    {
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "GerarNota";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNota";
                            break;
                    }
                    break;

                #endregion

                #region SMARAPD
                case PadroesNFSe.SMARAPD:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "nfdSaida";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "nfdEntradaCancelar";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "nfdEntrada";
                            break;
                        case Servicos.NFSeConsultarURL:
                            retorna = "urlNfd";
                            break;
                        case Servicos.NFSeConsultarURLSerie:
                            retorna = "";
                            break;
                    }
                    break;
                #endregion

                #region FINTEL
                case PadroesNFSe.FINTEL:
                    switch (servico)
                    {
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "RecepcionarLoteRpsSincrono";
                            break;
                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfseFaixa";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarLoteNotasFiscais";
                            break;
                        case Servicos.NFSeConsultarURL:
                            retorna = "";
                            break;
                        case Servicos.NFSeConsultarURLSerie:
                            retorna = "";
                            break;
                    }
                    break;
                #endregion

                #region EQUIPLANO
                case PadroesNFSe.EQUIPLANO:
                    switch (servico)
                    {
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "esRecepcionarLoteRps";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "esCancelarNfse";
                            break;
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "esConsultarLoteRps";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "esConsultarNfsePorRps";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "esConsultarNfse";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "esConsultarSituacaoLoteRps";
                            break;

                        case Servicos.NFSeConsultarURL:
                            retorna = "";
                            break;
                        case Servicos.NFSeConsultarURLSerie:
                            retorna = "";
                            break;
                    }
                    break;
                #endregion

                #region PRODATA
                case PadroesNFSe.PRODATA:
                    switch (servico)
                    {
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfsePorFaixa";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "";
                            break;

                        case Servicos.NFSeConsultarURL:
                            retorna = "";
                            break;
                        case Servicos.NFSeConsultarURLSerie:
                            retorna = "";
                            break;
                    }
                    break;
                #endregion

                #region VVISS
                case PadroesNFSe.VVISS:
                    switch (servico)
                    {
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "RecepcionarLoteRpsSincrono";
                            break;
                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfseFaixa";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarLoteNotasFiscais";
                            break;
                        case Servicos.NFSeConsultarURL:
                            retorna = "";
                            break;
                        case Servicos.NFSeConsultarURLSerie:
                            retorna = "";
                            break;
                    }
                    break;
                #endregion

                #region VVISS
                case PadroesNFSe.FISSLEX:
                    retorna = "Execute";
                    break;
                #endregion

                #region NATALENSE
                case PadroesNFSe.NATALENSE:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfse";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region NOTA INTELIGENTE
                case PadroesNFSe.NOTAINTELIGENTE:
                    switch (servico)
                    {
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region FREIRE INFORMATICA
                case PadroesNFSe.FREIRE_INFORMATICA:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfse";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region CAMACARI_BA
                case PadroesNFSe.CAMACARI_BA:
                    switch (servico)
                    {
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfsePorFaixa";
                            break;

                    }
                    break;
                    #endregion
            }

            return retorna;
        }
        #endregion

        #endregion

        #region LerXMLNFe()
        /// <summary>
        /// Le o conteúdo do XML da NFe
        /// </summary>
        /// <param name="arquivo">Arquivo XML da NFe</param>
        /// <returns>Retorna o conteúdo do XML da NFe</returns>
        public DadosNFeClass LerXMLNFe(string arquivo)
        {
            LerXML oLerXML = new LerXML();

            switch (Servico)
            {
                case Servicos.MDFeAssinarValidarEnvioEmLote:
                case Servicos.MDFeMontarLoteVarios:
                case Servicos.MDFeMontarLoteUm:
                    oLerXML.Mdfe(arquivo);
                    break;

                case Servicos.CTeAssinarValidarEnvioEmLote:
                case Servicos.CTeMontarLoteVarios:
                case Servicos.CTeMontarLoteUm:
                    oLerXML.Cte(arquivo);
                    break;

                default:
                    oLerXML.Nfe(arquivo);
                    break;
            }

            return oLerXML.oDadosNfe;
        }
        #endregion

        #region AssinarValidarXMLNFe()
        /// <summary>
        /// Assinar e validar o XML da Nota Fiscal Eletrônica e move para a pasta de assinados
        /// </summary>
        /// <param name="pasta">Nome da pasta onde está o XML a ser validado e assinado</param>
        /// <returns>true = Conseguiu assinar e validar</returns>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 03/04/2009
        /// </remarks>
        public void AssinarValidarXMLNFe(string pasta)
        {
            int emp = Empresas.FindEmpresaByThread();


            //Criar Pasta dos XML´s a ser enviado em Lote já assinados
            string pastaLoteAssinado = pasta + Propriedade.NomePastaXMLAssinado;

            //Se o arquivo XML já existir na pasta de assinados, vou avisar o ERP que já tem um em andamento
            string arqDestino = pastaLoteAssinado + "\\" + Path.GetFileName(NomeArquivoXML);// Functions.ExtrairNomeArq(NomeArquivoXML, ".xml") + ".xml";

            try
            {
                //Fazer uma leitura de algumas tags do XML
                DadosNFeClass dadosNFe = this.LerXMLNFe(NomeArquivoXML);
                string ChaveNfe = dadosNFe.chavenfe;
                string TpEmis = dadosNFe.tpEmis;

                //Inserir NFe no XML de controle do fluxo
                FluxoNfe oFluxoNfe = new FluxoNfe();
                if (oFluxoNfe.NfeExiste(ChaveNfe))
                {
                    //Mover o arquivo da pasta em processamento para a pasta de XML´s com erro
                    oAux.MoveArqErro(Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                        PastaEnviados.EmProcessamento.ToString() + "\\" +
                                        Path.GetFileName(NomeArquivoXML));//Functions.ExtrairNomeArq(NomeArquivoXML, ".xml") + ".xml");

                    //Deletar a NFE do arquivo de controle de fluxo
                    oFluxoNfe.ExcluirNfeFluxo(ChaveNfe);

                    //Vou forçar uma exceção, e o ERP através do inicio da mensagem de erro pode tratar e já gerar uma consulta
                    //situação para finalizar o processo. Assim envito perder os XML´s que estão na pasta EmProcessamento
                    //tendo assim a possibilidade de gerar o -procNfe.XML através da consulta situação.
                    //Wandrey 08/10/2009
                    //throw new Exception("NFE NO FLUXO: Esta nota fiscal já está na pasta de Notas Fiscais em processo de envio, desta forma não é possível envia-la novamente. Se a nota fiscal estiver presa no fluxo de envio sem conseguir finalizar o processo, gere um consulta situação da NFe para forçar a finalização.\r\n" + NomeArquivoXML);
                }
                else
                {
                    //Deletar o arquivo XML da pasta de temporários de XML´s com erros se o mesmo existir
                    Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + Path.GetFileName(NomeArquivoXML));//  Functions.ExtrairNomeArq(NomeArquivoXML, ".xml") + ".xml");
                }

                //Validações gerais
                ValidacoesGeraisXMLNFe(NomeArquivoXML, dadosNFe);

                //Assinar o arquivo XML
                AssinaturaDigital assDig = new AssinaturaDigital();
                assDig.Assinar(NomeArquivoXML, emp, Convert.ToInt32(dadosNFe.cUF));

                //Adicionar a tag do QRCode
                if (!String.IsNullOrEmpty(Empresas.Configuracoes[emp].IdentificadorCSC) && dadosNFe.mod == "65")
                {
                    QRCode qrCode = new QRCode(Empresas.Configuracoes[emp].IdentificadorCSC, Empresas.Configuracoes[emp].TokenCSC, NomeArquivoXML);

                    if (qrCode.CalcularLink())
                    {
                        string url = Empresas.Configuracoes[emp].AmbienteCodigo == (int)NFe.Components.TipoAmbiente.taHomologacao ? Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCeH : Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCe;
                        qrCode.GerarLinkConsulta(url);
                        qrCode.AddLinkQRCode();
                    }
                }

                // Validar o Arquivo XML da NFe com os Schemas se estiver assinado
                ValidarXML validar = new ValidarXML(NomeArquivoXML, Convert.ToInt32(dadosNFe.cUF), false);
                string cResultadoValidacao = validar.ValidarArqXML(NomeArquivoXML);
                if (cResultadoValidacao != "")
                {
                    //Registrar o erro da validação do schema para o sistema ERP
                    throw new Exception(cResultadoValidacao);
                }

                //Mover o arquivo XML da pasta de lote para a pasta de XML´s assinados
                //Se a pasta de assinados não existir, vamos criar
                if (!Directory.Exists(pastaLoteAssinado))
                {
                    Directory.CreateDirectory(pastaLoteAssinado);
                }

                FileInfo fiDestino = new FileInfo(arqDestino);

                if (!File.Exists(arqDestino) || (long)DateTime.Now.Subtract(fiDestino.LastWriteTime).TotalMilliseconds >= 60000) //60.000 ms que corresponde á 60 segundos que corresponde a 1 minuto
                {
                    //Mover o arquivo para a pasta de XML´s assinados
                    Functions.Move(NomeArquivoXML, arqDestino);

                    oFluxoNfe.InserirNfeFluxo(ChaveNfe, dadosNFe.mod, arqDestino);
                }

                else
                {
                    oFluxoNfe.InserirNfeFluxo(ChaveNfe, dadosNFe.mod, arqDestino);

                    throw new IOException("Esta nota fiscal já está na pasta de Notas Fiscais assinadas e em processo de envio, desta forma não é possível enviar a mesma novamente.\r\n" +
                        NomeArquivoXML);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    string extFinal = Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML;
                    string extErro = Propriedade.ExtRetorno.Nfe_ERR;
                    switch (Servico)
                    {
                        case Servicos.MDFeAssinarValidarEnvioEmLote:
                        case Servicos.MDFeMontarLoteUm:
                            extFinal = Propriedade.Extensao(Propriedade.TipoEnvio.MDFe).EnvioXML;
                            extErro = Propriedade.ExtRetorno.MDFe_ERR;
                            break;

                        case Servicos.CTeAssinarValidarEnvioEmLote:
                        case Servicos.CTeMontarLoteUm:
                            extFinal = Propriedade.Extensao(Propriedade.TipoEnvio.CTe).EnvioXML;
                            extErro = Propriedade.ExtRetorno.Cte_ERR;
                            break;
                    }

                    TFunctions.GravarArqErroServico(NomeArquivoXML, extFinal, extErro, ex);

                    //Se já foi movido o XML da Nota Fiscal para a pasta em Processamento, vou ter que 
                    //forçar mover para a pasta de XML com erro neste ponto.
                    oAux.MoveArqErro(arqDestino);
                }
                catch (Exception exx)
                {
                    Auxiliar.WriteLog(exx.Message, true);
                    //Se ocorrer algum erro na hora de tentar gravar o XML de erro para o ERP ou mover o arquivo XML para a pasta de XML com erro, não 
                    //vou poder fazer nada, pq foi algum erro de rede, permissão de acesso a pasta ou arquivo, etc.
                    //Wandey 13/03/2010
                }
                throw;
            }
        }
        #endregion

        #region ValidacoesGerais()
        /// <summary>
        /// Efetua uma leitura do XML da nota fiscal eletrônica e faz diversas conferências do seu conteúdo e bloqueia se não 
        /// estiver de acordo com as configurações do UNINFE
        /// </summary>
        /// <param name="arquivoNFe">Arquivo XML da NFe</param>
        /// <param name="dadosNFe">Objeto com o conteúdo das tags do XML</param>
        /// <returns>true = Validado com sucesso</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>16/04/2009</date>
        protected void ValidacoesGeraisXMLNFe(string arquivoNFe, DadosNFeClass dadosNFe)
        {
            int emp = Empresas.FindEmpresaByThread();

            bool gException = true;

            bool booValido = false;
            int nPos = 0;
            string cTextoErro = "";

            int tpEmis = Convert.ToInt32(dadosNFe.tpEmis);

            switch (Servico)
            {
                case Servicos.MDFeAssinarValidarEnvioEmLote:
                case Servicos.MDFeMontarLoteUm:
                    booValido = true;
                    nPos = 1;
                    goto case Servicos.NFeMontarLoteUma;

                case Servicos.CTeAssinarValidarEnvioEmLote:
                case Servicos.CTeMontarLoteUm:
                case Servicos.NFeAssinarValidarEnvioEmLote:
                case Servicos.NFeMontarLoteUma:
                    switch (Empresas.Configuracoes[emp].tpEmis)
                    {
                        case (int)TipoEmissao.teNormal:
                            switch (tpEmis)
                            {
                                case (int)TipoEmissao.teNormal:
                                ///
                                /// Foi emitido em contingencia e agora os quer enviar
                                /// 
                                case (int)TipoEmissao.teFS:
                                case (int)TipoEmissao.teFSDA:
                                case (int)TipoEmissao.teEPEC:
                                case (int)TipoEmissao.teOffLine:
                                    booValido = true;
                                    break;
                            }
                            break;

                        case (int)TipoEmissao.teSVCSP:
                            booValido = (tpEmis == (int)NFe.Components.TipoEmissao.teSVCSP);
                            break;

                        case (int)TipoEmissao.teSVCAN:
                            booValido = (tpEmis == (int)NFe.Components.TipoEmissao.teSVCAN);
                            break;

                        case (int)TipoEmissao.teSVCRS:
                            booValido = (tpEmis == (int)NFe.Components.TipoEmissao.teSVCRS);
                            break;

                        case (int)TipoEmissao.teFS:
                        case (int)TipoEmissao.teEPEC:
                        case (int)TipoEmissao.teFSDA:
                        case (int)TipoEmissao.teOffLine:
                            //Retorno somente falso mas sem exception para não fazer nada. Wandrey 09/06/2009
                            gException = booValido = false;
                            break;
                    }
                    break;
            }
            string cTextoErro2 = "O XML não será enviado e será movido para a pasta de XML com erro para análise.";

            if (!booValido && gException)
            {
                cTextoErro = "O XML está configurado para um tipo de emissão e o UniNFe para outro. " +
                         "XML: " + NFe.Components.EnumHelper.GetDescription((TipoEmissao)Enum.ToObject(typeof(TipoEmissao), tpEmis)) +
                         " (tpEmis = " + tpEmis.ToString() + "). " +
                         "UniNFe: " + NFe.Components.EnumHelper.GetDescription((TipoEmissao)Enum.ToObject(typeof(TipoEmissao), Empresas.Configuracoes[emp].tpEmis)) +
                         " (tpEmis = " + Empresas.Configuracoes[emp].tpEmis.ToString() + "). " +
                        cTextoErro2;

                throw new Exception(cTextoErro);
            }

            switch (tpEmis)
            {
                case (int)TipoEmissao.teSVCAN:
                case (int)TipoEmissao.teSVCRS:
                    var se = Propriedade.Estados.First(s => s.CodigoMunicipio.Equals(Convert.ToInt32(dadosNFe.cUF)));
                    if (se.svc != (TipoEmissao)tpEmis && se.svc != TipoEmissao.teNone)
                    {
                        throw new Exception("UF: " + Functions.CodigoParaUF(Convert.ToInt32(dadosNFe.cUF)) + " não está sendo atendida pelo WebService do SVC: " +
                                NFe.Components.EnumHelper.GetDescription((TipoEmissao)Enum.ToObject(typeof(TipoEmissao), tpEmis)) + ". " + cTextoErro2);
                    }
                    break;
            }

            #region Verificar o ambiente da nota com o que está configurado no uninfe. Wandrey 20/08/2014
            if (booValido)
            {
                switch (Empresas.Configuracoes[emp].AmbienteCodigo)
                {
                    case (int)NFe.Components.TipoAmbiente.taHomologacao:
                        if (Convert.ToInt32(dadosNFe.tpAmb) == (int)NFe.Components.TipoAmbiente.taProducao)
                        {
                            cTextoErro = "Conteúdo da tag tpAmb do XML está com conteúdo indicando o envio para ambiente de produção e o UniNFe está configurado para ambiente de homologação.";
                            throw new Exception(cTextoErro);
                        }
                        break;

                    case (int)NFe.Components.TipoAmbiente.taProducao:
                        if (Convert.ToInt32(dadosNFe.tpAmb) == (int)NFe.Components.TipoAmbiente.taHomologacao)
                        {
                            cTextoErro = "Conteúdo da tag tpAmb do XML está com conteúdo indicando o envio para ambiente de homologação e o UniNFe está configurado para ambiente de produção.";
                            throw new Exception(cTextoErro);
                        }
                        break;
                }
            }
            #endregion

            #region Verificar se os valores das tag´s que compõe a chave da nfe estão batendo com as informadas na chave
            //Verificar se os valores das tag´s que compõe a chave da nfe estão batendo com as informadas na chave
            if (booValido)
            {
                cTextoErro = string.Empty;

                #region Tag <cUF>
                if (dadosNFe.cUF != dadosNFe.chavenfe.Substring(3 + nPos, 2))
                {
                    cTextoErro += "O código da UF informado na tag <cUF> está diferente do informado na chave da NF-e.\r\n" +
                        "Código da UF informado na tag <cUF>: " + dadosNFe.cUF + "\r\n" +
                        "Código da UF informado na chave da NF-e: " + dadosNFe.chavenfe.Substring(3 + nPos, 2) + "\r\n\r\n";
                    booValido = false;
                }
                #endregion

                #region Tag <tpEmis>
                if (dadosNFe.tpEmis != dadosNFe.chavenfe.Substring(37 + nPos, 1))
                {
                    cTextoErro += "O código numérico informado na tag <tpEmis> está diferente do informado na chave da NF-e.\r\n" +
                        "Código numérico informado na tag <tpEmis>: " + dadosNFe.tpEmis + "\r\n" +
                        "Código numérico informado na chave da NF-e: " + dadosNFe.chavenfe.Substring(37 + nPos, 1) + "\r\n\r\n";
                    booValido = false;
                }
                #endregion

                #region Tag <cNF>
                if (dadosNFe.cNF != dadosNFe.chavenfe.Substring(38 + nPos, 8))
                {
                    cTextoErro += "O código numérico informado na tag <cNF> está diferente do informado na chave da NF-e.\r\n" +
                        "Código numérico informado na tag <cNF>: " + dadosNFe.cNF + "\r\n" +
                        "Código numérico informado na chave da NF-e: " + dadosNFe.chavenfe.Substring(38 + nPos, 8) + "\r\n\r\n";
                    booValido = false;
                }
                #endregion

                #region Tag <mod>
                if (dadosNFe.mod != dadosNFe.chavenfe.Substring(23 + nPos, 2))
                {
                    cTextoErro += "O modelo informado na tag <mod> está diferente do informado na chave da NF-e.\r\n" +
                        "Modelo informado na tag <mod>: " + dadosNFe.mod + "\r\n" +
                        "Modelo informado na chave da NF-e: " + dadosNFe.chavenfe.Substring(23 + nPos, 2) + "\r\n\r\n";
                    booValido = false;
                }
                #endregion

                #region Tag <nNF>
                if (Convert.ToInt32(dadosNFe.nNF) != Convert.ToInt32(dadosNFe.chavenfe.Substring(28 + nPos, 9)))
                {
                    cTextoErro += "O número da NF-e informado na tag <nNF> está diferente do informado na chave da NF-e.\r\n" +
                        "Número da NFe informado na tag <nNF>: " + Convert.ToInt32(dadosNFe.nNF).ToString() + "\r\n" +
                        "Número da NFe informado na chave da NF-e: " + Convert.ToInt32(dadosNFe.chavenfe.Substring(28 + nPos, 9)).ToString() + "\r\n\r\n";
                    booValido = false;
                }
                #endregion

                #region Tag <cDV>
                if (dadosNFe.cDV != dadosNFe.chavenfe.Substring(46 + nPos, 1))
                {
                    cTextoErro += "O número do dígito verificador informado na tag <cDV> está diferente do informado na chave da NF-e.\r\n" +
                        "Número do dígito verificador informado na tag <cDV>: " + dadosNFe.cDV + "\r\n" +
                        "Número do dígito verificador informado na chave da NF-e: " + dadosNFe.chavenfe.Substring(46 + nPos, 1) + "\r\n\r\n";
                    booValido = false;
                }
                #endregion

                #region Tag <CNPJ> da tag <emit>
                if (dadosNFe.CNPJ != dadosNFe.chavenfe.Substring(9 + nPos, 14))
                {
                    cTextoErro += "O CNPJ do emitente informado na tag <emit><CNPJ> está diferente do informado na chave da NF-e.\r\n" +
                        "CNPJ do emitente informado na tag <emit><CNPJ>: " + dadosNFe.CNPJ + "\r\n" +
                        "CNPJ do emitente informado na chave da NF-e: " + dadosNFe.chavenfe.Substring(9 + nPos, 14) + "\r\n\r\n";
                    booValido = false;
                }
                #endregion

                #region Tag <serie>
                if (Convert.ToInt32(dadosNFe.serie) != Convert.ToInt32(dadosNFe.chavenfe.Substring(25 + nPos, 3)))
                {
                    cTextoErro += "A série informada na tag <serie> está diferente da informada na chave da NF-e.\r\n" +
                        "Série informada na tag <cDV>: " + Convert.ToInt32(dadosNFe.serie).ToString() + "\r\n" +
                        "Série informada na chave da NF-e: " + Convert.ToInt32(dadosNFe.chavenfe.Substring(25 + nPos, 3)).ToString() + "\r\n\r\n";
                    booValido = false;
                }
                #endregion

                #region Tag <dEmi>
                if (dadosNFe.dEmi.Month.ToString("00") != dadosNFe.chavenfe.Substring(7 + nPos, 2) ||
                    dadosNFe.dEmi.Year.ToString("0000").Substring(2, 2) != dadosNFe.chavenfe.Substring(5 + nPos, 2))
                {
                    cTextoErro += "O ano e mês da emissão informada na tag " + dadosNFe.versao == "2.00" ? "<dEmi> " : "<dhEmi> " + "está diferente da informada na chave da NF-e.\r\n" +
                        "Mês/Ano da data de emissão informada na tag " + dadosNFe.versao == "2.00" ? "<dEmi>: " : "<dhEmi>: " + dadosNFe.dEmi.Month.ToString("00") + "/" + dadosNFe.dEmi.Year.ToString("0000").Substring(2, 2) + "\r\n" +
                        "Mês/Ano informados na chave da NF-e: " + dadosNFe.chavenfe.Substring(7 + nPos, 2) + "/" + dadosNFe.chavenfe.Substring(5 + nPos, 2) + "\r\n\r\n";
                    booValido = false;
                }
                #endregion

                if (!booValido)
                {
                    throw new Exception(cTextoErro);
                }
            }
            #endregion

        }
        #endregion

        #region LoteNfe()
        /// <summary>
        /// Auxiliar na geração do arquivo XML de Lote de notas fiscais
        /// </summary>
        /// <param name="Arquivo">Nome do arquivo XML da NFe para montagem do lote de 1 NFe</param>
        /// <date>07/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        protected void LoteNfe(string Arquivo, string versaoXml)
        {
            oGerarXML.LoteNfe(Servico, Arquivo, versaoXml);
        }
        #endregion

        #region LoteNfe()
        /// <summary>
        /// Auxliar na geração do arquivo XML de Lote de notas fiscais
        /// </summary>
        /// <param name="lstArquivoNfe">Lista de arquivos de NFe para montagem do lote de várias NFe</param>
        /// <param name="versaoXml">Versao do Xml de lote</param>
        /// <date>24/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        protected void LoteNfe(List<string> lstArquivoNfe, string versaoXml)
        {
            oGerarXML.LoteNfe(lstArquivoNfe, versaoXml);
        }
        #endregion

        #region ProcessaNFeDenegada
        protected void ProcessaNFeDenegada(int emp, LerXML oLerXml, string strArquivoNFe, string protNFe, string versao)
        {
            string strProtNfe;

            if (!File.Exists(strArquivoNFe))
                throw new Exception("Arquivo \"" + strArquivoNFe + "\" não encontrado");

            oLerXml.Nfe(strArquivoNFe);
            string nomePastaEnviado = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                        PastaEnviados.Denegados.ToString() + "\\" +
                                        Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(oLerXml.oDadosNfe.dEmi);
            string dArquivo = Path.Combine(nomePastaEnviado, Path.GetFileName(strArquivoNFe).Replace(Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML, Propriedade.ExtRetorno.Den));
            string strNomeArqDenegadaNFe = dArquivo;
            string arqDen = dArquivo;

            //danasa 11-4-2012
            bool addNFeDen = true;
            if (File.Exists(dArquivo))
            {
                // verifica se a NFe já tem protocolo gravado
                // só para atualizar notas denegadas que ainda não tem o protocolo atualizado 
                // e que já estao na pasta de notas denegadas.
                // Para futuras notas denegadas esta propriedade sempre será false
                if (File.ReadAllText(dArquivo).IndexOf("<protNFe>") > 0)
                    addNFeDen = false;
            }
            if (addNFeDen)
            {
                ///
                /// monta o XML de denegacao
                strProtNfe = protNFe;
                ///
                /// gera o arquivo de denegacao na pasta EmProcessamento
                strNomeArqDenegadaNFe = oGerarXML.XmlDistNFe(strArquivoNFe, strProtNfe, Propriedade.ExtRetorno.Den, versao);
                if (string.IsNullOrEmpty(strNomeArqDenegadaNFe))
                    throw new Exception("Erro de criação do arquivo de distribuição da nota denegada");

                ///
                /// exclui o XML denegado, se existir
                Functions.DeletarArquivo(dArquivo);
                ///
                /// Move a NFE-denegada da pasta em processamento para NFe Denegadas
                TFunctions.MoverArquivo(strNomeArqDenegadaNFe, PastaEnviados.Denegados, oLerXml.oDadosNfe.dEmi);
                ///
                /// verifica se o arquivo da NFe já existe na pasta denegadas
                dArquivo = Path.Combine(nomePastaEnviado, Path.GetFileName(strArquivoNFe));

                if (!File.Exists(dArquivo))
                {
                    if (!string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaBackup))
                    {
                        //Criar Pasta do Mês para gravar arquivos enviados
                        string nomePastaBackup = Empresas.Configuracoes[emp].PastaBackup + "\\" +
                                                    PastaEnviados.Denegados + "\\" +
                                                    Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(oLerXml.oDadosNfe.dEmi);
                        if (!Directory.Exists(nomePastaBackup))
                            System.IO.Directory.CreateDirectory(nomePastaBackup);

                        //Se conseguiu criar a pasta ele move o arquivo, caso contrário
                        if (Directory.Exists(nomePastaBackup))
                        {
                            //Mover o arquivo da nota fiscal para a pasta de backup
                            string destinoBackup = Path.Combine(nomePastaBackup, Path.GetFileName(strArquivoNFe));
                            Functions.DeletarArquivo(destinoBackup);
                            File.Copy(strArquivoNFe, destinoBackup);
                        }
                        else
                        {
                            //throw new Exception("Pasta de backup informada nas configurações não existe. (Pasta: " + nomePastaBackup + ")");
                        }
                    }
                    // move o arquivo NFe para a pasta Denegada
                    File.Move(strArquivoNFe, dArquivo);
                }
                else
                    // Como já existe na pasta Enviados\Denegados, só vou excluir da pasta EmProcessamento. Wandrey 22/12/2015
                    Functions.DeletarArquivo(strArquivoNFe);
            }
            try
            {
                TFunctions.ExecutaUniDanfe(arqDen, oLerXml.oDadosNfe.dEmi, Empresas.Configuracoes[emp]);
            }
            catch (Exception ex)
            {
                Auxiliar.WriteLog("ProcessaDenegada: " + ex.Message, false);
            }
        }
        #endregion

        #region XmlPedRec()
        /// <summary>
        /// Gerar o XML de consulta do recibo do lote da nfe
        /// </summary>
        /// <param name="empresa">Código da empresa</param>
        /// <param name=TpcnResources.nRec.ToString()>Número do recibo a ser consultado</param>
        /// <param name="versao">Versao do Schema XML</param>
        /// <param name="mod">Modelo do documento fiscal</param>
        public void XmlPedRec(int empresa, string nRec, string versao, string mod)
        {
            GerarXML gerarXML = new GerarXML(empresa);
            gerarXML.XmlPedRec(mod, nRec, versao);
        }
        #endregion

        #region Ultiliza WS compilado
        protected bool IsUtilizaCompilacaoWs(PadroesNFSe padrao, Servicos servico = Servicos.Nulo)
        {
            bool retorno = true;

            switch (padrao)
            {
                case PadroesNFSe.MEMORY:
                case PadroesNFSe.CONSIST:
                case PadroesNFSe.MGM:
                case PadroesNFSe.ELOTECH:
                case PadroesNFSe.GOVDIGITAL:
                case PadroesNFSe.EL:
                case PadroesNFSe.EGOVERNE:
                case PadroesNFSe.IPM:
                case PadroesNFSe.SYSTEMPRO:
                case PadroesNFSe.SIGCORP_SIGISS:
                case PadroesNFSe.FIORILLI:
                case PadroesNFSe.CONAM:
                case PadroesNFSe.SIMPLISS:
                case PadroesNFSe.RLZ_INFORMATICA:
                case PadroesNFSe.PAULISTANA:
                case PadroesNFSe.NOTAINTELIGENTE:
                case PadroesNFSe.NA_INFORMATICA:
                    retorno = false;
                    break;

                case PadroesNFSe.FISSLEX:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarPorRps:
                        case Servicos.NFSeConsultar:
                        case Servicos.NFSeConsultarSituacaoLoteRps:
                        case Servicos.NFSeConsultarLoteRps:
                            retorno = false;
                            break;

                        default:
                            retorno = true;
                            break;
                    }
                    break;
            }

            return retorno;
        }
        #endregion

        /// <summary>
        /// Se invoca o serviço
        /// </summary>
        /// <param name="padrao"></param>
        /// <param name="servico"></param>
        /// <returns></returns>
        protected bool IsInvocar(PadroesNFSe padrao, Servicos servico = Servicos.Nulo)
        {
            bool invocar = IsUtilizaCompilacaoWs(padrao, servico);

            switch (padrao)
            {
                case PadroesNFSe.NOTAINTELIGENTE:
                case PadroesNFSe.PAULISTANA:
                case PadroesNFSe.NA_INFORMATICA:
                    invocar = true;
                    break;
            }

            return invocar;
        }

        #region XmlRetorno()
        /// <summary>
        /// Auxiliar na geração do arquivo XML de retorno para o ERP quando estivermos utilizando o InvokeMember para chamar o método
        /// </summary>
        /// <param name="pFinalArqEnvio">Final do nome do arquivo de solicitação do serviço.</param>
        /// <param name="pFinalArqRetorno">Final do nome do arquivo que é para ser gravado o retorno.</param>
        /// <date>07/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        /// 
        /// NAO RENOMEAR ou EXCLUIR porque ela é acessada por Invoke
        /// 
        public void XmlRetorno(string pFinalArqEnvio, string pFinalArqRetorno)
        {
            oGerarXML.XmlRetorno(pFinalArqEnvio, pFinalArqRetorno, this.vStrXmlRetorno);
        }
        #endregion

        #region GetTipoServicoSincrono
        protected Servicos GetTipoServicoSincrono(Servicos servico, string file, PadroesNFSe padrao)
        {
            Servicos result = servico;
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            switch (padrao)
            {
                case PadroesNFSe.FINTEL:
                    if (servico == Servicos.NFSeRecepcionarLoteRps)
                    {
                        switch (doc.DocumentElement.Name)
                        {
                            case "EnviarLoteRpsSincronoEnvio":
                                result = Servicos.NFSeRecepcionarLoteRpsSincrono;
                                break;
                            case "GerarNfseEnvio":
                                result = Servicos.NFSeGerarNfse;
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case PadroesNFSe.BHISS:
                    if (doc.DocumentElement.Name.Equals("GerarNfseEnvio") && servico == Servicos.NFSeRecepcionarLoteRps)
                    {
                        result = Servicos.NFSeRecepcionarLoteRpsSincrono;
                    }
                    break;

                case PadroesNFSe.GOIANIA:
                    if (servico == Servicos.NFSeRecepcionarLoteRps)
                    {
                        switch (doc.DocumentElement.Name)
                        {
                            case "EnviarLoteRpsSincronoEnvio":
                                result = Servicos.NFSeRecepcionarLoteRpsSincrono;
                                break;
                            case "GerarNfseEnvio":
                                result = Servicos.NFSeGerarNfse;
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case PadroesNFSe.VVISS:
                    if (servico == Servicos.NFSeRecepcionarLoteRps)
                    {
                        switch (doc.DocumentElement.Name)
                        {
                            case "EnviarLoteRpsSincronoEnvio":
                                result = Servicos.NFSeRecepcionarLoteRpsSincrono;
                                break;
                            case "GerarNfseEnvio":
                                result = Servicos.NFSeGerarNfse;
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case PadroesNFSe.NA_INFORMATICA:
                    if (servico == Servicos.NFSeRecepcionarLoteRps)
                    {
                        switch (doc.DocumentElement.Name)
                        {
                            case "EnviarLoteRpsSincronoEnvio":
                                result = Servicos.NFSeRecepcionarLoteRpsSincrono;
                                break;
                            case "EnviarLoteRpsEnvio":
                                result = Servicos.NFSeGerarNfse;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
            }

            return result;
        }
        #endregion

        #region ValidaEvento
        protected void ValidaEvento(int emp, DadosenvEvento dadosEnvEvento)
        {
            string cErro = "";
            string currentEvento = dadosEnvEvento.eventos[0].tpEvento;
            string ctpEmis = dadosEnvEvento.eventos[0].chNFe.Substring(34, 1);
            foreach (Evento item in dadosEnvEvento.eventos)
            {
                if (!currentEvento.Equals(item.tpEvento))
                    throw new Exception(string.Format("Não é possivel mesclar tipos de eventos dentro de um mesmo xml/txt de eventos. O tipo de evento neste xml/txt é {0}", currentEvento));

                switch (NFe.Components.EnumHelper.StringToEnum<NFe.ConvertTxt.tpEventos>(currentEvento))
                {
                    case ConvertTxt.tpEventos.tpEvCancelamentoNFe:
                        if (!ctpEmis.Equals(item.chNFe.Substring(34, 1)))
                            cErro += "Não é possivel mesclar chaves com tipo de emissão dentro de um mesmo xml/txt de eventos.\r\n";
                        break;
                    case ConvertTxt.tpEventos.tpEvEPEC:
                        switch (Empresas.Configuracoes[emp].AmbienteCodigo)
                        {
                            case (int)NFe.Components.TipoAmbiente.taHomologacao:
                                if (Convert.ToInt32(item.tpAmb) == (int)NFe.Components.TipoAmbiente.taProducao)
                                {
                                    cErro += "Conteúdo da tag tpAmb do XML está com conteúdo indicando o envio para ambiente de produção e o UniNFe está configurado para ambiente de homologação.\r\n";
                                }
                                break;

                            case (int)NFe.Components.TipoAmbiente.taProducao:
                                if (Convert.ToInt32(item.tpAmb) == (int)NFe.Components.TipoAmbiente.taHomologacao)
                                {
                                    cErro += "Conteúdo da tag tpAmb do XML está com conteúdo indicando o envio para ambiente de homologação e o UniNFe está configurado para ambiente de produção.\r\n";
                                }
                                break;
                        }
                        int tpEmis = Convert.ToInt32(item.chNFe.Substring(34, 1));
                        if ((TipoEmissao)tpEmis != TipoEmissao.teEPEC)
                        {
                            cErro += string.Format("Tipo de emissão no XML deve ser \"{0}\" (tpEmis={1}), mas está informado \"{2}\" (tpEmis={3}).\r\n",
                                         NFe.Components.EnumHelper.GetDescription((TipoEmissao)Enum.ToObject(typeof(TipoEmissao), (int)TipoEmissao.teEPEC)),
                                         (int)TipoEmissao.teEPEC,
                                         NFe.Components.EnumHelper.GetDescription((TipoEmissao)Enum.ToObject(typeof(TipoEmissao), tpEmis)),
                                         tpEmis);
                        }
                        if ((TipoEmissao)Empresas.Configuracoes[emp].tpEmis != TipoEmissao.teEPEC)
                        {
                            cErro += string.Format("Tipo de emissão no Uninfe deve ser \"{0}\" (tpEmis={1}), mas está definido como \"{2}\" (tpEmis={3}).",
                                         NFe.Components.EnumHelper.GetDescription((TipoEmissao)Enum.ToObject(typeof(TipoEmissao), (int)TipoEmissao.teEPEC)),
                                         (int)TipoEmissao.teEPEC,
                                         NFe.Components.EnumHelper.GetDescription((TipoEmissao)Enum.ToObject(typeof(TipoEmissao), Empresas.Configuracoes[emp].tpEmis)),
                                         Empresas.Configuracoes[emp].tpEmis);
                        }
                        break;
                }
            }
            if (cErro != "")
                throw new Exception(cErro);
        }
        #endregion

        #region PedSta
        protected virtual void PedSta(int emp, DadosPedSta dadosPedSta)
        {
            dadosPedSta.tpAmb = 0;
            dadosPedSta.cUF = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
            dadosPedSta.tpEmis = Empresas.Configuracoes[emp].tpEmis;
            dadosPedSta.versao = "";// NFe.ConvertTxt.versoes.VersaoXMLStatusServico;

            ///
            /// danasa 12-9-2009
            /// 
            if (Path.GetExtension(this.NomeArquivoXML).ToLower() == ".txt")
            {
                // tpEmis|1						<<< opcional >>>
                // tpAmb|1
                // cUF|35
                // versao|3.10
                List<string> cLinhas = Functions.LerArquivo(this.NomeArquivoXML);
                Functions.PopulateClasse(dadosPedSta, cLinhas);
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(this.NomeArquivoXML);

                bool regravar = false;
                bool isCteMDFe = false;

                XmlNodeList consStatServList = doc.GetElementsByTagName("consStatServCte");
                if (consStatServList.Count == 0)
                {
                    consStatServList = doc.GetElementsByTagName("consStatServMDFe");
                    if (consStatServList.Count == 0)
                        consStatServList = doc.GetElementsByTagName("consStatServ");
                    else
                        isCteMDFe = true;
                }
                else
                    isCteMDFe = true;

                foreach (XmlNode consStatServNode in consStatServList)
                {
                    XmlElement consStatServElemento = (XmlElement)consStatServNode;

                    dadosPedSta.tpAmb = Convert.ToInt32("0" + consStatServElemento.GetElementsByTagName(TpcnResources.tpAmb.ToString())[0].InnerText);
                    dadosPedSta.versao = consStatServElemento.Attributes[NFe.Components.TpcnResources.versao.ToString()].InnerText;

                    if (consStatServElemento.GetElementsByTagName(TpcnResources.cUF.ToString()).Count != 0)
                    {
                        dadosPedSta.cUF = Convert.ToInt32("0" + consStatServElemento.GetElementsByTagName(TpcnResources.cUF.ToString())[0].InnerText);

                        if (isCteMDFe)
                        {
                            // para que o validador não rejeite, excluo a tag <cUF>
                            doc.DocumentElement.RemoveChild(consStatServElemento.GetElementsByTagName(TpcnResources.cUF.ToString())[0]);
                            regravar = true;
                        }
                    }

                    if (consStatServElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString()).Count != 0)
                    {
                        dadosPedSta.tpEmis = Convert.ToInt16(consStatServElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString())[0].InnerText);

                        // para que o validador não rejeite, excluo a tag <tpEmis>
                        doc.DocumentElement.RemoveChild(consStatServElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString())[0]);
                        regravar = true;
                    }

                    if (consStatServElemento.GetElementsByTagName(TpcnResources.mod.ToString()).Count != 0)
                    {
                        dadosPedSta.mod = consStatServElemento.GetElementsByTagName(TpcnResources.mod.ToString())[0].InnerText;
                        /// para que o validador não rejeite, excluo a tag <mod>
                        doc.DocumentElement.RemoveChild(consStatServElemento.GetElementsByTagName(TpcnResources.mod.ToString())[0]);
                        regravar = true;
                    }
                }
                // Salvar o arquivo modificado
                if (regravar)
                    doc.Save(this.NomeArquivoXML);
            }
        }
        #endregion

        #region PedSit
        protected virtual void PedSit(int emp, DadosPedSit dadosPedSit)
        {
            dadosPedSit.tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            dadosPedSit.chNFe = string.Empty;

            XmlDocument doc = new XmlDocument();
            doc.Load(this.NomeArquivoXML);

            bool doSave = false;

            XmlNodeList consSitNFeList = doc.GetElementsByTagName("consSitCTe");
            if (consSitNFeList.Count == 0)
            {
                consSitNFeList = doc.GetElementsByTagName("consSitMDFe");
            }
            foreach (XmlNode consSitNFeNode in consSitNFeList)
            {
                XmlElement consSitNFeElemento = (XmlElement)consSitNFeNode;

                dadosPedSit.tpAmb = Convert.ToInt32("0" + consSitNFeElemento.GetElementsByTagName(TpcnResources.tpAmb.ToString())[0].InnerText);
                dadosPedSit.chNFe = Functions.LerTag(consSitNFeElemento, TpcnResources.chCTe.ToString(), "") +
                                    Functions.LerTag(consSitNFeElemento, TpcnResources.chMDFe.ToString(), "");

                if (consSitNFeElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString()).Count != 0)
                {
                    dadosPedSit.tpEmis = Convert.ToInt16(consSitNFeElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString())[0].InnerText);
                    /// para que o validador não rejeite, excluo a tag <tpEmis>
                    doc.DocumentElement.RemoveChild(consSitNFeElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString())[0]);
                    /// salvo o arquivo modificado
                    doSave = true;
                }
            }
            /// salvo o arquivo modificado
            if (doSave)
                doc.Save(this.NomeArquivoXML);
        }
        #endregion

        #region EnvEvento
        protected virtual void EnvEvento(int emp, DadosenvEvento dadosEnvEvento)
        {
            //<?xml version="1.0" encoding="UTF-8"?>
            //<envEvento versao="1.00" xmlns="http://www.portalfiscal.inf.br/nfe">
            //  <idLote>000000000015255</idLote>
            //  <evento versao="1.00" xmlns="http://www.portalfiscal.inf.br/nfe">
            //    <infEvento Id="ID1101103511031029073900013955001000000001105112804108">
            //      <cOrgao>35</cOrgao>
            //      <tpAmb>2</tpAmb>
            //      <CNPJ>10290739000139</CNPJ>
            //      <chNFe>35110310290739000139550010000000011051128041</chNFe>
            //      <dhEvento>2011-03-03T08:06:00-03:00</dhEvento>
            //      <tpEvento>110110</tpEvento>
            //      <nSeqEvento>8</nSeqEvento>
            //      <verEvento>1.00</verEvento>
            //      <detEvento versao="1.00">
            //          <descEvento>Carta de Correção</descEvento>
            //          <xCorrecao>Texto de teste para Carta de Correção. Conteúdo do campo xCorrecao.</xCorrecao>
            //          <xCondUso>A Carta de Correção é disciplinada pelo § 1º-A do art. 7º do Convênio S/N, de 15 de dezembro de 1970 e pode ser utilizada para regularização de erro ocorrido na emissão de documento fiscal, desde que o erro não esteja relacionado com: I - as variáveis que determinam o valor do imposto tais como: base de cálculo, alíquota, diferença de preço, quantidade, valor da operação ou da prestação; II - a correção de dados cadastrais que implique mudança do remetente ou do destinatário; III - a data de emissão ou de saída.</xCondUso>
            //      </detEvento>
            //    </infEvento>
            //  </evento>
            //</envEvento>

            bool doSave = false;

            XmlDocument doc = new XmlDocument();
            doc.Load(this.NomeArquivoXML);

            XmlNodeList envEventoList = doc.GetElementsByTagName("infEvento");

            foreach (XmlNode envEventoNode in envEventoList)
            {
                XmlElement envEventoElemento = (XmlElement)envEventoNode;

                dadosEnvEvento.eventos.Add(new Evento()
                {
                    tpEvento = Functions.LerTag(envEventoElemento, NFe.Components.TpcnResources.tpEvento.ToString(), false),
                    tpAmb = Convert.ToInt32("0" + Functions.LerTag(envEventoElemento, NFe.Components.TpcnResources.tpAmb.ToString(), false)),
                    cOrgao = Convert.ToInt32("0" + Functions.LerTag(envEventoElemento, NFe.Components.TpcnResources.cOrgao.ToString(), false)),
                    nSeqEvento = Convert.ToInt32("0" + Functions.LerTag(envEventoElemento, NFe.Components.TpcnResources.nSeqEvento.ToString(), false))
                });
                dadosEnvEvento.eventos[dadosEnvEvento.eventos.Count - 1].chNFe =
                    Functions.LerTag(envEventoElemento, NFe.Components.TpcnResources.chNFe.ToString(), "") +
                    Functions.LerTag(envEventoElemento, NFe.Components.TpcnResources.chMDFe.ToString(), "") +
                    Functions.LerTag(envEventoElemento, NFe.Components.TpcnResources.chCTe.ToString(), "");

                dadosEnvEvento.eventos[dadosEnvEvento.eventos.Count - 1].tpEmis =
                    Convert.ToInt16(dadosEnvEvento.eventos[dadosEnvEvento.eventos.Count - 1].chNFe.Substring(34, 1));

                if (envEventoElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString()).Count != 0)
                {
                    var node = envEventoElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString())[0];

                    dadosEnvEvento.eventos[dadosEnvEvento.eventos.Count - 1].tpEmis = Convert.ToInt16("0" + node.InnerText);
                    /// para que o validador não rejeite, excluo a tag <tpEmis>
                    envEventoNode.RemoveChild(node);
                    doSave = true;
                }
            }
            /// salvo o arquivo modificado
            if (doSave)
                doc.Save(this.NomeArquivoXML);
        }
        #endregion

        #region XmlLMC()
        /// <summary>
        /// Efetua a leitura do XML de LMC e grava os dados no objeto "dadosLMC"
        /// </summary>
        /// <param name="emp">Empresa</param>
        /// <param name="dadosLMC">Objeto dados LMC para receber os valores</param>
        protected virtual void XmlLMC(int emp, DadosLMC dadosLMC)
        {
            dadosLMC.tpAmb = 0;
            dadosLMC.Id = string.Empty;
            dadosLMC.cUF = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
            dadosLMC.versao = string.Empty;

            XmlDocument doc = new XmlDocument();
            doc.Load(NomeArquivoXML);

            XmlElement infLivroCombustivel = (XmlElement)doc.GetElementsByTagName("infLivroCombustivel")[0];

            dadosLMC.tpAmb = Convert.ToInt32("0" + infLivroCombustivel.GetElementsByTagName(TpcnResources.tpAmb.ToString())[0].InnerText);
            dadosLMC.versao = infLivroCombustivel.Attributes[TpcnResources.versao.ToString()].InnerText;
            dadosLMC.Id = infLivroCombustivel.Attributes[TpcnResources.Id.ToString()].InnerText;

            XmlElement movimento = (XmlElement)infLivroCombustivel.GetElementsByTagName("movimento")[0];
            dadosLMC.dEmissao = Convert.ToDateTime(movimento.Attributes[TpcnResources.dEmissao.ToString()].InnerText);
        }
        #endregion

    }
}
