using System.Windows.Controls;

namespace StarCatalog
{
    /// <summary>
    /// Логика взаимодействия для StarViewPage.xaml
    /// </summary>
    public partial class StarViewPage : Page
    {
        public StarViewPage(int pageIndex)
        {
            InitializeComponent();
            var stars = CollectionManager.GetAllStars();
            var star = stars[pageIndex - 1];
            this.DataContext = star;
            this.PlanetListBox.ItemsSource = star.Planets;
        }
    }
}
