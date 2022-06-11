using ProLance.Models;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ProLance.AppShells;

namespace ProLance.ViewModels
{


    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string email;
        private string password;
        public string Email { get { return email; } set { email = value; PropertyChanged(this, new PropertyChangedEventArgs("Email")); } }
        public string Password { get { return password; } set { password = value; PropertyChanged(this, new PropertyChangedEventArgs("Password")); } }

        public ICommand OnSubmitCommand { get; set; }

        public LoginViewModel()
        {
            
            auth = CrossFirebaseAuth
                .Current
                .Instance;
            OnSubmitCommand = new Command(LoginClick);
        }
        private readonly IAuth auth;
        public async void LoginClick()
        {
            if(Email == null)
            {
                await Application.Current.MainPage.DisplayAlert("Warining", "Provide Email", "Ok");
                return;
            }
            if(Password == null)
            {
                await Application.Current.MainPage.DisplayAlert("Warining", "Provide Password", "Ok");
                return ;
            }
            try
            {
                var results = await auth.SignInWithEmailAndPasswordAsync(email, password);
                if (results.User != null)
                {
                    CrossCloudFirestore
                        .Current
                        .Instance
                        .Collection("USERS")
                        .Document(results.User.Uid)
                        .AddSnapshotListener((values, error) =>
                        {
                            if (values.Exists)
                            {
                                var user = values.ToObject<User>();
                                if (user.Role == "S")
                                {
                                    Application.Current.MainPage = new ProviderShell();
                                }
                                else
                                {
                                    Application.Current.MainPage = new ClientShell();
                                }
                            }
                        });
                }
            }
            catch (FirebaseAuthException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
               //await DisplayAlert("Error", ex.Message, "Ok");
            }
            catch (Exception ex)
            {
                //await DisplayAlert("Error", ex.Message, "Ok");
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }




    }
}
