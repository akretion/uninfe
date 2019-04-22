using System.Xml;

namespace Unimake.DFe.Servicos
{
    public class TipoServico
    {
        #region Propriedades

        /// <summary>
        /// Conteúdo do XML a ser analisado para definição do tipo de serviço que deve ser executado.
        /// </summary>
        private readonly XmlDocument ConteudoXML;

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="conteudoXML">Conteúdo do XML a ser analisado para definição do tipo do serviço que deve ser executado</param>
        public TipoServico(XmlDocument conteudoXML)
        {
            ConteudoXML = conteudoXML;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="arquivoXML">Arquivo XML de onde deve ser extraido o onteúdo a ser analisado para definição do tipo do serviço que deve ser executado</param>
        public TipoServico(string arquivoXML)
        {
            XmlDocument doc = new XmlDocument
            {
                PreserveWhitespace = false
            };

            doc.Load(arquivoXML);

            ConteudoXML = doc;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Analisa o XML e define qual o tipo de serviço que deve ser executado
        /// </summary>
        /// <returns>Tipo de serviço que deve ser executado</returns>
        public Servicos Definir()
        {
            Servicos tipoServico = Servicos.Nulo;

            switch (ConteudoXML.DocumentElement.Name)
            {
                case "consStatServ":
                    tipoServico = Servicos.NFeStatusServico;
                    break;

                case "consSitNFe":
                    tipoServico = Servicos.NFeConsultaProtocolo;
                    break;

                case "inutNFe":
                    tipoServico = Servicos.NFeInutilizacao;
                    break;

                case "ConsCad":
                    tipoServico = Servicos.NFeConsultaCadastro;
                    break;
            }

            return tipoServico;
        }

        #endregion
    }
}
