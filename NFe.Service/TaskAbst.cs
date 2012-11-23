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
        protected GerarXML oGerarXML = new GerarXML(new FindEmpresaThread(Thread.CurrentThread).Index);
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


        #region Métodos para definição dos nomes das classes e métodos da NFe, CTe e NFSe

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
                case TipoAplicativo.Cte:
                    retorna = NomeClasseWSCTe(servico, cUF);
                    break;
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
                case Servicos.CancelarNFe:
                    retorna = "NfeCancelamento2";
                    break;
                case Servicos.InutilizarNumerosNFe:
                    retorna = "NfeInutilizacao2";
                    break;
                case Servicos.PedidoConsultaSituacaoNFe:
                    retorna = "NfeConsulta2";
                    break;
                case Servicos.PedidoConsultaStatusServicoNFe:
                    retorna = "NfeStatusServico2";
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
                default:
                    break;
            }

            return retorna;
        }
        #endregion

        #region NomeClasseWSCTe()
        /// <summary>
        /// Retorna o nome da classe do serviço passado por parâmetro do WebService do SEFAZ - CTe
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cUF">Código da UF</param>
        /// <returns>Nome da classe</returns>
        private string NomeClasseWSCTe(Servicos servico, int cUF)
        {
            string retorna = string.Empty;

            switch (servico)
            {
                case Servicos.CancelarNFe:
                    retorna = "CteCancelamento";
                    break;
                case Servicos.InutilizarNumerosNFe:
                    retorna = "CteInutilizacao";
                    break;
                case Servicos.PedidoConsultaSituacaoNFe:
                    retorna = "CteConsulta";
                    break;
                case Servicos.PedidoConsultaStatusServicoNFe:
                    retorna = "CteStatusServico";
                    break;
                case Servicos.PedidoSituacaoLoteNFe:
                    retorna = "CteRetRecepcao";
                    break;
                case Servicos.ConsultaCadastroContribuinte:
                    if (cUF == 50)
                        retorna = "CadConsultaCadastro";
                    else
                        retorna = "CadConsultaCadastro2";
                    break;
                case Servicos.EnviarLoteNfe:
                    retorna = "CteRecepcao";
                    break;
                default:
                    break;
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
            string retorna = string.Empty;

            switch (Propriedade.TipoAplicativo)
            {
                case TipoAplicativo.Cte:
                    retorna = NomeClasseCabecWSCTe(cUF, servico);
                    break;
                case TipoAplicativo.Nfe:
                    retorna = NomeClasseCabecWSNFe();
                    break;
            }

            return retorna;
        }
        #endregion

        #region NomeClasseCabecWSNFe()
        /// <summary>
        /// Retorna o nome da classe de cabecalho do serviço de NFe
        /// </summary>
        /// <returns>nome da classe de cabecalho do serviço de NFe</returns>
        private string NomeClasseCabecWSNFe()
        {
            return "nfeCabecMsg";
        }
        #endregion

        #region NomeClasseCabecWSNFe()
        /// <summary>
        /// Retorna o nome da classe de cabecalho do serviço de CTe
        /// </summary>
        /// <returns>nome da classe de cabecalho do serviço de CTe</returns>
        private string NomeClasseCabecWSCTe(int cUF, Servicos servico)
        {
            string nomeClasse = "cteCabecMsg";
            switch (cUF)
            {
                case 50:
                    return nomeClasse = "CTeCabecMsg";

            }
            if (servico == Servicos.ConsultaCadastroContribuinte)
                nomeClasse = NomeClasseCabecWSNFe();



            return nomeClasse;
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
                case TipoAplicativo.Cte:
                    retorna = NomeMetodoWSCTe(servico, cUF);
                    break;
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
                case Servicos.CancelarNFe:
                    retorna = "nfeCancelamentoNF2";
                    break;
                case Servicos.InutilizarNumerosNFe:
                    retorna = "nfeInutilizacaoNF2";
                    break;
                case Servicos.PedidoConsultaSituacaoNFe:
                    retorna = "nfeConsultaNF2";
                    break;
                case Servicos.PedidoConsultaStatusServicoNFe:
                    retorna = "nfeStatusServicoNF2";
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
                case Servicos.EnviarLoteNfe:
                    retorna = "nfeRecepcaoLote2";
                    break;
                default:
                    break;
            }

            return retorna;
        }
        #endregion

        #region NomeMetodoWSCTe()
        /// <summary>
        /// Retorna o nome do método da classe de serviço - CTe
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cUF">Código da UF</param>
        /// <returns>nome do método da classe de serviço</returns>
        private string NomeMetodoWSCTe(Servicos servico, int cUF)
        {
            string retorna = string.Empty;

            switch (servico)
            {
                case Servicos.CancelarNFe:
                    retorna = "cteCancelamentoCT";
                    break;
                case Servicos.InutilizarNumerosNFe:
                    retorna = "cteInutilizacaoCT";
                    break;
                case Servicos.PedidoConsultaSituacaoNFe:
                    retorna = "cteConsultaCT";
                    break;
                case Servicos.PedidoConsultaStatusServicoNFe:
                    retorna = "cteStatusServicoCT";
                    break;
                case Servicos.PedidoSituacaoLoteNFe:
                    retorna = "cteRetRecepcao";
                    break;
                case Servicos.ConsultaCadastroContribuinte:
                    switch (cUF)
                    {
                        case 52:
                            retorna = "cadConsultaCadastro2";
                            break;
                        case 50:
                            retorna = "cadConsultaCadastro";
                            break;
                        default:
                            retorna = "consultaCadastro2";
                            break;
                    }
                    break;
                case Servicos.EnviarLoteNfe:
                    retorna = "cteRecepcaoLote";
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
                            retorna = "";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "";
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
            }

            return retorna;
        }
        #endregion

        #endregion

        #region GeraStrProtNFe
        /// <summary>
        /// GeraStrProtNFe
        /// </summary>
        /// <param name="infConsSitElemento"></param>
        /// <returns></returns>
        protected string GeraStrProtNFe(System.Xml.XmlElement infConsSitElemento)//danasa 11-4-2012
        {
            string atributoId = string.Empty;
            if (infConsSitElemento.GetAttribute("Id").Length != 0)
            {
                atributoId = " Id=\"" + infConsSitElemento.GetAttribute("Id") + "\"";
            }

            string strProtNfe = "<protNFe versao=\"" + ConfiguracaoApp.VersaoXMLNFe + "\">" +
                "<infProt" + atributoId + ">" +
                "<tpAmb>" + Functions.LerTag(infConsSitElemento, "tpAmb", false) + "</tpAmb>" +
                "<verAplic>" + Functions.LerTag(infConsSitElemento, "verAplic", false) + "</verAplic>" +
                "<chNFe>" + Functions.LerTag(infConsSitElemento, "chNFe", false) + "</chNFe>" +
                "<dhRecbto>" + Functions.LerTag(infConsSitElemento, "dhRecbto", false) + "</dhRecbto>" +
                "<nProt>" + Functions.LerTag(infConsSitElemento, "nProt", false) + "</nProt>" +
                "<digVal>" + Functions.LerTag(infConsSitElemento, "digVal", false) + "</digVal>" +
                "<cStat>" + Functions.LerTag(infConsSitElemento, "cStat", false) + "</cStat>" +
                "<xMotivo>" + Functions.LerTag(infConsSitElemento, "xMotivo", false) + "</xMotivo>" +
                "</infProt>" +
                "</protNFe>";
            return strProtNfe;
        }
        #endregion

        #region LerXMLNFe()
        /// <summary>
        /// Le o conteúdo do XML da NFe
        /// </summary>
        /// <param name="Arquivo">Arquivo XML da NFe</param>
        /// <returns>Retorna o conteúdo do XML da NFe</returns>
        public DadosNFeClass LerXMLNFe(string Arquivo)
        {
            LerXML oLerXML = new LerXML();

            try
            {
                oLerXML.Nfe(Arquivo);
            }
            catch (XmlException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
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
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            Boolean tudoOK = false;
            Boolean bAssinado = this.Assinado(NomeArquivoXML);
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
                try
                {
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
                    if (bValidacaoGeral && !bAssinado)
                    {
                        AssinaturaDigital assDig = new AssinaturaDigital();

                        assDig.Assinar(NomeArquivoXML, emp, Convert.ToInt32(oDadosNFe.cUF));

                        bAssinado = true;
                    }

                    // Validar o Arquivo XML da NFe com os Schemas se estiver assinado
                    if (bValidacaoGeral && bAssinado)
                    {
                        //TODO UNINFSE: Analisar namespace do metodo abaixo onde passei string.Empty. Vai ficar assim mesmo?
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
                        try
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
                                oFluxoNfe.InserirNfeFluxo(ChaveNfe, Functions.ExtrairNomeArq(arqDestino, ".xml") + ".xml");

                                throw new IOException("Esta nota fiscal já está na pasta de Notas Fiscais assinadas e em processo de envio, desta forma não é possível enviar a mesma novamente.\r\n" +
                                    NomeArquivoXML);
                            }
                        }
                        catch (IOException ex)
                        {
                            throw (ex);
                        }
                        catch (Exception ex)
                        {
                            throw (ex);
                        }
                    }

                    if (tudoOK)
                    {
                        try
                        {
                            oFluxoNfe.InserirNfeFluxo(ChaveNfe, Functions.ExtrairNomeArq(arqDestino, ".xml") + ".xml");
                        }
                        catch (Exception ex)
                        {
                            throw (ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.Nfe, Propriedade.ExtRetorno.Nfe_ERR, ex);

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

                throw (ex);
            }
        }
        #endregion

        #region Assinado()
        /// <summary>
        /// Verifica se o XML já está assinado digitalmente ou não
        /// </summary>
        /// <param name="cArquivoXML">Arquivo a ser verificado</param>
        /// <returns>true = Arquivo XML já assinado</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>03/04/2009</date>
        protected Boolean Assinado(string cArquivoXML)
        {
            Boolean bAssinado = false;

            //TODO: Tem que criar ainda o código que verifica se já está assinado ou não

            return bAssinado;
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
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            bool booValido = false;
            string cTextoErro = "";

            try
            {
                if (Propriedade.TipoAplicativo == TipoAplicativo.Cte)
                {
                    //Verificar o tipo de emissão se bate com o configurado, se não bater vai retornar um erro para o ERP
                    //Wandrey 15/08/2012
                    if ((Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teNormal && (oDadosNFe.tpEmis == "1" || oDadosNFe.tpEmis == "5")) ||
                        (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCRS && (oDadosNFe.tpEmis == "7")) ||
                        (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCSP && (oDadosNFe.tpEmis == "8")))
                    {
                        booValido = true;
                    }
                    else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teFSDA && (oDadosNFe.tpEmis == "5"))
                    {
                        booValido = false; //Retorno somente falso mas sem exception para não fazer nada. Wandrey 19/06/2009
                    }
                    else
                    {
                        booValido = false;

                        if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teNormal && oDadosNFe.tpEmis == "7")
                        {
                            cTextoErro = "O UniCTe está configurado para enviar a Nota Fiscal ao Ambiente da SEFAZ " +
                                "(Secretaria Estadual da Fazenda) e o XML está configurado para enviar " +
                                "para o SVC-RS.\r\n\r\n";

                        }
                        if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teNormal && oDadosNFe.tpEmis == "8")
                        {
                            cTextoErro = "O UniCTe está configurado para enviar a Nota Fiscal ao Ambiente da SEFAZ " +
                                "(Secretaria Estadual da Fazenda) e o XML está configurado para enviar " +
                                "para o SVC-SP.\r\n\r\n";

                        }
                        if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCRS && oDadosNFe.tpEmis == "8")
                        {
                            cTextoErro = "O UniCTe está configurado para enviar a Nota Fiscal ao Ambiente da SVC-RS " +
                                "e o XML está configurado para enviar para o SVC-SP.\r\n\r\n";

                        }
                        if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCSP && oDadosNFe.tpEmis == "7")
                        {
                            cTextoErro = "O UniCTe está configurado para enviar a Nota Fiscal ao Ambiente da SVC-SP " +
                                "e o XML está configurado para enviar para o SVC-RS.\r\n\r\n";

                        }
                        else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCSP && (oDadosNFe.tpEmis == "1" || oDadosNFe.tpEmis == "5"))
                        {
                            cTextoErro = "O UniCTe está configurado para enviar a Nota Fiscal ao SVCSP " +
                                "e o XML está configurado para enviar para o Ambiente da SEFAZ (Secretaria Estadual da Fazenda)\r\n\r\n";
                        }
                        else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSVCRS && (oDadosNFe.tpEmis == "1" || oDadosNFe.tpEmis == "5"))
                        {
                            cTextoErro = "O UniCTe está configurado para enviar a Nota Fiscal ao SVCRS " +
                                "e o XML está configurado para enviar para o Ambiente da SEFAZ (Secretaria Estadual da Fazenda)\r\n\r\n";
                        }

                        cTextoErro += "O XML não será enviado e será movido para a pasta de XML com erro para análise.";

                        throw new Exception(cTextoErro);
                    }
                }
                else
                {
                    //Verificar o tipo de emissão se bate com o configurado, se não bater vai retornar um erro para o ERP
                    //danasa 8-2009
                    if ((Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teNormal && (oDadosNFe.tpEmis == "1" || oDadosNFe.tpEmis == "2" || oDadosNFe.tpEmis == "5" || oDadosNFe.tpEmis == "4")) ||
                        (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSCAN && (oDadosNFe.tpEmis == "3")))
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
                        else if (Empresa.Configuracoes[emp].tpEmis == Propriedade.TipoEmissao.teSCAN && (oDadosNFe.tpEmis == "1" || oDadosNFe.tpEmis == "2" || oDadosNFe.tpEmis == "5"))
                        {
                            cTextoErro = "O UniNFe está configurado para enviar a Nota Fiscal ao SVCAN " +
                                "e o XML está configurado para enviar para o Ambiente da SEFAZ (Secretaria Estadual da Fazenda)\r\n\r\n";
                        }

                        cTextoErro += "O XML não será enviado e será movido para a pasta de XML com erro para análise.";

                        throw new Exception(cTextoErro);
                    }
                }

                #region Verificar se os valores das tag´s que compõe a chave da nfe estão batendo com as informadas na chave
                //Verificar se os valores das tag´s que compõe a chave da nfe estão batendo com as informadas na chave
                if (booValido)
                {
                    cTextoErro = string.Empty;

                    #region Tag <cUF>
                    if (oDadosNFe.cUF != oDadosNFe.chavenfe.Substring(3, 2))
                    {
                        cTextoErro += "O código da UF informado na tag <cUF> está diferente do informado na chave da NF-e.\r\n" +
                            "Código da UF informado na tag <cUF>: " + oDadosNFe.cUF + "\r\n" +
                            "Código da UF informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(3, 2) + "\r\n\r\n";
                        booValido = false;
                    }
                    #endregion

                    #region Tag <tpEmis>
                    if (oDadosNFe.tpEmis != oDadosNFe.chavenfe.Substring(37, 1))
                    {
                        cTextoErro += "O código numérico informado na tag <tpEmis> está diferente do informado na chave da NF-e.\r\n" +
                            "Código numérico informado na tag <tpEmis>: " + oDadosNFe.tpEmis + "\r\n" +
                            "Código numérico informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(37, 1) + "\r\n\r\n";
                        booValido = false;
                    }
                    #endregion

                    #region Tag <cNF>
                    if (oDadosNFe.cNF != oDadosNFe.chavenfe.Substring(38, 8))
                    {
                        cTextoErro += "O código numérico informado na tag <cNF> está diferente do informado na chave da NF-e.\r\n" +
                            "Código numérico informado na tag <cNF>: " + oDadosNFe.cNF + "\r\n" +
                            "Código numérico informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(38, 8) + "\r\n\r\n";
                        booValido = false;
                    }
                    #endregion

                    #region Tag <mod>
                    if (oDadosNFe.mod != oDadosNFe.chavenfe.Substring(23, 2))
                    {
                        cTextoErro += "O modelo informado na tag <mod> está diferente do informado na chave da NF-e.\r\n" +
                            "Modelo informado na tag <mod>: " + oDadosNFe.mod + "\r\n" +
                            "Modelo informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(23, 2) + "\r\n\r\n";
                        booValido = false;
                    }
                    #endregion

                    #region Tag <nNF>
                    if (Convert.ToInt32(oDadosNFe.nNF) != Convert.ToInt32(oDadosNFe.chavenfe.Substring(28, 9)))
                    {
                        cTextoErro += "O número da NF-e informado na tag <nNF> está diferente do informado na chave da NF-e.\r\n" +
                            "Número da NFe informado na tag <nNF>: " + Convert.ToInt32(oDadosNFe.nNF).ToString() + "\r\n" +
                            "Número da NFe informado na chave da NF-e: " + Convert.ToInt32(oDadosNFe.chavenfe.Substring(28, 9)).ToString() + "\r\n\r\n";
                        booValido = false;
                    }
                    #endregion

                    #region Tag <cDV>
                    if (oDadosNFe.cDV != oDadosNFe.chavenfe.Substring(46, 1))
                    {
                        cTextoErro += "O número do dígito verificador informado na tag <cDV> está diferente do informado na chave da NF-e.\r\n" +
                            "Número do dígito verificador informado na tag <cDV>: " + oDadosNFe.cDV + "\r\n" +
                            "Número do dígito verificador informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(46, 1) + "\r\n\r\n";
                        booValido = false;
                    }
                    #endregion

                    #region Tag <CNPJ> da tag <emit>
                    if (oDadosNFe.CNPJ != oDadosNFe.chavenfe.Substring(9, 14))
                    {
                        cTextoErro += "O CNPJ do emitente informado na tag <emit><CNPJ> está diferente do informado na chave da NF-e.\r\n" +
                            "CNPJ do emitente informado na tag <emit><CNPJ>: " + oDadosNFe.CNPJ + "\r\n" +
                            "CNPJ do emitente informado na chave da NF-e: " + oDadosNFe.chavenfe.Substring(9, 14) + "\r\n\r\n";
                        booValido = false;
                    }
                    #endregion

                    #region Tag <serie>
                    if (Convert.ToInt32(oDadosNFe.serie) != Convert.ToInt32(oDadosNFe.chavenfe.Substring(25, 3)))
                    {
                        cTextoErro += "A série informada na tag <serie> está diferente da informada na chave da NF-e.\r\n" +
                            "Série informada na tag <cDV>: " + Convert.ToInt32(oDadosNFe.serie).ToString() + "\r\n" +
                            "Série informada na chave da NF-e: " + Convert.ToInt32(oDadosNFe.chavenfe.Substring(25, 3)).ToString() + "\r\n\r\n";
                        booValido = false;
                    }
                    #endregion

                    #region Tag <dEmi>
                    if (oDadosNFe.dEmi.Month.ToString("00") != oDadosNFe.chavenfe.Substring(7, 2) ||
                        oDadosNFe.dEmi.Year.ToString("0000").Substring(2, 2) != oDadosNFe.chavenfe.Substring(5, 2))
                    {
                        cTextoErro += "A ano e mês da emissão informada na tag <dEmi> está diferente da informada na chave da NF-e.\r\n" +
                            "Mês/Ano da data de emissão informada na tag <dEmi>: " + oDadosNFe.dEmi.Month.ToString("00") + "/" + oDadosNFe.dEmi.Year.ToString("0000").Substring(2, 2) + "\r\n" +
                            "Mês/Ano informados na chave da NF-e: " + oDadosNFe.chavenfe.Substring(5, 2) + "/" + oDadosNFe.chavenfe.Substring(7, 2) + "\r\n\r\n";
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
            catch (Exception ex)
            {
                booValido = false;

                throw (ex);
            }

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
        public void LoteNfe(string Arquivo)
        {
            try
            {
                oGerarXML.LoteNfe(Arquivo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region LoteNfe() - Sobrecarga
        /// <summary>
        /// Auxliar na geração do arquivo XML de Lote de notas fiscais
        /// </summary>
        /// <param name="lstArquivoNfe">Lista de arquivos de NFe para montagem do lote de várias NFe</param>
        /// <date>24/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public void LoteNfe(List<string> lstArquivoNfe)
        {
            try
            {
                oGerarXML.LoteNfe(lstArquivoNfe);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region ProcessaNotaDenegada
        protected void ProcessaNotaDenegada(int emp, LerXML oLerXml, string strArquivoNFe, XmlElement infConsSitElemento)
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
                    strProtNfe = this.GeraStrProtNFe(infConsSitElemento);
                    ///
                    /// gera o arquivo de denegacao na pasta EmProcessamento
                    string strNomeArqDenegadaNFe = oGerarXML.XmlDistNFe(strArquivoNFe, strProtNfe, "-den.xml");
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
                        ///
                        /// move o arquivo NFe para a pasta Denegada
                        File.Move(strArquivoNFe, dArquivo);
                    }
                }
                //Move a NFE da pasta de NFE em processamento para NFe Denegadas
                //if (!oAux.EstaDenegada(strArquivoNFe, oLerXml.oDadosNfe.dEmi))
                //{
                //    oAux.MoverArquivo(strArquivoNFe, PastaEnviados.Denegados, oLerXml.oDadosNfe.dEmi);
                //}
            }
            //return strProtNfe;
        }
        #endregion

        #region XmlPedRec()
        /// <summary>
        /// Auxiliar na geração do arquivo XML de consulta do recibo do lote quando estivermos utilizando o InvokeMember para chamar este método
        /// </summary>
        /// <param name="pFinalArqEnvio">Final do nome do arquivo de solicitação do serviço.</param>
        /// <param name="pFinalArqRetorno">Final do nome do arquivo que é para ser gravado o retorno.</param>
        /// <date>07/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public void XmlPedRec(int empresa, string nRec)
        {
            GerarXML gerarXML = new GerarXML(empresa);
            gerarXML.XmlPedRec(nRec);
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
            try
            {
                oGerarXML.XmlRetorno(pFinalArqEnvio, pFinalArqRetorno, this.vStrXmlRetorno);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

    }
}
