using Prism.Unity;
using ArcTouch.TestDevelopment.Views;
using Xamarin.Forms;

namespace ArcTouch.TestDevelopment
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync("NavigationPage/MainPage?title=ArcTouch - Movies");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<MovieDetailsPage>();
        }
    }
}
