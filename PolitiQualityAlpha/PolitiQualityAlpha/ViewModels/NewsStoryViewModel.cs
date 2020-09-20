using MvvmCross.ViewModels;
using PolitiQualityAlpha.Logic.Objects;
using System;
using MvvmCross.Navigation;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PolitiQualityAlpha.Logic.Database;
using PolitiQualityAlpha.Logic.Objects.UI;

namespace PolitiQualityAlpha.Logic.ViewModels
{
    /* View model class for the news stories avaliable*/
    public class NewsStoryViewModel : MvxViewModel<CosineRecordSet>, IMvxNotifyPropertyChanged
    {
        private UserRatingConnection connection = new UserRatingConnection();

    
        public IMvxCommand CommentSubmit
        {
            get
            {
                return new MvxCommand(async () => await CommentAuthenticate());
            }
        }

        private string _usercomment;
        public string UserComment
        {
            get => _usercomment;
            set
            {
                _usercomment = value;
                RaisePropertyChanged(() => UserComment);

            }
        }
        private readonly IMvxNavigationService _navigationService;

        public NewsStoryViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

        }


        public MvxObservableCollection<ArticleSet> Articles { get; set; } = new MvxObservableCollection<ArticleSet>();

        public MvxObservableCollection<CommentSection> CommentSection { get; set; } = new MvxObservableCollection<CommentSection>();



