﻿using NFe.Components;
using NFe.Exceptions;
using NFe.Settings;
using System;
using System.IO;
using System.Threading;
using System.Xml;
using Unimake.Business.DFe.Servicos;
using Unimake.Business.DFe.Xml.MDFe;

namespace NFe.Service
{
    public class TaskMDFeRecepcao: TaskAbst
    {
        public TaskMDFeRecepcao(string arquivo)
        {
            var emp = Empresas.FindEmpresaByThread();

            if(Empresas.Configuracoes[emp].IndSincMDFe)
            {
                Servico = Servicos.MDFeEnviarLoteSinc;
            }
            else
            {
                Servico = Servicos.MDFeEnviarLote;
            }

            NomeArquivoXML = arquivo;
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(arquivo);
        }

        public TaskMDFeRecepcao(XmlDocument conteudoXML)
        {
            var emp = Empresas.FindEmpresaByThread();

            if(Empresas.Configuracoes[emp].IndSincMDFe)
            {
                Servico = Servicos.MDFeEnviarLoteSinc;
            }
            else
            {
                Servico = Servicos.MDFeEnviarLote;
            }

            ConteudoXML = conteudoXML;
            ConteudoXML.PreserveWhitespace = false;
            NomeArquivoXML = Empresas.Configuracoes[emp].PastaXmlEnvio + "\\temp\\" +
                conteudoXML.GetElementsByTagName(TpcnResources.idLote.ToString())[0].InnerText + Propriedade.Extensao(Propriedade.TipoEnvio.EnvLot).EnvioXML;
        }

        #region Classe com os dados do XML do retorno do envio do Lote de NFe

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do recibo do lote
        /// </summary>
        private DadosRecClass dadosRec;

        #endregion Classe com os dados do XML do retorno do envio do Lote de NFe

        #region Execute

