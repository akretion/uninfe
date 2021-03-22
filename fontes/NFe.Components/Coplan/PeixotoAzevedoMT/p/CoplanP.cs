using NFe.Components.Abstract;
using NFe.Components.PPeixotoAzevedoMT;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace NFe.Components.Coplan.PPeixotoAzevedoMT.p
{
    public class CoplanP : EmiteNFSeBase
    {
        private nfse_web_service Service = new nfse_web_service();
        private input Input = new input();
        private XmlDocument XmlDocument = new XmlDocument();

        public override string NameSpaces
        {
            get
            {
                return "http://www.abrasf.org.br/nfse.xsd";
            }
        }

        #region construtores

        public CoplanP(TipoAmbiente tpAmb, string pastaRetorno, string usuarioProxy, string senhaProxy, string domainProxy, X509Certificate certificado)
            : base(tpAmb, pastaRetorno)
        {
            if (usuarioProxy != string.Empty)
            {
                Service.Proxy = WebRequest.DefaultWebProxy;
                Service.Proxy.Credentials = new NetworkCredential(usuarioProxy, senhaProxy);
                Service.Credentials = new NetworkCredential(senhaProxy, senhaProxy);
            }

            Service.ClientCertificates.Add(certificado);

            ServicePointManager.Expect100Continue = false;

            Input.nfseCabecMsg = "<cabecalho xmlns=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.01\"><versaoDados>2.01</versaoDados></cabecalho>";
        }

        #endregion construtores

        #region Métodos

        public override void EmiteNF(string file)
        {
            XmlDocument.Load(file);
            Input.nfseDadosMsg = XmlDocument.InnerXml;

            string result = string.Empty;

            switch (XmlDocument.DocumentElement.Name)
            {
                case "EnviarLoteRpsEnvio":
                    result = Service.RECEPCIONARLOTERPS(Input)?.outputXML;
                    break;

                case "EnviarLoteRpsSincronoEnvio":
                    result = Service.RECEPCIONARLOTERPSSINCRONO(Input)?.outputXML;
                    break;
            }

            XmlDocument retornoXML = new XmlDocument();
            retornoXML.Load(Functions.StringXmlToStreamUTF8(result.Trim()));
            result = retornoXML.OuterXml;

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            XmlDocument.Load(file);
            Input.nfseDadosMsg = XmlDocument.InnerXml;

            string result = Service.CANCELARNFSE(Input)?.outputXML;
            XmlDocument retornoXML = new XmlDocument();
            retornoXML.Load(Functions.StringXmlToStreamUTF8(result.Trim()));
            result = retornoXML.OuterXml;

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        {
            XmlDocument.Load(file);
            Input.nfseDadosMsg = XmlDocument.InnerXml;

            string result = Service.CONSULTARLOTERPS(Input)?.outputXML;
            XmlDocument retornoXML = new XmlDocument();
            retornoXML.Load(Functions.StringXmlToStreamUTF8(result.Trim()));
            result = retornoXML.OuterXml;

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            throw new System.Exception("Padrão COPLAN não disponibiliza a consulta situação do lote por RPS.");
        }

        public override void ConsultarNfse(string file)
        {
            XmlDocument.Load(file);
            Input.nfseDadosMsg = XmlDocument.InnerXml;

            string result = Service.CONSULTARNFSEFAIXA(Input).outputXML;
            XmlDocument retornoXML = new XmlDocument();
            retornoXML.Load(Functions.StringXmlToStreamUTF8(result.Trim()));
            result = retornoXML.OuterXml;

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            XmlDocument.Load(file);
            Input.nfseDadosMsg = XmlDocument.InnerXml;

            string result = Service.CONSULTARNFSEPORRPS(Input)?.outputXML;
            XmlDocument retornoXML = new XmlDocument();
            retornoXML.Load(Functions.StringXmlToStreamUTF8(result.Trim()));
            result = retornoXML.OuterXml;

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML);
        }

        public override void SubstituirNfse(string file)
        {
            XmlDocument.Load(file);
            Input.nfseDadosMsg = XmlDocument.InnerXml;

            string result = Service.SUBSTITUIRNFSE(Input)?.outputXML;
            XmlDocument retornoXML = new XmlDocument();
            retornoXML.Load(Functions.StringXmlToStreamUTF8(result.Trim()));
            result = retornoXML.OuterXml;

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML);
        }

        #endregion Métodos
    }
}