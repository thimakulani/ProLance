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

namespace ProLance.Views.Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestServiceDlg
    {
        private readonly string id;
        public string Address { get; set; }
        public RequestServiceDlg(string id)
        {
            InitializeComponent();
            BindingContext = this;
            this.id = id;
            
        }
        private void Request()
        {
          
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"S_ID",id },
                {"Uid",CrossFirebaseAuth.Current.Instance.CurrentUser.Uid },
                {"Dates",PickerDate.Date.ToString("dd/MMM/yyyy") },
                {"Status", "0"},
                {"Address", Address },
            };
            

            CrossCloudFirestore
                .Current
                .Instance
                .Collection("REQUESTS")
                .AddAsync(data);
        }

        private void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            Request();
        }

        private void PickerDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            if(e.NewDate <= DateTime.Now)
            {
                DisplayAlert("Error", "Cannot select previous dates", "Got it");
            }
        }
    }
}