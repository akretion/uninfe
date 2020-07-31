using System;
using Unimake.Business.DFe.Servicos.Interop;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.CTe;

namespace Unimake.Business.DFe.Servicos.CTeOS
{
    public class ConsultaProtocolo: CTe.ConsultaProtocolo
    {
        #region Public Constructors

        public ConsultaProtocolo(ConsSitCTe consSitNFe, Configuracao configuracao)
            : base(consSitNFe, configuracao) { }

        public ConsultaProtocolo()
        {
        }

        #endregion Public Constructors
    }
}