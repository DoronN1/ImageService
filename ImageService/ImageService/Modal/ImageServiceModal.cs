using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing.Imaging;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private string m_TumbnailFolder;        // The Output Folder/Tumbnails
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size
        #endregion
        /**************************************************************************/
        public ImageServiceModal()
        {
            m_thumbnailSize = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
            m_OutputFolder = ConfigurationManager.AppSettings["OutputDir"];
            m_TumbnailFolder = System.IO.Path.Combine(m_OutputFolder, "Tumbnails");
            // creates the tumbnail folder on creation.
            bool result = false;
            createFolder(m_TumbnailFolder, out result);
        }
        /**************************************************************************/
        public string AddFile(string srcPath,string fileName ,out bool result)
        {
            result = false;
            bool tumbResult = false;
            string srcFilePath = System.IO.Path.Combine(srcPath, fileName);
            string dirYearPath = System.IO.Path.Combine(m_OutputFolder, GetDateTakenFromImage(srcFilePath).Year.ToString());
            string dirMonthPath = System.IO.Path.Combine(dirYearPath, GetDateTakenFromImage(srcFilePath).Month.ToString());
            string dirTumbYearPath = System.IO.Path.Combine(m_TumbnailFolder, GetDateTakenFromImage(srcFilePath).Year.ToString());
            string dirTumbMonthPath = System.IO.Path.Combine(dirTumbYearPath, GetDateTakenFromImage(srcFilePath).Month.ToString());
            string newFilePath = System.IO.Path.Combine(dirMonthPath, fileName);
            string newTumbFilePath = System.IO.Path.Combine(dirTumbMonthPath, fileName);
            // creates the year folder and Tumbnail/year folder
                createFolder(dirYearPath, out result);
                createFolder(dirTumbYearPath, out tumbResult);
                if ((result == false)||(tumbResult==false))
                {
                                return "Error: create year Folder has failed.";
                }
            // creates the month folder and Tumbnail/year/month folder
            createFolder(dirYearPath, out result);
                createFolder(dirTumbMonthPath, out tumbResult);
                if ((result == false)|| (tumbResult == false))
                {
                    return "Error: create month Folder has failed.";
                }
            // copy the image into OutPutDir/Tumbnail/year/month and resize into Tumbnail
            copyFile(srcFilePath, newTumbFilePath, out tumbResult);
            createTumbnail(newTumbFilePath);
            // move the image into OutPutDir/year/month
            moveFile(srcFilePath, newFilePath, out result);
            if ((result == false)|| (tumbResult == false))
            {
                return "Error: moving file has failed.";
            }
            result = true;
            return  dirMonthPath;
        }
        /**************************************************************************/
        public void createFolder(string folderPath, out bool result)
        {
            result = false;
            if (!Directory.Exists(folderPath))
            {
                // Create the subfolder.
                try
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                    result = true;
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        /**************************************************************************/
        public void moveFile(string SourcePath, string DestPath, out bool result)
        {
            // move a file to a new location:
            result = false;
            if (System.IO.File.Exists(SourcePath))
            {
                try
                {
                    System.IO.File.Move(SourcePath, DestPath);
                    result = true;
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

        }
        /**************************************************************************/
        public void copyFile(string sourceFile, string destFile, out bool result)
        {
            result = false;
            // To copy a file to another location and 
            // overwrite the destination file if it already exists.
            if (System.IO.File.Exists(sourceFile))
            {
                try
                {
                    System.IO.File.Copy(sourceFile, destFile, true);
                    result = true;
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

        }
        /**************************************************************************/
        private static Regex r = new Regex(":");

        //retrieves the datetime WITHOUT loading the whole image
        public static DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }

        /**************************************************************************/
        public void copyFolder(string sourceFolder, string destFolder, out bool result)
        {
            result = false;
            // To copy a folder's contents to a new location:
            if (Directory.Exists(sourceFolder))
            {
                // Create a new target folder, if necessary.
                if (!Directory.Exists(destFolder))
                {
                    createFolder(destFolder, out  result);
                }
                    // Create all of the directories
                    foreach (string dirPath in Directory.GetDirectories(sourceFolder, "*",
                                SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(dirPath.Replace(sourceFolder, destFolder));
                    }
                    //Copy all the files & Replaces any files with the same name
                    foreach (string newPath in Directory.GetFiles(sourceFolder, "*.*",
                        SearchOption.AllDirectories))
                    {
                        File.Copy(newPath, newPath.Replace(sourceFolder, destFolder), true);
                    }
                result = true;
            }
        }
        /**************************************************************************/


        public void moveFolder(string SourcePath, string DestPath, out bool result)
        {
            // To move a file or folder to a new location:
            result = false;
            if (System.IO.File.Exists(SourcePath))
            {
                try
                {
                    System.IO.File.Move(SourcePath,  DestPath);
                    result = true;
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        /**************************************************************************/

        public void deleteFile(string SourcePath, out bool result)
        {
            // Delete a file by using File class static method...
            result = false;
            if (System.IO.File.Exists(SourcePath))
            {
                try
                {
                    System.IO.File.Delete(SourcePath);
                    result = true;
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        /**************************************************************************/
        public void deleteFolder(string SourcePath,out bool result)
        {
            result = false;
            // Delete a directory. Must be writable or empty.
            if (System.IO.File.Exists(SourcePath))
            {
                try
                {
                    //delete all the files in folder
                    foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                        SearchOption.AllDirectories))
                    {
                        System.IO.File.Delete(newPath);
                                      }
                    System.IO.Directory.Delete(SourcePath);
                    result = true;
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        /**************************************************************************/
        public void createTumbnail(string imagePath)
        {
            Image image = Image.FromFile(imagePath);
            Image thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
            thumb.Save(Path.ChangeExtension(imagePath, "thumb"));
        }
    }
}