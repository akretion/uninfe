using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using NFe.Components;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;

namespace NFe.Settings
{
    public class Auxiliar
    {
        #region ExtrairNomeArq()
        /// <summary>
        /// Extrai somente o nome do arquivo de uma string; para ser utilizado na situação desejada. Veja os exemplos na documentação do código.
        /// </summary>
        /// <param name="pPastaArq">String contendo o caminho e nome do arquivo que é para ser extraido o nome.</param>
        /// <param name="pFinalArq">String contendo o final do nome do arquivo até onde é para ser extraído.</param>
        /// <returns>Retorna somente o nome do arquivo de acordo com os parâmetros passados - veja exemplos.</returns>
        /// <example>
        /// MessageBox.Show(this.ExtrairNomeArq("C:\\TESTE\\NFE\\ENVIO\\ArqSituacao-ped-sta.xml", "-ped-sta.xml"));
        /// //Será demonstrado no message a string "ArqSituacao"
        /// 
        /// MessageBox.Show(this.ExtrairNomeArq("C:\\TESTE\\NFE\\ENVIO\\ArqSituacao-ped-sta.xml", ".xml"));
        /// //Será demonstrado no message a string "ArqSituacao-ped-sta"
        /// </example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>19/06/2008</date>
        /// 
#if tirada
        public string xExtrairNomeArq(string pPastaArq, string pFinalArq)
        {
            FileInfo fi = new FileInfo(pPastaArq);
            string ret = fi.Name;
            ret = ret.Substring(0, ret.Length - pFinalArq.Length);
            return ret;
        }
#endif
        #endregion

        #region GravarArqErroERP
        /// <summary>
        /// grava um arquivo de erro ao ERP
        /// </summary>
        /// <param name="Arquivo"></param>
        /// <param name="Erro"></param>
        public void GravarArqErroERP(string Arquivo, string Erro)
        {
            if(!string.IsNullOrEmpty(Arquivo))
            {
                try
                {
                    int emp = Empresas.FindEmpresaByThread();
                    if(Empresas.Configuracoes[emp].PastaXmlRetorno != string.Empty)
                    {
                        //Grava arquivo de ERRO para o ERP
                        string cArqErro = Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" + Path.GetFileName(Arquivo);
                        File.WriteAllText(cArqErro, Erro, Encoding.Default);
                    }
                }
                catch
                {
                    //TODO: V3.0 - Não deveriamos retornar a exeção com throw?
                }
            }
        }
        #endregion

        #region WriteLog()
        public static void WriteLog(string msg, bool gravarStackTrace)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(msg);
#endif
            int emp = -1;
            try
            {
                emp = Empresas.FindEmpresaByThread();
            }
            catch { }
            NFe.Components.Functions.WriteLog(msg, gravarStackTrace, ConfiguracaoApp.GravarLogOperacoesRealizadas, emp >= 0 ? Empresas.Configuracoes[emp].CNPJ + "_" + Empresas.Configuracoes[emp].Servico.ToString() : "");
        }
        #endregion

        #region MoveArqErro
        /// <summary>
        /// Move arquivos XML com erro para uma pasta de xml´s com erro configurados no UniNFe.
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo a ser movido para a pasta de XML´s com erro</param>
        /// <example>this.MoveArqErro(this.vXmlNfeDadosMsg)</example>
        public void MoveArqErro(string Arquivo)
        {
            this.MoveArqErro(Arquivo, Path.GetExtension(Arquivo));
        }
        #endregion

        #region MoveArqErro()
        /// <summary>
        /// Move arquivos com a extensão informada e que está com erro para uma pasta de xml´s/arquivos com erro configurados no UniNFe.
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo a ser movido para a pasta de XML´s com erro</param>
        /// <param name="ExtensaoArq">Extensão do arquivo que vai ser movido. Ex: .xml</param>
        /// <example>this.MoveArqErro(this.vXmlNfeDadosMsg, ".xml")</example>
        public void MoveArqErro(string Arquivo, string ExtensaoArq)
        {
            int emp = Empresas.FindEmpresaByThread();

            if(File.Exists(Arquivo))
            {
                FileInfo oArquivo = new FileInfo(Arquivo);

                if(Directory.Exists(Empresas.Configuracoes[emp].PastaXmlErro))
                {
                    string vNomeArquivo = Empresas.Configuracoes[emp].PastaXmlErro + "\\" + Functions.ExtrairNomeArq(Arquivo, ExtensaoArq) + ExtensaoArq;

                    Functions.Move(Arquivo, vNomeArquivo);

                    Auxiliar.WriteLog("O arquivo " + Arquivo + " foi movido para " + vNomeArquivo, true);
                    //Auxiliar.WriteLog("O arquivo " + Arquivo + " foi movido para a pasta de XML com problemas.", true);

                    /*
                    //Deletar o arquivo da pasta de XML com erro se o mesmo existir lá para evitar erros na hora de mover. Wandrey
                    if (File.Exists(vNomeArquivo))
                        this.DeletarArquivo(vNomeArquivo);

                    //Mover o arquivo da nota fiscal para a pasta do XML com erro
                    oArquivo.MoveTo(vNomeArquivo);
                    */
                }
                else
                {
                    //Antes estava deletando o arquivo, agora vou retornar uma mensagem de erro
                    //pois não podemos excluir, pode ser coisa importante. Wandrey 25/02/2011
                    throw new Exception("A pasta de XML´s com erro informada nas configurações não existe, por favor verifique.");
                    //oArquivo.Delete();
                }
            }
        }
        #endregion

