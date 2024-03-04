using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace CPT_275_Capstone.App_Start
{
    public class EventViewerLogger
    {
        private string eventSource;
        public EventViewerLogger(string eventSource)
        {
            this.eventSource = eventSource;

            // Check if the event source exists and create it if it doesn't
            if (!EventLog.SourceExists(eventSource))
            {
                EventLog.CreateEventSource(eventSource, "SCCFleetServices");
            }
        }

        public void LogError(string errorMessage)
        {
            EventLog.WriteEntry(eventSource, errorMessage, EventLogEntryType.Error);
        }
    }
}