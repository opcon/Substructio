using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Substructio.Core
{
    public enum Distribution
    {
        Steam,
        Squirrel,
        Itch,
    }

    public static class DistributionMethod
    {
        public static Distribution GetDistributionMethod()
        {
            if (File.Exists(ServiceLocator.Directories.Locate("Application", Path.Combine("..", "Update.exe"))))
                return Distribution.Squirrel;
            else return Distribution.Itch;
        }
    }
}
