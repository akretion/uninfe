using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using NFe.Components;
using NFe.Settings;

namespace NFe.UI
{
#if DEBUG
    public class aaaa : NFe.Service.TaskAbst
    {
        public override void Execute()
        {
        }

        public string nome(NFe.Components.Servicos servico, int uf, string versao)
        {
            this.Servico = servico;
            return xNomeClasseWS(this.Servico, uf, versao);
        }
    }
#endif

    public enum uninfeOpcoes
    {
        opCadastro,
        opConfiguracoes,
        opDanfe,
        opLogs,
        opMunicipios,
        opServicos,
        opSobre,
        opValidarXML
    }

    public enum uninfeOpcoes2
    {
        opRestartServico,
        opStopServico,
        opRestartTasks
    }

    public static class uninfeDummy
    {
        public static string vStatus = "Ocorreu uma falha ao tentar obter a solicitação junto ao SEFAZ.\r\n\r\n" +
                "O problema pode ter ocorrido por causa dos seguintes fatores:\r\n\r\n" +
                "- Problema com o certificado digital\r\n" +
                "- Necessidade de atualização da cadeia de certificados digitais\r\n" +
                "- Falha de conexão com a internet\r\n" +
                "- Falha nos servidores do SEFAZ\r\n\r\n" +
                "Afirmamos que a produtora do software não se responsabiliza por decisões tomadas e/ou execuções realizadas com base nas informações acima.\r\n\r\n";


        public static DateTime UltimoAcessoConfiguracao { get; set; }
        public static Form_Main mainForm { get; set; }
        public static uninfeOpcoes2 opServicos { get; set; }

        private static NFe.Components.XMLIniFile _xml;
        public static XMLIniFile xmlParams
        {
            get
            {
                if (_xml == null)
                    _xml = new NFe.Components.XMLIniFile(NFe.Components.Propriedade.NomeArqXMLParams);
                return _xml;
            }
        }

        /// <summary>
        /// TempoExpirou
        /// </summary>
        /// <returns></returns>
        public static bool TempoExpirou()
        {
            if (uninfeDummy.UltimoAcessoConfiguracao == DateTime.MinValue)
                return true;

            DateTime stopTime = DateTime.Now;
            TimeSpan elapsedTime = stopTime.Subtract(uninfeDummy.UltimoAcessoConfiguracao);
            return (int)elapsedTime.TotalMinutes > 60;
        }

        /// <summary>
        /// FmtCgcCpf
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="juridica"></param>
        /// <returns></returns>
        public static string FmtCnpjCpf(string Value, bool juridica)
        {
            string Result = Value.PadLeft(14, '0');

            if (juridica)
                Result = Result.Substring(0, 2) + "." +
                        Result.Substring(2, 3) + "." +
                        Result.Substring(5, 3) + "/" +
                        Result.Substring(8, 4) + "-" +
                        Result.Substring(12, 2);
            else
                Result = Result.Substring(3, 3) + "." +
                            Result.Substring(6, 3) + "." +
                            Result.Substring(9, 3) + "-" +
                            Result.Substring(12, 2);
            return Result;
        }

        /// <summary>
        /// DatasouceTipoAplicativo
        /// </summary>
        /// <returns></returns>
        public static IList DatasouceTipoAplicativo(bool includeservico)
        {
            ArrayList list = new ArrayList();

            if (includeservico)
                list.Add(new KeyValuePair<int, string>((int)TipoAplicativo.Todos, EnumHelper.GetDescription(TipoAplicativo.Todos)));

            list.Add(new KeyValuePair<int, string>((int)TipoAplicativo.Nfe, EnumHelper.GetDescription(TipoAplicativo.Nfe)));
            list.Add(new KeyValuePair<int, string>((int)TipoAplicativo.Cte, EnumHelper.GetDescription(TipoAplicativo.Cte)));
            list.Add(new KeyValuePair<int, string>((int)TipoAplicativo.MDFe, EnumHelper.GetDescription(TipoAplicativo.MDFe)));
            list.Add(new KeyValuePair<int, string>((int)TipoAplicativo.NFCe, EnumHelper.GetDescription(TipoAplicativo.NFCe)));

            if (includeservico)
                list.Add(new KeyValuePair<int, string>((int)TipoAplicativo.Nfse, EnumHelper.GetDescription(TipoAplicativo.Nfse)));

            return list;
        }

