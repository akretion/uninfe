using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Web.Services.Protocols;

namespace uninfe
{
    class UniNfeClass
    {
        public X509Certificate2 oCertificado { get; set; }
        public int vUF { get; set; } //Código do Estado (UF) que é para certificar a Nota Fiscal Eletrônica
        public int vAmbiente { get; set; } //Código do Ambiente que é para certificar a Nota Fiscal Eletrônica
        public string vXmlNfeDadosMsg { get; set; } //Arquivo XML contendo os dados a serem enviados (Nota Fiscal, Pedido de Status, Cancelamento, etc...)
        public string vStrXmlRetorno { get; private set; } //Conteúdo do XML de retorno do serviço, ou seja, para cada serviço invocado a classe seta neste atributo a string do XML Retornado pelo serviço
        public string vArqXMLRetorno { get; private set; } //Pasta e nome do arquivo dos XML´s retornados pelos WebServices, sempre que um serviço for consumido, nesta propriedade será setado o caminho e nome do arquivo XML gravado que tem o conteúdo de retorno do webservice.
        public string vPastaXMLEnvio { get; set; } //Pasta que contém os XMLs para serem enviados para o WebService
        public string vPastaXMLRetorno { get; set; } //Pasta que é para ser gravado os XML´s retornados pelo WebService
        public string vPastaXMLEnviado { get; set; } //Pasta onde vai gravar os XML´s que foram assinados e enviados

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 04/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Verificar o status do Serviço da NFe do SEFAZ em questão
         *              
         * ------------------------------------------------------------------------------
         * Definição..: StatusServico()
         * Parâmetros.: 
         *
         * ------------------------------------------------------------------------------
         * Retorno....: Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
         *              XML com o retorno que foi dado do serviço do WebService.
         *              No caso do StatusServico se tudo estiver correto retorna um XML
         *              dizendo que o serviço está em operação
         *              Se der algum erro ele grava um arquivo txt com o erro em questão.
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * oUniNfe.vUF = 51; //Setar o Estado que é para ser verificado o status do serviço
         * oUniNfe.vXmlNfeDadosMsg = "c:\pedstatus.xml";
         * oUniNfe.StatusServico();
         * this.textBox_xmlretorno.Text = oUniNfe.vNfeRetorno;
         * //
         * //O conteúdo de retorno vai ser algo mais ou menos assim:
         * //
         * // <?xml version="1.0" encoding="UTF-8"?>
         * //   <retConsStatServ xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.07">
         * //      <tpAmb>2</tpAmb>
         * //      <verAplic>1.10</verAplic>
         * //      <cStat>107</cStat>
         * //      <xMotivo>Servico em Operacao</xMotivo>
         * //      <cUF>51</cUF>
         * //      <dhRecbto>2008-06-12T11:16:55</dhRecbto>
         * //      <tMed>2</tMed>
         * //   </retConsStatServ>
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
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

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 04/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Envia o XML de lote de nota fiscal pra o SEFAZ em questão
         *              
         * ------------------------------------------------------------------------------
         * Definição..: Recepcao()
         * Parâmetros.: 
         *
         * ------------------------------------------------------------------------------
         * Retorno....: Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
         *              XML com o retorno que foi dado do serviço do WebService.
         *              No caso do Recepcao se tudo estiver correto retorna um XML
         *              dizendo que a(s) nota(s) foram recebidas com sucesso.
         *              Se der algum erro ele grava um arquivo txt com o erro em questão.
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * oUniNfe.vXmlNfeDadosMsg = "c:\nfe.xml";
         * oUniNfe.Recepcao();
         * this.textBox_xmlretorno.Text = oUniNfe.vNfeRetorno;
         * //
         * //
         * //O conteúdo de retorno vai ser algo mais ou menos assim:
         * //
         * //<?xml version="1.0" encoding="UTF-8"?>
         * //   <retEnviNFe xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.10">
         * //      <tpAmb>2</tpAmb>
         * //      <verAplic>1.10</verAplic>
         * //      <cStat>103</cStat>
         * //      <xMotivo>Lote recebido com sucesso</xMotivo>
         * //      <cUF>51</cUF>
         * //      <infRec>
         * //         <nRec>510000000106704</nRec>
         * //         <dhRecbto>2008-06-12T10:49:30</dhRecbto>
         * //         <tMed>2</tMed>
         * //      </infRec>
         * //   </retEnviNFe>
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        public void Recepcao()
        {
            //Salvar o nome do arquivo da nota fiscal
            string vNomeArqNfe = this.vXmlNfeDadosMsg;

            //Assinar o arquivo XML
            UniAssinaturaDigitalClass oAD = new UniAssinaturaDigitalClass();
            oAD.Assinar(this.vXmlNfeDadosMsg, "infNFe", this.oCertificado);

            if (oAD.vResultado == 0)
            {
                //Gerar o Lote de Notas Fiscais
                this.vXmlNfeDadosMsg = this.GerarLoteNfe();

                //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
                object oServico = null;
                this.DefObjRecepcao(ref oServico);
                if (this.InvocarObjeto("1.10", oServico, "nfeRecepcaoLote", "-env-lot", "-rec") == true)
                {
                    this.MoveDeleteArq(vNomeArqNfe, "M"); //Mover o arquivo de notas fiscais eletrônicas para a pasta de enviados
                    this.MoveDeleteArq(this.vXmlNfeDadosMsg, "D"); //Deletar o arquivo de lote de nota fiscal enviado
                }
            }
            else
            {
                this.GravarArqErroServico("-nfe.xml", "-nfe.err", oAD.vResultadoString+" ("+oAD.vResultado.ToString()+")");
            }
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 04/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Busca no WebService da NFe a situação da nota fiscal enviada
         *              
         * ------------------------------------------------------------------------------
         * Definição..: RetRecepcao()
         * Parâmetros.: 
         *
         * ------------------------------------------------------------------------------
         * Retorno....: Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
         *              XML com o retorno que foi dado do serviço do WebService.
         *              No caso do RetRecepcao se tudo estiver correto retorna um XML
         *              dizendo que o lote foi processado ou não e se as notas foram 
         *              autorizadas ou não.
         *              Se der algum erro ele grava um arquivo txt com o erro em questão.
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * oUniNfe.vXmlNfeDadosMsg = "c:\teste-ped-rec.xml";
         * oUniNfe.RetRecepcao();
         * this.textBox_xmlretorno.Text = oUniNfe.vNfeRetorno;
         * //
         * //
         * //O conteúdo de retorno vai ser algo mais ou menos assim:
         * //
         * //<?xml version="1.0" encoding="UTF-8"?>
         * //   <retEnviNFe xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.10">
         * //      <tpAmb>2</tpAmb>
         * //      <verAplic>1.10</verAplic>
         * //      <cStat>103</cStat>
         * //      <xMotivo>Lote recebido com sucesso</xMotivo>
         * //      <cUF>51</cUF>
         * //      <infRec>
         * //         <nRec>510000000106704</nRec>
         * //         <dhRecbto>2008-06-12T10:49:30</dhRecbto>
         * //         <tMed>2</tMed>
         * //      </infRec>
         * //   </retEnviNFe>
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        public void RetRecepcao()
        {
            //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
            object oServico = null;
            this.DefObjRetRecepcao(ref oServico);
            if (this.InvocarObjeto("1.10", oServico, "nfeRetRecepcao", "-ped-rec", "-pro-rec") == true) 
            {                
                //Deletar o arquivo de solicitação do serviço
                this.MoveDeleteArq(this.vXmlNfeDadosMsg, "D");
            }
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 04/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Envia o XML de consulta da situação da nota fiscal
         *              
         * ------------------------------------------------------------------------------
         * Definição..: Consulta()
         * Parâmetros.: 
         *
         * ------------------------------------------------------------------------------
         * Retorno....: Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
         *              XML com o retorno que foi dado do serviço do WebService.
         *              No caso da Consulta se tudo estiver correto retorna um XML
         *              com a situação da nota fiscal (Se autorizada ou não).
         *              Se der algum erro ele grava um arquivo txt com o erro em questão.
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * oUniNfe.vXmlNfeDadosMsg = "c:\teste-ped-sit.xml";
         * oUniNfe.Consulta();
         * this.textBox_xmlretorno.Text = oUniNfe.vNfeRetorno;
         * //
         * //
         * //O conteúdo de retorno vai ser algo mais ou menos assim:
         * //
         * //<?xml version="1.0" encoding="UTF-8" ?>
         * //   <retConsSitNFe versao="1.07" xmlns="http://www.portalfiscal.inf.br/nfe">
         * //      <infProt>
         * //         <tpAmb>2</tpAmb>
         * //         <verAplic>1.10</verAplic>
         * //         <cStat>100</cStat>
         * //         <xMotivo>Autorizado o uso da NF-e</xMotivo>
         * //         <cUF>51</cUF>
         * //         <chNFe>51080612345678901234550010000001041671821888</chNFe>
         * //         <dhRecbto>2008-06-27T15:01:48</dhRecbto>
         * //         <nProt>151080000194296</nProt>
         * //         <digVal>WHM/TzTvF+LrdUwtwvk26qgsko0=</digVal>
         * //      </infProt>
         * //   </retConsSitNFe>
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
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

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 01/07/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Envia o XML de cancelamento de nota fiscal
         *              
         * ------------------------------------------------------------------------------
         * Definição..: Cancelamento()
         * Parâmetros.: 
         *
         * ------------------------------------------------------------------------------
         * Retorno....: Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
         *              XML com o retorno que foi dado do serviço do WebService.
         *              No caso do Cancelamento se tudo estiver correto retorna um XML
         *              dizendo se foi cancelado corretamente ou não.
         *              Se der algum erro ele grava um arquivo txt com o erro em questão.
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * oUniNfe.vXmlNfeDadosMsg = "c:\teste-ped-sit.xml";
         * oUniNfe.Consulta();
         * this.textBox_xmlretorno.Text = oUniNfe.vNfeRetorno;
         * //
         * //
         * //O conteúdo de retorno vai ser algo mais ou menos assim:
         * //
         * //<?xml version="1.0" encoding="UTF-8" ?> 
         * //<retCancNFe xmlns="http://www.portalfiscal.inf.br/nfe" xmlns:ds="http://www.w3.org/2000/09/xmldsig#" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemaLocation="http://www.portalfiscal.inf.br/nfe retCancNFe_v1.07.xsd" versao="1.07">
         * //   <infCanc>
         * //      <tpAmb>2</tpAmb> 
         * //      <verAplic>1.10</verAplic> 
         * //      <cStat>101</cStat> 
         * //      <xMotivo>Cancelamento de NF-e homologado</xMotivo> 
         * //      <cUF>51</cUF> 
         * //      <chNFe>51080612345678901234550010000001041671821888</chNFe> 
         * //      <dhRecbto>2008-07-01T16:37:22</dhRecbto> 
         * //      <nProt>151080000197648</nProt> 
         * //   </infCanc>
         * //</retCancNFe>
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        public void Cancelamento()
        {
            //Assinar o arquivo XML
            UniAssinaturaDigitalClass oAD = new UniAssinaturaDigitalClass();
            oAD.Assinar(this.vXmlNfeDadosMsg, "infCanc", this.oCertificado);

            if (oAD.vResultado == 0)
            {
                //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
                object oServico = null;
                this.DefObjCancelamento(ref oServico);
                if (this.InvocarObjeto("1.07", oServico, "nfeCancelamentoNF", "-ped-can", "-can") == true)
                {
                    //Deletar o arquivo de solicitação do serviço
                    this.MoveDeleteArq(this.vXmlNfeDadosMsg, "M");
                }
            }
            else
            {
                this.GravarArqErroServico("-ped-can.xml", "-can.err", oAD.vResultadoString + " (" + oAD.vResultado.ToString() + ")");
            }
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 01/07/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Envia o XML de inutilização de numeração de notas fiscais
         *              
         * ------------------------------------------------------------------------------
         * Definição..: Inutilizacao()
         * Parâmetros.: 
         *
         * ------------------------------------------------------------------------------
         * Retorno....: Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
         *              XML com o retorno que foi dado do serviço do WebService.
         *              No caso da Inutilização se tudo estiver correto retorna um XML
         *              dizendo se foi inutilizado corretamente ou não.
         *              Se der algum erro ele grava um arquivo txt com o erro em questão.
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * oUniNfe.vXmlNfeDadosMsg = "c:\teste-ped-sit.xml";
         * oUniNfe.Inutilizacao();
         * this.textBox_xmlretorno.Text = oUniNfe.vNfeRetorno;
         * //
         * //
         * //O conteúdo de retorno vai ser algo mais ou menos assim:
         * //
         * //<?xml version="1.0" encoding="UTF-8" ?> 
         * //<retInutNFe xmlns="http://www.portalfiscal.inf.br/nfe" xmlns:ds="http://www.w3.org/2000/09/xmldsig#" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemaLocation="http://www.portalfiscal.inf.br/nfe retInutNFe_v1.07.xsd" versao="1.07">
         * //   <infInut>
         * //      <tpAmb>2</tpAmb> 
         * //      <verAplic>1.10</verAplic> 
         * //      <cStat>102</cStat> 
         * //      <xMotivo>Inutilizacao de numero homologado</xMotivo> 
         * //      <cUF>51</cUF> 
         * //      <ano>08</ano> 
         * //      <CNPJ>12345678901234</CNPJ> 
         * //      <mod>55</mod> 
         * //      <serie>1</serie> 
         * //      <nNFIni>101</nNFIni> 
         * //      <nNFFin>101</nNFFin> 
         * //      <dhRecbto>2008-07-01T16:47:11</dhRecbto> 
         * //      <nProt>151080000197712</nProt> 
         * //   </infInut>
         * //</retInutNFe>
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        public void Inutilizacao()
        {
            //Assinar o arquivo XML
            UniAssinaturaDigitalClass oAD = new UniAssinaturaDigitalClass();
            oAD.Assinar(this.vXmlNfeDadosMsg, "infInut", this.oCertificado);

            if (oAD.vResultado == 0)
            {
                //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
                object oServico = null;
                this.DefObjInutilizacao(ref oServico);
                if (this.InvocarObjeto("1.07", oServico, "nfeInutilizacaoNF", "-ped-inu", "-inu") == true)
                {
                    //Deletar o arquivo de solicitação do serviço
                    this.MoveDeleteArq(this.vXmlNfeDadosMsg, "M");
                }
            }
            else
            {
                this.GravarArqErroServico("-ped-inu.xml", "-inu.err", oAD.vResultadoString + " (" + oAD.vResultado.ToString() + ")");
            }
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 01/07/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Invoca o método do objeto passado por parâmetro para fazer
         *              acesso aos WebServices do SEFAZ
         *              
         * ------------------------------------------------------------------------------
         * Definição..: InvocarMetodo(string, object, string, string, string)
         * Parâmetros.: cVersaoDados - Versão dos dados que será enviado para o 
         *                             WebService
         *              oObjeto      - Nome do Objeto do WebService que vai ser acessado
         *              cMetodo      - Nome do método que vai ser utilizado para acessar
         *                             o WebService
         *              cFinalArqEnvio   - string do final do arquivo a ser enviado.
         *                                 Sem a extensão ".xml"
         *              cFinalArqRetorno - string do final do arquivo a ser gravado 
         *                                 com o conteúdo do retorno. 
         *                                 Sem a extensão ".xml"
         *
         * ------------------------------------------------------------------------------
         * Retorno....: Atualiza a propriedade this.vNfeRetorno da classe com o conteúdo
         *              XML com o retorno que foi dado do serviço do WebService.
         *              Se der algum erro ele grava um arquivo txt com o erro em questão.
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         *  //Definir qual objeto será utilizado, ou seja, de qual estado (UF)
         *  object oServico = null;
         *  this.DefObjCancelamento(ref oServico);
         *  this.InvocarObjeto("1.07", oServico, "nfeCancelamentoNF", "-ped-can", "-can");
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        private bool InvocarObjeto(string cVersaoDados, object oServico, string cMetodo, string cFinalArqEnvio, string cFinalArqRetorno)
        {
            bool lRetorna;

            // Passo 1: Declara variável (tipo string) com o conteúdo do Cabecalho da mensagem a ser enviada para o webservice
            string vNFeCabecMsg = this.GerarXMLCabecMsg(cVersaoDados);

            // Passo 2: Montar o XML de Lote de envio de Notas fiscais
            string vNFeDadosMsg = this.XmlToString(this.vXmlNfeDadosMsg);

            // Passo 3: Passar para o Objeto qual vai ser o certificado digital que ele deve utilizar             
            this.RelacionarCertObj(oServico);

            // Passo 4: Limpa a variável de retorno
            this.vStrXmlRetorno = string.Empty;

            try
            {
                // Passo 5: (Invoke) Faz a chamada ao método de envio de Lote de NF-e, recebendo o resultado do processo em variável.
                Type tipoServico = oServico.GetType();
                this.vStrXmlRetorno = (string)(tipoServico.InvokeMember(cMetodo, System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));

                // Passo 6 e 7: Registra o retorno de acordo com o status obtido e Exclui o XML de solicitaÃ§Ã£o do serviÃ§o
                this.GravarXmlRetorno(cFinalArqEnvio+".xml",cFinalArqRetorno+".xml");

                lRetorna = true;
            }

            catch (Exception ex)
            {
                // Passo alternativo: Registra o retorno no sistema interno, de acordo com a exceção
                this.GravarArqErroServico(cFinalArqEnvio+".xml", cFinalArqRetorno+".err", ex.ToString());

                lRetorna = false;
            }

            return lRetorna;
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 24/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Pega o arquivo XML de nota fiscal e a partir dele monta o XML
         *              de lote de nota fiscal, é este que tem que ser enviado para
         *              o webservice NfeRecepcao.
         *              
         * ------------------------------------------------------------------------------
         * Definição..: GerarLoteNfe(),string
         * Parâmetros.: 
         *
         * ------------------------------------------------------------------------------
         * Retorno....: Como retorno temos o seguinte:
         *              Retorna o nome do arquivo XML de lote de nfe gerado
         *              
         *              - Além do retorno acima o método gera 2 arquivos XML:
         *              
         *                1º) Um XML contendo o número do lote utilizado para que o
         *                    sistema ERP que gerou a Nfe possa pegar como retorno
         *                    e saber o número do lote para depois identificar o 
         *                    nome do arquivo de retorno do sefaz.
         *                    O nome do arquivo terá o final -num-lot.xml
         *                    
         *                2º) Gera um arquivo XML do lote das notas fiscais deixando 
         *                    o mesmo preparado para o envido ao webservice NfeRecepcao
         *                    Nome do arquivo é Número do lote com o final "-env-lot.xml"
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * oNfe.vXmlNfeDadosMsg = "51080612345678901234550010000001041671821888-nfe.xml";
         * MessageBox.Show(oNfe.GerarLoteNfe()); //Demonstra o nome do arquivo XML do 
         *                                       //lote gerado (000000000000134-env-lot.xml)
         * 
         * //Arquivos de retorno gerados:
         * //
         * //O arquivo que contém o número do lote para o sistema ERP
         * //É o mesmo nome do arquivo de nfe mudando somente o final de -nfe.xml para 
         * //-num-lot.xml
         * //51080612345678901234550010000001041671821888-num-lot.xml
         * //
         * //O arquivo de lote para ser enviado ao webservice NfeRecepcao
         * //É o número do lote seguido do -env-lot.xml
         * //000000000000134-env-lot.xml
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        private string GerarLoteNfe()
        {
            string cVersaoDados = "1.10";

            //Montar a string da Nfe a ser montado o Lote
            string vNfeDadosMsg = this.XmlToString(this.vXmlNfeDadosMsg);

            //Pegar o último número de lote de NFe utilizado e acrescentar + 1 para novo envio
            string vArqXmlLote = "UniNfeLote.xml";
            Int32 nNumeroLote = 1;

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
                            oLerXml.Read(); nNumeroLote = Convert.ToInt32(oLerXml.Value) + 1;
                            break;
                        }
                    }
                }
                oLerXml.Close();
            }

