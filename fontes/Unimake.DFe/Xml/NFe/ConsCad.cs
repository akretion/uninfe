using System;
using System.Xml;
using Unimake.DFe.Servicos;

namespace Unimake.DFe.Xml.NFe
{
    public class ConsCad : BaseXml
    {
        public string versao { get; set; }
        public string xmlns { get; set; }
        public TInfCons infCons = new TInfCons();

        /// <summary>
        /// Efetua a leitura do XML e disponibilizar o conteúdo nas propriedades da classe
        /// </summary>
        /// <param name="doc">XML a ser lido</param>
        public override void Ler(XmlDocument doc)
        {
            XmlNodeList xmlNodeList = doc.GetElementsByTagName("ConsCad");
            XmlElement xmlElement = (XmlElement)xmlNodeList[0];

            versao = xmlElement.GetAttribute("versao");
            if (xmlElement.GetAttribute("xmlns") != null)
            {
                xmlns = xmlElement.GetAttribute("xmlns");
            }

            XmlNodeList xmlNodeListInfCons = doc.GetElementsByTagName("infCons");
            XmlElement xmlElementInfCons = (XmlElement)xmlNodeListInfCons[0];

            infCons.xServ = xmlElement.GetElementsByTagName("xServ")[0].InnerText;
            infCons.UF = xmlElementInfCons.GetElementsByTagName("UF")[0].InnerText;
            infCons.CNPJ = xmlElementInfCons.GetElementsByTagName("CNPJ")[0].InnerText;
        }

        public override XmlDocument Gerar()
        {
            throw new NotImplementedException();
        }
    }

    public class TInfCons
    {
        public string xServ { get; set; }
        public string CNPJ { get; set; }
        public int cUF { get; set; }

        public string UFField;
        public string UF
        {
            get => UFField;
            set
            {
                UFField = value;
                UnidadesFederativas unidade = (UnidadesFederativas)Enum.Parse(typeof(UnidadesFederativas), value.ToUpper());
                cUF = (int)unidade;
            }
        }
    }
}
