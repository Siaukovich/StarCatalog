namespace StarCatalog.Windows
{
    public partial class PageViewWindow
    {
        private static class CurrentManager
        {
            public static IAsyncPageManager Instance { get; set; }
        }
    }
}
