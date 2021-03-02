using NFe.Certificado;
using NFe.Components;
using NFe.Components.QRCode;
using NFe.Settings;
using NFe.Validate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace NFe.Service
{
    public abstract class TaskAbst
    {
        #region Objetos

        protected Auxiliar oAux = new Auxiliar();
        protected InvocarObjeto oInvocarObj = new InvocarObjeto();
        protected GerarXML oGerarXML = new GerarXML(Thread.CurrentThread);

        #endregion Objetos

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
            get => mNomeArquivoXML;
            set
            {
                mNomeArquivoXML = value;
                oGerarXML.NomeXMLDadosMsg = value;
            }
        }

        /// <summary>
        /// Conteúdo do XML para que será enviado
        /// </summary>
        public XmlDocument ConteudoXML = new XmlDocument();

        /// <summary>
        /// Serviço que está sendo executado (Envio de Nota, Cancelamento, Consulta, etc...)
        /// </summary>
        private Servicos mServico;

        public Servicos Servico
        {
            get => mServico;
            protected set
            {
                mServico = value;
                oGerarXML.Servico = value;
            }
        }

        /// <summary>
        /// Se o vXmlNFeDadosMsg é um XML
        /// </summary>
        public bool vXmlNfeDadosMsgEhXML    //danasa 12-9-2009
=> Path.GetExtension(NomeArquivoXML).ToLower() == ".xml";

        #endregion Propriedades

        public abstract void Execute();

        #region Métodos para definição dos nomes das classes e métodos da NFe, CTe, NFSe e MDFe

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

        #endregion NomeClasseCabecWS()

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
                case Servicos.MDFeEnviarLoteSinc:
                case Servicos.MDFeConsultaStatusServico:
                case Servicos.MDFeRecepcaoEvento:
                case Servicos.MDFeConsultaNaoEncerrado:
                    retorna = "mdfeCabecMsg";
                    break;

                #endregion MDFe

                #region CTe

                case Servicos.CTeInutilizarNumeros:
                case Servicos.CTePedidoConsultaSituacao:
                case Servicos.CTePedidoSituacaoLote:
                case Servicos.CTeEnviarLote:
                case Servicos.CTeConsultaStatusServico:
                case Servicos.CteRecepcaoOS:
                case Servicos.CTeRecepcaoEvento:
                    retorna = "cteCabecMsg";

                    if (cUF == 50 &&
                        (tpEmis == (int)TipoEmissao.teNormal || tpEmis == (int)TipoEmissao.teEPEC || tpEmis == (int)TipoEmissao.teFSDA)
                        && servico != Servicos.CteRecepcaoOS)
                    {
                        retorna = "CTeCabecMsg";
                    }

                    break;

                #endregion CTe

                #region NFe

                default:
                    retorna = "nfeCabecMsg";
                    break;

                    #endregion NFe
            }

            return retorna;
        }

        #endregion NomeClasseCabecWS()

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
            {
                retorna = NomeMetodoWSNFe(servico, cUF, versao);
            }

            return retorna;
        }

        protected string NomeMetodoWS(Servicos servico, int cUF)
        {
            return NomeMetodoWS(servico, cUF, "");
        }

        #endregion NomeMetodoWS()

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

                case Servicos.NFeEnviarLote:
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

                case Servicos.EventoCCe:
                case Servicos.EventoCancelamento:
                case Servicos.EventoManifestacaoDest:
                case Servicos.EventoRecepcao:
                    retorna = "nfeRecepcaoEvento";
                    break;

                #endregion NF-e

                #region MDF-e

                case Servicos.MDFeConsultaStatusServico:
                    retorna = "mdfeStatusServicoMDF";
                    break;

                case Servicos.MDFeEnviarLoteSinc:
                    retorna = "mdfeRecepcao";
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

                #endregion MDF-e

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

                #endregion CT-e

                default:
                    break;
            }

            return retorna;
        }

        #endregion NomeMetodoWSNFe()

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
            PadroesNFSe padroesNFSe = Functions.PadraoNFSe(cMunicipio);

            switch (padroesNFSe)
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

                #endregion ISSWEB

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

                #endregion GINFES

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

                #endregion THEMA

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

                #endregion BETHA

                #region GOIANIA

                case PadroesNFSe.GOIANIA:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfseRps";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "GerarNfse";
                            break;
                    }
                    break;

                #endregion GOIANIA

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

                #endregion CANOAS - RS (ABACO)

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

                #endregion ISSNET

                #region ISSONLINE

                case PadroesNFSe.ISSONLINE_ASSESSORPUBLICO:
                    retorna = "Execute";
                    break;

                #endregion ISSONLINE

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

                #endregion BHISS

                #region GIF

                case PadroesNFSe.GIF:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "";
                            break;

                        case Servicos.NFSeConsultar:
                            retorna = "consultarSituacaoNotaFiscal";
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

                #endregion GIF

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

                #endregion DUETO

                #region WEBISS

                case PadroesNFSe.WEBISS:
                case PadroesNFSe.WEBISS_202:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;

                        case Servicos.NFSeConsultar:
                            if (padroesNFSe == PadroesNFSe.WEBISS_202)
                            {
                                retorna = "ConsultarNfseServicoPrestado";
                            }
                            else if (cMunicipio.Equals(3300308) ||
                                     cMunicipio.Equals(3303302) ||
                                     cMunicipio.Equals(4301602))
                            {
                                retorna = "ConsultarNfsePorFaixa";
                            }
                            else
                            {
                                retorna = "ConsultarNfse";
                            }
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

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "RecepcionarLoteRpsSincrono";
                            break;

                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;
                    }
                    break;

                #endregion WEBISS

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
                            {
                                retorna = "TesteEnvioLoteRPS";
                            }
                            else
                            {
                                retorna = "EnvioLoteRPS";
                            }

                            break;

                        case Servicos.NFSeConsultarNFSeRecebidas:
                            retorna = "ConsultaNFeRecebidas";
                            break;
                    }
                    break;

                #endregion PAULISTANA

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

                        case Servicos.NFSeConsultarStatusNota:
                            retorna = "ConsultarSituacaoNfse";
                            break;
                    }
                    break;

                #endregion SALVADOR_BA

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

                #endregion PORTOVELHENSE

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

                        case Servicos.NFSeSubstituirNfse:
                            retorna = "SubstituirNfse";
                            break;
                    }
                    break;

                #endregion PRONIN

                #region ISSONLINE4R (4R Sistemas)

                case PadroesNFSe.ISSONLINE4R:
                    retorna = "Execute";
                    break;

                #endregion ISSONLINE4R (4R Sistemas)

                #region DSF

                case PadroesNFSe.DSF:
                    if (cMunicipio.ToString() == "3549904") //São José dos Campos-SP
                    {
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
                                retorna = "CancelarNfseV3";
                                break;

                            case Servicos.NFSeRecepcionarLoteRps:
                                retorna = "RecepcionarLoteRpsV3";
                                break;

                            default:
                                throw new NFe.Components.Exceptions.ServicoInexistenteException();
                        }
                    }
                    else
                    {
                        switch (servico)
                        {
                            case Servicos.NFSeConsultarLoteRps:
                                if (taHomologacao)
                                {
                                    throw new NFe.Components.Exceptions.ServicoInexistenteHomologacaoException(servico);
                                }
                                else
                                {
                                    retorna = "consultarLote";
                                }

                                break;

                            case Servicos.NFSeConsultar:
                                if (taHomologacao)
                                {
                                    throw new NFe.Components.Exceptions.ServicoInexistenteHomologacaoException(servico);
                                }
                                else
                                {
                                    retorna = "consultarNota";
                                }

                                break;

                            case Servicos.NFSeConsultarPorRps:
                                if (taHomologacao)
                                {
                                    throw new NFe.Components.Exceptions.ServicoInexistenteHomologacaoException(servico);
                                }
                                else
                                {
                                    retorna = "consultarNFSeRps";
                                }

                                break;

                            case Servicos.NFSeConsultarSituacaoLoteRps:
                                if (taHomologacao)
                                {
                                    throw new NFe.Components.Exceptions.ServicoInexistenteHomologacaoException(servico);
                                }
                                else
                                {
                                    retorna = "consultarSequencialRps";
                                }

                                break;

                            case Servicos.NFSeCancelar:
                                if (taHomologacao)
                                {
                                    throw new NFe.Components.Exceptions.ServicoInexistenteHomologacaoException(servico);
                                }
                                else
                                {
                                    retorna = "cancelar";
                                }

                                break;

                            case Servicos.NFSeRecepcionarLoteRps:
                                if (taHomologacao &&
                                    cMunicipio.ToString() != "2111300") //São Luiz - MA
                                {
                                    if (cMunicipio.ToString().Equals("5002704") || // Campo grande - MS não tem web service de teste
                                        cMunicipio.ToString().Equals("3303500")) //Nova Iguaçu-RS
                                    {
                                        throw new NFe.Components.Exceptions.ServicoInexistenteHomologacaoException(servico);
                                    }
                                    else
                                    {
                                        retorna = "testeEnviar";
                                    }
                                }
                                else
                                {
                                    retorna = "enviar";
                                }

                                break;

                            default:
                                throw new NFe.Components.Exceptions.ServicoInexistenteException();
                        }
                    }
                    break;

                #endregion DSF

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

                        case Servicos.NFSeConsultaSequenciaLoteNotaRPS:
                            retorna = "mConsultaSequenciaLoteNotaRPS";
                            break;

                        case Servicos.NFSeCancelar:
                            retorna = "mCancelamentoNFSe";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "mEnvioLoteRPSSincrono";
                            break;
                    }
                    break;

                #endregion TECNOSISTEMAS

                #region TIPLAN

                case PadroesNFSe.TIPLAN_203:
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
                            retorna = "ConsultarSituacaoLoteRps";
                            break;

                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "RecepcionarLoteRpsSincrono";
                            break;
                    }
                    break;

                case PadroesNFSe.TIPLAN:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            if (cMunicipio.Equals(3300407) ||
                                cMunicipio.Equals(3304003) ||
                                cMunicipio.Equals(2611606) ||
                                cMunicipio.Equals(3300100) ||
                                cMunicipio.Equals(3302403))
                            {
                                retorna = "ConsultarLoteRps";
                            }
                            else
                            {
                                retorna = "ConsultarLoteRPS";
                            }

                            break;

                        case Servicos.NFSeConsultar:
                            if (cMunicipio.Equals(3303302))
                            {
                                retorna = "ConsultarNfsePorFaixa";
                            }
                            else
                            {
                                retorna = "ConsultarNfse";
                            }

                            break;

                        case Servicos.NFSeConsultarPorRps:
                            if (cMunicipio.Equals(3300407) ||
                                cMunicipio.Equals(3304003) ||
                                cMunicipio.Equals(2611606) ||
                                cMunicipio.Equals(3300100) ||
                                cMunicipio.Equals(3302403))
                            {
                                retorna = "ConsultarNfsePorRps";
                            }
                            else
                            {
                                retorna = "ConsultarNfseRPS";
                            }

                            break;

                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            if (cMunicipio.Equals(3300407) ||
                                cMunicipio.Equals(3304003) ||
                                cMunicipio.Equals(2611606) ||
                                cMunicipio.Equals(3300100) ||
                                cMunicipio.Equals(3302403))
                            {
                                retorna = "ConsultarSituacaoLoteRps";
                            }
                            else
                            {
                                retorna = "ConsultarSituacaoLoteRPS";
                            }

                            break;

                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                    }
                    break;

                #endregion TIPLAN

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

                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;
                    }
                    break;

                #endregion CARIOCA

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

                #endregion SIGCORP_SIGISS

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

                #endregion SMARAPD

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
                            retorna = "ConsultarSituacaoLoteRps";
                            break;

                        case Servicos.RecepcionarLoteCfse:
                            retorna = "RecepcionarLoteCfse";
                            break;

                        case Servicos.RecepcionarLoteCfseSincrono:
                            retorna = "RecepcionarLoteCfseSincrono";
                            break;

                        case Servicos.CancelarCfse:
                            retorna = "CancelarCupom";
                            break;

                        case Servicos.ConsultarLoteCfse:
                            retorna = "ConsultarLoteCupom";
                            break;

                        case Servicos.ConsultarCfse:
                            retorna = "ConsultarCfse";
                            break;

                        case Servicos.ConfigurarTerminalCfse:
                            retorna = "ConfigurarTerminal";
                            break;

                        case Servicos.EnviarInformeManutencaoCfse:
                            retorna = "InformarManutencaoTerminal";
                            break;

                        case Servicos.InformeTrasmissaoSemMovimentoCfse:
                            retorna = "InformeTrasmissaoSemMovimento";
                            break;

                        case Servicos.ConsultarDadosCadastroCfse:
                            retorna = "ConsultarDadosCadastro";
                            break;
                    }
                    break;

                #endregion FINTEL

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

                #endregion EQUIPLANO

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

                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;

                        case Servicos.NFSeConsultarNFSePDF:
                            retorna = "ConsultarNotapdf";
                            break;
                    }
                    break;

                #endregion PRODATA

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

                #endregion VVISS

                #region FISSLEX

                case PadroesNFSe.FISSLEX:
                    retorna = "Execute";
                    break;

                #endregion FISSLEX

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

                #endregion NATALENSE

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

                #endregion NOTA INTELIGENTE

                #region CAMACARI_BA

                case PadroesNFSe.CAMACARI_BA:
                    switch (servico)
                    {
                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "RecepcionarLoteRpsSincrono";
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

                #endregion CAMACARI_BA

                #region ACTCON

                case PadroesNFSe.PORTALFACIL_ACTCON_202:
                case PadroesNFSe.PORTALFACIL_ACTCON:
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
                            retorna = "ConsultarNfsePorFaixa";
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

                #endregion ACTCON

                #region PUBLICA

                case PadroesNFSe.PUBLICA:
                    switch (servico)
                    {
                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;

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
                            retorna = "ConsultarNfseFaixa";
                            break;

                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;

                        case Servicos.NFSeConsultarURL:
                            retorna = "";
                            break;

                        case Servicos.NFSeConsultarURLSerie:
                            retorna = "";
                            break;
                    }
                    break;

                #endregion PUBLICA

                #region BSIT-BR

                case PadroesNFSe.BSITBR:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "consultarLoteRps";
                            break;

                        case Servicos.NFSeConsultarPorRps:
                            retorna = "consultarNfseRps";
                            break;

                        case Servicos.NFSeCancelar:
                            retorna = "cancelarNfse";
                            break;

                        case Servicos.NFSeGerarNfse:
                            retorna = "gerarNfse";
                            break;

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "enviarLoteRpsSincrono";
                            break;
                    }
                    break;

                #endregion BSIT-BR

                #region ABASE

                case PadroesNFSe.ABASE:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultaLoteRps";
                            break;

                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultaNfseRps";
                            break;

                        case Servicos.NFSeCancelar:
                            retorna = "CancelaNfse";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                    }
                    break;

                #endregion ABASE

                #region LEXSOM

                case PadroesNFSe.LEXSOM:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRPS";
                            break;

                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNFSEPorRPS";
                            break;

                        case Servicos.NFSeCancelar:
                            retorna = "CancelamentoNFSE";
                            break;

                        case Servicos.NFSeConsultar:
                            retorna = "ConsultaNFSE";
                            break;

                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRPS";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecebeLoteRPS";
                            break;
                    }
                    break;

                #endregion LEXSOM

                #region SH3

                case PadroesNFSe.SH3:
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
                            retorna = "ConsultarNfsePorFaixa";
                            break;
                    }
                    break;

                #endregion SH3

                #region SUPERNOVA

                case PadroesNFSe.SUPERNOVA:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;

                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfseFaixa";
                            break;

                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;

                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "RecepcionarLoteRpsSincrono";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;

                        case Servicos.NFSeConsultarNFSeTomados:
                            retorna = "ConsultarNfseServicoTomado";
                            break;
                    }
                    break;

                #endregion SUPERNOVA

                #region DBSELLER

                case PadroesNFSe.DBSELLER:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;

                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;

                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;

                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;
                    }
                    break;

                #endregion DBSELLER

                #region MARINGA_PR

                case PadroesNFSe.MARINGA_PR:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;

                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfseRps";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "EnviarLoteRps";
                            break;

                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "EnviarLoteRpsSincrono";
                            break;

                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfseFaixa";
                            break;
                    }
                    break;

                #endregion MARINGA_PR

                #region INTERSOL

                case PadroesNFSe.INTERSOL:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;

                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;

                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;

                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfse";
                            break;

                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;
                    }
                    break;

                #endregion INTERSOL

                #region MANAUS_AM

                case PadroesNFSe.MANAUS_AM:
                    retorna = "Execute";
                    break;

                #endregion MANAUS_AM

                #region JOINVILLE_SC

                case PadroesNFSe.JOINVILLE_SC:
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
                    }
                    break;

                #endregion JOINVILLE_SC

                #region AVMB_ASTEN e EMBRAS

                case PadroesNFSe.AVMB_ASTEN:
                case PadroesNFSe.EMBRAS:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;

                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;

                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "RecepcionarLoteRpsSincrono";
                            break;

                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfsePorFaixa";
                            break;

                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;

                        case Servicos.NFSeSubstituirNfse:
                            retorna = "SubstituirNfse";
                            break;
                    }
                    break;

                #endregion AVMB_ASTEN e EMBRAS

                #region DESENVOLVECIDADE

                case PadroesNFSe.DESENVOLVECIDADE:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "consultarLoteRpsEnvio";
                            break;

                        case Servicos.NFSeConsultarPorRps:
                            retorna = "consultarNfseRpsEnvio";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "enviarLoteRpsEnvio";
                            break;

                        case Servicos.NFSeCancelar:
                            retorna = "cancelarNfseEnvio";
                            break;

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "enviarLoteRpsSincronoEnvio";
                            break;

                        case Servicos.NFSeGerarNfse:
                            retorna = "gerarNfseEnvio";
                            break;
                    }
                    break;

                #endregion DESENVOLVECIDADE

                #region VITORIA_ES

                case PadroesNFSe.VITORIA_ES:
                    switch (servico)
                    {
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

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "RecepcionarLoteRpsSincrono";
                            break;

                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;

                        case Servicos.NFSeSubstituirNfse:
                            retorna = "SubstituirNfse";
                            break;
                    }
                    break;

                #endregion VITORIA_ES

                #region MODERNIZACAO_PUBLICA

                case PadroesNFSe.MODERNIZACAO_PUBLICA:
                    switch (servico)
                    {
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

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "RecepcionarLoteRpsSincrono";
                            break;

                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;

                        case Servicos.NFSeSubstituirNfse:
                            retorna = "SubstituirNfse";
                            break;
                    }
                    break;

                #endregion MODERNIZACAO_PUBLICA

                #region GEISWEB

                case PadroesNFSe.GEISWEB:
                    switch (servico)
                    {
                        case Servicos.NFSeCancelar:
                            retorna = "CancelaNfse";
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

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "RecepcionarLoteRpsSincrono";
                            break;

                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;

                        case Servicos.NFSeSubstituirNfse:
                            retorna = "SubstituirNfse";
                            break;
                    }
                    break;

                #endregion GEISWEB

                #region E_RECEITA

                case PadroesNFSe.E_RECEITA:
                    switch (servico)
                    {
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

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "RecepcionarLoteRpsSincrono";
                            break;

                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;

                        case Servicos.NFSeSubstituirNfse:
                            retorna = "SubstituirNfse";
                            break;
                    }
                    break;

                #endregion E_RECEITA

                #region ADM_SISTEMAS

                case PadroesNFSe.ADM_SISTEMAS:
                    switch (servico)
                    {
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

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "RecepcionarLoteRpsSincrono";
                            break;

                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;

                        case Servicos.NFSeSubstituirNfse:
                            retorna = "SubstituirNfse";
                            break;
                    }
                    break;

                #endregion ADM_SISTEMAS

                #region PUBLIC_SOFT

                case PadroesNFSe.PUBLIC_SOFT:
                    switch (servico)
                    {
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfseEnvio";
                            break;

                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfseRpsEnvio";
                            break;

                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfseFaixaEnvio";
                            break;

                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfseEnvio";
                            break;

                        case Servicos.NFSeConsultarURL:
                            retorna = "LinksNotaFiscal";
                            break;
                    }
                    break;

                #endregion PUBLIC_SOFT

                #region MEGASOFT

                case PadroesNFSe.MEGASOFT:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;

                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;
                    }
                    break;
                #endregion MEGASOFT

                #region CECAM

                case PadroesNFSe.CECAM:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNotaFiscal";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            if (Empresas.Configuracoes[Empresas.FindEmpresaByThread()].AmbienteCodigo == (int)NFe.Components.TipoAmbiente.taHomologacao)
                            {
                                retorna = "EnviarLoteNotaFiscalDeTeste";
                            }
                            else
                            {
                                retorna = "EnviarLoteNotaFiscal";
                            }

                            break;

                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNotaFiscalEletronica";
                            break;
                    }
                    break;
                #endregion CECAM

                #region INDAIATUBA_SP

                case PadroesNFSe.INDAIATUBA_SP:
                    switch (servico)
                    {
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

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "RecepcionarLoteRpsSincrono";
                            break;

                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;

                        case Servicos.NFSeSubstituirNfse:
                            retorna = "SubstituirNfse";
                            break;

                        case Servicos.NFSeConsultarNFSeTomados:
                            retorna = "ConsultarNfseServicoTomado";
                            break;
                    }
                    break;

                #endregion INDAIATUBA_SP

                #region SISPMJP

                case PadroesNFSe.SISPMJP:

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

                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "RecepcionarLoteRpsSincrono";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;

                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;
                    }
                    break;

                #endregion SISPMJP

                #region SIGCORP_SIGISS_203

                case PadroesNFSe.SIGCORP_SIGISS_203:
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
                            retorna = "ConsultarSituacaoLoteRps";
                            break;

                        case Servicos.NFSeCancelar:
                            retorna = "CancelaNota";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "RecepcionarLoteRpsSincrono";
                            break;

                        case Servicos.NFSeSubstituirNfse:
                            retorna = "SubstituirNfse";
                            break;

                        case Servicos.NFSeConsultarNFSeTomados:
                            retorna = "ConsultarNfseServicoTomado";
                            break;

                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;
                    }
                    break;

                #endregion SIGCORP_SIGISS_203

                #region SMARAPD_204

                case PadroesNFSe.SMARAPD_204:
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
                            retorna = "ConsultarSituacaoLoteRps";
                            break;

                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "RecepcionarLoteRpsSincrono";
                            break;

                        case Servicos.NFSeSubstituirNfse:
                            retorna = "SubstituirNfse";
                            break;

                        case Servicos.NFSeConsultarNFSeTomados:
                            retorna = "ConsultarNfseServicoTomado";
                            break;

                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;
                    }
                    break;

                #endregion SMARAPD_204

                #region D2TI

                case PadroesNFSe.D2TI:
                    retorna = "executar";
                    break;
                #endregion D2TI

                #region IIBRASIL
                case PadroesNFSe.IIBRASIL:
                    switch (servico)

                    {
                        case Servicos.NFSeCancelar:
                            retorna = "CancelarNfse";
                            break;

                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;

                        case Servicos.NFSeConsultarPorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;

                        case Servicos.NFSeGerarNfse:
                            retorna = "GerarNfse";
                            break;

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "RecepcionarLoteRpsSincrono";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;

                        case Servicos.NFSeConsultar:
                            retorna = "ConsultarNfsePorFaixa";
                            break;

                        case Servicos.NFSeSubstituirNfse:
                            retorna = "SubstituirNfse";
                            break;

                        case Servicos.NFSeConsultarNFSeTomados:
                            retorna = "ConsultarNfseServicoTomado";
                            break;
                    }
                    #endregion IIBRASIL
                    break;

                #region SYSMAR
                case PadroesNFSe.SYSMAR:
                    switch (servico)
                    {
                        case Servicos.NFSeConsultarLoteRps:
                            retorna = "CONSULTARLOTERPS";
                            break;

                        case Servicos.NFSeConsultar:
                            retorna = "CONSULTARNFSEFAIXA";
                            break;

                        case Servicos.NFSeConsultarPorRps:
                            retorna = "CONSULTARNFSERPS";
                            break;

                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;

                        case Servicos.NFSeCancelar:
                            retorna = "CANCELARNFSE";
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            retorna = "ENVIARLOTERPS";
                            break;

                        case Servicos.NFSeRecepcionarLoteRpsSincrono:
                            retorna = "ENVIARLOTERPSSINCRONO";
                            break;

                        case Servicos.NFSeSubstituirNfse:
                            retorna = "SUBSTITUIRNFSE";
                            break;

                        case Servicos.NFSeConsultarNFSeTomados:
                            retorna = "CONSULTARNFSESERVICOTOMADO";
                            break;

                        case Servicos.NFSeGerarNfse:
                            retorna = "GERARNFSE";
                            break;
                    }
                    break;
                #endregion SYSMAR

                #region RLZ_INFORMATICA_02
                case PadroesNFSe.RLZ_INFORMATICA_02:
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
                            retorna = "ConsultarNfsePorFaixa";
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

                    #endregion RLZ_INFORMATICA_02
            }
            return retorna;
        }

        #endregion NomeMetodoWSNFSe()

        #endregion Métodos para definição dos nomes das classes e métodos da NFe, CTe, NFSe e MDFe

        #region LerXMLNFe()

        /// <summary>
        /// Le o conteúdo do XML da NFe
        /// </summary>
        /// <param name="conteudoXML">Conteúdo do XML</param>
        /// <returns>Retorna o conteúdo do XML da NFe</returns>
        public DadosNFeClass LerXMLNFe(XmlDocument conteudoXML)
        {
            LerXML lerXML = new LerXML();

            switch (Servico)
            {
                case Servicos.MDFeAssinarValidarEnvioEmLote:
                case Servicos.MDFeMontarLoteVarios:
                case Servicos.MDFeMontarLoteUm:
                    lerXML.Mdfe(conteudoXML);
                    break;

                case Servicos.CTeAssinarValidarEnvioEmLote:
                case Servicos.CTeMontarLoteVarios:
                case Servicos.CTeMontarLoteUm:
                    lerXML.Cte(conteudoXML);
                    break;

                default:
                    lerXML.Nfe(conteudoXML);
                    break;
            }

            return lerXML.oDadosNfe;
        }

        #endregion LerXMLNFe()

        #region AssinarValidarXMLNFe()

        /// <summary>
        /// Assinar e validar o XML da Nota Fiscal Eletrônica e move para a pasta de assinados
        /// </summary>
        public void AssinarValidarXMLNFe()
        {
            XmlDocument conteudoXML = new XmlDocument();
            try
            {
                conteudoXML.Load(NomeArquivoXML);
            }
            catch
            {
                conteudoXML.LoadXml(File.ReadAllText(NomeArquivoXML, System.Text.Encoding.UTF8));
            }

            AssinarValidarXMLNFe(conteudoXML);

            StreamWriter sw = File.CreateText(NomeArquivoXML);
            sw.Write(conteudoXML.OuterXml);
            sw.Close();
        }

        #endregion AssinarValidarXMLNFe()

        #region AssinarValidarXMLNFe()

        /// <summary>
        /// Assinar e validar o XML da Nota Fiscal Eletrônica e move para a pasta de assinados
        /// </summary>
        /// <param name="conteudoXML">Nome da pasta onde está o XML a ser validado e assinado</param>
        /// <returns>true = Conseguiu assinar e validar</returns>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 03/04/2009
        /// </remarks>
        public void AssinarValidarXMLNFe(XmlDocument conteudoXML)
        {
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                //Fazer uma leitura de algumas tags do XML
                DadosNFeClass dadosNFe = LerXMLNFe(conteudoXML);

                RespTecnico respTecnico = new RespTecnico(Empresas.Configuracoes[emp].RespTecCNPJ,
                    Empresas.Configuracoes[emp].RespTecXContato,
                    Empresas.Configuracoes[emp].RespTecEmail,
                    Empresas.Configuracoes[emp].RespTecTelefone,
                    Empresas.Configuracoes[emp].RespTecIdCSRT,
                    Empresas.Configuracoes[emp].RespTecCSRT);

                respTecnico.AdicionarResponsavelTecnico(conteudoXML);

                string ChaveNfe = dadosNFe.chavenfe;
                string TpEmis = dadosNFe.tpEmis;

                //Inserir NFe no XML de controle do fluxo
                FluxoNfe oFluxoNfe = new FluxoNfe(emp);
                if (oFluxoNfe.NfeExiste(ChaveNfe))
                {
                    //Mover o arquivo da pasta em processamento para a pasta de XML´s com erro
                    oAux.MoveArqErro(Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                        PastaEnviados.EmProcessamento.ToString() + "\\" +
                        Path.GetFileName(NomeArquivoXML));

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
                    Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + Path.GetFileName(NomeArquivoXML));
                }

                //Assinar o arquivo XML
                AssinaturaDigital assDig = new AssinaturaDigital();
                assDig.Assinar(conteudoXML, emp, Convert.ToInt32(dadosNFe.cUF));

                #region Adicionar a tag do QrCode no NFCe
                if (!string.IsNullOrEmpty(Empresas.Configuracoes[emp].IdentificadorCSC) && dadosNFe.mod == "65")
                {
                    if (Empresas.Configuracoes[emp].URLConsultaDFe == null)
                    {
                        if (!File.Exists(Path.Combine(Application.StartupPath, "sefaz.inc")))
                        {
                            throw new Exception("Não foi possível localizar o arquivo SEFAZ.INC na pasta de execução do UniNFe, por favor, reinstale o aplicativo.");
                        }
                        else
                        {
                            throw new Exception("Não foi possível localizar o link do QRCode no arquivo SEFAZ.INC.");
                        }
                    }

                    QRCodeNFCe qrCode = new QRCodeNFCe(conteudoXML);

                    string url;

                    if (dadosNFe.versao == "4.00")
                    {
                        url = Empresas.Configuracoes[emp].AmbienteCodigo == (int)TipoAmbiente.taHomologacao ? Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCeH_400 : Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCe_400;
                    }
                    else
                    {
                        url = Empresas.Configuracoes[emp].AmbienteCodigo == (int)TipoAmbiente.taHomologacao ? Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCeH : Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCe;
                    }

                    string linkUFManual = Empresas.Configuracoes[emp].AmbienteCodigo == (int)TipoAmbiente.taHomologacao ? Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCeMH : Empresas.Configuracoes[emp].URLConsultaDFe.UrlNFCeM;

                    qrCode.GerarLinkConsulta(url, Empresas.Configuracoes[emp].IdentificadorCSC, Empresas.Configuracoes[emp].TokenCSC, linkUFManual);
                }
                #endregion Adicionar a tag do QrCode no NFCe

                #region Adicionar a tag do QrCode no MDFe
                else if (dadosNFe.mod == "58") // MDFe
                {
                    QRCodeMDFe qrCodeMDFe = new QRCodeMDFe(conteudoXML);
                    qrCodeMDFe.MontarLinkQRCode(Empresas.Configuracoes[emp].X509Certificado);
                }
                #endregion Adicionar a tag do QrCode no MDFe

                #region Adicionar a tag do QrCode no CTe
                else if (dadosNFe.mod == "57") // CTe
                {
                    string urlCte = Empresas.Configuracoes[emp].AmbienteCodigo == (int)TipoAmbiente.taHomologacao ?
                        Empresas.Configuracoes[emp].URLConsultaDFe.UrlCTeQrCodeH :
                        Empresas.Configuracoes[emp].URLConsultaDFe.UrlCTeQrCodeP;

                    QRCodeCTe qrCodeCte = new QRCodeCTe(conteudoXML, urlCte);
                    qrCodeCte.MontarLinkQRCode(Empresas.Configuracoes[emp].X509Certificado);
                }
                #endregion Adicionar a tag do QrCode no CTe

                // Validar o Arquivo XML da NFe com os Schemas se estiver assinado
                ValidarXML validar = new ValidarXML(conteudoXML, Convert.ToInt32(dadosNFe.cUF), false);
                string cResultadoValidacao = validar.ValidarArqXML(conteudoXML, NomeArquivoXML);
                if (cResultadoValidacao != "")
                {
                    //Registrar o erro da validação do schema para o sistema ERP
                    throw new Exception(cResultadoValidacao);
                }

                //Validações de modal
                if (conteudoXML.DocumentElement.Name.Equals("CTe") ||
                    conteudoXML.DocumentElement.Name.Equals("MDFe"))
                {
                    string resultValidacao = "";
                    XmlDocument infModal = new XmlDocument();
                    XmlDocument modal = new XmlDocument();

                    if (conteudoXML.GetElementsByTagName("infModal")[0] != null)
                    {
                        foreach (XmlElement item in conteudoXML.GetElementsByTagName("infModal"))
                        {
                            infModal.LoadXml(item.OuterXml);
                            modal.LoadXml(item.InnerXml);
                        }

                        ValidarXML validarModal = new ValidarXML(infModal, Empresas.Configuracoes[emp].UnidadeFederativaCodigo, false);
                        resultValidacao += validarModal.ValidarArqXML(modal, NomeArquivoXML);

                        if (resultValidacao != "")
                        {
                            throw new Exception(resultValidacao);
                        }
                    }
                }

                //Validações gerais
                ValidacoesGeraisXMLNFe(dadosNFe);

                oFluxoNfe.InserirNfeFluxo(ChaveNfe, dadosNFe.mod, NomeArquivoXML);
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

        #endregion AssinarValidarXMLNFe()

        #region ValidacoesGerais()

        /// <summary>
        /// Efetua uma leitura do XML da nota fiscal eletrônica e faz diversas conferências do seu conteúdo e bloqueia se não
        /// estiver de acordo com as configurações do UNINFE
        /// </summary>
        /// <param name="dadosNFe">Objeto com o conteúdo das tags do XML</param>
        /// <returns>true = Validado com sucesso</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>16/04/2009</date>
        protected void ValidacoesGeraisXMLNFe(DadosNFeClass dadosNFe)
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
                            booValido = (tpEmis == (int)TipoEmissao.teSVCSP);
                            break;

                        case (int)TipoEmissao.teSVCAN:
                            booValido = (tpEmis == (int)TipoEmissao.teSVCAN);
                            break;

                        case (int)TipoEmissao.teSVCRS:
                            booValido = (tpEmis == (int)TipoEmissao.teSVCRS);
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
                         "XML: " + EnumHelper.GetDescription((TipoEmissao)Enum.ToObject(typeof(TipoEmissao), tpEmis)) +
                         " (tpEmis = " + tpEmis.ToString() + "). " +
                         "UniNFe: " + EnumHelper.GetDescription((TipoEmissao)Enum.ToObject(typeof(TipoEmissao), Empresas.Configuracoes[emp].tpEmis)) +
                         " (tpEmis = " + Empresas.Configuracoes[emp].tpEmis.ToString() + "). " +
                        cTextoErro2;

                throw new Exception(cTextoErro);
            }

            switch (tpEmis)
            {
                case (int)TipoEmissao.teSVCAN:
                case (int)TipoEmissao.teSVCRS:
                    Components.Municipio se = Propriedade.Estados.First(s => s.CodigoMunicipio.Equals(Convert.ToInt32(dadosNFe.cUF)));
                    if (se.UF == "SP" && dadosNFe.mod == "57") // São Paulo para CTe é SVC-RS, então não dá para pegar o que está no Websevice.XML pois está definido o da NFe. Wandrey 12/03/2018
                    {
                        se.svc = TipoEmissao.teSVCRS;
                    }

                    if (se.svc != (TipoEmissao)tpEmis && se.svc != TipoEmissao.teNone)
                    {
                        throw new Exception("UF: " + Functions.CodigoParaUF(Convert.ToInt32(dadosNFe.cUF)) + " não está sendo atendida pelo WebService do SVC: " +
                                EnumHelper.GetDescription((TipoEmissao)Enum.ToObject(typeof(TipoEmissao), tpEmis)) + ". " + cTextoErro2);
                    }
                    break;
            }

            #region Verificar o ambiente da nota com o que está configurado no uninfe. Wandrey 20/08/2014

            if (booValido)
            {
                switch (Empresas.Configuracoes[emp].AmbienteCodigo)
                {
                    case (int)TipoAmbiente.taHomologacao:
                        if (Convert.ToInt32(dadosNFe.tpAmb) == (int)TipoAmbiente.taProducao)
                        {
                            cTextoErro = "Conteúdo da tag tpAmb do XML está com conteúdo indicando o envio para ambiente de produção e o UniNFe está configurado para ambiente de homologação.";
                            throw new Exception(cTextoErro);
                        }
                        break;

                    case (int)TipoAmbiente.taProducao:
                        if (Convert.ToInt32(dadosNFe.tpAmb) == (int)TipoAmbiente.taHomologacao)
                        {
                            cTextoErro = "Conteúdo da tag tpAmb do XML está com conteúdo indicando o envio para ambiente de homologação e o UniNFe está configurado para ambiente de produção.";
                            throw new Exception(cTextoErro);
                        }
                        break;
                }
            }

            #endregion Verificar o ambiente da nota com o que está configurado no uninfe. Wandrey 20/08/2014

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

                #endregion Tag <cUF>

                #region Tag <tpEmis>

                if (dadosNFe.tpEmis != dadosNFe.chavenfe.Substring(37 + nPos, 1))
                {
                    cTextoErro += "O código numérico informado na tag <tpEmis> está diferente do informado na chave da NF-e.\r\n" +
                        "Código numérico informado na tag <tpEmis>: " + dadosNFe.tpEmis + "\r\n" +
                        "Código numérico informado na chave da NF-e: " + dadosNFe.chavenfe.Substring(37 + nPos, 1) + "\r\n\r\n";
                    booValido = false;
                }

                #endregion Tag <tpEmis>

                #region Tag <cNF>

                if (dadosNFe.cNF != dadosNFe.chavenfe.Substring(38 + nPos, 8))
                {
                    cTextoErro += "O código numérico informado na tag <cNF> está diferente do informado na chave da NF-e.\r\n" +
                        "Código numérico informado na tag <cNF>: " + dadosNFe.cNF + "\r\n" +
                        "Código numérico informado na chave da NF-e: " + dadosNFe.chavenfe.Substring(38 + nPos, 8) + "\r\n\r\n";
                    booValido = false;
                }

                #endregion Tag <cNF>

                #region Tag <mod>

                if (dadosNFe.mod != dadosNFe.chavenfe.Substring(23 + nPos, 2))
                {
                    cTextoErro += "O modelo informado na tag <mod> está diferente do informado na chave da NF-e.\r\n" +
                        "Modelo informado na tag <mod>: " + dadosNFe.mod + "\r\n" +
                        "Modelo informado na chave da NF-e: " + dadosNFe.chavenfe.Substring(23 + nPos, 2) + "\r\n\r\n";
                    booValido = false;
                }

                #endregion Tag <mod>

                #region Tag <nNF>

                if (Convert.ToInt32(dadosNFe.nNF) != Convert.ToInt32(dadosNFe.chavenfe.Substring(28 + nPos, 9)))
                {
                    cTextoErro += "O número da NF-e informado na tag <nNF> está diferente do informado na chave da NF-e.\r\n" +
                        "Número da NFe informado na tag <nNF>: " + Convert.ToInt32(dadosNFe.nNF).ToString() + "\r\n" +
                        "Número da NFe informado na chave da NF-e: " + Convert.ToInt32(dadosNFe.chavenfe.Substring(28 + nPos, 9)).ToString() + "\r\n\r\n";
                    booValido = false;
                }

                #endregion Tag <nNF>

                #region Tag <cDV>

                if (dadosNFe.cDV != dadosNFe.chavenfe.Substring(46 + nPos, 1))
                {
                    cTextoErro += "O número do dígito verificador informado na tag <cDV> está diferente do informado na chave da NF-e.\r\n" +
                        "Número do dígito verificador informado na tag <cDV>: " + dadosNFe.cDV + "\r\n" +
                        "Número do dígito verificador informado na chave da NF-e: " + dadosNFe.chavenfe.Substring(46 + nPos, 1) + "\r\n\r\n";
                    booValido = false;
                }

                #endregion Tag <cDV>

                #region Tag <CNPJ> da tag <emit>

                if (string.IsNullOrEmpty(dadosNFe.CNPJ))
                {
                    if (string.IsNullOrEmpty(dadosNFe.CPF))
                    {
                        cTextoErro += "O CNPJ ou CPF do emitente não foi localizado no XML <emit><CNPJ> ou <emit><CPF>.\r\n\r\n";
                        booValido = false;
                    }
                    else if (dadosNFe.CPF != dadosNFe.chavenfe.Substring(12 + nPos, 11))
                    {
                        cTextoErro += "O CPF do emitente informado na tag <emit><CPF> está diferente do informado na chave da NF-e.\r\n" +
                            "CPF do emitente informado na tag <emit><CPF>: " + dadosNFe.CPF + "\r\n" +
                            "CPF do emitente informado na chave da NF-e: " + dadosNFe.chavenfe.Substring(12 + nPos, 11) + "\r\n\r\n";
                        booValido = false;
                    }
                }
                else if (dadosNFe.CNPJ != dadosNFe.chavenfe.Substring(9 + nPos, 14))
                {
                    cTextoErro += "O CNPJ do emitente informado na tag <emit><CNPJ> está diferente do informado na chave da NF-e.\r\n" +
                        "CNPJ do emitente informado na tag <emit><CNPJ>: " + dadosNFe.CNPJ + "\r\n" +
                        "CNPJ do emitente informado na chave da NF-e: " + dadosNFe.chavenfe.Substring(9 + nPos, 14) + "\r\n\r\n";
                    booValido = false;
                }

                #endregion Tag <CNPJ> da tag <emit>

                #region Tag <serie>

                if (Convert.ToInt32(dadosNFe.serie) != Convert.ToInt32(dadosNFe.chavenfe.Substring(25 + nPos, 3)))
                {
                    cTextoErro += "A série informada na tag <serie> está diferente da informada na chave da NF-e.\r\n" +
                        "Série informada na tag <cDV>: " + Convert.ToInt32(dadosNFe.serie).ToString() + "\r\n" +
                        "Série informada na chave da NF-e: " + Convert.ToInt32(dadosNFe.chavenfe.Substring(25 + nPos, 3)).ToString() + "\r\n\r\n";
                    booValido = false;
                }

                #endregion Tag <serie>

                #region Tag <dEmi>

                if (dadosNFe.dEmi.Month.ToString("00") != dadosNFe.chavenfe.Substring(7 + nPos, 2) ||
                    dadosNFe.dEmi.Year.ToString("0000").Substring(2, 2) != dadosNFe.chavenfe.Substring(5 + nPos, 2))
                {
                    cTextoErro += "O ano e mês da emissão informada na tag " + dadosNFe.versao == "2.00" ? "<dEmi> " : "<dhEmi> " + "está diferente da informada na chave da NF-e.\r\n" +
                        "Mês/Ano da data de emissão informada na tag " + dadosNFe.versao == "2.00" ? "<dEmi>: " : "<dhEmi>: " + dadosNFe.dEmi.Month.ToString("00") + "/" + dadosNFe.dEmi.Year.ToString("0000").Substring(2, 2) + "\r\n" +
                        "Mês/Ano informados na chave da NF-e: " + dadosNFe.chavenfe.Substring(7 + nPos, 2) + "/" + dadosNFe.chavenfe.Substring(5 + nPos, 2) + "\r\n\r\n";
                    booValido = false;
                }

                #endregion Tag <dEmi>

                if (!booValido)
                {
                    throw new Exception(cTextoErro);
                }
            }

            #endregion Verificar se os valores das tag´s que compõe a chave da nfe estão batendo com as informadas na chave
        }

        private bool ValidarInformacaoContingencia(DadosNFeClass dadosNFe)
        {
            if (string.IsNullOrEmpty(dadosNFe.dhCont) || string.IsNullOrEmpty(dadosNFe.xJust))
            {
                return false;
            }

            return true;
        }

        #endregion ValidacoesGerais()

        #region LoteNfe()

        /// <summary>
        /// Auxiliar na geração do arquivo XML de Lote de notas fiscais
        /// </summary>
        /// <param name="arquivo">Nome do arquivo XML da NFe</param>
        /// <param name="conteudoXML">Conteúdo do XML</param>
        /// <param name="versaoXml">Versão do XML</param>
        protected XmlDocument LoteNfe(XmlDocument conteudoXML, string arquivo, string versaoXml)
        {
            List<ArquivoXMLDFe> arquivos = new List<ArquivoXMLDFe>
            {
                new ArquivoXMLDFe() { NomeArquivoXML = arquivo, ConteudoXML = conteudoXML }
            };

            return oGerarXML.LoteNfe(Servico, arquivos, versaoXml);
        }

        #endregion LoteNfe()

        #region LoteNfe()

        /// <summary>
        /// Auxliar na geração do arquivo XML de Lote de notas fiscais
        /// </summary>
        /// <param name="arquivosNfe">Lista de arquivos de NFe para montagem do lote de várias NFe</param>
        /// <param name="versaoXml">Versao do Xml de lote</param>
        /// <date>24/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        protected XmlDocument LoteNfe(List<ArquivoXMLDFe> arquivosNfe, string versaoXml)
        {
            return oGerarXML.LoteNfe(Servico, arquivosNfe, versaoXml);
        }

        #endregion LoteNfe()

        #region ProcessaNFeDenegada

        protected void ProcessaNFeDenegada(int emp, LerXML oLerXml, string strArquivoNFe, XmlDocument conteudoXML, string protNFe)
        {
            string strProtNfe;

            if (!File.Exists(strArquivoNFe))
            {
                throw new Exception("Arquivo \"" + strArquivoNFe + "\" não encontrado");
            }

            if (conteudoXML == null)
            {
                conteudoXML = new XmlDocument();
                conteudoXML.Load(strArquivoNFe);
            }
            oLerXml.Nfe(conteudoXML);

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
                {
                    addNFeDen = false;
                }
            }
            if (addNFeDen)
            {
                ///
                /// monta o XML de denegacao
                strProtNfe = protNFe;

                ///
                /// gera o arquivo de denegacao na pasta EmProcessamento
                strNomeArqDenegadaNFe = oGerarXML.XmlDistNFe(strArquivoNFe, strProtNfe, Propriedade.ExtRetorno.Den, oLerXml.oDadosNfe.versao);
                if (string.IsNullOrEmpty(strNomeArqDenegadaNFe))
                {
                    throw new Exception("Erro de criação do arquivo de distribuição da nota denegada");
                }

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
                        {
                            System.IO.Directory.CreateDirectory(nomePastaBackup);
                        }

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
                {
                    // Como já existe na pasta Enviados\Denegados, só vou excluir da pasta EmProcessamento. Wandrey 22/12/2015
                    Functions.DeletarArquivo(strArquivoNFe);
                }
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

        #endregion ProcessaNFeDenegada

        #region XmlPedRec()

        /// <summary>
        /// Gerar o XML de consulta do recibo do lote da nfe
        /// </summary>
        /// <param name="empresa">Código da empresa</param>
        /// <param name="nRec">Número do recibo a ser inserido no XML de consulta</param>
        /// <param name="versao">Versao do Schema XML</param>
        /// <param name="mod">Modelo do documento fiscal</param>
        public XmlDocument XmlPedRec(int empresa, string nRec, string versao, string mod)
        {
            GerarXML gerarXML = new GerarXML(empresa);
            return gerarXML.XmlPedRec(mod, nRec, versao);
        }

        #endregion XmlPedRec()

        #region Ultiliza WS compilado

        protected bool IsUtilizaCompilacaoWs(PadroesNFSe padrao, Servicos servico = Servicos.Nulo, int cMunicipio = 0)
        {
            bool retorno = true;

            switch (padrao)
            {
                case PadroesNFSe.PRONIN:
                    if (cMunicipio == 4109401 ||
                        cMunicipio == 3131703 ||
                        cMunicipio == 4303004 ||
                        cMunicipio == 4322509 ||
                        cMunicipio == 3556602 ||
                        cMunicipio == 3512803 ||
                        cMunicipio == 4323002 ||
                        cMunicipio == 3505807 ||
                        cMunicipio == 3530300 ||
                        cMunicipio == 4308904 ||
                        cMunicipio == 4118501 ||
                        cMunicipio == 3554300 ||
                        cMunicipio == 3542404 ||
                        cMunicipio == 5005707 ||
                        cMunicipio == 4314423 ||
                        cMunicipio == 3511102 ||
                        cMunicipio == 3535804 ||
                        cMunicipio == 4306932 ||
                        cMunicipio == 4322400 ||
                        cMunicipio == 4302808 ||
                        cMunicipio == 3501301 ||
                        cMunicipio == 4300109 ||
                        cMunicipio == 4124053 ||
                        cMunicipio == 4101408 ||
                        cMunicipio == 3550407)
                    {
                        retorno = false;
                    }

                    break;

                case PadroesNFSe.EGOVERNEISS:
                case PadroesNFSe.COPLAN:
                case PadroesNFSe.BETHA202:
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
                case PadroesNFSe.TINUS:
                case PadroesNFSe.CONAM:
                case PadroesNFSe.SIMPLISS:
                case PadroesNFSe.RLZ_INFORMATICA:
                case PadroesNFSe.PAULISTANA:
                case PadroesNFSe.NOTAINTELIGENTE:
                case PadroesNFSe.NA_INFORMATICA:
                case PadroesNFSe.BSITBR:
                case PadroesNFSe.METROPOLIS:
                case PadroesNFSe.BAURU_SP:
                case PadroesNFSe.SOFTPLAN:
                case PadroesNFSe.JOINVILLE_SC:
                case PadroesNFSe.AVMB_ASTEN:
                case PadroesNFSe.ADM_SISTEMAS:
                case PadroesNFSe.SIMPLE:
                case PadroesNFSe.WEBFISCO_TECNOLOGIA:
                case PadroesNFSe.AGILI:
                case PadroesNFSe.GEISWEB:
                case PadroesNFSe.CENTI:
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

        #endregion Ultiliza WS compilado

        /// <summary>
        /// Se invoca o serviço
        /// </summary>
        /// <param name="padrao"></param>
        /// <param name="servico"></param>
        /// <returns></returns>
        protected bool IsInvocar(PadroesNFSe padrao, Servicos servico = Servicos.Nulo, int cMunicipio = 0)
        {
            bool invocar = IsUtilizaCompilacaoWs(padrao, servico, cMunicipio);

            switch (padrao)
            {
                case PadroesNFSe.NOTAINTELIGENTE:
                case PadroesNFSe.PAULISTANA:
                case PadroesNFSe.NA_INFORMATICA:
                case PadroesNFSe.BSITBR:
                case PadroesNFSe.JOINVILLE_SC:
                case PadroesNFSe.AVMB_ASTEN:
                case PadroesNFSe.ADM_SISTEMAS:
                case PadroesNFSe.IIBRASIL:
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
            oGerarXML.XmlRetorno(pFinalArqEnvio, pFinalArqRetorno, vStrXmlRetorno);
        }

        #endregion XmlRetorno()

        #region GetTipoServicoSincrono

        protected Servicos GetTipoServicoSincrono(Servicos servico, string file, PadroesNFSe padrao)
        {
            Servicos result = servico;
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            switch (padrao)
            {
                case PadroesNFSe.AVMB_ASTEN:
                case PadroesNFSe.WEBISS_202:
                case PadroesNFSe.EMBRAS:
                case PadroesNFSe.DESENVOLVECIDADE:
                case PadroesNFSe.MODERNIZACAO_PUBLICA:
                case PadroesNFSe.E_RECEITA:
                case PadroesNFSe.ADM_SISTEMAS:
                case PadroesNFSe.PUBLIC_SOFT:
                case PadroesNFSe.TIPLAN_203:
                case PadroesNFSe.MEGASOFT:
                case PadroesNFSe.INDAIATUBA_SP:
                case PadroesNFSe.PORTALFACIL_ACTCON_202:
                case PadroesNFSe.PORTALFACIL_ACTCON:
                case PadroesNFSe.SIGCORP_SIGISS_203:
                case PadroesNFSe.SMARAPD_204:
                case PadroesNFSe.IIBRASIL:
                case PadroesNFSe.SYSMAR:
                case PadroesNFSe.PUBLICA:
                case PadroesNFSe.RLZ_INFORMATICA_02:
                    if (servico == Servicos.NFSeRecepcionarLoteRps)
                    {
                        switch (doc.DocumentElement.Name)
                        {
                            case "EnviarLoteRpsEnvio":
                                result = Servicos.NFSeRecepcionarLoteRps;
                                break;

                            case "EnviarLoteRpsSincronoEnvio":
                                result = Servicos.NFSeRecepcionarLoteRpsSincrono;
                                break;

                            case "GerarNfseEnvio":
                                result = Servicos.NFSeGerarNfse;
                                break;
                        }
                    }
                    break;

                case PadroesNFSe.FINTEL:
                    if (servico == Servicos.NFSeRecepcionarLoteRps || servico == Servicos.RecepcionarLoteCfse)
                    {
                        switch (doc.DocumentElement.Name)
                        {
                            case "EnviarLoteRpsEnvio":
                                result = Servicos.NFSeRecepcionarLoteRps;
                                break;

                            case "EnviarLoteRpsSincronoEnvio":
                                result = Servicos.NFSeRecepcionarLoteRpsSincrono;
                                break;

                            case "GerarNfseEnvio":
                                result = Servicos.NFSeGerarNfse;
                                break;

                            case "EnviarLoteCupomEnvio":
                                result = Servicos.RecepcionarLoteCfse;
                                break;

                            case "EnviarLoteCupomSincronoEnvio":
                                result = Servicos.RecepcionarLoteCfseSincrono;
                                break;
                        }
                    }
                    break;

                case PadroesNFSe.CARIOCA:
                case PadroesNFSe.PRODATA:
                    if (servico == Servicos.NFSeRecepcionarLoteRps)
                    {
                        switch (doc.DocumentElement.Name)
                        {
                            case "EnviarLoteRpsEnvio":
                                result = Servicos.NFSeRecepcionarLoteRps;
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

                case PadroesNFSe.SUPERNOVA:
                case PadroesNFSe.MARINGA_PR:
                case PadroesNFSe.SISPMJP:
                    if (servico == Servicos.NFSeRecepcionarLoteRps)
                    {
                        switch (doc.DocumentElement.Name)
                        {
                            case "EnviarLoteRpsSincronoEnvio":
                                result = Servicos.NFSeRecepcionarLoteRpsSincrono;
                                break;

                            case "EnviarLoteRpsEnvio":
                                result = Servicos.NFSeRecepcionarLoteRps;
                                break;

                            default:
                                break;
                        }
                    }
                    break;

                case PadroesNFSe.BSITBR:
                    if (servico == Servicos.NFSeRecepcionarLoteRps)
                    {
                        switch (doc.DocumentElement.Name)
                        {
                            case "p:EnviarLoteRpsSincronoEnvio":
                                result = Servicos.NFSeRecepcionarLoteRpsSincrono;
                                break;

                            case "p:GerarNfseEnvio":
                                result = Servicos.NFSeGerarNfse;
                                break;

                            default:
                                break;
                        }
                    }
                    break;

                case PadroesNFSe.CAMACARI_BA:
                    if (servico == Servicos.NFSeRecepcionarLoteRps)
                    {
                        switch (doc.DocumentElement.Name)
                        {
                            case "EnviarLoteRpsSincronoEnvio":
                                result = Servicos.NFSeRecepcionarLoteRpsSincrono;
                                break;

                            case "EnviarLoteRpsEnvio":
                                result = Servicos.NFSeRecepcionarLoteRps;
                                break;

                            default:
                                break;
                        }
                    }
                    break;

                case PadroesNFSe.COPLAN:
                case PadroesNFSe.BETHA202:
                case PadroesNFSe.SH3:
                    if (servico == Servicos.NFSeRecepcionarLoteRps)
                    {
                        switch (doc.DocumentElement.Name)
                        {
                            case "EnviarLoteRpsSincronoEnvio":
                                result = Servicos.NFSeRecepcionarLoteRpsSincrono;
                                break;

                            case "EnviarLoteRpsEnvio":
                                result = Servicos.NFSeRecepcionarLoteRps;
                                break;

                            default:
                                break;
                        }
                    }
                    break;
            }

            return result;
        }

        #endregion GetTipoServicoSincrono

        #region ValidaEvento

        protected void ValidaEvento(int emp, DadosenvEvento dadosEnvEvento)
        {
            string cErro = "";
            string currentEvento = dadosEnvEvento.eventos[0].tpEvento;
            string ctpEmis = dadosEnvEvento.eventos[0].chNFe.Substring(34, 1);
            foreach (Evento item in dadosEnvEvento.eventos)
            {
                if (!currentEvento.Equals(item.tpEvento))
                {
                    throw new Exception(string.Format("Não é possivel mesclar tipos de eventos dentro de um mesmo xml/txt de eventos. O tipo de evento neste xml/txt é {0}", currentEvento));
                }

                switch (NFe.Components.EnumHelper.StringToEnum<NFe.ConvertTxt.tpEventos>(currentEvento))
                {
                    case ConvertTxt.tpEventos.tpEvCancelamentoNFe:
                    case ConvertTxt.tpEventos.tpEvCancelamentoSubstituicaoNFCe:
                        if (!ctpEmis.Equals(item.chNFe.Substring(34, 1)))
                        {
                            cErro += "Não é possivel mesclar chaves com tipo de emissão dentro de um mesmo xml/txt de eventos.\r\n";
                        }

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
            {
                throw new Exception(cErro);
            }
        }

        #endregion ValidaEvento

        #region PedSta

        protected virtual void PedSta(int emp, DadosPedSta dadosPedSta)
        {
            dadosPedSta.tpAmb = 0;
            dadosPedSta.cUF = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
            dadosPedSta.tpEmis = Empresas.Configuracoes[emp].tpEmis;
            dadosPedSta.versao = "";

            ///
            /// danasa 12-9-2009
            ///
            if (Path.GetExtension(NomeArquivoXML).ToLower() == ".txt")
            {
                // tpEmis|1						<<< opcional >>>
                // tpAmb|1
                // cUF|35
                // versao|3.10
                List<string> cLinhas = Functions.LerArquivo(NomeArquivoXML);
                Functions.PopulateClasse(dadosPedSta, cLinhas);
            }
            else
            {
                bool isCteMDFe = false;

                XmlNodeList consStatServList = ConteudoXML.GetElementsByTagName("consStatServCte");
                if (consStatServList.Count == 0)
                {
                    consStatServList = ConteudoXML.GetElementsByTagName("consStatServMDFe");
                    if (consStatServList.Count == 0)
                    {
                        consStatServList = ConteudoXML.GetElementsByTagName("consStatServ");
                    }
                    else
                    {
                        isCteMDFe = true;
                    }
                }
                else
                {
                    isCteMDFe = true;
                }

                foreach (XmlNode consStatServNode in consStatServList)
                {
                    XmlElement consStatServElemento = (XmlElement)consStatServNode;

                    dadosPedSta.tpAmb = Convert.ToInt32("0" + consStatServElemento.GetElementsByTagName(TpcnResources.tpAmb.ToString())[0].InnerText);
                    dadosPedSta.versao = consStatServElemento.Attributes[TpcnResources.versao.ToString()].InnerText;

                    if (consStatServElemento.GetElementsByTagName(TpcnResources.cUF.ToString()).Count != 0)
                    {
                        dadosPedSta.cUF = Convert.ToInt32("0" + consStatServElemento.GetElementsByTagName(TpcnResources.cUF.ToString())[0].InnerText);

                        if (isCteMDFe)
                        {
                            // para que o validador não rejeite, excluo a tag <cUF>
                            ConteudoXML.DocumentElement.RemoveChild(consStatServElemento.GetElementsByTagName(TpcnResources.cUF.ToString())[0]);
                        }
                    }

                    if (consStatServElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString()).Count != 0)
                    {
                        dadosPedSta.tpEmis = Convert.ToInt16(consStatServElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString())[0].InnerText);

                        // para que o validador não rejeite, excluo a tag <tpEmis>
                        ConteudoXML.DocumentElement.RemoveChild(consStatServElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString())[0]);
                    }

                    if (consStatServElemento.GetElementsByTagName(TpcnResources.mod.ToString()).Count != 0)
                    {
                        dadosPedSta.mod = consStatServElemento.GetElementsByTagName(TpcnResources.mod.ToString())[0].InnerText;

                        /// para que o validador não rejeite, excluo a tag <mod>
                        ConteudoXML.DocumentElement.RemoveChild(consStatServElemento.GetElementsByTagName(TpcnResources.mod.ToString())[0]);
                    }
                }
            }
        }

        #endregion PedSta

        #region PedSit

        protected virtual void PedSit(int emp, DadosPedSit dadosPedSit)
        {
            dadosPedSit.tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            dadosPedSit.chNFe = string.Empty;

            XmlNodeList consSitNFeList = ConteudoXML.GetElementsByTagName("consSitCTe");
            if (consSitNFeList.Count == 0)
            {
                consSitNFeList = ConteudoXML.GetElementsByTagName("consSitMDFe");
            }
            foreach (XmlNode consSitNFeNode in consSitNFeList)
            {
                XmlElement consSitNFeElemento = (XmlElement)consSitNFeNode;

                dadosPedSit.versao = consSitNFeElemento.Attributes[TpcnResources.versao.ToString()].Value;
                dadosPedSit.tpAmb = Convert.ToInt32("0" + consSitNFeElemento.GetElementsByTagName(TpcnResources.tpAmb.ToString())[0].InnerText);
                dadosPedSit.chNFe = Functions.LerTag(consSitNFeElemento, TpcnResources.chCTe.ToString(), "") +
                                    Functions.LerTag(consSitNFeElemento, TpcnResources.chMDFe.ToString(), "");

                if (consSitNFeElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString()).Count != 0)
                {
                    dadosPedSit.tpEmis = Convert.ToInt16(consSitNFeElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString())[0].InnerText);

                    /// para que o validador não rejeite, excluo a tag <tpEmis>
                    ConteudoXML.DocumentElement.RemoveChild(consSitNFeElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString())[0]);
                }
            }
        }

        #endregion PedSit

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

            XmlNodeList envEventoList = ConteudoXML.GetElementsByTagName("infEvento");

            foreach (XmlNode envEventoNode in envEventoList)
            {
                XmlElement envEventoElemento = (XmlElement)envEventoNode;

                dadosEnvEvento.eventos.Add(new Evento()
                {
                    tpEvento = Functions.LerTag(envEventoElemento, TpcnResources.tpEvento.ToString(), false),
                    tpAmb = Convert.ToInt32("0" + Functions.LerTag(envEventoElemento, TpcnResources.tpAmb.ToString(), false)),
                    cOrgao = Convert.ToInt32("0" + Functions.LerTag(envEventoElemento, TpcnResources.cOrgao.ToString(), false)),
                    nSeqEvento = Convert.ToInt32("0" + Functions.LerTag(envEventoElemento, TpcnResources.nSeqEvento.ToString(), false))
                });
                dadosEnvEvento.eventos[dadosEnvEvento.eventos.Count - 1].chNFe =
                    Functions.LerTag(envEventoElemento, TpcnResources.chNFe.ToString(), "") +
                    Functions.LerTag(envEventoElemento, TpcnResources.chMDFe.ToString(), "") +
                    Functions.LerTag(envEventoElemento, TpcnResources.chCTe.ToString(), "");

                dadosEnvEvento.eventos[dadosEnvEvento.eventos.Count - 1].tpEmis =
                    Convert.ToInt16(dadosEnvEvento.eventos[dadosEnvEvento.eventos.Count - 1].chNFe.Substring(34, 1));

                if (envEventoElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString()).Count != 0)
                {
                    XmlNode node = envEventoElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString())[0];

                    dadosEnvEvento.eventos[dadosEnvEvento.eventos.Count - 1].tpEmis = Convert.ToInt16("0" + node.InnerText);

                    /// para que o validador não rejeite, excluo a tag <tpEmis>
                    envEventoNode.RemoveChild(node);
                    doSave = true;
                }
            }

            /// salvo o arquivo modificado
            if (doSave)
            {
                ConteudoXML.Save(NomeArquivoXML);
            }
        }

        #endregion EnvEvento

        #region XmlLMC()

        /// <summary>
        /// Efetua a leitura do XML de LMC e grava os dados no objeto "dadosLMC"
        /// </summary>
        /// <param name="emp">Empresa</param>
        /// <param name="dadosLMC">Objeto dados LMC para receber os valores</param>
        protected virtual void XmlLMC(int emp, DadosLMC dadosLMC)
        {
            dadosLMC.tpAmb = 0;
            dadosLMC.cUF = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
            dadosLMC.Id =
                dadosLMC.versao = string.Empty;

            XmlElement infLivroCombustivel = (XmlElement)ConteudoXML.GetElementsByTagName("infLivroCombustivel")[0];

            dadosLMC.tpAmb = Convert.ToInt32("0" + infLivroCombustivel.GetElementsByTagName(TpcnResources.tpAmb.ToString())[0].InnerText);
            dadosLMC.versao = infLivroCombustivel.Attributes[TpcnResources.versao.ToString()].InnerText;
            dadosLMC.Id = infLivroCombustivel.Attributes[TpcnResources.Id.ToString()].InnerText;

            XmlElement movimento = (XmlElement)infLivroCombustivel.GetElementsByTagName("movimento")[0];
            dadosLMC.dEmissao = Convert.ToDateTime(movimento.Attributes[TpcnResources.dEmissao.ToString()].InnerText);
        }

        #endregion XmlLMC()
    }
}