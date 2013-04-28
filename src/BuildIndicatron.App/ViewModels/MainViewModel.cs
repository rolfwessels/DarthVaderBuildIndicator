using System.Collections.ObjectModel;
using ServiiMetro.Core.Helpers;

namespace BuildIndicatron.App.ViewModels
{
    public class MainViewModel : NotifyPropertyChangedBase
    {
        private string _text1;
        private string _text2;
        private string _text3;

        public MainViewModel()
        {
            Items = new ObservableCollection<ClipItemModel>();
            Text1 = "Luke I am your father";
        }

        public string Text1
        {
            get { return _text1; }
            set { SetField(ref _text1, value, "Text1"); }
        }

        public string Text2
        {
            get { return _text2; }
            set { SetField(ref _text2, value, "Text2"); }
        }

        public string Text3
        {
            get { return _text3; }
            set { SetField(ref _text3, value, "Text3"); }
        }

        /// <summary>
        /// A collection for ClipItemModel objects.
        /// </summary>
        public ObservableCollection<ClipItemModel> Items { get; private set; }

        
    }
}