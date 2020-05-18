using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace Unimake.Business.DFe
{
    public class ValidarSchema
    {
        #region Private Properties

        /// <summary>
        /// Erros ocorridos na validação
        /// </summary>
        private string ErroValidacao { get; set; }

        #endregion Private Properties

        #region Private Methods

        private IEnumerable<XmlSchema> ExtractSchemas(string arqSchema)
        {
            /*
             * .Net Core não está carregando todos os schemas
             * https://github.com/dotnet/runtime/issues/21946
             *
             *                                   |\    /|
             *   GO HORSE PROCESS             ___| \,,/_/
             *      POG MODE ON            ---__/ \/    \
             *                            __--/     (D)  \
             *                            _ -/    (_      \
             *                           // /       \_ /  -\
             *     __-------_____--___--/           / \_ O o)
             *    /                                 /   \__/
             *   /                                 /
             *  ||          )                   \_/\
             *  ||         /              _      /  |
             *  | |      /--______      ___\    /\  :
             *  | /   __-  - _/   ------    |  |   \ \
             *   |   -  -   /                | |     \ )
             *   |  |   -  |                 | )     | |
             *    | |    | |                 | |    | |
             *    | |    < |                 | |   |_/
             *    < |    /__\                <  \
             *    /__\                       /___\
             */

            var fi = new FileInfo(arqSchema);
            var doc = new XmlDocument();
            var files = new List<string>
            {
                fi.FullName,
            };

            var keys = new HashSet<string>
            {
                fi.Name
            };

            XmlAttribute attrib;
            doc.Load(fi.FullName);

            var getAttrib = new Func<XmlAttribute>(() =>
            {
                return doc.DocumentElement
                                .ChildNodes
                                .OfType<XmlNode>()
                                .Where(wNode => wNode.Attributes
                                                     .OfType<XmlAttribute>()
                                                     .Any(wAttrib => wAttrib.Name == "schemaLocation" &&
                                                                     !keys.Contains(wAttrib.Value)))
                                .Select(s => s.Attributes.OfType<XmlAttribute>()
                                                         .First(w => w.Name == "schemaLocation"))
                                .FirstOrDefault();
            });

            while ((attrib = getAttrib()) != null)
            {
                var schemaFile = attrib.Value;
                var path = Path.Combine(fi.DirectoryName, schemaFile);
                files.Add(path);
                keys.Add(schemaFile);

                if (getAttrib() == null)
                {
                    doc.Load(path);
                }
            }

            foreach (var file in files)
            {
                var result = XmlSchema.Read(File.OpenRead(file), null);
                yield return result;
            }
        }

        private void Reader_ValidationEventHandler(object sender, ValidationEventArgs e) =>
            ErroValidacao += "Linha: " + e.Exception.LineNumber + " Coluna: " + e.Exception.LinePosition + " Erro: " + e.Exception.Message + "\r\n";

        private void ValidateXMLAgainstSchema(XmlDocument conteudoXML, XmlReaderSettings settings)
        {
            using (var xmlReader = XmlReader.Create(new StringReader(conteudoXML.OuterXml), settings))
            {
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
        }

        #endregion Private Methods

        #region Public Properties

        /// <summary>
        /// Código do erro em caso de falhas na validação
        /// </summary>
        public int ErrorCode { get; private set; }

        /// <summary>
        /// Mensagem de erro em caso de falhas na validação
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Se a validação foi bem sucedida (true/false)
        /// </summary>
        public bool Success { get; private set; }

        #endregion Public Properties

        #region Public Methods

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
                var settings = new XmlReaderSettings
                {
                    ValidationType = ValidationType.Schema
                };

                try
                {
                    var schemas = new XmlSchemaSet();
                    settings.Schemas = schemas;

                    /* Se dentro do XSD houver referência a outros XSDs externos, pode ser necessário ter certas permissões para localizá-lo.
                     * Usa um "Resolver" com as credencias-padrão para obter esse recurso externo. */
                    var resolver = new XmlUrlResolver
                    {
                        Credentials = System.Net.CredentialCache.DefaultCredentials
                    };
                    /* Informa à configuração de leitura do XML que deve usar o "Resolver" criado acima e que a validação deve respeitar
                     * o esquema informado no início. */
                    settings.XmlResolver = resolver;

                    if (targetNS != string.Empty)
                    {
                        foreach (var schema in ExtractSchemas(arqSchema))
                        {
                            settings.Schemas.Add(schema);
                        }
                    }

                    settings.ValidationEventHandler += new ValidationEventHandler(Reader_ValidationEventHandler);

                    ValidateXMLAgainstSchema(conteudoXML, settings);
                }
                catch (Exception ex)
                {
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

        #endregion Public Methods
    }
}