using NFe.Components;
using NFe.Components.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            public override string Message => "Serviço não disponível para padrão IPM";
        }
    }

    /// <summary>
    /// Emite notas fiscais de serviço no padrão IPM
    /// </summary>
    public class IPM : EmiteNFSeBase, IEmiteNFSeIPM
    {
        public override string NameSpaces => throw new NotImplementedException();

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
                    return 7483;

                case 4309209: // Gravataí - RS
                    return 8683;

                case 4104204: // Campo Largo - PR
                    return 7481;

                case 4118204: // Paranaguá - PR
                    return 7745;

                case 4217808: // Taió - SC
                    return 8351;

                case 4201307: // Araquari - SC
                    return 8025;

                case 4215802: // São Bento do Sul-SC
                    return 8311;

                case 4307807: // Estrela-RS
                    return 8653;

                case 4211900: // Palhoça-SC
                    return 8233;

                case 4317202: // Santa Rosa-RS
                    return 8847;

                case 4202909: // Brusque-SC
                    return 8055;

                case 4302105: // Bento Gonçalves-RS
                    return 8041;

                case 4207502: // Indaial-SC
                    return 8147;

                case 4211801: // Ouro-SC
                    return 8231;

                case 4119152: // Pinhais-PR
                    return 5453;

                case 4127205: // Terra Boa - PR

                case 4313508: // Osório-RS 
                    return 8773;

                case 4118006: //Paraíso do Norte - PR
                    return 7741;

                case 4300604: //Alvorada-RS
                    return 8511;

                case 4104907: //Castro-PR
                    return 7495;

                case 4104808: //Cascavel-PR
                    return 74934;

                case 4303103: //Cachoeirinha-SC
                    return 85618;

                case 4114609: //Marechal Cândido Rondon-PR
                    return 7683;

                case 4213203: //Pomerode - SC
                    return 8259;
            }

            return 0;
        }

        public override void EmiteNF(string file)
        {
            string result = EnviaXML(file);

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            string result = EnviaXML(file);

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        { }

        public override void ConsultarSituacaoLoteRps(string file)
        { }

        public override void ConsultarNfse(string file)
        {
            string result = EnviaXML(file);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            doc.DocumentElement.RemoveChild(doc.GetElementsByTagName("codigo_html")[0]);

            result = doc.OuterXml;

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            string result = EnviaXML(file);

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML);
        }

        private string EnviaXML(string file)
        {
            string result = "";

            using (POSTRequest post = new POSTRequest
            {
                Proxy = Proxy
            })
            {
                if (Cidade == 74934 || Cidade == 4104808)
                {
                    //                                                                                                    informe 1 para retorno em xml
                    result = post.PostForm("http://sync-pr.nfs-e.net/datacenter/include/nfw/importa_nfw/nfw_import_upload.php?eletron=1", new Dictionary<string, string> {
                     {"login", Usuario  },  //CPF/CNPJ, sem separadores}
                     {"senha", Senha},      //Senha de acesso ao sistema: www.nfse.
                     {"cidade", Cidade.ToString()},   //Código da cidade na receita federal (TOM), pesquisei o código em http://www.ekwbrasil.com.br/municipio.php3.
                     {"f1", file}           //Endereço físico do arquivo
                });
                }
                else
                {
                    //                                                                                                    informe 1 para retorno em xml
                    result = post.PostForm("http://sync.nfs-e.net/datacenter/include/nfw/importa_nfw/nfw_import_upload.php?eletron=1", new Dictionary<string, string> {
                     {"login", Usuario  },  //CPF/CNPJ, sem separadores}
                     {"senha", Senha},      //Senha de acesso ao sistema: www.nfse.
                     {"cidade", Cidade.ToString()},   //Código da cidade na receita federal (TOM), pesquisei o código em http://www.ekwbrasil.com.br/municipio.php3.
                     {"f1", file}           //Endereço físico do arquivo
                });
                }
            }

            return result;
        }
    }
}