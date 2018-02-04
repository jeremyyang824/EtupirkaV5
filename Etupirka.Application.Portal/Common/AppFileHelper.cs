using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Abp.IO;

namespace Etupirka.Application.Portal.Common
{
    /// <summary>
    /// 文件操作
    /// </summary>
    public static class AppFileHelper
    {
        public static IEnumerable<string> ReadLines(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public static void DeleteFilesInFolderIfExists(string folderPath, string fileNameWithoutExtension)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new ArgumentNullException("folderPath");

            var directory = new DirectoryInfo(folderPath);
            var files = directory.GetFiles(fileNameWithoutExtension + ".*", SearchOption.TopDirectoryOnly).ToList();
            foreach (var file in files)
            {
                FileHelper.DeleteIfExists(file.FullName);
            }
        }
    }
}
