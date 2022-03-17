using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLance.Droid
{
    [Activity(Label = "ProLance", Icon = "@drawable/splash_image", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
            // Set our view from the "main" layout resource
            Task startWork = new Task(() =>
            {
                Task.Delay(3000);
            });
            startWork.ContinueWith(t =>
            {

                Intent intent = new Intent(Application.Context, typeof(MainActivity));
                StartActivity(intent);

            }, TaskScheduler.FromCurrentSynchronizationContext());
            startWork.Start();
        }

    }
}