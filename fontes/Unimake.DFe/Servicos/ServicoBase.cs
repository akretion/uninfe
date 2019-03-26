using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Unimake.DFe.Servicos
{
    public abstract class ServicoBase
    {
        protected XmlDocument ConteudoXML;
        protected Configuracao Configuracoes;
        public string RetornoWSString;
        public XmlDocument RetornoWSXML;

        public ServicoBase(XmlDocument conteudoXML, Configuracao configuracao)
        {
            ConteudoXML = conteudoXML;
            Configuracoes = configuracao;
        }

        public abstract void Executar();
    }
}
