using System.Configuration;

namespace StarCatalog
{
    public class HotkeySection : ConfigurationSection
    {
        [ConfigurationProperty("Hotkeys")]
        public HotkeysCollection Hotkeys
        {
            get => (HotkeysCollection) base["Hotkeys"];
            set => base["Hotkeys"] = value;
        }
    }
}