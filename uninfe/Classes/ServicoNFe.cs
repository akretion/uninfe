using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using UniNFeLibrary;
using UniNFeLibrary.Enums;

namespace uninfe
{
    #region Classe ServicoNFe
    public class ServicoNFe : absServicoNFe
    {
        #region Objetos
        public GerarXML oGerarXML = new GerarXML();
        #endregion

        #region Propriedades
        
        /// <summary>
        /// Arquivo XML contendo os dados a serem enviados (Nota Fiscal, Pedido de Status, Cancelamento, etc...)
        /// </summary>
        public override string vXmlNfeDadosMsg
        {
            get
            {
                return this.mXmlNfeDadosMsg;
            }
            set
            {
                this.mXmlNfeDadosMsg = value;
                oGerarXML.NomeXMLDadosMsg = value;
            }
        }

        /// <summary>
        /// Serviço que está sendo executado (Envio de Nota, Cancelamento, Consulta, etc...)
        /// </summary>
        protected override Servicos Servico 
        {
            get
            {
                return this.mServico;
            }
            set
            {
                this.mServico = value;
                oGerarXML.Servico = value;
            }
        }

        #endregion

        #region Métodos de execução dos serviços da NFE

        #region StatusServico()
        /// <summary>
        /// Verificar o status do Serviço da NFe do SEFAZ em questão
        /// </summary>
        /// <remark>
        /// Como retorno, o método atualiza a propriedade this.vNfeRetorno da classe 
        /// com o conteúdo do retorno do WebService.
        /// No caso do StatusServico se tudo estiver correto retorna um XML
        /// dizendo que o serviço está em operação
        /// Se der algum erro ele grava um arquivo txt com a extensão .ERR com o conteúdo do erro
        /// </remark>
        /// <example>
        /// oUniNfe.vUF = 51; //Setar o Estado que é para ser verificado o status do serviço
        /// oUniNfe.vXmlNfeDadosMsg = "c:\pedstatus.xml";
        /// oUniNfe.StatusServico();
        /// this.textBox_xmlretorno.Text = oUniNfe.vNfeRetorno;
        /// //
        /// //O conteúdo de retorno vai ser algo mais ou menos assim:
        /// //
        /// // <?xml version="1.0" encoding="UTF-8"?>
        /// //   <retConsStatServ xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.07">
        /// //      <tpAmb>2</tpAmb>
        /// //      <verAplic>1.10</verAplic>
        /// //      <cStat>107</cStat>
        /// //      <xMotivo>Servico em Operacao</xMotivo>
        /// //      <cUF>51</cUF>
        /// //      <dhRecbto>2008-06-12T11:16:55</dhRecbto>
        /// //      <tMed>2</tMed>
        /// //   </retConsStatServ>
        /// </example>
        /// <by>
        /// Wandrey Mundin Ferreira
        /// </by>
        /// <date>
        /// 01/04/2008
        /// </date>
        public override void StatusServico()
        {
            //Instanciar o objeto da classe DefObjServico
            DefObjServico oDefObj = new DefObjServico();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.PedidoConsultaStatusServicoNFe;

            //Ler o XML para pegar parâmetros de envio
            LerXML oLer = new LerXML();
            oLer.PedSta(this.vXmlNfeDadosMsg);
            ParametroEnvioXML oParam = new ParametroEnvioXML();
            oParam.tpAmb = Convert.ToInt32(oLer.oDadosPedSta.tpAmb);
            oParam.UFCod = Convert.ToInt32(oLer.oDadosPedSta.cUF);
            oParam.tpEmis = oLer.oDadosPedSta.tpEmis;// ConfiguracaoApp.tpEmis;

            //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
            object oServico = null;
            object oCabecMsg = null;
            oDefObj.StatusServico(ref oServico, ref oCabecMsg, oParam);
            if (oInvocarObj.Invocar(this, ConfiguracaoApp.VersaoXMLStatusServico, oServico, "nfeStatusServicoNF", "-ped-sta", "-sta") == true)
            {
                //Deletar o arquivo de solicitação do serviço
                oAux.DeletarArquivo(this.vXmlNfeDadosMsg);
            }
        }
        #endregion

