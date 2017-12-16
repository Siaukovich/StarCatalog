using System.Configuration;

namespace StarCatalog
{
    public class HotkeyElement : ConfigurationElement
    {
        [ConfigurationProperty("CommandName")]
        public string CommandName
        {
            get => (string) base["CommandName"];
            set => base["CommandName"] = value;
        }

        [ConfigurationProperty("Gesture")]
        public string Gesture
        {
            get => (string) base["Gesture"];
            set => base["Gesture"] = value;
        }
    }
}
