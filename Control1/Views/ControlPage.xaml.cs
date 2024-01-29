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
    public partial class ControlPage : ContentPage
    {
        public ControlPage()
        {
            Title = "Device Control";
            InitializeComponent();
            viewModel = new ControlPageViewModel();
            BindingContext = viewModel;

        }
    }
}