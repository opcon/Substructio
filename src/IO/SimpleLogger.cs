using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splat;
using System.Reflection;
using System.IO;

namespace Substructio.IO
{
    public class SimpleLogger : ILogger
    {
        public LogLevel Level { get; set; }
        public string LogFileDirectory {get; private set;}
        private string _logFileName;
        private object _lock = new object();

        public SimpleLogger(string logFileDirectory, string logFileName)
        {
            LogFileDirectory = logFileDirectory;
            _logFileName = logFileName;
        }

        public void Write([Localizable(false)] string message, LogLevel logLevel)
        {
            if ((int)logLevel < (int)Level) return;
            lock (_lock)
            {
                File.AppendAllLines(Path.Combine(LogFileDirectory, _logFileName), 
                    new[] { string.Format("{0}: {1} - {2}", logLevel.ToString(), DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss"), message) });
            }
        }
    }
}
