using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.IO;

namespace uninfe
{
    /// <summary>
    /// Classe possui métodos que retoram informações sobre o UniNFe
    /// </summary>
    class UniNfeInfClass
    {
        /// <summary>
        /// Retorna a versão do aplicativo UniNFe
        /// </summary>
        /// <returns>string contendo a versão do aplicativo UniNfe</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>29/01/2009</date>
        public string Versao()
        {
            //Montar a versão do programa
            Assembly objAssembly = Assembly.GetExecutingAssembly();

            string versao;

            foreach (Attribute attr in Attribute.GetCustomAttributes(objAssembly))
            {
                if (attr.GetType() == typeof(AssemblyVersionAttribute))
                {
                    versao = ((AssemblyVersionAttribute)attr).Version;
                }
            }
            string delimStr = ",=";
            char[] delimiter = delimStr.ToCharArray();
            string[] strAssembly = objAssembly.ToString().Split(delimiter);
            versao = strAssembly[2];

            return versao;
        }

        /// <summary>
        /// Grava XML com algumas informações do aplicativo UniNFe, 
        /// dentre elas os dados do certificado digital configurado nos parâmetros, 
        /// versão, última modificação, etc.
        /// </summary>
        /// <param name="sArquivo">Pasta e nome do arquivo XML a ser gravado com as informações</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>29/01/2009</date>
        public void GravarXMLInformacoes(string sArquivo)
        {
            string cStat = "1";
            string xMotivo = "Consulta efetuada com sucesso";

            //Ler os dados do certificado digital
            string sSubject = "";
            string sValIni = "";
            string sValFin = "";

            CertificadoDigitalClass oDigCert = new CertificadoDigitalClass();
            ConfigUniNFe oConfig = new ConfigUniNFe();
            oConfig.CarregarDados();

            oDigCert.PrepInfCertificado( oConfig.oCertificado );

            if (oDigCert.lLocalizouCertificado == true)
            {
                sSubject = oDigCert.sSubject;
                sValIni = oDigCert.dValidadeInicial.ToString();
                sValFin = oDigCert.dValidadeFinal.ToString();
            }
            else
            {
                cStat = "2";
                xMotivo = "Certificado digital não foi localizado";
            }

            //Gravar o XML com as informações do UniNFe
            try
            {
                XmlWriterSettings oSettings = new XmlWriterSettings();

                //Para começar, vamos criar um XmlWriterSettings para configurar nosso XML
                oSettings.Indent = true;
                oSettings.IndentChars = "";
                oSettings.NewLineOnAttributes = false;
                oSettings.OmitXmlDeclaration = false;

                //Agora vamos criar um XML Writer
                XmlWriter oXmlGravar = XmlWriter.Create(sArquivo, oSettings);

                //Abrir o XML
                oXmlGravar.WriteStartDocument();
                oXmlGravar.WriteStartElement("retConsInf");
                oXmlGravar.WriteElementString("cStat", cStat);
                oXmlGravar.WriteElementString("xMotivo", xMotivo);

                //Dados do certificado digital
                oXmlGravar.WriteStartElement("DadosCertificado");
                oXmlGravar.WriteElementString("sSubject", sSubject);
                oXmlGravar.WriteElementString("dValIni", sValIni);
                oXmlGravar.WriteElementString("dValFin", sValFin);
                oXmlGravar.WriteEndElement(); //DadosCertificado

                //Dados gerais do UniNFe
                oXmlGravar.WriteStartElement("DadosUniNfe");
                oXmlGravar.WriteElementString("versao", this.Versao());
                oXmlGravar.WriteElementString("dUltModif", File.GetLastWriteTimeUtc("uninfe.exe").ToString("dd/MM/yyyy hh:mm:ss"));
                oXmlGravar.WriteEndElement(); //DadosUniNfe

                //Finalizar o XML
                oXmlGravar.WriteEndElement(); //retConsInf
                oXmlGravar.WriteEndDocument();
                oXmlGravar.Flush();
                oXmlGravar.Close();
            }
            catch (Exception ex)
            {
                //TODO: Não conseguiu gravar o XML tem que tratar este erro e devolver algo para o ERP talvez um .ERR
            }
        }
    }
}
