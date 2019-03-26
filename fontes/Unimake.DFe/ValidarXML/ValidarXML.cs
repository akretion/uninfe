using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace Unimake.DFe
{
    internal class ValidarSchema
    {
        public int Retorno { get; private set; }
        public string RetornoString { get; private set; }
        private string ErroValidacao { get; set; }

        /// <summary>
        /// Método responsável por validar a estrutura do XML de acordo com o schema passado por parâmetro
        /// </summary>
        /// <param name="rotaArqXML">Nome do arquivo XML</param>
        /// <param name="conteudoXML">Conteúdo do XML a ser validado</param>
        private void Validar(XmlDocument conteudoXML, string arqSchema, string targetNS)
        {
            if (string.IsNullOrEmpty(arqSchema))
            {
                throw new Exception("Arquivo de schema (XSD), necessário para validação do XML, não foi informado.");
            }

            Retorno = 0;
            RetornoString = "";

            if (File.Exists(arqSchema))
            {
                XmlReader xmlReader = null;

                try
                {
                    XmlReaderSettings settings = new XmlReaderSettings
                    {
                        ValidationType = ValidationType.Schema
                    };

                    XmlSchemaSet schemas = new XmlSchemaSet();
                    settings.Schemas = schemas;

                    /* Se dentro do XSD houver referência a outros XSDs externos, pode ser necessário ter certas permissões para localizá-lo.
                     * Usa um "Resolver" com as credencias-padrão para obter esse recurso externo. */
                    XmlUrlResolver resolver = new XmlUrlResolver
                    {
                        Credentials = System.Net.CredentialCache.DefaultCredentials
                    };
                    /* Informa à configuração de leitura do XML que deve usar o "Resolver" criado acima e que a validação deve respeitar
                     * o esquema informado no início. */
                    settings.XmlResolver = resolver;

                    if (targetNS != string.Empty)
                    {
                        schemas.Add(targetNS, arqSchema);
                    }

                    settings.ValidationEventHandler += new ValidationEventHandler(reader_ValidationEventHandler);

                    xmlReader = XmlReader.Create(new StringReader(conteudoXML.OuterXml), settings);

                    ErroValidacao = "";
                    try
                    {
                        while (xmlReader.Read()) { }
                    }
                    catch (Exception ex)
                    {
                        ErroValidacao = ex.Message;
                    }

                    xmlReader.Close();
                }
                catch (Exception ex)
                {
                    if (xmlReader != null)
                    {
                        xmlReader.Close();
                    }

                    ErroValidacao = ex.Message + "\r\n";
                }

                if (ErroValidacao != "")
                {
                    Retorno = 1;
                    RetornoString = "Início da validação...\r\n\r\n";
                    RetornoString += ErroValidacao;
                    RetornoString += "\r\n...Final da validação";
                }
            }
            else 
            {
                Retorno = 3;
                RetornoString = "Arquivo XSD (schema) não foi encontrado em " + arqSchema;
            }
        }

        private void reader_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            ErroValidacao += "Linha: " + e.Exception.LineNumber + " Coluna: " + e.Exception.LinePosition + " Erro: " + e.Exception.Message + "\r\n";
        }
    }
}
