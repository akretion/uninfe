using NFe.Components;
using System;
using System.Xml;

namespace NFe.Validate
{
    public class RespTecnico
    {
        public RespTecnico(string respTecCNPJ, string respTecXContato, string respTecEmail, string respTecTelefone, string respTecIdCSRT, string respTecCSRT)
        {
            RespTecCNPJ = respTecCNPJ;
            RespTecXContato = respTecXContato;
            RespTecEmail = respTecEmail;
            RespTecTelefone = respTecTelefone;
            RespTecIdCSRT = respTecIdCSRT;
            RespTecCSRT = respTecCSRT;
        }

        private string RespTecCNPJ { get; set; }
        private string RespTecXContato { get; set; }
        private string RespTecEmail { get; set; }
        private string RespTecTelefone { get; set; }
        private string RespTecIdCSRT { get; set; }
        private string RespTecCSRT { get; set; }

        public void AdicionarResponsavelTecnico(XmlDocument conteudoXML)
        {
            if (conteudoXML.GetElementsByTagName("infNFe")[0] == null)
            {
                return;
            }

            DateTime dhEmi = Convert.ToDateTime(conteudoXML.GetElementsByTagName("dhEmi")[0].InnerText);

            if (conteudoXML.GetElementsByTagName("tpAmb")[0].InnerText == "1" && dhEmi >= new DateTime(2019, 5, 7))
                return;

            XmlNode infRespTec = conteudoXML.GetElementsByTagName("infRespTec")[0];

            if (infRespTec != null)
                return;

            if (!string.IsNullOrEmpty(RespTecCNPJ) ||
                !string.IsNullOrEmpty(RespTecXContato) ||
                !string.IsNullOrEmpty(RespTecEmail) ||
                !string.IsNullOrEmpty(RespTecTelefone) ||
                !string.IsNullOrEmpty(RespTecIdCSRT) ||
                !string.IsNullOrEmpty(RespTecCSRT))
            {
                XmlNode infNFe = conteudoXML.GetElementsByTagName("infNFe")[0];
                string chaveNFe = infNFe.Attributes.GetNamedItem("Id").InnerText.Substring(3, 44);

                XmlElement infRespTecnico = conteudoXML.CreateElement("infRespTec", infNFe.NamespaceURI);
                XmlNode cnpj = conteudoXML.CreateElement("CNPJ", infNFe.NamespaceURI);
                XmlNode contato = conteudoXML.CreateElement("xContato", infNFe.NamespaceURI);
                XmlNode email = conteudoXML.CreateElement("email", infNFe.NamespaceURI);
                XmlNode fone = conteudoXML.CreateElement("fone", infNFe.NamespaceURI);
                XmlNode idCSRT = conteudoXML.CreateElement("idCSRT", infNFe.NamespaceURI);
                XmlNode csrt = conteudoXML.CreateElement("hashCSRT", infNFe.NamespaceURI);

                cnpj.InnerText = RespTecCNPJ;
                contato.InnerText = RespTecXContato;
                email.InnerText = RespTecEmail;
                fone.InnerText = RespTecTelefone;

                infRespTecnico.AppendChild(cnpj);
                infRespTecnico.AppendChild(contato);
                infRespTecnico.AppendChild(email);
                infRespTecnico.AppendChild(fone);

                if (!string.IsNullOrEmpty(RespTecIdCSRT) &&
                    !string.IsNullOrEmpty(RespTecCSRT))
                {
                    idCSRT.InnerText = RespTecIdCSRT;
                    csrt.InnerText = GerarHashCSRT(RespTecCSRT, chaveNFe);

                    infRespTecnico.AppendChild(idCSRT);
                    infRespTecnico.AppendChild(csrt);
                }

                infNFe.AppendChild(infRespTecnico);
            }
        }

        private string GerarHashCSRT(string csrt, string chaveNFe)
        {
            string result = Criptografia.GetSHA1HashData(csrt + chaveNFe);
            result = Functions.ToBase64Hex(result);

            return result;
        }

    }
}
