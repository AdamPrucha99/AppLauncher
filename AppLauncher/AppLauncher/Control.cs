using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace AppLauncher
{
    class Control
    {

        public string FilePath;
        public string Lines;

        public override string ToString()
        {
            return FilePath;
        }
        public string[] GetData(string pathToFile)
        {
            string[] Lines = File.ReadAllLines(pathToFile);
            return Lines;
        }
        public string GetNotes(string pathToNotes)
        {
            string Notes = File.ReadAllText(pathToNotes);
            return Notes;
        }
        public void CheckExists(string pathToFile)
        {
            if (!File.Exists(pathToFile + ".csv"))
            {
                File.CreateText(pathToFile + ".csv").Close();
            }
        }
        public void CopyFiles(string SourcePath, string DestinationPath)
        {
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
            SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));
            }

            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
            }
        }
        public void deleteFile(string path)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }


        
    }
}
