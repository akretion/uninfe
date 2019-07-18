using System.Xml;

namespace Unimake.Business.DFe.Servicos
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
        public Servico Definir()
        {
            Servico tipoServico = Servico.Nulo;

            switch (ConteudoXML.DocumentElement.Name)
            {
                case "consStatServ":
                    tipoServico = Servico.NFeStatusServico;
                    break;

                case "consSitNFe":
                    tipoServico = Servico.NFeConsultaProtocolo;
                    break;

                case "inutNFe":
                    tipoServico = Servico.NFeInutilizacao;
                    break;

                case "ConsCad":
                    tipoServico = Servico.NFeConsultaCadastro;
                    break;

                case "envEvento":
                    tipoServico = Servico.NFeRecepcaoEvento;
                    break;

                case "enviNFe":
                    tipoServico = Servico.NFeAutorizacao;
                    break;
            }

            return tipoServico;
        }

        #endregion
    }
}
