using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Components.Exceptions
{
    /// <summary>
    /// Serviço não disponível
    /// </summary>
    public class ServicoInexistenteException: Exception
    {
        public override string Message
        {
            get
            {
                return "Serviço não disponível ou não existe.";
            }
        }
    }
}
