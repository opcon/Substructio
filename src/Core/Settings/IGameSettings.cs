using System;
using System.Collections.Generic;

namespace Substructio.Core.Settings
{
    public interface IGameSettings
    {
        object this[string key] { get; set; }
        void Save();
        void Load();
        Dictionary<string, object> GetAllSettings();
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

        public Dictionary<string, object> GetAllSettings()
        {
            return null;
        }
    }
}
