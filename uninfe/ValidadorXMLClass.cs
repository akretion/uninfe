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
            StreamReader cStreamReader = new StreamReader(cRotaArqXML);
            XmlTextReader cXmlTextReader = new XmlTextReader(cStreamReader);
            XmlValidatingReader reader = new XmlValidatingReader(cXmlTextReader);

            // Criar um coleção de schema, adicionar o XSD para ela
            XmlSchemaCollection schemaCollection = new XmlSchemaCollection();
            schemaCollection.Add("http://www.portalfiscal.inf.br/nfe", cRotaArqSchema);

            // Adicionar a coleção de schema para o XmlValidatingReader
            reader.Schemas.Add(schemaCollection);

            // Wire up the call back.  The ValidationEvent is fired when the
            // XmlValidatingReader hits an issue validating a section of the xml
            reader.ValidationEventHandler += new ValidationEventHandler(reader_ValidationEventHandler);

            // Iterate through the xml document
            this.cErro = "";
            while (reader.Read()) { }

            this.Retorno = 0;
            this.RetornoString = "";
            if (cErro != "")
            {
                this.Retorno        = 1;
                this.RetornoString  = "Início da validação...\r\n\r\n";
                this.RetornoString += this.cErro;
                this.RetornoString += "\r\n...Final da validação";
            }
        }

        private void reader_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            this.cErro = "Linha: "+e.Exception.LineNumber+" Coluna: "+e.Exception.LinePosition+" Erro: "+e.Exception.Message+"\r\n";
        }
    }
}
