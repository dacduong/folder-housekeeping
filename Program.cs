using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CleanUp
{
    class Program
    {
        private static string[] IgnoreFolders;
        static void Main(string[] args)
        {
            if (args.Length < 2)
                return;
            string iFolders = System.Configuration.ConfigurationManager.AppSettings["IgnoreFolders"];
            IgnoreFolders = iFolders.ToLower().Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            string rootFolder = args[0];
            int days = Math.Abs(Convert.ToInt32(args[1]));
            if (!Directory.Exists(rootFolder))
                return;
            CleanFolder(rootFolder, days);
        }

        private static void CleanFolder(string dir, int days)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            //delete files
            FileInfo[] fileArr = dirInfo.GetFiles();
            DateTime now = DateTime.Now;
            foreach (FileInfo fileInfo in fileArr)
            {
                if (fileInfo.CreationTime.AddDays(days) < now && fileInfo.LastWriteTime.AddDays(days) < now)
                    fileInfo.Delete();
            }
            
            //recusive
            DirectoryInfo[] subDirArr = dirInfo.GetDirectories();
            foreach (DirectoryInfo subDirInfo in subDirArr)
            {
                if (!IgnoreFolders.Contains(subDirInfo.Name.ToLower()))
                    CleanFolder(subDirInfo.FullName, days);
            }

            //remove dir if empty
            if (dirInfo.GetFiles().Length == 0)
                dirInfo.Delete();
        }
    }
}
