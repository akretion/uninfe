using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

/*
 * fonte: http://www.codeproject.com/Articles/528178/Load-DLL-From-Embedded-Resource
 * Revisado por: Marcelo
 */

namespace Unimake.Business.DFe.Utility
{
    internal static class EmbeddedAssembly
    {
        private static string GetFileHash(string filename)
        {
            var sha = new SHA1CryptoServiceProvider();

            using (var stream = File.OpenRead(filename))
            {
                return BitConverter.ToString(sha.ComputeHash(stream));
            }
        }

        #region Public Methods

        /// <summary>
        /// Load Assembly, DLL from Embedded Resources into memory.
        /// </summary>
        /// <param name="embeddedResource">Embedded Resource string. Example: WindowsFormsApplication1.SomeTools.dll</param>
        /// <param name="fullFileName">Full File Name. Example: c:\test\test\SomeTools.dll</param>
        /// <param name="currentAssembly">The current assembly to load the resources files</param>
        /// <param name="overwrite">Sobrepor o recurso já existe na pasta?</param>
        public static bool Load(Assembly currentAssembly, string embeddedResource, string fullFileName, bool overwrite = false)
        {
            byte[] ba = null;

            using (var stm = currentAssembly.GetManifestResourceStream(embeddedResource))
            {
                // Either the file is not existed or it is not mark as embedded resource
                if (stm == null)
                {
                    return false;
                }

                // Get byte[] from the file from embedded resource
                ba = new byte[(int)stm.Length];
                stm.Read(ba, 0, (int)stm.Length);
            }

            var fileExist = false;
            var fileHash = "";

            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                // Get the hash value from embedded DLL/assembly
                fileHash = BitConverter.ToString(sha1.ComputeHash(ba));

                // Define the temporary storage location of the DLL/assembly

                // Determines whether the DLL/assembly is existed or not
                fileExist = File.Exists(fullFileName);
            }

            // Create the file on disk
            if (!fileExist ||
               (overwrite && GetFileHash(fullFileName) != fileHash))
            {
                CreateFile(fullFileName, ba);
            }

            return true;
        }

        private static void CreateFile(string fullFileName, byte[] ba)
        {
            var fi = new FileInfo(fullFileName);
            fi.Directory.Create();

            try
            {
                File.WriteAllBytes(fullFileName, ba);
            }
            catch (IOException)
            {
                //nada
            }
        }

        #endregion Public Methods
    }
}