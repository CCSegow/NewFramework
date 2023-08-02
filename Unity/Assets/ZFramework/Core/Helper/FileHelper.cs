using UnityEngine;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System;

namespace ZFramework {
    public class FileHelper
    {
        public static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            foreach (string folderPath in Directory.GetDirectories(sourceDirName, "*", SearchOption.AllDirectories))
            {
                if (!Directory.Exists(folderPath.Replace(sourceDirName, destDirName)))
                    Directory.CreateDirectory(folderPath.Replace(sourceDirName, destDirName));
            }

            foreach (string filePath in Directory.GetFiles(sourceDirName, "*.*", SearchOption.AllDirectories))
            {
                var fileDirName = Path.GetDirectoryName(filePath).Replace("\\", "/");
                var fileName = Path.GetFileName(filePath);
                string newFilePath = Path.Combine(fileDirName.Replace(sourceDirName, destDirName), fileName);

                File.Copy(filePath, newFilePath, true);
            }
        }

        public static string GetFileMD5(string path)
        {
            string fileMD5 = string.Empty;
            try
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    MD5 md5 = MD5.Create();
                    byte[] fileMD5Bytes = md5.ComputeHash(fs);
                    fileMD5 = BitConverter.ToString(fileMD5Bytes).Replace("-", "").ToLower();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            return fileMD5;
        }
    }
}
