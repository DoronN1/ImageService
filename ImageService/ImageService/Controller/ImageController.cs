using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModal m_modal;                      // The Modal Object
        private Dictionary<CommandEnum, ICommand> commands;

        public ImageController(IImageServiceModal modal)
        {
            m_modal = modal;                    // Storing the Modal Of The System
            commands = new Dictionary<CommandEnum, ICommand>()
            {
				// For Now will contain NEW_FILE_COMMAND
                 { CommandEnum.NewFileCommand,new NewFileCommand(modal) }
            };
        }
        public void ExecuteCommand(CommandEnum commandID, string[] args, out bool result)
        {
            result = false;
            ICommand cmd;
            if(commands.TryGetValue(commandID,out cmd))
            {
                cmd.Execute(args,out result);
                result = true;
            }
     }
    }
}
;