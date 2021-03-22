using NFe.Components;
using NFe.Settings;
using System;
using System.IO;
using Unimake.Business.DFe.Servicos;
using Unimake.Business.DFe.Servicos.GNRE;
using Unimake.Business.DFe.Xml.GNRE;

namespace NFe.Service.GNRE
{
    class TaskLoteRecepcaoGNRE: TaskAbst
    {
        public TaskLoteRecepcaoGNRE(string arquivo)
        {
            Servico = Servicos.LoteRecepcaoGNRE;

            NomeArquivoXML = arquivo;
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(arquivo);
        }

        public override void Execute()
        {
            var emp = Empresas.FindEmpresaByThread();

            try
            {
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                    Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.GNRE).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.GNRE).RetornoERR);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + NomeArquivoXML);

                var xml = new TLoteGNRE();
                xml = xml.LerXML<TLoteGNRE>(ConteudoXML);

                var configuracao = new Configuracao
                {
                    TipoDFe = TipoDFe.GNRE,
                    TipoEmissao = Unimake.Business.DFe.Servicos.TipoEmissao.Normal,
                    CertificadoDigital = Empresas.Configuracoes[emp].X509Certificado,
                    CodigoUF = Empresas.Configuracoes[emp].UnidadeFederativaCodigo,
                    Servico = Unimake.Business.DFe.Servicos.Servico.GNRELoteRecepcao,
                    TipoAmbiente = (Unimake.Business.DFe.Servicos.TipoAmbiente)Empresas.Configuracoes[emp].AmbienteCodigo
                };

                var loteRecepcao = new LoteRecepcao(xml, configuracao);
                loteRecepcao.Executar();

                vStrXmlRetorno = loteRecepcao.RetornoWSString;
                XmlRetorno(Propriedade.Extensao(Propriedade.TipoEnvio.GNRE).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.GNRE).RetornoXML);

                /// grava o arquivo no FTP
                var filenameFTP = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno,
                    Functions.ExtrairNomeArq(NomeArquivoXML,
                    Propriedade.Extensao(Propriedade.TipoEnvio.GNRE).EnvioXML) + "\\" + Propriedade.Extensao(Propriedade.TipoEnvio.GNRE).RetornoXML);

                if(File.Exists(filenameFTP))
                {
                    new GerarXML(emp).XmlParaFTP(emp, filenameFTP);
                }
            }
            catch(Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.GNRE).EnvioXML, Propriedade.Extensao(Propriedade.TipoEnvio.GNRE).RetornoERR, ex);
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
    }
}