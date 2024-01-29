using Control1.ViewModels;
using Control1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Control1.include;

namespace Control1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WifiNetworksPage : ContentPage
    {
        public WifiNetworksPage(bleTransport transport, Security sec)
        {
            Title = "Available Wifi Networks";
            InitializeComponent();
            var viewModel = new WifiNetworksViewModel(transport, sec);
            BindingContext = viewModel;

            MessagingCenter.Subscribe<WifiNetworksViewModel, string>(this, "ShowAlert", (sender, message) =>
            {
                DisplayAlert("Alert: ", message, "OK");
            });
            MessagingCenter.Subscribe<WifiNetworksViewModel, string>(this, "ShowMessage", (sender, message) =>
            {
                DisplayAlert("Message: ", message, "OK");
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<WifiNetworksViewModel>(this, "ShowAlert");
            MessagingCenter.Unsubscribe<WifiNetworksViewModel>(this, "ShowMessage");
        }
    

        private void WifiNetworkTabbed(object sender, ItemTappedEventArgs e)
        {
            WifiNetworksModel network = e.Item as WifiNetworksModel;

            if (network != null)
            {
                ((WifiNetworksViewModel)BindingContext).WifiNetworkTappedCommand.Execute(network);
            }
        }
    }
}