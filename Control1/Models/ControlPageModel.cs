using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Control1.Models
{
    public class ControlPageModel : BindableObject
    {
        public ControlPageModel() { }
    }
    public class ControlBarModel : BindableObject
    {
       
        public string DeviceName { get; set; }
        private bool _isButtonOn;

        public ControlBarModel(string deviceName) 
        { 
            this.DeviceName = deviceName;
            
            _isButtonOn = false;
        }

        public bool IsButtonOn
        {
            get { return _isButtonOn; }
            set
            {
                if (_isButtonOn != value)
                {
                    _isButtonOn = value;
                    OnPropertyChanged(nameof(IsButtonOn));
                }
            }
        }
    }
}
