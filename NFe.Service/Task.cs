using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using NFe.Components;
using NFe.Settings;
using NFe.Certificado;
using NFe.Exceptions;

namespace NFe.Service
{
    public class Task : TaskAbs
    {
        #region Objetos
        public GerarXML oGerarXML = new GerarXML(new FindEmpresaThread(Thread.CurrentThread).Index);
        #endregion

        #region Propriedades

        /// <summary>
        /// Arquivo XML contendo os dados a serem enviados (Nota Fiscal, Pedido de Status, Cancelamento, etc...)
        /// </summary>
        public override string NomeArquivoXML
        {
            get
            {
                return this.mNomeArquivoXML;
            }
            set
            {
                this.mNomeArquivoXML = value;
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

        #region Cancelamento()
#if notused
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
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            Servico = Servicos.CancelarNFe;

            try
            {
                //Ler o XML para pegar parâmetros de envio
                LerXML oLer = new LerXML();
                oLer.PedCanc(NomeArquivoXML);

                if (this.vXmlNfeDadosMsgEhXML)
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servicos.CancelarNFe, emp, oLer.oDadosPedCanc.cUF, oLer.oDadosPedCanc.tpAmb, oLer.oDadosPedCanc.tpEmis);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oCancelamento = wsProxy.CriarObjeto(NomeClasseWS(Servico, oLer.oDadosPedCanc.cUF));
                    object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS());

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "cUF", oLer.oDadosPedCanc.cUF.ToString());
                    wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLCanc);

                    //Criar objeto da classe de assinatura digita
                    AssinaturaDigital oAD = new AssinaturaDigital();

                    //Assinar o XML
                    oAD.Assinar(NomeArquivoXML, emp, Convert.ToInt32(oLer.oDadosPedCanc.cUF));

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oCancelamento, NomeMetodoWS(Servico, oLer.oDadosPedCanc.cUF), oCabecMsg, this, "-ped-can", "-can");

                    //Ler o retorno do webservice
                    LerRetornoCanc(NomeArquivoXML);
                }
                else
                {
                    //Gerar o XML de solicitação de cancelamento de uma NFe a partir do TXT Gerado pelo ERP
                    oGerarXML.Cancelamento(Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml",
                        oLer.oDadosPedCanc.tpAmb,
                        oLer.oDadosPedCanc.tpEmis,
                        oLer.oDadosPedCanc.chNFe,
                        oLer.oDadosPedCanc.nProt,
                        oLer.oDadosPedCanc.xJust);
                }
            }
            catch (Exception ex)
            {
                string ExtRet = string.Empty;

                if (this.vXmlNfeDadosMsgEhXML) //Se for XML
                    ExtRet = Propriedade.ExtEnvio.PedCan_XML;
                else //Se for TXT
                    ExtRet = Propriedade.ExtEnvio.PedCan_TXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    GravarArqErroServico(NomeArquivoXML, ExtRet, Propriedade.ExtRetorno.Can_ERR, ex);
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
                    if (!this.vXmlNfeDadosMsgEhXML) //Se for o TXT para ser transformado em XML, vamos excluir o TXT depois de gerado o XML
                        Functions.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                    //Se falhou algo na hora de deletar o XML de cancelamento de NFe, infelizmente
                    //não posso fazer mais nada, o UniNFe vai tentar mandar o arquivo novamente para o webservice, pois ainda não foi excluido.
                    //Wandrey 09/03/2010
                }
            }
        }
#endif
        #endregion

        #region Consulta()
#if notused
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
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            Servico = Servicos.PedidoConsultaSituacaoNFe;

            try
            {
                //Ler o XML para pegar parâmetros de envio
                LerXML oLer = new LerXML();
                oLer.PedSit(NomeArquivoXML);

                if (vXmlNfeDadosMsgEhXML)
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = null;
                    switch (Propriedade.TipoAplicativo)
                    {
                        case TipoAplicativo.Cte:
                            wsProxy = ConfiguracaoApp.DefinirWS(Servicos.PedidoConsultaSituacaoNFe, emp, oLer.oDadosPedSit.cUF, oLer.oDadosPedSit.tpAmb, oLer.oDadosPedSit.tpEmis);
                            break;
                        case TipoAplicativo.Nfe:
                            wsProxy = ConfiguracaoApp.DefinirWS(Servicos.PedidoConsultaSituacaoNFe, emp, oLer.oDadosPedSit.cUF, oLer.oDadosPedSit.tpAmb, oLer.oDadosPedSit.tpEmis, oLer.oDadosPedSit.versaoNFe);
                            break;
                        default:
                            break;
                    }

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    if (oLer.oDadosPedSit.versaoNFe == 1 && Propriedade.TipoAplicativo == TipoAplicativo.Nfe)
                    {
                        object oConsulta = null;
                        if (oLer.oDadosPedSit.cUF == 41)
                            oConsulta = wsProxy.CriarObjeto("NfeConsultaService");
                        else
                            oConsulta = wsProxy.CriarObjeto("NfeConsulta");

                        //Invocar o método que envia o XML para o SEFAZ
                        oInvocarObj.Invocar(wsProxy, oConsulta, "nfeConsultaNF", this);
                    }
                    else
                    {
                        object oConsulta = wsProxy.CriarObjeto(NomeClasseWS(Servico, oLer.oDadosPedSit.cUF));
                        object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS());

                        //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                        wsProxy.SetProp(oCabecMsg, "cUF", oLer.oDadosPedSit.cUF.ToString());
                        switch (Propriedade.TipoAplicativo)
                        {
                            case TipoAplicativo.Cte:
                                wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLPedSit);
                                break;
                            case TipoAplicativo.Nfe:
                                wsProxy.SetProp(oCabecMsg, "versaoDados", oLer.oDadosPedSit.versaoNFe == 201 ? "2.01" : ConfiguracaoApp.VersaoXMLPedSit);
                                break;
                            default:
                                break;
                        }

                        //Invocar o método que envia o XML para o SEFAZ
                        oInvocarObj.Invocar(wsProxy, oConsulta, NomeMetodoWS(Servico, oLer.oDadosPedSit.cUF), oCabecMsg, this);
                    }

                    //Efetuar a leitura do retorno da situação para ver se foi autorizada ou não
                    //Na versão 1 não posso gerar o -procNfe, ou vou ter que tratar a estrutura do XML de acordo com a versão, a consulta na versão 1 é somente para obter o resultado mesmo.
                    switch (Propriedade.TipoAplicativo)
                    {
                        case TipoAplicativo.Cte:
                            LerRetornoSitCTe(oLer.oDadosPedSit.chNFe);
                            break;
                        case TipoAplicativo.Nfe:
                            if (oLer.oDadosPedSit.versaoNFe != 1)
                                LerRetornoSitNFe(oLer.oDadosPedSit.chNFe);
                            break;
                        default:
                            break;
                    }

                    //Gerar o retorno para o ERP
                    oGerarXML.XmlRetorno(Propriedade.ExtEnvio.PedSit_XML, Propriedade.ExtRetorno.Sit_XML, this.vStrXmlRetorno);
                }
                else
                {
                    oGerarXML.Consulta(Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml",
                                        oLer.oDadosPedSit.tpAmb,
                                        oLer.oDadosPedSit.tpEmis,
                                        oLer.oDadosPedSit.chNFe);
                }
            }
            catch (Exception ex)
            {
                string ExtRet = string.Empty;

                if (this.vXmlNfeDadosMsgEhXML) //Se for XML
                    ExtRet = Propriedade.ExtEnvio.PedSit_XML;
                else //Se for TXT
                    ExtRet = Propriedade.ExtEnvio.PedSit_TXT;

                try
                {
                    GravarArqErroServico(NomeArquivoXML, ExtRet, Propriedade.ExtRetorno.Sit_ERR, ex);
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
                    //Se falhou algo na hora de deletar o XML de pedido da consulta da situação da NFe, infelizmente
                    //não posso fazser mais nada, o UniNFe vai tentar mantar o arquivo novamente para o webservice, pois ainda não foi excluido.
                    //Wandrey 22/03/2010
                }
            }
        }
#endif
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
        /// 
#if notused
        public override void ConsultaCadastro()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            Servico = Servicos.ConsultaCadastroContribuinte;

            try
            {
                //Ler o XML para pegar parâmetros de envio
                LerXML oLer = new LerXML();
                oLer.ConsCad(NomeArquivoXML);

                if (this.vXmlNfeDadosMsgEhXML)  //danasa 12-9-2009
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servicos.ConsultaCadastroContribuinte, emp, oLer.oDadosConsCad.cUF, oLer.oDadosConsCad.tpAmb, Propriedade.TipoEmissao.teNormal);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oConsCad = wsProxy.CriarObjeto(NomeClasseWS(Servico, oLer.oDadosConsCad.cUF));
                    object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS());

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "cUF", oLer.oDadosConsCad.cUF.ToString());
                    wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLConsCad);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oConsCad, NomeMetodoWS(Servico, oLer.oDadosConsCad.cUF), oCabecMsg, this, "-cons-cad", "-ret-cons-cad");
                }
                else
                {
                    //Gerar o XML da consulta cadastro do contribuinte a partir do TXT gerado pelo ERP
                    oGerarXML.ConsultaCadastro(Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml",
                                               oLer.oDadosConsCad.UF,
                                               oLer.oDadosConsCad.CNPJ,
                                               oLer.oDadosConsCad.IE,
                                               oLer.oDadosConsCad.CPF);
                }
            }
            catch (Exception ex)
            {
                string ExtRet = string.Empty;

                if (this.vXmlNfeDadosMsgEhXML) //Se for XML
                    ExtRet = Propriedade.ExtEnvio.ConsCad_XML;
                else //Se for TXT
                    ExtRet = Propriedade.ExtEnvio.ConsCad_TXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    GravarArqErroServico(NomeArquivoXML, ExtRet, Propriedade.ExtRetorno.ConsCad_ERR, ex);
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
                    //Deletar o arquivo de solicitação do serviço
                    Functions.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                    //Se falhou algo na hora de deletar o XML de solicitação do serviço, 
                    //infelizmente não posso fazer mais nada, o UniNFe vai tentar mandar 
                    //o arquivo novamente para o webservice
                    //Wandrey 09/03/2010
                }
            }
        }
#endif
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
      
