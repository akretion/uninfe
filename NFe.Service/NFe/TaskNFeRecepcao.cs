﻿using System;
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
    public class TaskNFeRecepcao : TaskAbst
    {
        public TaskNFeRecepcao()
        {
            Servico = Servicos.EnviarLoteNfe;
        }

        #region Classe com os dados do XML do retorno do envio do Lote de NFe
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do recibo do lote
        /// </summary>
        private DadosRecClass oDadosRec;
        #endregion

        #region Execute
        public override void Execute()
        {
            int emp = Functions.FindEmpresaByThread();

            try
            {
                oDadosRec = new DadosRecClass();
                FluxoNfe oFluxoNfe = new FluxoNfe();
                LerXML oLer = new LerXML();

                #region Parte que envia o lote
                //Ler o XML de Lote para pegar o número do lote que está sendo enviado
                oLer.Nfe(NomeArquivoXML);

                if (oLer.oDadosNfe.versao != "2.00")
                {
                    Servico = Servicos.EnviarLoteNfe2;
                }

                var idLote = oLer.oDadosNfe.idLote;

                //Definir o objeto do WebService
                WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, Convert.ToInt32(oLer.oDadosNfe.cUF), Convert.ToInt32(oLer.oDadosNfe.tpAmb), Convert.ToInt32(oLer.oDadosNfe.tpEmis));

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                object oRecepcao = wsProxy.CriarObjeto(NomeClasseWS(Servico, Convert.ToInt32(oLer.oDadosNfe.cUF)));
                var oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS(Convert.ToInt32(oLer.oDadosNfe.cUF), Servico));

                //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                wsProxy.SetProp(oCabecMsg, "cUF", oLer.oDadosNfe.cUF);
                wsProxy.SetProp(oCabecMsg, "versaoDados", oLer.oDadosNfe.versao);

                //
                //XML neste ponto a NFe já está assinada, pois foi assinada, validada e montado o lote para envio por outro serviço. 
                //Fica aqui somente este lembrete. Wandrey 16/03/2010
                //

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.Invocar(wsProxy, oRecepcao, NomeMetodoWS(Servico, Convert.ToInt32(oLer.oDadosNfe.cUF)), oCabecMsg, this, "-env-lot", "-rec");
                #endregion

                #region Parte que trata o retorno do lote, ou seja, o número do recibo
                //Ler o XML de retorno com o recibo do lote enviado
                //var oLerRecibo = new LerXML();
                /*oLerRecibo.*/
                Recibo(vStrXmlRetorno);

                if (/*oLerRecibo.*/oDadosRec.cStat == "103") //Lote recebido com sucesso
                {
                    //Atualizar o número do recibo no XML de controle do fluxo de notas enviadas
                    oFluxoNfe.AtualizarTag(oLer.oDadosNfe.chavenfe, FluxoNfe.ElementoEditavel.tMed, /*oLerRecibo.*/oDadosRec.tMed.ToString());
                    oFluxoNfe.AtualizarTagRec(idLote, /*oLerRecibo.*/oDadosRec.nRec);
                }
                else if (Convert.ToInt32(/*oLerRecibo.*/oDadosRec.cStat) > 200 ||
                         Convert.ToInt32(/*oLerRecibo.*/oDadosRec.cStat) == 108 || //Verifica se o servidor de processamento está paralisado momentaneamente. Wandrey 13/04/2012
                         Convert.ToInt32(/*oLerRecibo.*/oDadosRec.cStat) == 109) //Verifica se o servidor de processamento está paralisado sem previsão. Wandrey 13/04/2012              
                {
                    //Se o status do retorno do lote for maior que 200 ou for igual a 108 ou 109, 
                    //vamos ter que excluir a nota do fluxo, porque ela foi rejeitada pelo SEFAZ
                    //Primeiro vamos mover o xml da nota da pasta EmProcessamento para pasta de XML´s com erro e depois tira ela do fluxo
                    //Wandrey 30/04/2009
                    oAux.MoveArqErro(Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + oFluxoNfe.LerTag(oLer.oDadosNfe.chavenfe, FluxoNfe.ElementoFixo.ArqNFe));
                    oFluxoNfe.ExcluirNfeFluxo(oLer.oDadosNfe.chavenfe);
                }

                //Deleta o arquivo de lote
                Functions.DeletarArquivo(NomeArquivoXML);
                #endregion
            }
            catch (ExceptionEnvioXML ex)
            {
                //Ocorreu algum erro no exato momento em que tentou enviar o XML para o SEFAZ, vou ter que tratar
                //para ver se o XML chegou lá ou não, se eu consegui pegar o número do recibo de volta ou não, etc.
                //E ver se vamos tirar o XML do Fluxo ou finalizar ele com a consulta situação da NFe

                //TODO: V3.0 - Tratar o problema de não conseguir pegar o recibo exatamente neste ponto

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.EnvLot, Propriedade.ExtRetorno.Rec_ERR, ex);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 16/03/2010
                }
            }
            catch (ExceptionSemInternet ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.EnvLot, Propriedade.ExtRetorno.Rec_ERR, ex, false);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 16/03/2010
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.EnvLot, Propriedade.ExtRetorno.Rec_ERR, ex);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 16/03/2010
                }
            }
        }
        #endregion

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
        private void Recibo(string strXml)
        {
            MemoryStream memoryStream = Functions.StringXmlToStream(strXml);

            this.oDadosRec.cStat = string.Empty;
            this.oDadosRec.nRec = string.Empty;
            this.oDadosRec.tMed = 0;

            XmlDocument xml = new XmlDocument();
            xml.Load(memoryStream);

            XmlNodeList retEnviNFeList = xml.GetElementsByTagName("retEnviNFe");

            foreach (XmlNode retEnviNFeNode in retEnviNFeList)
            {
                XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeNode;

                this.oDadosRec.cStat = retEnviNFeElemento.GetElementsByTagName("cStat")[0].InnerText;

                XmlNodeList infRecList = xml.GetElementsByTagName("infRec");

                foreach (XmlNode infRecNode in infRecList)
                {
                    XmlElement infRecElemento = (XmlElement)infRecNode;

                    this.oDadosRec.nRec = infRecElemento.GetElementsByTagName("nRec")[0].InnerText;
                    this.oDadosRec.tMed = Convert.ToInt32(infRecElemento.GetElementsByTagName("tMed")[0].InnerText);
                }
            }
        }
        #endregion
    }
}
