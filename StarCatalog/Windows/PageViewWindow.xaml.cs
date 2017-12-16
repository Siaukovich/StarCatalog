using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StarCatalog
{
    /// <summary>
    /// Логика взаимодействия для PageViewWindow.xaml
    /// </summary>
    public partial class PageViewWindow : Window
    {
        private static class CurrentPageManager
        {
            public static IPagesManager Instance { get; set; }
        }

        public PageViewWindow(IPagesManager pagesManager)
        {
            InitializeComponent();
            CurrentPageManager.Instance = pagesManager;
            SetUp();
        }

        private void SetUp()
        {
            this.PageViewFrame.Navigate(CurrentPageManager.Instance.CurrentPage);
            UpdateButtonState();
        }

        private async void ToNextButton_Click(object sender, RoutedEventArgs e)
        {
            Page nextPage = CurrentPageManager.Instance.NextPage;
            this.PageViewFrame.Navigate(nextPage);

            this.ToNextButton.IsEnabled = false;

            if (CurrentPageManager.Instance.NextPage == null)
                this.ToLastButton.IsEnabled = false;

            TaskScheduler uiTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            await CurrentPageManager.Instance.ShiftToNextPageAsync(uiTaskScheduler);

            UpdateButtonState();
        }

        private async void ToPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            Page previousPage = CurrentPageManager.Instance.PreviousPage;
            this.PageViewFrame.Navigate(previousPage);

            this.ToPreviousButton.IsEnabled = false;

            if (CurrentPageManager.Instance.FirstPage == null)
                this.ToFirstButton.IsEnabled = false;

            TaskScheduler uiTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            await Task.Run(() => CurrentPageManager.Instance.ShiftToPreviousPageAsync(uiTaskScheduler));

            UpdateButtonState();
        }

        private async void ToFirstButton_OnClick(object sender, RoutedEventArgs e)
        {
            Page firstPage = CurrentPageManager.Instance.FirstPage;
            this.PageViewFrame.Navigate(firstPage);

            this.ToPreviousButton.IsEnabled = false;
            this.ToNextButton.IsEnabled = false;

            TaskScheduler uiTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            await CurrentPageManager.Instance.MoveToFirstAsync(uiTaskScheduler);

            UpdateButtonState();
        }

        private async void ToLastButton_OnClick(object sender, RoutedEventArgs e)
        {
            Page lastPage = CurrentPageManager.Instance.LastPage;
            this.PageViewFrame.Navigate(lastPage);

            this.ToPreviousButton.IsEnabled = false;
            this.ToNextButton.IsEnabled = false;

            TaskScheduler uiTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            await CurrentPageManager.Instance.MoveToLastAsync(uiTaskScheduler);

            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            // If prev page is null, disable "prev" button. Same for "next" button.
            this.ToPreviousButton.IsEnabled = (CurrentPageManager.Instance.PreviousPage != null);
            this.ToNextButton.IsEnabled = (CurrentPageManager.Instance.NextPage != null);

            this.ToFirstButton.IsEnabled = this.ToPreviousButton.IsEnabled;
            this.ToLastButton.IsEnabled = this.ToNextButton.IsEnabled;
        }

        private void TypeComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            var text = this.TypeComboBox.Text;

            if (text == "Stars" && !(CurrentPageManager.Instance is PagesManager<StarViewPage>))
            {
                var starsList = CollectionManager.GetAllStarsWithParallel().ToList();
                CurrentPageManager.Instance = new PagesManager<StarViewPage>(starsList);
            }
            else if (text == "Constellations" && !(CurrentPageManager.Instance is PagesManager<ConstellationViewPage>))
            {
                var constellationList = CollectionManager.Constellations.ToList();
                CurrentPageManager.Instance = new PagesManager<ConstellationViewPage>(constellationList);
            }

            this.ToFirstButton_OnClick(sender, new RoutedEventArgs());
        }

        protected override void OnClosed(EventArgs e)
        {
            WindowsManager.GetLastWindow().Show();
        }
    }
}
