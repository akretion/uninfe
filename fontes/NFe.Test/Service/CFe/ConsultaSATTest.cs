using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unimake.SAT;

namespace NFe.Test.Service.CFe
{
    [TestClass]
    public class ConsultaSATTest
    {
        [TestMethod]
        public void ConsultaSAT()
        {
            Unimake.SAT.SAT sat = new Unimake.SAT.SAT(Unimake.SAT.Enuns.Fabricante.TANCA, "123456789");
            string result = sat.ConsultarSAT();

            if (string.IsNullOrEmpty(result))
                throw new Exception("Não foi possível se comunicar com o equipamento SAT.");
        }       
    }
}