        private double[] CaclulatePercentage(double votesX, double votesY)
        {



            var percentageUpVotes = 0.0;

            var percentageDownVotes = 0.0;

            if (votesX == 0 && votesY == 0)
            {

                percentageUpVotes = 0;

            }
            else if (votesX > 0 && votesY == 0)
            {
                percentageUpVotes = 100;
            }
            else if (votesX == 0 && votesY > 0)
            {
                percentageDownVotes = 100;

            }
            else
            {
                percentageUpVotes = (votesX / votesY) * 100;
                percentageDownVotes = 100 - percentageUpVotes;

            }

            return new double[] { percentageUpVotes, percentageDownVotes };
        }
        public async override Task Initialize()
        {
            var commentList = CommentConnection.GetComments(ArticleRecordInfo.ArticleSetId);  

            foreach (var comment in commentList)
            {
                var percentages = CaclulatePercentage(comment.CommentUpVotes, comment.CommentDownVotes);


                CommentSection.Add(new   CommentSection() { DownVoteCommentBackgroundColourToggle =  comment.DownVoteCommentBackgroundColourToggle, 
                    UpVoteCommentBackgroundColourToggle = comment.UpVoteCommentBackgroundColourToggle, Username = ArticleRecordInfo.Details.Username,
                    CommentId = comment.CommentId, UserId = ArticleRecordInfo.Details.UserId,   GeneratedComment = comment.CommentText, DateOfComment = DateTime.Today.Date.ToString("yyyy-MM-dd"), CommentDownVotes = percentages[1], CommentUpVotes = percentages[0] });

            }

            foreach (var item in ArticleRecordInfo.ArticleSet)
            {
                    var result = connection.GatherUserArticleData(ArticleRecordInfo.Details.UserId, item.Link);

                var articleVotes = connection.GetArticleResults(item.Link);

                var results = CaclulatePercentage( articleVotes[0], articleVotes[1]);

                var percentageUpVotes = results[0];

                var percentageDownVotes = results[1];

                if (result == 1)
                {

                    if (item.NewsPublisher == PublisherEnum.BBC)
                    {
                        Articles.Add(new ArticleSet()
                        {
                            Author = item.Author,
                            Link = item.Link,
                            DateRecord = item.DateRecord,
                            UserLoginDetails = ArticleRecordInfo.Details,
                            DownVoteBackgroundColourToggle = false,
                            UpVoteBackgroundColourToggle = true,
                            BbcLogoVisibility = true,
                            UserUpVotes = percentageUpVotes.ToString(),
                            UserDownVotes = percentageDownVotes.ToString()
                        });
                    }
                    else if (item.NewsPublisher == PublisherEnum.Guardian)
                    {
                        Articles.Add(new ArticleSet()
                        {
                            Author = item.Author,
                            Link = item.Link,
                            DateRecord = item.DateRecord,
                            UserLoginDetails = ArticleRecordInfo.Details,
                            DownVoteBackgroundColourToggle = false,
                            UpVoteBackgroundColourToggle = true,
                            GuardianLogoVisibility = true,
                            UserUpVotes = percentageUpVotes.ToString(),
                            UserDownVotes = percentageDownVotes.ToString()
                        });
                    }
                    else if (item.NewsPublisher == PublisherEnum.CNN)
                    {
                        Articles.Add(new ArticleSet()
                        {
                            Author = item.Author,
                            Link = item.Link,
                            DateRecord = item.DateRecord,
                            UserLoginDetails = ArticleRecordInfo.Details,
                            DownVoteBackgroundColourToggle = false,
                            UpVoteBackgroundColourToggle = true,
                            CnnLogoVisibility = true,
                            UserUpVotes = percentageUpVotes.ToString(),
                            UserDownVotes = percentageDownVotes.ToString()

                        });
                    }

                }
                else if (result == 2)
                {
                    if (item.NewsPublisher == PublisherEnum.BBC)
                    {
                        Articles.Add(new ArticleSet()
                        { Author = item.Author, Link = item.Link, DateRecord = item.DateRecord, UserLoginDetails = ArticleRecordInfo.Details, DownVoteBackgroundColourToggle = true, UpVoteBackgroundColourToggle = false, BbcLogoVisibility = true, UserUpVotes = percentageUpVotes.ToString(), UserDownVotes = percentageDownVotes.ToString() });
                    }
                    if (item.NewsPublisher == PublisherEnum.CNN)
                    {
                        Articles.Add(new ArticleSet()
                        {
                            Author = item.Author,
                            Link = item.Link,
                            DateRecord = item.DateRecord,
                            UserLoginDetails = ArticleRecordInfo.Details,
                            DownVoteBackgroundColourToggle = true,
                            UpVoteBackgroundColourToggle = false,
                            CnnLogoVisibility = true,
                            UserUpVotes = percentageUpVotes.ToString(),
                            UserDownVotes = percentageDownVotes.ToString()
                        });
                    }
                    if (item.NewsPublisher == PublisherEnum.Guardian)
                    {
                        Articles.Add(new ArticleSet()
                        { Author = item.Author, Link = item.Link, DateRecord = item.DateRecord, UserLoginDetails = ArticleRecordInfo.Details, DownVoteBackgroundColourToggle = true, UpVoteBackgroundColourToggle = false, GuardianLogoVisibility = true, UserUpVotes = percentageUpVotes.ToString(), UserDownVotes = percentageDownVotes.ToString() });



                    }
                }
                else if (result == 0)
                {
                    if (item.NewsPublisher == PublisherEnum.BBC)
                    {
                        Articles.Add(new ArticleSet()
                        { Author = item.Author, Link = item.Link, 
                            DateRecord = item.DateRecord, UserLoginDetails = ArticleRecordInfo.Details, 
                            BbcLogoVisibility = true, UserUpVotes = percentageUpVotes.ToString(), UserDownVotes = percentageDownVotes.ToString() });
                    }
                    if (item.NewsPublisher == PublisherEnum.CNN)
                    {
                        Articles.Add(new ArticleSet()
                        { Author = item.Author, Link = item.Link, DateRecord = item.DateRecord, 
                            UserLoginDetails = ArticleRecordInfo.Details, CnnLogoVisibility = true, UserUpVotes = percentageUpVotes.ToString(),
                            UserDownVotes = percentageDownVotes.ToString() });
                    }
                    if (item.NewsPublisher == PublisherEnum.Guardian)
                    {
                        Articles.Add(new ArticleSet()
                        { Author = item.Author, Link = item.Link, DateRecord = item.DateRecord, 
                            UserLoginDetails = ArticleRecordInfo.Details, GuardianLogoVisibility = true,
                            UserUpVotes = percentageUpVotes.ToString(), UserDownVotes = percentageDownVotes.ToString() });
                    }

                }


            }

        }

        private CommentConnection CommentConnection = new CommentConnection();
        private async Task CommentAuthenticate ()
        {
           
            await RaiseAllPropertiesChanged();

            CommentConnection.SendComment(ArticleRecordInfo.Details.UserId, _usercomment, ArticleRecordInfo.ArticleSetId);
        }

        private CosineRecordSet ArticleRecordInfo { get; set; }

        public override void Prepare(CosineRecordSet parameter)
        {
            ArticleRecordInfo = parameter;

        }

        private PublisherEnum _myDrawable;
        public PublisherEnum MyDrawable
        {
            get { return _myDrawable; }
            set
            {
                _myDrawable = value;
                RaisePropertyChanged(() => MyDrawable);
            }


        }




    }



}
