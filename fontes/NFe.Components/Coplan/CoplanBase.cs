using NFe.Components.Abstract;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.Coplan
{
    public abstract class CoplanBase : EmiteNFSeBase
    {
        #region Locais/Protegidos

        private int CodigoMun = 0;
        private string UsuarioProxy = "";
        private string SenhaProxy = "";
        private string DomainProxy = "";
        private X509Certificate Certificado = null;
        private EmiteNFSeBase coplanService;

        protected EmiteNFSeBase CoplanService
        {
            get
            {
                if (coplanService == null)
                {
                    switch (CodigoMun)
                    {
                        case 5102637: //Campo Novo do Parecis - MT
                            coplanService = tpAmb == TipoAmbiente.taHomologacao ?
                                new CampoNovoParecisMT.h.CoplanH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado) as EmiteNFSeBase :
                                new CampoNovoParecisMT.p.CoplanP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado) as EmiteNFSeBase;
                            break;

                        case 5106224: //Nova Mutum - MT
                            coplanService = tpAmb == TipoAmbiente.taHomologacao ?
                                new NovaMutumMT.h.CoplanH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado) as EmiteNFSeBase :
                                new NovaMutumMT.p.CoplanP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado) as EmiteNFSeBase;
                            break;

                        case 5104104: //Guarantã do Norte-MT
                            coplanService = tpAmb == TipoAmbiente.taHomologacao ?
                                new GuarantaNorteMT.h.CoplanH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado) as EmiteNFSeBase :
                                new GuarantaNorteMT.p.CoplanP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado) as EmiteNFSeBase;
                            break;

                        case 5107909: //Sinop-MT
                            coplanService = tpAmb == TipoAmbiente.taHomologacao ?
                                new SinopMT.h.CoplanH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado) as EmiteNFSeBase :
                                new SinopMT.p.CoplanP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado) as EmiteNFSeBase;
                            break;

                        case 5105606: //Matupá-MT
                            coplanService = tpAmb == TipoAmbiente.taHomologacao ?
                                new MatupaMT.h.CoplanH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado) as EmiteNFSeBase :
                                new MatupaMT.p.CoplanP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado) as EmiteNFSeBase;
                            break;
                        case 5107602: //Rondonópolis-MT
                            coplanService = tpAmb == TipoAmbiente.taHomologacao ?
                                new HRondonopolisMT.h.CoplanH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado) as EmiteNFSeBase :
                                new PRondonopolisMT.p.CoplanP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado) as EmiteNFSeBase;
                            break;
                        case 5106422: //Peixoto de Azevedo-MT
                            coplanService = tpAmb == TipoAmbiente.taHomologacao ?
                                new HPeixotoAzevedoMT.h.CoplanH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado) as EmiteNFSeBase :
                                new PPeixotoAzevedoMT.p.CoplanP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado) as EmiteNFSeBase;
                            break;

                        case 5105200: //Juscimeira-MT
                            coplanService = tpAmb == TipoAmbiente.taHomologacao ?
                                new HJuscimeiraMT.h.CoplanH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado) as EmiteNFSeBase :
                                new PJuscimeiraMT.p.CoplanP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado) as EmiteNFSeBase;
                            break;

                        default:
                            throw new Exceptions.ServicoInexistenteException();
                    }
                }
                return coplanService;
            }
        }

        #endregion Locais/Protegidos

        #region Construtores

        public CoplanBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuarioProxy, string senhaProxy, string domainProxy, X509Certificate certificado)
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
            CoplanService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            CoplanService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            CoplanService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            CoplanService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            CoplanService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            CoplanService.ConsultarNfsePorRps(file);
        }

        public override void SubstituirNfse(string file)
        {
            CoplanService.SubstituirNfse(file);
        }

        #endregion Métodos
    }
}