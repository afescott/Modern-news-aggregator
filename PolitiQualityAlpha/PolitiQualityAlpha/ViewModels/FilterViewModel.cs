using MvvmCross.Commands;
using MvvmCross.Core;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PolitiQualityAlpha.Logic.Database;
using PolitiQualityAlpha.Logic.Objects;
using PolitiQualityAlpha.Logic.Objects.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PolitiQualityAlpha.Logic.ViewModels
{
    public class FilterViewModel : MvxViewModel<CosineRecordSet>
    {


        private readonly IMvxNavigationService _navigationService;

        private LoginDetails _loginDetails;

        private UserFilterChoices StartUpChoices = new UserFilterChoices();
        public override void Prepare(CosineRecordSet parameter)
        {
            _loginDetails = parameter.Details;

            StartUpChoices.BbcPreference = parameter.PublisherBoolBbc;
            StartUpChoices.GuardianPreference = parameter.PublisherBoolGuardian;
            StartUpChoices.CnnPreference = parameter.PublisherBoolCnn;

            CheckBoxSelectedBbc = parameter.PublisherBoolBbc;
            CheckBoxSelectedGuardian = parameter.PublisherBoolGuardian;
            CheckBoxSelectedCnn = parameter.PublisherBoolCnn;
        }

        public FilterViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }


        private void Close()
        {

            var userConnection = new UserConnection();

            var choices = new UserFilterChoices
            {
                BbcPreference = _checkBoxSelectedBbc,
                CnnPreference = _checkBoxSelectedCnn,
                GuardianPreference = _checkBoxSelectedGuardian
            };

            userConnection.UpdateUserPreferences(_loginDetails.UserId, choices);



            _navigationService.Navigate<NewsDisplayViewModel, CosineRecordSet>(new CosineRecordSet() { NavigatedFromFilter = true, Details = _loginDetails, PublisherBoolBbc = CheckBoxSelectedBbc, PublisherBoolGuardian = CheckBoxSelectedGuardian, PublisherBoolCnn = CheckBoxSelectedCnn });

            _navigationService.Close(this);

        }

        public IMvxCommand CloseCommand
        {
            get
            {
                return new MvxCommand(() => Close());
            }
        }




        private bool _checkBoxSelectedCnn;
        public bool CheckBoxSelectedCnn
        {
            get { return _checkBoxSelectedCnn; }
            set { _checkBoxSelectedCnn = value; RaisePropertyChanged(() => CheckBoxSelectedCnn); }
        }

        private bool _checkBoxSelectedGuardian;
        public bool CheckBoxSelectedGuardian
        {
            get { return _checkBoxSelectedGuardian; }
            set { _checkBoxSelectedGuardian = value; RaisePropertyChanged(() => CheckBoxSelectedGuardian); }
        }

        private bool _checkBoxSelectedBbc;
        public bool CheckBoxSelectedBbc
        {
            get { return _checkBoxSelectedBbc; }
            set { _checkBoxSelectedBbc = value; RaisePropertyChanged(() => CheckBoxSelectedBbc); }
        }
    }

      
}
