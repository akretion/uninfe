using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFCe
{
    public class RetAutorizacao: NFe.RetAutorizacao
    {
        #region Public Constructors

        public RetAutorizacao(ConsReciNFe consReciNFe, Configuracao configuracao)
            : base(consReciNFe, configuracao) { }

        public RetAutorizacao()
        {
        }

        #endregion Public Constructors
    }
}