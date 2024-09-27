using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace WpfApp1
{
    /// <summary>
    /// Backup.xaml 的交互逻辑
    /// </summary>
    public partial class Backup : Window
    {
        string dirpathStr = "";
        public Backup()
        {
            InitializeComponent();
            TitleBar.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    DragMove();
            };

            BtClose.Click += (s, e) =>
            {
                Close();
            };
            ResourceDictionary resourceDictionary;
            string languageStr = ConfigurationManager.AppSettings["Language"];
            if (languageStr.Equals("english"))
            {
                string english = "pack://application:,,,/Language/English.xaml";
                resourceDictionary = new ResourceDictionary { Source = new Uri(english, UriKind.RelativeOrAbsolute) };

            }
            else
            {
                string chinese = "pack://application:,,,/Language/Chinese.xaml";
                resourceDictionary = new ResourceDictionary { Source = new Uri(chinese, UriKind.RelativeOrAbsolute) };


            }


            // 将当前的资源字典从应用程序资源中移除
            Resources.MergedDictionaries.Remove(resourceDictionary);
            // 将新的资源字典添加到应用程序资源中
            Resources.MergedDictionaries.Add(resourceDictionary);



            dirpathStr = ConfigurationManager.AppSettings["dirpath"];
            dirPath.Text = dirpathStr;


        }
        public static void SaveDirpath(string dirpathStr)
        {
            //打开可执行的配置文件的 *.exe.config
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //删除配置节
            config.AppSettings.Settings.Remove("dirpath");
            //添加新的配置节
            config.AppSettings.Settings.Add("dirpath", dirpathStr);
            //保存对配置文件所作的修改
            config.Save(ConfigurationSaveMode.Modified);
            //强制重载已更改部分
            ConfigurationManager.RefreshSection("appSettings");

        }
        private void chooseDirectory(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string foldername = dialog.SelectedPath.Trim();
            Console.WriteLine("foldername:" + foldername);
            dirpathStr = foldername;
            dirPath.Text = foldername;
            SaveDirpath(foldername);
        }

        public void CopyFileToDirectory(string sourceFilePath, string destinationDirectory, string fileName)
        {
            // 确保目标文件夹存在
            // 获取源文件名
            Directory.CreateDirectory(destinationDirectory);

     

            // 复制文件
            File.Copy(sourceFilePath + fileName, destinationDirectory + "\\" + fileName, true); // true 表示如果目标文件存在，则覆盖它
        }

        private void export(object sender, RoutedEventArgs e)
        {
            try
            {
                // 获取文件夹中的所有文件名
                string[] fileNames = Directory.GetFiles(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
                List<string> cruiseStrs = new List<string>();
                List<string> presetsStrs = new List<string>();
                // 遍历文件名并输出
                foreach (string fileName in fileNames)
                {
                    Console.WriteLine(fileName);
                    string patternCruise = @"^.*_cruises.xml";
                    string patternPresets = @"^.*_presets.xml";
                    //string path = @"C:\Users\Example\Documents\example.txt";

                    if (Regex.IsMatch(fileName, patternCruise))
                    {
                        int index  = fileName.LastIndexOf("\\");
                        string fileName2 = fileName.Substring(index + 1);
                        cruiseStrs.Add(fileName2);
                    }
                    else if (Regex.IsMatch(fileName, patternPresets))
                    {
                        int index = fileName.LastIndexOf("\\");
                        string fileName2 = fileName.Substring(index + 1);
                        presetsStrs.Add(fileName2);
                    }
                }


                //doc.LoadXml("<bookstore></bookstore>");//用这句话,会把以前的数据全部覆盖掉,只有你增加的数据
                //doc.Load(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "HistoryMessages.xml");
                //doc.Save(dirpathStr);
                CopyFileToDirectory(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, dirpathStr, "HistoryMessages.xml");
                CopyFileToDirectory(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, dirpathStr, "LoginInfo.xml");



                for (int i = 0; i < cruiseStrs.Count; i++) {
                    CopyFileToDirectory(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, dirpathStr, cruiseStrs[i]);
                }
                for (int i = 0;i < presetsStrs.Count;i++) {
                    CopyFileToDirectory(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, dirpathStr, presetsStrs[i]);
                }
            }
            catch (Exception e2)
            {
                Console.WriteLine(e2.ToString());
            }
        }
        private void import(object sender, RoutedEventArgs e)
        {



        }
    }
}
