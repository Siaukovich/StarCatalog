using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Documents;
using System.Windows.Input;

namespace StarCatalog
{
    public static class HotkeyCommands
    {
        public static RoutedCommand PageView { get; set; } = new RoutedCommand(nameof(PageView), typeof(MainWindow));
        public static RoutedCommand SaveFile { get; set; } = new RoutedCommand(nameof(SaveFile), typeof(MainWindow));
        public static RoutedCommand OpenFile { get; set; } = new RoutedCommand(nameof(OpenFile), typeof(MainWindow));
        public static RoutedCommand ReloadPlugins { get; set; } = new RoutedCommand(nameof(ReloadPlugins), typeof(MainWindow));
        public static RoutedCommand Exit { get; set; } = new RoutedCommand(nameof(Exit), typeof(MainWindow));

        public static List<KeyGesture> KeyGestures = new List<KeyGesture>();

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