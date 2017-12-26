namespace TieFighter.Services
{
    public interface IStringResourceService
    {
        bool TrySetConfigFileLocation(string fullPathToFile);
        bool IsInitialized { get; }
        string this[string id] { get;set; }
    }
}
