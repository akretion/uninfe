using NFe.Components;
using NFe.Service;
using NFe.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace NFe.UI
{
    public partial class userPedidoSituacao : UserControl1
    {
        private int Emp;

        public userPedidoSituacao()
        {
            InitializeComponent();
        }

        public override void UpdateControles()
        {
            base.UpdateControles();

            this.cbServico.SelectedIndexChanged -= cbServico_SelectedIndexChanged;
            this.cbEmpresa.SelectedIndexChanged -= cbEmpresa_SelectedIndexChanged;
            try
            {
                this.cbAmbiente.DataSource = EnumHelper.ToList(typeof(TipoAmbiente), true, true);
                this.cbAmbiente.DisplayMember = "Value";
                this.cbAmbiente.ValueMember = "Key";

                var lista = EnumHelper.ToList(typeof(TipoEmissao), true, true, "2,4,5,9");

                this.cbEmissao.DataSource = lista;
                this.cbEmissao.DisplayMember = "Value";
                this.cbEmissao.ValueMember = "Key";

                this.cbServico.DataSource = uninfeDummy.DatasouceTipoAplicativo(true);
                this.cbServico.DisplayMember = "Value";
                this.cbServico.ValueMember = "Key";

                this.cbEmpresa.DataSource = Auxiliar.CarregaEmpresa(true, true);
                this.cbEmpresa.ValueMember = "Key";
                this.cbEmpresa.DisplayMember = NFeStrConstants.Nome;

                this.comboUf.DisplayMember = "nome";
                this.comboUf.ValueMember = "valor";
                this.comboUf.DataSource = Functions.CarregaEstados();

                int posicao = uninfeDummy.xmlParams.ReadValue(this.GetType().Name, "last_empresa", 0);
                if (posicao > (this.cbEmpresa.DataSource as System.Collections.ArrayList).Count)
                    posicao = 0;

                this.cbEmpresa.SelectedIndex = posicao;
                this.cbVersao.SelectedIndex = 0;
            }
            finally
            {
                this.cbServico.SelectedIndexChanged += cbServico_SelectedIndexChanged;
                this.cbEmpresa.SelectedIndexChanged += cbEmpresa_SelectedIndexChanged;

                cbEmpresa_SelectedIndexChanged(null, null);
                if (cbServico.SelectedValue == null)
                {                    
                    ChangeVersao(Empresas.Configuracoes[0].Servico);
                }
                else
                {
                    ChangeVersao((TipoAplicativo)cbServico.SelectedValue);
                }
            }
        }

        private void cbEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Emp = -1;
            this.cbServico.SelectedIndexChanged -= cbServico_SelectedIndexChanged;
            this.cbServico.Enabled = true;

            try
            {
                if (!this.cbEmpresa.SelectedValue.Equals("Todos"))
                {
                    var list = (this.cbEmpresa.DataSource as System.Collections.ArrayList)[this.cbEmpresa.SelectedIndex] as ComboElem;
                    this.Emp = Empresas.FindConfEmpresaIndex(list.Valor, EnumHelper.StringToEnum<TipoAplicativo>(list.Servico));
                    if (this.Emp >= 0)
                    {
                        uninfeDummy.xmlParams.WriteValue(this.GetType().Name, "last_empresa", this.cbEmpresa.SelectedIndex);
                        uninfeDummy.xmlParams.Save();

                        this.comboUf.SelectedValue = Functions.CodigoParaUF(Empresas.Configuracoes[this.Emp].UnidadeFederativaCodigo).Trim();

                        //Posicionar o elemento da combo Ambiente
                        this.cbAmbiente.SelectedValue = Empresas.Configuracoes[this.Emp].AmbienteCodigo;

                        //Exibir CNPJ da empresa
                        txtCNPJ.Text = uninfeDummy.FmtCnpjCpf(Empresas.Configuracoes[Emp].CNPJ, true);

                        //Posicionar o elemento da combo tipo de emissão
                        if(Empresas.Configuracoes[this.Emp].tpEmis == 1 || Empresas.Configuracoes[this.Emp].tpEmis == 6 ||
                            Empresas.Configuracoes[this.Emp].tpEmis == 7 || Empresas.Configuracoes[this.Emp].tpEmis == 8)
                        {
                            this.cbEmissao.SelectedValue = Empresas.Configuracoes[this.Emp].tpEmis;
                        }

                        this.ChangeVersao(Empresas.Configuracoes[this.Emp].Servico);

                        //Posicionar o elemento da combo tipo de servico
                        if (Empresas.Configuracoes[this.Emp].Servico != TipoAplicativo.Todos)
                        {
                            this.cbServico.SelectedValue = (int)Empresas.Configuracoes[this.Emp].Servico;
                        }
                        else
                        {
                            this.cbServico.SelectedValue = (int)TipoAplicativo.Nfe;
                            this.cbServico.Enabled = true;
                        }
                    }
                }
                else
                {
                    comboUf.SelectedValue = string.Empty;
                    txtCNPJ.Text = string.Empty;
                    cbServico.SelectedValue = string.Empty;
                }
            }
            catch (Exception ex)
            {
                this.cbServico.Enabled = false;
                MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "");
            }
            finally
            {
                this.cbServico.SelectedIndexChanged += cbServico_SelectedIndexChanged;
                this.buttonPesquisa.Enabled =
                    this.cbAmbiente.Enabled =
                    this.cbEmissao.Enabled =
                    this.cbVersao.Enabled =
                    this.cbServico.Enabled =
                    this.comboUf.Enabled = Emp >= 0;

                if (cbEmpresa.SelectedValue.Equals("Todos"))
                {
                    this.cbAmbiente.Enabled = true;
                    this.buttonPesquisa.Enabled = true;
                }
                else
                {
                }
            }
        }

        private void ChangeVersao(TipoAplicativo Servico)
        {
            switch (Servico)
            {
                case TipoAplicativo.Todos:
                    cbVersao.Enabled = cbServico.Enabled = true;
                    cbVersao.Items.Clear();
                    cbVersao.Items.AddRange(new object[] { "4.00", "3.00" });
                    cbVersao.SelectedItem = "4.00";
                    break;

                case TipoAplicativo.Nfe:
                    cbVersao.Enabled = true;
                    cbVersao.Items.Clear();
                    cbVersao.Items.AddRange(new object[] { "4.00" });
                    cbVersao.SelectedItem = "4.00";
                    break;

                case TipoAplicativo.NFCe:
                    cbVersao.Enabled = true;
                    cbVersao.Items.Clear();
                    cbVersao.Items.AddRange(new object[] { "4.00" });
                    cbVersao.SelectedItem = "4.00";
                    break;

                case TipoAplicativo.Cte:
                    cbVersao.Enabled = true;
                    cbVersao.Items.Clear();
                    cbVersao.Items.AddRange(new object[] { "3.00" });
                    cbVersao.SelectedItem = "3.00";
                    break;

                case TipoAplicativo.MDFe:
                    cbVersao.Enabled = true;
                    cbVersao.Items.Clear();
                    cbVersao.Items.AddRange(new object[] { "3.00" });
                    cbVersao.SelectedItem = "3.00";
                    break;

                case TipoAplicativo.SAT:
                    cbVersao.Enabled = false;
                    break;
            }
        }

        private void cbServico_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbServico.SelectedValue != null)
            {
                TipoAplicativo servico = (TipoAplicativo)cbServico.SelectedValue;
                this.ChangeVersao(servico);
            }
        }

        private void buttonPesquisa_Click(object sender, EventArgs e)
        {
            this.Refresh();
            txtMensagem.Clear();
            metroGridSituacao.Rows.Clear();

            bool todasEmpresas = cbEmpresa.SelectedValue.Equals("Todos");

            try
            {
                TipoEmissao tpEmis = (TipoEmissao)this.cbEmissao.SelectedValue;
                int amb = (int)cbAmbiente.SelectedValue;
                string versao = this.cbVersao.SelectedItem.ToString();
                TipoAplicativo servico = TipoAplicativo.Nulo;
                int cUF = 0;

                if (!todasEmpresas)
                {
                    servico = (TipoAplicativo)cbServico.SelectedValue;
                    cUF = Functions.UFParaCodigo(this.comboUf.SelectedValue.ToString());

                    switch (servico)
                    {
                        case TipoAplicativo.Cte:
                            if (tpEmis == TipoEmissao.teSVCAN)// this.cbEmissao.SelectedIndex == 4)
                                throw new Exception("CT-e não dispõe do tipo de contingência SVCAN.");
                            break;

                        case TipoAplicativo.Nfe:
                            if (tpEmis == TipoEmissao.teSVCSP)// this.cbEmissao.SelectedIndex == 3)
                                throw new Exception("NF-e não dispõe do tipo de contingência SVCSP.");
                            break;

                        case TipoAplicativo.MDFe:
                            if (tpEmis != TipoEmissao.teNormal)
                                throw new Exception("MDF-e só dispõe do tipo de emissão Normal.");
                            break;

                        case TipoAplicativo.NFCe:
                            if (tpEmis != TipoEmissao.teNormal)
                                throw new Exception("NFC-e só dispõe do tipo de emissão Normal.");
                        break;
                    }
                }

                Formularios.Wait.Show("Consulta a situação do serviço...");

                List<ConsultaRealizada> consultasRealizadas = new List<ConsultaRealizada>();

                if (todasEmpresas)
                {
                    tpEmis = TipoEmissao.teNormal;

                    for (int i = 0; i < Empresas.Configuracoes.Count; i++)
                    {
                        int emp = Empresas.FindConfEmpresaIndex(Empresas.Configuracoes[i].CNPJ, Empresas.Configuracoes[i].Servico);
                        cUF = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;

                        switch (Empresas.Configuracoes[emp].Servico)
                        {
                            case TipoAplicativo.Todos:
                                GerarConsulta(emp, TipoAplicativo.Nfe, (int)tpEmis, cUF, amb, "4.00", consultasRealizadas);
                                GerarConsulta(emp, TipoAplicativo.NFCe, (int)tpEmis, cUF, amb, "4.00", consultasRealizadas);
                                GerarConsulta(emp, TipoAplicativo.MDFe, (int)tpEmis, cUF, amb, "3.00", consultasRealizadas);
                                GerarConsulta(emp, TipoAplicativo.Cte, (int)tpEmis, cUF, amb, "3.00", consultasRealizadas);
                                break;

                            case TipoAplicativo.Nfe:
                                GerarConsulta(emp, TipoAplicativo.Nfe, (int)tpEmis, cUF, amb, "4.00", consultasRealizadas);
                                break;

                            case TipoAplicativo.Cte:
                                GerarConsulta(emp, TipoAplicativo.Cte, (int)tpEmis, cUF, amb, "3.00", consultasRealizadas);
                                break;

                            case TipoAplicativo.MDFe:
                                GerarConsulta(emp, TipoAplicativo.MDFe, (int)tpEmis, cUF, amb, "3.00", consultasRealizadas);
                                break;

                            case TipoAplicativo.NFCe:
                                GerarConsulta(emp, TipoAplicativo.NFCe, (int)tpEmis, cUF, amb, "4.00", consultasRealizadas);
                                break;
                        }
                    }
                }
                else
                {
                    GerarConsulta(Emp, servico, (int)tpEmis, cUF, amb, versao, consultasRealizadas);
                }

                //Demonstrar o status do serviço
                for (int i = 0; i < consultasRealizadas.Count; i++)
                {
                    string nomeEmpresa = consultasRealizadas[i].Empresa.Nome;
                    string tipoServico = consultasRealizadas[i].Servico.ToString();
                    string uf = Functions.CodigoParaUF(consultasRealizadas[i].Empresa.UnidadeFederativaCodigo);
                    string result = VerStatusServico(consultasRealizadas[i].ArquivoRetorno, consultasRealizadas[i].Emp);

                    metroGridSituacao.Rows.Add(new object[] { nomeEmpresa, uf, tipoServico, result });
                }
            }
            catch (Exception ex)
            {
                Formularios.Wait.Close();
                txtMensagem.Text = ex.Message;
            }
        }

        private void metroGridSituacao_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            txtMensagem.Text = metroGridSituacao.SelectedRows[0].Cells[columnSitucao.Index].Value.ToString();
        }

        #region VerStatusServico()

        /// <summary>
        /// Verifica e retorna o Status do Servido da NFE. Para isso este método gera o arquivo XML necessário
        /// para obter o status do serviõ e faz a leitura do XML de retorno, disponibilizando uma string com a mensagem
        /// obtida.
        /// </summary>
        /// <returns>Retorna uma string com a mensagem obtida do webservice de status do serviço da NFe</returns>
        /// <example>string vPastaArq = this.CriaArqXMLStatusServico();</example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2008</date>
        private string VerStatusServico(string XmlNfeDadosMsg, int emp)
        {
            string result = string.Empty;

            string ArqXMLRetorno = Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                      Functions.ExtrairNomeArq(XmlNfeDadosMsg, Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML) +
                      Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).RetornoXML;//.ExtRetorno.Sta_XML;

            string ArqERRRetorno = Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                      Functions.ExtrairNomeArq(XmlNfeDadosMsg, Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML) +
                      Propriedade.ExtRetorno.Sta_ERR;

            try
            {
                result = (string)EnviaArquivoERecebeResposta(ArqXMLRetorno, ArqERRRetorno);
            }
            finally
            {
                Functions.DeletarArquivo(ArqERRRetorno);
                Functions.DeletarArquivo(ArqXMLRetorno);

                Formularios.Wait.Close();
            }
            return result;
        }

        /// <summary>
        /// Função Callback que analisa a resposta do Status do Servido
        /// </summary>
        /// <param name="elem"></param>
        /// <by>Marcos Diez</by>
        /// <date>30/8/2009</date>
        /// <returns></returns>
        private static string ProcessaStatusServico(string cArquivoXML)//XmlTextReader elem)
        {
            string rst = "Erro na leitura do XML " + cArquivoXML;
            XmlTextReader elem = new XmlTextReader(cArquivoXML);
            try
            {
                while (elem.Read())
                {
                    if (elem.NodeType == XmlNodeType.Element)
                    {
                        if (elem.Name == TpcnResources.xMotivo.ToString())
                        {
                            elem.Read();
                            rst = elem.Value;
                            break;
                        }
                    }
                }
            }
            finally
            {
                elem.Close();
            }

            return rst;
        }

        /// <summary>
        /// Envia um arquivo para o webservice da NFE e recebe a resposta.
        /// </summary>
        /// <returns>Retorna uma string com a mensagem obtida do webservice de status do serviço da NFe</returns>
        /// <example>string vPastaArq = this.CriaArqXMLStatusServico();</example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2009</date>
        private object EnviaArquivoERecebeResposta(string arqXMLRetorno, string arqERRRetorno)
        {
            object vStatus = uninfeDummy.vStatus;
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
                Application.DoEvents();

                if (elapsedMillieconds >= 60000)
                {
                    break;
                }

                if (File.Exists(arqXMLRetorno))
                {
                    if (!Functions.FileInUse(arqXMLRetorno))
                    {
                        try
                        {
                            //Ler o status do serviço no XML retornado pelo WebService
                            try
                            {
                                vStatus = ProcessaStatusServico(arqXMLRetorno);
                            }
                            catch (Exception ex)
                            {
                                vStatus = ex.Message;
                                break;
                                //Se não conseguir ler o arquivo vai somente retornar ao loop para tentar novamente, pois
                                //pode ser que o arquivo esteja em uso ainda.
                            }

                            //Detetar o arquivo de retorno
                            try
                            {
                                FileInfo oArquivoDel = new FileInfo(arqXMLRetorno);
                                oArquivoDel.Delete();
                                break;
                            }
                            catch
                            {
                                //Somente deixa fazer o loop novamente e tentar deletar
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                else if (File.Exists(arqERRRetorno))
                {
                    //Retornou um arquivo com a extensão .ERR, ou seja, deu um erro,
                    //futuramente tem que retornar esta mensagem para a MessageBox do usuário.

                    //Detetar o arquivo de retorno
                    try
                    {
                        vStatus += File.ReadAllText(arqERRRetorno, Encoding.Default);
                        File.Delete(arqERRRetorno);
                        break;
                    }
                    catch
                    {
                        //Somente deixa fazer o loop novamente e tentar deletar
                    }
                }
                Thread.Sleep(100);
            }
            //Retornar o status do serviço
            return vStatus;
        }

        #endregion VerStatusServico()

        private void GerarConsulta(int emp, TipoAplicativo servico, int tpEmis, int cUF, int amb, string versao, List<ConsultaRealizada> consultasRealizadas)
        {
            GerarXML oGerar = new GerarXML(emp);

            string XmlNfeDadosMsg = Empresas.Configuracoes[emp].PastaXmlEnvio + "\\" + oGerar.StatusServico(servico, tpEmis, cUF, amb, versao);

            consultasRealizadas.Add(new ConsultaRealizada
            {
                ArquivoRetorno = XmlNfeDadosMsg,
                Empresa = Empresas.Configuracoes[emp],
                Emp = emp,
                Servico = servico
            });

            Thread.Sleep(1000);
        }
    }

    public class ConsultaRealizada
    {
        public string ArquivoRetorno { get; set; }
        public Empresa Empresa { get; set; }
        public int Emp { get; set; }
        public TipoAplicativo Servico { get; set; }
    }
}