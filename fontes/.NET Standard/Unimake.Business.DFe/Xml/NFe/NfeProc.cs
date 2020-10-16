﻿#pragma warning disable CS1591

using System;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Unimake.Business.DFe.Xml.NFe
{
    [Serializable()]
    [XmlRoot("nfeProc", Namespace = "http://www.portalfiscal.inf.br/nfe", IsNullable = false)]
    public class NfeProc : XMLBase
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; }

        [XmlElement("NFe")]
        public NFe NFe { get; set; }

        [XmlElement("protNFe")]
        public ProtNFe ProtNFe { get; set; }

        /// <summary>
        /// Nome do arquivo de distribuição
        /// </summary>
        [XmlIgnore]
        public string NomeArquivoDistribuicao
        {
            get
            {
                switch (ProtNFe.InfProt.CStat)
                {
                    case 110: //Uso Denegado
                    case 205: //NF-e está denegada na base de dados da SEFAZ [nRec:999999999999999]
                    case 301: //Uso Denegado: Irregularidade fiscal do emitente
                    case 302: //Uso Denegado: Irregularidade fiscal do destinatário
                    case 303: //Uso Denegado: Destinatário não habilitado a operar na UF
                        return ProtNFe.InfProt.ChNFe + "-den.xml";

                    case 100: //Autorizado o uso da NF-e
                    case 150: //Autorizado o uso da NF-e, autorização fora de prazo
                    default:
                        return ProtNFe.InfProt.ChNFe + "-procnfe.xml";
                }
            }
        }

        public override XmlDocument GerarXML()
        {
            XmlDocument xmlDocument = base.GerarXML();

            XmlRootAttribute attribute = GetType().GetCustomAttribute<XmlRootAttribute>();
            XmlElement xmlElementNFe = (XmlElement)xmlDocument.GetElementsByTagName("NFe")[0];
            xmlElementNFe.SetAttribute("xmlns", attribute.Namespace);
            XmlElement xmlElementProtNFe = (XmlElement)xmlDocument.GetElementsByTagName("protNFe")[0];
            xmlElementProtNFe.SetAttribute("xmlns", attribute.Namespace);

            return xmlDocument;
        }
    }
}
