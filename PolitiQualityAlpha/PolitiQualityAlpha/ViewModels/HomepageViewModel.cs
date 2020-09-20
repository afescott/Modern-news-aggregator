using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PolitiQualityAlpha.Logic.Database;
using PolitiQualityAlpha.Logic.Objects;
using PolitiQualityAlpha.Logic.Objects.UI;
using PolitiQualityAlpha.Logic.View_validation;
using PolitiQualityAlpha.Logic.ViewModels;
using System.Threading.Tasks;

namespace PolitiQuality.ViewModels
{
    /* View model for the home page once the app is launched*/
    public class HomepageViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        private readonly UserConnection Connection = new UserConnection();

        public HomepageViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

        


        }




        public IMvxAsyncCommand ShowPeopleViewModelCommand { get; private set; }
        public bool TextView { get; set; }


        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                RaisePropertyChanged(() => Username);

            }
        }


        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);

            }
        }


        public IMvxCommand BindMe
        {
            get
            {
                return new MvxCommand(async () => await Authenticate());
            }
        }

        public IMvxCommand CreateAccount
        {
            get
            {
                return new MvxCommand(async () => await CreateAccountFSA());
            }
        }
        private async Task CreateAccountFSA()
        {
            LoginDetails dets = new LoginDetails();


            dets.Username = _username;

            var result = await _navigationService.Navigate<CreateLoginViewModel, LoginDetails, LoginDetails>(dets);

            _username = result.Username;
            


            if ((_username != null && _password != null))
            {
                _username = result.Username;
            

                Authenticate();
            }
        }


        private async Task Authenticate()
        {
            Mvx.Resolve<IUserDialogs>().Alert("This mobile application is intended for informational, educational and research purposes only." +
                                    " It is not, and is not intended for use in any real world scenario. Any component of this application" +
                                        " is not liable to Bournemouth university. Use of news articles are protected by the fair-" +
                                             "use doctrine in the Copyright, Designs and Patents Act 1988");


            var valdidate = new TextBox();

            var loginDets = new LoginDetails { UserId = 5, Username = "Ash" };



            await _navigationService.Navigate<NewsDisplayViewModel, CosineRecordSet>(new CosineRecordSet { Details = loginDets });


            var resultValidation = valdidate.TextValidation(_username, _password);

            if (resultValidation)
            {
                var result = Connection.Authenticate(_username, _password);

                

                if (result > 0)
                {
                    var loginDetails = new LoginDetails();

                    loginDetails.Username = _username;
                    loginDetails.UserId = result;


     

                }
               
            } else
            {

            }
                    
                        

        }

        public override async Task Initialize()
        {
           
        }





    }
}
