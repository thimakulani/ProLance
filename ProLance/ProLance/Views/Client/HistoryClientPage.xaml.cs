using ProLance.Models;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProLance.Views.Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryClientPage : ContentPage
    {
        public HistoryClientPage()
        {
            InitializeComponent();
            ItemRequests.BindingContext = this;
            ItemRequests.ItemsSource = Requests;
            LoadRequests();
        }
        private readonly ObservableCollection<Requests> requests = new ObservableCollection<Requests>();
        public ObservableCollection<Requests> Requests { get { return requests; } }
        private void LoadRequests()
        {
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
                                       _request.Name = await GetServiceNameAsync(_request.SiD);
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
    }
}