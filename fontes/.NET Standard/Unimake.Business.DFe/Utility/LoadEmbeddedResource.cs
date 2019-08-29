using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Unimake.Business.DFe.Utility
{
    internal class LoadEmbeddedResource
    {
        #region Private Methods

        /// <summary>
        /// Extrair recursos embitudos em um determinado Assembly (dll, exe, etc...)
        /// </summary>
        /// <param name="resourcePath">Caminho dos recursos embutidos dentro do Assembly</param>
        /// <param name="resources">Recursos embutidos que serão estraidos</param>
        /// <param name="assembly">Nome do Assembly onde os recursos estão embutidos</param>
        /// <param name="path">Pasta onde deve ser extraído os recursos</param>
        private void ExtrairRecurso(string resourcePath, string[] resources, Assembly assembly, string path)
        {
            var files = (from d in resources
                         where d.StartsWith(resourcePath)
                         select d);

            foreach(var item in files)
            {
                var filename = item.Substring(resourcePath.Length + 1);
                filename = Path.Combine(path, filename);

                if(!EmbeddedAssembly.Load(assembly, item, filename, true))
                {
                    throw new Exception("Falha ao extrair, do assembly, os recursos necessários para consumir os serviços. (Resource Path: " + resourcePath + ")");
                }
            }
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Extrair recursos embitudos nesta DLL
        /// </summary>
        public void Load()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var recursos = assembly.GetManifestResourceNames();
            if(recursos.GetLength(0) > 0)
            {
                var directoryAssembly = Path.GetDirectoryName(assembly.Location);

                //Estrair schemas de XML
                var resourcePath = "Unimake.Business.DFe.Xml.NFe.Schemas";
                var path = Path.Combine(directoryAssembly, @"Xml\\NFe\\Schemas");
                ExtrairRecurso(resourcePath, recursos, assembly, path);

                //Extrair os XMLs de configuração
                resourcePath = "Unimake.Business.DFe.Servicos.NFe.Config";
                path = Path.Combine(directoryAssembly, @"Servicos\\NFe\\Config");
                ExtrairRecurso(resourcePath, recursos, assembly, path);
            }
        }

        #endregion Public Methods
    }
}