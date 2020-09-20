using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PolitiQuality.Logic.Objects;
using PolitiQualityAlpha.Logic.Objects.UI;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace PolitiQualityAlpha.Logic.Objects
{
   public class CosineRecordSet : MvxNotifyPropertyChanged
    {
        public LoginDetails Details { get; set; }
      public List<HtmlRecord> ArticleSet { get; set; }

        public bool NavigatedFromFilter { get; set; }

        public int ArticleSetId { get; set; }

        public string HeaderMostSimilarToArticles { get; set; }

      

        private bool _publisherBoolCnn;
        public bool PublisherBoolCnn
        {
            get
            {
                return _publisherBoolCnn;
            }
            set
            {
                _publisherBoolCnn = value;
                RaisePropertyChanged(() => PublisherBoolCnn);
            }
        }

        private bool _publisherBoolGuardian;
        public bool PublisherBoolGuardian
        {
            get
            {
                return _publisherBoolGuardian;
            }
            set
            {
                _publisherBoolGuardian = value;
                RaisePropertyChanged(() =>  PublisherBoolGuardian);
            }
        }


        private bool _publisherBoolBbc;
        public bool PublisherBoolBbc
        {
            get
            {
                return _publisherBoolBbc;
            }
            set
            {
        _publisherBoolBbc = value;
                RaisePropertyChanged(() => PublisherBoolBbc);
            }
        }





    }
}
