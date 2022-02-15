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
            var auth = CrossFirebaseAuth.Current.Instance;
            auth.AuthState += Auth_AuthState;

            AppCenter.Start("android=ebbe9599-80cd-4dd1-b1ae-a6d1911d4f14;",
                  typeof(Analytics), typeof(Crashes));
        }
        private async void Auth_AuthState(object sender, AuthStateEventArgs e)
        {
            if (e.Auth.CurrentUser != null)
            {
                var query = await CrossCloudFirestore
                        .Current
                        .Instance
                        .Collection("USERS")
                        .Document(e.Auth.CurrentUser.Uid)
                        .GetAsync();
                var user = query.ToObject<User>();
                if (user.Role == "C")
                {
                    MainPage = new ClientShell();
                }
                else if(user.Role == "S")
                {
                    MainPage = new ProviderShell();
                }
                else
                {
                    MainPage = new SigninPage();
                    e.Auth.SignOut();
                }
            }
            else
            {
                MainPage = new SigninPage();
            }
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
