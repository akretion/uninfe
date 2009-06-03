using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Reflection;

namespace uninfe
{
    #region Classe ServicoUniNFe
    /// <summary>
    /// Classe responsável pela execução dos serviços do UniNFe
    /// </summary>
    public class ServicoUniNFe
    {
        #region Enumeradores

        #region Servicos
        public enum Servicos
        {
            /// <summary>
            /// Assina, valida e envia o XML de cancelamento de NFe para o webservice
            /// </summary>
            CancelarNFe,
            /// <summary>
            /// Assina, valida e envia o XML de Inutilização de números de NFe para o webservice
            /// </summary>
            InutilizarNumerosNFe,
            /// <summary>
            /// Valida e envia o XML de pedido de Consulta da Situação da NFe para o webservice
            /// </summary>
            PedidoConsultaSituacaoNFe,
            /// <summary>
            /// Valida e envia o XML de pedido de Consulta Status dos Serviços da NFe para o webservice
            /// </summary>
            PedidoConsultaStatusServicoNFe,
            /// <summary>
            /// Valida e envia o XML de pedido de Consulta da Situação do Lote da NFe para o webservice
            /// </summary>
            PedidoSituacaoLoteNFe,
            /// <summary>
            /// Valida e envia o XML de pedido de Consulta do Cadastro do Contribuinte para o webservice
            /// </summary>
            ConsultaCadastroContribuinte,
            /// <summary>
            /// Consultar Informações Gerais do UniNFe
            /// </summary>
            ConsultaInformacoesUniNFe,
            /// <summary>
            /// Solicitar ao UniNFe que altere suas configurações
            /// </summary>
            AlterarConfiguracoesUniNFe,
            /// <summary>
            /// Assinar e valida os XML´s de Notas Fiscais da Pasta de Envio
            /// </summary>
            AssinarNFePastaEnvio,
            /// <summary>
            /// Assinar e valida os XML´s de Notas Fiscais da Pasta de Envio em Lote
            /// </summary>
            AssinarNFePastaEnvioEmLote,
            /// <summary>
            /// Montar lote de notas com apenas uma nota fiscal
            /// </summary>
            MontarLoteUmaNFe,
            /// <summary>
            /// Montar lote de notas com várias notas fiscais
            /// </summary>
            MontarLoteVariasNFe,
            /// <summary>
            /// Envia os lotes de notas fiscais eletrônicas para os webservices
            /// </summary>
            EnviarLoteNfe,
            /// <summary>
            /// Somente validar e assinar o XML
            /// </summary>
            ValidarAssinar,
            /// <summary>
            /// Somente converter TXT da NFe para XML de NFe
            /// </summary>
            ConverterTXTparaXML
        }
        #endregion

        #region AtividadesNfe
        /// <summary>
        /// Enumerador das atividades executadas pela classe
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>14/04/2009</date>
        private enum AtividadesNfe
        {
            /// <summary>
            /// Envia os XML´s para os webservices
            /// </summary>
            EnviarXml,
            /// <summary>
            /// Assina o XML, Valida o XML da pasta de Envio em Lote e copia ele para a pasta de assinados
            /// </summary>
            AssinarXmlEnvioEmLote,
            /// <summary>
            /// Monta o arquivo de Lote de Notas Fiscais com mais de uma NFe e copia para a pasta de envio
            /// </summary>
            MontarLoteVariasNFe,
            /// <summary>
            /// Assina o XML, Valida o XML da pasta de Envio e copia ele para a pasta de assinados
            /// </summary>
            AssinarXmlEnvio,
            /// <summary>
            /// Monta o arquivo de Lote de Notas Fiscais com apenas uma NFe e copia para a pasta de envio
            /// </summary>
            MontarLoteUmaNFe,
            /// <summary>
            ///
            /// </summary>
            AssinarXml
        }
        #endregion

        #endregion

        #region Objetos Estaticos
        /// <summary>
        /// Lista que vai receber um string de identificação dos
        /// serviços que devem recarregar as configurações.
        /// </summary>
        static List<string> lstContrConfig = new List<string>();
        #endregion

