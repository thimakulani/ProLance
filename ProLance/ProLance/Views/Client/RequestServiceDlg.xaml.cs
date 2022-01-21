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
        public RequestServiceDlg(string id)
        {
            InitializeComponent();
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
                {"Address", "" },
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
    }
}