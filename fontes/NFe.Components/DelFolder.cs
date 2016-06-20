using System;
using System.Collections.Generic;
using System.IO;

namespace NFe.Components
{
    internal class DeltreeData
    {
        public string PathName { get; set; }
        public List<string> Files { get; set; }

        public DeltreeData()
        {
            Files = new List<string>();
        }
    }

    public sealed class DeltreeMain
    {
        #region Fields
        private static readonly Stack<DeltreeData> StackTree = new Stack<DeltreeData>();
        private static bool _quiet;
        #endregion

        #region Begin delete tree
        public static void BeginDeleteTree(string path, bool quiet)
        {
            _quiet = quiet;

            if (!string.IsNullOrEmpty(path))
                if (Directory.Exists(path))
                    DeltreeThreadCallback(path);
        }

        private static void DeltreeThreadCallback(string path)
        {
            /* We first must get our directory and file structure */
            if (BuildDirectoryList(path))
            {
                DeleteTree();
                if (Directory.Exists(path))
                    System.Threading.Thread.Sleep(500);
            }
        }
        #endregion

        #region Build directory and file list
        private static bool BuildDirectoryList(string path)
        {
#if DEBUG
            Console.WriteLine(Environment.NewLine + "DELTREE deleting tree structure " + "'" + path + "' ..." + Environment.NewLine);
#endif
            try
            {
                var directories = new List<string> { path };
                directories.AddRange(Directory.GetDirectories(path, "*", SearchOption.AllDirectories));
                foreach (var d in directories)
                {
                    var dtData = new DeltreeData { PathName = d };
                    dtData.Files.AddRange(Directory.GetFiles(d, "*.*"));
                    StackTree.Push(dtData);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine("DELTREE error: " + ex.Message);
                return false;
#else
                if (!_quiet)
                    throw ex;
#endif
            }
            return true;
        }
        #endregion

        #region Delete tree
        private static void DeleteTree()
        {
            /* Nothing to do */
            if (StackTree.Count == 0)
            {
#if DEBUG
                Console.WriteLine("DELTREE error: Nothing to delete!");
#endif
                return;
            }
            var fileCount = 0;
            var directoryCount = 0;
            while (StackTree.Count > 0)
            {
                /* Get first item on the stack */
                var dtData = StackTree.Pop();
                /* Shouldn't happen .... */
                if (dtData == null) { continue; }
                if (dtData.Files.Count > 0)
                {
                    foreach (var f in dtData.Files)
                    {
#if DEBUG
                        if (!_quiet) { Console.WriteLine("Deleting file: " + f); }
#endif
                        try
                        {
                            File.Delete(f);
                        }
                        catch (Exception ex)
                        {
#if DEBUG
                            Console.WriteLine("DELTREE error: " + ex.Message);
                            return;
#else
                            if (!_quiet)
                                throw ex;
#endif
                        }
                        ++fileCount;
                    }
                }

#if DEBUG
                /* Remove directory */
                if (!_quiet)
                {
                    Console.WriteLine("Removing directory: " + dtData.PathName);
                }
#endif
                try
                {
                    Directory.Delete(dtData.PathName);
                }
                catch (Exception ex)
                {
#if DEBUG
                    Console.WriteLine("DELTREE error: " + ex.Message);
                    return;
#else
                    if (!_quiet)
                        throw ex;
#endif
                }
                ++directoryCount;
            }
#if DEBUG
            Console.WriteLine(
                (!_quiet ? Environment.NewLine : null) +
                "DELTREE removed {0} file(s) and {1} directory(ies) ({2} total objects removed)",
                fileCount, directoryCount, fileCount + directoryCount);
#endif
        }
        #endregion
    }
}
