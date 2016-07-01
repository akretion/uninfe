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
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        switch (CodigoMun)
                        {
                            case 3130309: //Iguatama-MG
                            case 3515004: //Embu das Artes-SP
                            case 3538709: //Piracicaba-SP 
                                simplissService = new PiracicabaSP.h.SimplISSH(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            case 3541406: //Presidente Prudente-SP
                                simplissService = new PresidentePrudenteSP.h.SimplISSH(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
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

                            case 3130309:
                                simplissService = new IguatamaMG.p.SimplISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                }
                return simplissService;
            }
        }
        #endregion

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
        #endregion

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
        #endregion


    }
}
