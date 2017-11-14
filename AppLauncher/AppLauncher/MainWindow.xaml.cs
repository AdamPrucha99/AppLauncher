using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace AppLauncher
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>//
    public partial class MainWindow : Window
    {
        ObservableCollection<Control> dirList = new ObservableCollection<Control>();
        ObservableCollection<Control> infoDirList = new ObservableCollection<Control>();
        ObservableCollection<Control> RootDirList = new ObservableCollection<Control>();
        public MainWindow()
        {
            InitializeComponent();
            getRootDirs();
            ProjectsView.ItemsSource = dirList;

        }
        public void getRootDirs()
        {
            Control dbFile = new Control();
            string[] resLines = dbFile.GetData("data.csv");
            foreach(var line in resLines)
            {
                searchFiles(line);
            }
        }
        public void searchFiles(string searchedDir)
        {
            string[] dirs = Directory.GetFiles(@"" + searchedDir + "", "*.sln", SearchOption.AllDirectories);
            showAllFiles(searchedDir, dirs);
        }
        public void showAllFiles(string searchedDir, string[] dirs)
        {
            
            foreach (string dir in dirs)
            {
                Control dataFileRes = new Control();
                Control infoFileRes = new Control();
                Control rootFileRes = new Control();

                string newDir = System.IO.Path.ChangeExtension(dir, null);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(dir);

                string dirToFile = (newDir + @"\" + @"bin\Debug\" + (fileName) + ".exe" + "");
                string dirToInfoFile = (newDir + @"\" + @"bin\Debug\" + "appInfo" + ".csv" + "");
                string dirToRootFolder = (newDir.Replace(@"\" + fileName, ""));
                string finalDirToRootFolder = dirToRootFolder + @"\" + fileName;

                //Kontrola zda existuje soubor pro poznámky
                rootFileRes.CheckExists(newDir);

                dataFileRes.FilePath = dirToFile;
                infoFileRes.FilePath = (newDir + ".csv");
                rootFileRes.FilePath = finalDirToRootFolder;

                dirList.Add(dataFileRes);
                infoDirList.Add(infoFileRes);
                RootDirList.Add(rootFileRes);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int selectedItem = ProjectsView.SelectedIndex;
            string processToRun = dirList[selectedItem].FilePath;
            string pathToStart = (@"" + processToRun + "");

            Process.Start(pathToStart);
        }
        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedItem = ProjectsView.SelectedIndex;
            string path = infoDirList[selectedItem].FilePath;
            Control infoNotes = new Control();
            string infoData = infoNotes.GetNotes(path);
            infoBox.Text = infoData;
            string pathToNotes = e.AddedItems[0].ToString();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int selectedItem = ProjectsView.SelectedIndex;
            string path = infoDirList[selectedItem].FilePath;
            string content = infoBox.Text;
            File.WriteAllText(path, content);

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int selectedItem = ProjectsView.SelectedIndex;
            string path = RootDirList[selectedItem].FilePath;
            string SourcePath = path;
            string DestinationPath = dir.Text;

            //Kopírování do nové složky
            Control dataHandler = new Control();
            dataHandler.CopyFiles(SourcePath, DestinationPath);          
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = @"C:\";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                dir.Text = dialog.FileName;
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            int selectedItem = ProjectsView.SelectedIndex;
            string path = RootDirList[selectedItem].FilePath;

            // Mazání Adresářů
            Control dataHandler = new Control();
            dataHandler.deleteFile(path);
        }
    }
}
