namespace StarCatalog
{
    public interface IPluginable
    {
        string Name { get; }
        void Start();
        void ShowFinalMessage();
    }
}