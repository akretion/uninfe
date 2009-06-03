using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;
using uninfe;

namespace GerarProcNFe
{
    class Program
    {
        static void Main(string[] args)
        {
            GerarProcNFe oGerar = new GerarProcNFe();

            oGerar.ProcessarArquivos();
        }
    }

    class GerarProcNFe
    {
        #region Atributos
        /// <summary>
        /// Atributo que vai receber a lista de arquivos com o final de nome -nfe.xml
        /// </summary>
        List<string> lstArquivoNFe = new List<string>();
        /// <summary>
        /// Atributo que vai receber a lista de arquivos com o final de nome -pro-rec.xml
        /// </summary>
        List<string> lstArquivoProRec = new List<string>();
        #endregion

        #region Métodos

        #region LocalizarArquivos()
        /// <summary>
        /// Localizar e criar uma lista com os arquivos de nfe e protocolo da pasta do executável
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>02/05/2009</date>
        private void LocalizarArquivos()
        {
            lstArquivoNFe.Clear();
            lstArquivoProRec.Clear();

            try
            {
                foreach (string item in Directory.GetFiles(Directory.GetCurrentDirectory(), "*-nfe.xml"))
                {
                    lstArquivoNFe.Add(item);
                }
            }
            catch (IOException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            try
            {
                foreach (string item in Directory.GetFiles(Directory.GetCurrentDirectory(), "*-pro-rec.xml"))
                {
                    lstArquivoProRec.Add(item);
                }
            }
            catch (IOException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region ProcessarArquivos()
        public void ProcessarArquivos()
        {
            this.LocalizarArquivos();

            for (int i = 0; i < lstArquivoNFe.Count; i++)
            {
                Console.WriteLine("");
                Console.WriteLine((i + 1).ToString().Trim() + "/" + lstArquivoNFe.Count.ToString().Trim());
                Console.WriteLine("Aguarde! Gerando arquivo de distribuição da NF-e:");
                Console.WriteLine(lstArquivoNFe[i]);

                string strArqNfe = lstArquivoNFe[i];
                string strChaveNFe = LerChave(strArqNfe);

                string strArqProRec = BuscarArqProRec(strChaveNFe);

                if (strArqProRec != string.Empty)
                {
                    this.JuntarArquivo(strArqNfe, strArqProRec);
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("Não foi possível localizar o arquivo de protoloco desta NFe, o arquivo de distribuição não será gerado.");
                    Console.WriteLine("Pressione qualquer tecla para continuar criando os demais arquivos.");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }

            Console.WriteLine("");
            Console.WriteLine("Processo encerrado.");
            Console.ReadKey();
        }
        #endregion

        #region LerChave()
        /// <summary>
        /// Ler a chave da nfe
        /// </summary>
        /// <param name="cArqNfe">Nome do arquivo de NFe</param>
        /// <returns>a chave da nfe</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>02/05/2009</date>
        private string LerChave(string strArqNfe)
        {
            UniLerXMLClass oLerXml = new UniLerXMLClass();

            oLerXml.Nfe(strArqNfe);

            return oLerXml.oDadosNfe.chavenfe;
        }
        #endregion

        #region BuscarArqProRec()
        private string BuscarArqProRec(string strChaveNFe)
        {
            string strConteudo = string.Empty;

            for (int i = 0; i < lstArquivoProRec.Count; i++)
            {
                FileStream fsArquivo = null;

                try
                {
                    XmlDocument doc = new XmlDocument(); //Criar instância do XmlDocument Class
                    fsArquivo = new FileStream(lstArquivoProRec[i], FileMode.Open, FileAccess.ReadWrite, FileShare.Read); //Abrir um arquivo XML usando FileStream
                    doc.Load(fsArquivo); //Carregar o arquivo aberto no XmlDocument

                    XmlNodeList documentoList = doc.GetElementsByTagName("infProt");
                    foreach (XmlNode documentoNode in documentoList)
                    {
                        XmlElement documentoElemento = (XmlElement)documentoNode;

                        if ("NFe" + documentoElemento.GetElementsByTagName("chNFe")[0].InnerText == strChaveNFe)
                        {
                            strConteudo = lstArquivoProRec[i];

                            break;
                        }
                    }

                    fsArquivo.Close(); //Fecha o arquivo XML
                }
                catch (Exception ex)
                {
                    if (fsArquivo != null)
                    {
                        fsArquivo.Close();
                    }

                    throw (ex);
                }

                if (strConteudo != string.Empty)
                {
                    break;
                }
            }

            return strConteudo;
        }
        #endregion

        #region JuntarArquivo()
        private void JuntarArquivo(string strArqNFe, string strArqProRec)
        {
            this.CriarXmlDistNFe(strArqNFe, strArqProRec);
        }
        #endregion

        #region CriarXMLDistNFe()
        /// <summary>
        /// Criar o arquivo XML de distribuição das NFE com o protocolo de autorização anexado
        /// </summary>
        /// <param name="strArqNFe">Nome arquivo XML da NFe</param>
        /// <param name="strProtNfe">String contendo a parte do XML do protocolo a ser anexado</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>20/04/2009</date>
        private void CriarXmlDistNFe(string strArqNFe, string strArqProRec)
        {
            UniNfeClass oUniNfe = new UniNfeClass();

            string strProtNfe = this.SeparaProtocolo(strArqProRec);

            StreamWriter swProc = null;

            try
            {
                //Separar as tag´s da NFe que interessa <NFe> até </NFe>
                XmlDocument doc = new XmlDocument();

                doc.Load(strArqNFe);

                XmlNodeList NFeList = doc.GetElementsByTagName("NFe");
                XmlNode NFeNode = NFeList[0];
                string strNFe = NFeNode.OuterXml;

                //Montar o XML -proc-NFe.xml
                string strXmlProcNfe = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                    "<nfeProc xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"1.10\">" +
                    strNFe +
                    strProtNfe +
                    "</nfeProc>";

                //Montar o nome do arquivo -proc-NFe.xml
                string strNomeArqProcNFe = oUniNfe.ExtrairNomeArq(strArqNFe, "-nfe.xml") + "-procNFe.xml";

                //Gravar o XML em uma linha sÃƒÂ³ (sem quebrar as tagÃ‚Â´s linha a linha) ou dÃƒÂ¡ erro na hora de validar o XML pelos Schemas. Wandreu 13/05/2009
                swProc = File.CreateText(strNomeArqProcNFe);
                swProc.Write(strXmlProcNfe);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (swProc != null)
                {
                    swProc.Close();
                }
            }
        }
        #endregion

        #region SeparaProtocolo()
        private string SeparaProtocolo(string strArqProRec)
        {
            string strProtNfe = string.Empty;

            XmlDocument doc = new XmlDocument();
            doc.Load(strArqProRec);

            XmlNodeList retConsReciNFeList = doc.GetElementsByTagName("retConsReciNFe");

            foreach (XmlNode retConsReciNFeNode in retConsReciNFeList)
            {
                XmlElement retConsReciNFeElemento = (XmlElement)retConsReciNFeNode;

                XmlNodeList protNFeList = retConsReciNFeElemento.GetElementsByTagName("protNFe");

                foreach (XmlNode protNFeNode in protNFeList)
                {
                    XmlElement protNFeElemento = (XmlElement)protNFeNode;

                    strProtNfe = protNFeElemento.OuterXml;
                }
            }

            return strProtNfe;
        }
        #endregion


        #endregion
    }
}
