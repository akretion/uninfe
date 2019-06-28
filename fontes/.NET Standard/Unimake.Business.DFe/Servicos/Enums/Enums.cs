namespace Unimake.Business.DFe.Servicos
{
    #region Servico
    /// <summary>
    /// Serviços disponíveis
    /// </summary>
    public enum Servico
    {
        #region NFe

        /// <summary>
        /// Consulta status serviço NFe
        /// </summary>
        NFeStatusServico,

        /// <summary>
        /// Consulta protocolo da NFe
        /// </summary>
        NFeConsultaProtocolo,

        /// <summary>
        /// Inutilização de números da nota fiscal eletrônica
        /// </summary>
        NFeInutilizacao,

        /// <summary>
        /// Consulta cadastro do contribuinte
        /// </summary>
        NFeConsultaCadastro,

        /// <summary>
        /// Serviço não definido
        /// </summary>
        Nulo

        #endregion
    }
    #endregion

    #region DFE
    /// <summary>
    /// Tipos de DFe´s existentes
    /// </summary>
    public enum DFE
    {
        /// <summary>
        /// NF-e - Nota Fiscal Eletrônica
        /// </summary>
        NFe,
        /// <summary>
        /// NFC-e - Nota Fiscal de Venda a Consumidor Eletrônica
        /// </summary>
        NFCe,
        /// <summary>
        /// CT-e - Conhecimento de Transporte Eletrônico
        /// </summary>
        CTe,
        /// <summary>
        /// MDF-e - Manifesto Eletrônico de Documentos Fiscais
        /// </summary>
        MDFe,
        /// <summary>
        /// Nota Fiscal de Serviço Eletrônica
        /// </summary>
        NFSe,
        /// <summary>
        /// CFe-SAT - Sistema Autenticador e Transmissor de Cupons Fiscais Eletrônicos
        /// </summary>
        SAT
    }
    #endregion

    #region UFBrasilIBGE
    /// <summary>
    /// Unidades Federativas do Brasil com código do IBGE
    /// Unidades Federativas do Brasil (Tem como XmlEnumAttribute o código IBGE da UF)
    /// </summary>
    public enum UFBrasilIBGE
    {
        /// <summary>
        /// Acre - AC (12)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("12")]
        AC = 12,

        /// <summary>
        /// Alagoas - AL (17)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("17")]
        AL = 17,

        /// <summary>
        /// Amapá - AP (16)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("16")]
        AP = 16,

        /// <summary>
        /// Amazonas - AM (13)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("13")]
        AM = 13,

        /// <summary>
        /// Bahia - BA (29)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("29")]
        BA = 29,

        /// <summary>
        /// Ceará - CE (23)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("23")]
        CE = 23,

        /// <summary>
        /// Distrito Federal - DF (53)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("53")]
        DF = 53,

        /// <summary>
        /// Espírito Santo - ES (32)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("32")]
        ES = 32,

        /// <summary>
        /// Goiás - GO (52)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("52")]
        GO = 52,

        /// <summary>
        /// Maranhão - MA (21)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("21")]
        MA = 21,

        /// <summary>
        /// Mato Grosso - MT (51)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("51")]
        MT = 51,

        /// <summary>
        /// Mato Grosso do Sul - MS (50)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("50")]
        MS = 50,

        /// <summary>
        /// Minas Gerais - MG (31)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("31")]
        MG = 31,

        /// <summary>
        /// Pará - PA (15)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("15")]
        PA = 15,

        /// <summary>
        /// Paraíba - PB (25)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("25")]
        PB = 25,

        /// <summary>
        /// Paraná - PR (41)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("41")]
        PR = 41,

        /// <summary>
        /// Pernambuco - PE (26)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("26")]
        PE = 26,

        /// <summary>
        /// Piauí - PI (22)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("22")]
        PI = 22,

        /// <summary>
        /// Rio de Janeiro - RJ (33)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("33")]
        RJ = 33,

        /// <summary>
        /// Rio Grande do Norte - RN (24)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("24")]
        RN = 24,

        /// <summary>
        /// Rio Grande do Sul - RS (43)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("43")]
        RS = 43,

        /// <summary>
        /// Rondônia - RO (11)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("11")]
        RO = 11,

        /// <summary>
        /// Roraima - RR (14)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("14")]
        RR = 14,

        /// <summary>
        /// Santa Catarina - SC (42)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("42")]
        SC = 42,

        /// <summary>
        /// São Paulo - SP (35)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("35")]
        SP = 35,

        /// <summary>
        /// Sergipe - SE (28)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("28")]
        SE = 28,

        /// <summary>
        /// Tocantins - TO (17)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("17")]
        TO = 17
    }
    #endregion

    #region UF
    /// <summary>
    /// Unidades Federativas do Brasil (Tem como XmlEnumAttribute o nome abreviado da UF)
    /// </summary>
    public enum UFBrasil
    {
        /// <summary>
        /// Acre - AC (12)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("AC")]
        AC = 12,

        /// <summary>
        /// Alagoas - AL (17)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("AL")]
        AL = 17,

        /// <summary>
        /// Amapá - AP (16)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("AP")]
        AP = 16,

        /// <summary>
        /// Amazonas - AM (13)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("AM")]
        AM = 13,

        /// <summary>
        /// Bahia - BA (29)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("BA")]
        BA = 29,

        /// <summary>
        /// Ceará - CE (23)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("CE")]
        CE = 23,

        /// <summary>
        /// Distrito Federal - DF (53)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("DF")]
        DF = 53,

        /// <summary>
        /// Espírito Santo - ES (32)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("ES")]
        ES = 32,

        /// <summary>
        /// Goiás - GO (52)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("GO")]
        GO = 52,

        /// <summary>
        /// Maranhão - MA (21)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("MA")]
        MA = 21,

        /// <summary>
        /// Mato Grosso - MT (51)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("MT")]
        MT = 51,

        /// <summary>
        /// Mato Grosso do Sul - MS (50)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("MS")]
        MS = 50,

        /// <summary>
        /// Minas Gerais - MG (31)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("MG")]
        MG = 31,

        /// <summary>
        /// Pará - PA (15)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("PA")]
        PA = 15,

        /// <summary>
        /// Paraíba - PB (25)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("PB")]
        PB = 25,

        /// <summary>
        /// Paraná - PR (41)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("PR")]
        PR = 41,

        /// <summary>
        /// Pernambuco - PE (26)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("PE")]
        PE = 26,

        /// <summary>
        /// Piauí - PI (22)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("PI")]
        PI = 22,

        /// <summary>
        /// Rio de Janeiro - RJ (33)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("RJ")]
        RJ = 33,

        /// <summary>
        /// Rio Grande do Norte - RN (24)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("RN")]
        RN = 24,

        /// <summary>
        /// Rio Grande do Sul - RS (43)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("RS")]
        RS = 43,

        /// <summary>
        /// Rondônia - RO (11)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("RO")]
        RO = 11,

        /// <summary>
        /// Roraima - RR (14)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("RR")]
        RR = 14,

        /// <summary>
        /// Santa Catarina - SC (42)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("SC")]
        SC = 42,

        /// <summary>
        /// São Paulo - SP (35)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("SP")]
        SP = 35,

        /// <summary>
        /// Sergipe - SE (28)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("SE")]
        SE = 28,

        /// <summary>
        /// Tocantins - TO (17)
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("TO")]
        TO = 17
    }
    #endregion

    /// <summary>
    /// Tipo ambiente DFe (NFe, CTe, MDFe, NFCe, etc...)
    /// </summary>
    public enum TipoAmbiente
    {
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Producao = 1,
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Homologacao = 2
    }
}