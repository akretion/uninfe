using NFe.Certificado;
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;

namespace NFe.Service
{
    public class TaskNFeConsultaStatus : TaskAbst
    {
        public TaskNFeConsultaStatus(string arquivo)
        {
            Servico = Servicos.NFeConsultaStatusServico;
            NomeArquivoXML = arquivo;
            if (vXmlNfeDadosMsgEhXML)
            {
                ConteudoXML.PreserveWhitespace = false;
                ConteudoXML.Load(arquivo);
            }
        }

        #region Classe com os dados do XML da consulta do status do serviço da NFe

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do status do serviço
        /// </summary>
        private DadosPedSta dadosPedSta;

        #endregion Classe com os dados do XML da consulta do status do serviço da NFe

        #region Execute

        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                dadosPedSta = new DadosPedSta();
                //Ler o XML para pegar parâmetros de envio
                PedSta(emp, dadosPedSta);

                if (vXmlNfeDadosMsgEhXML)  //danasa 12-9-2009
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy =
                        ConfiguracaoApp.DefinirWS(Servico,
                        emp,
                        dadosPedSta.cUF,
                        dadosPedSta.tpAmb,
                        dadosPedSta.tpEmis,
                        dadosPedSta.versao,
                        dadosPedSta.mod,
                        0);

                    System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(dadosPedSta.cUF, dadosPedSta.tpAmb, dadosPedSta.tpEmis, Servico);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    var oStatusServico = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);

                    object oCabecMsg = null;
                    if (dadosPedSta.versao != "4.00")
                    {
                        oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(dadosPedSta.cUF, Servico));
                        wsProxy.SetProp(oCabecMsg, TpcnResources.cUF.ToString(), dadosPedSta.cUF.ToString());
                        wsProxy.SetProp(oCabecMsg, TpcnResources.versaoDados.ToString(), dadosPedSta.versao);
                    }

                    new AssinaturaDigital().CarregarPIN(emp, NomeArquivoXML, Servico);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy,
                                        oStatusServico,
                                        wsProxy.NomeMetodoWS[0],
                                        oCabecMsg, this,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).RetornoXML,
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
                    // Gerar o XML de solicitacao de situacao do servico a partir do TXT gerado pelo ERP
                    oGerarXML.StatusServicoNFe(f, dadosPedSta.tpAmb, dadosPedSta.tpEmis, dadosPedSta.cUF, dadosPedSta.versao);
                }
            }
            catch (Exception ex)
            {
                var extRet = vXmlNfeDadosMsgEhXML ? Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML :
                                                    Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioTXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, extRet, Propriedade.ExtRetorno.Sta_ERR, ex);
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

        #endregion Execute

        #region PedSta()

        /// <summary>
        /// Faz a leitura do XML de pedido do status de serviço
        /// </summary>
        /// <param name="cArquivoXml">Nome do XML a ser lido</param>
        /// <by>Wandrey Mundin Ferreira</by>
        ///
        protected override void PedSta(int emp, DadosPedSta dadosPedSta)
        {
            base.PedSta(emp, dadosPedSta);

            if (string.IsNullOrEmpty(dadosPedSta.versao))
                throw new Exception(NFeStrConstants.versaoError);
        }

#if f
        private void PedSta(int emp, string cArquivoXML)
        {
            dadosPedSta.tpAmb = 0;
            dadosPedSta.cUF = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
            dadosPedSta.versao = "";// NFe.ConvertTxt.versoes.VersaoXMLStatusServico;

            ///
            /// danasa 9-2009
            /// Assume o que está na configuracao
            ///
            dadosPedSta.tpEmis = Empresas.Configuracoes[emp].tpEmis;

            ///
            /// danasa 12-9-2009
            ///
            if (Path.GetExtension(cArquivoXML).ToLower() == ".txt")
            {
                // tpEmis|1						<<< opcional >>>
                // tpAmb|1
                // cUF|35
                // versao|3.10
                List<string> cLinhas = Functions.LerArquivo(cArquivoXML);
                Functions.PopulateClasse(dadosPedSta, cLinhas);
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                XmlNodeList consStatServList = doc.GetElementsByTagName("consStatServ");

                foreach (XmlNode consStatServNode in consStatServList)
                {
                    XmlElement consStatServElemento = (XmlElement)consStatServNode;

                    dadosPedSta.tpAmb = Convert.ToInt32("0" + consStatServElemento.GetElementsByTagName(TpcnResources.tpAmb.ToString())[0].InnerText);
                    dadosPedSta.versao = consStatServElemento.Attributes[NFe.Components.TpcnResources.versao.ToString()].InnerText;

                    if (consStatServElemento.GetElementsByTagName(NFe.Components.TpcnResources.cUF.ToString()).Count != 0)
                    {
                        dadosPedSta.cUF = Convert.ToInt32("0" + consStatServElemento.GetElementsByTagName(NFe.Components.TpcnResources.cUF.ToString())[0].InnerText);
                    }

                    bool saveXml = false;

                    if (consStatServElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString()).Count != 0)
                    {
                        dadosPedSta.tpEmis = Convert.ToInt16(consStatServElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString())[0].InnerText);
                        /// para que o validador não rejeite, excluo a tag <tpEmis>
                        doc.DocumentElement.RemoveChild(consStatServElemento.GetElementsByTagName(NFe.Components.TpcnResources.tpEmis.ToString())[0]);
                        saveXml = true;
                    }

                    if (consStatServElemento.GetElementsByTagName(TpcnResources.mod.ToString()).Count != 0)
                    {
                        dadosPedSta.mod = consStatServElemento.GetElementsByTagName(TpcnResources.mod.ToString())[0].InnerText;
                        /// para que o validador não rejeite, excluo a tag <mod>
                        doc.DocumentElement.RemoveChild(consStatServElemento.GetElementsByTagName(TpcnResources.mod.ToString())[0]);
                        saveXml = true;
                    }

                    // salvo o arquivo modificado
                    if (saveXml)
                        doc.Save(cArquivoXML);
                }
            }
            if (string.IsNullOrEmpty(dadosPedSta.versao))
                throw new Exception(NFeStrConstants.versaoError);
        }
#endif

        #endregion PedSta()
    }
}