using System;
using System.IO;
using UnityEngine;

public class IOUtils
{
    /// <summary>
    /// 拷贝文件夹
    /// </summary>
    /// <param name="fromFolder">需要复制的文件夹</param>
    /// <param name="toFolder">复制到哪个文件夹下</param>
    /// <param name="extent">指定文件类型，指定后只复制这些类型的文件</param>
    /// <param name="deleteOld">删除旧内容</param>
    public static void CopyFolder(string fromFolder,string toFolder,string extent = "*.bytes",bool deleteOld = true)
    {
        try
        {
            if (deleteOld)
            {
                if (Directory.Exists(toFolder))
                {
                    Directory.Delete(toFolder,true);
                }
            }

            if (!Directory.Exists(toFolder))
            {
                Directory.CreateDirectory(toFolder);
            }

            //复制当前目录下的所有文件
            string[] files = Directory.GetFiles(fromFolder,extent, SearchOption.AllDirectories);
            foreach (var file in files)
            {
                string name = file.Replace(fromFolder, toFolder);
                string dest = Path.Combine(toFolder, name);
                CopyFile(file, dest);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }        
    }
        
    /// <summary>
    /// 复制文件
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public static void CopyFile(string from, string to)
    {
        var dir = Path.GetDirectoryName(to);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        File.Copy(from, to, true);
    }        
}