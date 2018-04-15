using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController controller;
        private ILoggingService logging;
        private Dictionary<int, IDirectoryHandler> handlers;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved; // The event that notifies about a new Command being recieved
        #endregion

        public ImageServer(IImageController controller1, ILoggingService logging1,List<string> handlersPath)
        {
            controller = controller1;
            logging = logging1;
            handlers = new Dictionary<int, IDirectoryHandler> { };
            // creates the handlers dictionary
            int i = 0;
        foreach (string s in handlersPath)
            {
                createHandler(s,i);
                i++;
            }
        }
        /*********************************************************************/

        public void onCloseServer(object sender, DirectoryCloseEventArgs e)
        {
            IDirectoryHandler h = sender as IDirectoryHandler;
            CommandRecieved -= h.OnCommandRecieved;
        }

        /*********************************************************************/

        public void sendCommand()
        {
            CommandRecievedEventArgs Com = new CommandRecievedEventArgs(1, null, "*");
            CommandRecieved?.Invoke(this, Com);
        }
        /*********************************************************************/

        public void createHandler(string directory,int handler_num)
        {
            IDirectoryHandler h = new DirectoyHandler(directory,controller,logging);
            handlers.Add(handler_num, h);
            CommandRecieved += h.OnCommandRecieved;
            h.DirectoryClose += onCloseServer; 
        }
    }
}
