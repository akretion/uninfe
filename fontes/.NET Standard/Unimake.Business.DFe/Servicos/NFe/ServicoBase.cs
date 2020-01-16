using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace Unimake.Business.DFe.Servicos.NFe
{
    public abstract class ServicoBase: Servicos.ServicoBase
    {
        #region Protected Methods

        /// <summary>
        /// Definir configurações
        /// </summary>
        protected override void DefinirConfiguracao()
        {
            //Definir a pasta onde fica o schema do XML
            switch(Configuracoes.TipoDFe)
            {
                case DFE.NFe:
                    Configuracoes.SchemaPasta = ConfigurationManager.CurrentConfig.PastaSchemaNFe;
                    break;

                case DFE.NFCe:
                    Configuracoes.SchemaPasta = ConfigurationManager.CurrentConfig.PastaSchemaNFCe;
                    break;

                case DFE.CTe:
                    Configuracoes.SchemaPasta = ConfigurationManager.CurrentConfig.PastaSchemaCTe;
                    break;

                case DFE.MDFe:
                    break;

                case DFE.NFSe:
                    break;

                case DFE.SAT:
                    break;
            }
        }

        /// <summary>
        /// Validar o XML
        /// </summary>
        protected override void XmlValidar()
        {
            var validar = new ValidarSchema();
            validar.Validar(ConteudoXML, Path.Combine(Configuracoes.SchemaPasta, Configuracoes.SchemaArquivo), Configuracoes.TargetNS);

            if(!validar.Success)
            {
                throw new Exception(validar.ErrorMessage);
            }
        }

        #endregion Protected Methods

        #region Public Constructors

        public ServicoBase()
            : base()
        {
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="conteudoXML">Conteúdo do XML que será enviado para o WebService</param>
        /// <param name="configuracao">Objeto "Configuracoes" com as propriedade necessária para a execução do serviço</param>
        public ServicoBase(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao) { }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Executar o serviço
        /// </summary>
        public override void Executar()
        {
            XmlValidar();
            base.Executar();
        }

        /// <summary>
        /// Gravar o XML de distribuição em uma pasta no HD
        /// </summary>
        /// <param name="pasta">Pasta onde deve ser gravado o XML no HD</param>
        /// <param name="nomeArquivo">Nome do arquivo a ser gravado no HD</param>
        /// <param name="conteudoXML">String contendo o conteúdo do XML a ser gravado no HD</param>
        public override void GravarXmlDistribuicao(string pasta, string nomeArquivo, string conteudoXML)
        {
            StreamWriter streamWriter = null;

            try
            {
                var conteudoXmlDistribuicao = conteudoXML;

                streamWriter = File.CreateText(Path.Combine(pasta, nomeArquivo));
                streamWriter.Write(conteudoXmlDistribuicao);
            }
            finally
            {
                if(streamWriter != null)
                {
                    streamWriter.Close();
                }
            }
        }

        /// <summary>
        /// Gravar o XML de distribuição em um stream
        /// </summary>
        /// <param name="value">Conteúdo a ser gravado no stream</param>
        /// <param name="stream">Stream que vai receber o conteúdo do XML</param>
        /// <param name="encoding">Define o encodingo do stream, caso não informado ,será usado o UTF8</param>
        [ComVisible(false)]
        public virtual void GravarXmlDistribuicao(Stream stream,
                                                  string value,
                                                  Encoding encoding = null)
        {
            if(stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if(string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if(encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var byteData = encoding.GetBytes(value);
            stream.Write(byteData, 0, byteData.Length);
            stream.Close();
        }

        #endregion Public Methods
    }
}