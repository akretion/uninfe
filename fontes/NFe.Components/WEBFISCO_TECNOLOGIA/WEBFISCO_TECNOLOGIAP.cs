using NFe.Components.Abstract;
using System;
using System.Xml;

namespace NFe.Components.WEBFISCO_TECNOLOGIA.BarraBonitaSP.p
{
    public class WEBFISCO_TECNOLOGIAP : EmiteNFSeBase
    {
        private readonly string Login;
        private readonly string Senha;

        public override string NameSpaces => throw new NotImplementedException();

        private readonly PWebfiscoTecnologia_Enviar.webservice envioService = new PWebfiscoTecnologia_Enviar.webservice();
        private readonly PWebfiscoTecnologia_Cancelar.webservice cancelaService = new PWebfiscoTecnologia_Cancelar.webservice();
        private readonly PWebfiscoTecnologia_ConsultarNfe.webservice consultaNfeService = new PWebfiscoTecnologia_ConsultarNfe.webservice();
        private readonly PWebFiscoTecnologia_ConsultarXml.webservice consultaXmlService = new PWebFiscoTecnologia_ConsultarXml.webservice();

        #region construtores

        public WEBFISCO_TECNOLOGIAP(TipoAmbiente tpAmb, string pastaRetorno, string usuario, string senha)
            : base(tpAmb, pastaRetorno)
        {
            Login = usuario;
            Senha = senha;
        }

        #endregion construtores

        #region Métodos

        public override void EmiteNF(string file)
        {
            var reader = new XmlTextReader(file)
            {
                WhitespaceHandling = WhitespaceHandling.None
            };
            reader.MoveToContent();

            var oXml = new XmlDocument();
            oXml.Load(reader);
            reader.Close();

            var result = envioService.EnvNfe(Login, Senha,
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
                XmlDocumentUtilities.GetValue<string>(oXml, "ssdtini"),
                XmlDocumentUtilities.GetValue<string>(oXml, "percded"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsac1"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsaq1"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsav1"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsat1"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsac2"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsaq2"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsav2"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsat2"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsac3"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsaq3"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsav3"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsat3"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsac4"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsaq4"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsav4"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsat4"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsac5"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsaq5"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsav5"),
                XmlDocumentUtilities.GetValue<string>(oXml, "itemsat5"));

            var strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            var reader = new XmlTextReader(file)
            {
                WhitespaceHandling = WhitespaceHandling.None
            };
            reader.MoveToContent();

            var oXml = new XmlDocument();
            oXml.Load(reader);
            reader.Close();

            Array result = cancelaService.CancelaNfe(Login, Senha,
                XmlDocumentUtilities.GetValue<string>(oXml, "prf"),
                XmlDocumentUtilities.GetValue<string>(oXml, "usr"),
                XmlDocumentUtilities.GetValue<string>(oXml, "ctr"),
                XmlDocumentUtilities.GetValue<string>(oXml, "tipo"),
                XmlDocumentUtilities.GetValue<string>(oXml, "obs"));

            var strResult = base.CreateXML(result);
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
            var reader = new XmlTextReader(file)
            {
                WhitespaceHandling = WhitespaceHandling.None
            };
            reader.MoveToContent();

            var oXml = new XmlDocument();
            oXml.Load(reader);

            Array result = consultaNfeService.ConsultaNfe(Login, Senha,
                XmlDocumentUtilities.GetValue<string>(oXml, "prf"),
                XmlDocumentUtilities.GetValue<string>(oXml, "usr"),
                XmlDocumentUtilities.GetValue<string>(oXml, "ctr"),
                XmlDocumentUtilities.GetValue<string>(oXml, "tipo"));

            var strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }
        
        public override void ConsultarXml(string file)
        {
            var reader = new XmlTextReader(file)
            {
                WhitespaceHandling = WhitespaceHandling.None
            };
            reader.MoveToContent();

            var oXml = new XmlDocument();
            oXml.Load(reader);

            Array result = consultaXmlService.ConsultaNfe(Login, Senha,
                XmlDocumentUtilities.GetValue<string>(oXml, "prf"),
                XmlDocumentUtilities.GetValue<string>(oXml, "usr"),
                XmlDocumentUtilities.GetValue<string>(oXml, "ctr"),
                XmlDocumentUtilities.GetValue<string>(oXml, "tipo"));

            var strResult = base.CreateXML(result);
            GerarRetorno(file, strResult, 
                Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSeXML).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedNFSeXML).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        #endregion Métodos
    }
}