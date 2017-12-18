using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using StarCatalog.Helpers;
using StarCatalog.Windows;

namespace StarCatalog
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadCollectionToListBox(CollectionManager.GetConstellationsSortedBy("Name"));
            this.Show();
            LoadPlugins();
            SetHotkeys();
        }

        private async void LoadPlugins()
        {
            this.PluginsMenuItem.IsEnabled = false;
            this.PluginsMenuItem.Header = "Loading plugins...";

            var items = new List<object>();
            try
            {
                await PluginsCollectionManager.LoadPluginsAsync();

                // If no plugins were found.
                if (PluginsCollectionManager.Plugins.Count == 0)
                {
                    this.PluginsMenuItem.Header = "No plugins";
                    return;
                }

                items = PluginsCollectionManager.Plugins.Keys.Cast<object>().ToList();
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show(e.Message);
                this.PluginsMenuItem.Header = "No plugins";
            }
            catch (ConfigurationErrorsException e)
            {
                var message = e.Message + " Plugins are not going to be loaded. " +
                              "Check config file and try to reload plugins.";
                MessageBox.Show(message);
            }
            finally
            {
                items.Add(new Separator());
                MenuItem reloadPluginsMenuItem = GetReloadPluginsMenuItem();
                items.Add(reloadPluginsMenuItem);

                this.PluginsMenuItem.Header = "Plugins";
                this.PluginsMenuItem.Items.Clear();
                this.PluginsMenuItem.ItemsSource = items;
                this.PluginsMenuItem.IsEnabled = true;

                this.ReloadPluginsCommand.Gesture = GetKeyGesture("ReloadPlugins", reloadPluginsMenuItem, HotkeyCommands.ReloadPlugins);
            }
        }

        private MenuItem GetReloadPluginsMenuItem()
        {
            var newMenuItem = new MenuItem { Header = "Reload plugins" };
            newMenuItem.Click += ReloadPlugins_OnClick;
            return newMenuItem;
        }

        private void SetHotkeys()
        {
            this.SaveFileCommand.Gesture = GetKeyGesture("SaveFile", this.SaveFileMenuItem, HotkeyCommands.SaveFile);
            this.OpenFileCommand.Gesture = GetKeyGesture("OpenFile", this.OpenFileMenuItem, HotkeyCommands.OpenFile);
            this.ExitCommand.Gesture = GetKeyGesture("Exit", this.ExitMenuItem, HotkeyCommands.Exit);
        }

        private KeyGesture GetKeyGesture(string settingName, MenuItem menuItem, RoutedCommand routedCommand)
        {
            var keyGestureConverter = new KeyGestureConverter();
            var keyGesture = (KeyGesture)keyGestureConverter.ConvertFromString(ConfigurationManager.AppSettings[settingName]);
            menuItem.InputGestureText = HotkeyCommands.GetKeyGestureAsString(keyGesture);
            InputBindings.Add(new KeyBinding(routedCommand, keyGesture));

            return keyGesture;
        }

        private void ReloadPlugins_OnClick(object sender, RoutedEventArgs e)
        {
            if (!this.PluginsMenuItem.IsEnabled)
                return;

            LoadPlugins();
        }

        private void PluginsMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
                return;

            if (menuItem.Header.ToString() == "Reload plugins")
                return;

            var pluginName = menuItem.Header.ToString();
            IPluginable plugin = PluginsCollectionManager.Plugins[pluginName];
            plugin.Start();
            plugin.ShowFinalMessage();
        }

        protected override void OnActivated(EventArgs e)
        {
            LoadCollectionToListBox(CollectionManager.GetConstellationsSortedBy("Name"));
            this.SortTypeComboBox.Text = "Name";
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var nameStart = this.SearchByNameTextBox.Text;
            if (nameStart == String.Empty)
                LoadCollectionToListBox(CollectionManager.GetConstellationsSortedBy("Name"));

            var optionType = this.SearchTypeComboBox.Text;
            switch (optionType)
            {
                case "constellation":
                    LoadCollectionToListBox(CollectionManager.GetConstellationByNameStart(nameStart));
                    break;
                case "star":
                    LoadCollectionToListBox(CollectionManager.GetStarByNameStart(nameStart));
                    break;
                case "planet":
                    LoadCollectionToListBox(CollectionManager.GetPlanetByNameStart(nameStart));
                    break;
            }
        }

        private void SortTypeComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            if (!(sender is ComboBox comboBox))
                return;

            var optionSort = comboBox.Text;
            var typeList = this.SearchTypeComboBox.Text;

            if (typeList == "constellation")
            {
                LoadCollectionToListBox(CollectionManager.Constellations);
            }
            else if (typeList == "star") /// TODO. Now it's disabling combobox.
            {
                LoadCollectionToListBox(CollectionManager.GetAllStarsWithParallel());
            }

            LoadCollectionToListBox(CollectionManager.GetConstellationsSortedBy(optionSort));
        }

        private void RemoveAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (CollectionManager.IsCollectionEmpty())
            {
                MessageBox.Show("Collection is already empty!", "Oops", MessageBoxButton.OK);
                return;
            }

            var messageBoxResult = MessageBox.Show("Are you sure that you want to delete ALL constellations?",
                "Confirmation", MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.No)
                return;

            CollectionManager.ClearCollection();
            LoadCollectionToListBox(CollectionManager.Constellations);

            MessageBox.Show("All constellations were removed!", "Success", MessageBoxButton.OK);

            InfoStateController.InfoChanged();
        }

        private void LoadCollectionToListBox(IEnumerable collection)
        {
            ConstellationListBox.ItemsSource = collection;
        }

        private void SearchTypeComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            this.SearchByNameTextBox.Text = String.Empty;
            var option = (sender as ComboBox)?.Text;
            if (option == "constellation")
            {
                LoadCollectionToListBox(CollectionManager.GetConstellationsSortedBy("Name"));
                this.SortTypeComboBox.IsEnabled = true;
            }
            else if (option == "star")
            {
                LoadCollectionToListBox(CollectionManager.GetStarsGroupedByConstellation());
                this.SortTypeComboBox.IsEnabled = false;
            }
            else if (option == "planet")
            {
                LoadCollectionToListBox(CollectionManager.GetAllPlanets());
                this.SortTypeComboBox.IsEnabled = false;
            }
        }

        private void RemoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button senderButton))
                return;

            CollectionManager.Current = (int) senderButton.Tag;

            var result = MessageBox.Show("You sure you want to remove that element?\nYou cannot undo that later.",
                "Warning", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
                return;

            CollectionManager.RemoveCurrent();
            LoadCollectionToListBox(CollectionManager.GetConstellationsSortedBy("Name"));
            MessageBox.Show("Element was removed successfully!", "Success", MessageBoxButton.OK);
            InfoStateController.InfoChanged();
        }

        private void InfoButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button senderButton))
                return;

            CollectionManager.Current = (int) senderButton.Tag;
            ShowFullInfoWindow();
        }

        private void ShowFullInfoWindow()
        {
            WindowsManager.StoreWindow(this);
            this.Hide();
            var constellationInfoWindow = new ConstellationInfoWindow();
            constellationInfoWindow.Show();
        }

        private void AddConstellationButton_OnClick(object sender, RoutedEventArgs e)
        {
            WindowsManager.StoreWindow(this);
            this.Hide();
            var addConstellationWindow = new AddConstellationWindow();
            addConstellationWindow.Show();
        }

        private void SaveFile_OnClick(object sender, RoutedEventArgs e)
        {
            SaveToFile();
        }

        private void LoadFile_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = "Text (*.txt)|*.txt"
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            var fullPathToFile = openFileDialog.FileName;
            CollectionManager.LoadFromFile(fullPathToFile);

            LoadCollectionToListBox(CollectionManager.GetConstellationsSortedBy("Name"));

            MessageBox.Show("Loaded!");
            InfoStateController.Reset();
        }

        /// <summary>
        /// Saves collection of constellations to file.
        /// Returns true if info was saved to the file.
        /// False otherwise.
        /// </summary>
        private static bool SaveToFile()
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Filter = "Text (*.txt)|*.txt"
            };

            if (saveFileDialog.ShowDialog() != true)
                return false;

            var fullPathToFile = saveFileDialog.FileName;
            CollectionManager.SaveToFile(fullPathToFile);

            MessageBox.Show("Saved!");
            InfoStateController.Reset();
            return true;
        }

        private void PageViewButton_OnClick(object sender, RoutedEventArgs e)
        {
            WindowsManager.StoreWindow(this);
            this.Hide();
            var constellationList = CollectionManager.Constellations.ToList();
            var pageManager = new PagesManager<ConstellationViewPage>(constellationList);
            var pageViewWindow = new PageViewWindow(pageManager);
            pageViewWindow.Show();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            if (!InfoStateController.InfoWasChanged)
            {
                this.Close();
                return;
            }

            var result = MessageBox.Show(
                "Data was changed! Do you want to save changes?\nAny unsaved data will be lost otherwise!",
                "Warning!", MessageBoxButton.YesNoCancel);

            switch (result)
            {
                // Exit without save.
                case MessageBoxResult.No:
                {
                    this.Close();
                    break;
                }

                // Return back to program.
                case MessageBoxResult.Cancel:
                {
                    break;
                }

                // If was save was made, exit.
                // Back to program otherwise.
                case MessageBoxResult.Yes:
                {
                    if (SaveToFile())
                        this.Close();

                    break;
                }
            }
        }
    }
}
