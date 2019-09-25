﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Unimake.Business.DFe.Servicos
{
    public class Configuracao
    {
        /// <summary>
        /// Serviço que será executado
        /// </summary>
        public Servico Servico { get; set; }

        /// <summary>
        /// Nome do estado ou município
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Ambiente (2-Homologação ou 1-Produção)
        /// </summary>
        public TipoAmbiente TipoAmbiente { get; set; }

        /// <summary>
        /// Código da Unidade Federativa (UF)
        /// </summary>
        public int CodigoUF { get; set; }

        /// <summary>
        /// Nome da Unidade Federativa (UF)
        /// </summary>
        public string NomeUF { get; set; }

        /// <summary>
        /// Tipo de Emissao (1-Normal, 2-Contingencia, 6/7/8-SVC/AN/RS/SP, ...
        /// </summary>
        public TipoEmissao TipoEmissao { get; set; }

        /// <summary>
        /// Modelo do documento fiscal que é para consultar o status do serviço
        /// </summary>
        public ModeloDFe Modelo { get; set; }

        /// <summary>
        /// Tipo do Documento Fiscal Eletrônico (DF-e)
        /// </summary>
        public DFE TipoDFe { get; set; }

        /// <summary>
        /// Pasta do arquivo de schema de validação do XML
        /// </summary>
        public string SchemaPasta { get; set; }

        /// <summary>
        /// Nome do arquivo de schema para validação do XML
        /// </summary>
        public string SchemaArquivo { get; set; }

        /// <summary>
        /// Schemas específicos de um mesmo serviço (Tipos de Evento, Modal CTe ou Modal MDFe)
        /// </summary>
        public Dictionary<string, SchemaEspecifico> SchemasEspecificos = new Dictionary<string, SchemaEspecifico>();

        /// <summary>
        /// Versão do schema do XML
        /// </summary>
        public string SchemaVersao { get; set; }

        /// <summary>
        /// Namespace do XML para validação de schema
        /// </summary>
        public string TargetNS { get; set; }

        /// <summary>
        /// Nome da tag de Assinatura do XML
        /// </summary>
        public string TagAssinatura { get; set; }

        /// <summary>
        /// Nome da tag que tem o atributo de identificador único a ser utilizado no Reference.URI da assinatura
        /// </summary>
        public string TagAtributoID { get; set; }

        /// <summary>
        /// Nome da tag de Assinatura do XML, quando tem lote (Exemplo: Uma lote com várias NFe ou NFSe)
        /// </summary>
        public string TagLoteAssinatura { get; set; }

        /// <summary>
        /// Nome da tag que tem o atributo de identificador único a ser utilizado no Reference.URI da assinatura, quando tem lote (Exemplo: Uma lote com várias NFe ou NFSe)
        /// </summary>
        public string TagLoteAtributoID { get; set; }

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
        public string WebTagRetorno { get; set; }

        /// <summary>
        /// Descrição do serviço
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Configuração já foi definida anteriormente, não precisa carregar de acordo com os dados do XML
        /// </summary>
        public bool Definida { get; set; }

        /// <summary>
        /// Versão do SOAP utilizada pelo webservice
        /// </summary>
        public string WebSoapVersion { get; set; }

        /// <summary>
        /// ContentType para conexão via classe HttpWebRequest
        /// </summary>
        /// <example>
        /// Exemplos de conteúdo:
        /// 
        ///    application/soap+xml; charset=utf-8;
        ///    text/xml; charset=utf-8;
        ///    
        /// Deixe o conteúdo em brando para utilizar um valor padrão. 
        /// </example>
        public string WebContentType { get; set; }

        /// <summary>
        /// String do Soap para envio para o webservice;
        /// </summary>
        /// <example>
        /// Exemplo de conteúdo que deve ser inserido nesta propriedade:
        /// 
        ///    <![CDATA[<soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><soap:Header>{xmlHeader}</soap:Header><soap:Body><nfeDadosMsg xmlns="{ActionWeb}">{xmlBody}</nfeDadosMsg></soap:Body></soap:Envelope>]]>
        /// 
        ///    Onde estiver {xmlHeader} o conteúdo será substituido pelo XML do header em tempo de execução
        ///    Onde estiver {ActionWeb} o conteúdo será substituido pelo WebAction em tempo de execução
        ///    Onde estiver {xmlBody} o conteúdo será substituido pelo XML do Body em tempo de execução
        /// 
        ///    Deixe o conteúdo em branco para utilizar um soap padrão.
        /// </example>
        public string WebSoapString { get; set; }
    }

    /// <summary>
    /// Arquivos de schema específicos. Um mesmo serviço mais com vários arquivos de schema para validação, varia de acordo com uma determinada informação de tag, exemplo: CTe tem o Modal, Evento tem o tipo de evento, MDFe tem o modal, etc...
    /// </summary>
    public class SchemaEspecifico
    {
        /// <summary>
        /// ID da parte específica do XML. Pode ser o TipoEvento para eventos ou o Modal para CTe e MDFe.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Arquivo de schema principal
        /// </summary>
        public string SchemaArquivo { get; set; }
        /// <summary>
        /// Arquivo de schema da parte específica do XML
        /// </summary>
        public string SchemaArquivoEspecifico { get; set; }
    }
}
