using System;
using System.Collections.Generic;

public class KeywordIndex
{
    // Публичное свойство с индексом
    public Dictionary<string, List<string>> Index { get; } = new();

    public void Add(string keyword, string filePath)
    {
        if (!Index.ContainsKey(keyword))
            Index[keyword] = new List<string>();

        if (!Index[keyword].Contains(filePath))
            Index[keyword].Add(filePath);
    }

    public void Print()
    {
        foreach (var entry in Index)
        {
            Console.WriteLine($"Слово: {entry.Key}");
            foreach (var file in entry.Value)
            {
                Console.WriteLine($"  → {file}");
            }
        }
    }
}