        /// <summary>
        /// ClearControls
        /// </summary>
        /// <param name="Xcontrol"></param>
        public static void ClearControls(System.Windows.Forms.Control Xcontrol, bool clear, bool inverteTheme)
        {
            if (Xcontrol == null)
                return;

            MetroFramework.MetroThemeStyle uTheme = uninfeDummy.mainForm.metroStyleManager1.Theme;//.uTheme;
            /* */
            //if (inverteTheme)
            //{
                //uTheme = MetroFramework.MetroThemeStyle.Light;
            //}
            //return;
            if (Xcontrol.GetType().IsSubclassOf(typeof(MetroFramework.Controls.MetroUserControl)) ||
                Xcontrol.GetType().IsSubclassOf(typeof(MetroFramework.Controls.MetroTabPage)) ||
                Xcontrol.GetType().IsSubclassOf(typeof(MetroFramework.Controls.MetroTabControl)) ||
                Xcontrol.GetType().IsSubclassOf(typeof(MetroFramework.Forms.MetroForm)))
            {
                if (Xcontrol.GetType().IsSubclassOf(typeof(MetroFramework.Forms.MetroForm)))
                {
                    ((MetroFramework.Interfaces.IMetroForm)Xcontrol).Theme = uTheme;
                    ((MetroFramework.Interfaces.IMetroForm)Xcontrol).Style = uninfeDummy.mainForm.metroStyleManager1.Style;//.uStyle;
                    //((MetroFramework.Interfaces.IMetroForm)Xcontrol).StyleManager = uninfeDummy.mainForm.StyleManager;
                }
                else
                {
                    ((MetroFramework.Interfaces.IMetroControl)Xcontrol).Theme = uTheme;
                    ((MetroFramework.Interfaces.IMetroControl)Xcontrol).Style = uninfeDummy.mainForm.metroStyleManager1.Style;//.uStyle;
                    //((MetroFramework.Interfaces.IMetroControl)Xcontrol).StyleManager = uninfeDummy.mainForm.StyleManager;
                }
            }
            /* */

            foreach (Control control in Xcontrol.Controls)
            {
                if (control.Controls.Count > 0)
                {
                    ClearControls(control, clear, inverteTheme);
                }

                if (clear)
                {
                    if (control.GetType().IsAssignableFrom(typeof(TextBox)))
                    {
                        NFe.Components.Functions.SetProperty(control, "Text", "");
                    }
                    if (control.GetType().IsAssignableFrom(typeof(MetroFramework.Controls.MetroTextBox)))
                    {
                        NFe.Components.Functions.SetProperty(control, "Text", "");
                    }
                    NFe.Components.Functions.SetProperty(control, "PromptText", "");
                }
                NFe.Components.Functions.SetProperty(control, "UseStyleColors", false);

                if (control is MetroFramework.Interfaces.IMetroControl)
                {
                    try {
                        ((MetroFramework.Interfaces.IMetroControl)control).Theme = mainForm.metroStyleManager1.Theme;
                        ((MetroFramework.Interfaces.IMetroControl)control).Style = mainForm.metroStyleManager1.Style;
                        //((MetroFramework.Interfaces.IMetroControl)control).StyleManager = mainForm.metroStyleManager1; 
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// printDanfe
        /// </summary>
        public static void printDanfe()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.RestoreDirectory = true;
                dlg.Filter = "Todos os arquivos|*" + Propriedade.ExtRetorno.ProcNFe +
                    ";*" + Propriedade.ExtRetorno.ProcCTe +
                    ";*" + Propriedade.ExtRetorno.ProcMDFe +
                    ";*_110111_01" + Propriedade.ExtRetorno.ProcEventoNFe +
                    ";*_110111_01" + Propriedade.ExtRetorno.ProcEventoCTe +
                    ";*" + Propriedade.ExtRetorno.Den +
                    ";*" + Propriedade.Extensao(Propriedade.TipoEnvio.EnvCancelamento).EnvioXML +
                    ";*" + Propriedade.ExtRetorno.ProcEventoCTe +
                    ";*" + Propriedade.ExtRetorno.ProcEventoNFe +
                    "|Arquivos da NFe/NFCe (*.*" + Propriedade.ExtRetorno.ProcNFe + ")|*" + Propriedade.ExtRetorno.ProcNFe +
                    "|Arquivos de cancelamento por evento (*.*_110111_01" + Propriedade.ExtRetorno.ProcEventoNFe + ", *.*_110111_01" + Propriedade.ExtRetorno.ProcEventoCTe + ")|*_110111_01" + Propriedade.ExtRetorno.ProcEventoNFe + ";*_110111_01" + Propriedade.ExtRetorno.ProcEventoCTe +
                    "|Arquivos de CCe (*.*" + Propriedade.ExtRetorno.ProcEventoNFe + ", *.*" + Propriedade.ExtRetorno.ProcEventoCTe + ")|*" + Propriedade.ExtRetorno.ProcEventoNFe + ";*" + Propriedade.ExtRetorno.ProcEventoCTe +
                    "|Arquivos de CTe (*.*" + Propriedade.ExtRetorno.ProcCTe + ")|*" + Propriedade.ExtRetorno.ProcCTe +
                    "|Arquivos de MDFe (*.*" + Propriedade.ExtRetorno.ProcMDFe + ")|*" + Propriedade.ExtRetorno.ProcMDFe;

                while (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        bool executou = false;

                        for (int i = 0; i < Empresas.Configuracoes.Count; i++)
                        {
                            Empresa empresa = Empresas.Configuracoes[i];
                            if (Path.GetDirectoryName(dlg.FileName).ToLower().StartsWith((empresa.PastaXmlEnviado + "\\" + PastaEnviados.Autorizados.ToString()).ToLower()) ||
                                Path.GetDirectoryName(dlg.FileName).ToLower().StartsWith((empresa.PastaXmlEnviado + "\\" + PastaEnviados.Denegados.ToString()).ToLower()))
                            {
                                if (string.IsNullOrEmpty(empresa.PastaExeUniDanfe))
                                {
                                    throw new Exception("Pasta contendo o UniDANFE não definida para a empresa: " + empresa.Nome);
                                }
                                NFe.Service.TFunctions.ExecutaUniDanfe(dlg.FileName, DateTime.Today, empresa);
                                //NFe.Interface.PrintDANFE.printDANFE(dlg.FileName, empresa);

                                executou = true;
                                break;
                            }
                        }
                        if (!executou)
                            throw new Exception("Arquivo deve estar na pasta de 'Autorizados/Denegados' da empresa");
                    }
                    catch (Exception ex)
                    {
                        MetroFramework.MetroMessageBox.Show(uninfeDummy.mainForm, ex.Message, "");
                    }
                }
            }
        }
    }
}
