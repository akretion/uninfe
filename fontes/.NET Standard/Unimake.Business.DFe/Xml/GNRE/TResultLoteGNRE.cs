﻿#pragma warning disable CS1591

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
        public Resultado Resultado { get; set; }
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

        [XmlElement("guia")]
        public List<Guia> Guia { get; set; }

        [XmlElement("pdfGuias")]
        public string PDFGuias { get; set; }

#if INTEROP
        public void AddMotivosRejeicao(MotivosRejeicao motivosRejeicao)
        {
            if(MotivosRejeicao == null)
            {
                MotivosRejeicao = new List<MotivosRejeicao>();
            }

            MotivosRejeicao.Add(motivosRejeicao);
        }

        public void AddGuia(Guia guia)
        {
            if(Guia == null)
            {
                Guia = new List<Guia>();
            }

            Guia.Add(guia);
        }
#endif
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.gnre.pe.gov.br")]
    public class Guia
    {
        [XmlElement("situacaoGuia")]
        public SituacaoGuiaGNRE SituacaoGuia { get; set; }

        [XmlElement("TDadosGNRE")]
        public TDadosGNRE TDadosGNRE { get; set; }

        [XmlElement("representacaoNumerica")]
        public string RepresentacaoNumerica { get; set; }

        [XmlElement("codigoBarras")]
        public string CodigoBarras { get; set; }

        [XmlElement("linhaDigitavel")]
        public string LinhaDigitavel { get; set; }

        [XmlElement("motivosRejeicao")]
        public MotivosRejeicao MotivosRejeicao { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.gnre.pe.gov.br")]
    public class MotivosRejeicao
    {
        [XmlElement("motivo")]
        public List<Motivo> Motivo { get; set; }

#if INTEROP
        public void AddMotivo(Motivo motivo)
        {
            if(Motivo == null)
            {
                Motivo = new List<Motivo>();
            }

            Motivo.Add(motivo);
        }
#endif
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.gnre.pe.gov.br")]
    public class Motivo
    {
        [XmlElement("codigo")]
        public string Codigo { get; set; }

        [XmlElement("descricao")]
        public string Descricao { get; set; }

        [XmlElement("campo")]
        public string Campo { get; set; }
    }
}