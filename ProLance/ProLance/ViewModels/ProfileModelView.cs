using ProLance.Models;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProLance.ViewModels
{
    public class ProfileModelView: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string firstName;
        private string lastName;
        private string email;
        private string phone;
        private string address;
        private string imgUrl;
        public string FirstName { get { return firstName; } set { firstName = value; PropertyChanged(this, new PropertyChangedEventArgs("FirstName")); } }
        public string LastName { get { return lastName; } set { lastName = value; PropertyChanged(this, new PropertyChangedEventArgs("LastName")); } }
        public string Email { get { return email; } set { email = value; PropertyChanged(this,  new PropertyChangedEventArgs("Email")); } }
        public string Phone { get { return phone; } set { phone = value; PropertyChanged(this,  new PropertyChangedEventArgs("Phone")); } }
        public string Address { get { return address; } set { address = value; PropertyChanged(this, new PropertyChangedEventArgs("Address")); } }
        public string ImageUrl { get { return imgUrl; } set { imgUrl = value; PropertyChanged(this, new PropertyChangedEventArgs("ImageUrl")); } }
        public ICommand OnUpdateCommand { get; set; }   
        public ICommand OnImagePicker { get; set; }
        public ProfileModelView()
        {
            OnUpdateCommand = new Command(Update);
            OnImagePicker = new Command(UploadImage);

            GetData();
        }

        private async void UploadImage(object obj)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

            });


            if (file != null)
            {
                var storage_ref = Plugin.FirebaseStorage.CrossFirebaseStorage
                     .Current
                     .Instance
                     .RootReference
                     .Child(CrossFirebaseAuth.Current.Instance.CurrentUser.Uid);

               await storage_ref.PutStreamAsync(file.GetStream());

                var url = await storage_ref.GetDownloadUrlAsync();

                //    .PutStreamAsync(file.GetStream());
                await CrossCloudFirestore
                    .Current
                    .Instance
                    .Collection("USERS")
                    .Document(CrossFirebaseAuth.Current.Instance.CurrentUser.Uid)
                    .UpdateAsync("ImageUrl", url.ToString());
            }
        
        }

        private void GetData()
        {
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("USERS")
                .Document(CrossFirebaseAuth.Current.Instance.CurrentUser.Uid)
                .AddSnapshotListener((value, error) =>
                {
                    if (value.Exists)
                    {
                        var user = value.ToObject<User>();
                        FirstName = user.FirstName;
                        LastName = user.LastName;
                        Email = user.Email;
                        Phone = user.PhoneNumber;
                        Address = user.Address;
                        ImageUrl = user.ImageUrl;
                        
                    }
                });
        }

        private void Update()
        {


            ///**************////
            ///

            //validattion

            if(firstName == null)
            {
                App.Current.MainPage.DisplayAlert("Warning", "Enter first name", "Ok");
                return;
            }

            if(lastName == null)
            {
                App.Current.MainPage.DisplayAlert("Warning", "Enter last name", "Ok");
                return;
            }

            if(email == null)
            {
                App.Current.MainPage.DisplayAlert("Warning", "Enter email ", "Ok");
                return;
            }

            if(phone == null)
            {
                App.Current.MainPage.DisplayAlert("Warning", "Enter phone", "Ok");
                return;
            }

            if(address == null)
            {
                App.Current.MainPage.DisplayAlert("Warning", "Enter address", "Ok");
                return;
            }

            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"FirstName", firstName},
                {"Email", email},
                {"Address", address},
                {"Gender", imgUrl},
                {"PhoneNumber", phone},
                {"LastName", lastName},
            };
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("USERS")
                .Document(CrossFirebaseAuth.Current.Instance.CurrentUser.Uid)
                .UpdateAsync(data);
        }
    }
}
