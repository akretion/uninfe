using NFe.Components;
using NFe.Settings;
using System;
using System.Collections.Generic;

namespace NFe.Service
{
    #region Classe ParametroThread

    /// <summary>
    /// Classe para auxiliar na execução de várias thread´s com parâmetros
    /// </summary>
    public class ParametroThread
    {
        #region Propriedades

        /// <summary>
        /// Serviço que será executado
        /// </summary>
        public Servicos Servico { get; private set; }

        /// <summary>
        /// Arquivo que é para ser destinado/enviado/analisado
        /// </summary>
        public string Arquivo { get; private set; }

        #endregion Propriedades

        #region Construtores

        public ParametroThread(Servicos servico, string arquivo)
        {
            Servico = servico;
            Arquivo = arquivo;
        }

        #endregion Construtores
    }

    #endregion Classe ParametroThread

    #region infCad & RetConsCad

    public class enderConsCadInf
    {
        public string xLgr { get; set; }
        public string nro { get; set; }
        public string xCpl { get; set; }
        public string xBairro { get; set; }
        public int cMun { get; set; }
        public string xMun { get; set; }
        public int CEP { get; set; }
    }

    public class infCad
    {
        public string IE { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string UF { get; set; }
        public string xNome { get; set; }
        public string xFant { get; set; }
        public string IEAtual { get; set; }
        public string IEUnica { get; set; }
        public DateTime dBaixa { get; set; }
        public DateTime dUltSit { get; set; }
        public DateTime dIniAtiv { get; set; }
        public int CNAE { get; set; }
        public string xRegApur { get; set; }
        public string cSit { get; set; }
        public enderConsCadInf ender { get; set; }

        public infCad()
        {
            ender = new enderConsCadInf();
        }
    }

    public class RetConsCad
    {
        public int cStat { get; set; }
        public string xMotivo { get; set; }
        public string UF { get; set; }
        public string IE { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public DateTime dhCons { get; set; }
        public Int32 cUF { get; set; }
        public List<infCad> infCad { get; set; }

        public RetConsCad()
        {
            infCad = new List<infCad>();
        }
    }

    #endregion infCad & RetConsCad

    /// <summary>
    /// Classe utilizada para permitir o lock na hora de gravar os arquivos de fluxo da NFe
    /// </summary>
    public class FluxoNfeLock
    {
    }

    #region Classe com os Dados do XML da Consulta Cadastro do Contribuinte

    public class DadosConsCad
    {
        private string mUF;

        public DadosConsCad()
        {
            this.tpAmb = (int)NFe.Components.TipoAmbiente.taProducao;// "1";
        }

        /// <summary>
        /// Unidade Federativa (UF) - Sigla
        /// </summary>
        public string UF
        {
            get
            {
                return this.mUF;
            }
            set
            {
                this.mUF = value;
                this.cUF = 0;// string.Empty;

                this.cUF = Functions.UFParaCodigo(value.Trim());
                /*
                switch (this.mUF.ToUpper().Trim())
                {
                    case "AC":
                        this.cUF = 12;
                        break;

                    case "AL":
                        this.cUF = 27;
                        break;

                    case "AP":
                        this.cUF = 16;
                        break;

                    case "AM":
                        this.cUF = 13;
                        break;

                    case "BA":
                        this.cUF = 29;
                        break;

                    case "CE":
                        this.cUF = 23;
                        break;

                    case "DF":
                        this.cUF = 53;
                        break;

                    case "ES":
                        this.cUF = 32;
                        break;

                    case "GO":
                        this.cUF = 52;
                        break;

                    case "MA":
                        this.cUF = 21;
                        break;

                    case "MG":
                        this.cUF = 31;
                        break;

                    case "MS":
                        this.cUF = 50;
                        break;

                    case "MT":
                        this.cUF = 51;
                        break;

                    case "PA":
                        this.cUF = 15;
                        break;

                    case "PB":
                        this.cUF = 25;
                        break;

                    case "PE":
                        this.cUF = 26;
                        break;

                    case "PI":
                        this.cUF = 22;
                        break;

                    case "PR":
                        this.cUF = 41;
                        break;

                    case "RJ":
                        this.cUF = 33;
                        break;

                    case "RN":
                        this.cUF = 24;
                        break;

                    case "RO":
                        this.cUF = 11;
                        break;

                    case "RR":
                        this.cUF = 14;
                        break;

                    case "RS":
                        this.cUF = 43;
                        break;

                    case "SC":
                        this.cUF = 42;
                        break;

                    case "SE":
                        this.cUF = 28;
                        break;

                    case "SP":
                        this.cUF = 35;
                        break;

                    case "TO":
                        this.cUF = 17;
                        break;
                }*/
            }
        }

