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

namespace NFe.Components.Metropolis
{
    public abstract class MetropolisBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        int CodigoMun = 0;
        string ProxyUser = "";
        string ProxyPass = "";
        string ProxyServer = "";
        X509Certificate2 Certificado = null;
        EmiteNFSeBase metropolisService;

        protected EmiteNFSeBase MetropolisService
        {
            get
            {
                if (metropolisService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                    {
                        switch (CodigoMun)
                        {
                            case 2919207:
                                metropolisService = new LauroDeFreitasBA.h.MetropolisH(tpAmb, PastaRetorno, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 2913606:
                                metropolisService = new IlheusBA.h.MetropolisH(tpAmb, PastaRetorno, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            default:
                                break;
                        }
                    }
                    else
                        switch (CodigoMun)
                        {
                            case 2919207:
                                metropolisService = new LauroDeFreitasBA.p.MetropolisP(tpAmb, PastaRetorno, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                }
                return metropolisService;
            }
        }
        #endregion

        #region Construtores
        public MetropolisBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string proxyserver,string proxyuser, string proxypass, X509Certificate2 certificado)
            : base(tpAmb, pastaRetorno)
        {
            CodigoMun = codMun;
            ProxyUser = proxyuser;
            ProxyPass = proxypass;
            ProxyServer = proxyserver;
            Certificado = certificado;
        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            MetropolisService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            MetropolisService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            MetropolisService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            MetropolisService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            MetropolisService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            MetropolisService.ConsultarNfsePorRps(file);
        }
        #endregion


    }
}
