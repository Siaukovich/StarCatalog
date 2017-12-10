using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для PageViewWindow.xaml
    /// </summary>
    public partial class PageViewWindow : Window
    {
        public PageViewWindow()
        {
            InitializeComponent();
        }

        protected override async void OnActivated(EventArgs e)
        {
            try
            {
                int firtstPage = 1;
                await ConstellationPagesManager.UpdateAsync(firtstPage);
                UpdateButtonState();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void ToPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            int prevPageNumber = ConstellationPagesManager.CurrentPageNumber - 1;

            ConstellationViewPage prevPage = ConstellationPagesManager.PreviousPage;
            this.PageViewFrame.Navigate(prevPage);

            await ConstellationPagesManager.UpdateAsync(prevPageNumber);

            ConstellationCollectionManager.Current = ConstellationPagesManager.CurrentPageNumber - 1;
            UpdateButtonState();
        }

        private async void ToNextButton_Click(object sender, RoutedEventArgs e)
        {
            int nextPageNumber = ConstellationPagesManager.CurrentPageNumber + 1;

            ConstellationViewPage nextPage = ConstellationPagesManager.NextPage;
            this.PageViewFrame.Navigate(nextPage);

            await ConstellationPagesManager.UpdateAsync(nextPageNumber);

            ConstellationCollectionManager.Current = ConstellationPagesManager.CurrentPageNumber + 1;
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            this.ToPreviousButton.IsEnabled = ConstellationPagesManager.PreviousPage != null;
            this.ToNextButton.IsEnabled = ConstellationPagesManager.NextPage != null;
        }

        protected override void OnClosed(EventArgs e)
        {
            WindowsManager.GetLastWindow().Show();
        }
    }
}
