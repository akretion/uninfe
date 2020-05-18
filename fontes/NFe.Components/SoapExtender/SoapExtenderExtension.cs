using System;
using System.IO;
using System.Text;
using System.Web.Services.Protocols;

namespace NFe.Components.SoapExtender
{
    internal class SoapExtenderExtension: SoapExtension
    {
        #region Private Fields

        private Stream newStream;
        private Stream oldStream;
        private string oldXml = "";

        #endregion Private Fields

        #region Private Methods

        private MemoryStream ConvertStringToStream(string requestString)
        {
            var byteArray = Encoding.UTF8.GetBytes(requestString);
            var stream = new MemoryStream(byteArray)
            {
                Position = 0
            };

            return stream;
        }

        private void Copy(Stream from, Stream to)
        {
            TextReader reader = new StreamReader(from);
            TextWriter writer = new StreamWriter(to);
            writer.WriteLine(reader.ReadToEnd());
            writer.Flush();
        }

        private string RecreateSoapEnvelope()
        {
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.Append("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body>");
            sb.Append(oldXml);
            sb.Append("</soap:Body></soap:Envelope>");
            return sb.ToString();
        }

        private void WriteXml(SoapMessage message)
        {
            oldXml = ((System.Xml.XmlDocument)message.GetInParameterValue(0)).InnerXml;
            oldXml = oldXml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
        }

        #endregion Private Methods

        #region Public Methods

        public override Stream ChainStream(Stream stream)
        {
            oldStream = stream;
            newStream = new MemoryStream();
            return newStream;
        }

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute) => null;

        public override object GetInitializer(Type WebServiceType) => null;

        public override void Initialize(object initializer)
        {
        }

        public override void ProcessMessage(SoapMessage message)
        {
            switch(message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                    WriteXml(message);
                    break;

                case SoapMessageStage.AfterSerialize:
                    WriteOutput();
                    break;

                case SoapMessageStage.BeforeDeserialize:
                    WriteInput();
                    break;

                case SoapMessageStage.AfterDeserialize:
                default:
                    break;
            }
        }

        public void WriteInput()
        {
            Copy(oldStream, newStream);
            newStream.Position = 0;
        }

        public void WriteOutput()
        {
            newStream.Position = 0;
            Copy(ConvertStringToStream(RecreateSoapEnvelope()), oldStream);
        }

        #endregion Public Methods
    }
}