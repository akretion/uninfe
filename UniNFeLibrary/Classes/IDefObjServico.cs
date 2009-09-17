using System;
using System.Collections.Generic;
using System.Text;

namespace UniNFeLibrary
{
    public interface IDefObjServico
    {
        #region Métodos
        void Consulta(ref object pServico, ref object pCabecMsg, ParametroEnvioXML oParam);
        void Cancelamento(ref object pServico, ref object pCabecMsg, ParametroEnvioXML oParam);
        void ConsultaCadastro(ref object pServico, ParametroEnvioXML oParam);
        void Inutilizacao(ref object pServico, ref object pCabecMsg, ParametroEnvioXML oParam);
        void Recepcao(ref object pServico, ref object pCabecMsg);
        void RetRecepcao(ref object pServico, ref object pCabecMsg);
        void StatusServico(ref object pServico, ref object pCabecMsg, ParametroEnvioXML oParam);
        #endregion
    }
}
