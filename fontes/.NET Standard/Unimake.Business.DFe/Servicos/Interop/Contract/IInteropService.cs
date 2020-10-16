﻿using System.Runtime.InteropServices;
using Unimake.Business.DFe.Xml;

namespace Unimake.Business.DFe.Servicos.Interop
{
    /// <summary>
    /// Os objetos que irão se comunicar via interop, deverão implementar esta interface,
    /// pois a mesma expõe os métodos necessários para a comunicação via interop.
    /// </summary>
    /// <typeparam name="TInteropType">Objeto que irá expor seus métodos para o interop</typeparam>
    public interface IInteropService<TInteropType>
         where TInteropType : XMLBase
    {
        #region Public Methods

        /// <summary>
        /// Realiza a execução da consulta com a Sefaz
        /// </summary>
        [ComVisible(false)]
        void Executar();

        /// <summary>
        /// Realiza a execução da consulta com a Sefaz passando os parâmetros no método quando usado pelo interop.
        /// <para>Pode ser usado normalmente, mesmo que não seja via interop.</para>
        /// </summary>
        /// <param name="configuracao">Configuração específica para o tipo definido em TInteropType</param>
        /// <param name="interopType">Tipo esperado para execução da consulta.</param>
        [ComVisible(true)]
        void Executar(TInteropType interopType, Configuracao configuracao);

        #endregion Public Methods
    }
}