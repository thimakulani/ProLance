using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using ProLance.AppShells;
using ProLance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProLance.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var auth = CrossFirebaseAuth.Current.Instance;
            auth.AuthState += Auth_AuthState;
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
                    App.Current.MainPage = new ClientShell();
                }
                else if (user.Role == "S")
                {
                    App.Current.MainPage = new ProviderShell();
                }
                else
                {
                    App.Current.MainPage = new SigninPage();
                    e.Auth.SignOut();
                }
            }
            else
            {
                App.Current.MainPage = new SigninPage();
            }
        }
    }
}