        /// <summary>
        /// CPF
        /// </summary>
        public string CPF { get; set; }

        /// <summary>
        /// CNPJ
        /// </summary>
        public string CNPJ { get; set; }

        /// <summary>
        /// Inscrição Estadual
        /// </summary>
        public string IE { get; set; }

        /// <summary>
        /// Unidade Federativa (UF) - Código
        /// </summary>
        public int cUF { get; private set; }

        /// <summary>
        /// Ambiente (2-Homologação 1-Produção)
        /// </summary>
        public int tpAmb { get; set; }

        /// <summary>
        /// Versao (2.00 ou 3.10)
        /// </summary>
        public string versao { get; set; }
    }

    #endregion Classe com os Dados do XML da Consulta Cadastro do Contribuinte

    #region Classe com os dados do XML da NFe

    /// <summary>
    /// Esta classe possui as propriedades que vai receber o conteúdo
    /// do XML da nota fiscal eletrônica
    /// </summary>
    public class DadosNFeClass
    {
        /// <summary>
        /// Chave da nota fisca
        /// </summary>
        public string chavenfe { get; set; }

        /// <summary>
        /// Data de emissão
        /// </summary>
        public DateTime dEmi { get; set; }

        /// <summary>
        /// Tipo de emissão 1-Normal 2-Contigência em papel de segurança 6/7/8-Contigência SVC/AN/RS/SP
        /// </summary>
        public string tpEmis { get; set; }

        /// <summary>
        /// Tipo de Ambiente 1-Produção 2-Homologação
        /// </summary>
        public string tpAmb { get; set; }

        /// <summary>
        /// Lote que a NFe faz parte
        /// </summary>
        public string idLote { get; set; }

        /// <summary>
        /// Série da NFe
        /// </summary>
        public string serie { get; set; }

        /// <summary>
        /// UF do Emitente
        /// </summary>
        public string cUF { get; set; }

        /// <summary>
        /// Número randomico da chave da nfe
        /// </summary>
        public string cNF { get; set; }

        /// <summary>
        /// Modelo da nota fiscal
        /// </summary>
        public string mod { get; set; }

        /// <summary>
        /// Número da nota fiscal
        /// </summary>
        public string nNF { get; set; }

        /// <summary>
        /// Dígito verificador da chave da nfe
        /// </summary>
        public string cDV { get; set; }

        /// <summary>
        /// CNPJ do emitente
        /// </summary>
        public string CNPJ { get; set; }

        /// <summary>
        /// CPF do emitente
        /// </summary>
        public string CPF { get; set; }

        /// <summary>
        /// Versão do XML
        /// </summary>
        public string versao { get; set; }

        /// <summary>
        /// Enviar nota no modo síncrono? true/false
        /// </summary>
        public bool indSinc { get; set; }

        /// <summary>
        /// Data e hora da entrada em contingência
        /// </summary>
        public string dhCont { get; set; }

        /// <summary>
        /// Justificativa para a entrada em contingência
        /// </summary>
        public string xJust { get; set; }
    }

    #endregion Classe com os dados do XML da NFe

    #region Classe com os dados do XML do pedido de consulta do recibo do lote de nfe enviado

    /// <summary>
    /// Classe com os dados do XML do pedido de consulta do recibo do lote de nfe enviado
    /// </summary>
    public class DadosPedRecClass
    {
        /// <summary>
        /// Tipo de ambiente: 1-Produção 2-Homologação
        /// </summary>
        public int tpAmb { get; set; }

        /// <summary>
        /// Número do recibo do lote de NFe enviado
        /// </summary>
        public string nRec { get; set; }

