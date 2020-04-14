using Lanchat.Common.NetworkLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Lanchat.Xamarin
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public Network Network { get; private set; }

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(250);
                Input.Focus();
                AddToLog("Starting network");

                await Task.Run(() =>
                 {
                     Network = new Network(4001, "Xamarin", 4002, 5000, 10000);
                     var NetworkEventsHandlers = new NetworkEventsHandlers(this, Network);
                     Network.Events.HostStarted += NetworkEventsHandlers.OnHostStarted;
                     Network.Events.ReceivedMessage += NetworkEventsHandlers.OnReceivedMessage;
                     Network.Events.NodeConnected += NetworkEventsHandlers.OnNodeConnected;
                     Network.Events.NodeDisconnected += NetworkEventsHandlers.OnNodeDisconnected;
                     Network.Events.NodeSuspended += NetworkEventsHandlers.OnNodeSuspended;
                     Network.Events.NodeResumed += NetworkEventsHandlers.OnNodeResumed;
                     Network.Events.ChangedNickname += NetworkEventsHandlers.OnChangedNickname;
                     Network.Start();
                 });
            });
        }

        void OnSendClicked(object sender, EventArgs args)
        {
            AddToLog(Input.Text);
            Network.Methods.SendAll(Input.Text);
            Input.Text = string.Empty;
        }

        public void AddToLog(string text)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Log.Text = Log.Text += $"{Environment.NewLine}[{DateTime.Now:HH:mm}] {text}";
            });
        }
    }
}
