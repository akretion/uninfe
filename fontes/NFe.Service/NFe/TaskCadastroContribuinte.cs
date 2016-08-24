using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;

using NFe.Components;
using NFe.Settings;
using NFe.Certificado;
using NFe.Exceptions;

namespace NFe.Service
{
    public class TaskCadastroContribuinte : TaskAbst
    {
        public TaskCadastroContribuinte()
        {
            Servico = Servicos.ConsultaCadastroContribuinte;
        }

        #region Classe com os Dados do XML da Consulta Cadastro do Contribuinte
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta do cadastro do contribuinte
        /// </summary>
        private DadosConsCad dadosConsCad;
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                dadosConsCad = new DadosConsCad();
                //Ler o XML para pegar parâmetros de envio
                ConsCad(NomeArquivoXML);

                if (this.vXmlNfeDadosMsgEhXML)  //danasa 12-9-2009
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, dadosConsCad.cUF, dadosConsCad.tpAmb, (int)NFe.Components.TipoEmissao.teNormal, dadosConsCad.versao, 0);
                    System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(dadosConsCad.cUF, dadosConsCad.tpAmb, (int)TipoEmissao.teNormal, PadroesNFSe.NaoIdentificado, Servico);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oConsCad = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                    object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(dadosConsCad.cUF, Servico));

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, NFe.Components.TpcnResources.cUF.ToString(), dadosConsCad.cUF.ToString());
                    wsProxy.SetProp(oCabecMsg, NFe.Components.TpcnResources.versaoDados.ToString(), this.dadosConsCad.versao);

                    new AssinaturaDigital().CarregarPIN(emp, NomeArquivoXML, Servico);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy,
                                        oConsCad,
                                        wsProxy.NomeMetodoWS[0],
                                        oCabecMsg,
                                        this,
                                        NFe.Components.Propriedade.Extensao(Propriedade.TipoEnvio.ConsCad).EnvioXML,
                                        NFe.Components.Propriedade.Extensao(Propriedade.TipoEnvio.ConsCad).RetornoXML,
                                        true,
                                        securityProtocolType);
                }
                else
                {
                    string f = Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml";

                    if (NomeArquivoXML.IndexOf(Empresas.Configuracoes[emp].PastaValidar, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        f = Path.Combine(Empresas.Configuracoes[emp].PastaValidar, f);
                    }
                    //Gerar o XML da consulta cadastro do contribuinte a partir do TXT gerado pelo ERP
                    GravarXml(f);
                }
            }
            catch (Exception ex)
            {
                string ExtEnvio = string.Empty;

                if (this.vXmlNfeDadosMsgEhXML) //Se for XML
                    ExtEnvio = Propriedade.Extensao(Propriedade.TipoEnvio.ConsCad).EnvioXML;
                else //Se for TXT
                    ExtEnvio = Propriedade.Extensao(Propriedade.TipoEnvio.ConsCad).EnvioTXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, ExtEnvio, Propriedade.ExtRetorno.ConsCad_ERR, ex);
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
                    //Se falhou algo na hora de deletar o XML de solicitação do serviço, 
                    //infelizmente não posso fazer mais nada, o UniNFe vai tentar mandar 
                    //o arquivo novamente para o webservice
                    //Wandrey 09/03/2010
                }
            }
        }
        #endregion

        #region Gravar consCad
        public string GravarXml(string arquivo)
        {
            return oGerarXML.ConsultaCadastro(arquivo,
                dadosConsCad.UF,
                dadosConsCad.CNPJ,
                dadosConsCad.IE,
                dadosConsCad.CPF,
                dadosConsCad.versao);
        }
        #endregion

        #region ConsCad()
        /// <summary>
        /// Faz a leitura do XML de consulta do cadastro do contribuinte e disponibiliza os valores de algumas tag´s
        /// </summary>
        /// <param name="cArquivoXML">Caminho e nome do arquivo XML da consulta do cadastro do contribuinte a ser lido</param>
        private void ConsCad(string cArquivoXML)
        {
            this.dadosConsCad.CNPJ = string.Empty;
            this.dadosConsCad.IE = string.Empty;
            this.dadosConsCad.UF = string.Empty;
            this.dadosConsCad.versao = "";

            if (Path.GetExtension(cArquivoXML).ToLower() == ".txt")
            {
                List<string> cLinhas = Functions.LerArquivo(cArquivoXML);
                Functions.PopulateClasse(this.dadosConsCad, cLinhas);
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                XmlNodeList ConsCadList = doc.GetElementsByTagName("ConsCad");
                foreach (XmlNode ConsCadNode in ConsCadList)
                {
                    XmlElement ConsCadElemento = (XmlElement)ConsCadNode;

                    this.dadosConsCad.versao = ConsCadElemento.Attributes[NFe.Components.TpcnResources.versao.ToString()].InnerText;

                    XmlNodeList infConsList = ConsCadElemento.GetElementsByTagName("infCons");

                    foreach (XmlNode infConsNode in infConsList)
                    {
                        XmlElement infConsElemento = (XmlElement)infConsNode;
                        Functions.PopulateClasse(this.dadosConsCad, infConsElemento);
                    }
                }
            }
            if (this.dadosConsCad.versao == "")
                throw new Exception(NFeStrConstants.versaoError);
        }
        #endregion

    }
}
