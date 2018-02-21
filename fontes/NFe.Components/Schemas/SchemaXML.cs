using System.Collections.Generic;
using System.Linq;

namespace NFe.Components
{
    /// <summary>
    /// Classe responsável por definir uma lista dos arquivos de SCHEMAS para validação dos XMLs
    /// </summary>
    public class SchemaXML
    {
        /// <summary>
        /// Informações dos schemas para validação dos XML
        /// </summary>
        public static Dictionary<string, InfSchema> InfSchemas = new Dictionary<string, InfSchema>();

        /// <summary>
        /// O Maior ID que tem na lista
        /// </summary>
        public static int MaxID { get { return InfSchemas.Count; } }

        /// <summary>
        /// Cria várias listas com as TAG´s de identificação dos XML´s e seus Schemas
        /// </summary>
        /// <date>31/07/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public static void CriarListaIDXML()
        {
            InfSchemas.Clear();

            ///
            /// le todas as classes no projeto para ler as definicoes dos schemas
            ///
            System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
            var xx = ass.GetTypes().Where(p => p.IsClass && (p.Name.StartsWith("SchemaXML_") || p.Name.StartsWith("SchemaXMLNFSe_")));
            foreach (var h1 in xx)
            {
                //Console.WriteLine(h1 + " -> ");
                h1.InvokeMember("CriarListaIDXML", System.Reflection.BindingFlags.InvokeMethod, null, null, null);
            }
        }
    }

    public class InfSchema
    {
        /// <summary>
        /// TAG do XML que identifica qual XML é
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Identificador único numérico do XML
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Breve descrição do arquivo XML
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Nome do arquivo de schema para validar o XML
        /// </summary>
        public string ArquivoXSD { get; set; }

        /// <summary>
        /// Nome da tag do XML que será assinada
        /// </summary>
        public string TagAssinatura { get; set; }

        /// <summary>
        /// Nome da tag que tem o atributo ID
        /// </summary>
        public string TagAtributoId { get; set; }

        /// <summary>
        /// Nome da tag de lote do XML que será assinada
        /// </summary>
        public string TagLoteAssinatura { get; set; }

        /// <summary>
        /// Nome da tag de lote que tem o atributo ID
        /// </summary>
        public string TagLoteAtributoId { get; set; }

        /// <summary>
        /// Nome da tag do XML que será assinada (uma segunda tag que tem que ser assinada ex. SubstituirNfse Pelotas-RS)
        /// </summary>
        public string TagAssinatura0 { get; set; }
        /// <summary>
        /// Nome da tag que tem o atributo ID que será assinada, faz consunto com a TagAssinatura0
        /// </summary>
        public string TagAtributoId0 { get; set; }

        /// <summary>
        /// URL do schema de cada XML
        /// </summary>
        public string TargetNameSpace { get; set; }
    }
}