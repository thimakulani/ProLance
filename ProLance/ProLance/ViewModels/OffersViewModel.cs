using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace ProLance.ViewModels
{
    public class OffersViewModel : INotifyPropertyChanged
    {

        private string name;
        private string uid;
        public string Name { get { return name; } set { name = value; PropertyChanged(this, new PropertyChangedEventArgs("Name")); } }
        public string Uid { get { return uid; } set { uid = value; PropertyChanged(this, new PropertyChangedEventArgs("UId")); } }
        public Command <Action>OnDeleteCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public OffersViewModel()
        {
            OnDeleteCommand = new Command<Action>(Delete);
        }

        private void Delete(Action obj)
        {
            
        }
    }
}