        #region Recepcao()
        /// <summary>
        /// Envia o XML de lote de nota fiscal pra o SEFAZ em questão
        /// </summary>
        /// <remarks>
        /// Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
        /// XML com o retorno que foi dado do serviço do WebService.
        /// No caso do Recepcao se tudo estiver correto retorna um XML
        /// dizendo que a(s) nota(s) foram recebidas com sucesso.
        /// Se der algum erro ele grava um arquivo txt com o erro em questão.
        /// </remarks>
        /// <example>
        /// oUniNfe.vXmlNfeDadosMsg = "c:\nfe.xml";
        /// oUniNfe.Recepcao();
        /// this.textBox_xmlretorno.Text = oUniNfe.vNfeRetorno;
        /// //
        /// //
        /// //O conteúdo de retorno vai ser algo mais ou menos assim:
        /// //
        /// //<?xml version="1.0" encoding="UTF-8"?>
        /// //   <retEnviNFe xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.10">
        /// //      <tpAmb>2</tpAmb>
        /// //      <verAplic>1.10</verAplic>
        /// //      <cStat>103</cStat>
        /// //      <xMotivo>Lote recebido com sucesso</xMotivo>
        /// //      <cUF>51</cUF>
        /// //      <infRec>
        /// //         <nRec>510000000106704</nRec>
        /// //         <dhRecbto>2008-06-12T10:49:30</dhRecbto>
        /// //         <tMed>2</tMed>
        /// //      </infRec>
        /// //   </retEnviNFe>
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>04/06/2008</date>
        public override void Recepcao()
        {
            //Instanciar o objeto da classe DefObjServico
            DefObjServico oDefObj = new DefObjServico();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.EnviarLoteNfe;

            //Ler o XML de Lote para pegar o número do lote que está sendo enviado
            LerXML oLerXml = new LerXML();
            oLerXml.Nfe(this.vXmlNfeDadosMsg);

            string idLote = oLerXml.oDadosNfe.idLote;
            FluxoNfe oFluxoNfe = new FluxoNfe();

            //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
            object oServico = null;
            object oCabecMsg = null;
            oDefObj.Recepcao(ref oServico, ref oCabecMsg);
            if (oInvocarObj.Invocar(this, ConfiguracaoApp.VersaoXMLNFe, oServico, "nfeRecepcaoLote", "-env-lot", "-rec") == true)
            {
                LerXML oLerRecibo = new LerXML();
                oLerRecibo.Recibo(this.vStrXmlRetorno);

                if (oLerRecibo.oDadosRec.cStat == "103") //Lote recebido com sucesso
                {
                    //Atualizar o número do recibo no XML de controle do fluxo de notas enviadas
                    oFluxoNfe.AtualizarTagRec(idLote, oLerRecibo.oDadosRec.nRec);
                    oFluxoNfe.AtualizarTag(oLerXml.oDadosNfe.chavenfe, FluxoNfe.ElementoEditavel.tMed, oLerRecibo.oDadosRec.tMed.ToString());
                }
                else
                {
                    //Se o status do retorno do lote for diferente de 103, 
                    //vamos ter que excluir a nota do fluxo, porque ela foi rejeitada pelo SEFAZ
                    //Primeiro deleta o xml da nota da pasta EmProcessamento e depois tira ela do fluxo
                    //Wandrey 30/04/2009
                    oAux.DeletarArquivo(ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento + "\\" + oFluxoNfe.LerTag(oLerXml.oDadosNfe.chavenfe, FluxoNfe.ElementoFixo.ArqNFe));

                    oFluxoNfe.ExcluirNfeFluxo(oLerXml.oDadosNfe.chavenfe);
                }

                //Deleta o arquivo de lote
                oAux.DeletarArquivo(this.vXmlNfeDadosMsg);
            }
            else
            {
                //Se falhou algo no envio vamos ter que excluir a nota do fluxo, 
                //Primeiro deleta o xml da nota da pasta EmProcessamento e depois tira ela do fluxo
                //Wandrey 30/04/2009

                //TODO: QUEDA DE ENERGIA
                //ANOTACOES TODO ACIMA: Como já vai ter movido o arquivo de lote para a pasta de XML´s com erro
                //Neste ponto se der uma queda de energia ele não sai mais do fluxo, visto que não tem recibo
                //Não tem mais lote para enviar, enfim fica preso.

                oAux.DeletarArquivo(ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento + "\\" + oFluxoNfe.LerTag(oLerXml.oDadosNfe.chavenfe, FluxoNfe.ElementoFixo.ArqNFe));
                oFluxoNfe.ExcluirNfeFluxo(oLerXml.oDadosNfe.chavenfe);
            }
        }
        #endregion

        #region RetRecepcao()
        /// <summary>
        /// Busca no WebService da NFe a situação da nota fiscal enviada
        /// </summary>
        /// <remarks>
        /// Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
        /// XML com o retorno que foi dado do serviço do WebService.
        /// No caso do RetRecepcao se tudo estiver correto retorna um XML
        /// dizendo que o lote foi processado ou não e se as notas foram 
        /// autorizadas ou não.
        /// Se der algum erro ele grava um arquivo txt com o erro em questão.
        /// </remarks>
        /// <example>
        /// oUniNfe.vXmlNfeDadosMsg = "c:\teste-ped-rec.xml";
        /// oUniNfe.RetRecepcao();
        /// this.textBox_xmlretorno.Text = oUniNfe.vNfeRetorno;
        /// //
        /// //
        /// //O conteúdo de retorno vai ser algo mais ou menos assim:
        /// //
        /// //<?xml version="1.0" encoding="UTF-8"?>
        /// //   <retEnviNFe xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.10">
        /// //      <tpAmb>2</tpAmb>
        /// //      <verAplic>1.10</verAplic>
        /// //      <cStat>103</cStat>
        /// //      <xMotivo>Lote recebido com sucesso</xMotivo>
        /// //      <cUF>51</cUF>
        /// //      <infRec>
        /// //         <nRec>510000000106704</nRec>
        /// //         <dhRecbto>2008-06-12T10:49:30</dhRecbto>
        /// //         <tMed>2</tMed>
        /// //      </infRec>
        /// //   </retEnviNFe>
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>04/06/2008</date>        
        public override void RetRecepcao()
        {
            //Instanciar o objeto da classe DefObjServico
            DefObjServico oDefObj = new DefObjServico();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.PedidoSituacaoLoteNFe;

            //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
            object oServico = null;
            object oCabecMsg = null;
            oDefObj.RetRecepcao(ref oServico, ref oCabecMsg);

            //Invoca o método para acessar o webservice do SEFAZ mas sem gravar o XML retornado pelo mesmo
            if (oInvocarObj.Invocar(this, ConfiguracaoApp.VersaoXMLPedRec, oServico, "nfeRetRecepcao") == true)
            {
                try
                {
                    //Efetuar a leituras das notas do lote para ver se foi autorizada ou não
                    this.LerRetornoLote();

                    //Gravar o XML retornado pelo WebService do SEFAZ na pasta de retorno para o ERP
                    //Tem que ser feito neste ponto, pois somente aqui terminamos todo o processo
                    //Wandrey 18/06/2009
                    oGerarXML.XmlRetorno(ExtXml.PedRec/*"-ped-rec.xml"*/, "-pro-rec.xml", this.vStrXmlRetorno);

                    //Deletar o arquivo de solicitação do serviço
                    oAux.DeletarArquivo(this.vXmlNfeDadosMsg);
                }
                catch (Exception ex)
                {
                    //Deletar o arquivo de solicitação do serviço
                    oAux.DeletarArquivo(this.vXmlNfeDadosMsg);

                    throw (ex);
                }
            }
        }
        #endregion

