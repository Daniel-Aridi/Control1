using Control1.include;
using Control1.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace Control1.ViewModels
{
    public class WifiPasswordPopupViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isBusy;
        private bool _isEnabled;
        public Command CancelClickedCommand { get; set; }
        public Command SubmitClickedCommand {get; set; }

        private Provision _provision;
        private bleTransport _bleTransport;
        private Security _sercurity;
        private string _ssid;

        public WifiPasswordPopupViewModel() { }
        public WifiPasswordPopupViewModel(Provision provision, bleTransport transport, Security security, string ssid)
        {
            CancelClickedCommand = new Command(CancelClicked);
            SubmitClickedCommand = new Command(SubmitClicked);
            IsEnabled = true;
            _provision = provision;
            _bleTransport = transport;
            _sercurity = security;
            _ssid = ssid;
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                }
            }
        }
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    OnPropertyChanged(nameof(IsEnabled));
                }
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TurnOffBusyIndicator()
        {
            IsBusy = false;
        }
        private void TurnOnBusyIndicator()
        {
            IsBusy = true;
        }
        private void DisplayAlert(string message)
        {
            MessagingCenter.Send(this, "ShowAlert", message);
        }
        private void DisplayMessage(string message)
        {
            MessagingCenter.Send(this, "ShowMessage", message);
        }

        private async void CancelClicked()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new WifiNetworksPage(_bleTransport, _sercurity)));
        }
        private async void SubmitClicked(Object o)
        {
            IsEnabled = false;
            TurnOnBusyIndicator();
            try
            {
                if (o is string password)
                {
                    if (await _provision.ConnectToNetwork(_bleTransport, _sercurity, _ssid, password))
                    {
                        if (await _provision.WaitWifiConnected(_bleTransport, _sercurity))
                        {
                            TurnOffBusyIndicator();
                            DisplayMessage("Connection Successful!");
                            await Shell.Current.GoToAsync($"//ControlPage");
                        }
                        else
                        {
                            DisplayAlert("Connection Failed\nWrong Password!");
                        }
                    }
                    else
                    {
                        DisplayAlert("Error uccured while sending credentials\nPlease try connecting again");
                    }
                }
            }
            catch
            {

                DisplayAlert("Error uccured while sending credentials\nPlease try connecting again");
            }
            TurnOffBusyIndicator();
        }
    }
}