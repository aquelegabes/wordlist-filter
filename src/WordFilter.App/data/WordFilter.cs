using System;

namespace WordFilter.App
{
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
}