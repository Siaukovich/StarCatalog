using System.Windows.Controls;

namespace StarCatalog
{
    /// <summary>
    /// Логика взаимодействия для ConstellationView.xaml
    /// </summary>
    public partial class ConstellationViewPage : Page
    {
        public ConstellationViewPage(int pageIndex)
        {
            CollectionManager.Current = pageIndex - 1;
            InitializeComponent();
            this.DataContext = CollectionManager.Constellations[pageIndex - 1];
        }
    }
}
