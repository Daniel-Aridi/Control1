using Control1.include;
using Control1.Models;
using Control1.Views;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;


namespace Control1.ViewModels
{
    public class DevicesViewModel : INotifyPropertyChanged
    {
        public Command ScanNewDeviceCommand {  get; set; }
        public Command BleDeviceTabbedCommand {  get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isBusy;
        private bool _isScanButtonEnabled = true;
        private Color _listTextColor;

        //private const string supportedDeviceName = "PROV";
        private readonly IAdapter _bluetoothAdapter;

        private bleTransport _bleTransport;
        public ObservableCollection<ScanedBluetoothDevicesModel> BleDevices { get; set; }

        public DevicesViewModel() 
        {
            ScanNewDeviceCommand = new Command(ScanNewDeviceClicked);
            BleDeviceTabbedCommand = new Command<ScanedBluetoothDevicesModel>(BleDeviceTabbed);
            Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Default));


            _bluetoothAdapter = CrossBluetoothLE.Current.Adapter;

            BleDevices = new ObservableCollection<ScanedBluetoothDevicesModel>();


            _bluetoothAdapter.DeviceDiscovered += (sender, foundBleDevice) =>
            {
                if (foundBleDevice.Device != null && !string.IsNullOrEmpty(foundBleDevice.Device.Name))
                {
                    BleDevices.Add(new ScanedBluetoothDevicesModel(foundBleDevice.Device, foundBleDevice.Device.Name));
                }
            };
        }

        private async Task<bool> CheckPermissionsGrantedAsync()      
        {
            IBluetoothLE ble = CrossBluetoothLE.Current;
            var locationStatues = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (locationStatues != PermissionStatus.Granted)
            {
                await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Default));
                return false;
            }

            if (!ble.IsAvailable)
            {
                DisplayAlert("Ble is not available for this device!");
                return false;
            }
            if (!ble.IsOn)
            {
                DisplayMessage("please turn on Bluetooth before scanning!");
                return false;
            }
            return true;
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

        public Color ListTextColor
        {
            get { return _listTextColor; }
            set
            {
                if (_listTextColor != value)
                {
                    _listTextColor = value;
                    OnPropertyChanged(nameof(ListTextColor));
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


        ////////////////////////////////////////////////////////////buttons////////////////////////////////////////////
        public async void ScanNewDeviceClicked()
        {
            HapticFeedback.Perform(HapticFeedbackType.Click);
            IsScanButtonEnabled = false;
            ListTextColor = Color.LightGray;

            try
            {
                if (await CheckPermissionsGrantedAsync())
                {
                    TurnOnBusyIndicator();

                    BleDevices.Clear();

                    if (!_bluetoothAdapter.IsScanning)
                    {
                        await _bluetoothAdapter.StartScanningForDevicesAsync();
                    }
                    if (BleDevices.Count == 0)
                    {
                        DisplayMessage("No devices were found!\nPlease make sure location is enabled, and device is in provisioning mode");
                    }
                }
                TurnOffBusyIndicator();
                ListTextColor = Color.DodgerBlue;
            }
            catch 
            {
                TurnOffBusyIndicator();
                IsScanButtonEnabled = true;
                BleDevices.Clear();
                DisplayAlert("unexpected error has occurred, please make sure location access setting is enabled for app and try scaning again!");
            }
            IsScanButtonEnabled = true;
        }

        private async void BleDeviceTabbed(ScanedBluetoothDevicesModel selectedDevice)
        {
            TurnOnBusyIndicator();
            IsScanButtonEnabled = false;
            ListTextColor = Color.LightGray;

            IDevice bleDevice = selectedDevice.BleDevice;
            _bleTransport = new bleTransport(_bluetoothAdapter);


            if (await _bleTransport.connect_to_device(bleDevice))
            {
                if (await _bleTransport.find_required_services())
                {
                    if (await _bleTransport.get_charachtaristics())
                    {
                        await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new POPPopupPage(_bleTransport)));
                    }
                    else
                    {
                        BleDevices.Clear();
                        DisplayAlert("Required device charactaristics were not found");
                    }
                }
                else
                {
                    BleDevices.Clear();
                    DisplayAlert("Selected device is unsupported\nplease try scaning again");
                }
            }
            else
            {
                BleDevices.Clear();
                DisplayAlert($"Error connecting to BLE device: {selectedDevice.BleDeviceName}\nSelected device might not be Supported!");
            }
            TurnOffBusyIndicator();
            IsScanButtonEnabled = true;
            ListTextColor = Color.DodgerBlue;
        }
    }
}
