using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Reflection;

namespace NFe.Components.Abstract
{
    public abstract class EmiteNFSeBase : IEmiteNFSe
    {
        public TipoAmbiente tpAmb { get; set; }
        public string PastaRetorno { get; set; }

        public EmiteNFSeBase(TipoAmbiente tpAmb, string pastaRetorno)
        {
            this.tpAmb = tpAmb;
            this.PastaRetorno = pastaRetorno;
        }

        /// WANDREY:
        ///  nao precisa passar a extensao para excluir, porque podemos varrer a lista de extensoes e exclui-la
        ///  
        public void GerarRetorno(string file, string result, string extEnvio, string extRetorno)
        {
            string nomearq = Path.Combine(PastaRetorno, Functions.ExtrairNomeArq(file, extEnvio) + extRetorno);

            //FileInfo fi = new FileInfo(file);
            //string nomearq = PastaRetorno + "\\" + fi.Name.ToLower().Replace(extEnvio.ToLower(), extRetorno.ToLower());

            StreamWriter write = new StreamWriter(nomearq);
            write.Write(result);
            write.Flush();
            write.Close();
            write.Dispose();
        }

        public string CreateXML(Object objetoRetorno)
        {
            Object tcErros = null;

            return CreateXML(objetoRetorno, tcErros);
        }

        public string CreateXML(Object objetoRetorno, Object tcErros)
        {
            XmlDocument xmlDoc = new XmlDocument();

            if (objetoRetorno != null)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(objetoRetorno.GetType());
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, objetoRetorno);
                    xmlStream.Position = 0;
                    xmlDoc.Load(xmlStream);
                }
            }

            if (tcErros != null)
            {
                XmlDocument xmlDoc2 = new XmlDocument();
                XmlSerializer xmlSerializer2 = new XmlSerializer(tcErros.GetType());
                using (MemoryStream xmlStream2 = new MemoryStream())
                {
                    xmlSerializer2.Serialize(xmlStream2, tcErros);
                    xmlStream2.Position = 0;
                    xmlDoc2.Load(xmlStream2);
                }

                if (objetoRetorno != null)
                {
                    XmlNode importedDocument = xmlDoc.ImportNode(xmlDoc2.DocumentElement, true);
                    xmlDoc.DocumentElement.AppendChild(importedDocument);
                }
                else
                {
                    xmlDoc = xmlDoc2;
                }
                
            }

            return xmlDoc.InnerXml;
        }

        public abstract void EmiteNF(string file);

        public abstract void CancelarNfse(string file);

        public abstract void ConsultarLoteRps(string file);

        public abstract void ConsultarSituacaoLoteRps(string file);

        public abstract void ConsultarNfse(string file);

        public abstract void ConsultarNfsePorRps(string file);

        public object WSGeracao
        {
            get;
            protected set;
        }

        public object WSConsultas
        {
            get;
            protected set;
        }
    }
}
