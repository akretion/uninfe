using System;
using System.IO;
using System.Reflection;

namespace Unimake.Business.DFe.Xml
{
    public static class AssemblyResolver
    {
        #region Public Methods

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