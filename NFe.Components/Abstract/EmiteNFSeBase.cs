using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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

        public void GerarRetorno(string file, string result, string extEnvio, string extRetorno)
        {
            FileInfo fi = new FileInfo(file);
            string nomearq = PastaRetorno + "\\" + fi.Name.Replace(extEnvio, extRetorno);

            StreamWriter write = new StreamWriter(nomearq);
            write.Write(result);
            write.Flush();
            write.Close();
            write.Dispose();
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
