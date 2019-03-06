using NFe.Components.Abstract;
using NFe.Components.PPetropolisRJ;
using System;
using System.Reflection;
using System.Xml;

namespace NFe.Components.SigCorp.PetropolisRJ.p
{
    public class SigCorpP : EmiteNFSeBase
    {
        WebServiceSigISS service = new WebServiceSigISS();

        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region construtores

        public SigCorpP(TipoAmbiente tpAmb, string pastaRetorno)
            : base(tpAmb, pastaRetorno)
        {
        }

        #endregion construtores

        #region Métodos

        public override void EmiteNF(string file)
        {
            tcDescricaoRps oTcDescricaoRps = ReadXML<tcDescricaoRps>(file);
            tcEstruturaDescricaoErros[] tcErros = null;
            tcRetornoNota result = service.GerarNota(oTcDescricaoRps, out tcErros);
            string strResult = base.CreateXML(result, tcErros);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            tcDadosCancelaNota oTcDadosCancela = ReadXML<tcDadosCancelaNota>(file);
            tcEstruturaDescricaoErros[] tcErros = null;
            tcRetornoNota result = service.CancelarNota(oTcDadosCancela, out tcErros);
            string strResult = base.CreateXML(result, tcErros);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        {
            tcDadosConsultaNota oTcDadosConsultaNota = ReadXML<tcDadosConsultaNota>(file);
            tcEstruturaDescricaoErros[] tcErros = null;

            tcRetornoNota result = service.ConsultarNotaValida(oTcDadosConsultaNota, out tcErros);
            string strResult = base.CreateXML(result, tcErros);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        public override void ConsultarNfse(string file)
        {
            tcDadosPrestador oTcDadosPrestador = ReadXML<tcDadosPrestador>(file);
            tcEstruturaDescricaoErros[] tcErros = null;
            tcDadosNota result = service.ConsultarNotaPrestador(oTcDadosPrestador, NumeroNota(file, "urn:ConsultarNotaPrestador"), out tcErros);
            string strResult = base.CreateXML(result, tcErros);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        private T ReadXML<T>(string file)
            where T : new()
        {
            T result = new T();
            result = (T)ReadXML2(file, result, result.GetType().Name.Substring(2));
            return result;
        }

        private object ReadXML2(string file, object value, string tag)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNodeList nodes = doc.GetElementsByTagName(tag);
            XmlNode node = nodes[0];
            if (node == null)
                throw new Exception("Tag <" + tag + "> não encontrada");

            foreach (XmlNode n in node)
            {
                if (n.NodeType == XmlNodeType.Element)
                {
                    SetProperrty(value, n.Name, n.InnerXml);
                }
            }
            return value;
        }

        private int NumeroNota(string file, string tag)
        {
            int nNumeroNota = 0;
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNodeList nodes = doc.GetElementsByTagName(tag);
            XmlNode node = nodes[0];
            if (node == null)
                throw new Exception("Tag <" + tag + "> não encontrada");

            foreach (XmlNode n in node)
            {
                if (n.NodeType == XmlNodeType.Element)
                {
                    if (n.Name.Equals("Nota"))
                    {
                        nNumeroNota = Convert.ToInt32("0" + n.InnerText);
                        break;
                    }
                }
            }
            return nNumeroNota;
        }

        private void SetProperrty(object result, string propertyName, object value)
        {
            PropertyInfo pi = result.GetType().GetProperty(propertyName);

            if (pi != null && !String.IsNullOrEmpty(value.ToString()))
            {
                value = Convert.ChangeType(value, pi.PropertyType);
                pi.SetValue(result, value, null);
            }
        }

        #endregion Métodos
    }
}