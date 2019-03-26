using System.Xml;

namespace Unimake.DFe.Servicos.NFe
{
    public class StatusServico : NFeBase
    {
        public StatusServico(XmlDocument conteudoXML, Configuracao configuracao)
            : base(conteudoXML, configuracao)
        {
        }

        public override void Executar()
        {
            WSSoap soap = new WSSoap
            {
                EnderecoWeb = "https://homologacao.nfe.fazenda.sp.gov.br/ws/nfestatusservico4.asmx",
                ActionWeb = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeStatusServico4",
                TagRetorno = "nfeResultMsg",
                VersaoSoap = "soap12"
            };

            ConsumirWS consumirWS = new ConsumirWS();
            consumirWS.ExecutarServico(ConteudoXML, soap, Configuracoes.CertificadoDigital);

            RetornoWSString = consumirWS.RetornoServicoString;
            RetornoWSXML = consumirWS.RetornoServicoXML;

        }
    }
}
