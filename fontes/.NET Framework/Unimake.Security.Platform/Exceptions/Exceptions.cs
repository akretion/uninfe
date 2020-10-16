﻿using System;

namespace Unimake.Security.Platform.Exceptions
{
    /// <summary>
    /// Exceção ao carregar o certificado digital
    /// </summary>
    public class CarregarCertificadoException: Exception
    {
        #region Public Constructors

        /// <summary>
        /// Falha ao carregar certificado digital
        /// </summary>
        /// <param name="message">Mensagem da exceção</param>
        public CarregarCertificadoException(string message)
            : base(message)
        {
        }

        #endregion Public Constructors
    }

    /// <summary>
    /// Exceção ao trabalhar com certificado digital
    /// </summary>
    public class CertificadoDigitalException: Exception
    {
        #region Public Constructors

        /// <summary>
        /// Certificado digital não localizado ou com falhas
        /// </summary>
        public CertificadoDigitalException()
            : base("Certificado digital não localizado ou o mesmo está com falha.")
        {
        }

        #endregion Public Constructors
    }
}