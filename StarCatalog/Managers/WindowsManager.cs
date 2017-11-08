using System.Collections.Generic;
using System.Windows;

namespace StarCatalog
{
    public static class WindowsManager
    {
        private static readonly Stack<Window> Windows = new Stack<Window>();

        public static void StoreWindow(Window window) => Windows.Push(window);

        public static Window GetLastWindow() => Windows.Pop();
    }
}
