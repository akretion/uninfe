using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Windows.Forms;

namespace uninfe
{
    class ValidadorXMLClass
    {
        public int Retorno { get; private set; }
        public string RetornoString { get; private set; }
        public int nRetornoTipoArq { get; private set; }
        public string cRetornoTipoArq { get; private set; }
        public string cArquivoSchema { get; private set; }
        /// <summary>
        /// Pasta dos schemas para validação do XML
        /// </summary>
        private string PastaSchema = UniNfeInfClass.PastaSchemas();
        /// <summary>
        /// Tag que deve ser assinada no XML, se o conteúdo estiver em branco é por que o XML não deve ser assinado
        /// </summary>
        public string TagAssinar { get; private set; }

        private string cErro;

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 10/06/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Método responsável por validar os arquivos XML de acordo com a 
         *              estrutura determinada pelos schemas (XSD)
         *              
         * ------------------------------------------------------------------------------
         * Definição..: ValidarXML( string, string )
         * Parâmetros.: cRotaArqXML    - Rota e nome do arquivo XML que é
         *                               para ser validado. Exemplo:
         *                               c:\unimake\uninfe\envio\teste-nfe.xml
         * 
         *              cRotaArqSchema - Rota e nome do arquivo XSD que é para ser 
         *                               utilizado para validar o XML
         *                               c:\unimake\uninfe\schemas\nfe_v1.10.xsd
         *                        
         * ------------------------------------------------------------------------------
         * Retorno....: Atualiza duas propriedades da classe:
         *              this.Retorno       - 0=Sucesso na validação
         *                                   1=O XML não está em conformidade com o schema
         *                                   2=Arquivo XML não foi encontrato
         *                                   3=Arquivo XSD (schema) não foi encontrato
         *              this.RetornoString - em caso de algum erro, esta propriedade vai
         *                                   receber um texto contendo o erro para ser
         *                                   analisado. 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * ValidadorXMLClass oValidarXML = new ValidadorXMLClass;
         * oValidarXML(@"c:\unimake\uninfe\teste-nfe.xml", @"c:\unimake\uninfe\schemas\nfe_v1.10.xsd")
         * if (this.Retorno == 0)
         * {
         *    MessageBox.Show("Validado com sucesso")
         * }
         * else
         * {
         *    MessageBox.Show("Ocorreu erro na validação: \r\n\r\n" + this.RetornoString)
         * }
         * 
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        public void ValidarXML(string cRotaArqXML, string cRotaArqSchema)
        {            
            bool lArqXML = File.Exists(cRotaArqXML);
            bool lArqXSD = File.Exists(this.PastaSchema + "\\" + cRotaArqSchema);

            if (lArqXML && lArqXSD)
            {
                StreamReader cStreamReader = new StreamReader(cRotaArqXML);
                XmlTextReader cXmlTextReader = new XmlTextReader(cStreamReader);
                XmlValidatingReader reader = new XmlValidatingReader(cXmlTextReader);

                // Criar um coleção de schema, adicionar o XSD para ela
                XmlSchemaCollection schemaCollection = new XmlSchemaCollection();
                schemaCollection.Add("http://www.portalfiscal.inf.br/nfe", this.PastaSchema + "\\" + cRotaArqSchema);

                // Adicionar a coleção de schema para o XmlValidatingReader
                reader.Schemas.Add(schemaCollection);

                // Wire up the call back.  The ValidationEvent is fired when the
                // XmlValidatingReader hits an issue validating a section of the xml
                reader.ValidationEventHandler += new ValidationEventHandler(reader_ValidationEventHandler);

                // Iterate through the xml document
                this.cErro = "";
                try
                {
                    while (reader.Read()) { }
                }
                catch (Exception ex)
                {
                    this.cErro = ex.Message;
                }

                reader.Close();

                this.Retorno = 0;
                this.RetornoString = "";
                if (cErro != "")
                {
                    this.Retorno = 1;
                    this.RetornoString = "Início da validação...\r\n\r\n";
                    this.RetornoString += "Arquivo XML: " + cRotaArqXML + "\r\n";
                    this.RetornoString += "Arquivo SCHEMA: " + this.PastaSchema + "\\" + cRotaArqSchema + "\r\n\r\n";
                    this.RetornoString += this.cErro;
                    this.RetornoString += "\r\n...Final da validação";
                }
            }
            else
            {
                if (lArqXML == false)
                {
                    this.Retorno = 2;
                    this.RetornoString = "Arquivo XML não foi encontrato";
                }
                else if (lArqXSD == false)
                {
                    this.Retorno = 3;
                    this.RetornoString = "Arquivo XSD (schema) não foi encontrato";
                }
            }
        }

