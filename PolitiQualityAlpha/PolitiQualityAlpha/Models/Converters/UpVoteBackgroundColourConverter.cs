using MvvmCross.Platform.UI;
using MvvmCross.Plugin.Color;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace PolitiQualityAlpha.Logic.Converters
{
    public class UpVoteColourBackgroundValueConverter : MvxColorValueConverter<bool>
    {
     
        protected override Color Convert(bool value, object parameter, CultureInfo culture)
        {
            return value ? Color.FromArgb(0, 255, 80) : Color.FromArgb(255,255,255);
        }
    }
}
