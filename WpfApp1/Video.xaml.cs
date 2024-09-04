using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Vlc.DotNet.Wpf;

namespace WpfApp1
{
    /// <summary>
    /// Video.xaml 的交互逻辑
    /// </summary>
    public partial class Video : Window
    {
        public Video()
        {
            InitializeComponent();

            /*
                   var vlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(Environment.CurrentDirectory, "libvlc", "win-x64"));
                   vlcPlayer.SourceProvider.CreatePlayer(vlcLibDirectory);
                   vlcPlayer.SourceProvider.MediaPlayer.Play(new Uri(AlarmHistory.videoPath));*/
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var libDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, "libvlc", "win-x64"));
            this.vlcPlayer.SourceProvider.CreatePlayer(libDirectory);
            vlcPlayer.SourceProvider.MediaPlayer.Play(new Uri(AlarmHistory.videoPath));

        }
    
    }


}
