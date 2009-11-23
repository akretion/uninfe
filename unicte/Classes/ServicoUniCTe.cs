using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Reflection;
using UniNFeLibrary;
using UniNFeLibrary.Enums;

namespace unicte
{
    #region Classe ServicoUniNFe
    /// <summary>
    /// Classe responsável pela execução dos serviços do UniNFe
    /// </summary>
    public class ServicoUniCTe : absServicoApp
    {
        #region Métodos gerais

        #region BuscaXML()
        /// <summary>
        /// Procurar os arquivos XML´s a serem enviados aos web-services ou para ser executado alguma rotina
        /// </summary>
        /// <param name="pTipoArq">Mascara dos arquivos as serem pesquisados. Ex: *.xml   *-nfe.xml</param>
        public override void BuscaXML(Object srvServico)
        {
            ServicoCTe oNfe = new ServicoCTe();

            while (true)
            {
                this.ProcessaXML(oNfe, (Servicos)srvServico);

                Thread.Sleep(1000); //Pausa na Thread de 1000 milissegundos ou 1 segundo
            }
        }
        #endregion

        #region ConvTXT()
        /// <summary>
        /// Converter arquivos de NFe no formato TXT para XML
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>03/069/2009</date>
        protected override void ConvTXT(string vPasta)
        {
        }
        #endregion

        #region LerXMLNFe()
        protected override absLerXML.DadosNFeClass LerXMLNFe(string Arquivo)
        {
            LerXML oLerXML = new LerXML();
            oLerXML.Nfe(Arquivo);

            return oLerXML.oDadosNfe;
        }
        #endregion

        #region LerXMLRecibo()
        protected override absLerXML.DadosRecClass LerXMLRecibo(string Arquivo)
        {
            LerXML oLerXML = new LerXML();
            oLerXML.Recibo(Arquivo);

            return oLerXML.oDadosRec;
        }
        #endregion

        #region GerarChaveNFe
        protected override void GerarChaveNFe()
        {
            Auxiliar oAux = new Auxiliar();
            ///
            /// processa arquivos XML
            /// 
            List<string> lstArquivos = this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*" + ExtXml.GerarChaveNFe_XML);
            foreach (string ArqXMLPedido in lstArquivos)
            {
                oAux.GerarChaveNFe(ArqXMLPedido, true);
            }
            ///
            /// processa arquivos TXT
            /// 
            lstArquivos = this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*" + ExtXml.GerarChaveNFe_TXT);
            foreach (string ArqXMLPedido in lstArquivos)
            {
                oAux.GerarChaveNFe(ArqXMLPedido, false);
            }
        }
        #endregion

        #endregion
    }
    #endregion
}
