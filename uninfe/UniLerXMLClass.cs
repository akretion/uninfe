//------------------------------------------------------------------------------ 
// <copyright file="UniLerXMLClass.cs" company="Unimake"> 
// 
// Copyright (c) 2008 Unimake Softwares. All rights reserved.
//
// Programador: Wandrey Mundin Ferreira
// 
// </copyright> 
//------------------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace uninfe
{
    /// <summary>
    /// Classe responsável por ler os diversos XML´s utilizados na nota fiscal eletrônica
    /// e dispor as informações em propriedades para facilitar a leitura.
    /// </summary>
    class UniLerXMLClass
    {
        /// <summary>
        /// Esta classe possui as propriedades que vai receber o conteúdo
        /// do XML da nota fiscal eletrônica
        /// </summary>
        public class DadosNFeClass
        {
            public string chavenfe { get; set; } //Chave da nota fiscal
            public DateTime dEmi { get; set; } //Data de emissão
            public string tpEmis { get; set; } //Tipo de emissão 1-Normal 2-Contigência em papel de segurança 3-Contigência SCAN
            public string tpAmb { get; set; } //Tipo de Ambiente 1-Produção 2-Homologação
        }

        //Esta herança que deve ser utilizada fora da classe para obter o valores
        //das tag´s da nota fiscal eletrônica
        public DadosNFeClass oDadosNfe = new DadosNFeClass();

        /// <summary>
        /// Recebe uma mensagem de erro, caso ocorra alguma falha na leitura do XML
        /// </summary>
        public string cMensagemErro { get; private set; }

        /// <summary>
        /// Faz a leitura do XML da nota fiscal eletrônica e disponibiliza os valores
        /// de algumas tag´s
        /// </summary>
        /// <param name="cArquivoXML">Caminho e nome do arquivo XML da NFe a ser lido</param>
        /// <returns>Retorna se conseguiu ler ou não o conteúdo do XML (true or false)</returns>
        /// <example>
        /// UniLerXmlClass oLerXml = new UniLerXmlClass();
        /// oLerXml.Nfe( cPasta_Nome_ArquivoXML );
        /// DateTime dEmi = oLerXml.Nfe.oDadosNfe.dEmi;
        /// </example>
        public bool Nfe(string cArquivoXML)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                XmlNodeList infNFeList = doc.GetElementsByTagName("infNFe");

                foreach (XmlNode infNFeNode in infNFeList)
                {
                    XmlElement infNFeElemento = (XmlElement)infNFeNode;

                    //Pegar a chave da NF-e
                    if (infNFeElemento.HasAttributes)
                    {
                        this.oDadosNfe.chavenfe = infNFeElemento.Attributes["Id"].InnerText;
                    }

                    //Montar lista de tag´s da tag <ide>
                    XmlNodeList ideList = infNFeElemento.GetElementsByTagName("ide");

                    //Pegar o conteúdo da tag <dEmi>
                    foreach (XmlNode ideNode in ideList)
                    {
                        XmlElement ideElemento = (XmlElement)ideNode;
                        
                        this.oDadosNfe.dEmi = Convert.ToDateTime(ideElemento.GetElementsByTagName("dEmi")[0].InnerText);
                        this.oDadosNfe.tpEmis = ideElemento.GetElementsByTagName("tpEmis")[0].InnerText;
                        this.oDadosNfe.tpAmb = ideElemento.GetElementsByTagName("tpAmb")[0].InnerText;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                cMensagemErro = ex.Message;

                return false;
            }
        }
    }
}