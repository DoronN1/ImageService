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
        Dictionary<int, string> dict;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved; // The event that notifies about a new Command being recieved
        #endregion


        /*********************************************************************/

        public void onCloseServer(object sender, ... )
        {
            IDirectoryHandler h = sender;
            CommandRecieved -= h.OnCommandRecieved;
            CommandRecieved -= h.OnCloseServer;
        }

        /*********************************************************************/

        public void sendCommand()
        {
            CommandRecieved("*", CloseHandler());
        }
        /*********************************************************************/

        public void createHandler(string directory)
        {
            IDirectoryHandler h = new DirectoryHandler(directory);
            CommandRecieved += h.OnCommandRecieved;
            h.OnCloseServer += onCloseServer;

        }

    }
}
