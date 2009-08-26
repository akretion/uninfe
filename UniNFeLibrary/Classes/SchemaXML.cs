using System;
using System.Collections.Generic;
using System.Text;

namespace UniNFeLibrary
{
    /// <summary>
    /// Classe responsável por definir uma lista dos arquivos de SCHEMAS para validação dos XMLs
    /// </summary>
    public class SchemaXML
    {
        #region Listas
        /// <summary>
        /// Contém a TAG do XML que identifica qual XML é
        /// </summary>
        public static List<string> lstXMLTag = new List<string>();
        /// <summary>
        /// Contém um Identificador numérico que identifica qual XML é
        /// </summary>
        public static List<int> lstXMLID = new List<int>();
        /// <summary>
        /// Contém um texto explicativo que identifica qual XML é
        /// </summary>
        public static List<string> lstXMLTextoID = new List<string>();
        /// <summary>
        /// Contém o nome do arquivo de Schema vinculado ao XML
        /// </summary>
        public static List<string> lstXMLSchema = new List<string>();
        /// <summary>
        /// Contém as tag´s que devem ser assinadas em cada XML
        /// </summary>
        public static List<string> lstXMLTagAssinar = new List<string>();
        #endregion

        /// <summary>
        /// Cria várias listas com as TAG´s de identificação dos XML´s e seus Schemas
        /// </summary>
        /// <date>31/07/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public static void CriarListaIDXML()
        {
            #region Limpar listas
            lstXMLID.Clear();
            lstXMLSchema.Clear();
            lstXMLTag.Clear();
            lstXMLTagAssinar.Clear();
            lstXMLTextoID.Clear();
            #endregion

            #region XML Distribuição NFe
            lstXMLTag.Add("nfeProc");
            lstXMLID.Add(9);
            lstXMLTextoID.Add("XML de distribuição da NFe com protocolo de autorização anexado");
            lstXMLSchema.Add("procNFe_v1.10.xsd");
            lstXMLTagAssinar.Add(string.Empty);
            #endregion

            #region XML Distribuição Cancelamento
            lstXMLTag.Add("procCancNFe");
            lstXMLID.Add(10);
            lstXMLTextoID.Add("XML de distribuição do Cancelamento da NFe com protocolo de autorização anexado");
            lstXMLSchema.Add("procCancNFe_v1.07.xsd");
            lstXMLTagAssinar.Add(string.Empty);
            #endregion

            #region XML Distribuição Inutilização
            lstXMLTag.Add("procInutNFe");
            lstXMLID.Add(11);
            lstXMLTextoID.Add("XML de distribuição de Inutilização de Números de NFe com protocolo de autorização anexado");
            lstXMLSchema.Add("procInutNFe_v1.07.xsd");
            lstXMLTagAssinar.Add(string.Empty);
            #endregion

            #region XML NFe
            lstXMLTag.Add("NFe");
            lstXMLID.Add(1);
            lstXMLTextoID.Add("XML de Nota Fiscal Eletrônica");
            lstXMLSchema.Add("nfe_v1.10.xsd");
            lstXMLTagAssinar.Add("infNFe");
            #endregion

            #region XML Envio Lote
            lstXMLTag.Add("enviNFe");
            lstXMLID.Add(2);
            lstXMLTextoID.Add("XML de Envio de Lote de Notas Fiscais Eletrônicas");
            lstXMLSchema.Add("enviNFe_v1.10.xsd");
            lstXMLTagAssinar.Add(string.Empty);
            #endregion

            #region XML Cancelamento
            lstXMLTag.Add("cancNFe");
            lstXMLID.Add(3);
            lstXMLTextoID.Add("XML de Cancelamento de Nota Fiscal Eletrônica");
            lstXMLSchema.Add("cancNFe_v1.07.xsd");
            lstXMLTagAssinar.Add("infCanc");
            #endregion

            #region XML Inutilização
            lstXMLTag.Add("inutNFe");
            lstXMLID.Add(4);
            lstXMLTextoID.Add("XML de Inutilização de Numerações de Notas Fiscais Eletrônicas");
            lstXMLSchema.Add("inutNFe_v1.07.xsd");
            lstXMLTagAssinar.Add("infInut");
            #endregion

            #region XML Consulta Situação NFe
            lstXMLTag.Add("consSitNFe");
            lstXMLID.Add(5);
            lstXMLTextoID.Add("XML de Consulta da Situação da Nota Fiscal Eletrônica");
            lstXMLSchema.Add("consSitNFe_v1.07.xsd");
            lstXMLTagAssinar.Add(string.Empty);
            #endregion

            #region XML Consulta Recibo Lote
            lstXMLTag.Add("consReciNFe");
            lstXMLID.Add(6);
            lstXMLTextoID.Add("XML de Consulta do Recibo do Lote de Notas Fiscais Eletrônicas");
            lstXMLSchema.Add("consReciNfe_v1.10.xsd");
            lstXMLTagAssinar.Add(string.Empty);
            #endregion

            #region XML Consulta Situação Serviço NFe
            lstXMLTag.Add("consStatServ");
            lstXMLID.Add(7);
            lstXMLTextoID.Add("XML de Consulta da Situação do Serviço da Nota Fiscal Eletrônica");
            lstXMLSchema.Add("consStatServ_v1.07.xsd");
            lstXMLTagAssinar.Add(string.Empty);
            #endregion

            #region XML Consulta Cadastro Contribuinte
            lstXMLTag.Add("ConsCad");
            lstXMLID.Add(8);
            lstXMLTextoID.Add("XML de Consulta do Cadastro do Contribuinte");
            lstXMLSchema.Add("consCad_v1.01.xsd");
            lstXMLTagAssinar.Add(string.Empty);
            #endregion
        }

    }
}
