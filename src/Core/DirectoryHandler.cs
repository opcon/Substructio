using System.Collections.Generic;
using System.IO;

namespace Substructio.Core
{
    public class DirectoryHandler
    {
        private Dictionary<string, DirectoryInfo> DirectoryList;

        public DirectoryHandler()
        {
            DirectoryList = new Dictionary<string, DirectoryInfo>();
        }

        public DirectoryInfo this[string key]
        {
            get
            {
                return DirectoryList.ContainsKey(key) ? DirectoryList[key] : null;
            }
        }

        public void AddPath(string name, string path)
        {
            if (DirectoryList.ContainsKey(name)) return;
            Utilities.FixPathSeparators(ref path);
            DirectoryList.Add(name, new DirectoryInfo(path));
        }

    }
}
