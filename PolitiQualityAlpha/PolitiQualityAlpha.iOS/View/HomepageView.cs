using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmCross.Platforms.Ios.Views;
using Foundation;
using UIKit;
using PolitiQuality.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;

namespace PolitiQualityAlpha.iOS.View
{
    public partial class HomepageView : MvxViewController<HomepageViewModel>
    {
        public HomepageView() : base(nameof(HomepageView), null)
    {
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var set = this.CreateBindingSet<HomepageView, HomepageViewModel>();
        //set.Bind(TipLabel).To(vm => vm.Tip);
        //set.Bind(SubTotalTextField).To(vm => vm.SubTotal);
        //set.Bind(GenerositySlider).To(vm => vm.Generosity);
        //set.Apply();

        //View.AddGestureRecognizer(new UITapGestureRecognizer(() =>
        //{
        //    this.SubTotalTextField.ResignFirstResponder();
        //}));
    }
}
}