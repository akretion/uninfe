using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace NFe.Components
{
    public static class Functions
    {
        #region MemoryStream

        /// <summary>
        /// Método responsável por converter uma String contendo a estrutura de um XML em uma Stream para
        /// ser lida pela XMLDocument
        /// </summary>
        /// <returns>String convertida em Stream</returns>
        /// <remarks>Conteúdo do método foi fornecido pelo Marcelo da desenvolvedores.net</remarks>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        public static MemoryStream StringXmlToStream(string strXml)
        {
            byte[] byteArray = new byte[strXml.Length];
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byteArray = encoding.GetBytes(strXml);
            MemoryStream memoryStream = new MemoryStream(byteArray);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        public static MemoryStream StringXmlToStreamUTF8(string strXml)
        {
            byte[] byteArray = new byte[strXml.Length];
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byteArray = encoding.GetBytes(strXml);
            MemoryStream memoryStream = new MemoryStream(byteArray);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        #endregion MemoryStream

        #region Move()

        /// <summary>
        /// Mover arquivo para uma determinada pasta
        /// </summary>
        /// <param name="arquivoOrigem">Arquivo de origem (arquivo a ser movido)</param>
        /// <param name="arquivoDestino">Arquivo de destino (destino do arquivo)</param>
        public static void Move(string arquivoOrigem, string arquivoDestino)
        {
            if (!Directory.Exists(Path.GetDirectoryName(arquivoDestino)))
                Directory.CreateDirectory(Path.GetDirectoryName(arquivoDestino));
            else if (File.Exists(arquivoDestino))
                File.Delete(arquivoDestino);

            File.Move(arquivoOrigem, arquivoDestino);
        }

        #endregion Move()

        #region DeletarArquivo()

        /// <summary>
        /// Excluir arquivos do HD
        /// </summary>
        /// <param name="Arquivo">Nome do arquivo a ser excluido.</param>
        /// <date>05/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public static void DeletarArquivo(string arquivo)
        {
            if (File.Exists(arquivo))
            {
                File.Delete(arquivo);
            }
        }

        #endregion DeletarArquivo()

        #region CodigoParaUF()

        public static string CodigoParaUF(int codigo)
        {
            try
            {
                var es = Propriedade.Estados.First(s => s.CodigoMunicipio == codigo);
                return es.UF;
            }
            catch
            {
                return "";
            }
        }

        #endregion CodigoParaUF()

        #region UFParaCodigo()

        public static int UFParaCodigo(string uf)
        {
            try
            {
                var es = Propriedade.Estados.First(s => s.UF.Equals(uf));
                return es.CodigoMunicipio;
            }
            catch
            {
                return 0;
            }
        }

        #endregion UFParaCodigo()

        #region PadraoNFe()

        public static PadroesNFSe PadraoNFSe(int municipio)
        {
            PadroesNFSe result = PadroesNFSe.NaoIdentificado;

            foreach (Municipio mun in Propriedade.Municipios)
                if (mun.CodigoMunicipio == municipio)
                    result = mun.Padrao;

            return result;
        }

        #endregion PadraoNFe()

        #region OnlyNumbers()

        /// <summary>
        /// Remove caracteres não-numéricos e retorna.
        /// </summary>
        /// <param name="text">valor a ser convertido</param>
        /// <returns>somente números com decimais</returns>
        public static object OnlyNumbers(object text)
        {
            bool flagNeg = false;

            if (text == null || text.ToString().Length == 0) return "";
            string ret = "";

            foreach (char c in text.ToString().ToCharArray())
            {
                if (c.Equals('.') == true || c.Equals(',') == true || char.IsNumber(c) == true)
                    ret += c.ToString();
                else if (c.Equals('-') == true)
                    flagNeg = true;
            }

            if (flagNeg == true) ret = "-" + ret;

            return ret;
        }

        #endregion OnlyNumbers()

        #region OnlyNumbers()

        /// <summary>
        /// Remove caracteres não-numéricos e retorna.
        /// </summary>
        /// <param name="text">valor a ser convertido</param>
        /// <param name="additionalChars">caracteres adicionais a serem removidos</param>
        /// <returns>somente números com decimais</returns>
        public static object OnlyNumbers(object text, string removeChars)
        {
            string ret = OnlyNumbers(text).ToString();

            foreach (char c in removeChars.ToCharArray())
            {
                ret = ret.Replace(c.ToString(), "");
            }

            return ret;
        }

        #endregion OnlyNumbers()

        #region Gerar MD5

        public static string GerarMD5(string valor)
        {
            // Cria uma nova intância do objeto que implementa o algoritmo para
            // criptografia MD5
            System.Security.Cryptography.MD5 md5Hasher = System.Security.Cryptography.MD5.Create();

            // Criptografa o valor passado
            byte[] valorCriptografado = md5Hasher.ComputeHash(Encoding.Default.GetBytes(valor));

            // Cria um StringBuilder para passarmos os bytes gerados para ele
            StringBuilder strBuilder = new StringBuilder();

            // Converte cada byte em um valor hexadecimal e adiciona ao
            // string builder
            // and format each one as a hexadecimal string.
            for (int i = 0; i < valorCriptografado.Length; i++)
            {
                strBuilder.Append(valorCriptografado[i].ToString("x2"));
            }

            // retorna o valor criptografado como string
            return strBuilder.ToString();
        }

        #endregion Gerar MD5

        #region LerArquivo()

        /// <summary>
        /// Le arquivos no formato TXT
        /// Retorna uma lista do conteudo do arquivo
        /// </summary>
        /// <param name="cArquivo"></param>
        /// <returns></returns>
        public static List<string> LerArquivo(string cArquivo)
        {
            List<string> lstRetorno = new List<string>();
            if (File.Exists(cArquivo))
            {
                using (System.IO.StreamReader txt = new StreamReader(cArquivo, Encoding.Default, true))
                {
                    try
                    {
                        string cLinhaTXT = txt.ReadLine();
                        while (cLinhaTXT != null)
                        {
                            string[] dados = cLinhaTXT.Split('|');
                            if (dados.GetLength(0) > 1)
                            {
                                lstRetorno.Add(cLinhaTXT);
                            }
                            cLinhaTXT = txt.ReadLine();
                        }
                    }
                    finally
                    {
                        txt.Close();
                    }
                    if (lstRetorno.Count == 0)
                        throw new Exception("Arquivo: " + cArquivo + " vazio");
                }
            }
            return lstRetorno;
        }

        #endregion LerArquivo()

        #region ExtrairNomeArq()

        /// <summary>
        /// Extrai o nome do arquivo de uma determinada string. Este não mantem a pasta que ele está localizado, fica somente o nome do arquivo.
        /// </summary>
        /// <param name="arquivo">string contendo o caminho e nome do arquivo que é para ser extraído o conteúdo desejado</param>
        /// <param name="finalArq">string contendo o final do nome do arquivo que é para ser retirado do nome</param>
        /// <returns>Retorna somente o nome do arquivo de acordo com os parâmetros passado</returns>
        /// <example>
        /// MessageBox.Show(ExtrairNomeArq("C:\\TESTE\\NFE\\ENVIO\\ArqSituacao-ped-sta.xml", "-ped-sta.xml"));
        /// //Será demonstrado no message a string "ArqSituacao"
        ///
        /// MessageBox.Show(ExtrairNomeArq("C:\\TESTE\\NFE\\ENVIO\\ArqSituacao-ped-sta.xml", ".xml"));
        /// //Será demonstrado no message a string "ArqSituacao-ped-sta"
        /// </example>
        public static string ExtrairNomeArq(string arquivo, string finalArq)
        {
            if (string.IsNullOrEmpty(arquivo))
                return "";

            FileInfo fi = new FileInfo(arquivo);
            string ret = fi.Name;
            string retorno = "";

            if (!string.IsNullOrEmpty(finalArq) && finalArq.Length == 4 && finalArq.StartsWith("."))
                return ret.Substring(0, ret.Length - finalArq.Length);

            ///
            /// alteracao feita pq um usuario comentou que estava truncando uma parte do nome original do arquivo
            ///
            /// se o nome do arquivo for: 123456790-nfe.xml e
            ///             finalArq for:      -ret-nfe.xml, retornaria: 12345
            /// ou
            /// se o nome do arquivo for: 123456790-ret-nfe.xml e
            ///             finalArq for:              -nfe.xml, retornaria: 123456789-ret
            ///
            /*
                -pro-rec.err
                -pro-rec.xml
                -rec.err
                -rec.xml
             */

            ///
            /// pesquisa primeiro pela lista de retornos, porque geralmente os nomes são maiores que os de envio
            /// isso evita conflito de nomes como por ex: -cons-cad.xml x -ret-cons-cad.xml
            ///
            foreach (var pS in typeof(Propriedade.ExtRetorno).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                string extensao = pS.GetValue(null).ToString();

                if (ret.EndsWith(extensao, StringComparison.InvariantCultureIgnoreCase))
                {
                    retorno = ret.Substring(0, ret.Length - extensao.Length);
                    break;
                }
            }

            if (retorno == "")
            {
                foreach (Propriedade.TipoEnvio item in Enum.GetValues(typeof(Propriedade.TipoEnvio)))
                {
                    var EXT = Propriedade.Extensao(item);

                    ///
                    /// pesquisa primeiro pelas extensões de retorno, pois geralmente, elas são maiores que as de envio
                    ///
                    if (!string.IsNullOrEmpty(EXT.RetornoXML))
                        if (ret.EndsWith(EXT.RetornoXML, StringComparison.InvariantCultureIgnoreCase))
                            retorno = ret.Substring(0, ret.Length - EXT.RetornoXML.Length);

                    if (!string.IsNullOrEmpty(EXT.RetornoTXT))
                        if (ret.EndsWith(EXT.RetornoTXT, StringComparison.InvariantCultureIgnoreCase))
                            retorno = ret.Substring(0, ret.Length - EXT.RetornoTXT.Length);

                    if (ret.EndsWith(EXT.EnvioXML, StringComparison.InvariantCultureIgnoreCase))
                        retorno = ret.Substring(0, ret.Length - EXT.EnvioXML.Length);

                    if (!string.IsNullOrEmpty(EXT.EnvioTXT))
                        if (ret.EndsWith(EXT.EnvioTXT, StringComparison.InvariantCultureIgnoreCase))
                            retorno = ret.Substring(0, ret.Length - EXT.EnvioTXT.Length);
                }
            }

            if (retorno == "")
                if (!string.IsNullOrEmpty(finalArq))
                    if (ret.EndsWith(finalArq, StringComparison.InvariantCultureIgnoreCase))
                        retorno = ret.Substring(0, ret.Length - finalArq.Length);

            if (retorno != "")
            {
                if (retorno.ToLower().EndsWith("-ped"))
                    return retorno.Substring(0, retorno.ToLower().IndexOf("-ped"));

                if (retorno.ToLower().EndsWith("-ret"))
                    return retorno.Substring(0, retorno.ToLower().IndexOf("-ret"));

                if (retorno.ToLower().EndsWith("-con"))
                    return retorno.Substring(0, retorno.ToLower().IndexOf("-con"));

                if (retorno.ToLower().EndsWith("-env"))
                    return retorno.Substring(0, retorno.ToLower().IndexOf("-env"));

                return retorno.TrimEnd(new char[] { '-' });
            }

            return fi.Name;
        }

        #endregion ExtrairNomeArq()

        #region ExtraiPastaNomeArq()

        /// <summary>
        /// Extrai o nome do arquivo de uma determinada string mantendo a pasta que ele está localizado
        /// </summary>
        /// <param name="arquivo">string contendo o caminho e nome do arquivo que é para ser extraído o conteúdo desejado</param>
        /// <param name="finalArq">string contendo o final do nome do arquivo que é para ser retirado do nome</param>
        /// <returns>Retorna a pasta e o nome do arquivo de acordo com os parâmetros passado.</returns>
        /// <example>
        /// MessageBox.Show(ExtrairPastaNomeArq("C:\\TESTE\\NFE\\ENVIO\\ArqSituacao-ped-sta.xml", "-ped-sta.xml"));
        /// //Será demonstrado no message a string "C:\\TESTE\\NFE\\ENVIO\\ArqSituacao"
        ///
        /// MessageBox.Show(ExtrairPastaNomeArq("C:\\TESTE\\NFE\\ENVIO\\ArqSituacao-ped-sta.xml", ".xml"));
        /// //Será demonstrado no message a string "C:\\TESTE\\NFE\\ENVIO\\ArqSituacao-ped-sta"
        /// </example>
        public static string ExtraiPastaNomeArq(string arquivo, string finalArq)
        {
            FileInfo fi = new FileInfo(arquivo);
            string ret = fi.FullName;
            ret = ret.Substring(0, ret.Length - finalArq.Length);
            return ret;
        }

        #endregion ExtraiPastaNomeArq()

        public static string ExtractExtension(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return (value.IndexOf('.') >= 0 ? Path.ChangeExtension(("X" + value), "").Substring(1).Replace(".", "") : value);
        }

        public static string GetAttributeXML(string node, string attribute, string file)
        {
            string result = "";
            XmlDocument conteudoXML = new XmlDocument();
            conteudoXML.Load(file);

            XmlElement elementos = (XmlElement)conteudoXML.GetElementsByTagName(node)[0];
            if(elementos != null)
            {
                result = elementos.GetAttribute(attribute);
            }

            return result;
        }

        #region FileInUse()

        /// <summary>
        /// detectar se o arquivo está em uso
        /// </summary>
        /// <param name="file">caminho do arquivo</param>
        /// <returns>true se estiver em uso</returns>
        /// <by>http://desenvolvedores.net/marcelo</by>
        [System.Diagnostics.DebuggerHidden()]
        public static bool FileInUse(string file)
        {
            bool ret = false;

            try
            {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    fs.Close();//fechar o arquivo para nao dar erro em outras aplicações
                }
            }
            catch
            {
                ret = true;
            }

            return ret;
        }

        #endregion FileInUse()

        #region LerTag()

        /// <summary>
        /// Busca o nome de uma determinada TAG em um Elemento do XML para ver se existe, se existir retorna seu conteúdo com um ponto e vírgula no final do conteúdo.
        /// </summary>
        /// <param name="Elemento">Elemento a ser pesquisado o Nome da TAG</param>
        /// <param name="NomeTag">Nome da Tag</param>
        /// <returns>Conteúdo da tag</returns>
        /// <date>05/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public static string LerTag(XmlElement Elemento, string NomeTag)
        {
            return LerTag(Elemento, NomeTag, true);
        }

        #endregion LerTag()

        #region LerTag()

        /// <summary>
        /// Busca o nome de uma determinada TAG em um Elemento do XML para ver se existe, se existir retorna seu conteúdo, com ou sem um ponto e vírgula no final do conteúdo.
        /// </summary>
        /// <param name="Elemento">Elemento a ser pesquisado o Nome da TAG</param>
        /// <param name="NomeTag">Nome da Tag</param>
        /// <param name="RetornaPontoVirgula">Retorna com ponto e vírgula no final do conteúdo da tag</param>
        /// <returns>Conteúdo da tag</returns>
        /// <date>05/08/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public static string LerTag(XmlElement Elemento, string NomeTag, bool RetornaPontoVirgula)
        {
            string Retorno = string.Empty;

            if (Elemento.GetElementsByTagName(NomeTag).Count != 0)
            {
                if (RetornaPontoVirgula)
                {
                    Retorno = Elemento.GetElementsByTagName(NomeTag)[0].InnerText.Replace(";", " ");  //danasa 19-9-2009
                    Retorno += ";";
                }
                else
                {
                    Retorno = Elemento.GetElementsByTagName(NomeTag)[0].InnerText;  //Wandrey 07/10/2009
                }
            }

            return Retorno;
        }

        public static string LerTag(XmlElement Elemento, string NomeTag, string defaultValue)
        {
            string result = LerTag(Elemento, NomeTag, false);
            if (string.IsNullOrEmpty(result))
                result = defaultValue;
            return result;
        }

        #endregion LerTag()

        #region IsConnectedToInternet()

        //Creating the extern function...
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        //Creating a function that uses the API function...
        public static bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }

        #endregion IsConnectedToInternet()

        #region XmlToString()

        /// <summary>
        /// Método responsável por ler o conteúdo de um XML e retornar em uma string
        /// </summary>
        /// <param name="parNomeArquivo">Caminho e nome do arquivo XML que é para pegar o conteúdo e retornar na string.</param>
        /// <returns>Retorna uma string com o conteúdo do arquivo XML</returns>
        /// <example>
        /// string ConteudoXML;
        /// ConteudoXML = THIS.XmltoString( @"c:\arquivo.xml" );
        /// MessageBox.Show( ConteudoXML );
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>04/06/2008</date>
        public static string XmlToString(string parNomeArquivo)
        {
            string conteudo_xml = string.Empty;

            StreamReader SR = null;
            try
            {
                SR = File.OpenText(parNomeArquivo);
                conteudo_xml = SR.ReadToEnd();
            }
            finally
            {
                SR.Close();
            }

            return conteudo_xml;
        }

        #endregion XmlToString()

        #region getDateTime()

        public static DateTime GetDateTime(string value)
        {
            if (string.IsNullOrEmpty(value))
                return DateTime.MinValue;

            int _ano = Convert.ToInt16(value.Substring(0, 4));
            int _mes = Convert.ToInt16(value.Substring(5, 2));
            int _dia = Convert.ToInt16(value.Substring(8, 2));
            if (value.Contains("T") && value.Contains(":"))
            {
                int _hora = Convert.ToInt16(value.Substring(11, 2));
                int _min = Convert.ToInt16(value.Substring(14, 2));
                int _seg = Convert.ToInt16(value.Substring(17, 2));
                return new DateTime(_ano, _mes, _dia, _hora, _min, _seg);
            }
            return new DateTime(_ano, _mes, _dia);
        }

        #endregion getDateTime()

        #region CarregaUF()

        /// <summary>
        /// Carrega os Estados que possuem serviço de NFE já disponível. Estes Estados são carregados a partir do XML Webservice.xml que fica na pasta do executável do UNINFE
        /// </summary>
        /// <returns>Retorna a lista de UF e seus ID´s</returns>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 01/03/2010
        /// </remarks>
        ///
        public static ArrayList CarregaMunicipios()
        {
            ArrayList UF = new ArrayList();

            Propriedade.Municipios.ForEach((mun) => { UF.Add(new ComboElem(mun.UF, mun.CodigoMunicipio, mun.Nome)); });

            if (File.Exists(Propriedade.NomeArqXMLWebService_NFSe))
            {
                //Carregar os dados do arquivo XML de configurações da Aplicação
                XElement axml = XElement.Load(Propriedade.NomeArqXMLWebService_NFSe);
                var s = (from p in axml.Descendants(NFe.Components.NFeStrConstants.Estado)
                         where (string)p.Attribute(TpcnResources.UF.ToString()) != "XX"
                         select p);
                foreach (var item in s)
                {
                    if (Convert.ToInt32("0" + OnlyNumbers(item.Attribute(TpcnResources.ID.ToString()).Value)) == 0)
                        continue;

                    var temp = Propriedade.Municipios.FirstOrDefault(x => x.CodigoMunicipio == Convert.ToInt32(item.Attribute(TpcnResources.ID.ToString()).Value));
                    if (temp == null)
                    {
                        UF.Add(new ComboElem(item.Attribute(TpcnResources.UF.ToString()).Value,
                            Convert.ToInt32(item.Attribute(NFe.Components.TpcnResources.ID.ToString()).Value),
                            item.Element(NFe.Components.NFeStrConstants.Nome).Value));
                    }
                }
            }
            UF.Sort(new OrdenacaoPorNome());
            return UF;
        }

        public static ArrayList CarregaUF()
        {
            ArrayList UF = new ArrayList();
            UF = CarregaEstados();
            return UF;
        }

        public static ArrayList CarregaEstados()
        {
            ArrayList UF = new ArrayList();
            foreach (var estado in Propriedade.Estados)
            {
                UF.Add(new ComboElem(estado.UF, estado.CodigoMunicipio, estado.Nome));
            }
            UF.Sort(new OrdenacaoPorNome());
            return UF;
        }

        #endregion CarregaUF()

        #region ComputeHexadecimal()

        /// <summary>
        /// Calcula valor hexadecimal
        /// Usado para calcular o Link do QRCode da NFCe
        /// </summary>
        /// <param name="input">Valor a ser convertido</param>
        /// <returns></returns>
        public static string ComputeHexadecimal(string input)
        {
            string hexOutput = "";
            char[] values = input.ToCharArray();
            foreach (char letter in values)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(letter);

                // Convert the decimal value to a hexadecimal value in string form.
                hexOutput += String.Format("{0:x}", value);
            }

            return hexOutput;
        }

        #endregion ComputeHexadecimal()

        /// <summary>
        /// Criptografar conteúdo com MD5
        /// </summary>
        /// <param name="input">Conteúdo a ser criptografado</param>
        /// <returns>Conteúdo criptografado com MD5</returns>
        public static string GetMD5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// Retorna a empresa pela thread atual
        /// </summary>
        /// <returns></returns>
        public static int _FindEmpresaByThread()
        {
            return Convert.ToInt32(Thread.CurrentThread.Name);
        }

        #region Ticket: #110

        /*
         * Marcelo
         * 03/06/2013
         */

        /// <summary>
        /// Retorna o endereço IP desta estação
        /// </summary>
        /// <returns>Endereço ip da estação</returns>
        public static string GetIPAddress()
        {
            var hostEntry = Dns.GetHostEntry(Environment.MachineName);
            string ip = (
                       from addr in hostEntry.AddressList
                       where addr.AddressFamily.ToString() == "InterNetwork"
                       select addr.ToString()
                ).FirstOrDefault();

            return ip;
        }

        #endregion Ticket: #110

        [System.Diagnostics.DebuggerHidden()]
        public static void CopyObjectTo(this object Source, object Destino)
        {
            foreach (var pS in Source.GetType().GetProperties())
            {
                if (!pS.CanWrite) continue;

                foreach (var pT in Destino.GetType().GetProperties())
                {
                    if (!pT.Name.Equals(pS.Name, StringComparison.InvariantCultureIgnoreCase)) continue;

                    try
                    {
                        (pT.GetSetMethod()).Invoke(Destino, new object[] { pS.GetGetMethod().Invoke(Source, null) });
                    }
                    catch //(Exception ex)
                    {
                        //Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public static bool SetProperty(object _this, string propName, object value)
        {
            try
            {
                PropertyInfo aProperties = _this.GetType().GetProperty(propName, BindingFlags.Instance | BindingFlags.Public);
                if (aProperties != null)
                {
                    SetProperty(_this, aProperties, value);
                    return true;
                }
                return false;
            }
            catch //(Exception ex)
            {
                //Console.WriteLine("AAAAAAAAAAAAAA>>>>>>>>>"+ex.Message);
                return false;
            }
        }

        public static void SetProperty(object _this, PropertyInfo propertyInfo, object value)
        {
            if (value == null)
                propertyInfo.SetValue(_this, null, null);
            else
                switch (propertyInfo.PropertyType.Name)
                {
                    case "Int32":
                        propertyInfo.SetValue(_this, Convert.ToInt32(value), null);
                        break;

                    case "String":
                        propertyInfo.SetValue(_this, value.ToString(), null);
                        break;

                    case "Boolean":
                        propertyInfo.SetValue(_this, Convert.ToBoolean(value), null);
                        break;

                    case "Double":
                        propertyInfo.SetValue(_this, Convert.ToDouble(value), null);
                        break;

                    case "Decimal":
                        propertyInfo.SetValue(_this, Convert.ToDecimal(value), null);
                        break;

                    case "DateTime":
                        propertyInfo.SetValue(_this, Convert.ToDateTime(value), null);
                        break;

                    default:
                        switch (propertyInfo.PropertyType.FullName)
                        {
                            case "NFe.Components.TipoAplicativo":
                                var ta1 = (NFe.Components.TipoAplicativo)Enum.Parse(typeof(NFe.Components.TipoAplicativo), value.ToString(), true);
                                propertyInfo.SetValue(_this, ta1, null);
                                break;

                            case "NFe.Components.TipoAmbiente":
                                var ta2 = (NFe.Components.TipoAmbiente)Enum.Parse(typeof(NFe.Components.TipoAmbiente), value.ToString(), true);
                                propertyInfo.SetValue(_this, ta2, null);
                                break;

                            case "NFe.Components.TipoEmissao":
                                var ta3 = (NFe.Components.TipoEmissao)Enum.Parse(typeof(NFe.Components.TipoEmissao), value.ToString(), true);
                                propertyInfo.SetValue(_this, ta3, null);
                                break;

                            case "NFe.Components.DiretorioSalvarComo":

                                //propertyInfo.SetValue(_this, value.ToString(), null);
                                break;

                            default:
                                throw new Exception(propertyInfo.Name + "..." + propertyInfo.PropertyType.FullName + "...." + value.ToString());
                        }
                        break;
                }
        }

        private static bool populateClasse(object classe, string[] origem)
        {
            PropertyInfo[] allClassToProperties = classe.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);

            bool lEncontrou = false;
            foreach (var xi in origem)
            {
                if (!string.IsNullOrEmpty(xi))
                {
                    var xii = xi.Split(new char[] { '|' });
                    if (xii.Length == 2)
                    {
                        if (xii[0].Equals("DiretorioSalvarComo", StringComparison.InvariantCultureIgnoreCase))
                            xii[0] = "diretorioSalvarComo";

                        var pi = (from i in allClassToProperties where i.Name.Equals(xii[0], StringComparison.InvariantCultureIgnoreCase) select i).FirstOrDefault();
                        if (pi == null)
                            Console.WriteLine(xi + ": NOT FOUND");
                        else
                        {
                            Functions.SetProperty(classe, pi, xii[1]);
                            lEncontrou = true;
                        }
                    }
                }
            }
            return lEncontrou;
        }

        public static bool PopulateClasse(object classe, object origem)
        {
            bool lEncontrou = false;

            if (origem.GetType().IsAssignableFrom(typeof(XmlElement)))
            {
                List<System.String> temp = new List<string>();
                var xx = (origem as XmlElement).ChildNodes;
                foreach (var xa in xx)
                {
                    temp.Add((xa as XmlElement).Name + "|" + (xa as XmlElement).InnerText);
                }
                lEncontrou = populateClasse(classe, temp.ToArray());
            }
            else
            {
                if (origem.GetType().IsAssignableFrom(typeof(System.String[])))
                {
                    lEncontrou = populateClasse(classe, origem as string[]);
                }
                else
                    if (origem.GetType().IsAssignableFrom(typeof(List<System.String>)))
                {
                    lEncontrou = populateClasse(classe, (origem as List<String>).ToArray());
                }
                else
                    throw new Exception("Tipo de dados da origem desconhecido. (" + origem.GetType().ToString() + ")");
            }
            return lEncontrou;
        }

        public static void GravaTxtXml(object w, string fieldname, string content)
        {
            MethodInfo method = w.GetType().GetMethod("WriteElementString", new Type[] { typeof(System.String), typeof(System.String) });
            if (method == null)
            {
                method = w.GetType().GetMethod("WriteLine", new Type[] { typeof(System.String) });
                method.Invoke(w, new object[] { fieldname + "|" + content });
            }
            else
            {
                method.Invoke(w, new object[] { fieldname, content });
            }
        }

        #region WriteLog()

        public static void WriteLog(string msg, bool gravarStackTrace, bool geraLog, string CNPJEmpresa)
        {
            if (string.IsNullOrEmpty(msg)) return;

#if DEBUG
            System.Diagnostics.Debug.WriteLine(msg);
#endif
            if (geraLog)
            {
                if (!string.IsNullOrEmpty(CNPJEmpresa))
                    CNPJEmpresa += "_";

                string fileName = Propriedade.PastaLog + "\\uninfe_" +
                    (string.IsNullOrEmpty(CNPJEmpresa) ? "" : CNPJEmpresa) +
                    DateTime.Now.ToString("yyyy-MMM-dd") + ".log";

                DateTime startTime;
                DateTime stopTime;
                TimeSpan elapsedTime;

                long elapsedMillieconds;
                startTime = DateTime.Now;

                while (true)
                {
                    stopTime = DateTime.Now;
                    elapsedTime = stopTime.Subtract(startTime);
                    elapsedMillieconds = (int)elapsedTime.TotalMilliseconds;

                    StreamWriter arquivoWS = null;
                    try
                    {
                        //Se for para gravar ot race
                        if (gravarStackTrace)
                        {
                            msg += "\r\nSTACK TRACE:";
                            msg += "\r\n" + Environment.StackTrace;

                            /*
                            StackTrace stackTrace = new StackTrace();
                            StackFrame[] stackFrames = stackTrace.GetFrames();
                            foreach (StackFrame s in stackFrames)
                            {
                                msg += "\r\nModule: " + s.GetMethod().ReflectedType.Module.Name + " Class: " + s.GetMethod().ReflectedType.FullName + " Method: " + s.GetMethod().Name;
                                msg += " line: " + s.GetFileLineNumber();
                            }*/
                        }

                        arquivoWS = new StreamWriter(fileName, true, Encoding.UTF8);
                        arquivoWS.WriteLine(DateTime.Now.ToLongTimeString() + "  " + msg);
                        arquivoWS.Flush();
                        arquivoWS.Close();
                        break;
                    }
                    catch
                    {
                        if (arquivoWS != null)
                        {
                            arquivoWS.Close();
                        }

                        if (elapsedMillieconds >= 60000) //60.000 ms que corresponde á 60 segundos que corresponde a 1 minuto
                        {
                            break;
                        }
                    }
                    Thread.Sleep(2);
                }
            }
        }

        #endregion WriteLog()

        /// <summary>
        ///
        /// </summary>
        /// <param name="file"></param>
        /// <param name="resultFolder"></param>
        /// <param name="ex"></param>
        public static void GravarErroMover(string file, string resultFolder, string ex)
        {
            if (!string.IsNullOrEmpty(resultFolder) && Directory.Exists(Path.GetDirectoryName(resultFolder)) && !string.IsNullOrEmpty(file))
            {
                FileInfo infFile = new FileInfo(file);
                string extFile = infFile.Name.Replace(infFile.Extension, "");
                string extError = extFile + ".err";

                string nomearq = resultFolder + "\\" + extError;

                StreamWriter write = new StreamWriter(nomearq);
                write.Write(ex);
                write.Flush();
                write.Close();
                write.Dispose();
            }
            else
                WriteLog(ex, false, true, "");
        }

        /// <summary>
        /// Codificar em base 64 um determinado valor
        /// </summary>
        /// <param name="value">Valor a ser codificado</param>
        /// <returns></returns>
        public static string Base64Encode(string value)
        {
            byte[] encode = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(encode);
        }

        /// <summary>
        /// Retorna um Base64 com 28 caracteres
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToBase64Hex(string value)
        {
            int countChars = value.Length;
            byte[] bytes = new byte[countChars / 2];

            for (int i = 0; i < countChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(value.Substring(i, 2), 16);
            }

            return Convert.ToBase64String(bytes);
        }
    }
}