        #region Consulta()
        /// <summary>
        /// Envia o XML de consulta da situação da nota fiscal
        /// </summary>
        /// <remarks>
        /// Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
        /// XML com o retorno que foi dado do serviço do WebService.
        /// No caso da Consulta se tudo estiver correto retorna um XML
        /// com a situação da nota fiscal (Se autorizada ou não).
        /// Se der algum erro ele grava um arquivo txt com o erro em questão.
        /// </remarks>
        /// <example>
        /// oUniNfe.vXmlNfeDadosMsg = "c:\teste-ped-sit.xml";
        /// oUniNfe.Consulta();
        /// this.textBox_xmlretorno.Text = oUniNfe.vNfeRetorno;
        /// //
        /// //
        /// //O conteúdo de retorno vai ser algo mais ou menos assim:
        /// //
        /// //<?xml version="1.0" encoding="UTF-8" ?>
        /// //   <retConsSitNFe versao="1.07" xmlns="http://www.portalfiscal.inf.br/nfe">
        /// //      <infProt>
        /// //         <tpAmb>2</tpAmb>
        /// //         <verAplic>1.10</verAplic>
        /// //         <cStat>100</cStat>
        /// //         <xMotivo>Autorizado o uso da NF-e</xMotivo>
        /// //         <cUF>51</cUF>
        /// //         <chNFe>51080612345678901234550010000001041671821888</chNFe>
        /// //         <dhRecbto>2008-06-27T15:01:48</dhRecbto>
        /// //         <nProt>151080000194296</nProt>
        /// //         <digVal>WHM/TzTvF+LrdUwtwvk26qgsko0=</digVal>
        /// //      </infProt>
        /// //   </retConsSitNFe>
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>04/06/2008</date>
        public override void Consulta()
        {
            //Instanciar o objeto da classe DefObjServico
            DefObjServico oDefObj = new DefObjServico();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.PedidoConsultaSituacaoNFe;

            //Ler o XML para pegar parâmetros de envio
            LerXML oLer = new LerXML();
            oLer.PedSit(this.vXmlNfeDadosMsg);
            ParametroEnvioXML oParam = new ParametroEnvioXML();
            oParam.tpAmb = Convert.ToInt32(oLer.oDadosPedSit.tpAmb);
            oParam.UFCod = Convert.ToInt32(oLer.oDadosPedSit.cUF);
            oParam.tpEmis = Convert.ToInt32(oLer.oDadosPedSit.tpEmis);

            //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
            object oServico = null;
            object oCabecMsg = null;
            oDefObj.Consulta(ref oServico, ref oCabecMsg, oParam);
            if (oInvocarObj.Invocar(this, ConfiguracaoApp.VersaoXMLPedSit, oServico, "nfeConsultaNF", "-ped-sit", "-sit") == true)
            {
                //Deletar o arquivo de solicitação do serviço
                oAux.DeletarArquivo(this.vXmlNfeDadosMsg);
            }
        }
        #endregion

