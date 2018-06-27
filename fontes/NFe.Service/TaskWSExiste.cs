using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;

using NFe.Components;
using NFe.Settings;

namespace NFe.Service
{
    public class TaskWSExiste : TaskAbst
    {
        internal class pedidoWSExiste
        {
            public Int32 cUF { get; set; }
            public Int32 tpAmb { get; set; }
            public Int32 tpEmis { get; set; }
            public string servicos { get; set; }
        }
        /// <summary>
        /// Alguns estados nao tem o servico de consulta ao cadastro de contribuinte, por exemplo.
        /// Então, antes da aplicacao chamar um servico, ela pode consultar para saber ser um servico está disponivel para um estado
        /// </summary>
        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            pedidoWSExiste odados = new pedidoWSExiste();
            odados.cUF = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
            odados.tpEmis = Empresas.Configuracoes[emp].tpEmis;
            odados.tpAmb = Empresas.Configuracoes[emp].AmbienteCodigo;
            odados.servicos = "";

            //Definir o serviço que será executado para a classe
            Servico = Servicos.WSExiste;

            try
            {
                int intValue;

                if (this.vXmlNfeDadosMsgEhXML)  //danasa 12-9-2009
                {
#if modelo_xml
<?xml version="1.0" encoding="utf-8"?>
<dados>
    <cUF>31</cUF>           opcional ou se informada a UF por sigla, convertemos para UF->inteiro
    <tpAmb>2</tpAmb>        opcional
    <tpEmis>1</tpEmis>      opcional
    <servicos>NFeConsultaCadastro,NFeStatusServico,...</servicos>
</dados>
#endif
                    Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, 
                                Path.GetFileName(NomeArquivoXML.Replace(Propriedade.Extensao(Propriedade.TipoEnvio.EnvWSExiste).EnvioXML,
                                Propriedade.Extensao(Propriedade.TipoEnvio.EnvWSExiste).RetornoXML).Replace(".xml", ".err"))));

                    XmlDocument doc = new XmlDocument();
                    doc.Load(NomeArquivoXML);
                    foreach (XmlNode node in doc.GetElementsByTagName("dados"))
                    {
                        XmlElement elementConfig = (XmlElement)node;

                        odados.tpEmis = Convert.ToInt32(Functions.LerTag(elementConfig, TpcnResources.tpEmis.ToString(), odados.tpEmis.ToString()));
                        odados.tpAmb = Convert.ToInt32(Functions.LerTag(elementConfig, TpcnResources.tpAmb.ToString(), odados.tpAmb.ToString()));
                        string temp = Functions.LerTag(elementConfig, TpcnResources.cUF.ToString(), odados.cUF.ToString());
                        if (int.TryParse(temp, out intValue))
                            odados.cUF = intValue;
                        else
                            odados.cUF = Functions.UFParaCodigo(temp);
                        odados.servicos = Functions.LerTag(elementConfig, "servicos", false);
                    }
                }
                else
                {
#if modelo_txt
cUF|31              --opcional ou se informada a UF por sigla, convertemos para UF->inteiro
tpAmb|2             --opcional
tpEmis|1            --opcional: (1)Normal, (2)Contingencia, ...
servicos|NFeConsultaCadastro,NFeStatusServico,...
#endif
                    Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, 
                                Path.GetFileName(NomeArquivoXML.Replace(Propriedade.Extensao(Propriedade.TipoEnvio.EnvWSExiste).EnvioTXT, 
                                Propriedade.Extensao(Propriedade.TipoEnvio.EnvWSExiste).EnvioTXT).Replace(".txt", ".err"))));

                    List<string> cLinhas = Functions.LerArquivo(NomeArquivoXML);

                    foreach (string cTexto in cLinhas)
                    {
                        string[] dados = cTexto.Split('|');
                        if (dados.GetLength(0) == 1) continue;

                        switch (dados[0].ToLower())
                        {
                            case "tpamb":
                                odados.tpAmb = Convert.ToInt32("0" + dados[1].Trim());
                                break;
                            case "tpemis":
                                odados.tpEmis = Convert.ToInt32("0" + dados[1].Trim());
                                break;
                            case "cuf":
                                if (int.TryParse(dados[1].Trim(), out intValue))
                                    odados.cUF = intValue;
                                else
                                    odados.cUF = Functions.UFParaCodigo(dados[1].Trim());
                                break;
                            case "servicos":
                                odados.servicos = dados[1].Trim();
                                break;
                        }
                    }
                }

