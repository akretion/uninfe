using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading;

using UniNFeLibrary;
using UniNFeLibrary.Enums;

///
/// wandrey
/// o monitoramento de se uma nfe está autorizada ou denegada, no meu erp eu faço pela presenca do arquivo na pasta
/// de autorizadas\aaaamm ou denegadas\aaaamm
/// 
/// nao sei como os outros o fazem, se pelo protocolo ou pelo meu jeito.
/// se pelo protocolo, e ele não é retornado, então por este processo eles deverão monitorar pelo -sit.xml ou -sit.err
/// já que os estou gravando mesmo quando os arquivos já estejam na pasta Autorizadas\aaaamm ou Denegadas\aaaamm
/// 
/// Se o xml -procNFe.xml ou -den.xml não estão nas pastas, gero um -ped-sit.xml e se cStat=100, 
/// gero um -procNFe.xml ou cStat=301 ou 302, gero um -den.xml
/// e deixo o -sit.xml ou -sit.err para monitoramento.
/// 
/// NAO sei como fazer se no fluxo tem a nfe e na pasta em processamento NAO existir a nota
/// neste caso precisaria verificar o status da nota no fluxo
/// 
namespace uninfe
{
    public class NFeEmProcessamento
    {
        private Auxiliar oAux = null;
        private LerXML oLerXml = null;
        private GerarXML oGerarXml = null;
        private FluxoNfe fluxo = null;
        private const int _Minutos = 10;  //10 minutos?

        public NFeEmProcessamento()
        {
            if (ConfiguracaoApp.dUltimaAtualizacaoEmProcessamento.Year > 1)
            {
                ///
                /// executa de 10x10 minutos para evitar ter que acessar o HD sem necessidade
                /// 
                DateTime dCheck = ConfiguracaoApp.dUltimaAtualizacaoEmProcessamento.AddMinutes(_Minutos);
                if (dCheck > DateTime.Now)
                    return;
            }

            ConfiguracaoApp.dUltimaAtualizacaoEmProcessamento = DateTime.Now;
            this.oAux = new Auxiliar();

            try
            {
                ///
                /// le todos os arquivos que estão na pasta em processamento
                /// 
                string[] files = Directory.GetFiles(ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento.ToString(),
                                                "*" + ExtXml.Nfe,
                                                SearchOption.TopDirectoryOnly);
                ///
                /// considera os arquivos em que a data do ultimo acesso é superior a 10 minutos
                /// 
                DateTime UltimaData = DateTime.Now.AddMinutes(-_Minutos);

                foreach (string file in files)
                {
                    if (!Auxiliar.FileInUse(file))
                    {
                        FileInfo fi = new FileInfo(file);

                        //usar a última data de acesso, e não a data de criação
                        if (fi.LastWriteTime <= UltimaData)
                        {
                            if (this.oLerXml == null)
                            {
                                this.oLerXml = new LerXML();
                                this.oGerarXml = new GerarXML();
                                this.fluxo = new FluxoNfe();
                            }

                            try
                            {
                                ///
                                /// le a NFe
                                /// 
                                oLerXml.Nfe(file);


                                ///
                                /// gera um -ped-sit.xml mesmo sendo autorizada ou denegada, pois assim sendo, o ERP precisaria dele
                                /// 
                                string vArquivoSit = oLerXml.oDadosNfe.chavenfe.Substring(3);

                                oGerarXml.Consulta(vArquivoSit + ExtXml.PedSit,
                                    Convert.ToInt32(oLerXml.oDadosNfe.tpAmb),
                                    Convert.ToInt32(oLerXml.oDadosNfe.tpEmis),
                                    oLerXml.oDadosNfe.chavenfe.Substring(3));
                            }
                            catch (Exception ex)
                            {
                                ///
                                /// grava o arquivo com extensao .ERR 
                                /// 
                                oAux.GravarArqErroERP(Path.GetFileNameWithoutExtension(file) + ".err", (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ///
                /// grava o arquivo generico 
                /// 
                oAux.GravarArqErroERP(string.Format(InfoApp.NomeArqERRUniNFe, DateTime.Now.ToString("yyyyMMddThhmmss")), (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
        }
    }
}
