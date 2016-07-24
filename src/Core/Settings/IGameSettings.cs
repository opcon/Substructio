using System;

namespace Substructio.Core.Settings
{
    public interface IGameSettings
    {
        object this[string key] { get; set; }
        void Save();
        void Load();
    }

    public class NullGameSettings : IGameSettings
    {
        public object this[string key]
        {
            get
            {
                return null;
            }

            set
            {
            }
        }

        public void Load()
        {
        }

        public void Save()
        {
        }
    }
}
