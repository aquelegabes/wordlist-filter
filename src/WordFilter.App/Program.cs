using System;
using WordFilter.App.Helpers;

namespace WordFilter.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const string CLIPreMessage = "[*]:>";
            try
            {
                var optionInfo = OptionHelper.Setup(args, out var extra);
                var wordList = new WordList(optionInfo.Culture);

                Console.WriteLine($"{CLIPreMessage} Words that match your filter:");
                Console.WriteLine($"{CLIPreMessage} {new string('-', 30)}");
                foreach (var word in wordList.Filter(optionInfo.Filters).Words)
                    Console.WriteLine($"{CLIPreMessage} {word}");
                Console.WriteLine($"{CLIPreMessage} {new string('-', 30)}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{CLIPreMessage} {e.Message}");
            }
        }
    }
}
