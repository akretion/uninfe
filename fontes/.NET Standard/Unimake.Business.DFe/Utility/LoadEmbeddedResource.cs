using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Unimake.Business.DFe.Utility
{
    public class LoadEmbeddedResource
    {
        public void Load()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var recursos = assembly.GetManifestResourceNames();
            if (recursos.GetLength(0) > 0)
            {
                var directoryAssembly = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

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

        private void ExtrairRecurso(string resourcePath, string[] resources, Assembly assembly, string path)
        {
            var files = (from d in resources
                         where d.StartsWith(resourcePath)
                         select d);

            foreach (var item in files)
            {
                var nameFile = item.Substring(resourcePath.Length + 1);

                if (!EmbeddedAssembly.Load(assembly, item, Path.Combine(path, nameFile)))
                {
                    throw new Exception("Falha ao extrair, do assembly, os recursos necessários para consumir os serviços. (Resource Path: " + resourcePath + ")");
                }
            }
        }
    }
}
