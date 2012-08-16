using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFe.Service
{
    /// <summary>
    /// Assinar e Validar todos os arquivos XML de notas fiscais encontrados na pasta informada por parâmetro
    /// </summary>
    /// <param name="nfe">Objeto da classe ServicoNFe</param>
    /// <param name="arquivo">Arquivo a ser validado e assinado</param>

    public class TaskAssinarValidarNFe : TaskAbst
    {
        public override void Execute()
        {
        }

        ///
        /// será acessada para chamar o metodo AssinarValidarXMLNFe pela classe Processar.AssinarValidarNFe
        /// 
    }
}
