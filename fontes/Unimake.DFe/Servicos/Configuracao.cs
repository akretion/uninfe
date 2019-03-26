using System.Security.Cryptography.X509Certificates;

namespace Unimake.DFe.Servicos
{
    public class Configuracao
    {
        /// <summary>
        /// Serviço que será executado
        /// </summary>
        public Servicos Servico { get; set; }

        /// <summary>
        /// Nome do estado ou município
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Ambiente (2-Homologação ou 1-Produção)
        /// </summary>
        public int tpAmb { get; set; }

        /// <summary>
        /// Código da Unidade Federativa (UF)
        /// </summary>
        public int cUF { get; set; }

        /// <summary>
        /// Tipo de Emissao (1-Normal, 2-Contingencia, 6/7/8-SVC/AN/RS/SP, ...
        /// </summary>
        public int tpEmis { get; set; }

        /// <summary>
        /// Versão do XML
        /// </summary>
        public string versao { get; set; }

        /// <summary>
        /// Modelo do documento fiscal que é para consultar o status do serviço
        /// </summary>
        public string mod { get; set; }

        /// <summary>
        /// Nome do arquivo de schema para validação do XML
        /// </summary>
        public string ArquivoXSD { get; set; }

        /// <summary>
        /// Namespace do XML para validação de schema
        /// </summary>
        public string TargetNS { get; set; }

        /// <summary>
        /// Certificado digital
        /// </summary>
        public X509Certificate2 CertificadoDigital;

        /// <summary>
        /// Endereço WebService do ambiente de homologação
        /// </summary>
        public string WebEnderecoHomologacao { get; set; }

        /// <summary>
        /// Endereço WebService do ambiente de produção
        /// </summary>
        public string WebEnderecoProducao { get; set; }
        
        /// <summary>
        /// Ação, do webservice, a ser executada no ambiente de homologação
        /// </summary>
        public string WebActionHomologacao { get; set; }

        /// <summary>
        /// Ação, do webservice, a ser executada no ambiente de produção
        /// </summary>
        public string WebActionProducao { get; set; }

        /// <summary>
        /// Nome da tag de retorno do serviço
        /// </summary>
        public string WebTagRetorno  { get; set; }

        /// <summary>
        /// Descrição do serviço
        /// </summary>
        public string Descricao  { get; set; }

        /// <summary>
        /// Configuração já foi definida anteriormente, não precisa carregar de acordo com os dados do XML
        /// </summary>
        public bool Definida { get; set; }
    }
}