        public override void Execute()
        {
            var emp = Empresas.FindEmpresaByThread();

            try
            {
                dadosRec = new DadosRecClass();
                var fluxoNfe = new FluxoNfe();
                var lerXml = new LerXML();
                lerXml.Mdfe(ConteudoXML);

                var idLote = lerXml.oDadosNfe.idLote;

                var xmlMDFe = new EnviMDFe();
                xmlMDFe = Unimake.Business.DFe.Utility.XMLUtility.Deserializar<EnviMDFe>(ConteudoXML);

                var configuracao = new Configuracao
                {
                    TipoDFe = TipoDFe.MDFe,
                    CertificadoDigital = Empresas.Configuracoes[emp].X509Certificado
                };

                if(ConfiguracaoApp.Proxy)
                {
                    configuracao.HasProxy = true;
                    configuracao.ProxyAutoDetect = ConfiguracaoApp.DetectarConfiguracaoProxyAuto;
                    configuracao.ProxyUser = ConfiguracaoApp.ProxyUsuario;
                    configuracao.ProxyPassword = ConfiguracaoApp.ProxySenha;
                }

                var autorizacao = new Unimake.Business.DFe.Servicos.MDFe.Autorizacao(xmlMDFe, configuracao);
                autorizacao.Executar();

                ConteudoXML = autorizacao.ConteudoXMLAssinado;

                SalvarArquivoEmProcessamento(emp, lerXml.oDadosNfe.chavenfe);

                vStrXmlRetorno = autorizacao.RetornoWSString;

                if(Servico == Servicos.MDFeEnviarLoteSinc)
                {
                    Protocolo(vStrXmlRetorno);
                }
                else
                {
                    Recibo(vStrXmlRetorno, emp);
                }


                if(dadosRec.cStat == "104") //Lote processado - Envio Síncrono
                {
                    FinalizarNFeSincrono(vStrXmlRetorno, emp, lerXml.oDadosNfe.chavenfe);

                    oGerarXML.XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.EnvLot).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedRec).RetornoXML, vStrXmlRetorno);
                }
                else if(dadosRec.cStat == "103") //Lote recebido com sucesso
                {
                    oGerarXML.XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.EnvLot).EnvioXML, Propriedade.ExtRetorno.Rec, vStrXmlRetorno);

                    if(dadosRec.tMed > 0)
                    {
                        Thread.Sleep(dadosRec.tMed * 1000);
                    }

                    //Atualizar o número do recibo no XML de controle do fluxo de notas enviadas
                    fluxoNfe.AtualizarTag(lerXml.oDadosNfe.chavenfe, FluxoNfe.ElementoEditavel.tMed, (dadosRec.tMed + 2).ToString());
                    fluxoNfe.AtualizarTagRec(idLote, dadosRec.nRec);
                    var xmlPedRec = oGerarXML.XmlPedRecMDFe(dadosRec.nRec, dadosRec.versao, emp);
                    var mdfeRetRecepcao = new TaskMDFeRetRecepcao(xmlPedRec);
                    mdfeRetRecepcao.Execute();
                }
                else if(Convert.ToInt32(dadosRec.cStat) > 200 ||
                    Convert.ToInt32(dadosRec.cStat) == 108 || //Verifica se o servidor de processamento está paralisado momentaneamente. Wandrey 13/04/2012
                    Convert.ToInt32(dadosRec.cStat) == 109) //Verifica se o servidor de processamento está paralisado sem previsão. Wandrey 13/04/2012
                {
                    //Se o status do retorno do lote for maior que 200 ou for igual a 108 ou 109,
                    //vamos ter que excluir a nota do fluxo, porque ela foi rejeitada pelo SEFAZ
                    //Primeiro vamos mover o xml da nota da pasta EmProcessamento para pasta de XML´s com erro e depois tira ela do fluxo
                    //Wandrey 30/04/2009
                    oAux.MoveArqErro(Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + fluxoNfe.LerTag(lerXml.oDadosNfe.chavenfe, FluxoNfe.ElementoFixo.ArqNFe));
                    fluxoNfe.ExcluirNfeFluxo(lerXml.oDadosNfe.chavenfe);
                }

                //Deleta o arquivo de lote
                Functions.DeletarArquivo(NomeArquivoXML);
            }
            catch(ExceptionEnvioXML ex)
            {
                //Ocorreu algum erro no exato momento em que tentou enviar o XML para o SEFAZ, vou ter que tratar
                //para ver se o XML chegou lá ou não, se eu consegui pegar o número do recibo de volta ou não, etc.
                //E ver se vamos tirar o XML do Fluxo ou finalizar ele com a consulta situação da NFe

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLot).EnvioXML, Propriedade.ExtRetorno.Rec_ERR, ex);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 16/03/2010
                }
            }
            catch(ExceptionSemInternet ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLot).EnvioXML, Propriedade.ExtRetorno.Rec_ERR, ex, false);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 16/03/2010
                }
            }
            catch(Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLot).EnvioXML, Propriedade.ExtRetorno.Rec_ERR, ex);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 16/03/2010
                }
            }
        }

        /// <summary>
        /// Salvar o arquivo do MDFe assinado na pasta EmProcessamento
        /// </summary>
        /// <param name="emp">Codigo da empresa</param>
        /// <param name="chaveMDFe">Chave do MDFe</param>
        private void SalvarArquivoEmProcessamento(int emp, string chaveMDFe)
        {
            Empresas.Configuracoes[emp].CriarSubPastaEnviado();

            var fluxoNFe = new FluxoNfe();
            var nomeArqMDFe = fluxoNFe.LerTag(chaveMDFe, FluxoNfe.ElementoFixo.ArqNFe);

            var arqEmProcessamento = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + nomeArqMDFe;
            var sw = File.CreateText(arqEmProcessamento);
            sw.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + ConteudoXML.GetElementsByTagName("MDFe")[0].OuterXml);
            sw.Close();

            if(File.Exists(arqEmProcessamento))
            {
                File.Delete(Empresas.Configuracoes[emp].PastaXmlEnvio + "\\temp\\" + nomeArqMDFe);
            }
        }

        #endregion Execute

        #region Recibo

        /// <summary>
        /// Faz a leitura do XML do Recibo do lote enviado e disponibiliza os valores
        /// de algumas tag´s
        /// </summary>
        /// <param name="cArquivoXML">Caminho e nome do arquivo XML da NFe a ser lido</param>
        /// <param name="strXml">String contendo o XML</param>
        /// <example>
        /// UniLerXmlClass oLerXml = new UniLerXmlClass();
        /// oLerXml.Recibo( strXml );
        /// String nRec = oLerXml.oDadosRec.nRec;
        /// </example>
        /// <remarks>
        /// Gera exception em caso de problemas na leitura
        /// </remarks>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        private void Recibo(string strXml, int emp)
        {
            dadosRec.cStat = dadosRec.nRec = string.Empty;
            dadosRec.tMed = 0;

            var xml = new XmlDocument();
            xml.Load(Functions.StringXmlToStream(strXml));

            var retEnviNFeList = xml.GetElementsByTagName("retEnviMDFe");

            foreach(XmlNode retEnviNFeNode in retEnviNFeList)
            {
                var retEnviNFeElemento = (XmlElement)retEnviNFeNode;

                dadosRec.cStat = retEnviNFeElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText;
                dadosRec.versao = retEnviNFeElemento.Attributes[TpcnResources.versao.ToString()].InnerText;

                var infRecList = xml.GetElementsByTagName("infRec");

                foreach(XmlNode infRecNode in infRecList)
                {
                    var infRecElemento = (XmlElement)infRecNode;

                    dadosRec.nRec = infRecElemento.GetElementsByTagName(TpcnResources.nRec.ToString())[0].InnerText;
                    dadosRec.tMed = Convert.ToInt32(infRecElemento.GetElementsByTagName(TpcnResources.tMed.ToString())[0].InnerText);

                    if(dadosRec.tMed > 15)
                    {
                        dadosRec.tMed = 15;
                    }

                    if(dadosRec.tMed <= 0)
                    {
                        dadosRec.tMed = Empresas.Configuracoes[emp].TempoConsulta;
                    }
                }
            }
        }

        #endregion Recibo

        #region Protocolo()

        /// <summary>
        /// Faz leitura do protocolo quando configurado para processo Síncrono
        /// </summary>
        /// <param name="xmlProtocolo">String contendo o XML do protocolo retornado</param>
        private void Protocolo(string xmlProtocolo)
        {
            dadosRec.cStat = dadosRec.nRec = string.Empty;
            dadosRec.tMed = 0;

            var xml = new XmlDocument();
            xml.Load(Functions.StringXmlToStream(xmlProtocolo));

            var retEnviNFeList = xml.GetElementsByTagName(xml.FirstChild.Name);

            foreach(XmlNode retEnviNFeNode in retEnviNFeList)
            {
                var retEnviNFeElemento = (XmlElement)retEnviNFeNode;

                dadosRec.cStat = retEnviNFeElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText;

                if(retEnviNFeElemento.GetElementsByTagName(TpcnResources.nRec.ToString())[0] != null)
                {
                    dadosRec.nRec = retEnviNFeElemento.GetElementsByTagName(TpcnResources.nRec.ToString())[0].InnerText;
                }
            }
        }

        #endregion Protocolo()

        #region FinalizarNFeSincrono()

        /// <summary>
        /// Finalizar a NFe no processo Síncrono
        /// </summary>
        /// <param name="xmlRetorno">Conteúdo do XML retornado da SEFAZ</param>
        /// <param name="emp">Código da empresa para buscar as configurações</param>
        private void FinalizarNFeSincrono(string xmlRetorno, int emp, string chMDFe)
        {
            var xml = new XmlDocument();
            xml.Load(Functions.StringXmlToStream(xmlRetorno));

            var protMDFe = xml.GetElementsByTagName("protMDFe");

            var fluxoNFe = new FluxoNfe();

            var retRecepcao = new TaskMDFeRetRecepcao
            {
                chMDFe = chMDFe
            };

            retRecepcao.FinalizarMDFe(protMDFe, fluxoNFe, emp, ConteudoXML);
        }

        #endregion FinalizarNFeSincrono()
    }
}