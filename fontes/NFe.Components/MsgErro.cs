﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.Components
{
    public class MsgErro
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

                case ErroPadrao.FalhaInternet:
                    Mensagem = "Sem conexão com a internet.";
                    break;

                case ErroPadrao.FalhaEnvioXmlNFeWS:
                    Mensagem = "Não foi possível recuperar o número do recibo retornado pelo sefaz, pois ocorreu uma falha no exato momento que o XML foi enviado. "+
                        "Esta falha pode ter sido ocasionada por falha na internet ou erro no servidor do SEFAZ. "+
                        "Não tendo o número do recibo, a única forma de finalizar a nota fiscal é através da consulta situação da NF-e (-ped-sit.xml).";
                    break;

                case ErroPadrao.FalhaEnvioXmlWS:
                    Mensagem = "Não foi possível obter o retorno do sefaz, pois ocorreu uma falha no exato momento que o XML foi enviado. " +
                        "Esta falha pode ter sido ocasionada por falha na internet ou erro no servidor do SEFAZ. ";
                    break;

                case ErroPadrao.CertificadoVencido:
                    Mensagem = "Validade do certificado digital está vencida.";
                    break;

                case ErroPadrao.CertificadoNaoEncontrado:
                    Mensagem = "Certificado digital não localizado! Ou não foi informado nas configurações do UniNfe ou o mesmo está com falha.";
                    break;

                default:
                    Mensagem = "Não foi possível identificar o erro.";
                    break;
            }

            if (ComplementoMensagem != string.Empty)
            {
                Mensagem = Mensagem + " " + ComplementoMensagem;
            }

            return Mensagem;
        }
    }
}
