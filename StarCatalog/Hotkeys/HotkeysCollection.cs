using System.Configuration;

namespace StarCatalog
{
    [ConfigurationCollection(typeof(HotkeyElement), AddItemName = "Hotkey")]
    public class HotkeysCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new HotkeyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HotkeyElement) element).CommandName;
        }
    }
}