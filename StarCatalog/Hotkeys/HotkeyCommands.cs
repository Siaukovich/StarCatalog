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

        public static RoutedCommand GetCommand(string commandName)
        {
            switch (commandName)
            {
                case nameof(SaveFile): return SaveFile;
                case nameof(OpenFile): return OpenFile;
                case nameof(ReloadPlugins): return ReloadPlugins;
                case nameof(Exit): return Exit;

                default: throw new ConfigurationErrorsException();
            }
        }

        public static KeyGesture GetKeyGesture(string gesture)
        {
            string[] parts = gesture.Split('+');

            if (!IsPartsValid(parts))
                throw new ConfigurationErrorsException();

            Key key;
            if (parts.Length == 1)
            {
                key = GetKey(parts[0][0]);

                return new KeyGesture(key);
            }

            ModifierKeys modifierKey = GetModifierKey(parts[0]);
            key = GetKey(parts[1][0]);

            return new KeyGesture(key, modifierKey);
        }

        private static bool IsPartsValid(string[] parts)
        {
            switch (parts.Length)
            {
                case 1: return parts[0].Length == 1 && Char.IsUpper(parts[0][0]);
                case 2: return parts[1].Length == 1 && Char.IsUpper(parts[1][0]);
            }

            return false;
        }

        private static Key GetKey(char key)
        {
            const int differenceBetweenKeyAndUnicode = 21;
            return (Key)(key - differenceBetweenKeyAndUnicode);
        }

        private static ModifierKeys GetModifierKey(string modifierKey)
        {
            switch (modifierKey)
            {
                case "CTRL": return ModifierKeys.Control;
                case "ALT": return ModifierKeys.Alt;
            }

            throw new ConfigurationErrorsException();
        }
    }
}