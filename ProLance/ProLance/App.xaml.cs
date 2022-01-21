using ProLance.Models;
using ProLance.Views;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using Xamarin.Forms;
using ProLance.AppShells;

namespace ProLance
{
    public partial class App : Application
    {
        string current_role = null;
        public App()
        {
            InitializeComponent();

            // DependencyService.Register<MockDataStore>();
            var auth = CrossFirebaseAuth.Current.Instance;
            if (auth.CurrentUser != null)
            {
                CrossCloudFirestore
                        .Current
                        .Instance
                        .Collection("USERS")
                        .Document(auth.CurrentUser.Uid)
                        .AddSnapshotListener((values, error) =>
                        {
                            if (values.Exists)
                            {
                                var user = values.ToObject<User>();

                                if (user.Role == "S" && current_role != user.Role)
                                {
                                    MainPage = new ProviderShell();
                                    current_role = user.Role;
                                }
                                if(user.Role == "C" && current_role != user.Role)
                                {
                                    MainPage = new ClientShell();
                                    current_role = user.Role;
                                }
                                current_role = user.Role;
                            }
                            else
                            {
                                MainPage = new SigninPage();
                                auth.SignOut();
                            }
                        });
                
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
