using System.Threading.Tasks;
using System.Windows;

namespace Wolfy {

    public partial class SplashScren : Window {

        public SplashScren() {
            InitializeComponent();

            // Start
            Init();
        }

        private async void Init() {

            // Await
            await Task.Delay(500);

            // Init all
            Classes.Init.Start();

        }

    }

}
