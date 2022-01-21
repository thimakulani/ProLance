using ProLance.Views;
using ProLance.Views.Client;
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
    public partial class ClientShell : Shell
    {
        public ClientShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(HomeClientPage), typeof(HomeClientPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        }
    }
}