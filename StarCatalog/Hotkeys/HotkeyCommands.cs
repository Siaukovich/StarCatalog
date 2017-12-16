using System;
using System.Windows.Input;

namespace StarCatalog
{
    public static class HotkeyCommands
    {
        public static RoutedCommand SaveFile { get; set; } = new RoutedCommand(nameof(SaveFile), typeof(MainWindow));
        public static RoutedCommand OpenFile { get; set; } = new RoutedCommand(nameof(OpenFile), typeof(MainWindow));
        public static RoutedCommand ReloadPlugins { get; set; } = new RoutedCommand(nameof(ReloadPlugins), typeof(MainWindow));

        public static RoutedCommand CloseWindow { get; set; } = new RoutedCommand(nameof(CloseWindow), typeof(MainWindow));

        public static RoutedCommand GetCommand(string commandName)
        {
            switch (commandName)
            {
                case nameof(SaveFile): return SaveFile;
                case nameof(OpenFile): return OpenFile;
                case nameof(ReloadPlugins): return ReloadPlugins;
                case nameof(CloseWindow): return CloseWindow;

                default: throw new ArgumentException();
            }
        }

        public static KeyGesture GetKeyGesture(string gesture)
        {
            switch (gesture)
            {
                case "CTRL+S": return new KeyGesture(Key.S, ModifierKeys.Control);
                case "CTRL+O": return new KeyGesture(Key.O, ModifierKeys.Control);
                case "CTRL+R": return new KeyGesture(Key.R, ModifierKeys.Control);
                case "ESC": return new KeyGesture(Key.Escape);

                default: throw new ArgumentException();
            }
        }
    }
}