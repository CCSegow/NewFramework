using System.Diagnostics;
using System.Net.Mime;
using UnityEngine;

public class BatUtil
{
    static Process CreateProcess(string fileName,string args,string workingDir)
    {
        ProcessStartInfo info = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = args,
            CreateNoWindow = false,
            UseShellExecute = true,
            WorkingDirectory = workingDir,
            RedirectStandardError = false,
            RedirectStandardInput = false,
            RedirectStandardOutput = false
        };
        return Process.Start(info);
    }

    public static void RunBat(string fileName,string args,string workingDir = "")
    {
        var p = CreateProcess(fileName,args,workingDir);
        p.Close();
    }

    /// <summary>
    /// 校正路径格式
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    static string CorrectionPath(string path)
    {
        path = path.Replace("/", "\\");
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            path = path.Replace("/", "\\");
        }

        return path;
    }
}