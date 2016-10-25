using NFe.Components.Abstract;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.Fiorilli
{
    public abstract class FiorilliBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        int CodigoMun = 0;
        string Usuario = "";
        string SenhaWs = "";
        string ProxyUser = "";
        string ProxyPass = "";
        string ProxyServer = "";
        X509Certificate2 Certificado;
        EmiteNFSeBase fiorilliService;
        protected EmiteNFSeBase FiorilliService
        {
            get
            {
                if (fiorilliService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        switch (CodigoMun)
                        {
                            case 3504008: //Assis-SP
                            case 3522802: //Itaporanga-SP
                            case 3512902: //Cosmorama-SP 
                            case 3553807: //Taquarituba-SP
                                fiorilliService = new TaquaraSP.h.FiorilliH(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 3504503: //Avaré-SP
                                fiorilliService = new AvareSP.h.FiorilliH(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                    else
                        switch (CodigoMun)
                        {
                            case 3504008: //Assis-SP
                                fiorilliService =  new AssisSP.p.FiorilliP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 3553807: //Taquarituba-SP
                                fiorilliService = new TaquaraSP.p.FiorilliP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 3512902: //Cosmorama-SP
                                fiorilliService = new CosmoramaSP.p.FiorilliP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 3504503: //Avaré-SP
                                fiorilliService = new AvareSP.p.FiorilliP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 3522802: //Itaporanga-SP
                                fiorilliService = new ItaporangaSP.p.FiorilliP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                }
                return fiorilliService;
            }
        }
        #endregion

        #region Construtores
        public FiorilliBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver, X509Certificate2 certificado)
            : base(tpAmb, pastaRetorno)
        {
            CodigoMun = codMun;
            Usuario = usuario;
            SenhaWs = senhaWs;
            ProxyUser = proxyuser;
            ProxyPass = proxypass;
            ProxyServer = proxyserver;
            Certificado = certificado;
        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            FiorilliService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            FiorilliService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            FiorilliService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            FiorilliService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            FiorilliService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            FiorilliService.ConsultarNfsePorRps(file);
        }
        #endregion
    }
}
