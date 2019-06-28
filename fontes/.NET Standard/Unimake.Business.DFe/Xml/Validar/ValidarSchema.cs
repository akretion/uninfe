using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace Unimake.Business.DFe
{
    internal class ValidarSchema
    {
        #region Propriedades

        /// <summary>
        /// Se a validação foi bem sucedida (true/false)
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Código do erro em caso de falhas na validação
        /// </summary>
        public int ErrorCode { get; private set; }

        /// <summary>
        /// Mensagem de erro em caso de falhas na validação
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Erros ocorridos na validação
        /// </summary>
        private string ErroValidacao { get; set; }

        #endregion

        #region Métodos

        /// <summary>
        /// Método responsável por validar a estrutura do XML de acordo com o schema passado por parâmetro
        /// </summary>
        /// <param name="conteudoXML">Nome do arquivo XML a ser validado</param>
        /// <param name="arqSchema">Arquivo de schema para validação do XML (XSD)</param>
        /// <param name="targetNS">Target Name Space, se existir, para validação</param>
        public void Validar(XmlDocument conteudoXML, string arqSchema, string targetNS = "")
        {
            if (string.IsNullOrEmpty(arqSchema))
            {
                throw new Exception("Arquivo de schema (XSD), necessário para validação do XML, não foi informado.");
            }

            Success = true;
            ErrorCode = 0;
            ErrorMessage = "";

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

                    settings.ValidationEventHandler += new ValidationEventHandler(Reader_ValidationEventHandler);

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
                    Success = false;
                    ErrorCode = 1;
                    ErrorMessage = "Início da validação...\r\n\r\n";
                    ErrorMessage += ErroValidacao;
                    ErrorMessage += "\r\n...Final da validação";
                }
            }
            else
            {
                Success = false;
                ErrorCode = 3;
                ErrorMessage = "Arquivo XSD (schema) não foi encontrado em " + arqSchema;
            }
        }

        private void Reader_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            ErroValidacao += "Linha: " + e.Exception.LineNumber + " Coluna: " + e.Exception.LinePosition + " Erro: " + e.Exception.Message + "\r\n";
        }

        #endregion
    }
}
