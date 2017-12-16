namespace StarCatalog
{
    public partial class PageViewWindow
    {
        private static class CurrentPageManager
        {
            public static IPageManager Instance { get; set; }
        }
    }
}
