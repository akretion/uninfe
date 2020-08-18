using NFe.Components.Abstract;

namespace NFe.Components.SimplISS
{
    public abstract class SimplISSBase : EmiteNFSeBase
    {
        #region locais/ protegidos

        int CodigoMun = 0;
        string Usuario = "";
        string SenhaWs = "";
        string ProxyUser = "";
        string ProxyPass = "";
        string ProxyServer = "";
        EmiteNFSeBase simplissService;

        protected EmiteNFSeBase SimplISSService
        {
            get
            {
                if (simplissService == null)
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        switch (CodigoMun)
                        {
                            case 3541406: //Presidente Prudente-SP
                                simplissService = new PresidentePrudenteSP.h.SimplISSH(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            case 4202404: //Blumenau-SC
                                simplissService = new BlumenauSC.h.SimplISSH(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            default:
                                simplissService = new Homologacao.SimplISSH(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;
                        }
                    else
                        switch (CodigoMun)
                        {
                            case 3538709: //Piracicaba-SP
                                simplissService = new PiracicabaSP.p.SimplISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            case 3541406: //Presidente Prudente-SP
                                simplissService = new PresidentePrudenteSP.p.SimplISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            case 3515004: //Embu das Artes-SP
                                simplissService = new EmbuDasArtesSP.p.SimplISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            case 3306305: //Volta Redonda-RJ
                                simplissService = new VoltaRedondaRJ.p.SimplISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            case 3130309: //Iguatama-MG
                                simplissService = new IguatamaMG.p.SimplISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            case 4101101: // Andirá-PR
                                simplissService = new AndiraPR.p.SimplISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            case 4202008: // Balneário Camboriú-SC
                                simplissService = new BalnearioCamboriu.p.SimplISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            case 3148103: // Patrocínio-MG
                                simplissService = new PatrocinioMG.p.SimplISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            case 3528809: //Macaraí-SP
                                simplissService = new MacaraiSP.p.SimplISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            case 4102109: //Astorgar-PR
                                simplissService = new AstorgaPR.p.SimplISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            case 3503307: //Araras-SP
                                simplissService = new ArarasSP.p.SimplISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            case 3535507: //Paraguaçu Paulista-SP
                                simplissService = new ParaguacuPaulistaSP.p.SimplISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            case 3143104: //Monte Carmelo-MG
                                simplissService = new MonteCarmeloMG.p.SimplISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            case 3549102: //São João da Boa Vista - SP
                                simplissService = new SaoJoaoDaBoaVistaSP.p.SimplISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            case 4202404: //Blumenau-SC
                                simplissService = new BlumenauSC.p.SimplISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            case 3556404: //Vargem Grande do Sul-SP
                                simplissService = new  VargemGrandeDoSulSP.p.SimplISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }

                return simplissService;
            }
        }

        #endregion locais/ protegidos

        #region Construtores

        public SimplISSBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver)
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
            SimplISSService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            SimplISSService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            SimplISSService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            SimplISSService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            SimplISSService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            SimplISSService.ConsultarNfsePorRps(file);
        }

        #endregion Métodos
    }
}