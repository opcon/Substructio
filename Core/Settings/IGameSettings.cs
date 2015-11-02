namespace Substructio.Core.Settings
{
    public interface IGameSettings
    {
        object this[string key] { get; set; }
        void Save();
        void Load();
    }
}
