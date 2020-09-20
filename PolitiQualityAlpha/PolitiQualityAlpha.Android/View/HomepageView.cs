

using Android.App;
using Android.OS;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views;
using PolitiQuality.ViewModels;
using PolitiQualityAlpha.Droid;
using System.Globalization;
using System.IO.MemoryMappedFiles;


namespace PolitiQuality.Droid.View
{
    [Activity(Label = "Main View", MainLauncher = true)]
    public class HomepageView : MvxActivity<HomepageViewModel>
    {

    
        protected override void OnCreate(Bundle bundle)
        {
           
            //  string curFile = @"C:\Users\Swifty\Documents\TipCalc\Values.txt";
            //var  fsrw = new FileStream("Values.txt", FileMode.Open, FileAccess.ReadWrite);


            base.OnCreate(bundle);
            SetContentView(Resource.Layout.HomepageView123);

            Acr.UserDialogs.UserDialogs.Init(this);
        }

        protected override void OnViewModelSet()
        {





            //var edit = this.FindViewById<TextView>(Resource.Id.edit);



            //var textBox = this.FindViewById<EditText>(Resource.Id.editText1);
            //var btn = this.FindViewById<Button>(Resource.Id.button2);
            ////var btn1 = this.FindViewById<Button>(Resource.Id.button2);
            ////var btn2 = this.FindViewById<Button>(Resource.Id.button3);

            //var set = this.CreateBindingSet<HomepageView, HomepageViewModel>();
            //set.Bind(edit).To(vm => vm.TextView);
            //////set.Bind(textBox).To(vm => vm.MyProperty);
            ////set.Bind(btn).To(vm => vm.Command);
            //////set.Bind(btn1).To(vm => vm.Command);
            //////set.Bind(btn2).To(vm => vm.Command);
            //set.Apply();

        }
        }
}