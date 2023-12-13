using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtfUnknown;

namespace Netpad.Utils
{
    public static class Util
    {
        public static string ReadTextFileWithEncoding(string filePath)
        {
            // 使用 UtfUnknown 库检测文件编码
            DetectionResult result = CharsetDetector.DetectFromFile(filePath);
            return File.ReadAllText(filePath, result?.Detected?.Encoding ?? Encoding.UTF8);
        }
    }
}
