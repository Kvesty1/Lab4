using System;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

[Serializable]
public class TextFile
{
    public string FilePath { get; set; }
    public string Content { get; set; }

    public TextFile() { }

    public TextFile(string filePath, string content)
    {
        FilePath = filePath;
        Content = content;
    }

    public TextFile(string filePath)
    {
        FilePath = filePath;
        Content = File.ReadAllText(filePath);
    }

    // Заменённый BinarySerialize на Json в бинарной форме
    public void BinarySerialize(string path)
    {
        byte[] data = JsonSerializer.SerializeToUtf8Bytes(this);
        File.WriteAllBytes(path, data);
    }

    public static TextFile BinaryDeserialize(string path)
    {
        byte[] data = File.ReadAllBytes(path);
        return JsonSerializer.Deserialize<TextFile>(data) ?? throw new InvalidDataException("Десериализация не удалась");
    }

    public void XmlSerialize(string path)
    {
        using FileStream fs = new FileStream(path, FileMode.Create);
        XmlSerializer serializer = new XmlSerializer(typeof(TextFile));
        serializer.Serialize(fs, this);
    }

    public static TextFile XmlDeserialize(string path)
    {
        using FileStream fs = new FileStream(path, FileMode.Open);
        XmlSerializer serializer = new XmlSerializer(typeof(TextFile));
        return (TextFile)serializer.Deserialize(fs);
    }
}