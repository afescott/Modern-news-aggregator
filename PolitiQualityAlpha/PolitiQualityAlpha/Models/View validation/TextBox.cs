using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolitiQualityAlpha.Logic.View_validation
{
   public class TextBox
    {

        public bool TextValidation(string username, string password)
        {

            if ((username != null) && (password != null))
            {

                if ((username.Count() >= 3) && (password.Count() >= 3))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
