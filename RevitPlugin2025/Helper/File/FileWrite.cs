using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin2025.Helper.File
{
    public class FileWrite
    {

        public static void WriteForStream(string filePath, string inputData, out long startBytePosition, out long endBytePosition)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                startBytePosition = fs.Position;
                byte[] data = Encoding.UTF8.GetBytes(inputData);
                fs.Write(data, 0, inputData.Length);
                endBytePosition = fs.Position;
            }
        }
        public static void WriteForStream(string filePath, byte[] bytes, out long startBytePosition, out long endBytePosition)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                startBytePosition = fs.Position;
                fs.Write(bytes, 0, bytes.Length);
                endBytePosition = fs.Position;
            }
        }

        public static void WriteForStream(FileStream fs, string inputData, out long startBytePosition, out long endBytePosition)
        {

            startBytePosition = fs.Position;
            byte[] data = Encoding.UTF8.GetBytes(inputData);
            fs.Write(data, 0, inputData.Length);
            endBytePosition = fs.Position;
        }

    }
}
