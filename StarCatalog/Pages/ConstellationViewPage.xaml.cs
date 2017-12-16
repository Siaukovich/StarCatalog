using System.Windows.Controls;

namespace StarCatalog
{
    /// <summary>
    /// Логика взаимодействия для ConstellationView.xaml
    /// </summary>
    public partial class ConstellationViewPage : Page, IViewPage
    {
        public ConstellationViewPage()
        {
            InitializeComponent();
        }

        public void SetDataContext(int pageIndex)
        {
            CollectionManager.Current = pageIndex - 1;
            this.DataContext = CollectionManager.Constellations[pageIndex - 1];
        }
    }
}
