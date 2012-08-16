using System;
using System.Collections.Generic;
using System.Text;
using NFe.Components;
using NFe.Settings;

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
        #endregion

        #region Construtores
        public ParametroThread(Servicos servico, string arquivo)
        {
            Servico = servico;
            Arquivo = arquivo;
        }
        #endregion
    }
    #endregion

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
    #endregion

    /// <summary>
    /// danasa 21/10/2010
    /// </summary>
    /// 
    /*
    public class URLws
    {
        public string NFeRecepcao { get; set; }
        public string NFeRetRecepcao { get; set; }
        public string NFeCancelamento { get; set; }
        public string NFeInutilizacao { get; set; }
        /// <summary>
        /// Consulta da Situação da NFe versão 2.0
        /// </summary>
        public string NFeConsulta { get; set; }
        /// <summary>
        /// Consulta Situação da NFe na versão 1.10
        /// </summary>
        public string NFeConsulta1 { get; set; }
        public string NFeStatusServico { get; set; }
        public string NFeConsultaCadastro { get; set; }
        public string NFeCCe { get; set; }
        /// <summary>
        /// Enviar Lote RPS NFS-e 
        /// </summary>
        public string RecepcionarLoteRps { get; set; }
        /// <summary>
        /// Consultar Situação do lote RPS NFS-e
        /// </summary>
        public string ConsultarSituacaoLoteRps { get; set; }
        /// <summary>
        /// Consultar NFS-e por RPS
        /// </summary>
        public string ConsultarNfsePorRps { get; set; }
        /// <summary>
        /// Consultar NFS-e por NFS-e
        /// </summary>
        public string ConsultarNfse { get; set; }
        /// <summary>
        /// Consultar lote RPS
        /// </summary>
        public string ConsultarLoteRps { get; set; }
        /// <summary>
        /// Cancelar NFS-e
        /// </summary>
        public string CancelarNfse { get; set; }
    }

    public class webServices
    {
        public int ID { get; private set; }
        public string Nome { get; private set; }
        public string UF { get; private set; }
        public URLws URLHomologacao { get; private set; }
        public URLws URLProducao { get; private set; }
        public URLws LocalHomologacao { get; private set; }
        public URLws LocalProducao { get; private set; }

        public webServices(int id, string nome, string uf)
        {
            URLHomologacao = new URLws();
            URLProducao = new URLws();
            LocalHomologacao = new URLws();
            LocalProducao = new URLws();
            ID = id;
            Nome = nome;
            UF = uf;
        }
    }
*/
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
            this.tpAmb = Propriedade.TipoAmbiente.taProducao;// "1";
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
                }
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
        public int tpAmb { get; private set; }
    }
    #endregion

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
        /// Tipo de emissão 1-Normal 2-Contigência em papel de segurança 3-Contigência SCAN
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
    }
    #endregion

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
        /// Tipo de Emissão: 1-Normal 2-Contingência FS 3-Contingência SCAN 4-Contingência DEPEC 5-Contingência FS-DA
        /// </summary>
        public int tpEmis { get; set; }
        /// <summary>
        /// Código da Unidade Federativa (UF)
        /// </summary>
        public int cUF { get; set; }
    }
    #endregion

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
    }
    #endregion

    #region Classe com os dados do XML da consulta do pedido de cancelamento
    /// <summary>
    /// Classe com os dados do XML da consulta do pedido de cancelamento
    /// </summary>
    public class DadosPedCanc
    {
        private string mchNFe;

        public int tpAmb { get; set; }
        public int tpEmis { get; set; }
        public int cUF { get; private set; }
        public string chNFe
        {
            get
            {
                return this.mchNFe;
            }
            set
            {
                this.mchNFe = value;
                string serie = this.mchNFe.Substring(22, 3);

                if (Propriedade.TipoAplicativo == TipoAplicativo.Cte)
                {
                    tpEmis = Convert.ToInt32(this.mchNFe.Substring(34, 1));
                }
                else
                    this.tpEmis = (Convert.ToInt32(serie) >= 900 ? Propriedade.TipoEmissao.teSCAN : this.tpEmis);

                this.cUF = Convert.ToInt32(this.mchNFe.Substring(0, 2));
            }
        }
        public string nProt { get; set; }
        public string xJust { get; set; }

        public DadosPedCanc(int emp)
        {
            //int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
            this.tpEmis = Empresa.Configuracoes[emp].tpEmis;
        }
    }
    #endregion

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
                this.tpEmis = (value >= 900 ? Propriedade.TipoEmissao.teSCAN : this.tpEmis);
            }
        }
        public int nNFIni { get; set; }
        public int nNFFin { get; set; }
        public string xJust { get; set; }

        public DadosPedInut(int emp)
        {
            //int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
            this.tpEmis = Empresa.Configuracoes[emp].tpEmis;
        }
    }
    #endregion

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
                    this.cUF = Convert.ToInt32(this.mchNFe.Substring(0, 2));
                    int serie = Convert.ToInt32(this.mchNFe.Substring(22, 3));
                    if (Propriedade.TipoAplicativo == TipoAplicativo.Cte)
                    {
                        tpEmis = Convert.ToInt32(this.mchNFe.Substring(34, 1));
                    }
                    else
                        this.tpEmis = (serie >= 900 ? Propriedade.TipoEmissao.teSCAN : this.tpEmis);
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
        public int versaoNFe { get; set; }

        public DadosPedSit()
        {
            this.cUF = 0;
            //this.serie = string.Empty;
            this.tpEmis = Propriedade.TipoEmissao.teNormal;// ConfiguracaoApp.tpEmis;
        }
    }
    #endregion

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
        /// Tipo de Emissao (1-Normal, 2-Contingencia, 3-SCAN, ...
        /// </summary>
        public int tpEmis { get; set; }
    }
    #endregion

    #region Classe com os dados do XML de registro do DPEC
    /// <summary>
    /// Classe com os dados do XML de registro do DPEC
    /// </summary>
    public class DadosEnvDPEC
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
        /// Tipo de Emissao (1-Normal, 2-Contingencia, 3-SCAN, ...
        /// </summary>
        public int tpEmis { get; set; }

        public string CNPJ { get; set; }
        public string IE { get; set; }
        public string verProc { get; set; }
        public string chNFe { get; set; }
        public string CNPJCPF { get; set; }
        public string UF { get; set; }
        public string vNF { get; set; }
        public string vICMS { get; set; }
        public string vST { get; set; }
    }
    #endregion

    #region Classe com os dados do XML de consulta do registro do DPEC
    /// <summary>
    /// Classe com os dados do XML de registro do DPEC
    /// </summary>
    public class DadosConsDPEC
    {
        /// <summary>
        /// Ambiente (2-Homologação ou 1-Produção)
        /// </summary>
        public int tpAmb { get; set; }
        /// <summary>
        /// Código da Unidade Federativa (UF)
        /// </summary>
        //public int cUF { get; set; }
        /// <summary>
        /// Tipo de Emissao (1-Normal, 2-Contingencia, 3-SCAN, ...
        /// </summary>
        public int tpEmis { get; set; }

        public string chNFe { get; set; }
        public string nRegDPEC { get; set; }
        public string verAplic { get; set; }
    }
    #endregion

    #region Classe com os dados do XML do registro de eventos
    public class Evento
    {
        public string versao { get; set; }
        public string Id { get; set; }
        public int cOrgao { get; set; }
        public int tpAmb { get; set; }
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

        public Evento()
        {
            verEvento = "1.00";
            versao = "1.00";
            tpEvento = "110110";
        }
    }

    public class DadosenvEvento
    {
        public string versao { get; set; }
        public string idLote { get; set; }
        public List<Evento> eventos { get; private set; }

        public DadosenvEvento()
        {
            versao = "1.00";
            eventos = new List<Evento>();
        }
    }
    #endregion

    #region Classe com os dados do XML do registro de download de nfe
    public class DadosenvDownload
    {
        public int tpAmb { get; set; }
        public string chNFe { get; set; }
        public string CNPJ { get; set; }
    }
    #endregion

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
    #endregion


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
            tpEmis = Empresa.Configuracoes[emp].tpEmis;
            tpAmb = Empresa.Configuracoes[emp].tpAmb;
            cMunicipio = Empresa.Configuracoes[emp].UFCod;
        }
    }
    #endregion

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
            tpEmis = Empresa.Configuracoes[emp].tpEmis;
            tpAmb = Empresa.Configuracoes[emp].tpAmb;
            cMunicipio = Empresa.Configuracoes[emp].UFCod;
        }
    }
    #endregion

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
            tpEmis = Empresa.Configuracoes[emp].tpEmis;
            tpAmb = Empresa.Configuracoes[emp].tpAmb;
            cMunicipio = Empresa.Configuracoes[emp].UFCod;
        }
    }
    #endregion

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
            tpEmis = Empresa.Configuracoes[emp].tpEmis;
            tpAmb = Empresa.Configuracoes[emp].tpAmb;
            cMunicipio = Empresa.Configuracoes[emp].UFCod;
        }
    }
    #endregion

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
            tpEmis = Empresa.Configuracoes[emp].tpEmis;
            tpAmb = Empresa.Configuracoes[emp].tpAmb;
            cMunicipio = Empresa.Configuracoes[emp].UFCod;
        }
    }
    #endregion

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
            tpEmis = Empresa.Configuracoes[emp].tpEmis;
            tpAmb = Empresa.Configuracoes[emp].tpAmb;
            cMunicipio = Empresa.Configuracoes[emp].UFCod;
        }
    }
    #endregion

    #endregion

}
