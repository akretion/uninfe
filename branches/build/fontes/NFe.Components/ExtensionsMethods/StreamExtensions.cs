using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NFe
{
    public static class StreamExtensions
    {
        public static long CopyTo(this Stream from, Stream to)
        {
            byte[] buffer = new byte[2048];
            int bytesRead;
            long totalBytes = 0;
            while ((bytesRead = from.Read(buffer, 0, buffer.Length)) > 0)
            {
                to.Write(buffer, 0, bytesRead);
                totalBytes += bytesRead;
            }
            return totalBytes;
        }

        public static string ToBase64(this FileInfo fi)
        {
            FileStream fs = fi.OpenRead();
            byte[] filebytes = new byte[fs.Length];
            fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
            string encodedData = Convert.ToBase64String(filebytes, Base64FormattingOptions.InsertLineBreaks);
            fs.Close();
            return encodedData;
        }
    }
}
