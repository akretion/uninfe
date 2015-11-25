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

namespace NFe.Components.Consist
{
    public abstract class ConsistBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        int CodigoMun = 0;
        string Usuario = "";
        string SenhaWs = "";
        string ProxyUser = "";
        string ProxyPass = "";
        string ProxyServer = "";
        EmiteNFSeBase consistService;
        protected EmiteNFSeBase ConsistService
        {
            get
            {
                if (consistService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        switch (CodigoMun)
                        {
                            case 3148004: //Patos de Minas - MG
                                consistService = new NFe.Components.Consist.PatosdeMinasMG.h.ConsistH(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                    else
                        switch (CodigoMun)
                        {
                            case 3148004: //Patos de Minas - MG
                                consistService = new NFe.Components.Consist.PatosdeMinasMG.p.ConsistP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                }
                return consistService;
            }
        }
        #endregion

        #region Construtores
        public ConsistBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver)
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
            ConsistService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            ConsistService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            ConsistService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            ConsistService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            ConsistService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            ConsistService.ConsultarNfsePorRps(file);
        }
        #endregion


    }
}