#if notused
        public override void Inutilizacao()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            Servico = Servicos.InutilizarNumerosNFe;

            try
            {
                //Ler o XML para pegar parâmetros de envio
                LerXML oLer = new LerXML();
                oLer.PedInut(NomeArquivoXML);

                if (this.vXmlNfeDadosMsgEhXML)  //danasa 12-9-2009
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servicos.InutilizarNumerosNFe, emp, oLer.oDadosPedInut.cUF, oLer.oDadosPedInut.tpAmb, oLer.oDadosPedInut.tpEmis);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oInutilizacao = wsProxy.CriarObjeto(NomeClasseWS(Servico, oLer.oDadosPedInut.cUF));
                    object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS());

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "cUF", oLer.oDadosPedInut.cUF.ToString());
                    wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLInut);

                    //Criar objeto da classe de assinatura digita
                    AssinaturaDigital oAD = new AssinaturaDigital();

                    //Assinar o XML
                    oAD.Assinar(NomeArquivoXML, emp, Convert.ToInt32(oLer.oDadosPedInut.cUF));

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oInutilizacao, NomeMetodoWS(Servico, oLer.oDadosPedInut.cUF), oCabecMsg, this, "-ped-inu", "-inu");

                    //Ler o retorno do webservice
                    this.LerRetornoInut();
                }
                else
                {
                    oGerarXML.Inutilizacao(Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml",
                        oLer.oDadosPedInut.tpAmb,
                        oLer.oDadosPedInut.tpEmis,
                        oLer.oDadosPedInut.cUF,
                        oLer.oDadosPedInut.ano,
                        oLer.oDadosPedInut.CNPJ,
                        oLer.oDadosPedInut.mod,
                        oLer.oDadosPedInut.serie,
                        oLer.oDadosPedInut.nNFIni,
                        oLer.oDadosPedInut.nNFFin,
                        oLer.oDadosPedInut.xJust);
                }

            }
            catch (Exception ex)
            {
                string ExtRet = string.Empty;

                if (this.vXmlNfeDadosMsgEhXML) //Se for XML
                    ExtRet = Propriedade.ExtEnvio.PedInu_XML;
                else //Se for TXT
                    ExtRet = Propriedade.ExtEnvio.PedInu_TXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    GravarArqErroServico(NomeArquivoXML, ExtRet, Propriedade.ExtRetorno.Inu_ERR, ex);
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
                    if (!this.vXmlNfeDadosMsgEhXML) //Se for o TXT para ser transformado em XML, vamos excluir o TXT depois de gerado o XML
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
#endif
        #endregion

#if notused
        #region GerarXmlDistCanc()
        /// <summary>
        /// Gera o XML de distribuição do cancelamento dos arquivos -ped-can.xml que estão parados na pasta EmProcessamento
        /// </summary>
        /// <param name="chaveNFe">Chave da nfe/cte que está sendo consultada a situação</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 12/01/2012
        /// </remarks>
        public void GerarXmlDistCanc(string chaveNFe)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            try
            {
                string[] files = Directory.GetFiles(Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString(),
                                 "*" + Propriedade.ExtEnvio.PedCan_XML,
                                 SearchOption.TopDirectoryOnly);

                foreach (string file in files)
                {
                    if (!Functions.FileInUse(file))
                    {
                        XmlDocument xmlCanc = new XmlDocument();
                        xmlCanc.Load(file);

                        string chaveNFeCanc = string.Empty;
                        if (Propriedade.TipoAplicativo == TipoAplicativo.Cte)
                        {
                            chaveNFeCanc = xmlCanc.GetElementsByTagName("chCTe")[0].InnerText;
                        }
                        else
                        {
                            chaveNFeCanc = xmlCanc.GetElementsByTagName("chNFe")[0].InnerText;
                        }

                        if (chaveNFeCanc == chaveNFe)
                        {
                            LerRetornoCanc(file);
                        }
                    }
                }
            }
            catch
            {
            }
        }
        #endregion

        #region LerRetornoCanc()
        /// <summary>
        /// Efetua a leitura do XML de retorno do processamento do cancelamento
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        protected override void LerRetornoCanc(string xmlCanc)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            XmlDocument doc = new XmlDocument();

            try
            {
                MemoryStream msXml = Functions.StringXmlToStream(this.vStrXmlRetorno);
                doc.Load(msXml);

                XmlNodeList retCancNFeList = null;

                switch (Propriedade.TipoAplicativo)
                {
                    case TipoAplicativo.Cte:
                        retCancNFeList = doc.GetElementsByTagName("retCancCTe");
                        break;
                    case TipoAplicativo.Nfe:
                        retCancNFeList = doc.GetElementsByTagName("retCancNFe");
                        break;
                    default:
                        break;
                }

                foreach (XmlNode retCancNFeNode in retCancNFeList)
                {
                    XmlElement retCancNFeElemento = (XmlElement)retCancNFeNode;

                    XmlNodeList infCancList = retCancNFeElemento.GetElementsByTagName("infCanc");

                    foreach (XmlNode infCancNode in infCancList)
                    {
                        XmlElement infCancElemento = (XmlElement)infCancNode;

                        if (infCancElemento.GetElementsByTagName("cStat")[0].InnerText == "101") //Cancelamento Homologado
                        {
                            string retCancNFe = retCancNFeNode.OuterXml;

                            oGerarXML.XmlDistCanc(xmlCanc, retCancNFe);
                            ///
                            /// danasa 9-2009
                            /// pega a data da emissão da nota para mover os XML's para a pasta de origem da NFe
                            /// 
                            string cChaveNFe = string.Empty;
                            switch (Propriedade.TipoAplicativo)
                            {
                                case TipoAplicativo.Cte:
                                    cChaveNFe = infCancElemento.GetElementsByTagName("chCTe")[0].InnerText;
                                    break;
                                case TipoAplicativo.Nfe:
                                    cChaveNFe = infCancElemento.GetElementsByTagName("chNFe")[0].InnerText;
                                    break;
                                default:
                                    break;
                            }

                            //
                            //TODO: Cancelamento - Se for pasta por dia, tem que pegar a data de dentro do XML da NFe
                            DateTime dtEmissaoNFe = new DateTime(Convert.ToInt16("20" + cChaveNFe.Substring(2, 2)), Convert.ToInt16(cChaveNFe.Substring(4, 2)), 1);
                            //DateTime dtEmissaoNFe = DateTime.Now;                            

                            //Move o arquivo de Distribuição para a pasta de enviados autorizados
                            string strNomeArqProcCancNFe = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
                                                            PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                            Functions/*oAux*/.ExtrairNomeArq(NomeArquivoXML, Propriedade.ExtEnvio.PedCan_XML) + Propriedade.ExtRetorno.ProcCancNFe;
                            MoverArquivo(strNomeArqProcCancNFe, PastaEnviados.Autorizados, dtEmissaoNFe);//DateTime.Now);

                            //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                            MoverArquivo(xmlCanc, PastaEnviados.Autorizados, dtEmissaoNFe);//DateTime.Now);
                        }
                        else
                        {
                            //Deletar o arquivo de solicitação do serviço da pasta de envio
                            Functions.DeletarArquivo(NomeArquivoXML);
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
#endif


#if notused
        #region LerRetornoInut()
        /// <summary>
        /// Efetua a leitura do XML de retorno do processamento da Inutilização
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>21/04/2009</date>
        protected override void LerRetornoInut()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            XmlDocument doc = new XmlDocument();

            try
            {
                MemoryStream msXml = Functions.StringXmlToStream(this.vStrXmlRetorno);
                doc.Load(msXml);

                XmlNodeList retInutNFeList = null;

                switch (Propriedade.TipoAplicativo)
                {
                    case TipoAplicativo.Cte:
                        retInutNFeList = doc.GetElementsByTagName("retInutCTe");
                        break;
                    case TipoAplicativo.Nfe:
                        retInutNFeList = doc.GetElementsByTagName("retInutNFe");
                        break;
                    default:
                        break;
                }

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

                            oGerarXML.XmlDistInut(NomeArquivoXML, strRetInutNFe);

                            //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                            MoverArquivo(NomeArquivoXML, PastaEnviados.Autorizados, DateTime.Now);

                            //Move o arquivo de Distribuição para a pasta de enviados autorizados
                            string strNomeArqProcInutNFe = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
                                                            PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                            Functions/*oAux*/.ExtrairNomeArq(NomeArquivoXML, Propriedade.ExtEnvio.PedInu_XML/*"-ped-inu.xml"*/) + Propriedade.ExtRetorno.ProcInutNFe;// "-procInutNFe.xml";
                            MoverArquivo(strNomeArqProcInutNFe, PastaEnviados.Autorizados, DateTime.Now);
                        }
                        else
                        {
                            //Deletar o arquivo de solicitação do serviço da pasta de envio
                            Functions.DeletarArquivo(NomeArquivoXML);
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
#endif

        #region LerRetornoLoteNFe()
        /// <summary>
        /// Efetua a leitura do XML de retorno do processamento do lote de notas fiscais e 
        /// atualiza o arquivo de fluxo e envio de notas
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        /// 
#if notused
        protected override void LerRetornoLoteNFe()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
            var oLerXml = new LerXML();
            var msXml = Functions.StringXmlToStream(vStrXmlRetorno);

            var fluxoNFe = new FluxoNfe();

            try
            {
                var doc = new XmlDocument();
                doc.Load(msXml);

                var retConsReciNFeList = doc.GetElementsByTagName("retConsReciNFe");

                foreach (XmlNode retConsReciNFeNode in retConsReciNFeList)
                {
                    var retConsReciNFeElemento = (XmlElement)retConsReciNFeNode;

                    //Pegar o número do recibo do lote enviado
                    var nRec = string.Empty;
                    if (retConsReciNFeElemento.GetElementsByTagName("nRec")[0] != null)
                    {
                        nRec = retConsReciNFeElemento.GetElementsByTagName("nRec")[0].InnerText;
                    }

                    //Pegar o status de retorno do lote enviado
                    var cStatLote = string.Empty;
                    if (retConsReciNFeElemento.GetElementsByTagName("cStat")[0] != null)
                    {
                        cStatLote = retConsReciNFeElemento.GetElementsByTagName("cStat")[0].InnerText;
                    }

                    switch (cStatLote)
                    {
                        #region Rejeições do XML de consulta do recibo (Não é o lote que foi rejeitado e sim o XML de consulta do recibo)

                        #region Validação do certificado de transmissão
                        case "280":
                        case "281":
                        case "282":
                        case "283":
                        case "284":
                        case "285":
                        case "286":
                        #endregion

                        #region Validação inicial da mensagem no webservice
                        case "214":
                        case "243":
                        case "108":
                        case "109":
                        #endregion

                        #region Validação das informações de controle da chamada ao webservice
                        case "242":
                        case "409":
                        case "410":
                        case "411":
                        case "238":
                        case "239":
                        #endregion

                        #region Validação da forma da área de dados
                        case "215":
                        case "516":
                        case "517":
                        case "545":
                        case "587":
                        case "588":
                        case "404":
                        case "402":
                        #endregion

                        #region Validação das regras de negócio da consulta recibo
                        case "252":
                        case "248":
                        case "553":
                        case "105":
                        case "223":
                        #endregion
                            break;

                        #region Lote não foi localizado pelo recibo que está sendo consultado
                        case "106": //E-Verifica se o lote não está na fila de saída, nem na fila de entrada (Lote não encontrado)
                            //No caso do lote não encontrado através do recibo, o ERP vai ter que consultar a situação da NFe para encerrar ela
                            //Vou somente excluir ela do fluxo para não ficar consultando o recibo que não existe
                            if (nRec != string.Empty)
                            {
                                fluxoNFe.ExcluirNfeFluxoRec(nRec.Trim());
                            }
                            break;
                        #endregion

                        #endregion

                        #region Lote foi processado, agora tenho que tratar as notas fiscais dele
                        case "104": //Lote processado
                            var protNFeList = retConsReciNFeElemento.GetElementsByTagName("protNFe");

                            foreach (XmlNode protNFeNode in protNFeList)
                            {
                                var protNFeElemento = (XmlElement)protNFeNode;

                                var strProtNfe = protNFeElemento.OuterXml;

                                var infProtList = protNFeElemento.GetElementsByTagName("infProt");

                                foreach (XmlNode infProtNode in infProtList)
                                {
                                    bool tirarFluxo = true;
                                    var infProtElemento = (XmlElement)infProtNode;

                                    var strChaveNFe = string.Empty;
                                    var strStat = string.Empty;

                                    if (infProtElemento.GetElementsByTagName("chNFe")[0] != null)
                                    {
                                        strChaveNFe = "NFe" + infProtElemento.GetElementsByTagName("chNFe")[0].InnerText;
                                    }

                                    if (infProtElemento.GetElementsByTagName("cStat")[0] != null)
                                    {
                                        strStat = infProtElemento.GetElementsByTagName("cStat")[0].InnerText;
                                    }

                                    //Definir o nome do arquivo da NFe e seu caminho
                                    var strNomeArqNfe = fluxoNFe.LerTag(strChaveNFe, FluxoNfe.ElementoFixo.ArqNFe);

                                    // danasa 8-2009
                                    // se por algum motivo o XML não existir no "Fluxo", então o arquivo tem que existir
                                    // na pasta "EmProcessamento" assinada.
                                    if (string.IsNullOrEmpty(strNomeArqNfe))
                                    {
                                        if (string.IsNullOrEmpty(strChaveNFe))
                                            throw new Exception("LerRetornoLoteNFe(): Não pode obter o nome do arquivo");

                                        strNomeArqNfe = strChaveNFe.Substring(3) + Propriedade.ExtEnvio.Nfe;
                                    }
                                    var strArquivoNFe = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + strNomeArqNfe;

                                    //Atualizar a Tag de status da NFe no fluxo para que se ocorrer alguma falha na exclusão eu tenha esta campo para ter uma referencia em futuras consultas
                                    fluxoNFe.AtualizarTag(strChaveNFe, FluxoNfe.ElementoEditavel.cStat, strStat);

                                    //Atualizar a tag da data e hora da ultima consulta do recibo
                                    fluxoNFe.AtualizarDPedRec(nRec, DateTime.Now);

                                    switch (strStat)
                                    {
                                        case "100": //NFe Autorizada
                                            if (File.Exists(strArquivoNFe))
                                            {
                                                //Juntar o protocolo com a NFE já copiando para a pasta de autorizadas
                                                var strArquivoNFeProc = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
                                                                        PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                                        Functions.ExtrairNomeArq(strNomeArqNfe, Propriedade.ExtEnvio.Nfe) + Propriedade.ExtRetorno.ProcNFe;

                                                //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s autorizados
                                                oLerXml.Nfe(strArquivoNFe);

                                                //Verificar se a -nfe.xml existe na pasta de autorizados
                                                bool NFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.ExtEnvio.Nfe);

                                                //Verificar se o -procNfe.xml existe na past de autorizados
                                                bool procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.ExtRetorno.ProcNFe);

                                                //Se o XML de distribuição não estiver na pasta de autorizados
                                                if (!procNFeJaNaAutorizada)
                                                {
                                                    if (!File.Exists(strArquivoNFeProc))
                                                    {
                                                        oGerarXML.XmlDistNFe(strArquivoNFe, strProtNfe, Propriedade.ExtRetorno.ProcNFe);
                                                    }
                                                }

                                                if (!procNFeJaNaAutorizada)
                                                {
                                                    //Mover a nfePRoc da pasta de NFE em processamento para a NFe Autorizada
                                                    //Para envitar falhar, tenho que mover primeiro o XML de distribuição (-procnfe.xml) para
                                                    //depois mover o da nfe (-nfe.xml), pois se ocorrer algum erro, tenho como reconstruir o senário, 
                                                    //assim sendo não inverta as posições. Wandrey 08/10/2009
                                                    MoverArquivo(strArquivoNFeProc, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);

                                                    //Atualizar a situação para que eu só mova o arquivo com final -NFe.xml para a pasta autorizado se 
                                                    //a procnfe já estiver lá, ou vai ficar na pasta emProcessamento para tentar gerar novamente.
                                                    //Isso vai dar uma maior segurança para não deixar sem gerar o -procnfe.xml. Wandrey 13/12/2012
                                                    procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.ExtRetorno.ProcNFe);
                                                }

                                                if (!NFeJaNaAutorizada && procNFeJaNaAutorizada)
                                                {
                                                    //Mover a NFE da pasta de NFE em processamento para NFe Autorizada
                                                    //Para envitar falhar, tenho que mover primeiro o XML de distribuição (-procnfe.xml) para 
                                                    //depois mover o da nfe (-nfe.xml), pois se ocorrer algum erro, tenho como reconstruir o senário.
                                                    //assim sendo não inverta as posições. Wandrey 08/10/2009
                                                    MoverArquivo(strArquivoNFe, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);
                                                }

                                                //Disparar a geração/impressçao do UniDanfe. 03/02/2010 - Wandrey
                                                if (procNFeJaNaAutorizada)
                                                    ExecutaUniDanfe(strNomeArqNfe, oLerXml.oDadosNfe.dEmi);

                                                //Vou verificar se estão os dois arquivos na pasta Autorizados, se tiver eu tiro do fluxo caso contrário não. Wandrey 13/02/2012
                                                NFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.ExtEnvio.Nfe);
                                                procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.ExtRetorno.ProcNFe);
                                                if (!procNFeJaNaAutorizada || !NFeJaNaAutorizada)
                                                {
                                                    tirarFluxo = false;
                                                }
                                            }
                                            break;

                                        case "110":
                                        case "205":
                                        case "301":
                                        case "302":
                                            ProcessaNotaDenegada(emp, oLerXml, strArquivoNFe, infProtElemento);
                                            /*
                                                                                        if (File.Exists(strArquivoNFe))
                                                                                        {
                                                                                            //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s Denegados
                                                                                            oLerXml.Nfe(strArquivoNFe);

                                                                                            //Mover a NFE da pasta de NFE em processamento para NFe Denegadas
                                                                                            oAux.MoverArquivo(strArquivoNFe, PastaEnviados.Denegados, oLerXml.oDadosNfe.dEmi);
                                                                                        }*/
                                            break;

                                        //case "302": //NFe Denegada - Problemas com o destinatário
                                        //    goto case "301";

                                        //case "110": //NFe Denegada - Não sei quando ocorre este, mas descobrir ele no manual então estou incluindo. Wandrey 20/10/2009
                                        //    goto case "301";

                                        default: //NFe foi rejeitada
                                            //O Status da NFe tem que ser maior que 1 ou deu algum erro na hora de ler o XML de retorno da consulta do recibo, sendo assim, vou mantar a nota no fluxo para consultar novamente.
                                            if (Convert.ToInt32(strStat) >= 1)
                                            {
                                                //Mover o XML da NFE a pasta de XML´s com erro
                                                oAux.MoveArqErro(strArquivoNFe);
                                            }
                                            else
                                                tirarFluxo = false;
                                            break;
                                    }

                                    //Deletar a NFE do arquivo de controle de fluxo
                                    if (tirarFluxo)
                                        fluxoNFe.ExcluirNfeFluxo(strChaveNFe);

                                    break;
                                }
                            }
                            break;
                        #endregion

                        #region Qualquer outro tipo de status que não for os acima relacionados, vai tirar a nota fiscal do fluxo.
                        default:
                            //Qualquer outro tipo de rejeião vou tirar todas as notas do lote do fluxo, pois se o lote foi rejeitado, todas as notas fiscais também foram
                            //De acordo com o manual de integração se o status do lote não for 104, tudo foi rejeitado. Wandrey 20/07/2010
                            if (Convert.ToInt32(cStatLote) >= 1)
                            {
                                //Vou retirar as notas do fluxo pelo recibo
                                if (nRec != string.Empty)
                                {
                                    fluxoNFe.ExcluirNfeFluxoRec(nRec.Trim());
                                }
                            }

                            break;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
#endif
        #endregion

        #region LerRetornoLoteCTe()
        /// <summary>
        /// Efetua a leitura do XML de retorno do processamento do lote de notas fiscais e 
        /// atualiza o arquivo de fluxo e envio de notas
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        /// 
#if notused
        protected override void LerRetornoLoteCTe()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
            var oLerXml = new LerXML();
            var msXml = Functions.StringXmlToStream(vStrXmlRetorno);

            var fluxoNFe = new FluxoNfe();

            try
            {
                var doc = new XmlDocument();
                doc.Load(msXml);

                var retConsReciNFeList = doc.GetElementsByTagName("retConsReciCTe");

                foreach (XmlNode retConsReciNFeNode in retConsReciNFeList)
                {
                    var retConsReciNFeElemento = (XmlElement)retConsReciNFeNode;

                    //Pegar o número do recibo do lote enviado
                    var nRec = string.Empty;
                    if (retConsReciNFeElemento.GetElementsByTagName("nRec")[0] != null)
                    {
                        nRec = retConsReciNFeElemento.GetElementsByTagName("nRec")[0].InnerText;
                    }

                    //Pegar o status de retorno do lote enviado
                    var cStatLote = string.Empty;
                    if (retConsReciNFeElemento.GetElementsByTagName("cStat")[0] != null)
                    {
                        cStatLote = retConsReciNFeElemento.GetElementsByTagName("cStat")[0].InnerText;
                    }

                    switch (cStatLote)
                    {
                        #region Rejeições do XML de consulta do recibo (Não é o lote que foi rejeitado e sim o XML de consulta do recibo)
                        case "280": //A-Certificado transmissor inválido
                        case "281": //A-Validade do certificado
                        case "283": //A-Verifica a cadeia de Certificação
                        case "286": //A-LCR do certificado de Transmissor
                        case "284": //A-Certificado do Transmissor revogado
                        case "285": //A-Certificado Raiz difere da "IPC-Brasil"
                        case "282": //A-Falta a extensão de CNPJ no Certificado
                        case "214": //B-Tamanho do XML de dados superior a 500 Kbytes
                        case "243": //B-XML de dados mal formatado
                        case "108": //B-Verifica se o Serviço está paralisado momentaneamente
                        case "109": //B-Verifica se o serviço está paralisado sem previsão
                        case "242": //C-Elemento nfeCabecMsg inexistente no SOAP Header
                        case "409": //C-Campo cUF inexistente no elemento nfeCabecMsg do SOAP Header
                        case "410": //C-Campo versaoDados inexistente no elemento nfeCabecMsg do SOAP
                        case "411": //C-Campo versaoDados inexistente no elemento nfeCabecMsg do SOAP
                        case "238": //C-Versão dos Dados informada é superior à versão vigente
                        case "239": //C-Versão dos Dados não suportada
                        case "215": //D-Verifica schema XML da área de dados
                        case "404": //D-Verifica o uso de prefixo no namespace
                        case "402": //D-XML utiliza codificação diferente de UTF-8
                        case "252": //E-Tipo do ambiente da NF-e difere do ambiente do web service
                        case "226": //E-UF da Chave de Acesso difere da UF do Web Service
                        case "236": //E-Valida DV da Chave de Acesso
                        case "217": //E-Acesso BD CTE
                        case "216": //E-Verificar se campo "Codigo Numerico"
                            break;
                        #endregion

                        #region Lote ainda está sendo processado
                        case "105": //E-Verifica se o lote não está na fila de resposta, mas está na fila de entrada (Lote em processamento)
                            //Ok vou aguardar o ERP gerar uma nova consulta para encerrar o fluxo da nota
                            break;
                        #endregion

                        #region Lote não foi localizado pelo recibo que está sendo consultado
                        case "106": //E-Verifica se o lote não está na fila de saída, nem na fila de entrada (Lote não encontrado)
                            //No caso do lote não encontrado através do recibo, o ERP vai ter que consultar a situação da NFe para encerrar ela
                            //Vou somente excluir ela do fluxo para não ficar consultando o recibo que não existe
                            if (nRec != string.Empty)
                            {
                                fluxoNFe.ExcluirNfeFluxoRec(nRec.Trim());
                            }
                            break;
                        #endregion

                        #region Lote foi processado, agora tenho que tratar as notas fiscais dele
                        case "104": //Lote processado
                            var protNFeList = retConsReciNFeElemento.GetElementsByTagName("protCTe");

                            foreach (XmlNode protNFeNode in protNFeList)
                            {
                                var protNFeElemento = (XmlElement)protNFeNode;

                                var strProtNfe = protNFeElemento.OuterXml;

                                var infProtList = protNFeElemento.GetElementsByTagName("infProt");

                                foreach (XmlNode infProtNode in infProtList)
                                {
                                    var infProtElemento = (XmlElement)infProtNode;

                                    var strChaveNFe = string.Empty;
                                    var strStat = string.Empty;

                                    if (infProtElemento.GetElementsByTagName("chCTe")[0] != null)
                                    {
                                        strChaveNFe = "CTe" + infProtElemento.GetElementsByTagName("chCTe")[0].InnerText;
                                    }

                                    if (infProtElemento.GetElementsByTagName("cStat")[0] != null)
                                    {
                                        strStat = infProtElemento.GetElementsByTagName("cStat")[0].InnerText;
                                    }

                                    //Definir o nome do arquivo da NFe e seu caminho
                                    var strNomeArqNfe = fluxoNFe.LerTag(strChaveNFe, FluxoNfe.ElementoFixo.ArqNFe);

                                    // danasa 8-2009
                                    // se por algum motivo o XML não existir no "Fluxo", então o arquivo tem que existir
                                    // na pasta "EmProcessamento" assinada.
                                    if (string.IsNullOrEmpty(strNomeArqNfe))
                                    {
                                        if (string.IsNullOrEmpty(strChaveNFe))
                                            throw new Exception("LerRetornoLoteCTe(): Não pode obter o nome do arquivo");

                                        strNomeArqNfe = strChaveNFe.Substring(3) + Propriedade.ExtEnvio.Nfe;
                                    }
                                    var strArquivoNFe = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + strNomeArqNfe;

                                    //Atualizar a Tag de status da NFe no fluxo para que se ocorrer alguma falha na exclusão eu tenha esta campo para ter uma referencia em futuras consultas
                                    fluxoNFe.AtualizarTag(strChaveNFe, FluxoNfe.ElementoEditavel.cStat, strStat);

                                    //Atualizar a tag da data e hora da ultima consulta do recibo
                                    fluxoNFe.AtualizarDPedRec(nRec, DateTime.Now);

                                    switch (strStat)
                                    {
                                        case "100": //NFe Autorizada
                                            if (File.Exists(strArquivoNFe))
                                            {
                                                //Juntar o protocolo com a NFE já copiando para a pasta de autorizadas
                                                oGerarXML.XmlDistNFe(strArquivoNFe, strProtNfe, Propriedade.ExtRetorno.ProcNFe);
                                                var strArquivoNFeProc = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
                                                                        PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                                        Functions/*oAux*/.ExtrairNomeArq(strNomeArqNfe, Propriedade.ExtEnvio.Nfe) + Propriedade.ExtRetorno.ProcNFe;

                                                //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s autorizados
                                                oLerXml.Nfe(strArquivoNFe);

                                                //Mover a nfePRoc da pasta de NFE em processamento para a NFe Autorizada
                                                //Para envitar falhar, tenho que mover primeiro o XML de distribuição (-procnfe.xml) para
                                                //depois mover o da nfe (-nfe.xml), pois se ocorrer algum erro, tenho como reconstruir o senário, 
                                                //assim sendo não inverta as posições. Wandrey 08/10/2009
                                                MoverArquivo(strArquivoNFeProc, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);

                                                //Mover a NFE da pasta de NFE em processamento para NFe Autorizada
                                                //Para envitar falhar, tenho que mover primeiro o XML de distribuição (-procnfe.xml) para 
                                                //depois mover o da nfe (-nfe.xml), pois se ocorrer algum erro, tenho como reconstruir o senário.
                                                //assim sendo não inverta as posições. Wandrey 08/10/2009
                                                MoverArquivo(strArquivoNFe, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);

                                                //Disparar a geração/impressçao do UniDanfe. 03/02/2010 - Wandrey
                                                //ExecutaUniDanfe(strNomeArqNfe, oLerXml.oDadosNfe.dEmi);
                                            }
                                            break;

                                        case "301": //NFe Denegada - Irregularidade fiscal do emitente
                                            if (File.Exists(strArquivoNFe))
                                            {
                                                //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s Denegados
                                                oLerXml.Nfe(strArquivoNFe);

                                                //Mover a NFE da pasta de NFE em processamento para NFe Denegadas
                                                MoverArquivo(strArquivoNFe, PastaEnviados.Denegados, oLerXml.oDadosNfe.dEmi);
                                            }
                                            break;

                                        case "302": //NFe Denegada - Irregularidade fiscal do remetente
                                            goto case "301";

                                        case "303": //NFe Denegada - Irregularidade fiscal do destinatário
                                            goto case "301";

                                        case "304": //NFe Denegada - Irregularidade fiscal do expedidor
                                            goto case "301";

                                        case "305": //NFe Denegada - Irregularidade fiscal do recebedor
                                            goto case "301";

                                        case "306": //NFe Denegada - Irregularidade fiscal do tomador
                                            goto case "301";

                                        case "110": //NFe Denegada - Não sei quando ocorre este, mas descobrir ele no manual então estou incluindo. Wandrey 20/10/2009
                                            goto case "301";

                                        default: //NFe foi rejeitada
                                            //Mover o XML da NFE a pasta de XML´s com erro
                                            oAux.MoveArqErro(strArquivoNFe);
                                            break;
                                    }

                                    //Deletar a NFE do arquivo de controle de fluxo
                                    fluxoNFe.ExcluirNfeFluxo(strChaveNFe);
                                    break;
                                }
                            }
                            break;
                        #endregion

                        #region Qualquer outro tipo de status que não for os acima relacionados, vai tirar a nota fiscal do fluxo.
                        default:
                            //Qualquer outro tipo de rejeião vou tirar todas as notas do lote do fluxo, pois se o lote foi rejeitado, todas as notas fiscais também foram
                            //De acordo com o manual de integração se o status do lote não for 104, tudo foi rejeitado. Wandrey 20/07/2010

                            //Vou retirar as notas do fluxo pelo recibo
                            if (nRec != string.Empty)
                            {
                                fluxoNFe.ExcluirNfeFluxoRec(nRec.Trim());
                            }

                            break;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
#endif
        #endregion

        #region LerRetornoSitNFe()
        /// <summary>
        /// Ler o retorno da consulta situação da nota fiscal e de acordo com o status ele trata as notas enviadas se ainda não foram tratadas
        /// </summary>
        /// <param name="ChaveNFe">Chave da NFe que está sendo consultada</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 16/06/2010
        /// </remarks>
        /// 
#if notused
        protected override void LerRetornoSitNFe(string ChaveNFe)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            LerXML oLerXml = new LerXML();
            MemoryStream msXml = Functions.StringXmlToStream(this.vStrXmlRetorno);

            FluxoNfe oFluxoNfe = new FluxoNfe();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(msXml);

                #region Distribuicao de Eventos

                oGerarXML.XmlDistEvento(emp, this.vStrXmlRetorno);    //<<<danasa 6-2011

                #endregion

                #region Cancelamento NFe
                new TaskCancelamento().GerarXmlDistCanc(ChaveNFe); //Wandrey 12/01/2012
                #endregion

                XmlNodeList retConsSitList = doc.GetElementsByTagName("retConsSitNFe");

                foreach (XmlNode retConsSitNode in retConsSitList)
                {
                    XmlElement retConsSitElemento = (XmlElement)retConsSitNode;

                    //Definir a chave da NFe a ser pesquisada
                    string strChaveNFe = "NFe" + ChaveNFe;

                    //Definir o nome do arquivo da NFe e seu caminho
                    string strNomeArqNfe = oFluxoNfe.LerTag(strChaveNFe, FluxoNfe.ElementoFixo.ArqNFe);

                    if (string.IsNullOrEmpty(strNomeArqNfe))
                    {
                        if (string.IsNullOrEmpty(strChaveNFe))
                            throw new Exception("LerRetornoSitNFe(): Não pode obter o nome do arquivo");

                        strNomeArqNfe = strChaveNFe.Substring(3) + Propriedade.ExtEnvio.Nfe;
                    }

                    string strArquivoNFe = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + strNomeArqNfe;

                    //Pegar o status de retorno da NFe que está sendo consultada a situação
                    var cStatCons = string.Empty;
                    if (retConsSitElemento.GetElementsByTagName("cStat")[0] != null)
                    {
                        cStatCons = retConsSitElemento.GetElementsByTagName("cStat")[0].InnerText;
                    }

                    switch (cStatCons)
                    {
                        #region Rejeições do XML de consulta da situação da NFe (Não é a nfe que foi rejeitada e sim o XML de consulta da situação da nfe)

                        #region Validação do Certificado de Transmissão
                        case "280":
                        case "281":
                        case "283":
                        case "286":
                        case "284":
                        case "285":
                        case "282":
                        #endregion

                        #region Validação Inicial da Mensagem no WebService
                        case "214":
                        case "243":
                        case "108":
                        case "109":
                        #endregion

                        #region Validação das informações de controle da chamada ao WebService
                        case "242":
                        case "409":
                        case "410":
                        case "411":
                        case "238":
                        case "239":
                        #endregion

                        #region Validação da forma da área de dados
                        case "215":
                        case "516":
                        case "517":
                        case "545":
                        case "587":
                        case "588":
                        case "404":
                        case "402":
                        #endregion

                        #region Validação das regras de negócios da consulta a NF-e
                        case "252":
                        case "226":
                        case "236":
                        case "614":
                        case "615":
                        case "616":
                        case "617":
                        case "618":
                        case "619":
                        case "620":
                            break;
                        #endregion

                        #region Nota fiscal rejeitada
                        case "217": //J-NFe não existe na base de dados do SEFAZ
                            goto case "TirarFluxo";

                        case "562": //J-Verificar se o campo "Código Numérico" informado na chave de acesso é diferente do existente no BD
                            goto case "TirarFluxo";

                        case "561": //J-Verificar se campo MM (mês) informado na Chave de Acesso é diferente do existente no BD
                            goto case "TirarFluxo";
                        #endregion

                        #endregion

                        #region Nota fiscal autorizada
                        case "100": //Autorizado o uso da NFe
                            XmlNodeList infConsSitList = retConsSitElemento.GetElementsByTagName("infProt");
                            if (infConsSitList != null)
                            {
                                foreach (XmlNode infConsSitNode in infConsSitList)
                                {
                                    XmlElement infConsSitElemento = (XmlElement)infConsSitNode;

                                    //Pegar o Status do Retorno da consulta situação
                                    string strStat = Functions.LerTag(infConsSitElemento, "cStat").Replace(";", "");

                                    switch (strStat)
                                    {
                                        case "100":
                                            //O retorno da consulta situação a posição das tag´s é diferente do que vem 
                                            //na consulta do recibo, assim sendo tenho que montar esta parte do XML manualmente
                                            //para que fique um XML de distribuição válido. Wandrey 07/10/2009
                                            string strProtNfe = GeraStrProtNFe(infConsSitElemento);//danasa 11-4-2012

                                            //Definir o nome do arquivo -procNfe.xml                                               
                                            string strArquivoNFeProc = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
                                                                        PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                                        Functions/*oAux*/.ExtrairNomeArq(strArquivoNFe, Propriedade.ExtEnvio.Nfe) + Propriedade.ExtRetorno.ProcNFe;

                                            //Se existir o strArquivoNfe, tem como eu fazer alguma coisa, se ele não existir
                                            //Não tenho como fazer mais nada. Wandrey 08/10/2009
                                            if (File.Exists(strArquivoNFe))
                                            {
                                                //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s autorizados
                                                oLerXml.Nfe(strArquivoNFe);

                                                //Verificar se a -nfe.xml existe na pasta de autorizados
                                                bool NFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.ExtEnvio.Nfe);

                                                //Verificar se o -procNfe.xml existe na past de autorizados
                                                bool procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.ExtRetorno.ProcNFe);

                                                //Se o XML de distribuição não estiver na pasta de autorizados
                                                if (!procNFeJaNaAutorizada)
                                                {
                                                    if (!File.Exists(strArquivoNFeProc))
                                                    {
                                                        oGerarXML.XmlDistNFe(strArquivoNFe, strProtNfe, Propriedade.ExtRetorno.ProcNFe);
                                                    }
                                                }

                                                //Se o XML de distribuição não estiver ainda na pasta de autorizados
                                                if (!procNFeJaNaAutorizada)
                                                {
                                                    //Move a nfeProc da pasta de NFE em processamento para a NFe Autorizada
                                                    MoverArquivo(strArquivoNFeProc, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);

                                                    //Atualizar a situação para que eu só mova o arquivo com final -NFe.xml para a pasta autorizado se 
                                                    //a procnfe já estiver lá, ou vai ficar na pasta emProcessamento para tentar gerar novamente.
                                                    //Isso vai dar uma maior segurança para não deixar sem gerar o -procnfe.xml. Wandrey 13/12/2012
                                                    procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.ExtRetorno.ProcNFe);
                                                }

                                                //Se a NFe não existir ainda na pasta de autorizados
                                                if (!NFeJaNaAutorizada)
                                                {
                                                    //1-Mover a NFE da pasta de NFE em processamento para NFe Autorizada
                                                    //2-Só vou mover o -nfe.xml para a pasta autorizados se já existir a -procnfe.xml, caso contrário vou manter na pasta EmProcessamento
                                                    //  para tentar gerar novamente o -procnfe.xml
                                                    //  Isso vai dar uma maior segurança para não deixar sem gerar o -procnfe.xml. Wandrey 13/12/2012
                                                    if (procNFeJaNaAutorizada)
                                                        MoverArquivo(strArquivoNFe, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);
                                                }
                                                else
                                                {
                                                    //1-Se já estiver na pasta de autorizados, vou somente mover ela da pasta de XML´s em processamento
                                                    //2-Só vou mover o -nfe.xml da pasta EmProcessamento se também existir a -procnfe.xml na pasta autorizados, caso contrário vou manter na pasta EmProcessamento
                                                    //  para tentar gerar novamente o -procnfe.xml
                                                    //  Isso vai dar uma maior segurança para não deixar sem gerar o -procnfe.xml. Wandrey 13/12/2012
                                                    if (procNFeJaNaAutorizada)
                                                        oAux.MoveArqErro(strArquivoNFe);
                                                    //oAux.DeletarArquivo(strArquivoNFe);
                                                }

                                                //Disparar a geração/impressão do UniDanfe. 03/02/2010 - Wandrey
                                                if (procNFeJaNaAutorizada)
                                                    ExecutaUniDanfe(strNomeArqNfe, oLerXml.oDadosNfe.dEmi);
                                            }

                                            if (File.Exists(strArquivoNFeProc))
                                            {
                                                //Se já estiver na pasta de autorizados, vou somente excluir ela da pasta de XML´s em processamento
                                                Functions.DeletarArquivo(strArquivoNFeProc);
                                            }

                                            break;

                                        //danasa 11-4-2012
                                        case "110": //Uso Denegado
                                        case "301":
                                        case "302":
                                            //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s Denegados
                                            //
                                            // NFe existe na pasta EmProcessamento?
                                            ProcessaNotaDenegada(emp, oLerXml, strArquivoNFe, infConsSitElemento);
                                            break;

                                        //case "302":
                                        //    goto case "301";

                                        //case "110": //Uso Denegado
                                        //    goto case "301";

                                        default:
                                            //Mover o XML da NFE a pasta de XML´s com erro
                                            oAux.MoveArqErro(strArquivoNFe);
                                            break;
                                    }

                                    //Deletar a NFE do arquivo de controle de fluxo
                                    oFluxoNfe.ExcluirNfeFluxo(strChaveNFe);
                                }
                            }
                            break;
                        #endregion

                        #region Nota fiscal cancelada
                        case "101": //Cancelamento Homologado ou Nfe Cancelada
                            goto case "100";
                        #endregion

                        #region Nota fiscal Denegada
                        case "110": //NFe Denegada
                            goto case "100";

                        case "301": //NFe Denegada
                            goto case "100";

                        case "302": //NFe Denegada
                            goto case "100";

                        case "205": //Nfe já está denegada na base do SEFAZ
                            goto case "100";    ///<<<<<<<<<< ??????????????????? >>>>>>>>>>>>
                        ///
                        //Ler o XML para pegar a data de emissão para criar a psta dos XML´s Denegados
                        /*
                        if (File.Exists(strArquivoNFe))
                        {
                            oLerXml.Nfe(strArquivoNFe);

                            //Move a NFE da pasta de NFE em processamento para NFe Denegadas
                            if (!oAux.EstaDenegada(strArquivoNFe, oLerXml.oDadosNfe.dEmi))
                            {
                                oAux.MoverArquivo(strArquivoNFe, PastaEnviados.Denegados, oLerXml.oDadosNfe.dEmi);
                            }
                        }
                        break;*/

                        #endregion

                        #region Conteúdo para retirar a nota fiscal do fluxo
                        case "TirarFluxo":
                            //Mover o XML da NFE a pasta de XML´s com erro
                            oAux.MoveArqErro(strArquivoNFe);

                            //Deletar a NFE do arquivo de controle de fluxo
                            oFluxoNfe.ExcluirNfeFluxo(strChaveNFe);
                            break;
                        #endregion

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
#endif

        private void ProcessaNotaDenegada(int emp, LerXML oLerXml, string strArquivoNFe, XmlElement infConsSitElemento)
        {
            string strProtNfe;

            if (File.Exists(strArquivoNFe))
            {
                oLerXml.Nfe(strArquivoNFe);

                string nomePastaEnviado = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
                                            PastaEnviados.Denegados.ToString() + "\\" +
                                            Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(oLerXml.oDadosNfe.dEmi);
                string dArquivo = Path.Combine(nomePastaEnviado, Path.GetFileName(strArquivoNFe).Replace(Propriedade.ExtEnvio.Nfe, "-den.xml"));

                //danasa 11-4-2012
                bool addNFeDen = true;
                if (File.Exists(dArquivo))
                {
                    // verifica se a NFe já tem protocolo gravado
                    // só para atualizar notas denegadas que ainda não tem o protocolo atualizado 
                    // e que já estao na pasta de notas denegadas.
                    // Para futuras notas denegadas esta propriedade sempre será false
                    if (File.ReadAllText(dArquivo).IndexOf("<protNFe>") > 0)
                        addNFeDen = false;
                }
                if (addNFeDen)
                {
                    ///
                    /// monta o XML de denegacao
                    strProtNfe = this.GeraStrProtNFe(infConsSitElemento);
                    ///
                    /// gera o arquivo de denegacao na pasta EmProcessamento
                    string strNomeArqDenegadaNFe = oGerarXML.XmlDistNFe(strArquivoNFe, strProtNfe, "-den.xml");
                    ///
                    /// exclui o XML denegado, se existir
                    //string destinoArquivo = Path.Combine(nomePastaEnviado, Path.GetFileName(strNomeArqDenegadaNFe));
                    Functions.DeletarArquivo(dArquivo);
                    ///
                    /// Move a NFE-denegada da pasta em processamento para NFe Denegadas
                    MoverArquivo(strNomeArqDenegadaNFe, PastaEnviados.Denegados, oLerXml.oDadosNfe.dEmi);
                    ///
                    /// verifica se o arquivo da NFe já existe na pasta denegadas
                    dArquivo = Path.Combine(nomePastaEnviado, Path.GetFileName(strArquivoNFe));
                    if (!File.Exists(dArquivo))
                    {
                        if (Empresa.Configuracoes[emp].PastaBackup.Trim() != "")
                        {
                            //Criar Pasta do Mês para gravar arquivos enviados
                            string nomePastaBackup = Empresa.Configuracoes[emp].PastaBackup + "\\" +
                                                        PastaEnviados.Denegados + "\\" +
                                                        Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(oLerXml.oDadosNfe.dEmi);
                            if (!Directory.Exists(nomePastaBackup))
                                System.IO.Directory.CreateDirectory(nomePastaBackup);

                            //Se conseguiu criar a pasta ele move o arquivo, caso contrário
                            if (Directory.Exists(nomePastaBackup))
                            {
                                //Mover o arquivo da nota fiscal para a pasta de backup
                                string destinoBackup = Path.Combine(nomePastaBackup, Path.GetFileName(strArquivoNFe));
                                Functions.DeletarArquivo(destinoBackup);
                                File.Copy(strArquivoNFe, destinoBackup);
                            }
                            else
                            {
                                //throw new Exception("Pasta de backup informada nas configurações não existe. (Pasta: " + nomePastaBackup + ")");
                            }
                        }
                        ///
                        /// move o arquivo NFe para a pasta Denegada
                        File.Move(strArquivoNFe, dArquivo);
                    }
                }
                //Move a NFE da pasta de NFE em processamento para NFe Denegadas
                //if (!oAux.EstaDenegada(strArquivoNFe, oLerXml.oDadosNfe.dEmi))
                //{
                //    oAux.MoverArquivo(strArquivoNFe, PastaEnviados.Denegados, oLerXml.oDadosNfe.dEmi);
                //}
            }
            //return strProtNfe;
        }

        /// <summary>
        /// GeraStrProtNFe
        /// </summary>
        /// <param name="infConsSitElemento"></param>
        /// <returns></returns>
        private string GeraStrProtNFe(XmlElement infConsSitElemento)//danasa 11-4-2012
        {
            string atributoId = string.Empty;
            if (infConsSitElemento.GetAttribute("Id").Length != 0)
            {
                atributoId = " Id=\"" + infConsSitElemento.GetAttribute("Id") + "\"";
            }

            string strProtNfe = "<protNFe versao=\"" + ConfiguracaoApp.VersaoXMLNFe + "\">" +
                "<infProt" + atributoId + ">" +
                "<tpAmb>" + Functions.LerTag(infConsSitElemento, "tpAmb", false) + "</tpAmb>" +
                "<verAplic>" + Functions.LerTag(infConsSitElemento, "verAplic", false) + "</verAplic>" +
                "<chNFe>" + Functions.LerTag(infConsSitElemento, "chNFe", false) + "</chNFe>" +
                "<dhRecbto>" + Functions.LerTag(infConsSitElemento, "dhRecbto", false) + "</dhRecbto>" +
                "<nProt>" + Functions.LerTag(infConsSitElemento, "nProt", false) + "</nProt>" +
                "<digVal>" + Functions.LerTag(infConsSitElemento, "digVal", false) + "</digVal>" +
                "<cStat>" + Functions.LerTag(infConsSitElemento, "cStat", false) + "</cStat>" +
                "<xMotivo>" + Functions.LerTag(infConsSitElemento, "xMotivo", false) + "</xMotivo>" +
                "</infProt>" +
                "</protNFe>";
            return strProtNfe;
        }

        #endregion

        #region LerRetornoSitCTe()
        /// <summary>
        /// Ler o retorno da consulta situação da nota fiscal e de acordo com o status ele trata as notas enviadas se ainda não foram tratadas
        /// </summary>
        /// <param name="ChaveNFe">Chave da NFe que está sendo consultada</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 16/06/2010
        /// </remarks>
        protected override void LerRetornoSitCTe(string ChaveNFe)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            LerXML oLerXml = new LerXML();
            MemoryStream msXml = Functions.StringXmlToStream(this.vStrXmlRetorno);

            FluxoNfe oFluxoNfe = new FluxoNfe();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(msXml);

                XmlNodeList retConsSitList = doc.GetElementsByTagName("retConsSitCTe");

                foreach (XmlNode retConsSitNode in retConsSitList)
                {
                    XmlElement retConsSitElemento = (XmlElement)retConsSitNode;

                    //Definir a chave da NFe a ser pesquisada
                    string strChaveNFe = "CTe" + ChaveNFe;

                    //Definir o nome do arquivo da NFe e seu caminho
                    string strNomeArqNfe = oFluxoNfe.LerTag(strChaveNFe, FluxoNfe.ElementoFixo.ArqNFe);

                    if (string.IsNullOrEmpty(strNomeArqNfe))
                    {
                        if (string.IsNullOrEmpty(strChaveNFe))
                            throw new Exception("LerRetornoSitCTe(): Não pode obter o nome do arquivo");

                        strNomeArqNfe = strChaveNFe.Substring(3) + Propriedade.ExtEnvio.Nfe;
                    }

                    string strArquivoNFe = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + strNomeArqNfe;

                    //Pegar o status de retorno da NFe que está sendo consultada a situação
                    var cStatCons = string.Empty;
                    if (retConsSitElemento.GetElementsByTagName("cStat")[0] != null)
                    {
                        cStatCons = retConsSitElemento.GetElementsByTagName("cStat")[0].InnerText;
                    }

                    switch (cStatCons)
                    {
                        #region Rejeições do XML de consulta do recibo (Não é o lote que foi rejeitado e sim o XML de consulta do recibo)
                        case "280": //A-Certificado transmissor inválido
                        case "281": //A-Validade do certificado
                        case "283": //A-Verifica a cadeia de Certificação
                        case "286": //A-LCR do certificado de Transmissor
                        case "284": //A-Certificado do Transmissor revogado
                        case "285": //A-Certificado Raiz difere da "IPC-Brasil"
                        case "282": //A-Falta a extensão de CNPJ no Certificado
                        case "214": //B-Tamanho do XML de dados superior a 500 Kbytes
                        case "243": //B-XML de dados mal formatado
                        case "108": //B-Verifica se o Serviço está paralisado momentaneamente
                        case "109": //B-Verifica se o serviço está paralisado sem previsão
                        case "242": //C-Elemento nfeCabecMsg inexistente no SOAP Header
                        case "409": //C-Campo cUF inexistente no elemento nfeCabecMsg do SOAP Header
                        case "410": //C-Campo versaoDados inexistente no elemento nfeCabecMsg do SOAP
                        case "411": //C-Campo versaoDados inexistente no elemento nfeCabecMsg do SOAP
                        case "238": //C-Versão dos Dados informada é superior à versão vigente
                        case "239": //C-Versão dos Dados não suportada
                        case "215": //D-Verifica schema XML da área de dados
                        case "404": //D-Verifica o uso de prefixo no namespace
                        case "402": //D-XML utiliza codificação diferente de UTF-8
                        case "252": //E-Tipo do ambiente da NF-e difere do ambiente do web service
                        case "226": //E-UF da Chave de Acesso difere da UF do Web Service
                        case "236": //E-Valida DV da Chave de Acesso
                        case "216": //E-Verificar se campo "Codigo Numerico"
                            break;
                        #endregion

                        #region Nota fiscal rejeitada
                        case "217": //J-NFe não existe na base de dados do SEFAZ
                            goto case "TirarFluxo";
                        #endregion

                        #region Nota fiscal autorizada
                        case "100": //Autorizado o uso da NFe
                            XmlNodeList infConsSitList = retConsSitElemento.GetElementsByTagName("infProt");
                            if (infConsSitList != null)
                            {
                                foreach (XmlNode infConsSitNode in infConsSitList)
                                {
                                    XmlElement infConsSitElemento = (XmlElement)infConsSitNode;

                                    //Pegar o Status do Retorno da consulta situação
                                    string strStat = Functions.LerTag(infConsSitElemento, "cStat").Replace(";", "");

                                    switch (strStat)
                                    {
                                        case "100":
                                            //O retorno da consulta situação a posição das tag´s é diferente do que vem 
                                            //na consulta do recibo, assim sendo tenho que montar esta parte do XML manualmente
                                            //para que fique um XML de distribuição válido. Wandrey 07/10/2009
                                            string atributoId = string.Empty;
                                            if (infConsSitElemento.GetAttribute("Id").Length != 0)
                                            {
                                                atributoId = " Id=\"" + infConsSitElemento.GetAttribute("Id") + "\"";
                                            }

                                            string strProtNfe = "<protCTe versao=\"" + ConfiguracaoApp.VersaoXMLNFe + "\">" +
                                                "<infProt" + atributoId + ">" +
                                                "<tpAmb>" + Functions.LerTag(infConsSitElemento, "tpAmb", false) + "</tpAmb>" +
                                                "<verAplic>" + Functions.LerTag(infConsSitElemento, "verAplic", false) + "</verAplic>" +
                                                "<chCTe>" + Functions.LerTag(infConsSitElemento, "chCTe", false) + "</chCTe>" +
                                                "<dhRecbto>" + Functions.LerTag(infConsSitElemento, "dhRecbto", false) + "</dhRecbto>" +
                                                "<nProt>" + Functions.LerTag(infConsSitElemento, "nProt", false) + "</nProt>" +
                                                "<digVal>" + Functions.LerTag(infConsSitElemento, "digVal", false) + "</digVal>" +
                                                "<cStat>" + Functions.LerTag(infConsSitElemento, "cStat", false) + "</cStat>" +
                                                "<xMotivo>" + Functions.LerTag(infConsSitElemento, "xMotivo", false) + "</xMotivo>" +
                                                "</infProt>" +
                                                "</protCTe>";

                                            //Definir o nome do arquivo -procNfe.xml                                               
                                            string strArquivoNFeProc = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
                                                                        PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                                        Functions/*oAux*/.ExtrairNomeArq(strArquivoNFe, Propriedade.ExtEnvio.Nfe) + Propriedade.ExtRetorno.ProcNFe;

                                            //Se existir o strArquivoNfe, tem como eu fazer alguma coisa, se ele não existir
                                            //Não tenho como fazer mais nada. Wandrey 08/10/2009
                                            if (File.Exists(strArquivoNFe))
                                            {
                                                //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s autorizados
                                                oLerXml.Nfe(strArquivoNFe);

                                                //Verificar se a -nfe.xml existe na pasta de autorizados
                                                bool NFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.ExtEnvio.Nfe);

                                                //Verificar se o -procNfe.xml existe na past de autorizados
                                                bool procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, Propriedade.ExtRetorno.ProcNFe);

                                                //Se o XML de distribuição não estiver na pasta de autorizados
                                                if (!procNFeJaNaAutorizada)
                                                {
                                                    if (!File.Exists(strArquivoNFeProc))
                                                    {
                                                        oGerarXML.XmlDistNFe(strArquivoNFe, strProtNfe, Propriedade.ExtRetorno.ProcNFe);
                                                    }
                                                }

                                                //Se o XML de distribuição não estiver ainda na pasta de autorizados
                                                if (!procNFeJaNaAutorizada)
                                                {
                                                    //Move a nfeProc da pasta de NFE em processamento para a NFe Autorizada
                                                    MoverArquivo(strArquivoNFeProc, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);
                                                }

                                                //Se a NFe não existir ainda na pasta de autorizados
                                                if (!NFeJaNaAutorizada)
                                                {
                                                    //Mover a NFE da pasta de NFE em processamento para NFe Autorizada
                                                    MoverArquivo(strArquivoNFe, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);
                                                }
                                                else
                                                {
                                                    //Se já estiver na pasta de autorizados, vou somente excluir ela da pasta de XML´s em processamento
                                                    Functions.DeletarArquivo(strArquivoNFe);
                                                }

                                                //Disparar a geração/impressçao do UniDanfe. 03/02/2010 - Wandrey
                                                //ExecutaUniDanfe(strNomeArqNfe, oLerXml.oDadosNfe.dEmi);
                                            }

                                            if (File.Exists(strArquivoNFeProc))
                                            {
                                                //Se já estiver na pasta de autorizados, vou somente excluir ela da pasta de XML´s em processamento
                                                Functions.DeletarArquivo(strArquivoNFeProc);
                                            }

                                            break;

                                        case "301":
                                            //Ler o XML para pegar a data de emissão para criar a psta dos XML´s Denegados
                                            if (File.Exists(strArquivoNFe))
                                            {
                                                oLerXml.Nfe(strArquivoNFe);

                                                //Move a NFE da pasta de NFE em processamento para NFe Denegadas
                                                if (!oAux.EstaDenegada(strArquivoNFe, oLerXml.oDadosNfe.dEmi))
                                                {
                                                    MoverArquivo(strArquivoNFe, PastaEnviados.Denegados, oLerXml.oDadosNfe.dEmi);
                                                }
                                            }

                                            break;

                                        case "302":
                                            goto case "301";

                                        case "303":
                                            goto case "301";

                                        case "304":
                                            goto case "301";

                                        case "305":
                                            goto case "301";

                                        case "306":
                                            goto case "301";

                                        case "110": //Uso Denegado
                                            goto case "301";

                                        default:
                                            //Mover o XML da NFE a pasta de XML´s com erro
                                            oAux.MoveArqErro(strArquivoNFe);
                                            break;
                                    }

                                    //Deletar a NFE do arquivo de controle de fluxo
                                    oFluxoNfe.ExcluirNfeFluxo(strChaveNFe);
                                }
                            }
                            break;
                        #endregion

                        #region Nota fiscal cancelada
                        case "101": //Cancelamento Homologado ou Nfe Cancelada
                            goto case "100";
                        #endregion

                        #region Nota fiscal Denegada
                        case "110": //NFe Denegada
                            goto case "100";

                        case "301": //NFe Denegada
                            goto case "100";

                        case "302": //NFe Denegada
                            goto case "100";

                        case "303": //NFe Denegada
                            goto case "100";

                        case "304": //NFe Denegada
                            goto case "100";

                        case "305": //NFe Denegada
                            goto case "100";

                        case "306": //NFe Denegada
                            goto case "100";
                        #endregion

                        #region Conteúdo para retirar a nota fiscal do fluxo
                        case "TirarFluxo":
                            //Mover o XML da NFE a pasta de XML´s com erro
                            oAux.MoveArqErro(strArquivoNFe);

                            //Deletar a NFE do arquivo de controle de fluxo
                            oFluxoNfe.ExcluirNfeFluxo(strChaveNFe);
                            break;
                        #endregion

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
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
            try
            {
                oGerarXML.LoteNfe(Arquivo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
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
            try
            {
                oGerarXML.LoteNfe(lstArquivoNfe);
            }
            catch (Exception ex)
            {
                throw (ex);
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
        /// 
#if notused
        public override void Recepcao()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
            //Definir o serviço que será executado para a classe
            Servico = Servicos.EnviarLoteNfe;

            try
            {
                FluxoNfe oFluxoNfe = new FluxoNfe();
                LerXML oLer = new LerXML();

                #region Parte que envia o lote
                //Ler o XML de Lote para pegar o número do lote que está sendo enviado
                oLer.Nfe(NomeArquivoXML);

                var idLote = oLer.oDadosNfe.idLote;

                //Definir o objeto do WebService
                WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servicos.EnviarLoteNfe, emp, Convert.ToInt32(oLer.oDadosNfe.cUF), Convert.ToInt32(oLer.oDadosNfe.tpAmb), Convert.ToInt32(oLer.oDadosNfe.tpEmis));

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                object oRecepcao = wsProxy.CriarObjeto(NomeClasseWS(Servico, Convert.ToInt32(oLer.oDadosNfe.cUF)));
                var oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS());

                //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                wsProxy.SetProp(oCabecMsg, "cUF", oLer.oDadosNfe.cUF);
                wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLNFe);

                //
                //XML neste ponto a NFe já está assinada, pois foi assinada, validada e montado o lote para envio por outro serviço. 
                //Fica aqui somente este lembrete. Wandrey 16/03/2010
                //

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.Invocar(wsProxy, oRecepcao, NomeMetodoWS(Servico, Convert.ToInt32(oLer.oDadosNfe.cUF)), oCabecMsg, this, "-env-lot", "-rec");
                #endregion

                #region Parte que trata o retorno do lote, ou seja, o número do recibo
                //Ler o XML de retorno com o recibo do lote enviado
                var oLerRecibo = new LerXML();
                oLerRecibo.Recibo(vStrXmlRetorno);

                if (oLerRecibo.oDadosRec.cStat == "103") //Lote recebido com sucesso
                {
                    //Atualizar o número do recibo no XML de controle do fluxo de notas enviadas
                    oFluxoNfe.AtualizarTag(oLer.oDadosNfe.chavenfe, FluxoNfe.ElementoEditavel.tMed, oLerRecibo.oDadosRec.tMed.ToString());
                    oFluxoNfe.AtualizarTagRec(idLote, oLerRecibo.oDadosRec.nRec);
                }
                else if (Convert.ToInt32(oLerRecibo.oDadosRec.cStat) > 200 ||
                         Convert.ToInt32(oLerRecibo.oDadosRec.cStat) == 108 || //Verifica se o servidor de processamento está paralisado momentaneamente. Wandrey 13/04/2012
                         Convert.ToInt32(oLerRecibo.oDadosRec.cStat) == 109) //Verifica se o servidor de processamento está paralisado sem previsão. Wandrey 13/04/2012              
                {
                    //Se o status do retorno do lote for maior que 200 ou for igual a 108 ou 109, 
                    //vamos ter que excluir a nota do fluxo, porque ela foi rejeitada pelo SEFAZ
                    //Primeiro vamos mover o xml da nota da pasta EmProcessamento para pasta de XML´s com erro e depois tira ela do fluxo
                    //Wandrey 30/04/2009
                    oAux.MoveArqErro(Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + oFluxoNfe.LerTag(oLer.oDadosNfe.chavenfe, FluxoNfe.ElementoFixo.ArqNFe));
                    oFluxoNfe.ExcluirNfeFluxo(oLer.oDadosNfe.chavenfe);
                }

                //Deleta o arquivo de lote
                Functions.DeletarArquivo(NomeArquivoXML);
                #endregion
            }
            catch (ExceptionEnvioXML ex)
            {
                //Ocorreu algum erro no exato momento em que tentou enviar o XML para o SEFAZ, vou ter que tratar
                //para ver se o XML chegou lá ou não, se eu consegui pegar o número do recibo de volta ou não, etc.
                //E ver se vamos tirar o XML do Fluxo ou finalizar ele com a consulta situação da NFe

                //TODO: V3.0 - Tratar o problema de não conseguir pegar o recibo exatamente neste ponto

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.EnvLot, Propriedade.ExtRetorno.Rec_ERR, ex);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 16/03/2010
                }
            }
            catch (ExceptionSemInternet ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.EnvLot, Propriedade.ExtRetorno.Rec_ERR, ex, false);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 16/03/2010
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.EnvLot, Propriedade.ExtRetorno.Rec_ERR, ex);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 16/03/2010
                }
            }
        }