        #region EstaAutorizada()
        /// <summary>
        /// Verifica se o XML de Distribuição da Nota Fiscal (-procNFe) já está na pasta de Notas Autorizadas
        /// </summary>
        /// <param name="arquivo">Arquivo XML a ser verificado</param>
        /// <param name="emissao">Data de emissão da NFe</param>
        /// <param name="extNFe">Extensão a ser substituida no arquivo</param>
        /// <param name="extArqProtNfe">Nova extensão a ser verificada</param>
        /// <returns>Se está na pasta de XML´s autorizados</returns>
        public bool EstaAutorizada(string arquivo, DateTime emissao, string extNFe, string extArqProtNfe)
        {
            int emp = Empresas.FindEmpresaByThread();

            string strNomePastaEnviado = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                            PastaEnviados.Autorizados.ToString() + "\\" +
                                            Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(emissao);
            return File.Exists(strNomePastaEnviado + "\\" + Functions.ExtrairNomeArq(arquivo, extNFe) + extArqProtNfe);
        }
        #endregion

        #region EstaDenegada()
        /// <summary>
        /// Verifica se o XML da nota fiscal já está na pasta de Notas Denegadas
        /// </summary>
        /// <param name="Arquivo">Arquivo XML a ser verificado</param>
        /// <param name="Emissao">Data de emissão da NFe</param>
        /// <returns>Se está na pasta de XML´s denegados</returns>
        public bool EstaDenegada(string Arquivo, DateTime Emissao)
        {
            int emp = Empresas.FindEmpresaByThread();
            string strNomePastaEnviado = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                            PastaEnviados.Denegados.ToString() + "\\" +
                                            Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(Emissao);
            return File.Exists(strNomePastaEnviado + "\\" + Functions.ExtrairNomeArq(Arquivo, Propriedade.ExtEnvio.Nfe) + Propriedade.ExtRetorno.Den);
        }
        #endregion

        #region ArquivosPasta()
        /// <summary>
        /// Monta uma lista dos arquivos existentes em uma determinada pasta
        /// </summary>
        /// <param name="strPasta">Pasta a ser verificada a existencia de arquivos</param>
        /// <param name="strMascara">Mascara dos arquivos a serem procurados</param>
        /// <returns>Retorna a lista dos arquivos da pasta</returns>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>15/04/2009</date>
        public List<string> ArquivosPasta(string strPasta, string strMascara)
        {
            //Criar uma Lista dos arquivos existentes na pasta
            List<string> lstArquivos = new List<string>();

            if(strPasta.Trim() != "" && Directory.Exists(strPasta))
            {
                string cError = "";
                try
                {
                    string[] filesInFolder = Directory.GetFiles(strPasta, strMascara);
                    foreach(string item in filesInFolder)
                    {
                        lstArquivos.Add(item);
                    }
                }
                catch(Exception ex)
                {
                    cError = ex.Message;
                }
                if(!string.IsNullOrEmpty(cError))
                {
                    this.GravarArqErroERP(string.Format(Propriedade.NomeArqERRUniNFe, DateTime.Now.ToString("yyyyMMddTHHmmss")), cError);
                    lstArquivos.Clear();
                }
            }

            return lstArquivos;
        }
        #endregion

        #region ConversaoNovaVersao()
        /// <summary>
        /// Conversões que são executadas quando atualizado o aplicativo.
        /// Alguns ajustes que são necessários serem executados automaticamente
        /// para evitar falhas no aplicativo
        /// </summary>
        public static string ConversaoNovaVersao(string cnpjEmpresa)    //danasa 20-9-2010
        {
            #region Estamos sem nenhuma conversão no momento
            return "";
            #endregion
        }
        #endregion

        #region CarregaEmpresa()
        /// <summary>
        /// Carrega as Emoresas que foram cadastradas e estão gravadas no XML
        /// </summary>
        /// <returns>Retorna uma ArrayList das empresas cadastradas</returns>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 28/07/2010
        /// </remarks>
        public static ArrayList CarregaEmpresa(bool sonfe)
        {
            ArrayList empresa = new ArrayList();

            string arqXML = Propriedade.NomeArqEmpresas;

            if(File.Exists(arqXML))
            {
                    //Carregar os dados do arquivo XML de configurações da Aplicação
                    int codEmp = 0;
                    XElement axml = XElement.Load(arqXML);
                    var b1 = axml.Descendants(NFe.Components.NFeStrConstants.Registro);
                    foreach (var item in b1)
                    {
                        string cnpj = item.Attribute(NFe.Components.NFeStrConstants.CNPJ).Value;
                        string nome = item.Element(NFe.Components.NFeStrConstants.Nome).Value;
                        string servico = "";
                        if (item.Attribute(NFe.Components.NFeStrConstants.Servico) != null)
                        {
                            servico = item.Attribute(NFe.Components.NFeStrConstants.Servico).Value;
                            if (!string.IsNullOrEmpty(servico))
                                servico = ((TipoAplicativo)Convert.ToInt16(servico)).ToString();
                        }
                        if (string.IsNullOrEmpty(servico))
                            servico = Propriedade.TipoAplicativo.ToString();

                        if (sonfe && servico.Equals(TipoAplicativo.Nfse.ToString()))
                            continue;

                        empresa.Add(new ComboElem
                        {
                            Valor = cnpj,
                            Codigo = codEmp,
                            Nome = nome + "  <" + servico + ">",
                            Servico = servico
                        });
                        codEmp++;
                    }
            }
            empresa.Sort(new OrdenacaoPorNome());

            return empresa;
        }
        #endregion
    }
}
