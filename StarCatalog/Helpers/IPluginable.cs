namespace StarCatalog
{
    public interface IPluginable
    {
        string Name { get; }
        void Start(MainWindow mainWindow);
    }
}