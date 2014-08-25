using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using BuildIndicatron.App.Core.Task;
using BuildIndicatron.App.ViewModels;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Shared.Enums;
using BuildIndicatron.Shared.Models.ApiResponses;
using BuildIndicatron.Shared.Models.Composition;

namespace BuildIndicatron.App
{
    public partial class MainPage
    {
  
        private readonly string _hostApi;
        private readonly List<object> _isOn;
        private readonly MainViewModel _mainViewModel;
        private readonly RobotApi _robotApi;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the list box control to the sample data
            _mainViewModel = new MainViewModel();
            _hostApi = string.Format("http://{0}:{1}/api/", Settings.Instance.Host, Settings.Instance.Port);
            _robotApi = new RobotApi(_hostApi);
            DataContext = _mainViewModel;
            Loaded += OnLoaded;
            _isOn = new List<object>();
        }


        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Task<GetClipsResponse> clips = _robotApi.GetClips();
            clips.ContinueWith(Action);
        }

        private void Action(Task<GetClipsResponse> task)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    _mainViewModel.Items.Clear();
                    if (task.Result != null)
                        foreach (Folder trigger in task.Result.Folders)
                        {
                            _mainViewModel.Items.Add(new ClipItemModel {Name = trigger.Name});
                            foreach (string file in trigger.Files)
                            {
                                _mainViewModel.Items.Add(new ClipItemModel {Name = trigger.Name + "/" + file});
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
                if (clipItemModel != null)
                    _robotApi.Enqueue(new Choreography
                        {
                            Sequences = new List<Sequences> {new SequencesPlaySound {File = clipItemModel.Name}}
                        });
            }
        }

        private void OnGlowControlTap(object sender, GestureEventArgs e)
        {
            if (sender == LsRed)
            {
				_robotApi.GpIoOutput(PinName.MainLightRed, Switch(LsRed));
            }
            else if (sender == LsGreen)
            {
				_robotApi.GpIoOutput(PinName.MainLightGreen, Switch(LsGreen));
            }
            else if (sender == LsBlue)
            {
				_robotApi.GpIoOutput(PinName.MainLightBlue, Switch(LsBlue));
            }
            else if (sender == GlGreen)
            {
				_robotApi.GpIoOutput(PinName.SecondaryLightGreen, Switch(GlGreen));
            }
            else if (sender == GlRed)
            {
				_robotApi.GpIoOutput(PinName.SecondaryLightRed, Switch(GlRed));
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
            _robotApi.Enqueue(new Choreography
                {
                    Sequences = new List<Sequences>
                        {
                            new SequencesGpIo(PinName.MainLightBlue, true),
                            new SequencesOneLiner(),
                            new SequencesPlaySound {File = "Stop/jabba_laugh.wav"},
                            new SequencesGpIo(PinName.MainLightBlue, false),
                        }
                });
        }

        private void OnQuoteTap(object sender, GestureEventArgs e)
        {
            _robotApi.Enqueue(new Choreography
            {
                Sequences = new List<Sequences>
                        {
                            new SequencesGpIo(PinName.MainLightBlue, true),
                            new SequencesQuotes(),
                            new SequencesGpIo(PinName.MainLightBlue, false),
                        }
            });
        }

        private void OnInsultTap(object sender, GestureEventArgs e)
        {
            _robotApi.Enqueue(new Choreography
            {
                Sequences = new List<Sequences>
                        {
                            new SequencesGpIo(PinName.MainLightBlue, true),
                            new SequencesInsult(),
                            new SequencesGpIo(PinName.MainLightBlue, false),
                        }
            });
        }

        private IEnumerable<Sequences> SequencesText2Speeches(bool disableTransform)
        {
            yield return new SequencesGpIo(PinName.MainLightBlue, true);
            foreach (
                string s in
                    new[] {_mainViewModel.Text1, _mainViewModel.Text2, _mainViewModel.Text3}.Where(
                        x => !string.IsNullOrEmpty(x)))
            {
                yield return new SequencesText2Speech {Text = s, DisableTransform = disableTransform};
            }
            yield return new SequencesGpIo(PinName.MainLightBlue, false) {BeginTime = 1000};
        }


        private void OnTweetTap(object sender, GestureEventArgs e)
        {
            var choreography = new Choreography
            {
                Sequences = SequencesTweet().ToList()
            };
            _robotApi.Enqueue(choreography);
        }

        private IEnumerable<Sequences> SequencesTweet()
        {
            yield return new SequencesGpIo(PinName.MainLightBlue, true);
            foreach (
                string s in
                    new[] { _mainViewModel.Text1, _mainViewModel.Text2, _mainViewModel.Text3 }.Where(
                        x => !string.IsNullOrEmpty(x)))
            {
                yield return new SequencesTweet() { Text = s };
            }
            yield return new SequencesGpIo(PinName.MainLightBlue, false) { BeginTime = 1000 };
        }

        

        private void OnSpeakNormalTap(object sender, GestureEventArgs e)
        {
            var choreography = new Choreography
                {
                    Sequences = SequencesText2Speeches(true).ToList()
                };
            _robotApi.Enqueue(choreography);
        }


        private void OnSpeakDarthTap(object sender, GestureEventArgs e)
        {
            var choreography = new Choreography
                {
                    Sequences = SequencesText2Speeches(false).ToList()
                };
            _robotApi.Enqueue(choreography);
        }

        private void OnAssignToButtonTap(object sender, GestureEventArgs e)
        {
            IEnumerable<Sequences> sequenceses =
                SequencesText2Speeches(false)
                    .OfType<SequencesText2Speech>()
                    .Cast<Sequences>()
                    .Concat(new[] {new SequencesPlaySound {File = "Funny"}});
            IEnumerable<Choreography> choreographys = sequenceses.Select(sequences => new Choreography
                {
                    Sequences = new List<Sequences> {sequences}
                });
            _robotApi.SetButtonChoreography(choreographys.ToArray());
        }


        
    }
}