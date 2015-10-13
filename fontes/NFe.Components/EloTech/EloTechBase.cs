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

namespace NFe.Components.EloTech
{
    public abstract class EloTechBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        int CodigoMun = 0;
        string Usuario = "";
        string SenhaWs = "";
        string ProxyUser = "";
        string ProxyPass = "";
        string ProxyServer = "";
        EmiteNFSeBase elotechService;
        X509Certificate2 Certificate = null;

        protected EmiteNFSeBase EloTechService
        {
            get
            {
                if (elotechService == null)
                {
                    switch (CodigoMun)
                    {
                        case 4120804: //Quatro Barras - PR
                            elotechService = new NFe.Components.EloTech.QuatroBarrasPR.EloTechHP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, Certificate);
                            break;

                        default:
                            throw new Exceptions.ServicoInexistenteException();
                    }                                        
                }
                return elotechService;
            }
        }
        #endregion

        #region Construtores
        public EloTechBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver, X509Certificate2 certificado)
            : base(tpAmb, pastaRetorno)
        {
            CodigoMun = codMun;
            Usuario = usuario;
            SenhaWs = senhaWs;
            ProxyUser = proxyuser;
            ProxyPass = proxypass;
            ProxyServer = proxyserver;
            Certificate = certificado;
        }
        #endregion

        #region Métodos        
        public override void EmiteNF(string file)
        {
            EloTechService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            EloTechService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            EloTechService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            EloTechService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            EloTechService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            EloTechService.ConsultarNfsePorRps(file);
        }
        #endregion


    }
}
