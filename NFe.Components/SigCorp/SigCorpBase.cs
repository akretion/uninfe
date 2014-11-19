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

namespace NFe.Components.SigCorp
{
    public abstract class SigCorpBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        int CodigoMun = 0;
        EmiteNFSeBase sigCorpService;
        protected EmiteNFSeBase SigCorpService
        {
            get
            {
                if (sigCorpService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        switch (CodigoMun)
                        {
                            case 3507506: //Botucatu-SP
                                throw new Exception(); // não tem

                            case 3506003: //Bauru-SP
                                sigCorpService = new NFe.Components.SigCorp.BauruSP.h.SigCorpH(tpAmb, PastaRetorno);
                                break;

                            case 4113700: //Londrina-PR
                                sigCorpService = new NFe.Components.SigCorp.LondrinaPR.h.SigCorpH(tpAmb, PastaRetorno);
                                break;

                            default:
                                throw new Exception(); // não tem                                
                        }
                    else
                        switch (CodigoMun)
                        {
                            case 3507506: //Botucatu-SP
                                sigCorpService = new NFe.Components.SigCorp.BotucatuSP.p.SigCorpP(tpAmb, PastaRetorno);
                                break;

                            case 3506003: //Bauru-SP
                                sigCorpService = new NFe.Components.SigCorp.BauruSP.p.SigCorpP(tpAmb, PastaRetorno);
                                break;

                            case 4113700: //Londrina-PR 
                                sigCorpService = new NFe.Components.SigCorp.LondrinaPR.p.SigCorpP(tpAmb, PastaRetorno);
                                break;

                            default:
                                throw new Exception(); // não tem                                
                        }
                }
                return sigCorpService;
            }
        }
        #endregion

        #region Construtores
        public SigCorpBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun)
            : base(tpAmb, pastaRetorno)
        {
            CodigoMun = codMun;
        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            SigCorpService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            SigCorpService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            SigCorpService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            SigCorpService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            SigCorpService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            SigCorpService.ConsultarNfsePorRps(file);
        }
        #endregion


    }
}
