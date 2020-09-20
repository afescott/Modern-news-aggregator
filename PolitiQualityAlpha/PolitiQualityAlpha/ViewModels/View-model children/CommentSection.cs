using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PolitiQualityAlpha.Logic.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PolitiQualityAlpha.Logic.Objects.UI
{
    /* Child view model class for generating the comment section */
    public class CommentSection : MvxNotifyPropertyChanged
    {


        public int CommentId { get; set; }

        public string CommentText { get; set; }

        public int UserId { get; set; }

        public DateTime CommentDate { get; set; }

        public int ArticleSetId { get; set; }


        public string Username { get; set; }


        private string _generatedComment;
        public string GeneratedComment
        {
            get => _generatedComment;
            set => SetProperty(ref _generatedComment, value);
        }

        public double CommentUpVotes { get; set; }

        public double CommentDownVotes { get; set; }

        public string DateOfComment { get; set; }

        private UserRatingConnection Connection = new UserRatingConnection();
        public bool DownVoteCommentBackgroundColourToggle
        { get; set; }


        public bool UpVoteCommentBackgroundColourToggle
        {
            get; set;
        }


        public IMvxCommand UpVoteComment
        {
            get
            {
                return new MvxCommand(async () => await ProcessUpVoteComment());

            }
        }

        public IMvxCommand DownVoteComment
        {
            get
            {
                return new MvxCommand(async () => await ProcessDownVoteComment());

            }
        }

        private UserRatingConnection UserRatingConnection = new UserRatingConnection();

        public async Task ProcessDownVoteComment()
        { 
           UserRatingConnection.RecordCommentVote(new CommentSection { CommentId = CommentId, UserId = UserId }, false);

            if (DownVoteCommentBackgroundColourToggle)
            {
                DownVoteCommentBackgroundColourToggle = false;

            }
            else
            {

                UpVoteCommentBackgroundColourToggle = false;

                DownVoteCommentBackgroundColourToggle = true;


            }
            RaiseAllPropertiesChanged();

        }

        public async Task ProcessUpVoteComment()
        {

            UserRatingConnection.RecordCommentVote(new CommentSection { CommentId = CommentId, UserId = UserId },true);

            if (UpVoteCommentBackgroundColourToggle)
            {
                UpVoteCommentBackgroundColourToggle = false;

            }
            else
            {
                UpVoteCommentBackgroundColourToggle = true;

                DownVoteCommentBackgroundColourToggle = false;

            }

            RaiseAllPropertiesChanged();


        }





    }
}
