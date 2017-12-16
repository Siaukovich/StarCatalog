using System.Collections.Generic;
using System.Windows.Controls;

namespace StarCatalog
{
    /// <summary>
    /// Логика взаимодействия для StarViewPage.xaml
    /// </summary>
    public partial class StarViewPage : Page, IViewPage
    {
        public StarViewPage()
        {
            InitializeComponent();
        }

        public void SetDataContext(int pageIndex)
        {
            List<Star> stars = CollectionManager.GetAllStars();
            Star star = stars[pageIndex - 1];
            this.DataContext = star;
            this.PlanetListBox.ItemsSource = star.Planets;
        }
    }
}
