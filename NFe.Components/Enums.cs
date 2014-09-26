using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NFe.Components
{
    #region SubPastas da pasta de enviados
    /// <summary>
    /// SubPastas da pasta de XML´s enviados para os webservices
    /// </summary>
    public enum PastaEnviados
    {
        EmProcessamento,
        Autorizados,
        Denegados
    }
    #endregion

    #region Servicos
    /// <summary>
    /// Serviços executados pelo Aplicativo
    /// </summary>
    public enum Servicos
    {
        #region NFe
        /// <summary>
        /// Consulta status serviço NFe
        /// </summary>
        ConsultaStatusServicoNFe,
        /// <summary>
        /// Assinar e montar lote de uma NFe
        /// </summary>
        MontarLoteUmaNFe,
        /// <summary>
        /// Envia os lotes de NFe para os webservices (NfeRecepcao)
        /// </summary>
        EnviarLoteNfe,
        /// <summary>
        /// Envia os lotes de NFe para os webservices (NFeAutorizacao)
        /// </summary>
        EnviarLoteNfe2,
        /// <summary>
        /// Envia os lotes de NFe para os webservices de forma Compactada (NFeAutorizacao)
        /// </summary>
        EnviarLoteNfeZip2,
        /// <summary>
        /// Consulta recibo do lote nfe (NFeRetRecepcao)
        /// </summary>
        PedidoSituacaoLoteNFe,
        /// <summary>
        /// Consulta recibo do lote nfe (NFeRetAutorizacao)
        /// </summary>
        PedidoSituacaoLoteNFe2,
        /// <summary>
        /// Consulta situação da NFe
        /// </summary>
        PedidoConsultaSituacaoNFe,
        /// <summary>
        /// Envia XML de Inutilização da NFe
        /// </summary>
        InutilizarNumerosNFe,
        /// <summary>
        /// Somente converter TXT da NFe para XML de NFe
        /// </summary>
        ConverterTXTparaXML,

        #region Eventos NFe
        /// <summary>
        /// Enviar XML de Evento NFe
        /// </summary>
        RecepcaoEvento,
        /// <summary>
        /// Enviar XML Evento - Carta de Correção
        /// </summary>
        EnviarCCe,
        /// <summary>
        /// Enviar XML Evento - Cancelamento
        /// </summary>
        EnviarEventoCancelamento,
        /// <summary>
        /// Enviar um evento de manifestacao
        /// </summary>
        EnviarManifDest,
        /// <summary>
        /// Enviar um evento de EPEC
        /// </summary>
        EnviarEPEC,
        #endregion

        /// <summary>
        /// Assinar e validar um XML de NFe no envio em Lote
        /// </summary>
        AssinarValidarNFeEnvioEmLote,
        /// <summary>
        /// Assinar e montar lote de várias NFe
        /// </summary>
        MontarLoteVariasNFe,
        /// <summary>
        /// Monta chave de acesso
        /// </summary>
        GerarChaveNFe,
        /// <summary>
        /// Enviar o XML do DPEC para o SCE - Sistema de Contingência Eletrônica
        /// </summary>
        EnviarDPEC,
        /// <summary>
        /// Consultar o registro do DPEC no SCE - Sistema de Contingência Eletrônica        
        /// </summary>
        ConsultarDPEC,
        /// <summary>
        /// Enviar solicitacao de download de nfe
        /// </summary>
        DownloadNFe,
        /// <summary>
        /// Enviar uma consulta de nfe de destinatario
        /// </summary>
        ConsultaNFDest,
        /// <summary>
        /// Registro de saida
        /// </summary>
        //RegistroDeSaida,
        /// <summary>
        /// Registro de saida
        /// </summary>
        //RegistroDeSaidaCancelamento,
        #endregion

        #region CTe
        /// <summary>
        /// Consulta Status Serviço CTe
        /// </summary>
        ConsultaStatusServicoCTe,
        /// <summary>
        /// Montar lote de um CTe
        /// </summary>
        MontarLoteUmCTe,
        /// <summary>
        /// Envia os lotes de CTe para os webservices
        /// </summary>
        EnviarLoteCTe,
        /// <summary>
        /// Consulta recibo do lote CTe
        /// </summary>
        PedidoSituacaoLoteCTe,
        /// <summary>
        /// Consulta situação da CTe
        /// </summary>
        PedidoConsultaSituacaoCTe,
        /// <summary>
        /// Envia XML de Inutilização da CTe
        /// </summary>
        InutilizarNumerosCTe,
        /// <summary>
        /// Enviar XML Evento CTe
        /// </summary>
        RecepcaoEventoCTe,
        /// <summary>
        /// Assinar e validar um XML de CTe no envio em Lote
        /// </summary>
        AssinarValidarCTeEnvioEmLote,
        /// <summary>
        /// Assinar e montar lote de várias CTe
        /// </summary>
        MontarLoteVariosCTe,
        #endregion

        #region NFSe
        /// <summary>
        /// Enviar Lote RPS NFS-e 
        /// </summary>
        [Description("Enviar Lote RPS NFS-e ")]
        RecepcionarLoteRps,
        /// <summary>
        /// Consultar Situação do lote RPS NFS-e
        /// </summary>
        [Description("Consultar Situação do lote RPS NFS-e")]
        ConsultarSituacaoLoteRps,
        /// <summary>
        /// Consultar NFS-e por RPS
        /// </summary>
        [Description("Consultar NFS-e por RPS")]
        ConsultarNfsePorRps,
        /// <summary>
        /// Consultar NFS-e por Data
        /// </summary>
        [Description("Consultar NFS-e por Data")]
        ConsultarNfse,
        /// <summary>
        /// Consultar lote RPS
        /// </summary>
        [Description("ConsultarLoteRPS")]
        ConsultarLoteRps,
        /// <summary>
        /// Cancelar NFS-e
        /// </summary>
        [Description("Cancelar NFS-e")]
        CancelarNfse,
        /// <summary>
        /// Consultar a URL de visualização da NFSe
        /// </summary>
        [Description("Consultar a URL de Visualização da NFS-e")]
        ConsultarURLNfse,
        /// <summary>
        /// Consultar a URL de visualização da NFSe
        /// </summary>
        [Description("Consultar a URL de Visualização da NFS-e com a Série")]
        ConsultarURLNfseSerie,
        #endregion

        #region MDFe
        /// <summary>
        /// Consulta Status Serviço MDFe
        /// </summary>
        ConsultaStatusServicoMDFe,
        /// <summary>
        /// Montar lote de um MDFe
        /// </summary>
        MontarLoteUmMDFe,
        /// <summary>
        /// Envia os lotes de MDFe para os webservices
        /// </summary>
        EnviarLoteMDFe,
        /// <summary>
        /// Consulta recibo do lote MDFe
        /// </summary>
        PedidoSituacaoLoteMDFe,
        /// <summary>
        /// Consulta situação da MDFe
        /// </summary>
        PedidoConsultaSituacaoMDFe,
        /// <summary>
        /// Assinar e validar um XML de MDFe no envio em Lote
        /// </summary>
        AssinarValidarMDFeEnvioEmLote,
        /// <summary>
        /// Assinar e montar lote de várias MDFe
        /// </summary>
        MontarLoteVariosMDFe,
        /// <summary>
        /// Enviar XML Evento MDFe
        /// </summary>
        RecepcaoEventoMDFe,
        #endregion

        #region Serviços em comum NFe, CTe, MDFe e NFSe
        /// <summary>
        /// Valida e envia o XML de pedido de Consulta do Cadastro do Contribuinte para o webservice
        /// </summary>
        ConsultaCadastroContribuinte,
        /// <summary>
        /// Efetua verificações nas notas em processamento para evitar algumas falhas e perder retornos de autorização de notas
        /// </summary>
        EmProcessamento,
        /// <summary>
        /// Somente assinar e validar o XML
        /// </summary>
        AssinarValidar,
        #endregion

        #region Serviços gerais
        /// <summary>
        /// Consultar Informações Gerais do UniNFe
        /// </summary>
        ConsultaInformacoesUniNFe,
        /// <summary>
        /// Solicitar ao UniNFe que altere suas configurações
        /// </summary>
        AlterarConfiguracoesUniNFe,
        /// <summary>
        /// Efetua uma limpeza das pastas que recebem arquivos temporários
        /// </summary>
        LimpezaTemporario,
        /// <summary>
        /// Consultas efetuadas pela pasta GERAL.
        /// </summary>
        ConsultaGeral,
        /// <summary>
        /// Consulta Certificados Instalados na estação do UniNFe.
        /// </summary>
        ConsultaCertificados,
        #endregion

        #region Não sei para que serve - Wandrey
        /// <summary>
        /// WSExiste
        /// </summary>
        WSExiste,
        #endregion

        #region Impressao do DANFE
        ImpressaoNFe,
        ImpressaoNFe_Contingencia,
        #endregion

        #region Impressao do relatorio de e-mails do DANFE
        DanfeRelatorio,
        #endregion

        EnviarDFe,

        /// <summary>
        /// Nulo / Nenhum serviço em execução
        /// </summary>        
        Nulo
    }
    #endregion

    #region TipoAplicativo
    public enum TipoAplicativo
    {
        /// <summary>
        /// Aplicativo ou serviços para processamento dos XMLs da NF-e
        /// </summary>
        /// 
        [Description("NF-e e NFC-e")]
        Nfe = 0,
        /// <summary>
        /// Aplicativo ou serviços para processamento dos XMLs do CT-e
        /// </summary>
        [Description("CT-e")]
        Cte = 1,
        /// <summary>
        /// Aplicativo ou servicos para processamento dos XMLs da NFS-e
        /// </summary>
        [Description("NFS-e")]
        Nfse = 2,
        /// <summary>
        /// Aplicativo ou serviços para processamento dos XMLs do MDF-e
        /// </summary>
        [Description("MDF-e")]
        MDFe = 3,
        /// <summary>
        /// Aplicativo ou serviços para processamento dos XMLs da NFC-e
        /// </summary>
        [Description("NFC-e")]
        NFCe = 4,
        [Description("NF-e, NFC-e, CT-e e MDF-e")]
        Todos = 10,
        [Description("")]
        Nulo = 100
    }
    #endregion

    #region Padrão NFSe
    public enum PadroesNFSe
    {
        /// <summary>
        /// Não Identificado
        /// </summary>
        NaoIdentificado,
        /// <summary>
        /// Padrão GINFES
        /// </summary>
        GINFES,
        /// <summary>
        /// Padrão da BETHA Sistemas
        /// </summary>
        BETHA,
        /// <summary>
        /// Padrão da THEMA Informática
        /// </summary>
        THEMA,
        /// <summary>
        /// Padrão da prefeitura de Salvador-BA
        /// </summary>
        SALVADOR_BA,
        /// <summary>
        /// Padrão da prefeitura de Canoas-RS
        /// </summary>
        CANOAS_RS,
        /// <summary>
        /// Padrão da ISS Net
        /// </summary>    
        ISSNET,
        /// <summary>
        /// Padrão da prefeitura de Apucarana-PR
        /// Padrão da prefeitura de Aracatuba-SP
        /// </summary>
        ISSONLINE,
        /// <summary>
        /// Padrão da prefeitura de Blumenau-SC
        /// </summary>
        BLUMENAU_SC,
        /// <summary>
        /// Padrão da prefeitura de Juiz de Fora-MG
        /// </summary>
        BHISS,
        /// <summary>
        /// Padrao GIF
        /// Prefeitura de Campo Bom-RS
        /// </summary>
        GIF,
        /// <summary>
        /// Padrão IPM
        /// <para>Prefeitura de Campo Mourão.</para>
        /// </summary>
        IPM,
        /// <summary>
        /// Padrão DUETO
        /// Prefeitura de Nova Santa Rita - RS
        /// </summary>
        DUETO,
        /// <summary>
        /// Padrão WEB ISS
        /// Prefeitura de Feira de Santana - BA
        /// </summary>
        WEBISS,
        /// <summary>
        /// Padrão Nota Fiscal Eletrônica Paulistana -
        /// Prefeitura São Paulo - SP
        /// </summary>
        PAULISTANA,
        /// <summary>
        /// Padrão Nota Fiscal Eletrônica Porto Velhense
        /// Prefeitura de Porto Velho - RO
        /// </summary>
        PORTOVELHENSE,
        /// <summary>
        /// Padrão Nota Fiscal Eletrônica da PRONIN (GovBR)
        /// Prefeitura de Mirassol - SP
        /// </summary>
        PRONIN,
        /// <summary>
        /// Padrão Nota Fiscal Eletrônica ISS-ONline da 4R Sistemas
        /// Prefeitura de Governador Valadares - SP
        /// </summary>
        ISSONLINE4R,
        /// <summary>
        /// Padrão Nota Fiscal eletrônica DSF 
        /// Prefeitura de Campinas - SP
        /// Prefeitura de Campo Grande - MS
        /// </summary>
        DSF,
        /// <summary>
        /// Padrão Tecno Sistemas
        /// Prefeitura de Portão - RS
        /// </summary>
        TECNOSISTEMAS,
        /// <summary>
        /// Padrão System-PRO
        /// Prefeitura de Erechim - RS
        /// </summary>
        SYSTEMPRO,
        /// <summary>
        /// Preifetura de Macaé - RJ
        /// </summary>
        TIPLAN,
        /// <summary>
        /// Prefeitura do Rio de Janeiro - RJ
        /// </summary>
        CARIOCA,
        /// <summary>
        /// Prefeitura de Bauru - SP
        /// </summary>
        SIGCORP_SIGISS,
        /// <summary>
        /// Padrão SmaraPD
        /// Prefeitura de Sertãozinho - SP
        /// </summary>
        SMARAPD

        ///Atencao Wandrey.
        ///o nome deste enum tem que coincidir com o nome da url, pq faço um "IndexOf" deste enum para pegar o padrao

    }
    #endregion

    #region Classe dos tipos de ambiente da NFe
    /// <summary>
    /// Tipo de ambiente
    /// </summary>
    public enum TipoAmbiente
    {
        [Description("Produção")]
        taProducao = 1,
        [Description("Homologação")]
        taHomologacao = 2
    }
    #endregion

    #region TipoEmissao
    /// <summary>
    /// TipoEmissao
    /// </summary>
    public enum TipoEmissao
    {
        [Description("")]
        teNone = 0,
        [Description("Normal")]
        teNormal = 1,
        [Description("Contingência com formulário de segurança (FS)")]
        teFS = 2,
        [Description("Contingência com DPEC")]
        teDPEC = 4,
        [Description("Contingência com formulário de segurança (FS-DA)")]
        teFSDA = 5,
        [Description("Contingência com SVC-AN")]
        teSVCAN = 6,
        [Description("Contingência com SVC-RS")]
        teSVCRS = 7,
        [Description("Contingência com SVC-SP")]
        teSVCSP = 8,
        [Description("Contingência Off-Line (NFC-e)")]
        teOffLine = 9
    }
    #endregion

    #region Erros Padrões
    public enum ErroPadrao
    {
        ErroNaoDetectado = 0,
        FalhaInternet = 1,
        FalhaEnvioXmlWS = 2,
        CertificadoVencido = 3,
        FalhaEnvioXmlWSDPEC = 4, //danasa 21/10/2010
        FalhaEnvioXmlNFeWS = 5
    }
    #endregion

    #region EnumHelper

    /*
ComboBox combo = new ComboBox();
combo.DataSource = EnumHelper.ToList(typeof(SimpleEnum));
combo.DisplayMember = "Value";
combo.ValueMember = "Key";
    
        foreach (string value in Enum.GetNames(typeof(Model.TipoCampanhaSituacao)))
        {
            Model.TipoCampanhaSituacao stausEnum = (Model.TipoCampanhaSituacao)Enum.Parse(typeof(Model.TipoCampanhaSituacao), value);
            Console.WriteLine(" Description: " + value+"  "+ Model.EnumHelper.GetDescription(stausEnum));
        }
     
 */

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class AttributeTipoAplicacao : Attribute
    {
        private TipoAplicativo aplicacao;
        public TipoAplicativo Aplicacao
        {
            get
            {
                return this.aplicacao;
            }
        }

        public AttributeTipoAplicacao(TipoAplicativo aplicacao)
            : base()
        {
            this.aplicacao = aplicacao;
        }
    }

    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumDescriptionAttribute : Attribute
    {
        private string description;
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        public EnumDescriptionAttribute(string description)
            : base()
        {
            this.description = description;
        }
    }

    /// <summary>
    /// Classe com metodos para serem utilizadas nos Enuns
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Retorna a description do enum
        /// </summary>
        /// <param name="value">Enum para buscar a description</param>
        /// <returns>Retorna a description do enun</returns>
        /*public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }*/




        public static T StringToEnum<T>(string name) { return (T)Enum.Parse(typeof(T), name, true); }

        /// <summary>
        /// Gets the <see cref="DescriptionAttribute"/> of an <see cref="Enum"/> type value.
        /// </summary>
        /// <param name="value">The <see cref="Enum"/> type value.</param>
        /// <returns>A string containing the text of the <see cref="DescriptionAttribute"/>.</returns>
        public static string GetDescription(Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            string description = value.ToString();
            FieldInfo fieldInfo = value.GetType().GetField(description);
            EnumDescriptionAttribute[] attributes = (EnumDescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            else
            {
                return GetEnumItemDescription(value);
                //DescriptionAttribute[] dattributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                //if (dattributes != null && dattributes.Length > 0)
                //description = dattributes[0].Description;
            }
            return description;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string GetEnumItemDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        /// <summary>
        ///  Converts the <see cref="Enum"/> type to an <see cref="IList"/> compatible object.
        /// </summary>
        /// <param name="type">The <see cref="Enum"/> type.</param>
        /// <returns>An <see cref="IList"/> containing the enumerated type value and description.</returns>
        public static IList ToList(Type type, bool returnInt, bool excluibrancos)
        {
            return ToList(type, returnInt, excluibrancos, "");
        }

        public static IList ToList(Type type, bool returnInt, bool excluibrancos, string eliminar)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            ArrayList list = new ArrayList();
            Array enumValues = Enum.GetValues(type);

            foreach (Enum value in enumValues)
            {
                string _descr = GetDescription(value);
                if (excluibrancos && string.IsNullOrEmpty(_descr)) continue;

                if (eliminar.IndexOf(Convert.ToInt32(value).ToString()) != -1) continue;

                if (returnInt)
                    list.Add(new KeyValuePair<int, string>(Convert.ToInt32(value), _descr));
                else
                    list.Add(new KeyValuePair<Enum, string>(value, _descr));
            }

            return list;
        }

        public static IList ToStrings(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            ArrayList list = new ArrayList();
            Array enumValues = Enum.GetValues(type);

            foreach (Enum value in enumValues)
            {
                list.Add(GetDescription(value));
            }

            return list;
        }
    }

    #endregion
}
