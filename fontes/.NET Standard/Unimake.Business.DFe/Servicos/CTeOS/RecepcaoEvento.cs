using System;
using System.Runtime.InteropServices;
using Unimake.Business.DFe.Xml.CTe;

namespace Unimake.Business.DFe.Servicos.CTeOS
{
    [ComVisible(true)]
    public class RecepcaoEvento: CTe.RecepcaoEvento
    {
        #region Public Constructors

        public RecepcaoEvento(EventoCTe envEvento, Configuracao configuracao)
            : base(envEvento, configuracao) { }

        public RecepcaoEvento() { }

        #endregion

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