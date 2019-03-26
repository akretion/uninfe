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

namespace NFe.Components.Betha.NewVersion
{
    public abstract class BethaBase : EmiteNFSeBase, Ambiente.IAmbiente
    {
        #region locais/ protegidos
        int CodigoMun = 0;
        string Usuario = "";
        string SenhaWs = "";
        string ProxyUser = "";
        string ProxyPass = "";
        string ProxyServer = "";
        EmiteNFSeBase bethaService;
        protected EmiteNFSeBase BethaService
        {
            get
            {
                if (bethaService == null)
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        bethaService = new Ambiente.Homologacao(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);
                    else
                        bethaService = new Ambiente.Producao(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer);

                return bethaService;
            }
        }
        #endregion

        #region Construtores
        public BethaBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver)
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
            BethaService.EmiteNF(file);
        }

        public void EmiteNFSincrono(string file)
        {
            Ambiente.IAmbiente service = BethaService as Ambiente.IAmbiente;
            service.EmiteNFSincrono(file);                        
        }

        public override void CancelarNfse(string file)
        {
            BethaService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            BethaService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            BethaService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            BethaService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            BethaService.ConsultarNfsePorRps(file);
        }

        public override void ConsultarNfseServicoTomado(string file)
        {
            BethaService.ConsultarNfseServicoTomado(file);
        }
        
        #endregion
    }
}
