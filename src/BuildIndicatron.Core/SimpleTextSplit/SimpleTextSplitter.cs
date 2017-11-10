using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BuildIndicatron.Core.SimpleTextSplit
{
    public class SimpleTextSplitter
    {
        public void Lookup(string empty)
        {
        }

        public static TextSplitter<TA> ApplyTo<TA>()
        {
            var textSplitter = new TextSplitter<TA> { };
            return textSplitter;
        }

        public static Regex ToRegEx(string help)
        {
            return new Regex("", RegexOptions.IgnoreCase);
        }
    }

    public class TextSplitter<T>
    {
        private List<Regex> _regexes;

        public TextSplitter()
        {
            _regexes = new List<Regex>();
        }


        public TextSplitter<T> Map(string values)
        {
            values = values.Replace("ANYTHING", ".*");
            values = values.Replace("WORD", "[A-z]+");
            values = "^" + values + "$";

            _regexes.Add(new Regex(values, RegexOptions.IgnoreCase));
            return this;
        }

        public TextSplitterResult<T> Process(string text)
        {
            return Process(text, Activator.CreateInstance<T>());
        }

        public bool IsMatch(string text)
        {
            return _regexes.Any(x => x.IsMatch(text));
        }

        public TextSplitterResult<T> Process(string text, T value)
        {
            foreach (var regex in _regexes)
            {
                var match = regex.Match(text);
                if (match.Success)
                {
                    ApplyNamedValues(regex, match, value);
                    return new TextSplitterResult<T>(match.Success, value);
                }
            }
            return new TextSplitterResult<T>(false, value);
        }

        private void ApplyNamedValues(Regex regex, Match match, T value)
        {
            var keyValuePairs = regex.GetGroupNames().ToDictionary(x => x.ToLower(), x => match.Groups[x].Value);
            var propertyInfos = typeof(T).GetProperties();
            foreach (var source in propertyInfos.Where(x => keyValuePairs.ContainsKey(x.Name.ToLower())))
            {
                var keyValuePair = keyValuePairs[source.Name.ToLower()];
                source.SetValue(value, keyValuePair);
            }
        }
    }


    public class TextSplitterResult<T>
    {
        private readonly T _value;
        private readonly bool _isMatch;

        public TextSplitterResult(bool isMatch, T value)
        {
            _value = value;
            _isMatch = isMatch;
        }

        public T Value
        {
            get { return _value; }
        }

        public bool IsMatch
        {
            get { return _isMatch; }
        }
    }
}