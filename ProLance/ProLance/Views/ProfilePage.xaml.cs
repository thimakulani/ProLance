using ProLance.Models;
using ProLance.ViewModels;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProLance.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
		public User user = new User();
		public ProfilePage ()
		{
			InitializeComponent ();

			ProfileModelView profileModelView = new ProfileModelView ();
			BindingContext = profileModelView;

		}
	}
}