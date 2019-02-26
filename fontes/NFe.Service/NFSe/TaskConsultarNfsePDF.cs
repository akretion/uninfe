using NFe.Certificado;
using NFe.Components;
using NFe.Settings;
using System;
using System.IO;
using System.Xml;

namespace NFe.Service.NFSe
{
    public class TaskConsultarNfsePDF : TaskAbst
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
            int emp = Empresas.FindEmpresaByThread();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.NFSeConsultarNFSePDF;

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                                         Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).EnvioXML) +
                                         Propriedade.ExtRetorno.NFSePDF_ERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                oDadosPedNfsePDF = new DadosPedSitNfse(emp);

                //Criar objetos das classes dos serviços dos webservices do municipio
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(oDadosPedNfsePDF.cMunicipio);
                WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, oDadosPedNfsePDF.cMunicipio, oDadosPedNfsePDF.tpAmb, oDadosPedNfsePDF.tpEmis, padraoNFSe, oDadosPedNfsePDF.cMunicipio);
                object pedNfsePNG = wsProxy.CriarObjeto(wsProxy.NomeClasseWS);
                string cabecMsg = "";

                switch (padraoNFSe)
                {
                    case PadroesNFSe.PRODATA:
                        cabecMsg = "<cabecalho><versaoDados>2.01</versaoDados></cabecalho>";
                        break;
                }

                System.Net.SecurityProtocolType securityProtocolType = WebServiceProxy.DefinirProtocoloSeguranca(oDadosPedNfsePDF.cMunicipio, oDadosPedNfsePDF.tpAmb, oDadosPedNfsePDF.tpEmis, padraoNFSe, Servico);

                //Assinar o XML
                AssinaturaDigital ad = new AssinaturaDigital();
                ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(oDadosPedNfsePDF.cMunicipio));

                //Invocar o método que envia o XML para o municipio
                oInvocarObj.InvocarNFSe(wsProxy, pedNfsePNG, NomeMetodoWS(Servico, oDadosPedNfsePDF.cMunicipio), cabecMsg, this,
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).EnvioXML,   
                                        Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).RetornoXML, 
                                        padraoNFSe, Servico, securityProtocolType);

                ConvertBase64ToPDF(emp, padraoNFSe);

                /// grava o arquivo no FTP
                string filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                                                Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).EnvioXML) +
                                                Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).RetornoXML);
                if (File.Exists(filenameFTP))
                {
                    new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                }
            }
            catch (Exception ex)
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
            if (padraoNFSe != PadroesNFSe.GIF)
            {
                return;
            }

            string arqPDF = Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).EnvioXML) +
                            Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSePDF).RetornoXML;
            arqPDF = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, arqPDF.Replace(".xml", ".pdf"));

            if (File.Exists(arqPDF))
            {
                File.Delete(arqPDF);
            }

            BinaryWriter writer = null;

            try
            {
                string base64BinaryStr = "";

                MemoryStream msXml = Functions.StringXmlToStream(vStrXmlRetorno);
                XmlDocument doc = new XmlDocument();
                doc.Load(msXml);

                switch (padraoNFSe)
                {
                    case PadroesNFSe.GIF:
                        if (doc.GetElementsByTagName("NFS-ePDF")[0] != null)
                        {
                            base64BinaryStr = doc.GetElementsByTagName("NFS-ePDF")[0].InnerText;
                        }
                        else
                        {
                            throw new Exception("Não foi possível localizar a tag <NFS-ePDF> no xml retornado pela prefeitura, sendo assim, o arquivo PDF da NFS-e não foi gerado.");
                        }

                        break;
                }

                byte[] sPDFDecoded = Convert.FromBase64String(base64BinaryStr);

                writer = new BinaryWriter(File.Open(arqPDF, FileMode.CreateNew));
                writer.Write(sPDFDecoded);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }
        #endregion
    }
}
