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
using Vlc.DotNet.Core.Interops.Signatures;
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

            TitleBar.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    DragMove();
            };

            BtClose.Click += (s, e) =>
            {
                Close();
            };
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var libDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, "libvlc", "win-x64"));
            this.vlcPlayer.SourceProvider.CreatePlayer(libDirectory);
            vlcPlayer.SourceProvider.MediaPlayer.Play(new Uri(AlarmHistory.videoPath));


       
        }
  
        public void PauseOrPlayFun(object sender, RoutedEventArgs e)
        {
            if (PauseOrPlay.Content.ToString() == "播放")
            {
                PauseOrPlay.Content = "暂停";
                this.vlcPlayer.SourceProvider.MediaPlayer.Play();
            }
            else
            {
                PauseOrPlay.Content = "播放";
                this.vlcPlayer.SourceProvider.MediaPlayer.Pause();
            }
        }
        private void StopFun(object sender, RoutedEventArgs e)
        {
            PauseOrPlay.Content = "播放";
            new Task(() =>
            {
                this.vlcPlayer.SourceProvider.MediaPlayer.Stop();//这里要开线程处理，不然会阻塞播放

            }).Start();
        }

    }


}
