
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ServiceProcess;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        private EventLog eventLog;
        private int eventId = 0;
        public LoggingService(EventLog log)
        {
            this.eventLog = log;
        }
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public void Log(string message, MessageTypeEnum type)
        {
            eventLog.WriteEntry("checkkkkk", EventLogEntryType.Information, eventId++);
            // MessageRecieved.Invoke(message,type);
            MessageRecieved?.Invoke(this, new MessageRecievedEventArgs() { Message = message, Status = type });
         //   eventLog.WriteEntry(message, EventLogEntryType.Information, eventId++);
        }
    }
}

