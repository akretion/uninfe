using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Reflection;
using UniNFeLibrary;
using UniNFeLibrary.Enums;

namespace uninfe
{
    #region Classe ServicoUniNFe
    /// <summary>
    /// Classe responsável pela execução dos serviços do UniNFe
    /// </summary>
    public class ServicoUniNFe : absServicoApp
    {
        #region Métodos gerais

        #region BuscaXML()
        /// <summary>
        /// Procurar os arquivos XML´s a serem enviados aos web-services ou para ser executado alguma rotina
        /// </summary>
        /// <param name="pTipoArq">Mascara dos arquivos as serem pesquisados. Ex: *.xml   *-nfe.xml</param>
        public override void BuscaXML(Object srvServico)
        {
            ServicoNFe oNfe = new ServicoNFe();

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
        protected override void ConvTXT()
        {
            Auxiliar oAux = new Auxiliar();

            List<string> lstArquivos = new List<string>();

            try
            {
                lstArquivos = this.ArquivosPasta(ConfiguracaoApp.vPastaXMLEnvio, "*-nfe.txt");

                for (int i = 0; i < lstArquivos.Count; i++)
                {
                    if (Auxiliar.FileInUse(lstArquivos[i]))
                        continue;

                    UnitxtTOxmlClass oUniTxtToXml = new UnitxtTOxmlClass();

                    oUniTxtToXml.Converter(lstArquivos[i], ConfiguracaoApp.vPastaXMLEnvio);

                    string ccMessage = string.Empty;
                    string ccExtension = "-nfe.err";

                    //Deu tudo certo com a conversão
                    if (oUniTxtToXml.cMensagemErro == string.Empty)
                    {
                        ///
                        /// danasa 8-2009
                        /// 
                        if (oUniTxtToXml.cRetorno.Count == 0)
                        {
                            ccMessage = "cStat=02\r\n" +
                                "xMotivo=Falha na conversão. Sem informações para converter o arquivo texto";

                            oAux.MoveArqErro(lstArquivos[i], ".txt");
                        }
                        else
                        {
                            ///
                            /// exclui o arquivo texto original
                            ///
                            oAux.DeletarArquivo(lstArquivos[i]);

                            ccExtension = "-nfe.txt";
                            ccMessage = "cStat=01\r\n" +
                                "xMotivo=Convertido com sucesso. Foi(ram) convertida(s) " + oUniTxtToXml.cRetorno.Count.ToString() + " nota(s) fiscal(is)";

                            foreach (txtTOxmlClassRetorno txtClass in oUniTxtToXml.cRetorno)
                            {
                                ///
                                /// monta o texto que será gravado no arquivo de aviso ao ERP
                                /// 
                                ccMessage += Environment.NewLine +
                                        "Nota fiscal: " + txtClass.NotaFiscal.ToString("000000000") +
                                        " Série: " + txtClass.Serie.ToString("000") +
                                        " - ChaveNFe: " + txtClass.ChaveNFe;
                                ///
                                /// move o arquivo XML criado na pasta Envio\Convertidos para a pasta Envio
                                /// 
                                FileInfo oArquivo = new FileInfo(ConfiguracaoApp.vPastaXMLEnvio + "\\convertidos\\" + txtClass.XMLFileName);

                                //System.Windows.Forms.MessageBox.Show(oNfe.vPastaXMLEnvio + "\\convertidos\\" + txtClass.XMLFileName);

                                string vNomeArquivoDestino = ConfiguracaoApp.vPastaXMLEnvio + "\\" + txtClass.XMLFileName;
                                if (File.Exists(vNomeArquivoDestino))
                                {
                                    ///
                                    /// excluo o XML se ele já existe
                                    /// 
                                    oAux.DeletarArquivo(vNomeArquivoDestino);
                                }
                                ///
                                /// move o arquivo da pasta "Envio\Convertidos" para a pasta "Envio"
                                /// 
                                //System.Windows.Forms.MessageBox.Show(ConfiguracaoApp.vPastaXMLEnvio + @"\convertidos\" + txtClass.XMLFileName + "\n\r" + vNomeArquivoDestino);
                                oArquivo.MoveTo(vNomeArquivoDestino);
                            }
                        }
                    }
                    else
                    {
                        ///
                        /// danasa 8-2009
                        /// 
                        ccMessage = "cStat=99\r\n" +
                            "xMotivo=Falha na conversão\r\n" +
                            "MensagemErro=" + oUniTxtToXml.cMensagemErro;

                        oAux.MoveArqErro(lstArquivos[i], ".txt");
                        ///
                        /// exclui todos os XML gerados na pasta Enviados\convertidos
                        /// 
                        foreach (txtTOxmlClassRetorno txtClass in oUniTxtToXml.cRetorno)
                        {
                            oAux.DeletarArquivo(ConfiguracaoApp.vPastaXMLEnvio + "\\convertidos\\" + txtClass.XMLFileName);
                        }
                    }
                    ///
                    /// danasa 8-2009
                    /// 
                    /// Gravar o retorno para o ERP em formato TXT com o erro ocorrido
                    /// 
                    oAux.GravarArqErroERP(oAux.ExtrairNomeArq(lstArquivos[i], "-nfe.txt") + ccExtension, ccMessage);
                }
            }
            catch (Exception ex)
            {
                //TODO: Retornar erro para o ERP
            }
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
