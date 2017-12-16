using System;
using System.Windows;
using System.Windows.Controls;
using StarCatalog.Helpers;

namespace StarCatalog
{
    /// <summary>
    /// Логика взаимодействия для AddPlanetWindow.xaml
    /// </summary>
    public partial class AddPlanetWindow : Window
    {
        public AddPlanetWindow()
        {
            InitializeComponent();
            this.StarNameComboBox.ItemsSource = CollectionManager.GetCurrentStars();
            this.DataContext = new Planet();
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

            if (StarNameComboBox.Text == String.Empty)
            {
                MessageBox.Show("Choose a host star!", "Host star not chosen", MessageBoxButton.OK);
                return;
            }

            try
            {
                var p = new Planet();
                p.Name = this.NameTextBox.Text;
                p.Radius = Convert.ToSingle(this.RadiusTextBox.Text);
                p.Mass = Convert.ToDouble(this.MassTextBox.Text);
                p.OrbitRadius = Convert.ToSingle(this.OrbitRadiusTextBox.Text);
                p.SiderealDay = Convert.ToSingle(this.SideralDayTextBox.Text);
                p.SiderealYear = Convert.ToSingle(this.SiderealYearTextBox.Text);

                foreach (var star in CollectionManager.GetCurrentStars())
                {
                    if (star.Name != StarNameComboBox.Text)
                        continue;

                    star.AddPlanet(p);
                    break;
                }

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
            return !(Validation.GetHasError(this.NameTextBox)        ||
                     Validation.GetHasError(this.RadiusTextBox)      ||
                     Validation.GetHasError(this.MassTextBox)        ||
                     Validation.GetHasError(this.OrbitRadiusTextBox) ||
                     Validation.GetHasError(this.SideralDayTextBox)  ||
                     Validation.GetHasError(this.SiderealYearTextBox)); 
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
