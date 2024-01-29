using Control1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Control1.ViewModels
{
    public class ControlPageViewModel
    {
        private const string serverIp = "smartSwitch.local";
        private const int serverPort = 55555;
        private TcpClient tcpClient = null;
        private NetworkStream networkStream = null; 

        private readonly int numberofRelays = 3;
        public ICommand TurnONCommand {get;set;}
        public ICommand TurnOFFCommand { get;set;}
        public ObservableCollection<ControlBarModel> Bars { get; set; }

        public ControlPageViewModel()
        {
            TurnONCommand = new Command(async (object o) => await TurnONClicked(o));
            TurnOFFCommand = new Command(async (object o) => await TurnOFFClicked(o));
            Bars = new ObservableCollection<ControlBarModel>();


            for (int i = 0; i < numberofRelays; i++)
            {
                Bars.Add(new ControlBarModel($"Relay {i+1}"));
            }
        }

        private void ConnectToServer()
        {
            try
            {
                if (tcpClient == null)
                    tcpClient = new TcpClient();

                if (!tcpClient.Connected)
                    tcpClient.Connect(serverIp, serverPort);

                if(networkStream == null)
                    networkStream = tcpClient.GetStream();
            }
            catch (Exception ex)
            {
                // Handle exception
                
            }
        }

        public async Task SendCommandAsync(string command)
        {
            try
            {
                if (networkStream != null)
                {
                    byte[] messageBytes = Encoding.UTF8.GetBytes(command);
                    await networkStream.WriteAsync(messageBytes, 0, messageBytes.Length);
                    await networkStream.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                
            }
        }


        public async Task TurnONClicked(object o)
        {
            HapticFeedback.Perform(HapticFeedbackType.Click);

            ControlBarModel controlBarModel = o as ControlBarModel;
            controlBarModel.IsButtonOn = true;

            ConnectToServer();
            await SendCommandAsync("TurnON");
        }
        public async Task TurnOFFClicked(object o)
        {
            HapticFeedback.Perform(HapticFeedbackType.Click);

            ControlBarModel controlBarModel = o as ControlBarModel;
            controlBarModel.IsButtonOn = false;

            ConnectToServer();
            await SendCommandAsync("TurnOFF");
        }

        
    }
}
