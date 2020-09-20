using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PolitiQualityAlpha.Logic.Database;
using PolitiQualityAlpha.Logic.Objects.UI;
using PolitiQualityAlpha.Logic.View_validation;
using System.Threading.Tasks;

namespace PolitiQualityAlpha.Logic.ViewModels
{
    /* View model class for the login creation*/
    public class CreateLoginViewModel : MvxViewModel<LoginDetails, LoginDetails>
    {

        private readonly IMvxNavigationService _navigationService;

        public CreateLoginViewModel(IMvxNavigationService navigationService)
        {

            _navigationService = navigationService;
        }


        private string _usernameEntry;
        public string UsernameEntry
        {
            get => _usernameEntry;
            set
            {
                _usernameEntry = value;
                RaisePropertyChanged(() => UsernameEntry);

            }
        }


        private string _passwordEntry;
        public string PasswordEntry
        {
            get => _passwordEntry;
            set
            {
                _passwordEntry = value;
                RaisePropertyChanged(() => PasswordEntry);

            }
        }

        public IMvxCommand CreateAccountBtn
        {
            get
            {
                return new MvxCommand(async () => await CreateAccount());
            }
        }

        private async Task CreateAccount()
        {
            var valdidate = new TextBox();


           var resultValdidate = valdidate.TextValidation(_usernameEntry, _passwordEntry);

            if (resultValdidate)
            {


                var connection = new UserConnection();

                var credentials = new LoginDetails();



                credentials.Username = _usernameEntry;
               




                var result = connection.CreateUser(_usernameEntry, _passwordEntry);



                if (!result)
                {
                    Mvx.Resolve<IUserDialogs>().Alert("Username already exists");

                }
                else
                {

                    await _navigationService.Close(this, credentials);
                }
            }

        }

        public override async Task Initialize()
        {
            await base.Initialize();

        }

        public override void Prepare(LoginDetails parameter)
        {
            
        }



    }
}
