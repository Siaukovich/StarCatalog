using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using StarCatalog.Helpers;

namespace StarCatalog.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddConstellationWindow.xaml
    /// </summary>
    public partial class AddConstellationWindow : Window
    {
        private string _pictureUri;

        public AddConstellationWindow()
        {
            InitializeComponent();
            this.DataContext = new ConstellationChecker();
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

                var constellation = new Constellation();
                constellation.Name = this.NameTextBox.Text;
                constellation.Coordinates = new Coordinates
                {
                    RightAscension = new Angle(Convert.ToSingle(this.RightAscensionTextBox.Text)),
                    Declination = new Angle(Convert.ToSingle(this.DeclinationTextBox.Text))
                };
                constellation.ImageUri = _pictureUri;

                ConstellationCollectionManager.AddConstellation(constellation);

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
            return !(Validation.GetHasError(this.NameTextBox)           ||
                     Validation.GetHasError(this.RightAscensionTextBox) ||
                     Validation.GetHasError(this.DeclinationTextBox));
        }

        private void ChoosePictureButton_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Pictures (*.JPG;*.PNG)|*.JPG;*.PNG",
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            _pictureUri = openFileDialog.FileName;
            this.Image.Source = new BitmapImage(new Uri(_pictureUri));
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