        #region Cancelamento()
        /// <summary>
        /// Envia o XML de cancelamento de nota fiscal
        /// </summary>
        /// <remarks>
        /// Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
        /// XML com o retorno que foi dado do serviço do WebService.
        /// No caso do Cancelamento se tudo estiver correto retorna um XML
        /// dizendo se foi cancelado corretamente ou não.
        /// Se der algum erro ele grava um arquivo txt com o erro em questão.
        /// </remarks>
        /// <example>
        /// oUniNfe.vXmlNfeDadosMsg = "c:\teste-ped-sit.xml";
        /// oUniNfe.Consulta();//
        /// this.textBox_xmlretorno.Text = oUniNfe.vNfeRetorno;
        /// //
        /// //O conteúdo de retorno vai ser algo mais ou menos assim:
        /// //
        /// //<?xml version="1.0" encoding="UTF-8" ?> 
        /// //<retCancNFe xmlns="http://www.portalfiscal.inf.br/nfe" xmlns:ds="http://www.w3.org/2000/09/xmldsig#" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemaLocation="http://www.portalfiscal.inf.br/nfe retCancNFe_v1.07.xsd" versao="1.07">
        /// //   <infCanc>
        /// //      <tpAmb>2</tpAmb> 
        /// //      <verAplic>1.10</verAplic> 
        /// //      <cStat>101</cStat> 
        /// //      <xMotivo>Cancelamento de NF-e homologado</xMotivo> 
        /// //      <cUF>51</cUF> 
        /// //      <chNFe>51080612345678901234550010000001041671821888</chNFe> 
        /// //      <dhRecbto>2008-07-01T16:37:22</dhRecbto> 
        /// //      <nProt>151080000197648</nProt> 
        /// //   </infCanc>
        /// //</retCancNFe>
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>01/07/2008</date>
        public override void Cancelamento()
        {
            //Instanciar o objeto da classe DefObjServico
            DefObjServico oDefObj = new DefObjServico();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.CancelarNFe;

            //Assinar o arquivo XML
            AssinaturaDigital oAD = new AssinaturaDigital();
            try
            {
                oAD.Assinar(this.vXmlNfeDadosMsg, "infCanc", ConfiguracaoApp.oCertificado);
                if (oAD.intResultado == 0)
                {
                    //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
                    object oServico = null;
                    object oCabecMsg = null;
                    oDefObj.Cancelamento(ref oServico, ref oCabecMsg);
                    if (oInvocarObj.Invocar(this, ConfiguracaoApp.VersaoXMLCanc, oServico, "nfeCancelamentoNF", "-ped-can", "-can") == true)
                    {
                        this.LerRetornoCanc();
                    }
                }
            }
            catch (Exception ex)
            {
                oAux.GravarArqErroServico(this.vXmlNfeDadosMsg, ExtXml.PedCan/*"-ped-can.xml"*/, "-can.err", ex.Message);
            }
        }
        #endregion

        #region Inutilizacao()
        /// <summary>
        /// Envia o XML de inutilização de numeração de notas fiscais
        /// </summary>
        /// <remarks>
        /// Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
        /// XML com o retorno que foi dado do serviço do WebService.
        /// No caso da Inutilização se tudo estiver correto retorna um XML
        /// dizendo se foi inutilizado corretamente ou não.
        /// Se der algum erro ele grava um arquivo txt com o erro em questão.
        /// </remarks>
        /// <example>
        /// oUniNfe.vXmlNfeDadosMsg = "c:\teste-ped-sit.xml";
        /// oUniNfe.Inutilizacao();
        /// this.textBox_xmlretorno.Text = oUniNfe.vNfeRetorno;
        /// //
        /// //
        /// //O conteúdo de retorno vai ser algo mais ou menos assim:
        /// //
        /// //<?xml version="1.0" encoding="UTF-8" ?> 
        /// //<retInutNFe xmlns="http://www.portalfiscal.inf.br/nfe" xmlns:ds="http://www.w3.org/2000/09/xmldsig#" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemaLocation="http://www.portalfiscal.inf.br/nfe retInutNFe_v1.07.xsd" versao="1.07">
        /// //   <infInut>
        /// //      <tpAmb>2</tpAmb> 
        /// //      <verAplic>1.10</verAplic> 
        /// //      <cStat>102</cStat> 
        /// //      <xMotivo>Inutilizacao de numero homologado</xMotivo> 
        /// //      <cUF>51</cUF> 
        /// //      <ano>08</ano> 
        /// //      <CNPJ>12345678901234</CNPJ> 
        /// //      <mod>55</mod> 
        /// //      <serie>1</serie> 
        /// //      <nNFIni>101</nNFIni> 
        /// //      <nNFFin>101</nNFFin> 
        /// //      <dhRecbto>2008-07-01T16:47:11</dhRecbto> 
        /// //      <nProt>151080000197712</nProt> 
        /// //   </infInut>
        /// //</retInutNFe>
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>03/04/2009</date>
        public override void Inutilizacao()
        {
            //Instanciar o objeto da classe DefObjServico
            DefObjServico oDefObj = new DefObjServico();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.InutilizarNumerosNFe;

            //Assinar o arquivo XML
            AssinaturaDigital oAD = new AssinaturaDigital();
            try
            {
                oAD.Assinar(this.vXmlNfeDadosMsg, "infInut", ConfiguracaoApp.oCertificado);

                if (oAD.intResultado == 0)
                {
                    //Resolver um falha do estado da bahia que gera um método com nome diferente dos demais estados
                    string cMetodo = "nfeInutilizacaoNF";
                    //TODO: CONFIG
                    if (ConfiguracaoApp.tpAmb == 29 &&
                        ConfiguracaoApp.tpEmis != TipoEmissao.teSCAN/*3*/)
                    {
                        cMetodo = "nfeInutilizacao";
                    }

                    //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
                    object oServico = null;
                    object oCabecMsg = null;
                    oDefObj.Inutilizacao(ref oServico, ref oCabecMsg);
                    if (oInvocarObj.Invocar(this, ConfiguracaoApp.VersaoXMLInut, oServico, cMetodo, "-ped-inu", "-inu") == true)
                    {
                        this.LerRetornoInut();
                    }
                }
            }
            catch (Exception ex)
            {
                oAux.GravarArqErroServico(this.vXmlNfeDadosMsg, ExtXml.PedInu/*"-ped-inu.xml"*/, "-inu.err", ex.Message);
            }
        }
        #endregion

