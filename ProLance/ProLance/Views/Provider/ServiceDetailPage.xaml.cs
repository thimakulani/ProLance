using Plugin.CloudFirestore;
using ProLance.Models;
using ProLance.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProLance.Views.Provider
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceDetailPage
    {
        
        public Requests Item;
        public ServiceDetailPage(string id)
        {
            InitializeComponent();
            ServiceDetailViewModel model = new ServiceDetailViewModel(id);

            BindingContext = model;
            //CrossCloudFirestore
            //    .Current
            //    .Instance
            //    .Collection("REQUESTS")
            //    .Document(this.id)
            //    .AddSnapshotListener(async (values, error) =>
            //    {
            //        if(values.Exists)
            //        {
            //            Item = values.ToObject<Requests>();
            //            var name = await GetServiceNameAsync(Item.SiD);
            //            Item.Name = name;
            //        }
            //    });

            

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


        private async void ImgClose_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}