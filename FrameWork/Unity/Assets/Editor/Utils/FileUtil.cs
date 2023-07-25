using System.IO;

public class FileUtil
{
    /// <summary>
    /// 比较两个文件的内容是否相同，读入到字节数组中比较(while循环比较字节数组)
    /// </summary>
    /// <param name="file1">文件1的路径</param>
    /// <param name="file2">文件2的路径</param>
    /// <returns></returns>
    public static bool CompareByByteArray(string file1, string file2)
    {
        const int BYTES_TO_READ = 1024 * 10;

        using (FileStream fs1 = File.Open(file1, FileMode.Open))
        using (FileStream fs2 = File.Open(file2, FileMode.Open))
        {
            if (fs1.Length != fs2.Length)
            {
                return false;
            }

            byte[] one = new byte[BYTES_TO_READ];
            byte[] two = new byte[BYTES_TO_READ];
                
            while (true)
            {
                int len1 = fs1.Read(one, 0, BYTES_TO_READ);
                int len2 = fs2.Read(two, 0, BYTES_TO_READ);
                int index = 0;
                while (index < len1 && index < len2)
                {
                    if (one[index] != two[index]) return false;
                    index++;
                }
                if (len1 == 0 || len2 == 0) break;
            }
        }

        return true;
    }
        
    public static string GetFileName(string path)
    {
        FileInfo dir = new FileInfo(path);
        return dir.Name.Split('.')[0];
    }
}