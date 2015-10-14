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

namespace NFe.Components.MGM
{
    public abstract class MGMBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        int CodigoMun = 0;
        string Usuario = "";
        string Senha = "";
        EmiteNFSeBase mgmService;
        protected EmiteNFSeBase MGMService
        {
            get
            {
                if (mgmService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        switch (CodigoMun)
                        {
                            case 3537305: //Penapolis-SP
                                mgmService = new NFe.Components.MGM.PenapolisSP.h.MGMH(tpAmb, PastaRetorno);
                                break;

                            default:
                                throw new Exception(); // não tem                                
                        }
                    else
                        switch (CodigoMun)
                        {
                            case 3537305: //Penapolis-SP
                                mgmService = new NFe.Components.MGM.PenapolisSP.p.MGMP(tpAmb, PastaRetorno, Usuario, Senha);
                                break;

                            default:
                                throw new Exception(); // não tem                                
                        }
                }
                return mgmService;
            }
        }
        #endregion

        #region Construtores
        public MGMBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senha)
            : base(tpAmb, pastaRetorno)
        {
            CodigoMun = codMun;
            Usuario = usuario;
            Senha = senha;

        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            MGMService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            MGMService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            MGMService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            MGMService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            MGMService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            MGMService.ConsultarNfsePorRps(file);
        }
        #endregion


    }
}
