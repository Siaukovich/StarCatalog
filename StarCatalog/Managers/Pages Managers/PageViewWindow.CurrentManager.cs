namespace StarCatalog.Windows
{
    public partial class PageViewWindow
    {
        private static class CurrentPageManager
        {
            public static AsyncPageManager Instance { get; set; }
        }
    }
}
