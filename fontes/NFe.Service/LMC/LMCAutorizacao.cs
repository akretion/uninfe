using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using NFe.Settings;
using NFe.Components;
using NFe.Exceptions;
using NFe.Certificado;

namespace NFe.Service
{
    /// <summary>
    /// Classe para envio do XML de LMC para a SEFAZ
    /// </summary>
    class TaskLMCAutorizacao : TaskAbst
    {
        /// <summary>
        /// Conteúdo de algumas tags do XML de LMC
        /// </summary>
        private DadosLMC dadosLMC;

        public TaskLMCAutorizacao()
        {
            Servico = Servicos.LMCAutorizacao;
        }

        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                //Leitura dos dados do XML
                dadosLMC = new DadosLMC();
                XmlLMC(emp, dadosLMC);

                //Criar objeto para envio do XML
                WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, dadosLMC.cUF, Empresas.Configuracoes[emp].AmbienteCodigo);
                object oAutorizacao = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);

                //Assinar o XML
                new AssinaturaDigital().Assinar(NomeArquivoXML, emp, Empresas.Configuracoes[emp].UnidadeFederativaCodigo);

                //Enviar o XML
                oInvocarObj.Invocar(wsProxy,
                                    oAutorizacao,
                                    wsProxy.NomeMetodoWS[0],
                                    null,
                                    this);

                LerRetornoLMC(dadosLMC);

                oGerarXML.XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.LMC).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.LMC).RetornoXML, vStrXmlRetorno);
            }
            catch (ExceptionEnvioXML ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.LMC).EnvioXML, Propriedade.ExtRetorno.LMCRet_ERR, ex, false);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                }
            }
            catch (ExceptionSemInternet ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.LMC).EnvioXML, Propriedade.ExtRetorno.LMCRet_ERR, ex, false);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.LMC).EnvioXML, Propriedade.ExtRetorno.LMCRet_ERR, ex, false);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
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

        #region LerRetornoLMC()
        /// <summary>
        /// Le o retorno do LMC, e de acordo com o status guarda o XML enviado na pasta enviados
        /// </summary>
        private void LerRetornoLMC(DadosLMC dadosLMC)
        {
            int emp = Empresas.FindEmpresaByThread();
            var doc = new XmlDocument();
            doc.Load(Functions.StringXmlToStreamUTF8(vStrXmlRetorno));

            XmlElement protLivroCombustivel = (XmlElement)doc.GetElementsByTagName(TpcnResources.protLivroCombustivel.ToString())[0];
            string cStat = protLivroCombustivel.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText;

            if (cStat == "1001" || cStat == "100" || cStat == "101")
            {
                var arquivoNFeProc = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                        PastaEnviados.EmProcessamento.ToString() + "\\" +
                        Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.LMC).EnvioXML) + Propriedade.ExtRetorno.ProcLMC;

                var protLMC = protLivroCombustivel.OuterXml;
                if (!File.Exists(arquivoNFeProc))
                {
                    oGerarXML.XmlDistLMC(NomeArquivoXML, protLMC, Propriedade.ExtRetorno.ProcLMC);
                }

                if (!oAux.EstaAutorizada(NomeArquivoXML, dadosLMC.dEmissao, Propriedade.Extensao(Propriedade.TipoEnvio.LMC).EnvioXML, Propriedade.ExtRetorno.ProcLMC))
                {
                    TFunctions.MoverArquivo(arquivoNFeProc, PastaEnviados.Autorizados, dadosLMC.dEmissao);
                }

                if (!oAux.EstaAutorizada(NomeArquivoXML, dadosLMC.dEmissao, Propriedade.Extensao(Propriedade.TipoEnvio.LMC).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.LMC).EnvioXML))
                {
                    TFunctions.MoverArquivo(NomeArquivoXML, PastaEnviados.Autorizados, dadosLMC.dEmissao);
                }
            }
        }
        #endregion
    }
}