#pragma warning disable CS1591

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Xml.GNRE
{
    [ComVisible(true)]
    [Serializable()]
    [XmlRoot("TResultLote_GNRE", Namespace = "http://www.gnre.pe.gov.br", IsNullable = false)]
    public class TResultLoteGNRE: XMLBase
    {
        [XmlElement("ambiente")]
        public TipoAmbiente Ambiente { get; set; }

        [XmlElement("numeroRecibo")]
        public string NumeroRecibo { get; set; }

        [XmlElement("situacaoProcess")]
        public SituacaoProcess SituacaoProcess { get; set; }

        [XmlElement("resultado")]
        public List<Resultado> Resultado { get; set; }

        public void AddResultado(Resultado resultado)
        {
            if(Resultado == null)
            {
                Resultado = new List<Resultado>();
            }

            Resultado.Add(resultado);
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.gnre.pe.gov.br")]
    public class SituacaoProcess
    {
        [XmlElement("codigo")]
        public string Codigo { get; set; }

        [XmlElement("descricao")]
        public string Descricao { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.gnre.pe.gov.br")]
    public class Resultado
    {
        [XmlAttribute(AttributeName = "versao", DataType = "token")]
        public string Versao { get; set; } = "2.00";

        [XmlElement("situacaoGuia")]
        public SituacaoGuiaGNRE SituacaoGuia { get; set; }

        [XmlElement("TDadosGNRE")]
        public TDadosGNRE TDadosGNRE { get; set; }

        [XmlElement("linhaDigitavel")]
        public string LinhaDigitavel { get; set; }

        [XmlElement("codigoBarras")]
        public string CodigoBarras { get; set; }

        [XmlElement("motivosRejeicao")]
        public List<MotivosRejeicao> MotivosRejeicao { get; set; }

        public void AddMotivosRejeicao(MotivosRejeicao motivosRejeicao)
        {
            if(MotivosRejeicao == null)
            {
                MotivosRejeicao = new List<MotivosRejeicao>();
            }

            MotivosRejeicao.Add(motivosRejeicao);
        }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.gnre.pe.gov.br")]
    public class MotivosRejeicao
    {
        [XmlElement("codigo")]
        public string Codigo { get; set; }

        [XmlElement("descricao")]
        public string Descricao { get; set; }

        [XmlElement("campo")]
        public string Campo { get; set; }
    }
}
