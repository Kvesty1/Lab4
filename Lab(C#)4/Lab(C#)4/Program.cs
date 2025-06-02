using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("Демонстрация текстовой файловой системы");
        Console.WriteLine("=====================\n");

        // Создаем тестовую директорию
        string directory = "Тестовые файлы";
        Directory.CreateDirectory(directory);

        // Универсальный текст
        string initialContent = "Все 3 файла созданы, тест прошел успешно.";

        // Создание .txt файла
        string txtPath = Path.Combine(directory, "Тест.txt");
        File.WriteAllText(txtPath, initialContent);

        // Создание .bin файла
        string binPath = Path.Combine(directory, "Тест.bin");
        TextFile binFile = new TextFile(binPath, initialContent);
        binFile.BinarySerialize(binPath);

        // Создание .xml файла
        string xmlPath = Path.Combine(directory, "Тест.xml");
        TextFile xmlFile = new TextFile(xmlPath, initialContent);
        xmlFile.XmlSerialize(xmlPath);

        Console.WriteLine("Созданы три файла с одинаковым содержанием:\n");
        Console.WriteLine($"- {txtPath}");
        Console.WriteLine($"- {binPath}");
        Console.WriteLine($"- {xmlPath}");
        Console.WriteLine();

    }
}