#endif
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
        /// 
#if notused
        public override void RetRecepcao()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
            //Definir o serviço que será executado para a classe
            Servico = Servicos.PedidoSituacaoLoteNFe;

            try
            {
                #region Parte do código que envia o XML de pedido de consulta do recibo
                var oLer = new LerXML();
                oLer.PedRec(NomeArquivoXML);

                //Definir o objeto do WebService
                WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servicos.PedidoSituacaoLoteNFe, emp, oLer.oDadosPedRec.cUF, oLer.oDadosPedRec.tpAmb, oLer.oDadosPedRec.tpEmis);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                var oCancelamento = wsProxy.CriarObjeto(NomeClasseWS(Servico, oLer.oDadosPedRec.cUF));
                var oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS());

                //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                wsProxy.SetProp(oCabecMsg, "cUF", oLer.oDadosPedRec.cUF.ToString());
                wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLPedRec);

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.Invocar(wsProxy, oCancelamento, NomeMetodoWS(Servico, oLer.oDadosPedRec.cUF), oCabecMsg, this);
                #endregion

                #region Parte do código que trata o XML de retorno da consulta do recibo
                //Efetuar a leituras das notas do lote para ver se foi autorizada ou não
                switch (Propriedade.TipoAplicativo)
                {
                    case TipoAplicativo.Cte:
                        LerRetornoLoteCTe();
                        break;
                    case TipoAplicativo.Nfe:
                        LerRetornoLoteNFe();
                        break;
                    default:
                        break;
                }

                //Gravar o XML retornado pelo WebService do SEFAZ na pasta de retorno para o ERP
                //Tem que ser feito neste ponto, pois somente aqui terminamos todo o processo
                //Wandrey 18/06/2009
                oGerarXML.XmlRetorno(Propriedade.ExtEnvio.PedRec_XML, Propriedade.ExtRetorno.ProRec_XML, vStrXmlRetorno);
                #endregion
            }
            catch (Exception ex)
            {
                try
                {
                    GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.PedRec_XML, Propriedade.ExtRetorno.ProRec_ERR, ex);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Pois ocorreu algum erro de rede, hd, permissão das pastas, etc. Wandrey 22/03/2010
                }
            }
            finally
            {
                //Deletar o arquivo de solicitação do serviço
                Functions.DeletarArquivo(NomeArquivoXML);
            }
        }
