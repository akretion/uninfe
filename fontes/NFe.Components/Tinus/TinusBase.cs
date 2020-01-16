using NFe.Components.Abstract;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.Tinus
{
    public abstract class TinusBase : EmiteNFSeBase
    {
        #region locais/ protegidos

        private int CodigoMun = 0;
        private string ProxyUser = "";
        private string ProxyPass = "";
        private string ProxyServer = "";
        private X509Certificate2 Certificado;
        private EmiteNFSeBase tinusService;

        protected EmiteNFSeBase TinusService
        {
            get
            {
                if (tinusService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        switch (CodigoMun)
                        {
                            case 2607901: //Jaboatão dos Guararapes-PE
                                tinusService = new JaboataodDosGuararapesPE.h.TinusH(tpAmb, PastaRetorno, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 2602902: // Cabo de Santo Agostinho-PE
                                tinusService = new CaboDeSantoAgostinhoPE.h.TinusH(tpAmb, PastaRetorno, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 2606200: //Goiana - PE
                                tinusService = new GoianaPE.h.TinusH(tpAmb, PastaRetorno, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 2111201: //São José do Ribamar-MA
                                tinusService = new SaoJoseDoRibamarMA.h.TinusH(tpAmb, PastaRetorno, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 2610707: // //Paulista-PE
                                tinusService = new PaulistaPE.h.TinusH(tpAmb, PastaRetorno, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 2408003: //Mossoró-RN
                                tinusService = new MossoroRN.h.TinusH(tpAmb, PastaRetorno, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                    else
                        switch (CodigoMun)
                        {
                            case 2607901: //Jaboatão dos Guararapes-PE
                                tinusService = new JaboataodDosGuararapesPE.p.TinusP(tpAmb, PastaRetorno, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 2606200: //Goiana-PE
                                tinusService = new GoianaPE.p.TinusP(tpAmb, PastaRetorno, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 2602902: //Cabo de Santo Agostinho-PE
                                tinusService = new CaboDeSantoAgostinhoPE.p.TinusP(tpAmb, PastaRetorno, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 2503209: //Cabedelo-PB
                                tinusService = new CabedeloPB.p.TinusP(tpAmb, PastaRetorno, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 2610707: //Paulista-PE
                                tinusService = new PaulistaPE.p.TinusP(tpAmb, PastaRetorno, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 2408003: //Mossoró-RN
                                tinusService = new MossoroRN.p.TinusP(tpAmb, PastaRetorno, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                }

                return tinusService;
            }
        }

        #endregion locais/ protegidos

        #region Construtores

        public TinusBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string proxyuser, string proxypass, string proxyserver, X509Certificate2 certificado)
            : base(tpAmb, pastaRetorno)
        {
            CodigoMun = codMun;
            ProxyUser = proxyuser;
            ProxyPass = proxypass;
            ProxyServer = proxyserver;
            Certificado = certificado;
        }

        #endregion Construtores

        #region Métodos

        public override void EmiteNF(string file)
        {
            TinusService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            TinusService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            TinusService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            TinusService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            TinusService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            TinusService.ConsultarNfsePorRps(file);
        }

        #endregion Métodos
    }
}