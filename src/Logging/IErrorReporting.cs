using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Substructio.Logging
{
    public interface IErrorReporting
    {
        string ReportError(Exception e);
        string ReportMessage(string message);
        Task<string> ReportErrorAsync(Exception e);
        Task<string> ReportMessageAsync(string message);
    }

    public class NullErrorReporting : IErrorReporting
    {
        public string ReportError(Exception e) { return ""; }

        public string ReportMessage(string message) { return ""; }

        public Task<string> ReportErrorAsync(Exception e) { return new Task<string>(() => ""); }

        public Task<string> ReportMessageAsync(string message) { return new Task<string>(() => ""); }
    }
}
