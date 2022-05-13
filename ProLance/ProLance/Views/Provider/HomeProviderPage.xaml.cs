using ProLance.Models;
using ProLance.Views.Provider;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace ProLance.Views.Provider
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeProviderPage : ContentPage
    {
        private readonly ObservableCollection<Services> service = new ObservableCollection<Services>();
        public ObservableCollection<Services> Services { get { return service; } }


        private readonly ObservableCollection<ServiceCategories> serviceCategories = new ObservableCollection<ServiceCategories>();
        public ObservableCollection<ServiceCategories> ServiceCategories { get { return serviceCategories; } }
        public HomeProviderPage()
        {
            string uid = CrossFirebaseAuth.Current.Instance.CurrentUser.Uid;
            InitializeComponent();
            //Title = ;
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("USERS")
                .Document(uid)
                .AddSnapshotListener((value, error) =>
                {
                    if (value.Exists)
                    {
                        var user = value.ToObject<User>();
                        Title = $"{user.FirstName} {user.LastName}";
                    }
                });
            GetLocation();
            GetOffers();
            GetCategories();
        }

        private async void GetLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    Geocoder geocode = new Geocoder();
                    var address = await geocode.GetAddressesForPositionAsync(new Position(location.Latitude, location.Longitude));
                    TxtLocation.Text = address.FirstOrDefault().ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void GetCategories()
        {
            ServiceCategory.BindingContext = this;
            ServiceCategory.ItemsSource = ServiceCategories;
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("CATEGORIES")
                .AddSnapshotListener((value, error) =>
                {
                    if (!value.IsEmpty)
                    {
                        foreach (var item in value.DocumentChanges)
                        {
                            var category = new ServiceCategories();
                            switch (item.Type)
                            {
                                case DocumentChangeType.Added:
                                    category = item.Document.ToObject<ServiceCategories>();
                                    ServiceCategories.Add(category);
                                    break;
                                case DocumentChangeType.Modified:
                                    break;
                                case DocumentChangeType.Removed:
                                    ServiceCategories.RemoveAt(item.OldIndex);
                                    break;
                            }
                        }
                    }
                });
        }

        private void GetOffers()
        {
            SelectedServices.BindingContext = this;
            SelectedServices.ItemsSource = Services;
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("OFFERS")
                .WhereEqualsTo("Uid", CrossFirebaseAuth.Current.Instance.CurrentUser.Uid)
                .AddSnapshotListener((value, error) =>
                {
                    if (!value.IsEmpty)
                    {
                        foreach (var item in value.DocumentChanges)
                        {
                            var _service = new Services();
                            switch (item.Type)
                            {
                                case DocumentChangeType.Added:
                                    _service = item.Document.ToObject<Services>();
                                    service.Add(_service);
                                    break;
                                case DocumentChangeType.Modified:
                                    break;
                                case DocumentChangeType.Removed:
                                    service.RemoveAt(item.OldIndex);
                                    break;
                            }
                        }
                    }
                });


        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            CrossFirebaseAuth.Current.Instance.SignOut();
        }
        int i = 0;
        private void BtnAddService_Clicked(object sender, EventArgs e)
        {

            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "Category", "IT SERVICE" },
                { "Name", $"IT - {i++}" },
                { "Uid", CrossFirebaseAuth.Current.Instance.CurrentUser.Uid }
            };
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("OFFERS")
                .AddAsync(data);
        }
        private async void ImgRemoveOffer_Clicked(object sender, EventArgs e)
        {
            var id = (Xamarin.Forms.ImageButton)sender;
            if (await DisplayAlert("Confirm", "Are You Sure You Want To Remove This Service?", "YES", "NO"))
            {
                await CrossCloudFirestore.Current.Instance
                    .Collection("OFFERS")
                    .Document(id.CommandParameter.ToString())
                    .DeleteAsync();
            }
        }

        private async void ImgService_Clicked(object sender, EventArgs e)
        {
            var Id = (Xamarin.Forms.ImageButton)sender;
            //DisplayAlert("Id", Id.CommandParameter.ToString(), "OK");
            await PopupNavigation.Instance.PushAsync(new SericesDlgPage(Id.CommandParameter.ToString()));
        }

        private void SelectedServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ImgLogout_Clicked(object sender, EventArgs e)
        {
            CrossFirebaseAuth.Current.Instance.SignOut();
        }
    }
}