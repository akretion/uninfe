using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NFe.Components
{
    public class PadroesDataSource
    {
        public string fromDescription { get; set; }
        public string fromType { get; set; }
    }

    public class WebServiceNFSe
    {
        private static List<PadroesDataSource> _PadroesDataSource = null;
        private static List<PadroesDataSource> _PadroesUnicoWSDLDataSource = null;

        /// <summary>
        /// lista de padrões usados para preencher o datagrid e pesquisas internas
        /// </summary>
        public static List<PadroesDataSource> PadroesNFSeListDataSource
        {
            get
            {
                if (_PadroesDataSource == null)
                {
                    Array arr = Enum.GetValues(typeof(PadroesNFSe));
                    _PadroesDataSource = new List<PadroesDataSource>();
                    foreach (PadroesNFSe type in arr)
                        _PadroesDataSource.Add(new PadroesDataSource { fromType = type.ToString(), fromDescription = EnumHelper.GetEnumItemDescription(type) });
                }
                return _PadroesDataSource.OrderBy(p => p.fromDescription).ToList();
            }
        }

        public static List<PadroesDataSource> PadroesNFSeUnicoWSDLDataSource
        {
            get
            {
                if (_PadroesUnicoWSDLDataSource == null)
                {
                    Array arr = Enum.GetValues(typeof(PadroesNFSe));
                    _PadroesUnicoWSDLDataSource = new List<PadroesDataSource>();
                    foreach (PadroesNFSe type in arr)
                        switch (type)
                        {
                            case PadroesNFSe.NaoIdentificado:
                            case PadroesNFSe.GINFES:
                            case PadroesNFSe.BETHA:
                            case PadroesNFSe.EQUIPLANO:
                            case PadroesNFSe.ABASE:
                                _PadroesUnicoWSDLDataSource.Add(new PadroesDataSource { fromType = type.ToString(), fromDescription = EnumHelper.GetEnumItemDescription(type) });
                                break;
                        }
                }
                return _PadroesUnicoWSDLDataSource.OrderBy(p => p.fromDescription).ToList();
            }
        }

        private static List<string> _Padroes = null;

        /// <summary>
        /// lista de padrões usados para preencher o datagrid e pesquisas internas
        /// </summary>
        public static string[] PadroesNFSeList
        {
            get
            {
                if (_Padroes == null)
                {
                    Array arr = Enum.GetValues(typeof(PadroesNFSe));
                    _Padroes = new List<string>();
                    foreach (PadroesNFSe type in arr)
                        _Padroes.Add(type.ToString());
                }
                return _Padroes.ToArray();
            }
        }

        private static string getURLs(string local, ref PadroesNFSe padrao, int idMunicipio)
        {
            /*
             * tenta ler as URL's do 'WebService.xml'
             * não encontrando, assume o ID='padrao' e UF='XX' e padrao='padrao'
             */

            if (System.IO.File.Exists(Propriedade.NomeArqXMLWebService_NFSe))
            {
                XElement axml = null;

                try
                {
                    axml = XElement.Load(Propriedade.NomeArqXMLWebService_NFSe);
                }
                catch (Exception ex)
                {
                    Functions.WriteLog("Ocorreu um erro na tentativa de carregamento do arquivo " + Propriedade.NomeArqXMLWebService_NFSe + ".\r\n" +
                        "Acesse novamente o sistema para que se recupere automaticamente do erro.\r\n\r\n" +
                        "Erro:\r\n\r\n" + ex.Message, true, true, "");

                    if (System.IO.File.Exists(Propriedade.XMLVersaoWSDLXSD))
                        System.IO.File.Delete(Propriedade.XMLVersaoWSDLXSD);

                    Environment.Exit(0);
                }

                ///
                /// primeiro, pesquisa pelo ID
                ///
                var s = (from p in axml.Descendants(NFeStrConstants.Estado)
                         where  //(string)p.Attribute(NFe.Components.NFeStrConstants.Padrao) == padrao.ToString() &&
                                (string)p.Attribute(TpcnResources.ID.ToString()) == idMunicipio.ToString()
                         select p);
                foreach (var item in s)
                {
                    if (item.Element(local) != null)
                    {
                        ///
                        /// pega o padrao definido no WebService.xml descartando o que constar no UninMunic.xml
                        ///
                        padrao = WebServiceNFSe.GetPadraoFromString(item.Attribute(NFeStrConstants.Padrao).Value);

                        return local.Equals(NFeStrConstants.LocalHomologacao) ?
                            item.FirstNode.ToString() : item.LastNode.ToString();
                    }
                }
                ///
                /// não encontrei, assume o ID='padrao' e UF='XX' e padrao='padrao'
                ///
                PadroesNFSe pdr = padrao;

                var xs = (from p in axml.Descendants(NFeStrConstants.Estado)
                          where (string)p.Attribute(NFeStrConstants.Padrao) == pdr.ToString() &&
                                 (string)p.Attribute(TpcnResources.UF.ToString()) == "XX" &&
                                 (string)p.Attribute(TpcnResources.ID.ToString()) == pdr.ToString()
                          select p);
                foreach (var item in xs)
                {
                    if (item.Element(local) != null)
                        return local.Equals(NFeStrConstants.LocalHomologacao) ?
                            item.FirstNode.ToString() : item.LastNode.ToString();
                }
            }
            else
            {
                Functions.WriteLog("Ocorreu um erro na tentativa de carregamento do arquivo " + Propriedade.NomeArqXMLWebService_NFSe + ".\r\n" +
                    "Acesse novamente o sistema para que se recupere automaticamente do erro.", true, true, "");

                if (System.IO.File.Exists(Propriedade.XMLVersaoWSDLXSD))
                    System.IO.File.Delete(Propriedade.XMLVersaoWSDLXSD);

                Environment.Exit(0);
            }

            return "";
        }

        public static string WebServicesHomologacao(ref PadroesNFSe padrao, int idMunicipio = 0)
        {
            return getURLs(NFeStrConstants.LocalHomologacao, ref padrao, idMunicipio);
        }

        public static string WebServicesProducao(ref NFe.Components.PadroesNFSe padrao, int idMunicipio = 0)
        {
            return getURLs(NFeStrConstants.LocalProducao, ref padrao, idMunicipio);
        }

        public static PadroesNFSe GetPadraoFromString(string padrao)
        {
            try
            {
                return (PadroesNFSe)EnumHelper.StringToEnum<PadroesNFSe>(padrao);
            }
            catch
            {
                return PadroesNFSe.NaoIdentificado;
            }
        }

        public static void SalvarXMLMunicipios(string uf, string cidade, int codigomunicipio, string padrao, bool forcaAtualizacao)
        {
            try
            {
                if (uf != null)
                {
                    Municipio mun = Propriedade.Municipios.FirstOrDefault(w=>w.CodigoMunicipio == codigomunicipio);

                    var geral = PadroesNFSeUnicoWSDLDataSource.FirstOrDefault(w => w.fromType == GetPadraoFromString(padrao).ToString());

                    if (geral != null && mun != null)/*(padrao == PadroesNFSe.NaoIdentificado.ToString() ||
                         padrao == PadroesNFSe.BETHA.ToString() ||
                         padrao == PadroesNFSe.GINFES.ToString() ||
                         padrao == PadroesNFSe.EQUIPLANO.ToString() ||
                         padrao == PadroesNFSe.ABASE.ToString()) && mun != null)*/
                    {
                        ///
                        /// sai da lista "Municipios" pq esses padroes tem enderecos unicos
                        /// 
                        Propriedade.Municipios.Remove(mun);
                    }
                    else
                    {
                        if (padrao != PadroesNFSe.NaoIdentificado.ToString())
                        {
                            if (mun != null)
                            {
                                ///
                                /// é o mesmo padrão definido?
                                /// o parametro "forcaAtualizacao" é "true" somente quando vier da aba "Municipios definidos"
                                /// desde que o datagrid atualiza automaticamente o membro "padrao" da classe "Municipio" quando ele é alterado.
                                if (mun.PadraoStr == padrao && !forcaAtualizacao)
                                    return;

                                mun.Padrao = GetPadraoFromString(padrao);
                                mun.PadraoStr = padrao;
                            }
                            else
                                Propriedade.Municipios.Add(new Municipio(codigomunicipio, uf, cidade.Trim(), GetPadraoFromString(padrao)));
                        }
                    }
                }
                if (System.IO.File.Exists(Propriedade.NomeArqXMLMunicipios))
                {
                    ///
                    /// faz uma copia por segurança
                    if (System.IO.File.Exists(Propriedade.NomeArqXMLMunicipios + ".bck"))
                        System.IO.File.Delete(Propriedade.NomeArqXMLMunicipios + ".bck");
                    System.IO.File.Copy(Propriedade.NomeArqXMLMunicipios, Propriedade.NomeArqXMLMunicipios + ".bck");
                }

                /*
                <nfes_municipios>
                    <Registro ID="4125506" Nome="São José dos Pinais - PR" Padrao="GINFES" />
                </nfes_municipios>
                 */

                var xml = new XDocument(new XDeclaration("1.0", "utf-8", null));
                XElement elementos = new XElement("nfes_municipios");
                var r = (from ss in Propriedade.Municipios orderby ss.Nome select ss);
                foreach (Municipio item in r)
                {
                    elementos.Add(new XElement(NFeStrConstants.Registro,
                                    new XAttribute(TpcnResources.ID.ToString(), item.CodigoMunicipio.ToString()),
                                    new XAttribute(NFeStrConstants.Nome, item.Nome.Trim()),
                                    new XAttribute(NFeStrConstants.Padrao, item.PadraoStr)));
                }
                xml.Add(elementos);
                xml.Save(Propriedade.NomeArqXMLMunicipios);
            }
            catch (Exception ex)
            {
                //recupera a copia feita se houve erro na criacao do XML de municipios
                if (System.IO.File.Exists(Propriedade.NomeArqXMLMunicipios + ".bck"))
                    Functions.Move(Propriedade.NomeArqXMLMunicipios + ".bck", Propriedade.NomeArqXMLMunicipios);
                throw ex;
            }
        }

        /// <summary>
        /// Responsavel pela gravacao do arquivo de municipios, caso nao exista
        /// </summary>
        public static void SalvarXMLMunicipios()
        {
            if (!System.IO.File.Exists(Propriedade.NomeArqXMLMunicipios) &&
                System.IO.File.Exists(Propriedade.NomeArqXMLWebService_NFSe))
            {
                var xml = new XDocument(new XDeclaration("1.0", "utf-8", null));
                XElement elementos = new XElement("nfes_municipios");

                XElement axml = XElement.Load(Propriedade.NomeArqXMLWebService_NFSe);
                var s = (from p in axml.Descendants(NFeStrConstants.Estado)
                         where (string)p.Attribute(TpcnResources.UF.ToString()) != "XX"
                         orderby p.Attribute(NFeStrConstants.Nome).Value
                         select p);
                foreach (var item in s)
                {
                    string padrao = PadroesNFSe.NaoIdentificado.ToString();
                    if (item.Attribute(NFeStrConstants.Padrao) != null)
                        padrao = item.Attribute(NFeStrConstants.Padrao).Value;

                    if (padrao != PadroesNFSe.NaoIdentificado.ToString())
                    {
                        string ID = item.Attribute(TpcnResources.ID.ToString()).Value;
                        string Nome = item.Attribute(NFeStrConstants.Nome).Value;
                        string UF = item.Attribute(TpcnResources.UF.ToString()).Value;

                        elementos.Add(new XElement(NFeStrConstants.Registro,
                                        new XAttribute(TpcnResources.ID.ToString(), ID),
                                        new XAttribute(NFeStrConstants.Nome, Nome.Trim()),
                                        new XAttribute(NFeStrConstants.Padrao, padrao)));
                    }
                }
                if (!elementos.IsEmpty)
                {
                    xml.Add(elementos);
                    xml.Save(Propriedade.NomeArqXMLMunicipios);
                }
            }
        }
    }
}