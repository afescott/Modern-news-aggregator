using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PolitiQuality.Logic.Objects;
using PolitiQualityAlpha.Logic.Database;
using PolitiQualityAlpha.Logic.Objects;
using PolitiQualityAlpha.Logic.Objects.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolitiQualityAlpha.Logic.ViewModels
{
    /* View model class for the news stories avaliable*/
    public class NewsDisplayViewModel : MvxViewModel<CosineRecordSet, LoginDetails>
    {
        private readonly IMvxNavigationService _navigationService;

        public MvxCommandCollection Collection;

        public List<HtmlRecord> Lists = new List<HtmlRecord>();

        public List<CosineRecordSet> List = new List<CosineRecordSet>();

        private UserConnection UserConnection = new UserConnection();
                   
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }
        public IMvxCommand FilterBtn
        {
            get
            {
                return new MvxCommand(() => NavigateToFilterView());
            }
        }
        private void NavigateToFilterView()
        {
          
            _navigationService.Navigate<FilterViewModel, CosineRecordSet>(UserPreferencesAndDetails);
        }

        private bool[] UserPublisherPreferences = new bool[2];

        public NewsDisplayViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            NewsConnection newsConnection = new NewsConnection();
            
            List = newsConnection.GatherArticles();
        }

        public IMvxAsyncCommand ShowPeopleViewModelCommand { get; private set; }
        public override async Task Initialize()
        {

            GetUserPublisherChoices();

            var list = new List<bool>() { false , false}.Any();
    
            foreach (var item in List)
            {

                if (new List<bool>() { UserPreferencesAndDetails.PublisherBoolBbc, UserPreferencesAndDetails.PublisherBoolCnn, UserPreferencesAndDetails.PublisherBoolGuardian }.Contains(true))

                {
                    var indexLocation = item.ArticleSet.FindIndex(x => x.NewsPublisher.Equals(PublisherEnum.Unspecified));


                    if (UserPreferencesAndDetails.PublisherBoolBbc == true && UserPreferencesAndDetails.PublisherBoolCnn == true && UserPreferencesAndDetails.PublisherBoolGuardian == false)
                    {
                        var returnValue = CheckIfArticlesExists(PublisherEnum.BBC, PublisherEnum.CNN, item.ArticleSet);

                        if (returnValue)
                        {
                            CosineRecords.Add(new CosineRecordSet
                            {
                                HeaderMostSimilarToArticles = item.HeaderMostSimilarToArticles,
                                ArticleSetId = item.ArticleSetId,
                                ArticleSet = item.ArticleSet,
                                Details = _loginDetails,
                                PublisherBoolCnn = true,
                                PublisherBoolGuardian = false,
                                PublisherBoolBbc = true
                            });
                        }
                    }
                    else
                         if (UserPreferencesAndDetails.PublisherBoolBbc == true && UserPreferencesAndDetails.PublisherBoolCnn == false && UserPreferencesAndDetails.PublisherBoolGuardian == true)
                    {
                        var returnValue = CheckIfArticlesExists(PublisherEnum.BBC, PublisherEnum.Guardian, item.ArticleSet);

                        if (returnValue)
                        {

                            CosineRecords.Add(new CosineRecordSet
                            {
                                ArticleSetId = item.ArticleSetId,
                                HeaderMostSimilarToArticles = item.HeaderMostSimilarToArticles,
                                ArticleSet = item.ArticleSet,
                                Details = _loginDetails,
                                PublisherBoolCnn = false,
                                PublisherBoolGuardian = true,
                                PublisherBoolBbc = true
                            });
                        }
                    }
                    else
                                if (UserPreferencesAndDetails.PublisherBoolBbc == false && UserPreferencesAndDetails.PublisherBoolCnn == true && UserPreferencesAndDetails.PublisherBoolGuardian == true)
                    {

                        var returnValue = CheckIfArticlesExists(PublisherEnum.Guardian, PublisherEnum.CNN, item.ArticleSet);

                        if (returnValue)
                        {
                            CosineRecords.Add(new CosineRecordSet
                            {
                                ArticleSetId = item.ArticleSetId,
                                HeaderMostSimilarToArticles = item.HeaderMostSimilarToArticles,
                                ArticleSet = item.ArticleSet,
                                Details = _loginDetails,
                                PublisherBoolCnn = true,
                                PublisherBoolGuardian = true,
                                PublisherBoolBbc = false
                            });
                        }
                    }

                    else if (UserPreferencesAndDetails.PublisherBoolBbc == true && UserPreferencesAndDetails.PublisherBoolCnn == false && UserPreferencesAndDetails.PublisherBoolGuardian == false)
                    {
                        var result = CheckIfArticleExists(PublisherEnum.BBC, item.ArticleSet);

                        if (result)
                        {
                            CosineRecords.Add(new CosineRecordSet
                            {
                                ArticleSetId = item.ArticleSetId,
                                HeaderMostSimilarToArticles = item.HeaderMostSimilarToArticles,
                                ArticleSet = item.ArticleSet,
                                Details = _loginDetails,
                                PublisherBoolCnn = false,
                                PublisherBoolGuardian = false,
                                PublisherBoolBbc = true
                            });
                        }


                    }

                    else if (UserPreferencesAndDetails.PublisherBoolBbc == false && UserPreferencesAndDetails.PublisherBoolCnn == true && UserPreferencesAndDetails.PublisherBoolGuardian == false)
                    {
                        var result = CheckIfArticleExists(PublisherEnum.CNN, item.ArticleSet);

                        if (result)
                        {
                            CosineRecords.Add(new CosineRecordSet
                            {
                                ArticleSetId = item.ArticleSetId,
                                HeaderMostSimilarToArticles = item.HeaderMostSimilarToArticles,
                                ArticleSet = item.ArticleSet,
                                Details = _loginDetails,
                                PublisherBoolCnn = true,
                                PublisherBoolGuardian = false,
                                PublisherBoolBbc = false
                            });
                        }
                    }

                    else if (UserPreferencesAndDetails.PublisherBoolBbc == false && UserPreferencesAndDetails.PublisherBoolCnn == false && UserPreferencesAndDetails.PublisherBoolGuardian == true)
                    {

                        var result = CheckIfArticleExists(PublisherEnum.Guardian, item.ArticleSet);

                        if (result)
                        {
                            CosineRecords.Add(new CosineRecordSet
                            {
                                ArticleSetId = item.ArticleSetId,
                                HeaderMostSimilarToArticles = item.HeaderMostSimilarToArticles,
                                ArticleSet = item.ArticleSet,
                                Details = _loginDetails,
                                PublisherBoolCnn = false,
                                PublisherBoolGuardian = true,
                                PublisherBoolBbc = false
                            });
                        }

                    }




                    else
                    {

                        var boolResults= determineVisibility(item.ArticleSet.ElementAt(0).NewsPublisher, item.ArticleSet.ElementAt(1).NewsPublisher, item.ArticleSet.ElementAt(2).NewsPublisher);

                        if (boolResults.Contains(true))
                        {

                            CosineRecords.Add(new CosineRecordSet
                            {
                                ArticleSetId = item.ArticleSetId,
                                HeaderMostSimilarToArticles = item.HeaderMostSimilarToArticles,
                                ArticleSet = item.ArticleSet,
                                Details = _loginDetails,
                                PublisherBoolCnn = boolResults[1],
                                PublisherBoolGuardian = boolResults[2],
                                PublisherBoolBbc = boolResults[0]
                            });
                        }
                    }

                }

            }

            await base.Initialize();

        }

        private bool[] determineVisibility (PublisherEnum publisherX, PublisherEnum publisherY, PublisherEnum publisherZ)
        {
            if (publisherX == PublisherEnum.CNN && publisherY == PublisherEnum.Guardian && publisherZ == PublisherEnum.BBC  )
            {
                return new bool [] { true, true, true };
            } 
            else if (publisherX == PublisherEnum.CNN && publisherY == PublisherEnum.Guardian && publisherZ == PublisherEnum.Unspecified)
            {
                return new bool[] { true, true, false };
            }
            else if (publisherX == PublisherEnum.CNN && publisherY == PublisherEnum.Unspecified && publisherZ == PublisherEnum.Unspecified)
            {
                return new bool[] { true, false, false };
            }
            else if (publisherX == PublisherEnum.CNN && publisherY == PublisherEnum.Unspecified && publisherZ == PublisherEnum.BBC)
            {
                return new bool[] { true, false, true };
            }
            else if (publisherX == PublisherEnum.Unspecified && publisherY == PublisherEnum.Guardian && publisherZ == PublisherEnum.BBC)
            {
                return new bool[] { false, true, true };
            }
            else if (publisherX == PublisherEnum.Unspecified && publisherY == PublisherEnum.Unspecified && publisherZ == PublisherEnum.BBC)
            {
                return new bool[] { false, false, true };
            }
            else if (publisherX == PublisherEnum.Unspecified && publisherY == PublisherEnum.Guardian && publisherZ == PublisherEnum.Unspecified)
            {
                return new bool[] { false, true, false };
            }
            
                return new bool[] { false, false, false };
            

        }

        private bool CheckIfArticleExists(PublisherEnum publisher, List<HtmlRecord> records)
        {
            var publishers = records.Select(x => x.NewsPublisher);

            if (publishers.Contains(publisher))
            {

                return true;
            }
            
                return false;
            
        }


            private bool CheckIfArticlesExists(PublisherEnum publisherX, PublisherEnum publisherY, List<HtmlRecord> records)
        {
           
            var publishers = records.Select(x => x.NewsPublisher).ToList();

            if (publishers.Contains(publisherX) && publishers.Contains(publisherY))

            {
                return true;
            }
            return false;

        }


        

        private MvxCommand<CosineRecordSet> _itemClickCommand;


        public MvxCommand<CosineRecordSet> ItemClickCommand => _itemClickCommand =
     _itemClickCommand ?? new MvxCommand<CosineRecordSet>(OnItemClickCommand);

        private void OnItemClickCommand(CosineRecordSet recordSet)
        {
          



            _navigationService.Navigate<NewsStoryViewModel, CosineRecordSet>(recordSet);
        }

        public MvxObservableCollection<CosineRecordSet> CosineRecords { get; set; } = new MvxObservableCollection<CosineRecordSet>();

        private LoginDetails _loginDetails = new LoginDetails();

        private CosineRecordSet UserPreferencesAndDetails = new CosineRecordSet();

        private void GetUserPublisherChoices()
        {

            if (UserPreferencesAndDetails.NavigatedFromFilter != true)
            {



                var userPublisherResults = UserConnection.GetPublisherChoices(UserPreferencesAndDetails.Details.UserId);

                for (int i = 0; i <= 2; i++)
                {
                    if (i == 0)
                    {
                        UserPreferencesAndDetails.PublisherBoolGuardian = userPublisherResults.ElementAt(i);
                    }
                    else if (i == 1)
                    {

                        UserPreferencesAndDetails.PublisherBoolCnn = userPublisherResults.ElementAt(i);

                    }
                    else if (i == 2)
                    {

                        UserPreferencesAndDetails.PublisherBoolBbc = userPublisherResults.ElementAt(i);

                    }

                }

            }
        }

        public override void Prepare(CosineRecordSet parameter)
        {    
            UserPreferencesAndDetails = parameter;

            _loginDetails = UserPreferencesAndDetails.Details;

        }

   


    }
}
