using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uninfe2
{
    public class uninfeDummy
    {
        public static DateTime UltimoAcessoConfiguracao { get; set; }
        public static Form1 mainForm { get; set; }

        public static bool TempoExpirou()
        {
            if (uninfeDummy.UltimoAcessoConfiguracao == DateTime.MinValue)
                return true;

            DateTime stopTime = DateTime.Now;
            TimeSpan elapsedTime = stopTime.Subtract(uninfeDummy.UltimoAcessoConfiguracao);
            return (int)elapsedTime.TotalMinutes > 60;
        }
    }

    public enum uninfeOpcoes
    {
        opConfiguracoes,
        opLogs,
        opSobre,
        opValidarXML
    }
}
