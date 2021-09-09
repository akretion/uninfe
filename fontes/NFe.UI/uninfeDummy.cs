using NFe.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NFe.UI
{
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
                "- O windows é inferior a versão 7 SP1 (Service Pack 1).\r\n" +
                "- O windows utilizado não é compatível com o TLS 1.2 (XP, Server 2003, Windows 8.0).\r\n" +
                "- O windows não está 100% atualizado, para atualizar execute o windows update mais de uma vez, garantindo que o sistema está completamente atualizado.\r\n" +
                "- Problema com o certificado digital.\r\n" +
                "- Necessidade de atualização da cadeia de certificados digitais.\r\n" +
                "- Falha de conexão com a internet.\r\n" +
                "- Falha nos servidores do SEFAZ.\r\n" +
                "- Antivírus ativado pode interferir na comunicação com os servidores da SEFAZ. Configure o antivírus para não verificar conexões seguras (https) e inclua a pasta do UniNFE nas exceções.\r\n" +
                "- Existe proxy na rede, o mesmo deve ser configurado na aba configurações. \r\n" +
                "- Existe firewall ativado impedindo a comunicação com os servidores da SEFAZ.\r\n\r\n" +
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
        /// <param name="soConsulta">Inserir somente os serviços que tem consulta status</param>
        /// <returns></returns>
        public static IList DatasouceTipoAplicativo(bool soConsulta)
        {
            ArrayList list = new ArrayList();

            if (!soConsulta)
                list.Add(new KeyValuePair<int, string>((int)TipoAplicativo.Todos, EnumHelper.GetDescription(TipoAplicativo.Todos)));

            list.Add(new KeyValuePair<int, string>((int)TipoAplicativo.Nfe, EnumHelper.GetDescription(TipoAplicativo.Nfe)));
            list.Add(new KeyValuePair<int, string>((int)TipoAplicativo.Cte, EnumHelper.GetDescription(TipoAplicativo.Cte)));
            list.Add(new KeyValuePair<int, string>((int)TipoAplicativo.MDFe, EnumHelper.GetDescription(TipoAplicativo.MDFe)));
            list.Add(new KeyValuePair<int, string>((int)TipoAplicativo.NFCe, EnumHelper.GetDescription(TipoAplicativo.NFCe)));
            list.Add(new KeyValuePair<int, string>((int)TipoAplicativo.SAT, EnumHelper.GetDescription(TipoAplicativo.SAT)));

            if (!soConsulta)
            {
                list.Add(new KeyValuePair<int, string>((int)TipoAplicativo.Nfse, EnumHelper.GetDescription(TipoAplicativo.Nfse)));
                list.Add(new KeyValuePair<int, string>((int)TipoAplicativo.EFDReinfeSocial, EnumHelper.GetDescription(TipoAplicativo.EFDReinfeSocial)));
                list.Add(new KeyValuePair<int, string>((int)TipoAplicativo.EFDReinf, EnumHelper.GetDescription(TipoAplicativo.EFDReinf)));
                list.Add(new KeyValuePair<int, string>((int)TipoAplicativo.eSocial, EnumHelper.GetDescription(TipoAplicativo.eSocial)));
                list.Add(new KeyValuePair<int, string>((int)TipoAplicativo.GNRE, EnumHelper.GetDescription(TipoAplicativo.GNRE)));
            }

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
                    try
                    {
                        ((MetroFramework.Interfaces.IMetroControl)control).Theme = mainForm.metroStyleManager1.Theme;
                        ((MetroFramework.Interfaces.IMetroControl)control).Style = mainForm.metroStyleManager1.Style;
                        //((MetroFramework.Interfaces.IMetroControl)control).StyleManager = mainForm.metroStyleManager1;
                    }
                    catch { }
                }
            }
        }
    }
}