using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NFe.Components;

namespace NFe.Components
{
    public class WebServiceNFSe
    {
        private static List<string> _Padroes = null;

        /// <summary>
        /// lista de padrões usados para preencher o datagrid e pesquisas internas
        /// </summary>
        public static string[] PadroesNFSeList
        {
            get
            {
                if (_Padroes == null)
                {
                    Array arr = Enum.GetValues(typeof(PadroesNFSe));
                    _Padroes = new List<string>();
                    foreach (PadroesNFSe type in arr)
                        _Padroes.Add(type.ToString());
                }
                return _Padroes.ToArray();
            }
        }

        public static string URLHomologacao(PadroesNFSe padrao)
        {
            return "<URLHomologacao></URLHomologacao>";
        }

        public static string URLProducao(PadroesNFSe padrao)
        {
            return "<URLProducao></URLProducao>";
        }

        public static string WebServicesHomologacao(PadroesNFSe padrao)
        {
            switch (padrao)
            {
                case PadroesNFSe.THEMA:
                    return "<LocalHomologacao>" +
                            @"<RecepcionarLoteRps>wsdl\homologacao\HThemaRemessa.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HThemaConsulta.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\homologacao\HThemaConsulta.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\homologacao\HThemaConsulta.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\homologacao\HThemaConsulta.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\homologacao\HThemaCancelamento.wsdl</CancelarNfse>" +
                            "</LocalHomologacao>";

                case PadroesNFSe.GINFES:
                    return "<LocalHomologacao>" +
                            @"<RecepcionarLoteRps>wsdl\homologacao\hginfes.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\homologacao\hginfes.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\homologacao\hginfes.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\homologacao\hginfes.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\homologacao\hginfes.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\homologacao\hginfes.wsdl</CancelarNfse>" +
                            "</LocalHomologacao>";

                case PadroesNFSe.BETHA:
                    return "<LocalHomologacao>" +
                            @"<RecepcionarLoteRps>wsdl\homologacao\HBethaRecepcionarLoteRps.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HBethaConsultarSituacaoLoteRPS.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\homologacao\HBethaConsultarNFSePorRPS.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\homologacao\HBethaConsultarNFSe.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\homologacao\HBethaConsultarLoteRPS.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\homologacao\HBethaCancelarNFSe.wsdl</CancelarNfse>" +
                            "</LocalHomologacao>";

                case PadroesNFSe.SALVADOR_BA:
                    return "<LocalHomologacao>" +
                            @"<RecepcionarLoteRps>wsdl\homologacao\HSalvadorBAEnvioLoteRPS.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HSalvadorBAConsultaSituacaoLoteRPS.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\homologacao\HSalvadorBAConsultaNfseRPS.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\homologacao\HSalvadorBAConsultaNfse.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\homologacao\HSalvadorBAConsultaLoteRPS.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\homologacao\HSalvadorBA.wsdl</CancelarNfse>" +
                            "</LocalHomologacao>";

                case PadroesNFSe.CANOAS_RS:
                    return "<LocalHomologacao>" +
                            @"<RecepcionarLoteRps>wsdl\homologacao\HCanoasRSRecepcionarLoteRps.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\homologacao\HCanoasRSConsultarSituacaoLoteRps.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\homologacao\HCanoasRSConsultarNfsePorRps.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\homologacao\HCanoasRSConsultarNfse.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\homologacao\HCanoasRSConsultarLoteRps.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\homologacao\HCanoasRSCancelarNfse.wsdl</CancelarNfse>" +
                            "</LocalHomologacao>";

                default:
                    return "<LocalHomologacao></LocalHomologacao>";
            }
        }

