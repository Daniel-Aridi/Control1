using Control1.include;
using Control1.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Control1.ViewModels
{
    public class POPPopupViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bleTransport _bleTransport;
        private bool _isBusy;
        private bool _isEnabled;
        public Command CancelClickedCommand {get; set;}
        public Command SubmitClickedCommand {get; set;}

        public POPPopupViewModel() { }
        public POPPopupViewModel(bleTransport bleTransport)
        {
            CancelClickedCommand = new Command(CancelClicked);
            SubmitClickedCommand = new Command(SubmitClicked);
            IsEnabled = true;
            _bleTransport = bleTransport;
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
                if (_isEnabled != value)
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

        public async void SubmitClicked(object o)
        {
            IsEnabled = false;
            TurnOnBusyIndicator();
            if (o is string _proofOfPossession)
            {
                if (_bleTransport != null)
                {
                    Security security = new Security(_bleTransport, _proofOfPossession);

                    if (await security.establish_session())
                    {
                        TurnOffBusyIndicator();
                        DisplayMessage("A secure session has been established,\nyou can now scan for available networks!");
                        await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new WifiNetworksPage(_bleTransport, security)));
                        IsEnabled = true;
                    }
                    else
                    {
                        DisplayAlert("Unable to establish a secure connection,\nmake sure you typed in the correct POP");
                        await Shell.Current.GoToAsync($"//Devices");
                    }
                }
                else
                {
                    DisplayAlert("bleTransport is not initialized. Make sure you selected a device.");
                    await Shell.Current.GoToAsync($"//Devices");
                }
            }
            else
            {
                IsEnabled=true;
                DisplayAlert("Proof of possession field can't be null!");
            }
            TurnOffBusyIndicator();
        }

        public async void CancelClicked()
        {
            //await Shell.Current.GoToAsync($"//Devices");
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}