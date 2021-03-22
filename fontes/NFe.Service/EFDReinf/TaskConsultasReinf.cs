using NFe.Components;
using NFe.Settings;
using System;
using System.IO;
using System.Xml;

namespace NFe.Service
{
    public class TaskConsultasReinf : TaskAbst
    {
        public TaskConsultasReinf(string arquivo)
        {
            Servico = Servicos.ConsultasReinf;

            NomeArquivoXML = arquivo;
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(arquivo);
        }

        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                                         Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.Reinf_cons).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.Reinf_cons).RetornoERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                WebServiceProxy wsProxy = null;
                object Reinf = null;

                wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, 991, Empresas.Configuracoes[emp].AmbienteCodigo, 0);
                if (wsProxy != null)
                    Reinf = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);

                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(991, 1, 0, Servico);

                string nomeMetodo = NomeMetodo();

                oInvocarObj.Invocar(wsProxy,
                                    Reinf,
                                    nomeMetodo,
                                    null,
                                    this,
                                    Propriedade.Extensao(Propriedade.TipoEnvio.Reinf_cons).EnvioXML,
                                    Propriedade.Extensao(Propriedade.TipoEnvio.Reinf_cons).RetornoXML,
                                    true,
                                    securityProtocolType);

                ///
                /// grava o arquivo no FTP
                string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                  Functions.ExtrairNomeArq(NomeArquivoXML,
                                                  Propriedade.Extensao(Propriedade.TipoEnvio.Reinf_cons).EnvioXML) + "\\" + Propriedade.Extensao(Propriedade.TipoEnvio.Reinf_cons).RetornoXML);
                if (File.Exists(filenameFTP))
                    new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.Reinf_cons).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.Reinf_cons).RetornoERR, ex);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 31/08/2011
                }
            }
            finally
            {
                try
                {
                    Functions.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                    //Se falhou algo na hora de deletar o XML de cancelamento de NFe, infelizmente
                    //não posso fazer mais nada, o UniNFe vai tentar mandar o arquivo novamente para o webservice, pois ainda não foi excluido.
                    //Wandrey 31/08/2011
                }
            }
        }

        private string NomeMetodo()
        {
            var result = "";
            var consultaEventoTotalizacao = ConteudoXML.GetElementsByTagName("ConsultaTotalizadores")[0];
            var consultaEvento = ((XmlElement)ConteudoXML.GetElementsByTagName("tipoEvento")[0])?.InnerText;

            if (consultaEventoTotalizacao != null)
                result = "ConsultaInformacoesConsolidadas";

            if (consultaEvento != null)
            {
                switch (consultaEvento)
                {
                    case "1000":
                        result = "ConsultaReciboEvento1000";
                        break;
                    case "1070":
                        result = "ConsultaReciboEvento1070";
                        break;
                    case "2010":
                        result = "ConsultaReciboEvento2010";
                        break;
                    case "2020":
                        result = "ConsultaReciboEvento2020";
                        break;
                    case "2030":
                        result = "ConsultaReciboEvento2030";
                        break;
                    case "2040":
                        result = "ConsultaReciboEvento2040";
                        break;
                    case "2050":
                        result = "ConsultaReciboEvento2050";
                        break;
                    case "2055":
                        result = "ConsultaReciboEvento2055";
                        break;
                    case "2060":
                        result = "ConsultaReciboEvento2060";
                        break;
                    case "2098":
                        result = "ConsultaReciboEvento2098";
                        break;
                    case "2099":
                        result = "ConsultaReciboEvento2099";
                        break;
                    case "3010":
                        result = "ConsultaReciboEvento3010";
                        break;
                    case "4004":
                        result = "ConsultaReciboEvento4004";
                        break;
                    case "4010":
                        result = "ConsultaReciboEvento4010";
                        break;
                    case "4020":
                        result = "ConsultaReciboEvento4020";
                        break;
                    case "4040":
                        result = "ConsultaReciboEvento4040";
                        break;
                    case "4080":
                        result = "ConsultaReciboEvento4080";
                        break;
                    case "4098":
                        result = "ConsultaReciboEvento4098";
                        break;
                    case "4099":
                        result = "ConsultaReciboEvento4099";
                        break;
                }
            }

            return result;
        }
    }
}