using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFCe
{
    public class Inutilizacao : NFe.Inutilizacao
    {
        public Inutilizacao(InutNFe inutNFe, Configuracao configuracao) 
            : base(inutNFe, configuracao)
        {
        }
    }
}
