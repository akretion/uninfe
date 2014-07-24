using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace uninfe_ws
{
    public class Estado
    {
        public string Nome { get; set; }
        public string ID { get; set; }
        public string UF { get; set; }
        public string Padrao { get; set; }

        public string text
        {
            get
            {
                if (string.IsNullOrEmpty(Padrao))
                    return Nome + " - " + UF + " - " + ID;

                return Nome + " - " +  ID + (string.IsNullOrEmpty(Padrao) ? "" : " - " + Padrao);
            }
        }
        public string key
        {
            get
            {
                return UF + " - " + ID + (string.IsNullOrEmpty(Padrao) ? "" : " - " + Padrao);
            }
        }

        public Estado()
        {
            this.Nome = this.Padrao = this.UF = this.ID = "";
        }
    }

    public class dummy
    {
        public static Dictionary<string, int> listageral = new Dictionary<string, int>();

        public static string LerWSDLS(string configname, string ID, string UF, string Padrao)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(configname);

            foreach (var node in doc.GetElementsByTagName(NFe.Components.NFeStrConstants.Estado))
            {
                XmlElement ele = (XmlElement)node;
                string p = NFe.Components.PadroesNFSe.NaoIdentificado.ToString();
                if (ele.Attributes.Count > 0)
                {
                    ///
                    /// versao antiga
                    try
                    {
                        p = ele.Attributes[NFe.Components.NFeStrConstants.Padrao].Value;
                    }
                    catch { }
                    if (ele.Attributes[NFe.Components.NFeStrConstants.ID].Value.Equals(ID) &&
                        ele.Attributes[NFe.Components.NFeStrConstants.UF].Value.Equals(UF) &&
                        p.Equals(Padrao))
                    {
                        return ele.OuterXml;
                    }
                }
                else
                {
                    string id = NFe.Components.Functions.LerTag(ele, NFe.Components.NFeStrConstants.ID, false);
                    string uf = NFe.Components.Functions.LerTag(ele, NFe.Components.NFeStrConstants.UF, false);
                    string pa = NFe.Components.Functions.LerTag(ele, NFe.Components.NFeStrConstants.Padrao, false);
                    if (id == ID && uf == UF && Padrao == pa)
                        return ele.OuterXml;
                }
            }
            return "";
        }

        public static void ArquivoExiste(StringBuilder erro, string configname, string wsdlname)
        {
            if (!string.IsNullOrEmpty(wsdlname))
            {
                string o = wsdlname;
                ///
                /// configname contem a path completa
                /// extraimos o diretorio dela até o nome "wsdl"
                /// 
                /// a variavel "wsdlname" já contem o texto "wsdl\nome do wsdl"
                wsdlname = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(configname)) + "\\" + wsdlname;
                if (!System.IO.File.Exists(wsdlname))
                    erro.AppendLine(".WSDL '" + o + "' não encontrado");
            }
        }
    }
}
