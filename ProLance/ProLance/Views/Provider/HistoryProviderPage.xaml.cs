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

namespace ProLance.Views.Provider
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryProviderPage : ContentPage
    {
        private readonly ObservableCollection<Requests> requests = new ObservableCollection<Requests>();
        public ObservableCollection<Requests> Requests { get { return requests; } }
        public HistoryProviderPage()
        {
            InitializeComponent();
            HistoryItems.BindingContext = this;
            HistoryItems.ItemsSource = Requests;

            LoadRequests();
        }
        private void LoadRequests()
        {
            CrossCloudFirestore
                   .Current
                   .Instance
                   .Collection("REQUESTS")
                   .WhereEqualsTo("Uid", CrossFirebaseAuth.Current.Instance.CurrentUser.Uid)
                   //.WhereEqualsTo("Status", "1")
                   .AddSnapshotListener((data, error) =>
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
    }
}