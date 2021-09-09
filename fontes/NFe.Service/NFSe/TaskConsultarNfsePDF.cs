using NFe.Certificado;
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;
using System.Xml;

namespace NFe.Service.NFSe
{
    public class TaskConsultarNfsePDF: TaskAbst
    {
        #region Objeto com os dados do XML da consulta nfse
        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s da consulta nfse
        /// </summary>
        private DadosPedSitNfse oDadosPedNfsePDF;
        #endregion

        #region Execute
        public override void Execute()
        {
            var emp = Empresas.FindEmpresaByThread();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.NFSeConsultarNFSePDF;

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                                         Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).EnvioXML) +
                                         Propriedade.ExtRetorno.NFSePDF_ERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                oDadosPedNfsePDF = new DadosPedSitNfse(emp);
                var padraoNFSe = Functions.PadraoNFSe(oDadosPedNfsePDF.cMunicipio);

                switch(padraoNFSe)
                {
                    case PadroesNFSe.PRODATA:
                        ExecuteDLL(emp, oDadosPedNfsePDF.cMunicipio, padraoNFSe);
                        break;

                    default:
                        var wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedNfsePDF.cMunicipio, oDadosPedNfsePDF.tpAmb, oDadosPedNfsePDF.tpEmis, padraoNFSe, oDadosPedNfsePDF.cMunicipio);
                        var pedNfsePNG = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                        var cabecMsg = "";

                        var securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(oDadosPedNfsePDF.cMunicipio, oDadosPedNfsePDF.tpAmb, oDadosPedNfsePDF.tpEmis, padraoNFSe, Servico);

