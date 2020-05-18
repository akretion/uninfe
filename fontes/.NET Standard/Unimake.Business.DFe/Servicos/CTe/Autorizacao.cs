using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.CTe;

namespace Unimake.Business.DFe.Servicos.CTe
{
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

        #region Private Fields

        private EnviCTe _enviCTe;
        private Dictionary<string, CteProc> CteProcs = new Dictionary<string, CteProc>();

        #endregion Private Fields

        #region Protected Properties

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

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// Propriedade com o conteúdo retornado da consulta recibo
        /// </summary>
        public RetConsReciCTe RetConsReciCTe { get; set; }

        /// <summary>
        /// Propriedade com o conteúdo retornado da consulta situção do CTe
        /// </summary>
        public List<RetConsSitCTe> RetConsSitCTe = new List<RetConsSitCTe>();

        /// <summary>
        /// Propriedade contendo o XML da CTe com o protocolo de autorização anexado
        /// </summary>
        public Dictionary<string, CteProc> CteProcResult
        {
            get
            {
                if(RetConsReciCTe == null && RetConsSitCTe.Count <= 0)
                {
                    throw new Exception("Defina o conteúdo da Propriedade RetConsReciCTe ou RetConsSitCte, sem a definição de uma delas não é possível obter o conteúdo da CteProcResult.");
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
                    else if(RetConsSitCTe.Count > 0)
                    {
                        #region Resultado do envio do CT-e através da consulta situação

                        foreach(var item in RetConsSitCTe)
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
                        CteProcs[EnviCTe.CTe[i].InfCTe.Chave].ProtCTe = protCTe ;
                    }
                    else
                    {
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

        public Autorizacao()
            : base() => CteProcs.Clear();

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
            foreach(var item in CteProcResult)
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
            foreach(var item in CteProcResult)
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