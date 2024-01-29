using Control1.include;
using Control1.Models;
using Control1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Control1.ViewModels
{
    public class WifiNetworksViewModel : INotifyPropertyChanged
    {
        private bleTransport _bleTransport;
        private Security _security;
        private Provision _provision = new Provision();

        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isBusy;
        private bool _isScanButtonEnabled;

        public Command ScanWifiNetworkCommand {get; set;}
        public Command WifiNetworkTappedCommand {get; set;}

        private List<ScanResult> _APs = new List<ScanResult>();
        public ObservableCollection<WifiNetworksModel> WifiNetworks {get; set;}

        public WifiNetworksViewModel() { }
        public WifiNetworksViewModel(bleTransport transport, Security sec)
        {
            ScanWifiNetworkCommand = new Command<WifiNetworksModel>(ScanWifiNetworkClicked);
            WifiNetworkTappedCommand = new Command<WifiNetworksModel>(WifiNetworkTapped);
            _bleTransport = transport;
            _security = sec;
            IsBusy = false;
            IsScanButtonEnabled = true;
            WifiNetworks = new ObservableCollection<WifiNetworksModel>();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        public bool IsScanButtonEnabled
        {
            get { return _isScanButtonEnabled; }
            set
            {
                if (_isScanButtonEnabled != value)
                {
                    _isScanButtonEnabled = value;
                    OnPropertyChanged(nameof(IsScanButtonEnabled));
                }
            }
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
        
        private async void ScanWifiNetworkClicked(WifiNetworksModel network)
        {
            IsScanButtonEnabled = false;
            TurnOnBusyIndicator();

            _APs.Clear();
            WifiNetworks.Clear();
            _APs = await _provision.ScanWifiAPs(_bleTransport, _security);

            if (_APs != null)
            {
                foreach (ScanResult ap in _APs)
                {
                    WifiNetworks.Add(new WifiNetworksModel(ap.SSID, ap.BSSID, ap.Channel, ap.RSSI, ap.Auth));
                }
                TurnOffBusyIndicator();
                IsScanButtonEnabled = true;
            }
            else
            {
                DisplayAlert("there were no available networks found!\nPlease try scaning again");
                IsScanButtonEnabled = true;
                TurnOffBusyIndicator();
            }
        }

        private async void WifiNetworkTapped(WifiNetworksModel network)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new WifiPasswordPopupPage(_provision, _bleTransport, _security, network.SSID)));
        }
    }
}