using System;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFCe
{
    public class ConsultaProtocolo: NFe.ConsultaProtocolo
    {
        #region Public Constructors

        public ConsultaProtocolo(ConsSitNFe consSitNFe, Configuracao configuracao)
            : base(consSitNFe, configuracao) { }

        public ConsultaProtocolo()
        {
        }

        #endregion Public Constructors

        /// <summary>
        /// Validar o XML
        /// </summary>
        protected override void XmlValidar()
        {
            var validar = new ValidarSchema();
            validar.Validar(ConteudoXML, TipoDFe.NFe.ToString() + "." + Configuracoes.SchemaArquivo, Configuracoes.TargetNS);

            if(!validar.Success)
            {
                throw new Exception(validar.ErrorMessage);
            }
        }
    }
}