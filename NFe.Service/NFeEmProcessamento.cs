using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading;
using NFe.Settings;
using NFe.Components;

namespace NFe.Service
{
    public class NFeEmProcessamento
    {
        private Auxiliar oAux = null;
        private LerXML oLerXml = null;
        private GerarXML oGerarXml = null;
        private FluxoNfe fluxo = null;
        private const int _Minutos = 10;  //10 minutos?

        public void Analisar()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            this.oAux = new Auxiliar();

            try
            {
                ///
                /// le todos os arquivos que estão na pasta em processamento
                /// 
                string[] files = Directory.GetFiles(Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString(),
                                                "*" + Propriedade.ExtEnvio.Nfe,
                                                SearchOption.TopDirectoryOnly);
                ///
                /// considera os arquivos em que a data do ultimo acesso é superior a 5 minutos
                /// 
                DateTime UltimaData = DateTime.Now.AddMinutes(-_Minutos);

                foreach (string file in files)
                {
                    if (!Functions.FileInUse(file))
                    {
                        FileInfo fi = new FileInfo(file);

                        //usar a última data de acesso, e não a data de criação
                        if (fi.LastWriteTime <= UltimaData)
                        {
                            if (this.oLerXml == null)
                            {
                                this.oLerXml = new LerXML();
                                this.oGerarXml = new GerarXML(emp);
                                this.fluxo = new FluxoNfe(emp);
                            }

                            try
                            {
                                //Ler a NFe
                                oLerXml.Nfe(file);

                                //Verificar se o -nfe.xml existe na pasta de autorizados
                                bool NFeJaNaAutorizada = oAux.EstaAutorizada(file, oLerXml.oDadosNfe.dEmi, Propriedade.ExtEnvio.Nfe);

                                //Verificar se o -procNfe.xml existe na past de autorizados
                                bool procNFeJaNaAutorizada = oAux.EstaAutorizada(file, oLerXml.oDadosNfe.dEmi, Propriedade.ExtRetorno.ProcNFe);

                                //Se um dos XML´s não estiver na pasta de autorizadas ele força finalizar o processo da NFe.
                                if (!NFeJaNaAutorizada || !procNFeJaNaAutorizada)
                                {
                                    //Verificar se a NFe está no fluxo, se não estiver vamos incluir ela para que funcione
                                    //a rotina de gerar o -procNFe.xml corretamente. Wandrey 21/10/2009
                                    if (!fluxo.NfeExiste(oLerXml.oDadosNfe.chavenfe))
                                    {
                                        fluxo.InserirNfeFluxo(oLerXml.oDadosNfe.chavenfe, Functions.ExtrairNomeArq(file, Propriedade.ExtEnvio.Nfe) + Propriedade.ExtEnvio.Nfe);
                                    }

                                    //gera um -ped-sit.xml mesmo sendo autorizada ou denegada, pois assim sendo, o ERP precisaria dele
                                    string vArquivoSit = oLerXml.oDadosNfe.chavenfe.Substring(3);

                                    oGerarXml.Consulta(vArquivoSit + Propriedade.ExtEnvio.PedSit_XML,
                                        Convert.ToInt32(oLerXml.oDadosNfe.tpAmb),
                                        Convert.ToInt32(oLerXml.oDadosNfe.tpEmis),
                                        oLerXml.oDadosNfe.chavenfe.Substring(3));
                                }
                                else
                                {
                                    //Move o XML da pasta em processamento para a pasta de XML´s com erro (-nfe.xml)
                                    oAux.MoveArqErro(file);

                                    //Move o XML da pasta em processamento para a pasta de XML´s com erro (-procNFe.xml)
                                    oAux.MoveArqErro(Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + Functions.ExtrairNomeArq(file, Propriedade.ExtEnvio.Nfe) + Propriedade.ExtRetorno.ProcNFe);

                                    //Tirar a nota fiscal do fluxo
                                    fluxo.ExcluirNfeFluxo(oLerXml.oDadosNfe.chavenfe);
                                }
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    // grava o arquivo com extensao .ERR 
                                    oAux.GravarArqErroERP(Path.GetFileNameWithoutExtension(file) + ".err", ex.Message);
                                }
                                catch
                                {
                                    //Se deu erro na hora de gravar o erro para o ERP, infelizmente não posso fazer mais nada. Wandrey 28/04/2011
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    // grava o arquivo generico 
                    oAux.GravarArqErroERP(string.Format(Propriedade.NomeArqERRUniNFe, DateTime.Now.ToString("yyyyMMddTHHmmss")), ex.Message);
                }
                catch
                {
                    //Se deu erro na hora de gravar o erro para o ERP, infelizmente não posso fazer mais nada. Wandrey 28/04/2011
                }
            }
            finally
            {
                Thread.CurrentThread.Abort();
            }            
        }
    }
}