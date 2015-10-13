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

namespace NFe.Components.Conam
{
    public abstract class ConamBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        int CodigoMun = 0;
        string Usuario = "";
        string SenhaWs = "";
        EmiteNFSeBase conamService;
        protected EmiteNFSeBase ConamService
        {
            get
            {
                if (conamService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        switch (CodigoMun)
                        {
                            case 3170701: //Varginha-MG
                                conamService = new NFe.Components.Conam.VarginhaMG.h.ConamH(tpAmb, PastaRetorno, Usuario, SenhaWs);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                    else
                        switch (CodigoMun)
                        {
                            case 3170701: //Varginha-MG
                                conamService = new NFe.Components.Conam.VarginhaMG.p.ConamP(tpAmb, PastaRetorno, Usuario, SenhaWs);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                }
                return conamService;
            }
        }
        #endregion

        #region Construtores
        public ConamBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs)
            : base(tpAmb, pastaRetorno)
        {
            CodigoMun = codMun;
            Usuario = usuario;
            SenhaWs = senhaWs;
        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            ConamService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            ConamService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            ConamService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            ConamService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            ConamService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            ConamService.ConsultarNfsePorRps(file);
        }
        #endregion


    }
}
