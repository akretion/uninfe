using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using NFe.Components;

namespace NFe.Components
{
    public class WebServiceNFSe
    {
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

        private static string getURLs(string local, PadroesNFSe padrao, int idMunicipio)
        {
            /*
             * tenta ler as URL's do 'WebService.xml'
             * não encontrando, assume o ID='padrao' e UF='XX' e padrao='padrao'
             */

            if (System.IO.File.Exists(Propriedade.NomeArqXMLWebService_NFSe))
            {
                XElement axml = XElement.Load(Propriedade.NomeArqXMLWebService_NFSe);
                var s = (from p in axml.Descendants(NFe.Components.NFeStrConstants.Estado)
                         where (string)p.Attribute(NFe.Components.NFeStrConstants.Padrao) == padrao.ToString() &&
                                (string)p.Attribute(NFe.Components.NFeStrConstants.ID) == idMunicipio.ToString()
                         select p);
                foreach (var item in s)
                {
                    if (item.Element(local) != null)
                        return local.Equals(NFe.Components.NFeStrConstants.LocalHomologacao) ?
                            item.FirstNode.ToString() : item.LastNode.ToString();
                }

                var xs = (from p in axml.Descendants(NFe.Components.NFeStrConstants.Estado)
                          where (string)p.Attribute(NFe.Components.NFeStrConstants.Padrao) == padrao.ToString() &&
                                 (string)p.Attribute(NFe.Components.NFeStrConstants.UF) == "XX" &&
                                 (string)p.Attribute(NFe.Components.NFeStrConstants.ID) == padrao.ToString()
                          select p);
                foreach (var item in xs)
                {
                    if (item.Element(local) != null)
                        return local.Equals(NFe.Components.NFeStrConstants.LocalHomologacao) ?
                            item.FirstNode.ToString() : item.LastNode.ToString();
                }
            }

            return "";
        }

        public static string WebServicesHomologacao(PadroesNFSe padrao, int idMunicipio = 0)
        {
            string result = getURLs(NFe.Components.NFeStrConstants.LocalHomologacao, padrao, idMunicipio);
    
            return result;

        }

        public static string WebServicesProducao(NFe.Components.PadroesNFSe padrao, int idMunicipio = 0)
        {
            string result = getURLs(NFe.Components.NFeStrConstants.LocalProducao, padrao, idMunicipio);

            return result;
        }

        public static NFe.Components.PadroesNFSe GetPadraoFromString(string padrao)
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
                    Municipio mun = null;
                    for (int i = 0; i < Propriedade.Municipios.Count; ++i)
                        if (Propriedade.Municipios[i].CodigoMunicipio == codigomunicipio)
                        {
                            mun = Propriedade.Municipios[i];
                            break;
                        }

                    if (padrao == PadroesNFSe.NaoIdentificado.ToString() && mun != null)
                        Propriedade.Municipios.Remove(mun);

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
                foreach (Municipio item in Propriedade.Municipios)
                {
                    elementos.Add(new XElement(NFe.Components.NFeStrConstants.Registro,
                                    new XAttribute(NFe.Components.NFeStrConstants.ID, item.CodigoMunicipio.ToString()),
                                    new XAttribute(NFe.Components.NFeStrConstants.Nome, item.Nome.Trim()),
                                    new XAttribute(NFe.Components.NFeStrConstants.Padrao, item.PadraoStr)));
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
                         where  (string)p.Attribute(NFe.Components.NFeStrConstants.UF) != "XX"
                         select p);
                foreach (var item in s)
                {
                    string padrao = PadroesNFSe.NaoIdentificado.ToString();
                    if (item.Attribute(NFe.Components.NFeStrConstants.Padrao) != null)
                        padrao = item.Attribute(NFe.Components.NFeStrConstants.Padrao).Value;

                    if (padrao != PadroesNFSe.NaoIdentificado.ToString())
                    {
                        string ID = item.Attribute(NFe.Components.NFeStrConstants.ID).Value;
                        string Nome = item.Attribute(NFe.Components.NFeStrConstants.Nome).Value;
                        string UF = item.Attribute(NFe.Components.NFeStrConstants.UF).Value;

                        elementos.Add(new XElement(NFe.Components.NFeStrConstants.Registro,
                                        new XAttribute(NFe.Components.NFeStrConstants.ID, ID),
                                        new XAttribute(NFe.Components.NFeStrConstants.Nome, Nome.Trim()),
                                        new XAttribute(NFe.Components.NFeStrConstants.Padrao, padrao)));
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
