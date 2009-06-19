using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace uninfe
{
    #region Classe UniNfeClass
    public class UniNfeClass
    {
        #region Enumeradores

        /// <summary>
        /// Tipos de pastas dos XML enviados
        /// </summary>
        private enum PastaEnviados
        {
            EmProcessamento,
            Autorizados,
            Denegados
        }

        #endregion

        #region propriedades da classe

        /// <summary>
        /// Certificado digital a ser utilizado
        /// </summary>
        public X509Certificate2 oCertificado { get; set; }
        /// <summary>
        /// Código do estado que é para enviar a nota fiscal eletrônica
        /// </summary>
        public int vUF { get; set; }
        /// <summary>
        /// Código do Ambiente que é para certificar a Nota Fiscal Eletrônica
        /// </summary>
        public int vAmbiente { get; set; }
        /// <summary>
        /// Tipo de emissão 1-Normal 2-Contingência em formulário de Segurança 3-Contingência SCAN 4-Contingência Eletrônica
        /// </summary>
        public int vTpEmis { get; set; }
        /// <summary>
        /// Arquivo XML contendo os dados a serem enviados (Nota Fiscal, Pedido de Status, Cancelamento, etc...)
        /// </summary>
        public string vXmlNfeDadosMsg { get; set; }
        /// <summary>
        /// Conteúdo do XML de retorno do serviço, ou seja, para cada serviço invocado a classe seta neste atributo a string do XML Retornado pelo serviço
        /// </summary>
        public string vStrXmlRetorno { get; private set; }
        /// <summary>
        /// Pasta e nome do arquivo dos XML´s retornados pelos WebServices, sempre que um serviço for consumido, nesta propriedade será setado o caminho e nome do arquivo XML gravado que tem o conteúdo de retorno do webservice.
        /// </summary>
        public string vArqXMLRetorno { get; private set; }
        /// <summary>
        /// Pasta que contém os XMLs para serem enviados para o WebService
        /// </summary>
        public string vPastaXMLEnvio { get; set; }
        /// <summary>
        /// Pasta que contém os XMLs para serem enviados em lote para o WebService 
        /// </summary>
        public string vPastaXMLEmLote { get; set; }
        /// <summary>
        /// Pasta que é para ser gravado os XML´s retornados pelo WebService
        /// </summary>
        public string vPastaXMLRetorno { get; set; }
        /// <summary>
        /// Pasta onde vai gravar os XML´s que foram assinados e enviados
        /// </summary>
        public string vPastaXMLEnviado { get; set; }
        /// <summary>
        /// Pasta para arquivamento temporário dos XML que apresentaram erro na validação
        /// </summary>
        public string vPastaXMLErro { get; set; }
        /// <summary>
        /// Pasta para gravar o backup dos XML enviados
        /// </summary>
        public string cPastaBackup { get; set; }
        /// <summary>
        /// Pasta para onde o UNINFE vai verificar se tem XML para ser somente Validado e Assinado
        /// </summary>
        public string PastaValidar { get; set; }

        #endregion

        #region Atributos

        /// <summary>
        /// Data que vai ser utilizada para criar a sub-pasta dentro da pasta dos xml enviados
        /// </summary>
        private DateTime dtDataToPastaXml;
        /// <summary>
        /// Pasta e nome do arquivo dos Erros ocorridos ao tentar consumir um serviço.
        /// </summary>
        private string vArqERRRetorno;
        /// <summary>
        /// Atributo que vai receber a string do XML de lote de NFe´s para que este conteúdo seja gravado após finalizado em arquivo físico no HD
        /// </summary>
        private string strXMLLoteNfe;

        #endregion

        #region Constantes
        private string vArqXmlLote = UniNfeInfClass.PastaExecutavel() + "\\UniNfeLote.xml";
        public const string strNomeSubPastaAssinado = "\\Assinado";
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
        public void StatusServico()
        {
            //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
            object oServico = null;
            this.DefObjStatusServico(ref oServico);
            if (this.InvocarObjeto("1.07", oServico, "nfeStatusServicoNF", "-ped-sta", "-sta") == true)
            {
                //Deletar o arquivo de solicitação do serviço
                this.MoveDeleteArq(this.vXmlNfeDadosMsg, "D");
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
        public void Recepcao()
        {
            //Ler o XML de Lote para pegar o número do lote que está sendo enviado
            UniLerXMLClass oLerXml = new UniLerXMLClass();
            oLerXml.Nfe(this.vXmlNfeDadosMsg);

            string idLote = oLerXml.oDadosNfe.idLote;
            FluxoNfe oFluxoNfe = new FluxoNfe();

            //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
            object oServico = null;
            this.DefObjRecepcao(ref oServico);
            if (this.InvocarObjeto("1.10", oServico, "nfeRecepcaoLote", "-env-lot", "-rec") == true)
            {
                UniLerXMLClass oLerRecibo = new UniLerXMLClass();
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
                    this.MoveDeleteArq(this.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento + "\\" + oFluxoNfe.LerTag(oLerXml.oDadosNfe.chavenfe, FluxoNfe.ElementoFixo.ArqNFe), "D");

                    oFluxoNfe.ExcluirNfeFluxo(oLerXml.oDadosNfe.chavenfe);
                }

                //Deleta o arquivo de lote
                this.MoveDeleteArq(this.vXmlNfeDadosMsg, "D");
            }
            else
            {
                //Se falhou algo no envio vamos ter que excluir a nota do fluxo, 
                //Primeiro deleta o xml da nota da pasta EmProcessamento e depois tira ela do fluxo
                //Wandrey 30/04/2009
                this.MoveDeleteArq(this.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento + "\\" + oFluxoNfe.LerTag(oLerXml.oDadosNfe.chavenfe, FluxoNfe.ElementoFixo.ArqNFe), "D");
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
        public void RetRecepcao()
        {
            //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
            object oServico = null;
            this.DefObjRetRecepcao(ref oServico);

            //Invoca o método para acessar o webservice do SEFAZ mas sem gravar o XML retornado pelo mesmo
            if (this.InvocarObjeto("1.10", oServico, "nfeRetRecepcao") == true)
            {
                try
                {
                    //Efetuar a leituras das notas do lote para ver se foi autorizada ou não
                    this.LerRetornoLote();

                    //Gravar o XML retornado pelo WebService do SEFAZ na pasta de retorno para o ERP
                    //Tem que ser feito neste ponto, pois somente aqui terminamos todo o processo
                    //Wandrey 18/06/2009
                    this.GravarXmlRetorno("-ped-rec.xml", "-pro-rec.xml");

                    //Deletar o arquivo de solicitação do serviço
                    this.MoveDeleteArq(this.vXmlNfeDadosMsg, "D");
                }
                catch (Exception ex)
                {
                    //Deletar o arquivo de solicitação do serviço
                    this.MoveDeleteArq(this.vXmlNfeDadosMsg, "D");

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
        public void Consulta()
        {
            //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
            object oServico = null;
            this.DefObjConsulta(ref oServico);
            if (this.InvocarObjeto("1.07", oServico, "nfeConsultaNF", "-ped-sit", "-sit") == true)
            {
                //Deletar o arquivo de solicitação do serviço
                this.MoveDeleteArq(this.vXmlNfeDadosMsg, "D");
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
        public void Cancelamento()
        {
            //Assinar o arquivo XML
            UniAssinaturaDigitalClass oAD = new UniAssinaturaDigitalClass();
            try
            {
                oAD.Assinar(this.vXmlNfeDadosMsg, "infCanc", this.oCertificado);
                if (oAD.intResultado == 0)
                {
                    //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
                    object oServico = null;
                    this.DefObjCancelamento(ref oServico);
                    if (this.InvocarObjeto("1.07", oServico, "nfeCancelamentoNF", "-ped-can", "-can") == true)
                    {
                        //Setar a propriedade que vai determinar a pasta que vai ser gravado
                        //o XML enviado
                        this.dtDataToPastaXml = DateTime.Now; //Data Atual

                        this.LerRetornoCanc();
                    }
                }
            }
            catch (Exception ex)
            {
                this.GravarArqErroServico("-ped-can.xml", "-can.err", ex.Message);
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
        public void Inutilizacao()
        {
            //Assinar o arquivo XML
            UniAssinaturaDigitalClass oAD = new UniAssinaturaDigitalClass();
            try
            {
                oAD.Assinar(this.vXmlNfeDadosMsg, "infInut", this.oCertificado);

                if (oAD.intResultado == 0)
                {
                    //Resolver um falha do estado da bahia que gera um método com nome diferente dos demais estados
                    string cMetodo = "nfeInutilizacaoNF";
                    if (this.vUF == 29 && this.vTpEmis != 3)
                    {
                        cMetodo = "nfeInutilizacao";
                    }

                    //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
                    object oServico = null;
                    this.DefObjInutilizacao(ref oServico);
                    if (this.InvocarObjeto("1.07", oServico, cMetodo, "-ped-inu", "-inu") == true)
                    {
                        //Setar a propriedade que vai determinar a pasta que vai ser gravado
                        //o XML enviado
                        this.dtDataToPastaXml = DateTime.Now; //Data Atual

                        this.LerRetornoInut();
                    }
                }
            }
            catch (Exception ex)
            {
                this.GravarArqErroServico("-ped-inu.xml", "-inu.err", ex.Message);
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
        public void ConsultaCadastro()
        {
            //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
            object oServico = null;
            this.DefObjConsultaCadastro(ref oServico);
            if (oServico == null)
            {
                string sAmbiente;
                if (vAmbiente == 1)
                {
                    sAmbiente = "produção";
                }
                else
                {
                    sAmbiente = "homologação";
                }

                this.GravarArqErroServico("-cons-cad.xml", "-ret-cons-cad.err", "O Estado (" + vUF.ToString() + ") configurado para a consulta do cadastro em ambiente de " + sAmbiente + " ainda não possui este serviço.");
            }
            else
            {
                if (this.InvocarObjeto("1.01", oServico, "consultaCadastro", "-cons-cad", "-ret-cons-cad") == true)
                {
                    //Deletar o arquivo de solicitação do serviço
                    this.MoveDeleteArq(this.vXmlNfeDadosMsg, "D");
                }
            }
        }
        #endregion

        #endregion

        #region Métodos de definição dos objetos

        #region DefObjStatusServico()
        /// <summary>
        /// Defini o Objeto que vai ser utilizado para consultar o status do 
        /// serviço de acordo com o Estado e Ambiente informado para a classe
        /// </summary>
        /// <param name="pObjeto">Objeto que é para receber a instancia do serviço</param>
        /// <example>
        /// object oServico = null;
        /// this.DefObjStatusServico(ref oServico);                       
        /// Type tipoServico = oServico.GetType();
        /// 
        /// object oClientCertificates;
        /// Type tipoClientCertificates;
        /// 
        /// oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, oServico, new Object[] { });
        /// tipoClientCertificates = oClientCertificates.GetType();
        /// tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
        ///          
        /// this.vNfeRetorno = string.Empty;
        /// 
        /// this.vNfeRetorno = (string)(tipoServico.InvokeMember("nfeStatusServicoNF", System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2008</date>
        private void DefObjStatusServico(ref object pObjeto)
        {
            if (this.vAmbiente == 1)
            {
                if (this.vTpEmis == 3) { pObjeto = new wsSCANPStatusServico.NfeStatusServico(); } //Contingência SCAN Ambiente Nascional
                else if (this.vUF == 51) { pObjeto = new wsMTPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 43) { pObjeto = new wsRSPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 31) { pObjeto = new wsMGPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 35) { pObjeto = new wsSPPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 50) { pObjeto = new wsMSPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 52) { pObjeto = new wsGOPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 41) { pObjeto = new wsPRPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 23) { pObjeto = new wsCEPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 29) { pObjeto = new wsBAPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 53) { pObjeto = new wsDFPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 26) { pObjeto = new wsPEPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 11) { pObjeto = new wsROPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 13) { pObjeto = new wsAMPStatusServico.NfeStatusServico(); }

                else if (this.vUF == 15) { pObjeto = new wsVNPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 21) { pObjeto = new wsVNPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 22) { pObjeto = new wsVNPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 24) { pObjeto = new wsVNPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 32) { pObjeto = new wsVNPStatusServico.NfeStatusServico(); }

                else if (this.vUF == 42) { pObjeto = new wsVRPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 17) { pObjeto = new wsVRPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 28) { pObjeto = new wsVRPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 33) { pObjeto = new wsVRPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 12) { pObjeto = new wsVRPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 27) { pObjeto = new wsVRPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 16) { pObjeto = new wsVRPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 25) { pObjeto = new wsVRPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 14) { pObjeto = new wsVRPStatusServico.NfeStatusServico(); }
            }
            else if (this.vAmbiente == 2)
            {
                if (this.vTpEmis == 3) { pObjeto = new wsSCANHStatusServico.NfeStatusServico(); } //Contingência SCAN Ambiente Nascional
                else if (this.vUF == 51) { pObjeto = new wsMTHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 43) { pObjeto = new wsRSHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 31) { pObjeto = new wsMGHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 35) { pObjeto = new wsSPHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 50) { pObjeto = new wsMSHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 52) { pObjeto = new wsGOHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 41) { pObjeto = new wsPRHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 23) { pObjeto = new wsCEHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 29) { pObjeto = new wsBAHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 53) { pObjeto = new wsDFHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 26) { pObjeto = new wsPEHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 11) { pObjeto = new wsROHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 13) { pObjeto = new wsAMHStatusServico.NfeStatusServico(); }

                else if (this.vUF == 15) { pObjeto = new wsVNHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 21) { pObjeto = new wsVNHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 22) { pObjeto = new wsVNHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 24) { pObjeto = new wsVNHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 32) { pObjeto = new wsVNHStatusServico.NfeStatusServico(); }

                else if (this.vUF == 42) { pObjeto = new wsVRHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 17) { pObjeto = new wsVRHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 28) { pObjeto = new wsVRHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 33) { pObjeto = new wsVRHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 12) { pObjeto = new wsVRHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 27) { pObjeto = new wsVRHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 16) { pObjeto = new wsVRHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 25) { pObjeto = new wsVRHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 14) { pObjeto = new wsVRHStatusServico.NfeStatusServico(); }
            }
        }
        #endregion

        #region DefObjRecepcao()
        /// <summary>
        /// Defini o Objeto que vai ser utilizado para enviar a nota fiscal
        /// de acordo com o Estado e Ambiente informado para a classe
        /// </summary>
        /// <param name="pObjeto">Objeto que é para receber a instancia do serviço</param>
        /// <example>
        /// object oServico = null;
        /// this.DefObjRecepcao(ref oServico);                       
        /// Type tipoServico = oServico.GetType();
        /// 
        /// object oClientCertificates;
        /// Type tipoClientCertificates;
        /// 
        /// oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, oServico, new Object[] { });
        /// tipoClientCertificates = oClientCertificates.GetType();
        /// tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
        ///          
        /// this.vNfeRetorno = string.Empty;
        /// 
        /// this.vNfeRetorno = (string)(tipoServico.InvokeMember("nfeRecepcao", System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2008</date>
        private void DefObjRecepcao(ref object pObjeto)
        {
            if (this.vAmbiente == 1)
            {
                if (this.vTpEmis == 3) { pObjeto = new wsSCANPRecepcao.NfeRecepcao(); } //Contingência SCAN Ambiente Nascional
                else if (this.vUF == 51) { pObjeto = new wsMTPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 43) { pObjeto = new wsRSPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 31) { pObjeto = new wsMGPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 35) { pObjeto = new wsSPPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 50) { pObjeto = new wsMSPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 52) { pObjeto = new wsGOPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 41) { pObjeto = new wsPRPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 23) { pObjeto = new wsCEPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 29) { pObjeto = new wsBAPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 53) { pObjeto = new wsDFPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 26) { pObjeto = new wsPEPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 11) { pObjeto = new wsROPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 13) { pObjeto = new wsAMPRecepcao.NfeRecepcao(); }

                else if (this.vUF == 15) { pObjeto = new wsVNPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 21) { pObjeto = new wsVNPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 22) { pObjeto = new wsVNPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 24) { pObjeto = new wsVNPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 32) { pObjeto = new wsVNPRecepcao.NfeRecepcao(); }

                else if (this.vUF == 42) { pObjeto = new wsVRPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 17) { pObjeto = new wsVRPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 28) { pObjeto = new wsVRPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 33) { pObjeto = new wsVRPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 12) { pObjeto = new wsVRPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 27) { pObjeto = new wsVRPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 16) { pObjeto = new wsVRPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 25) { pObjeto = new wsVRPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 14) { pObjeto = new wsVRPRecepcao.NfeRecepcao(); }
            }
            else if (this.vAmbiente == 2)
            {
                if (this.vTpEmis == 3) { pObjeto = new wsSCANHRecepcao.NfeRecepcao(); } //Contingência SCAN Ambiente Nascional
                else if (this.vUF == 51) { pObjeto = new wsMTHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 43) { pObjeto = new wsRSHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 31) { pObjeto = new wsMGHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 35) { pObjeto = new wsSPHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 50) { pObjeto = new wsMSHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 52) { pObjeto = new wsGOHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 41) { pObjeto = new wsPRHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 23) { pObjeto = new wsCEHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 29) { pObjeto = new wsBAHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 53) { pObjeto = new wsDFHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 26) { pObjeto = new wsPEHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 11) { pObjeto = new wsROHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 13) { pObjeto = new wsAMHRecepcao.NfeRecepcao(); }

                else if (this.vUF == 15) { pObjeto = new wsVNHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 21) { pObjeto = new wsVNHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 22) { pObjeto = new wsVNHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 24) { pObjeto = new wsVNHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 32) { pObjeto = new wsVNHRecepcao.NfeRecepcao(); }

                else if (this.vUF == 42) { pObjeto = new wsVRHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 17) { pObjeto = new wsVRHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 28) { pObjeto = new wsVRHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 33) { pObjeto = new wsVRHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 12) { pObjeto = new wsVRHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 27) { pObjeto = new wsVRHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 16) { pObjeto = new wsVRHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 25) { pObjeto = new wsVRHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 14) { pObjeto = new wsVRHRecepcao.NfeRecepcao(); }
            }
        }
        #endregion

        #region DefObjRetRecepcao()
        /// <summary>
        /// Defini o Objeto que vai ser utilizado para receber o retorno
        /// da situação da nota fiscal de acordo com o Estado e Ambiente
        /// informado para a classe
        /// </summary>
        /// <param name="pObjeto">Objeto que vai receber a instância do serviço</param>
        /// <example>
        /// object oServico = null;
        /// this.DefObjRetRecepcao(ref oServico);                       
        /// Type tipoServico = oServico.GetType();
        /// 
        /// object oClientCertificates;
        /// Type tipoClientCertificates;
        /// 
        /// oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, oServico, new Object[] { });
        /// tipoClientCertificates = oClientCertificates.GetType();
        /// tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
        ///          
        /// this.vNfeRetorno = string.Empty;
        /// 
        /// this.vNfeRetorno = (string)(tipoServico.InvokeMember("nfeRetRecepcao", System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2008</date>
        private void DefObjRetRecepcao(ref object pObjeto)
        {
            if (this.vAmbiente == 1)
            {
                if (this.vTpEmis == 3) { pObjeto = new wsSCANPRetRecepcao.NfeRetRecepcao(); } //Contingência SCAN Ambiente Nascional
                else if (this.vUF == 51) { pObjeto = new wsMTPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 43) { pObjeto = new wsRSPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 31) { pObjeto = new wsMGPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 35) { pObjeto = new wsSPPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 50) { pObjeto = new wsMSPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 52) { pObjeto = new wsGOPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 41) { pObjeto = new wsPRPRetRecepcao.NfeRetRecepcaoService(); }
                else if (this.vUF == 23) { pObjeto = new wsCEPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 29) { pObjeto = new wsBAPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 53) { pObjeto = new wsDFPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 26) { pObjeto = new wsPEPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 11) { pObjeto = new wsROPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 13) { pObjeto = new wsAMPRetRecepcao.NfeRetRecepcao(); }

                else if (this.vUF == 15) { pObjeto = new wsVNPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 21) { pObjeto = new wsVNPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 22) { pObjeto = new wsVNPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 24) { pObjeto = new wsVNPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 32) { pObjeto = new wsVNPRetRecepcao.NfeRetRecepcao(); }

                else if (this.vUF == 42) { pObjeto = new wsVRPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 17) { pObjeto = new wsVRPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 28) { pObjeto = new wsVRPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 33) { pObjeto = new wsVRPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 12) { pObjeto = new wsVRPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 27) { pObjeto = new wsVRPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 16) { pObjeto = new wsVRPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 25) { pObjeto = new wsVRPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 14) { pObjeto = new wsVRPRetRecepcao.NfeRetRecepcao(); }
            }
            else if (this.vAmbiente == 2)
            {
                if (this.vTpEmis == 3) { pObjeto = new wsSCANHRetRecepcao.NfeRetRecepcao(); } //Contingência SCAN Ambiente Nascional
                else if (this.vUF == 51) { pObjeto = new wsMTHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 43) { pObjeto = new wsRSHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 31) { pObjeto = new wsMGHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 35) { pObjeto = new wsSPHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 50) { pObjeto = new wsMSHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 52) { pObjeto = new wsGOHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 41) { pObjeto = new wsPRHRetRecepcao.NfeRetRecepcaoService(); }
                else if (this.vUF == 23) { pObjeto = new wsCEHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 29) { pObjeto = new wsBAHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 53) { pObjeto = new wsDFHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 26) { pObjeto = new wsPEHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 11) { pObjeto = new wsROHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 13) { pObjeto = new wsAMHRetRecepcao.NfeRetRecepcao(); }

                else if (this.vUF == 15) { pObjeto = new wsVNHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 21) { pObjeto = new wsVNHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 22) { pObjeto = new wsVNHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 24) { pObjeto = new wsVNHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 32) { pObjeto = new wsVNHRetRecepcao.NfeRetRecepcao(); }

                else if (this.vUF == 42) { pObjeto = new wsVRHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 17) { pObjeto = new wsVRHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 28) { pObjeto = new wsVRHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 33) { pObjeto = new wsVRHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 12) { pObjeto = new wsVRHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 27) { pObjeto = new wsVRHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 16) { pObjeto = new wsVRHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 25) { pObjeto = new wsVRHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 14) { pObjeto = new wsVRHRetRecepcao.NfeRetRecepcao(); }
            }
        }
        #endregion

        #region DefObjConsulta
        /// <summary>
        /// Defini o Objeto que vai ser utilizado para consultar   
        /// a situação da nota fiscal de acordo com o Estado e Ambiente
        /// informado para a classe
        /// </summary>
        /// <param name="pObjeto">Variável de objeto que é para receber a instancia do serviço.</param>
        /// <example>
        /// object oServico = null;
        /// this.DefObjConsulta(ref oServico);                       
        /// Type tipoServico = oServico.GetType();
        /// 
        /// object oClientCertificates;
        /// Type tipoClientCertificates;
        /// 
        /// oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, oServico, new Object[] { });
        /// tipoClientCertificates = oClientCertificates.GetType();
        /// tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
        ///          
        /// this.vNfeRetorno = string.Empty;
        /// 
        /// this.vNfeRetorno = (string)(tipoServico.InvokeMember("nfeConsultaNF", System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2008</date>
        private void DefObjConsulta(ref object pObjeto)
        {
            if (this.vAmbiente == 1)
            {
                if (this.vTpEmis == 3) { pObjeto = new wsSCANPConsulta.NfeConsulta(); } //Contingência SCAN Ambiente Nascional
                else if (this.vUF == 51) { pObjeto = new wsMTPConsulta.NfeConsulta(); }
                else if (this.vUF == 43) { pObjeto = new wsRSPConsulta.NfeConsulta(); }
                else if (this.vUF == 31) { pObjeto = new wsMGPConsulta.NfeConsulta(); }
                else if (this.vUF == 35) { pObjeto = new wsSPPConsulta.NfeConsulta(); }
                else if (this.vUF == 50) { pObjeto = new wsMSPConsulta.NfeConsulta(); }
                else if (this.vUF == 52) { pObjeto = new wsGOPConsulta.NfeConsulta(); }
                else if (this.vUF == 41) { pObjeto = new wsPRPConsulta.NfeConsultaService(); }
                else if (this.vUF == 23) { pObjeto = new wsCEPConsulta.NfeConsulta(); }
                else if (this.vUF == 29) { pObjeto = new wsBAPConsulta.NfeConsulta(); }
                else if (this.vUF == 53) { pObjeto = new wsDFPConsulta.NfeConsulta(); }
                else if (this.vUF == 26) { pObjeto = new wsPEPConsulta.NfeConsulta(); }
                else if (this.vUF == 11) { pObjeto = new wsROPConsulta.NfeConsulta(); }
                else if (this.vUF == 13) { pObjeto = new wsAMPConsulta.NfeConsulta(); }

                else if (this.vUF == 15) { pObjeto = new wsVNPConsulta.NfeConsulta(); }
                else if (this.vUF == 21) { pObjeto = new wsVNPConsulta.NfeConsulta(); }
                else if (this.vUF == 22) { pObjeto = new wsVNPConsulta.NfeConsulta(); }
                else if (this.vUF == 24) { pObjeto = new wsVNPConsulta.NfeConsulta(); }
                else if (this.vUF == 32) { pObjeto = new wsVNPConsulta.NfeConsulta(); }

                else if (this.vUF == 42) { pObjeto = new wsVRPConsulta.NfeConsulta(); }
                else if (this.vUF == 17) { pObjeto = new wsVRPConsulta.NfeConsulta(); }
                else if (this.vUF == 28) { pObjeto = new wsVRPConsulta.NfeConsulta(); }
                else if (this.vUF == 33) { pObjeto = new wsVRPConsulta.NfeConsulta(); }
                else if (this.vUF == 12) { pObjeto = new wsVRPConsulta.NfeConsulta(); }
                else if (this.vUF == 27) { pObjeto = new wsVRPConsulta.NfeConsulta(); }
                else if (this.vUF == 16) { pObjeto = new wsVRPConsulta.NfeConsulta(); }
                else if (this.vUF == 25) { pObjeto = new wsVRPConsulta.NfeConsulta(); }
                else if (this.vUF == 14) { pObjeto = new wsVRPConsulta.NfeConsulta(); }
            }
            else if (this.vAmbiente == 2)
            {
                if (this.vTpEmis == 3) { pObjeto = new wsSCANHConsulta.NfeConsulta(); } //Contingência SCAN Ambiente Nascional
                else if (this.vUF == 51) { pObjeto = new wsMTHConsulta.NfeConsulta(); }
                else if (this.vUF == 43) { pObjeto = new wsRSHConsulta.NfeConsulta(); }
                else if (this.vUF == 31) { pObjeto = new wsMGHConsulta.NfeConsulta(); }
                else if (this.vUF == 35) { pObjeto = new wsSPHConsulta.NfeConsulta(); }
                else if (this.vUF == 50) { pObjeto = new wsMSHConsulta.NfeConsulta(); }
                else if (this.vUF == 52) { pObjeto = new wsGOHConsulta.NfeConsulta(); }
                else if (this.vUF == 41) { pObjeto = new wsPRHConsulta.NfeConsultaService(); }
                else if (this.vUF == 23) { pObjeto = new wsCEHConsulta.NfeConsulta(); }
                else if (this.vUF == 29) { pObjeto = new wsBAHConsulta.NfeConsulta(); }
                else if (this.vUF == 53) { pObjeto = new wsDFHConsulta.NfeConsulta(); }
                else if (this.vUF == 26) { pObjeto = new wsPEHConsulta.NfeConsulta(); }
                else if (this.vUF == 11) { pObjeto = new wsROHConsulta.NfeConsulta(); }
                else if (this.vUF == 13) { pObjeto = new wsAMHConsulta.NfeConsulta(); }

                else if (this.vUF == 15) { pObjeto = new wsVNHConsulta.NfeConsulta(); }
                else if (this.vUF == 21) { pObjeto = new wsVNHConsulta.NfeConsulta(); }
                else if (this.vUF == 22) { pObjeto = new wsVNHConsulta.NfeConsulta(); }
                else if (this.vUF == 24) { pObjeto = new wsVNHConsulta.NfeConsulta(); }
                else if (this.vUF == 32) { pObjeto = new wsVNHConsulta.NfeConsulta(); }

                else if (this.vUF == 42) { pObjeto = new wsVRHConsulta.NfeConsulta(); }
                else if (this.vUF == 17) { pObjeto = new wsVRHConsulta.NfeConsulta(); }
                else if (this.vUF == 28) { pObjeto = new wsVRHConsulta.NfeConsulta(); }
                else if (this.vUF == 33) { pObjeto = new wsVRHConsulta.NfeConsulta(); }
                else if (this.vUF == 12) { pObjeto = new wsVRHConsulta.NfeConsulta(); }
                else if (this.vUF == 27) { pObjeto = new wsVRHConsulta.NfeConsulta(); }
                else if (this.vUF == 16) { pObjeto = new wsVRHConsulta.NfeConsulta(); }
                else if (this.vUF == 25) { pObjeto = new wsVRHConsulta.NfeConsulta(); }
                else if (this.vUF == 14) { pObjeto = new wsVRHConsulta.NfeConsulta(); }
            }
        }
        #endregion

        #region DefObjCancelamento()
        /// <summary>
        /// Definir o Objeto que vai ser utilizado para cancelar notas fiscais de acordo com o 
        /// Estado e Ambiente informado para a classe
        /// </summary>
        /// <example>
        /// object oServico = null;
        /// this.DefObjCancelamento(ref oServico);                       
        /// 
        /// Type tipoServico = oServico.GetType();
        /// 
        /// object oClientCertificates;
        /// Type tipoClientCertificates;
        /// 
        /// oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, oServico, new Object[] { });
        /// tipoClientCertificates = oClientCertificates.GetType();
        /// tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
        /// 
        /// this.vNfeRetorno = string.Empty;
        /// 
        /// this.vNfeRetorno = (string)(tipoServico.InvokeMember("nfeCancelamentoNF", System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));
        /// </example>
        /// <param name="pObjeto">Objeto que é para receber a instância do serviço</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>01/07/2008</date>
        private void DefObjCancelamento(ref object pObjeto)
        {
            if (this.vAmbiente == 1)
            {
                if (this.vTpEmis == 3) { pObjeto = new wsSCANPCancelamento.NfeCancelamento(); } //Contingência SCAN Ambiente Nascional
                else if (this.vUF == 51) { pObjeto = new wsMTPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 43) { pObjeto = new wsRSPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 31) { pObjeto = new wsMGPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 35) { pObjeto = new wsSPPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 50) { pObjeto = new wsMSPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 52) { pObjeto = new wsGOPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 41) { pObjeto = new wsPRPCancelamento.NfeCancelamentoService(); }
                else if (this.vUF == 23) { pObjeto = new wsCEPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 29) { pObjeto = new wsBAPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 53) { pObjeto = new wsDFPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 26) { pObjeto = new wsPEPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 11) { pObjeto = new wsROPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 13) { pObjeto = new wsAMPCancelamento.NfeCancelamento(); }

                else if (this.vUF == 15) { pObjeto = new wsVNPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 21) { pObjeto = new wsVNPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 22) { pObjeto = new wsVNPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 24) { pObjeto = new wsVNPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 32) { pObjeto = new wsVNPCancelamento.NfeCancelamento(); }

                else if (this.vUF == 42) { pObjeto = new wsVRPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 17) { pObjeto = new wsVRPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 28) { pObjeto = new wsVRPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 33) { pObjeto = new wsVRPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 12) { pObjeto = new wsVRPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 27) { pObjeto = new wsVRPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 16) { pObjeto = new wsVRPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 25) { pObjeto = new wsVRPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 14) { pObjeto = new wsVRPCancelamento.NfeCancelamento(); }
            }
            else if (this.vAmbiente == 2)
            {
                if (this.vTpEmis == 3) { pObjeto = new wsSCANHCancelamento.NfeCancelamento(); } //Contingência SCAN Ambiente Nascional
                else if (this.vUF == 51) { pObjeto = new wsMTHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 43) { pObjeto = new wsRSHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 31) { pObjeto = new wsMGHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 35) { pObjeto = new wsSPHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 50) { pObjeto = new wsMSHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 52) { pObjeto = new wsGOHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 41) { pObjeto = new wsPRHCancelamento.NfeCancelamentoService(); }
                else if (this.vUF == 23) { pObjeto = new wsCEHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 29) { pObjeto = new wsBAHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 53) { pObjeto = new wsDFHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 26) { pObjeto = new wsPEHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 11) { pObjeto = new wsROHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 13) { pObjeto = new wsAMHCancelamento.NfeCancelamento(); }

                else if (this.vUF == 15) { pObjeto = new wsVNHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 21) { pObjeto = new wsVNHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 22) { pObjeto = new wsVNHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 24) { pObjeto = new wsVNHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 32) { pObjeto = new wsVNHCancelamento.NfeCancelamento(); }

                else if (this.vUF == 42) { pObjeto = new wsVRHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 17) { pObjeto = new wsVRHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 28) { pObjeto = new wsVRHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 33) { pObjeto = new wsVRHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 12) { pObjeto = new wsVRHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 27) { pObjeto = new wsVRHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 16) { pObjeto = new wsVRHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 25) { pObjeto = new wsVRHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 14) { pObjeto = new wsVRHCancelamento.NfeCancelamento(); }
            }
        }
        #endregion

        #region DefObjInutilizacao()
        /// <summary>
        /// Definir o Objeto que vai ser utilizado para inutizar núemros de notas fiscais de acordo com o Estado e 
        /// Ambiente informado para a classe
        /// </summary>
        /// <param name="pObjeto">Objeto que é para receber a instância do serviço</param>
        /// <example>
        /// object oServico = null;
        /// this.DefObjInutilizacao(ref oServico);                       
        /// Type tipoServico = oServico.GetType();
        ///
        /// object oClientCertificates;
        /// Type tipoClientCertificates;
        /// 
        /// oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, oServico, new Object[] { });
        /// tipoClientCertificates = oClientCertificates.GetType();
        /// tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
        ///          
        /// this.vNfeRetorno = string.Empty;
        /// 
        /// this.vNfeRetorno = (string)(tipoServico.InvokeMember("nfeInutilizacaoNF", System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>01/07/2008</date>
        private void DefObjInutilizacao(ref object pObjeto)
        {
            if (this.vAmbiente == 1)
            {
                if (this.vTpEmis == 3) { pObjeto = new wsSCANPInutilizacao.NfeInutilizacao(); } //Contingência SCAN Ambiente Nascional
                else if (this.vUF == 51) { pObjeto = new wsMTPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 43) { pObjeto = new wsRSPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 31) { pObjeto = new wsMGPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 35) { pObjeto = new wsSPPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 50) { pObjeto = new wsMSPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 52) { pObjeto = new wsGOPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 41) { pObjeto = new wsPRPInutilizacao.NfeInutilizacaoService(); }
                else if (this.vUF == 23) { pObjeto = new wsCEPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 29) { pObjeto = new wsBAPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 53) { pObjeto = new wsDFPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 26) { pObjeto = new wsPEPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 11) { pObjeto = new wsROPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 13) { pObjeto = new wsAMPInutilizacao.NfeInutilizacao(); }

                else if (this.vUF == 15) { pObjeto = new wsVNPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 21) { pObjeto = new wsVNPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 22) { pObjeto = new wsVNPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 24) { pObjeto = new wsVNPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 32) { pObjeto = new wsVNPInutilizacao.NfeInutilizacao(); }

                else if (this.vUF == 42) { pObjeto = new wsVRPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 17) { pObjeto = new wsVRPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 28) { pObjeto = new wsVRPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 33) { pObjeto = new wsVRPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 12) { pObjeto = new wsVRPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 27) { pObjeto = new wsVRPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 16) { pObjeto = new wsVRPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 25) { pObjeto = new wsVRPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 14) { pObjeto = new wsVRPInutilizacao.NfeInutilizacao(); }
            }
            else if (this.vAmbiente == 2)
            {
                if (this.vTpEmis == 3) { pObjeto = new wsSCANHInutilizacao.NfeInutilizacao(); } //Contingência SCAN Ambiente Nascional
                else if (this.vUF == 51) { pObjeto = new wsMTHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 43) { pObjeto = new wsRSHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 31) { pObjeto = new wsMGHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 35) { pObjeto = new wsSPHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 50) { pObjeto = new wsMSHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 52) { pObjeto = new wsGOHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 41) { pObjeto = new wsPRHInutilizacao.NfeInutilizacaoService(); }
                else if (this.vUF == 23) { pObjeto = new wsCEHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 29) { pObjeto = new wsBAHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 53) { pObjeto = new wsDFHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 26) { pObjeto = new wsPEHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 11) { pObjeto = new wsROHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 13) { pObjeto = new wsAMHInutilizacao.NfeInutilizacao(); }

                else if (this.vUF == 15) { pObjeto = new wsVNHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 21) { pObjeto = new wsVNHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 22) { pObjeto = new wsVNHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 24) { pObjeto = new wsVNHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 32) { pObjeto = new wsVNHInutilizacao.NfeInutilizacao(); }

                else if (this.vUF == 42) { pObjeto = new wsVRHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 17) { pObjeto = new wsVRHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 28) { pObjeto = new wsVRHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 33) { pObjeto = new wsVRHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 12) { pObjeto = new wsVRHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 27) { pObjeto = new wsVRHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 16) { pObjeto = new wsVRHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 25) { pObjeto = new wsVRHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 14) { pObjeto = new wsVRHInutilizacao.NfeInutilizacao(); }
            }
        }
        #endregion

        #region DefObjConsultaCadastro()
        /// <summary>
        /// Defini o Objeto que vai ser utilizado para consultar o cadastro do contribuinte de acordo com o Estado e Ambiente informado para a classe
        /// </summary>
        /// <param name="pObjeto">Variável de objeto que é para receber a instância do serviço</param>
        /// <example>
        /// object oServico = null;
        /// this.DefObjConsultaCadastro(ref oServico);
        /// if (this.InvocarObjeto("1.01", oServico, "consultaCadastro", "-cons-cad", "-ret-cons-cad") == true)
        /// {
        ///    //Deletar o arquivo de solicitação do serviço
        ///    this.MoveDeleteArq(this.vXmlNfeDadosMsg, "D");
        /// }
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/01/2009</date>
        private void DefObjConsultaCadastro(ref object pObjeto)
        {
            if (this.vAmbiente == 1)
            {
                if (this.vUF == 29) { pObjeto = new wsBAPConsultaCadastro.CadConsultaCadastro(); }
                else if (this.vUF == 23) { pObjeto = new wsCEPConsultaCadastro.CadConsultaCadastro(); }
                else if (this.vUF == 53) { pObjeto = new wsDFPConsultaCadastro.CadConsultaCadastro(); }
                else if (this.vUF == 52) { pObjeto = new wsGOPConsultaCadastro.CadConsultaCadastro(); }
                else if (this.vUF == 26) { pObjeto = new wsPEPConsultaCadastro.CadConsultaCadastro(); }
                else if (this.vUF == 11) { pObjeto = new wsROPConsultaCadastro.CadConsultaCadastro(); }
                else if (this.vUF == 43) { pObjeto = new wsRSPConsultaCadastro.CadConsultaCadastro(); }
                else if (this.vUF == 35) { pObjeto = new wsSPPConsultaCadastro.CadConsultaCadastro(); }
            }
            else if (this.vAmbiente == 2)
            {
                if (this.vUF == 23) { pObjeto = new wsCEHConsultaCadastro.CadConsultaCadastro(); }
                else if (this.vUF == 53) { pObjeto = new wsDFHConsultaCadastro.CadConsultaCadastro(); }
                else if (this.vUF == 52) { pObjeto = new wsGOHConsultaCadastro.CadConsultaCadastro(); }
                else if (this.vUF == 26) { pObjeto = new wsPEHConsultaCadastro.CadConsultaCadastro(); }
                else if (this.vUF == 11) { pObjeto = new wsROHConsultaCadastro.CadConsultaCadastro(); }
                else if (this.vUF == 35) { pObjeto = new wsSPHConsultaCadastro.CadConsultaCadastro(); }
            }
        }
        #endregion

        #endregion

        #region Métodos auxiliares

        #region InvocarObjeto() - Sobrecarga()
        /// <summary>
        /// Invoca o método do objeto passado por parâmetro para fazer acesso aos WebServices do SEFAZ e não grava o XML retornado
        /// </summary>
        /// <param name="cVersaoDados">Versão dos dados que será enviado para o WebService</param>
        /// <param name="oServico">Nome do Objeto do WebService que vai ser acessado</param>
        /// <param name="cMetodo">Nome do método que vai ser utilizado para acessar o WebService</param>
        /// <returns>
        /// Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
        /// XML com o retorno que foi dado do serviço do WebService.
        /// Se der algum erro ele grava um arquivo txt com o erro em questão.
        /// </returns>
        /// <example>
        /// //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
        /// object oServico = null;
        /// this.DefObjCancelamento(ref oServico);
        /// this.InvocarObjeto("1.07", oServico, "nfeCancelamentoNF");
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>01/07/2008</date>
        private bool InvocarObjeto(string cVersaoDados, object oServico, string cMetodo)
        {
            return InvocarObjeto(cVersaoDados, oServico, cMetodo, string.Empty, string.Empty);
        }
        #endregion

        #region InvocarObjeto()
        /// <summary>
        /// Invoca o método do objeto passado por parâmetro para fazer acesso aos WebServices do SEFAZ e Grava o XML retornado
        /// </summary>
        /// <param name="cVersaoDados">Versão dos dados que será enviado para o WebService</param>
        /// <param name="oServico">Nome do Objeto do WebService que vai ser acessado</param>
        /// <param name="cMetodo">Nome do método que vai ser utilizado para acessar o WebService</param>
        /// <param name="cFinalArqEnvio">string do final do arquivo a ser enviado. Sem a extensão ".xml"</param>
        /// <param name="cFinalArqRetorno">string do final do arquivo a ser gravado com o conteúdo do retorno. Sem a extensão ".xml"</param>
        /// <returns>
        /// Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
        /// XML com o retorno que foi dado do serviço do WebService.
        /// Se der algum erro ele grava um arquivo txt com o erro em questão.
        /// </returns>
        /// <example>
        /// //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
        /// object oServico = null;
        /// this.DefObjCancelamento(ref oServico);
        /// this.InvocarObjeto("1.07", oServico, "nfeCancelamentoNF", "-ped-can", "-can");
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>01/07/2008</date>
        private bool InvocarObjeto(string cVersaoDados, object oServico, string cMetodo, string cFinalArqEnvio, string cFinalArqRetorno)
        {
            bool lRetorna = false;

            // Validar o Arquivo XML
            string cResultadoValidacao = this.ValidarArqXML();
            if (cResultadoValidacao != "")
            {
                //Registrar o erro da validação para o sistema ERP
                this.GravarArqErroServico(cFinalArqEnvio + ".xml", cFinalArqRetorno + ".err", cResultadoValidacao);
                lRetorna = false;
                return lRetorna;
            }

            // Passo 1: Declara variável (tipo string) com o conteúdo do Cabecalho da mensagem a ser enviada para o webservice
            string vNFeCabecMsg = this.GerarXMLCabecMsg(cVersaoDados);

            // Passo 2: Montar o XML de Lote de envio de Notas fiscais
            string vNFeDadosMsg = this.XmlToString(this.vXmlNfeDadosMsg);

            // Passo 3: Passar para o Objeto qual vai ser o certificado digital que ele deve utilizar             
            this.RelacionarCertObj(oServico);

            // Passo 4: Limpa a variável de retorno
            this.vStrXmlRetorno = string.Empty;

            // Definir o tipo de serviço
            Type tipoServico = oServico.GetType();

            //Vou fazer 3 tentativas de envio, se na terceira falhar eu gravo o erro de Retorno para o ERP
            for (int i = 1; i <= 5; i++)
            {
                try
                {
                    //Deu erro na primeira tentativa, sendo assim, vou aumentar o timeout para ver se não resolve a questão na segunda e terceira tentativa
                    if (i == 2)
                    {
                        tipoServico.InvokeMember("Timeout", System.Reflection.BindingFlags.SetProperty, null, oServico, new object[] { 300000 });
                    }

                    //Invocar o membro
                    this.vStrXmlRetorno = (string)(tipoServico.InvokeMember(cMetodo, System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));

                    // Passo 6 e 7: Registra o retorno de acordo com o status obtido e Exclui o XML de solicitaÃ§Ã£o do serviÃ§o
                    if (cFinalArqEnvio != string.Empty && cFinalArqRetorno != string.Empty)
                    {
                        this.GravarXmlRetorno(cFinalArqEnvio + ".xml", cFinalArqRetorno + ".xml");
                    }

                    lRetorna = true;
                }

                catch (Exception ex)
                {
                    // Passo alternativo: Registra o retorno no sistema interno, de acordo com a exceção
                    if (i == 5)
                    {
                        this.GravarArqErroServico(cFinalArqEnvio + ".xml", cFinalArqRetorno + ".err", ex.ToString());
                        lRetorna = false;
                    }
                }

                if (lRetorna == true)
                {
                    break;
                }
            }

            return lRetorna;
        }
        #endregion

        #region GerarXMLCabecMsg()
        /// <summary>
        /// Gera uma string com o XML do cabeçalho dos dados a serem enviados para os serviços da NFe
        /// </summary>
        /// <param name="pVersaoDados">
        /// Versão do arquivo XML que será enviado para os WebServices. Esta versão varia de serviço para
        /// serviço e deve ser consultada no manual de integração da NFE
        /// </param>
        /// <returns>
        /// Retorna uma string com o XML do cabeçalho dos dados a serem enviados para os serviços da NFe
        /// </returns>
        /// <example>
        /// vCabecMSG = GerarXMLCabecMsg("1.07");
        /// MessageBox.Show( vCabecMSG );
        /// 
        /// //O conteúdo que será demonstrado no MessageBox é:
        /// //
        /// //  <?xml version="1.0" encoding="UTF-8" ?>
        /// //  <cabecMsg xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.02">
        /// //     <versaoDados>1.07</versaoDados>
        /// //  </cabecMsg>
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>04/06/2008</date>
        private string GerarXMLCabecMsg(string pVersaoDados)
        {
            string vCabecMsg = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><cabecMsg xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"1.02\"><versaoDados>" + pVersaoDados + "</versaoDados></cabecMsg>";

            return vCabecMsg;
        }
        #endregion

        #region CriarArqXMLStatusServico()
        /// <summary>
        /// Criar um arquivo XML com a estrutura necessária para consultar o status do serviço
        /// </summary>
        /// <returns>Retorna o caminho e nome do arquivo criado</returns>
        /// <example>
        /// string vPastaArq = this.CriaArqXMLStatusServico();
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2008</date>
        private string CriarArqXMLStatusServico()
        {
            string vDadosMsg = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><consStatServ xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"1.07\" xmlns=\"http://www.portalfiscal.inf.br/nfe\"><tpAmb>" + this.vAmbiente.ToString() + "</tpAmb><cUF>" + this.vUF.ToString() + "</cUF><xServ>STATUS</xServ></consStatServ>";

            string _arquivo_saida = this.vPastaXMLEnvio + "\\" + DateTime.Now.ToString("yyyyMMddThhmmss") + "-ped-sta.xml";

            StreamWriter SW = File.CreateText(_arquivo_saida);
            SW.Write(vDadosMsg);
            SW.Close();

            return _arquivo_saida;
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
        public string VerStatusServico()
        {
            string vStatus = "Ocorreu uma falha ao tentar obter a situação do serviço. Aguarde um momento e tente novamente.";

            //Criar XML para obter o status do serviço
            this.vXmlNfeDadosMsg = this.CriarArqXMLStatusServico();

            this.vArqXMLRetorno = this.vPastaXMLRetorno + "\\" +
                      this.ExtrairNomeArq(this.vXmlNfeDadosMsg, "-ped-sta.xml") +
                      "-sta.xml";

            this.vArqERRRetorno = this.vPastaXMLRetorno + "\\" +
                      this.ExtrairNomeArq(this.vXmlNfeDadosMsg, "-ped-sta.xml") +
                      "-sta.err";

            DateTime startTime;
            DateTime stopTime;
            TimeSpan elapsedTime;

            long elapsedMillieconds;
            startTime = DateTime.Now;

            while (true)
            {
                stopTime = DateTime.Now;
                elapsedTime = stopTime.Subtract(startTime);
                elapsedMillieconds = (int)elapsedTime.TotalMilliseconds;

                if (elapsedMillieconds >= 120000) //120.000 ms que corresponde á 120 segundos que corresponde a 2 minutos
                {
                    break;
                }

                if (File.Exists(this.vArqXMLRetorno))
                {
                    try
                    {
                        //Verificar se consegue abrir o arquivo em modo exclusivo, se conseguir ele dá sequencia
                        using (FileStream fs = File.Open(this.vArqXMLRetorno, FileMode.Open, FileAccess.ReadWrite, FileShare.Write))
                        {
                            //Conseguiu abrir o arquivo, significa que está perfeitamente gerado
                            //assim vou iniciar o processo de envio do XML
                            fs.Close();

                            //Ler o status do serviço no XML retornado pelo WebService
                            XmlTextReader oLerXml = new XmlTextReader(this.vArqXMLRetorno);

                            try
                            {
                                while (oLerXml.Read())
                                {
                                    if (oLerXml.NodeType == XmlNodeType.Element)
                                    {
                                        if (oLerXml.Name == "xMotivo")
                                        {
                                            oLerXml.Read();
                                            vStatus = oLerXml.Value;
                                            break;
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                //Se não conseguir ler o arquivo vai somente retornar ao loop para tentar novamente, pois 
                                //pode ser que o arquivo esteja em uso ainda.
                            }

                            oLerXml.Close();

                            //Detetar o arquivo de retorno
                            try
                            {
                                FileInfo oArquivoDel = new FileInfo(this.vArqXMLRetorno);
                                oArquivoDel.Delete();

                                break;
                            }
                            catch
                            {
                                //Somente deixa fazer o loop novamente e tentar deletar
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                else if (File.Exists(this.vArqERRRetorno))
                {
                    //Retornou um arquivo com a extensão .ERR, ou seja, deu um erro,
                    //futuramente tem que retornar esta mensagem para a MessageBox do usuário.

                    //Detetar o arquivo de retorno
                    try
                    {
                        FileInfo oArquivoDel = new FileInfo(this.vArqERRRetorno);
                        oArquivoDel.Delete();
                        break;
                    }
                    catch
                    {
                        //Somente deixa fazer o loop novamente e tentar deletar
                    }
                }

                Thread.Sleep(5000);
            }

            //Retornar o status do serviço
            return vStatus;
        }
        #endregion

        #region XmlToString()
        /// <summary>
        /// Método responsável por ler o conteúdo de um XML e retornar em uma string
        /// </summary>
        /// <param name="parNomeArquivo">Caminho e nome do arquivo XML que é para pegar o conteúdo e retornar na string.</param>
        /// <returns>Retorna uma string com o conteúdo do arquivo XML</returns>
        /// <example>
        /// string ConteudoXML;
        /// ConteudoXML = THIS.XmltoString( @"c:\arquivo.xml" );
        /// MessageBox.Show( ConteudoXML );
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>04/06/2008</date>
        public string XmlToString(string parNomeArquivo)
        {
            string conteudo_xml = string.Empty;

            StreamReader SR = null;
            try
            {
                SR = File.OpenText(parNomeArquivo);
                conteudo_xml = SR.ReadToEnd();
            }
            catch (IOException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                SR.Close();
            }

            return conteudo_xml;
        }
        #endregion

        #region ExtrairNomeArq()
        /// <summary>
        /// Extrai somente o nome do arquivo de uma string; para ser utilizado na situação desejada. Veja os exemplos na documentação do código.
        /// </summary>
        /// <param name="pPastaArq">String contendo o caminho e nome do arquivo que é para ser extraido o nome.</param>
        /// <param name="pFinalArq">String contendo o final do nome do arquivo até onde é para ser extraído.</param>
        /// <returns>Retorna somente o nome do arquivo de acordo com os parâmetros passados - veja exemplos.</returns>
        /// <example>
        /// MessageBox.Show(this.ExtrairNomeArq("C:\\TESTE\\NFE\\ENVIO\\ArqSituacao-ped-sta.xml", "-ped-sta.xml"));
        /// //Será demonstrado no message a string "ArqSituacao"
        /// 
        /// MessageBox.Show(this.ExtrairNomeArq("C:\\TESTE\\NFE\\ENVIO\\ArqSituacao-ped-sta.xml", ".xml"));
        /// //Será demonstrado no message a string "ArqSituacao-ped-sta"
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>19/06/2008</date>
        public string ExtrairNomeArq(string pPastaArq, string pFinalArq)
        {
            //Achar o posição inicial do nome do arquivo
            //procura por pastas, tira elas para ficar somente o nome do arquivo
            Int32 nAchou = 0;
            Int32 nPosI = 0;
            for (Int32 nCont = 0; nCont < pPastaArq.Length; nCont++)
            {
                nAchou = pPastaArq.IndexOf("\\", nCont);
                if (nAchou >= 0)
                {
                    nCont = nAchou;
                    nPosI = nAchou + 1;
                }
                else
                {
                    break;
                }
            }

            //Achar a posição final do nome do arquivo
            Int32 nPosF = pPastaArq.ToUpper().IndexOf(pFinalArq.ToUpper());

            //Extrair o nome do arquivo
            string cRetorna = pPastaArq.Substring(nPosI, nPosF - nPosI);

            return cRetorna;
        }
        #endregion

        #region RelacionarCertObj()
        /// <summary>
        /// Relaciona o certificdo digital a ser utilizado na autenticação com o objeto do serviço.
        /// </summary>
        /// <param name="pObjeto">Objeto que é para ser relacionado o certificado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>19/06/2008</date>
        private void RelacionarCertObj(object pObjeto)
        {
            //Detectar o tipo do objeto
            Type tipoServico = pObjeto.GetType();

            //Relacionar o certificado ao objeto
            object oClientCertificates;
            Type tipoClientCertificates;
            oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, pObjeto, new Object[] { });
            tipoClientCertificates = oClientCertificates.GetType();
            tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
        }
        #endregion

        #region GravarXmlRetorno()
        /// <summary>
        /// Grava o XML com os dados do retorno dos webservices e deleta o XML de solicitação do serviço.
        /// </summary>
        /// <param name="pFinalArqEnvio">Final do nome do arquivo de solicitação do serviço.</param>
        /// <param name="pFinalArqRetorno">Final do nome do arquivo que é para ser gravado o retorno.</param>
        /// <example>
        /// // Arquivo de envio: 20080619T19113320-ped-sta.xml
        /// // Arquivo de retorno que vai ser gravado: 20080619T19113320-sta.xml
        /// this.GravarXmlRetorno("-ped-sta.xml", "-sta.xml");
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        private void GravarXmlRetorno(string pFinalArqEnvio, string pFinalArqRetorno)
        {
            //Deletar o arquivo XML da pasta de temporários de XML´s com erros se 
            //o mesmo existir
            this.DeletarArqXMLErro();

            //Gravar o arquivo XML de retorno
            StreamWriter SW;
            this.vArqXMLRetorno = this.vPastaXMLRetorno + "\\" +
                                  this.ExtrairNomeArq(this.vXmlNfeDadosMsg, pFinalArqEnvio) +
                                  pFinalArqRetorno;
            SW = File.CreateText(this.vArqXMLRetorno);
            SW.Write(this.vStrXmlRetorno);
            SW.Close();
        }
        #endregion

        #region GravarArqErroServico()
        /// <summary>
        /// Grava um arquivo texto com um erro ocorrido na invocação dos WebServices ou na execusão de alguma
        /// rotina de validação, etc. Este arquivo é gravado para que o sistema ERP tenha condições de interagir
        /// com o usuário.
        /// </summary>
        /// <param name="pFinalArqEnvio">Final do nome do arquivo de solicitação do serviço</param>
        /// <param name="pFinalArqErro">Final do nome do arquivo que é para ser gravado o erro</param>
        /// <param name="cErro">Texto do erro ocorrido a ser gravado no arquivo</param>
        /// <example>
        /// //Arquivo de envio: 20080619T19113320-ped-sta.xml
        /// //Arquivo de retorno que vai ser gravado: 20080619T19113320-sta.err
        /// this.GravarXmlRetorno("-ped-sta.xml", "-sta.err");
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        private void GravarArqErroServico(string pFinalArqEnvio, string pFinalArqErro, string cErro)
        {
            //Qualquer erro ocorrido o aplicativo vai mover o XML com falha da pasta de envio
            //para a pasta de XML´s com erros. Futuramente ele é excluido quando outro igual
            //for gerado corretamente.
            this.MoveArqErro(this.vXmlNfeDadosMsg);

            //Grava arquivo de ERRO para o ERP
            string cArqErro = this.vPastaXMLRetorno + "\\" +
                              this.ExtrairNomeArq(this.vXmlNfeDadosMsg, pFinalArqEnvio) +
                              pFinalArqErro;

            this.vArqERRRetorno = cArqErro;

            File.WriteAllText(cArqErro, cErro, Encoding.Default);
        }
        #endregion

        #region MoveArqErro()
        /// <summary>
        /// Move arquivos com a extensão informada e que está com erro para uma pasta de xml´s/arquivos com erro configurados no UniNFe.
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo a ser movido para a pasta de XML´s com erro</param>
        /// <param name="ExtensaoArq">Extensão do arquivo que vai ser movido. Ex: .xml</param>
        /// <example>this.MoveArqErro(this.vXmlNfeDadosMsg, ".xml")</example>
        public void MoveArqErro(string cArquivo, string ExtensaoArq)
        {
            if (File.Exists(cArquivo))
            {
                FileInfo oArquivo = new FileInfo(cArquivo);

                if (Directory.Exists(this.vPastaXMLErro) == true)
                {
                    //Mover o arquivo da nota fiscal para a pasta do XML com erro
                    string vNomeArquivo = this.vPastaXMLErro + "\\" + ExtrairNomeArq(cArquivo, ExtensaoArq) + ExtensaoArq;
                    if (File.Exists(vNomeArquivo))
                    {
                        FileInfo oArqDestino = new FileInfo(vNomeArquivo);
                        oArqDestino.Delete();
                    }

                    oArquivo.MoveTo(vNomeArquivo);
                }
                else
                {
                    oArquivo.Delete();
                }
            }
        }
        #endregion

        #region MoveArqErro
        /// <summary>
        /// Move arquivos XML com erro para uma pasta de xml´s com erro configurados no UniNFe.
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo a ser movido para a pasta de XML´s com erro</param>
        /// <example>this.MoveArqErro(this.vXmlNfeDadosMsg)</example>
        public void MoveArqErro(string cArquivo)
        {
            this.MoveArqErro(cArquivo, ".xml");
        }
        #endregion

        #region MoveDeleteArq()
        /// <summary>
        /// Move os arquivos XML da pasta de envio para a pasta dos XML enviados Em Processamento, Denegados, Autorizados e de backup (se configurado para isso)
        /// ou somente exclui os arquivos XML da pasta de envio no caso de não ter sido possível envia-lo ao SEFAZ
        /// </summary>
        /// <param name="cArquivo">
        /// Nome do arquivo que é para ser movido ou deletado
        /// </param>
        /// <param name="cOpcao">
        /// M = Move o arquivo da pasta de envio para a pasta de XML´s enviados
        /// D = Deleta o arquivo da pasta de envio    
        /// </param>
        /// <param name="Pasta">Para qual pasta de enviados é para ser movido</param>
        /// <remarks>
        /// Normalmente os arquivos que são movidos para a pasta de enviados
        /// são os de nota fiscal, cancelamento e inutilização, pois são 
        /// documentos assinados digitalmente e necessários para comprovar
        /// algo futuramente. Os demais são deletados.
        /// </remarks>
        /// <example>
        /// //Mover o arquivo da pasta c:\ para a pasta de enviados configurado na
        /// //tela de configurações do uninfe.
        /// this.MoveDeleteArq( "c:\teste.xml", "M", PastaEnviados.EmProcessamento ) 
        /// 
        /// //Deletar o arquivo
        /// this.MoveDeleteArq( "c:\teste.xml", "D" )
        /// </example>
        /// <by>
        /// Wandrey Mundin Ferreira
        /// </by>
        /// <date>
        /// 16/07/2008
        /// </date>
        private void MoveDeleteArq(string cArquivo, string cOpcao, PastaEnviados Pasta)
        {
            //TODO: Criar vários try/catch neste método para evitar erros

            //Definir o arquivo que vai ser deletado ou movido para outra pasta
            FileInfo oArquivo = new FileInfo(cArquivo);

            if (cOpcao == "M") //Mover o arquivo para outra pasta 
            {
                //Criar a pasta EmProcessamento
                if (!Directory.Exists(this.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento.ToString()))
                {
                    System.IO.Directory.CreateDirectory(this.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento.ToString());
                }

                //Criar a Pasta Autorizado
                if (!Directory.Exists(this.vPastaXMLEnviado + "\\" + PastaEnviados.Autorizados.ToString()))
                {
                    System.IO.Directory.CreateDirectory(this.vPastaXMLEnviado + "\\" + PastaEnviados.Autorizados.ToString());
                }

                //Criar a Pasta Denegado
                if (!Directory.Exists(this.vPastaXMLEnviado + "\\" + PastaEnviados.Denegados.ToString()))
                {
                    System.IO.Directory.CreateDirectory(this.vPastaXMLEnviado + "\\" + PastaEnviados.Denegados.ToString());
                }

                //Criar Pasta do Mês para gravar arquivos enviados autorizados ou denegados
                string strNomePastaEnviado = string.Empty;
                string strDestinoArquivo = string.Empty;
                switch (Pasta)
                {
                    case PastaEnviados.EmProcessamento:
                        strNomePastaEnviado = this.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento.ToString();
                        strDestinoArquivo = strNomePastaEnviado + "\\" + ExtrairNomeArq(cArquivo, ".xml") + ".xml";
                        break;

                    case PastaEnviados.Autorizados:
                        strNomePastaEnviado = this.vPastaXMLEnviado + "\\" + PastaEnviados.Autorizados.ToString() + "\\" + this.dtDataToPastaXml.ToString("yyyyMM");
                        strDestinoArquivo = strNomePastaEnviado + "\\" + ExtrairNomeArq(cArquivo, ".xml") + ".xml";
                        goto default;

                    case PastaEnviados.Denegados:
                        strNomePastaEnviado = this.vPastaXMLEnviado + "\\" + PastaEnviados.Denegados.ToString() + "\\" + this.dtDataToPastaXml.ToString("yyyyMM");
                        strDestinoArquivo = strNomePastaEnviado + "\\" + ExtrairNomeArq(cArquivo, "-nfe.xml") + "-den.xml";
                        goto default;

                    default:
                        if (!Directory.Exists(strNomePastaEnviado))
                        {
                            System.IO.Directory.CreateDirectory(strNomePastaEnviado);
                        }
                        break;
                }

                //Se conseguiu criar a pasta ele move o arquivo, caso contrário
                if (Directory.Exists(strNomePastaEnviado) == true)
                {
                    //Mover o arquivo da nota fiscal para a pasta dos enviados
                    if (File.Exists(strDestinoArquivo))
                    {
                        FileInfo oArqDestino = new FileInfo(strDestinoArquivo);
                        oArqDestino.Delete();
                    }
                    oArquivo.MoveTo(strDestinoArquivo);

                    if (Pasta == PastaEnviados.Autorizados || Pasta == PastaEnviados.Denegados)
                    {
                        //Fazer um backup do XML que foi copiado para a pasta de enviados
                        //para uma outra pasta para termos uma maior segurança no arquivamento
                        //Normalmente esta pasta é em um outro computador ou HD
                        if (this.cPastaBackup.Trim() != "")
                        {
                            //Criar Pasta do Mês para gravar arquivos enviados
                            string strNomePastaBackup = string.Empty;
                            switch (Pasta)
                            {
                                case PastaEnviados.Autorizados:
                                    strNomePastaBackup = this.cPastaBackup + "\\" + PastaEnviados.Autorizados + "\\" + this.dtDataToPastaXml.ToString("yyyyMM");
                                    goto default;

                                case PastaEnviados.Denegados:
                                    strNomePastaBackup = this.cPastaBackup + "\\" + PastaEnviados.Denegados + "\\" + this.dtDataToPastaXml.ToString("yyyyMM");
                                    goto default;

                                default:
                                    if (Directory.Exists(strNomePastaBackup) == false)
                                    {
                                        System.IO.Directory.CreateDirectory(strNomePastaBackup);
                                    }
                                    break;
                            }

                            //Se conseguiu criar a pasta ele move o arquivo, caso contrário
                            if (Directory.Exists(strNomePastaBackup) == true)
                            {
                                //Mover o arquivo da nota fiscal para a pasta de backup
                                string strNomeArquivoBkp = strNomePastaBackup + "\\" + ExtrairNomeArq(cArquivo, ".xml") + ".xml";
                                if (File.Exists(strNomeArquivoBkp))
                                {
                                    FileInfo oArqDestinoBkp = new FileInfo(strNomeArquivoBkp);
                                    oArqDestinoBkp.Delete();
                                }
                                FileInfo oArquivoBkp = new FileInfo(strDestinoArquivo);

                                oArquivoBkp.CopyTo(strNomeArquivoBkp, true);
                            }
                            else
                            {
                                //TODO: Tenho que tratar este Erro e retornar algo para o ERP urgente, pois pode falhar
                            }
                        }
                    }
                }
                else
                {
                    //TODO: Tenho que tratar este Erro e retornar algo para o ERP urgente, pois pode falhar                
                }
            }
            else if (cOpcao == "D") //Deletar o arquivo
            {
                if (File.Exists(cArquivo))
                {
                    oArquivo.Delete();
                }
            }
        }
        #endregion

        #region MoveDeleteArq() - Sobrecarga
        /// <summary>
        /// Move os arquivos XML da pasta de envio para a pasta dos XML enviados em processamento
        /// ou somente exclui os arquivos XML da pasta de envio no caso de não ter sido possível envia-lo ao SEFAZ
        /// </summary>
        /// <param name="cArquivo">
        /// Nome do arquivo que é para ser movido ou deletado
        /// </param>
        /// <param name="cOpcao">
        /// M = Move o arquivo da pasta de envio para a pasta de XML´s enviados
        /// D = Deleta o arquivo da pasta de envio    
        /// </param>
        /// <remarks>
        /// Normalmente os arquivos que são movidos para a pasta de enviados
        /// são os de nota fiscal, cancelamento e inutilização, pois são 
        /// documentos assinados digitalmente e necessários para comprovar
        /// algo futuramente. Os demais são deletados.
        /// </remarks>
        /// <example>
        /// //Mover o arquivo da pasta c:\ para a pasta de enviados configurado na
        /// //tela de configurações do uninfe.
        /// this.MoveDeleteArq( "c:\teste.xml", "M" ) 
        /// 
        /// //Deletar o arquivo
        /// this.MoveDeleteArq( "c:\teste.xml", "D" )
        /// </example>
        /// <by>
        /// Wandrey Mundin Ferreira
        /// </by>
        /// <date>
        /// 20/04/2009
        /// </date>
        private void MoveDeleteArq(string cArquivo, string cOpcao)
        {
            this.MoveDeleteArq(cArquivo, cOpcao, PastaEnviados.EmProcessamento);
        }
        #endregion

        #region ValidarArqXML()
        /// <summary>
        /// Valida o arquivo XML 
        /// </summary>
        /// <returns>
        /// Se retornar uma string em branco, significa que o XML foi 
        /// validado com sucesso, ou seja, não tem nenhum erro. Se o retorno
        /// tiver algo, algum erro ocorreu na validação.
        /// </returns>
        /// <example>
        /// string cResultadoValidacao = this.ValidarArqXML();
        /// 
        /// if (cResultadoValidacao == "")
        /// {
        ///     MessageBox.Show( "Arquivo validado com sucesso" );
        /// }
        /// else
        /// {
        ///     MessageBox.Show( cResultadoValidacao );
        /// }
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>31/07/2008</date>         
        private string ValidarArqXML()
        {
            string cRetorna = "";

            // Validar Arquivo XML
            ValidadorXMLClass oValidador = new ValidadorXMLClass();
            oValidador.TipoArquivoXML(this.vXmlNfeDadosMsg);

            if (oValidador.nRetornoTipoArq >= 1 && oValidador.nRetornoTipoArq <= 11)
            {
                oValidador.ValidarXML(this.vXmlNfeDadosMsg, oValidador.cArquivoSchema);
                if (oValidador.Retorno != 0)
                {
                    cRetorna = "XML INCONSISTENTE!\r\n\r\n" + oValidador.RetornoString;
                }
            }
            else
            {
                cRetorna = "XML INCONSISTENTE!\r\n\r\n" + oValidador.cRetornoTipoArq;
            }

            return cRetorna;
        }
        #endregion

        #region DeletarArqXMLErro()
        /// <summary>
        /// Deleta o XML da pata temporária dos arquivos com erro se o mesmo existir.
        /// </summary>
        private void DeletarArqXMLErro()
        {
            string vNomeArquivo = this.vPastaXMLErro + "\\" + ExtrairNomeArq(this.vXmlNfeDadosMsg, ".xml") + ".xml";

            try
            {
                if (File.Exists(vNomeArquivo))
                {
                    FileInfo oArqXMLComErro = new FileInfo(vNomeArquivo);
                    oArqXMLComErro.Delete();
                }
            }
            catch (IOException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region AssinarValidarXML()
        /// <summary>
        /// Assinar e validar o XML da Nota Fiscal Eletrônica e move para a pasta de assinados
        /// </summary>
        /// <param name="bMoverXML">true = Mover XML assinado da pasta de Lote para a subpasta Assinado</param>
        /// <param name="strPasta">Nome da pasta onde está o XML a ser validado e assinado</param>
        /// <returns>true = Conseguiu assinar e validar</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>03/04/2009</date>
        public Boolean AssinarValidarXMLNFe(string strPasta)
        {
            Boolean bRetorna = false;
            Boolean bAssinado = this.Assinado(this.vXmlNfeDadosMsg);
            Boolean bValidadoSchema = false;
            Boolean bValidacaoGeral = false;

            //Criar Pasta dos XML´s a ser enviado em Lote já assinados
            string strPastaLoteAssinado = strPasta + strNomeSubPastaAssinado;

            //Se o arquivo XML já existir na pasta de assinados, vou avisar o ERP que já tem um em andamento
            string strArqDestino = strPastaLoteAssinado + "\\" + ExtrairNomeArq(this.vXmlNfeDadosMsg, ".xml") + ".xml";

            try
            {
                //Fazer uma leitura de algumas tags do XML
                UniLerXMLClass oLerXml = new UniLerXMLClass();
                oLerXml.Nfe(this.vXmlNfeDadosMsg);

                //Inserir NFe no XML de controle do fluxo
                FluxoNfe oFluxoNfe = new FluxoNfe();
                if (!oFluxoNfe.NfeExiste(oLerXml.oDadosNfe.chavenfe))
                {
                    try
                    {
                        //Deletar o arquivo XML da pasta de temporários de XML´s com erros se o mesmo existir             
                        this.DeletarArqXMLErro();

                        //Validações gerais
                        if (this.ValidacoesGeraisXMLNFe(this.vXmlNfeDadosMsg, oLerXml))
                        {
                            bValidacaoGeral = true;
                        }

                        //Assinar o arquivo XML
                        if (bValidacaoGeral && !bAssinado)
                        {
                            UniAssinaturaDigitalClass oAD = new UniAssinaturaDigitalClass();

                            oAD.Assinar(this.vXmlNfeDadosMsg, "infNFe", this.oCertificado);

                            if (oAD.intResultado == 0)
                            {
                                bAssinado = true;
                            }
                        }

                        // Validar o Arquivo XML da NFe com os Schemas se estiver assinado
                        if (bValidacaoGeral && bAssinado)
                        {
                            string cResultadoValidacao = this.ValidarArqXML();
                            if (cResultadoValidacao == "")
                            {
                                bValidadoSchema = true;
                            }
                            else
                            {
                                //Registrar o erro da validação do schema para o sistema ERP
                                throw new Exception(cResultadoValidacao);
                            }
                        }

                        //Mover o arquivo XML da pasta de lote para a pasta de XML´s assinados
                        if (bValidadoSchema)
                        {
                            try
                            {
                                //Se a pasta de assinados não existir, vamos criar
                                if (!Directory.Exists(strPastaLoteAssinado))
                                {
                                    Directory.CreateDirectory(strPastaLoteAssinado);
                                }

                                if (!File.Exists(strArqDestino))
                                {
                                    //Mover o arquivo para a pasta de XML´s assinados
                                    FileInfo oArquivo = new FileInfo(this.vXmlNfeDadosMsg);
                                    oArquivo.MoveTo(strArqDestino);

                                    bRetorna = true;
                                }
                                else
                                {
                                    oFluxoNfe.InserirNfeFluxo(oLerXml.oDadosNfe.chavenfe, this.ExtrairNomeArq(strArqDestino, ".xml") + ".xml");

                                    throw new IOException("Esta nota fiscal já está na pasta de Notas Fiscais assinadas e em processo de envio, desta forma não é possível enviar a mesma novamente.\r\n" +
                                        this.vXmlNfeDadosMsg);
                                }
                            }
                            catch (IOException ex)
                            {
                                throw (ex);
                            }
                            catch (Exception ex)
                            {
                                throw (ex);
                            }
                        }

                        if (bRetorna)
                        {
                            try
                            {
                                oFluxoNfe.InserirNfeFluxo(oLerXml.oDadosNfe.chavenfe, this.ExtrairNomeArq(strArqDestino, ".xml") + ".xml");
                            }
                            catch (Exception ex)
                            {
                                throw (ex);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                }
                else
                {
                    throw new Exception("A nota fiscal abaixo já foi enviada para o SEFAZ e está somente aguardando a consulta do recibo, efetue a consulta para finalizar a transação da NFe.\r\n" +
                        this.vXmlNfeDadosMsg);
                }
            }
            catch (Exception ex)
            {
                this.GravarArqErroServico("-nfe.xml", "-nfe.err", ex.Message);

                //Se já foi movido o XML da Nota Fiscal para a pasta em Processamento, vou ter que 
                //forçar mover para a pasta de XML com erro neste ponto.
                this.MoveArqErro(strArqDestino);
            }

            return bRetorna;
        }
        #endregion

        #region Assinado()
        /// <summary>
        /// Verifica se o XML já está assinado digitalmente ou não
        /// </summary>
        /// <param name="cArquivoXML">Arquivo a ser verificado</param>
        /// <returns>true = Arquivo XML já assinado</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>03/04/2009</date>
        private Boolean Assinado(string cArquivoXML)
        {
            Boolean bAssinado = false;

            //TODO: Tem que criar ainda o código que verifica se já está assinado ou não

            return bAssinado;
        }
        #endregion

        #region ValidacoesGerais()
        /// <summary>
        /// Efetua uma leitura do XML da nota fiscal eletrônica e faz diversas conferências do seu conteúdo e bloqueia se não 
        /// estiver de acordo com as configurações do UNINFE
        /// </summary>
        /// <param name="strArquivoNFe">Arquivo XML da NFe</param>
        /// <returns>true = Validado com sucesso</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>16/04/2009</date>
        public bool ValidacoesGeraisXMLNFe(string strArquivoNFe, UniLerXMLClass oLerXml)
        {
            bool booValido = false;

            try
            {
                //Verificar o tipo de emissão de bate com o configurado, se não bater vai retornar um erro 
                //para o ERP
                if ((vTpEmis == 1 && (oLerXml.oDadosNfe.tpEmis == "1" || oLerXml.oDadosNfe.tpEmis == "2")) ||
                    (vTpEmis == 3 && (oLerXml.oDadosNfe.tpEmis == "3")))
                {
                    booValido = true;
                }
                else if (vTpEmis == 2 && (oLerXml.oDadosNfe.tpEmis == "2"))
                {
                    booValido = false; //Retorno somente falso mas sem exception para não fazer nada. Wandrey 09/06/2009
                }
                else
                {
                    booValido = false;

                    //Registrar o erro referente ao tipo de emissão, está diferente, do que foi 
                    //configurado na tela do UniNFe
                    string cTextoErroTpEmis = "";

                    if (vTpEmis == 1 && oLerXml.oDadosNfe.tpEmis == "3")
                    {
                        cTextoErroTpEmis = "O UniNFe está configurado para enviar a Nota Fiscal ao Ambiente da SEFAZ " +
                                           "(Secretaria Estadual da Fazenda) e o XML está configurado para enviar " +
                                           "para o SCAN do Ambiente Nacional.\r\n\r\n";

                    }
                    else if (vTpEmis == 3 && (oLerXml.oDadosNfe.tpEmis == "1" || oLerXml.oDadosNfe.tpEmis == "2"))
                    {
                        cTextoErroTpEmis = "O UniNFe está configurado para enviar a Nota Fiscal ao SCAN do Ambiente Nacional " +
                                           "e o XML está configurado para enviar para o Ambiente da SEFAZ (Secretaria Estadual da Fazenda)\r\n\r\n";

                    }

                    cTextoErroTpEmis += "O XML não será enviado e será movido para a pasta de XML com erro para análise.";

                    throw new Exception(cTextoErroTpEmis);
                }
            }
            catch (Exception ex)
            {
                booValido = false;

                throw (ex);
            }

            return booValido;
        }
        #endregion

        #region LerRetornoLote()
        /// <summary>
        /// Efetua a leitura do XML de retorno do processamento do lote de notas fiscais e 
        /// atualiza o arquivo de fluxo e envio de notas
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        private void LerRetornoLote()
        {
            UniLerXMLClass oLerXml = new UniLerXMLClass();
            MemoryStream msXml = UniLerXMLClass.StringXmlToStream(this.vStrXmlRetorno);

            FluxoNfe oFluxoNfe = new FluxoNfe();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(msXml);

                XmlNodeList retConsReciNFeList = doc.GetElementsByTagName("retConsReciNFe");

                foreach (XmlNode retConsReciNFeNode in retConsReciNFeList)
                {
                    XmlElement retConsReciNFeElemento = (XmlElement)retConsReciNFeNode;

                    if (retConsReciNFeElemento.GetElementsByTagName("cStat")[0].InnerText == "104") //Lote processado
                    {
                        string nRec = retConsReciNFeElemento.GetElementsByTagName("nRec")[0].InnerText;

                        XmlNodeList protNFeList = retConsReciNFeElemento.GetElementsByTagName("protNFe");

                        foreach (XmlNode protNFeNode in protNFeList)
                        {
                            XmlElement protNFeElemento = (XmlElement)protNFeNode;

                            string strProtNfe = protNFeElemento.OuterXml;

                            XmlNodeList infProtList = protNFeElemento.GetElementsByTagName("infProt");

                            foreach (XmlNode infProtNode in infProtList)
                            {
                                XmlElement infProtElemento = (XmlElement)infProtNode;

                                string strChaveNFe = "NFe" + infProtElemento.GetElementsByTagName("chNFe")[0].InnerText;
                                string strStat = infProtElemento.GetElementsByTagName("cStat")[0].InnerText;
                                string strProt = string.Empty;

                                //Se o strStat for de rejeição a tag nProt não existe, assim sendo tenho que tratar
                                //para evitar um erro. Wandrey 01/06/2009
                                if (infProtElemento.GetElementsByTagName("nProt")[0] != null)
                                {
                                    strProt = infProtElemento.GetElementsByTagName("nProt")[0].InnerText;
                                }

                                //Definir o nome do arquivo da NFe e seu caminho
                                string strNomeArqNfe = oFluxoNfe.LerTag(strChaveNFe, FluxoNfe.ElementoFixo.ArqNFe);
                                string strArquivoNFe = this.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + strNomeArqNfe;

                                //Atualizar a Tag de status da NFe no fluxo para que se ocorrer alguma falha na exclusão eu tenha esta campo para ter uma referencia em futuras consultas
                                oFluxoNfe.AtualizarTag(strChaveNFe, FluxoNfe.ElementoEditavel.cStat, strStat);

                                //Atualizar a tag da data e hora da ultima consulta do recibo
                                oFluxoNfe.AtualizarDPedRec(nRec, DateTime.Now);

                                switch (strStat)
                                {
                                    case "100": //NFe Autorizada
                                        //Juntar o protocolo com a NFE já copiando para a pasta de autorizadas
                                        this.CriarXmlDistNFe(strArquivoNFe, strProtNfe);
                                        string strArquivoNFeProc = this.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + this.ExtrairNomeArq(strNomeArqNfe, "-nfe.xml") + "-procNFe.xml";

                                        //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s autorizados
                                        oLerXml.Nfe(strArquivoNFe);
                                        this.dtDataToPastaXml = oLerXml.oDadosNfe.dEmi;

                                        //Mover a NFE da pasta de NFE em processamento para NFe Autorizada
                                        this.MoveDeleteArq(strArquivoNFe, "M", PastaEnviados.Autorizados);

                                        //Mover a nfePRoc da pasta de NFE em processamento para a NFe Autorizada
                                        this.MoveDeleteArq(strArquivoNFeProc, "M", PastaEnviados.Autorizados);
                                        break;

                                    case "301": //NFe Denegada - Problemas com o emitente
                                        //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s Denegados
                                        oLerXml.Nfe(strArquivoNFe);
                                        this.dtDataToPastaXml = oLerXml.oDadosNfe.dEmi;

                                        //Mover a NFE da pasta de NFE em processamento para NFe Denegadas
                                        this.MoveDeleteArq(strArquivoNFe, "M", PastaEnviados.Denegados);
                                        break;

                                    case "302": //NFe Denegada - Problemas com o destinatário
                                        goto case "301";

                                    default: //NFe foi rejeitada
                                        //Mover o XML da NFE a pasta de XML´s com erro
                                        this.MoveArqErro(strArquivoNFe);
                                        break;
                                }

                                //Deletar a NFE do arquivo de controle de fluxo
                                oFluxoNfe.ExcluirNfeFluxo(strChaveNFe);
                                break;
                            }
                        }
                    }
                    else if (retConsReciNFeElemento.GetElementsByTagName("cStat")[0].InnerText == "105") //Lote em processamento
                    {
                        //Ok vou aguardar o ERP gerar uma nova consulta para encerrar o fluxo da nota
                    }
                    else if (retConsReciNFeElemento.GetElementsByTagName("cStat")[0].InnerText == "106") //Lote não encontrado
                    {
                        //Se o recibo que está sendo consultado não existir no SEFAZ, vamos excluir a nota do fluxo de envio
                        //TODO: 1-Tenho que ver isso ainda como vai ficar
                        //Tenho somente o Recibo, a partir dele tenho que encontrar a chave da NFe ou das NFs e excluir do fluxo
                        //ou criar uma exclusão do fluxo através do recibo.
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region CriarXMLDistNFe()
        /// <summary>
        /// Criar o arquivo XML de distribuição das NFE com o protocolo de autorização anexado
        /// </summary>
        /// <param name="strArqNFe">Nome arquivo XML da NFe</param>
        /// <param name="strProtNfe">String contendo a parte do XML do protocolo a ser anexado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        private void CriarXmlDistNFe(string strArqNFe, string strProtNfe)
        {
            StreamWriter swProc = null;

            try
            {
                //Separar as tag´s da NFe que interessa <NFe> até </NFe>
                XmlDocument doc = new XmlDocument();

                doc.Load(strArqNFe);

                XmlNodeList NFeList = doc.GetElementsByTagName("NFe");
                XmlNode NFeNode = NFeList[0];
                string strNFe = NFeNode.OuterXml;

                //Montar a string contendo o XML -proc-NFe.xml
                string strXmlProcNfe = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                    "<nfeProc xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"1.10\">" +
                    strNFe +
                    strProtNfe +
                    "</nfeProc>";

                //Montar o nome do arquivo -proc-NFe.xml
                string strNomeArqProcNFe = this.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento + "\\" + this.ExtrairNomeArq(strArqNFe, "-nfe.xml") + "-procNFe.xml";

                //Gravar o XML em uma linha só (sem quebrar as tag´s linha a linha) ou dá erro na hora de validar o XML pelos Schemas. Wandreu 13/05/2009
                swProc = File.CreateText(strNomeArqProcNFe);
                swProc.Write(strXmlProcNfe);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (swProc != null)
                {
                    swProc.Close();
                }
            }
        }
        #endregion

        #region GerarXmlPedRec()
        /// <summary>
        /// Gera o XML de pedido de analise do recibo do lote
        /// </summary>
        /// <param name="strRecibo">Número do recibo a ser consultado o lote</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        public void GerarXmlPedRec(string strRecibo)
        {
            string strXml = string.Empty;
            string strNomeArqPedRec = this.vPastaXMLEnvio + "\\" + strRecibo + "-ped-rec.xml";
            if (!File.Exists(strNomeArqPedRec))
            {
                strXml += "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                    "<consReciNFe xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"1.10\" xmlns=\"http://www.portalfiscal.inf.br/nfe\">" +
                    "<tpAmb>" + this.vAmbiente.ToString() + "</tpAmb>" +
                    "<nRec>" + strRecibo + "</nRec>" +
                    "</consReciNFe>";

                //Gravar o XML
                MemoryStream oMemoryStream = UniLerXMLClass.StringXmlToStream(strXml);
                XmlDocument docProc = new XmlDocument();
                docProc.Load(oMemoryStream);
                docProc.Save(strNomeArqPedRec);
            }
        }
        #endregion

        #region LerRetornoCanc()
        /// <summary>
        /// Efetua a leitura do XML de retorno do processamento do cancelamento
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        private void LerRetornoCanc()
        {
            MemoryStream msXml = UniLerXMLClass.StringXmlToStream(this.vStrXmlRetorno);

            XmlDocument doc = new XmlDocument();
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

                        this.CriarXmlDistCanc(this.vXmlNfeDadosMsg, strRetCancNFe);

                        //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                        this.MoveDeleteArq(this.vXmlNfeDadosMsg, "M", PastaEnviados.Autorizados);

                        //Move o arquivo de Distribuição para a pasta de enviados autorizados
                        string strNomeArqProcCancNFe = this.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento + "\\" + this.ExtrairNomeArq(this.vXmlNfeDadosMsg, "-ped-can.xml") + "-procCancNFe.xml";
                        this.MoveDeleteArq(strNomeArqProcCancNFe, "M", PastaEnviados.Autorizados);
                    }
                    else
                    {
                        //Deletar o arquivo de solicitação do serviço da pasta de envio
                        this.MoveDeleteArq(this.vXmlNfeDadosMsg, "D");
                    }
                }
            }
        }
        #endregion

        #region this.CriarXMLDistCanc()
        /// <summary>
        /// Criar o arquivo XML de distribuição dos Cancelamentos com o protocolo de autorização anexado
        /// </summary>
        /// <param name="strArqCanc">Nome arquivo XML de Cancelamento</param>
        /// <param name="strProtNfe">String contendo a parte do XML do protocolo a ser anexado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        private void CriarXmlDistCanc(string strArqCanc, string strRetCancNFe)
        {
            StreamWriter swProc = null;

            try
            {
                //Separar as tag´s da NFe que interessa <NFe> até </NFe>
                XmlDocument doc = new XmlDocument();

                doc.Load(strArqCanc);

                XmlNodeList CancNFeList = doc.GetElementsByTagName("cancNFe");
                XmlNode CancNFeNode = CancNFeList[0];
                string strCancNFe = CancNFeNode.OuterXml;

                //Montar o XML -procCancNFe.xml
                string strXmlProcCancNfe = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                    "<procCancNFe xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"1.07\">" +
                    strCancNFe +
                    strRetCancNFe +
                    "</procCancNFe>";

                //Montar o nome do arquivo -proc-NFe.xml
                string strNomeArqProcCancNFe = this.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento + "\\" + this.ExtrairNomeArq(strArqCanc, "-ped-can.xml") + "-procCancNFe.xml";

                //Gravar o XML em uma linha sÃ³ (sem quebrar as tagÂ´s linha a linha) ou dÃ¡ erro na hora de validar o XML pelos Schemas. Wandreu 13/05/2009
                swProc = File.CreateText(strNomeArqProcCancNFe);
                swProc.Write(strXmlProcCancNfe);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (swProc != null)
                {
                    swProc.Close();
                }
            }
        }
        #endregion

        #region LerRetornoInut()
        /// <summary>
        /// Efetua a leitura do XML de retorno do processamento da Inutilização
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        private void LerRetornoInut()
        {
            MemoryStream msXml = UniLerXMLClass.StringXmlToStream(this.vStrXmlRetorno);

            XmlDocument doc = new XmlDocument();
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

                        this.CriarXmlDistInut(this.vXmlNfeDadosMsg, strRetInutNFe);

                        //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                        this.MoveDeleteArq(this.vXmlNfeDadosMsg, "M", PastaEnviados.Autorizados);

                        //Move o arquivo de Distribuição para a pasta de enviados autorizados
                        string strNomeArqProcInutNFe = this.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento + "\\" + this.ExtrairNomeArq(this.vXmlNfeDadosMsg, "-ped-inu.xml") + "-procInutNFe.xml";
                        this.MoveDeleteArq(strNomeArqProcInutNFe, "M", PastaEnviados.Autorizados);
                    }
                    else
                    {
                        //Deletar o arquivo de solicitação do serviço da pasta de envio
                        this.MoveDeleteArq(this.vXmlNfeDadosMsg, "D");
                    }
                }
            }
        }
        #endregion

        #region this.CriarXMLDistInut()
        /// <summary>
        /// Criar o arquivo XML de distribuição das Inutilizações de Números de NFe´s com o protocolo de autorização anexado
        /// </summary>
        /// <param name="strArqInut">Nome arquivo XML de Inutilização</param>
        /// <param name="strProtNfe">String contendo a parte do XML do protocolo a ser anexado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        private void CriarXmlDistInut(string strArqInut, string strRetInutNFe)
        {
            StreamWriter swProc = null;

            try
            {
                //Separar as tag´s da NFe que interessa <NFe> até </NFe>
                XmlDocument doc = new XmlDocument();

                doc.Load(strArqInut);

                XmlNodeList InutNFeList = doc.GetElementsByTagName("inutNFe");
                XmlNode InutNFeNode = InutNFeList[0];
                string strInutNFe = InutNFeNode.OuterXml;

                //Montar o XML -procCancNFe.xml
                string strXmlProcInutNfe = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                    "<procInutNFe xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"1.07\">" +
                    strInutNFe +
                    strRetInutNFe +
                    "</procInutNFe>";

                //Montar o nome do arquivo -proc-NFe.xml
                string strNomeArqProcInutNFe = this.vPastaXMLEnviado + "\\" + PastaEnviados.EmProcessamento + "\\" + this.ExtrairNomeArq(strArqInut, "-ped-inu.xml") + "-procInutNFe.xml";

                //Gravar o XML em uma linha sÃ³ (sem quebrar as tagÂ´s linha a linha) ou dÃ¡ erro na hora de validar o XML pelos Schemas. Wandreu 13/05/2009
                swProc = File.CreateText(strNomeArqProcInutNFe);
                swProc.Write(strXmlProcInutNfe);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (swProc != null)
                {
                    swProc.Close();
                }
            }

        }
        #endregion

        #region SleepTimer()
        /// <summary>
        /// Efetua uma pausa no processo
        /// </summary>
        /// <param name="intMilisegundos">Quantidade em milisegundos para a pausa</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        private void SleepTimer(int intMilisegundos)
        {
            DateTime startTime;
            DateTime stopTime;
            TimeSpan elapsedTime;

            long elapsedMillieconds;
            startTime = DateTime.Now;

            while (true)
            {
                stopTime = DateTime.Now;
                elapsedTime = stopTime.Subtract(startTime);
                elapsedMillieconds = (int)elapsedTime.TotalMilliseconds;

                if (elapsedMillieconds >= intMilisegundos)
                {
                    break;
                }
            }

        }
        #endregion

        #region ValidarAssinarXML()
        /// <summary>
        /// Efetua a validação de qualquer XML, NFE, Cancelamento, Inutilização, etc..., e retorna se está ok ou não
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>28/05/2009</date>
        public void ValidarAssinarXML()
        {
            Boolean Assinou = true;
            ValidadorXMLClass oValidador = new ValidadorXMLClass();
            oValidador.TipoArquivoXML(this.vXmlNfeDadosMsg);

            //Assinar o XML se tiver tag para assinar
            if (oValidador.TagAssinar != string.Empty)
            {
                UniAssinaturaDigitalClass oAD = new UniAssinaturaDigitalClass();

                try
                {
                    oAD.Assinar(this.vXmlNfeDadosMsg, oValidador.TagAssinar, this.oCertificado);

                    if (oAD.intResultado != 0)
                    {
                        Assinou = false;
                    }
                }
                catch (Exception ex)
                {
                    Assinou = false;
                    this.GravarXMLRetornoValidacao("2", "Ocorreu um erro ao assinar o XML: " + ex.Message);
                    this.MoveArqErro(this.vXmlNfeDadosMsg);
                }
            }


            if (Assinou)
            {
                // Validar o Arquivo XML
                if (oValidador.nRetornoTipoArq >= 1 && oValidador.nRetornoTipoArq <= 11)
                {
                    oValidador.ValidarXML(this.vXmlNfeDadosMsg, oValidador.cArquivoSchema);
                    if (oValidador.Retorno != 0)
                    {
                        this.GravarXMLRetornoValidacao("3", "Ocorreu um erro ao validar o XML: " + oValidador.RetornoString);
                        this.MoveArqErro(this.vXmlNfeDadosMsg);
                    }
                    else
                    {
                        if (!Directory.Exists(this.PastaValidar + "\\Validado"))
                        {
                            Directory.CreateDirectory(this.PastaValidar + "\\Validado");
                        }

                        string ArquivoNovo = this.PastaValidar + "\\Validado\\" + this.ExtrairNomeArq(this.vXmlNfeDadosMsg, ".xml") + ".xml";

                        if (File.Exists(ArquivoNovo))
                        {
                            FileInfo oArqNovo = new FileInfo(ArquivoNovo);
                            oArqNovo.Delete();
                        }

                        FileInfo oArquivo = new FileInfo(this.vXmlNfeDadosMsg);
                        oArquivo.MoveTo(ArquivoNovo);

                        this.GravarXMLRetornoValidacao("1", "XML assinado e validado com sucesso.");
                    }
                }
                else
                {
                    this.GravarXMLRetornoValidacao("4", "Ocorreu um erro ao validar o XML: " + oValidador.cRetornoTipoArq);
                    this.MoveArqErro(this.vXmlNfeDadosMsg);
                }
            }
        }
        #endregion

        #region GravarXMLRetornoValidacao()
        /// <summary>
        /// Na tentativa de somente validar ou assinar o XML se encontrar um erro vai ser retornado um XML para o ERP
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>28/05/2009</date>
        private void GravarXMLRetornoValidacao(string cStat, string xMotivo)
        {
            //Definir o nome do arquivo de retorno
            string ArquivoRetorno = this.ExtrairNomeArq(this.vXmlNfeDadosMsg, ".xml") + "-ret.xml";

            XmlWriterSettings oSettings = new XmlWriterSettings();
            UTF8Encoding c = new UTF8Encoding(false);

            //Para começar, vamos criar um XmlWriterSettings para configurar nosso XML
            oSettings.Encoding = c;
            oSettings.Indent = true;
            oSettings.IndentChars = "";
            oSettings.NewLineOnAttributes = false;
            oSettings.OmitXmlDeclaration = false;

            //Agora vamos criar um XML Writer
            XmlWriter oXmlGravar = XmlWriter.Create(this.vPastaXMLRetorno + "\\" + ArquivoRetorno);

            //Agora vamos gravar os dados
            oXmlGravar.WriteStartDocument();
            oXmlGravar.WriteStartElement("Validacao");
            oXmlGravar.WriteElementString("cStat", cStat);
            oXmlGravar.WriteElementString("xMotivo", xMotivo);
            oXmlGravar.WriteEndElement(); //nfe_configuracoes
            oXmlGravar.WriteEndDocument();
            oXmlGravar.Flush();
            oXmlGravar.Close();
        }
        #endregion

        #endregion

        #region Métodos para gerar o lote da nota fiscal eletrônica

        #region GerarrLoteNfe()
        /// <summary>
        /// Gera o Lote das Notas Fiscais passada por parâmetro na pasta de envio
        /// </summary>
        /// <param name="lstArquivosNFe">Lista dos XML´s de Notas Fiscais a serem gerados os lotes</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        public void GerarLoteNfe(List<string> lstArquivosNFe)
        {
            try
            {
                bool booLiberado = false;
                //Vamos verificar se todos os XML´s estão disponíveis
                for (int i = 0; i < lstArquivosNFe.Count; i++)
                {
                    booLiberado = false;
                    //Verificar se consegue abrir o arquivo em modo exclusivo
                    using (FileStream fs = File.Open(lstArquivosNFe[i], FileMode.Open, FileAccess.ReadWrite, FileShare.Write))
                    {
                        //Fechar o arquivo
                        fs.Close();

                        booLiberado = true;

                        Thread.Sleep(100);
                    }
                }

                if (booLiberado)
                {
                    //Buscar o número do lote a ser utilizado
                    Int32 intNumeroLote = this.ProximoNumeroLote();

                    //Iniciar o Lote de NFe
                    this.IniciarLoteNfe(intNumeroLote);

                    for (int i = 0; i < lstArquivosNFe.Count; i++)
                    {
                        //Inserir o arquivo de XML da NFe na string do lote
                        this.InserirNFeLote(lstArquivosNFe[i]);

                        Thread.Sleep(100);
                    }

                    //Encerrar o Lote
                    this.EncerrarLoteNfe(intNumeroLote);

                    //Gravar o XML de retorno do número do lote para o ERP
                    this.GravarXMLLoteRetERP(intNumeroLote);

                    //Vou atualizar os lotes das NFE´s no fluxo de envio somente depois de encerrado o lote onde eu 
                    //tenho certeza que ele foi gerado e que nenhum erro aconteceu, pois desta forma, se falhar somente na 
                    //atualização eu tenho como fazer o UniNFe se recuperar de um erro. Assim sendo não mude de ponto.
                    UniLerXMLClass oLerXml = new UniLerXMLClass();
                    FluxoNfe oFluxoNfe = new FluxoNfe();
                    for (int i = 0; i < lstArquivosNFe.Count; i++)
                    {
                        //Efetua a leitura do XML, tem que acontecer antes de mover o arquivo
                        oLerXml.Nfe(lstArquivosNFe[i]);

                        //Mover o XML da NFE para a pasta de enviados em processamento
                        this.MoveDeleteArq(lstArquivosNFe[i], "M", PastaEnviados.EmProcessamento);

                        //Atualiza o arquivo de controle de fluxo
                        oFluxoNfe.AtualizarTag(oLerXml.oDadosNfe.chavenfe, FluxoNfe.ElementoEditavel.idLote, intNumeroLote.ToString("000000000000000"));

                        Thread.Sleep(100);
                    }
                }
            }
            catch (IOException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region GerarLoteNfe() - Sobrecarga
        /// <summary>
        /// Gera lote da nota fiscal eletrônica com somente uma nota fiscal
        /// </summary>
        /// <param name="strArquivoNfe">Nome do arquivo XML da Nota Fiscal</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        public void GerarLoteNfe(string strArquivoNfe)
        {
            List<string> lstArquivo = new List<string>();

            lstArquivo.Add(strArquivoNfe);

            try
            {
                this.GerarLoteNfe(lstArquivo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region IniciarLoteNfe()
        /// <summary>
        /// Inicia a string do XML do Lote de notas fiscais
        /// </summary>
        /// <param name="intNumeroLote">Número do lote que será enviado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        private void IniciarLoteNfe(Int32 intNumeroLote)
        {
            string cVersaoDados = "1.10";

            strXMLLoteNfe = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            strXMLLoteNfe += "<enviNFe xmlns=\"http://www.portalfiscal.inf.br/nfe\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" versao=\"" + cVersaoDados + "\">";
            strXMLLoteNfe += "<idLote>" + intNumeroLote.ToString("000000000000000") + "</idLote>";
        }

        #endregion

        #region InserirNFeLote()
        /// <summary>
        /// Insere o XML de Nota Fiscal passado por parâmetro na string do XML de Lote de NFe
        /// </summary>
        /// <param name="strArquivoNfe">Nome do arquivo XML de nota fiscal eletrônica a ser inserido no lote</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        private void InserirNFeLote(string strArquivoNfe)
        {
            try
            {
                string vNfeDadosMsg = this.XmlToString(strArquivoNfe);

                //Separar somente o conteúdo a partir da tag <NFe> até </NFe>
                Int32 nPosI = vNfeDadosMsg.IndexOf("<NFe");
                Int32 nPosF = vNfeDadosMsg.Length - nPosI;
                strXMLLoteNfe += vNfeDadosMsg.Substring(nPosI, nPosF);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region EncerrarLoteNfe()
        /// <summary>
        /// Encerra a string do XML de lote de notas fiscais eletrônicas e grava o XML fisicamente no HD na pasta de envio
        /// </summary>
        /// <param name="intNumeroLote">Número do lote que será enviado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        private void EncerrarLoteNfe(Int32 intNumeroLote)
        {
            strXMLLoteNfe += "</enviNFe>";

            //Gravar o XML do lote das notas fiscais
            string vNomeArqLoteNfe = this.vPastaXMLEnvio + "\\" +
                                     intNumeroLote.ToString("000000000000000") +
                                     "-env-lot.xml";

            StreamWriter SW_2 = null;

            try
            {
                SW_2 = File.CreateText(vNomeArqLoteNfe);
                SW_2.Write(strXMLLoteNfe);
                SW_2.Close();
            }
            catch (IOException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                SW_2.Close();
            }
        }
        #endregion

        #region ProximoNumeroLote()
        /// <summary>
        /// Pega o ultimo número de lote utilizado e acrescenta mais 1 para novo envio
        /// </summary>
        /// <returns>Retorna o um novo número de lote a ser utilizado nos envios das notas fiscais</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        private Int32 ProximoNumeroLote()
        {
            Int32 intNumeroLote = 1;

            //TODO: Estudar uma forma de colocar um try neste ponto e retornar erro para o ERP caso não consiga ler o XML do numero do lote
            if (File.Exists(vArqXmlLote))
            {
                //Carregar os dados do arquivo XML de configurações do UniNfe
                XmlTextReader oLerXml = new XmlTextReader(vArqXmlLote);

                while (oLerXml.Read())
                {
                    if (oLerXml.NodeType == XmlNodeType.Element)
                    {
                        if (oLerXml.Name == "UltimoLoteEnviado")
                        {
                            oLerXml.Read(); intNumeroLote = Convert.ToInt32(oLerXml.Value) + 1;
                            break;
                        }
                    }
                }
                oLerXml.Close();
            }

            this.SalvarNumeroLoteUtilizado(intNumeroLote);

            return intNumeroLote;
        }
        #endregion

        #region SalvarNumeroLoteUtilizado()
        /// <summary>
        /// Salva em XML o número do ultimo lote utilizado para envio
        /// </summary>
        /// <param name="intNumeroLote">Numero do lote a ser salvo</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        private void SalvarNumeroLoteUtilizado(Int32 intNumeroLote)
        {
            //TODO: Estudar uma forma de colocar um try neste ponto para se ocorrer de der algum erro, podermos retornar para o ERP
            XmlWriterSettings oSettings = new XmlWriterSettings();
            UTF8Encoding c = new UTF8Encoding(false);

            oSettings.Encoding = c;
            oSettings.Indent = true;
            oSettings.IndentChars = "";
            oSettings.NewLineOnAttributes = false;
            oSettings.OmitXmlDeclaration = false;

            XmlWriter oXmlGravar = XmlWriter.Create(UniNfeInfClass.PastaExecutavel() + "\\UniNfeLote.xml", oSettings);

            oXmlGravar.WriteStartDocument();
            oXmlGravar.WriteStartElement("DadosLoteNfe");
            oXmlGravar.WriteElementString("UltimoLoteEnviado", intNumeroLote.ToString());
            oXmlGravar.WriteEndElement(); //DadosLoteNfe
            oXmlGravar.WriteEndDocument();
            oXmlGravar.Flush();
            oXmlGravar.Close();
        }
        #endregion

        #region GravarXMLLoteRetERP()
        /// <summary>
        /// Grava um XML com o número de lote utilizado na pasta de retorno para que o ERP possa pegar este número
        /// </summary>
        /// <param name="intNumeroLote">Número do lote a ser gravado no retorno para o ERP</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        private void GravarXMLLoteRetERP(Int32 intNumeroLote)
        {
            //TODO: Estudar uma forma de colocar um try neste ponto para se ocorrer de der algum erro, podermos retornar para o ERP
            XmlWriterSettings oSettings = new XmlWriterSettings();
            UTF8Encoding c = new UTF8Encoding(false);

            oSettings.Encoding = c;
            oSettings.Indent = true;
            oSettings.IndentChars = "";
            oSettings.NewLineOnAttributes = false;
            oSettings.OmitXmlDeclaration = false;
            oSettings.Encoding = Encoding.UTF8;

            string cArqLoteRetorno = this.vPastaXMLRetorno + "\\" +
                         this.ExtrairNomeArq(this.vXmlNfeDadosMsg, "-nfe.xml") +
                         "-num-lot.xml";

            XmlWriter oXmlLoteERP = XmlWriter.Create(cArqLoteRetorno, oSettings);

            oXmlLoteERP.WriteStartDocument();
            oXmlLoteERP.WriteStartElement("DadosLoteNfe");
            oXmlLoteERP.WriteElementString("NumeroLoteGerado", intNumeroLote.ToString());
            oXmlLoteERP.WriteEndElement(); //DadosLoteNfe
            oXmlLoteERP.WriteEndDocument();
            oXmlLoteERP.Flush();
            oXmlLoteERP.Close();
        }
        #endregion

        #endregion
    }
    #endregion
}