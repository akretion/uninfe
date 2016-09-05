using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NFe.Components.Abstract;
using NFe.Components.br.com.rlz.saturno.santafedosul.h;

namespace NFe.Components.RLZ_INFORMATICA.SantaFeDoSul.h
{
    public class Rlz_InformaticaH : EmiteNFSeBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        WebservicePrefeitura service = new WebservicePrefeitura();
        string XmlString = "";

        #region Construtores
        public Rlz_InformaticaH(TipoAmbiente tpAmb, string pastaRetorno)
            : base(tpAmb, pastaRetorno)
        {

        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            ReadXML(file);
            string result = service.gravaNotaXML(XmlString);
            PrepararRetorno(file, base.CreateXML(result), Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML, 
                                                          Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        public override void ConsultarLoteRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        public override void ConsultarNfse(string file)
        {
            ReadXML(file);
            string result = service.listarNotasXML(XmlString);
            PrepararRetorno(file, base.CreateXML(result), Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        private void PrepararRetorno(string file, string result, string extEnvio, string extRetorno)
        {
            result = result.Substring(29);
            result = result.Substring(0, result.Length - 9);
            result = result.Replace("&lt;", "<");
            result = result.Replace("&gt;", ">");
            base.GerarRetorno(file, result.Trim(), extEnvio, extRetorno);
        }

        private void ReadXML(string file)
        {
            StreamReader SR = null;
            try
            {
                SR = File.OpenText(file);
                XmlString = SR.ReadToEnd();

            }
            catch (Exception)
            {
                throw new Exceptions.ProblemaLeituraXML(file);
            }
            finally
            {
                SR.Close();
                SR = null;
            }
        }
        #endregion
    }
}
