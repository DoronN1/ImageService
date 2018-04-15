
using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Drawing;
using System.Collections;
using System.Drawing.Printing;

namespace ImageService.Controller.Handlers

{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        public ImageServiceModal ism;
        FileSystemWatcher watcher = new FileSystemWatcher();

        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose; // The Event That Notifies that the Directory is being closed

        public DirectoyHandler(string directory)
        {
            this.m_path = directory;
        }
        /*********************************************************************/

        public void StartHandleDirectory(string dirPath)
        {
            //watcher.BeginInit();
            watcher.Filter = "*.jpg";
            watcher.Filter = "*.png";
            watcher.Filter = "*.gif";
            watcher.Filter = "*.bmp";
            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            watcher.NotifyFilter = NotifyFilters.FileName;

            // Begin watching.
            watcher.EnableRaisingEvents = true;


        }

        /*********************************************************************/
        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
        }
        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }

        public void OnCloseServer()
        {

        }

        /*********************************************************************/
        //check if command is meant for its directory, if yes – handle command(for now will just be to close handler)

        public CommandRecievedEventArgs onCommandReceived(object sender, CommandRecievedEventArgs e)
        {
            if (m_dirWatcher.Path.CompareTo(e.RequestDirPath) == 0)
            {
                closeHandler(e.RequestDirPath);
            }
            return e;
        }

        /*********************************************************************/

        public void closeHandler(string path)
        {
            //– close FileSystemWatcher and invoke onClose event
            m_dirWatcher.EnableRaisingEvents = false;
            DirectoryClose.Invoke(this, new DirectoryCloseEventArgs(path, "Directory Closed"));
        }



        /*********************************************************************/
        public void createDateFolder(RenamedEventArgs e)
        {
            string result;

            string pathOut = ConfigurationManager.AppSettings["OutputDir"];
            //add image
            string pathOrigin = ism.AddFile(pathOut + e.OldFullPath, out result);

            //notify logger
            m_logging.Log("created outputDir directory", MessageTypeEnum.INFO);

            /*for thumbnails directory*/
            ism.createFolder(pathOut, "Tumbnails", out result);
            string path = ism.AddFile(pathOut + "/Tumbnails", out result);

            //notify logger
            m_logging.Log("created thumbnail directory", MessageTypeEnum.INFO);
        }

    }
}
