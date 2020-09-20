using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Converters;

namespace PolitiQualityAlpha.Droid.Resources.Converters
{
    public class BackgroundValueConverter : MvxValueConverter<bool, ColorDrawable>
    {
        protected override ColorDrawable Convert(bool value, System.Type targetType, object parameter, CultureInfo culture)
        {
            return value ? new ColorDrawable(new Color(125, 255, 0)) : new ColorDrawable(new Color(255, 123, 0));
        }
    }
}