        /// <summary>
        /// Tipo de Emissão: 1-Normal 2-Contingência FS 6/7/8-Contingência SVC/AN/RS/SP 4-Contingência DEPEC 5-Contingência FS-DA
        /// </summary>
        public int tpEmis { get; set; }

        /// <summary>
        /// Código da Unidade Federativa (UF)
        /// </summary>
        public int cUF { get; set; }

        /// <summary>
        /// Versão do XML
        /// </summary>
        public string versao { get; set; }

        /// <summary>
        /// Modelo do documento fiscal
        /// </summary>
        public string mod { get; set; }
    }

    #endregion Classe com os dados do XML do pedido de consulta do recibo do lote de nfe enviado

    #region Classe com os dados do XML do retorno do envio do Lote de NFe

    /// <summary>
    /// Esta classe possui as propriedades que vai receber o conteúdo do XML do recibo do lote
    /// </summary>
    public class DadosRecClass
    {
        /// <summary>
        /// Recibo do lote de notas fiscais enviado
        /// </summary>
        public string nRec { get; set; }

        /// <summary>
        /// Status do Lote
        /// </summary>
        public string cStat { get; set; }

        /// <summary>
        /// Tempo médio de resposta em segundos
        /// </summary>
        public int tMed { get; set; }

        /// <summary>
        /// Versão do XML
        /// </summary>
        public string versao { get; set; }
    }

    #endregion Classe com os dados do XML do retorno do envio do Lote de NFe

    #region Classe com os dados do XML do pedido de inutilização de números de NF

    /// <summary>
    /// Classe com os dados do XML do pedido de inutilização de números de NF
    /// </summary>
    public class DadosPedInut
    {
        private int mSerie;
        public int tpAmb { get; set; }
        public int tpEmis { get; set; }
        public int cUF { get; set; }
        public int ano { get; set; }
        public string CNPJ { get; set; }
        public int mod { get; set; }

        public int serie
        {
            get
            {
                return this.mSerie;
            }
            set
            {
                this.mSerie = value;
            }
        }

        public int nNFIni { get; set; }
        public int nNFFin { get; set; }
        public string xJust { get; set; }
        public string versao { get; set; }

        public DadosPedInut(int emp)
        {
            this.tpEmis = Empresas.Configuracoes[emp].tpEmis;
        }
    }

    #endregion Classe com os dados do XML do pedido de inutilização de números de NF

    #region Classe com os dados do XML da pedido de consulta da situação da NFe

    /// <summary>
    /// Classe com os dados do XML da pedido de consulta da situação da NFe
    /// </summary>
    public class DadosPedSit
    {
        private string mchNFe;

        /// <summary>
        /// Ambiente (2-Homologação ou 1-Produção)
        /// </summary>
        public int tpAmb { get; set; }

        /// <summary>
        /// Chave do documento fiscal
        /// </summary>
        public string chNFe
        {
            get
            {
                return this.mchNFe;
            }
            set
            {
                this.mchNFe = value;
                if (this.mchNFe != string.Empty)
                {
                    cUF = Convert.ToInt32(this.mchNFe.Substring(0, 2));
                    int serie = Convert.ToInt32(this.mchNFe.Substring(22, 3));
                }
            }
        }

        /// <summary>
        /// Código da Unidade Federativa (UF)
        /// </summary>
        public int cUF { get; private set; }

        /// <summary>
        /// Série da NFe que está sendo consultada a situação
        /// </summary>
        //            public string serie { get; private set; }
        /// <summary>
        /// Tipo de emissão para saber para onde será enviado a consulta da situação da nota
        /// </summary>
        public int tpEmis { get; set; }

        public string versao { get; set; }

        /// <summary>
        /// Modelo do documento fiscal
        /// </summary>
        public string mod { get; set; }

        public DadosPedSit()
        {
            this.cUF = 0;
            this.tpEmis = (int)NFe.Components.TipoEmissao.teNormal;
        }
    }

    #endregion Classe com os dados do XML da pedido de consulta da situação da NFe

    #region Classe com os dados do XML da consulta do status do serviço da NFe

