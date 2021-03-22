using NFe.Components.Abstract;
using NFSe.Components;
using System;
using System.IO;
using System.Text;

namespace NFe.Components.CENTI
{
    public class CENTI : EmiteNFSeBase
    {
        #region Public Properties

        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private string servico = "";
        private string rps = "";

        public string URLAPIBase
        {
            get
            {
                if (tpAmb.Equals(TipoAmbiente.taHomologacao))
                    return $@"https://api.centi.com.br/nfe/{servico}/homologacao{rps}/GO/edeia";
                else
                    return $@"https://api.centi.com.br/nfe/{servico}{rps}/go/edeia";
            }
        }

        #endregion Public Properties

        #region Public Construstor

        public CENTI(TipoAmbiente tpAmb, string pastaRetorno, string usuario, string senha)
            : base(tpAmb, pastaRetorno)
        {
            Usuario = usuario;
            Senha = senha;
        }

        #endregion Public Construstor

        #region Public Methods

        public override void EmiteNF(string file)
        {
            string result = "";
            servico = "gerar";

            using (POSTRequest post = new POSTRequest
            {
                Proxy = Proxy
            })
            {
                result = post.Post(Usuario, Senha, URLAPIBase, file);
            }

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                           Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void GerarRetorno(string file, string result, string extEnvio, string extRetorno)
        {
            FileInfo fi = new FileInfo(file);
            string nomearq = PastaRetorno + "\\" + fi.Name.Replace(extEnvio, extRetorno);

            Encoding iso = Encoding.GetEncoding("UTF-8");
            StreamWriter write = new StreamWriter(nomearq, false, iso);
            write.Write(result);
            write.Flush();
            write.Close();
            write.Dispose();
        }

        public override void CancelarNfse(string file)
        {
            string result = "";
            servico = "cancelar";

            using (POSTRequest post = new POSTRequest
            {
                Proxy = Proxy
            })
            {
                result = post.Post(Usuario, Senha, URLAPIBase, file);
            }

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void ConsultarNfse(string file)
        {
            string result = "";
            servico = "consultar";
            rps = "/rps";

            using (POSTRequest post = new POSTRequest
            {
                Proxy = Proxy
            })
            {
                result = post.Post(Usuario, Senha, URLAPIBase, file);
            }

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                                   Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        {
            throw new Exception();
        }

        public override void ConsultarNfsePorRps(string file)
        {
            throw new Exception();
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            throw new Exception();
        }

        #endregion Public Methods
    }
}