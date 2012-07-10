using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
//using UniNFeLibrary;
//using UniNFeLibrary.Enums;

#if x
namespace UniNFeServico
{
    #region Classe ServicoNFe
    public class ServicoNFe : AServicoNFe
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
                    object oCancelamento = wsProxy.CriarObjeto("NfeCancelamento2");
                    object oCabecMsg = wsProxy.CriarObjeto("nfeCabecMsg");

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "cUF", oLer.oDadosPedCanc.cUF.ToString());
                    wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLCanc);

                    //Criar objeto da classe de assinatura digita
                    AssinaturaDigital oAD = new AssinaturaDigital();

                    //Assinar o XML
                    oAD.Assinar(NomeArquivoXML, "infCanc", Empresa.Configuracoes[emp].X509Certificado);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oCancelamento, "nfeCancelamentoNF2", oCabecMsg, this, "-ped-can", "-can");

                    //Ler o retorno do webservice
                    this.LerRetornoCanc();
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
                    ExtRet = ExtXml.PedCan;
                else //Se for TXT
                    ExtRet = ExtXml.PedCan_TXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    oAux.GravarArqErroServico(NomeArquivoXML, ExtRet, ExtXmlRet.Can_ERR, ex);
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
                        oAux.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                    //Se falhou algo na hora de deletar o XML de cancelamento de NFe, infelizmente
                    //não posso fazer mais nada, o UniNFe vai tentar mandar o arquivo novamente para o webservice, pois ainda não foi excluido.
                    //Wandrey 09/03/2010
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
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servicos.PedidoConsultaSituacaoNFe, emp, oLer.oDadosPedSit.cUF, oLer.oDadosPedSit.tpAmb, oLer.oDadosPedSit.tpEmis, oLer.oDadosPedSit.versaoNFe);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    if (oLer.oDadosPedSit.versaoNFe == 1)
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
                        object oConsulta = wsProxy.CriarObjeto("NfeConsulta2");
                        object oCabecMsg = wsProxy.CriarObjeto("nfeCabecMsg");

                        //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                        wsProxy.SetProp(oCabecMsg, "cUF", oLer.oDadosPedSit.cUF.ToString());
                        wsProxy.SetProp(oCabecMsg, "versaoDados", oLer.oDadosPedSit.versaoNFe == 201 ? "2.01" : ConfiguracaoApp.VersaoXMLPedSit);

                        //Invocar o método que envia o XML para o SEFAZ
                        oInvocarObj.Invocar(wsProxy, oConsulta, "nfeConsultaNF2", oCabecMsg, this);
                    }

                    //Efetuar a leitura do retorno da situação para ver se foi autorizada ou não
                    //Na versão 1 não posso gerar o -procNfe, ou vou ter que tratar a estrutura do XML de acordo com a versão, a consulta na versão 1 é somente para obter o resultado mesmo.
                    if (oLer.oDadosPedSit.versaoNFe != 1)
                        this.LerRetornoSit(oLer.oDadosPedSit.chNFe);

                    //Gerar o retorno para o ERP
                    oGerarXML.XmlRetorno(ExtXml.PedSit, ExtXmlRet.Sit, this.vStrXmlRetorno);
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
                    ExtRet = ExtXml.PedSit;
                else //Se for TXT
                    ExtRet = ExtXml.PedSit_TXT;

                try
                {
                    oAux.GravarArqErroServico(NomeArquivoXML, ExtRet, ExtXmlRet.Sit_ERR, ex);
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
                    oAux.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                    //Se falhou algo na hora de deletar o XML de pedido da consulta da situação da NFe, infelizmente
                    //não posso fazser mais nada, o UniNFe vai tentar mantar o arquivo novamente para o webservice, pois ainda não foi excluido.
                    //Wandrey 22/03/2010
                }
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
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servicos.ConsultaCadastroContribuinte, emp, oLer.oDadosConsCad.cUF, oLer.oDadosConsCad.tpAmb, TipoEmissao.teNormal);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oConsCad = wsProxy.CriarObjeto("CadConsultaCadastro2");                                                           
                    object oCabecMsg = wsProxy.CriarObjeto("nfeCabecMsg");

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "cUF", oLer.oDadosConsCad.cUF.ToString());
                    wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLConsCad);

                    string cMetodo = "consultaCadastro2";
                    switch (oLer.oDadosConsCad.cUF)
                    {
                        case 52:
                            cMetodo = "cadConsultaCadastro2";
                            goto default;

                        default:
                            break;
                    }

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oConsCad, cMetodo, oCabecMsg, this, "-cons-cad", "-ret-cons-cad");
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
                    ExtRet = ExtXml.ConsCad;
                else //Se for TXT
                    ExtRet = ExtXml.ConsCad_TXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    oAux.GravarArqErroServico(NomeArquivoXML, ExtRet, ExtXmlRet.ConsCad_ERR, ex);
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
                    oAux.DeletarArquivo(NomeArquivoXML);
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
                    object oInutilizacao = wsProxy.CriarObjeto("NfeInutilizacao2");
                    object oCabecMsg = wsProxy.CriarObjeto("nfeCabecMsg");

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "cUF", oLer.oDadosPedInut.cUF.ToString());
                    wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLInut);

                    //Criar objeto da classe de assinatura digita
                    AssinaturaDigital oAD = new AssinaturaDigital();

                    //Assinar o XML
                    oAD.Assinar(NomeArquivoXML, "infInut", Empresa.Configuracoes[emp].X509Certificado);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oInutilizacao, "nfeInutilizacaoNF2", oCabecMsg, this, "-ped-inu", "-inu");

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
                    ExtRet = ExtXml.PedInu;
                else //Se for TXT
                    ExtRet = ExtXml.PedInu_TXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    oAux.GravarArqErroServico(NomeArquivoXML, ExtRet, ExtXmlRet.Inu_ERR, ex);
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
                        oAux.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                    //Se falhou algo na hora de deletar o XML de inutilização, infelizmente não posso 
                    //fazer mais nada. Com certeza o uninfe sendo restabelecido novamente vai tentar enviar o mesmo 
                    //xml de inutilização para o SEFAZ. Este erro pode ocorrer por falha no HD, rede, Permissão de pastas, etc. Wandrey 23/03/2010
                }
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
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

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

                            oGerarXML.XmlDistCanc(NomeArquivoXML, strRetCancNFe);
                            ///
                            /// danasa 9-2009
                            /// pega a data da emissão da nota para mover os XML's para a pasta de origem da NFe
                            /// 
                            string cChaveNFe = infCancElemento.GetElementsByTagName("chNFe")[0].InnerText;
                            //TODO: Cancelamento - Se for pasta por dia, tem que pegar a data de dentro do XML da NFe
                            DateTime dtEmissaoNFe = new DateTime(Convert.ToInt16("20" + cChaveNFe.Substring(2, 2)), Convert.ToInt16(cChaveNFe.Substring(4, 2)), 1);
                            //DateTime dtEmissaoNFe = DateTime.Now;                            

                            //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                            oAux.MoverArquivo(NomeArquivoXML, PastaEnviados.Autorizados, dtEmissaoNFe);//DateTime.Now);

                            //Move o arquivo de Distribuição para a pasta de enviados autorizados
                            string strNomeArqProcCancNFe = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
                                                            PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                            oAux.ExtrairNomeArq(NomeArquivoXML, ExtXml.PedCan) + ExtXmlRet.ProcCancNFe;
                            oAux.MoverArquivo(strNomeArqProcCancNFe, PastaEnviados.Autorizados, dtEmissaoNFe);//DateTime.Now);
                        }
                        else
                        {
                            //Deletar o arquivo de solicitação do serviço da pasta de envio
                            oAux.DeletarArquivo(NomeArquivoXML);
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
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

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

                            oGerarXML.XmlDistInut(NomeArquivoXML, strRetInutNFe);

                            //Move o arquivo de solicitação do serviço para a pasta de enviados autorizados
                            oAux.MoverArquivo(NomeArquivoXML, PastaEnviados.Autorizados, DateTime.Now);

                            //Move o arquivo de Distribuição para a pasta de enviados autorizados
                            string strNomeArqProcInutNFe = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
                                                            PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                            oAux.ExtrairNomeArq(NomeArquivoXML, ExtXml.PedInu/*"-ped-inu.xml"*/) + ExtXmlRet.ProcInutNFe;// "-procInutNFe.xml";
                            oAux.MoverArquivo(strNomeArqProcInutNFe, PastaEnviados.Autorizados, DateTime.Now);
                        }
                        else
                        {
                            //Deletar o arquivo de solicitação do serviço da pasta de envio
                            oAux.DeletarArquivo(NomeArquivoXML);
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

        #region LerRetornoLote()
        /// <summary>
        /// Efetua a leitura do XML de retorno do processamento do lote de notas fiscais e 
        /// atualiza o arquivo de fluxo e envio de notas
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        protected override void LerRetornoLote()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
            var oLerXml = new LerXML();
            var msXml = Auxiliar.StringXmlToStream(vStrXmlRetorno);

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
                        case "516": //D-Em caso de falha de schema, verificar se existe a tag raiz esperada para mensagem
                        case "517": //D-Em caso de falha de Schema, verificar se existe o atributo versão para a tag raiz da mensagem
                        case "545": //D-Em caso de falha de schema, verificar se o conteúdo do atributo versão difere do conteúdo da versaoDados informado no SOAPHeader
                        case "404": //D-Verifica o uso de prefixo no namespace
                        case "402": //D-XML utiliza codificação diferente de UTF-8
                        case "252": //E-Tipo do ambiente da NF-e difere do ambiente do web service
                        case "248": //E-UF do recibo difere da UF do Web Service
                        case "553": //E-Tipo autorizador do recibo diverge do órgão autorizador
                        case "223": //E-CNPJ do transmissor do lote difere do CNPJ do transmissor da consulta
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
                            var protNFeList = retConsReciNFeElemento.GetElementsByTagName("protNFe");

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
                                            throw new Exception("LerRetornoLote(): Não pode obter o nome do arquivo");

                                        strNomeArqNfe = strChaveNFe.Substring(3) + ExtXml.Nfe;
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
                                                oGerarXML.XmlDistNFe(strArquivoNFe, strProtNfe);
                                                var strArquivoNFeProc = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
                                                                        PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                                        oAux.ExtrairNomeArq(strNomeArqNfe, ExtXml.Nfe) + ExtXmlRet.ProcNFe;

                                                //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s autorizados
                                                oLerXml.Nfe(strArquivoNFe);

                                                //Mover a nfePRoc da pasta de NFE em processamento para a NFe Autorizada
                                                //Para envitar falhar, tenho que mover primeiro o XML de distribuição (-procnfe.xml) para
                                                //depois mover o da nfe (-nfe.xml), pois se ocorrer algum erro, tenho como reconstruir o senário, 
                                                //assim sendo não inverta as posições. Wandrey 08/10/2009
                                                oAux.MoverArquivo(strArquivoNFeProc, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);

                                                //Mover a NFE da pasta de NFE em processamento para NFe Autorizada
                                                //Para envitar falhar, tenho que mover primeiro o XML de distribuição (-procnfe.xml) para 
                                                //depois mover o da nfe (-nfe.xml), pois se ocorrer algum erro, tenho como reconstruir o senário.
                                                //assim sendo não inverta as posições. Wandrey 08/10/2009
                                                oAux.MoverArquivo(strArquivoNFe, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);

                                                //Disparar a geração/impressçao do UniDanfe. 03/02/2010 - Wandrey
                                                oAux.ExecutaUniDanfe(strNomeArqNfe, oLerXml.oDadosNfe.dEmi);
                                            }
                                            break;

                                        case "301": //NFe Denegada - Problemas com o emitente
                                            //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s Denegados
                                            oLerXml.Nfe(strArquivoNFe);

                                            //Mover a NFE da pasta de NFE em processamento para NFe Denegadas
                                            oAux.MoverArquivo(strArquivoNFe, PastaEnviados.Denegados, oLerXml.oDadosNfe.dEmi);
                                            break;

                                        case "302": //NFe Denegada - Problemas com o destinatário
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
        #endregion

        #region LerRetornoSit()
        /// <summary>
        /// Ler o retorno da consulta situação da nota fiscal e de acordo com o status ele trata as notas enviadas se ainda não foram tratadas
        /// </summary>
        /// <param name="ChaveNFe">Chave da NFe que está sendo consultada</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 16/06/2010
        /// </remarks>
        protected override void LerRetornoSit(string ChaveNFe)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            LerXML oLerXml = new LerXML();
            MemoryStream msXml = Auxiliar.StringXmlToStream(this.vStrXmlRetorno);

            FluxoNfe oFluxoNfe = new FluxoNfe();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(msXml);

                #region Distribuicao de Eventos

                oGerarXML.XmlDistEvento(emp, this.vStrXmlRetorno);    //<<<danasa 6-2011

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
                            throw new Exception("LerRetornoSit(): Não pode obter o nome do arquivo");

                        strNomeArqNfe = strChaveNFe.Substring(3) + ExtXml.Nfe;
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
                        case "516": //D-Em caso de falha de schema, verificar se existe a tag raiz esperada para mensagem
                        case "517": //D-Em caso de falha de Schema, verificar se existe o atributo versão para a tag raiz da mensagem
                        case "545": //D-Em caso de falha de schema, verificar se o conteúdo do atributo versão difere do conteúdo da versaoDados informado no SOAPHeader
                        case "404": //D-Verifica o uso de prefixo no namespace
                        case "402": //D-XML utiliza codificação diferente de UTF-8
                        case "252": //J-Tipo do ambiente da NF-e difere do ambiente do web service
                        case "226": //J-UF da chave de acesso difere da UF do web service
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

                        #region Nota fiscal autorizada
                        case "100": //Autorizado o uso da NFe
                            XmlNodeList infConsSitList = retConsSitElemento.GetElementsByTagName("infProt");
                            if (infConsSitList != null)
                            {
                                foreach (XmlNode infConsSitNode in infConsSitList)
                                {
                                    XmlElement infConsSitElemento = (XmlElement)infConsSitNode;

                                    //Pegar o Status do Retorno da consulta situação
                                    string strStat = oAux.LerTag(infConsSitElemento, "cStat").Replace(";", "");

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

                                            string strProtNfe = "<protNFe versao=\"" + ConfiguracaoApp.VersaoXMLNFe + "\">" +
                                                "<infProt" + atributoId + ">" +
                                                "<tpAmb>" + oAux.LerTag(infConsSitElemento, "tpAmb", false) + "</tpAmb>" +
                                                "<verAplic>" + oAux.LerTag(infConsSitElemento, "verAplic", false) + "</verAplic>" +
                                                "<chNFe>" + oAux.LerTag(infConsSitElemento, "chNFe", false) + "</chNFe>" +
                                                "<dhRecbto>" + oAux.LerTag(infConsSitElemento, "dhRecbto", false) + "</dhRecbto>" +
                                                "<nProt>" + oAux.LerTag(infConsSitElemento, "nProt", false) + "</nProt>" +
                                                "<digVal>" + oAux.LerTag(infConsSitElemento, "digVal", false) + "</digVal>" +
                                                "<cStat>" + oAux.LerTag(infConsSitElemento, "cStat", false) + "</cStat>" +
                                                "<xMotivo>" + oAux.LerTag(infConsSitElemento, "xMotivo", false) + "</xMotivo>" +
                                                "</infProt>" +
                                                "</protNFe>";

                                            //Definir o nome do arquivo -procNfe.xml                                               
                                            string strArquivoNFeProc = Empresa.Configuracoes[emp].PastaEnviado + "\\" +
                                                                        PastaEnviados.EmProcessamento.ToString() + "\\" +
                                                                        oAux.ExtrairNomeArq(strArquivoNFe, ExtXml.Nfe) + ExtXmlRet.ProcNFe;

                                            //Se existir o strArquivoNfe, tem como eu fazer alguma coisa, se ele não existir
                                            //Não tenho como fazer mais nada. Wandrey 08/10/2009
                                            if (File.Exists(strArquivoNFe))
                                            {
                                                //Ler o XML para pegar a data de emissão para criar a pasta dos XML´s autorizados
                                                oLerXml.Nfe(strArquivoNFe);

                                                //Verificar se a -nfe.xml existe na pasta de autorizados
                                                bool NFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, ExtXml.Nfe);

                                                //Verificar se o -procNfe.xml existe na past de autorizados
                                                bool procNFeJaNaAutorizada = oAux.EstaAutorizada(strArquivoNFe, oLerXml.oDadosNfe.dEmi, ExtXmlRet.ProcNFe);

                                                //Se o XML de distribuição não estiver na pasta de autorizados
                                                if (!procNFeJaNaAutorizada)
                                                {
                                                    if (!File.Exists(strArquivoNFeProc))
                                                    {
                                                        oGerarXML.XmlDistNFe(strArquivoNFe, strProtNfe);
                                                    }
                                                }

                                                //Se o XML de distribuição não estiver ainda na pasta de autorizados
                                                if (!procNFeJaNaAutorizada)
                                                {
                                                    //Move a nfeProc da pasta de NFE em processamento para a NFe Autorizada
                                                    oAux.MoverArquivo(strArquivoNFeProc, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);
                                                }

                                                //Se a NFe não existir ainda na pasta de autorizados
                                                if (!NFeJaNaAutorizada)
                                                {
                                                    //Mover a NFE da pasta de NFE em processamento para NFe Autorizada
                                                    oAux.MoverArquivo(strArquivoNFe, PastaEnviados.Autorizados, oLerXml.oDadosNfe.dEmi);
                                                }
                                                else
                                                {
                                                    //Se já estiver na pasta de autorizados, vou somente excluir ela da pasta de XML´s em processamento
                                                    oAux.DeletarArquivo(strArquivoNFe);
                                                }

                                                //Disparar a geração/impressçao do UniDanfe. 03/02/2010 - Wandrey
                                                oAux.ExecutaUniDanfe(strNomeArqNfe, oLerXml.oDadosNfe.dEmi);
                                            }

                                            if (File.Exists(strArquivoNFeProc))
                                            {
                                                //Se já estiver na pasta de autorizados, vou somente excluir ela da pasta de XML´s em processamento
                                                oAux.DeletarArquivo(strArquivoNFeProc);
                                            }

                                            break;

                                        case "301":
                                            //Ler o XML para pegar a data de emissão para criar a psta dos XML´s Denegados
                                            oLerXml.Nfe(strArquivoNFe);

                                            //Move a NFE da pasta de NFE em processamento para NFe Denegadas
                                            if (!oAux.EstaDenegada(strArquivoNFe, oLerXml.oDadosNfe.dEmi))
                                            {
                                                oAux.MoverArquivo(strArquivoNFe, PastaEnviados.Denegados, oLerXml.oDadosNfe.dEmi);
                                            }

                                            break;

                                        case "302":
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
        public override void Recepcao()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;
            //Definir o serviço que será executado para a classe
            Servico = Servicos.EnviarLoteNfe;

            FluxoNfe oFluxoNfe = new FluxoNfe();
            LerXML oLer = new LerXML();

            try
            {
                #region Parte que envia o lote
                //Ler o XML de Lote para pegar o número do lote que está sendo enviado
                oLer.Nfe(NomeArquivoXML);

                var idLote = oLer.oDadosNfe.idLote;

                //Definir o objeto do WebService
                WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servicos.EnviarLoteNfe, emp, Convert.ToInt32(oLer.oDadosNfe.cUF), Convert.ToInt32(oLer.oDadosNfe.tpAmb), Convert.ToInt32(oLer.oDadosNfe.tpEmis));
                
                //Criar objetos das classes dos serviços dos webservices do SEFAZ
                object oRecepcao = oRecepcao = wsProxy.CriarObjeto("NfeRecepcao2");
                var oCabecMsg = wsProxy.CriarObjeto("nfeCabecMsg");

                //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                wsProxy.SetProp(oCabecMsg, "cUF", oLer.oDadosNfe.cUF);
                wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLNFe);

                //
                //XML neste ponto a NFe já está assinada, pois foi assinada, validada e montado o lote para envio por outro serviço. 
                //Fica aqui somente este lembrete. Wandrey 16/03/2010
                //

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.Invocar(wsProxy, oRecepcao, "nfeRecepcaoLote2", oCabecMsg, this, "-env-lot", "-rec");
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
                else if (Convert.ToInt32(oLerRecibo.oDadosRec.cStat) > 200)
                {
                    //Se o status do retorno do lote for maior que 200, 
                    //vamos ter que excluir a nota do fluxo, porque ela foi rejeitada pelo SEFAZ
                    //Primeiro vamos mover o xml da nota da pasta EmProcessamento para pasta de XML´s com erro e depois tira ela do fluxo
                    //Wandrey 30/04/2009
                    oAux.MoveArqErro(Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.EmProcessamento.ToString() + "\\" + oFluxoNfe.LerTag(oLer.oDadosNfe.chavenfe, FluxoNfe.ElementoFixo.ArqNFe));
                    oFluxoNfe.ExcluirNfeFluxo(oLer.oDadosNfe.chavenfe);
                }

                //Deleta o arquivo de lote
                oAux.DeletarArquivo(NomeArquivoXML);
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
                    oAux.GravarArqErroServico(NomeArquivoXML, ExtXml.EnvLot, ExtXmlRet.Rec_ERR, ex);
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
                    oAux.GravarArqErroServico(NomeArquivoXML, ExtXml.EnvLot, ExtXmlRet.Rec_ERR, ex, false);
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
                    oAux.GravarArqErroServico(NomeArquivoXML, ExtXml.EnvLot, ExtXmlRet.Rec_ERR, ex);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 16/03/2010
                }
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
                var oCancelamento = wsProxy.CriarObjeto("NfeRetRecepcao2");
                var oCabecMsg = wsProxy.CriarObjeto("nfeCabecMsg");

                //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                wsProxy.SetProp(oCabecMsg, "cUF", oLer.oDadosPedRec.cUF.ToString());
                wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLPedRec);

                //Invocar o método que envia o XML para o SEFAZ
                oInvocarObj.Invocar(wsProxy, oCancelamento, "nfeRetRecepcao2", oCabecMsg, this);
                #endregion

                #region Parte do código que trata o XML de retorno da consulta do recibo
                //Efetuar a leituras das notas do lote para ver se foi autorizada ou não
                LerRetornoLote();

                //Gravar o XML retornado pelo WebService do SEFAZ na pasta de retorno para o ERP
                //Tem que ser feito neste ponto, pois somente aqui terminamos todo o processo
                //Wandrey 18/06/2009
                oGerarXML.XmlRetorno(ExtXml.PedRec, ExtXmlRet.ProRec, vStrXmlRetorno);
                #endregion
            }
            catch (Exception ex)
            {
                try
                {
                    oAux.GravarArqErroServico(NomeArquivoXML, ExtXml.PedRec, ExtXmlRet.ProRec_ERR, ex);
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
                oAux.DeletarArquivo(NomeArquivoXML);
            }
        }
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
                    var oStatusServico = wsProxy.CriarObjeto("NfeStatusServico2");
                    var oCabecMsg = wsProxy.CriarObjeto("nfeCabecMsg");

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "cUF", oLer.oDadosPedSta.cUF.ToString());
                    wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLStatusServico);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oStatusServico, "nfeStatusServicoNF2", oCabecMsg, this, "-ped-sta", "-sta");
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
                var extRet = vXmlNfeDadosMsgEhXML ? ExtXml.PedSta : ExtXml.PedSta_TXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    oAux.GravarArqErroServico(NomeArquivoXML, extRet, ExtXmlRet.Sta_ERR, ex);
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
                    oAux.DeletarArquivo(NomeArquivoXML);
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
                    oAD.Assinar(NomeArquivoXML, "infDPEC", Empresa.Configuracoes[emp].X509Certificado);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oRecepcaoDPEC, "sceRecepcaoDPEC", oCabecMsg, this);

                    //Ler o retorno
                    LerRetDPEC();

                    //Gravar o XML retornado pelo WebService do SEFAZ na pasta de retorno para o ERP
                    //Tem que ser feito neste ponto, pois somente aqui terminamos todo o processo
                    oGerarXML.XmlRetorno(ExtXml.EnvDPEC, ExtXmlRet.retDPEC, vStrXmlRetorno);
                }
                else
                {
                    // Gerar o XML de solicitacao de situacao do servico a partir do TXT gerado pelo ERP
                    oGerarXML.EnvioDPEC(Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml", oLer.dadosEnvDPEC);
                }
            }
            catch (Exception ex)
            {
                var ExtRet = vXmlNfeDadosMsgEhXML ? ExtXml.EnvDPEC : ExtXml.EnvDPEC_TXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    oAux.GravarArqErroServico(NomeArquivoXML, ExtRet, ExtXmlRet.retDPEC_ERR, ex);
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
                    oAux.DeletarArquivo(NomeArquivoXML);
                }
                catch
                {
                    //Se falhou algo na hora de deletar o XML de cancelamento de NFe, infelizmente
                    //não posso fazer mais nada, o UniNFe vai tentar mandar o arquivo novamente para o webservice, pois ainda não foi excluido.
                    //Wandrey 09/03/2010
                }
            }

        }
        #endregion

        #region LerRetDPEC()
        protected override void LerRetDPEC()
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            XmlDocument doc = new XmlDocument();

            try
            {
                MemoryStream msXml = Auxiliar.StringXmlToStream(this.vStrXmlRetorno);
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
                            oAux.MoverArquivo(NomeArquivoXML, PastaEnviados.Autorizados, dtEmissaoDPEC);

                            //Gravar o XML retornado pelo WebService do SEFAZ na pasta de autorizados. Wandrey 25/11/2010
                            string nomePastaEnviado = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.Autorizados.ToString() + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(dtEmissaoDPEC);
                            oGerarXML.XmlRetorno(ExtXml.EnvDPEC, ExtXmlRet.retDPEC, vStrXmlRetorno, nomePastaEnviado);
                        }
                        else
                        {
                            //Deletar o arquivo de solicitação do serviço da pasta de envio
                            oAux.DeletarArquivo(NomeArquivoXML);
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

        #region ConsultaDPEC()
        /// <summary>
        /// Envia o XML de consulta do registro do DPEC para o SCE (Sistema de Contingência Eletronica)
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 19/10/2010
        /// </remarks>
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
                    oGerarXML.XmlRetorno(ExtXml.ConsDPEC, ExtXmlRet.retConsDPEC, vStrXmlRetorno);
                }
                else
                {
                    // Gerar o XML de solicitacao de situacao do servico a partir do TXT gerado pelo ERP
                    oGerarXML.ConsultaDPEC(Path.GetFileNameWithoutExtension(NomeArquivoXML) + ".xml", oLer.dadosConsDPEC);
                }
            }
            catch (Exception ex)
            {
                var ExtRet = vXmlNfeDadosMsgEhXML ? ExtXml.ConsDPEC : ExtXml.ConsDPEC_TXT;

                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    oAux.GravarArqErroServico(NomeArquivoXML, ExtRet, ExtXmlRet.retConsDPEC_ERR, ex);
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
                    oAux.DeletarArquivo(NomeArquivoXML);
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
                MemoryStream msXml = Auxiliar.StringXmlToStream(this.vStrXmlRetorno);
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
                            string arqEnvDpec = Empresa.Configuracoes[emp].PastaEnvio + "\\" + oAux.ExtrairNomeArq(NomeArquivoXML, ExtXml.ConsDPEC) + ExtXml.EnvDPEC;
                            if (File.Exists(arqEnvDpec))
                            {
                                oAux.MoverArquivo(arqEnvDpec, PastaEnviados.Autorizados, dtEmissaoDPEC);
                            }

                            //Gravar o XML retornado pelo WebService do SEFAZ na pasta de autorizados. Wandrey 25/11/2010
                            string nomePastaEnviado = Empresa.Configuracoes[emp].PastaEnviado + "\\" + PastaEnviados.Autorizados.ToString() + "\\" + Empresa.Configuracoes[emp].DiretorioSalvarComo.ToString(dtEmissaoDPEC);
                            oGerarXML.XmlRetorno(ExtXml.ConsDPEC, ExtXmlRet.retConsDPEC, vStrXmlRetorno, nomePastaEnviado);
                        }
                        else
                        {
                            //Deletar o arquivo de solicitação do serviço da pasta de envio
                            oAux.DeletarArquivo(NomeArquivoXML);
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

        #region Servicos Envio

        /// <summary>
        /// RecepcaoEvento
        /// </summary>
        public override void RecepcaoEvento()   //<<<danasa 6-2011
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

            //Definir o serviço que será executado para a classe
            Servico = Servicos.EnviarCCe;

            try
            {
                //Ler o XML para pegar parâmetros de envio
                LerXML oLer = new LerXML();
                oLer.EnvEvento(emp, NomeArquivoXML);

                if (vXmlNfeDadosMsgEhXML)
                {
                    //Definir o objeto do WebService
                    WebServiceProxy wsProxy = ConfiguracaoApp.DefinirWS(Servicos.EnviarCCe, 
                        emp,
                        Empresa.Configuracoes[emp].UFCod,   //oLer.oDadosEnvEvento.eventos[0].cOrgao, 
                        Empresa.Configuracoes[emp].tpAmb,   //oLer.oDadosEnvEvento.eventos[0].tpAmb, 
                        1);

                    //Criar objetos das classes dos serviços dos webservices do SEFAZ
                    object oRecepcaoEvento = wsProxy.CriarObjeto("RecepcaoEvento");
                    object oCabecMsg = wsProxy.CriarObjeto("nfeCabecMsg");

                    //Atribuir conteúdo para duas propriedades da classe nfeCabecMsg
                    wsProxy.SetProp(oCabecMsg, "cUF", Empresa.Configuracoes[emp].UFCod/*oLer.oDadosEnvEvento.eventos[0].cOrgao*/.ToString());
                    wsProxy.SetProp(oCabecMsg, "versaoDados", ConfiguracaoApp.VersaoXMLEnvCCe);

                    //Criar objeto da classe de assinatura digital
                    AssinaturaDigital oAD = new AssinaturaDigital();

                    //Assinar o XML
                    oAD.Assinar(NomeArquivoXML, "infEvento", Empresa.Configuracoes[emp].X509Certificado);

                    //Invocar o método que envia o XML para o SEFAZ
                    oInvocarObj.Invocar(wsProxy, oRecepcaoEvento, "nfeRecepcaoEvento", oCabecMsg, this, ExtXml.EnvCCe.Replace(".xml",""), ExtXmlRet.retEnvCCe.Replace(".xml",""));

                    //Ler o retorno
                    LerRetornoEvento(emp);
                }
                else
                {
                    // Gerar o XML de eventos a partir do TXT gerado pelo ERP
                    oGerarXML.EnvioEvento(Path.GetFileNameWithoutExtension(NomeArquivoXML) + ExtXml.EnvCCe, oLer.oDadosEnvEvento);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var ExtRet = vXmlNfeDadosMsgEhXML ? ExtXml.EnvCCe : ExtXml.EnvCCe_TXT;
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    oAux.GravarArqErroServico(NomeArquivoXML, ExtRet, ExtXmlRet.retEnvCCe_ERR, ex);
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
                    oAux.DeletarArquivo(NomeArquivoXML);
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
                docEventoOriginal.Load(Auxiliar.StringXmlToStreamUTF8(txt.ReadToEnd()));

                MemoryStream msXml = Auxiliar.StringXmlToStream(this.vStrXmlRetorno);
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

                                                oGerarXML.XmlDistEvento(emp, chNFe, nSeqEvento, env.ParentNode.OuterXml, eleRetorno.OuterXml, dhRegEvento);

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
    }
    #endregion
}
#endif