using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using BuildIndicatron.App.Core.Task;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Shared.Models.ApiResponses;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace BuildIndicatron.App
{
    public partial class ConnectView : PhoneApplicationPage
    {
        public ConnectView()
        {
            InitializeComponent();
            Host.Text = Settings.Instance.Host;
            Port.Text = Settings.Instance.Port;
        }

        private void OnConnectTap(object sender, GestureEventArgs e)
        {
            var robotApi = new RobotApi(string.Format("http://{0}:{1}/", Host.Text, Port.Text));
            ConnectButton.IsEnabled = false;
            robotApi.Ping().ContinueWith(Result);
        }

        private void Result(Task<PingResponse> obj)
        {
            Settings.Instance.Host = Host.Text;
            Settings.Instance.Port = Port.Text;
            Settings.Instance.Save();
            Dispatcher.BeginInvoke(EnableButton);
            if (obj.Exception != null)
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show(obj.Exception.Message));
            }
            else
            {
                Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri("/MainPage.xaml",UriKind.Relative)));
            }
        }

        private void EnableButton()
        {
            ConnectButton.IsEnabled = true;
        }
    }
}