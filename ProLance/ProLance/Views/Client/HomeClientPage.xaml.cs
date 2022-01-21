using ProLance.Models;
using Plugin.CloudFirestore;
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
    public partial class HomeClientPage : ContentPage
    {
        public HomeClientPage()
        {
            InitializeComponent();
            GetServices();
        }
		private ObservableCollection<ServiceCategories> serviceCategories = new ObservableCollection<ServiceCategories>();
		public ObservableCollection<ServiceCategories> ServiceCategories { get { return serviceCategories; } }
		private void GetServices()
        {
			ClientServiceCategory.BindingContext = this;
			ClientServiceCategory.ItemsSource = ServiceCategories;
			CrossCloudFirestore
				.Current
				.Instance
				.Collection("CATEGORIES")
				.AddSnapshotListener((value, error) =>
				{
					if (!value.IsEmpty)
					{
						foreach (var item in value.DocumentChanges)
						{
							var category = new ServiceCategories();
							switch (item.Type)
							{
								case DocumentChangeType.Added:
									category = item.Document.ToObject<ServiceCategories>();
									ServiceCategories.Add(category);
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

        private void ImgService_Clicked(object sender, EventArgs e)
        {
			var img = (ImageButton)sender;
			var id  = img.CommandParameter.ToString();
			Navigation.PushModalAsync(new ServiceDetailsPage(id));
        }

       
    }
}