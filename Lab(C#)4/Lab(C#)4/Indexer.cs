using System.Collections.Generic;
using System.Linq;

public class Indexer
{
    private readonly List<string> _keywords;

    public Indexer(List<string> keywords)
    {
        _keywords = keywords.Select(k => k.ToLower()).ToList();
    }

    public KeywordIndex BuildIndex(List<TextFile> files)
    {
        var index = new KeywordIndex();

        foreach (var file in files)
        {
            string contentLower = file.Content.ToLower();

            foreach (var keyword in _keywords)
            {
                if (contentLower.Contains(keyword))
                    index.Add(keyword, file.FilePath);
            }
        }

        return index;
    }
}