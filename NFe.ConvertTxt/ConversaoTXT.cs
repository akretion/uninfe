using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace NFe.ConvertTxt
{
    public class ConversaoTXT
    {
        #region --- public properties

        public List<txtTOxmlClassRetorno> cRetorno = null;
        public string cMensagemErro { get; private set; }

        #endregion

        #region -- private proprieties

        private NFe NFe = null;
        private string FSegmento;
        private int LinhaLida;
        private string Registro;
        private string layout;
        private string chave;
        /// <summary>
        /// conteudo do arquivo de cada nota
        /// </summary>
        private Dictionary<int, List<string>> xConteudoArquivo;

        #endregion

        public ConversaoTXT() 
        {
            this.xConteudoArquivo = new Dictionary<int, List<string>>();
            this.cRetorno = new List<txtTOxmlClassRetorno>();
            this.cMensagemErro = "";
            this.LinhaLida = 0;
        }

        /// <summary>
        /// CarregarArquivo
        /// </summary>
        private bool CarregarArquivo(string cArquivo)
        {
            if (File.Exists(cArquivo))
            {
                TextReader txt = new StreamReader(cArquivo, Encoding.Default, true);
                try
                {
                    int nNota = -1;
                    string cLinhaTXT = txt.ReadLine();
                    if (cLinhaTXT != null)
                    {
                        if (!cLinhaTXT.StartsWith("NOTAFISCAL") && !cLinhaTXT.StartsWith("NOTA FISCAL"))
                        {
                            this.cMensagemErro = Properties.Resources.msg_001;
                        }
                        cLinhaTXT = txt.ReadLine();
                        this.LinhaLida = 1;
                    }
                    while (cLinhaTXT != null)
                    {
                        ++LinhaLida;

                        if (cLinhaTXT.Trim().Length > 0)
                        {
                            if (cLinhaTXT.StartsWith("A|"))
                            {
                                ++nNota;
                                xConteudoArquivo.Add(nNota, new List<string>());
                            }
                            List<string> temp;
                            xConteudoArquivo.TryGetValue(nNota, out temp);
                            temp.Add("§" + cLinhaTXT.Trim() + "|");
                        }
                        cLinhaTXT = txt.ReadLine();
                    }
                }
                catch (IOException ex)
                {
                    this.cMensagemErro += ex.Message;
                }
                catch (Exception ex)
                {
                    this.cMensagemErro += ex.Message;
                }
                finally
                {
                    txt.Close();
                }
            }
            else
                this.cMensagemErro = "Arquivo [" + cArquivo + "] não encontrado";

            return ((this.xConteudoArquivo.Count == 0 || !string.IsNullOrEmpty(this.cMensagemErro)) ? false : true);
        }

        /// <summary>
        /// Converter
        /// </summary>
        public bool Converter(string cArquivo, string cFolderDestino)//, string cFolderRetorno)
        {
            cRetorno.Clear();

            if (this.CarregarArquivo(cArquivo))
            {
                this.LinhaLida = 0;
                foreach (List<string> content in this.xConteudoArquivo.Values)
                {
                    NFe = null;
                    NFe = new NFe();
                    bool houveErro = false;

                    foreach (string xContent in content)
                    {
                        houveErro = false;
                        ++this.LinhaLida;
                        try
                        {
                            ///
                            /// processa o TXT
                            /// 
                            this.LerRegistro(xContent);
                        }
                        catch(Exception ex)
                        {
                            houveErro = true;
                            this.cMensagemErro += "Linha lida: " + this.LinhaLida.ToString()+ Environment.NewLine+
                                                    "Conteudo: " + xContent.Substring(1) + Environment.NewLine +
                                                    ex.Message + Environment.NewLine;
                        }
                    }
                    
                    if (!houveErro && this.cMensagemErro == "")
                    {
                        NFeW nfew = new NFeW();
                        try
                        {
                            nfew.cMensagemErro = this.cMensagemErro;
                            ///
                            /// gera o XML da nota
                            /// 
                            nfew.GerarXml(NFe, cFolderDestino);//cFolderRetorno);
                            if (nfew.cFileName != "")
                            {
                                ///
                                /// Adiciona o XML na lista de arquivos convertidos
                                /// 
                                this.cRetorno.Add(new txtTOxmlClassRetorno(nfew.cFileName, NFe.infNFe.ID, NFe.ide.nNF, NFe.ide.serie));
                            }
                        }
                        catch (Exception ex)
                        {
                            nfew.cMensagemErro += ex.Message;
                        }
                        this.cMensagemErro = nfew.cMensagemErro;
                    }

                    if (this.cMensagemErro != "")
                    {
                        ///
                        /// exclui os arquivos gerados
                        /// 
                        foreach (txtTOxmlClassRetorno txtClass in this.cRetorno)
                        {
                            string dArquivo = txtClass.XMLFileName;
                            if (File.Exists(dArquivo))
                            {
                                FileInfo fi = new FileInfo(dArquivo);
                                fi.Delete();
                            }
                        }
                    }
                }
                return string.IsNullOrEmpty(this.cMensagemErro);
            }
            return false;
        }

        /// <summary>
        /// getDateTime
        /// </summary>
        public DateTime getDateTime(TpcnTipoCampo Tipo, string value)
        {
            if (string.IsNullOrEmpty(value))
                return DateTime.MinValue;

            try
            {
                int _ano = Convert.ToInt16(value.Substring(0, 4));
                int _mes = Convert.ToInt16(value.Substring(5, 2));
                int _dia = Convert.ToInt16(value.Substring(8, 2));
                if (Tipo == TpcnTipoCampo.tcDatHor && value.Contains(":"))
                {
                    int _hora = Convert.ToInt16(value.Substring(11, 2));
                    int _min = Convert.ToInt16(value.Substring(14, 2));
                    int _seg = Convert.ToInt16(value.Substring(17, 2));
                    return new DateTime(_ano, _mes, _dia, _hora, _min, _seg);
                }
                return new DateTime(_ano, _mes, _dia);
            }
            catch
            {
                throw new Exception("Data inválida do conteudo [" + value + "]");
            }
        }

        /// <summary>
        /// getDateTime2
        /// </summary>
        public DateTime getDate2(TpcnTipoCampo Tipo, string value)
        {
            if (string.IsNullOrEmpty(value))
                return DateTime.MinValue;

            if (value.Contains("-"))
                return this.getDateTime(Tipo, value);

            try
            {
                int _ano = Convert.ToInt16(value.Substring(0, 4));
                int _mes = Convert.ToInt16(value.Substring(4, 2));
                int _dia = Convert.ToInt16(value.Substring(6, 2));
                return new DateTime(_ano, _mes, _dia);
            }
            catch
            {
                throw new Exception("Data inválida do conteudo [" + value + "]");
            }
        }

        /// <summary>
        /// getTime
        /// </summary>
        private DateTime getTime(string value)
        {
            if (string.IsNullOrEmpty(value))
                return DateTime.MinValue;

            try
            {
                int _hora = Convert.ToInt16(value.Substring(0, 2));
                int _min = Convert.ToInt16(value.Substring(3, 2));
                int _seg = Convert.ToInt16(value.Substring(6, 2));
                return new DateTime(1,1,1, _hora, _min, _seg);
            }
            catch
            {
                throw new Exception("Hora inválida do conteudo [" + value + "]");
            }
        }

        /// <summary>
        /// RetornarConteudoTag
        /// </summary>
        private string RetornarConteudoTag(string TAG)
        {
            ///
            /// "§B14|cUF¨|AAMM¨|CNPJ¨|Mod¨|serie¨|nNF¨"); //ok
            /// 
            /// se a tag a ser consulta é CNPJ, então é verificada no layout quantoa pipes existem até ela.
            /// neste caso no comando abaixo será retornado "§B14|cUF¨|AAMM¨|" existindo 3 pipes para pegar
            /// o valor do retorno
            /// 
            string fValue = layout.Substring(0, layout.ToUpper().IndexOf("|" + TAG.ToUpper().Trim() + "¨") + 1);
            if (fValue == "")
                throw new Exception("Segmento: " + this.FSegmento + " - Tag: " + TAG + " não encontrada");

            string[] pipes = fValue.Split('|');
            int j = pipes.GetLength(0) - 2;
            if (j >= 0)
            {
                ///
                /// qual a posicao do conteudo do registro lido
                /// 
                string[] dados = this.Registro.Split('|');
                try
                {
                    return dados[j + 1].TrimStart().TrimEnd();
                }
                catch
                {
                    return "";
                }
            }
            else
                return "";
        }

        /// <summary>
        /// SomenteNumeros
        /// </summary>
        private string SomenteNumeros(string entrada)
        {
            if (string.IsNullOrEmpty(entrada)) return "";

            StringBuilder saida = new StringBuilder(entrada.Length);
            foreach (char c in entrada)
            {
                if (char.IsDigit(c))
                {
                    saida.Append(c);
                }
            }
            return saida.ToString();
        }

        /// <summary>
        /// LerCampo
        /// </summary>
        private double LerCampo(TpcnTipoCampo Tipo, string TAG, ObOp optional, int maxLength)
        {
            return (double)LerCampo(Tipo, TAG, optional, 0, maxLength);
        }

        /// <summary>
        /// LerCampo
        /// </summary>
        private object LerCampo(TpcnTipoCampo Tipo, string TAG, ObOp optional, int minLength, int maxLength)
        {
            int nDecimais = 0;
            string ConteudoTag = "";
            try
            {
                ConteudoTag = RetornarConteudoTag(TAG);

                if (ConteudoTag != "")
                    if (ConteudoTag.StartsWith("§"))
                        ConteudoTag = "";

                int len = ConteudoTag.Length;
                if (len == 0 && optional == ObOp.Opcional)
                {
                }
                else
                {
                    switch (Tipo)
                    {
                        case TpcnTipoCampo.tcHor:
                            maxLength = minLength = 8; //hh:mm:ss
                            break;
                        case TpcnTipoCampo.tcDat:
                            maxLength = minLength = 10; //yyyy-MM-dd
                            break;
                        case TpcnTipoCampo.tcDat2:
                            maxLength = minLength = 8; //yyyyMMdd
                            break;
                        case TpcnTipoCampo.tcDatHor:
                            maxLength = minLength = 19; //aaaa-mm-dd hh:mm:ss
                            break;
                        case TpcnTipoCampo.tcDec2:
                            nDecimais = 2;
                            break;
                        case TpcnTipoCampo.tcDec3:
                            nDecimais = 3;
                            break;
                        case TpcnTipoCampo.tcDec4:
                            nDecimais = 4;
                            break;
                        case TpcnTipoCampo.tcDec10:
                            nDecimais = 10;
                            break;
                    }

                    if (len == 0 && minLength > 0)
                    {
                        this.cMensagemErro += string.Format("Segmento [{0}]: tag <{1}> deve ser informada.\r\n" +
                                                            "\tLinha: {2}: Conteudo do segmento: {3}",
                                                            this.FSegmento, TAG, this.LinhaLida, this.Registro.Substring(1)) + Environment.NewLine;
                    }
                    else
                    {
                        switch (Tipo)
                        {
                            case TpcnTipoCampo.tcDec2:
                            case TpcnTipoCampo.tcDec3:
                            case TpcnTipoCampo.tcDec4:
                            case TpcnTipoCampo.tcDec10:
                                //quando numerico do tipo double não consiste o tamanho minimo nem maximo
                                break;
                            default:
                                if ((len > maxLength || len < minLength) && (maxLength + minLength > 0))
                                    this.cMensagemErro += string.Format("Segmento [{0}]: tag <{1}> deve ter seu tamanho entre {2} e {3}. Conteudo: {4}" +
                                                            "\r\n\tLinha: {5}: Conteudo do segmento: {6}",
                                                            this.FSegmento, TAG, minLength, maxLength, ConteudoTag, this.LinhaLida, this.Registro.Substring(1)) + Environment.NewLine;
                                break;
                        }
                    }
                }

                if (optional == ObOp.Obrigatorio || (optional == ObOp.Opcional && len != 0))
                {
                    switch (Tipo)
                    {
                        case TpcnTipoCampo.tcDec2:
                        case TpcnTipoCampo.tcDec3:
                        case TpcnTipoCampo.tcDec4:
                        case TpcnTipoCampo.tcDec10:
                            {
                                int pos = ConteudoTag.IndexOf(".") + 1;
                                int ndec = ConteudoTag.Substring(pos).Length;
                                string xdec = ConteudoTag.Substring(pos);
                                //
                                // ajusta o numero de casas decimais
                                while (ndec > nDecimais)
                                {
                                    if (xdec.Substring(ndec - 1, 1) == "0")
                                        --ndec;
                                    else
                                        break;
                                }

                                if (ndec > nDecimais)
                                    this.cMensagemErro += string.Format("Segmento [{0}]: tag <{1}> número de casas decimais deve ser de {2} e existe(m) {3}" +
                                                                        "\r\n\tLinha: {4}: Conteudo do segmento: {5}",
                                                                        this.FSegmento, TAG, nDecimais, ndec, this.LinhaLida, this.Registro.Substring(1)) + Environment.NewLine;

                                #region -- atribui o numero de casas decimais que serão gravadas

                                if (TAG == Properties.Resources.vUnCom)
                                    switch (ndec)
                                    {
                                        case 2:
                                            NFe.det[NFe.det.Count - 1].Prod.vUnCom_Tipo = TpcnTipoCampo.tcDec2;
                                            break;
                                        case 3:
                                            NFe.det[NFe.det.Count - 1].Prod.vUnCom_Tipo = TpcnTipoCampo.tcDec3;
                                            break;
                                        case 4:
                                            NFe.det[NFe.det.Count - 1].Prod.vUnCom_Tipo = TpcnTipoCampo.tcDec4;
                                            break;
                                        case 10:
                                            NFe.det[NFe.det.Count - 1].Prod.vUnCom_Tipo = TpcnTipoCampo.tcDec10;
                                            break;
                                    }

                                if (TAG == Properties.Resources.vUnTrib)
                                    switch (ndec)
                                    {
                                        case 2:
                                            NFe.det[NFe.det.Count - 1].Prod.vUnTrib_Tipo = TpcnTipoCampo.tcDec2;
                                            break;
                                        case 3:
                                            NFe.det[NFe.det.Count - 1].Prod.vUnTrib_Tipo = TpcnTipoCampo.tcDec3;
                                            break;
                                        case 4:
                                            NFe.det[NFe.det.Count - 1].Prod.vUnTrib_Tipo = TpcnTipoCampo.tcDec4;
                                            break;
                                        case 10:
                                            NFe.det[NFe.det.Count - 1].Prod.vUnTrib_Tipo = TpcnTipoCampo.tcDec10;
                                            break;
                                    }

                                if (TAG == Properties.Resources.qTotMes)
                                    switch (ndec)
                                    {
                                        case 2:
                                            NFe.cana.qTotMes_Tipo = TpcnTipoCampo.tcDec2;
                                            break;
                                        case 3:
                                            NFe.cana.qTotMes_Tipo = TpcnTipoCampo.tcDec3;
                                            break;
                                        case 4:
                                            NFe.cana.qTotMes_Tipo = TpcnTipoCampo.tcDec4;
                                            break;
                                        case 10:
                                            NFe.cana.qTotMes_Tipo = TpcnTipoCampo.tcDec10;
                                            break;
                                    }

                                if (TAG == Properties.Resources.qTotAnt)
                                    switch (ndec)
                                    {
                                        case 2:
                                            NFe.cana.qTotAnt_Tipo = TpcnTipoCampo.tcDec2;
                                            break;
                                        case 3:
                                            NFe.cana.qTotAnt_Tipo = TpcnTipoCampo.tcDec3;
                                            break;
                                        case 4:
                                            NFe.cana.qTotAnt_Tipo = TpcnTipoCampo.tcDec4;
                                            break;
                                        case 10:
                                            NFe.cana.qTotAnt_Tipo = TpcnTipoCampo.tcDec10;
                                            break;
                                    }

                                if (TAG == Properties.Resources.qTotGer)
                                    switch (ndec)
                                    {
                                        case 2:
                                            NFe.cana.qTotGer_Tipo = TpcnTipoCampo.tcDec2;
                                            break;
                                        case 3:
                                            NFe.cana.qTotGer_Tipo = TpcnTipoCampo.tcDec3;
                                            break;
                                        case 4:
                                            NFe.cana.qTotGer_Tipo = TpcnTipoCampo.tcDec4;
                                            break;
                                        case 10:
                                            NFe.cana.qTotGer_Tipo = TpcnTipoCampo.tcDec10;
                                            break;
                                    }

                                if (TAG == Properties.Resources.qtde)
                                    if (TAG == Properties.Resources.qTotGer)
                                        switch (ndec)
                                        {
                                            case 2:
                                                NFe.cana.fordia[NFe.cana.fordia.Count - 1].qtde_Tipo = TpcnTipoCampo.tcDec2;
                                                break;
                                            case 3:
                                                NFe.cana.fordia[NFe.cana.fordia.Count - 1].qtde_Tipo = TpcnTipoCampo.tcDec3;
                                                break;
                                            case 4:
                                                NFe.cana.fordia[NFe.cana.fordia.Count - 1].qtde_Tipo = TpcnTipoCampo.tcDec4;
                                                break;
                                            case 10:
                                                NFe.cana.fordia[NFe.cana.fordia.Count - 1].qtde_Tipo = TpcnTipoCampo.tcDec10;
                                                break;
                                        }
                                
                                #endregion
                            }
                            break;
                    }
                }

                switch (Tipo)
                {
                    case TpcnTipoCampo.tcDat2:
                        return this.getDate2(Tipo, ConteudoTag);
                        break;

                    case TpcnTipoCampo.tcDat:
                    case TpcnTipoCampo.tcDatHor:
                        return this.getDateTime(Tipo, ConteudoTag);

                    case TpcnTipoCampo.tcHor:
                        return this.getTime(ConteudoTag);

                    case TpcnTipoCampo.tcDec2:
                    case TpcnTipoCampo.tcDec3:
                    case TpcnTipoCampo.tcDec4:
                    case TpcnTipoCampo.tcDec10:
                        return Convert.ToDouble("0" + ConteudoTag.Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator));

                    case TpcnTipoCampo.tcInt:
                        return Convert.ToInt32("0" + SomenteNumeros(ConteudoTag));

                    default:
                        if (ConteudoTag.StartsWith("<![CDATA["))
                            return ConteudoTag.Trim();
                        return ConteudoTag.Replace("&", "&amp;").
                                        Replace("<", "&lt;").
                                        Replace(">", "&gt;").
                                        Replace("\"", "&quot;").
                                        Replace("\r\n", "|").
                                        Replace("'", "&#39;").Trim();
                }
            }
            catch (Exception ex)
            {
                this.cMensagemErro += string.Format("Segmento [{0}]: tag <{1}> Conteudo: {2}\r\n" +
                                                    "\tLinha: {3}: Conteudo do segmento: {4}\r\n\tMensagem de erro: {5}",
                                                    this.FSegmento, TAG, ConteudoTag, this.LinhaLida, this.Registro.Substring(1),
                                                    ex.Message) + Environment.NewLine;
                switch (Tipo)
                {
                    case TpcnTipoCampo.tcHor:
                    case TpcnTipoCampo.tcDat:
                    case TpcnTipoCampo.tcDat2:
                    case TpcnTipoCampo.tcDatHor:
                        return DateTime.MinValue;

                    case TpcnTipoCampo.tcDec2:
                    case TpcnTipoCampo.tcDec3:
                    case TpcnTipoCampo.tcDec4:
                    case TpcnTipoCampo.tcDec10:
                        return 0.0;

                    case TpcnTipoCampo.tcInt:
                        return 0;

                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// LerRegistro
        /// </summary>
        private void LerRegistro(string aRegistro)
        {
            int nProd = NFe.det.Count - 1;
            this.Registro = aRegistro;
            this.FSegmento = this.Registro.Substring(1, this.Registro.IndexOf("|")-1);

            switch (this.FSegmento.ToUpper())
            {
                case "A":
                    layout = "§A|versao¨|Id¨";
                    double v = (double)LerCampo(TpcnTipoCampo.tcDec2, "versao", ObOp.Opcional, 0, 6);
                    this.chave = (string)LerCampo(TpcnTipoCampo.tcStr, "ID", ObOp.Opcional, 0, 47);
                    if (this.chave.Equals("NFe")) this.chave = string.Empty;
                    this.chave = this.chave.Replace("NFe", "");
                    NFe.infNFe.Versao = (v>0 ? Convert.ToDecimal(v) : 2);
                    break;

                case "B":
                    if (NFe.infNFe.Versao >= 3)
                        layout = "§B|cUF¨|cNF¨|NatOp¨|indPag¨|mod¨|serie¨|nNF¨|dhEmi¨|dhSaiEnt¨|tpNF¨|cMunFG¨|TpImp¨|TpEmis¨|cDV¨|TpAmb¨|FinNFe¨|ProcEmi¨|VerProc¨|dhCont¨|xJust¨";
                    else
                        layout = "§B|cUF¨|cNF¨|NatOp¨|indPag¨|mod¨|serie¨|nNF¨|dEmi¨|dSaiEnt¨|hSaiEnt¨|tpNF¨|cMunFG¨|TpImp¨|TpEmis¨|cDV¨|TpAmb¨|FinNFe¨|ProcEmi¨|VerProc¨|dhCont¨|xJust¨";

                    ///
                    /// Grupo da TAG <ide>
                    /// 
                    #region -- <ide>

                    NFe.ide.cUF     = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.cUF, ObOp.Obrigatorio, 2, 2);
                    NFe.ide.cNF     = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.cNF, ObOp.Opcional, 8, 8);
                    NFe.ide.natOp   = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.natOp, ObOp.Obrigatorio, 1, 60);
                    NFe.ide.indPag  = (TpcnIndicadorPagamento)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.indPag, ObOp.Obrigatorio, 1, 1);
                    NFe.ide.mod     = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.mod, ObOp.Obrigatorio, 2, 2);
                    NFe.ide.serie   = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.serie, ObOp.Obrigatorio, 1, 3);
                    NFe.ide.nNF     = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.nNF, ObOp.Obrigatorio, 1, 9);
                    if (NFe.infNFe.Versao >= 3)
                    {
                        NFe.ide.dhEmi    = (string)LerCampo(TpcnTipoCampo.tcStr, "dhEmi",    ObOp.Obrigatorio, 19, 25);
                        NFe.ide.dhSaiEnt = (string)LerCampo(TpcnTipoCampo.tcStr, "dhSaiEnt", ObOp.Opcional,     0, 25);
                    }
                    else
                    {
                        NFe.ide.dEmi    = (DateTime)LerCampo(TpcnTipoCampo.tcDat, Properties.Resources.dEmi, ObOp.Obrigatorio, 10, 10);
                        NFe.ide.dSaiEnt = (DateTime)LerCampo(TpcnTipoCampo.tcDat, Properties.Resources.dSaiEnt, ObOp.Opcional, 10, 10);
                        NFe.ide.hSaiEnt = (DateTime)LerCampo(TpcnTipoCampo.tcHor, Properties.Resources.hSaiEnt, ObOp.Opcional, 0, 0);
                    }
                    NFe.ide.tpNF    = (TpcnTipoNFe)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.tpNF, ObOp.Obrigatorio, 1, 1);
                    NFe.ide.cMunFG  = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.cMunFG, ObOp.Obrigatorio, 7, 7);
                    NFe.ide.tpImp   = (TpcnTipoImpressao)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.tpImp, ObOp.Obrigatorio, 1, 1);
                    NFe.ide.tpEmis  = (TpcnTipoEmissao)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.tpEmis, ObOp.Obrigatorio, 1, 1);
                    NFe.ide.cDV     = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.cDV, ObOp.Opcional, 1, 1);
                    NFe.ide.tpAmb   = (TpcnTipoAmbiente)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.tpAmb, ObOp.Obrigatorio, 1, 1);
                    NFe.ide.finNFe  = (TpcnFinalidadeNFe)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.finNFe, ObOp.Obrigatorio, 1, 1);
                    NFe.ide.procEmi = (TpcnProcessoEmissao)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.procEmi, ObOp.Obrigatorio, 1, 1);
                    NFe.ide.verProc = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.verProc, ObOp.Obrigatorio, 1, 20);
                    NFe.ide.dhCont  = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.dhCont, ObOp.Opcional, 0, 25);
                    NFe.ide.xJust   = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xJust, ObOp.Opcional, 15, 256);

                    if (!string.IsNullOrEmpty(this.chave))
                    {
                        if (NFe.ide.cNF == 0)
                            NFe.ide.cNF = Convert.ToInt32(this.chave.Substring(35, 8));

                        if (NFe.ide.cDV == 0)
                            NFe.ide.cDV = Convert.ToInt32(this.chave.Substring(this.chave.Length - 1, 1));
                    }
                    break;
                    #endregion

                case "B11A":
                    layout = "§B11a|idDest¨"; //ok
                    NFe.ide.idDest = (TpcnDestinoOperacao)LerCampo(TpcnTipoCampo.tcInt, "idDest", ObOp.Obrigatorio, 1, 1);
                    break;

                case "B13":
                    layout = "§B13|refNFe¨"; //ok

                    ///
                    /// Grupo da TAG <ide><NFref><refNFe>
                    ///
                    #region <ide><NFref><refNFe>

                    NFe.ide.NFref.Add(new NFref((string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.refNFe, ObOp.Obrigatorio, 44, 44), null));

                    #endregion
                    break;

                case "B14":
                    layout = "§B14|cUF¨|AAMM¨|CNPJ¨|Mod¨|serie¨|nNF¨"; //ok

                    ///
                    /// Grupo da TAG <ide><NFref><RefNF>
                    ///
                    #region <ide><NFref><RefNF>
                    {
                        NFref item = new NFref();
                        item.refNF = new refNF();

                        item.refNF.cUF   = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.cUF, ObOp.Obrigatorio, 2, 2);
                        item.refNF.AAMM  = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.AAMM, ObOp.Obrigatorio, 4, 4);
                        item.refNF.CNPJ  = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CNPJ, ObOp.Obrigatorio, 14, 14);
                        item.refNF.mod   = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.mod, ObOp.Obrigatorio, 2, 2);
                        item.refNF.serie = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.serie, ObOp.Obrigatorio, 1, 3);
                        item.refNF.nNF   = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.nNF, ObOp.Obrigatorio, 1, 9);

                        NFe.ide.NFref.Add(item);
                    }
                    #endregion
                    break;

                case "B20A":
                    layout = "§B20a|cUF¨|AAMM¨|IE¨|Mod¨|serie¨|nNF¨"; //ok

                    #region B20a
                    {
                        NFref item = new NFref();
                        item.refNFP = new refNFP();
                        item.refNFP.cUF     = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.cUF, ObOp.Obrigatorio, 2, 2);
                        item.refNFP.AAMM    = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.AAMM, ObOp.Obrigatorio, 4, 4);
                        item.refNFP.IE      = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.IE, ObOp.Obrigatorio, 1, 14);
                        item.refNFP.mod     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.mod, ObOp.Obrigatorio, 2, 2);
                        item.refNFP.serie   = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.serie, ObOp.Obrigatorio, 1, 3);
                        item.refNFP.nNF     = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.nNF, ObOp.Obrigatorio, 1, 9);

                        NFe.ide.NFref.Add(item);
                    }
                    #endregion
                    break;

                case "B20D":
                    layout = "§B20d|CNPJ¨"; //ok

                    if (NFe.ide.NFref.Count == 0 || (NFe.ide.NFref.Count > 0 && NFe.ide.NFref[NFe.ide.NFref.Count-1].refNFP == null))
                        throw new Exception("Segmento B20d sem segmento B20A");
                    NFe.ide.NFref[NFe.ide.NFref.Count-1].refNFP.CNPJ = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CNPJ, ObOp.Obrigatorio, 14, 14);
                    break;

                case "B20E":
                    layout = "§B20e|CPF¨"; //ok

                    if (NFe.ide.NFref.Count == 0 || (NFe.ide.NFref.Count > 0 && NFe.ide.NFref[NFe.ide.NFref.Count - 1].refNFP == null))
                        throw new Exception("Segmento B20e sem segmento B20A");
                    NFe.ide.NFref[NFe.ide.NFref.Count - 1].refNFP.CPF = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CPF, ObOp.Obrigatorio, 11, 11);
                    break;

                case "B20I":    
                    layout = "§B20i|refCTe¨"; //ok

                    NFe.ide.NFref.Add(new NFref(null, (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.refCTe, ObOp.Obrigatorio, 44, 44)));
                    break;

                case "B20J":
                    layout = "§B20j|mod¨|nECF¨|nCOO¨"; //ok
                    {
                        NFref item = new NFref();
                        item.refECF = new refECF();
                        item.refECF.mod  = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.mod, ObOp.Obrigatorio, 2, 2);
                        item.refECF.nECF = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.nECF, ObOp.Obrigatorio, 1, 3);
                        item.refECF.nCOO = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.nCOO, ObOp.Obrigatorio, 1, 6);
                        NFe.ide.NFref.Add(item);
                    }
                    break;

                case "B25A":
                    layout = "§B25A|indFinal¨"; //ok
                    NFe.ide.indFinal =  (TpcnConsumidorFinal)LerCampo(TpcnTipoCampo.tcInt, "indFinal", ObOp.Obrigatorio, 1, 1);
                    break;

                case "B25B":
                    layout = "§B25b|indPres¨"; //ok
                    NFe.ide.indPres = (TpcnPresencaComprador)LerCampo(TpcnTipoCampo.tcInt, "indPres", ObOp.Obrigatorio, 1, 1);
                    break;

                case "C":
                    layout = "§C|XNome¨|XFant¨|IE¨|IEST¨|IM¨|CNAE¨|CRT¨"; //ok

                    ///
                    /// Grupo da TAG <emit>
                    ///
                    #region <emit>

                    NFe.emit.xNome  = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xNome, ObOp.Obrigatorio, 2, 60);
                    NFe.emit.xFant  = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xFant, ObOp.Opcional, 1, 60);
                    NFe.emit.IE     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.IE, ObOp.Opcional, 0, 14);
                    NFe.emit.IEST   = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.IEST, ObOp.Opcional, 2, 14);
                    NFe.emit.IM     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.IM, ObOp.Opcional, 1, 15);
                    NFe.emit.CNAE   = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CNAE, ObOp.Opcional, 7, 7);
                    NFe.emit.CRT    = (TpcnCRT)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.CRT, ObOp.Obrigatorio, 1, 1);

                    #endregion
                    break;

                case "C02": 
                    layout = "§C02|CNPJ¨"; //ok
                    
                    NFe.emit.CNPJ = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CNPJ, ObOp.Obrigatorio, 14, 14);
                    break;

                case "C02A":    
                    layout = "§C02A|CPF¨"; //ok
                    NFe.emit.CPF = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CPF, ObOp.Obrigatorio, 11, 11);
                    break;

                case "C05":
                    layout = "§C05|XLgr¨|Nro¨|xCpl¨|xBairro¨|CMun¨|XMun¨|UF¨|CEP¨|CPais¨|XPais¨|fone¨"; //ok

                    ///
                    /// Grupo da TAG <emit><EnderEmit>
                    /// 
                    #region <emit><EnderEmit>

                    NFe.emit.enderEmit.xLgr     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xLgr, ObOp.Obrigatorio, 2, 60);
                    NFe.emit.enderEmit.nro      = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nro, ObOp.Obrigatorio, 1, 60);
                    NFe.emit.enderEmit.xCpl     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xCpl, ObOp.Opcional, 1, 60);
                    NFe.emit.enderEmit.xBairro  = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xBairro, ObOp.Obrigatorio, 2, 60);
                    NFe.emit.enderEmit.cMun     = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.cMun, ObOp.Obrigatorio, 7, 7);
                    NFe.emit.enderEmit.xMun     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xMun, ObOp.Obrigatorio, 2, 60);
                    NFe.emit.enderEmit.UF       = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.UF, ObOp.Obrigatorio, 2, 2);
                    NFe.emit.enderEmit.CEP      = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.CEP, ObOp.Opcional, 0, 8);
                    NFe.emit.enderEmit.cPais    = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.cPais, ObOp.Obrigatorio, 4, 4);
                    NFe.emit.enderEmit.xPais    = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xPais, ObOp.Opcional, 1, 60);
                    NFe.emit.enderEmit.fone     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.fone, ObOp.Opcional, 6, 14);

                    #endregion
                    break;

                case "D":
                    layout = "§D|CNPJ¨|xOrgao¨|Matr¨|xAgente¨|fone¨|UF¨|nDAR¨|dEmi¨|vDAR¨|RepEmi¨|dPag¨"; //ok

                    ///
                    /// Grupo da TAG <avulsa>
                    /// 
                    #region <avulsa>

                    NFe.avulsa.CNPJ     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CNPJ, ObOp.Obrigatorio, 14, 14);
                    NFe.avulsa.xOrgao   = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xOrgao, ObOp.Obrigatorio, 1, 60);
                    NFe.avulsa.matr     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.matr, ObOp.Obrigatorio, 1, 60);
                    NFe.avulsa.xAgente  = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xAgente, ObOp.Obrigatorio, 1, 60);
                    NFe.avulsa.fone     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.fone, ObOp.Obrigatorio, 6, 14);
                    NFe.avulsa.UF       = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.UF, ObOp.Obrigatorio, 2, 2);
                    NFe.avulsa.nDAR     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nDAR, ObOp.Obrigatorio, 1, 60);
                    NFe.avulsa.dEmi     = (DateTime)LerCampo(TpcnTipoCampo.tcDat, Properties.Resources.dEmi, ObOp.Obrigatorio, 10, 10);
                    NFe.avulsa.vDAR     = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vDAR, ObOp.Obrigatorio, 15);
                    NFe.avulsa.repEmi   = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.repEmi, ObOp.Obrigatorio, 1, 60);
                    NFe.avulsa.dPag     = (DateTime)LerCampo(TpcnTipoCampo.tcDat, Properties.Resources.dPag, ObOp.Opcional, 10, 10);

                    #endregion
                    break;

                case "E":
                    layout = "§E|xNome¨|IE¨|ISUF¨|email¨ "; //ok
                    ///
                    /// Grupo da TAG <dest>
                    /// 
                    #region <dest>

                    NFe.dest.xNome  = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xNome, ObOp.Obrigatorio, 2, 60);
                    NFe.dest.IE     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.IE, ObOp.Opcional, 0, 14);
                    NFe.dest.ISUF   = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.ISUF, ObOp.Opcional, 8, 9);
                    NFe.dest.email  = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.email, ObOp.Opcional, 1, 60);

                    #endregion
                    break;

                case "E02":
                    layout = "§E02|CNPJ¨"; //ok
                    NFe.dest.CNPJ = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CNPJ, ObOp.Opcional, 14, 14);
                    break;

                case "E03": 
                    layout = "§E03|CPF¨"; //ok
                    NFe.dest.CPF = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CPF, ObOp.Obrigatorio, 11, 11);
                    break;

                case "E03A":
                    layout = "§E03a|idEstrangeiro¨"; //ok
                    if ((double)NFe.infNFe.Versao >= 3.10)
                        NFe.dest.idEstrangeiro = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.idEstrangeiro, ObOp.Opcional, 0, 20);
                    break;

                case "E05": 
                    layout = "§E05|xLgr¨|nro¨|xCpl¨|xBairro¨|cMun¨|xMun¨|UF¨|CEP¨|cPais¨|xPais¨|fone¨"; //ok
                    ///
                    /// Grupo da TAG <dest><EnderDest>
                    /// 
                    #region <dest><EnderDest>
                    NFe.dest.enderDest.xLgr     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xLgr, ObOp.Obrigatorio, 2, 60);
                    NFe.dest.enderDest.nro      = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nro, ObOp.Obrigatorio, 1, 60);
                    NFe.dest.enderDest.xCpl     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xCpl, ObOp.Opcional, 1, 60);
                    NFe.dest.enderDest.xBairro  = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xBairro, ObOp.Obrigatorio, 1, 60);
                    NFe.dest.enderDest.cMun     = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.cMun, ObOp.Obrigatorio, 7, 7);
                    NFe.dest.enderDest.xMun     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xMun, ObOp.Obrigatorio, 2, 60);
                    NFe.dest.enderDest.UF       = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.UF, ObOp.Obrigatorio, 2, 2);
                    NFe.dest.enderDest.CEP      = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.CEP, ObOp.Opcional, 0, 8);
                    NFe.dest.enderDest.cPais    = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.cPais, ObOp.Obrigatorio, 2, 4);
                    NFe.dest.enderDest.xPais    = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xPais, ObOp.Opcional, 2, 60);
                    NFe.dest.enderDest.fone     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.fone, ObOp.Opcional, 6, 14);
                
                    #endregion                    
                    break;

                case "E16A":
                    layout = "§E16a|indIEDest¨"; //ok
                    if ((double)NFe.infNFe.Versao >= 3.10)
                        NFe.dest.indIEDest = (TpcnindIEDest)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.indIEDest, ObOp.Obrigatorio, 1, 1);
                    break;

                case "E17":
                    layout = "§E17|IE¨"; //ok
                    NFe.dest.IE = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.IE, ObOp.Opcional, 2, 14);
                    break;

                case "E18A":
                    layout = "§E18A|IM¨"; //ok
                    NFe.dest.IM = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.IM, ObOp.Opcional, 1, 15);
                    break;

                case "F":
                    layout = "§F|xLgr¨|nro¨|xCpl¨|xBairro¨|cMun¨|xMun¨|UF¨"; //ok
                    ///
                    /// Grupo da TAG <retirada> 
                    /// 
                    #region <retirada> 
                    NFe.retirada.xLgr   = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xLgr, ObOp.Obrigatorio, 2, 60);
                    NFe.retirada.nro    = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nro, ObOp.Obrigatorio, 1, 60);
                    NFe.retirada.xCpl   = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xCpl, ObOp.Opcional, 1, 60);
                    NFe.retirada.xBairro = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xBairro, ObOp.Obrigatorio, 1, 60);
                    NFe.retirada.cMun   = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.cMun, ObOp.Obrigatorio, 7, 7);
                    NFe.retirada.xMun   = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xMun, ObOp.Obrigatorio, 2, 60);
                    NFe.retirada.UF     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.UF, ObOp.Obrigatorio, 2, 2);
                
                    #endregion                    
                    break;

                case "F02": 
                    layout = "§F02|CNPJ¨"; //ok
                    NFe.retirada.CNPJ = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CNPJ, ObOp.Obrigatorio, 14, 14);
                    break;

                case "F02A":    
                    layout = "§F02a|CPF¨"; //ok
                    NFe.retirada.CPF = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CPF, ObOp.Obrigatorio, 11, 11);
                    break;

                case "G":   
                    layout = "§G|xLgr¨|nro¨|xCpl¨|xBairro¨|cMun¨|xMun¨|UF¨"; //ok
                    ///
                    /// Grupo da TAG <entrega>
                    /// 
                    #region <entrega>
                    NFe.entrega.xLgr    = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xLgr, ObOp.Obrigatorio, 2, 60);
                    NFe.entrega.nro     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nro, ObOp.Obrigatorio, 1, 60);
                    NFe.entrega.xCpl    = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xCpl, ObOp.Opcional, 1, 60);
                    NFe.entrega.xBairro = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xBairro, ObOp.Obrigatorio, 1, 60);
                    NFe.entrega.cMun    = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.cMun, ObOp.Obrigatorio, 7, 7);
                    NFe.entrega.xMun    = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xMun, ObOp.Obrigatorio, 2, 60);
                    NFe.entrega.UF      = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.UF, ObOp.Obrigatorio, 2, 2);
                
                    #endregion                    
                    break;

                case "G02": 
                    layout = "§G02|CNPJ¨"; //ok
                    NFe.entrega.CNPJ = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CNPJ, ObOp.Obrigatorio, 14, 14);
                    break;

                case "G02A":    
                    layout = "§G02a|CPF¨"; //ok
                    NFe.entrega.CPF = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CPF, ObOp.Obrigatorio, 11, 11);
                    break;

                case "G51":
                    layout = "§G51|CNPJ¨"; //ok
                    NFe.autXML.Add(new autXML{ CNPJ = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CNPJ, ObOp.Obrigatorio, 14, 14) });
                    break;

                case "G52":
                    layout = "§G52|CPF¨"; //ok
                    NFe.autXML.Add(new autXML{ CPF = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CPF, ObOp.Obrigatorio, 11, 11) });
                    break;

                ///
                /// Grupo da TAG <det>
                /// 
                case "H":   
                    layout = "§H|NItem¨|InfAdProd¨"; //ok

                    NFe.det.Add(new Det());
                    nProd = NFe.det.Count - 1;
                    NFe.det[nProd].Prod.nItem = (int)this.LerCampo(TpcnTipoCampo.tcInt, "NItem", ObOp.Obrigatorio, 1, 3);
                    NFe.det[nProd].infAdProd = (string)this.LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.infAdProd, ObOp.Opcional, 0, 500);
                    break;

                case "I":
                    layout = "§I|CProd¨|CEAN¨|XProd¨|NCM¨|EXTIPI¨|CFOP¨|UCom¨|QCom¨|VUnCom¨|VProd¨|CEANTrib¨|UTrib¨|QTrib¨|VUnTrib¨|VFrete¨|VSeg¨|VDesc¨|vOutro¨|indTot¨|xPed¨|nItemPed¨|nFCI¨"; //ok
                    ///
                    /// Grupo da TAG <det><prod>
                    /// 
                    #region <det><prod>

                    NFe.det[nProd].Prod.cProd   = (string)this.LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.cProd, ObOp.Obrigatorio, 1, 60);
                    NFe.det[nProd].Prod.cEAN    = (string)this.LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.cEAN, ObOp.Obrigatorio, 0, 14);
                    NFe.det[nProd].Prod.xProd   = (string)this.LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xProd, ObOp.Obrigatorio, 1, 120);
                    NFe.det[nProd].Prod.NCM     = (string)this.LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.NCM, ObOp.Obrigatorio, 2, 8);
                    NFe.det[nProd].Prod.EXTIPI  = (string)this.LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.EXTIPI, ObOp.Opcional, 2, 3);
                    NFe.det[nProd].Prod.CFOP    = (string)this.LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CFOP, ObOp.Obrigatorio, 4, 4);
                    NFe.det[nProd].Prod.uCom    = (string)this.LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.uCom, ObOp.Obrigatorio, 1, 6);
                    NFe.det[nProd].Prod.qCom    =         this.LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.qCom, ObOp.Obrigatorio, 11);
                    NFe.det[nProd].Prod.vUnCom  =         this.LerCampo(TpcnTipoCampo.tcDec10, Properties.Resources.vUnCom, ObOp.Opcional, 21);
                    NFe.det[nProd].Prod.vProd   =         this.LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vProd, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Prod.cEANTrib= (string)this.LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.cEANTrib, ObOp.Obrigatorio, 0, 14);
                    NFe.det[nProd].Prod.uTrib   = (string)this.LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.uTrib, ObOp.Obrigatorio, 1, 6);
                    NFe.det[nProd].Prod.qTrib   =         this.LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.qTrib, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Prod.vUnTrib =         this.LerCampo(TpcnTipoCampo.tcDec10, Properties.Resources.vUnTrib, ObOp.Obrigatorio, 21);
                    NFe.det[nProd].Prod.vFrete  =         this.LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vFrete, ObOp.Opcional, 15);
                    NFe.det[nProd].Prod.vSeg    =         this.LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vSeg, ObOp.Opcional, 15);
                    NFe.det[nProd].Prod.vDesc   =         this.LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vDesc, ObOp.Opcional, 15);
                    NFe.det[nProd].Prod.vOutro  =         this.LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vOutro, ObOp.Opcional, 15);
                    NFe.det[nProd].Prod.indTot  = (TpcnIndicadorTotal)this.LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.indTot, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Prod.xPed    = (string)this.LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xPed, ObOp.Opcional, 1, 15);
                    NFe.det[nProd].Prod.nItemPed= (int)   this.LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.nItemPed, ObOp.Opcional, 0, 6);
                    NFe.det[nProd].Prod.nFCI    = (string)this.LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nFCI, ObOp.Opcional, 0, 255);
                    NFe.det[nProd].Imposto.ISSQN.cSitTrib = string.Empty;
                    
                    #endregion                    
                    break;

                case "105A":
                    layout = "§105A|NVE¨"; //ok
                    NFe.det[nProd].Prod.NVE = (string)this.LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.NVE, ObOp.Opcional, 0, 6);
                    break;

                case "I18":
                    layout = "§I18|NDI¨|DDI¨|XLocDesemb¨|UFDesemb¨|DDesemb¨|CExportador¨"; //ok
                    ///
                    /// Grupo da TAG <det><prod><DI>
                    /// 
                    #region <det><prod><DI>

                    DI diItem = new DI();

                    diItem.nDI          = (string)  LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nDI, ObOp.Obrigatorio, 1, 12);
                    diItem.dDI          = (DateTime)LerCampo(TpcnTipoCampo.tcDat, Properties.Resources.dDI, ObOp.Obrigatorio, 10, 10);
                    diItem.xLocDesemb   = (string)  LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xLocDesemb, ObOp.Obrigatorio, 1, 60);
                    diItem.UFDesemb     = (string)  LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.UFDesemb, ObOp.Obrigatorio, 2, 2);
                    diItem.dDesemb      = (DateTime)LerCampo(TpcnTipoCampo.tcDat, Properties.Resources.dDesemb, ObOp.Obrigatorio, 10, 10);
                    diItem.cExportador  = (string)  LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.cExportador, ObOp.Obrigatorio, 1, 60);

                    NFe.det[nProd].Prod.DI.Add(diItem);
                    #endregion
                    break;

                case "I23A":
                    layout = "§I23a|tpViaTransp¨|vAFRMM¨|tpIntermedio¨|CNPJ¨|UFTerceiro¨"; //ok
                    var dii = NFe.det[nProd].Prod.DI[NFe.det[nProd].Prod.DI.Count - 1];
                    if (dii != null)
                    {
                        dii.tpViaTransp = (TpcnTipoViaTransp)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.tpViaTransp, ObOp.Opcional, 1, 2);
                        dii.vAFRMM = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vAFRMM, ObOp.Opcional, 15);
                        dii.tpIntermedio = (TpcnTipoIntermedio)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.tpIntermedio, ObOp.Opcional, 1, 1);
                        dii.CNPJ = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CNPJ, ObOp.Opcional, 14, 14);
                        dii.UFTerceiro = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.UFTerceiro, ObOp.Opcional, 2, 2);
                    }
                    break;

                case "I25": 
                    layout = "§I25|NAdicao¨|NSeqAdic¨|CFabricante¨|VDescDI¨"; //ok
                    ///
                    /// Grupo da TAG <det><prod><DI><adi> 
                    /// 
                    #region <det><prod><DI><adi>

                    Adi adiItem = new Adi();

                    adiItem.nAdicao     = (int)   LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.nAdicao, ObOp.Obrigatorio, 1, 3);
                    adiItem.nSeqAdi     = (int)   LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.nSeqAdic, ObOp.Obrigatorio, 1, 3);
                    adiItem.cFabricante = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.cFabricante, ObOp.Obrigatorio, 1, 60);
                    adiItem.vDescDI     =         LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vDescDI, ObOp.Opcional, 15);

                    NFe.det[nProd].Prod.DI[NFe.det[nProd].Prod.DI.Count-1].adi.Add(adiItem);
                    #endregion
                    break;

                case "I29A":
                    layout = "§I29A|nDraw¨"; //ok
                    #region <det><prod><DI><adi>
                    {
                        int l = NFe.det[nProd].Prod.DI[NFe.det[nProd].Prod.DI.Count - 1].adi.Count - 1;
                        var item = NFe.det[nProd].Prod.DI[NFe.det[nProd].Prod.DI.Count - 1].adi[l];

                        item.nDraw = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nDraw, ObOp.Opcional, 0, 11);
                    }
                    #endregion
                    break;

                case "I50":
                    layout = "§I50|nDraw¨";//|nRE¨|chNFe¨|qExport¨"; //ok
                    #region <det><prod><detExport>
                    NFe.det[nProd].Prod.detExport.Add(new detExport { nDraw = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nDraw, ObOp.Opcional, 0, 11) });
                    //NFe.det[nProd].Prod.detExport[NFe.det[nProd].Prod.detExport.Count - 1].exportInd.nRE = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nRE, ObOp.Opcional, 12, 12);
                    //NFe.det[nProd].Prod.detExport[NFe.det[nProd].Prod.detExport.Count - 1].exportInd.chNFe = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.chNFe, ObOp.Opcional, 44, 44);
                    //NFe.det[nProd].Prod.detExport[NFe.det[nProd].Prod.detExport.Count - 1].exportInd.qExport = (double)LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.qExport, ObOp.Opcional, 0, 6);
                    #endregion
                    break;

                case "I52":
                    layout = "§I52|nRE¨|chNFe¨|qExport¨"; //ok
                    #region <det><prod><detExport><exportInd>
                    //NFe.det[nProd].Prod.detExport.Add(new detExport { nDraw = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nDraw, ObOp.Opcional, 0, 11) });
                    NFe.det[nProd].Prod.detExport[NFe.det[nProd].Prod.detExport.Count - 1].exportInd.nRE = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nRE, ObOp.Obrigatorio, 12, 12);
                    NFe.det[nProd].Prod.detExport[NFe.det[nProd].Prod.detExport.Count - 1].exportInd.chNFe = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.chNFe, ObOp.Obrigatorio, 44, 44);
                    NFe.det[nProd].Prod.detExport[NFe.det[nProd].Prod.detExport.Count - 1].exportInd.qExport = (double)LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.qExport, ObOp.Obrigatorio, 1, 15);
                    #endregion
                    break;

                case "J":   
                    layout = "§J|TpOp¨|Chassi¨|CCor¨|XCor¨|Pot¨|cilin¨|pesoL¨|pesoB¨|NSerie¨|TpComb¨|NMotor¨|CMT¨|Dist¨|anoMod¨|anoFab¨|tpPint¨|tpVeic¨|espVeic¨|VIN¨|condVeic¨|cMod¨|cCorDENATRAN¨|lota¨|tpRest¨"; //ok
                    ///
                    /// Grupo da TAG <det><prod><veicProd>
                    /// 
                    #region <det><prod><veicProd>

                    NFe.det[nProd].Prod.veicProd.tpOp   = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.tpOp, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Prod.veicProd.chassi = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.chassi, ObOp.Obrigatorio, 17, 17);
                    NFe.det[nProd].Prod.veicProd.cCor   = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.cCor, ObOp.Obrigatorio, 4, 4);
                    NFe.det[nProd].Prod.veicProd.xCor   = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xCor, ObOp.Obrigatorio, 1, 40);
                    NFe.det[nProd].Prod.veicProd.pot    = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.pot, ObOp.Obrigatorio, 4, 4);
                    NFe.det[nProd].Prod.veicProd.cilin  = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.cilin, ObOp.Obrigatorio, 4, 4);
                    NFe.det[nProd].Prod.veicProd.pesoL  = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.pesoL, ObOp.Obrigatorio, 1, 9);
                    NFe.det[nProd].Prod.veicProd.pesoB  = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.pesoB, ObOp.Obrigatorio, 1, 9);
                    NFe.det[nProd].Prod.veicProd.nSerie = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nSerie, ObOp.Obrigatorio, 9, 9);
                    NFe.det[nProd].Prod.veicProd.tpComb = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.tpComb, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Prod.veicProd.nMotor = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nMotor, ObOp.Obrigatorio, 21, 21);
                    NFe.det[nProd].Prod.veicProd.CMT    = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CMT, ObOp.Obrigatorio, 9, 9);
                    NFe.det[nProd].Prod.veicProd.dist   = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.dist, ObOp.Obrigatorio, 4, 4);
                    NFe.det[nProd].Prod.veicProd.anoMod = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.anoMod, ObOp.Obrigatorio, 4, 4);
                    NFe.det[nProd].Prod.veicProd.anoFab = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.anoFab, ObOp.Obrigatorio, 4, 4);
                    NFe.det[nProd].Prod.veicProd.tpPint = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.tpPint, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Prod.veicProd.tpVeic = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.tpVeic, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Prod.veicProd.espVeic= (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.espVeic, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Prod.veicProd.VIN    = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.VIN, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Prod.veicProd.condVeic = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.condVeic, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Prod.veicProd.cMod   = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.cMod, ObOp.Obrigatorio, 6, 6);
                    NFe.det[nProd].Prod.veicProd.cCorDENATRAN = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.cCorDENATRAN, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Prod.veicProd.lota   = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.lota, ObOp.Obrigatorio, 1, 3);
                    NFe.det[nProd].Prod.veicProd.tpRest = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.tpRest, ObOp.Obrigatorio, 1, 1);
                    
                    #endregion
                    break;

                case "K":
                    layout = "§K|NLote¨|QLote¨|DFab¨|DVal¨|VPMC¨"; //ok

                    ///
                    /// Grupo da TAG <det><prod><med>
                    /// 
                    #region <det><prod><med>
                    Med medItem = new Med();

                    medItem.nLote = (string) LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nLote, ObOp.Obrigatorio, 1, 20);
                    medItem.qLote =          LerCampo(TpcnTipoCampo.tcDec3, Properties.Resources.qLote, ObOp.Obrigatorio, 11);
                    medItem.dFab = (DateTime)LerCampo(TpcnTipoCampo.tcDat, Properties.Resources.dFab, ObOp.Obrigatorio, 10, 10);
                    medItem.dVal = (DateTime)LerCampo(TpcnTipoCampo.tcDat, Properties.Resources.dVal, ObOp.Obrigatorio, 10, 10);
                    medItem.vPMC =           LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vPMC, ObOp.Obrigatorio, 15);

                    NFe.det[nProd].Prod.med.Add(medItem);
                    #endregion
                    break;

                case "L":
                    layout = "§L|TpArma¨|NSerie¨|NCano¨|Descr¨"; //ok
                    ///
                    /// Grupo da TAG <det><prod><arma>
                    /// 
                    #region <det><prod><arma>
                    Arma armaItem = new Arma();

                    armaItem.tpArma = (TpcnTipoArma)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.tpArma, ObOp.Obrigatorio, 1, 1);
                    armaItem.nSerie =          (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.nSerie, ObOp.Obrigatorio, 1, (double)NFe.infNFe.Versao>=3.10 ? 15 : 9);
                    armaItem.nCano = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.nCano, ObOp.Obrigatorio, 1, (double)NFe.infNFe.Versao >= 3.10 ? 15 : 9);
                    armaItem.descr  =       (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.descr, ObOp.Obrigatorio, 1, 256);

                    NFe.det[nProd].Prod.arma.Add(armaItem);
                    #endregion
                    break;

                case "L01":
                    layout = "§L01|CProdANP¨|CODIF¨|QTemp¨|UFCons¨"; //ok
                    ///
                    /// Grupo da TAG <det><prod><comb>
                    /// 
                    #region <det><prod><comb>
                    NFe.det[nProd].Prod.comb.cProdANP = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.cProdANP, ObOp.Obrigatorio, 9, 9);
                    NFe.det[nProd].Prod.comb.CODIF  = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CODIF, ObOp.Opcional, 0, 21);
                    NFe.det[nProd].Prod.comb.qTemp  = LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.qTemp, ObOp.Opcional, 16);
                    NFe.det[nProd].Prod.comb.UFCons = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.UFCons, ObOp.Obrigatorio, 2, 2);
                    #endregion
                    break;

                case "L102A":
                    layout = "§L102a|pMixGN¨"; //ok
                    NFe.det[nProd].Prod.comb.pMixGN = LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.pMixGN, ObOp.Opcional, 6);
                    break;

                case "L105":
                    layout = "§L105|QBCProd¨|VAliqProd¨|VCIDE¨"; //ok
                    ///
                    /// Grupo da TAG <det><prod><comb><CIDE>
                    /// 
                    #region <det><prod><comb><CIDE>
                    NFe.det[nProd].Prod.comb.CIDE.qBCprod = LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.qBCProd, ObOp.Obrigatorio, 16);
                    NFe.det[nProd].Prod.comb.CIDE.vAliqProd = LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.vAliqProd, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Prod.comb.CIDE.vCIDE = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vCIDE, ObOp.Obrigatorio, 15);
                    #endregion
                    break;

                case "L109":
                    layout = "§L109|nRECOPI¨"; //ok
                    NFe.det[nProd].Prod.nRECOPI = (string)this.LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nRECOPI, ObOp.Opcional, 20, 20);
                    break;
