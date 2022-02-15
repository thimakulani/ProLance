using ProLance.Models;
using Plugin.CloudFirestore;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProLance.Views.Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceDetailsPage : ContentPage
    {
        public ServiceDetailsPage(string id)
        {
            InitializeComponent();
            BindingContext = this;
            CategoryServices.ItemsSource = Items;
            
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("CATEGORIES")
                .Document(id)
                .AddSnapshotListener((v, e) =>
                {
                    if (v.Exists)
                    {
                        TxtHeading.Text = $"SERVICE CATEGORY SELECTED: {v.ToObject<ServiceCategories>().Name}";
                    }
                });
            LoadSearvices(id);
        }
        private readonly ObservableCollection<Services> items = new ObservableCollection<Services>();
        public ObservableCollection<Services> Items { get { return items; } }
        private void LoadSearvices(string id)
        {
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("SERVICES")
                .WhereEqualsTo("Category", id)
                .AddSnapshotListener((v, e) =>
                {
                    if (!v.IsEmpty)
                    {
                        foreach (var item in v.DocumentChanges)
                        {
                            switch (item.Type)
                            {
                                case DocumentChangeType.Added:
                                    var data = item.Document.ToObject<Services>();
                                    items.Add(data);
                                    break;
                                case DocumentChangeType.Modified:
                                    break;
                                case DocumentChangeType.Removed:
                                    break;
                            }
                        }
                    }
                });
        }

        private void CategoryServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.CurrentSelection.FirstOrDefault() as Services;
            PopupNavigation.Instance.PushAsync(new RequestServiceDlg(item.Id));
            
        }
    }
}