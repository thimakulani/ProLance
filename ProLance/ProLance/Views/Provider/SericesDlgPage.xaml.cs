using ProLance.Models;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using Plugin.FirebaseAuth;
using Rg.Plugins.Popup.Services;
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
    public partial class SericesDlgPage
    {
        private readonly ObservableCollection<TempServices> items = new ObservableCollection<TempServices>();
        private List<Offers> offer_items;

        public ObservableCollection<TempServices> Items { get { return items; } }

        public SericesDlgPage(string v)
        {
            InitializeComponent();

            ItemServices.BindingContext = this;
            ItemServices.ItemsSource = Items;
            _ = Load(v);
        }

        private async Task Load(string v)
        {
            //get user offers
            var offers_query = await CrossCloudFirestore
                .Current
                .Instance
                .Collection("OFFERS")
                .WhereEqualsTo("Uid", CrossFirebaseAuth.Current.Instance.CurrentUser.Uid)
                .WhereEqualsTo("Category", v)
                .GetAsync();
            //to list
            offer_items = offers_query.ToObjects<Offers>().ToList();

            //get services of specific category
            var services_query = await CrossCloudFirestore
                .Current
                .Instance
                .Collection("SERVICES")
                .WhereEqualsTo("Category", v)
                .GetAsync();
            foreach (var item in services_query.Documents)
            {
                var _service = item.ToObject<Services>();
                if (!offer_items.Any(s => s.Id == _service.Id))
                {
                    items.Add(new TempServices { Category = _service.Category, Id = _service.Id, Name = _service.Name });
                }
            }
        }
        private void ImgClose_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
        private async void ItemCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var s = (CheckBox)sender;
            var item = (TempServices)s.BindingContext;
            if (e.Value == true)
            {
                var q = await CrossCloudFirestore
                    .Current
                    .Instance
                    .Collection("OFFERS")
                    .WhereEqualsTo("Uid", CrossFirebaseAuth.Current.Instance.CurrentUser.Uid)
                    .WhereEqualsTo("Id", item.Id)
                    .GetAsync();

                if (q.IsEmpty) 
                { 
                    Dictionary<string, object> map = new Dictionary<string, object>
                    {
                        { "Name", item.Name },
                        { "Category", item.Category },
                        { "Uid", CrossFirebaseAuth.Current.Instance.CurrentUser.Uid },
                        { "Id", item.Id }
                    };
                    await CrossCloudFirestore
                        .Current
                        .Instance
                        .Collection("OFFERS")
                        .AddAsync(map);
                }
                else
                {
                    await DisplayAlert("Warning", "Already added", "Okay");
                }
            }
        }
    }
    public class TempServices
    {
        public string Offer_Id { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string IsSelected { get; set; }
    }
    public class Offers
    {
        [Id]
        public string Offer_Id { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string IsSelected { get; set; }
    }
}