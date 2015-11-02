using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;

namespace Substructio.IO
{
    public class CrashReporter
    {
        public string CrashReportDirectory
        {
            get;
            private set;
        }

        public CrashReporter(string reportDirectory)
        {
            if (!Directory.Exists(reportDirectory)) Directory.CreateDirectory(reportDirectory);
            CrashReportDirectory = reportDirectory;
        }

        public void LogError(Exception exception)
        {
            try
            {
                Assembly caller = Assembly.GetEntryAssembly();
                Process thisProcess = Process.GetCurrentProcess();

                string errorFile = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss") + ".txt";

                using (StreamWriter sw = new StreamWriter(Path.Combine(CrashReportDirectory, errorFile)))
                {
                    sw.WriteLine("==============================================================================");
                    sw.WriteLine(caller.FullName);
                    sw.WriteLine("------------------------------------------------------------------------------");
                    sw.WriteLine("Application Information");
                    sw.WriteLine("------------------------------------------------------------------------------");
                    sw.WriteLine("Program      : " + caller.Location);
                    sw.WriteLine("Time         : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    sw.WriteLine("User         : " + Environment.UserName);
                    sw.WriteLine("Computer     : " + Environment.MachineName);
                    sw.WriteLine("OS           : " + Environment.OSVersion.ToString());
                    sw.WriteLine("Culture      : " + CultureInfo.CurrentCulture.Name);
                    sw.WriteLine("Processors   : " + Environment.ProcessorCount);
                    sw.WriteLine("Working Set  : " + Environment.WorkingSet);
                    sw.WriteLine("Framework    : " + Environment.Version);
                    sw.WriteLine("Run Time     : " + (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString());
                    sw.WriteLine("------------------------------------------------------------------------------");
                    sw.WriteLine("Exception Information");
                    sw.WriteLine("------------------------------------------------------------------------------");
                    sw.WriteLine("Source       : " + exception.Source.ToString().Trim());
                    sw.WriteLine("Method       : " + exception.TargetSite.Name.ToString());
                    sw.WriteLine("Type         : " + exception.GetType().ToString());
                    sw.WriteLine("Error        : " + GetExceptionStack(exception));
                    sw.WriteLine("Stack Trace  : " + exception.StackTrace.ToString().Trim());
                    sw.WriteLine("------------------------------------------------------------------------------");
                    sw.WriteLine("Loaded Modules");
                    sw.WriteLine("------------------------------------------------------------------------------");
                    foreach (ProcessModule module in thisProcess.Modules)
                    {
                        try
                        {
                            sw.WriteLine(module.FileName + " | " + module.FileVersionInfo.FileVersion + " | " + module.ModuleMemorySize);
                        }
                        catch (FileNotFoundException)
                        {
                            sw.WriteLine("File Not Found: " + module.ToString());
                        }
                        catch (Exception)
                        {

                        }
                    }
                    sw.WriteLine("------------------------------------------------------------------------------");
                    sw.WriteLine(errorFile);
                    sw.WriteLine("==============================================================================");
                }
            }
            catch (Exception)
            {
                //our error reporting crashed, so suppress exception to prevent infinte recursion
            }
        }

        private string GetExceptionStack(Exception e)
        {
            StringBuilder message = new StringBuilder();
            message.Append(e.Message);
            while (e.InnerException != null)
            {
                e = e.InnerException;
                message.Append(Environment.NewLine);
                message.Append(e.Message);
            }

            return message.ToString();
        }
    }
}
