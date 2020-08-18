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

        private IEnumerable<XmlSchema> ExtractSchemasResource(string arqSchema)
        {
            var files = new List<string>();
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var resources = assembly.GetManifestResourceNames();
            var arquivoResource = Configuration.NamespaceSchema + arqSchema;
            var stringXSD = "";
            using(var stm = assembly.GetManifestResourceStream(arquivoResource))
            {
                if(stm != null)
                {
                    var reader = new StreamReader(stm);
                    stringXSD = reader.ReadToEnd();
                    files.Add(stringXSD);
                }
            }

            var schemaPrincipal = arqSchema.Substring(arqSchema.IndexOf(".") + 1);
            var doc = new XmlDocument();

            var keys = new HashSet<string>
            {
                schemaPrincipal
            };

            XmlAttribute attrib;
            doc.LoadXml(stringXSD);

            var getAttrib = new Func<XmlAttribute>(() =>
            {
                return doc.DocumentElement.ChildNodes.OfType<XmlNode>()
                .Where(wNode => wNode.Attributes.OfType<XmlAttribute>().Any(wAttrib => wAttrib.Name == "schemaLocation" && !keys.Contains(wAttrib.Value)))
                .Select(s => s.Attributes.OfType<XmlAttribute>().First(w => w.Name == "schemaLocation"))
                .FirstOrDefault();
            });

            while((attrib = getAttrib()) != null)
            {
                var schemaFile = attrib.Value;

                var schemaInterno = arquivoResource.Replace(schemaPrincipal, schemaFile);
                var conteudoSchemaInterno = "";
                using(var stm = assembly.GetManifestResourceStream(schemaInterno))
                {
                    if(stm != null)
                    {
                        var reader = new StreamReader(stm);
                        conteudoSchemaInterno = reader.ReadToEnd();
                        files.Add(conteudoSchemaInterno);
                    }
                }

                keys.Add(schemaFile);

                if(getAttrib() == null)
                {
                    doc.LoadXml(conteudoSchemaInterno);
                }
            }

            foreach(var file in files)
            {
                var result = XmlSchema.Read(GenerateStreamFromString(file), null);
                yield return result;
            }
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private void Reader_ValidationEventHandler(object sender, ValidationEventArgs e) =>
            ErroValidacao += "Linha: " + e.Exception.LineNumber + " Coluna: " + e.Exception.LinePosition + " Erro: " + e.Exception.Message + "\r\n";

        private void ValidateXMLAgainstSchema(XmlDocument conteudoXML, XmlReaderSettings settings)
        {
            using(var xmlReader = XmlReader.Create(new StringReader(conteudoXML.OuterXml), settings))
            {
                ErroValidacao = "";

                try
                {
                    while(xmlReader.Read()) { }
                }
                catch(Exception ex)
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
        /// <param name="arqSchema">Arquivo de schema para validação do XML (XSD) contido nos recursos da DLL.</param>
        /// <param name="targetNS">Target Name Space, se existir, para validação</param>
        /// <example>
        /// //Validar arquivos de NFe
        /// Validar(xmlDocument, "NFe.consStatServCTe_v3.00.xsd")
        /// 
        /// //Validar arquivos de CTe
        /// Validar(xmlDocument, "CTe.consStatServCTe_v3.00.xsd")
        /// 
        /// //Validar arquivos de MDFe
        /// Validar(xmlDocument, "MDFe.consStatServ_v4.00.xsd")
        /// </example>
        public void Validar(XmlDocument conteudoXML, string arqSchema, string targetNS = "")
        {
            if(string.IsNullOrEmpty(arqSchema))
            {
                throw new Exception("Arquivo de schema (XSD), necessário para validação do XML, não foi informado.");
            }

            Success = true;
            ErrorCode = 0;
            ErrorMessage = "";

            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema
            };

            try
            {
                var schemas = new XmlSchemaSet();
                settings.Schemas = schemas;

                var resolver = new XmlUrlResolver
                {
                    Credentials = System.Net.CredentialCache.DefaultCredentials
                };
                settings.XmlResolver = resolver;

                if(targetNS != string.Empty)
                {
                    foreach(var schema in ExtractSchemasResource(arqSchema))
                    {
                        settings.Schemas.Add(schema);
                    }
                }

                settings.ValidationEventHandler += new ValidationEventHandler(Reader_ValidationEventHandler);

                ValidateXMLAgainstSchema(conteudoXML, settings);
            }
            catch(Exception ex)
            {
                ErroValidacao = ex.Message + "\r\n";
            }

            if(ErroValidacao != "")
            {
                Success = false;
                ErrorCode = 1;
                ErrorMessage = "Início da validação...\r\n\r\n";
                ErrorMessage += ErroValidacao;
                ErrorMessage += "\r\n...Final da validação";
            }
        }

        #endregion Public Methods
    }
}