        private void reader_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            this.cErro = "Linha: " + e.Exception.LineNumber + " Coluna: " + e.Exception.LinePosition + " Erro: " + e.Exception.Message + "\r\n";
        }

        /*
         * ==============================================================================
         * UNIMAKE - SOLUÇÕES CORPORATIVAS
         * ==============================================================================
         * Data.......: 31/07/2008
         * Autor......: Wandrey Mundin Ferreira
         * ------------------------------------------------------------------------------
         * Descrição..: Método responsável por retornar de que tipo é o arquivo XML.
         *              A principio o método retorna o tipo somente se é um XML for de:
         *              - Nota Fiscal Eletrônica
         *              - XML de envio de Lote de Notas Fiscais Eletrônicas
         *              - Cancelamento de Nota Fiscal Eletrônica
         *              - Inutilização de Numeração de Notas Fiscais Eletrônicas
         *              - Consulta da Situação da Nota Fiscal Eletrônica
         *              - Consulta do Recibo do Lote das Notas Fiscais Eletrônicas
         *              - Consulta do Status do Serviço da Nota Fiscal Eletrônica
         *              
         * ------------------------------------------------------------------------------
         * Definição..: TipoArquivoXML( string )
         * Parâmetros.: cRotaArqXML    - Rota e nome do arquivo XML que é
         *                               para ser retornado o tipo. Exemplo:
         *                               c:\unimake\uninfe\envio\teste-nfe.xml
         *                        
         * ------------------------------------------------------------------------------
         * Retorno....: A propriedade this.nRetornoTipoArq vai receber um número 
         *              identificando se foi possível identificar o arquivo ou não
         *              1=Nota Fiscal Eletrônica
         *              2=XML de envio de Lote de Notas Fiscais Eletrônicas
         *              3=Cancelamento de Nota Fiscal Eletrônica
         *              4=Inutilização de Numeração de Notas Fiscais Eletrônicas
         *              5=Consulta da Situação da Nota Fiscal Eletrônica
         *              6=Consulta Recibo da Nota Fiscal Eletrônica
         *              7=Consulta do Status do Serviço da Nota Fiscal Eletrônica
         *              
         *              100=Arquivo XML não foi encontrato
         *              101=Arquivo não foi identificado
         * 
         * ------------------------------------------------------------------------------
         * Exemplos...:
         * 
         * oObj.TipoArquivoXML(@"c:\unimake\uninfe\teste-nfe.xml")
         * if (oObj.nRetornoTipoArq == 1)
         * {
         *    MessageBox.Show("Nota Fiscal Eletrônica");
         * }
         * 
         * ------------------------------------------------------------------------------
         * Notas......:
         * 
         * ==============================================================================         
         */
        public void TipoArquivoXML(string cRotaArqXML)
        {
            this.nRetornoTipoArq = 0;
            this.cRetornoTipoArq = string.Empty;
            this.cArquivoSchema = string.Empty;
            this.TagAssinar = string.Empty;

            if (File.Exists(cRotaArqXML))
            {
                //Carregar os dados do arquivo XML de configurações do UniNfe
                XmlTextReader oLerXml = null;

                try
                {
                    oLerXml = new XmlTextReader(cRotaArqXML);

                    while (oLerXml.Read())
                    {
                        if (oLerXml.NodeType == XmlNodeType.Element)
                        {
                            switch (oLerXml.Name)
                            {
                                case "nfeProc":
                                    this.nRetornoTipoArq = 9;
                                    this.cRetornoTipoArq = "XML de distribuição da NFe com protocolo de autorização anexado";
                                    this.cArquivoSchema = "procNFe_v1.10.xsd";
                                    break;

                                case "procCancNFe":
                                    this.nRetornoTipoArq = 10;
                                    this.cRetornoTipoArq = "XML de distribuição do Cancelamento da NFe com protocolo de autorização anexado";
                                    this.cArquivoSchema = "procCancNFe_v1.07.xsd";
                                    break;

                                case "procInutNFe":
                                    this.nRetornoTipoArq = 11;
                                    this.cRetornoTipoArq = "XML de distribuição de Inutilização de Números de NFe com protocolo de autorização anexado";
                                    this.cArquivoSchema = "procInutNFe_v1.07.xsd";
                                    break;

                                case "NFe":
                                    this.nRetornoTipoArq = 1;
                                    this.cRetornoTipoArq = "XML de Nota Fiscal Eletrônica";
                                    this.cArquivoSchema = "nfe_v1.10.xsd";
                                    this.TagAssinar = "infNFe";
                                    break;

                                case "enviNFe":
                                    this.nRetornoTipoArq = 2;
                                    this.cRetornoTipoArq = "XML de Envio de Lote de Notas Fiscais Eletrônicas";
                                    this.cArquivoSchema = "enviNFe_v1.10.xsd";
                                    break;

                                case "cancNFe":
                                    this.nRetornoTipoArq = 3;
                                    this.cRetornoTipoArq = "XML de Cancelamento de Nota Fiscal Eletrônica";
                                    this.cArquivoSchema = "cancNFe_v1.07.xsd";
                                    this.TagAssinar = "infCanc";
                                    break;

                                case "inutNFe":
                                    this.nRetornoTipoArq = 4;
                                    this.cRetornoTipoArq = "XML de Inutilização de Numerações de Notas Fiscais Eletrônicas";
                                    this.cArquivoSchema = "inutNFe_v1.07.xsd";
                                    this.TagAssinar = "infInut";
                                    break;

                                case "consSitNFe":
                                    this.nRetornoTipoArq = 5;
                                    this.cRetornoTipoArq = "XML de Consulta da Situação da Nota Fiscal Eletrônica";
                                    this.cArquivoSchema = "consSitNFe_v1.07.xsd";
                                    break;

                                case "consReciNFe":
                                    this.nRetornoTipoArq = 6;
                                    this.cRetornoTipoArq = "XML de Consulta do Recibo do Lote de Notas Fiscais Eletrônicas";
                                    this.cArquivoSchema = "consReciNfe_v1.10.xsd";
                                    break;

                                case "consStatServ":
                                    this.nRetornoTipoArq = 7;
                                    this.cRetornoTipoArq = "XML de Consulta da Situação do Serviço da Nota Fiscal Eletrônica";
                                    this.cArquivoSchema = "consStatServ_v1.07.xsd";
                                    break;

                                case "ConsCad":
                                    this.nRetornoTipoArq = 8;
                                    this.cRetornoTipoArq = "XML de Consulta do Cadastro do Contribuinte";
                                    this.cArquivoSchema = "consCad_v1.01.xsd";
                                    break;
                            }

                            if (this.nRetornoTipoArq != 0) //Arquivo já foi identificado
                            {
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.nRetornoTipoArq = 102;
                    this.cRetornoTipoArq = ex.Message;
                }
                finally
                {
                    if (oLerXml != null)
                    {
                        if (oLerXml.ReadState != ReadState.Closed)
                        {
                            oLerXml.Close();
                        }
                    }
                }
            }
            else
            {
                this.nRetornoTipoArq = 100;
                this.cRetornoTipoArq = "Arquivo XML não foi encontrado";
            }

            if (this.nRetornoTipoArq == 0)
            {
                this.nRetornoTipoArq = 101;
                this.cRetornoTipoArq = "Não foi possível identificar o arquivo XML";
            }
        }
    }
}
