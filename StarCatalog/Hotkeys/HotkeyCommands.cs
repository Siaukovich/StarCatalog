using System;
using System.Configuration;
using System.Windows.Input;

namespace StarCatalog
{
    public static class HotkeyCommands
    {
        public static RoutedCommand SaveFile { get; set; } = new RoutedCommand(nameof(SaveFile), typeof(MainWindow));
        public static RoutedCommand OpenFile { get; set; } = new RoutedCommand(nameof(OpenFile), typeof(MainWindow));
        public static RoutedCommand ReloadPlugins { get; set; } = new RoutedCommand(nameof(ReloadPlugins), typeof(MainWindow));
        public static RoutedCommand Exit { get; set; } = new RoutedCommand(nameof(Exit), typeof(MainWindow));

        public static string GetKeyGestureAsString(KeyGesture gesture)
        {
            var modifiersString = String.Empty;

            switch (gesture.Modifiers)
            {
                case ModifierKeys.Control:
                    modifiersString = "Ctrl";
                    break;

                case ModifierKeys.Alt:
                    modifiersString = "Alt";
                    break;
            }

            return modifiersString + "+" + gesture.Key;
        }
    }
}