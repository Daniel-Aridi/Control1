using Control1.include;
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
	public partial class POPPopupPage : ContentPage
	{
		private bool _state = false;
		private bleTransport _bleTransport;
		public POPPopupPage(bleTransport bleTransport)
		{
			Title = "Enter Proof Of Possession";
			InitializeComponent();
			_bleTransport = bleTransport;
            BindingContext = new POPPopupViewModel(_bleTransport);
            passwordEntry.IsPassword = true;

            MessagingCenter.Subscribe<POPPopupViewModel, string>(this, "ShowAlert", (sender, message) =>
            {
                DisplayAlert("Alert: ", message, "OK");
            });
            MessagingCenter.Subscribe<POPPopupViewModel, string>(this, "ShowMessage", (sender, message) =>
            {
                DisplayAlert("Message: ", message, "OK");
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<POPPopupViewModel>(this, "ShowAlert");
            MessagingCenter.Unsubscribe<POPPopupViewModel>(this, "ShowMessage");
        }
    

		private void ToglePasswordVisibility(object sender, EventArgs e)
		{
            passwordEntry.IsPassword = _state;
			if(_state ==  false)
			{
				_state = true;
			}
			else
			{
				_state = false;
			}
        }

    }
       
}