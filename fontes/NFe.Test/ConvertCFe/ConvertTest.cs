using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.Xml.Schema;
using NFe.SAT.Conversao;

namespace NFe.Test.ConvertCFe
{
    [TestClass]
    public class ConvertTest
    {
        static string Result = "";
//        static string InputFile = @"C:\projetos\UniNFe\exemplos\NFe - Envio\NFCe\40160506117473000150650010000221801311754299-nfe.xml";
        static string OutputFile = @"C:\projetos\UniNFe\exemplos\NFe - Envio\NFCe\23160506117473000150650010000221801311754299-cfe.xml";
        static string XSD = @"C:\Users\renan\Downloads\XSD_00.07\CfeRecepcao_0007.xsd";

        [TestMethod]
        public void Convert()
        {
//            ConverterNFCe convert = new ConverterNFCe(InputFile, empresa, OutputFile);
//            convert.ConverterSAT();
            ValidarXML();

            if (!string.IsNullOrEmpty(Result))
                throw new Exception(Result);
        }

        private void ValidarXML()
        {
            XmlReaderSettings rdSet = new XmlReaderSettings();
            rdSet.Schemas.Add("", XSD);
            rdSet.ValidationType = ValidationType.Schema;
            rdSet.ValidationEventHandler += new ValidationEventHandler(booksSettingsValidationEventHandler);

            XmlReader xml = XmlReader.Create(OutputFile, rdSet);

            while (xml.Read()) { }
        }

        static void booksSettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                Result += "Advertência: " + e.Message + "\n";
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                Result += "Erro: " + e.Message + "\n";
            }
        }
    }
}
