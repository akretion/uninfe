using NFe.Components;
using NFe.Components.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NFSe.Components
{
    namespace Excpetions
    {
        /// <summary>
        /// Serviço não disponível para o padrão IPM
        /// </summary>
        public class ServicoInexistenteIPMException : NFe.Components.Exceptions.ServicoInexistenteException
        {
            public override string Message
            {
                get
                {
                    return "Serviço não disponível para padrão IPM";
                }
            }
        }
    }

    /// <summary>
    /// Emite notas fiscais de serviço no padrão IPM
    /// </summary>
    public class IPM : EmiteNFSeBase, IEmiteNFSeIPM
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region Construtores
        public IPM(TipoAmbiente tpAmb, string pastaRetorno, string usuario, string senha, int cidade)
            : base(tpAmb, pastaRetorno)
        {
            Usuario = usuario;
            Senha = senha;
            Cidade = CodigoTom(cidade);
            PastaRetorno = pastaRetorno;
        }

        #endregion Construtores

        public override void GerarRetorno(string file, string result, string extEnvio, string extRetorno)
        {
            FileInfo fi = new FileInfo(file);
            string nomearq = PastaRetorno + "\\" + fi.Name.Replace(extEnvio, extRetorno);

            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            StreamWriter write = new StreamWriter(nomearq, true, iso);
            write.Write(result);
            write.Flush();
            write.Close();
            write.Dispose();
        }

        public int CodigoTom(int nCodIbge)
        {
            switch (nCodIbge)
            {
                case 4104303: // Campo mourão - PR
                    return (int)7483;

                case 4309209: // Gravataí - RS
                    return (int)8683;

                case 4104204: // Campo Largo - PR
                    return (int)7481;

                case 4118204: // Paranaguá - PR
                    return (int)7745;

                case 4217808: // Taió - SC
                    return (int)8351;

                case 4201307: // Araquari - SC
                    return (int)8025;

                case 4215802: // São Bento do Sul-SC
                    return (int)8311;

                case 4307807: // Estrela-RS
                    return (int)8653;

                case 4211900: // Palhoça-SC
                    return (int)8233;
            }

            return 0;
        }

        public override void EmiteNF(string file)
        { }

        public override void CancelarNfse(string file)
        { }

        public override void ConsultarLoteRps(string file)
        { }

        public override void ConsultarSituacaoLoteRps(string file)
        { }

        public override void ConsultarNfse(string file)
        { }

        public override void ConsultarNfsePorRps(string file)
        { }

        public override string EmiteNF(string file, bool cancelamento)
        {
            string result = "";

            using (POSTRequest post = new POSTRequest
            {
                Proxy = Proxy
            })
            {
                //                                                                                                    informe 1 para retorno em xml
                result = post.PostForm("http://sync.nfs-e.net/datacenter/include/nfw/importa_nfw/nfw_import_upload.php?eletron=1", new Dictionary<string, string> {
                     {"login", Usuario  },  //CPF/CNPJ, sem separadores}
                     {"senha", Senha},      //Senha de acesso ao sistema: www.nfse.
                     {"cidade", Cidade.ToString()},   //Código da cidade na receita federal (TOM), pesquisei o código em http://www.ekwbrasil.com.br/municipio.php3.
                     {"f1", file}           //Endereço físico do arquivo
                });
            }

            if (!cancelamento)
                GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
            else
                GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);

            return result;
        }
    }
}