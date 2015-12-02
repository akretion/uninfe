using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFe.Components.Abstract;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace NFe.Components.FISSLEX
{
    public abstract class FISSLEXBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        int CodigoMun = 0;
        string Usuario = "";
        string SenhaWs = "";
        string ProxyUser = "";
        string ProxyPass = "";
        string ProxyServer = "";
        EmiteNFSeBase fisslexService;
        protected EmiteNFSeBase FISSLEXService
        {
            get
            {
                if (fisslexService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        switch (CodigoMun)
                        {
                            case 5107909: //Sinop-MT
                                fisslexService = new NFe.Components.FISSLEX.SinopMT.h.FISSLEXH(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                    else
                        switch (CodigoMun)
                        {
                            case 5107909: //Sinop-MT
                                fisslexService = new NFe.Components.FISSLEX.SinopMT.p.FISSLEXP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                //fisslexService = new NFe.Components.SimplISS.PiracicabaSP.p.SimplISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                }
                return fisslexService;
            }
        }
        #endregion

        #region Construtores
        public FISSLEXBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver)
            : base(tpAmb, pastaRetorno)
        {
            CodigoMun = codMun;
            Usuario = usuario;
            SenhaWs = senhaWs;
            ProxyUser = proxyuser;
            ProxyPass = proxypass;
            ProxyServer = proxyserver;
        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            FISSLEXService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            FISSLEXService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            FISSLEXService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            FISSLEXService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            FISSLEXService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            FISSLEXService.ConsultarNfsePorRps(file);
        }
        #endregion


    }
}
