namespace Unimake.DFe.Servicos
{
    /// <summary>
    /// Serviços disponíveis
    /// </summary>
    public enum Servicos
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

    /// <summary>
    /// Tipos de DFe´s existentes
    /// </summary>
    public enum DFEs
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

    /// <summary>
    /// Unidades Federativas
    /// </summary>
    public enum UnidadesFederativas
    {
        /// <summary>
        /// Acre - AC (12)
        /// </summary>
        AC = 12,

        /// <summary>
        /// Alagoas - AL (17)
        /// </summary>
        AL = 17,

        /// <summary>
        /// Amapá - AP (16)
        /// </summary>
        AP = 16,

        /// <summary>
        /// Amazonas - AM (13)
        /// </summary>
        AM = 13,

        /// <summary>
        /// Bahia - BA (29)
        /// </summary>
        BA = 29,

        /// <summary>
        /// Ceará - CE (23)
        /// </summary>
        CE = 23,

        /// <summary>
        /// Distrito Federal - DF (53)
        /// </summary>
        DF = 53,

        /// <summary>
        /// Espírito Santo - ES (32)
        /// </summary>
        ES = 32,

        /// <summary>
        /// Goiás - GO (52)
        /// </summary>
        GO = 52,

        /// <summary>
        /// Maranhão - MA (21)
        /// </summary>
        MA = 21,

        /// <summary>
        /// Mato Grosso - MT (51)
        /// </summary>
        MT = 51,

        /// <summary>
        /// Mato Grosso do Sul - MS (50)
        /// </summary>
        MS = 50,

        /// <summary>
        /// Minas Gerais - MG (31)
        /// </summary>
        MG = 31,

        /// <summary>
        /// Pará - PA (15)
        /// </summary>
        PA = 15,

        /// <summary>
        /// Paraíba - PB (25)
        /// </summary>
        PB = 25,

        /// <summary>
        /// Paraná - PR (41)
        /// </summary>
        PR = 41,

        /// <summary>
        /// Pernambuco - PE (26)
        /// </summary>
        PE = 26,

        /// <summary>
        /// Piauí - PI (22)
        /// </summary>
        PI = 22,

        /// <summary>
        /// Rio de Janeiro - RJ (33)
        /// </summary>
        RJ = 33,

        /// <summary>
        /// Rio Grande do Norte - RN (24)
        /// </summary>
        RN = 24,

        /// <summary>
        /// Rio Grande do Sul - RS (43)
        /// </summary>
        RS = 43,

        /// <summary>
        /// Rondônia - RO (11)
        /// </summary>
        RO = 11,

        /// <summary>
        /// Roraima - RR (14)
        /// </summary>
        RR = 14,

        /// <summary>
        /// Santa Catarina - SC (42)
        /// </summary>
        SC = 42,

        /// <summary>
        /// São Paulo - SP (35)
        /// </summary>
        SP = 35,

        /// <summary>
        /// Sergipe - SE (28)
        /// </summary>
        SE = 28,

        /// <summary>
        /// Tocantins - TO (17)
        /// </summary>
        TO = 17
    }
}