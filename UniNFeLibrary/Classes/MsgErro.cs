using System;
using System.Collections.Generic;
using System.Text;

namespace UniNFeLibrary
{
    public enum ErroPadrao
    {
        ErroNaoDetectado = 0,
        FalhaInternet = 1
    }

    class MsgErro
    {
        public static string ErroPreDefinido(ErroPadrao Erro)
        {
            return MsgErro.ErroPreDefinido(Erro, string.Empty);
        }

        public static string ErroPreDefinido(ErroPadrao Erro, string ComplementoMensagem)
        {
            string Mensagem = string.Empty;

            switch (Erro)
            {
                case ErroPadrao.ErroNaoDetectado:
                    goto default;
                    break;

                case ErroPadrao.FalhaInternet:
                    Mensagem = "Sem conexão com a internet.";
                    break;

                default:
                    Mensagem = "Não foi possível identificar o erro.";
                    break;
            }

            Mensagem = ((int)Erro).ToString("0000000000") + " - " + Mensagem;

            if (ComplementoMensagem != string.Empty)
            {
                Mensagem = Mensagem + " " + ComplementoMensagem;
            }

            return Mensagem;
        }
    }
}