            //Salvar o número de lote de NFe utilizado
            XmlWriterSettings oSettings = new XmlWriterSettings();
            oSettings.Indent = true;
            oSettings.IndentChars = "";
            oSettings.NewLineOnAttributes = false;
            oSettings.OmitXmlDeclaration = false;

            XmlWriter oXmlGravar = XmlWriter.Create("UniNfeLote.xml", oSettings);

            oXmlGravar.WriteStartDocument();
            oXmlGravar.WriteStartElement("DadosLoteNfe");
            oXmlGravar.WriteElementString("UltimoLoteEnviado", nNumeroLote.ToString());
            oXmlGravar.WriteEndElement(); //DadosLoteNfe
            oXmlGravar.WriteEndDocument();
            oXmlGravar.Flush();
            oXmlGravar.Close();

            //Separar somente o conteúdo a partir da tag <NFe> até </NFe>
            Int32 nPosI = vNfeDadosMsg.IndexOf("<NFe");
            Int32 nPosF = vNfeDadosMsg.Length - nPosI;
            string vStringNfe = vNfeDadosMsg.Substring(nPosI, nPosF);

            //Montar a parte do XML referente ao Lote e acrescentar a Nota Fiscal
            string vStringLoteNfe = string.Empty;
            vStringLoteNfe += "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            vStringLoteNfe += "<enviNFe xmlns=\"http://www.portalfiscal.inf.br/nfe\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" versao=\""+cVersaoDados+"\">";
            vStringLoteNfe += "<idLote>" + nNumeroLote.ToString("000000000000000") + "</idLote>";
            vStringLoteNfe += vStringNfe;
            vStringLoteNfe += "</enviNFe>";