#if lixo
                case "L109":
                    layout2.Add("L109", "§L109|VBCICMS¨|VICMS¨|VBCICMSST¨|VICMSST¨"); //ok
                    ///
                    /// Grupo da TAG <det><prod><comb><ICMSComb>
                    /// 
                    NFe.det[nProd].Prod.comb.ICMS.vBCICMS  = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCICMS);
                    NFe.det[nProd].Prod.comb.ICMS.vICMS = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMS);
                    NFe.det[nProd].Prod.comb.ICMS.vBCICMSST =LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCICMSST);
                    NFe.det[nProd].Prod.comb.ICMS.vICMSST = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSST);
                    break;

                case "L114":
                    layout2.Add("L114", "§L114|VBCICMSSTDest¨|VICMSSTDest¨"); //ok
                    ///
                    /// Grupo da TAG <det><prod><comb><ICMSInter>
                    /// 
                    NFe.det[nProd].Prod.comb.ICMSInter.vBCICMSSTDest = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCICMSSTDest);
                    NFe.det[nProd].Prod.comb.ICMSInter.vICMSSTDest = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSSTDest);
                    break;

                case "L117":
                    layout2.Add("L117", "§L117|VBCICMSSTCons¨|VICMSSTCons¨|UFCons¨"); //ok
                    ///
                    /// Grupo da TAG <det><prod><comb><ICMSCons>
                    /// 
                    NFe.det[nProd].Prod.comb.ICMSCons.vBCICMSSTCons = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCICMSSTCons);
                    NFe.det[nProd].Prod.comb.ICMSCons.vICMSSTCons = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSSTCons);
                    NFe.det[nProd].Prod.comb.ICMSCons.UFcons = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.UFCons);
                    break;
