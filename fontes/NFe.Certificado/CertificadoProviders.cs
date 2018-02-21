using NFe.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace NFe.Certificado
{
    public class CertificadoProviders
    {
        #region Propriedades
        /// <summary>
        /// Propriedade que identifica se foi possível identificar um provider valido
        /// </summary>
        public bool ProviderIdentificado { get; private set; }
        /// <summary>
        /// Dados referente ao provider valido
        /// </summary>
        public CertProviders ProviderValido { get; private set; }
        /// <summary>
        /// Propriedade referente ao certificado que será testado
        /// </summary>
        public bool IsA3 { get; private set; }

        private CertificadoDigital oCertificado = new CertificadoDigital();
        private X509Certificate2 Certificado { get; set; }
        private List<CertProviders> ProvidersIdentificados { get; set; }
        private string TempFile { get; set; }
        private int CodEmp { get; set; }
        private string PIN { get; set; }
        #endregion

        #region Construtores
        public CertificadoProviders(X509Certificate2 certificado,
                                    string folderTemp,
                                    int codEmp,
                                    string pin)
        {
            Certificado = certificado;
            TempFile = folderTemp + "\\Temp\\Simulacao.xml";
            CodEmp = codEmp;
            PIN = pin;
            IsA3 = Certificado.IsA3();
            ProviderIdentificado = false;

            if (!Directory.Exists(folderTemp + "\\temp"))
                Directory.CreateDirectory(Path.GetFullPath(folderTemp + "\\temp"));
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Define a propriedade com o provider que deverá ser sugerida ao usuario
        /// </summary>
        private void GetProviderValido()
        {
            for (int i = 0; i < ProvidersIdentificados.Count; i++)
            {
                if (TestarProvider(ProvidersIdentificados[i]))
                {
                    ProviderValido = ProvidersIdentificados[i];
                    ProviderIdentificado = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Realiza efetivamente o teste assinando o XML de simualação
        /// </summary>
        /// <param name="provider">Objeto com as informaçoes do provider que será testado</param>
        /// <returns></returns>
        public bool TestarProvider(CertProviders provider)
        {
            bool result = false;

            this.SimularXML(provider);
            AssinaturaDigital assinatura = new AssinaturaDigital();
            assinatura.TesteCertificado = true;
            result = assinatura.TestarProviderCertificado(TempFile,
                "SimulacaoProvider",
                "Provider",
                Certificado,
                CodEmp,
                PIN);

            ApagarXMLTeste();

            return result;
        }

        /// <summary>
        /// Apagar arquivo XML de teste para assinatura
        /// </summary>
        private void ApagarXMLTeste()
        {
            if (File.Exists(TempFile))
                File.Delete(TempFile);
        }

        /// <summary>
        /// Gera um XML para realizar a simulação do teste da assinatura
        /// </summary>
        /// <param name="provider">Objeto CertProviders que contem o provider que será testado</param>
        /// <author>Renan Borges</author>
        private void SimularXML(CertProviders provider)
        {
            Empresas.CriarPasta(true);

            ApagarXMLTeste();

            XElement docElement = new XElement("SimulacaoProvider");
            XElement eProvider = new XElement("Provider");

            eProvider.Add(new XElement("Nome", provider.NameKey));
            eProvider.Add(new XElement("Type", provider.Type));

            docElement.Add(eProvider);

            docElement.Save(TempFile);
        }

        /// <summary>
        /// Busca uma lista de providers que podem ser utilizados pelo certificado
        /// </summary>
        /// <author>Renan Borges</author>
        public void GetProviders()
        {
            ProvidersIdentificados = oCertificado.GetListProviders();
        }

        /// <summary>
        /// Busca os types dos providers encontrados
        /// </summary>
        /// <author>Renan Borges</author>
        public void GetProvidersType()
        {
            for (int i = 0; i < ProvidersIdentificados.Count; i++)
            {
                ProvidersIdentificados[i].Type = oCertificado.GetInfoProvider(ProvidersIdentificados[i].NameKey.ToString()).Type;
            }
        }
        #endregion
    }
}
