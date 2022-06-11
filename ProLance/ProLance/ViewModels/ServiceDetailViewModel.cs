using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using ProLance.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProLance.ViewModels
{
    public class ServiceDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Requests requests;
        public Requests Requests { get { return requests; } set { requests = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Requests")); } }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }

        public ICommand BtnIgnore { get; set; }
        public ICommand BtnAccept { get; set; }

        private string id;

        public ServiceDetailViewModel(string id)
        {
            this.id = id;
            BtnIgnore = new Command(Ignore);
            BtnAccept = new Command(Accept);

            CrossCloudFirestore
                .Current
                .Instance
                .Collection("REQUESTS")
                .Document(this.id)
                .AddSnapshotListener(async (values, error) =>
                {
                    if (values.Exists)
                    {
                        Requests = values.ToObject<Requests>();
                        Name = await GetServiceNameAsync(Requests.SiD);
                        
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
    

        private async void Accept(object obj)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("SID", Requests.SiD);
            data.Add("Status", "Interested");
            data.Add("UID", CrossFirebaseAuth.Current.Instance.CurrentUser.Uid);
            data.Add("Response", null);
            data.Add("Requests", Requests);


            Interests interests = new Interests()
            {
                Uid = CrossFirebaseAuth.Current.Instance.CurrentUser.Uid,
                Rate = "200",
                Status = "Interested"
            };
            await CrossCloudFirestore
                .Current
                .Instance
                .Collection("REQUESTS")
                .Document(this.id)
                .Collection("Interests")
                .Document(CrossFirebaseAuth.Current.Instance.CurrentUser.Uid)
                .SetAsync(interests);
            //await CrossCloudFirestore
            //    .Current
            //    .Instance
            //    .Collection("INTERESTS")
            //    .AddAsync(data);

        }

        private void Ignore(object obj)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("SID", Requests.SiD);
            data.Add("Status", "Ignore");
            data.Add("UID", CrossFirebaseAuth.Current.Instance.CurrentUser.Uid);
            data.Add("Response", null);
            data.Add("Requests", Requests);

            CrossCloudFirestore
                .Current
                .Instance
                .Collection("INTERESTS")
                .AddAsync(data);
        }
    }
}
