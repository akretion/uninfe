using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Linq;
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
        private const int _Minutos = 12;  //12 minutos para atender o consumo indevido da SEFAZ

        public void Analisar(int emp)
        {
            this.oAux = new Auxiliar();

            try
            {
                if (string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaXmlEnviado) || !Directory.Exists(Empresas.Configuracoes[emp].PastaXmlEnviado)) return;

                // le todos os arquivos que estão na pasta em processamento

                string[] files = Directory.GetFiles(Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString()).Where(w => w.EndsWith(Propriedade.ExtEnvio.Nfe, StringComparison.InvariantCultureIgnoreCase) ||
                                                                                                                                                          w.EndsWith(Propriedade.ExtEnvio.Cte, StringComparison.InvariantCultureIgnoreCase) ||
                                                                                                                                                          w.EndsWith(Propriedade.ExtEnvio.MDFe, StringComparison.InvariantCultureIgnoreCase)).ToArray<string>();

                // considera os arquivos em que a data do ultimo acesso é superior a 5 minutos
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
                                XmlDocument doc = new XmlDocument();
                                doc.Load(file);

                                TipoAplicativo tipoArquivo = TipoAplicativo.Nfe;
                                string extNFe = Propriedade.ExtEnvio.Nfe;
                                string extProcNFe = Propriedade.ExtRetorno.ProcNFe;
                                string arquivoSit = string.Empty;
                                string chNFe = string.Empty;

                                switch (doc.DocumentElement.Name)
                                {
                                    case "MDFe":
                                        tipoArquivo = TipoAplicativo.MDFe;
                                        extNFe = Propriedade.ExtEnvio.MDFe;
                                        extProcNFe = Propriedade.ExtRetorno.ProcMDFe;

                                        oLerXml.Mdfe(file);
                                        arquivoSit = oLerXml.oDadosNfe.chavenfe.Substring(4);
                                        chNFe = oLerXml.oDadosNfe.chavenfe.Substring(4);
                                        break;

                                    case "NFe":
                                        tipoArquivo = TipoAplicativo.Nfe;
                                        extNFe = Propriedade.ExtEnvio.Nfe;
                                        extProcNFe = Propriedade.ExtRetorno.ProcNFe;

                                        oLerXml.Nfe(file);
                                        arquivoSit = oLerXml.oDadosNfe.chavenfe.Substring(3);
                                        chNFe = oLerXml.oDadosNfe.chavenfe.Substring(3);
                                        break;

                                    case "CTe":
                                        tipoArquivo = TipoAplicativo.Cte;
                                        extNFe = Propriedade.ExtEnvio.Cte;
                                        extProcNFe = Propriedade.ExtRetorno.ProcCTe;

                                        oLerXml.Cte(file);
                                        arquivoSit = oLerXml.oDadosNfe.chavenfe.Substring(3);
                                        chNFe = oLerXml.oDadosNfe.chavenfe.Substring(3);
                                        break;
                                }

                                //Ler a NFe

                                //Verificar se o -nfe.xml existe na pasta de autorizados
                                bool NFeJaNaAutorizada = oAux.EstaAutorizada(file, oLerXml.oDadosNfe.dEmi, extNFe, extNFe);

                                //Verificar se o -procNfe.xml existe na past de autorizados
                                bool procNFeJaNaAutorizada = oAux.EstaAutorizada(file, oLerXml.oDadosNfe.dEmi, extNFe, extProcNFe);

                                //Se um dos XML´s não estiver na pasta de autorizadas ele força finalizar o processo da NFe.
                                if (!NFeJaNaAutorizada || !procNFeJaNaAutorizada)
                                {
                                    //Verificar se a NFe está no fluxo, se não estiver vamos incluir ela para que funcione
                                    //a rotina de gerar o -procNFe.xml corretamente. Wandrey 21/10/2009
                                    if (!fluxo.NfeExiste(oLerXml.oDadosNfe.chavenfe))
                                    {
                                        fluxo.InserirNfeFluxo(oLerXml.oDadosNfe.chavenfe, oLerXml.oDadosNfe.mod, file);
                                    }

                                    //gera um -ped-sit.xml mesmo sendo autorizada ou denegada, pois assim sendo, o ERP precisaria dele
                                    oGerarXml.Consulta(tipoArquivo, arquivoSit + Propriedade.ExtEnvio.PedSit_XML,
                                        Convert.ToInt32(oLerXml.oDadosNfe.tpAmb),
                                        Convert.ToInt32(oLerXml.oDadosNfe.tpEmis),
                                        chNFe,
                                        oLerXml.oDadosNfe.versao);
                                }
                                else
                                {
                                    //Move o XML da pasta em processamento para a pasta de XML´s com erro (-nfe.xml)
                                    oAux.MoveArqErro(file);

                                    //Move o XML da pasta em processamento para a pasta de XML´s com erro (-procNFe.xml)
                                    oAux.MoveArqErro(Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + Functions.ExtrairNomeArq(file, extNFe) + extProcNFe);

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
        }
    }
}