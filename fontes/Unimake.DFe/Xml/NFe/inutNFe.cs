using System;
using System.Xml;

namespace Unimake.DFe.Xml.NFe
{
    public class InutNFe : BaseXml
    {
        public string versao { get; set; }
        public string xmlns { get; set; }
        public TInfInut infInut = new TInfInut();

        /// <summary>
        /// Efetua a leitura do XML e disponibilizar o conteúdo nas propriedades da classe
        /// </summary>
        /// <param name="doc">XML a ser lido</param>
        public override void Ler(XmlDocument doc)
        {
            XmlNodeList xmlNodeList = doc.GetElementsByTagName("inutNFe");
            XmlElement xmlElement = (XmlElement)xmlNodeList[0];

            versao = xmlElement.GetAttribute("versao");
            if (xmlElement.GetAttribute("xmlns") != null)
            {
                xmlns = xmlElement.GetAttribute("xmlns");
            }

            XmlNodeList xmlNodeListInfInut = doc.GetElementsByTagName("infInut");
            XmlElement xmlElementInfInut = (XmlElement)xmlNodeListInfInut[0];

            infInut.Id = xmlElementInfInut.GetAttribute("Id");
            infInut.tpAmb = Convert.ToInt32(xmlElementInfInut.GetElementsByTagName("tpAmb")[0].InnerText);
            infInut.xServ = xmlElement.GetElementsByTagName("xServ")[0].InnerText;
            infInut.cUF = Convert.ToInt32(xmlElementInfInut.GetElementsByTagName("cUF")[0].InnerText);
            infInut.ano = xmlElementInfInut.GetElementsByTagName("ano")[0].InnerText;
            infInut.CNPJ = xmlElementInfInut.GetElementsByTagName("CNPJ")[0].InnerText;
            infInut.mod = xmlElementInfInut.GetElementsByTagName("mod")[0].InnerText;
            infInut.serie = xmlElementInfInut.GetElementsByTagName("serie")[0].InnerText;
            infInut.nNFIni = xmlElementInfInut.GetElementsByTagName("nNFIni")[0].InnerText;
            infInut.nNFFin = xmlElementInfInut.GetElementsByTagName("nNFFin")[0].InnerText;
            infInut.xJust = xmlElementInfInut.GetElementsByTagName("xJust")[0].InnerText;
        }

        public override XmlDocument Gerar()
        {
            throw new NotImplementedException();
        }
    }

    public class TInfInut
    {
        public string Id { get; set; }
        public int tpAmb { get; set; }
        public string xServ { get; set; }
        public int cUF { get; set; }
        public string ano { get; set; }
        public string CNPJ { get; set; }
        public string mod { get; set; }
        public string serie { get; set; }
        public string nNFIni { get; set; }
        public string nNFFin { get; set; }
        public string xJust { get; set; }
    }
}
