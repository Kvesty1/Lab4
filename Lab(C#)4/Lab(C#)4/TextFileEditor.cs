using System;
using System.Collections.Generic;

public class TextFileEditor
{
    private TextFile _textFile;
    private readonly Stack<Memento> _history = new();

    public TextFileEditor(TextFile file)
    {
        _textFile = file;
        Save(); // начальное состояние
    }

    public void Display()
    {
        Console.WriteLine("==== Текущее содержимое ====");
        Console.WriteLine(_textFile.Content);
        Console.WriteLine("============================\n");
    }

    public void Edit(string newContent)
    {
        Save(); // сохранить текущее перед изменением
        _textFile.Content = newContent;
    }

    public void Undo()
    {
        if (_history.Count > 1)
        {
            _history.Pop(); // удалить текущее состояние
            var previous = _history.Peek();
            _textFile.Content = previous.Content;
        }
        else
        {
            Console.WriteLine("Нет изменений для отката.");
        }
    }

    public void Save()
    {
        _history.Push(new Memento(_textFile.Content));
    }

    public void SaveToFile()
    {
        _textFile.BinarySerialize(_textFile.FilePath);
        Console.WriteLine($"Файл сохранён в {_textFile.FilePath} (бин. сериализация)");
    }
}

public class Memento
{
    public string Content { get; }

    public Memento(string content)
    {
        Content = content;
    }
}
