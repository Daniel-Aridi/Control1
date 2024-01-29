using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Control1.Models
{
    public class WifiNetworksModel
    {
        public string SSID { get; set; }
        public string BSSID { get; set; }
        public uint Channel { get; set; }
        public int RSSI { get; set; }
        public string Auth { get; set; }

        public WifiNetworksModel(string ssid, string bssid, uint channel, int rssi, string auth)
        {
            this.SSID = ssid;
            this.BSSID = bssid;
            this.Channel = channel;
            this.RSSI = rssi;
            this.Auth = auth;
        }
    }
}