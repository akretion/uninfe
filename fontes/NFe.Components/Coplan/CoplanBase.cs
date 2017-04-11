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

namespace NFe.Components.Coplan
{
    public abstract class CoplanBase : EmiteNFSeBase
    {
        #region Locais/Protegidos
        int CodigoMun = 0;
        string UsuarioProxy = "";
        string SenhaProxy = "";
        string DomainProxy = "";
        X509Certificate Certificado = null;
        EmiteNFSeBase coplanService;

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
                                new CampoNovoParecisMT.h.CoplanH(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado) as EmiteNFSeBase:
                                new CampoNovoParecisMT.p.CoplanP(tpAmb, PastaRetorno, UsuarioProxy, SenhaProxy, DomainProxy, Certificado) as EmiteNFSeBase;
                            break;

                        default:
                            throw new Exceptions.ServicoInexistenteException();
                    }
                }
                return coplanService;
            }
        }
        #endregion

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
        #endregion

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
        #endregion


    }
}
