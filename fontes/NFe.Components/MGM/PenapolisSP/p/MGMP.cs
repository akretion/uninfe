using NFe.Components.Abstract;
using System;
using System.Xml;

namespace NFe.Components.MGM.PenapolisSP.p
{
    public class MGMP : EmiteNFSeBase
    {
        string Login;
        string Senha;

        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        NFe.Components.br.com.fgmaiss.www.p.penapois.envio.webservice serviceEnvio = new NFe.Components.br.com.fgmaiss.www.p.penapois.envio.webservice();
        NFe.Components.br.com.fgmaiss.www.p.penapolis.cancela.webservice serviceCancela = new br.com.fgmaiss.www.p.penapolis.cancela.webservice();
        NFe.Components.br.com.fgmaiss.www.p.penapolis.consulta.webservice serviceConsulta = new br.com.fgmaiss.www.p.penapolis.consulta.webservice();
        NFe.Components.br.com.fgmaiss.www.p.penapolis.consultaxml.webservice serviceConsultaXML = new br.com.fgmaiss.www.p.penapolis.consultaxml.webservice();

        #region construtores

        public MGMP(TipoAmbiente tpAmb, string pastaRetorno, string usuario, string senha)
            : base(tpAmb, pastaRetorno)
        {
            Login = usuario;
            Senha = senha;
        }

        #endregion construtores

        #region Métodos

        public override void EmiteNF(string file)
        {
            XmlTextReader reader = new XmlTextReader(file);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            reader.MoveToContent();

            XmlDocument oXml = new XmlDocument();
            oXml.Load(reader);
            reader.Close();

            Array result = serviceEnvio.EnvNfe(Login,
                                                Senha,
                                                XmlDocumentUtilities.GetValue<string>(oXml, "prf"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "usr"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "ctr"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "cnpj"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "cnpjn"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "ie"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "im"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "lgr"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "num"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "cpl"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "bai"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "cid"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "est"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "cep"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "fon"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "mail"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "dat"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "f1n"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "f1d"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "f1v"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "f2n"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "f2d"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "f2v"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "f3n"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "f3d"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "f3v"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "f4n"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "f4d"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "f4v"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "f5n"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "f5d"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "f5v"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "f6n"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "f6d"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "f6v"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "item1"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "item2"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "item3"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "aliq1"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "aliq2"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "aliq3"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "val1"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "val2"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "val3"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "loc"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "ret"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "txt"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "val"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "valtrib"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "iss"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "issret"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "desci"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "desco"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "binss"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "birrf"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "bcsll"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "bpis"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "bcofins"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "ainss"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "airrf"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "acsll"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "apis"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "acofins"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "inss"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "irrf"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "csll"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "pis"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "cofins"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "item4"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "item5"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "item6"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "item7"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "item8"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "aliq4"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "aliq5"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "aliq6"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "aliq7"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "aliq8"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "val4"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "val5"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "val6"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "val7"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "val8"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "iteser1"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "iteser2"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "iteser3"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "iteser4"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "iteser5"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "iteser6"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "iteser7"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "iteser8"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "alqser1"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "alqser2"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "alqser3"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "alqser4"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "alqser5"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "alqser6"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "alqser7"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "alqser8"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "valser1"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "valser2"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "valser3"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "valser4"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "valser5"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "valser6"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "valser7"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "valser8"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "paisest"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "ssrecbr"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "ssanexo"),
                                                XmlDocumentUtilities.GetValue<string>(oXml, "ssdtini"));
                
            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);//.LoteRps);
        }

        public override void CancelarNfse(string file)
        {
            XmlTextReader reader = new XmlTextReader(file);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            reader.MoveToContent();

            XmlDocument oXml = new XmlDocument();
            oXml.Load(reader);

            Array result = serviceCancela.CancelaNfe(Login,
                                                     Senha,
                                                     XmlDocumentUtilities.GetValue<string>(oXml, "prf"),
                                                     XmlDocumentUtilities.GetValue<string>(oXml, "usr"),
                                                     XmlDocumentUtilities.GetValue<string>(oXml, "ctr"),
                                                     XmlDocumentUtilities.GetValue<string>(oXml, "tipo"),
                                                     XmlDocumentUtilities.GetValue<string>(oXml, "obs"));

            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
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
            XmlTextReader reader = new XmlTextReader(file);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            reader.MoveToContent();

            XmlDocument oXml = new XmlDocument();
            oXml.Load(reader);

            Array result = serviceConsulta.ConsultaNfe(Login,
                                                       Senha,
                                                       XmlDocumentUtilities.GetValue<string>(oXml, "prf"),
                                                       XmlDocumentUtilities.GetValue<string>(oXml, "usr"),
                                                       XmlDocumentUtilities.GetValue<string>(oXml, "ctr"),
                                                       XmlDocumentUtilities.GetValue<string>(oXml, "tipo"));

            string strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        #endregion Métodos
    }
}