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

namespace NFe.Components.EGoverne
{
    public abstract class EGoverneBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        int CodigoMun = 0;
        string UsuarioProxy = "";
        string SenhaProxy = "";
        string DomainProxy = "";
        X509Certificate Certificado = null;
        EmiteNFSeBase egoverneService;
        protected EmiteNFSeBase EGoverneService
        {
            get
            {
                if (egoverneService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        switch (CodigoMun)
                        {
                            case 4106902: //Curitiba-PR
                                egoverneService = new NFe.Components.EGoverne.CuritibaPR.h.EGoverneH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                    else
                        switch (CodigoMun)
                        {
                            case 4106902: //Curitiba-PR
                                egoverneService = new NFe.Components.EGoverne.CuritibaPR.p.EGoverneP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                }
                return egoverneService;
            }
        }
        #endregion

        #region Construtores
        public EGoverneBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuarioProxy, string senhaProxy, string domainProxy, X509Certificate certificado)
            : base(tpAmb, pastaRetorno)
        {
            CodigoMun = codMun;
            UsuarioProxy = usuarioProxy;
            SenhaProxy = senhaProxy;
            DomainProxy = domainProxy;
            Certificado = certificado;
            
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
