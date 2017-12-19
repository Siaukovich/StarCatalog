using System;
using System.Collections.Generic;
using System.Windows;
using StarCatalog.Windows;

namespace StarCatalog
{
    /// <summary>
    /// Логика взаимодействия для ConstellationInfoWindow.xaml
    /// </summary>
    public partial class ConstellationInfoWindow : Window
    {
        public ConstellationInfoWindow()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            this.ItemsControl.ItemsSource = new List<Constellation>
            {
                CollectionManager.GetCurrectConstellation()
            };
        }

        private void ConstellationInfoWindow_OnClosed(object sender, EventArgs e)
        {
            WindowsManager.GetLastWindow().Show();
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddPlanetButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (CollectionManager.GetCurrentStars().Count == 0)
            {
                MessageBox.Show("You should add stars before adding planets!", "No stars", MessageBoxButton.OK);
                return;
            }

            WindowsManager.StoreWindow(this);
            this.Hide();
            var addPlanetWindow = new AddPlanetWindow();
            addPlanetWindow.Show();
        }

        private void AddStarButton_OnClick(object sender, RoutedEventArgs e)
        {
            WindowsManager.StoreWindow(this);
            this.Hide();
            var addStarWindow = new AddStarWindow();
            addStarWindow.Show();
        }

        private void CorrectButton_OnClick(object sender, RoutedEventArgs e)
        {
            WindowsManager.StoreWindow(this);
            this.Hide();
            var correctConstellationInfo = new CorrectConstellationWindow();
            correctConstellationInfo.Show();
        }
    }
}
