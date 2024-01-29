using Control1.include;
using Control1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Control1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WifiPasswordPopupPage : ContentPage
    {
        private bool _state = false;
       
        public WifiPasswordPopupPage(Provision provision, bleTransport transport, Security security, string ssid)
        {
            Title = "Enter Network Password";
            InitializeComponent();
            passwordEntry.IsPassword = true;
            BindingContext = new WifiPasswordPopupViewModel(provision, transport, security, ssid);

            MessagingCenter.Subscribe<WifiPasswordPopupViewModel, string>(this, "ShowAlert", (sender, message) =>
            {
                DisplayAlert("Alert: ", message, "OK");
            });
            MessagingCenter.Subscribe<WifiPasswordPopupViewModel, string>(this, "ShowMessage", (sender, message) =>
            {
                DisplayAlert("Message: ", message, "OK");
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<WifiPasswordPopupViewModel>(this, "ShowAlert");
            MessagingCenter.Unsubscribe<WifiPasswordPopupViewModel>(this, "ShowMessage");
        }
    

        private void ToglePasswordVisibility(object sender, EventArgs e)
        {
            passwordEntry.IsPassword = _state;
            if (_state == false)
            {
                _state = true;
            }
            else
            {
                _state = false;
            }
        }
    }
}