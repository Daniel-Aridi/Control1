using Control1.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace Control1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tab : Xamarin.Forms.Shell
    {
        
        public Tab()
        {
            InitializeComponent();
            MainTabBar.CurrentItem = Tab2;
            Navigating += MainTabBar_OnTabSelected;
        }
        private void MainTabBar_OnTabSelected(object sender, ShellNavigatingEventArgs e)
        {
            // Perform haptic feedback when a tab is selected
            HapticFeedback.Perform(HapticFeedbackType.Click);
        }
    }
}