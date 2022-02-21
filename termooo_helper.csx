#load "WordList.cs"
#load "OptionHelper.cs"
#r "nuget: Mono.Options, 6.12.0.148"

using Mono.Options;

const string CLIPreMessage = "[*]:>";
try
{
    var optionInfo = OptionHelper.Setup(Args.ToArray(), out var extra);
    var wordList = new WordList(optionInfo.Culture);

    WriteLine($"{CLIPreMessage} Words that match your filter:");
    WriteLine($"{CLIPreMessage} {new string('-', 30)}");
    foreach (var word in wordList.Filter(optionInfo.Filters).Words)
        WriteLine($"{CLIPreMessage} {word}");
    WriteLine($"{CLIPreMessage} {new string('-', 30)}");
}
catch (Exception e)
{
    WriteLine($"{CLIPreMessage} {e.Message}");
}