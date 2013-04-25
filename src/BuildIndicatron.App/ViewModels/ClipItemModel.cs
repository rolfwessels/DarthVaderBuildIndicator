using System;
using System.ComponentModel;
using System.IO;
using ServiiMetro.Core.Helpers;

namespace BuildIndicatron.App.ViewModels
{
    public class ClipItemModel : NotifyPropertyChangedBase
    {
        private string _name;
        private string _displayName;

        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value, () => Name , CallOnChange); }
        }

        private void CallOnChange()
        {
            DisplayName = Path.GetFileNameWithoutExtension(_name);
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { SetField(ref _displayName, value, "DisplayName"); }
        }
    }
}