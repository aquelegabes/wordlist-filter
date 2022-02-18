using Mono.Options;

public static class OptionHelper
{
    internal static WordFilter[] Setup(
        string[] args,
        out List<string> extraArgs)
    {
        List<WordFilter> filters = new();

        var opts = new OptionSet
        {
            {
                "s|size=", "word length", 
                act => { 
                    filters.Add(new WordFilter { Type = TypeFilter.Size, Size = int.Parse(act) }); 
                } 
            },
            { 
                "n|notcontains=", "does not contains letter(s), separated by comma (',')",
                act => {
                    var letters = act.Split(',');
                    foreach (var letter in letters)
                        filters.Add(
                            new WordFilter
                            {
                                Type = TypeFilter.LetterNotContains,
                                Letter = char.Parse(letter)
                            }
                        );
                } 
            },
            { 
                "c|contains=", "contains letter(s), separated by comma (',')",
                act => {
                    var letters = act.Split(',');
                    foreach (var letter in letters)
                        filters.Add(
                            new WordFilter
                            {
                                Type = TypeFilter.LetterContains,
                                Letter = char.Parse(letter)
                            }
                        );
                }
            },
            { 
                "p|positioncontains=", "contains in letter in position, separated by comma (',')",
                act => {
                    var letter = act.Split(',')[0];
                    var position = act.Split(',')[1];
                    filters.Add(
                        new WordFilter
                        {
                            Type = TypeFilter.PositionContains,
                            Position = int.Parse(position),
                            Letter = char.Parse(letter)
                        }
                    );
                }
            },
            { 
                "o|positionnotcontains=", "does not contains letter in position, separated by comma (',')",
                act => {
                    var letter = act.Split(',')[0];
                    var position = act.Split(',')[1];
                    filters.Add(
                        new WordFilter
                        {
                            Type = TypeFilter.PositionNotContains,
                            Position = int.Parse(position),
                            Letter = char.Parse(letter)
                        }
                    );
                }
            }
        };

        extraArgs = opts.Parse(args);
        return filters.ToArray();
    }
}