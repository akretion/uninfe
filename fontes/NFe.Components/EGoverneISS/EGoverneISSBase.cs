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

namespace NFe.Components.EGoverneISS
{
    public abstract class EGoverneISSBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        int CodigoMun = 0;
        string Usuario = "";
        string SenhaWs = "";

        EmiteNFSeBase egoverneService;

        protected EmiteNFSeBase SimplISSService
        {
            get
            {
                if (egoverneService == null)
                    switch (CodigoMun)
                    {
                        case 3534401: //Osasco-SP
                            egoverneService = new OsascoSP.p.EGoverneISSP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                            break;

                        default:
                            throw new Exceptions.ServicoInexistenteException();
                    }

                return egoverneService;
            }
        }
        #endregion

        #region Construtores
        public EGoverneISSBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver)
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
