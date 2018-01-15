﻿using NFe.Components.Abstract;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.Fiorilli
{
    public abstract class FiorilliBase : EmiteNFSeBase
    {
        #region locais/ protegidos

        private int CodigoMun = 0;
        private string Usuario = "";
        private string SenhaWs = "";
        private string ProxyUser = "";
        private string ProxyPass = "";
        private string ProxyServer = "";
        private X509Certificate2 Certificado;
        private EmiteNFSeBase fiorilliService;

        protected EmiteNFSeBase FiorilliService
        {
            get
            {
                if (fiorilliService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        switch (CodigoMun)
                        {
                            case 3522802: //Itaporanga-SP
                            case 3512902: //Cosmorama-SP
                            case 3553807: //Taquarituba-SP
                                fiorilliService = new TaquaraSP.h.FiorilliH(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 3504503: //Avaré-SP
                            case 3524501: //Jaci-SP
                                fiorilliService = new AvareSP.h.FiorilliH(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 3504008: //Assis-SP
                            case 3530409: //Mirassolândia-SP
                                fiorilliService = new MirassolandiaSP.h.FiorilliH(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 5003207: //Corumbá-MS
                                fiorilliService = new CorumbaMS.h.FiorilliH(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 1600303: //Macapá-AP
                                fiorilliService = new MacapaAP.h.FiorilliH(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 3540804: //Potirendaba-SP
                                fiorilliService = new PotirendabaSP.h.FiorilliH(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 4320404: //Serafina Corrêa-RS
                                fiorilliService = new SerafinaCorreaRS.h.FiorilliH(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                    else
                        switch (CodigoMun)
                        {
                            case 3504008: //Assis-SP
                                fiorilliService = new AssisSP.p.FiorilliP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 3553807: //Taquarituba-SP
                                fiorilliService = new TaquaraSP.p.FiorilliP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 3512902: //Cosmorama-SP
                                fiorilliService = new CosmoramaSP.p.FiorilliP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 3504503: //Avaré-SP
                                fiorilliService = new AvareSP.p.FiorilliP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 3522802: //Itaporanga-SP
                                fiorilliService = new ItaporangaSP.p.FiorilliP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 3524501: //Jaci-SP
                                fiorilliService = new JaciSP.p.FiorilliP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 3530409: //Mirassolândia-SP
                                fiorilliService = new MirassolandiaSP.p.FiorilliP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 5003207: //Corumbá-MS
                                fiorilliService = new CorumbaMS.p.FiorilliP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 1600303: //Macapá-AP
                                fiorilliService = new MacapaAP.p.FiorilliP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 3540804: //Potirendaba-SP
                                fiorilliService = new PotirendabaSP.p.FiorilliP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            case 4320404: //Serafina Corrêa-RS
                                fiorilliService = new SerafinaCorreaRS.p.FiorilliP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificado);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                }
                return fiorilliService;
            }
        }

        #endregion locais/ protegidos

        #region Construtores

        public FiorilliBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver, X509Certificate2 certificado)
            : base(tpAmb, pastaRetorno)
        {
            CodigoMun = codMun;
            Usuario = usuario;
            SenhaWs = senhaWs;
            ProxyUser = proxyuser;
            ProxyPass = proxypass;
            ProxyServer = proxyserver;
            Certificado = certificado;
        }

        #endregion Construtores

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

        #endregion Métodos
    }
}