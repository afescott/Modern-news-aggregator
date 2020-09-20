using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Threading.Tasks;
using PolitiQualityAlpha.Logic.Database;
using PolitiQualityAlpha.Logic.Objects.UI;
using MvvmCross;
using MvvmCross.Plugin.WebBrowser;

namespace PolitiQualityAlpha.Logic.Objects
{
    /* Child view model class for the article sets*/
    public class ArticleSet : MvxNotifyPropertyChanged
    {


        private UserRatingConnection UserRatingConnection = new UserRatingConnection();


        public string Link { get; set; }

        public string Description { get; set; }

        public string Article { get; set; }

        public string Author { get; set; }

        public bool GuardianLogoVisibility { get; set; }

        public bool CnnLogoVisibility { get; set; }

        public bool BbcLogoVisibility { get; set; }


        public DateTime DateRecord { get; set; }

        public LoginDetails UserLoginDetails { get; set; }

        public string UserUpVotes
        { get; set; }

        public string UserDownVotes
        { get; set; }

        public bool DownVoteBackgroundColourToggle
        { get; set; }


        public bool UpVoteBackgroundColourToggle
        {
            get; set;
        }



        public IMvxCommand ArticleLink
        {
            get
            {
                return new MvxCommand(() => NavigateToBrowser());

            }
        }

        private void NavigateToBrowser()

        {


            var task = Mvx.IoCProvider.Resolve<IMvxWebBrowserTask>();
            task.ShowWebPage(Link);


        }




        public IMvxCommand DownVote
        {
            get
            {
                return new MvxCommand(async () => await ProcessDownVote());

            }
        }
        public IMvxCommand UpVote
        {

            get
            {

                return new MvxCommand(async () => await ProcessUpVote());



            }

        }


        public async Task ProcessUpVote()
        {

            UserRatingConnection.RecordVote(true, Author, UserLoginDetails.UserId, Link);

            if (UpVoteBackgroundColourToggle)
            {
                UpVoteBackgroundColourToggle = false;

            }
            else
            {
                UpVoteBackgroundColourToggle = true;

                DownVoteBackgroundColourToggle = false;

            }

            RaiseAllPropertiesChanged();
        }

        public async Task ProcessDownVote()
        {

            UserRatingConnection.RecordVote(false, Author, UserLoginDetails.UserId, Link);

            if (DownVoteBackgroundColourToggle)
            {



                DownVoteBackgroundColourToggle = false;


            }
            else
            {
                UpVoteBackgroundColourToggle = false;

                DownVoteBackgroundColourToggle = true;
            }

            RaiseAllPropertiesChanged();
        }


    }
}
