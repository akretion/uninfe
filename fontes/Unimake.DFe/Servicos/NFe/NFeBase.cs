using System;
using System.Xml;

namespace Unimake.DFe.Servicos.NFe
{
    public abstract class NFeBase : ServicoBase
    {
        private const string ArquivoConfig = @"Servicos\Nfe\Config\Config.xml";

        public NFeBase(XmlDocument conteudoXML, Configuracao configuracao) :
            base(conteudoXML, configuracao)
        {
            if (!configuracao.Definida)
            {
                DefinirConfiguracao(configuracao);
            }
        }

        private void DefinirConfiguracao(Configuracao configuracao)
        {
            if (configuracao.cUF.Equals(0))
            {
                if (ConteudoXML.GetElementsByTagName("cUF")[0] != null)
                {
                    configuracao.cUF = Convert.ToInt32(ConteudoXML.GetElementsByTagName("cUF")[0].InnerText);
                }
            }

            if (configuracao.tpAmb.Equals(0))
            {
                if (ConteudoXML.GetElementsByTagName("tpAmb")[0] != null)
                {
                    configuracao.tpAmb = Convert.ToInt32(ConteudoXML.GetElementsByTagName("tpAmb")[0].InnerText);
                }
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(ArquivoConfig);

            XmlNodeList listConfiguracoes = doc.GetElementsByTagName("Configuracoes");

            foreach (object nodeConfiguracoes in listConfiguracoes)
            {
                XmlElement elementConfiguracoes = (XmlElement)nodeConfiguracoes;
                if (elementConfiguracoes.GetAttribute("ID") == configuracao.cUF.ToString())
                {
                    XmlDocument docConfig = new XmlDocument();
                    docConfig.Load(elementConfiguracoes.GetElementsByTagName("Arquivo")[0].InnerText);


                }
            }
        }
    }
}
