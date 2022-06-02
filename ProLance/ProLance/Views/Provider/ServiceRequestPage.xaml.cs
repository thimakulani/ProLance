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
using Rg.Plugins.Popup.Services;

namespace ProLance.Views.Provider
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceRequestPage : ContentPage
    {
        private readonly ObservableCollection<Requests> requests = new ObservableCollection<Requests>();
        public ObservableCollection<Requests> Requests { get { return requests; } }
        public ServiceRequestPage()
        {


            InitializeComponent();




            RequestsItems.BindingContext = this;
            RequestsItems.ItemsSource = Requests;
            
            FetchData();
        }

        private async void FetchData()
        {
            var query = await CrossCloudFirestore
                .Current
                .Instance
                .Collection("OFFERS")
                .WhereEqualsTo("Uid", CrossFirebaseAuth.Current.Instance.CurrentUser.Uid)
                .GetAsync();


            if (!query.IsEmpty)
            {
                List<Offers> offers = query.ToObjects<Offers>().ToList<Offers>();

                var arr = offers.Select(x => x.Id).ToArray();


                CrossCloudFirestore
                    .Current
                    .Instance
                    .Collection("REQUESTS")
                    .WhereIn("S_ID", arr)
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
                                        var name = await GetServiceNameAsync(_request.SiD);
                                        _request.Name = name;
                                        requests.Add(_request);
                                        break;
                                    case DocumentChangeType.Modified:
                                        break;
                                    case DocumentChangeType.Removed:
                                        requests.RemoveAt(item.OldIndex);
                                        break;
                                }
                            }
                        }
                    });
            }

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

        private void BtnInterest_Clicked(object sender, EventArgs e)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Price", "");
            data.Add("UID", CrossFirebaseAuth.Current.Instance.CurrentUser.Uid);
        }


        private void BtnView_Clicked(object sender, EventArgs e)
        {
            var img = (Button)sender;
            var id = img.CommandParameter.ToString();
            Navigation.PushModalAsync(new ServiceDetailPage(id));
            
        }
    }
}