    /// <summary>
    /// Classe com os dados do XML da consulta do status do serviço da NFe
    /// </summary>
    public class DadosPedSta
    {
        /// <summary>
        /// Ambiente (2-Homologação ou 1-Produção)
        /// </summary>
        public int tpAmb { get; set; }

        /// <summary>
        /// Código da Unidade Federativa (UF)
        /// </summary>
        public int cUF { get; set; }

        /// <summary>
        /// Tipo de Emissao (1-Normal, 2-Contingencia, 6/7/8-SVC/AN/RS/SP, ...
        /// </summary>
        public int tpEmis { get; set; }

        /// <summary>
        /// Versão do XML
        /// </summary>
        public string versao { get; set; }

        /// <summary>
        /// Modelo do documento fiscal que é para consultar o status do serviço
        /// </summary>
        public string mod { get; set; }
    }

    #endregion Classe com os dados do XML da consulta do status do serviço da NFe

    #region Classe com os dados do XML do registro de eventos

    public class Evento
    {
        public string versao { get; set; }
        public string Id { get; set; }
        public int cOrgao { get; set; }
        public int tpAmb { get; set; }
        public int tpEmis { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string chNFe { get; set; }
        public string dhEvento { get; set; }
        public string tpEvento { get; set; }
        public int nSeqEvento { get; set; }
        public string verEvento { get; set; }
        public string descEvento { get; set; }

        // evento de carta de correcao
        public string xCorrecao { get; set; }

        public string xCondUso { get; set; }

        // Cancelamento de NFe como Evento
        public string nProt { get; set; }

        /// Cancelamento de NFe como Evento e Manifestação do Destinatário
        public string xJust { get; set; }
        
        /// <summary>
        /// EPEC
        /// </summary>
        public EventoEPEC epec { get; set; }

        public string mod
        {
            get { return chNFe.Substring(20, 2); }
        }

        /// <summary>
        /// Prorrogacao de ICMS
        /// </summary>
        public List<ProrrogacaoICMS> prorrogacaoICMS { get; set; }

        /// <summary>
        /// Cancelamento/Prorrogacao de ICMS
        /// </summary>
        public string idPedidoCancelado { get; set; }

        public string idPedido { get; set; }
        public RespPedido respPedido { get; set; }
        public RespCancPedido respCancPedido { get; set; }

        /// <summary>
        /// Cancelamento por subistituição da NFCe
        /// </summary>
        public EventoCancelamentoSubstituicao cancelamentoSubstituicao { get; set; }

        public Evento()
        {
            epec = new EventoEPEC();
            prorrogacaoICMS = new List<ProrrogacaoICMS>();
            respPedido = new RespPedido();
            respCancPedido = new RespCancPedido();
            verEvento = "1.00";
            versao = "1.00";
            tpEvento = "110110";
            tpEmis = 0;
            cancelamentoSubstituicao = new EventoCancelamentoSubstituicao();
        }
    }

    public class RespPedido
    {
        public string statPrazo { get; set; }

        public List<ItemPedido> itemPedido { get; set; }

        public RespPedido()
        {
            itemPedido = new List<ItemPedido>();
        }
    }

    public class ItemPedido
    {
        public Int32 numItem { get; set; }
        public Int32 statPedido { get; set; }
        public Int32 justStatus { get; set; }
        public string justStaOutra { get; set; }
    }

    public class RespCancPedido
    {
        public Int32 statCancPedido { get; set; }
        public Int32 justStatus { get; set; }
        public string justStaOutra { get; set; }
    }

    public class ProrrogacaoICMS
    {
        public string numItem { get; set; }
        public string qtdeItem { get; set; }
    }

    public class EventoEPEC
    {
        public Int32 cOrgaoAutor { get; set; }
        public NFe.ConvertTxt.TpcnTipoAutor tpAutor { get; set; }
        public string verAplic { get; set; }
        public string dhEmi { get; set; }
        public NFe.ConvertTxt.TpcnTipoNFe tpNF { get; set; }
        public string IE { get; set; }

        public EventoDestinatario dest { get; set; }

        public EventoEPEC()
        {
            dest = new EventoDestinatario();
            IE = verAplic = dhEmi = string.Empty;
        }
    }

