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

namespace NFe.Components.Pronin
{
    public abstract class ProninBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        int CodigoMun = 0;
        string UsuarioProxy = "";
        string SenhaProxy = "";
        string DomainProxy = "";
        X509Certificate Certificado = null;
        EmiteNFSeBase proninService;
        protected EmiteNFSeBase ProninService
        {
            get
            {
                if (proninService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        switch (CodigoMun)
                        {
                            case 4109401: //Guarapuava-PR
                                proninService = new GuarapuavaPR.h.ProninH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                    else
                        switch (CodigoMun)
                        {
                            case 4109401: //Guarapuava-PR
                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                }
                return proninService;
            }
        }
        #endregion

        #region Construtores
        public ProninBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuarioProxy, string senhaProxy, string domainProxy, X509Certificate certificado)
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
            ProninService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            ProninService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            ProninService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            ProninService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            ProninService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            ProninService.ConsultarNfsePorRps(file);
        }
        #endregion


    }
}
