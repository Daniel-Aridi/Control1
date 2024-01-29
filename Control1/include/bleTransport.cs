using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;

namespace Control1.include
{
    public class bleTransport
    {
        private readonly string _requiredServiceUUID = "021a9004-0382-4aea-bff4-6b3f1c5adfb4";
        private readonly Dictionary<string, string> _characteristicUuids = new Dictionary<string, string>
        {
            { "prov-session", "021aff51-0382-4aea-bff4-6b3f1c5adfb4" },
            { "prov-scan", "021aff50-0382-4aea-bff4-6b3f1c5adfb4" },
            { "prov-config", "021aff52-0382-4aea-bff4-6b3f1c5adfb4" },
            { "proto-ver", "021aff53-0382-4aea-bff4-6b3f1c5adfb4" }
        };
        
        private readonly IAdapter _adapter;
        private IDevice _device;
        private IService _service;
        private List<ICharacteristic> _characteristicList = new List<ICharacteristic>();

        public bool result = false;
        public bleTransport(IAdapter adapter)
        {
            _adapter = adapter;
        }

        public async Task<bool> connect_to_device(IDevice device)
        {
            _device = device;

            if (_device.State == DeviceState.Connected)
            {
                return true;
            }
            else
            {
                try
                {
                    var connectParameters = new ConnectParameters(false, true);
                    await _adapter.ConnectToDeviceAsync(_device, connectParameters);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

        }

        public async Task<bool> find_required_services()
        {

            var foundServices = await _device.GetServicesAsync();
            _service = null;

            foreach (var service in foundServices)
            {
                if (service.Id.ToString() == _requiredServiceUUID)
                {
                    _service = service;
                }
            }

            if (_service != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> get_charachtaristics()
        {
            try
            {
                var foundCharactaristics = await _service.GetCharacteristicsAsync();
                _characteristicList.Clear();

                foreach (var charactaristic in foundCharactaristics)
                {
                    _characteristicList.Add(charactaristic);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<byte[]> send_bytes(string charachtaristicName, byte[] inputData)
        {

            int index = find_charactaristic(charachtaristicName);

            if (index != -1)
            {
                await _characteristicList[index].WriteAsync(inputData);
                (byte[] data, int resultCode) = await _characteristicList[index].ReadAsync();

                return data;
            }
            else
            {
                return null;
            }
        }

        private int find_charactaristic(string charactaristicName)
        {

            string uuid = _characteristicUuids[charactaristicName];

            for (int i = 0; i < _characteristicList.Count; i++)
            {
                if (uuid == _characteristicList[i].Id.ToString())
                {
                    return i;
                }
            }
            return -1;
        }

    }
}
