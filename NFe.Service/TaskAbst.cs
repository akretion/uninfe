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

        #region NomeClasseWS()
        /// <summary>
        /// Retorna o nome da classe do serviço passado por parâmetro do WebService do SEFAZ - CTe
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cUF">Código da UF</param>
        /// <returns>Nome da classe</returns>
        protected string NomeClasseWS(Servicos servico, int cUF)
        {
            string retorna = string.Empty;

            switch (Propriedade.TipoAplicativo)
            {
                case TipoAplicativo.Nfe:
                    retorna = NomeClasseWSNFe(servico, cUF);
                    break;
                case TipoAplicativo.Nfse:
                    retorna = NomeClasseWSNFSe(servico, cUF);
                    break;
            }

            return retorna;
        }
        #endregion

        #region NomeClasseWSNFe()
        /// <summary>
        /// Retorna o nome da classe do serviço passado por parâmetro do WebService do SEFAZ - NFe
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cUF">Código da UF</param>
        /// <returns>Nome da classe</returns>
        private string NomeClasseWSNFe(Servicos servico, int cUF)
        {
            string retorna = string.Empty;

            switch (servico)
            {
                #region NF-e
                case Servicos.InutilizarNumerosNFe:
                    retorna = "NfeInutilizacao2";
                    break;
                case Servicos.PedidoConsultaSituacaoNFe:
                    retorna = "NfeConsulta2";
                    break;
                case Servicos.ConsultaStatusServicoNFe:
                    retorna = "NfeStatusServico2";
                    break;
                case Servicos.PedidoSituacaoLoteNFe2:
                    retorna = "NfeRetAutorizacao";
                    break;
                case Servicos.PedidoSituacaoLoteNFe:
                    retorna = "NfeRetRecepcao2";
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
            bool taHomologacao = (Empresa.Configuracoes[Functions.FindEmpresaByThread()].tpAmb == Propriedade.TipoAmbiente.taHomologacao);

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
                        case Servicos.ConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "";
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
                                retorna = "CancelarNfse";
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
        #endregion

        #region NomeClasseCabecWS()
        /// <summary>
        /// Retorna o nome da classe de cabecalho do serviço
        /// </summary>
        /// <param name="cUF">Código da UF</param>
        /// <returns>Nome da classe de cabecalho</returns>
        protected string NomeClasseCabecWS(int cUF, Servicos servico)
        {
            return NomeClasseCabecWSNFe(cUF, servico);
        }
        #endregion

        #region NomeClasseCabecWSNFe()
        /// <summary>
        /// Retorna o nome da classe de cabecalho do serviço de NFe
        /// </summary>
        /// <returns>nome da classe de cabecalho do serviço de NFe</returns>
        private string NomeClasseCabecWSNFe(int cUF, Servicos servico)
        {
            string retorna = string.Empty;

            switch (servico)
            {
                #region MDFe
                case Servicos.PedidoConsultaSituacaoMDFe:
                case Servicos.PedidoSituacaoLoteMDFe:
                case Servicos.EnviarLoteMDFe:
                case Servicos.ConsultaStatusServicoMDFe:
                case Servicos.RecepcaoEventoMDFe:
                    retorna = "mdfeCabecMsg";
                    break;
                #endregion

                #region CTe
                case Servicos.InutilizarNumerosCTe:
                case Servicos.PedidoConsultaSituacaoCTe:
                case Servicos.PedidoSituacaoLoteCTe:
                case Servicos.EnviarLoteCTe:
                case Servicos.ConsultaStatusServicoCTe:
                case Servicos.RecepcaoEventoCTe:
                    retorna = "cteCabecMsg";

                    switch (cUF)
                    {
                        case 50:
                            retorna = "CTeCabecMsg";
                            break;
                    }
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
        /// <returns>nome do método da classe de serviço</returns>
        protected string NomeMetodoWS(Servicos servico, int cUF)
        {
            string retorna = string.Empty;

            switch (Propriedade.TipoAplicativo)
            {
                case TipoAplicativo.Nfe:
                    retorna = NomeMetodoWSNFe(servico, cUF);
                    break;
                case TipoAplicativo.Nfse:
                    retorna = NomeMetodoWSNFSe(servico, cUF);
                    break;
            }

            return retorna;
        }
        #endregion

        #region NomeMetodoWSNFe()
        /// <summary>
        /// Retorna o nome do método da classe de serviço - NFe
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cUF">Código da UF</param>
        /// <returns>nome do método da classe de serviço</returns>
        private string NomeMetodoWSNFe(Servicos servico, int cUF)
        {
            string retorna = string.Empty;

            switch (servico)
            {
                #region NF-e
                case Servicos.InutilizarNumerosNFe:
                    retorna = "nfeInutilizacaoNF2";
                    break;
                case Servicos.PedidoConsultaSituacaoNFe:
                    retorna = "nfeConsultaNF2";
                    break;
                case Servicos.ConsultaStatusServicoNFe:
                    retorna = "nfeStatusServicoNF2";
                    break;
                case Servicos.PedidoSituacaoLoteNFe2:
                    switch (cUF)
                    {
                        case 50:
                            retorna = "nfeRetAutorizacao";
                            break;
                        default:
                            retorna = "nfeRetAutorizacaoLote";
                            break;
                    }
                    break;
                case Servicos.PedidoSituacaoLoteNFe:
                    retorna = "nfeRetRecepcao2";
                    break;
                case Servicos.ConsultaCadastroContribuinte:
                    switch (cUF)
                    {
                        case 52:
                            retorna = "cadConsultaCadastro2";
                            break;
                        default:
                            retorna = "consultaCadastro2";
                            break;
                    }
                    break;
                case Servicos.EnviarLoteNfe2:
                    retorna = "nfeAutorizacaoLote";
                    break;

                case Servicos.EnviarLoteNfeZip2:
                    retorna = "nfeAutorizacaoLoteZip";
                    break;

                case Servicos.EnviarLoteNfe:
                    retorna = "nfeRecepcaoLote2";
                    break;
                case Servicos.EnviarCCe:
                case Servicos.EnviarEventoCancelamento:
                case Servicos.EnviarManifDest:
                case Servicos.RecepcaoEvento:
                    retorna = "nfeRecepcaoEvento";
                    break;
                #endregion

                #region MDF-e
                case Servicos.ConsultaStatusServicoMDFe:
                    retorna = "mdfeStatusServicoMDF";
                    break;
                case Servicos.EnviarLoteMDFe:
                    retorna = "mdfeRecepcaoLote";
                    break;
                case Servicos.PedidoSituacaoLoteMDFe:
                    retorna = "mdfeRetRecepcao";
                    break;
                case Servicos.PedidoConsultaSituacaoMDFe:
                    retorna = "mdfeConsultaMDF";
                    break;
                case Servicos.RecepcaoEventoMDFe:
                    retorna = "mdfeRecepcaoEvento";
                    break;
                #endregion

                #region CT-e
                case Servicos.ConsultaStatusServicoCTe:
                    retorna = "cteStatusServicoCT";
                    break;
                case Servicos.EnviarLoteCTe:
                    retorna = "cteRecepcaoLote";
                    break;
                case Servicos.PedidoSituacaoLoteCTe:
                    retorna = "cteRetRecepcao";
                    break;
                case Servicos.PedidoConsultaSituacaoCTe:
                    retorna = "cteConsultaCT";
                    break;
                case Servicos.InutilizarNumerosCTe:
                    retorna = "cteInutilizacaoCT";
                    break;
                case Servicos.RecepcaoEventoCTe:
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
            bool taHomologacao = (Empresa.Configuracoes[Functions.FindEmpresaByThread()].tpAmb == Propriedade.TipoAmbiente.taHomologacao);

            switch (Functions.PadraoNFSe(cMunicipio))
            {
                #region GINFES
                case PadroesNFSe.GINFES:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "ConsultarLoteRpsV3";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "ConsultarNfseV3";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "ConsultarNfsePorRpsV3";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRpsV3";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "RecepcionarLoteRpsV3";
                            break;
                    }
                    break;
                #endregion

                #region THEMA
                case PadroesNFSe.THEMA:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "consultarLoteRps";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "consultarNfse";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "consultarNfsePorRps";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "consultarSituacaoLoteRps";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "cancelarNfse";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "recepcionarLoteRpsLimitado"; //"recepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region BETHA
                case PadroesNFSe.BETHA:
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
                            retorna = "RecepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region CANOAS - RS (ABACO)
                case PadroesNFSe.CANOAS_RS:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "Execute";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "Execute";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "Execute";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "Execute";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "Execute";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "Execute";
                            break;
                    }
                    break;
                #endregion

                #region ISSNET
                case PadroesNFSe.ISSNET:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "ConsultarNfse";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "ConsultaNFSePorRPS";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "ConsultaSituacaoLoteRPS";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                        case Servicos.ConsultarURLNfse:
                            retorna = "ConsultarUrlVisualizacaoNfse";
                            break;
                        case Servicos.ConsultarURLNfseSerie:
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
                        case Servicos.ConsultarLoteRps:
                            retorna = "ConsultaLote";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "ConsultaNFeEmitidas";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "ConsultaNFe";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "ConsultaInformacoesLote";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "CancelamentoNFe";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            if (Empresa.Configuracoes[Functions.FindEmpresaByThread()].tpAmb == Propriedade.TipoAmbiente.taHomologacao)
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
                            retorna = "RecepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region GIF
                case PadroesNFSe.GIF:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "consultarNotaFiscal";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "obterCriticaLote";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "anularNotaFiscal";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "enviarLoteNotas";
                            break;
                        case Servicos.ConsultarURLNfse:
                            retorna = "obterNotasEmPNG";
                            break;
                    }
                    break;
                #endregion

                #region DUETO
                case PadroesNFSe.DUETO:
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
                            retorna = "RecepcionarLoteRps";
                            break;
                        case Servicos.ConsultarURLNfse:
                            retorna = "";
                            break;
                    }
                    break;
                #endregion

                #region WEBISS
                case PadroesNFSe.WEBISS:
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
                            retorna = "RecepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region PAULISTANA
                case PadroesNFSe.PAULISTANA:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "ConsultaLote";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "ConsultaNFeEmitidas";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "ConsultaNFe";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "ConsultaInformacoesLote";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "CancelamentoNFe";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            if (Empresa.Configuracoes[Functions.FindEmpresaByThread()].tpAmb == Propriedade.TipoAmbiente.taHomologacao)
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
                        case Servicos.ConsultarLoteRps:
                            retorna = "ConsultarLoteRPS";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "ConsultarNfse";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "ConsultarNfseRPS";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRPS";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "EnviarLoteRPS";
                            break;
                    }
                    break;
                #endregion

                #region PORTOVELHENSE
                case PadroesNFSe.PORTOVELHENSE:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "ConsultarNfsePorFaixa";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = ""; //Ainda não implmentado pelo municipio somenete pelo Site - Renan 
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "GerarNfse";
                            break;
                    }
                    break;


                #endregion

                #region PRONIN
                case PadroesNFSe.PRONIN:
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

                        case Servicos.CancelarNfse:
                            retorna = "CancelarNfse";
                            break;

                        case Servicos.RecepcionarLoteRps:
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
                        case Servicos.ConsultarLoteRps:
                            if (taHomologacao)
                                throw new NFe.Components.Exceptions.ServicoInexistenteHomologacaoException(servico);
                            else
                                retorna = "consultarLote";
                            break;

                        case Servicos.ConsultarNfse:
                            if (taHomologacao)
                                throw new NFe.Components.Exceptions.ServicoInexistenteHomologacaoException(servico);
                            else
                                retorna = "consultarNota";
                            break;

                        case Servicos.ConsultarNfsePorRps:
                            if (taHomologacao)
                                throw new NFe.Components.Exceptions.ServicoInexistenteHomologacaoException(servico);
                            else
                                retorna = "consultarNFSeRps";
                            break;

                        case Servicos.ConsultarSituacaoLoteRps:
                            if (taHomologacao)
                                throw new NFe.Components.Exceptions.ServicoInexistenteHomologacaoException(servico);
                            else
                                retorna = "consultarSequencialRps";
                            break;

                        case Servicos.CancelarNfse:
                            if (taHomologacao)
                                throw new NFe.Components.Exceptions.ServicoInexistenteHomologacaoException(servico);
                            else
                                retorna = "cancelar";
                            break;

                        case Servicos.RecepcionarLoteRps:
                            if (taHomologacao)
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
                        case Servicos.ConsultarLoteRps:
                            retorna = "mConsultaLoteRPS";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "mConsultaNFSeServicosPrestados";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "mConsultaNFSePorRPS";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "mConsultaNFSePorFaixa";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "mCancelamentoNFSe";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "mEnvioLoteRPSSincrono";
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
                case Servicos.AssinarValidarMDFeEnvioEmLote:
                case Servicos.MontarLoteVariosMDFe:
                case Servicos.MontarLoteUmMDFe:
                    oLerXML.Mdfe(arquivo);
                    break;

                case Servicos.AssinarValidarCTeEnvioEmLote:
                case Servicos.MontarLoteVariosCTe:
                case Servicos.MontarLoteUmCTe:
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
            int emp = Functions.FindEmpresaByThread();

            Boolean tudoOK = false;
            Boolean bAssinado = false;
            Boolean bValidadoSchema = false;
            Boolean bValidacaoGeral = false;

            //Criar Pasta dos XML´s a ser enviado em Lote já assinados
            string pastaLoteAssinado = pasta + Propriedade.NomePastaXMLAssinado;

            //Se o arquivo XML já existir na pasta de assinados, vou avisar o ERP que já tem um em andamento
            string arqDestino = pastaLoteAssinado + "\\" + Functions.ExtrairNomeArq(NomeArquivoXML, ".xml") + ".xml";

            try
            {
                //Fazer uma leitura de algumas tags do XML
                DadosNFeClass oDadosNFe = this.LerXMLNFe(NomeArquivoXML);
                string ChaveNfe = oDadosNFe.chavenfe;
                string TpEmis = oDadosNFe.tpEmis;

                //Inserir NFe no XML de controle do fluxo
                FluxoNfe oFluxoNfe = new FluxoNfe();
                if (oFluxoNfe.NfeExiste(ChaveNfe))
                {
                    //Mover o arquivo da pasta em processamento para a pasta de XML´s com erro
                    oAux.MoveArqErro(Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + Functions.ExtrairNomeArq(NomeArquivoXML, ".xml") + ".xml");

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
                    Functions.DeletarArquivo(Empresa.Configuracoes[emp].PastaErro + "\\" + Functions.ExtrairNomeArq(NomeArquivoXML, ".xml") + ".xml");
                }

                //Validações gerais
                if (ValidacoesGeraisXMLNFe(NomeArquivoXML, oDadosNFe))
                {
                    bValidacaoGeral = true;
                }

                //Assinar o arquivo XML
                if (bValidacaoGeral)
                {
                    AssinaturaDigital assDig = new AssinaturaDigital();

                    assDig.Assinar(NomeArquivoXML, emp, Convert.ToInt32(oDadosNFe.cUF));

                    bAssinado = true;
                }

                // Validar o Arquivo XML da NFe com os Schemas se estiver assinado
                if (bValidacaoGeral && bAssinado)
                {
                    ValidarXML validar = new ValidarXML(NomeArquivoXML, Convert.ToInt32(oDadosNFe.cUF));

                    string cResultadoValidacao = validar.ValidarArqXML(NomeArquivoXML);
                    if (cResultadoValidacao == "")
                    {
                        bValidadoSchema = true;
                    }
                    else
                    {
                        //Registrar o erro da validação do schema para o sistema ERP
                        throw new Exception(cResultadoValidacao);
                    }
                }

                //Mover o arquivo XML da pasta de lote para a pasta de XML´s assinados
                if (bValidadoSchema)
                {
                    //Se a pasta de assinados não existir, vamos criar
                    if (!Directory.Exists(pastaLoteAssinado))
                    {
                        Directory.CreateDirectory(pastaLoteAssinado);
                    }

                    FileInfo fiDestino = new FileInfo(arqDestino);

                    if (!File.Exists(arqDestino) || (long)DateTime.Now.Subtract(fiDestino.LastWriteTime).TotalMilliseconds >= 60000) //60.000 ms que corresponde á 60 segundos que corresponde a 1 minuto
                    {
                        //Mover o arquivo para a pasta de XML´s assinados
                        //FileInfo oArquivo = new FileInfo(NomeArquivoXML);
                        //oArquivo.MoveTo(arqDestino);
                        Functions.Move(NomeArquivoXML, arqDestino);

                        tudoOK = true;
                    }
                    else
                    {
                        oFluxoNfe.InserirNfeFluxo(ChaveNfe, arqDestino);

                        throw new IOException("Esta nota fiscal já está na pasta de Notas Fiscais assinadas e em processo de envio, desta forma não é possível enviar a mesma novamente.\r\n" +
                            NomeArquivoXML);
                    }
                }

                if (tudoOK)
                {
                    oFluxoNfe.InserirNfeFluxo(ChaveNfe, arqDestino);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    string extFinal = Propriedade.ExtEnvio.Nfe;
                    string extErro = Propriedade.ExtRetorno.Nfe_ERR;
                    switch (Servico)
                    {
                        case Servicos.AssinarValidarMDFeEnvioEmLote:
                        case Servicos.MontarLoteUmMDFe:
                            extFinal = Propriedade.ExtEnvio.MDFe;
                            extErro = Propriedade.ExtRetorno.MDFe_ERR;
                            break;

                        case Servicos.AssinarValidarCTeEnvioEmLote:
                        case Servicos.MontarLoteUmCTe:
                            extFinal = Propriedade.ExtEnvio.Cte;
                            extErro = Propriedade.ExtRetorno.Cte_ERR;
                            break;
                    }

                    TFunctions.GravarArqErroServico(NomeArquivoXML, extFinal, extErro, ex);

                    //Se já foi movido o XML da Nota Fiscal para a pasta em Processamento, vou ter que 
                    //forçar mover para a pasta de XML com erro neste ponto.
                    oAux.MoveArqErro(arqDestino);
                }
                catch
                {
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
        /// <param name="strArquivoNFe">Arquivo XML da NFe</param>
        /// <returns>true = Validado com sucesso</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>16/04/2009</date>
        protected bool ValidacoesGeraisXMLNFe(string strArquivoNFe, DadosNFeClass oDadosNFe)
        {
            int emp = Functions.FindEmpresaByThread();

            bool booValido = false;
            int nPos = 0;
            string cTextoErro = "";

            switch (Servico)
            {
                case Servicos.AssinarValidarMDFeEnvioEmLote:
                case Servicos.MontarLoteUmMDFe:
                    booValido = true;
                    nPos = 1;
                    break;

                case Servicos.AssinarValidarCTeEnvioEmLote:
                case Servicos.MontarLoteUmCTe:
                    //Verificar o tipo de emissão Wbate com o configurado, se não bater vai retornar um erro para o ERP
                    //Wandrey 15/08/2012
                    if ((Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teNormal && (oDadosNFe.tpEmis == "1" || oDadosNFe.tpEmis == "5" || oDadosNFe.tpEmis == "9")) ||
                        (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCAN && (oDadosNFe.tpEmis == "6")) ||
                        (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCRS && (oDadosNFe.tpEmis == "7")) ||
                        (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCSP && (oDadosNFe.tpEmis == "8")))
                    {
                        booValido = true;
                    }
                    else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teFSDA && (oDadosNFe.tpEmis == "5"))
                    {
                        booValido = false; //Retorno somente falso mas sem exception para não fazer nada. Wandrey 19/06/2009
                    }
                    else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teOffLine && (oDadosNFe.tpEmis == "9"))
                    {
                        booValido = false; //Retorno somente falso mas sem exception para não fazer nada. Wandrey 19/06/2009
                    }
                    else
                    {
                        booValido = false;

                        if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teNormal && oDadosNFe.tpEmis == "6")
                        {
                            cTextoErro = "O UniNFe está configurado para enviar a Nota Fiscal ao Ambiente Normal " +
                                "e o XML está configurado para enviar ao SVC-AN.\r\n\r\n";
                        }
                        else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teNormal && oDadosNFe.tpEmis == "7")
                        {
                            cTextoErro = "O UniNFe está configurado para enviar a Nota Fiscal ao Ambiente Normal " +
                                "e o XML está configurado para enviar ao SVC-RS.\r\n\r\n";
                        }
                        else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teNormal && oDadosNFe.tpEmis == "8")
                        {
                            cTextoErro = "O UniNFe está configurado para enviar a Nota Fiscal ao Ambiente Normal " +
                                "e o XML está configurado para enviar ao SVC-SP.\r\n\r\n";
                        }
                        else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCRS && oDadosNFe.tpEmis == "8")
                        {
                            cTextoErro = "O UniNFe está configurado para enviar a Nota Fiscal ao Ambiente da SVC-RS " +
                                "e o XML está configurado para enviar ao SVC-SP.\r\n\r\n";
                        }
                        else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCRS && oDadosNFe.tpEmis == "6")
                        {
                            cTextoErro = "O UniNFe está configurado para enviar a Nota Fiscal ao Ambiente da SVC-RS " +
                                "e o XML está configurado para enviar ao SVC-AN.\r\n\r\n";
                        }
                        else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCSP && oDadosNFe.tpEmis == "7")
                        {
                            cTextoErro = "O UniNFe está configurado para enviar a Nota Fiscal ao Ambiente da SVC-SP " +
                                "e o XML está configurado para enviar ao SVC-RS.\r\n\r\n";
                        }
                        else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCSP && oDadosNFe.tpEmis == "6")
                        {
                            cTextoErro = "O UniNFe está configurado para enviar a Nota Fiscal ao Ambiente da SVC-SP " +
                                "e o XML está configurado para enviar ao SVC-AN.\r\n\r\n";
                        }
                        else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCAN && oDadosNFe.tpEmis == "7")
                        {
                            cTextoErro = "O UniNFe está configurado para enviar a Nota Fiscal ao Ambiente da SVC-AN " +
                                "e o XML está configurado para enviar ao SVC-RS.\r\n\r\n";
                        }
                        else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCAN && oDadosNFe.tpEmis == "8")
                        {
                            cTextoErro = "O UniNFe está configurado para enviar a Nota Fiscal ao Ambiente da SVC-AN " +
                                "e o XML está configurado para enviar ao SVC-SP.\r\n\r\n";
                        }
                        else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCSP && (oDadosNFe.tpEmis == "1" || oDadosNFe.tpEmis == "5" || oDadosNFe.tpEmis == "9"))
                        {
                            cTextoErro = "O UniNFe está configurado para enviar a Nota Fiscal ao SVCSP " +
                                "e o XML está configurado para enviar ao ambiente normal.\r\n\r\n";
                        }
                        else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCRS && (oDadosNFe.tpEmis == "1" || oDadosNFe.tpEmis == "5" || oDadosNFe.tpEmis == "9"))
                        {
                            cTextoErro = "O UniNFe está configurado para enviar a Nota Fiscal ao SVCRS " +
                                "e o XML está configurado para enviar ao ambiente normal.\r\n\r\n";
                        }
                        else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCAN && (oDadosNFe.tpEmis == "1" || oDadosNFe.tpEmis == "5" || oDadosNFe.tpEmis == "9"))
                        {
                            cTextoErro = "O UniNFe está configurado para enviar a Nota Fiscal ao SVCAN " +
                                "e o XML está configurado para enviar ao ambiente normal.\r\n\r\n";
                        }

                        cTextoErro += "O XML não será enviado e será movido para a pasta de XML com erro para análise.";

                        throw new Exception(cTextoErro);
                    }

                    break;

                case Servicos.AssinarValidarNFeEnvioEmLote:
                case Servicos.MontarLoteUmaNFe:
                    //Verificar o tipo de emissão se bate com o configurado, se não bater vai retornar um erro para o ERP
                    //danasa 8-2009
                    if ((Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teNormal && (oDadosNFe.tpEmis == "1" || oDadosNFe.tpEmis == "2" || oDadosNFe.tpEmis == "5" || oDadosNFe.tpEmis == "4" || oDadosNFe.tpEmis == "9")) ||
                        (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSCAN && oDadosNFe.tpEmis == "3") ||
                        (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCAN && oDadosNFe.tpEmis == "6") ||
                        (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCRS && oDadosNFe.tpEmis == "7"))
                    {
                        booValido = true;
                    }
                    // danasa 8-2009
                    else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teContingencia && (oDadosNFe.tpEmis == "2"))
                    {
                        booValido = false; //Retorno somente falso mas sem exception para não fazer nada. Wandrey 09/06/2009
                    }
                    else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teDPEC && (oDadosNFe.tpEmis == "4"))
                    {
                        booValido = false; //Retorno somente falso mas sem exception para não fazer nada. Wandrey 19/06/2009
                    }
                    else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teFSDA && (oDadosNFe.tpEmis == "5"))
                    {
                        booValido = false; //Retorno somente falso mas sem exception para não fazer nada. Wandrey 19/06/2009
                    }
                    else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teOffLine && (oDadosNFe.tpEmis == "9"))
                    {
                        booValido = false; //Retorno somente falso mas sem exception para não fazer nada. Wandrey 19/06/2009
                    }
                    else
                    {
                        booValido = false;

                        // danasa 8-2009
                        if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teNormal && oDadosNFe.tpEmis == "3")
                        {
                            cTextoErro = "O UniNFe está configurado para enviar a Nota Fiscal ao Ambiente da SEFAZ " +
                                "(Secretaria Estadual da Fazenda) e o XML está configurado para enviar " +
                                "para o SVCAN.\r\n\r\n";
                        }
                        // danasa 8-2009
                        else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSCAN && (oDadosNFe.tpEmis == "1" || oDadosNFe.tpEmis == "2" || oDadosNFe.tpEmis == "5" || oDadosNFe.tpEmis == "9"))
                        {
                            cTextoErro = "O UniNFe está configurado para enviar a Nota Fiscal ao SVCAN " +
                                "e o XML está configurado para enviar para o Ambiente da SEFAZ (Secretaria Estadual da Fazenda)\r\n\r\n";
                        }

                        cTextoErro += "O XML não será enviado e será movido para a pasta de XML com erro para análise.";

                        throw new Exception(cTextoErro);
                    }

                    break;
            }


            #region Verificar se os valores das tag´s que compõe a chave da nfe estão batendo com as informadas na chave
            //Verificar se os valores das tag´s que compõe a chave da nfe estão batendo com as informadas na chave
            if (booValido)
            {
                cTextoErro = string.Empty;

                #region Tag <cUF>
                if (oDadosNFe.cUF != oDadosNFe.chavenfe.Substring(3 + nPos, 2))
                {
                    cTextoErro += "O código da UF informado na tag <cUF> está diferente do informado na chave da NF-e.\r\n" +
                        "Código da UF informado na tag <cUF>: " + oDadosNFe.cUF + "\r\n" +
                        "Código da UF informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(3 + nPos, 2) + "\r\n\r\n";
                    booValido = false;
                }
                #endregion

                #region Tag <tpEmis>
                if (oDadosNFe.tpEmis != oDadosNFe.chavenfe.Substring(37 + nPos, 1))
                {
                    cTextoErro += "O código numérico informado na tag <tpEmis> está diferente do informado na chave da NF-e.\r\n" +
                        "Código numérico informado na tag <tpEmis>: " + oDadosNFe.tpEmis + "\r\n" +
                        "Código numérico informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(37 + nPos, 1) + "\r\n\r\n";
                    booValido = false;
                }
                #endregion

                #region Tag <cNF>
                if (oDadosNFe.cNF != oDadosNFe.chavenfe.Substring(38 + nPos, 8))
                {
                    cTextoErro += "O código numérico informado na tag <cNF> está diferente do informado na chave da NF-e.\r\n" +
                        "Código numérico informado na tag <cNF>: " + oDadosNFe.cNF + "\r\n" +
                        "Código numérico informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(38 + nPos, 8) + "\r\n\r\n";
                    booValido = false;
                }
                #endregion

                #region Tag <mod>
                if (oDadosNFe.mod != oDadosNFe.chavenfe.Substring(23 + nPos, 2))
                {
                    cTextoErro += "O modelo informado na tag <mod> está diferente do informado na chave da NF-e.\r\n" +
                        "Modelo informado na tag <mod>: " + oDadosNFe.mod + "\r\n" +
                        "Modelo informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(23 + nPos, 2) + "\r\n\r\n";
                    booValido = false;
                }
                #endregion

                #region Tag <nNF>
                if (Convert.ToInt32(oDadosNFe.nNF) != Convert.ToInt32(oDadosNFe.chavenfe.Substring(28 + nPos, 9)))
                {
                    cTextoErro += "O número da NF-e informado na tag <nNF> está diferente do informado na chave da NF-e.\r\n" +
                        "Número da NFe informado na tag <nNF>: " + Convert.ToInt32(oDadosNFe.nNF).ToString() + "\r\n" +
                        "Número da NFe informado na chave da NF-e: " + Convert.ToInt32(oDadosNFe.chavenfe.Substring(28 + nPos, 9)).ToString() + "\r\n\r\n";
                    booValido = false;
                }
                #endregion

                #region Tag <cDV>
                if (oDadosNFe.cDV != oDadosNFe.chavenfe.Substring(46 + nPos, 1))
                {
                    cTextoErro += "O número do dígito verificador informado na tag <cDV> está diferente do informado na chave da NF-e.\r\n" +
                        "Número do dígito verificador informado na tag <cDV>: " + oDadosNFe.cDV + "\r\n" +
                        "Número do dígito verificador informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(46 + nPos, 1) + "\r\n\r\n";
                    booValido = false;
                }
                #endregion

                #region Tag <CNPJ> da tag <emit>
                if (oDadosNFe.CNPJ != oDadosNFe.chavenfe.Substring(9 + nPos, 14))
                {
                    cTextoErro += "O CNPJ do emitente informado na tag <emit><CNPJ> está diferente do informado na chave da NF-e.\r\n" +
                        "CNPJ do emitente informado na tag <emit><CNPJ>: " + oDadosNFe.CNPJ + "\r\n" +
                        "CNPJ do emitente informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(9 + nPos, 14) + "\r\n\r\n";
                    booValido = false;
                }
                #endregion

                #region Tag <serie>
                if (Convert.ToInt32(oDadosNFe.serie) != Convert.ToInt32(oDadosNFe.chavenfe.Substring(25 + nPos, 3)))
                {
                    cTextoErro += "A série informada na tag <serie> está diferente da informada na chave da NF-e.\r\n" +
                        "Série informada na tag <cDV>: " + Convert.ToInt32(oDadosNFe.serie).ToString() + "\r\n" +
                        "Série informada na chave da NF-e: " + Convert.ToInt32(oDadosNFe.chavenfe.Substring(25 + nPos, 3)).ToString() + "\r\n\r\n";
                    booValido = false;
                }
                #endregion

                #region Tag <dEmi>
                if (oDadosNFe.dEmi.Month.ToString("00") != oDadosNFe.chavenfe.Substring(7 + nPos, 2) ||
                    oDadosNFe.dEmi.Year.ToString("0000").Substring(2, 2) != oDadosNFe.chavenfe.Substring(5 + nPos, 2))
                {
                    cTextoErro += "A ano e mês da emissão informada na tag <dEmi> está diferente da informada na chave da NF-e.\r\n" +
                        "Mês/Ano da data de emissão informada na tag <dEmi>: " + oDadosNFe.dEmi.Month.ToString("00") + "/" + oDadosNFe.dEmi.Year.ToString("0000").Substring(2, 2) + "\r\n" +
                        "Mês/Ano informados na chave da NF-e: " + oDadosNFe.chavenfe.Substring(5 + nPos, 2) + "/" + oDadosNFe.chavenfe.Substring(7 + nPos, 2) + "\r\n\r\n";
                    booValido = false;
                }
                #endregion

                if (!booValido)
                {
                    throw new Exception(cTextoErro);
                }
            }
            #endregion

            return booValido;
        }
        #endregion

        #region LoteNfe()
        /// <summary>
        /// Auxiliar na geração do arquivo XML de Lote de notas fiscais
        /// </summary>
        /// <param name="Arquivo">Nome do arquivo XML da NFe para montagem do lote de 1 NFe</param>
        /// <date>07/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public void LoteNfe(string Arquivo, string versaoXml)
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
        public void LoteNfe(List<string> lstArquivoNfe, string versaoXml)
        {
            oGerarXML.LoteNfe(lstArquivoNfe, versaoXml);
        }
        #endregion

        #region ProcessaNFeDenegada
        protected void ProcessaNFeDenegada(int emp, LerXML oLerXml, string strArquivoNFe, string protNFe, string versao)
        {
            string strProtNfe;

            if (File.Exists(strArquivoNFe))
            {
                oLerXml.Nfe(strArquivoNFe);
                string nomePastaEnviado = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
                                            PastaEnviados.Denegados.ToString() + "\\" +
                                            Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(oLerXml.oDadosNfe.dEmi);
                string dArquivo = Path.Combine(nomePastaEnviado, Path.GetFileName(strArquivoNFe).Replace(Propriedade.ExtEnvio.Nfe, "-den.xml"));

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
                    string strNomeArqDenegadaNFe = oGerarXML.XmlDistNFe(strArquivoNFe, strProtNfe, "-den.xml", versao);
                    ///
                    /// exclui o XML denegado, se existir
                    //string destinoArquivo = Path.Combine(nomePastaEnviado, Path.GetFileName(strNomeArqDenegadaNFe));
                    Functions.DeletarArquivo(dArquivo);
                    ///
                    /// Move a NFE-denegada da pasta em processamento para NFe Denegadas
                    TFunctions.MoverArquivo(strNomeArqDenegadaNFe, PastaEnviados.Denegados, oLerXml.oDadosNfe.dEmi);
                    ///
                    /// verifica se o arquivo da NFe já existe na pasta denegadas
                    dArquivo = Path.Combine(nomePastaEnviado, Path.GetFileName(strArquivoNFe));
                    if (!File.Exists(dArquivo))
                    {
                        if (Empresa.Configuracoes[emp].PastaBackup.Trim() != "")
                        {
                            //Criar Pasta do Mês para gravar arquivos enviados
                            string nomePastaBackup = Empresa.Configuracoes[emp].PastaBackup + "\\" +
                                                        PastaEnviados.Denegados + "\\" +
                                                        Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(oLerXml.oDadosNfe.dEmi);
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

                    TFunctions.ExecutaUniDanfe(strNomeArqDenegadaNFe, oLerXml.oDadosNfe.dEmi, "danfe");
                }
            }
        }
        #endregion

        #region XmlPedRec()
        /// <summary>
        /// Gerar o XML de consulta do recibo do lote da nfe
        /// </summary>
        /// <param name="empresa">Código da empresa</param>
        /// <param name="nRec">Número do recibo a ser consultado</param>
        /// <param name="versao">Versao do Schema XML</param>
        public void XmlPedRec(int empresa, string nRec, string versao)
        {
            GerarXML gerarXML = new GerarXML(empresa);
            gerarXML.XmlPedRec(Servicos.PedidoSituacaoLoteNFe, nRec, versao);
        }
        #endregion

        #region XmlPedRecCTe()
        /// <summary>
        /// Gerar o XML de consulta do recibo do lote da cte
        /// </summary>
        /// <param name="empresa">Código da empresa</param>
        /// <param name="nRec">Número do recibo a ser consultado</param>
        /// <param name="versao">Versao do Schema XML</param>
        public void XmlPedRecCTe(int empresa, string nRec, string versao)
        {
            GerarXML gerarXML = new GerarXML(empresa);
            gerarXML.XmlPedRec(Servicos.PedidoSituacaoLoteCTe, nRec, versao);
        }
        #endregion

        #region XmlPedRecMDFe()
        /// <summary>
        /// Gerar o XML de consulta do recibo do lote da cte
        /// </summary>
        /// <param name="empresa">Código da empresa</param>
        /// <param name="nRec">Número do recibo a ser consultado</param>
        /// <param name="versao">Versao do Schema XML</param>
        public void XmlPedRecMDFe(int empresa, string nRec, string versao)
        {
            GerarXML gerarXML = new GerarXML(empresa);
            gerarXML.XmlPedRec(Servicos.PedidoSituacaoLoteMDFe, nRec, versao);
        }
        #endregion

        #region XmlRetorno()
        /// <summary>
        /// Auxiliar na geração do arquivo XML de retorno para o ERP quando estivermos utilizando o InvokeMember para chamar o método
        /// </summary>
        /// <param name="pFinalArqEnvio">Final do nome do arquivo de solicitação do serviço.</param>
        /// <param name="pFinalArqRetorno">Final do nome do arquivo que é para ser gravado o retorno.</param>
        /// <date>07/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public void XmlRetorno(string pFinalArqEnvio, string pFinalArqRetorno)
        {
            oGerarXML.XmlRetorno(pFinalArqEnvio, pFinalArqRetorno, this.vStrXmlRetorno);
        }
        #endregion
    }
}
