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
            var currectConstellation = CollectionManager.GetCurrectConstellation();
            this.Image.Source = new BitmapImage(new Uri(currectConstellation.ImageUri, UriKind.RelativeOrAbsolute));
            this.DataContext = new ConstellationChecker
            {
                Declination = currectConstellation.Coordinates.Declination.Value,
                RightAscension = currectConstellation.Coordinates.RightAscension.Value,
                Name = currectConstellation.Name
            };
        }

        private void ChangeImageButton_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Pictures (*.JPG;*.PNG)|*.JPG;*.PNG",
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            var imageUri = openFileDialog.FileName;
            CollectionManager.GetCurrectConstellation().ImageUri = imageUri;
            this.Image.Source = new BitmapImage(new Uri(imageUri));
        }

        private void ResetImageButton_OnClick(object sender, RoutedEventArgs e)
        {
            var path = Environment.CurrentDirectory;
            const string cutOffPart = @"Debug\bin";
            path = path.Substring(0, path.Length - cutOffPart.Length) + @"Images\NoImage.png";
            this.Image.Source = new BitmapImage(new Uri(path));
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

            var currentConstellation = CollectionManager.GetCurrectConstellation();

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
