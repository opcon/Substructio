using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Substructio.Logging;
using Substructio.Core.Settings;

namespace Substructio.Core
{
    public static class ServiceLocator
    {
        public static IErrorReporting ErrorReporting { get; set; } = new NullErrorReporting();
        public static IAnalytics Analytics { get; set; } = new NullAnalytics();
        public static IGameSettings Settings { get; set; } = new NullGameSettings();
        public static IDirectoryHandler Directories { get; set; } = new NullDirectoryHandler();
    }
}
