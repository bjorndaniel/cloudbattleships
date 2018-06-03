using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Battleships.XamFormsUI
{
    public partial class App : Application
	{
		public App ()
		{
#if DEBUG
            //LiveReload.Init(); TODO: Fix version mismatch with extension.
#endif

            InitializeComponent();

			MainPage = new MainPage();
		}

		protected override void OnStart ()
		{
			
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}


	}
}