        #region Métodos gerais

        #region CarregarConfiguracoes()
        /// <summary>
        /// Atualiza a lista de serviços que devem recarregar as configurações. Este
        /// método marca todos os serviços de uma única vez para recarregar.
        /// </summary>
        /// <date>10/02/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>        
        public static void CarregarConfiguracoes()
        {
            lstContrConfig.Clear();

            lstContrConfig.Add(Servicos.AlterarConfiguracoesUniNFe.ToString());
            lstContrConfig.Add(Servicos.AssinarNFePastaEnvio.ToString());
            lstContrConfig.Add(Servicos.AssinarNFePastaEnvioEmLote.ToString());
            lstContrConfig.Add(Servicos.CancelarNFe.ToString());
            lstContrConfig.Add(Servicos.ConsultaCadastroContribuinte.ToString());
            lstContrConfig.Add(Servicos.ConsultaInformacoesUniNFe.ToString());
            lstContrConfig.Add(Servicos.InutilizarNumerosNFe.ToString());
            lstContrConfig.Add(Servicos.MontarLoteUmaNFe.ToString());
            lstContrConfig.Add(Servicos.MontarLoteVariasNFe.ToString());
            lstContrConfig.Add(Servicos.PedidoConsultaSituacaoNFe.ToString());
            lstContrConfig.Add(Servicos.PedidoConsultaStatusServicoNFe.ToString());
            lstContrConfig.Add(Servicos.PedidoSituacaoLoteNFe.ToString());
            lstContrConfig.Add(Servicos.EnviarLoteNfe.ToString());
            lstContrConfig.Add(Servicos.ValidarAssinar.ToString());
            lstContrConfig.Add(Servicos.ConverterTXTparaXML.ToString());
        }
        #endregion

        #region BuscaXML()
        /// <summary>
        /// Procurar os arquivos XML´s a serem enviados aos web-services ou para ser executado alguma rotina
        /// </summary>
        /// <param name="pTipoArq">Mascara dos arquivos as serem pesquisados. Ex: *.xml   *-nfe.xml</param>
        public void BuscaXML(Object srvServico)
        {
            ConfigUniNFe oConfig = new ConfigUniNFe();
            UniNfeClass oNfe = new UniNfeClass();

            while (true)
            {
                if (lstContrConfig.Count > 0 && lstContrConfig.FindIndex(delegate(string s) { return s.Equals(Thread.CurrentThread.Name.Trim()); }) > -1)
                {
                    //Encontrou o sMascaraArq na lista, então tenho que recarregar as configurações pq algo foi modificado pelo usuário na tela de configurações.
                    oConfig.CarregarDados();

                    oNfe.oCertificado = oConfig.oCertificado;
                    oNfe.vUF = oConfig.vUnidadeFederativaCodigo;
                    oNfe.vAmbiente = oConfig.vAmbienteCodigo;
                    oNfe.vPastaXMLEnvio = oConfig.vPastaXMLEnvio;
                    oNfe.vPastaXMLEmLote = oConfig.cPastaXMLEmLote;
                    oNfe.vPastaXMLRetorno = oConfig.vPastaXMLRetorno;
                    oNfe.vPastaXMLEnviado = oConfig.vPastaXMLEnviado;
                    oNfe.vPastaXMLErro = oConfig.vPastaXMLErro;
                    oNfe.cPastaBackup = oConfig.cPastaBackup;
                    oNfe.PastaValidar = oConfig.PastaValidar;
                    oNfe.vTpEmis = oConfig.vTpEmis;

                    //Remover o item da lista para não recarregar mais a configuração
                    lstContrConfig.Remove(Thread.CurrentThread.Name.Trim());
                }
                else
                {
                    this.ProcessaXML(oNfe, (Servicos)srvServico);
                }

                Thread.Sleep(1000); //Pausa na Thread de 1000 milissegundos ou 1 segundo
            }
        }
        #endregion

