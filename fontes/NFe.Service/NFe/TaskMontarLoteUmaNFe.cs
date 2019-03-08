using NFe.Components;
using NFe.Settings;
using System;
using System.IO;
using System.Xml;

namespace NFe.Service
{
    /// <summary>
    /// Executar as tarefas pertinentes a assinatura e montagem do lote de uma única nota fiscal eletrônica
    /// </summary>
    public class TaskNFeMontarLoteUmaNFe : TaskAbst
    {
        public TaskNFeMontarLoteUmaNFe(string arquivo)
        {
            Servico = Servicos.NFeMontarLoteUma;
            NomeArquivoXML = arquivo;
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(arquivo);
        }

        public override void Execute()
        {
            try
            {
                int emp = Empresas.FindEmpresaByThread();

                DadosNFeClass oDadosNfe = LerXMLNFe(ConteudoXML);

                AdicionarResponsavelTecnico(emp, oDadosNfe.chavenfe.Substring(3, 44));

                AssinarValidarXMLNFe(ConteudoXML);

                //Montar lote de nfe
                FluxoNfe oFluxoNfe = new FluxoNfe();

                string cError = "";
                try
                {
                    if (!oFluxoNfe.NFeComLote(oDadosNfe.chavenfe))
                    {
                        XmlDocument xmlLote = LoteNfe(ConteudoXML, NomeArquivoXML, oDadosNfe.versao);

                        TaskNFeRecepcao nfeRecepcao = new TaskNFeRecepcao(xmlLote);
                        nfeRecepcao.Execute();
                    }
                }
                catch (IOException ex)
                {
                    cError = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
                catch (Exception ex)
                {
                    cError = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }

                if (!string.IsNullOrEmpty(cError))
                {
                    try
                    {
                        // grava o arquivo de erro
                        oAux.GravarArqErroERP(Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".err", cError);
                        // move o arquivo para a pasta de erro
                        oAux.MoveArqErro(NomeArquivoXML);
                    }
                    catch
                    {
                        // A principio não vou fazer nada Wandrey 24/04/2011
                    }
                }
            }
            catch { }
        }

        private void AdicionarResponsavelTecnico(int empresa, string chaveNFe)
        {
            var infRespTec = ConteudoXML.GetElementsByTagName("infRespTec")[0];

            if (infRespTec != null)
                return;

            if (String.IsNullOrEmpty(Empresas.Configuracoes[empresa].RespTecCNPJ) ||
                String.IsNullOrEmpty(Empresas.Configuracoes[empresa].RespTecXContato) ||
                String.IsNullOrEmpty(Empresas.Configuracoes[empresa].RespTecEmail) ||
                String.IsNullOrEmpty(Empresas.Configuracoes[empresa].RespTecTelefone) ||
                String.IsNullOrEmpty(Empresas.Configuracoes[empresa].RespTecIdCSRT) ||
                String.IsNullOrEmpty(Empresas.Configuracoes[empresa].RespTecCSRT))
                return;

            var infNFe = ConteudoXML.GetElementsByTagName("infNFe")[0];

            XmlElement infRespTecnico = ConteudoXML.CreateElement("infRespTec", infNFe.NamespaceURI);
            XmlNode cnpj = ConteudoXML.CreateElement("CNPJ", infNFe.NamespaceURI);
            XmlNode contato = ConteudoXML.CreateElement("xContato", infNFe.NamespaceURI);
            XmlNode email = ConteudoXML.CreateElement("email", infNFe.NamespaceURI);
            XmlNode fone = ConteudoXML.CreateElement("fone", infNFe.NamespaceURI);
            XmlNode idCSRT = ConteudoXML.CreateElement("idCSRT", infNFe.NamespaceURI);
            XmlNode csrt = ConteudoXML.CreateElement("hashCSRT", infNFe.NamespaceURI);

            cnpj.InnerText = Empresas.Configuracoes[empresa].RespTecCNPJ;
            contato.InnerText = Empresas.Configuracoes[empresa].RespTecXContato;
            email.InnerText = Empresas.Configuracoes[empresa].RespTecEmail;
            fone.InnerText = Empresas.Configuracoes[empresa].RespTecTelefone;
            idCSRT.InnerText = Empresas.Configuracoes[empresa].RespTecIdCSRT;
            csrt.InnerText = GerarHashCSRT(Empresas.Configuracoes[empresa].RespTecCSRT, chaveNFe);

            infRespTecnico.AppendChild(cnpj);
            infRespTecnico.AppendChild(contato);
            infRespTecnico.AppendChild(email);
            infRespTecnico.AppendChild(fone);
            infRespTecnico.AppendChild(idCSRT);
            infRespTecnico.AppendChild(csrt);
            infNFe.AppendChild(infRespTecnico);

            ConteudoXML.Save(NomeArquivoXML);
        }

        private string GerarHashCSRT(string csrt, string chaveNFe)
        {
            var result = Criptografia.GetSHA1HashData(csrt + chaveNFe);

            return result;
        }
    }
}