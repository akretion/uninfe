using System;
using System.IO;
using System.Reflection;

namespace Unimake.Business.DFe.Xml
{
    /// <summary>
    /// Resolve as referências dos assemblies e ignora as versões
    /// </summary>
    public static class AssemblyResolver
    {
        #region Public Methods

        /// <summary>
        /// Resolve o assembly em questão ignorando a versão do mesmo
        /// </summary>
        /// <param name="sender">Objeto que iniciou o evento</param>
        /// <param name="args">Argumento que dispõe a versão esperada, assembly requerido e nome do assembly</param>
        /// <returns></returns>
        public static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            FileInfo fileInfo;

            try
            {
                var assemblyInfo = args.Name;
                var parts = assemblyInfo.Split(',');
                var name = parts[0];
                fileInfo = new FileInfo($"{name}.dll");
            }
            catch
            {
                return null;
            }

            return fileInfo?.Exists ?? false ? Assembly.LoadFile(fileInfo.FullName) : null;
        }

        #endregion Public Methods
    }
}