#if modelo_de_retorno_xml
<?xml version="1.0" encoding="utf-8"?>
<dados>
    <cUF>31</cUF>
    <tpAmb>2</tpAmb>
    <tpEmis>1</tpEmis>
    <servicos>NFeConsultaCadastro=True|False,NFeStatusServico=True|False,...</servicos>
</dados>
#endif
#if modelo_de_retorno_txt
cUF|31
tpAmb|2
tpEmis|1
servicos|NFeConsultaCadastro=True|False,NFeStatusServico=True|False,...
#endif
                string nomeArquivoRetorno;
                if (this.vXmlNfeDadosMsgEhXML)
                    nomeArquivoRetorno = Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvWSExiste).EnvioXML) + 
                                            Propriedade.Extensao(Propriedade.TipoEnvio.EnvWSExiste).RetornoXML;
                else
                    nomeArquivoRetorno = Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvWSExiste).EnvioTXT) + 
                                            Propriedade.Extensao(Propriedade.TipoEnvio.EnvWSExiste).RetornoTXT;
                nomeArquivoRetorno = Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Path.GetFileName(nomeArquivoRetorno));
                Functions.DeletarArquivo(nomeArquivoRetorno);

                StringBuilder outStr = new StringBuilder();
                if (this.vXmlNfeDadosMsgEhXML)
                {
                    outStr.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?><dados>");
                    outStr.AppendFormat("<cUF>{0}</cUF>", odados.cUF);
                    outStr.AppendFormat("<tpAmb>{0}</tpAmb>", odados.tpAmb);
                    outStr.AppendFormat("<tpEmis>{0}</tpEmis>", odados.tpEmis);
                }
                else
                {
                    outStr.AppendFormat("cUF|{0}\r\n", odados.cUF);
                    outStr.AppendFormat("tpAmb|{0}\r\n", odados.tpAmb);
                    outStr.AppendFormat("tpEmis|{0}\r\n", odados.tpEmis);
                }

                Servicos srv;
                string outServicos = "";
                string[] oservicos = (odados.servicos + ",").Split(new char[] { ',', ';' });
                foreach (string aservico in oservicos)
                {
                    if (!string.IsNullOrEmpty(aservico))
                    {
                        srv = Servicos.Nulo;

                        switch (aservico.ToLower())
                        {
                            case "mdfeconsulta":
                                srv = Servicos.MDFePedidoSituacaoLote;
                                break;
                            case "mdfeconsultanaoencerrado":
                                srv = Servicos.MDFeConsultaNaoEncerrado;
                                break;
                            case "mdferecepcaoevento":
                                srv = Servicos.MDFeRecepcaoEvento;
                                break;
                            case "mdfestatusservico":
                                srv = Servicos.MDFeConsultaStatusServico;
                                break;

                            case "cancelarnfse":
                                srv = Servicos.NFSeCancelar; 
                                break;
                            case "consultarloterps":
                                srv = Servicos.NFSeConsultarLoteRps; 
                                break;
                            case "consultarnfse":
                                srv = Servicos.NFSeConsultar; 
                                break;
                            case "consultarnfseporrps":
                                srv = Servicos.NFSeConsultarPorRps; 
                                break;
                            case "consultarsituacaoloterps":
                                srv = Servicos.NFSeConsultarSituacaoLoteRps;
                                break;
                            case "recepcionarloterps":
                                srv = Servicos.NFSeRecepcionarLoteRps; 
                                break;
                            case "recepcaoevento":
                                srv = Servicos.EventoRecepcao; 
                                break;
                            case "cterecepcaoevento":
                                srv = Servicos.CTeRecepcaoEvento; 
                                break;
                            case "recepcaoeventocte":
                                srv = Servicos.CTeRecepcaoEvento; 
                                break;
                            case "enviarcce":
                                srv = Servicos.EventoCCe; 
                                break;
                            case "enviarepec":
                                srv = Servicos.EventoEPEC; break;
                            case "enviareventocancelamento":
                                srv = Servicos.EventoCancelamento; break;
                            case "nfeconsulta1":
                            case "nfeconsulta":
                                srv = Servicos.NFePedidoConsultaSituacao; break;
                            case "nfeconsultacadastro":
                                srv = Servicos.ConsultaCadastroContribuinte; break;
                            case "nfeinutilizacao":
                                srv = Servicos.NFeInutilizarNumeros; break;
                            case "nfemanifestacao":
                                srv = Servicos.EventoManifestacaoDest; break;
                            case "dferecepcao":
                                srv = Servicos.DFeEnviar; break;
                            case "nfestatusservico":
                                srv = Servicos.NFeConsultaStatusServico; break;
                        }

                        if (srv == Servicos.Nulo)
                        {
                            string aServicos = "";

                            System.Reflection.PropertyInfo[] fieldInfo = typeof(URLws).GetProperties();
                            foreach (System.Reflection.PropertyInfo info in fieldInfo)
                            {
                                if (info.Name.Equals("NFeStatusServico"))
                                {
                                    aServicos += "," + info.Name;
                                    continue;
                                }
                                bool isNFse = ("ConsultarURLNfse,RecepcionarLoteRps,ConsultarSituacaoLoteRps,ConsultarNfsePorRps,ConsultarNfse,ConsultarLoteRps,CancelarNfse").Contains(info.Name);

                                if (Empresas.Configuracoes[emp].Servico != TipoAplicativo.Nfse && isNFse)
                                    continue;

                                if (Empresas.Configuracoes[emp].Servico == TipoAplicativo.Nfse && 
                                    info.Name.StartsWith("NFe")) continue;

                                aServicos += "," + info.Name;
                            }
                            if (aServicos.StartsWith(",")) aServicos = aServicos.Substring(1);
                            throw new Exception("Serviços disponiveis: " + aServicos);
                        }
                        else
                            try
                            {
                                WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(srv, emp, odados.cUF, odados.tpAmb, odados.tpEmis, string.Empty, 0);
                                if (wsProxy != null)
                                    outServicos += aservico + "=True,";
                                else
                                    outServicos += aservico + "=False,";
                            }
                            catch
                            {
                                outServicos += aservico + "=False,";
                            }
                    }
                }
                if (string.IsNullOrEmpty(outServicos))
                    ///
                    ///previne erro ao acessa-lo por outServicos.Substring(0,...
                    outServicos = "Sem servicos solicitados.";

                if (this.vXmlNfeDadosMsgEhXML)
                {
                    outStr.AppendFormat("<servicos>{0}</servicos></dados>", outServicos.Substring(0, outServicos.Length - 1));
                }
                else
                {
                    outStr.AppendFormat("servicos|{0}", outServicos.Substring(0, outServicos.Length - 1));
                }
                System.IO.File.WriteAllText(nomeArquivoRetorno, outStr.ToString());//, Encoding.Default);
            }
            catch (Exception ex)
            {
                try
                {
                    string ExtRet = (this.vXmlNfeDadosMsgEhXML ? Propriedade.Extensao(Propriedade.TipoEnvio.EnvWSExiste).EnvioXML : 
                                                                 Propriedade.Extensao(Propriedade.TipoEnvio.EnvWSExiste).EnvioTXT);

                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    //retorna um arquivo com o nome NomeArquivoSolicitado-ret-env-ws.err
                    TFunctions.GravarArqErroServico(NomeArquivoXML, ExtRet, Propriedade.Extensao(Propriedade.TipoEnvio.EnvWSExiste).RetornoXML.Replace(".xml", ".err"), ex);
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
                    Functions.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                    //Se falhou algo na hora de deletar o XML de inutilização, infelizmente não posso 
                    //fazer mais nada. Com certeza o uninfe sendo restabelecido novamente vai tentar enviar o mesmo 
                    //xml de inutilização para o SEFAZ. Este erro pode ocorrer por falha no HD, rede, Permissão de pastas, etc. Wandrey 23/03/2010
                }
            }
        }
    }
}
