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
                            case 3529005: //Marilia-SP
                                sigCorpService = new MariliaSP.h.SigCorpH(tpAmb, PastaRetorno);
                                break;

                            case 4113700: //Londrina-PR
                                sigCorpService = new LondrinaPR.h.SigCorpH(tpAmb, PastaRetorno);
                                break;

                            case 4105508: //Cianorte-PR
                                sigCorpService = new CianortePR.h.SigCorpH(tpAmb, PastaRetorno);
                                break;

                            case 3130101: //Igarapé-MG
                                sigCorpService = new IgarapeMG.h.SigCorpH(tpAmb, PastaRetorno);
                                break;

                            case 4315602: //Rio Grande-RS
                                sigCorpService = new RioGrandeRS.h.SigCorpH(tpAmb, PastaRetorno);
                                break;

                            case 3505500: //Barretos-SP
                                sigCorpService = new BarretosSP.h.SigCorpH(tpAmb, PastaRetorno);
                                break;

                            case 3530706: //Mogi Guaçu-SP
                                sigCorpService = new MogiGuacuSP.h.SigCorpH(tpAmb, PastaRetorno);
                                break;

                            case 3507506: //Botucatu-SP
                                sigCorpService = new BotucatuSP.h.SigCorpH(tpAmb, PastaRetorno);
                                break;

                            default:
                                throw new Exception("Município não possui ambiente de homologação (padrão SIGCORP/SIGISS).");
                        }
                    else
                        switch (CodigoMun)
                        {
                            case 3529005: //Marilia-SP
                                sigCorpService = new MariliaSP.p.SigCorpP(tpAmb, PastaRetorno);
                                break;

                            case 3304904: //São Gonçalo-RJ
                                sigCorpService = new SaoGoncaloRJ.p.SigCorpP(tpAmb, PastaRetorno);
                                break;

                            case 3507506: //Botucatu-SP
                                sigCorpService = new BotucatuSP.p.SigCorpP(tpAmb, PastaRetorno);
                                break;

                            case 4113700: //Londrina-PR 
                                sigCorpService = new LondrinaPR.p.SigCorpP(tpAmb, PastaRetorno);
                                break;

                            case 3554805: //Tremembé-SP
                                sigCorpService = new TremembeSP.p.SigCorpP(tpAmb, PastaRetorno);
                                break;

                            case 4105508: //Cianorte-PR
                                sigCorpService = new CianortePR.p.SigCorpP(tpAmb, PastaRetorno);
                                break;

                            case 3130101: //Igarape-MG
                                sigCorpService = new IgarapeMG.p.SigCorpP(tpAmb, PastaRetorno);
                                break;

                            case 4315602: //Rio Grande-RS
                                sigCorpService = new RioGrandeRS.p.SigCorpP(tpAmb, PastaRetorno);
                                break;

                            case 3505500: //Barretos-SP
                                sigCorpService = new BarretosSP.p.SigCorpP(tpAmb, PastaRetorno);
                                break;

                            case 3530706: //Mogi Guaçu-SP
                                sigCorpService = new MogiGuacuSP.p.SigCorpP(tpAmb, PastaRetorno);
                                break;

                            default:
                                throw new Exception("Município não implementado para o ambiente de produção no padrão SIGCORP/SIGISS.");
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
