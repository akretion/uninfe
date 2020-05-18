﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.MDFe;

namespace Unimake.Business.DFe.Servicos.MDFe
{
    public class Autorizacao: ServicoBase, IInteropService<EnviMDFe>
    {
        #region Private Fields

        private EnviMDFe _enviMDFe;

        private Dictionary<string, MdfeProc> MdfeProcs = new Dictionary<string, MdfeProc>();

        #endregion Private Fields

        #region Private Methods

        private void MontarQrCode()
        {
            EnviMDFe = new EnviMDFe().LerXML<EnviMDFe>(ConteudoXML);

            if(EnviMDFe.MDFe.InfMDFeSupl == null)
            {
                EnviMDFe.MDFe.InfMDFeSupl = new InfMDFeSupl();

                var urlQrCode = (Configuracoes.TipoAmbiente == TipoAmbiente.Homologacao ? Configuracoes.UrlQrCodeHomologacao : Configuracoes.UrlQrCodeProducao);

                var paramLinkQRCode = urlQrCode +
                    "?chMDFe=" + EnviMDFe.MDFe.InfMDFe.Chave +
                    "&tpAmb=" + ((int)EnviMDFe.MDFe.InfMDFe.Ide.TpAmb).ToString();

                if(EnviMDFe.MDFe.InfMDFe.Ide.TpEmis == TipoEmissao.ContingenciaEPEC || EnviMDFe.MDFe.InfMDFe.Ide.TpEmis == TipoEmissao.ContingenciaFSDA)
                {
                    paramLinkQRCode = "&sign=" + Converter.ToRSASHA1(Configuracoes.CertificadoDigital, EnviMDFe.MDFe.InfMDFe.Chave);
                }

                EnviMDFe.MDFe.InfMDFeSupl.QrCodMDFe = paramLinkQRCode.Trim();
            }

            //Atualizar a propriedade do XML do MDFe novamente com o conteúdo atual já a tag de QRCode e link de consulta
            ConteudoXML = EnviMDFe.GerarXML();
        }

        /// <summary>
        /// Validar o XML do MDFe e também o Modal específico
        /// </summary>
        /// <param name="xml">XML a ser validado</param>
        /// <param name="schemaArquivo">Nome do arquivo de schemas para ser utilizado na validação</param>
        /// <param name="targetNS">Namespace a ser utilizado na validação</param>
        private void ValidarXMLMDFe(XmlDocument xml, string schemaArquivo, string targetNS)
        {
            var validar = new ValidarSchema();
            validar.Validar(xml, Path.Combine(Configuracoes.SchemaPasta, schemaArquivo), targetNS);

            if(!validar.Success)
            {
                throw new Exception(validar.ErrorMessage);
            }
        }

        #endregion Private Methods

        #region Protected Methods

        /// <summary>
        /// Efetuar um Ajustse no XML da NFCe logo depois de assinado
        /// </summary>
        protected override void AjustarXMLAposAssinado()
        {
            MontarQrCode();
            base.AjustarXMLAposAssinado();
        }

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            if(EnviMDFe == null)
            {
                Configuracoes.Definida = false;
                return;
            }

            var xml = EnviMDFe;

            if(!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.MDFeAutorizacao;
                Configuracoes.CodigoUF = (int)xml.MDFe.InfMDFe.Ide.CUF;
                Configuracoes.TipoAmbiente = xml.MDFe.InfMDFe.Ide.TpAmb;
                Configuracoes.Modelo = xml.MDFe.InfMDFe.Ide.Mod;
                Configuracoes.TipoEmissao = xml.MDFe.InfMDFe.Ide.TpEmis;
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        #endregion Protected Methods

        #region Public Fields

        /// <summary>
        /// Propriedade com o conteúdo retornado da consulta situção do MDFe
        /// </summary>
        public List<RetConsSitMDFe> RetConsSitMDFe = new List<RetConsSitMDFe>();

        #endregion Public Fields

        #region Public Properties

        public EnviMDFe EnviMDFe
        {
            get => _enviMDFe ?? (_enviMDFe = new EnviMDFe().LerXML<EnviMDFe>(ConteudoXML));
            protected set => _enviMDFe = value;
        }

        /// <summary>
        /// Propriedade contendo o XML da MDFe com o protocolo de autorização anexado
        /// </summary>
        public Dictionary<string, MdfeProc> MDFeProcResult
        {
            get
            {
                if(RetConsReciMDFe == null && RetConsSitMDFe.Count <= 0)
                {
                    throw new Exception("Defina o conteúdo da Propriedade RetConsReciMDFe ou RetConsSitMDFe, sem a definição de uma delas não é possível obter o conteúdo da MDFeProcResult.");
                }

                ProtMDFe protMDFe = null;

                if(RetConsReciMDFe != null && RetConsReciMDFe.ProtMDFe != null)
                {
                    #region Resultado do envio do CT-e através da consulta recibo

                    if(RetConsReciMDFe.CStat == 104) //Lote Processado
                    {
                        foreach(var item in RetConsReciMDFe.ProtMDFe)
                        {
                            if(item.InfProt.ChMDFe == EnviMDFe.MDFe.InfMDFe.Chave)
                            {
                                switch(item.InfProt.CStat)
                                {
                                    case 100: //MDFe Autorizado
                                        protMDFe = item;
                                        break;
                                }

                                break;
                            }
                        }
                    }

                    #endregion Resultado do envio do CT-e através da consulta recibo
                }
                else if(RetConsSitMDFe.Count > 0)
                {
                    #region Resultado do envio do CT-e através da consulta situação

                    foreach(var item in RetConsSitMDFe)
                    {
                        if(item != null && item.ProtMDFe != null)
                        {
                            if(item.ProtMDFe.InfProt.ChMDFe == EnviMDFe.MDFe.InfMDFe.Chave)
                            {
                                switch(item.ProtMDFe.InfProt.CStat)
                                {
                                    case 100: //MDFe Autorizado
                                        protMDFe = item.ProtMDFe;
                                        break;
                                }
                            }
                        }
                    }

                    #endregion Resultado do envio do CT-e através da consulta situação
                }

                if(MdfeProcs.ContainsKey(EnviMDFe.MDFe.InfMDFe.Chave))
                {
                    MdfeProcs[EnviMDFe.MDFe.InfMDFe.Chave].ProtMDFe = protMDFe;
                }
                else
                {
                    MdfeProcs.Add(EnviMDFe.MDFe.InfMDFe.Chave,
                        new MdfeProc
                        {
                            Versao = EnviMDFe.Versao,
                            MDFe = EnviMDFe.MDFe,
                            ProtMDFe = protMDFe
                        });
                }

                return MdfeProcs;
            }
        }

        public RetEnviMDFe Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetEnviMDFe>(RetornoWSXML);
                }

