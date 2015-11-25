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

namespace NFe.Components.EL
{
    public abstract class ELBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        int CodigoMun = 0;
        string UsuarioWs = "";
        string SenhaWs = "";
        string UsuarioProxy = "";
        string SenhaProxy = "";
        string DomainProxy = "";
        //X509Certificate Certificado = null;
        EmiteNFSeBase elService;
        protected EmiteNFSeBase EGoverneService
        {
            get
            {
                if (elService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        switch (CodigoMun)
                        {
                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                    else
                        switch (CodigoMun)
                        {
                            case 2930709: //Simões Filho-BA
                                elService = new NFe.Components.EL.SimoesFilhoBA.p.ELP(tpAmb, PastaRetorno, UsuarioWs, SenhaWs, UsuarioProxy, SenhaProxy, DomainProxy);
                                break;

                            case 3201506: //Colatina-ES
                                elService = new NFe.Components.EL.ColatinaES.p.ELP(tpAmb, PastaRetorno, UsuarioWs, SenhaWs, UsuarioProxy, SenhaProxy, DomainProxy);
                                break;                            

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                }
                return elService;
            }
        }
        #endregion

        #region Construtores
        public ELBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuarioWs, string senhaWs, string usuarioProxy, string senhaProxy, string domainProxy)
            : base(tpAmb, pastaRetorno)
        {
            UsuarioWs = usuarioWs;
            SenhaWs = senhaWs;
            CodigoMun = codMun;
            UsuarioProxy = usuarioProxy;
            SenhaProxy = senhaProxy;
            DomainProxy = domainProxy;
        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            EGoverneService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            EGoverneService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            EGoverneService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            EGoverneService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            EGoverneService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            EGoverneService.ConsultarNfsePorRps(file);
        }
        #endregion


    }
}
