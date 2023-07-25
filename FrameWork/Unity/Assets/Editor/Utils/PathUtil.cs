using System.IO;
using UnityEngine;

public class PathUtil
{
    /// <summary>
    /// 项目地址 ,Unity工程文件夹 
    /// </summary>
    public static string ProjectPath => Application.dataPath.Substring(0, Application.dataPath.Length - 6);
        
    //本地服务器地址
    public static string LocalServerPath => ProjectPath + "../Server/";
        
   
    /// <summary>
    /// 获取文件所在文件目录（所在文件夹的路径）
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetDirectory(string path)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        return dir.Parent.FullName;
    }
}