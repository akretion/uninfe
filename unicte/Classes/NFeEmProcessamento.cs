using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading;
using unicte;
using UniNFeLibrary;
using UniNFeLibrary.Enums;

namespace unicte
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
                                //Ler a NFe
                                oLerXml.Nfe(file);

                                //Verificar se o -nfe.xml existe na pasta de autorizados
                                bool NFeJaNaAutorizada = oAux.EstaAutorizada(file, oLerXml.oDadosNfe.dEmi, ExtXml.Nfe);

                                //Verificar se o -procNfe.xml existe na past de autorizados
                                bool procNFeJaNaAutorizada = oAux.EstaAutorizada(file, oLerXml.oDadosNfe.dEmi, ExtXmlRet.ProcNFe);

                                //Se um dos XML´s não estiver na pasta de autorizadas ele força finalizar o processo da NFe.
                                if (!NFeJaNaAutorizada || !procNFeJaNaAutorizada)
                                {
                                    //Verificar se a NFe está no fluxo, se não estiver vamos incluir ela para que funcione
                                    //a rotina de gerar o -procNFe.xml corretamente. Wandrey 21/10/2009
                                    if (!fluxo.NfeExiste(oLerXml.oDadosNfe.chavenfe))
                                    {
                                        fluxo.InserirNfeFluxo(oLerXml.oDadosNfe.chavenfe, oAux.ExtrairNomeArq(file, ExtXml.Nfe) + ExtXml.Nfe);
                                    }

                                    //gera um -ped-sit.xml mesmo sendo autorizada ou denegada, pois assim sendo, o ERP precisaria dele
                                    string vArquivoSit = oLerXml.oDadosNfe.chavenfe.Substring(3);

                                    oGerarXml.Consulta(vArquivoSit + ExtXml.PedSit,
                                        Convert.ToInt32(oLerXml.oDadosNfe.tpAmb),
                                        Convert.ToInt32(oLerXml.oDadosNfe.tpEmis),
                                        oLerXml.oDadosNfe.chavenfe.Substring(3));
                                }
                                else
                                {
                                    //Move o XML da pasta em processamento para a pasta de XML´s com erro (-nfe.xml)
                                    oAux.MoveArqErro(file);

                                    //Move o XML da pasta em processamento para a pasta de XML´s com erro (-procNFe.xml)
                                    oAux.MoveArqErro(ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + oAux.ExtrairNomeArq(file, ExtXml.Nfe) + ExtXmlRet.ProcNFe);

                                    //Tirar a nota fiscal do fluxo
                                    fluxo.ExcluirNfeFluxo(oLerXml.oDadosNfe.chavenfe);
                                }
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
