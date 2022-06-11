using ProLance.Models;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;

namespace ProLance.Views.Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestServiceDlg
    {
        private readonly IDocumentReference id;
        public string Address { get; set; }
        public string Description { get; set; }
        public RequestServiceDlg(IDocumentReference id)
        {
            InitializeComponent();
            BindingContext = this;
            this.id = id;

        }
        private async void Request()
        {
            Requests requests = new Requests()
            {
                Address = Address,
                Dates = PickerDate.Date.ToString("dd/MMM/yyyy"),
                Description = Description,
                SiD = id.Id,
                IDocumentReference = id,
                Name = null,
                Status = 0.ToString(),
                Uid = CrossFirebaseAuth.Current.Instance.CurrentUser.Uid,
            };


            await CrossCloudFirestore
                    .Current
                    .Instance
                    .Collection("REQUESTS")
                    .AddAsync(requests);
            await PopupNavigation.Instance.PopAsync();

        }

        private void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            Request();
        }

        private void PickerDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (e.NewDate <= DateTime.Now)
            {
                DisplayAlert("Error", "Cannot select previous dates", "Got it");
            }
        }
    }
}