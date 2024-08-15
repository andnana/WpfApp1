﻿using System;
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
        public ObservableCollection<AlarmHistoryInfo> AlarmHistoryInfo { get; set; }

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
            AlarmHistoryInfo = new ObservableCollection<AlarmHistoryInfo>
            {
                new AlarmHistoryInfo { DeviceIP = "12.52.12.126", DeviceName = "12.52.12.126", Datetime = "2024/8/8 16:50:05" , 
                concentration = 10, horizontalAngle = 1.1, verticalAngle = 2.1, presetComment = "备注1", presetPoint = 2},
                new AlarmHistoryInfo { DeviceIP = "12.52.12.121", DeviceName = "12.52.12.121", Datetime = "2024/8/8 16:50:05" ,
                concentration = 101, horizontalAngle = 1.1, verticalAngle = 4.1, presetComment = "备注2", presetPoint = 3},
                new AlarmHistoryInfo { DeviceIP = "12.52.12.126", DeviceName = "12.52.12.126", Datetime = "2024/8/8 16:50:05",
                concentration = 102, horizontalAngle = 1.1, verticalAngle = 3.1, presetComment = "备注3", presetPoint = 4}
            };
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