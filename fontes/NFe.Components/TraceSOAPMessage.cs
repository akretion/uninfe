//Adicionado a extensão para capturar as mensagens do SOAP enquanto debug
//https://msdn.microsoft.com/pt-br/library/system.web.services.protocols.soapextension(v=vs.110).aspx

//#if DEBUG
using System;
using System.Diagnostics;
using System.IO;
using System.Web.Services.Protocols;

namespace NFe.Components
{
    public class TraceSOAPMessage : SoapExtension
    {
        #region Private Fields

        private Stream _originalStream;
        private Stream _workingStream;

        #endregion Private Fields

        #region Private Methods

        private void Copy(Stream from, Stream to)
        {
            TextReader reader = new StreamReader(from);
            TextWriter writer = new StreamWriter(to);
            writer.Write(reader.ReadToEnd());
            writer.Flush();
        }

        private void LogMessageFromStream(Stream stream, string stage)
        {
            string soapMessage = string.Empty;

            if (stream.CanRead && stream.CanSeek)
            {
                stream.Position = 0;

                StreamReader rdr = new StreamReader(stream);
                soapMessage = rdr.ReadToEnd();
                stream.Position = 0;
            }

            Trace.WriteLine($"\r\n{"".PadLeft(40, '=')} START SOAP MESSAGE {DateTime.Now} - {"".PadLeft(40, '=')}\r\n");
            Trace.WriteLine($"STAGE: {stage}");
            Trace.WriteLine("SOAP MESSAGE:\r\n");
            Trace.WriteLine(soapMessage);
            Trace.WriteLine($"\r\n{"".PadLeft(40, '=')} END  SOAP  MESSAGE {DateTime.Now} - {"".PadLeft(40, '=')}\r\n");
        }

        #endregion Private Methods

        #region Public Methods

        public override Stream ChainStream(Stream stream)
        {
            _originalStream = stream;
            _workingStream = new MemoryStream();
            return _workingStream;
        }

        public override object GetInitializer(Type serviceType)
        {
            return null;
        }

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null;
        }

        public override void Initialize(object initializer)
        {
        }

        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeDeserialize:
                    Copy(_originalStream, _workingStream);
                    LogMessageFromStream(_workingStream, message.Stage.ToString());
                    break;

                //case SoapMessageStage.AfterDeserialize:
                //    break;

                //case SoapMessageStage.BeforeSerialize:
                //    break;

                case SoapMessageStage.AfterSerialize:
                    LogMessageFromStream(_workingStream, message.Stage.ToString());
                    Copy(_workingStream, _originalStream);
                    break;
            }
        }

        #endregion Public Methods
    }
}
//#endif