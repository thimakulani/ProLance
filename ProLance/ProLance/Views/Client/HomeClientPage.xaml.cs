using ProLance.Models;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.FirebaseAuth;

namespace ProLance.Views.Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeClientPage : ContentPage
    {
        public HomeClientPage()
        {
            InitializeComponent();
            GetServicesCategories();
            GetRequestedServices();
        }
        private readonly ObservableCollection<Requests> requests = new ObservableCollection<Requests>();
        public ObservableCollection<Requests> Requests { get { return requests; } }
        private void GetRequestedServices()
        {
            ServiceRequested.BindingContext = this;
            ServiceRequested.ItemsSource = Requests;
            CrossCloudFirestore
                   .Current
                   .Instance
                   .Collection("REQUESTS")
                   .WhereEqualsTo("Uid", CrossFirebaseAuth.Current.Instance.CurrentUser.Uid)
                   //.WhereEqualsTo("Status", "1")
                   .AddSnapshotListener(async (data, error) =>
                   {
                       if (!data.IsEmpty)
                       {
                           foreach (var item in data.DocumentChanges)
                           {
                               var _request = new Requests();
                               switch (item.Type)
                               {
                                   case DocumentChangeType.Added:
                                       _request = item.Document.ToObject<Requests>();
                                       var name =  await GetServiceNameAsync(_request.SiD);
                                       _request.Name = name;

                                       
                                       requests.Add(_request);
                                       break;
                                   case DocumentChangeType.Modified:
                                       break;
                                   case DocumentChangeType.Removed:
                                       //requests.RemoveAt(item.OldIndex);
                                       break;
                               }
                           }
                       }
                   });
        }
        private List<Interests> GetInterests()
        {

            return null;
        }
        private async Task<string> GetServiceNameAsync(string id)
        {
            var query = await CrossCloudFirestore
                .Current
                .Instance
                .Collection("SERVICES")
                .Document(id)
                .GetAsync();
            return query.ToObject<Services>().Name;

        }
        private readonly ObservableCollection<ServiceCategories> serviceCategories = new ObservableCollection<ServiceCategories>();
        public ObservableCollection<ServiceCategories> ServiceCategories { get { return serviceCategories; } }
        private void GetServicesCategories()
        {
            ClientServiceCategory.BindingContext = this;
            ClientServiceCategory.ItemsSource = ServiceCategories;
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
                                    break;
                            }
                        }
                    }
                });
        }

        private void ImgService_Clicked(object sender, EventArgs e)
        {
            var img = (ImageButton)sender;
            var id = img.CommandParameter.ToString();
            Navigation.PushAsync(new ServiceDetailsPage(id));
            Console.WriteLine(id);
        }


    }
}