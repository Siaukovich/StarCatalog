using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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
            SetMenuIcons();
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
                string message = e.Message + " Plugins are not going to be loaded. " +
                              "Check config file and reload program.";
                MessageBox.Show(message);
            }
            finally
            {
                items.Add(new Separator());

                MenuItem reloadPluginsMenuItem = GetReloadPluginsMenuItem();
                items.Add(reloadPluginsMenuItem);

                this.PluginsMenuItem.Header = "Plugins";

                this.PluginsMenuItem.ItemsSource = null;
                this.PluginsMenuItem.Items.Clear();

                this.PluginsMenuItem.ItemsSource = items;
                this.PluginsMenuItem.IsEnabled = true;

                SetKeyGesture(this.ReloadPluginsCommand, "ReloadPlugins", reloadPluginsMenuItem, HotkeyCommandsManager.ReloadPlugins);
                SetIcon(reloadPluginsMenuItem, "ReloadPluginsIcon");
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
            SetKeyGesture(this.SaveFileCommand, "SaveFile", this.SaveFileMenuItem, HotkeyCommandsManager.SaveFile);
            SetKeyGesture(this.OpenFileCommand, "OpenFile", this.OpenFileMenuItem, HotkeyCommandsManager.OpenFile);
            SetKeyGesture(this.ExitCommand, "Exit", this.ExitMenuItem, HotkeyCommandsManager.Exit);
            SetKeyGesture(this.PageViewCommand, "PageView", this.NewConstellationMenuItem, HotkeyCommandsManager.PageView);
        }

        private void SetKeyGesture(KeyBinding keyBinding, string settingName, MenuItem menuItem, ICommand routedCommand)
        {
            KeyGesture keyGesture;
            bool elementWithSameGesturesAlreadyExists = false;
            bool validShortcut;

            if (ConfigurationManager.AppSettings.AllKeys.Contains(settingName))
            {
                validShortcut = TryConvertGestureFromString(settingName, out keyGesture);

                if (keyGesture != null)
                {
                    elementWithSameGesturesAlreadyExists = HotkeyCommandsManager.KeyGestures.Any
                        (kg => kg.Modifiers == keyGesture.Modifiers && kg.Key == keyGesture.Key);
                }
            }
            else
            {
                return;
            }

            if (!validShortcut)  // If not valid shortcut in config file, use default shortcut defined in xaml.
            {
                keyGesture = (KeyGesture)keyBinding.Gesture;
                menuItem.InputGestureText = HotkeyCommandsManager.GetKeyGestureAsString(keyGesture);
            }

            if (!elementWithSameGesturesAlreadyExists)
            {
                menuItem.InputGestureText = HotkeyCommandsManager.GetKeyGestureAsString(keyGesture);
                keyBinding.Gesture = keyGesture;
            }
            else // If shortcut not found in config file, use default shortcut defined in xaml.
            {
                keyGesture = (KeyGesture)keyBinding.Gesture;
                menuItem.InputGestureText = HotkeyCommandsManager.GetKeyGestureAsString(keyGesture);
            }
            
            InputBindings.Add(new KeyBinding(routedCommand, keyGesture));
            HotkeyCommandsManager.KeyGestures.Add(keyGesture);
        }

        private static bool TryConvertGestureFromString(string settingName, out KeyGesture keyGesture)
        {
            try
            {
                var keyGestureConverter = new KeyGestureConverter();
                keyGesture = (KeyGesture)keyGestureConverter.ConvertFromString(ConfigurationManager.AppSettings[settingName]);

                // Shift is ambigious and Win used for OS commands.
                bool modifierIsWinOrShift = keyGesture.Modifiers == ModifierKeys.Windows ||
                                            keyGesture.Modifiers == ModifierKeys.Shift;

                if (modifierIsWinOrShift)
                {
                    keyGesture = null;
                    return false;
                }

                return true;
            }
            catch (Exception e) when (e is ArgumentException || e is NotSupportedException)
            {
                // ArgumentException when not valid keys in shortcut (like "Ctrrr").
                // NotSupportedException when shortcut is ambigious (like "Shift+A").
                keyGesture = null;
                return false;
            }
        }

        private void SetMenuIcons()
        {
            SetIcon(this.SaveFileMenuItem, "SaveFileIcon");
            SetIcon(this.OpenFileMenuItem, "OpenFileIcon");
            SetIcon(this.ExitMenuItem, "ExitIcon");
        }

        private void SetIcon(MenuItem menuItem, string settingName)
        {
            string IconPathSetting = "IconPath";
            if (!ConfigurationManager.AppSettings.AllKeys.Contains(IconPathSetting))
                return;

            if (!ConfigurationManager.AppSettings.AllKeys.Contains(settingName))
                return;

            string path = ConfigurationManager.AppSettings[IconPathSetting];
            string fileName = ConfigurationManager.AppSettings[settingName];
            string fullPath = Path.Combine(Environment.CurrentDirectory ,path, fileName);

            if (!File.Exists(fullPath))
                return;

            var uri = new Uri(fullPath, UriKind.Absolute);
            var image = new BitmapImage(uri);

            menuItem.Icon = new Image {Source = image, MaxHeight = 20, MaxWidth = 20 };
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

        private void PageView_OnClick(object sender, RoutedEventArgs e)
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
