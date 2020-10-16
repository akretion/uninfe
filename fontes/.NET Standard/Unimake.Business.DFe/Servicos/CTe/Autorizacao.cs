﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.CTe;

namespace Unimake.Business.DFe.Servicos.CTe
{
    /// <summary>
    /// Envio do XML de lote de CTe para o WebService
    /// </summary>
    public class Autorizacao: ServicoBase, IInteropService<EnviCTe>
    {
        private void MontarQrCode()
        {
            EnviCTe = new EnviCTe().LerXML<EnviCTe>(ConteudoXML);

            for(var i = 0; i < EnviCTe.CTe.Count; i++)
            {
                if(EnviCTe.CTe[i].InfCTeSupl == null)
                {
                    EnviCTe.CTe[i].InfCTeSupl = new InfCTeSupl();

                    var urlQrCode = (Configuracoes.TipoAmbiente == TipoAmbiente.Homologacao ? Configuracoes.UrlQrCodeHomologacao : Configuracoes.UrlQrCodeProducao);

                    var paramLinkQRCode = urlQrCode +
                        "?chCTe=" + EnviCTe.CTe[i].InfCTe.Chave +
                        "&tpAmb=" + ((int)EnviCTe.CTe[i].InfCTe.Ide.TpAmb).ToString();

                    if(EnviCTe.CTe[i].InfCTe.Ide.TpEmis == TipoEmissao.ContingenciaEPEC || EnviCTe.CTe[i].InfCTe.Ide.TpEmis == TipoEmissao.ContingenciaFSDA)
                    {
                        paramLinkQRCode = "&sign=" + Converter.ToRSASHA1(Configuracoes.CertificadoDigital, EnviCTe.CTe[i].InfCTe.Chave);
                    }

                    EnviCTe.CTe[i].InfCTeSupl.QrCodCTe = paramLinkQRCode.Trim();
                }
            }

            //Atualizar a propriedade do XML do CTe novamente com o conteúdo atual já a tag de QRCode e link de consulta
            ConteudoXML = EnviCTe.GerarXML();
        }

        /// <summary>
        /// Validar o XML do CTe e também o Modal específico
        /// </summary>
        /// <param name="xml">XML a ser validado</param>
        /// <param name="schemaArquivo">Nome do arquivo de schemas para ser utilizado na validação</param>
        /// <param name="targetNS">Namespace a ser utilizado na validação</param>
        private void ValidarXMLCTe(XmlDocument xml, string schemaArquivo, string targetNS)
        {
            var validar = new ValidarSchema();
            validar.Validar(xml, Configuracoes.TipoDFe.ToString() + "." + schemaArquivo, targetNS);

            if(!validar.Success)
            {
                throw new Exception(validar.ErrorMessage);
            }
        }

        #region Private Fields

        private EnviCTe _enviCTe;
        private Dictionary<string, CteProc> CteProcs = new Dictionary<string, CteProc>();

        #endregion Private Fields

        #region Protected Properties

        /// <summary>
        /// Objeto do XML do CTe
        /// </summary>
        public EnviCTe EnviCTe
        {
            get => _enviCTe ?? (_enviCTe = new EnviCTe().LerXML<EnviCTe>(ConteudoXML));
            protected set => _enviCTe = value;
        }

        #endregion Protected Properties

        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            if(EnviCTe == null)
            {
                Configuracoes.Definida = false;
                return;
            }

            var xml = EnviCTe;

