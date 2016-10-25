using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFe.Components;
using System.Net;
using System.IO;
using System.Xml;

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
    public class IPM : IEmiteNFSeIPM
    {
        #region Propriedades
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public int Cidade { get; set; }
        public IWebProxy Proxy { get; set; }
        public string PastaRetorno { get; set; }
        #endregion

        #region Construtores
        public IPM(string usuario, string senha, int cidade, string caminhoRetorno)
        {
            Usuario = usuario;
            Senha = senha;
            Cidade = CodigoTom(cidade);
            PastaRetorno = caminhoRetorno;
        }

        #endregion

        public string EmitirNF(string file, TipoAmbiente tpAmb, bool cancelamento = false)
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

        public void GerarRetorno(string file, string result, string extEnvio, string extRetorno)
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
            }

            return 0;
        }

    }

}
