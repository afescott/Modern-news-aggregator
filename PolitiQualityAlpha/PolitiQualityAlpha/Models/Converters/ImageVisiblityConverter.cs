using MvvmCross.UI;
using MvvmCross.Plugin.Visibility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PolitiQualityAlpha.Logic.Converters
{
    public class ImageVisiblityConverter : MvxBaseVisibilityValueConverter<bool>
    {
        protected override MvxVisibility Convert(bool value, object parameter, CultureInfo culture)
        {
           return (value) ? MvxVisibility.Visible : MvxVisibility.Collapsed;
        }
    }
}