    public class EventoDestinatario
    {
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string idEstrangeiro { get; set; }
        public string IE { get; set; }
        public string UF { get; set; }
        public double vNF { get; set; }
        public double vICMS { get; set; }
        public double vST { get; set; }

        public EventoDestinatario()
        {
            this.CNPJ = this.CPF = this.idEstrangeiro = this.IE = this.UF = string.Empty;
        }
    }

    public class DadosenvEvento
    {
        public string versao { get; set; }
        public string idLote { get; set; }
        public List<Evento> eventos { get; set; }

        public DadosenvEvento()
        {
            versao = "1.00";
            eventos = new List<Evento>();
        }
    }

    public class EventoCancelamentoSubstituicao : EventoEPEC
    {
        public string chNFeRef { get; set; }
    }

    #endregion Classe com os dados do XML do registro de eventos

    #region Classe com os dados do XML do registro de download de nfe

    public class DadosenvDownload
    {
        public int tpAmb { get; set; }
        public string chNFe { get; set; }
        public string CNPJ { get; set; }
    }

    #endregion Classe com os dados do XML do registro de download de nfe

    #region Classe com os dados do XML do registro de consulta de nfe de destinatario

    public class DadosConsultaNFeDest
    {
        public int tpAmb { get; set; }
        public string xServ { get; set; }
        public string CNPJ { get; set; }
        public int indNFe { get; set; }
        public int indEmi { get; set; }
        public string ultNSU { get; set; }
    }

    #endregion Classe com os dados do XML do registro de consulta de nfe de destinatario

    #region Classe para receber os dados dos XML´s da NFS-e

    public class Municipio
    {
        public int CodigoMunicipio { get; set; }
        public string UF { get; set; }
        public string Nome { get; set; }
        public string PadraoStr { get; set; }   //usado para manutencao
        public PadroesNFSe Padrao { get; set; }

        public Municipio(int _cod, string _uf, string _nome, PadroesNFSe _padrao)
        {
            this.Nome = _nome;
            if (!_nome.Trim().EndsWith(" - " + _uf))
                this.Nome = _nome.Trim() + " - " + _uf;
            this.CodigoMunicipio = _cod;
            this.Padrao = _padrao;
            this.PadraoStr = _padrao.ToString();
            this.UF = _uf;
        }
    }

    #region DadosPedLoteRps

    /// <summary>
    /// Classe com os dados do XML da consulta do lote de rps
    /// </summary>
    public class DadosPedLoteRps
    {
        public int cMunicipio { get; set; }
        public int tpAmb { get; set; }
        public int tpEmis { get; set; }
        public string Protocolo { get; set; }
        public string Cnpj { get; set; }
        public string InscricaoMunicipal { get; set; }

        public DadosPedLoteRps(int emp)
        {
            tpEmis = Empresas.Configuracoes[emp].tpEmis;
            tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            cMunicipio = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
        }
    }

    #endregion DadosPedLoteRps

    #region DadosPedSitNfse

    /// <summary>
    /// Classe com os dados do XML da consulta da nfse por numero da nfse
    /// </summary>
    public class DadosPedSitNfse
    {
        public int cMunicipio { get; set; }
        public int tpAmb { get; set; }
        public int tpEmis { get; set; }

        public DadosPedSitNfse(int emp)
        {
            tpEmis = Empresas.Configuracoes[emp].tpEmis;
            tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            cMunicipio = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
        }
    }

    #endregion DadosPedSitNfse

    #region DadosPedStaNfse
    /// <summary>
    /// Classe com os dados do XML da consulta de status da nfse por numero da nfse
    /// </summary>
    public class DadosPedStaNfse
    {
        public int cMunicipio { get; set; }
        public int tpAmb { get; set; }
        public int tpEmis { get; set; }

        public DadosPedStaNfse(int emp)
        {
            tpEmis = Empresas.Configuracoes[emp].tpEmis;
            tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            cMunicipio = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
        }
    }

    #endregion DadosPedStaNfse

    #region DadosPedSitNfseRps

    /// <summary>
    /// Classe com os dados do XML da consulta da nfse por rps
    /// </summary>
    public class DadosPedSitNfseRps
    {
        public int cMunicipio { get; set; }
        public int tpAmb { get; set; }
        public int tpEmis { get; set; }

