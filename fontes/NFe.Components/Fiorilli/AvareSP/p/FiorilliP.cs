using System;
using NFe.Components.Abstract;
using NFe.Components.br.com.fiorilli.avaresp.p;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using System.IO;

namespace NFe.Components.Fiorilli.AvareSP.p
{
    public class FiorilliP : EmiteNFSeBase
    {
        IssWebWS service = new IssWebWS();
        string UsuarioWs = "";
        string SenhaWs = "";

        #region construtores
        public FiorilliP(TipoAmbiente tpAmb, string pastaRetorno, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver, X509Certificate2 certificado)
            : base(tpAmb, pastaRetorno)
        {
            ServicePointManager.Expect100Continue = false;
            UsuarioWs = usuario;
            SenhaWs = senhaWs;

            service.ClientCertificates.Add(certificado);

            if (!String.IsNullOrEmpty(proxyuser))
            {
                NetworkCredential credentials = new NetworkCredential(proxyuser, proxypass, proxyserver);
                WebRequest.DefaultWebProxy.Credentials = credentials;

                service.Proxy = WebRequest.DefaultWebProxy;
                service.Proxy.Credentials = new NetworkCredential(proxyuser, proxypass);
                service.Credentials = new NetworkCredential(proxyuser, proxypass);
            }
        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            GerarNfseEnvio envio = new GerarNfseEnvio();
            GerarNfseResposta resposta = new GerarNfseResposta();

            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "GerarNfseEnvio";
            xRoot.Namespace = "http://www.abrasf.org.br/nfse.xsd";

            XmlSerializer serializer = new XmlSerializer(typeof(GerarNfseEnvio), xRoot);
            StreamReader reader = new StreamReader(file);
            envio = (GerarNfseEnvio)serializer.Deserialize(reader);
            reader.Close();

            resposta = service.gerarNfse(envio, UsuarioWs, SenhaWs);

            XmlSerializer serializerResposta = new XmlSerializer(typeof(GerarNfseResposta));
            StringWriter textWriter = new StringWriter();
            serializerResposta.Serialize(textWriter, resposta);
            string strResult = textWriter.ToString();

            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            CancelarNfseEnvio envio = new CancelarNfseEnvio();
            CancelarNfseResposta resposta = new CancelarNfseResposta();

            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "CancelarNfseEnvio";
            xRoot.Namespace = "http://www.abrasf.org.br/nfse.xsd";

            XmlSerializer serializer = new XmlSerializer(typeof(CancelarNfseEnvio), xRoot);
            StreamReader reader = new StreamReader(file);
            envio = (CancelarNfseEnvio)serializer.Deserialize(reader);
            reader.Close();

            resposta = service.cancelarNfse(envio, UsuarioWs, SenhaWs);

            XmlSerializer serializerResposta = new XmlSerializer(typeof(CancelarNfseResposta));
            StringWriter textWriter = new StringWriter();
            serializerResposta.Serialize(textWriter, resposta);
            string strResult = textWriter.ToString();

            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        {
            ConsultarLoteRpsEnvio envio = new ConsultarLoteRpsEnvio();
            ConsultarLoteRpsResposta resposta = new ConsultarLoteRpsResposta();

            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "ConsultarLoteRpsEnvio";
            xRoot.Namespace = "http://www.abrasf.org.br/nfse.xsd";

            XmlSerializer serializer = new XmlSerializer(typeof(ConsultarLoteRpsEnvio), xRoot);
            StreamReader reader = new StreamReader(file);
            envio = (ConsultarLoteRpsEnvio)serializer.Deserialize(reader);
            reader.Close();

            resposta = service.consultarLoteRps(envio, UsuarioWs, SenhaWs);

            XmlSerializer serializerResposta = new XmlSerializer(typeof(ConsultarLoteRpsResposta));
            StringWriter textWriter = new StringWriter();
            serializerResposta.Serialize(textWriter, resposta);
            string strResult = textWriter.ToString();

            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        public override void ConsultarNfse(string file)
        {
            ConsultarNfseServicoPrestadoEnvio envio = new ConsultarNfseServicoPrestadoEnvio();
            ConsultarNfseServicoPrestadoResposta resposta = new ConsultarNfseServicoPrestadoResposta();

            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "ConsultarNfseServicoPrestadoEnvio";
            xRoot.Namespace = "http://www.abrasf.org.br/nfse.xsd";

            XmlSerializer serializer = new XmlSerializer(typeof(ConsultarNfseServicoPrestadoEnvio), xRoot);
            StreamReader reader = new StreamReader(file);
            envio = (ConsultarNfseServicoPrestadoEnvio)serializer.Deserialize(reader);
            reader.Close();

            resposta = service.consultarNfseServicoPrestado(envio, UsuarioWs, SenhaWs);

            XmlSerializer serializerResposta = new XmlSerializer(typeof(ConsultarNfseServicoPrestadoResposta));
            StringWriter textWriter = new StringWriter();
            serializerResposta.Serialize(textWriter, resposta);
            string strResult = textWriter.ToString();

            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            ConsultarNfseRpsEnvio envio = new ConsultarNfseRpsEnvio();
            ConsultarNfseRpsResposta resposta = new ConsultarNfseRpsResposta();

            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "ConsultarNfseRpsEnvio";
            xRoot.Namespace = "http://www.abrasf.org.br/nfse.xsd";

            XmlSerializer serializer = new XmlSerializer(typeof(ConsultarNfseRpsEnvio), xRoot);
            StreamReader reader = new StreamReader(file);
            envio = (ConsultarNfseRpsEnvio)serializer.Deserialize(reader);
            reader.Close();

            resposta = service.consultarNfsePorRps(envio, UsuarioWs, SenhaWs);

            XmlSerializer serializerResposta = new XmlSerializer(typeof(ConsultarNfseRpsResposta));
            StringWriter textWriter = new StringWriter();
            serializerResposta.Serialize(textWriter, resposta);
            string strResult = textWriter.ToString();

            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML,
                                          Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML);
        }
        #endregion
    }
}
