using System.Collections.Generic;
using System.IO;
using System.Linq;

public class FileScanner
{
    public static List<string> GetTextFiles(string directory)
    {
        return Directory.GetFiles(directory, "*.txt", SearchOption.AllDirectories).ToList();
    }
}