        public DadosPedSitNfseRps(int emp)
        {
            tpEmis = Empresas.Configuracoes[emp].tpEmis;
            tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            cMunicipio = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
        }
    }

    #endregion DadosPedSitNfseRps

    #region Classe com os dados do XML da consulta do lote de rps

    /// <summary>
    /// Classe com os dados do XML da consulta do lote de rps
    /// </summary>
    public class DadosPedCanNfse
    {
        public int cMunicipio { get; set; }
        public int tpAmb { get; set; }
        public int tpEmis { get; set; }

        public DadosPedCanNfse(int emp)
        {
            tpEmis = Empresas.Configuracoes[emp].tpEmis;
            tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            cMunicipio = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
        }
    }

    #endregion Classe com os dados do XML da consulta do lote de rps

    #region Classe com os dados do XML da consulta situacao do lote de rps

    /// <summary>
    /// Classe com os dados do XML da consulta do lote de rps
    /// </summary>
    public class DadosPedSitLoteRps
    {
        public int cMunicipio { get; set; }
        public int tpAmb { get; set; }
        public int tpEmis { get; set; }

        public DadosPedSitLoteRps(int emp)
        {
            tpEmis = Empresas.Configuracoes[emp].tpEmis;
            tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            cMunicipio = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
        }
    }

    #endregion Classe com os dados do XML da consulta situacao do lote de rps

    #region Classe com os dados do XML do Lote RPS

    /// <summary>
    /// Classe com os dados do XML do Lote RPS
    /// </summary>
    public class DadosEnvLoteRps
    {
        public int cMunicipio { get; set; }
        public int tpAmb { get; set; }
        public int tpEmis { get; set; }

        public DadosEnvLoteRps(int emp)
        {
            tpEmis = Empresas.Configuracoes[emp].tpEmis;
            tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            cMunicipio = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
        }
    }

    #endregion Classe com os dados do XML do Lote RPS

    #region DadosPedURLNfse

    /// <summary>
    /// Classe com os dados do XML da consulta da URL da Nfse
    /// </summary>
    public class DadosPedURLNfse
    {
        public int cMunicipio { get; set; }
        public int tpAmb { get; set; }
        public int tpEmis { get; set; }

        public DadosPedURLNfse(int emp)
        {
            tpEmis = Empresas.Configuracoes[emp].tpEmis;
            tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            cMunicipio = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
        }
    }

    #endregion DadosPedURLNfse

    #region DadosPedSeqLoteNotaRPS

    /// <summary>
    /// Classe com os dados do XML da consulta sequencia do lote da nota RPS
    /// </summary>
    public class DadosPedSeqLoteNotaRPS
    {
        public int cMunicipio { get; set; }
        public int tpAmb { get; set; }
        public int tpEmis { get; set; }

        public DadosPedSeqLoteNotaRPS(int emp)
        {
            tpEmis = Empresas.Configuracoes[emp].tpEmis;
            tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            cMunicipio = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
        }
    }

    #endregion DadosPedSeqLoteNotaRPS

    #endregion Classe para receber os dados dos XML´s da NFS-e

    #region Classe para receber dados do XML de Distribuição do DFe

    public class distDFeInt
    {
        public string versao { get; set; }
        public int tpAmb { get; set; }
        public string cUFAutor { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string ultNSU { get; set; }
        public string NSU { get; set; }
    }

    #endregion Classe para receber dados do XML de Distribuição do DFe

    #region Classe para receber dados do XML de LMC

    /// <summary>
    /// Classe com os dados do XML da consulta do status do serviço da NFe
    /// </summary>
    public class DadosLMC
    {
        /// <summary>
        /// Ambiente (2-Homologação ou 1-Produção)
        /// </summary>
        public int tpAmb { get; set; }

        /// <summary>
        /// Versão do XML
        /// </summary>
        public string versao { get; set; }

        /// <summary>
        /// Id do LMC
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Data de emissão do LMC
        /// </summary>
        public DateTime dEmissao { get; set; }

        /// <summary>
        /// Código da UF do emitente
        /// </summary>
        public int cUF { get; set; }
    }

    #endregion Classe para receber dados do XML de LMC
}