        public static string WebServicesProducao(NFe.Components.PadroesNFSe padrao)
        {
            switch (padrao)
            {
                case PadroesNFSe.THEMA:
                    return "<LocalProducao>" +
                            @"<RecepcionarLoteRps>wsdl\producao\PThemaRemessa.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\producao\PThemaConsulta.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\producao\PThemaConsulta.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\producao\PThemaConsulta.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\producao\PThemaConsulta.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\producao\PThemaCancelamento.wsdl</CancelarNfse>" +
                            @"</LocalProducao>";

                case PadroesNFSe.GINFES:
                    return "<LocalProducao>" +
                            @"<RecepcionarLoteRps>wsdl\producao\pginfes.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\producao\pginfes.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\producao\pginfes.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\producao\pginfes.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\producao\pginfes.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\producao\pginfes.wsdl</CancelarNfse>" +
                            "</LocalProducao>";

                case PadroesNFSe.BETHA:
                    return "<LocalProducao>" +
                            @"<RecepcionarLoteRps>wsdl\producao\PBethaRecepcionarLoteRPS.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\producao\PBethaConsultarSituacaoLoteRPS.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\producao\PBethaConsultarNFSePorRPS.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\producao\PBethaConsultarNFSe.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\producao\PBethaConsultarLoteRPS.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\producao\PBethaCancelarNFSe.wsdl</CancelarNfse>" +
                            "</LocalProducao>";

                case PadroesNFSe.SALVADOR_BA:
                    return "<LocalProducao>" +
                            @"<RecepcionarLoteRps>wsdl\producao\PSalvadorBAEnvioLoteRPS.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\producao\PSalvadorBAConsultaSituacaoLoteRPS.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\producao\PSalvadorBAConsultaNfseRPS.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\producao\PSalvadorBAConsultaNfse.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\producao\PSalvadorBAConsultaLoteRPS.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\producao\PSalvadorBA.wsdl</CancelarNfse>" +
                            "</LocalProducao>";

                case PadroesNFSe.CANOAS_RS:
                    return "<LocalProducao>" +
                            @"<RecepcionarLoteRps>wsdl\producao\PCanoasRSRecepcionarLoteRps.wsdl</RecepcionarLoteRps>" +
                            @"<ConsultarSituacaoLoteRps>wsdl\producao\PCanoasRSConsultarSituacaoLoteRps.wsdl</ConsultarSituacaoLoteRps>" +
                            @"<ConsultarNfsePorRps>wsdl\producao\PCanoasRSConsultarNfsePorRps.wsdl</ConsultarNfsePorRps>" +
                            @"<ConsultarNfse>wsdl\producao\PCanoasRSConsultarNfse.wsdl</ConsultarNfse>" +
                            @"<ConsultarLoteRps>wsdl\producao\PCanoasRSConsultarLoteRps.wsdl</ConsultarLoteRps>" +
                            @"<CancelarNfse>wsdl\producao\PCanoasRSCancelarNfse.wsdl</CancelarNfse>" +
                            "</LocalProducao>";

                default:
                    return "<LocalProducao></LocalProducao>";
            }
        }

        public static NFe.Components.PadroesNFSe GetPadraoFromString(string padrao)
        {
            Array arr = Enum.GetValues(typeof(PadroesNFSe));
            foreach (PadroesNFSe type in arr)
                if (padrao.ToLower() == type.ToString().ToLower())
                    return type;

            return PadroesNFSe.NaoIdentificado;
        }

        public static void SavePadrao(string uf, string cidade, int codigomunicipio, string padrao, bool forcaAtualizacao)
        {
            try
            {
                Municipio mun = null;
                for (int i = 0; i < Propriedade.Municipios.Count; ++i)
                    if (Propriedade.Municipios[i].CodigoMunicipio == codigomunicipio)
                    {
                        mun = Propriedade.Municipios[i];
                        break;
                    }

                if (padrao == PadroesNFSe.NaoIdentificado.ToString() && mun != null)
                    Propriedade.Municipios.Remove(mun);

                if (padrao != PadroesNFSe.NaoIdentificado.ToString())
                {
                    if (mun != null)
                    {
                        ///
                        /// é o mesmo padrão definido?
                        /// o parametro "forcaAtualizacao" é "true" somente quando vier da aba "Municipios definidos"
                        /// desde que o datagrid atualiza automaticamente o membro "padrao" da classe "Municipio" quando ele é alterado.
                        if (mun.PadraoStr == padrao && !forcaAtualizacao)
                            return;

                        mun.Padrao = GetPadraoFromString(padrao);
                        mun.PadraoStr = padrao;
                    }
                    else
                        Propriedade.Municipios.Add(new Municipio(codigomunicipio, uf, cidade.Trim(), GetPadraoFromString(padrao)));
                }

                if (System.IO.File.Exists(Propriedade.NomeArqXMLMunicipios))
                {
                    ///
                    /// faz uma copia por segurança
                    if (System.IO.File.Exists(Propriedade.NomeArqXMLMunicipios + ".bck"))
                        System.IO.File.Delete(Propriedade.NomeArqXMLMunicipios + ".bck");
                    System.IO.File.Copy(Propriedade.NomeArqXMLMunicipios, Propriedade.NomeArqXMLMunicipios + ".bck");
                }

                /*
                <nfes_municipios>
                    <Registro ID="4125506" Nome="São José dos Pinais - PR" Padrao="GINFES" />
                </nfes_municipios>
                 */
                XmlWriter oXmlGravar = null;
                XmlWriterSettings oSettings = new XmlWriterSettings();
                UTF8Encoding c = new UTF8Encoding(false);
                oSettings.Encoding = c;
                oSettings.Indent = true;
                oSettings.IndentChars = "";
                oSettings.NewLineOnAttributes = false;
                oSettings.OmitXmlDeclaration = false;
                oXmlGravar = XmlWriter.Create(Propriedade.NomeArqXMLMunicipios, oSettings);

                //Agora vamos gravar os dados
                oXmlGravar.WriteStartDocument();
                oXmlGravar.WriteStartElement("nfes_municipios");
                {
                    foreach (Municipio item in Propriedade.Municipios)
                    {
                        oXmlGravar.WriteStartElement("Registro");
                        {
                            oXmlGravar.WriteStartAttribute("ID");
                            oXmlGravar.WriteString(item.CodigoMunicipio.ToString());
                            oXmlGravar.WriteEndAttribute();

                            oXmlGravar.WriteStartAttribute("Nome");
                            oXmlGravar.WriteString(item.Nome);
                            oXmlGravar.WriteEndAttribute();

                            oXmlGravar.WriteStartAttribute("Padrao");
                            oXmlGravar.WriteString(item.PadraoStr);
                            oXmlGravar.WriteEndAttribute();
                        }
                        oXmlGravar.WriteEndElement();   //Registro
                    }
                }
                oXmlGravar.WriteEndElement(); //nfes_municipios
                oXmlGravar.WriteEndDocument();
                oXmlGravar.Flush();
                oXmlGravar.Close();
            }
            catch (Exception ex)
            {
                //recupera a copia feita se houve erro na criacao do XML de municipios
                if (System.IO.File.Exists(Propriedade.NomeArqXMLMunicipios + ".bck"))
                    Functions.Move(Propriedade.NomeArqXMLMunicipios + ".bck", Propriedade.NomeArqXMLMunicipios);
                throw ex;
            }
        }

