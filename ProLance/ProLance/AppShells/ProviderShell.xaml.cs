using ProLance.Views;
using ProLance.Views.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProLance.AppShells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProviderShell : Shell
    {
        public ProviderShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(HomeProviderPage), typeof(HomeProviderPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        }
    }
}