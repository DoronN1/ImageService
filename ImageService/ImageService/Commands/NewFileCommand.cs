﻿using ImageService.Infrastructure;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;

        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }

        public string Execute(string[] args, out bool result)
        {
            // The String Will Return the New Path if result = true else will return the error message 
            result = false;
            // args[0] = file path , args[1] = file name
            string newPath = m_modal.AddFile(args[0], args[1], out result);
            if (result == false)
            {
                return "Error: executing new file command has failed";
            }
            return newPath;
        }
    }
}
;