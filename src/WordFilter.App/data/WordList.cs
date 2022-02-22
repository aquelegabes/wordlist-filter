using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace WordFilter.App
{
    public class WordList
    {
        private static string _basePath = Environment.CurrentDirectory;
        private static string _filePath = Path.Join(_basePath, "wordlist");
        private IEnumerable<string> _wordList;

        public IEnumerable<string> Words
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
                            .Where(_ => _.EndsWith(filter, StringComparison.OrdinalIgnoreCase));
                    break;
                case TypeFilter.StartsWith:
                    this._wordList =
                        this._wordList
                            .Where(_ => _.StartsWith(filter, StringComparison.OrdinalIgnoreCase));
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
                    .Where(_ => _.Length == size);

            return this;
        }

        internal WordList PositionFilter(
            char letter,
            int position,
            bool validator)
        {
            this._wordList =
                this._wordList
                    .Where(_ => (_[position] == letter) == validator);

            return this;
        }

        internal WordList LetterFilter(
            char letter,
            bool validator)
        {
            this._wordList =
                this._wordList
                    .Where(_ => _.Contains(letter.ToString(), StringComparison.OrdinalIgnoreCase) == validator);

            return this;
        }
    }
}