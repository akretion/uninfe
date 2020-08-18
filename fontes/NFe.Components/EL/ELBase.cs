using NFe.Components.Abstract;

namespace NFe.Components.EL
{
    public abstract class ELBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        int CodigoMun = 0;
        string UsuarioWs = "";
        string SenhaWs = "";
        string UsuarioProxy = "";
        string SenhaProxy = "";
        string DomainProxy = "";

        //X509Certificate Certificado = null;
        EmiteNFSeBase elService;

        protected EmiteNFSeBase EGoverneService
        {
            get
            {
                if (elService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        switch (CodigoMun)
                        {
                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                    else
                        switch (CodigoMun)
                        {
                            case 2930709: //Simões Filho-BA
                                elService = new SimoesFilhoBA.p.ELP(tpAmb, PastaRetorno, UsuarioWs, SenhaWs, UsuarioProxy, SenhaProxy, DomainProxy);
                                break;

                            case 3201506: //Colatina-ES
                                elService = new ColatinaES.p.ELP(tpAmb, PastaRetorno, UsuarioWs, SenhaWs, UsuarioProxy, SenhaProxy, DomainProxy);
                                break;

                            case 3202603: //Iconha-ES
                                elService = new IconhaES.p.ELP(tpAmb, PastaRetorno, UsuarioWs, SenhaWs, UsuarioProxy, SenhaProxy, DomainProxy);
                                break;

                            case 3200904: //Barra de São Francisco-ES
                                elService = new BarraDeSaoFranciscoES.p.ELP(tpAmb, PastaRetorno, UsuarioWs, SenhaWs, UsuarioProxy, SenhaProxy, DomainProxy);
                                break;

                            case 3162708: //São João do Paraíso-MG
                                elService = new SaoJoaoParaisoMG.p.ELP(tpAmb, PastaRetorno, UsuarioWs, SenhaWs, UsuarioProxy, SenhaProxy, DomainProxy);
                                break;

                            case 3168002: //Taiobeiras-MG
                                elService = new TaiobeirasMG.p.ELP(tpAmb, PastaRetorno, UsuarioWs, SenhaWs, UsuarioProxy, SenhaProxy, DomainProxy);
                                break;
                            case 2931350: //Teixeira de Freitas-BA
                                elService = new TeixeiraDeFreitasBA.p.ELP(tpAmb, PastaRetorno, UsuarioWs, SenhaWs, UsuarioProxy, SenhaProxy, DomainProxy);
                                break;
                            case 3205101: //Viana-ES
                                elService = new Viana.p.ELP(tpAmb, PastaRetorno, UsuarioWs, SenhaWs, UsuarioProxy, SenhaProxy, DomainProxy);
                                break;

                            case 3204203: //Piuma-ES
                                elService = new Piuma.p.ELP(tpAmb, PastaRetorno, UsuarioWs, SenhaWs, UsuarioProxy, SenhaProxy, DomainProxy);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                }
                return elService;
            }
        }
        #endregion locais/ protegidos

        #region Construtores
        public ELBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuarioWs, string senhaWs, string usuarioProxy, string senhaProxy, string domainProxy)
            : base(tpAmb, pastaRetorno)
        {
            UsuarioWs = usuarioWs;
            SenhaWs = senhaWs;
            CodigoMun = codMun;
            UsuarioProxy = usuarioProxy;
            SenhaProxy = senhaProxy;
            DomainProxy = domainProxy;
        }
        #endregion Construtores

        #region Métodos
        public override void EmiteNF(string file)
        {
            EGoverneService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            EGoverneService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            EGoverneService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            EGoverneService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            EGoverneService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            EGoverneService.ConsultarNfsePorRps(file);
        }
        #endregion Métodos
    }
}