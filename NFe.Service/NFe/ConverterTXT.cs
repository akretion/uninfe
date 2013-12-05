using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;

using NFe.Components;
using NFe.Settings;
using NFe.Certificado;

namespace NFe.Service
{
    /// <summary>
    /// Converter o arquivo de NFe do formato TXT para XML
    /// </summary>
    /// <param name="arquivo">Nome completo do arquivo a ser convertido (Pasta e arquivo)</param>
    /// <remarks>
    /// Autor: Wandrey Mundin Ferreira
    /// </remarks>

    public class ConverterTXT
    {
        public ConverterTXT(string arquivo)
        {
            Auxiliar oAux = new Auxiliar();

            NFe.ConvertTxt.ConversaoTXT oUniTxtToXml = new NFe.ConvertTxt.ConversaoTXT();

            string pasta = new FileInfo(arquivo).DirectoryName;
            pasta = pasta.Substring(0, pasta.Length - 5); //Retirar a pasta \Temp do final - Wandrey 03/08/2011

            string ccMessage = string.Empty;
            string ccExtension = "-nfe.err";

            try
            {
                int emp = Functions.FindEmpresaByThread();

                ///
                /// exclui o arquivo de erro
                /// 
                Functions.DeletarArquivo(Empresa.Configuracoes[emp].PastaRetorno + "\\" + Path.GetFileName(Functions.ExtrairNomeArq(arquivo, "-nfe.txt") + ccExtension));
                Functions.DeletarArquivo(Empresa.Configuracoes[emp].PastaRetorno + "\\" + Path.GetFileName(Functions.ExtrairNomeArq(arquivo, "-nfe.txt") + "-nfe-ret.xml"));
                Functions.DeletarArquivo(Empresa.Configuracoes[emp].PastaErro + "\\" + Path.GetFileName(arquivo));
                ///
                /// exclui o arquivo TXT original
                /// 
                Functions.DeletarArquivo(Empresa.Configuracoes[emp].PastaRetorno + "\\" + Path.GetFileNameWithoutExtension(arquivo) + "-orig.txt");

                ///
                /// processa a conversão
                /// 
                oUniTxtToXml.Converter(arquivo, pasta);//Empresa.Configuracoes[emp].PastaRetorno);

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

                        oAux.MoveArqErro(arquivo, ".txt");
                    }
                    else
                    {
                        //
                        // salva o arquivo texto original
                        //
                        if (pasta.ToLower().Equals(Empresa.Configuracoes[emp].PastaEnvio.ToLower()) || pasta.ToLower().Equals(Empresa.Configuracoes[emp].PastaValidar.ToLower()))
                        {
                            FileInfo ArqOrig = new FileInfo(arquivo);

                            string vvNomeArquivoDestino = Empresa.Configuracoes[emp].PastaRetorno + "\\" + Path.GetFileNameWithoutExtension(arquivo) + "-orig.txt";
                            ArqOrig.MoveTo(vvNomeArquivoDestino);
                        }
                        ccExtension = "-nfe.txt";
                        ccMessage = "cStat=01\r\n" +
                            "xMotivo=Convertido com sucesso. Foi(ram) convertida(s) " + oUniTxtToXml.cRetorno.Count.ToString() + " nota(s) fiscal(is)";

                        foreach (NFe.ConvertTxt.txtTOxmlClassRetorno txtClass in oUniTxtToXml.cRetorno)
                        {
                            ///
                            /// monta o texto que será gravado no arquivo de aviso ao ERP
                            /// 
                            ccMessage += Environment.NewLine +
                                    "Nota fiscal: " + txtClass.NotaFiscal.ToString("000000000") +
                                    " Série: " + txtClass.Serie.ToString("000") +
                                    " - ChaveNFe: " + txtClass.ChaveNFe;

                            // move o arquivo XML criado na pasta Envio\Convertidos para a pasta Envio
                            // ou
                            // move o arquivo XML criado na pasta Validar\Convertidos para a pasta Validar
                            string nomeArquivoDestino = Path.Combine(pasta, Path.GetFileName(txtClass.XMLFileName));
                            Functions.Move(txtClass.XMLFileName, nomeArquivoDestino);
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
                oAux.MoveArqErro(arquivo, ".txt");

                if (ccMessage.StartsWith("cStat=02") || ccMessage.StartsWith("cStat=99"))
                {
                    ///
                    /// exclui todos os XML gerados na pasta Envio\convertidos somente se houve erro na conversão
                    /// 
                    foreach (NFe.ConvertTxt.txtTOxmlClassRetorno txtClass in oUniTxtToXml.cRetorno)
                    {
                        Functions.DeletarArquivo(pasta + "\\convertidos\\" + Path.GetFileName(txtClass.XMLFileName));
                    }
                }
                ///
                /// danasa 8-2009
                /// 
                /// Gravar o retorno para o ERP em formato TXT com o erro ocorrido
                /// 
                oAux.GravarArqErroERP(Functions.ExtrairNomeArq(arquivo, "-nfe.txt") + ccExtension, ccMessage);
            }
        }
    }
}
