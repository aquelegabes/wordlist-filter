public enum TypeFilter
{
    Default = 0,
    Size = 1,
    PositionContains = 2,
    PositionNotContains = 3,
    LetterContains = 4,
    LetterNotContains = 5,
    EndsWith = 6,
    StartsWith = 7,
}

public struct WordFilter : IComparable
{
    public bool? Validator
    {
        get
        {
            return this.Type switch
            {
                TypeFilter.LetterContains => true,
                TypeFilter.LetterNotContains => false,
                TypeFilter.PositionContains => true,
                TypeFilter.PositionNotContains => false,
                _ => null
            };
        }
    }

    public TypeFilter Type { get; set; }
    public int? Size { get; set; }
    public int? Position { get; set; }
    public char? Letter { get; set; }
    public string StringFilter { get; set; }

    public int CompareTo(object obj)
    {
        try
        {
            if (obj is null)
                return 1;

            WordFilter other = (WordFilter)obj;
            return this.Type.CompareTo(other.Type);
        }
        catch (System.Exception)
        {
            throw new ArgumentException(paramName: nameof(obj), message: $"Object is not a {nameof(WordFilter)} type.");
        }
    }

    public void Validate()
    {
        if (this.Type == TypeFilter.Default)
            throw new ArgumentException(paramName: nameof(this.Type), message: "Filter must have a type.");

        if (this.Type == TypeFilter.Size
            && (this.Size == null || this.Size <= 0))
            throw new ArgumentException(paramName: nameof(this.Size), message: "Filter with the size type must have a size valeu.");

        if (this.Type != TypeFilter.Size
            && this.Letter.HasValue
            && !char.IsLetter(this.Letter.Value))
            throw new ArgumentException(paramName: nameof(this.Letter), message: "This filter must have a letter.");

        if ((this.Type == TypeFilter.PositionContains
            || this.Type == TypeFilter.PositionNotContains)
            && (this.Position is null
                || this.Position < 0))
            throw new ArgumentException(paramName: nameof(this.Position), message: "Position type filter must have a position.");

        if ((this.Type == TypeFilter.StartsWith
            || this.Type == TypeFilter.EndsWith)
            && string.IsNullOrWhiteSpace(this.StringFilter))
            throw new ArgumentException(paramName: nameof(this.StringFilter), message: "When using 'ends with' or 'starts with' filter a string must be passed.");

    }
}

public class WordList
{
    private static string _basePath = Environment.CurrentDirectory;
    private static string _filePath = Path.Join(_basePath, "wordlist");
    private string[] _wordList;

    public string[] Words
    {
        get { return _wordList; }
    }

    public WordList(
        string[] wordList)
    {
        if (wordList is null
            || wordList.Length == 0)
            throw new ArgumentNullException(paramName: nameof(wordList), message: "Wordlist must not be null");

        this._wordList = wordList;
    }

    public WordList(
        string culture)
    {
        if (string.IsNullOrWhiteSpace(culture))
            throw new ArgumentNullException(paramName: nameof(culture), message: "Culture must not be a null value.");

        if (!culture.Contains(".txt"))
            culture += ".txt";

        string fullPath = Path.Join(_filePath, culture);
        _wordList = System.IO.File.ReadAllLines(fullPath, System.Text.Encoding.UTF8);
    }

    public WordList Filter(
        params WordFilter[] filters)
    {
        foreach (var filter in filters)
        {
            switch (filter.Type)
            {
                case TypeFilter.Size:
                    this.SizeFilter(filter.Size.Value);
                    break;
                case TypeFilter.LetterContains:
                case TypeFilter.LetterNotContains:
                    this.LetterFilter(filter.Letter.Value, filter.Validator.Value);
                    break;
                case TypeFilter.PositionContains:
                case TypeFilter.PositionNotContains:
                    this.PositionFilter(filter.Letter.Value, filter.Position.Value, filter.Validator.Value);
                    break;
                case TypeFilter.EndsWith:
                case TypeFilter.StartsWith:
                    this.WithFilter(filter.StringFilter, filter.Type);
                    break;
                default:
                    break;
            }
        }

        return this;
    }

    internal WordList WithFilter(
        string filter,
        TypeFilter type)
    {
        switch (type)
        {
            case TypeFilter.EndsWith:
                this._wordList =
                    this._wordList
                        .Where(_ => _.EndsWith(filter, StringComparison.OrdinalIgnoreCase))
                        .ToArray();
                break;
            case TypeFilter.StartsWith:
                this._wordList =
                    this._wordList
                        .Where(_ => _.StartsWith(filter, StringComparison.OrdinalIgnoreCase))
                        .ToArray();
                break;
            default:
                break;
        }

        return this;
    }

    internal WordList SizeFilter(
        int size)
    {
        this._wordList =
            this._wordList
                .Where(_ => _.Length == size)
                .ToArray();

        return this;
    }

    internal WordList PositionFilter(
        char letter,
        int position,
        bool validator)
    {
        this._wordList =
            this._wordList
                .Where(_ => (_[position] == letter) == validator)
                .ToArray();

        return this;
    }

    internal WordList LetterFilter(
        char letter,
        bool validator)
    {
        this._wordList =
            this._wordList
                .Where(_ => _.Contains(letter.ToString(), StringComparison.OrdinalIgnoreCase) == validator)
                .ToArray();

        return this;
    }
}