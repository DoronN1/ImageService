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
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size
        #endregion

        public ImageServiceModal()
        {
            m_thumbnailSize = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
        }

        public string AddFile(string path, out bool result)
        {
            string dirYearPath = path + GetDateTakenFromImage(path).Year.ToString();
            if (!Directory.Exists(dirYearPath))
            {
                createFolder(path, GetDateTakenFromImage(path).Year.ToString(), out result);
            }
            string dirMonthPath = dirYearPath + GetDateTakenFromImage(path).Month.ToString();
            if (!Directory.Exists(dirMonthPath))
            {
                createFolder(dirYearPath, GetDateTakenFromImage(path).Month.ToString(), out result);
            }

            moveFile(path, dirMonthPath, out result);

            dirMonthPath = path;
            return result;
        }


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

        public string createFolder(string path, string folderName, out string result)
        {
            string pathString = System.IO.Path.Combine(path, folderName);

            // Create the subfolder. You can verify in File Explorer that you have this
            System.IO.Directory.CreateDirectory(pathString);
            result = path;
            return result;
        }

        public string copyFile(string sourceFile, string destFile, out string result)
        {
            // To copy a file to another location and 
            // overwrite the destination file if it already exists.
            System.IO.File.Copy(sourceFile, destFile, true);
            result = destFile;
            return result;
        }

        public string copyFolder(string path, string fileName, out string result)
        {
            string sourcePath = @"C:\Users\Public\TestFolder";
            string targetPath = @"C:\Users\Public\TestFolder\SubDir";

            // Use Path class to manipulate file and directory paths.
            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, fileName);

            // To copy a folder's contents to a new location:
            // Create a new target folder, if necessary.
            if (!System.IO.Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);
            }
            result = targetPath;
            return result;

        }

        public string moveFile(string SourcePath, string DestPath, out string result)
        {
            // To move a file or folder to a new location:
            if (!File.Exists(DestPath))
                System.IO.File.Move(SourcePath, DestPath);
            result = DestPath;
            return result;
        }

        public string moveFolder(string SourcePath, string DestPath, out string result)
        {
            // To move a file or folder to a new location:
            System.IO.File.Move(SourcePath, DestPath);
            result = DestPath;
            return result;
        }


        public string deleteFile(string SourcePath, string DestPath, out string result)
        {
            // Delete a file by using File class static method...
            if (System.IO.File.Exists(@"C:\Users\Public\DeleteTest\test.txt"))
            {
                try
                {
                    System.IO.File.Delete(@"C:\Users\Public\DeleteTest\test.txt");
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);

                }
            }
            result = DestPath;
            return result;
        }

        public string deleteFolder(string SourcePath, string DestPath, out string result)
        {
            // Delete a directory. Must be writable or empty.
            try
            {
                System.IO.Directory.Delete(@"C:\Users\Public\DeleteTest");
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
            }
            result = DestPath;
            return result;
        }    
    }
}