using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Substructio.Logging
{
    public interface IAnalytics
    {
        void TrackApplicationStartup();
        void TrackApplicationShutdown();
        void TrackEvent(string eventCategory, string eventAction, string eventSubjectName = "", string eventValue = "");
        void TrackApplicationView(string relativeURL, string title = "");
        void SetCustomVariable(int variableID, string variableName, string variableValue, CustomVariableScope variableScope);
    }

    public class NullAnalytics : IAnalytics
    {
        public void SetCustomVariable(int variableID, string variableName, string variableValue, CustomVariableScope variableScope) { }

        public void TrackApplicationShutdown() { }

        public void TrackApplicationStartup() { }

        public void TrackApplicationView(string relativeURL, string title = "") { }

        public void TrackEvent(string eventCategory, string eventAction, string eventSubjectName = "", string eventValue = "") { }
    }

    public enum CustomVariableScope
    {
        ApplicationLaunch,
        ApplicationView
    }
}
