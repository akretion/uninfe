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

namespace NFe.Components.RLZ_INFORMATICA
{
    public abstract class Rlz_InformaticaBase : EmiteNFSeBase
    {
        #region Locais / Protegidos
        int CodigoMun = 0;
        //string Usuario = "";
        //string SenhaWs = "";
        EmiteNFSeBase rlz_informaticaService;
        protected EmiteNFSeBase Rlz_InformaticaService
        {
            get
            {
                if (rlz_informaticaService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        switch (CodigoMun)
                        {
                            case 3546603: //Santa Fé do Sul-SP
                                rlz_informaticaService = new NFe.Components.RLZ_INFORMATICA.SantaFeDoSul.h.Rlz_InformaticaH(tpAmb, PastaRetorno);
                                break;
                            case 3524808: //Jales - SP
                                rlz_informaticaService = new NFe.Components.RLZ_INFORMATICA.Jales.h.Rlz_InformaticaH(tpAmb, PastaRetorno);
                                break;
                      
                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                    else
                        switch (CodigoMun)
                        {
                            case 3546603: //Santa Fé do Sul-SP
                                rlz_informaticaService = new NFe.Components.RLZ_INFORMATICA.SantaFeDoSul.p.Rlz_InformaticaP(tpAmb, PastaRetorno);
                                break;
                            case 3524808: //Jales - SP
                                rlz_informaticaService = new NFe.Components.RLZ_INFORMATICA.Jales.p.Rlz_InformaticaP(tpAmb, PastaRetorno);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                }
                return rlz_informaticaService;
            }
        }
        #endregion

        #region Construtores
        public Rlz_InformaticaBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun)
            : base(tpAmb, pastaRetorno)
        {
            CodigoMun = codMun;
        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            Rlz_InformaticaService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            Rlz_InformaticaService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            Rlz_InformaticaService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            Rlz_InformaticaService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            Rlz_InformaticaService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            Rlz_InformaticaService.ConsultarNfsePorRps(file);
        }
        #endregion


    }
}
