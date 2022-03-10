using ProLance.Models;
using ProLance.Views;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using Xamarin.Forms;
using ProLance.AppShells;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace ProLance
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            AppCenter.Start("android=ebbe9599-80cd-4dd1-b1ae-a6d1911d4f14;",
                  typeof(Analytics), typeof(Crashes));
            MainPage = new LoadingPage();
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
