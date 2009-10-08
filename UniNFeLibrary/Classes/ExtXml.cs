using System;
using System.Collections.Generic;
using System.Text;

namespace UniNFeLibrary
{
    #region Classe Extensões dos XML´s ou TXT´s de envio
    /// <summary>
    /// Classe das Extensões dos XML´s ou TXT´s de envio
    /// </summary>
    public class ExtXml
    {
        public const string AltCon = "-alt-con.xml";
        public const string AltCon_TXT = "-alt-con.txt";
        public const string ConsCad = "-cons-cad.xml";
        public const string ConsCad_TXT = "-cons-cad.txt";
        public const string ConsInf = "-cons-inf.xml";
        public const string ConsInf_TXT = "-cons-inf.txt";
        public const string GerarChaveNFe_XML = "-gerar-chave.xml";
        public const string GerarChaveNFe_TXT = "-gerar-chave.txt";
        public const string MontarLote = "-montar-lote.xml";
        public const string EnvLot = "-env-lot.xml";
        public const string Nfe = "-nfe.xml";
        public const string PedCan = "-ped-can.xml";
        public const string PedCan_TXT = "-ped-can.txt";
        public const string PedInu = "-ped-inu.xml";
        public const string PedInu_TXT = "-ped-inu.txt";
        public const string PedRec = "-ped-rec.xml";
        public const string PedSit = "-ped-sit.xml";
        public const string PedSit_TXT = "-ped-sit.txt";
        public const string PedSta = "-ped-sta.xml";
        public const string PedSta_TXT = "-ped-sta.txt";
    }
    #endregion

    #region Classe das Extensões dos XML´s e TXT´s de retorno
    /// <summary>
    /// Classe das extensões dos XML´s de retorno
    /// </summary>
    public class ExtXmlRet
    {
        /// <summary>
        /// Retorno da consulta situação da NFe (-sit.xml)
        /// </summary>
        public const string Sit = "-sit.xml";
        /// <summary>
        /// Retorno da consulta do recibo do lote das nfe´s (-pro-rec.xml)
        /// </summary>
        public const string ProRec = "-pro-rec.xml";
        /// <summary>
        /// Retorno do status do serviço da nfe (-sta.xml)
        /// </summary>
        public const string Sta = "-sta.xml";
        /// <summary>
        /// XML de Distribuição da NFe (-procNFe.xml)
        /// </summary>
        public const string ProcNFe = "-procNFe.xml";
        /// <summary>
        /// XML de Distribuição do cancelamento da NFe
        /// </summary>
        public const string ProcCancNFe = "-procCancNFe.xml";
        /// <summary>
        /// XML de Distribuição da inutilização de números da NFe
        /// </summary>
        public const string ProcInutNFe = "-procInutNFe.xml";

    }
    #endregion

    #region Classe dos tipos de emissão da NFe
    /// <summary>
    /// Tipo de emissão da NFe - danasa 8-2009
    /// </summary>
    public class TipoEmissao
    {
        public const int teNormal = 1;
        public const int teContingencia = 2;
        public const int teSCAN = 3;
        public const int teDPEC = 4;
        public const int teFSDA = 5;
    }
    #endregion

    #region Classe dos tipos de ambiente da NFe
    /// <summary>
    /// Tipos de ambientes da NFe - danasa 8-2009
    /// </summary>
    public class TipoAmbiente
    {
        public const int taProducao = 1;
        public const int taHomologacao = 2;
    }
    #endregion

    #region Classe dos Parmâmetros necessários para o envio dos XML´s
    /// <summary>
    /// Parâmetros necessários para o envio dos XML´s
    /// </summary>
    public class ParametroEnvioXML
    {
        public int tpAmb { get; set; }
        public int tpEmis { get; set; }
        public int UFCod { get; set; }
    }
    #endregion
}
