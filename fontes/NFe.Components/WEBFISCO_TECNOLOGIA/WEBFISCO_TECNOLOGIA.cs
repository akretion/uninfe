using NFe.Components.Abstract;
using NFe.Components.WEBFISCO_TECNOLOGIA.BarraBonitaSP.h;
using NFe.Components.WEBFISCO_TECNOLOGIA.BarraBonitaSP.p;
using System;

namespace NFe.Components.WEBFISCO_TECNOLOGIA
{
    public class WEBFISCO_TECNOLOGIA : EmiteNFSeBase
    {
        private readonly int CodigoMun = 0;
        private readonly string Usuario = "";
        private readonly string Senha = "";
        private readonly EmiteNFSeBase MGMService;

        public override string NameSpaces => throw new NotImplementedException();

        #region Construtures

        public WEBFISCO_TECNOLOGIA(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuario, string senha) : base(tpAmb, pastaRetorno)
        {
            CodigoMun = codMun;
            Usuario = usuario;
            Senha = senha;

            if (tpAmb == TipoAmbiente.taHomologacao)
            {
                MGMService = new WEBFISCO_TECNOLOGIAH(tpAmb, PastaRetorno);
            }
            else
            {
                MGMService = new WEBFISCO_TECNOLOGIAP(tpAmb, PastaRetorno, Usuario, Senha);
            }
        }

        #endregion Construtures


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

        public override void ConsultarXml(string file)
        {
            MGMService.ConsultarXml(file);
        }

        #endregion Métodos
    }
}