            //Gravar o XML do lote das notas fiscais
            string vNomeArqLoteNfe = this.vPastaXMLEnvio + "\\" +
                                     nNumeroLote.ToString("000000000000000") +
                                     "-env-lot.xml";

            StreamWriter SW_2 = File.CreateText(vNomeArqLoteNfe);
            SW_2.Write(vStringLoteNfe);
            SW_2.Close();

            //Gerar o XML de retorno para o ERP com o número do Lote de Nfe Utilizado
            string cArqLoteRetorno = this.vPastaXMLRetorno + "\\" +
                                     this.ExtrairNomeArq(this.vXmlNfeDadosMsg,"-nfe.xml") +
                                     "-num-lot.xml";

            XmlWriter oXmlLoteERP = XmlWriter.Create(cArqLoteRetorno, oSettings);

            oXmlLoteERP.WriteStartDocument();
            oXmlLoteERP.WriteStartElement("DadosLoteNfe");
            oXmlLoteERP.WriteElementString("NumeroLoteGerado", nNumeroLote.ToString());
            oXmlLoteERP.WriteEndElement(); //DadosLoteNfe
            oXmlLoteERP.WriteEndDocument();
            oXmlLoteERP.Flush();
            oXmlLoteERP.Close();

            return vNomeArqLoteNfe;
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 04/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Gera uma string com o XML do cabeçalho dos dados a serem enviados
         *              para os serviços da NFe
         *              
         * ------------------------------------------------------------------------------
         * Definição..: GerarXMLCabecMsg(string),string
         * Parâmetros.: pVersaoDados - Versão do arquivo XML que será enviado para os
         *                             WebServices. Esta versão varia de serviço para
         *                             serviço e deve ser consultada no manual de
         *                             integração da NFE
         *
         * ------------------------------------------------------------------------------
         * Retorno....: Retorna uma string com o XML do cabeçalho dos dados a serem
         *              enviados para os serviços da NFe
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         *              vCabecMSG = GerarXMLCabecMsg("1.07");
         *              MessageBox.Show( vCabecMSG );
         *              
         *              //O conteúdo que será demonstrado no MessageBox é:
         *              
         *                <?xml version="1.0" encoding="UTF-8" ?>
         *                <cabecMsg xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.02">
         *                   <versaoDados>1.07</versaoDados>
         *                </cabecMsg>
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        private string GerarXMLCabecMsg(string pVersaoDados)
        {
            string vCabecMsg = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><cabecMsg xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"1.02\"><versaoDados>" + pVersaoDados + "</versaoDados></cabecMsg>";

            return vCabecMsg;
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 17/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Cria um arquivo XML com a estrutura necessária para consultar 
         *              o Status do Serviço
         *              
         * ------------------------------------------------------------------------------
         * Definição..: CriaArqXMLStatusServico(),string
         * Parâmetros.: 
         * 
         * ------------------------------------------------------------------------------
         * Retorno....: Retorna o caminho e nome do arquivo criado.
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         *              string vPastaArq = this.CriaArqXMLStatusServico();
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        private string CriarArqXMLStatusServico()
        {
            string vDadosMsg = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><consStatServ xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" versao=\"1.07\" xmlns=\"http://www.portalfiscal.inf.br/nfe\"><tpAmb>"+this.vAmbiente.ToString()+"</tpAmb><cUF>"+this.vUF.ToString()+"</cUF><xServ>STATUS</xServ></consStatServ>";

            string _arquivo_saida = this.vPastaXMLEnvio + "\\" + DateTime.Now.ToString("yyyyMMddThhmmss") + "-ped-sta.xml";

            StreamWriter SW = File.CreateText(_arquivo_saida);
            SW.Write(vDadosMsg);
            SW.Close();

            return _arquivo_saida;
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 17/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Verifica e retorna o Status do Serviço da NFE. Para isso este 
         *              método gera o arquivo XML necessário para obter o status do 
         *              serviço e faz a leitura do XML de retorno, retornando uma 
         *              string com a mensagem obtida.
         *              
         * ------------------------------------------------------------------------------
         * Definição..: VerStatusServico(),string
         * Parâmetros.: 
         * 
         * ------------------------------------------------------------------------------
         * Retorno....: Retorna a mensagem obtida de retorno do webservice da nfe.
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         *              string vPastaArq = this.CriaArqXMLStatusServico();
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        public string VerStatusServico()
        {
            string vStatus     = "Ocorreu uma falha ao tentar obter a situação do serviço. Aguarde um momento e tente novamente.";

            //Criar XML para obter o status do serviço
            this.vXmlNfeDadosMsg = this.CriarArqXMLStatusServico();

            //Obter o status do serviço
            this.StatusServico();

            if (File.Exists(this.vArqXMLRetorno))
            {
                //Ler o status do serviço no XML retornado pelo WebService
                XmlTextReader oLerXml = new XmlTextReader(this.vArqXMLRetorno);

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
                oLerXml.Close();

                //Detetar o arquivo de retorno
                FileInfo oArquivoDel = new FileInfo(this.vArqXMLRetorno);
                oArquivoDel.Delete();
            }

            //Retornar o status do serviço
            return vStatus;
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 04/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Método responsável por ler o conteúdo de um XML e retornar em uma
         *              string
         * ------------------------------------------------------------------------------
         * Definição..: XMLtoString( string ),string
         * Parâmetros.: parNomeArquivo - Caminho e nome do arquivo XML que é para pegar
         *                               o conteúdo e retornar na string.
         * 
         * ------------------------------------------------------------------------------
         * Retorno....: Retorna uma string com o conteúdo do arquivo XML
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         *              string ConteudoXML;
         *              ConteudoXML = THIS.XmltoString( @"c:\arquivo.xml" );
         *              MessageBox.Show( ConteudoXML );
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        public string XmlToString(string parNomeArquivo)
        {
            StreamReader SR;
            SR = File.OpenText(parNomeArquivo);
            string conteudo_xml = SR.ReadToEnd();
            SR.Close();

            return conteudo_xml;
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 04/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Método responsável por abrir um browse para selecionar o 
         *              o certificado digital que será utilizado para autenticação
         *              dos WebServices e gravar ele no atributo oCertificado
         * ------------------------------------------------------------------------------
         * Definição..: SelecionarCertificado(),bool
         * Parâmetros.: 
         * 
         * ------------------------------------------------------------------------------
         * Retorno....: Retorna se o certificado foi selecionado corretamente ou não
         *              true  = foi selecionado corretamente
         *              false = não foi selecionado, algum problema ocorreu ou foi 
         *                      cancelado o selecionamento pelo usuário
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        public bool SelecionarCertificado()
        {
            bool vRetorna;

            X509Certificate2 oX509Cert = new X509Certificate2();
            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            X509Certificate2Collection collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
            X509Certificate2Collection collection2 = (X509Certificate2Collection)collection.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);
            X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(collection2, "Certificado(s) Digital(is) disponível(is)", "Selecione o certificado digital para uso no aplicativo", X509SelectionFlag.SingleSelection);

            if (scollection.Count == 0)
            {
                string msgResultado = "Nenhum certificado digital foi selecionado ou o certificado selecionado está com problemas.";
                MessageBox.Show(msgResultado, "Advertência", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                vRetorna = false;
            }
            else
            {
                oX509Cert = scollection[0];
                oCertificado = oX509Cert;
                vRetorna = true;
            }

            return vRetorna;
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 04/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Exibi uma tela com o certificado digital selecionado para ser
         *              utilizado na integração com os WEBServices da NFe
         *              
         * ------------------------------------------------------------------------------
         * Definição..: ExibirCertSel()
         * Parâmetros.: 
         * 
         * ------------------------------------------------------------------------------
         * Retorno....: 
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        public void ExibirCertSel()
        {
            if (oCertificado == null)
            {
                MessageBox.Show("Nenhum certificado foi selecionado.", "Advertência", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                X509Certificate2UI.DisplayCertificate(oCertificado);
            }
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 17/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Defini o Objeto que vai ser utilizado para consultar o status do 
         *              serviço de acordo com o Estado e Ambiente informado para a classe
         *              
         * ------------------------------------------------------------------------------
         * Definição..: DefObjStatusServico( ref object )
         * Parâmetros.: pObjeto = Variável de objeto que é para receber a instancia
         *                        do serviço.
         *                        
         * ------------------------------------------------------------------------------
         * Retorno....: 
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * object oServico = null;
         * this.DefObjStatusServico(ref oServico);                       
         * Type tipoServico = oServico.GetType();
         * 
         * object oClientCertificates;
         * Type tipoClientCertificates;
         * 
         * oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, oServico, new Object[] { });
         * tipoClientCertificates = oClientCertificates.GetType();
         * tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
         *          
         * this.vNfeRetorno = string.Empty;
         * 
         * this.vNfeRetorno = (string)(tipoServico.InvokeMember("nfeStatusServicoNF", System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        private void DefObjStatusServico(ref object pObjeto)
        {
            if (this.vAmbiente == 1)
            {
                if (this.vUF == 51) { pObjeto = new wsMTPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 43) { pObjeto = new wsRSPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 31) { pObjeto = new wsMGPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 35) { pObjeto = new wsSPPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 50) { pObjeto = new wsMSPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 52) { pObjeto = new wsGOPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 41) { pObjeto = new wsVNPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 42) { pObjeto = new wsVRPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 13) { pObjeto = new wsVRPStatusServico.NfeStatusServico(); }
                else if (this.vUF == 17) { pObjeto = new wsVRPStatusServico.NfeStatusServico(); }
            }
            else if (this.vAmbiente == 2)
            {
                if (this.vUF == 51) { pObjeto = new wsMTHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 43) { pObjeto = new wsRSHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 31) { pObjeto = new wsMGHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 35) { pObjeto = new wsSPHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 50) { pObjeto = new wsMSHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 52) { pObjeto = new wsGOHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 41) { pObjeto = new wsVNHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 42) { pObjeto = new wsVRHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 13) { pObjeto = new wsVRHStatusServico.NfeStatusServico(); }
                else if (this.vUF == 17) { pObjeto = new wsVRHStatusServico.NfeStatusServico(); }
            }
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 17/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Defini o Objeto que vai ser utilizado para enviar a nota fiscal
         *              de acordo com o Estado e Ambiente informado para a classe
         *              
         * ------------------------------------------------------------------------------
         * Definição..: DefObjRecepcao( ref object )
         * Parâmetros.: pObjeto = Variável de objeto que é para receber a instancia
         *                        do serviço.
         *                        
         * ------------------------------------------------------------------------------
         * Retorno....: 
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * object oServico = null;
         * this.DefObjRecepcao(ref oServico);                       
         * Type tipoServico = oServico.GetType();
         * 
         * object oClientCertificates;
         * Type tipoClientCertificates;
         * 
         * oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, oServico, new Object[] { });
         * tipoClientCertificates = oClientCertificates.GetType();
         * tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
         *          
         * this.vNfeRetorno = string.Empty;
         * 
         * this.vNfeRetorno = (string)(tipoServico.InvokeMember("nfeRecepcao", System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        private void DefObjRecepcao(ref object pObjeto)
        {
            if (this.vAmbiente == 1)
            {
                if (this.vUF == 51) { pObjeto = new wsMTPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 43) { pObjeto = new wsRSPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 31) { pObjeto = new wsMGPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 35) { pObjeto = new wsSPPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 50) { pObjeto = new wsMSPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 52) { pObjeto = new wsGOPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 41) { pObjeto = new wsVNPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 42) { pObjeto = new wsVRPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 13) { pObjeto = new wsVRPRecepcao.NfeRecepcao(); }
                else if (this.vUF == 17) { pObjeto = new wsVRPRecepcao.NfeRecepcao(); }
            }
            else if (this.vAmbiente == 2)
            {
                if (this.vUF == 51) { pObjeto = new wsMTHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 43) { pObjeto = new wsRSHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 31) { pObjeto = new wsMGHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 35) { pObjeto = new wsSPHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 50) { pObjeto = new wsMSHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 52) { pObjeto = new wsGOHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 41) { pObjeto = new wsVNHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 42) { pObjeto = new wsVRHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 13) { pObjeto = new wsVRHRecepcao.NfeRecepcao(); }
                else if (this.vUF == 17) { pObjeto = new wsVRHRecepcao.NfeRecepcao(); }
            }
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 17/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Defini o Objeto que vai ser utilizado para receber o retorno 
         *              da situação da nota fiscal de acordo com o Estado e Ambiente 
         *              informado para a classe
         *              
         * ------------------------------------------------------------------------------
         * Definição..: DefObjRetRecepcao( ref object )
         * Parâmetros.: pObjeto = Variável de objeto que é para receber a instancia
         *                        do serviço.
         *                        
         * ------------------------------------------------------------------------------
         * Retorno....: 
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * object oServico = null;
         * this.DefObjRetRecepcao(ref oServico);                       
         * Type tipoServico = oServico.GetType();
         * 
         * object oClientCertificates;
         * Type tipoClientCertificates;
         * 
         * oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, oServico, new Object[] { });
         * tipoClientCertificates = oClientCertificates.GetType();
         * tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
         *          
         * this.vNfeRetorno = string.Empty;
         * 
         * this.vNfeRetorno = (string)(tipoServico.InvokeMember("nfeRetRecepcao", System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        private void DefObjRetRecepcao(ref object pObjeto)
        {
            if (this.vAmbiente == 1)
            {
                if (this.vUF == 51) { pObjeto = new wsMTPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 43) { pObjeto = new wsRSPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 31) { pObjeto = new wsMGPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 35) { pObjeto = new wsSPPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 50) { pObjeto = new wsMSPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 52) { pObjeto = new wsGOPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 41) { pObjeto = new wsVNPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 42) { pObjeto = new wsVRPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 13) { pObjeto = new wsVRPRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 17) { pObjeto = new wsVRPRetRecepcao.NfeRetRecepcao(); }
            }
            else if (this.vAmbiente == 2)
            {
                if (this.vUF == 51) { pObjeto = new wsMTHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 43) { pObjeto = new wsRSHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 31) { pObjeto = new wsMGHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 35) { pObjeto = new wsSPHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 50) { pObjeto = new wsMSHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 52) { pObjeto = new wsGOHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 41) { pObjeto = new wsVNHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 42) { pObjeto = new wsVRHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 13) { pObjeto = new wsVRHRetRecepcao.NfeRetRecepcao(); }
                else if (this.vUF == 17) { pObjeto = new wsVRHRetRecepcao.NfeRetRecepcao(); }
            }
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 17/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Defini o Objeto que vai ser utilizado para consultar   
         *              a situação da nota fiscal de acordo com o Estado e Ambiente 
         *              informado para a classe
         *              
         * ------------------------------------------------------------------------------
         * Definição..: DefObjConsulta( ref object )
         * Parâmetros.: pObjeto = Variável de objeto que é para receber a instancia
         *                        do serviço.
         *                        
         * ------------------------------------------------------------------------------
         * Retorno....: 
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * object oServico = null;
         * this.DefObjConsulta(ref oServico);                       
         * Type tipoServico = oServico.GetType();
         * 
         * object oClientCertificates;
         * Type tipoClientCertificates;
         * 
         * oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, oServico, new Object[] { });
         * tipoClientCertificates = oClientCertificates.GetType();
         * tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
         *          
         * this.vNfeRetorno = string.Empty;
         * 
         * this.vNfeRetorno = (string)(tipoServico.InvokeMember("nfeConsultaNF", System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        private void DefObjConsulta(ref object pObjeto)
        {
            if (this.vAmbiente == 1)
            {
                if (this.vUF == 51) { pObjeto = new wsMTPConsulta.NfeConsulta(); }
                else if (this.vUF == 43) { pObjeto = new wsRSPConsulta.NfeConsulta(); }
                else if (this.vUF == 31) { pObjeto = new wsMGPConsulta.NfeConsulta(); }
                else if (this.vUF == 35) { pObjeto = new wsSPPConsulta.NfeConsulta(); }
                else if (this.vUF == 50) { pObjeto = new wsMSPConsulta.NfeConsulta(); }
                else if (this.vUF == 52) { pObjeto = new wsGOPConsulta.NfeConsulta(); }
                else if (this.vUF == 41) { pObjeto = new wsVNPConsulta.NfeConsulta(); }
                else if (this.vUF == 42) { pObjeto = new wsVRPConsulta.NfeConsulta(); }
                else if (this.vUF == 13) { pObjeto = new wsVRPConsulta.NfeConsulta(); }
                else if (this.vUF == 17) { pObjeto = new wsVRPConsulta.NfeConsulta(); }
            }
            else if (this.vAmbiente == 2)
            {
                if (this.vUF == 51) { pObjeto = new wsMTHConsulta.NfeConsulta(); }
                else if (this.vUF == 43) { pObjeto = new wsRSHConsulta.NfeConsulta(); }
                else if (this.vUF == 31) { pObjeto = new wsMGHConsulta.NfeConsulta(); }
                else if (this.vUF == 35) { pObjeto = new wsSPHConsulta.NfeConsulta(); }
                else if (this.vUF == 50) { pObjeto = new wsMSHConsulta.NfeConsulta(); }
                else if (this.vUF == 52) { pObjeto = new wsGOHConsulta.NfeConsulta(); }
                else if (this.vUF == 41) { pObjeto = new wsVNHConsulta.NfeConsulta(); }
                else if (this.vUF == 42) { pObjeto = new wsVRHConsulta.NfeConsulta(); }
                else if (this.vUF == 13) { pObjeto = new wsVRHConsulta.NfeConsulta(); }
                else if (this.vUF == 17) { pObjeto = new wsVRHConsulta.NfeConsulta(); }
            }
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 01/07/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Defini o Objeto que vai ser utilizado para cancelar notas fiscais
         *              de acordo com o Estado e Ambiente informado para a classe
         *              
         * ------------------------------------------------------------------------------
         * Definição..: DefObjCancelamento( ref object )
         * Parâmetros.: pObjeto = Variável de objeto que é para receber a instancia
         *                        do serviço.
         *                        
         * ------------------------------------------------------------------------------
         * Retorno....: 
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * object oServico = null;
         * this.DefObjCancelamento(ref oServico);                       
         * Type tipoServico = oServico.GetType();
         * 
         * object oClientCertificates;
         * Type tipoClientCertificates;
         * 
         * oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, oServico, new Object[] { });
         * tipoClientCertificates = oClientCertificates.GetType();
         * tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
         *          
         * this.vNfeRetorno = string.Empty;
         * 
         * this.vNfeRetorno = (string)(tipoServico.InvokeMember("nfeCancelamentoNF", System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        private void DefObjCancelamento(ref object pObjeto)
        {
            if (this.vAmbiente == 1)
            {
                if (this.vUF == 51) { pObjeto = new wsMTPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 43) { pObjeto = new wsRSPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 31) { pObjeto = new wsMGPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 35) { pObjeto = new wsSPPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 50) { pObjeto = new wsMSPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 52) { pObjeto = new wsGOPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 41) { pObjeto = new wsVNPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 42) { pObjeto = new wsVRPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 13) { pObjeto = new wsVRPCancelamento.NfeCancelamento(); }
                else if (this.vUF == 17) { pObjeto = new wsVRPCancelamento.NfeCancelamento(); }
            }
            else if (this.vAmbiente == 2)
            {
                if (this.vUF == 51) { pObjeto = new wsMTHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 43) { pObjeto = new wsRSHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 31) { pObjeto = new wsMGHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 35) { pObjeto = new wsSPHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 50) { pObjeto = new wsMSHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 52) { pObjeto = new wsGOHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 41) { pObjeto = new wsVNHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 42) { pObjeto = new wsVRHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 13) { pObjeto = new wsVRHCancelamento.NfeCancelamento(); }
                else if (this.vUF == 17) { pObjeto = new wsVRHCancelamento.NfeCancelamento(); }
            }
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 01/07/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Defini o Objeto que vai ser utilizado para inutilizar números 
         *              de notas fiscais de acordo com o Estado e Ambiente informado 
         *              para a classe
         *              
         * ------------------------------------------------------------------------------
         * Definição..: DefObjInutilizacao( ref object )
         * Parâmetros.: pObjeto = Variável de objeto que é para receber a instancia
         *                        do serviço.
         *                        
         * ------------------------------------------------------------------------------
         * Retorno....: 
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * object oServico = null;
         * this.DefObjInutilizacao(ref oServico);                       
         * Type tipoServico = oServico.GetType();
         * 
         * object oClientCertificates;
         * Type tipoClientCertificates;
         * 
         * oClientCertificates = tipoServico.InvokeMember("ClientCertificates", System.Reflection.BindingFlags.GetProperty, null, oServico, new Object[] { });
         * tipoClientCertificates = oClientCertificates.GetType();
         * tipoClientCertificates.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, oClientCertificates, new Object[] { this.oCertificado });
         *          
         * this.vNfeRetorno = string.Empty;
         * 
         * this.vNfeRetorno = (string)(tipoServico.InvokeMember("nfeInutilizacaoNF", System.Reflection.BindingFlags.InvokeMethod, null, oServico, new Object[] { vNFeCabecMsg, vNFeDadosMsg }));
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        private void DefObjInutilizacao(ref object pObjeto)
        {
            if (this.vAmbiente == 1)
            {
                if (this.vUF == 51) { pObjeto = new wsMTPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 43) { pObjeto = new wsRSPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 31) { pObjeto = new wsMGPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 35) { pObjeto = new wsSPPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 50) { pObjeto = new wsMSPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 52) { pObjeto = new wsGOPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 41) { pObjeto = new wsVNPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 42) { pObjeto = new wsVRPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 13) { pObjeto = new wsVRPInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 17) { pObjeto = new wsVRPInutilizacao.NfeInutilizacao(); }
            }
            else if (this.vAmbiente == 2)
            {
                if (this.vUF == 51) { pObjeto = new wsMTHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 43) { pObjeto = new wsRSHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 31) { pObjeto = new wsMGHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 35) { pObjeto = new wsSPHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 50) { pObjeto = new wsMSHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 52) { pObjeto = new wsGOHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 41) { pObjeto = new wsVNHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 42) { pObjeto = new wsVRHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 13) { pObjeto = new wsVRHInutilizacao.NfeInutilizacao(); }
                else if (this.vUF == 17) { pObjeto = new wsVRHInutilizacao.NfeInutilizacao(); }
            }
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 19/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Extrai somente o nome do arquivo de uma string para ser utilizado
         *              na situação desejada. Veja os exemplos.
         *              
         * ------------------------------------------------------------------------------
         * Definição..: ExtrairNomeArq(string,string),string
         * Parâmetros.: pPastaArq = String contendo o caminho e nome do arquivo que é
         *                          para ser extraido o nome.
         *              pFinalArq = String contendo o final do nome do arquivo até onde
         *                          é para ser extraido.
         *                        
         * ------------------------------------------------------------------------------
         * Retorno....: Retorna somente o some do arquivo de acordo com os parametros
         *              passados - veja exemplos.
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * MessageBox.Show(this.ExtrairNomeArq("C:\\TESTE\\NFE\\ENVIO\\ArqSituacao-ped-sta.xml", "-ped-sta.xml"));
         * //Será demonstrado no message a string "ArqSituacao"
         * 
         * MessageBox.Show(this.ExtrairNomeArq("C:\\TESTE\\NFE\\ENVIO\\ArqSituacao-ped-sta.xml", ".xml"));
         * //Será demonstrado no message a string "ArqSituacao-ped-sta"
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        public string ExtrairNomeArq( string pPastaArq, string pFinalArq )
        {
            //Achar o posição inicial do nome do arquivo
            //procura por pastas, tira elas para ficar somente o nome do arquivo
            Int32 nAchou = 0;
            Int32 nPosI = 0;
            for (Int32 nCont = 0; nCont < pPastaArq.Length; nCont++)
            {
                nAchou = pPastaArq.IndexOf("\\", nCont);
                if (nAchou > 0)
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

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 19/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Relaciona o certificdo digital a ser utilizado na autenticação  
         *              com o objeto do serviço.
         *              
         * ------------------------------------------------------------------------------
         * Definição..: RelacionarCertObj( type, Object )
         * Parâmetros.: ptipoObjeto = Tipo do objeto que é para ser relacionado o   
         *                            certificado
         *              pObjeto     = Objeto que é para ser relacionado o certificado              
         *                            
         * ------------------------------------------------------------------------------
         * Retorno....: Retorna somente o some do arquivo de acordo com os parametros
         *              passados - veja exemplos.
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
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

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 19/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Grava o XML com os dados do retorno dos webservices e deleta
         *              o XML de solicitação do serviço.
         *              
         * ------------------------------------------------------------------------------
         * Definição..: GravarXmlRetorno( string, string )
         * Parâmetros.: pFinalArqEnvio = Final do nome do arquivo de solicitação do 
         *                               serviço.
         *              pFinalArqRetorno = Final do nome do arquivo que é para ser
         *                                 gravado o retorno.
         *                            
         * ------------------------------------------------------------------------------
         * Retorno....: 
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * // Arquivo de envio: 20080619T19113320-ped-sta.xml
         * // Arquivo de retorno que vai ser gravado: 20080619T19113320-sta.xml
         * this.GravarXmlRetorno("-ped-sta.xml", "-sta.xml");
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        private void GravarXmlRetorno(string pFinalArqEnvio, string pFinalArqRetorno)
        {
            //Gravar o arquivo XML
            StreamWriter SW;
            this.vArqXMLRetorno = this.vPastaXMLRetorno + "\\" +
                                  this.ExtrairNomeArq(this.vXmlNfeDadosMsg, pFinalArqEnvio) +
                                  pFinalArqRetorno;
            SW = File.CreateText(this.vArqXMLRetorno);
            SW.Write(this.vStrXmlRetorno);
            SW.Close();
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 24/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Grava um arquivo Texto com um erro ocorrido na invocação dos 
         *              WebServices. Este arquivo é gravado para que o sistema ERP saiba
         *              o que está acontecendo.
         *              
         * ------------------------------------------------------------------------------
         * Definição..: GravaArqErroServico(string, string, string)
         * Parâmetros.: pFinalArqEnvio   = Final do nome do arquivo de solicitação do 
         *                                 serviço.
         *              pFinalArqRetorno = Final do nome do arquivo que é para ser
         *                                 gravado o erro.
         *              cErro            = Texto do erro ocorrido a ser gravado no 
         *                                 arquivo
         *                            
         * ------------------------------------------------------------------------------
         * Retorno....: 
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * // Arquivo de envio: 20080619T19113320-ped-sta.xml
         * // Arquivo de retorno que vai ser gravado: 20080619T19113320-sta.err
         * this.GravarXmlRetorno("-ped-sta.xml", "-sta.err");
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        private void GravarArqErroServico(string pFinalArqEnvio, string pFinalArqErro, string cErro)
        {
            string cArqErro = this.vPastaXMLRetorno + "\\" +
                              this.ExtrairNomeArq(this.vXmlNfeDadosMsg, pFinalArqEnvio) +
                              pFinalArqErro;

            StreamWriter SW_2 = File.CreateText(cArqErro);
            SW_2.Write(cErro);
            SW_2.Close();
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 16/07/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Move ou Excluir os arquivos XML da pasta de envio
         *              
         * ------------------------------------------------------------------------------
         * Definição..: MoveDeleteArq( string, string )
         * Parâmetros.: cArquivo - Nome do arquivo que é para ser movido ou deletado
         *              cOpcao   - M = Move o arquivo da pasta de envio para a pasta de
         *                             XML´s enviados
         *                         D = Deleta o arquivo da pasta de envio    
         *                        
         * ------------------------------------------------------------------------------
         * Retorno....: 
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * //Mover o arquivo da pasta c:\ para a pasta de enviados configurado na 
         * //tela de configurações do uninfe.
         * this.MoveDeleteArq( "c:\teste.xml", "M" ) 
         * 
         * //Deletar o arquivo
         * this.MoveDeleteArq( "c:\teste.xml", "D" )
         * 
         * ------------------------------------------------------------------------------
         * Notas......: Normalmente os arquivos que são movidos para a pasta de enviados
         *              são os de nota fiscal, cancelamento e inutilização, pois são 
         *              documentos assinados digitalmente e necessários para comprovar
         *              algo futuramente.
         *              Os demais são deletados.
         * 
         * ==============================================================================         
         */
        private void MoveDeleteArq(string cArquivo, string cOpcao)
        {
            //Definir o arquivo que vai ser deletado ou movido para outra pasta
            FileInfo oArquivo = new FileInfo(cArquivo);

            if (cOpcao == "M") //Mover o arquivo para outra pasta 
            {
                //Criar Pasta do Mês para gravar arquivos enviados
                string vNomePastaEnviado = this.vPastaXMLEnviado + "\\" + DateTime.Now.ToString("yyyyMM");
                if (Directory.Exists(vNomePastaEnviado) == false)
                {
                    System.IO.Directory.CreateDirectory(vNomePastaEnviado);
                }

                //Se conseguiu criar a pasta ele move o arquivo, caso contrário
                if (Directory.Exists(vNomePastaEnviado) == true)
                {
                    //Mover o arquivo da nota fiscal para a pasta dos enviados
                    string vNomeArquivo = vNomePastaEnviado + "\\" + ExtrairNomeArq(cArquivo, ".xml") + ".xml";
                    if (File.Exists(vNomeArquivo))
                    {
                        FileInfo oArqDestino = new FileInfo(vNomeArquivo);
                        oArqDestino.Delete();
                    }
                    oArquivo.MoveTo(vNomeArquivo);
                }
                else
                {
                    //FAZER: Futuramente tenho que tratar este erro, pois se não existe o diretório
                    //       Tenho que retornar um erro para o ERP
                }
            }
            else if (cOpcao == "D") //Deletar o arquivo
            {
                oArquivo.Delete();
            }
        }
    }
}