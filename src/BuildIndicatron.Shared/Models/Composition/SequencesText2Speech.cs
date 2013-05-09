using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BuildIndicatron.Shared.Models.Composition
{
    public class SequencesText2Speech : Sequences
    {
        private string _text;
        private static IDictionary<string, string> _replace;

        public SequencesText2Speech() : base("Text2Speech")
        {
        }

        public string Text
        {
            get { return _text; }
            set { 
                
                _text = value;
                if (_text != null)
                {
                    foreach (var replacement in _replace)
                    {
                        _text = Regex.Replace(_text, replacement.Key, replacement.Value, RegexOptions.IgnoreCase);
                    }
                }
            }
        }

        public static void SetDefaultCleanAndReplace(IDictionary<string, string> replace)
        {
            _replace = replace;
        }

        public bool DisableTransform { get; set; }
    }
}