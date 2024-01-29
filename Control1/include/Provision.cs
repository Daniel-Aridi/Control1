using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Control1.include
{
    public class ScanResult
    {
        public string SSID { get; internal set; }
        public string BSSID { get; internal set; }
        public uint Channel { get; internal set; }
        public int RSSI { get; internal set; }
        public string Auth { get; internal set; }
    }

    
    public class Provision
    {
        private bool scanFinished = false;
        List<ScanResult> APs = new List<ScanResult>();

        public async Task<bool> ConnectToNetwork(bleTransport transport, Security sec, string ssid, string password)
        {
            byte[] message = ConfigSetConfigRequest(sec, ssid, password);
            byte[] response = await transport.send_bytes("prov-config", message);
            if(ConfigSetConfigResponse(sec, response) == 0)
            {
                message = ConfigApplyConfigRequest(sec);
                response = await transport.send_bytes("prov-config", message);
                if(configApplyConfigResponse(sec, response) == 0)
                {
                    return true;
                }
            }
            return false;
        }
        public async Task<List<ScanResult>> ScanWifiAPs(bleTransport transport, Security sec)
        {
            uint readLen = 4;
            uint groupChannels = 0;

            byte[] message = ScanStartRequest(sec, true, false, groupChannels, 120);
            var response = await transport.send_bytes("prov-scan", message);

            ScanStartResponse(sec, response);

            message = ScanStatusRequest(sec);

            response = await transport.send_bytes("prov-scan", message);

            var result = ScanStatusResponse(sec, response);

            if (result != 0 && scanFinished)
            {
                uint index = 0;
                uint remaining = result;

                while (remaining > 0)
                {
                    uint count = Math.Min(remaining, readLen);
                    message = ScanResultRequest(sec, index, count);
                    response = await transport.send_bytes("prov-scan", message);
                    APs.AddRange(ScanResultResponse(sec, response));
                    remaining -= count;
                    index += count;
                }
                return APs;
            }
            else
            {
                return null;
            }
            
        }

        public async Task<bool> WaitWifiConnected(bleTransport transport, Security sec)
        {
            int timePerPole = 3000;
            int retry = 3;
            string result;
            while (true)
            {
                await Task.Delay(timePerPole);

                result = await GetWifiConfig(transport, sec);

                if (result == "connected")
                {
                    return true;
                }
                if (result == "connecting")
                {
                    continue;
                }
                else if (retry > 0)
                {
                    retry--;
                }
                else
                {
                    return false;
                }
            }
        }


        //////Scaning for available Network by Device/////////////////
        public byte[] ScanStartRequest(Security sec, bool blocking, bool passive, uint groupChannels, uint periodMs)
        {

            WiFiScanPayload wiFiScanPayload = new WiFiScanPayload();

            wiFiScanPayload.CmdScanStart = new CmdScanStart();

            wiFiScanPayload.Msg = WiFiScanMsgType.TypeCmdScanStart;

            wiFiScanPayload.CmdScanStart.Blocking = blocking;

            wiFiScanPayload.CmdScanStart.Passive = passive;

            wiFiScanPayload.CmdScanStart.GroupChannels = groupChannels;

            wiFiScanPayload.CmdScanStart.PeriodMs = periodMs;

            return sec.EncryptData(wiFiScanPayload.ToByteArray());
        }

        public void ScanStartResponse(Security sec, byte[] response)
        {
            byte[] decryptedResponse = sec.DecryptData(response.ToArray());

            WiFiScanPayload wiFiScanPayload = new WiFiScanPayload();

            wiFiScanPayload = WiFiScanPayload.Parser.ParseFrom(decryptedResponse);

        }

        public byte[] ScanStatusRequest(Security sec)
        {
            WiFiScanPayload wiFiScanPayload = new WiFiScanPayload();

            wiFiScanPayload.Msg = new WiFiScanMsgType();

            wiFiScanPayload.Msg = WiFiScanMsgType.TypeCmdScanStatus;

            return sec.EncryptData(wiFiScanPayload.ToByteArray());
        }

        public uint ScanStatusResponse(Security sec, byte[] response)
        {
            byte[] decryptedResponse = sec.DecryptData(response);

            WiFiScanPayload wiFiScanPayload = new WiFiScanPayload();

            wiFiScanPayload = WiFiScanPayload.Parser.ParseFrom(decryptedResponse);

            scanFinished = wiFiScanPayload.RespScanStatus.ScanFinished;

            return wiFiScanPayload.RespScanStatus.ResultCount;
        }

        public byte[] ScanResultRequest(Security sec, uint index, uint count)
        {
            WiFiScanPayload wiFiScanPayload = new WiFiScanPayload();

            wiFiScanPayload.Msg = new WiFiScanMsgType();

            wiFiScanPayload.CmdScanResult = new CmdScanResult();

            wiFiScanPayload.Msg = WiFiScanMsgType.TypeCmdScanResult;

            wiFiScanPayload.CmdScanResult.StartIndex = index;

            wiFiScanPayload.CmdScanResult.Count = count;

            return sec.EncryptData(wiFiScanPayload.ToByteArray());
        }

        public List<ScanResult> ScanResultResponse(Security sec, byte[] response)
        {
            byte[] decryptedMessage = sec.DecryptData(response);

            WiFiScanPayload wiFiScanPayload= new WiFiScanPayload();

            wiFiScanPayload = WiFiScanPayload.Parser.ParseFrom(decryptedMessage);

            string[] authModeStr = { "Open", "WEP", "WPA_PSK", "WPA2_PSK", "WPA_WPA2_PSK",
                                     "WPA2_ENTERPRISE", "WPA3_PSK", "WPA2_WPA3_PSK"};

            List<ScanResult> tempAPs = new List<ScanResult>();

            foreach (var entry in wiFiScanPayload.RespScanResult.Entries)
            {
                tempAPs.Add(new ScanResult
                {
                    SSID = entry.Ssid.ToStringUtf8(),
                    BSSID = entry.Bssid.ToString(),
                    Channel = entry.Channel,
                    RSSI = entry.Rssi,
                    Auth = authModeStr[(int)entry.Auth]
                });
            }
            return tempAPs;
        }


        /////////Aplying config and connecting to network////////////
        
        public byte[] ConfigSetConfigRequest(Security sec, string ssid, string passphrase)
        {
            byte[] ssidBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(ssid);
            byte[] passBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(passphrase);

            WiFiConfigPayload wiFiConfigPayload = new WiFiConfigPayload();
            wiFiConfigPayload.Msg = new WiFiConfigMsgType();
            wiFiConfigPayload.CmdSetConfig = new CmdSetConfig();

            wiFiConfigPayload.Msg = WiFiConfigMsgType.TypeCmdSetConfig;
            wiFiConfigPayload.CmdSetConfig.Ssid = ByteString.CopyFrom(ssidBytes);
            wiFiConfigPayload.CmdSetConfig.Passphrase = ByteString.CopyFrom(passBytes);

            return sec.EncryptData(wiFiConfigPayload.ToByteArray());
        }

        public int ConfigSetConfigResponse(Security sec, byte[] response)
        {
            byte[] decryptedResponse = sec.DecryptData(response);

            WiFiConfigPayload wiFiConfigPayload = new WiFiConfigPayload();
            wiFiConfigPayload = WiFiConfigPayload.Parser.ParseFrom(decryptedResponse);

            return (int)wiFiConfigPayload.RespSetConfig.Status;
        }

        public byte[] ConfigApplyConfigRequest(Security sec)
        {
            WiFiConfigPayload wiFiConfigPayload = new WiFiConfigPayload();
            wiFiConfigPayload.Msg = new WiFiConfigMsgType();

            wiFiConfigPayload.Msg = WiFiConfigMsgType.TypeCmdApplyConfig;

            return sec.EncryptData(wiFiConfigPayload.ToByteArray());
        }

        public int configApplyConfigResponse(Security sec, byte[] response)
        {
            byte[] decryptedResponse = sec.DecryptData(response);

            WiFiConfigPayload wiFiConfigPayload= new WiFiConfigPayload();

            wiFiConfigPayload = WiFiConfigPayload.Parser.ParseFrom(decryptedResponse);

            return (int)wiFiConfigPayload.RespApplyConfig.Status;
        }

        /////////Check Connection Statues//////////////

        public byte[] ConfigGetStatusRequest(Security sec)
        {
            WiFiConfigPayload wiFiConfigPayload = new WiFiConfigPayload();
            wiFiConfigPayload.Msg = new WiFiConfigMsgType();

            wiFiConfigPayload.Msg = WiFiConfigMsgType.TypeCmdGetStatus;

            wiFiConfigPayload.CmdGetStatus = new CmdGetStatus();


            wiFiConfigPayload.CmdGetStatus.MergeFrom(wiFiConfigPayload.CmdGetStatus);

            return sec.EncryptData(wiFiConfigPayload.ToByteArray());
        }

        public string ConfigGetStatusResponse(Security sec, byte[] response)
        {
            byte[] decryptedResponse = sec.DecryptData(response);

            WiFiConfigPayload wiFiConfigPayload = new WiFiConfigPayload();

            wiFiConfigPayload = WiFiConfigPayload.Parser.ParseFrom(decryptedResponse);

            if((int)wiFiConfigPayload.RespGetStatus.StaState == 0)
            {
                return "connected";
            }
            if ((int)wiFiConfigPayload.RespGetStatus.StaState == 1)
            {
                return "connecting";
            }
            if ((int)wiFiConfigPayload.RespGetStatus.StaState == 2)
            {
                return "disconnected";
            }
            if ((int)wiFiConfigPayload.RespGetStatus.StaState == 3)
            {
                if((int)wiFiConfigPayload.RespGetStatus.FailReason == 0)
                {
                    return "incorrectPassword";
                }
                if ((int)wiFiConfigPayload.RespGetStatus.FailReason == 1)
                {
                    return "incorrectSSID";
                }
            }
            return "unknown";

        }

        public async Task<string> GetWifiConfig(bleTransport transport, Security sec)
        {
            byte[] message = ConfigGetStatusRequest(sec);
            byte[] response = await transport.send_bytes("prov-config", message);

            return ConfigGetStatusResponse(sec, response);
        }
    }
}
