using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Asynctest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml http://www.c-sharpcorner.com/article/async-and-await-in-c-sharp/
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }
        bool AutomaticMode = true;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AutomaticMode = true;

            Method2();
            Method1();
        }
        public async Task Method1()
        {
            await Task.Run(() =>
            {
                //LongOperation();
                WatchForFile();
            });
        }
        public async Task Method2()
        {
            await Task.Run(() =>
            {
                LongOperation();
                //WatchForFile();
            });
        }
        void WatchForFile()
        {
            while (AutomaticMode == true)
            { string path = @"C:\temp\";
                string[] files = System.IO.Directory.GetFiles(path, "*.txt"); //https://stackoverflow.com/questions/3152157/find-a-file-with-a-certain-extension-in-folder
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => this.txt2.Text = "waiting for file"));

                if (files.Length > 0)
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => this.txt2.Text = "found file"));

                    AutomaticMode = false;
                    foreach (string file in files)
                    {
                        string[] Splitfilename=file.Split(new string[] { @"\" }, StringSplitOptions.None);
                        CutAndPasteFile(Splitfilename[Splitfilename.Length - 1], path, path + @"\moved\");

                        MessageBox.Show(file);
                    }
                    AutomaticMode = true;
                }
            }




        }
        void LongOperation()
        {
            while (AutomaticMode == true)
            {
                for (int i = 0; i < 10; i++)
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,new Action(() => this.txt1.Text = i.ToString())); //https://stackoverflow.com/questions/1644079/change-wpf-controls-from-a-non-main-thread-using-dispatcher-invoke
                    Console.WriteLine(" Method 1");
                    Thread.Sleep(100);
                }

            }
            MessageBox.Show("done!");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AutomaticMode = false;
        }

        public void CopyAndPasteFile(string SourceFileName, string SourcePath, string TargetPath, string TargetFileName = "", bool OverWriteFile = false)
        {
            string SourceFilePathString = SourcePath + @"\" + SourceFileName;
            if (TargetFileName == "") { TargetFileName = SourceFileName; }
            string TargetFilePathString = TargetPath + @"\" + TargetFileName;

            if (!System.IO.Directory.Exists(TargetPath))
            { CreateFolder(TargetPath); }

            if (System.IO.File.Exists(SourceFilePathString))
            {
                System.IO.File.Copy(SourceFilePathString, TargetFilePathString, OverWriteFile);
            }
            else
            { MessageBox.Show("File: " + SourceFilePathString + " doesn't exists!"); }

        }
        public void CutAndPasteFile(string SourceFileName, string SourcePath, string TargetPath, string TargetFileName = "", bool OverWriteFile = false)
        {
            string SourceFilePathString = SourcePath + @"\" + SourceFileName;
            if (TargetFileName == "") { TargetFileName = SourceFileName; }
            string TargetFilePathString = TargetPath + @"\" + TargetFileName;

            if (!System.IO.Directory.Exists(TargetPath))
            { CreateFolder(TargetPath); }

            if (System.IO.File.Exists(SourceFilePathString))
            {
                System.IO.File.Move(SourceFilePathString, TargetFilePathString);
            }
            else
            { MessageBox.Show("File: " + SourceFilePathString + " doesn't exists!"); }


        }

        public void CreateFolder(string FolderPathString)
        {
            if (!System.IO.Directory.Exists(FolderPathString))
            {
                System.IO.Directory.CreateDirectory(FolderPathString);
            }
            else
            { MessageBox.Show("Folder: " + FolderPathString + " already exists!"); }


        }
        //  private async void Button_Click(object sender, RoutedEventArgs e)
        //  {
        //      int number1 = int.Parse(txt1.Text);
        //              int number2 = int.Parse(txt2.Text);
        //               int result = 0;
        //              result = await Calculate(number1, number2);
        //              anwser.Text = result.ToString();
        //  }

        //  private Task<int> Calculate(int number1, int number2)
        // {
        //      Task<int> t = 5;
        //  //Task<int> t = new Task<int>(BeginCalculate, new Tuple<int, int>(number1, number2));
        //     t.Start();
        //     return t;
        //}

    }
}
    