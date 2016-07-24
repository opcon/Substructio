using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Substructio.Logging
{
    public interface IErrorReporting
    {
        void ReportError(Exception e);
        void ReportMessage(string message);
    }

    public class NullErrorReporting : IErrorReporting
    {
        public void ReportError(Exception e) { }

        public void ReportMessage(string message) { }
    }
}
