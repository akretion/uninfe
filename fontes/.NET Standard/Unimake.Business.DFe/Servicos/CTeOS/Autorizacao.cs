﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.CTe;
using Unimake.Business.DFe.Xml.CTeOS;

namespace Unimake.Business.DFe.Servicos.CTeOS
{
    /// <summary>
    /// Envio do XML de CTeOS para webservice
    /// </summary>
    public class Autorizacao: ServicoBase, IInteropService<Xml.CTeOS.CTeOS>
    {
        private void MontarQrCode()
        {
            CTeOS = new Xml.CTeOS.CTeOS().LerXML<Xml.CTeOS.CTeOS>(ConteudoXML);

            if(CTeOS.InfCTeSupl == null)
            {
                CTeOS.InfCTeSupl = new Xml.CTeOS.InfCTeSupl();

                var urlQrCode = (Configuracoes.TipoAmbiente == TipoAmbiente.Homologacao ? Configuracoes.UrlQrCodeHomologacao : Configuracoes.UrlQrCodeProducao);

                var paramLinkQRCode = urlQrCode +
                    "?chCTe=" + CTeOS.InfCTe.Chave +
                    "&tpAmb=" + ((int)CTeOS.InfCTe.Ide.TpAmb).ToString();

                if(CTeOS.InfCTe.Ide.TpEmis == TipoEmissao.ContingenciaEPEC || CTeOS.InfCTe.Ide.TpEmis == TipoEmissao.ContingenciaFSDA)
                {
                    paramLinkQRCode = "&sign=" + Converter.ToRSASHA1(Configuracoes.CertificadoDigital, CTeOS.InfCTe.Chave);
                }

                CTeOS.InfCTeSupl.QrCodCTe = paramLinkQRCode.Trim();
            }

            //Atualizar a propriedade do XML do CTe novamente com o conteúdo atual já a tag de QRCode e link de consulta
            ConteudoXML = CTeOS.GerarXML();
        }

        #region Private Fields

        private Xml.CTeOS.CTeOS _cteOS;
        private Dictionary<string, CteOSProc> CteOSProcs = new Dictionary<string, CteOSProc>();

        #endregion Private Fields

        #region Protected Properties

        /// <summary>
        /// Objeto do XML do CTe-OS
        /// </summary>
        public Xml.CTeOS.CTeOS CTeOS
        {
            get => _cteOS ?? (_cteOS = new Xml.CTeOS.CTeOS().LerXML<Xml.CTeOS.CTeOS>(ConteudoXML));
            protected set => _cteOS = value;
        }

        #endregion Protected Properties

        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            if(CTeOS == null)
            {
                Configuracoes.Definida = false;
                return;
            }

            var xml = CTeOS;

            if(!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.CTeAutorizacaoOS;
                Configuracoes.CodigoUF = (int)xml.InfCTe.Ide.CUF;
                Configuracoes.TipoAmbiente = xml.InfCTe.Ide.TpAmb;
                Configuracoes.Modelo = xml.InfCTe.Ide.Mod;
                Configuracoes.TipoEmissao = xml.InfCTe.Ide.TpEmis;
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
        /// Propriedade com o conteúdo retornado da consulta situção do CTe
        /// </summary>
        public List<RetConsSitCTe> RetConsSitCTes = new List<RetConsSitCTe>();

        /// <summary>
        /// Propriedade contendo o XML da CTe com o protocolo de autorização anexado - Envio Assincrono
        /// </summary>
        public Dictionary<string, CteOSProc> CteOSProcResults
        {
            get
            {
                if(Result.ProtCTe != null)
                {
                    if(CteOSProcs.ContainsKey(CTeOS.InfCTe.Chave))
                    {
                        CteOSProcs[CTeOS.InfCTe.Chave].ProtCTe = Result.ProtCTe;
                    }
                    else
                    {
                        CteOSProcs.Add(CTeOS.InfCTe.Chave,
                            new CteOSProc
                            {
                                Versao = CTeOS.Versao,
                                CTeOS = CTeOS,
                                ProtCTe = Result.ProtCTe
                            });
                    }
                }
                else
                {
                    if(RetConsSitCTes.Count <= 0)
                    {
                        throw new Exception("Defina o conteúdo da Propriedade RetConsSitCte, sem a definição dela não é possível obter o conteúdo da CteOSProcResults.");
                    }

                    ProtCTe protCTe = null;

                    #region Resultado do envio do CTeOS através da consulta situação

                    foreach(var item in RetConsSitCTes)
                    {
                        if(item != null && item.ProtCTe != null)
                        {
                            if(item.ProtCTe.InfProt.ChCTe == CTeOS.InfCTe.Chave)
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

                    if(CteOSProcs.ContainsKey(CTeOS.InfCTe.Chave))
                    {
                        CteOSProcs[CTeOS.InfCTe.Chave].ProtCTe = protCTe;
                    }
                    else
                    {
                        CteOSProcs.Add(CTeOS.InfCTe.Chave,
                            new CteOSProc
                            {
                                Versao = CTeOS.Versao,
                                CTeOS = CTeOS,
                                ProtCTe = protCTe
                            });
                    }

                    #endregion
                }

                return CteOSProcs;
            }
        }

        /// <summary>
        /// Conteúdo retornado pelo webservice depois do envio do XML
        /// </summary>
        public RetCTeOS Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetCTeOS>(RetornoWSXML);
                }

                return new RetCTeOS
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
            : base() => CteOSProcs.Clear();

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="cteOS">Objeto contendo o XML a ser enviado</param>
        /// <param name="configuracao">Configurações para conexão e envio do XML para o webservice</param>
        public Autorizacao(Xml.CTeOS.CTeOS cteOS, Configuracao configuracao)
            : base(cteOS?.GerarXML() ?? throw new ArgumentNullException(nameof(cteOS)), configuracao)
        {
            Inicializar();

            CteOSProcs.Clear();
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
                if(CTeOS == null)
                {
                    throw new NullReferenceException($"{nameof(CTeOS)} não pode ser nulo.");
                }

                DefinirConfiguracao();
            }

            base.Executar();
        }

#if INTEROP

        /// <summary>
        /// Executa o serviço: Assina o XML, valida e envia para o webservice
        /// </summary>
        /// <param name="cteOS">Objeto contendo o XML a ser enviado</param>
        /// <param name="configuracao">Configurações a serem utilizadas na conexão e envio do XML para o webservice</param>
        [ComVisible(true)]
        public void Executar(Xml.CTeOS.CTeOS cteOS, Configuracao configuracao)
        {
            PrepararServico(cteOS?.GerarXML() ?? throw new ArgumentNullException(nameof(cteOS)), configuracao);
            Executar();
        } 

#endif

        /// <summary>
        /// Gravar o XML de distribuição em uma pasta no HD
        /// </summary>
        /// <param name="pasta">Pasta onde deve ser gravado o XML</param>
        public void GravarXmlDistribuicao(string pasta)
        {
            foreach(var item in CteOSProcResults)
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
            foreach(var item in CteOSProcResults)
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