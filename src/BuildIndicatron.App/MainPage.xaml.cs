using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BuildIndicatron.App.Api;
using BuildIndicatron.App.Core.Task;
using BuildIndicatron.App.ViewModels;
using BuildIndicatron.Shared.Models.ApiResponses;
using BuildIndicatron.Shared.Models.Composition;
using Microsoft.Phone.Controls;

namespace BuildIndicatron.App
{
    public partial class MainPage : PhoneApplicationPage
    {
        private MainViewModel _mainViewModel;
        private RobotApi _robotApi;
        private string _hostApi;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            _mainViewModel = new MainViewModel();
            _hostApi = "http://192.168.1.13:5000/";
            _robotApi = new RobotApi(_hostApi);
            DataContext = _mainViewModel;
            Loaded += OnLoaded;

        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var clips = _robotApi.GetClips();
            clips.ContinueWith(Action);
        }

        private void Action(Task<GetClipsResponse> task)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    _mainViewModel.Items.Clear();
                    foreach (var trigger in task.Result.Folders)
                    {
                        _mainViewModel.Items.Add(new ClipItemModel() { Name = trigger.Name });
                        foreach (var file in trigger.Files)
                        {
                            _mainViewModel.Items.Add(new ClipItemModel() { Name = trigger.Name+"/"+file });
                        }
                    }
                });
           
        }

        private void OnClipTapped(object sender, GestureEventArgs e)
        {
            var textBox = sender as TextBlock;
            if (textBox != null)
            {
                var clipItemModel = textBox.DataContext as ClipItemModel;
                _robotApi.Enqueue(new Choreography() { Sequences = new List<Sequences>() { new SequencesPlaySound() { File = clipItemModel.Name } } });
            }
        }
    }
}