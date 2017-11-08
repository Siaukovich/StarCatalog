using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using StarCatalog.Helpers;

namespace StarCatalog.Windows
{
    /// <summary>
    /// Логика взаимодействия для CorrectConstellation.xaml
    /// </summary>
    public partial class CorrectConstellationWindow : Window
    {
        public CorrectConstellationWindow()
        {
            InitializeComponent();
            var currectConstellation = ConstellationCollectionManager.GetCurrectConstellation();
            this.Image.Source = new BitmapImage(new Uri(currectConstellation.ImageUri, UriKind.RelativeOrAbsolute));
            this.DataContext = new ConstellationChecker
            {
                Declination = currectConstellation.Coordinates.Declination.Value,
                RightAscension = currectConstellation.Coordinates.RightAscension.Value,
                Name = currectConstellation.Name
            };
        }

        private void ChangePictureButton_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Pictures (*.JPG;*.PNG)|*.JPG;*.PNG",
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            var imageUri = openFileDialog.FileName;
            ConstellationCollectionManager.GetCurrectConstellation().ImageUri = imageUri;
            this.Image.Source = new BitmapImage(new Uri(imageUri));
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!IsValid())
            {
                MessageBox.Show("Data marked with red is incorrect.\n" +
                                "Make data valid and try again.",
                                "Invalid data", MessageBoxButton.OK);

                return;
            }

            var currentConstellation = ConstellationCollectionManager.GetCurrectConstellation();

            currentConstellation.Name = this.NameTextBox.Text;
            currentConstellation.Coordinates = new Coordinates
            (
                new Angle(Convert.ToSingle(this.DeclinationTextBox.Text, CultureInfo.InvariantCulture)),
                new Angle(Convert.ToSingle(this.RightAscensionTextBox.Text, CultureInfo.InvariantCulture))
            );

            MessageBox.Show("Successfully saved!", "Success!", MessageBoxButton.OK);
            InfoStateController.InfoChanged();
            this.Close();
        }

        private bool IsValid()
        {
            return !(Validation.GetHasError(this.NameTextBox) ||
                     Validation.GetHasError(this.RightAscensionTextBox) ||
                     Validation.GetHasError(this.DeclinationTextBox));
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
