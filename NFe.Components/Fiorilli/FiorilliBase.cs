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

namespace NFe.Components.Fiorilli
{
    public abstract class FiorilliBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        int CodigoMun = 0;
        string Usuario = "";
        string SenhaWs = "";
        EmiteNFSeBase fiorilliService;
        protected EmiteNFSeBase FiorilliService
        {
            get
            {
                if (fiorilliService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        switch (CodigoMun)
                        {
                            case 3553807: //Taquarituba-SP <-Ambiente da Fiorilli para testes
                                fiorilliService = new NFe.Components.Fiorilli.TaquaraSP.h.FiorilliH(tpAmb, PastaRetorno, Usuario, SenhaWs);
                                break;

                            case 3512902: //Cosmorama-SP <-Ambiente da Fiorilli para testes
                                fiorilliService = new NFe.Components.Fiorilli.TaquaraSP.h.FiorilliH(tpAmb, PastaRetorno, Usuario, SenhaWs);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                    else
                        switch (CodigoMun)
                        {
                            case 3553807: //Taquarituba-SP
                                fiorilliService = new NFe.Components.Fiorilli.TaquaraSP.p.FiorilliP(tpAmb, PastaRetorno, Usuario, SenhaWs);
                                break;

                            case 3512902: //Cosmorama-SP
                                fiorilliService = new NFe.Components.Fiorilli.CosmoramaSP.p.FiorilliP(tpAmb, PastaRetorno, Usuario, SenhaWs);                                
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                }
                return fiorilliService;
            }
        }
        #endregion

        #region Construtores
        public FiorilliBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs)
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
            FiorilliService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            FiorilliService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            FiorilliService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            FiorilliService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            FiorilliService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            FiorilliService.ConsultarNfsePorRps(file);
        }
        #endregion


    }
}