        #region ConsultaCadastro()
        /// <summary>
        /// Envia o XML de consulta do cadastro do contribuinte para o web-service do sefaz
        /// </summary>
        /// <remark>
        /// Como retorno, o método atualiza a propriedade this.vNfeRetorno da classe 
        /// com o conteúdo do retorno do WebService.
        /// No caso do consultaCadastro se tudo estiver correto retorna um XML
        /// com o resultado da consulta
        /// Se der algum erro ele grava um arquivo txt com a extensão .ERR com o conteúdo do erro
        /// </remark>
        /// <example>
        /// oUniNfe.vUF = 51; //Setar o Estado que é para efetuar a consulta
        /// oUniNfe.vXmlNfeDadosMsg = "c:\00000000000000-cons-cad.xml";
        /// oUniNfe.ConsultaCadastro();
        /// this.textBox_xmlretorno.Text = oUniNfe.vNfeRetorno;
        /// //
        /// //O conteúdo de retorno vai ser algo mais ou menos assim:
        /// //
        /// //<retConsCad xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.01">
        /// //   <infCons>
        /// //      <verAplic>SP_NFE_PL_005c</verAplic>
        /// //      <cStat>111</cStat>
        /// //      <xMotivo>Consulta cadastro com uma ocorrência</xMotivo>
        /// //      <UF>SP</UF>
        /// //      <CNPJ>00000000000000</CNPJ>
        /// //      <dhCons>2009-01-29T10:36:33</dhCons>
        /// //      <cUF>35</cUF>
        /// //      <infCad>
        /// //         <IE>000000000000</IE>
        /// //         <CPF />
        /// //         <CNPJ>00000000000000</CNPJ>
        /// //         <UF>SP</UF>
        /// //         <cSit>1</cSit>
        /// //         <xNome>EMPRESA DE TESTE PARA AVALIACAO DO SERVICO</xNome>
        /// //      </infCad>
        /// //   </infCons>
        /// //</retConsCad>
        /// </example>
        /// <by>
        /// Wandrey Mundin Ferreira
        /// </by>
        /// <date>
        /// 15/01/2009
        /// </date>
        public override void ConsultaCadastro()
        {
            //Instanciar o objeto da classe DefObjServico
            DefObjServico oDefObj = new DefObjServico();

            //Definir o serviço que será executado para a classe
            Servico = Servicos.ConsultaCadastroContribuinte;

            //Ler o XML para pegar parâmetros de envio
            LerXML oLer = new LerXML();
            oLer.ConsCad(this.vXmlNfeDadosMsg);
            ParametroEnvioXML oParam = new ParametroEnvioXML();
            oParam.tpAmb = Convert.ToInt32(oLer.oDadosConsCad.tpAmb);
            oParam.UFCod = Convert.ToInt32(oLer.oDadosConsCad.cUF);
            oParam.tpEmis = TipoEmissao.teNormal; //Sempre NORMAL pois SCAN não tem esta consulta de cadastro do contribuinte - 01/09/2009 Wandrey.

            //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
            object oServico = null;
            oDefObj.ConsultaCadastro(ref oServico, oParam);
            if (oServico == null)
            {
                string sAmbiente = (oParam.tpAmb == TipoAmbiente.taProducao ? "produção" : "homologação");
                oAux.GravarArqErroServico(this.vXmlNfeDadosMsg,
                    ExtXml.ConsCad,//"-cons-cad.xml",
                    "-ret-cons-cad.err",
                    "O Estado (" + oParam.UFCod.ToString() + ") configurado para a consulta do cadastro em ambiente de " + sAmbiente + " ainda não possui este serviço.");
            }
            else
            {
                if (oInvocarObj.Invocar(this, ConfiguracaoApp.VersaoXMLConsCad, oServico, "consultaCadastro", "-cons-cad", "-ret-cons-cad") == true)
                {
                    //Deletar o arquivo de solicitação do serviço
                    oAux.DeletarArquivo(this.vXmlNfeDadosMsg);
                }
            }
        }
        #endregion

        #endregion

        #region Métodos auxiliares

        #region CabecMsg()
        /// <summary>
        /// Auxiliar na geração do cabecalho da mensagem quando estivermos utilizando o InvokeMember para chamar o método
        /// </summary>
        /// <param name="cVersaoDados">Versão dos dados do XML</param>
        /// <returns>Conteúdo do XML</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>07/08/2009</date>
        public override string CabecMsg(string cVersaoDados)
        {
            return oGerarXML.CabecMsg(cVersaoDados);
        }
        #endregion