#endif
        #endregion

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
#if unsed
        public override void StatusServico()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            Servico = Servicos.PedidoConsultaStatusServicoNFe;

            try
            {
                //Ler o XML para pegar parâmetros de envio
                var oLer = new LerXML();
                oLer.PedSta(NomeArquivoXML);

                if (vXmlNfeDadosMsgEhXML)  //danasa 12-9-2009
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servicos.PedidoConsultaStatusServicoNFe, emp, oLer.oDadosPedSta.cUF, oLer.oDadosPedSta.tpAmb, oLer.oDadosPedSta.tpEmis);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    var oStatusServico = wsProxy.CriarObjeto(NomeClasseWS(Servico, oLer.oDadosPedSta.cUF));
                    var oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS());

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "cUF", oLer.oDadosPedSta.cUF.ToString());
                    wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLStatusServico);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oStatusServico, NomeMetodoWS(Servico, oLer.oDadosPedSta.cUF), oCabecMsg, this, "-ped-sta", "-sta");
                }
                else
                {
                    // Gerar o XML de solicitacao de situacao do servico a partir do TXT gerado pelo ERP
                    oGerarXML.StatusServico(Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml",
                                            oLer.oDadosPedSta.tpAmb,
                                            oLer.oDadosPedSta.tpEmis,
                                            oLer.oDadosPedSta.cUF);
                }
            }
            catch (Exception ex)
            {
                var extRet = vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.PedSta_XML : Propriedade.ExtEnvio.PedSta_TXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    GravarArqErroServico(NomeArquivoXML, extRet, Propriedade.ExtRetorno.Sta_ERR, ex);
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
                    //Deletar o arquivo de solicitação do serviço
                    Functions.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                    //Se falhou algo na hora de deletar o XML de solicitação do serviço, 
                    //infelizmente não posso fazer mais nada, o UniNFe vai tentar mandar 
                    //o arquivo novamente para o webservice
                    //Wandrey 09/03/2010
                }
            }
        }