#endif

                case "M":
                    if (aRegistro.Split(new char[] { '|' }).Length > 0)
                    {
                        layout = "§M|vTotTrib¨"; //ok   
                        NFe.det[nProd].Imposto.vTotTrib = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vTotTrib, ObOp.Opcional, 15);
                    }
                    break;

                case "M02":
                    layout = "§M02|vTotTrib¨"; //ok   
                    NFe.det[nProd].Imposto.vTotTrib = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vTotTrib, ObOp.Opcional, 15);
                    break;

                ///
                /// Grupo da TAG <det><imposto><ICMS>
                /// 
                case "N02": 
                    layout = "§N02|Orig¨|CST¨|ModBC¨|VBC¨|PICMS¨|VICMS¨"; //ok   
                    #region ICMS00

                    NFe.det[nProd].Imposto.ICMS.orig    = (TpcnOrigemMercadoria)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.orig, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.CST     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Imposto.ICMS.modBC   = (TpcnDeterminacaoBaseIcms)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.modBC, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.vBC     = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.pICMS = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pICMS, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vICMS   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMS, ObOp.Obrigatorio, 15);

                    #endregion
                    break;

                case "N03": 
                    layout = "§N03|Orig¨|CST¨|ModBC¨|VBC¨|PICMS¨|VICMS¨|ModBCST¨|PMVAST¨|PRedBCST¨|VBCST¨|PICMSST¨|VICMSST¨"; //ok
                    #region ICMS10

                    NFe.det[nProd].Imposto.ICMS.ICMSPart10 = 0;
                    NFe.det[nProd].Imposto.ICMS.orig    = (TpcnOrigemMercadoria)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.orig, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.CST     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Imposto.ICMS.modBC   = (TpcnDeterminacaoBaseIcms)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.modBC, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.vBC     = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.pICMS = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pICMS, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vICMS   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMS, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.modBCST = (TpcnDeterminacaoBaseIcmsST)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.modBCST, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.pMVAST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pMVAST, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.pRedBCST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pRedBCST, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vBCST   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCST, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.pICMSST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pICMSST, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vICMSST = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSST, ObOp.Obrigatorio, 15);

                    #endregion
                    break;

                case "N04":
                    layout = "§N04|Orig¨|CST¨|ModBC¨|PRedBC¨|VBC¨|PICMS¨|VICMS¨|motDesICMS¨|vICMSDeson¨"; //ok
                    #region ICMS20

                    NFe.det[nProd].Imposto.ICMS.orig = (TpcnOrigemMercadoria)this.LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.orig, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.CST = (string)this.LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Imposto.ICMS.modBC = (TpcnDeterminacaoBaseIcms)this.LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.modBC, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.pRedBC = this.LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pRedBC, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vBC = this.LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.pICMS = this.LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pICMS, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vICMS = this.LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMS, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.motDesICMS = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.motDesICMS, ObOp.Opcional, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.vICMSDeson = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSDeson, ObOp.Opcional, 15);

                    #endregion
                    break;

                case "N05": 
                    layout = "§N05|Orig¨|CST¨|ModBCST¨|PMVAST¨|PRedBCST¨|VBCST¨|PICMSST¨|VICMSST¨|motDesICMS¨|vICMSDeson¨"; //ok

                    #region ICMS30

                    NFe.det[nProd].Imposto.ICMS.orig    = (TpcnOrigemMercadoria)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.orig, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.CST     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Imposto.ICMS.modBCST = (TpcnDeterminacaoBaseIcmsST)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.modBCST, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.pMVAST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pMVAST, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.pRedBCST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pRedBCST, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vBCST   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCST, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.pICMSST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pICMSST, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vICMSST = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSST, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.motDesICMS = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.motDesICMS, ObOp.Opcional, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.vICMSDeson = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSDeson, ObOp.Opcional, 15);

                    #endregion
                    break;

                case "N06": 
                    layout = "§N06|Orig¨|CST¨|vICMS¨|motDesICMS¨|vICMSDeson¨"; //ok

                    #region ICMS40, ICMS41 ICMS50

                    NFe.det[nProd].Imposto.ICMS.orig    = (TpcnOrigemMercadoria)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.orig, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.CST     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Imposto.ICMS.vICMS   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMS, ObOp.Opcional, 15);
                    if (NFe.det[nProd].Imposto.ICMS.vICMS > 0)
                        NFe.det[nProd].Imposto.ICMS.motDesICMS = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.motDesICMS, ObOp.Opcional, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.vICMSDeson = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSDeson, ObOp.Opcional, 15);

                    #endregion
                    break;

                case "N07": 
                    layout = "§N07|Orig¨|CST¨|ModBC¨|PRedBC¨|VBC¨|PICMS¨|VICMS¨|vICMSOp¨|pDif¨|vICMSDif¨|"; //ok

                    #region ICMS51

                    NFe.det[nProd].Imposto.ICMS.orig    = (TpcnOrigemMercadoria)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.orig, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.CST     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Imposto.ICMS.modBC   = (TpcnDeterminacaoBaseIcms)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.modBC, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.pRedBC = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pRedBC, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vBC     = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Opcional, 15);
                    NFe.det[nProd].Imposto.ICMS.pICMS   = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pICMS, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vICMS   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMS, ObOp.Opcional, 15);
                    NFe.det[nProd].Imposto.ICMS.vICMSOp = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSOp, ObOp.Opcional, 15);
                    NFe.det[nProd].Imposto.ICMS.pDif    = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pDif, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vICMSDif = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSDif, ObOp.Opcional, 15);

                    #endregion
                    break;

                case "N08": 
                    layout = "§N08|Orig¨|CST¨|vBCSTRet¨|vICMSSTRet¨"; //ok

                    #region ICMS60

                    NFe.det[nProd].Imposto.ICMS.orig        = (TpcnOrigemMercadoria)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.orig, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.CST         = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Imposto.ICMS.vBCSTRet    = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCSTRet, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.vICMSSTRet  = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSSTRet, ObOp.Obrigatorio, 15);
                    
                    #endregion
                    break;

                case "N09": 
                    layout = "§N09|Orig¨|CST¨|ModBC¨|PRedBC¨|VBC¨|PICMS¨|VICMS¨|ModBCST¨|PMVAST¨|PRedBCST¨|VBCST¨|PICMSST¨|VICMSST¨|motDesICMS¨|vICMSDeson¨"; //ok

                    #region ICMS70

                    NFe.det[nProd].Imposto.ICMS.orig    = (TpcnOrigemMercadoria)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.orig, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.CST     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Imposto.ICMS.modBC   = (TpcnDeterminacaoBaseIcms)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.modBC, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.pRedBC = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pRedBC, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vBC     = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.pICMS = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pICMS, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vICMS   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMS, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.modBCST = (TpcnDeterminacaoBaseIcmsST)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.modBCST, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.pMVAST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pMVAST, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.pRedBCST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pRedBCST, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vBCST   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCST, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.pICMSST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pICMSST, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vICMSST = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSST, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.motDesICMS = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.motDesICMS, ObOp.Opcional, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.vICMSDeson = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSDeson, ObOp.Opcional, 15);

                    #endregion
                    break;

                case "N10":
                    layout = "§N10|Orig¨|CST¨|ModBC¨|PRedBC¨|VBC¨|PICMS¨|VICMS¨|ModBCST¨|PMVAST¨|PRedBCST¨|VBCST¨|PICMSST¨|VICMSST¨|motDesICMS¨|vICMSDeson¨"; //ok

                    #region ICMS90

                    NFe.det[nProd].Imposto.ICMS.ICMSPart90 = 0;
                    NFe.det[nProd].Imposto.ICMS.orig    = (TpcnOrigemMercadoria)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.orig, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.CST     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Imposto.ICMS.modBC   = (TpcnDeterminacaoBaseIcms)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.modBC, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.pRedBC = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pRedBC, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vBC     = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.pICMS = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pICMS, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vICMS   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMS, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.modBCST = (TpcnDeterminacaoBaseIcmsST)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.modBCST, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.pMVAST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pMVAST, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.pRedBCST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pRedBCST, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vBCST   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCST, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.pICMSST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pICMSST, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vICMSST = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSST, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.motDesICMS = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.motDesICMS, ObOp.Opcional, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.vICMSDeson = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSDeson, ObOp.Opcional, 15);

                    #endregion                    
                    break;

                case "N10A":    
                    layout = "§N10a|Orig¨|CST¨|ModBC¨|PRedBC¨|VBC¨|PICMS¨|VICMS¨|ModBCST¨|PMVAST¨|PRedBCST¨|VBCST¨|PICMSST¨|VICMSST¨|pBCOp¨|UFST¨";

                    #region ICMSPart-10, ICMSPart-90

                    NFe.det[nProd].Imposto.ICMS.orig    = (TpcnOrigemMercadoria)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.orig, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.CST     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Imposto.ICMS.modBC   = (TpcnDeterminacaoBaseIcms)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.modBC, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.pRedBC = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pRedBC, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vBC     = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.pICMS = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pICMS, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vICMS   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMS, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.modBCST = (TpcnDeterminacaoBaseIcmsST)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.modBCST, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.pMVAST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pMVAST, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.pRedBCST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pRedBCST, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vBCST   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCST, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.pICMSST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pICMSST, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vICMSST = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSST, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.pBCOp = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pBCOp, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.UFST    = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.UFST, ObOp.Obrigatorio, 2, 2);
                    if (NFe.det[nProd].Imposto.ICMS.CST == "10")
                        NFe.det[nProd].Imposto.ICMS.ICMSPart10 = 1;
                    else
                        NFe.det[nProd].Imposto.ICMS.ICMSPart90 = 1;
                    
                    #endregion
                    break;

                case "N10B":    
                    layout = "§N10b|Orig¨|CST¨|vBCSTRet¨|vICMSSTRet¨|vBCSTDest¨|vICMSSTDest¨";

                    #region ICMS-ST

                    NFe.det[nProd].Imposto.ICMS.ICMSst = 1;
                    NFe.det[nProd].Imposto.ICMS.orig        = (TpcnOrigemMercadoria)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.orig, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.CST         = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Imposto.ICMS.vBCSTRet    = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCSTRet, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.vICMSSTRet  = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSSTRet, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.vBCSTDest   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCSTDest, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.vICMSSTDest = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSSTDest, ObOp.Obrigatorio, 15);
                    
                    #endregion
                    break;

                case "N10C":    
                    layout = "§N10c|Orig¨|CSOSN¨|pCredSN¨|vCredICMSSN¨";

                    #region ICMSSN101

                    NFe.det[nProd].Imposto.ICMS.orig        = (TpcnOrigemMercadoria)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.orig, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.CSOSN       = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.CSOSN, ObOp.Obrigatorio, 3, 3);
                    NFe.det[nProd].Imposto.ICMS.pCredSN = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pCredSN, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vCredICMSSN = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vCredICMSSN, ObOp.Obrigatorio, 15);

                    #endregion
                    break;

                case "N10D":    
                    layout = "§N10d|Orig¨|CSOSN¨";

                    #region ICMSSN102

                    NFe.det[nProd].Imposto.ICMS.orig  = (TpcnOrigemMercadoria)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.orig, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.CSOSN = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.CSOSN, ObOp.Obrigatorio, 3, 3);

                    #endregion
                    break;

                case "N10E":    
                    layout = "§N10e|Orig¨|CSOSN¨|modBCST¨|pMVAST¨|pRedBCST¨|vBCST¨|pICMSST¨|vICMSST¨|pCredSN¨|vCredICMSSN¨";

                    #region ICMSSN201

                    NFe.det[nProd].Imposto.ICMS.orig    = (TpcnOrigemMercadoria)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.orig, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.CSOSN   = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.CSOSN, ObOp.Obrigatorio, 3, 3);
                    NFe.det[nProd].Imposto.ICMS.modBCST = (TpcnDeterminacaoBaseIcmsST)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.modBCST, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.pMVAST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pMVAST, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.pRedBCST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pRedBCST, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vBCST   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCST, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.pICMSST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pICMSST, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vICMSST = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSST, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.pCredSN = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pCredSN, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vCredICMSSN = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vCredICMSSN, ObOp.Obrigatorio, 15);

                    #endregion
                    break;

                case "N10F":    
                    layout = "§N10f|Orig¨|CSOSN¨|modBCST¨|pMVAST¨|pRedBCST¨|vBCST¨|pICMSST¨|vICMSST¨";

                    #region ICMSSN202

                    NFe.det[nProd].Imposto.ICMS.orig    = (TpcnOrigemMercadoria)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.orig, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.CSOSN   = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.CSOSN, ObOp.Obrigatorio, 3, 3);
                    NFe.det[nProd].Imposto.ICMS.modBCST = (TpcnDeterminacaoBaseIcmsST)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.modBCST, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.pMVAST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pMVAST, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.pRedBCST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pRedBCST, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vBCST   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCST, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.pICMSST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pICMSST, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vICMSST = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSST, ObOp.Obrigatorio, 15);

                    #endregion
                    break;

                case "N10G":
                    layout = "§N10g|Orig¨|CSOSN¨|modBCST¨|vBCSTRet¨|vICMSSTRet¨";

                    #region ICMSSN500

                    NFe.det[nProd].Imposto.ICMS.orig    = (TpcnOrigemMercadoria)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.orig, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.CSOSN   = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.CSOSN, ObOp.Obrigatorio, 3, 3);
                    //modBCST nao existe no XML
                    NFe.det[nProd].Imposto.ICMS.modBCST = (TpcnDeterminacaoBaseIcmsST)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.modBCST, ObOp.Opcional, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.vBCSTRet = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCSTRet, ObOp.Opcional, 15);
                    NFe.det[nProd].Imposto.ICMS.vICMSSTRet = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSSTRet, ObOp.Opcional, 15);

                    #endregion
                    break;
                    
                case "N10H":    
                    layout = "§N10h|Orig¨|CSOSN¨|modBC¨|vBC¨|pRedBC¨|pICMS¨|vICMS¨|modBCST¨|pMVAST¨|pRedBCST¨|vBCST¨|pICMSST¨|vICMSST¨|pCredSN¨|vCredICMSSN¨";
                    
                    #region ICMSSN900

                    NFe.det[nProd].Imposto.ICMS.orig    = (TpcnOrigemMercadoria)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.orig, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.CSOSN   = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.CSOSN, ObOp.Obrigatorio, 3, 3);
                    NFe.det[nProd].Imposto.ICMS.modBC   = (TpcnDeterminacaoBaseIcms)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.modBC, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.vBC     = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.pRedBC = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pRedBC, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.pICMS = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pICMS, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vICMS   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMS, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.modBCST = (TpcnDeterminacaoBaseIcmsST)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.modBCST, ObOp.Obrigatorio, 1, 1);
                    NFe.det[nProd].Imposto.ICMS.pMVAST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pMVAST, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.pRedBCST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pRedBCST, ObOp.Opcional, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vBCST   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCST, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.pICMSST = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pICMSST, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vICMSST = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSST, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ICMS.pCredSN = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pCredSN, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ICMS.vCredICMSSN = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vCredICMSSN, ObOp.Obrigatorio, 15);

                    #endregion
                    break;

                case "O":   
                    layout = "§O|ClEnq¨|CNPJProd¨|CSelo¨|QSelo¨|CEnq¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><IPI>
                    /// 
                    //i := nProd;
                    #region <det><imposto><IPI>
                    NFe.det[nProd].Imposto.IPI.clEnq    = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.clEnq, ObOp.Opcional, 5, 5);
                    NFe.det[nProd].Imposto.IPI.CNPJProd = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CNPJProd, ObOp.Opcional, 14, 14);
                    NFe.det[nProd].Imposto.IPI.cSelo    = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.cSelo, ObOp.Opcional, 1, 60);
                    NFe.det[nProd].Imposto.IPI.qSelo    = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.qSelo, ObOp.Opcional, 1, 12);
                    NFe.det[nProd].Imposto.IPI.cEnq     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.cEnq, ObOp.Obrigatorio, 3, 3);
                
                    #endregion                    
                    break;

                case "O07": 
                    layout = "§O07|CST¨|VIPI¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><IPITrib>
                    /// 
                    #region <det><imposto><IPITrib>
                    NFe.det[nProd].Imposto.IPI.CST  = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Imposto.IPI.vIPI = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vIPI, ObOp.Obrigatorio, 15);
                
                    #endregion                    
                    break;

                case "O08": 
                    layout = "§O08|CST¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><IPINT>
                    /// 
                    NFe.det[nProd].Imposto.IPI.CST = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    break;

                case "O10": 
                    layout = "§O10|VBC¨|PIPI¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><IPI>
                    /// 
                    NFe.det[nProd].Imposto.IPI.vBC = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.IPI.pIPI = LerCampo(NFe.infNFe.Versao>3 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pIPI, ObOp.Obrigatorio, NFe.infNFe.Versao>3 ? 7 : 5);
                    break;

                case "O11": 
                    layout = "§O11|QUnid¨|VUnid¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><IPI>
                    /// 
                    NFe.det[nProd].Imposto.IPI.qUnid = LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.qUnid, ObOp.Obrigatorio, 16);
                    NFe.det[nProd].Imposto.IPI.vUnid = LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.vUnid, ObOp.Obrigatorio, 15);
                    break;

                case "P":   
                    layout = "§P|VBC¨|VDespAdu¨|VII¨|VIOF¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><II>
                    /// 
                    NFe.det[nProd].Imposto.II.vBC = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.II.vDespAdu = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vDespAdu, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.II.vII = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vII, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.II.vIOF = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vIOF, ObOp.Obrigatorio, 15);
                    break;

                case "Q02": 
                    layout = "§Q02|CST¨|VBC¨|PPIS¨|VPIS¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><pis><pisaliq>
                    /// 
                    NFe.det[nProd].Imposto.PIS.CST = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Imposto.PIS.vBC = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.PIS.pPIS = LerCampo(NFe.infNFe.Versao>3 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pPIS, ObOp.Obrigatorio, NFe.infNFe.Versao>3 ? 7 : 5);
                    NFe.det[nProd].Imposto.PIS.vPIS = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vPIS, ObOp.Obrigatorio, 15);
                    break;

                case "Q03": 
                    layout = "§Q03|CST¨|QBCProd¨|VAliqProd¨|VPIS¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><pis><pisqtde>
                    /// 
                    NFe.det[nProd].Imposto.PIS.CST = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Imposto.PIS.qBCProd = LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.qBCProd, ObOp.Obrigatorio, 16);
                    NFe.det[nProd].Imposto.PIS.vAliqProd = LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.vAliqProd, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.PIS.vPIS = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vPIS, ObOp.Obrigatorio, 15);
                    break;

                case "Q04": 
                    layout = "§Q04|CST¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><pis><pisNT>
                    /// 
                    NFe.det[nProd].Imposto.PIS.CST = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    break;

                case "Q05": 
                    layout = "§Q05|CST¨|VPIS¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><pis><pisOutr>
                    /// 
                    NFe.det[nProd].Imposto.PIS.CST = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Imposto.PIS.vPIS = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vPIS, ObOp.Obrigatorio, 15);
                    break;

                case "Q07": 
                    layout = "§Q07|VBC¨|PPIS¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><pis><pisqtde>
                    /// 
                    NFe.det[nProd].Imposto.PIS.vBC = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.PIS.pPIS = LerCampo(NFe.infNFe.Versao>3 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pPIS, ObOp.Obrigatorio, NFe.infNFe.Versao>3 ? 7 : 5);
                    break;

                case "Q10": 
                    layout = "§Q10|QBCProd¨|VAliqProd¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><pis><pisqtde>
                    /// 
                    NFe.det[nProd].Imposto.PIS.qBCProd = LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.qBCProd, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.PIS.vAliqProd = LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.vAliqProd, ObOp.Obrigatorio, 15);
                    break;

                case "R":   
                    layout = "§R|VPIS¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><pisST>
                    /// 
                    NFe.det[nProd].Imposto.PISST.vPIS = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vPIS, ObOp.Obrigatorio, 15);
                    break;

                case "R02": 
                    layout = "§R02|VBC¨|PPIS¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><pisST>
                    /// 
                    NFe.det[nProd].Imposto.PISST.vBC = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.PISST.pPis = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pPIS, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    break;

                case "R04": 
                    layout = "§R04|QBCProd¨|VAliqProd¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><pisST>
                    /// 
                    NFe.det[nProd].Imposto.PISST.qBCProd = LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.qBCProd, ObOp.Obrigatorio, 16);
                    NFe.det[nProd].Imposto.PISST.vAliqProd = LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.vAliqProd, ObOp.Obrigatorio, 15);
                    break;

                case "S02": 
                    layout = "§S02|CST¨|VBC¨|PCOFINS¨|VCOFINS¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><COFINS>
                    /// 
                    NFe.det[nProd].Imposto.COFINS.CST = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Imposto.COFINS.vBC = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.COFINS.pCOFINS = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pCOFINS, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.COFINS.vCOFINS = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vCOFINS, ObOp.Obrigatorio, 15);
                    break;

                case "S03":
                    layout = "§S03|CST¨|QBCProd¨|VAliqProd¨|VCOFINS¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><COFINSQtde>
                    /// 
                    NFe.det[nProd].Imposto.COFINS.CST = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Imposto.COFINS.qBCProd = LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.qBCProd, ObOp.Obrigatorio, 16);
                    NFe.det[nProd].Imposto.COFINS.vAliqProd = LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.vAliqProd, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.COFINS.vCOFINS = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vCOFINS, ObOp.Obrigatorio, 15);
                    break;

                case "S04":
                    layout = "§S04|CST¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><COFINSNT>
                    /// 
                    NFe.det[nProd].Imposto.COFINS.CST = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    break;

                case "S05":
                    layout = "§S05|CST¨|VCOFINS¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><COFINSOutr>
                    /// 
                    NFe.det[nProd].Imposto.COFINS.CST = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CST, ObOp.Obrigatorio, 2, 2);
                    NFe.det[nProd].Imposto.COFINS.vCOFINS = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vCOFINS, ObOp.Obrigatorio, 15);
                    break;

                case "S07":
                    layout = "§S07|VBC¨|PCOFINS¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><COFINSOutr>
                    /// 
                    NFe.det[nProd].Imposto.COFINS.vBC = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.COFINS.pCOFINS = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pCOFINS, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    break;

                case "S09":
                    layout = "§S09|QBCProd¨|VAliqProd¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><COFINSST>
                    /// 
                    NFe.det[nProd].Imposto.COFINS.qBCProd = LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.qBCProd, ObOp.Obrigatorio, 16);
                    NFe.det[nProd].Imposto.COFINS.vAliqProd = LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.vAliqProd, ObOp.Obrigatorio, 15);
                    break;

                case "T":
                    layout = "§T|VCOFINS¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><COFINSST>
                    /// 
                    NFe.det[nProd].Imposto.COFINSST.vCOFINS = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vCOFINS, ObOp.Obrigatorio, 15);
                    break;

                case "T02":
                    layout = "§T02|VBC¨|PCOFINS¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><COFINSST>
                    /// 
                    NFe.det[nProd].Imposto.COFINSST.vBC = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.COFINSST.pCOFINS = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pCOFINS, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    break;

                case "T04":
                    layout = "§T04|QBCProd¨|VAliqProd¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><COFINSST>
                    /// 
                    NFe.det[nProd].Imposto.COFINSST.qBCProd = LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.qBCProd, ObOp.Obrigatorio, 16);
                    NFe.det[nProd].Imposto.COFINSST.vAliqProd = LerCampo(TpcnTipoCampo.tcDec4, Properties.Resources.vAliqProd, ObOp.Obrigatorio, 15);
                    break;

                case "U":
                    layout = "§U|VBC¨|VAliq¨|VISSQN¨|CMunFG¨|CListServ¨|cSitTrib¨"; //ok
                    ///
                    /// Grupo da TAG <det><imposto><ISSQN>
                    /// 
                    NFe.det[nProd].Imposto.ISSQN.vBC    = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ISSQN.vAliq  = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.vAliq, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.det[nProd].Imposto.ISSQN.vISSQN = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vISSQN, ObOp.Obrigatorio, 15);
                    NFe.det[nProd].Imposto.ISSQN.cMunFG = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.cMunFG, ObOp.Obrigatorio, 7, 7);
                    if ((double)NFe.infNFe.Versao>=3.10)
                        NFe.det[nProd].Imposto.ISSQN.cListServ = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.cListServ, ObOp.Obrigatorio, 5, 5);
                    else
                        NFe.det[nProd].Imposto.ISSQN.cListServ = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.cListServ, ObOp.Obrigatorio, 3, 4);
                    NFe.det[nProd].Imposto.ISSQN.cSitTrib   = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.cSitTrib, ObOp.Obrigatorio, 1, 1);
                    break;

                case "U51":
                    layout = "§U51|pDevol¨"; //ok
                    NFe.det[nProd].impostoDevol.pDevol = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.pDevol, ObOp.Opcional, 5);
                    break;

                case "U61":
                    layout = "§U61|vIPIDevol¨"; //ok
                    NFe.det[nProd].impostoDevol.vIPIDevol = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vIPIDevol, ObOp.Opcional, 5);
                    break;

                case "W02":
                    layout = "§W02|VBC¨|VICMS¨|VBCST¨|VST¨|VProd¨|VFrete¨|VSeg¨|VDesc¨|VII¨|VIPI¨|VPIS¨|VCOFINS¨|VOutro¨|VNF¨|vTotTrib¨|vICMSDeson¨"; //ok
                    ///
                    /// Grupo da TAG <total><ICMSTot>
                    /// 
                    NFe.Total.ICMSTot.vBC   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Obrigatorio, 15);
                    NFe.Total.ICMSTot.vICMS = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMS, ObOp.Obrigatorio, 15);
                    NFe.Total.ICMSTot.vBCST = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCST, ObOp.Obrigatorio, 15);
                    NFe.Total.ICMSTot.vST   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vST, ObOp.Obrigatorio, 15);
                    NFe.Total.ICMSTot.vProd = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vProd, ObOp.Obrigatorio, 15);
                    NFe.Total.ICMSTot.vFrete = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vFrete, ObOp.Obrigatorio, 15);
                    NFe.Total.ICMSTot.vSeg  = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vSeg, ObOp.Obrigatorio, 15);
                    NFe.Total.ICMSTot.vDesc = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vDesc, ObOp.Obrigatorio, 15);
                    NFe.Total.ICMSTot.vII   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vII, ObOp.Obrigatorio, 15);
                    NFe.Total.ICMSTot.vIPI  = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vIPI, ObOp.Obrigatorio, 15);
                    NFe.Total.ICMSTot.vPIS  = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vPIS, ObOp.Obrigatorio, 15);
                    NFe.Total.ICMSTot.vCOFINS = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vCOFINS, ObOp.Obrigatorio, 15);
                    NFe.Total.ICMSTot.vOutro  = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vOutro, ObOp.Obrigatorio, 15);
                    NFe.Total.ICMSTot.vNF   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vNF, ObOp.Obrigatorio, 15);
                    NFe.Total.ICMSTot.vTotTrib = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vTotTrib, ObOp.Opcional, 15);
                    NFe.Total.ICMSTot.vICMSDeson = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSDeson, ObOp.Opcional, 15);
                    break;

                case "W17":
                    layout = "§W17|VServ¨|VBC¨|VISS¨|VPIS¨|VCOFINS¨|dCompet¨|vDeducao¨|vINSS¨|vIR¨|vCSLL¨|vOutro¨|vDescIncond¨|vDescCond¨|indISSRet¨|indISS¨|cServico¨|cMun¨|cPais¨|nProcesso¨|cRegTrib¨|indIncentivo¨|";
                    ///
                    /// Grupo da TAG <total><ISSQNtot>
                    /// 
                    NFe.Total.ISSQNtot.vServ    = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vServ, ObOp.Opcional, 15);
                    NFe.Total.ISSQNtot.vBC      = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Opcional, 15);
                    NFe.Total.ISSQNtot.vISS     = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vISS, ObOp.Opcional, 15);
                    NFe.Total.ISSQNtot.vPIS     = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vPIS, ObOp.Opcional, 15);
                    NFe.Total.ISSQNtot.vCOFINS  = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vCOFINS, ObOp.Opcional, 15);

                    NFe.Total.ISSQNtot.dCompet  = (DateTime)LerCampo(TpcnTipoCampo.tcDat2, Properties.Resources.dCompet, ObOp.Opcional, 8, 8);
                    NFe.Total.ISSQNtot.vDeducao = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vDeducao, ObOp.Opcional, 15);
                    NFe.Total.ISSQNtot.vISS     = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vISS, ObOp.Opcional, 15);
                    NFe.Total.ISSQNtot.vIR      = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vIR, ObOp.Opcional, 15);
                    NFe.Total.ISSQNtot.vCSLL    = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vCSLL, ObOp.Opcional, 15);
                    NFe.Total.ISSQNtot.vOutro   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vOutro, ObOp.Opcional, 15);
                    NFe.Total.ISSQNtot.vDescIncond = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vDescIncond, ObOp.Opcional, 15);
                    NFe.Total.ISSQNtot.vDescCond = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vDescCond, ObOp.Opcional, 15);
                    NFe.Total.ISSQNtot.indISSRet = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.indISSRet, ObOp.Opcional, 1,1)==1;
                    NFe.Total.ISSQNtot.indISS   = (TpcnindISS)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.indISS, ObOp.Opcional, 1);
                    NFe.Total.ISSQNtot.cServico = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.cServico, ObOp.Opcional, 1, 20);
                    NFe.Total.ISSQNtot.cMun     = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.cMun, ObOp.Opcional, 7, 7);
                    NFe.Total.ISSQNtot.cPais    = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.cPais, ObOp.Opcional, 4, 4);
                    NFe.Total.ISSQNtot.nProcesso = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nProcesso, ObOp.Opcional, 1, 30);
                    NFe.Total.ISSQNtot.cRegTrib = (TpcnRegimeTributario)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.cRegTrib, ObOp.Opcional, 2, 2);
                    NFe.Total.ISSQNtot.indIncentivo = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.indIncentivo, ObOp.Opcional, 1, 1) == 1;
                    break;

                case "W23":
                    layout = "§W23|VRetPIS¨|VRetCOFINS¨|VRetCSLL¨|VBCIRRF¨|VIRRF¨|VBCRetPrev¨|VRetPrev¨"; //ok
                    ///
                    /// Grupo da TAG <total><retTrib>
                    /// 
                    NFe.Total.retTrib.vRetPIS   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vRetPIS, ObOp.Opcional, 15);
                    NFe.Total.retTrib.vRetCOFINS = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vRetCOFINS, ObOp.Opcional, 15);
                    NFe.Total.retTrib.vRetCSLL  = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vRetCSLL, ObOp.Opcional, 15);
                    NFe.Total.retTrib.vBCIRRF   = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCIRRF, ObOp.Opcional, 15);
                    NFe.Total.retTrib.vIRRF     = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vIRRF, ObOp.Opcional, 15);
                    NFe.Total.retTrib.vBCRetPrev = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCRetPrev, ObOp.Opcional, 15);
                    NFe.Total.retTrib.vRetPrev  = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vRetPrev, ObOp.Opcional, 15);
                    break;

                case "X":
                    layout = "§X|ModFrete¨"; //ok
                    ///
                    /// Grupo da TAG <transp>
                    /// 
                    NFe.Transp.modFrete = (TpcnModalidadeFrete)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.modFrete, ObOp.Obrigatorio, 1, 1);
                    break;

                case "X03":
                    layout = "§X03|XNome¨|IE¨|XEnder¨|UF¨|XMun¨"; //ok
                    ///
                    /// Grupo da TAG <transp><TRansportadora>
                    /// 
                    NFe.Transp.Transporta.xNome = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xNome, ObOp.Opcional, 1, 60);
                    NFe.Transp.Transporta.IE    = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.IE, ObOp.Opcional, 0, 14);
                    NFe.Transp.Transporta.xEnder = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xEnder, ObOp.Opcional, 1, 60);
                    NFe.Transp.Transporta.xMun  = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xMun, ObOp.Opcional, 1, 60);
                    NFe.Transp.Transporta.UF    = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.UF, ObOp.Opcional, 2, 2);
                    break;

                case "X04":
                    layout = "§X04|CNPJ¨"; //ok

                    NFe.Transp.Transporta.CNPJ = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CNPJ, ObOp.Opcional, 14, 14);
                    break;

                case "X05":
                    layout = "§X05|CPF¨"; //ok

                    NFe.Transp.Transporta.CPF = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CPF, ObOp.Opcional, 11, 11);
                    break;

                case "X11":
                    layout = "§X11|VServ¨|VBCRet¨|PICMSRet¨|VICMSRet¨|CFOP¨|CMunFG¨"; //ok
                    ///
                    /// Grupo da TAG <transp><retTransp>
                    /// 
                    NFe.Transp.retTransp.vServ  = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vServ, ObOp.Obrigatorio, 15);
                    NFe.Transp.retTransp.vBCRet = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vBCRet, ObOp.Obrigatorio, 15);
                    NFe.Transp.retTransp.pICMSRet = LerCampo((double)NFe.infNFe.Versao >= 3.10 ? TpcnTipoCampo.tcDec4 : TpcnTipoCampo.tcDec2, Properties.Resources.pICMSRet, ObOp.Obrigatorio, (double)NFe.infNFe.Versao >= 3.10 ? 7 : 5);
                    NFe.Transp.retTransp.vICMSRet = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vICMSRet, ObOp.Obrigatorio, 15);
                    NFe.Transp.retTransp.CFOP   = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CFOP, ObOp.Obrigatorio, 4, 4);
                    NFe.Transp.retTransp.cMunFG = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.cMunFG, ObOp.Obrigatorio, 7, 7);
                    break;

                case "X18":
                    layout = "§X18|Placa¨|UF¨|RNTC¨"; //ok
                    ///
                    /// Grupo da TAG <transp><veicTransp>
                    /// 
                    NFe.Transp.veicTransp.placa = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.placa, ObOp.Obrigatorio, 1, 8);
                    NFe.Transp.veicTransp.UF    = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.UF, ObOp.Obrigatorio, 2, 2);
                    NFe.Transp.veicTransp.RNTC  = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.RNTC, ObOp.Opcional, 1, 20);
                    break;

                case "X22":
                    layout = "§X22|Placa¨|UF¨|RNTC¨"; //ok
                    ///
                    /// Grupo da TAG <transp><reboque>
                    /// 
                    NFe.Transp.Reboque.Add(new Reboque());
                    NFe.Transp.Reboque[NFe.Transp.Reboque.Count - 1].placa = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.placa, ObOp.Obrigatorio, 1, 8);
                    NFe.Transp.Reboque[NFe.Transp.Reboque.Count - 1].UF = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.UF, ObOp.Obrigatorio, 2, 2);
                    NFe.Transp.Reboque[NFe.Transp.Reboque.Count - 1].RNTC = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.RNTC, ObOp.Opcional, 1, 20);
                    break;

                case "X25a":
                    layout = "§X25a|vagao¨"; //ok
                    NFe.Transp.vagao = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.vagao, ObOp.Opcional, 1, 20);
                    break;

                case "X25b":
                    layout = "§X25b|balsa¨"; //ok
                    NFe.Transp.balsa = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.balsa, ObOp.Opcional, 1, 20);
                    break;

                case "X26":
                    layout = "§X26|QVol¨|Esp¨|Marca¨|NVol¨|PesoL¨|PesoB¨"; //ok
                    ///
                    /// Grupo da TAG <transp><vol>
                    /// 
                    NFe.Transp.Vol.Add(new Vol());
                    NFe.Transp.Vol[NFe.Transp.Vol.Count - 1].qVol = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.qVol, ObOp.Obrigatorio, 1, 15);
                    NFe.Transp.Vol[NFe.Transp.Vol.Count - 1].esp = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.esp, ObOp.Opcional, 1, 60);
                    NFe.Transp.Vol[NFe.Transp.Vol.Count - 1].marca = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.marca, ObOp.Opcional, 1, 60);
                    NFe.Transp.Vol[NFe.Transp.Vol.Count - 1].nVol = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nVol, ObOp.Opcional, 1, 60);
                    NFe.Transp.Vol[NFe.Transp.Vol.Count - 1].pesoL = LerCampo(TpcnTipoCampo.tcDec3, Properties.Resources.pesoL, ObOp.Opcional, 15);
                    NFe.Transp.Vol[NFe.Transp.Vol.Count - 1].pesoB = LerCampo(TpcnTipoCampo.tcDec3, Properties.Resources.pesoB, ObOp.Opcional, 15);
                    break;

                case "X33":
                    layout = "§X33|NLacre¨"; //ok
                    ///
                    /// Grupo da TAG <transp><vol><lacres>
                    /// 
                    Lacres lacreItem = new Lacres();
                    lacreItem.nLacre = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nLacre, ObOp.Obrigatorio, 1, 60);

                    NFe.Transp.Vol[NFe.Transp.Vol.Count - 1].Lacres.Add(lacreItem);
                    break;

                case "Y02":
                    layout = "§Y02|NFat¨|VOrig¨|VDesc¨|VLiq¨"; //ok
                    ///
                    /// Grupo da TAG <cobr>
                    /// 
                    NFe.Cobr.Fat.nFat = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nFat, ObOp.Opcional, 1, 60);
                    NFe.Cobr.Fat.vOrig = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vOrig, ObOp.Opcional, 15);
                    NFe.Cobr.Fat.vDesc = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vDesc, ObOp.Opcional, 15);
                    NFe.Cobr.Fat.vLiq = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vLiq, ObOp.Opcional, 15);
                    break;

                case "Y07":
                    layout = "§Y07|NDup¨|DVenc¨|VDup¨"; //ok
                    ///
                    /// Grupo da TAG <cobr><dup>
                    /// 
                    NFe.Cobr.Dup.Add(new Dup());
                    NFe.Cobr.Dup[NFe.Cobr.Dup.Count - 1].nDup = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nDup, ObOp.Opcional, 1, 60);
                    NFe.Cobr.Dup[NFe.Cobr.Dup.Count - 1].dVenc = (DateTime)LerCampo(TpcnTipoCampo.tcDat, Properties.Resources.dVenc, ObOp.Opcional, 10, 10);
                    NFe.Cobr.Dup[NFe.Cobr.Dup.Count - 1].vDup = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vDup, ObOp.Opcional, 15);
                    break;

                ///
                /// NFC-e
                /// 
                case "YA02":
                    layout = "§YA02|tPag¨";
                    NFe.pag.Add(new pag());
                    NFe.pag[NFe.pag.Count - 1].tPag = (TpcnFormaPagamento)LerCampo(TpcnTipoCampo.tcInt, "tPag", ObOp.Obrigatorio, 2);
                    break;
                case "YA03":
                    layout = "§YA03|vPag¨";
                    NFe.pag[NFe.pag.Count - 1].vPag = LerCampo(TpcnTipoCampo.tcDec2, "vPag", ObOp.Obrigatorio, 15);
                    break;
                case "YA05":
                    layout = "§YA05|CNPJ¨";
                    NFe.pag[NFe.pag.Count - 1].CNPJ = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.CNPJ, ObOp.Obrigatorio, 14, 14);
                    break;
                case "YA06":
                    layout = "§YA06|tBand¨";
                    NFe.pag[NFe.pag.Count - 1].tBand = (TpcnBandeiraCartao)LerCampo(TpcnTipoCampo.tcInt, "tBand", ObOp.Obrigatorio, 2);
                    break;
                case "YA07":
                    layout = "§YA07|cAut¨";
                    NFe.pag[NFe.pag.Count - 1].cAut = (string)LerCampo(TpcnTipoCampo.tcStr, "cAut", ObOp.Obrigatorio, 1, 15);
                    break;

                case "Z":
                    layout = "§Z|InfAdFisco¨|InfCpl¨"; //ok
                    ///
                    /// Grupo da TAG <InfAdic>
                    /// 
                    NFe.InfAdic.infAdFisco = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.infAdFisco, ObOp.Opcional, 1, 2000);
                    NFe.InfAdic.infCpl = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.infCpl, ObOp.Opcional, 1, 5000);
                    break;

                case "Z04":
                    layout = "§Z04|XCampo¨|XTexto¨"; //ok
                    ///
                    /// Grupo da TAG <infAdic><obsCont>
                    /// 
                    NFe.InfAdic.obsCont.Add(new obsCont());
                    NFe.InfAdic.obsCont[NFe.InfAdic.obsCont.Count - 1].xCampo = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xCampo, ObOp.Obrigatorio, 1, 20);
                    NFe.InfAdic.obsCont[NFe.InfAdic.obsCont.Count - 1].xTexto = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xTexto, ObOp.Obrigatorio, 1, 60);
                    break;

                case "Z07":
                    layout = "§Z07|XCampo¨|XTexto¨"; //ok - ?
                    ///
                    /// Grupo da TAG <infAdic><obsFisco>
                    /// 
                    NFe.InfAdic.obsFisco.Add(new obsFisco());
                    NFe.InfAdic.obsFisco[NFe.InfAdic.obsFisco.Count - 1].xCampo = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xCampo, ObOp.Obrigatorio, 1, 20);
                    NFe.InfAdic.obsFisco[NFe.InfAdic.obsFisco.Count - 1].xTexto = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xTexto, ObOp.Obrigatorio, 1, 60);
                    break;

                case "Z10":
                    layout = "§Z10|NProc¨|IndProc¨"; //ok
                    ///
                    /// Grupo da TAG <infAdic><procRef>
                    /// 
                    NFe.InfAdic.procRef.Add(new procRef());
                    NFe.InfAdic.procRef[NFe.InfAdic.procRef.Count - 1].nProc = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.nProc, ObOp.Obrigatorio, 1, 60);
                    NFe.InfAdic.procRef[NFe.InfAdic.procRef.Count - 1].indProc = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.indProc, ObOp.Obrigatorio, 1, 1);
                    break;

                case "ZA":
                    layout = "§ZA|UFEmbarq¨|XLocEmbarq¨"; //ok
                    ///
                    /// Grupo da TAG <exporta>
                    /// 
                    NFe.exporta.UFEmbarq = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.UFEmbarq, ObOp.Obrigatorio, 2, 2);
                    NFe.exporta.xLocEmbarq = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xLocEmbarq, ObOp.Obrigatorio, 1, 60);
                    break;

                case "ZA01":
                    if ((double)NFe.infNFe.Versao >= 3.10)
                    {
                        layout = "§ZA|UFSaidaPais¨|xLocExporta¨|xLocDespacho¨"; //ok
                        ///
                        /// Grupo da TAG <exporta>
                        /// 
                        NFe.exporta.UFSaidaPais = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.UFSaidaPais, ObOp.Obrigatorio, 2, 2);
                        NFe.exporta.xLocExporta = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xLocExporta, ObOp.Obrigatorio, 1, 60);
                        NFe.exporta.xLocDespacho = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xLocDespacho, ObOp.Obrigatorio, 1, 60);
                    }
                    break;

                case "ZB":
                    layout = "§ZB|XNEmp¨|XPed¨|XCont¨"; //ok
                    ///
                    /// Grupo da TAG <compra>
                    /// 
                    NFe.compra.xNEmp = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xNEmp, ObOp.Opcional, 1, 17);
                    NFe.compra.xPed = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xPed, ObOp.Opcional, 1, 60);
                    NFe.compra.xCont = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xCont, ObOp.Opcional, 1, 60);
                    break;

                case "ZC01":
                    layout = "§ZC01|safra¨|ref¨|qTotMes¨|qTotAnt¨|qTotGer¨|vFor¨|vTotDed¨|vLiqFor¨";
                    NFe.cana.safra = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.safra, ObOp.Obrigatorio, 4, 9);
                    NFe.cana.Ref = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.Ref, ObOp.Obrigatorio, 7, 7);
                    NFe.cana.qTotMes = LerCampo(TpcnTipoCampo.tcDec10, Properties.Resources.qTotMes, ObOp.Obrigatorio, 11);
                    NFe.cana.qTotAnt = LerCampo(TpcnTipoCampo.tcDec10, Properties.Resources.qTotAnt, ObOp.Obrigatorio, 11);
                    NFe.cana.qTotGer = LerCampo(TpcnTipoCampo.tcDec10, Properties.Resources.qTotGer, ObOp.Obrigatorio, 11);
                    NFe.cana.vFor = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vFor, ObOp.Obrigatorio, 15);
                    NFe.cana.vTotDed = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vTotDed, ObOp.Obrigatorio, 15);
                    NFe.cana.vLiqFor = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vLiqFor, ObOp.Obrigatorio, 15);
                    break;

                case "ZC04":
                    layout = "§ZC04|dia¨|qtde¨";
                    NFe.cana.fordia.Add(new fordia());
                    NFe.cana.fordia[NFe.cana.fordia.Count - 1].dia = (int)LerCampo(TpcnTipoCampo.tcInt, Properties.Resources.dia, ObOp.Obrigatorio, 1, 2);
                    NFe.cana.fordia[NFe.cana.fordia.Count - 1].qtde = LerCampo(TpcnTipoCampo.tcDec10, Properties.Resources.qtde, ObOp.Obrigatorio, 11);
                    break;

                case "ZC10":
                    layout = "§ZC10|xDed¨|vDed¨";
                    NFe.cana.deduc.Add(new deduc());
                    NFe.cana.deduc[NFe.cana.deduc.Count - 1].xDed = (string)LerCampo(TpcnTipoCampo.tcStr, Properties.Resources.xDed, ObOp.Obrigatorio, 1, 60);
                    NFe.cana.deduc[NFe.cana.deduc.Count - 1].vDed = LerCampo(TpcnTipoCampo.tcDec2, Properties.Resources.vDed, ObOp.Obrigatorio, 15);
                    break;
            }
        }
    }

    public class txtTOxmlClassRetorno
    {
        public Int32 NotaFiscal { get; set; }
        public Int32 Serie { get; set; }
        public string XMLFileName { get; set; }
        public string ChaveNFe { get; set; }

        public txtTOxmlClassRetorno(string xmlFileName, string chaveNFe, Int32 notaFiscal, Int32 serie)
        {
            this.XMLFileName = xmlFileName;
            this.ChaveNFe = chaveNFe;
            this.NotaFiscal = notaFiscal;
            this.Serie = serie;
        }
    }
}