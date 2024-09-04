using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace WpfApp1
{
    public class AlarmHistoryModelView
    {
        public ObservableCollection<History_Message> HistoryInfos  =
        new ObservableCollection<History_Message>();

        private ICommand _valueCommand;
        public ICommand ValueCommand
        {
            get
            {
                return _valueCommand;
            }
        }
        private void ValueCommandAction(object obj)
        {
            MessageBox.Show("成功123" + obj, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public AlarmHistoryModelView()
        {
            _valueCommand = new MyCommand() { DoAction = new Action<object>(ValueCommandAction) };
            for (int i = 0; i < MainWindow.historyMessages.Count; i++)
            {
                HistoryInfos.Add(MainWindow.historyMessages[i]);
            }
        }

    }
    public class AlarmHistoryInfo
    {
        public string DeviceIP { get; set; }
        public string DeviceName { get; set; }
        public string Datetime { get; set; }
        public int concentration {  get; set; }
        public double horizontalAngle { get; set; }
        public double verticalAngle { get; set; } = 0;

        public int presetPoint { get; set; }

        public string presetComment { get; set; }
    }
}
