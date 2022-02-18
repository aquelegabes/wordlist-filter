#load "WordList.cs"
#load "OptionHelper.cs"
#r "nuget: Mono.Options, 6.12.0.148"

using Mono.Options;

var wordList = new WordList("pt-br");
var filters = OptionHelper.Setup(Args.ToArray(), out var extra);

foreach (var word in wordList.Filter(filters).Words)
    WriteLine(word);