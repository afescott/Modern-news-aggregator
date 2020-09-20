using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public class FilterView : MvxActivity<FilterViewModel>

    {
        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.FilterView);
        }


        }
}