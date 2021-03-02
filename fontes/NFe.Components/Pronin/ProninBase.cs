using NFe.Components.Abstract;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.Pronin
{
    public abstract class ProninBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        private int CodigoMun = 0;
        private string UsuarioProxy = "";
        private string SenhaProxy = "";
        private string DomainProxy = "";
        private X509Certificate Certificado = null;
        private EmiteNFSeBase proninService;

        protected EmiteNFSeBase ProninService
        {
            get
            {
                if (proninService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        switch (CodigoMun)
                        {
                            case 4109401: //Guarapuava-PR
                                proninService = new GuarapuavaPR.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3131703: //Itabira-MG
                                proninService = new ItabiraMG.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;
                            case 4303004: //Cachoeira do Sul-RS
                                proninService = new CachoeiraSulRS.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4322509: //Vacari-RS
                                proninService = new VacariRS.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4323002: //Viamão-RS
                                proninService = new ViamaoRS.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3505807: //Bastos-SP
                                proninService = new BastosSP.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4308904: //Getúlio Vargas-RS
                                proninService = new GetulioVargasRS.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4118501: //Pato Branco-PR
                                proninService = new PatoBrancoPR.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3554300: //Teodoro Sampaio-SP
                                proninService = new TeodoroSampaioSP.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3542404: //Regente Feijó-SP
                                proninService = new RegenteFeijoSP.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 5005707: //Navirai-SP
                                proninService = new NaviraiMS.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4314423: //Picada Café-RS
                                proninService = new PicadaCafe.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3511102: //Catanduva-SP
                                proninService = new Catanduva.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3535804: //Paranapanema-SP
                                proninService = new ParanapanemaSP.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4306932: //Entre-Ijuís-RS
                                proninService = new EntreIjuisRS.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4322400: // Uruguaiana-RS 
                                proninService = new Uruguaiana.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;
                            
                            case 4302808: //Caçapava do Sul-RS 
                                proninService = new CacapavaDoSulRS.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

					       case 3501301: //Álvares Machado-SP
                                proninService = new AlvaresMachadoSP.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4300109: //Agudo-RS
                                proninService = new AgudoRS.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3550407: //São Pedro - SP
                                proninService = new SaoPedro.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;
                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                    else
                        switch (CodigoMun)
                        {
                            case 4109401: //Guarapuava-PR
                                proninService = new GuarapuavaPR.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3131703: //Itabira-MG
                                proninService = new ItabiraMG.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4303004: //Cachoeira do Sul-RS
                                proninService = new CachoeiraSulRS.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4322509: //Vacari-RS
                                proninService = new VacariRS.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3556602: //Vera Cruz-RS
                                proninService = new VeraCruzRS.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3512803: //Cosmópolis-SP
                                proninService = new CosmopolisSP.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4323002: //Viamão-RS
                                proninService = new ViamaoRS.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3505807: //Bastos-SP
                                proninService = new BastosSP.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3530300: //Mirassol-SP
                                proninService = new MirassolSP.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4308904: //Getúlio Vargas-RS
                                proninService = new GetulioVargasRS.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4118501: //Pato Branco-PR
                                proninService = new PatoBrancoPR.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3554300: //Teodoro Sampaio-SP
                                proninService = new TeodoroSampaioSP.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3542404: //Regente Feijó-SP
                                proninService = new RegenteFeijoSP.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 5005707: //Naviraí-MS
                                proninService = new NaviraiMS.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4314423://Picada Café-RS
                                proninService = new PicadaCafe.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3511102: //Catanduva-SP 
                                proninService = new Catanduva.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3535804: //Paranapanema-SP
                                proninService = new ParanapanemaSP.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4306932: //Entre-Ijuís-RS
                                proninService = new EntreIjuisRS.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4322400: // Uruguaiana-RS 
                                proninService = new Uruguaiana.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4302808: //Caçapava do Sul-RS 
                                proninService = new CacapavaDoSulRS.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

							case 3501301: //Álvares Machado-SP
                               proninService = new AlvaresMachadoSP.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
						       break;

                            case 4300109: //Agudo-RS
                                proninService = new AgudoRS.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4124053://Santa Terezinha de Itaipu-Pr
                                proninService = new SantaTerezinhaItaipuPR.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 4101408://Apucarana - PR
                                proninService = new ApucaranaPR.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            case 3550407://São Pedro - SP
                                proninService = new SaoPedro.p.ProninP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                }
                return proninService;
            }
        }

        #endregion locais/ protegidos

        #region Construtores

        public ProninBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuarioProxy, string senhaProxy, string domainProxy, X509Certificate certificado)
            : base(tpAmb, pastaRetorno)
        {
            CodigoMun = codMun;
            UsuarioProxy = usuarioProxy;
            SenhaProxy = senhaProxy;
            DomainProxy = domainProxy;
            Certificado = certificado;
        }

        #endregion Construtores

        #region Métodos

        public override void EmiteNF(string file)
        {
            ProninService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            ProninService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            ProninService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            ProninService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            ProninService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            ProninService.ConsultarNfsePorRps(file);
        }

        public override void SubstituirNfse(string file)
        {
            ProninService.SubstituirNfse(file);
        }

        #endregion Métodos
    }
}