using Mono.Options;
using System.Collections.Generic;

namespace WordFilter.App.Helpers
{
    public struct OptionInfo
    {
        public WordFilter[] Filters { get; set; }
        public string Culture { get; set; }
    }

    public static class OptionHelper
    {
        private static void WithFilterHelper(
            string act,
            List<WordFilter> filters,
            TypeFilter type)
        {
            WordFilter filter = new()
            {
                Type = type,
                StringFilter = act
            };
            filter.Validate();
            filters.Add(filter);
        }

        private static void PositionFilterHelper(
            string act,
            List<WordFilter> filters,
            TypeFilter containsType)
        {
            var letter = act.Split(',')[0];
            var position = act.Split(',')[1];
            WordFilter filter = new()
            {
                Type = containsType,
                Position = int.Parse(position),
                Letter = char.Parse(letter)
            };
            filter.Validate();
            filters.Add(filter);
        }

        private static void ContainsFilterHelper(
            string act,
            List<WordFilter> filters,
            TypeFilter containsType)
        {
            var letters = act.Split(',');
            foreach (var letter in letters)
            {
                WordFilter filter = new()
                {
                    Type = containsType,
                    Letter = char.Parse(letter)
                };
                filter.Validate();
                filters.Add(filter);
            }
        }

        internal static OptionInfo Setup(
            string[] args,
            out List<string> extraArgs)
        {
            List<WordFilter> filters = new();
            string culture = string.Empty;

            var opts = new OptionSet
        {
            {
                "f|filename=", "sets file name to read words from",
                act => culture = act
            },
            {
                "s|size=", "word length",
                act => filters.Add(new WordFilter { Type = TypeFilter.Size, Size = int.Parse(act) })
            },
            {
                "n|notcontains=", "does not contains letter(s), separated by comma (',')",
                act => ContainsFilterHelper(act, filters, TypeFilter.LetterNotContains)
            },
            {
                "c|contains=", "contains letter(s), separated by comma (',')",
                act => ContainsFilterHelper(act, filters, TypeFilter.LetterContains)
            },
            {
                "p|positioncontains=", "contains in letter in position, separated by comma (',')(position starts at index 0)",
                act => PositionFilterHelper(act, filters, TypeFilter.PositionContains)
            },
            {
                "o|positionnotcontains=", "does not contains letter in position, separated by comma (',')(position starts at index 0)",
                act => PositionFilterHelper(act, filters, TypeFilter.PositionNotContains)
            },
            {
                "e|endswith=", "ends with specified string",
                act => WithFilterHelper(act, filters, TypeFilter.EndsWith)
            },
            {
                "b|beginswith=", "begins with specified string",
                act => WithFilterHelper(act, filters, TypeFilter.StartsWith)
            }
        };

            extraArgs = opts.Parse(args);
            filters.Sort();

            return new OptionInfo
            {
                Filters = filters.ToArray(),
                Culture = culture
            };
        }
    }
}