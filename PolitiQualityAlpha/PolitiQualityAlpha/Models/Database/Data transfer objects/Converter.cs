using System;
using System.Collections.Generic;
using System.Text;

namespace PolitiQualityAlpha.Logic.Database.Data_transfer_objects
{
  public  class Converter
    {

        public DateTime StringToDate (string date)
        {


            return (DateTime)Convert.ToDateTime(date);
        }
    }
}
