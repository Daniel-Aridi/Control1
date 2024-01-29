using Control1.include;
using Control1.ViewModels;
using Control1.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Control1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //bleTransport bleTransport;
            //NavigationPage navigationPage = new NavigationPage(new POPPopupPage(bleTransport));
            //MainPage = navigationPage;
            MainPage = new Tab();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
