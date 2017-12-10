using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StarCatalog.Windows
{
    /// <summary>
    /// Логика взаимодействия для ListViewConstellation.xaml
    /// </summary>
    public partial class ListViewConstellation : Window
    {
        public ListViewConstellation()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            LoadCollectionToListBox(ConstellationCollectionManager.GetConstellationsSortedBy("Name"));
        }

        private void LoadCollectionToListBox<T>(IEnumerable<T> collection)
        {
            ConstellationListBox.ItemsSource = collection;
        }
    }
}
