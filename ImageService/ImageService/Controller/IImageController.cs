using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public interface IImageController
    {
        void ExecuteCommand(CommandEnum commandID, string[] args, out bool result);          // Executing the Command Requset
    }
}
