using ProLance.Models;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProLance.AppShells;

namespace ProLance.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPage
    {
        private readonly IAuth auth;
        private readonly IFirestore database;
        public SignupPage()
        {
            InitializeComponent();
            auth = CrossFirebaseAuth
                .Current
                .Instance;

            database = CrossCloudFirestore
                .Current
                .Instance;
        }

        private async void BtnSignup_Clicked(object sender, EventArgs e)
        {
            //validations
            if (InputEmail.Text == null)
            {
                await DisplayAlert("Error", "Enter Email", "Ok");
                return;
            }
            if (InputPassword.Text == null)
            {
                await DisplayAlert("Error", "Enter Password", "Ok");
                return;
            }
            string role;
            if (Radio_C.IsChecked)
            {
                role = "C";
            }
            else
            {
                role = "S";
            }
            User user = new User()
            {
                Email = InputEmail.Text,
                Address = InputAddress.Text,
                FirstName = InputFirstName.Text,
                LastName = InputLastName.Text,
                ImageUrl = null,
                PhoneNumber = InputPhone.Text,
                Role = role,
            };
            
            try
            {
                var results = await auth
                    .CreateUserWithEmailAndPasswordAsync(InputEmail.Text.Trim(), InputPassword.Text.Trim());
                if (results.User != null)
                {
                    await database.Collection("USERS")
                    .Document(results.User.Uid)
                    .SetAsync(user);
                    if(role == "S")
                    {
                        App.Current.MainPage = new ProviderShell();
                    }
                    else
                    {
                        App.Current.MainPage = new ClientShell();
                    }
                }

            }
            catch (FirebaseAuthException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {

            }
            
        }
    }
}