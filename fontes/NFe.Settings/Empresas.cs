﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using NFe.Components;

namespace NFe.Settings
{
    public class Empresas
    {
        /// <summary>
        /// Caminho das pastas com erro no caminho dos diretorios
        /// </summary>
        public static string ErroCaminhoDiretorio { get; set; }
        /// <summary>
        /// Propriedade para exibição  de mensagem de erro referente ao erro no caminho das pastas informadas
        /// </summary>
        public static bool ExisteErroDiretorio { get; set; }

        public static List<Empresa> Configuracoes = new List<Empresa>();

        /// <summary>
        /// Verifica se já existe alguma instância do UniNFe executando para os diretórios informados
        /// <para>Se existir, retorna uma mensagem com todos os diretórios que estão executando uma instânmcia do UniNFe</para>
        /// </summary>
        /// <param name="showMessage">Se verdadeiro, irá exibir a mensage e retornar o resultado
        /// <para>O padrão é verdadeiro</para></param>
        /// <returns></returns>
        public static void CanRun()
        {
            if (Empresas.Configuracoes == null || Empresas.Configuracoes.Count == 0)
            {
                return;
            }

            //se no diretório de envio existir o arquivo "nome da máquina.locked" o diretório já está sendo atendido por alguma instancia do UniNFe

            foreach (Empresa emp in Empresas.Configuracoes)
            {
                if (string.IsNullOrEmpty(emp.PastaBase))
                {
                    throw new NFe.Components.Exceptions.ProblemaExecucaoUniNFe("Pasta de envio da empresa '" + emp.Nome + "' não está definida.");
                }
                else
                {
                    string dir = emp.PastaBase;

                    if (!Directory.Exists(dir))
                    {
                        throw new NFe.Components.Exceptions.ProblemaExecucaoUniNFe("Pasta de envio da empresa '" + emp.Nome + "' não existe.");
                    }
                    else
                    {
                        string fileName = String.Format("{0}-{1}.lock", Propriedade.NomeAplicacao, Environment.MachineName);
                        string filePath = String.Format("{0}\\{1}", dir, fileName);

                        //se já existe um arquivo de lock e o nome do arquivo for diferente desta máquina
                        //não pode deixar executar

                        string fileLock = (from x in
                                               (from f in Directory.GetFiles(dir, "*" + Propriedade.NomeAplicacao + "*.lock")
                                                select new FileInfo(f))
                                           where !x.Name.Equals(fileName, StringComparison.InvariantCultureIgnoreCase)
                                           select x.FullName).FirstOrDefault();

                        if (!String.IsNullOrEmpty(fileLock))
                        {
                            FileInfo fi = new FileInfo(fileLock);

                            throw new NFe.Components.Exceptions.AppJaExecutando("Já existe uma instância do " + Propriedade.NomeAplicacao +
                                                                                 " em Execução que atende a conjunto de pastas: " + fi.Directory.FullName + " (*Incluindo subdiretórios).\r\n\r\n" +
                                                                                 "Nome da estação que está executando: " + fi.Name.Replace(Propriedade.NomeAplicacao + "-", "").Replace(".lock", ""));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Cria os arquivos de lock para os diretórios de envio que esta instância vai atender.
        /// <param name="clearIfExist">Se verdadeiro, irá excluir os arquivos existentes antes de recriar</param>
        /// </summary>
        public static void CreateLockFile(bool clearIfExist = false)
        {
            if (Empresas.Configuracoes == null || Empresas.Configuracoes.Count == 0) return;

            if (clearIfExist) ClearLockFiles(false);

            IEnumerable<string> diretorios = (from d in Empresas.Configuracoes
                                              select d.PastaBase);

            foreach (string dir in diretorios)
            {
                if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                {
                    string file = String.Format("{0}\\{1}-{2}.lock", dir, Propriedade.NomeAplicacao, Environment.MachineName);
                    FileInfo fi = new FileInfo(file);

                    using (StreamWriter sw = new StreamWriter(file, false)
                    {
                        AutoFlush = true
                    })
                    {
                        sw.WriteLine("Iniciado em: {0:dd/MM/yyyy hh:mm:ss}", DateTime.Now);
                        sw.WriteLine("Estação: {0}", Environment.MachineName);
                        sw.WriteLine("IP: {0}", Functions.GetIPAddress());
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
        }

        #region CarregaConfiguracao()
        /// <summary>
        /// Carregar as configurações de todas as empresas na coleção "Configuracoes" 
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 29/07/2010
        /// </remarks>
        public static void CarregaConfiguracao()
        {
            Empresas.Configuracoes.Clear();
            Empresas.ExisteErroDiretorio = false;
            Empresas.CriarPasta(true);

            if (File.Exists(Propriedade.NomeArqEmpresas))
            {
                try
                {
                    XElement axml = XElement.Load(Propriedade.NomeArqEmpresas);
                    var b1 = axml.Descendants(NFeStrConstants.Registro);
                    foreach (var item in b1)
                    {
                        Empresa empresa = new Empresa();

                        empresa.CNPJ = item.Attribute(TpcnResources.CNPJ.ToString()).Value;
                        empresa.Nome = item.Element(NFeStrConstants.Nome).Value.Trim();
                        empresa.Servico = Propriedade.TipoAplicativo;
                        if (item.Attribute(NFeStrConstants.Servico) != null)
                            empresa.Servico = (TipoAplicativo)Convert.ToInt16(item.Attribute(NFeStrConstants.Servico).Value.Trim());

                        string cArqErro = null;
                        bool erro = false;

                        try
                        {
                            int tipoerro = 0;
                            string rc = empresa.BuscaConfiguracao(ref tipoerro);
                            switch(tipoerro)
                            {
                                case 0:
                                    string uf = GetUF(empresa.UnidadeFederativaCodigo);
                                    if (uf != null)
                                        empresa.URLConsultaDFe = ConfiguracaoApp.CarregarURLConsultaDFe(uf);
                                    break;
                                case 1:
                                    erro = true;
                                    throw new Exception(rc);
                                case 2:
                                    throw new Exception(rc);
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                ///
                                /// nao acessar o metodo Auxiliar.GravarArqErroERP(string Arquivo, string Erro) já que nela tem a pesquisa da empresa
                                /// com base em "int emp = Empresas.FindEmpresaByThread();" e neste ponto ainda não foi criada
                                /// as thread's
                                cArqErro = CriaArquivoDeErro(empresa, cArqErro);

                                //Grava arquivo de ERRO para o ERP
                                File.WriteAllText(cArqErro, ex.Message);//, Encoding.Default);
                            }
                            catch { }
                        }
                        if (!erro)
                        {
                            ///
                            /// mesmo com erro, adicionar a lista para que o usuário possa altera-la
                            empresa.ChecaCaminhoDiretorio();

                            if (!string.IsNullOrEmpty(Empresas.ErroCaminhoDiretorio) && Empresas.ExisteErroDiretorio)
                            {
                                try
                                {
                                    if (cArqErro == null)
                                    {
                                        cArqErro = CriaArquivoDeErro(empresa, cArqErro);
                                    }
                                    //Grava arquivo de ERRO para o ERP
                                    File.AppendAllText(cArqErro, "Erros de diretorios:\r\n\r\n" + Empresas.ErroCaminhoDiretorio, Encoding.Default);
                                }
                                catch { }
                            }
                            Configuracoes.Add(empresa);
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }
            if (!Empresas.ExisteErroDiretorio)
                Empresas.CriarPasta(false);
        }
        #endregion

        public static string GetUF(int codigoUnidade)
        {
            if (codigoUnidade < 100)    //desconsidera empresa que é só NFS-e
                try
                {
                    return Propriedade.Estados.Where(p => p.CodigoMunicipio == codigoUnidade).Select(p => p.UF).First();
                }
                catch { }

            return null;
        }

        private static string CriaArquivoDeErro(Empresa empresa, string cArqErro)
        {
            if (string.IsNullOrEmpty(empresa.PastaXmlRetorno))
                cArqErro = Path.Combine(Propriedade.PastaGeralRetorno, string.Format(Propriedade.NomeArqERRUniNFe, DateTime.Now.ToString("yyyyMMddTHHmmss")));
            else
                cArqErro = Path.Combine(empresa.PastaXmlRetorno, string.Format(Propriedade.NomeArqERRUniNFe, DateTime.Now.ToString("yyyyMMddTHHmmss")));

            if (!Directory.Exists(Path.GetDirectoryName(cArqErro)))
            {
                cArqErro = Path.Combine(Propriedade.PastaLog, Path.GetFileName(cArqErro));
            }
            return cArqErro;
        }

        /// <summary>
        /// Exclui todos os arquivos de lock existentes nas configurações de pasta das empresas
        /// <param name="confirm">Se verdadeiro confirma antes de apagar os arquivos</param>
        /// </summary>
        public static bool ClearLockFiles(bool confirm = true)
        {
            if (Empresas.Configuracoes == null || Empresas.Configuracoes.Count == 0) return true;

            bool result = false;

            if (confirm && MessageBox.Show("Exclui os arquivos de \".lock\" configurados para esta instância?\r\nA aplicação será encerrada ao terminar a exclusão dos arquivos.\r\n\r\n\tTem certeza que deseja continuar? ", "Arquivos de .lock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return false;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                foreach (Empresa empresa in Empresas.Configuracoes)
                {
                    empresa.DeleteLockFile();
                }
                if (confirm)
                    MessageBox.Show("Arquivos de \".lock\" excluídos com sucesso.", "Aviso!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                result = true;
            }
            catch (Exception ex)
            {
                if (confirm)
                    MessageBox.Show(ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            return result;
        }

        #region CriarPasta()
        /// <summary>
        /// Criar as pastas para todas as empresas cadastradas e configuradas no sistema se as mesmas não existirem
        /// </summary>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>29/09/2009</date>
        public static void CriarPasta(bool onlygeral)
        {
            if (onlygeral)
            {
                if (!Directory.Exists(Propriedade.PastaGeral))
                    Directory.CreateDirectory(Propriedade.PastaGeral);

                if (!Directory.Exists(Propriedade.PastaGeralRetorno))
                    Directory.CreateDirectory(Propriedade.PastaGeralRetorno);

                if (!Directory.Exists(Propriedade.PastaGeralTemporaria))
                    Directory.CreateDirectory(Propriedade.PastaGeralTemporaria);

                if (!Directory.Exists(Propriedade.PastaLog))
                    Directory.CreateDirectory(Propriedade.PastaLog);
            }
            else
            {
                foreach (Empresa empresa in Empresas.Configuracoes)
                {
                    empresa.CriarPastasDaEmpresa();
                }
                Empresas.CriarSubPastaEnviado();
            }
        }
        #endregion

        #region CriarSubPastaEnviado()
        /// <summary>
        /// Criar as subpastas (Autorizados/Denegados/EmProcessamento) dentro da pasta dos XML´s enviados para todas as empresas cadastradas e configuradas
        /// </summary>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Date: 20/04/2010
        /// </remarks>
        private static void CriarSubPastaEnviado()
        {
            for (int i = 0; i < Empresas.Configuracoes.Count; i++)
            {
                Empresas.Configuracoes[i].CriarSubPastaEnviado();
            }
        }
        #endregion

        #region #10316
        /*
         * Solução para o problema do certificado do tipo A3
         * Marcelo
         * 29/07/2013
         */
        #region Reset certificado
        /// <summary>
        /// Reseta o certificado da empresa e recria o mesmo
        /// </summary>
        /// <param name="index">identificador da empresa</param>
        /// <returns></returns>
        public static X509Certificate2 ResetCertificado(int index)
        {
            Empresa empresa = Empresas.Configuracoes[index];
            if (empresa.UsaCertificado)
            {
                empresa.X509Certificado.Reset();

                Thread.Sleep(0);

                empresa.X509Certificado = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();

                //Ajustar o certificado digital de String para o tipo X509Certificate2
                X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
                X509Certificate2Collection collection1 = null;
                if (!string.IsNullOrEmpty(empresa.CertificadoDigitalThumbPrint))
                    collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindByThumbprint, empresa.CertificadoDigitalThumbPrint, false);
                else
                    collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindBySubjectDistinguishedName, empresa.Certificado, false);

                for (int i = 0; i < collection1.Count; i++)
                {
                    //Verificar a validade do certificado
                    if (DateTime.Compare(DateTime.Now, collection1[i].NotAfter) == -1)
                    {
                        empresa.X509Certificado = collection1[i];
                        break;
                    }
                }

                //Se não encontrou nenhum certificado com validade correta, vou pegar o primeiro certificado, porem vai travar na hora de tentar enviar a nota fiscal, por conta da validade. Wandrey 06/04/2011
                if (empresa.X509Certificado == null && collection1.Count > 0)
                    empresa.X509Certificado = collection1[0];
            }
            return empresa.X509Certificado;

        }
        #endregion
        #endregion

        #region FindConfEmpresa()
        /// <summary>
        /// Procurar o cnpj na coleção das empresas
        /// </summary>
        /// <param name="cnpj">CNPJ a ser pesquisado</param>
        /// <param param name="servico">Serviço a ser pesquisado</param>
        /// <returns>objeto empresa localizado, null se nada for localizado</returns>
        public static Empresa FindConfEmpresa(string cnpj, TipoAplicativo servico)
        {
            Empresa retorna = null;
            foreach (Empresa empresa in Empresas.Configuracoes)
            {
                if (empresa.CNPJ.Equals(cnpj) && empresa.Servico.Equals(servico))
                {
                    retorna = empresa;
                    break;
                }
            }
            return retorna;
        }
        #endregion

        #region FindConfEmpresaIndex()
        /// <summary>
        /// Procurar o cnpj na coleção das empresas
        /// </summary>
        /// <param name="cnpj">CNPJ a ser pesquisado</param>
        /// <param name="servico">Serviço a ser pesquisado</param>
        /// <returns>Retorna o index do objeto localizado ou null se nada for localizado</returns>
        public static int FindConfEmpresaIndex(string cnpj, TipoAplicativo servico)
        {
            int retorna = -1;

            for (int i = 0; i < Empresas.Configuracoes.Count; i++)
            {
                Empresa empresa = Empresas.Configuracoes[i];

                if (empresa.CNPJ.Equals(cnpj) && empresa.Servico.Equals(servico))
                {
                    retorna = i;
                    break;
                }
            }
            return retorna;
        }
        #endregion

        /// <summary>
        /// Retorna a empresa pela thread atual
        /// </summary>
        /// <returns></returns>
        public static int FindEmpresaByThread()
        {
            return Convert.ToInt32(Thread.CurrentThread.Name);
        }

        #region Valid()
        /// <summary>
        /// Retorna se o indice da coleção que foi pesquisado é valido ou não
        /// </summary>
        /// <param name="index">Indice a ser validado</param>
        /// <returns>Retorna true or false</returns>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 30/07/2010
        /// </remarks>
        public static bool Valid(int index)
        {
            bool retorna = true;
            if (index.Equals(-1))
                retorna = false;

            return retorna;
        }
        #endregion

        #region Valid()
        /// <summary>
        /// Retorna se o objeto da coleção que foi pesquisado é valido ou não
        /// </summary>
        /// <param name="empresa">Objeto da empresa</param>
        /// <returns>Retorna true or false</returns>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 30/07/2010
        /// </remarks>
        public static bool Valid(Empresa empresa)
        {
            bool retorna = true;
            if (empresa.Equals(null))
                retorna = false;

            return retorna;
        }
        #endregion

        #region verificaPasta
        public static void verificaPasta(Empresa empresa, XmlElement configElemento, string tagName, string descricao, bool isObrigatoria)
        {
            XmlNode node = configElemento.GetElementsByTagName(tagName)[0];
            if (node != null)
            {
                if (!isObrigatoria && node.InnerText.Trim() == "")
                    return;

                if (isObrigatoria && node.InnerText.Trim() == "")
                {
                    Empresas.ExisteErroDiretorio = true;
                    ErroCaminhoDiretorio += "Empresa: " + empresa.Nome + "   : \"" + descricao + "\"\r\n";
                }
                else
                    if (!Directory.Exists(node.InnerText.Trim()) && node.InnerText.Trim() != "")
                    {
                        Empresas.ExisteErroDiretorio = true;
                        ErroCaminhoDiretorio += "Empresa: " + empresa.Nome + "   Pasta: " + node.InnerText.Trim() + "\r\n";
                    }
            }
            else
            {
                if (isObrigatoria)
                {
                    Empresas.ExisteErroDiretorio = true;
                    ErroCaminhoDiretorio += "Empresa: " + empresa.Nome + "   : \"" + descricao + "\"\r\n";
                }
            }
        }
        #endregion

        #region Conta quantas empresas sao para NFe/CTe/MDFe/NFCe
        public static int CountEmpresasNFe
        {
            get
            {
                if (Configuracoes == null) return 0;
                return Configuracoes.Where(x => x.Servico != TipoAplicativo.Nfse).Count();
            }
        }
        #endregion

        #region Conta quantas empresas sao para NFSe
        public static int CountEmpresasNFse
        {
            get
            {
                if (Configuracoes == null) return 0;
                return Configuracoes.Where(x => x.Servico == TipoAplicativo.Nfse).Count();
            }
        }
        #endregion
    }
}
