using Control1.include;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Control1.Models
{
    public class ScanedBluetoothDevicesModel : BindableObject
    {
        public IDevice BleDevice {  get; set; }
        public string BleDeviceName { get; set; }

        public ScanedBluetoothDevicesModel(IDevice idvice, string ideviceName)
        {
            this.BleDevice = idvice;
            this.BleDeviceName = ideviceName;
        }
    }
}
