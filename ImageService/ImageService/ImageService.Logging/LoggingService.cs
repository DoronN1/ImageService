
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {

        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public void Log(string message, MessageTypeEnum type)
        {
            // MessageRecieved.Invoke(message,type);
            MessageRecieved.Invoke(this, new MessageRecievedEventArgs() { Message = message, Status = type });
        }
    }
}

