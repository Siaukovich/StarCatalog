namespace StarCatalog.Helpers
{
    public static class InfoStateController
    {
        public static bool InfoWasChanged { get; private set; } = false;
        public static void InfoChanged() => InfoWasChanged = true;
        public static void Reset() => InfoWasChanged = false;
    }
}
