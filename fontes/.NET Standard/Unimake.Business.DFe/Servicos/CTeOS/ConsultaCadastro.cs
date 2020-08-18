using System;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.CTeOS
{
    public class ConsultaCadastro: NFe.ConsultaCadastro
    {
        #region Public Constructors

        public ConsultaCadastro(ConsCadBase consCad, Configuracao configuracao)
            : base(consCad, configuracao) { }

        public ConsultaCadastro()
        {
        }

        #endregion Public Constructors

        /// <summary>
        /// Validar o XML
        /// </summary>
        protected override void XmlValidar()
        {
            var validar = new ValidarSchema();
            validar.Validar(ConteudoXML, TipoDFe.CTe.ToString() + "." + Configuracoes.SchemaArquivo, Configuracoes.TargetNS);

            if(!validar.Success)
            {
                throw new Exception(validar.ErrorMessage);
            }
        }
    }
}