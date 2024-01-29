using Control1.Models;
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
	public partial class Devices : ContentPage
	{
		public Devices ()
		{
			Title = "Available Devices";
			InitializeComponent ();

			BindingContext = new DevicesViewModel();

            MessagingCenter.Subscribe<DevicesViewModel, string>(this, "ShowAlert", (sender, message) =>
            {
                DisplayAlert("Alert: ", message, "OK");
            });
            MessagingCenter.Subscribe<DevicesViewModel, string>(this, "ShowMessage", (sender, message) =>
            {
                DisplayAlert("Message: ", message, "OK");
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<DevicesViewModel>(this, "ShowAlert");
            MessagingCenter.Unsubscribe<DevicesViewModel>(this, "ShowMessage");
        }

        private void BleDeviceTabbed(object sender, ItemTappedEventArgs e)
        {
            ScanedBluetoothDevicesModel Device = e.Item as ScanedBluetoothDevicesModel;

            if (Device != null)
            {
                ((DevicesViewModel)BindingContext).BleDeviceTabbedCommand.Execute(Device);
            }
        }

    }
}