        #region XmlRetorno()
        /// <summary>
        /// Auxiliar na geração do arquivo XML de retorno para o ERP quando estivermos utilizando o InvokeMember para chamar o método
        /// </summary>
        /// <param name="pFinalArqEnvio">Final do nome do arquivo de solicitação do serviço.</param>
        /// <param name="pFinalArqRetorno">Final do nome do arquivo que é para ser gravado o retorno.</param>
        /// <date>07/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public override void XmlRetorno(string pFinalArqEnvio, string pFinalArqRetorno)
        {
            try
            {
                oGerarXML.XmlRetorno(pFinalArqEnvio, pFinalArqRetorno, this.vStrXmlRetorno);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region XmlPedRec()
        /// <summary>
        /// Auxiliar na geração do arquivo XML de consulta do recibo do lote quando estivermos utilizando o InvokeMember para chamar este método
        /// </summary>
        /// <param name="pFinalArqEnvio">Final do nome do arquivo de solicitação do serviço.</param>
        /// <param name="pFinalArqRetorno">Final do nome do arquivo que é para ser gravado o retorno.</param>
        /// <date>07/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public override void XmlPedRec(string nRec)
        {
            oGerarXML.XmlPedRec(nRec);
        }
        #endregion

        #region LoteNfe()
        /// <summary>
        /// Auxiliar na geração do arquivo XML de Lote de notas fiscais
        /// </summary>
        /// <param name="Arquivo">Nome do arquivo XML da NFe para montagem do lote de 1 NFe</param>
        /// <date>07/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public override void LoteNfe(string Arquivo)
        {
            oGerarXML.LoteNfe(Arquivo);
        }
        #endregion

        #region LoteNfe() - Sobrecarga
        /// <summary>
        /// Auxliar na geração do arquivo XML de Lote de notas fiscais
        /// </summary>
        /// <param name="lstArquivoNfe">Lista de arquivos de NFe para montagem do lote de várias NFe</param>
        /// <date>24/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public override void LoteNfe(List<string> lstArquivoNfe)
        {
            oGerarXML.LoteNfe(lstArquivoNfe);
        }
        #endregion

        #region VerStatusServico()
        /// <summary>
        /// Verifica e retorna o Status do Servido da NFE. Para isso este método gera o arquivo XML necessário
        /// para obter o status do serviõ e faz a leitura do XML de retorno, disponibilizando uma string com a mensagem
        /// obtida.
        /// </summary>
        /// <returns>Retorna uma string com a mensagem obtida do webservice de status do serviço da NFe</returns>
        /// <example>string vPastaArq = this.CriaArqXMLStatusServico();</example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2008</date>
        public override string VerStatusServico(int tpEmis, int cUF)
        {
            /// Wandrey - para sua avaliacao
            ///
            /// danasa 9-2009
            /// 
            /// verifica se tem conexão com a internet
            /// 
            if (!InternetCS.IsConnectedToInternet())
            {
                //Registrar o erro da validação para o sistema ERP
                return "Sem conexão com a internet";
            }

            //Criar XML para obter o status do serviço
            string XmlNfeDadosMsg = oGerarXML.StatusServico(tpEmis, cUF);

            //Retornar o status do serviço
            return oAux.VerStatusServico(XmlNfeDadosMsg);
        }
        #endregion

        #region ConsultaCadastro()

        /// <summary>
        /// Verifica um cadastro no site da receita.
        /// Voce deve preencher o estado e mais um dos tres itens: CPNJ, IE ou CPF.
        /// </summary>
        /// <param name="uf">Sigla do UF do cadastro a ser consultado. Tem que ter dois dígitos. SU para suframa.</param>
        /// <param name="cnpj"></param>
        /// <param name="ie"></param>
        /// <param name="cpf"></param>
        /// <returns>String com o resultado da consuta do cadastro</returns>
        /// <by>Marcos Diez</by>
        /// <date>29/08/2009</date>
        /*public string ConsultaCadastro(string uf, string cnpj, string ie, string cpf)
        {
            //Criar XML para obter o status do serviço
            string XmlNfeConsultaCadastro = oGerarXML.ConsultaCadastro(uf, cnpj, ie, cpf);

            //Retornar o status do serviço
            //return oAux.VerStatusServico(XmlNfeConsultaCadastro);
            return oAux.VerConsultaCadastro(XmlNfeConsultaCadastro);
        }*/
        public object ConsultaCadastroClass(string uf, string cnpj, string ie, string cpf)
        {
            //Criar XML para obter o cadastro do contribuinte
            string XmlNfeConsultaCadastro = oGerarXML.ConsultaCadastro(uf, cnpj, ie, cpf);

            return oAux.VerConsultaCadastroClass(XmlNfeConsultaCadastro);
        }
        #endregion

        #region LerXMLNFe()
        protected override absLerXML.DadosNFeClass LerXMLNFe(string Arquivo)
        {
            LerXML oLerXML = new LerXML();
            oLerXML.Nfe(Arquivo);

            return oLerXML.oDadosNfe;
        }
        #endregion

        #region LerRetornoLote()
        /// <summary>
        /// Efetua a leitura do XML de retorno do processamento do lote de notas fiscais e 
        /// atualiza o arquivo de fluxo e envio de notas
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        protected override void LerRetornoLote()
        {
            LerXML oLerXml = new LerXML();
            MemoryStream msXml = Auxiliar.StringXmlToStream(this.vStrXmlRetorno);

            FluxoNfe oFluxoNfe = new FluxoNfe();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(msXml);

                XmlNodeList retConsReciNFeList = doc.GetElementsByTagName("retConsReciNFe");

                foreach (XmlNode retConsReciNFeNode in retConsReciNFeList)
                {
                    XmlElement retConsReciNFeElemento = (XmlElement)retConsReciNFeNode;

                    //Pegar o número do recibo do lote enviado
                    string nRec = string.Empty;
                    if (retConsReciNFeElemento.GetElementsByTagName("nRec")[0] != null)
                    {
                        nRec = retConsReciNFeElemento.GetElementsByTagName("nRec")[0].InnerText;
                    }

                    //Pegar o status de retorno do lote enviado
                    string cStatLote = string.Empty;
                    if (retConsReciNFeElemento.GetElementsByTagName("cStat")[0] != null)
                    {
                        cStatLote = retConsReciNFeElemento.GetElementsByTagName("cStat")[0].InnerText;
                    }

                    if (cStatLote == "104") //Lote processado
                    {
                        XmlNodeList protNFeList = retConsReciNFeElemento.GetElementsByTagName("protNFe");

                        foreach (XmlNode protNFeNode in protNFeList)
                        {
                            XmlElement protNFeElemento = (XmlElement)protNFeNode;

                            string strProtNfe = protNFeElemento.OuterXml;

                            XmlNodeList infProtList = protNFeElemento.GetElementsByTagName("infProt");

                            foreach (XmlNode infProtNode in infProtList)
                            {
                                XmlElement infProtElemento = (XmlElement)infProtNode;

                                string strChaveNFe = string.Empty;
                                string strStat = string.Empty;
                                string strProt = string.Empty;

                                if (infProtElemento.GetElementsByTagName("chNFe")[0] != null)
                                {
                                    strChaveNFe = "NFe" + infProtElemento.GetElementsByTagName("chNFe")[0].InnerText;
                                }

                                if (infProtElemento.GetElementsByTagName("cStat")[0] != null)
                                {
                                    strStat = infProtElemento.GetElementsByTagName("cStat")[0].InnerText;
                                }

                                //Se o strStat for de rejeição a tag nProt não existe, assim sendo tenho que tratar
                                //para evitar um erro. Wandrey 01/06/2009
                                if (infProtElemento.GetElementsByTagName("nProt")[0] != null)
                                {
                                    strProt = infProtElemento.GetElementsByTagName("nProt")[0].InnerText;
                                }

                                //Definir o nome do arquivo da NFe e seu caminho
                                string strNomeArqNfe = oFluxoNfe.LerTag(strChaveNFe, FluxoNfe.ElementoFixo.ArqNFe);

                                ///
                                /// danasa 8-2009
                                /// se por algum motivo o XML não existir no "Fluxo", então o arquivo tem que existir
                                /// na pasta "EmProcessamento" assinada.
                                /// 
                                if (string.IsNullOrEmpty(strNomeArqNfe))
                                {
                                    if (string.IsNullOrEmpty(strChaveNFe))
                                        throw new Exception("LerRetornoLote(): Não pode obter o nome do arquivo");

                                    strNomeArqNfe = strChaveNFe.Substring(3) + ExtXml.Nfe;
                                    //throw new Exception(strChaveNFe + " não pode ser localizada");
                                }
                                string strArquivoNFe = ConfiguracaoApp.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + strNomeArqNfe;

                                //Atualizar a Tag de status da NFe no fluxo para que se ocorrer alguma falha na exclusão eu tenha esta campo para ter uma referencia em futuras consultas
                                oFluxoNfe.AtualizarTag(strChaveNFe, FluxoNfe.ElementoEditavel.cStat, strStat);

                                //Atualizar a tag da data e hora da ultima consulta do recibo
                                oFluxoNfe.AtualizarDPedRec(nRec, DateTime.Now);

                                switch (strStat)
                                {
                                    case "100": //NFe Autorizada
                                        //Juntar o protocolo com a NFE já copiando para a pasta de autorizadas
                                        oGerarXML.XmlDistNFe(strArquivoNFe, strProtNfe);
                                        string strArquivoNFeProc = ConfiguracaoApp.vPastaXMLEnviado + "\\" + 
                                                                    PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                                    oAux.ExtrairNomeArq(strNomeArqNfe, ExtXml.Nfe/*"-nfe.xml"*/) + ExtXml.ProcNFe;// "-procNFe.xml";

                                        //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s autorizados
                                        oLerXml.Nfe(strArquivoNFe);

                                        //Mover a NFE da pasta de NFE em processamento para NFe Autorizada
                                        oAux.MoverArquivo(strArquivoNFe, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);

                                        //Mover a nfePRoc da pasta de NFE em processamento para a NFe Autorizada
                                        oAux.MoverArquivo(strArquivoNFeProc, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);
                                        break;

                                    case "301": //NFe Denegada - Problemas com o emitente
                                        //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s Denegados
                                        oLerXml.Nfe(strArquivoNFe);

                                        //Mover a NFE da pasta de NFE em processamento para NFe Denegadas
                                        oAux.MoverArquivo(strArquivoNFe, PastaEnviados.Denegados, oLerXml.oDadosNfe.dEmi);
                                        break;

                                    case "302": //NFe Denegada - Problemas com o destinatário
                                        goto case "301";

                                    default: //NFe foi rejeitada
                                        //Mover o XML da NFE a pasta de XML´s com erro
                                        oAux.MoveArqErro(strArquivoNFe);
                                        break;
                                }

                                //Deletar a NFE do arquivo de controle de fluxo
                                oFluxoNfe.ExcluirNfeFluxo(strChaveNFe);
                                break;
                            }
                        }
                    }
                    else if (cStatLote == "105") //Lote em processamento
                    {
                        //Ok vou aguardar o ERP gerar uma nova consulta para encerrar o fluxo da nota
                    }
                    else if (cStatLote == "106") //Lote não encontrado
                    {
                        //Se o recibo que está sendo consultado não existir no SEFAZ, vamos excluir a nota do fluxo de envio
                        //TODO: 1-Tenho que ver isso ainda como vai ficar
                        //Tenho somente o Recibo, a partir dele tenho que encontrar a chave da NFe ou das NFs e excluir do fluxo
                        //ou criar uma exclusão do fluxo através do recibo.
                        //TODO: URGENTE - Aqui é um ponto que pode estar deixando o XML no fluxo sem tirar
                    }
                    else //Rejeitou a consulta do lote
                    {
                        //A consulta do recibo do lote pode ser rejeitada por diversas motivos, eu não posso tirar do fluxo
                        //tenho que continuar tentando consultar, o ERP vai ter que analisar o retorno gravado, arrumar o problema, 
                        //se preciso for sair do uninfe entrar novamente para que ele gere nova consulta e tente finalizar.
                        //Wandrey 18/08/2009
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region LerRetornoCanc()
        /// <summary>
        /// Efetua a leitura do XML de retorno do processamento do cancelamento
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        protected override void LerRetornoCanc()
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                MemoryStream msXml = Auxiliar.StringXmlToStream(this.vStrXmlRetorno);
                doc.Load(msXml);

                XmlNodeList retCancNFeList = doc.GetElementsByTagName("retCancNFe");

                foreach (XmlNode retCancNFeNode in retCancNFeList)
                {
                    XmlElement retCancNFeElemento = (XmlElement)retCancNFeNode;

                    XmlNodeList infCancList = retCancNFeElemento.GetElementsByTagName("infCanc");

                    foreach (XmlNode infCancNode in infCancList)
                    {
                        XmlElement infCancElemento = (XmlElement)infCancNode;

                        if (infCancElemento.GetElementsByTagName("cStat")[0].InnerText == "101") //Cancelamento Homologado
                        {
                            string strRetCancNFe = retCancNFeNode.OuterXml;

                            oGerarXML.XmlDistCanc(this.vXmlNfeDadosMsg, strRetCancNFe);
                            ///
                            /// danasa 9-2009
                            /// pega a data da emissão da nota para mover os XML's para a pasta de origem da NFe
                            /// 
                            string cChaveNFe = infCancElemento.GetElementsByTagName("chNFe")[0].InnerText;
                            //TODO: Cancelamento - Acertar a data
                            DateTime dtEmissaoNFe = new DateTime(Convert.ToInt16("20" + cChaveNFe.Substring(2, 2)), Convert.ToInt16(cChaveNFe.Substring(4, 2)), 1);
                            //DateTime dtEmissaoNFe = DateTime.Now;
                            

                            //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                            oAux.MoverArquivo(this.vXmlNfeDadosMsg, PastaEnviados.Autorizados, dtEmissaoNFe);//DateTime.Now);

                            //Move o arquivo de Distribuição para a pasta de enviados autorizados
                            string strNomeArqProcCancNFe = ConfiguracaoApp.vPastaXMLEnviado + "\\" + 
                                                            PastaEnviados.EmProcessamento + "\\" +
                                                            oAux.ExtrairNomeArq(this.vXmlNfeDadosMsg, ExtXml.PedCan/*"-ped-can.xml"*/) + ExtXml.ProcCancNFe;// "-procCancNFe.xml";
                            oAux.MoverArquivo(strNomeArqProcCancNFe, PastaEnviados.Autorizados, dtEmissaoNFe);//DateTime.Now);
                        }
                        else
                        {
                            //Deletar o arquivo de solicitação do serviço da pasta de envio
                            oAux.DeletarArquivo(this.vXmlNfeDadosMsg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region LerRetornoInut()
        /// <summary>
        /// Efetua a leitura do XML de retorno do processamento da Inutilização
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        protected override void LerRetornoInut()
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                MemoryStream msXml = Auxiliar.StringXmlToStream(this.vStrXmlRetorno);
                doc.Load(msXml);

                XmlNodeList retInutNFeList = doc.GetElementsByTagName("retInutNFe");

                foreach (XmlNode retInutNFeNode in retInutNFeList)
                {
                    XmlElement retInutNFeElemento = (XmlElement)retInutNFeNode;

                    XmlNodeList infInutList = retInutNFeElemento.GetElementsByTagName("infInut");

                    foreach (XmlNode infInutNode in infInutList)
                    {
                        XmlElement infInutElemento = (XmlElement)infInutNode;

                        if (infInutElemento.GetElementsByTagName("cStat")[0].InnerText == "102") //Inutilização de Número Homologado
                        {
                            string strRetInutNFe = retInutNFeNode.OuterXml;

                            oGerarXML.XmlDistInut(this.vXmlNfeDadosMsg, strRetInutNFe);

                            //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                            oAux.MoverArquivo(this.vXmlNfeDadosMsg, PastaEnviados.Autorizados, DateTime.Now);

                            //Move o arquivo de Distribuição para a pasta de enviados autorizados
                            string strNomeArqProcInutNFe = ConfiguracaoApp.vPastaXMLEnviado + "\\" + 
                                                            PastaEnviados.EmProcessamento + "\\" +
                                                            oAux.ExtrairNomeArq(this.vXmlNfeDadosMsg, ExtXml.PedInu/*"-ped-inu.xml"*/) + ExtXml.ProcInutNFe;// "-procInutNFe.xml";
                            oAux.MoverArquivo(strNomeArqProcInutNFe, PastaEnviados.Autorizados, DateTime.Now);
                        }
                        else
                        {
                            //Deletar o arquivo de solicitação do serviço da pasta de envio
                            oAux.DeletarArquivo(this.vXmlNfeDadosMsg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #endregion
    }
    #endregion
}