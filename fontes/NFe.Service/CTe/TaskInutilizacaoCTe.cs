using NFe.Components;
using NFe.Settings;
using System;
using System.IO;
using System.Xml;
using Unimake.Business.DFe.Servicos;
using Unimake.Business.DFe.Xml.CTe;

namespace NFe.Service
{
    /// <summary>
    /// Classe para envio de XMLs de inutilização do CTe
    /// </summary>
    public class TaskCTeInutilizacao: TaskAbst
    {
        public TaskCTeInutilizacao(string arquivo)
        {
            Servico = Servicos.CTeInutilizarNumeros;
            NomeArquivoXML = arquivo;
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(arquivo);
        }

        #region Classe com os dados do XML do pedido de inutilização de números de NF

        /// <summary>
        /// Esta herança que deve ser utilizada fora da classe para obter os valores das tag´s do pedido de inutilizacao
        /// </summary>
        private DadosPedInut dadosPedInut;

        #endregion Classe com os dados do XML do pedido de inutilização de números de NF

        #region Execute

        public override void Execute()
        {
            var emp = Empresas.FindEmpresaByThread();

            try
            {
                dadosPedInut = new DadosPedInut(emp);
                PedInut(emp);

                var xml = new InutCTe();
                xml = Unimake.Business.DFe.Utility.XMLUtility.Deserializar<InutCTe>(ConteudoXML);

                var configuracao = new Configuracao
                {
                    TipoDFe = (dadosPedInut.mod == 67 ? TipoDFe.CTeOS : TipoDFe.CTe),
                    TipoEmissao = (Unimake.Business.DFe.Servicos.TipoEmissao)dadosPedInut.tpEmis,
                    CertificadoDigital = Empresas.Configuracoes[emp].X509Certificado
                };

                if(ConfiguracaoApp.Proxy)
                {
                    configuracao.HasProxy = true;
                    configuracao.ProxyAutoDetect = ConfiguracaoApp.DetectarConfiguracaoProxyAuto;
                    configuracao.ProxyUser = ConfiguracaoApp.ProxyUsuario;
                    configuracao.ProxyPassword = ConfiguracaoApp.ProxySenha;
                }

                if(dadosPedInut.mod == 67)
                {
                    var inutilizacao = new Unimake.Business.DFe.Servicos.CTeOS.Inutilizacao(xml, configuracao);
                    inutilizacao.Executar();

                    ConteudoXML = inutilizacao.ConteudoXMLAssinado;
                    vStrXmlRetorno = inutilizacao.RetornoWSString;
                }
                else
                {
                    var inutilizacao = new Unimake.Business.DFe.Servicos.CTe.Inutilizacao(xml, configuracao);
                    inutilizacao.Executar();

                    ConteudoXML = inutilizacao.ConteudoXMLAssinado;
                    vStrXmlRetorno = inutilizacao.RetornoWSString;
                }

                LerRetornoInut();

                XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).RetornoXML);
            }
            catch(Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).EnvioXML,
                        Propriedade.ExtRetorno.Inu_ERR, ex);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 09/03/2010
                }
            }
        }

        #endregion Execute

        #region PedInut()

        /// <summary>
        /// PedInut(string cArquivoXML)
        /// </summary>
        /// <param name="emp">Código da empresa</param>
        private void PedInut(int emp)
        {
            dadosPedInut.tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            dadosPedInut.tpEmis = Empresas.Configuracoes[emp].tpEmis;

            var InutNFeList = ConteudoXML.GetElementsByTagName("inutCTe");

            foreach(XmlNode InutNFeNode in InutNFeList)
            {
                var InutNFeElemento = (XmlElement)InutNFeNode;

                var infInutList = InutNFeElemento.GetElementsByTagName("infInut");
                dadosPedInut.versao = InutNFeElemento.Attributes[TpcnResources.versao.ToString()].InnerText;

                foreach(XmlNode infInutNode in infInutList)
                {
                    var infInutElemento = (XmlElement)infInutNode;
                    Functions.PopulateClasse(dadosPedInut, infInutElemento);

                    if(infInutElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString()).Count != 0)
                    {
                        dadosPedInut.tpEmis = Convert.ToInt16(infInutElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString())[0].InnerText);
                        /// para que o validador não rejeite, excluo a tag <tpEmis>
                        ConteudoXML.DocumentElement["infInut"].RemoveChild(infInutElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString())[0]);
                        /// salvo o arquivo modificado
                        ConteudoXML.Save(NomeArquivoXML);
                    }
                }
            }
        }

        #endregion PedInut()

        #region LerRetornoInut()

        /// <summary>
        /// Efetua a leitura do XML de retorno do processamento da Inutilização
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        private void LerRetornoInut()
        {
            var emp = Empresas.FindEmpresaByThread();

            //            vStrXmlRetorno = "<retInutCTe versao=\"2.00\" xmlns=\"http://www.portalfiscal.inf.br/cte\"><infInut><tpAmb>2</tpAmb><verAplic>SP-CTe-05-12-2013</verAplic><cStat>102</cStat><xMotivo>Inutilização de número homologado</xMotivo><cUF>35</cUF><ano>13</ano><CNPJ>11319532000102</CNPJ><mod>57</mod><serie>1</serie><nCTIni>2017</nCTIni><nCTFin>2017</nCTFin><dhRecbto>2013-12-13T17:43:06</dhRecbto><nProt>135130006325186</nProt></infInut></retInutCTe>";

            var doc = new XmlDocument();
            doc.Load(Functions.StringXmlToStream(vStrXmlRetorno));

            var retInutNFeList = doc.GetElementsByTagName("retInutCTe");

            foreach(XmlNode retInutNFeNode in retInutNFeList)
            {
                var retInutNFeElemento = (XmlElement)retInutNFeNode;

                var infInutList = retInutNFeElemento.GetElementsByTagName("infInut");

                foreach(XmlNode infInutNode in infInutList)
                {
                    var infInutElemento = (XmlElement)infInutNode;

                    if(infInutElemento.GetElementsByTagName(TpcnResources.cStat.ToString())[0].InnerText == "102") //Inutilização de Número Homologado
                    {
                        var strRetInutNFe = retInutNFeNode.OuterXml;

                        dadosPedInut = new DadosPedInut(emp);
                        PedInut(emp);

                        oGerarXML.XmlDistInutCTe(ConteudoXML, strRetInutNFe, NomeArquivoXML, dadosPedInut.versao);

                        //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                        var sw = File.CreateText(NomeArquivoXML);
                        sw.Write(ConteudoXML.OuterXml);
                        sw.Close();
                        TFunctions.MoverArquivo(NomeArquivoXML, PastaEnviados.Autorizados, DateTime.Now);

                        //Move o arquivo de Distribuição para a pasta de enviados autorizados
                        var strNomeArqProcInutNFe = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                            PastaEnviados.EmProcessamento.ToString() + "\\" +
                            Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedInu).EnvioXML) +
                            Propriedade.ExtRetorno.ProcInutCTe;
                        TFunctions.MoverArquivo(strNomeArqProcInutNFe, PastaEnviados.Autorizados, DateTime.Now);
                    }
                    else
                    {
                        //Deletar o arquivo de solicitação do serviço da pasta de envio
                        Functions.DeletarArquivo(NomeArquivoXML);
                    }
                }
            }
        }

        #endregion LerRetornoInut()
    }
}