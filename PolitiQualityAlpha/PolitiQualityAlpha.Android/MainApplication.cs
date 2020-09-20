using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views;

using PolitiQuality.ViewModels;


namespace PolitiQuality.Droid
{
    [Application]
    public class MainApplication : MvxAndroidApplication<MvxAndroidSetup<App>, App>
    {
        
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

            //var homepageModel = new HomepageViewModel();


            OnCreate(new Bundle());




            //homepageModel.DoNewsStuff();



        }

        public void OnCreate (Bundle bundle)
        {
                       

          
        }

    }
}





