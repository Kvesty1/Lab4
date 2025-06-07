using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("==== ОСНОВНОЕ МЕНЮ ====");
            Console.WriteLine("1. Открыть директорию с файлами");
            Console.WriteLine("0. Выход из программы");
            Console.Write("Выберите: ");
            string? mainChoice = Console.ReadLine();

            switch (mainChoice)
            {
                case "1":
                    HandleDirectoryMenu();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный выбор.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void HandleDirectoryMenu()
    {
        Console.Clear();
        Console.Write("Введите (Тестовые файлы) без скобок для выбора папки, как стандартную\n");
        Console.Write("Введите путь к директории с файлами: ");
        string? directory = Console.ReadLine();
        directory = Path.GetFullPath(directory);

        if (!Directory.Exists(directory))
        {
            Console.WriteLine("Директория не найдена.");
            Console.ReadKey();
            return;
        }

        List<string> filePaths = Directory.GetFiles(directory, "*.*", SearchOption.TopDirectoryOnly)
                                          .Where(p => p.EndsWith(".txt") || p.EndsWith(".bin") || p.EndsWith(".xml"))
                                          .ToList();

        if (filePaths.Count == 0)
        {
            Console.WriteLine("Нет подходящих файлов в директории.");
            Console.ReadKey();
            return;
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("\nФайлы:");
            for (int i = 0; i < filePaths.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(filePaths[i])}");
            }
            Console.WriteLine("0. Назад");

            Console.Write("\nВыберите файл для редактирования (номер): ");
            if (!int.TryParse(Console.ReadLine(), out int index))
            {
                Console.WriteLine("Неверный ввод.");
                Console.ReadKey();
                continue;
            }

            if (index == 0)
                return;

            if (index < 1 || index > filePaths.Count)
            {
                Console.WriteLine("Неверный номер файла.");
                Console.ReadKey();
                continue;
            }

            string selectedPath = filePaths[index - 1];
            TextFile file = selectedPath.EndsWith(".bin") ? TextFile.BinaryDeserialize(selectedPath)
                                : selectedPath.EndsWith(".xml") ? TextFile.XmlDeserialize(selectedPath)
                                : new TextFile(selectedPath);

            HandleFileMenu(file, filePaths);
        }
    }

    static void HandleFileMenu(TextFile file, List<string> allFiles)
    {
        TextFileEditor editor = new TextFileEditor(file);

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== Редактирование: {Path.GetFileName(file.FilePath)} ===");
            Console.WriteLine("1. Показать содержимое");
            Console.WriteLine("2. Редактировать");
            Console.WriteLine("3. Откатить");
            Console.WriteLine("4. Сохранить");
            Console.WriteLine("5. Индексация по ключевым словам");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите: ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.Clear();
                    editor.Display();
                    Console.ReadKey();
                    break;
                case "2":
                    Console.WriteLine("Введите новый текст:");
                    string? newText = Console.ReadLine();
                    if (newText != null)
                        editor.Edit(newText);
                    break;
                case "3":
                    editor.Undo();
                    Console.WriteLine("Откат выполнен.");
                    Console.ReadKey();
                    break;
                case "4":
                    editor.SaveToFile();
                    Console.ReadKey();
                    break;
                case "5":
                    Console.Write("Введите ключевые слова через запятую: ");
                    var keywords = Console.ReadLine()?.Split(',').Select(k => k.Trim()).ToList() ?? new();
                    Indexer indexer = new Indexer(keywords);
                    List<TextFile> files = allFiles.Select(path =>
                    {
                        if (path.EndsWith(".bin"))
                            return TextFile.BinaryDeserialize(path);
                        else if (path.EndsWith(".xml"))
                            return TextFile.XmlDeserialize(path);
                        else
                            return new TextFile(path);
                    }).ToList();

                    var indexResult = indexer.BuildIndex(files);
                    Console.WriteLine("Результаты индексации:");
                    foreach (var pair in indexResult.Index)
                    {
                        Console.WriteLine($"Ключ: {pair.Key} — Найдено в:");
                        foreach (var path in pair.Value)
                            Console.WriteLine("  - " + Path.GetFileName(path));
                    }
                    Console.ReadKey();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный выбор.");
                    Console.ReadKey();
                    break;
            }
        }
    }
}
