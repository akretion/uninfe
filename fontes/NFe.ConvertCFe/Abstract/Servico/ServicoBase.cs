using NFe.SAT.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NFe.SAT.Abstract.Servico
{
    /// <summary>
    /// Classe abstrata para comunicação com as DLLs
    /// </summary>
    public abstract class ServicoBase : IServico
    {
        /// <summary>
        /// Marca do fabricante do SAT
        /// </summary>
        public Unimake.SAT.Enuns.Fabricante Marca { get; set; }

        /// <summary>
        /// Código de ativação do SAT
        /// </summary>
        public string CodigoAtivacao { get; set; }

        /// <summary>
        /// Arquivo XML carregado no construtor
        /// </summary>
        public abstract string ArquivoXML { get; set; }

        /// <summary>
        /// Define o objeto de comunicação SAT
        /// </summary>
        Unimake.SAT.SAT _Sat = null;

        /// <summary>
        /// Objeto de comunicação com o SAT
        /// </summary>
        protected Unimake.SAT.SAT Sat
        {
            get
            {
                if (_Sat == null)
                    _Sat = new Unimake.SAT.SAT(Marca, CodigoAtivacao);

                return _Sat;
            }
        }

        /// <summary>
        /// Método responsável em transmitir a comunicação com o SAT
        /// </summary>
        /// <returns></returns>
        public abstract void Enviar();

        /// <summary>
        /// Método responsável por salvar o retorno do SAT
        /// </summary>
        /// <returns>caminho do arquivo de resposta</returns>
        public abstract string SaveResponse();

        /// <summary>
        /// Deserializar o objeto para string
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <returns></returns>
        public T DeserializarObjeto<T>()
            where T : new()
        {
            T envio = new T();

            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = envio.GetType().Name;

            XmlSerializer serializer = new XmlSerializer(typeof(T), xRoot);
            FileStream stream = File.Open(ArquivoXML, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader reader = new StreamReader(stream);
            envio = (T)serializer.Deserialize(reader);
            reader.Close();
            stream.Close();
            return envio;
        }

        /// <summary>
        /// Retorna o valor do documento a partir da raiz
        /// </summary>
        /// <param name="document">XML carregado</param>
        /// <param name="groupTag">nome do nó</param>
        /// <param name="nameTag">nome da tag</param>
        /// <returns></returns>
        public string GetValueXML(XmlDocument document, string groupTag, string nameTag)
        {
            string value = "";
            XmlNodeList nodes = document.GetElementsByTagName(groupTag);
            XmlNode node = nodes[0];

            foreach (XmlNode n in node)
            {
                if (n.NodeType == XmlNodeType.Element)
                {
                    if (n.Name.Equals(nameTag))
                    {
                        value = n.InnerText;
                        break;
                    }
                }
            }

            return value;
        }
    }
}