        #region ProcessaXML()
        /// <summary>
        /// Processa/envia os XML´s gravados na pasta de envio
        /// </summary>
        /// <param name="oNfe">Objeto referente a UniNfeClass</param>
        /// <param name="strPasta">Pasta de envio</param>
        /// <param name="strMascara">Mascara dos arquivos a serem pesquisados e processados</param>
        /// <param name="strAtividade">Atividade a ser executada com o(s) XML(s) encontrados</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>03/04/2009</date>
        private void ProcessaXML(UniNfeClass oNfe, Servicos srvServico)
        {
            string strPasta = string.Empty;
            List<string> lstArquivos = new List<string>();
            lstArquivos.Clear();

            string strMascaraArq = string.Empty;
            string strMetodo = string.Empty;

            switch (srvServico)
            {
                case Servicos.CancelarNFe:
                    strMetodo = "Cancelamento";
                    lstArquivos = this.ArquivosPasta(oNfe.vPastaXMLEnvio, "*-ped-can.xml");
                    goto default;

                case Servicos.InutilizarNumerosNFe:
                    strMetodo = "Inutilizacao";
                    lstArquivos = this.ArquivosPasta(oNfe.vPastaXMLEnvio, "*-ped-inu.xml");
                    goto default;

                case Servicos.PedidoConsultaSituacaoNFe:
                    strMetodo = "Consulta";
                    lstArquivos = this.ArquivosPasta(oNfe.vPastaXMLEnvio, "*-ped-sit.xml");
                    goto default;

                case Servicos.PedidoConsultaStatusServicoNFe:
                    strMetodo = "StatusServico";
                    lstArquivos = this.ArquivosPasta(oNfe.vPastaXMLEnvio, "*-ped-sta.xml");
                    goto default;

                case Servicos.PedidoSituacaoLoteNFe:
                    strMetodo = "RetRecepcao";
                    this.GerarXMLPedRec(oNfe);
                    lstArquivos = this.ArquivosPasta(oNfe.vPastaXMLEnvio, "*-ped-rec.xml"); //Tem que ficar depois de ter gerado os XML de consulta do recibo ou dá falha. Wandrey 22/05/2009
                    goto default;

                case Servicos.ConsultaCadastroContribuinte:
                    strMetodo = "ConsultaCadastro";
                    lstArquivos = this.ArquivosPasta(oNfe.vPastaXMLEnvio, "*-cons-cad.xml");
                    goto default;

                case Servicos.ConsultaInformacoesUniNFe:
                    strMetodo = "GravarXMLDadosCertificado";
                    lstArquivos = this.ArquivosPasta(oNfe.vPastaXMLEnvio, "*-cons-inf.xml");
                    goto default;

                case Servicos.AlterarConfiguracoesUniNFe:
                    strMetodo = "ReconfigurarUniNfe";
                    lstArquivos = this.ArquivosPasta(oNfe.vPastaXMLEnvio, "*-alt-con.xml");
                    goto default;

                case Servicos.AssinarNFePastaEnvio:
                    this.AssinarValidarNFe(oNfe, oNfe.vPastaXMLEnvio);
                    break;

                case Servicos.AssinarNFePastaEnvioEmLote:
                    if (oNfe.vPastaXMLEmLote != string.Empty)
                    {
                        this.AssinarValidarNFe(oNfe, oNfe.vPastaXMLEmLote);
                    }
                    break;

                case Servicos.MontarLoteUmaNFe:
                    this.MontarLoteUmaNfe(oNfe);
                    break;

                case Servicos.MontarLoteVariasNFe:
                    if (oNfe.vPastaXMLEmLote != string.Empty)
                    {
                        this.MontarLoteVariasNfe(oNfe);
                    }
                    break;

                case Servicos.EnviarLoteNfe:
                    strMetodo = "Recepcao";
                    lstArquivos = this.ArquivosPasta(oNfe.vPastaXMLEnvio, "*-env-lot.xml");
                    goto default;

                case Servicos.ValidarAssinar:
                    this.AssinarValidarXML(oNfe); //Somente validar e assinar os diversos XML´s da NFe
                    break;

                case Servicos.ConverterTXTparaXML:
                    this.ConvTXT(oNfe);
                    break;

                default:  //Assinar, validar, enviar ou somente processar os arquivos XML encontrados na pasta de envio
                    for (int i = 0; i < lstArquivos.Count; i++)
                    {
                        try
                        {
                            //Verificar se consegue abrir o arquivo em modo exclusivo
                            //Se conseguir significa que está perfeitamente gerado e liberado pelo ERP
                            using (FileStream fs = File.Open(lstArquivos[i], FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                            {
                                //Fechar o arquivo
                                fs.Close();

                                //Processa ou envia o XML
                                this.EnviarArquivo(lstArquivos[i], oNfe, strMetodo);
                            }
                        }
                        catch (IOException ex)
                        {
                            //TODO: Tenho que gravar o log de erros neste ponto, pois pode ocorrer do usuário não ter acesso ao arquivo.
                        }
                        catch (Exception ex)
                        {
                            //TODO: Tenho que gravar o log de erros neste ponto, pois pode ocorrer do usuário não ter acesso ao arquivo.
                        }
                    }
                    break;
            }
        }
        #endregion

        #region AssinarValidarArquivo()
        /// <summary>
        /// Assinar e Validar todos os arquivos XML de notas fiscais encontrados na pasta informada por parâmetro
        /// </summary>
        /// <param name="oNfe">Objeto do UniNFeClass a ser utilizado</param>
        /// <param name="strPasta">Pasta onde está os XML´s</param>
        private void AssinarValidarNFe(UniNfeClass oNfe, string strPasta)
        {
            //Monta a lista de XML´s encontrados na pasta
            List<string> lstArquivos = new List<string>();
            lstArquivos = this.ArquivosPasta(strPasta, "*-nfe.xml");

            //Assinar, Validar, Enviar ou somente processar os arquivos XML encontrados na pasta de envio
            for (int i = 0; i < lstArquivos.Count; i++)
            {
                try
                {
                    //Verificar se consegue abrir o arquivo em modo exclusivo
                    //Se conseguir significa que está perfeitamente gerado e liberado pelo ERP
                    using (FileStream fs = File.Open(lstArquivos[i], FileMode.Open, FileAccess.ReadWrite, FileShare.Write))
                    {
                        //Fechar o arquivo
                        fs.Close();

                        //Assinar e Validar o XML de nota fiscal eletrônica e coloca na pasta de Assinados
                        oNfe.vXmlNfeDadosMsg = lstArquivos[i]; //Definir o arquivo XML para a classe
                        oNfe.AssinarValidarXMLNFe(strPasta);
                    }
                }
                catch (IOException ex)
                {
                    //TODO: Tenho que gravar o log de erros neste ponto, pois pode ocorrer do usuário não ter acesso ao arquivo.
                }
                catch (Exception ex)
                {
                    //TODO: Tenho que gravar o log de erros neste ponto, pois pode ocorrer do usuário não ter acesso ao arquivo.
                }
            }
        }
        #endregion

        #region EnviarArquivo()
        /// <summary>
        /// Analisa o tipo do XML que está na pasta de envio e executa a operação necessária. Exemplo: Envia ao SEFAZ, reconfigura o UniNFE, etc... 
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo XML a ser enviado ou analisado</param>
        /// <param name="oNfe">Objeto da classe UniNfeClass a ser utilizado nas operações</param>
        private void EnviarArquivo(string cArquivo, Object oNfe, string strMetodo)
        {
            //Definir o tipo do serviço
            Type tipoServico = oNfe.GetType();

            //Definir o arquivo XML para a classe UniNfeClass
            tipoServico.InvokeMember("vXmlNfeDadosMsg", System.Reflection.BindingFlags.SetProperty, null, oNfe, new object[] { cArquivo });

            //Buscar o tipo de emissão configurado
            int vTpEmis = (int)tipoServico.InvokeMember("vTpEmis", BindingFlags.GetProperty, null, oNfe, null);

            if (vTpEmis != 2) //2-Confingência em Formulário decimal segurança não envia na hora, tem que aguardar voltar para normal.
            {
                if (strMetodo == "ReconfigurarUniNfe")
                {
                    this.ReconfigurarUniNFe(cArquivo);
                }
                else if (strMetodo == "GravarXMLDadosCertificado")
                {
                    this.GravarXMLDadosCertificado(oNfe);
                }
                else
                {
                    tipoServico.InvokeMember(strMetodo, System.Reflection.BindingFlags.InvokeMethod, null, oNfe, null);
                }
            }
            else
            {
                if (strMetodo == "ReconfigurarUniNfe")
                {
                    this.ReconfigurarUniNFe(cArquivo);
                }
                else if (strMetodo == "RetRecepcao" || strMetodo == "Consulta" || strMetodo == "StatusServico")
                {
                    tipoServico.InvokeMember(strMetodo, System.Reflection.BindingFlags.InvokeMethod, null, oNfe, null);
                }
                else if (strMetodo == "GravarXMLDadosCertificado")
                {
                    this.GravarXMLDadosCertificado(oNfe);
                }
            }
        }
        #endregion

        #region ArquivosPasta()
        /// <summary>
        /// Monta uma lista dos arquivos existentes em uma determinada pasta
        /// </summary>
        /// <param name="strPasta">Pasta a ser verificada a existencia de arquivos</param>
        /// <param name="strMascara">Mascara dos arquivos a serem procurados</param>
        /// <returns>Retorna a lista dos arquivos da pasta</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        private List<string> ArquivosPasta(string strPasta, string strMascara)
        {
            //Criar uma Lista dos arquivos existentes na pasta
            List<string> lstArquivos = new List<string>();

            if (strPasta.Trim() != "" && Directory.Exists(strPasta))
            {
                try
                {
                    foreach (string item in Directory.GetFiles(strPasta, strMascara))
                    {
                        lstArquivos.Add(item);
                    }
                }
                catch (IOException ex)
                {
                    throw (ex);
                    //TODO: Tenho que gravar o log de erros neste ponto, pois pode ocorrer do usuário não ter acesso a pasta.
                }
                catch (Exception ex)
                {
                    throw (ex);
                    //TODO: Tenho que gravar o log de erros neste ponto, pois pode ocorrer do usuário não ter acesso a pasta.
                }
            }

            return lstArquivos;
        }
        #endregion

        #region MontarLoteUmaNfe()
        /// <summary>
        /// Monta o um lote para cada NFe
        /// </summary>
        /// <param name="oNfe">Objeto referente a instância da classe UniNfeClass</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/04/2009</date>
        private void MontarLoteUmaNfe(UniNfeClass oNfe)
        {
            List<string> lstArquivos = new List<string>();
            UniLerXMLClass oLerXml = new UniLerXMLClass();
            FluxoNfe oFluxoNfe = new FluxoNfe();

            lstArquivos = this.ArquivosPasta(oNfe.vPastaXMLEnvio + UniNfeClass.strNomeSubPastaAssinado, "*-nfe.xml");
            for (int i = 0; i < lstArquivos.Count; i++)
            {
                try
                {
                    oLerXml.Nfe(lstArquivos[i]);
                    if (!oFluxoNfe.NFeComLote(oLerXml.oDadosNfe.chavenfe))
                    {
                        //Gerar lote
                        oNfe.vXmlNfeDadosMsg = lstArquivos[i];
                        oNfe.GerarLoteNfe(lstArquivos[i]);
                    }
                }
                catch (IOException ex)
                {
                    //TODO: Tenho que gravar o log de erros neste ponto, pois pode ocorrer do usuário não ter acesso ao arquivo.
                }
                catch (Exception ex)
                {
                    //TODO: Tenho que gravar o log de erros neste ponto, pois pode ocorrer do usuário não ter acesso ao arquivo.
                }
            }
        }
        #endregion

        #region MontarLoteVariasNfe()
        /// <summary>
        /// Monta o um lote com várias NFe´s
        /// </summary>
        /// <param name="oNfe">Objeto referente a instância da classe UniNfeClass</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>28/04/2009</date>
        private void MontarLoteVariasNfe(UniNfeClass oNfe)
        {
            /*
            List<string> lstArquivos = new List<string>();
            UniLerXMLClass oLerXml = new UniLerXMLClass();
            FluxoNfe oFluxoNfe = new FluxoNfe();

            lstArquivos = this.ArquivosPasta(oNfe.vPastaXMLEnvio + UniNfeClass.strNomeSubPastaAssinado, "*-nfe.xml");
            for (int i = 0; i < lstArquivos.Count; i++)
            {
                try
                {
                    oLerXml.Nfe(lstArquivos[i]);
                    if (!oFluxoNfe.NFeComLote(oLerXml.oDadosNfe.chavenfe))
                    {
                        //Gerar lote
                        oNfe.vXmlNfeDadosMsg = lstArquivos[i];
                        oNfe.GerarLoteNfe(lstArquivos[i]);
                    }
                }
                catch (IOException ex)
                {
                    //TODO: Tenho que gravar o log de erros neste ponto, pois pode ocorrer do usuário não ter acesso ao arquivo.
                }
                catch (Exception ex)
                {
                    //TODO: Tenho que gravar o log de erros neste ponto, pois pode ocorrer do usuário não ter acesso ao arquivo.
                }
            }
             */
        }
        #endregion

        #region GravarXMLDadosCertificado()
        /// <summary>
        /// Gravar o XML de retorno com as informações do UniNFe para o aplicativo de ERP
        /// </summary>
        /// <param name="oNfe">Objeto da classe UniNfeClass para conseguir pegar algumas informações para gravar o XML</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>29/01/2009</date>
        private void GravarXMLDadosCertificado(Object oNfe)
        {
            //Definir o tipo do serviço
            Type tipoServico = oNfe.GetType();

            string cArquivoEnvio = (string)tipoServico.InvokeMember("vXmlNfeDadosMsg", BindingFlags.GetProperty, null, oNfe, null);
            string cPastaRetorno = (string)tipoServico.InvokeMember("vPastaXMLRetorno", BindingFlags.GetProperty, null, oNfe, null);

            UniNfeInfClass oInfUniNfe = new UniNfeInfClass();

            //Deletar o arquivo de solicitação do serviço
            FileInfo oArquivo = new FileInfo(cArquivoEnvio);
            oArquivo.Delete();

            oInfUniNfe.GravarXMLInformacoes(cPastaRetorno + "\\uninfe-ret-cons-inf.xml");
        }
        #endregion

        #region ReconfigurarUniNFe()
        /// <summary>
        /// Reconfigura o UniNFe, gravando as novas informações na tela de configuração
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo XML contendo as novas configurações</param>
        private void ReconfigurarUniNFe(string cArquivo)
        {
            ConfigUniNFe oConfig = new ConfigUniNFe();
            oConfig.ReconfigurarUniNFe(cArquivo);

            CarregarConfiguracoes();
        }
        #endregion

        #region GerarXMLPedRec()
        /// <summary>
        /// Gera o XML de consulta do recibo do lote de notas enviadas
        /// </summary>
        /// <param name="oNfe">Objeto referente a classe UniNFeClass a ser utilizado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        private void GerarXMLPedRec(UniNfeClass oNfe)
        {
            //Criar a lista dos recibos a serem consultados no SEFAZ
            List<ReciboCons> lstRecibo = new List<ReciboCons>();

            FluxoNfe oFluxoNfe = new FluxoNfe();

            try
            {
                lstRecibo = oFluxoNfe.CriarListaRec();
            }
            catch
            {
                //Não precisa fazer nada se não conseguiu criar a lista, somente con
            }

            ReciboCons oReciboCons;

            for (int i = 0; i < lstRecibo.Count; i++)
            {
                oReciboCons = lstRecibo[i];
                int intTempoConsulta = 0;
                if (oReciboCons.tMed > 10)
                {
                    intTempoConsulta = oReciboCons.tMed / 2;
                }

                if (DateTime.Now.Subtract(oReciboCons.dPedRec).Seconds >= intTempoConsulta)
                {
                    //Atualizar a tag da data e hora da ultima consulta do recibo
                    oFluxoNfe.AtualizarDPedRec(oReciboCons.nRec, DateTime.Now);

                    oNfe.GerarXmlPedRec(oReciboCons.nRec);
                }
            }
        }
        #endregion

        #region AssinarValidarXML()
        private void AssinarValidarXML(UniNfeClass oNfe)
        {
            List<string> lstMascaras = new List<string>();
            lstMascaras.Add("-nfe.xml");
            lstMascaras.Add("-env-lot.xml");
            lstMascaras.Add("-ped-rec.xml");
            lstMascaras.Add("-ped-sit.xml");
            lstMascaras.Add("-ped-can.xml");
            lstMascaras.Add("-ped-inu.xml");
            lstMascaras.Add("-ped-sta.xml");
            lstMascaras.Add("-procNFe.xml");
            lstMascaras.Add("-procCancNFe.xml");
            lstMascaras.Add("-procInutNFe.xml");
            lstMascaras.Add("-cons-cad.xml");

            List<string> lstArquivos = new List<string>();

            for (int i = 0; i < lstMascaras.Count; i++)
            {
                lstArquivos = this.ArquivosPasta(oNfe.PastaValidar, "*" + lstMascaras[i]);

                for (int b = 0; b < lstArquivos.Count; b++)
                {
                    oNfe.vXmlNfeDadosMsg = lstArquivos[b];
                    oNfe.ValidarAssinarXML();
                }

                lstArquivos.Clear();
            }

            Thread.Sleep(2000);
        }
        #endregion

        #region ConvTXT()
        /// <summary>
        /// Converter arquivos de NFe no formato TXT para XML
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>03/069/2009</date>
        private void ConvTXT(UniNfeClass oNfe)
        {
            List<string> lstArquivos = new List<string>();

            lstArquivos = this.ArquivosPasta(oNfe.vPastaXMLEnvio, "*-nfe.txt");

            for (int i = 0; i < lstArquivos.Count; i++)
            {
                UnitxtTOxmlClass oUniTxtToXml = new UnitxtTOxmlClass();

                //Verificar se consegue abrir o arquivo em modo exclusivo
                //Se conseguir significa que está perfeitamente gerado e liberado pelo ERP
                using (FileStream fs = File.Open(lstArquivos[i], FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    //Fechar o arquivo
                    fs.Close();

                    oUniTxtToXml.Converter(lstArquivos[i], oNfe.vPastaXMLEnvio);

                    //Deu tudo certo com a conversão
                    if (oUniTxtToXml.cMensagemErro == string.Empty)
                    {
                        //Gravar o retorno para o ERP em formato TXT
                        string cArqErro = oNfe.vPastaXMLRetorno + "\\" +
                                          oNfe.ExtrairNomeArq(lstArquivos[i], "-nfe.txt") +
                                          "-nfe.txt";

                        string cTexto = "cStat=01\r\n"+
                            "xMotivo=Convertido com sucesso\r\n" +
                            "ChaveNfe=" + oUniTxtToXml.ChaveNfe;

                        File.WriteAllText(cArqErro, cTexto, Encoding.Default);

                        FileInfo oArquivo = new FileInfo(lstArquivos[i]);
                        oArquivo.Delete();
                    }
                    else
                    {
                        //Gravar o retorno para o ERP em formato TXT com o erro ocorrido
                        string cArqErro = oNfe.vPastaXMLRetorno + "\\" +
                                          oNfe.ExtrairNomeArq(lstArquivos[i], "-nfe.txt") +
                                          "-nfe.err";

                        string cTexto = "cStat=99\r\n"+
                            "xMotivo=Falha na conversão\r\n" +
                            "MensagemErro=" + oUniTxtToXml.cMensagemErro;

                        File.WriteAllText(cArqErro, cTexto, Encoding.Default);

                        oNfe.MoveArqErro(lstArquivos[i],".txt");
                    }
                 }
            }
        }
        #endregion

        #endregion
    }
    #endregion
}
