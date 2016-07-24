using System;
using System.IO;

namespace Substructio.Core
{
    public interface IDirectoryHandler
    {
        DirectoryInfo this[string key] { get; }

        void AddPath(string name, string path);
        string Locate(string key, string path);
    }

    public class NullDirectoryHandler : IDirectoryHandler
    {
        public DirectoryInfo this[string key]
        {
            get
            {
                return null;
            }
        }

        public void AddPath(string name, string path)
        {
        }

        public string Locate(string key, string path)
        {
            return "";
        }
    }
}