            if(!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.CTeAutorizacao;
                Configuracoes.CodigoUF = (int)xml.CTe[0].InfCTe.Ide.CUF;
                Configuracoes.TipoAmbiente = xml.CTe[0].InfCTe.Ide.TpAmb;
                Configuracoes.Modelo = xml.CTe[0].InfCTe.Ide.Mod;
                Configuracoes.TipoEmissao = xml.CTe[0].InfCTe.Ide.TpEmis;
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        /// <summary>
        /// Efetuar um Ajustse no XML da NFCe logo depois de assinado
        /// </summary>
        protected override void AjustarXMLAposAssinado()
        {
            MontarQrCode();
            base.AjustarXMLAposAssinado();
        }

        /// <summary>
        /// Validar o XML
        /// </summary>
        protected override void XmlValidar()
        {
            var xml = EnviCTe;

            if(Configuracoes.SchemasEspecificos.Count > 0)
            {
                for(var i = 0; i < xml.CTe.Count; i++)
                {
                    var modal = (int)xml.CTe[i].InfCTe.Ide.Modal;

                    var schemaArquivo = Configuracoes.SchemasEspecificos[modal.ToString()].SchemaArquivo;
                    var schemaArquivoEspecifico = Configuracoes.SchemasEspecificos[modal.ToString()].SchemaArquivoEspecifico;

                    #region Validar o XML geral

                    ValidarXMLCTe(ConteudoXML, schemaArquivo, Configuracoes.TargetNS);

                    #endregion Validar o XML geral

                    #region Validar a parte específica de modal do CTe

                    var xmlEspecifico = new XmlDocument();
                    switch(xml.CTe[i].InfCTe.Ide.Modal)
                    {
                        case ModalidadeTransporteCTe.Rodoviario:
                            xmlEspecifico.LoadXml(XMLUtility.Serializar<Rodo>(xml.CTe[i].InfCTe.InfCTeNorm.InfModal.Rodo).OuterXml);
                            goto default;

                        case ModalidadeTransporteCTe.Aereo:
                            xmlEspecifico.LoadXml(XMLUtility.Serializar<Aereo>(xml.CTe[i].InfCTe.InfCTeNorm.InfModal.Aereo).OuterXml);
                            goto default;

                        case ModalidadeTransporteCTe.Aquaviario:
                            xmlEspecifico.LoadXml(XMLUtility.Serializar<Aquav>(xml.CTe[i].InfCTe.InfCTeNorm.InfModal.Aquav).OuterXml);
                            goto default;

                        case ModalidadeTransporteCTe.Ferroviario:
                            xmlEspecifico.LoadXml(XMLUtility.Serializar<Ferrov>(xml.CTe[i].InfCTe.InfCTeNorm.InfModal.Ferrov).OuterXml);
                            goto default;

                        case ModalidadeTransporteCTe.Dutoviario:
                            xmlEspecifico.LoadXml(XMLUtility.Serializar<Duto>(xml.CTe[i].InfCTe.InfCTeNorm.InfModal.Duto).OuterXml);
                            goto default;

                        case ModalidadeTransporteCTe.Multimodal:
                            xmlEspecifico.LoadXml(XMLUtility.Serializar<MultiModal>(xml.CTe[i].InfCTe.InfCTeNorm.InfModal.MultiModal).OuterXml);
                            goto default;

                        default:
                            ValidarXMLCTe(xmlEspecifico, schemaArquivoEspecifico, Configuracoes.TargetNS);
                            break;
                    }

                    #endregion Validar a parte específica de cada evento

                }
            }
        }

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// Propriedade com o conteúdo retornado da consulta recibo
        /// </summary>
        public RetConsReciCTe RetConsReciCTe { get; set; }

        /// <summary>
        /// Propriedade com o conteúdo retornado da consulta situção do CTe
        /// </summary>
        public List<RetConsSitCTe> RetConsSitCTes = new List<RetConsSitCTe>();

        /// <summary>
        /// Propriedade contendo o XML da CTe com o protocolo de autorização anexado - Envio Assincrono
        /// </summary>
        public Dictionary<string, CteProc> CteProcResults
        {
            get
            {
                if(RetConsReciCTe == null && RetConsSitCTes.Count <= 0)
                {
                    throw new Exception("Defina o conteúdo da Propriedade RetConsReciCTe ou RetConsSitCte, sem a definição de uma delas não é possível obter o conteúdo da CteProcResults.");
                }

                for(var i = 0; i < EnviCTe.CTe.Count; i++)
                {
                    ProtCTe protCTe = null;

                    if(RetConsReciCTe != null && RetConsReciCTe.ProtCTe != null)
                    {
                        #region Resultado do envio do CT-e através da consulta recibo

                        if(RetConsReciCTe.CStat == 104) //Lote Processado
                        {
                            foreach(var item in RetConsReciCTe.ProtCTe)
                            {
                                if(item.InfProt.ChCTe == EnviCTe.CTe[i].InfCTe.Chave)
                                {
                                    switch(item.InfProt.CStat)
                                    {
                                        case 100: //CTe Autorizado
                                        case 110: //CTe Denegado - Não sei quando ocorre este, mas descobrir ele no manual então estou incluindo. 
                                        case 301: //CTe Denegado - Irregularidade fiscal do emitente
                                        case 302: //CTe Denegado - Irregularidade fiscal do remetente
                                        case 303: //CTe Denegado - Irregularidade fiscal do destinatário
                                        case 304: //CTe Denegado - Irregularidade fiscal do expedidor
                                        case 305: //CTe Denegado - Irregularidade fiscal do recebedor
                                        case 306: //CTe Denegado - Irregularidade fiscal do tomador
                                            protCTe = item;
                                            break;
                                    }

                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                    else if(RetConsSitCTes.Count > 0)
                    {
                        #region Resultado do envio do CT-e através da consulta situação

                        foreach(var item in RetConsSitCTes)
                        {
                            if(item != null && item.ProtCTe != null)
                            {
                                if(item.ProtCTe.InfProt.ChCTe == EnviCTe.CTe[i].InfCTe.Chave)
                                {
                                    switch(item.ProtCTe.InfProt.CStat)
                                    {
                                        case 100: //CTe Autorizado
                                        case 110: //CTe Denegado - Não sei quando ocorre este, mas descobrir ele no manual então estou incluindo. 
                                        case 301: //CTe Denegado - Irregularidade fiscal do emitente
                                        case 302: //CTe Denegado - Irregularidade fiscal do remetente
                                        case 303: //CTe Denegado - Irregularidade fiscal do destinatário
                                        case 304: //CTe Denegado - Irregularidade fiscal do expedidor
                                        case 305: //CTe Denegado - Irregularidade fiscal do recebedor
                                        case 306: //CTe Denegado - Irregularidade fiscal do tomador
                                            protCTe = item.ProtCTe;
                                            break;
                                    }
                                }
                            }
                        }

                        #endregion
                    }

                    if(CteProcs.ContainsKey(EnviCTe.CTe[i].InfCTe.Chave))
                    {
                        CteProcs[EnviCTe.CTe[i].InfCTe.Chave].ProtCTe = protCTe;
                    }
                    else
                    {
                        //Se por algum motivo não tiver assinado, só vou forçar atualizar o ConteudoXML para ficar correto na hora de gerar o arquivo de distribuição. Pode estar sem assinar no caso do desenvolvedor estar forçando gerar o XML já autorizado a partir de uma consulta situação da NFe, caso tenha perdido na tentativa do primeiro envio.
                        if(EnviCTe.CTe[i].Signature == null)
                        {
                            ConteudoXML = ConteudoXMLAssinado;
                            AjustarXMLAposAssinado();
                        }

                        CteProcs.Add(EnviCTe.CTe[i].InfCTe.Chave,
                            new CteProc
                            {
                                Versao = EnviCTe.Versao,
                                CTe = EnviCTe.CTe[i],
                                ProtCTe = protCTe
                            });
                    }
                }

                return CteProcs;
            }
        }

        /// <summary>
        /// Conteúdo retornado pelo webservice depois do envio do XML
        /// </summary>
        public RetEnviCTe Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetEnviCTe>(RetornoWSXML);
                }

                return new RetEnviCTe
                {
                    CStat = 0,
                    XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                };
            }
        }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Construtor
        /// </summary>
        public Autorizacao()
            : base() => CteProcs.Clear();

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="enviCTe">Objeto contendo o XML a ser enviado</param>
        /// <param name="configuracao">Configurações para conexão e envio do XML para o webservice</param>
        public Autorizacao(EnviCTe enviCTe, Configuracao configuracao)
            : base(enviCTe?.GerarXML() ?? throw new ArgumentNullException(nameof(enviCTe)), configuracao)
        {
            Inicializar();

            CteProcs.Clear();
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
                if(EnviCTe == null)
                {
                    throw new NullReferenceException($"{nameof(EnviCTe)} não pode ser nulo.");
                }

                DefinirConfiguracao();
            }

            base.Executar();
        }

        /// <summary>
        /// Executa o serviço: Assina o XML, valida e envia para o webservice
        /// </summary>
        /// <param name="enviCTe">Objeto contendo o XML a ser enviado</param>
        /// <param name="configuracao">Configurações a serem utilizadas na conexão e envio do XML para o webservice</param>
        [ComVisible(true)]
        public void Executar(EnviCTe enviCTe, Configuracao configuracao)
        {
            PrepararServico(enviCTe?.GerarXML() ?? throw new ArgumentNullException(nameof(enviCTe)), configuracao);
            Executar();
        }

        /// <summary>
        /// Gravar o XML de distribuição em uma pasta no HD
        /// </summary>
        /// <param name="pasta">Pasta onde deve ser gravado o XML</param>
        public void GravarXmlDistribuicao(string pasta)
        {
            foreach(var item in CteProcResults)
            {
                if(item.Value.ProtCTe != null)
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
            foreach(var item in CteProcResults)
            {
                if(item.Value.ProtCTe != null)
                {
                    GravarXmlDistribuicao(stream, item.Value.GerarXML().OuterXml);
                }
            }
        }

        #endregion Public Methods
    }
}