using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Views;
using PolitiQualityAlpha.Logic.ViewModels;

namespace PolitiQualityAlpha.Droid.View
{
    [Activity(Label = "View", MainLauncher = false)]
    public class CreateLoginView : MvxActivity<CreateLoginViewModel>
    {

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.CreateLoginView);


            
            //var toolbar = FindViewById<Toolbar>(Resource.Id.toolbarJim);

            //Toolbar will now take on default actionbar characteristics
            //SetActionBar(toolbar);


            //do article stored procedures and prototype how the article info will be portrayed- in grid of some kind 

            ActionBar.Title = "Hello from Toolbar";


            var toolbarBottom = FindViewById<Toolbar>(Resource.Id.toolbarJim);

            toolbarBottom.Title = "Photo Editing";
            //toolbarBottom.InflateMenu(Resource.Layout.);
            toolbarBottom.MenuItemClick += (sender, e) =>
            {
                Toast.MakeText(this, "Bottom toolbar pressed: " + e.Item.TitleFormatted, ToastLength.Short).Show();
            };


            UserDialogs.Init(this);

        }

    }
}