        /// <summary>
        /// Responsavel pela gravacao do arquivo de muncipios, caso nao exista
        /// </summary>
        public static void Start()
        {
            if (!System.IO.File.Exists(Propriedade.NomeArqXMLMunicipios) && System.IO.File.Exists(Propriedade.NomeArqXMLWebService))
            {
                XmlWriter oXmlGravar = null;
                XmlWriterSettings oSettings = new XmlWriterSettings();
                UTF8Encoding c = new UTF8Encoding(false);
                oSettings.Encoding = c;
                oSettings.Indent = true;
                oSettings.IndentChars = "";
                oSettings.NewLineOnAttributes = false;
                oSettings.OmitXmlDeclaration = false;
                oXmlGravar = XmlWriter.Create(Propriedade.NomeArqXMLMunicipios, oSettings);
                //Agora vamos gravar os dados
                oXmlGravar.WriteStartDocument();
                oXmlGravar.WriteStartElement("nfes_municipios");

                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(Propriedade.NomeArqXMLWebService);
                    XmlNodeList estadoList = doc.GetElementsByTagName("Estado");
                    foreach (XmlNode estadoNode in estadoList)
                    {
                        XmlElement estadoElemento = (XmlElement)estadoNode;
                        if (estadoElemento.Attributes.Count > 0)
                        {
                            if (estadoElemento.Attributes[2].Value != "XX")
                            {
                                int ID = Convert.ToInt32(estadoElemento.Attributes[0].Value);
                                string Nome = estadoElemento.Attributes[1].Value;
                                string UF = estadoElemento.Attributes[2].Value;

                                string padrao = PadroesNFSe.NaoIdentificado.ToString();
                                XmlNodeList urlList = estadoElemento.GetElementsByTagName("LocalHomologacao");
                                if (urlList.Count > 0)
                                    ///
                                    /// verifica qual o padrao com base nas url's
                                    foreach (string p0 in PadroesNFSeList)
                                        if (p0.ToLower() != PadroesNFSe.NaoIdentificado.ToString().ToLower())
                                            if (urlList[0].ChildNodes[0].InnerText.ToLower().IndexOf(p0.ToLower()) >= 0)
                                            {
                                                padrao = p0;
                                                break;
                                            }

                                if (padrao != PadroesNFSe.NaoIdentificado.ToString())
                                {
                                    oXmlGravar.WriteStartElement("Registro");
                                    {
                                        oXmlGravar.WriteStartAttribute("ID");
                                        oXmlGravar.WriteString(ID.ToString());
                                        oXmlGravar.WriteEndAttribute();

                                        oXmlGravar.WriteStartAttribute("Nome");
                                        oXmlGravar.WriteString(Nome);
                                        oXmlGravar.WriteEndAttribute();

                                        oXmlGravar.WriteStartAttribute("Padrao");
                                        oXmlGravar.WriteString(padrao);
                                        oXmlGravar.WriteEndAttribute();
                                    }
                                    oXmlGravar.WriteEndElement();   //Registro
                                }
                            }
                        }
                    }
                    oXmlGravar.WriteEndElement(); //nfes_municipios
                    oXmlGravar.WriteEndDocument();
                    oXmlGravar.Flush();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (oXmlGravar != null)
                        oXmlGravar.Close();
                }
            }
        }
    }
}
