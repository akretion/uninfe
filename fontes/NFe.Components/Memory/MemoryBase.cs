using NFe.Components.Abstract;

namespace NFe.Components.Memory
{
    public abstract class MemoryBase : EmiteNFSeBase
    {
        #region locais/ protegidos

        int CodigoMun = 0;
        string Usuario = "";
        string SenhaWs = "";
        string ProxyUser = "";
        string ProxyPass = "";
        string ProxyServer = "";
        EmiteNFSeBase memoryService;

        protected EmiteNFSeBase MemoryService
        {
            get
            {
                if (memoryService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        memoryService = new H.MemoryH(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, CodigoMun);
                    else
                        memoryService = new P.MemoryP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, CodigoMun);
                }
                return memoryService;
            }
        }

        #endregion locais/ protegidos

        #region Construtores

        public MemoryBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver)
            : base(tpAmb, pastaRetorno)
        {
            CodigoMun = codMun;
            Usuario = usuario;
            SenhaWs = senhaWs;
            ProxyUser = proxyuser;
            ProxyPass = proxypass;
            ProxyServer = proxyserver;
        }

        #endregion Construtores

        #region Métodos

        public override void EmiteNF(string file)
        {
            MemoryService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            MemoryService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            MemoryService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            MemoryService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            MemoryService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            MemoryService.ConsultarNfsePorRps(file);
        }

        #endregion Métodos
    }
}