using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1;

namespace WpfApp1
{

    public class DeviceModelView 
    {
      
        public ObservableCollection<Device> Device { get; set; }

        private ICommand _valueCommand;
        public ICommand ValueCommand
        {
            get {
                return _valueCommand; 
                }
        }
        private void ValueCommandAction(object obj)
        {
            MessageBox.Show("成功123" + obj, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public DeviceModelView()
        {
            _valueCommand = new MyCommand() { DoAction = new Action<object>(ValueCommandAction) };
            Device = new ObservableCollection<Device>
            {
                new Device { DeviceIP = "12.52.12.126", DeviceName = "12.52.12.126", Datetime = "2024/8/8 16:50:05" },
                new Device { DeviceIP = "12.52.12.121", DeviceName = "12.52.12.121", Datetime = "2024/8/8 16:50:05" },
                new Device { DeviceIP = "12.52.12.126", DeviceName = "12.52.12.126", Datetime = "2024/8/8 16:50:05" }
            };
        }
   


    }
    public class Device
    {
        public string DeviceIP { get; set; }
        public string DeviceName { get; set; }
        public string Datetime { get; set; }
    }
}
