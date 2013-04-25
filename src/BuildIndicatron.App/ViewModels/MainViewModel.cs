using System.Collections.ObjectModel;
using ServiiMetro.Core.Helpers;

namespace BuildIndicatron.App.ViewModels
{
    public class MainViewModel : NotifyPropertyChangedBase
    {
        public MainViewModel()
        {
            Items = new ObservableCollection<ClipItemModel>();
        }

        /// <summary>
        /// A collection for ClipItemModel objects.
        /// </summary>
        public ObservableCollection<ClipItemModel> Items { get; private set; }

        
    }
}