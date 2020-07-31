﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public class Autorizacao: ServicoBase, IInteropService<EnviNFe>
    {
        #region Private Fields

        private EnviNFe _enviNFe;
        private Dictionary<string, NfeProc> NfeProcs = new Dictionary<string, NfeProc>();

        #endregion Private Fields

        #region Protected Properties

        public EnviNFe EnviNFe
        {
            get => _enviNFe ?? (_enviNFe = new EnviNFe().LerXML<EnviNFe>(ConteudoXML));
            protected set => _enviNFe = value;
        }

        #endregion Protected Properties

        #region Protected Methods

        /// <summary>
        /// Definir o valor de algumas das propriedades do objeto "Configuracoes"
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            if(EnviNFe == null)
            {
                Configuracoes.Definida = false;
                return;
            }

            var xml = EnviNFe;

            if(!Configuracoes.Definida)
            {
                Configuracoes.Servico = Servico.NFeAutorizacao;
                Configuracoes.CodigoUF = (int)xml.NFe[0].InfNFe[0].Ide.CUF;
                Configuracoes.TipoAmbiente = xml.NFe[0].InfNFe[0].Ide.TpAmb;
                Configuracoes.Modelo = xml.NFe[0].InfNFe[0].Ide.Mod;
                Configuracoes.TipoEmissao = xml.NFe[0].InfNFe[0].Ide.TpEmis;
                Configuracoes.SchemaVersao = xml.Versao;

                base.DefinirConfiguracao();
            }
        }

        protected override void AjustarXMLAposAssinado() => EnviNFe = new EnviNFe().LerXML<EnviNFe>(ConteudoXML);

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// Propriedade com o conteúdo retornado da consulta recibo
        /// </summary>
        public RetConsReciNFe RetConsReciNFe { get; set; }

        /// <summary>
        /// Propriedade com o conteúdo retornado da consulta situção do NFe
        /// </summary>
        public List<RetConsSitNFe> RetConsSitNFes = new List<RetConsSitNFe>();

        /// <summary>
        /// Propriedade contendo o XML da NFe com o protocolo de autorização anexado - Funciona para envio Assíncrono ou Síncrono
        /// </summary>
        public Dictionary<string, NfeProc> NfeProcResults
        {
            get
            {
                if(EnviNFe.IndSinc == SimNao.Sim && Result.ProtNFe != null) //Envio síncrono
                {
                    if(NfeProcs.ContainsKey(EnviNFe.NFe[0].InfNFe[0].Chave))
                    {
                        NfeProcs[EnviNFe.NFe[0].InfNFe[0].Chave].ProtNFe = Result.ProtNFe;
                    }
                    else
                    {
                        NfeProcs.Add(EnviNFe.NFe[0].InfNFe[0].Chave,
                            new NfeProc
                            {
                                Versao = EnviNFe.Versao,
                                NFe = EnviNFe.NFe[0],
                                ProtNFe = Result.ProtNFe
                            });
                    }
                }
                else
                {
                    if(RetConsReciNFe == null && RetConsSitNFes.Count <= 0)
                    {
                        throw new Exception("Defina o conteúdo da Propriedade RetConsReciNFe ou RetConsSitNFe, sem a definição de uma delas não é possível obter o conteúdo da NFeProcResults.");
                    }

                    for(var i = 0; i < EnviNFe.NFe.Count; i++)
                    {
                        ProtNFe protNFe = null;

                        if(RetConsReciNFe != null && RetConsReciNFe.ProtNFe != null)
                        {
                            #region Resultado do envio do CT-e através da consulta recibo

                            if(RetConsReciNFe.CStat == 104) //Lote Processado
                            {
                                foreach(var item in RetConsReciNFe.ProtNFe)
                                {
                                    if(item.InfProt.ChNFe == EnviNFe.NFe[i].InfNFe[0].Chave)
                                    {
                                        switch(item.InfProt.CStat)
                                        {
                                            case 100: //Autorizado o uso da NF-e
                                            case 110: //Uso Denegado
                                            case 150: //Autorizado o uso da NF-e, autorização fora de prazo
                                            case 205: //NF-e está denegada na base de dados da SEFAZ [nRec:999999999999999]
                                            case 301: //Uso Denegado: Irregularidade fiscal do emitente
                                            case 302: //Uso Denegado: Irregularidade fiscal do destinatário
                                            case 303: //Uso Denegado: Destinatário não habilitado a operar na UF
                                                protNFe = item;
                                                break;
                                        }

                                        break;
                                    }
                                }
                            }
                            #endregion
                        }
                        else if(RetConsSitNFes.Count > 0)
                        {
                            #region Resultado do envio do NF-e através da consulta situação

                            foreach(var item in RetConsSitNFes)
                            {
                                if(item != null && item.ProtNFe != null)
                                {
                                    if(item.ProtNFe.InfProt.ChNFe == EnviNFe.NFe[i].InfNFe[0].Chave)
                                    {
                                        switch(item.ProtNFe.InfProt.CStat)
                                        {
                                            case 100: //Autorizado o uso da NF-e
                                            case 110: //Uso Denegado
                                            case 150: //Autorizado o uso da NF-e, autorização fora de prazo
                                            case 205: //NF-e está denegada na base de dados da SEFAZ [nRec:999999999999999]
                                            case 301: //Uso Denegado: Irregularidade fiscal do emitente
                                            case 302: //Uso Denegado: Irregularidade fiscal do destinatário
                                            case 303: //Uso Denegado: Destinatário não habilitado a operar na UF
                                                protNFe = item.ProtNFe;
                                                break;
                                        }
                                    }
                                }
                            }

                            #endregion
                        }

                        if(NfeProcs.ContainsKey(EnviNFe.NFe[i].InfNFe[0].Chave))
                        {
                            NfeProcs[EnviNFe.NFe[i].InfNFe[0].Chave].ProtNFe = protNFe;
                        }
                        else
                        {
                            //Se por algum motivo não tiver assinado, só vou forçar atualizar o ConteudoXML para ficar correto na hora de gerar o arquivo de distribuição. Pode estar sem assinar no caso do desenvolvedor estar forçando gerar o XML já autorizado a partir de uma consulta situação da NFe, caso tenha perdido na tentativa do primeiro envio.
                            if(EnviNFe.NFe[i].Signature == null)
                            {
                                ConteudoXML = ConteudoXMLAssinado;
                                AjustarXMLAposAssinado();
                            }

                            NfeProcs.Add(EnviNFe.NFe[i].InfNFe[0].Chave,
                                new NfeProc
                                {
                                    Versao = EnviNFe.Versao,
                                    NFe = EnviNFe.NFe[i],
                                    ProtNFe = protNFe
                                });
                        }
                    }
                }

                return NfeProcs;
            }
        }

        /// <summary>
        /// Propriedade contendo o XML da NFe com o protocolo de autorização anexado - Funciona somente para envio síncrono
        /// </summary>
        public NfeProc NfeProcResult
        {
            get
            {
                if(EnviNFe.IndSinc == SimNao.Sim) //Envio síncrono
                {
                    return new NfeProc
                    {
                        Versao = EnviNFe.Versao,
                        NFe = EnviNFe.NFe[0],
                        ProtNFe = Result.ProtNFe
                    };
                }
                else
                {
                    throw new Exception("Para envio assíncrono utilize a propriedade NFeProcResults para obter os XMLs com o protocolo anexado.");
                }
            }
        }

        public RetEnviNFe Result
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(RetornoWSString))
                {
                    return XMLUtility.Deserializar<RetEnviNFe>(RetornoWSXML);
                }

                return new RetEnviNFe
                {
                    CStat = 0,
                    XMotivo = "Ocorreu uma falha ao tentar criar o objeto a partir do XML retornado da SEFAZ."
                };
            }
        }

        #endregion Public Properties

        #region Public Constructors

        public Autorizacao()
            : base()
        {
        }

        public Autorizacao(EnviNFe enviNFe, Configuracao configuracao)
            : base(enviNFe?.GerarXML() ?? throw new ArgumentNullException(nameof(enviNFe)), configuracao) => Inicializar();

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
                if(EnviNFe == null)
                {
                    throw new NullReferenceException($"{nameof(EnviNFe)} não pode ser nulo.");
                }

                DefinirConfiguracao();
            }

            base.Executar();
        }

        [ComVisible(true)]
        public void Executar(EnviNFe enviNFe, Configuracao configuracao)
        {
            PrepararServico(enviNFe?.GerarXML() ?? throw new ArgumentNullException(nameof(enviNFe)), configuracao);
            Executar();
        }

        /// <summary>
        /// Gravar o XML de distribuição em uma pasta no HD
        /// </summary>
        /// <param name="pasta">Pasta onde deve ser gravado o XML</param>
        public void GravarXmlDistribuicao(string pasta)
        {
            foreach(var item in NfeProcResults)
            {
                if(item.Value.ProtNFe != null)
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
            foreach(var item in NfeProcResults)
            {
                if(item.Value.ProtNFe != null)
                {
                    GravarXmlDistribuicao(stream, item.Value.GerarXML().OuterXml);
                }
            }
        }

        #endregion Public Methods
    }
}