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

namespace NFe.Components.Memory
{
    public abstract class MemoryBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        int CodigoMun = 0;
        string Usuario = "";
        string SenhaWs = "";
        string ProxyUser = "";
        string ProxyPass = "";
        string ProxyServer = "";        
        EmiteNFSeBase memoryService;

        protected EmiteNFSeBase MemoryService
        {
            get
            {
                if (memoryService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                        memoryService = new PonteNovaMG.h.MemoryH(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, CodigoMun);

                    else
                        switch (CodigoMun)
                        {
                            case 3152105: // Ponte Nova - MG
                                memoryService = new PonteNovaMG.p.MemoryP(tpAmb, PastaRetorno, Usuario, SenhaWs, ProxyUser, ProxyPass, ProxyServer, CodigoMun);
                                break;

                            default:
                                throw new Exceptions.ServicoInexistenteException();
                        }
                }
                return memoryService;
            }
        }
        #endregion

        #region Construtores
        public MemoryBase(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver)
            : base(tpAmb, pastaRetorno)
        {
            CodigoMun = codMun;
            Usuario = usuario;
            SenhaWs = senhaWs;
            ProxyUser = proxyuser;
            ProxyPass = proxypass;
            ProxyServer = proxyserver;
        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            MemoryService.EmiteNF(file);
        }

        public override void CancelarNfse(string file)
        {
            MemoryService.CancelarNfse(file);
        }

        public override void ConsultarLoteRps(string file)
        {
            MemoryService.ConsultarLoteRps(file);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            MemoryService.ConsultarSituacaoLoteRps(file);
        }

        public override void ConsultarNfse(string file)
        {
            MemoryService.ConsultarNfse(file);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            MemoryService.ConsultarNfsePorRps(file);
        }
        #endregion


    }
}
