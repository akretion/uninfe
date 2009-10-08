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
                if ( dCheck > DateTime.Now)
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
                                /// verifica se o XML ja existe na pasta de autorizados ou denegados
                                /// 
                                bool Denegada = this.EstaDenegada(Path.GetFileName(file), oLerXml.oDadosNfe.dEmi);
                                bool jaAD = this.EstaAutorizada(Path.GetFileName(file), oLerXml.oDadosNfe.dEmi) || Denegada;
                                ///
                                /// SE jah fora autorizada ou denegada, mas não notificada ao erp
                                /// vou gerar um -ped-sit.xml e o ERP deverá monitorar pelo retorno -sit.xml ou -sit.err
                                /// 

                                /* Wandrey, creio que não precisa saber se está no fluxo
                                if (!jaAD)
                                {
                                    if (fluxo.NfeExiste(oLerXml.oDadosNfe.chavenfe))
                                    {
                                        //fluxo.InserirNfeFluxo(chavenfe, file);
                                    }
                                }*/

                                ///
                                /// se já autorizada ou denegada, gera o -ped-sit.xml com o proprio nome da chave da NFe.
                                /// se não, prefixa on "NFe" para ser proprietario deste processo
                                /// 
                                string vArquivoSit = (jaAD ? oLerXml.oDadosNfe.chavenfe.Substring(3) : oLerXml.oDadosNfe.chavenfe);
                                ///
                                /// monta os nomes dos arquivos de retorno
                                /// 
                                string ArqXMLRetorno = ConfiguracaoApp.vPastaXMLRetorno + "\\" + vArquivoSit + "-sit.xml";
                                string ArqERRRetorno = ConfiguracaoApp.vPastaXMLRetorno + "\\" + vArquivoSit + "-sit.err";
                                ///
                                /// exclui os arquivos de retorno para evitar ler conteudo incorreto
                                /// 
                                oAux.DeletarArquivo(ArqERRRetorno);
                                oAux.DeletarArquivo(ArqXMLRetorno);

                                ///
                                /// gera um -ped-sit.xml mesmo sendo autorizada ou denegada, pois assim sendo, o ERP precisaria dele
                                /// 
                                oGerarXml.Consulta(vArquivoSit + ExtXml.PedSit,
                                                    Convert.ToInt32(oLerXml.oDadosNfe.tpAmb), 
                                                    Convert.ToInt32(oLerXml.oDadosNfe.tpEmis),
                                                    oLerXml.oDadosNfe.chavenfe.Substring(3));

                                if (!jaAD)  //se ainda não autorizada ou denegada
                                {
                                    bool excluiErro = true;
                                    bool excluiRetorno = true;
                                    try
                                    {
                                        ///
                                        /// monitora os retornos do -ped-sit.xml
                                        /// 
                                        this.RecebeResposta(ArqXMLRetorno, ArqERRRetorno);

                                        if (File.Exists(ArqXMLRetorno))
                                        {
                                            ///
                                            /// processa o arquivo de retorno
                                            /// se retorno=true, renomeia o arquivo de retorno para que o ERP possa interpretá-lo
                                            /// pois ele foi autorizado ou denegado
                                            /// 
                                            if (this.LerRetornoSit(file, ArqXMLRetorno))
                                            {
                                                excluiRetorno = false;
                                                oAux.RenomearArquivo(ArqXMLRetorno, ConfiguracaoApp.vPastaXMLRetorno + "\\" + Path.GetFileName(ArqXMLRetorno).Replace("NFe", ""));
                                            }
                                        }
                                        else
                                            if (File.Exists(ArqERRRetorno))
                                            {
                                                ///
                                                /// renomeia o arquivo de retorno para que o ERP possa interpretá-lo
                                                /// 
                                                excluiErro = false;
                                                oAux.RenomearArquivo(ArqERRRetorno, ConfiguracaoApp.vPastaXMLRetorno + "\\" + Path.GetFileName(ArqERRRetorno).Replace("NFe", ""));
                                            }
                                            else
                                                throw new Exception("VerificaNFeEmProcessamento: Não pode obter o retorno do pedido de um -ped-sit.xml");
                                    }
                                    catch (Exception ex)
                                    {
                                        ///
                                        /// grava o arquivo de erro modificando o nome para que o ERP possa interpretá-lo
                                        /// 
                                        ArqERRRetorno = Path.GetFileName(ArqERRRetorno).Replace("NFe", "");
                                        oAux.GravarArqErroERP(ArqERRRetorno, (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                                        excluiErro = false;
                                    }
                                    if (excluiRetorno)
                                        oAux.DeletarArquivo(ArqXMLRetorno);
                                    if (excluiErro)
                                        oAux.DeletarArquivo(ArqERRRetorno);
                                }
                                if (jaAD)
                                {
                                    ///
                                    /// exclui a nota do fluxo
                                    /// 
                                    fluxo.ExcluirNfeFluxo(oLerXml.oDadosNfe.chavenfe);
                                    ///
                                    /// move o XML da pasta EmProcessamento para a Autorizada ou Denegada
                                    /// 
                                    oAux.MoverArquivo(file, (Denegada ? PastaEnviados.Denegados : PastaEnviados.Autorizados), oLerXml.oDadosNfe.dEmi);
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

        private bool EstaAutorizada(string Arquivo, DateTime Emissao)
        {
            string strNomePastaEnviado = ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.Autorizados.ToString() + "\\" + ConfiguracaoApp.DiretorioSalvarComo.ToString(Emissao);
            return File.Exists(strNomePastaEnviado + "\\" + oAux.ExtrairNomeArq(Arquivo, ExtXml.Nfe) + ExtXmlRet.ProcNFe);
        }

        private bool EstaDenegada(string Arquivo, DateTime Emissao)
        {
            string strNomePastaEnviado = ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.Denegados.ToString() + "\\" + ConfiguracaoApp.DiretorioSalvarComo.ToString(Emissao);
            return File.Exists(strNomePastaEnviado + "\\" + oAux.ExtrairNomeArq(Arquivo, ExtXml.Nfe) + "-den.xml");
        }

        private bool LerRetornoSit(string strArquivoNFe, string ArqXMLRetorno)
        {
            bool rst = false;
            try
            {
                XmlDocument docretConsSit = new XmlDocument();
                docretConsSit.Load(ArqXMLRetorno);

                XmlNodeList retConsSitList = docretConsSit.GetElementsByTagName("retConsSitNFe");
                if (retConsSitList != null)
                {
                    if (retConsSitList.Count > 0)
                    {
                        XmlElement retConsSitElemento = (XmlElement)retConsSitList.Item(0);
                        if (retConsSitElemento != null)
                        {
                            if (retConsSitElemento.ChildNodes.Count > 0)
                            {
                                XmlNodeList infConsSitList = retConsSitElemento.GetElementsByTagName("infProt");
                                if (infConsSitList != null)
                                {
                                    foreach (XmlNode infConsSitNode in infConsSitList)
                                    {
                                        XmlElement infConsSitElemento = (XmlElement)infConsSitNode;

                                        //O retorno da consulta situação a posição das tag´s é diferente do que vem 
                                        //na consulta do recibo, assim sendo tenho que montar esta parte do XML manualmente
                                        //para que fique um XML de distribuição válido. Wandrey 07/10/2009
                                        string strProtNfe = "<protNFe versao=\"1.10\">" +
                                            "<infProt>" +
                                            "<tpAmb>" + oAux.LerTag(infConsSitElemento, "tpAmb", false) + "</tpAmb>" +
                                            "<verAplic>" + oAux.LerTag(infConsSitElemento, "verAplic", false) + "</verAplic>" +
                                            "<chNFe>" + oAux.LerTag(infConsSitElemento, "chNFe", false) + "</chNFe>" +
                                            "<dhRecbto>" + oAux.LerTag(infConsSitElemento, "dhRecbto", false) + "</dhRecbto>" +
                                            "<nProt>" + oAux.LerTag(infConsSitElemento, "nProt", false) + "</nProt>" +
                                            "<digVal>" + oAux.LerTag(infConsSitElemento, "digVal", false) + "</digVal>" +
                                            "<cStat>" + oAux.LerTag(infConsSitElemento, "cStat", false) + "</cStat>" +
                                            "<xMotivo>" + oAux.LerTag(infConsSitElemento, "xMotivo", false) + "</xMotivo>" +
                                            "</infProt>" +
                                            "</protNFe>";

                                        string strStat = oAux.LerTag(infConsSitElemento, "cStat").Replace(";", "");
                                        string strChaveNFe = "NFe" + oAux.LerTag(infConsSitElemento, "chNFe").Replace(";", "");

                                        switch (strStat)
                                        {
                                            case "100":
                                                //Juntar o protocolo com a NFE já copiando para a pasta de autorizadas
                                                oGerarXml.XmlDistNFe(strArquivoNFe, strProtNfe);

                                                //Mover a NFE da pasta de NFE em processamento para NFe Autorizada
                                                oAux.MoverArquivo(strArquivoNFe, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);

                                                //Move a nfeProc da pasta de NFE em processamento para a NFe Autorizada
                                                string strArquivoNFeProc = ConfiguracaoApp.vPastaXMLEnviado + "\\" +
                                                                            PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                                            Path.GetFileName(oAux.ExtrairNomeArq(strArquivoNFe, ExtXml.Nfe) + ExtXmlRet.ProcNFe);
                                                oAux.MoverArquivo(strArquivoNFeProc, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);
   
                                                rst = true;
                                                break;

                                            case "301":
                                            case "302":
                                                //Move a NFE da pasta de NFE em processamento para NFe Denegadas
                                                oAux.MoverArquivo(strArquivoNFe, PastaEnviados.Denegados, oLerXml.oDadosNfe.dEmi);

                                                rst = true;
                                                break;

                                            default:
                                                //Mover o XML da NFE a pasta de XML´s com erro
                                                oAux.MoveArqErro(strArquivoNFe);

                                                //Atualizar a Tag de status da NFe no fluxo para que se ocorrer alguma falha 
                                                //na exclusão eu tenha esta campo para ter uma referencia em futuras consultas
                                                //fluxo.AtualizarTag(strChaveNFe, FluxoNfe.ElementoEditavel.cStat, strStat);

                                                break;
                                        }
                                        // exclui a nota do fluxo
                                        fluxo.ExcluirNfeFluxo(strChaveNFe);
                                    }
                                }
                            }
                        }
                    }
                }
                return rst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void RecebeResposta(string ArqXMLRetorno, string ArqERRRetorno)
        {
            DateTime startTime;
            DateTime stopTime;
            TimeSpan elapsedTime;

            long elapsedMillieconds;
            startTime = DateTime.Now;

            while (true)
            {
                stopTime = DateTime.Now;
                elapsedTime = stopTime.Subtract(startTime);
                elapsedMillieconds = (int)elapsedTime.TotalMilliseconds;

                if (elapsedMillieconds >= 120000) //120.000 ms que corresponde á 120 segundos que corresponde a 2 minutos
                {
                    break;
                }

                if (File.Exists(ArqXMLRetorno))
                {
                    if (!Auxiliar.FileInUse(ArqXMLRetorno))
                    {
                        break;
                    }
                }
                else if (File.Exists(ArqERRRetorno))
                {
                    if (!Auxiliar.FileInUse(ArqERRRetorno))
                    {
                        break;
                    }
                }
                Thread.Sleep(5000);
            }
        }
    }
}
