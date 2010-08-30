using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Reflection;
using UniNFeLibrary;
using UniNFeLibrary.Enums;
using System.Xml;
using System.Windows.Forms;

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
        public override void BuscaXML(object parametroThread)
        {
            ParametroThread param = (ParametroThread)parametroThread;

            ServicoNFe oNfe = new ServicoNFe();

            //Criar XML de controle de fluxo de envios de Notas Fiscais
            FluxoNfe oFluxoNfe = new FluxoNfe();
            try
            {
                oFluxoNfe.CriarXml(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao tentar criar o XML para o controle do fuxo do envio dos documentos eletrônicos.\r\n\r\nErro:" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            while (true)
            {
                this.ProcessaXML(oNfe, param.Servico);

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
            int emp = Empresa.FindEmpresaThread(Thread.CurrentThread.Name);
            Auxiliar oAux = new Auxiliar();

            List<string> lstArquivos = this.ArquivosPasta(vPasta/*ConfiguracaoApp.vPastaXMLEnvio*/, "*-nfe.txt");

            for (int i = 0; i < lstArquivos.Count; i++)
            {
                if (Auxiliar.FileInUse(lstArquivos[i]))
                    continue;

                UnitxtTOxmlClass oUniTxtToXml = new UnitxtTOxmlClass();
                string ccMessage = string.Empty;
                string ccExtension = "-nfe.err";

                try
                {
                    ///
                    /// exclui o arquivo de erro
                    /// 
                    oAux.DeletarArquivo(Empresa.Configuracoes[emp].PastaRetorno + "\\" + Path.GetFileName(oAux.ExtrairNomeArq(lstArquivos[i], "-nfe.txt") + ccExtension));
                    oAux.DeletarArquivo(Empresa.Configuracoes[emp].PastaRetorno + "\\" + Path.GetFileName(oAux.ExtrairNomeArq(lstArquivos[i], "-nfe.txt") + "-nfe-ret.xml"));
                    oAux.DeletarArquivo(Empresa.Configuracoes[emp].PastaErro + "\\" + Path.GetFileName(lstArquivos[i]));
                    ///
                    /// exclui o arquivo TXT original
                    /// 
                    oAux.DeletarArquivo(Empresa.Configuracoes[emp].PastaRetorno + "\\" + Path.GetFileNameWithoutExtension(lstArquivos[i]) + "-orig.txt");

                    ///
                    /// processa a conversão
                    /// 
                    oUniTxtToXml.Converter(lstArquivos[i], vPasta/*ConfiguracaoApp.vPastaXMLEnvio*/);

                    //Deu tudo certo com a conversão?
                    if (string.IsNullOrEmpty(oUniTxtToXml.cMensagemErro))
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
                            /// salva o arquivo texto original
                            ///
                            FileInfo otxtArquivo = new FileInfo(lstArquivos[i]);
                            if (vPasta.Equals(Empresa.Configuracoes[emp].PastaEnvio))
                            {
                                string vvNomeArquivoDestino = Empresa.Configuracoes[emp].PastaRetorno + "\\" + Path.GetFileNameWithoutExtension(lstArquivos[i]) + "-orig.txt";
                                otxtArquivo.MoveTo(vvNomeArquivoDestino);
                            }
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
                                /// ou
                                /// move o arquivo XML criado na pasta Validar\Convertidos para a pasta Validar
                                /// 
                                FileInfo oArquivo = new FileInfo(vPasta/*ConfiguracaoApp.vPastaXMLEnvio*/ + "\\convertidos\\" + txtClass.XMLFileName);

                                string vNomeArquivoDestino = vPasta/*ConfiguracaoApp.vPastaXMLEnvio*/ + "\\" + txtClass.XMLFileName;
                                ///
                                /// excluo o XML se já existe
                                /// 
                                oAux.DeletarArquivo(vNomeArquivoDestino);

                                ///
                                /// move o arquivo da pasta "Envio\Convertidos" para a pasta "Envio"
                                /// ou
                                /// move o arquivo da pasta "Validar\Convertidos" para a pasta "Validar"
                                /// 
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
                    }
                }
                catch (Exception ex)
                {
                    ccMessage = ex.Message;
                    ccExtension = "-nfe.err";
                }

                if (!string.IsNullOrEmpty(ccMessage))
                {
                    oAux.MoveArqErro(lstArquivos[i], ".txt");
                    ///
                    /// exclui todos os XML gerados na pasta Enviados\convertidos
                    /// 
                    foreach (txtTOxmlClassRetorno txtClass in oUniTxtToXml.cRetorno)
                    {
                        oAux.DeletarArquivo(vPasta/*ConfiguracaoApp.vPastaXMLEnvio*/ + "\\convertidos\\" + txtClass.XMLFileName);
                    }
                    ///
                    /// danasa 8-2009
                    /// 
                    /// Gravar o retorno para o ERP em formato TXT com o erro ocorrido
                    /// 
                    oAux.GravarArqErroERP(oAux.ExtrairNomeArq(lstArquivos[i], "-nfe.txt") + ccExtension, ccMessage);
                }
            }
        }
        #endregion

        #region GerarChaveNFe
        protected override void GerarChaveNFe()
        {
            int emp = Empresa.FindEmpresaThread(Thread.CurrentThread.Name);

            Auxiliar oAux = new Auxiliar();
            ///
            /// processa arquivos XML
            /// 
            List<string> lstArquivos = this.ArquivosPasta(Empresa.Configuracoes[emp].PastaEnvio, "*" + ExtXml.GerarChaveNFe_XML);
            foreach (string ArqXMLPedido in lstArquivos)
            {
                oAux.GerarChaveNFe(ArqXMLPedido, true);
            }
            ///
            /// processa arquivos TXT
            /// 
            lstArquivos = this.ArquivosPasta(Empresa.Configuracoes[emp].PastaEnvio, "*" + ExtXml.GerarChaveNFe_TXT);
            foreach (string ArqXMLPedido in lstArquivos)
            {
                oAux.GerarChaveNFe(ArqXMLPedido, false);
            }
        }
        #endregion

        #region EmProcessamento
        protected override void EmProcessamento()
        {
            new NFeEmProcessamento();
        }
        #endregion


        #region LerXMLNFe()
        protected override absLerXML.DadosNFeClass LerXMLNFe(string Arquivo)
        {
            LerXML oLerXML = new LerXML();

            try
            {
                oLerXML.Nfe(Arquivo);
            }
            catch (XmlException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

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

        #endregion

    }
    #endregion
}