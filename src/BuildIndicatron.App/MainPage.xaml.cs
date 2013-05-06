using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using BuildIndicatron.App.Core.Task;
using BuildIndicatron.App.ViewModels;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Shared.Models.ApiResponses;
using BuildIndicatron.Shared.Models.Composition;
using Microsoft.Phone.Controls;

namespace BuildIndicatron.App
{
    public partial class MainPage : PhoneApplicationPage
    {
        const int pinRed = 27;
        const int pinBlue = 11;
        const int pinGreen = 9;
        const int gRed = 17;
        const int gGreen = 24; 
        private MainViewModel _mainViewModel;
        private RobotApi _robotApi;
        private string _hostApi;
        private List<object> _isOn;
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
            _isOn = new List<object>();
            
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
                    if (task.Result != null)
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

        private void OnGlowControlTap(object sender, GestureEventArgs e)
        {
            if (sender == LsRed)
            {
                _robotApi.GpIoOutput(pinRed, Switch(LsRed));
            }
            else if (sender == LsGreen)
            {
                _robotApi.GpIoOutput(pinGreen, Switch(LsGreen));
            }
            else if (sender == LsBlue)
            {
                _robotApi.GpIoOutput(pinBlue, Switch(LsBlue));
            }
            else if (sender == GlGreen)
            {
                _robotApi.GpIoOutput(gGreen, Switch(GlGreen));
            }
            else if (sender == GlRed)
            {
                _robotApi.GpIoOutput(gRed, Switch(GlRed));
            }
        }

        private bool Switch(Rectangle storedValue)
        {
            if (_isOn.Contains(storedValue))
            {
                _isOn.Remove(storedValue);
            }
            else
            {
                _isOn.Add(storedValue);
            }
            return IsOn(storedValue);
        }

        private bool IsOn(Rectangle storedValue)
        {
            return _isOn.Contains(storedValue);
        }

        private void OnJokeTap(object sender, GestureEventArgs e)
        {
            _robotApi.Enqueue(new Choreography()
                {
                    Sequences = new List<Sequences>()
                        {
                            new SequencesGpIo(pinBlue, true),
                            new SequencesOneLiner(),
                            new SequencesPlaySound() {File = "Stop/jabba_laugh.wav"},
                            new SequencesGpIo(pinBlue, false),
                        }
                });
        }

        private IEnumerable<Sequences> SequencesText2Speeches(bool disableTransform)
        {
            yield return new SequencesGpIo(pinBlue, disableTransform);
            foreach (var s in new[] { _mainViewModel.Text1, _mainViewModel.Text2, _mainViewModel.Text3 }.Where(x => !string.IsNullOrEmpty(x)))
            {
                yield return new SequencesText2Speech() { Text = s, DisableTransform = disableTransform };
            }
            yield return new SequencesGpIo(pinBlue, false) { BeginTime = 1000 };
        }


        private void OnSpeakNormalTap(object sender, GestureEventArgs e)
        {
            var choreography = new Choreography()
                {
                    Sequences = SequencesText2Speeches(true).ToList()
                };
            _robotApi.Enqueue(choreography);
        }

        
        private void OnSpeakDarthTap(object sender, GestureEventArgs e)
        {
            var choreography = new Choreography()
            {
                Sequences = SequencesText2Speeches(false).ToList()
            };
            _robotApi.Enqueue(choreography);
        }

        private void OnAssignToButtonTap(object sender, GestureEventArgs e)
        {
            var sequenceses =
                SequencesText2Speeches(false)
                    .OfType<SequencesText2Speech>()
                    .Cast<Sequences>()
                    .Concat(new[] {new SequencesPlaySound() {File = "Funny"}});
            var choreographys = sequenceses.Select(sequences => new Choreography()
                {
                    Sequences = new List<Sequences> {sequences}
                });
            _robotApi.SetButtonChoreography(choreographys.ToArray());
        }
    }
}