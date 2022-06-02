using Plugin.CloudFirestore;
using ProLance.Models;
using Rg.Plugins.Popup.Pages;
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
        private string id;

        public ServiceDetailPage(string id)
        {
            InitializeComponent();

            this.id = id;
            Item = new Requests();
            Item.Description = id;
            BindingContext = Item;
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("")
                .Document(this.id)
                .AddSnapshotListener(async (values, error) =>
                {
                    if(values != null)
                    {
                        Item = values.ToObject<Requests>();
                        var name = await GetServiceNameAsync(Item.Id);
                        Item.Name = name;
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


        private void ImgClose_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}