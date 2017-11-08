using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using StarCatalog.Helpers;

namespace StarCatalog.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddStarWindow.xaml
    /// </summary>
    public partial class AddStarWindow : Window
    {
        public AddStarWindow()
        {
            InitializeComponent();
            this.DataContext = new Star();
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!IsValid())
            {
                MessageBox.Show("Data marked with red is incorrect.\n" +
                                "Make data valid and try again.",
                                "Invalid data", MessageBoxButton.OK);

                return;
            }

            try
            {
                var s = new Star();
                s.Name = this.NameTextBox.Text;
                s.Radius = Convert.ToDouble(this.RadiusTextBox.Text);
                s.Mass = Convert.ToDouble(this.MassTextBox.Text);
                s.Temperature = Convert.ToDouble(this.TemperatureTextBox.Text);

                ConstellationCollectionManager.GetCurrectConstellation().AddStar(s);

                MessageBox.Show("Successfully added!", "Success!", MessageBoxButton.OK);

                InfoStateController.InfoChanged();

                this.Close();
            }
            catch
            {
                MessageBox.Show("One of your values was too big!", "Overflow error", MessageBoxButton.OK);
            }
        }

        private bool IsValid()
        {
            return !(Validation.GetHasError(this.NameTextBox)      ||
                     Validation.GetHasError(this.RadiusTextBox)    ||
                     Validation.GetHasError(this.MassTextBox)      ||
                     Validation.GetHasError(this.TemperatureTextBox)); 
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            WindowsManager.GetLastWindow().Show();
        }
    }
}
