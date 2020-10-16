using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NFe.Components;
using System.Xml.Linq;

namespace NFe.Settings
{
    public class Auxiliar
    {
        #region GravarArqErroERP
        /// <summary>
        /// grava um arquivo de erro ao ERP
        /// </summary>
        /// <param name="Arquivo"></param>
        /// <param name="Erro"></param>
        public void GravarArqErroERP(string Arquivo, string Erro)
        {
            if (!string.IsNullOrEmpty(Arquivo) && !string.IsNullOrEmpty(Erro))
            {
                try
                {
                    ///
                    /// grava o erro na pasta de retorno geral do UniNFe caso a pasta de retorno da empresa nao exista
                    /// 
                    string fFolder = Propriedade.PastaGeralRetorno;

                    int emp = Empresas.FindEmpresaByThread();
                    if (emp >= 0)
                        if (!string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaXmlRetorno))
                            fFolder = Empresas.Configuracoes[emp].PastaXmlRetorno;

                    if (Directory.Exists(fFolder))
                    {
                        //Grava arquivo de ERRO para o ERP
                        string cArqErro = Path.Combine(fFolder, Path.GetFileName(Arquivo));
                        File.WriteAllText(cArqErro, Erro);//, Encoding.Default);
                    }
                }
                catch (Exception ex)
                {
                    WriteLog(ex.Message, true);
                }
            }
        }
        #endregion

        #region WriteLog()
        public static void WriteLog(string msg, bool gravarStackTrace)
        {
            if (string.IsNullOrEmpty(msg)) return;
#if DEBUG
            System.Diagnostics.Debug.WriteLine(msg);
#endif
            if (ConfiguracaoApp.GravarLogOperacoesRealizadas)
            {
                int emp = -1;
                if (Empresas.Configuracoes.Count != 0) //Tive que comparar aqui se tem empresas, pois quando não tem nenhuma empresa e tento consultar os certificados pela pasta geral, retorna um erro. Wandrey 02/02/2015
                {
                    try
                    {
                        emp = Empresas.FindEmpresaByThread();
                    }
                    catch { }
                }

                Functions.WriteLog(msg, gravarStackTrace, true, emp >= 0 ? Empresas.Configuracoes[emp].CNPJ + "_" + Empresas.Configuracoes[emp].Servico.ToString() : "");
            }
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

            if (File.Exists(Arquivo))
            {
                FileInfo oArquivo = new FileInfo(Arquivo);

                if (!string.IsNullOrEmpty(Empresas.Configuracoes[emp].PastaXmlErro) && Directory.Exists(Empresas.Configuracoes[emp].PastaXmlErro))
                {
                    string vNomeArquivo = Empresas.Configuracoes[emp].PastaXmlErro + "\\" + Functions.ExtrairNomeArq(Arquivo, ExtensaoArq) + ExtensaoArq;

                    Functions.Move(Arquivo, vNomeArquivo);

                    Auxiliar.WriteLog("O arquivo " + Arquivo + " foi movido para " + vNomeArquivo, true);

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

            return File.Exists(strNomePastaEnviado + Functions.ExtrairNomeArq(arquivo, extNFe) + extArqProtNfe);
        }
        #endregion

        #region EstaDenegada()
        /// <summary>
        /// Verifica se o XML da nota fiscal já está na pasta de Notas Denegadas
        /// </summary>
        /// <param name="Arquivo">Arquivo XML a ser verificado</param>
        /// <param name="Emissao">Data de emissão da NFe</param>
        /// <returns>Se está na pasta de XML´s denegados</returns>
        public bool EstaDenegada(string Arquivo, DateTime Emissao, string extNFe, string extArqProtNfe)
        {
            int emp = Empresas.FindEmpresaByThread();
            string strNomePastaEnviado = Empresas.Configuracoes[emp].PastaXmlEnviado + "\\" +
                                            PastaEnviados.Denegados.ToString() + "\\" +
                                            Empresas.Configuracoes[emp].DiretorioSalvarComo.ToString(Emissao);
            return File.Exists(strNomePastaEnviado + Functions.ExtrairNomeArq(Arquivo, extNFe) + extArqProtNfe);
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

            if (strPasta.Trim() != "" && Directory.Exists(strPasta))
            {
                string cError = "";
                try
                {
                    string[] filesInFolder = Directory.GetFiles(strPasta, strMascara);
                    foreach (string item in filesInFolder)
                    {
                        lstArquivos.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    cError = ex.Message;
                }
                if (!string.IsNullOrEmpty(cError))
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
        /// Carrega as Empresas que foram cadastradas e estão gravadas no XML
        /// </summary>
        /// <param name="sonfe"></param>
        /// <param name="pedidoSituacao">Se verdadeiro, está sendo chamado pela tela de pedido de situação</param>
        /// <returns>Retorna uma ArrayList das empresas cadastradas</returns>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 28/07/2010
        /// </remarks>
        public static ArrayList CarregaEmpresa(bool sonfe, bool pedidoSituacao = false)
        {
            ArrayList empresa = new ArrayList();

            string arqXML = Propriedade.NomeArqEmpresas;
            int codEmp = 0;

            if (File.Exists(arqXML))
            {
                //Carregar os dados do arquivo XML de configurações da Aplicação
                XElement axml = XElement.Load(arqXML);
                var b1 = axml.Descendants(NFeStrConstants.Registro);
                foreach (var item in b1)
                {
                    string cnpj = item.Attribute(TpcnResources.CNPJ.ToString()).Value;
                    string nome = item.Element(NFeStrConstants.Nome).Value;
                    string servico = "";
                    if (item.Attribute(NFeStrConstants.Servico) != null)
                    {
                        servico = item.Attribute(NFeStrConstants.Servico).Value;
                        if (!string.IsNullOrEmpty(servico))
                            servico = ((TipoAplicativo)Convert.ToInt16(servico)).ToString();
                    }
                    if (string.IsNullOrEmpty(servico))
                        servico = Propriedade.TipoAplicativo.ToString();

                    Empresa emp = new Empresa()
                    {
                        Nome = nome,
                        CNPJ = cnpj,
                        Servico = (TipoAplicativo)Enum.Parse(typeof(TipoAplicativo), servico, true)
                    };

                    if (File.Exists(emp.NomeArquivoConfig))
                    {
                        if (sonfe && (servico.Equals(TipoAplicativo.Nfse.ToString()) ||
                                      servico.Equals(TipoAplicativo.GNRE.ToString()) || 
                                      servico.Equals(TipoAplicativo.eSocial.ToString()) ||
                                      servico.Equals(TipoAplicativo.EFDReinf.ToString()) ||
                                      servico.Equals(TipoAplicativo.EFDReinfeSocial.ToString())))
                        {
                            codEmp++;
                            continue;
                        }

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
            }

            if (pedidoSituacao)
            {
                empresa.Add(new ComboElem
                {
                    Valor = string.Empty,
                    Codigo = codEmp,
                    Nome = "Todas as empresas",
                    Servico = TipoAplicativo.Todos.ToString()
                });
            }

            empresa.Sort(new OrdenacaoPorNome());

            return empresa;
        }
        #endregion
    }
}