#endif
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
        public override void XmlPedRec(int empresa, string nRec)
        {
            GerarXML gerarXML = new GerarXML(empresa);
            gerarXML.XmlPedRec(nRec);
        }
        #endregion

        #region Serviços DPEC

        #region RecepcaoDPEC()
        /// <summary>
        /// Envia o XML do registro do DPEC para o SCE (Sistema de Contingência Eletronica)
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 19/10/2010
        /// </remarks>
        /// 
#if notused
        public override void RecepcaoDPEC()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            Servico = Servicos.EnviarDPEC;

            try
            {
                //Ler o XML para pegar parâmetros de envio
                LerXML oLer = new LerXML();
                oLer.EnvDPEC(emp, NomeArquivoXML);    //danasa 21/10/2010

                if (vXmlNfeDadosMsgEhXML)  //danasa 12-9-2009
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servicos.EnviarDPEC, emp, oLer.dadosEnvDPEC.cUF, oLer.dadosEnvDPEC.tpAmb, oLer.dadosEnvDPEC.tpEmis);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oRecepcaoDPEC = wsProxy.CriarObjeto("SCERecepcaoRFB");
                    object oCabecMsg = wsProxy.CriarObjeto("sceCabecMsg");

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    //oWSProxy.SetProp(oCabecMsg, "cUF", oLer.dadosEnvDPEC.cUF.ToString());
                    wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLEnvDPEC);

                    //Criar objeto da classe de assinatura digita
                    AssinaturaDigital oAD = new AssinaturaDigital();

                    //Assinar o XML
                    oAD.Assinar(NomeArquivoXML, emp, Convert.ToInt32(oLer.dadosEnvDPEC.cUF));

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oRecepcaoDPEC, "sceRecepcaoDPEC", oCabecMsg, this);

                    //Ler o retorno
                    LerRetDPEC();

                    //Gravar o XML retornado pelo WebService do SEFAZ na pasta de retorno para o ERP
                    //Tem que ser feito neste ponto, pois somente aqui terminamos todo o processo
                    oGerarXML.XmlRetorno(Propriedade.ExtEnvio.EnvDPEC_XML, Propriedade.ExtRetorno.retDPEC_XML, vStrXmlRetorno);
                }
                else
                {
                    // Gerar o XML de solicitacao de situacao do servico a partir do TXT gerado pelo ERP
                    oGerarXML.EnvioDPEC(Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml", oLer.dadosEnvDPEC);
                }
            }
            catch (Exception ex)
            {
                var ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.EnvDPEC_XML : Propriedade.ExtEnvio.EnvDPEC_TXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    GravarArqErroServico(NomeArquivoXML, ExtRet, Propriedade.ExtRetorno.retDPEC_ERR, ex);
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
                    //Se falhou algo na hora de deletar o XML de cancelamento de NFe, infelizmente
                    //não posso fazer mais nada, o UniNFe vai tentar mandar o arquivo novamente para o webservice, pois ainda não foi excluido.
                    //Wandrey 09/03/2010
                }
            }

        }
