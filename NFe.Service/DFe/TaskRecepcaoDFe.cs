using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

using NFe.Components;
using NFe.Settings;
using NFe.Certificado;
using NFe.Exceptions;

namespace NFe.Service
{
    public class TaskRecepcaoDFe : TaskAbst
    {
        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();
            distDFeInt _distDFeInt = new distDFeInt();

            Servico = Servicos.EnviarDFe;
            try
            {
                if (!this.vXmlNfeDadosMsgEhXML)
                {
                    ///versao|1.00
                    ///tpAmb|1
                    ///cUFAutor|35
                    ///CNPJ|
                    /// ou
                    ///CPF|
                    ///ultNSU|123456789012345
                    /// ou
                    ///NSU|123456789012345
                    List<string> cLinhas = Functions.LerArquivo(this.NomeArquivoXML);
                    Functions.PopulateClasse(_distDFeInt, cLinhas);

                    string f = System.IO.Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml";

                    if (NomeArquivoXML.IndexOf(Empresas.Configuracoes[emp].PastaValidar, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        f = Path.Combine(Empresas.Configuracoes[emp].PastaValidar, f);
                    }
                    // Gerar o XML de envio de DFe a partir do TXT gerado pelo ERP
                    oGerarXML.RecepcaoDFe(f, _distDFeInt);
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(this.NomeArquivoXML);

                    XmlNodeList consdistDFeIntList = doc.GetElementsByTagName("distDFeInt");

                    foreach (XmlNode consdistDFeIntNode in consdistDFeIntList)
                    {
                        XmlElement consdistDFeIntElemento = (XmlElement)consdistDFeIntNode;
                        Functions.PopulateClasse(_distDFeInt, consdistDFeIntElemento);
                    }

                    int cUF = 91;

                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy =
                        ConfiguracaoApp.DefinirWS(Servico,
                                                    emp,
                                                    cUF,
                                                    _distDFeInt.tpAmb);

                    object oConsNFDestEvento = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                    object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(cUF, Servico));

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "cUF", cUF.ToString("00"));
                    wsProxy.SetProp(oCabecMsg, "versaoDados", NFe.ConvertTxt.versoes.VersaoXMLEnvDFe);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy,
                                        oConsNFDestEvento,
                                        wsProxy.NomeMetodoWS[0],
                                        oCabecMsg,
                                        this,
                                        Propriedade.ExtEnvio.EnvDFe_XML.Replace(".xml", ""),
                                        Propriedade.ExtRetorno.retEnvDFe_XML.Replace(".xml", ""));
                }
            }
            catch (Exception ex)
            {
                var extRet = vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.EnvDFe_XML : Propriedade.ExtEnvio.EnvDFe_TXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, extRet, Propriedade.ExtRetorno.retEnvDFe_ERR, ex);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 09/03/2010
                }
            }
            finally
            {
                try
                {
                    //Deletar o arquivo de solicitação do serviço
                    Functions.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                }
            }
        }
    }
}