                        var ad = new AssinaturaDigital();
                        ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(oDadosPedNfsePDF.cMunicipio));

                        oInvocarObj.InvocarNFSe(wsProxy, pedNfsePNG, NomeMetodoWS(Servico, oDadosPedNfsePDF.cMunicipio), cabecMsg, this,
                                                Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).EnvioXML,
                                                Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).RetornoXML,
                                                padraoNFSe, Servico, securityProtocolType);

                        ConvertBase64ToPDF(emp, padraoNFSe);

                        var filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                        Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).EnvioXML) +
                                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).RetornoXML);
                        if(File.Exists(filenameFTP))
                        {
                            new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                        }

                        break;
                }
            }
            catch(Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML,
                                                    Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).EnvioXML,
                                                    Propriedade.ExtRetorno.NFSePDF_ERR, ex);
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
        #endregion

        #region ConvertBase64ToPDF()
        /// <summary>
        /// Converte o retorno em Base64 para PDF e grava o PDF na pasta de retorno com o mesmo nome do retorno em XML, modificando somente a extensão para PDF.
        /// </summary>
        /// <param name="emp">Código da empresa</param>
        /// <param name="padraoNFSe">Padrão da NFSe</param>
        public void ConvertBase64ToPDF(int emp, PadroesNFSe padraoNFSe)
        {
            if(padraoNFSe != PadroesNFSe.GIF)
            {
                return;
            }

            var arqPDF = Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).EnvioXML) +
                            Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).RetornoXML;
            arqPDF = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, arqPDF.Replace(".xml", ".pdf"));

            if(File.Exists(arqPDF))
            {
                File.Delete(arqPDF);
            }

            BinaryWriter writer = null;

            try
            {
                var base64BinaryStr = "";

                var msXml = Functions.StringXmlToStream(vStrXmlRetorno);
                var doc = new XmlDocument();
                doc.Load(msXml);

                switch(padraoNFSe)
                {
                    case PadroesNFSe.GIF:
                        if(doc.GetElementsByTagName("NFS-ePDF")[0] != null)
                        {
                            base64BinaryStr = doc.GetElementsByTagName("NFS-ePDF")[0].InnerText;
                        }
                        else
                        {
                            throw new Exception("Não foi possível localizar a tag <NFS-ePDF> no xml retornado pela prefeitura, sendo assim, o arquivo PDF da NFS-e não foi gerado.");
                        }

                        break;
                }

                var sPDFDecoded = Convert.FromBase64String(base64BinaryStr);

                writer = new BinaryWriter(File.Open(arqPDF, FileMode.CreateNew));
                writer.Write(sPDFDecoded);
            }
            finally
            {
                if(writer != null)
                {
                    writer.Close();
                }
            }
        }
        #endregion

        /// <summary>
        /// Executa o serviço utilizando a DLL do UniNFe.
        /// </summary>
        /// <param name="emp">Empresa que está enviando o XML</param>
        /// <param name="municipio">Código do município para onde será enviado o XML</param>
        /// <param name="padraoNFSe">Padrão do munípio para NFSe</param>
        private void ExecuteDLL(int emp, int municipio, PadroesNFSe padraoNFSe)
        {
            var conteudoXML = new XmlDocument();
            conteudoXML.Load(NomeArquivoXML);

            var finalArqEnvio = Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).EnvioXML;
            var finalArqRetorno = Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).RetornoXML;
            var servico = DefinirServico(municipio, conteudoXML, padraoNFSe);
            var versaoXML = DefinirVersaoXML(municipio, conteudoXML, padraoNFSe);

            Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" + Functions.ExtrairNomeArq(NomeArquivoXML, finalArqEnvio) + Functions.ExtractExtension(finalArqRetorno) + ".err");

            var configuracao = new Unimake.Business.DFe.Servicos.Configuracao
            {
                TipoDFe = Unimake.Business.DFe.Servicos.TipoDFe.NFSe,
                CertificadoDigital = Empresas.Configuracoes[emp].X509Certificado,
                TipoAmbiente = (Unimake.Business.DFe.Servicos.TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo,
                CodigoMunicipio = municipio,
                Servico = servico,
                SchemaVersao = versaoXML
            };

            var consultarNfsePDF = new Unimake.Business.DFe.Servicos.NFSe.ConsultarNfsePDF(conteudoXML, configuracao);
            consultarNfsePDF.Executar();
            vStrXmlRetorno = consultarNfsePDF.RetornoWSString;

            XmlRetorno(finalArqEnvio, finalArqRetorno);

            ExtrairPDF(emp, consultarNfsePDF, padraoNFSe);

            /// grava o arquivo no FTP
            var filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).RetornoXML);

            if(File.Exists(filenameFTP))
            {
                new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
            }
        }

        /// <summary>
        /// Define qual o tipo de serviço de envio de NFSe será utilizado. Envio em lote sincrono, Envio em lote assincrono ou envio de uma única NFSe síncrono.
        /// </summary>
        /// <param name="municipio">Código do município para onde será enviado o XML</param>
        /// <param name="doc">Conteúdo do XML da NFSe</param>
        /// <param name="padraoNFSe">Padrão do munípio para NFSe</param>
        /// <returns>Retorna o tipo de serviço de envio da NFSe da prefeitura será utilizado</returns>
        private Unimake.Business.DFe.Servicos.Servico DefinirServico(int municipio, XmlDocument doc, PadroesNFSe padraoNFSe)
        {
            var result = Unimake.Business.DFe.Servicos.Servico.NFSeRecepcionarLoteRps;

            switch(padraoNFSe)
            {
                case PadroesNFSe.PRODATA:
                    result = Unimake.Business.DFe.Servicos.Servico.NFSeConsultarNfsePDF;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Retorna a versão do XML que está sendo enviado para o município de acordo com o Padrão/Município
        /// </summary>
        /// <param name="codMunicipio">Código do município para onde será enviado o XML</param>
        /// <param name="xmlDoc">Conteúdo do XML da NFSe</param>
        /// <param name="padraoNFSe">Padrão do munípio para NFSe</param>
        /// <returns>Retorna a versão do XML que está sendo enviado para o município de acordo com o Padrão/Município</returns>
        private string DefinirVersaoXML(int codMunicipio, XmlDocument xmlDoc, PadroesNFSe padraoNFSe)
        {
            var versaoXML = "0.00";

            switch(padraoNFSe)
            {
                case PadroesNFSe.PRODATA:
                    versaoXML = "2.01";
                    break;
            }

            return versaoXML;
        }

        /// <summary>
        /// Extrair o PDF retornado pela prefeitura na pasta de retorno
        /// </summary>
        /// <param name="consultarNfsePDF">Objeto do serviço de consulta do PDF da NFSe</param>
        /// <param name="padraoNFSe">Padrão de NFSe do município</param>
        private void ExtrairPDF(int emp, Unimake.Business.DFe.Servicos.NFSe.ConsultarNfsePDF consultarNfsePDF, PadroesNFSe padraoNFSe)
        {
            var nomeTag = string.Empty;

            switch(padraoNFSe)
            {
                case PadroesNFSe.PRODATA:
                    nomeTag = "Base64Pdf";
                    break;
            }

            var arqPDF = (Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).EnvioXML) +
                Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).RetornoXML).Replace(".xml", ".pdf");

            consultarNfsePDF.ExtrairPDF(Empresas.Configuracoes[emp].PastaXmlRetorno, arqPDF, nomeTag);
        }
    }
}