#endif
        #endregion

        #region LerRetDPEC()
#if notused
        protected override void LerRetDPEC()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            XmlDocument doc = new XmlDocument();

            try
            {
                MemoryStream msXml = Functions.StringXmlToStream(this.vStrXmlRetorno);
                doc.Load(msXml);

                XmlNodeList retDPECList = doc.GetElementsByTagName("retDPEC");

                foreach (XmlNode retDPECNode in retDPECList)
                {
                    XmlElement retDPECElemento = (XmlElement)retDPECNode;

                    XmlNodeList infDPECRegList = retDPECElemento.GetElementsByTagName("infDPECReg");

                    foreach (XmlNode infDPECRegNode in infDPECRegList)
                    {
                        XmlElement infDPECRegElemento = (XmlElement)infDPECRegNode;

                        if (infDPECRegElemento.GetElementsByTagName("cStat")[0].InnerText == "124" ||
                            infDPECRegElemento.GetElementsByTagName("cStat")[0].InnerText == "125") //DPEC Homologado
                        {
                            string cChaveNFe = infDPECRegElemento.GetElementsByTagName("chNFe")[0].InnerText;
                            string dhRegDPEC = infDPECRegElemento.GetElementsByTagName("dhRegDPEC")[0].InnerText;
                            DateTime dtEmissaoDPEC = new DateTime(Convert.ToInt16(dhRegDPEC.Substring(0, 4)), Convert.ToInt16(dhRegDPEC.Substring(5, 2)), Convert.ToInt16(dhRegDPEC.Substring(8, 2)));

                            //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                            MoverArquivo(NomeArquivoXML, PastaEnviados.Autorizados, dtEmissaoDPEC);

                            //Gravar o XML retornado pelo WebService do SEFAZ na pasta de autorizados. Wandrey 25/11/2010
                            string nomePastaEnviado = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.Autorizados.ToString() + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(dtEmissaoDPEC);
                            oGerarXML.XmlRetorno(Propriedade.ExtEnvio.EnvDPEC_XML, Propriedade.ExtRetorno.retDPEC_XML, vStrXmlRetorno, nomePastaEnviado);
                        }
                        else
                        {
                            //Deletar o arquivo de solicitação do serviço da pasta de envio
                            Functions.DeletarArquivo(NomeArquivoXML);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
#endif
        #endregion

        #region ConsultaDPEC()
        /// <summary>
        /// Envia o XML de consulta do registro do DPEC para o SCE (Sistema de Contingência Eletronica)
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 19/10/2010
        /// </remarks>
        /// 
#if notused
        public override void ConsultaDPEC()
        {
            //           throw new NotImplementedException();
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            Servico = Servicos.ConsultarDPEC;

            try
            {
                //Ler o XML para pegar parâmetros de envio
                LerXML oLer = new LerXML();
                oLer.ConsDPEC(emp, NomeArquivoXML);

                if (vXmlNfeDadosMsgEhXML)  //danasa 12-9-2009
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servicos.ConsultarDPEC, emp, 0, oLer.dadosConsDPEC.tpAmb, oLer.dadosConsDPEC.tpEmis);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oRecepcaoDPEC = wsProxy.CriarObjeto("SCEConsultaRFB");
                    object oCabecMsg = wsProxy.CriarObjeto("sceCabecMsg");

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLConsDPEC);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oRecepcaoDPEC, "sceConsultaDPEC", oCabecMsg, this);

                    //Ler o retorno
                    LerRetConsDPEC(emp);

                    //Gravar o XML retornado pelo WebService do SEFAZ na pasta de retorno para o ERP
                    //Tem que ser feito neste ponto, pois somente aqui terminamos todo o processo
                    oGerarXML.XmlRetorno(Propriedade.ExtEnvio.ConsDPEC_XML, Propriedade.ExtRetorno.retConsDPEC_XML, vStrXmlRetorno);
                }
                else
                {
                    // Gerar o XML de solicitacao de situacao do servico a partir do TXT gerado pelo ERP
                    oGerarXML.ConsultaDPEC(Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml", oLer.dadosConsDPEC);
                }
            }
            catch (Exception ex)
            {
                var ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.ConsDPEC_XML : Propriedade.ExtEnvio.ConsDPEC_TXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    GravarArqErroServico(NomeArquivoXML, ExtRet, Propriedade.ExtRetorno.retConsDPEC_ERR, ex);
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
                    //Se falhou algo na hora de deletar o XML de cancelamento de NFe, infelizmente
                    //não posso fazer mais nada, o UniNFe vai tentar mandar o arquivo novamente para o webservice, pois ainda não foi excluido.
                    //Wandrey 09/03/2010
                }
            }
        }

        private void LerRetConsDPEC(int emp)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                MemoryStream msXml = Functions.StringXmlToStream(this.vStrXmlRetorno);
                doc.Load(msXml);

                XmlNodeList retDPECList = doc.GetElementsByTagName("retConsDPEC");

                foreach (XmlNode retDPECNode in retDPECList)
                {
                    XmlElement retDPECElemento = (XmlElement)retDPECNode;

                    XmlNodeList infDPECRegList = retDPECElemento.GetElementsByTagName("infDPECReg");

                    foreach (XmlNode infDPECRegNode in infDPECRegList)
                    {
                        XmlElement infDPECRegElemento = (XmlElement)infDPECRegNode;

                        if (infDPECRegElemento.GetElementsByTagName("cStat")[0].InnerText == "124" ||
                            infDPECRegElemento.GetElementsByTagName("cStat")[0].InnerText == "125") //DPEC Homologado
                        {
                            //string cChaveNFe = infDPECRegElemento.GetElementsByTagName("chNFe")[0].InnerText;
                            string dhRegDPEC = infDPECRegElemento.GetElementsByTagName("dhRegDPEC")[0].InnerText;
                            DateTime dtEmissaoDPEC = new DateTime(Convert.ToInt16(dhRegDPEC.Substring(0, 4)), Convert.ToInt16(dhRegDPEC.Substring(5, 2)), Convert.ToInt16(dhRegDPEC.Substring(8, 2)));

                            //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                            string arqEnvDpec = Empresa.Configuracoes[emp].PastaEnvio + "\\" + Functions/*oAux*/.ExtrairNomeArq(NomeArquivoXML, Propriedade.ExtEnvio.ConsDPEC_XML) + Propriedade.ExtEnvio.EnvDPEC_XML;
                            if (File.Exists(arqEnvDpec))
                            {
                                MoverArquivo(arqEnvDpec, PastaEnviados.Autorizados, dtEmissaoDPEC);
                            }

                            //Gravar o XML retornado pelo WebService do SEFAZ na pasta de autorizados. Wandrey 25/11/2010
                            string nomePastaEnviado = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.Autorizados.ToString() + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(dtEmissaoDPEC);
                            oGerarXML.XmlRetorno(Propriedade.ExtEnvio.ConsDPEC_XML, Propriedade.ExtRetorno.retConsDPEC_XML, vStrXmlRetorno, nomePastaEnviado);
                        }
                        else
                        {
                            //Deletar o arquivo de solicitação do serviço da pasta de envio
                            Functions.DeletarArquivo(NomeArquivoXML);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
#endif
        #endregion

        #endregion



#if unsed
        #region Servicos Envio

        /// <summary>
        /// RecepcaoEvento
        /// </summary>
        public override void RecepcaoEvento()   //<<<danasa 6-2011
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            if (NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.EnvCCe_XML) || NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.EnvCCe_TXT))
                Servico = Servicos.EnviarCCe;
            else
                if (NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.EnvManifestacao_XML) || NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.EnvManifestacao_TXT))
                    Servico = Servicos.EnviarManifestacao;
                else
                    if (NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.EnvCancelamento_XML) || NomeArquivoXML.ToLower().EndsWith(Propriedade.ExtEnvio.EnvCancelamento_TXT))
                        Servico = Servicos.EnviarEventoCancelamento;

            try
            {
                //Ler o XML para pegar parâmetros de envio
                LerXML oLer = new LerXML();
                oLer.EnvEvento(emp, NomeArquivoXML);

                if (vXmlNfeDadosMsgEhXML)
                {
                    int currentEvento = Convert.ToInt32(oLer.oDadosEnvEvento.eventos[0].tpEvento);
                    foreach (Evento item in oLer.oDadosEnvEvento.eventos)
                        if (currentEvento != Convert.ToInt32(item.tpEvento))
                            throw new Exception(string.Format("Não é possivel mesclar tipos de eventos dentro de um mesmo xml de eventos. O tipo de evento neste xml é {0}", currentEvento));

                    int cOrgao = oLer.oDadosEnvEvento.eventos[0].cOrgao;
                    if (cOrgao == 90 || cOrgao == 91)   //Amb.Nacional
                        cOrgao = Convert.ToInt32(oLer.oDadosEnvEvento.eventos[0].chNFe.Substring(0, 2));//<<< 7/2012

                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(
                        Servico,
                        emp,
                        cOrgao,
                        oLer.oDadosEnvEvento.eventos[0].tpAmb,
                        1);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oRecepcaoEvento;
                    if (oLer.oDadosEnvEvento.eventos[0].cOrgao == 52)
                    {
                        oRecepcaoEvento = wsProxy.CriarObjeto("NfeRecepcaoEvento");
                    }
                    else
                    {
                        oRecepcaoEvento = wsProxy.CriarObjeto("RecepcaoEvento");
                    }
                    object oCabecMsg = wsProxy.CriarObjeto(NomeClasseCabecWS());

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "cUF", cOrgao.ToString());
                    switch (Servico)
                    {
                        case Servicos.EnviarCCe:
                            wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLEnvCCe);
                            break;
                        case Servicos.EnviarEventoCancelamento:
                            wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLEnvCancelamento);
                            break;
                        default:
                            wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLEnvManifestacao);
                            break;
                    }

                    //Criar objeto da classe de assinatura digital
                    AssinaturaDigital oAD = new AssinaturaDigital();

                    //Assinar o XML
                    oAD.Assinar(NomeArquivoXML, emp, cOrgao);//Convert.ToInt32(oLer.oDadosEnvEvento.eventos[0].cOrgao));

                    //Invocar o método que envia o XML para o SEFAZ
                    string xmlExtEnvio = string.Empty;
                    string xmlExtRetorno = string.Empty;
                    switch (Servico)
                    {
                        case Servicos.EnviarCCe:
                            xmlExtEnvio = Propriedade.ExtEnvio.EnvCCe_XML.Replace(".xml", "");
                            xmlExtRetorno = Propriedade.ExtRetorno.retEnvCCe_XML.Replace(".xml", "");
                            break;
                        case Servicos.EnviarEventoCancelamento:
                            xmlExtEnvio = Propriedade.ExtEnvio.EnvCancelamento_XML.Replace(".xml", "");
                            xmlExtRetorno = Propriedade.ExtRetorno.retCancelamento_XML.Replace(".xml", "");
                            break;
                        default:
                            xmlExtEnvio = Propriedade.ExtEnvio.EnvManifestacao_XML.Replace(".xml", "");
                            xmlExtRetorno = Propriedade.ExtRetorno.retManifestacao_XML.Replace(".xml", "");
                            break;
                    }
                    oInvocarObj.Invocar(wsProxy, oRecepcaoEvento, "nfeRecepcaoEvento", oCabecMsg, this, xmlExtEnvio, xmlExtRetorno);

                    //Ler o retorno
                    if (Servico == Servicos.EnviarManifestacao)
                        LerRetornoManifestacao(emp);
                    else
                        LerRetornoEvento(emp);
                }
                else
                {
                    // Gerar o XML de eventos a partir do TXT gerado pelo ERP
                    string xmlFileExt = string.Empty;
                    string xmlFileExtTXT = string.Empty;
                    switch (Servico)
                    {
                        case Servicos.EnviarCCe:
                            xmlFileExt = Propriedade.ExtEnvio.EnvCCe_XML;
                            xmlFileExtTXT = Propriedade.ExtEnvio.EnvCCe_TXT;
                            break;
                        case Servicos.EnviarEventoCancelamento:
                            xmlFileExt = Propriedade.ExtEnvio.EnvCancelamento_XML;
                            xmlFileExtTXT = Propriedade.ExtEnvio.EnvCancelamento_TXT;
                            break;
                        default:
                            xmlFileExt = Propriedade.ExtEnvio.EnvManifestacao_XML;
                            xmlFileExtTXT = Propriedade.ExtEnvio.EnvManifestacao_TXT;
                            break;
                    }
                    oGerarXML.EnvioEvento(Functions.ExtrairNomeArq(NomeArquivoXML, xmlFileExtTXT) + xmlFileExt, oLer.oDadosEnvEvento);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    string ExtRet = string.Empty;
                    string ExtRetorno = string.Empty;

                    switch (Servico)
                    {
                        case Servicos.EnviarCCe:
                            ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.EnvCCe_XML : Propriedade.ExtEnvio.EnvCCe_TXT;
                            ExtRetorno = Propriedade.ExtRetorno.retEnvCCe_ERR;
                            break;
                        case Servicos.EnviarEventoCancelamento:
                            ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.EnvCancelamento_XML : Propriedade.ExtEnvio.EnvCancelamento_TXT;
                            ExtRetorno = Propriedade.ExtRetorno.retCancelamento_ERR;
                            break;
                        default:
                            ExtRet = vXmlNfeDadosMsgEhXML ? Propriedade.ExtEnvio.EnvManifestacao_XML : Propriedade.ExtEnvio.EnvManifestacao_TXT;
                            ExtRetorno = Propriedade.ExtRetorno.retManifestacao_ERR;
                            break;
                    }
                    if (ExtRetorno != string.Empty)
                        GravarArqErroServico(NomeArquivoXML, ExtRet, ExtRetorno, ex);
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
                    //Se falhou algo na hora de deletar o XML de evento, infelizmente
                    //não posso fazer mais nada, o UniNFe vai tentar mandar o arquivo novamente para o webservice, pois ainda não foi excluido.
                    //Wandrey 09/03/2010
                }
            }
        }

        protected override void LerRetornoEvento(int emp)
        {
            try
            {
                //
                //<<<danasa 6-2011
                //<<<UTF8 -> tem acentuacao no retorno
                TextReader txt = new StreamReader(NomeArquivoXML, Encoding.Default);
                XmlDocument docEventoOriginal = new XmlDocument();
                docEventoOriginal.Load(Functions.StringXmlToStreamUTF8(txt.ReadToEnd()));
                txt.Close();

                MemoryStream msXml = Functions.StringXmlToStream(this.vStrXmlRetorno);
                XmlDocument doc = new XmlDocument();
                doc.Load(msXml);

                XmlNodeList retEnvRetornoList = doc.GetElementsByTagName("retEnvEvento");

                foreach (XmlNode retConsSitNode in retEnvRetornoList)
                {
                    XmlElement retConsSitElemento = (XmlElement)retConsSitNode;

                    //Pegar o status de retorno da NFe que está sendo consultada a situação
                    var cStatCons = string.Empty;
                    if (retConsSitElemento.GetElementsByTagName("cStat")[0] != null)
                    {
                        cStatCons = retConsSitElemento.GetElementsByTagName("cStat")[0].InnerText;
                    }
                    switch (cStatCons)
                    {
                        case "128": //Lote de Evento Processado
                            {
#if structRetorno
                                <retEnvEvento versao="1.00" xmlns="http://www.portalfiscal.inf.br/nfe">
                                    <idLote>000000000015256</idLote>
                                    <tpAmb>2</tpAmb>
                                    <verAplic>SP_EVENTOS_PL_100</verAplic><
                                    cOrgao>35</cOrgao>
                                    <cStat>128</cStat>
                                    <xMotivo>Lote de Evento Processado</xMotivo>
                                    <retEvento versao="1.00">
                                        <infEvento>
                                            <tpAmb>2</tpAmb>
                                            <verAplic>SP_EVENTOS_PL_100</verAplic>
                                            <cOrgao>35</cOrgao>
                                            <cStat>135</cStat>
                                            <xMotivo>Evento registrado e vinculado a NF-e</xMotivo>
                                            <chNFe>35110610238568000107550010000121751000137350</chNFe>
                                            <tpEvento>110110</tpEvento>
                                            <xEvento>Carta de Correção registrada</xEvento>
                                            <nSeqEvento>3</nSeqEvento>
                                            <CNPJDest>99999999000191</CNPJDest>
                                            <dhRegEvento>2011-06-09T04:00:16-03:00</dhRegEvento>
                                            <nProt>135110004500940</nProt>
                                        </infEvento>
                                    </retEvento>
                                    <retEvento versao="1.00">
                                        <infEvento>
                                            <tpAmb>2</tpAmb>
                                            <verAplic>SP_EVENTOS_PL_100</verAplic>
                                            <cOrgao>35</cOrgao>
                                            <cStat>135</cStat>
                                            <xMotivo>Evento registrado e vinculado a NF-e</xMotivo>
                                            <chNFe>35110610238568000107550010000121751000137350</chNFe>
                                            <tpEvento>110110</tpEvento>
                                            <xEvento>Carta de Correção registrada</xEvento>
                                            <nSeqEvento>4</nSeqEvento>
                                            <CNPJDest>99999999000191</CNPJDest>
                                            <dhRegEvento>2011-06-09T04:00:16-03:00</dhRegEvento>
                                            <nProt>135110004500941</nProt>
                                        </infEvento>
                                    </retEvento>
                                </retEnvEvento>
#endif
                                XmlNodeList envEventosList = doc.GetElementsByTagName("retEvento");
                                for (int i = 0; i < envEventosList.Count; ++i)
                                {
                                    XmlElement eleRetorno = envEventosList.Item(i) as XmlElement;
                                    cStatCons = eleRetorno.GetElementsByTagName("cStat")[0].InnerText;
                                    if (cStatCons == "135" || cStatCons == "136")
                                    {
                                        string chNFe = eleRetorno.GetElementsByTagName("chNFe")[0].InnerText;
                                        Int32 nSeqEvento = Convert.ToInt32("0" + eleRetorno.GetElementsByTagName("nSeqEvento")[0].InnerText);
                                        string tpEvento = eleRetorno.GetElementsByTagName("tpEvento")[0].InnerText;
                                        string Id = "ID" + tpEvento + chNFe + nSeqEvento.ToString("00");
                                        ///
                                        ///procura no Xml de envio pelo Id retornado
                                        ///nao sei se a Sefaz retorna na ordem em que foi enviado, então é melhor pesquisar
                                        foreach (XmlNode env in docEventoOriginal.GetElementsByTagName("infEvento"))
                                        {
                                            string Idd = env.Attributes.GetNamedItem("Id").Value;
                                            if (Idd == Id)
                                            {
                                                DateTime dhRegEvento = Convert.ToDateTime(eleRetorno.GetElementsByTagName("dhRegEvento")[0].InnerText);

                                                if (Empresa.Configuracoes[emp].DiretorioSalvarComo == "AM")
                                                    dhRegEvento = new DateTime(Convert.ToInt16("20" + chNFe.Substring(2, 2)), Convert.ToInt16(chNFe.Substring(4, 2)), 1);

                                                //Gerar o arquivo XML de distribuição da CCe
                                                oGerarXML.XmlDistEvento(emp, chNFe, nSeqEvento, Convert.ToInt32(tpEvento), env.ParentNode.OuterXml, eleRetorno.OuterXml, dhRegEvento);

                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

        #region RecepcaoManifestacao
        public override void RecepcaoManifestacao()
        {
        }
        protected override void LerRetornoManifestacao(int emp)
        {
            LerRetornoEvento(emp);
        }
        #endregion
#endif


        #region Métodos dos serviços da NFS-e (Nota Fiscal de Serviços Eletrônica)

        #region RecepcionarLoteRps()
        /// <summary>
        /// Enviar o Lote de RPS (NFSe) para o Municipio
        /// </summary>
        public override void RecepcionarLoteRps()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            Servico = Servicos.RecepcionarLoteRps;

            try
            {
                //Ler o XML para pegar parâmetros de envio
                LerXML ler = new LerXML();
                ler.EnvLoteRps(NomeArquivoXML);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                WebServiceProxy wsProxy = null;
                object envLoteRps = null;
                string cabecMsg = "";
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(ler.oDadosPedSitLoteRps.cMunicipio);
                switch (padraoNFSe)
                {
                    case PadroesNFSe.GINFES:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosEnvLoteRps.cMunicipio, ler.oDadosEnvLoteRps.tpAmb, ler.oDadosEnvLoteRps.tpEmis);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosEnvLoteRps.cMunicipio));
                        cabecMsg = "<ns2:cabecalho versao=\"3\" xmlns:ns2=\"http://www.ginfes.com.br/cabecalho_v03.xsd\"><versaoDados>3</versaoDados></ns2:cabecalho>";
                        break;

                    case PadroesNFSe.BETHA:
                        wsProxy = new WebServiceProxy(Empresa.Configuracoes[emp].X509Certificado);
                        break;

                    case PadroesNFSe.THEMA:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosEnvLoteRps.cMunicipio, ler.oDadosEnvLoteRps.tpAmb, ler.oDadosEnvLoteRps.tpEmis);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosEnvLoteRps.cMunicipio));
                        break;

                    case PadroesNFSe.SALVADOR_BA:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosEnvLoteRps.cMunicipio, ler.oDadosEnvLoteRps.tpAmb, ler.oDadosEnvLoteRps.tpEmis);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosEnvLoteRps.cMunicipio));
                        break;

                    case PadroesNFSe.CANOAS_RS:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosEnvLoteRps.cMunicipio, ler.oDadosEnvLoteRps.tpAmb, ler.oDadosEnvLoteRps.tpEmis);
                        envLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosEnvLoteRps.cMunicipio));
                        cabecMsg = "<cabecalho versao=\"201001\"><versaoDados>V2010</versaoDados></cabecalho>";
                        break;

                    default:
                        throw new Exception("Não foi possível detectar o padrão da NFS-e.");
                }


                //Assinar o XML
                AssinaturaDigital ad = new AssinaturaDigital();
                ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(ler.oDadosEnvLoteRps.cMunicipio));

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.InvocarNFSe(wsProxy, envLoteRps, NomeMetodoWS(Servico, ler.oDadosEnvLoteRps.cMunicipio), cabecMsg, this, "-env-loterps", "-ret-loterps", padraoNFSe);
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.EnvLoteRps, Propriedade.ExtRetorno.RetLoteRps_ERR, ex);
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

        #region ConsultarSituacaoLoteRps()
        /// <summary>
        /// Consultar a situação do Lote de RPS 
        /// </summary>
        public override void ConsultarSituacaoLoteRps()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            Servico = Servicos.ConsultarSituacaoLoteRps;

            try
            {
                //Ler o XML para pegar parâmetros de envio
                LerXML ler = new LerXML();
                ler.PedSitLoteRps(NomeArquivoXML);

                //Definir o objeto do WebService

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                WebServiceProxy wsProxy = null;
                object pedSitLoteRps = null;
                string cabecMsg = "";
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(ler.oDadosPedSitLoteRps.cMunicipio);
                switch (padraoNFSe)
                {
                    case PadroesNFSe.GINFES:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosPedSitLoteRps.cMunicipio, ler.oDadosPedSitLoteRps.tpAmb, ler.oDadosPedSitLoteRps.tpEmis);
                        pedSitLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosPedSitLoteRps.cMunicipio));
                        cabecMsg = "<ns2:cabecalho versao=\"3\" xmlns:ns2=\"http://www.ginfes.com.br/cabecalho_v03.xsd\"><versaoDados>3</versaoDados></ns2:cabecalho>";
                        break;

                    case PadroesNFSe.BETHA:
                        wsProxy = new WebServiceProxy(Empresa.Configuracoes[emp].X509Certificado);
                        break;

                    case PadroesNFSe.THEMA:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosPedSitLoteRps.cMunicipio, ler.oDadosPedSitLoteRps.tpAmb, ler.oDadosPedSitLoteRps.tpEmis);
                        pedSitLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosPedSitLoteRps.cMunicipio));
                        break;

                    case PadroesNFSe.CANOAS_RS:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosPedSitLoteRps.cMunicipio, ler.oDadosPedSitLoteRps.tpAmb, ler.oDadosPedSitLoteRps.tpEmis);
                        pedSitLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosPedSitLoteRps.cMunicipio));
                        cabecMsg = "<cabecalho versao=\"201001\"><versaoDados>V2010</versaoDados></cabecalho>";
                        break;

                    default:
                        throw new Exception("Não foi possível detectar o padrão da NFS-e.");
                }

                //Assinar o XML
                AssinaturaDigital ad = new AssinaturaDigital();
                ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(ler.oDadosPedSitLoteRps.cMunicipio));

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.InvocarNFSe(wsProxy, pedSitLoteRps, NomeMetodoWS(Servico, ler.oDadosPedSitLoteRps.cMunicipio), cabecMsg, this, "-ped-sitloterps", "-sitloterps", padraoNFSe);
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.PedSitLoteRps, Propriedade.ExtRetorno.SitLoteRps_ERR, ex);
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

        #region ConsultarNfsePorRps()
        /// <summary>
        /// Consultar uma NFS-e por RPS
        /// </summary>
        public override void ConsultarNfsePorRps()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            Servico = Servicos.ConsultarNfsePorRps;

            try
            {
                //Ler o XML para pegar parâmetros de envio
                LerXML ler = new LerXML();
                ler.PedSitNfseRps(NomeArquivoXML);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                WebServiceProxy wsProxy = null;
                object pedLoteRps = null;
                string cabecMsg = "";
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(ler.oDadosPedSitNfseRps.cMunicipio);
                switch (padraoNFSe)
                {
                    case PadroesNFSe.GINFES:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosPedSitNfseRps.cMunicipio, ler.oDadosPedSitNfseRps.tpAmb, ler.oDadosPedSitNfseRps.tpEmis);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosPedSitNfseRps.cMunicipio));
                        cabecMsg = "<ns2:cabecalho versao=\"3\" xmlns:ns2=\"http://www.ginfes.com.br/cabecalho_v03.xsd\"><versaoDados>3</versaoDados></ns2:cabecalho>";
                        break;

                    case PadroesNFSe.BETHA:
                        wsProxy = new WebServiceProxy(Empresa.Configuracoes[emp].X509Certificado);
                        break;

                    case PadroesNFSe.THEMA:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosPedSitNfseRps.cMunicipio, ler.oDadosPedSitNfseRps.tpAmb, ler.oDadosPedSitNfseRps.tpEmis);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosPedSitNfseRps.cMunicipio));
                        break;

                    case PadroesNFSe.CANOAS_RS:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosPedSitNfseRps.cMunicipio, ler.oDadosPedSitNfseRps.tpAmb, ler.oDadosPedSitNfseRps.tpEmis);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosPedSitNfseRps.cMunicipio));
                        cabecMsg = "<cabecalho versao=\"201001\"><versaoDados>V2010</versaoDados></cabecalho>";
                        break;

                    default:
                        throw new Exception("Não foi possível detectar o padrão da NFS-e.");
                }

                //Assinar o XML
                AssinaturaDigital ad = new AssinaturaDigital();
                ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(ler.oDadosPedSitNfseRps.cMunicipio));

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.InvocarNFSe(wsProxy, pedLoteRps, NomeMetodoWS(Servico, ler.oDadosPedSitNfseRps.cMunicipio), cabecMsg, this, "-ped-sitnfserps", "-sitnfserps", padraoNFSe);
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.PedSitNfseRps, Propriedade.ExtRetorno.SitNfseRps_ERR, ex);
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

        #region ConsultarNfse()
        /// <summary>
        /// Consultar uma NFS-e por período
        /// </summary>
        public override void ConsultarNfse()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            Servico = Servicos.ConsultarNfse;

            try
            {
                //Ler o XML para pegar parâmetros de envio
                LerXML ler = new LerXML();
                ler.PedSitNfse(NomeArquivoXML);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                WebServiceProxy wsProxy = null;
                object pedLoteRps = null;
                string cabecMsg = "";
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(ler.oDadosPedSitNfse.cMunicipio);
                switch (padraoNFSe)
                {
                    case PadroesNFSe.GINFES:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosPedSitNfse.cMunicipio, ler.oDadosPedSitNfse.tpAmb, ler.oDadosPedSitNfse.tpEmis);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosPedSitNfse.cMunicipio));
                        cabecMsg = "<ns2:cabecalho versao=\"3\" xmlns:ns2=\"http://www.ginfes.com.br/cabecalho_v03.xsd\"><versaoDados>3</versaoDados></ns2:cabecalho>";
                        break;

                    case PadroesNFSe.BETHA:
                        wsProxy = new WebServiceProxy(Empresa.Configuracoes[emp].X509Certificado);
                        break;

                    case PadroesNFSe.THEMA:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosPedSitNfse.cMunicipio, ler.oDadosPedSitNfse.tpAmb, ler.oDadosPedSitNfse.tpEmis);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosPedSitNfse.cMunicipio));
                        break;

                    case PadroesNFSe.CANOAS_RS:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosPedSitNfse.cMunicipio, ler.oDadosPedSitNfse.tpAmb, ler.oDadosPedSitNfse.tpEmis);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosPedSitNfse.cMunicipio));
                        cabecMsg = "<cabecalho versao=\"201001\"><versaoDados>V2010</versaoDados></cabecalho>";
                        break;

                    default:
                        throw new Exception("Não foi possível detectar o padrão da NFS-e.");
                }

                //Assinar o XML
                AssinaturaDigital ad = new AssinaturaDigital();
                ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(ler.oDadosPedSitNfse.cMunicipio));

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.InvocarNFSe(wsProxy, pedLoteRps, NomeMetodoWS(Servico, ler.oDadosPedSitNfse.cMunicipio), cabecMsg, this, "-ped-sitnfse", "-sitnfse", padraoNFSe);
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.PedSitNfse, Propriedade.ExtRetorno.SitNfse_ERR, ex);
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

        #region ConsultarLoteRps()
        /// <summary>
        /// Consultar um Lote de RPS
        /// </summary>
        public override void ConsultarLoteRps()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            Servico = Servicos.ConsultarLoteRps;

            try
            {
                //Ler o XML para pegar parâmetros de envio
                LerXML ler = new LerXML();
                ler.PedLoteRps(NomeArquivoXML);


                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                WebServiceProxy wsProxy = null;
                object pedLoteRps = null;
                string cabecMsg = "";
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(ler.oDadosPedSitLoteRps.cMunicipio);
                switch (padraoNFSe)
                {
                    case PadroesNFSe.GINFES:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosPedLoteRps.cMunicipio, ler.oDadosPedLoteRps.tpAmb, ler.oDadosPedLoteRps.tpEmis);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosPedLoteRps.cMunicipio));
                        cabecMsg = "<ns2:cabecalho versao=\"3\" xmlns:ns2=\"http://www.ginfes.com.br/cabecalho_v03.xsd\"><versaoDados>3</versaoDados></ns2:cabecalho>";
                        break;

                    case PadroesNFSe.BETHA:
                        wsProxy = new WebServiceProxy(Empresa.Configuracoes[emp].X509Certificado);
                        break;

                    case PadroesNFSe.THEMA:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosPedLoteRps.cMunicipio, ler.oDadosPedLoteRps.tpAmb, ler.oDadosPedLoteRps.tpEmis);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosPedLoteRps.cMunicipio));
                        break;

                    case PadroesNFSe.CANOAS_RS:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosPedLoteRps.cMunicipio, ler.oDadosPedLoteRps.tpAmb, ler.oDadosPedLoteRps.tpEmis);
                        pedLoteRps = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosPedLoteRps.cMunicipio));
                        cabecMsg = "<cabecalho versao=\"201001\"><versaoDados>V2010</versaoDados></cabecalho>";
                        break;

                    default:
                        throw new Exception("Não foi possível detectar o padrão da NFS-e.");
                }

                //Assinar o XML
                AssinaturaDigital ad = new AssinaturaDigital();
                ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(ler.oDadosPedLoteRps.cMunicipio));

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.InvocarNFSe(wsProxy, pedLoteRps, NomeMetodoWS(Servico, ler.oDadosPedLoteRps.cMunicipio), cabecMsg, this, "-ped-loterps", "-loterps", padraoNFSe);
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.PedLoteRps, Propriedade.ExtRetorno.LoteRps_ERR, ex);
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

        #region CancelarNfse()
        /// <summary>
        /// Cancelar uma NFS-e
        /// </summary>
        public override void CancelarNfse()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            Servico = Servicos.CancelarNfse;

            try
            {
                //Ler o XML para pegar parâmetros de envio
                LerXML ler = new LerXML();
                ler.PedCanNfse(NomeArquivoXML);

                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                WebServiceProxy wsProxy = null;
                object pedCanNfse = null;
                string cabecMsg = "";
                PadroesNFSe padraoNFSe = Functions.PadraoNFSe(ler.oDadosPedSitLoteRps.cMunicipio);
                switch (padraoNFSe)
                {
                    case PadroesNFSe.GINFES:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosPedCanNfse.cMunicipio, ler.oDadosPedCanNfse.tpAmb, ler.oDadosPedCanNfse.tpEmis);
                        pedCanNfse = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosPedCanNfse.cMunicipio));
                        cabecMsg = ""; //Cancelamento ainda tá na versão 2.0 então não tem o cabecMsg
                        break;

                    case PadroesNFSe.BETHA:
                        wsProxy = new WebServiceProxy(Empresa.Configuracoes[emp].X509Certificado);
                        break;

                    case PadroesNFSe.THEMA:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosPedCanNfse.cMunicipio, ler.oDadosPedCanNfse.tpAmb, ler.oDadosPedCanNfse.tpEmis);
                        pedCanNfse = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosPedCanNfse.cMunicipio));
                        break;

                    case PadroesNFSe.CANOAS_RS:
                        wsProxy = ConfiguracaoApp.DefinirWS(Servico, emp, ler.oDadosPedCanNfse.cMunicipio, ler.oDadosPedCanNfse.tpAmb, ler.oDadosPedCanNfse.tpEmis);
                        pedCanNfse = wsProxy.CriarObjeto(NomeClasseWS(Servico, ler.oDadosPedCanNfse.cMunicipio));
                        cabecMsg = "<cabecalho versao=\"201001\"><versaoDados>V2010</versaoDados></cabecalho>";
                        break;

                    default:
                        throw new Exception("Não foi possível detectar o padrão da NFS-e.");
                }

                //Assinar o XML
                AssinaturaDigital ad = new AssinaturaDigital();
                ad.Assinar(NomeArquivoXML, emp, Convert.ToInt32(ler.oDadosPedCanNfse.cMunicipio));

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.InvocarNFSe(wsProxy, pedCanNfse, NomeMetodoWS(Servico, ler.oDadosPedCanNfse.cMunicipio), cabecMsg, this, "-ped-cannfse", "-cannfse", padraoNFSe);
            }
            catch (Exception ex)
            {
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    GravarArqErroServico(NomeArquivoXML, Propriedade.ExtEnvio.PedCanNfse, Propriedade.ExtRetorno.CanNfse_ERR, ex);
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

        #endregion

        #region Métodos para definição dos nomes das classes e métodos da NFe, CTe e NFSe

        #region NomeClasseWS()
        /// <summary>
        /// Retorna o nome da classe do serviço passado por parâmetro do WebService do SEFAZ - CTe
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cUF">Código da UF</param>
        /// <returns>Nome da classe</returns>
        private string NomeClasseWS(Servicos servico, int cUF)
        {
            string retorna = string.Empty;

            switch (Propriedade.TipoAplicativo)
            {
                case TipoAplicativo.Cte:
                    retorna = NomeClasseWSCTe(servico, cUF);
                    break;
                case TipoAplicativo.Nfe:
                    retorna = NomeClasseWSNFe(servico, cUF);
                    break;
                case TipoAplicativo.Nfse:
                    retorna = NomeClasseWSNFSe(servico, cUF);
                    break;
            }

            return retorna;
        }
        #endregion

        #region NomeClasseWSNFe()
        /// <summary>
        /// Retorna o nome da classe do serviço passado por parâmetro do WebService do SEFAZ - NFe
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cUF">Código da UF</param>
        /// <returns>Nome da classe</returns>
        private string NomeClasseWSNFe(Servicos servico, int cUF)
        {
            string retorna = string.Empty;

            switch (servico)
            {
                case Servicos.CancelarNFe:
                    retorna = "NfeCancelamento2";
                    break;
                case Servicos.InutilizarNumerosNFe:
                    retorna = "NfeInutilizacao2";
                    break;
                case Servicos.PedidoConsultaSituacaoNFe:
                    retorna = "NfeConsulta2";
                    break;
                case Servicos.PedidoConsultaStatusServicoNFe:
                    retorna = "NfeStatusServico2";
                    break;
                case Servicos.PedidoSituacaoLoteNFe:
                    retorna = "NfeRetRecepcao2";
                    break;
                case Servicos.ConsultaCadastroContribuinte:
                    retorna = "CadConsultaCadastro2";
                    break;
                case Servicos.EnviarLoteNfe:
                    retorna = "NfeRecepcao2";
                    break;
                default:
                    break;
            }

            return retorna;
        }
        #endregion

        #region NomeClasseWSCTe()
        /// <summary>
        /// Retorna o nome da classe do serviço passado por parâmetro do WebService do SEFAZ - CTe
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cUF">Código da UF</param>
        /// <returns>Nome da classe</returns>
        private string NomeClasseWSCTe(Servicos servico, int cUF)
        {
            string retorna = string.Empty;

            switch (servico)
            {
                case Servicos.CancelarNFe:
                    retorna = "CteCancelamento";
                    break;
                case Servicos.InutilizarNumerosNFe:
                    retorna = "CteInutilizacao";
                    break;
                case Servicos.PedidoConsultaSituacaoNFe:
                    retorna = "CteConsulta";
                    break;
                case Servicos.PedidoConsultaStatusServicoNFe:
                    retorna = "CteStatusServico";
                    break;
                case Servicos.PedidoSituacaoLoteNFe:
                    retorna = "CteRetRecepcao";
                    break;
                case Servicos.ConsultaCadastroContribuinte:
                    retorna = "CadConsultaCadastro2";
                    break;
                case Servicos.EnviarLoteNfe:
                    retorna = "CteRecepcao";
                    break;
                default:
                    break;
            }

            return retorna;
        }
        #endregion

        #region NomeClasseWSNFSe()
        /// <summary>
        /// Retorna o nome da classe do serviço passado por parâmetro do WebService do SEFAZ - CTe
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cMunicipio">Código do Municipio UF</param>
        /// <returns>Nome da classe</returns>
        private string NomeClasseWSNFSe(Servicos servico, int cMunicipio)
        {
            string retorna = string.Empty;

            switch (Functions.PadraoNFSe(cMunicipio))
            {
                #region GINFES
                case PadroesNFSe.GINFES:
                    retorna = "ServiceGinfesImplService";
                    break;
                #endregion

                #region THEMA
                case PadroesNFSe.THEMA:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "NFSEconsulta";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "NFSEconsulta";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "NFSEconsulta";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "NFSEconsulta";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "NFSEcancelamento";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "NFSEremessa";
                            break;
                    }
                    break;
                #endregion

                #region BETHA
                case PadroesNFSe.BETHA:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region CANOAS-RS (ABACO)
                case PadroesNFSe.CANOAS_RS:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "ConsultarNfse";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "ConsultarNfsePorRps";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "RecepcionarLoteRPS";
                            break;
                    }
                    break;
                #endregion
            }

            return retorna;
        }
        #endregion

        #region NomeClasseCabecWS()
        /// <summary>
        /// Retorna o nome da classe de cabecalho do serviço
        /// </summary>
        /// <returns>Nome da classe de cabecalho</returns>
        private string NomeClasseCabecWS()
        {
            string retorna = string.Empty;

            switch (Propriedade.TipoAplicativo)
            {
                case TipoAplicativo.Cte:
                    retorna = NomeClasseCabecWSCTe();
                    break;
                case TipoAplicativo.Nfe:
                    retorna = NomeClasseCabecWSNFe();
                    break;
            }

            return retorna;
        }
        #endregion

        #region NomeClasseCabecWSNFe()
        /// <summary>
        /// Retorna o nome da classe de cabecalho do serviço de NFe
        /// </summary>
        /// <returns>nome da classe de cabecalho do serviço de NFe</returns>
        private string NomeClasseCabecWSNFe()
        {
            return "nfeCabecMsg";
        }
        #endregion

        #region NomeClasseCabecWSNFe()
        /// <summary>
        /// Retorna o nome da classe de cabecalho do serviço de CTe
        /// </summary>
        /// <returns>nome da classe de cabecalho do serviço de CTe</returns>
        private string NomeClasseCabecWSCTe()
        {
            return "cteCabecMsg";
        }
        #endregion

        #region NomeMetodoWS()
        /// <summary>
        /// Retorna o nome do método da classe de serviço
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cUF">Código da UF</param>
        /// <returns>nome do método da classe de serviço</returns>
        private string NomeMetodoWS(Servicos servico, int cUF)
        {
            string retorna = string.Empty;

            switch (Propriedade.TipoAplicativo)
            {
                case TipoAplicativo.Cte:
                    retorna = NomeMetodoWSCTe(servico, cUF);
                    break;
                case TipoAplicativo.Nfe:
                    retorna = NomeMetodoWSNFe(servico, cUF);
                    break;
                case TipoAplicativo.Nfse:
                    retorna = NomeMetodoWSNFSe(servico, cUF);
                    break;
            }

            return retorna;
        }
        #endregion

        #region NomeMetodoWSNFe()
        /// <summary>
        /// Retorna o nome do método da classe de serviço - NFe
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cUF">Código da UF</param>
        /// <returns>nome do método da classe de serviço</returns>
        private string NomeMetodoWSNFe(Servicos servico, int cUF)
        {
            string retorna = string.Empty;

            switch (servico)
            {
                case Servicos.CancelarNFe:
                    retorna = "nfeCancelamentoNF2";
                    break;
                case Servicos.InutilizarNumerosNFe:
                    retorna = "nfeInutilizacaoNF2";
                    break;
                case Servicos.PedidoConsultaSituacaoNFe:
                    retorna = "nfeConsultaNF2";
                    break;
                case Servicos.PedidoConsultaStatusServicoNFe:
                    retorna = "nfeStatusServicoNF2";
                    break;
                case Servicos.PedidoSituacaoLoteNFe:
                    retorna = "nfeRetRecepcao2";
                    break;
                case Servicos.ConsultaCadastroContribuinte:
                    switch (cUF)
                    {
                        case 52:
                            retorna = "cadConsultaCadastro2";
                            break;
                        default:
                            retorna = "consultaCadastro2";
                            break;
                    }
                    break;
                case Servicos.EnviarLoteNfe:
                    retorna = "nfeRecepcaoLote2";
                    break;
                default:
                    break;
            }

            return retorna;
        }
        #endregion

        #region NomeMetodoWSCTe()
        /// <summary>
        /// Retorna o nome do método da classe de serviço - CTe
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cUF">Código da UF</param>
        /// <returns>nome do método da classe de serviço</returns>
        private string NomeMetodoWSCTe(Servicos servico, int cUF)
        {
            string retorna = string.Empty;

            switch (servico)
            {
                case Servicos.CancelarNFe:
                    retorna = "cteCancelamentoCT";
                    break;
                case Servicos.InutilizarNumerosNFe:
                    retorna = "cteInutilizacaoCT";
                    break;
                case Servicos.PedidoConsultaSituacaoNFe:
                    retorna = "cteConsultaCT";
                    break;
                case Servicos.PedidoConsultaStatusServicoNFe:
                    retorna = "cteStatusServicoCT";
                    break;
                case Servicos.PedidoSituacaoLoteNFe:
                    retorna = "cteRetRecepcao";
                    break;
                case Servicos.ConsultaCadastroContribuinte:
                    switch (cUF)
                    {
                        case 52:
                            retorna = "cadConsultaCadastro2";
                            break;
                        default:
                            retorna = "consultaCadastro2";
                            break;
                    }
                    break;
                case Servicos.EnviarLoteNfe:
                    retorna = "cteRecepcaoLote";
                    break;
            }

            return retorna;
        }
        #endregion

        #region NomeMetodoWSNFSe()
        /// <summary>
        /// Retorna o nome da classe do serviço passado por parâmetro do WebService do SEFAZ - CTe
        /// </summary>
        /// <param name="servico">Servico</param>
        /// <param name="cMunicipio">Código do Municipio UF</param>
        /// <returns>Nome da classe</returns>
        private string NomeMetodoWSNFSe(Servicos servico, int cMunicipio)
        {
            string retorna = string.Empty;

            switch (Functions.PadraoNFSe(cMunicipio))
            {
                #region GINFES
                case PadroesNFSe.GINFES:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "ConsultarLoteRpsV3";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRpsV3";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "RecepcionarLoteRpsV3";
                            break;
                    }
                    break;
                #endregion

                #region THEMA
                case PadroesNFSe.THEMA:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "consultarLoteRps";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "consultarNfse";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "consultarNfsePorRps";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "consultarSituacaoLoteRps";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "cancelarNfse";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "recepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region BETHA
                case PadroesNFSe.BETHA:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "ConsultarLoteRps";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "ConsultarSituacaoLoteRps";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "CancelarNfse";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "RecepcionarLoteRps";
                            break;
                    }
                    break;
                #endregion

                #region CANOAS - RS (ABACO)
                case PadroesNFSe.CANOAS_RS:
                    switch (servico)
                    {
                        case Servicos.ConsultarLoteRps:
                            retorna = "Execute";
                            break;
                        case Servicos.ConsultarNfse:
                            retorna = "Execute";
                            break;
                        case Servicos.ConsultarNfsePorRps:
                            retorna = "Execute";
                            break;
                        case Servicos.ConsultarSituacaoLoteRps:
                            retorna = "Execute";
                            break;
                        case Servicos.CancelarNfse:
                            retorna = "Execute";
                            break;
                        case Servicos.RecepcionarLoteRps:
                            retorna = "Execute";
                            break;
                    }
                    break;
                #endregion
            }

            return retorna;
        }
        #endregion

        #endregion
    }
}