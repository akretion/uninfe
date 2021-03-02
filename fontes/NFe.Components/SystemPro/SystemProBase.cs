using NFe.Components.Abstract;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.SystemPro
{
    public abstract class SystemProBase: EmiteNFSeBase
    {
        #region locais/ protegidos

        private int CodMun = 0;
        private object systemProService;
        protected object SystemProService
        {
            get
            {
                if(systemProService == null)
                {
                    if(tpAmb == TipoAmbiente.taHomologacao)
                    {
                        switch(CodMun)
                        {
                            case 4307005:
                                systemProService = new br.gov.rs.erechim.nfse.www.h.NfseService_Homolog();
                                break;

                            case 4301701:
                                systemProService = new HBaraoDeCotegipeRS.NfseService_Homolog();
                                break;

                            case 4307559:
                                systemProService = new HEstacaoRS.NfseService_Homolog();
                                break;

                            case 4303806:
                                systemProService = new HCampinasDoSul.NfseService_Homolog();
                                break;
                        }
                    }
                    else
                    {
                        switch(CodMun)
                        {
                            case 4307005:
                                systemProService = new br.gov.rs.erechim.nfse.www.p.NfseService();
                                break;

                            case 4301701:
                                systemProService = new PBaraoDeCotegipeRS.NfseService();
                                break;

                            case 4307559:
                                systemProService = new PEstacaoRS.NfseService();
                                break;

                            case 4303806:
                                systemProService = new PCampinasDoSul.NfseService();
                                break;
                        }
                    }

                    AddClientCertificates();
                }

                return systemProService;
            }
        }

        private void AddClientCertificates()
        {
            X509CertificateCollection certificates = null;
            var t = systemProService.GetType();
            var pi = t.GetProperty("ClientCertificates");
            certificates = pi.GetValue(systemProService, null) as X509CertificateCollection;
            certificates.Add(Certificate);
        }

        #endregion locais/ protegidos

        #region propriedades

        public X509Certificate Certificate { get; private set; }

        private string NfseCabecMsg = "<cabecalho versao=\"2.01\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"<versaoDados>2.01</versaoDados></cabecalho>";
        #endregion propriedades

        #region Construtores

        public SystemProBase(TipoAmbiente tpAmb, string pastaRetorno, X509Certificate certificate, int codMun)
            : base(tpAmb, pastaRetorno)
        {
            Certificate = certificate;
            CodMun = codMun;
        }

        #endregion Construtores

        #region Métodos

        public override void EmiteNF(string file)
        {
            var strResult = Invoke("EnviarLoteRpsSincrono", new[] { NfseCabecMsg, ReaderXML(file) });
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            var strResult = Invoke("CancelarNfse", new[] { NfseCabecMsg, ReaderXML(file) });
            GerarRetorno(file, strResult,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public void SubstituirNfse(string file)
        {
            var strResult = Invoke("SubstituirNfse", new[] { NfseCabecMsg, ReaderXML(file) });
            GerarRetorno(file, strResult,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }


        public override void ConsultarLoteRps(string file) => throw new Exceptions.ServicoInexistenteException();

        public override void ConsultarSituacaoLoteRps(string file) => throw new Exceptions.ServicoInexistenteException();

        public override void ConsultarNfse(string file)
        {
            var strResult = Invoke("ConsultarNfseFaixa", new[] { NfseCabecMsg, ReaderXML(file) });
            GerarRetorno(file, strResult, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                                            Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file) => throw new Exceptions.ServicoInexistenteException();

        #endregion Métodos

        #region invoke

        private string Invoke(string methodName, params object[] _params)
        {
            object result = "";
            var t = SystemProService.GetType();
            var mi = t.GetMethod(methodName);
            result = mi.Invoke(SystemProService, _params);
            return result.ToString();
        }

        #endregion invoke

        #region ReaderXML

        private string ReaderXML(string file)
        {
            var result = "";

            using(var reader = new StreamReader(file))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    result += line;
                }
            }
            return result;
        }

        #endregion ReaderXML
    }
}