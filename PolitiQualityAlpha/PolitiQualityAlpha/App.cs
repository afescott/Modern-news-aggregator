using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using PolitiQuality.ViewModels;

namespace PolitiQuality
{

    public class App : MvxApplication
    {

        public override void Initialize()
        {

            //var find = new FindArticleInfoModel();

            //find.CalculateNewArticles();

         
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.IoCProvider.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);

            RegisterAppStart<HomepageViewModel>();
        }

    }
}
