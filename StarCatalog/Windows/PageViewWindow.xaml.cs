using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StarCatalog.Windows
{
    /// <summary>
    /// Логика взаимодействия для PageViewWindow.xaml
    /// </summary>
    public partial class PageViewWindow : Window
    {
        public PageViewWindow(IAsyncPageManager pageManager)
        {
            InitializeComponent();
            CurrentManager.Instance = pageManager;
            SetUp();
        }

        private void SetUp()
        {
            this.PageViewFrame.Navigate(CurrentManager.Instance);
            UpdateButtonState();
        }

        private async void ToNextButton_Click(object sender, RoutedEventArgs e)
        {
            Page nextPage = CurrentManager.Instance.NextPage;
            this.PageViewFrame.Navigate(nextPage);

            this.ToNextButton.IsEnabled = false;

            await CurrentManager.Instance.ShiftToNextPageAsync();

            UpdateButtonState();
        }

        private async void ToPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            Page previousPage = CurrentManager.Instance.PreviousPage;
            this.PageViewFrame.Navigate(previousPage);

            this.ToPreviousButton.IsEnabled = false;

            await CurrentManager.Instance.ShiftToPreviousAsync();

            UpdateButtonState();
        }

        private async void ToFirstButton_OnClick(object sender, RoutedEventArgs e)
        {
            Page firstPage = CurrentManager.Instance.FirstPage;
            this.PageViewFrame.Navigate(firstPage);

            this.ToPreviousButton.IsEnabled = false;
            this.ToNextButton.IsEnabled = false;

            await CurrentManager.Instance.MoveToFirstAsync();

            UpdateButtonState();
        }

        private async void ToLastButton_OnClick(object sender, RoutedEventArgs e)
        {
            Page lastPage = CurrentManager.Instance.LastPage;
            this.PageViewFrame.Navigate(lastPage);

            this.ToPreviousButton.IsEnabled = false;
            this.ToNextButton.IsEnabled = false;

            await CurrentManager.Instance.MoveToLastAsync();

            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            // If prev page is null, disable "prev" button. Same for "next" button.
            this.ToPreviousButton.IsEnabled = (CurrentManager.Instance.PreviousPage != null);
            this.ToNextButton.IsEnabled = (CurrentManager.Instance.NextPage != null);
            
            this.ToFirstButton.IsEnabled = this.ToPreviousButton.IsEnabled;
            this.ToLastButton.IsEnabled = this.ToNextButton.IsEnabled;
        }

        private void TypeComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            var text = this.TypeComboBox.Text;

            if (text == "Stars" && !(CurrentManager.Instance is StarPagesManager))
            {
                var starsList = ConstellationCollectionManager.GetAllStarsWithParallel().ToList();
                CurrentManager.Instance = new StarPagesManager(starsList);
            }
            else if (text == "Constellations" && !(CurrentManager.Instance is ConstellationPagesManager))
            {
                var constellationList = ConstellationCollectionManager.Constellations.ToList();
                CurrentManager.Instance = new ConstellationPagesManager(constellationList);
            }

            this.ToFirstButton_OnClick(sender, new RoutedEventArgs());
        }

        protected override void OnClosed(EventArgs e)
        {
            WindowsManager.GetLastWindow().Show();
        }
    }
}
