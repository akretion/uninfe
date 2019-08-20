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
        /// <summary>
        /// Load Assembly, DLL from Embedded Resources into memory.
        /// </summary>
        /// <param name="embeddedResource">Embedded Resource string. Example: WindowsFormsApplication1.SomeTools.dll</param>
        /// <param name="fullFileName">Full File Name. Example: c:\test\test\SomeTools.dll</param>
        /// <param name="currentAssembly">The current assembly to load the resources files</param>
        public static bool Load(Assembly currentAssembly, string embeddedResource, string fullFileName)
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

            var fileOk = false;

            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                // Get the hash value from embedded DLL/assembly
                var fileHash = BitConverter.ToString(sha1.ComputeHash(ba)).Replace("-", string.Empty);

                // Define the temporary storage location of the DLL/assembly

                // Determines whether the DLL/assembly is existed or not
                fileOk = File.Exists(fullFileName);
            }

            // Create the file on disk
            if (!fileOk)
            {
                var fi = new FileInfo(fullFileName);
                fi.Directory.Create();
                fi = null;
                try
                {
                    File.WriteAllBytes(fullFileName, ba);
                }
                catch (IOException)
                {
                    //nada
                }
            }

            return true;
        }
    }
}