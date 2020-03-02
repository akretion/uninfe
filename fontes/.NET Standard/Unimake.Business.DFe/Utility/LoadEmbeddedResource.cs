using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unimake.Business.DFe.ConfigurationManager;

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
        private void ExtrairRecurso(string resourcePath, ref string[] resources, Assembly assembly, string path)
        {
            var filesLeftOver = new List<string>();

            for (var i = 0; i < resources.Length; i++)
            {
                if (resources[i].Substring(0, resourcePath.Length) == resourcePath)
                {
                    var item = resources[i];
                    var filename = Path.Combine(path, item.Replace(resourcePath, ""));

                    if (!EmbeddedAssembly.Load(assembly, item, filename, true))
                    {
                        throw new Exception("Falha ao extrair, do assembly, os recursos necessários para consumir os serviços. (Resource Path: " + resourcePath + ")");
                    }
                }
                else
                {
                    filesLeftOver.Add(resources[i]);
                }
            }

            resources = filesLeftOver.ToArray(); //Remover os recursos já extraidos
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Extrair recursos embitudos nesta DLL
        /// </summary>
        public void Load()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resources = assembly.GetManifestResourceNames();
            if (resources.GetLength(0) > 0)
            {
                #region Extrair schemas de XML
                /*
                 * IMPORTANTE: 
                 * Tem que extrair do maior namespace para o menor, ou seja, começando pelo NFCe, depois NFe para depois a pasta schemas, ou gera problema na descompactação. Wandrey 30/10/2019.
                 */

                var resourcePath = "Unimake.Business.DFe.Xml.Schemas.NFe.";
                ExtrairRecurso(resourcePath, ref resources, assembly, CurrentConfig.PastaSchemaNFe);

                resourcePath = "Unimake.Business.DFe.Xml.Schemas.CTe.";
                ExtrairRecurso(resourcePath, ref resources, assembly, CurrentConfig.PastaSchemaCTe);

                resourcePath = "Unimake.Business.DFe.Xml.Schemas.MDFe.";
                ExtrairRecurso(resourcePath, ref resources, assembly, CurrentConfig.PastaSchemaMDFe);

                resourcePath = "Unimake.Business.DFe.Xml.Schemas.";
                ExtrairRecurso(resourcePath, ref resources, assembly, CurrentConfig.PastaSchema);

                #endregion

                #region Extrair os XMLs de configuração

                /*
                 * IMPORTANTE: 
                 * Tem que extrair do maior namespace para o menor, ou seja, começando pelo NFCe, depois NFe para depois a pasta config, ou gera problema na descompactação. Wandrey 30/10/2019.
                 */

                resourcePath = "Unimake.Business.DFe.Servicos.Config.NFCe.";
                ExtrairRecurso(resourcePath, ref resources, assembly, CurrentConfig.PastaArqConfigNFCe);

                resourcePath = "Unimake.Business.DFe.Servicos.Config.NFe.";
                ExtrairRecurso(resourcePath, ref resources, assembly, CurrentConfig.PastaArqConfigNFe);

                resourcePath = "Unimake.Business.DFe.Servicos.Config.CTe.";
                ExtrairRecurso(resourcePath, ref resources, assembly, CurrentConfig.PastaArqConfigCTe);

                resourcePath = "Unimake.Business.DFe.Servicos.Config.MDFe.";
                ExtrairRecurso(resourcePath, ref resources, assembly, CurrentConfig.PastaArqConfigMDFe);

                resourcePath = "Unimake.Business.DFe.Servicos.Config.";
                ExtrairRecurso(resourcePath, ref resources, assembly, CurrentConfig.PastaArqConfig);

                #endregion
            }
        }

        #endregion Public Methods
    }
}