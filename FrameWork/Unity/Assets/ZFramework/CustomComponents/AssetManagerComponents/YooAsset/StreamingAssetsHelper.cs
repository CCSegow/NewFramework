using System.IO;

public class StreamingAssetsHelper
{
    public static bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }
}