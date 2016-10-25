using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFe.SAT.Contract
{
    interface IServico
    {        
        string ArquivoXML { get; set; }        
        string Enviar();
        string SaveResponse();
    }
}