                return new RetEnviMDFe
                {
                    CStat = 0,
                    XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                };
            }
        }

        /// <summary>
        /// Propriedade com o conteúdo retornado da consulta recibo
        /// </summary>
        public RetConsReciMDFe RetConsReciMDFe { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public Autorizacao()
            : base() => MdfeProcs.Clear();

        public Autorizacao(EnviMDFe enviMDFe, Configuracao configuracao)
            : base(enviMDFe?.GerarXML() ?? throw new ArgumentNullException(nameof(enviMDFe)), configuracao)
        {
            Inicializar();

            MdfeProcs.Clear();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Executar o serviço
        /// </summary>
        [ComVisible(false)]
        public override void Executar()
        {
            if(!Configuracoes.Definida)
            {
                if(EnviMDFe == null)
                {
                    throw new NullReferenceException($"{nameof(EnviMDFe)} não pode ser nulo.");
                }

                DefinirConfiguracao();
            }

            base.Executar();
        }

        protected override void XmlValidar()
        {
            var xml = EnviMDFe;

            var schemaArquivo = string.Empty;
            var schemaArquivoEspecifico = string.Empty;

            if(Configuracoes.SchemasEspecificos.Count > 0)
            {
                var modal = (int)xml.MDFe.InfMDFe.Ide.Modal;

                schemaArquivo = Configuracoes.SchemasEspecificos[modal.ToString()].SchemaArquivo;
                schemaArquivoEspecifico = Configuracoes.SchemasEspecificos[modal.ToString()].SchemaArquivoEspecifico;
            }

            #region Validar o XML geral

            ValidarXMLMDFe(ConteudoXML, schemaArquivo, Configuracoes.TargetNS);

            #endregion Validar o XML geral

            #region Validar a parte específica de modal do MDFe

            var xmlEspecifico = new XmlDocument();
            switch(xml.MDFe.InfMDFe.Ide.Modal)
            {
                case ModalidadeTransporteMDFe.Rodoviario:
                    xmlEspecifico.LoadXml(XMLUtility.Serializar<Rodo>((Rodo)xml.MDFe.InfMDFe.InfModal.Rodo).OuterXml);
                    goto default;

                case ModalidadeTransporteMDFe.Aereo:
                    xmlEspecifico.LoadXml(XMLUtility.Serializar<Aereo>((Aereo)xml.MDFe.InfMDFe.InfModal.Aereo).OuterXml);
                    goto default;

                case ModalidadeTransporteMDFe.Aquaviario:
                    xmlEspecifico.LoadXml(XMLUtility.Serializar<Aquav>((Aquav)xml.MDFe.InfMDFe.InfModal.Aquav).OuterXml);
                    goto default;

                case ModalidadeTransporteMDFe.Ferroviario:
                    xmlEspecifico.LoadXml(XMLUtility.Serializar<Ferrov>((Ferrov)xml.MDFe.InfMDFe.InfModal.Ferrov).OuterXml);
                    goto default;

                default:
                    ValidarXMLMDFe(xmlEspecifico, schemaArquivoEspecifico, Configuracoes.TargetNS);
                    break;
             
            }

            #endregion Validar a parte específica de cada evento
        }

        [ComVisible(true)]
        public void Executar(EnviMDFe enviMDFe, Configuracao configuracao)
        {
            PrepararServico(enviMDFe?.GerarXML() ?? throw new ArgumentNullException(nameof(enviMDFe)), configuracao);
            Executar();
        }

        /// <summary>
        /// Gravar o XML de distribuição em uma pasta no HD
        /// </summary>
        /// <param name="pasta">Pasta onde deve ser gravado o XML</param>
        public void GravarXmlDistribuicao(string pasta)
        {
            foreach(var item in MDFeProcResult)
            {
                if(item.Value.ProtMDFe != null)
                {
                    GravarXmlDistribuicao(pasta, item.Value.NomeArquivoDistribuicao, item.Value.GerarXML().OuterXml);
                }
            }
        }

        /// <summary>
        /// Grava o XML de dsitribuição no stream
        /// </summary>
        /// <param name="stream">Stream que vai receber o XML de distribuição</param>
        public void GravarXmlDistribuicao(System.IO.Stream stream)
        {
            foreach(var item in MDFeProcResult)
            {
                if(item.Value.ProtMDFe != null)
                {
                    GravarXmlDistribuicao(stream, item.Value.GerarXML().OuterXml);
                }
            }
        }

        #endregion Public Methods
    }
}