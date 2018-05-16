using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using NFe.Components;

namespace uninfe_ws
{
    public class Estado
    {
        public string Nome { get; set; }
        public string ID { get; set; }
        public string UF { get; set; }
        public string Padrao { get; set; }
        public string svc { get; set; }

        public string text
        {
            get
            {
                if (string.IsNullOrEmpty(Padrao))
                    return Nome + " - " + UF + " - " + ID;

                if (Nome.StartsWith("Geral - "))
                    return Nome;
                return Nome + " - " + ID + (string.IsNullOrEmpty(Padrao) ? "" : " - " + Padrao);
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
