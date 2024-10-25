using HandyControl.Controls;
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
using System.Windows.Shapes;
using System.Xml;
using static NPOI.XSSF.UserModel.Helpers.ColumnHelper;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using static WpfApp1.CHCNetSDK;

namespace WpfApp1
{
    /// <summary>
    /// Presets.xaml 的交互逻辑
    /// </summary>
    public partial class Presets : System.Windows.Window
    {
        int presetNum = 0;
        public List<Preset> presets = new List<Preset>();
        private List<int> presetIntList = new List<int>();
        public Presets()
        {
            InitializeComponent();
            for (int i = 1; i <= 255; i++) {
                if(i != 5 &&
                   i != 10 &&
                   i != 15 &&
                   i != 20 &&
                   i != 25 &&
                   i != 30 &&
                   i != 35 &&
                   i != 40 &&
                   i != 45 &&
                   i != 50 &&
                   i != 55 &&
                   i != 60 &&
                   i != 65 &&
                   i != 70 &&
                   i != 72 &&
                   i != 73 &&
                   i != 87 &&
                   i != 88 &&
                   i != 90 &&
                   i != 92 &&
                   i != 104 &&
                   i != 121 &&
                   i != 122)
                {
                    presetIntList.Add(i);
                }
                

            }
            
           
            TitleBar.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    DragMove();
            };

            BtMin.Click += (s, e) =>
            {
                WindowState = WindowState.Minimized;
            };

            //BtMax.Click += (s, e) =>
            //{
            //    WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            //};

            BtClose.Click += (s, e) =>
            {
                Close();
            };
            LoadPresetsFile();
            PresetsDataGrid.ItemsSource = presets;
        }


        private void remove(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("确认删除？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information);

            if (result == MessageBoxResult.OK)
            {
                Preset rowView = (Preset)((Button)e.Source).DataContext;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_presets.xml");
                XmlElement xe = xmlDoc.DocumentElement; // DocumentElement 获取xml文档对象的根XmlElement.
                string strPath = string.Format("/presets/preset[@preset_num=\"{0}\"]", rowView.preset_num.ToString());
                XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);  //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
                selectXe.ParentNode.RemoveChild(selectXe);
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_presets.xml");

                int removeIndex = presets.FindIndex(item => item.preset_num.ToString().Equals(rowView.preset_num.ToString()));
                presets.RemoveAt(removeIndex);
                PresetsDataGrid.Items.Refresh();
                new TipsWindow("删除成功", 3, TipsEnum.OK).Show();
            }
        }

        public void LoadPresetsFile()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_presets.xml", settings);
                xmlDoc.Load(reader);

                //xn 代表一个结点
                //xn.Name;//这个结点的名称
                //xn.Value;//这个结点的值
                //xn.ChildNodes;//这个结点的所有子结点
                //xn.ParentNode;//这个结点的父结点

                // 得到根节点bookstore
                XmlNode xn = xmlDoc.SelectSingleNode("presets");


                // 得到根节点的所有子节点
                XmlNodeList xnl = xn.ChildNodes;

                foreach (XmlNode xn1 in xnl)
                {
                    Preset preset = new Preset();
                    // 将节点转换为元素，便于得到节点的属性值
                    XmlElement xe = (XmlElement)xn1;
                    // 得到Type和ISBN两个属性的属性值
                    //bookModel.BookISBN = xe.GetAttribute("ISBN").ToString();
                    //bookModel.BookType = xe.GetAttribute("Type").ToString();
                    // 得到LoginInfo节点的所有子节点
                    XmlNodeList xnl0 = xe.ChildNodes;
                    preset.preset_num = int.Parse(xnl0.Item(0).InnerText);
                    preset.notes = xnl0.Item(1).InnerText;
                    presets.Add(preset);
                }

                reader.Close();
            }
            catch (Exception e)
            {
                //创建一个空的XML
                XmlDocument document = new XmlDocument();
                //声明头部
                XmlDeclaration dec = document.CreateXmlDeclaration("1.0", "utf-8", "yes");
                document.AppendChild(dec);

                //创建根节点
                XmlElement root = document.CreateElement("presets");
                document.AppendChild(root);


                //保存文档
                document.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_presets.xml");


                Console.WriteLine(e.Message);
            }
            // 创建XmlDDocument对象，并装入xml文件

        }
        /// <summary>
        /// 调用预置点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void invokePreset(object sender, RoutedEventArgs e)
        {
            if (MainWindow.sbmc == "")
            {
                new TipsWindow("请先登录", 3, TipsEnum.FAIL).Show();
                return;
            }
            Preset rowView = (Preset)((Button)e.Source).DataContext;
            NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)CHCNetSDK.GOTO_PRESET, (uint)rowView.preset_num);
        }
        private void Confirm_Closed(object sender, EventArgs e)
        {
            ConfirmWindow confirmWindow = sender as ConfirmWindow;
            if (confirmWindow != null)
            {
                if (confirmWindow.Result == 1)
                {
                    Console.WriteLine("ok");
                    if (MainWindow.sbmc == "")
                    {
                        new TipsWindow("请先登录", 3, TipsEnum.FAIL).Show();
                        return;
                    }
                    bool isOK = NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)CHCNetSDK.SET_PRESET, (uint)presetNum);
                    if (!isOK)
                    {
                        new TipsWindow("预置点修改失败", 3, TipsEnum.FAIL).Show();
                    }
                    else
                    {
                        new TipsWindow("预置点修改成功", 3, TipsEnum.OK).Show();
                    }
                    presetNum = 0;
                }
                else
                {
                    Console.WriteLine("cancel");
                }
            }
        }
        private void updatePreset(object sender, RoutedEventArgs e)
        {

            ConfirmWindow confirm = new ConfirmWindow("确认修改？", TipsEnum.BOTH);
            Preset rowView = (Preset)((Button)e.Source).DataContext;
            presetNum = rowView.preset_num;
            confirm.Closed += Confirm_Closed;
            confirm.Show();

        
        }
        private void AddPreset(object sender, RoutedEventArgs e)
        {
            int presetMax = 2;
            int preset = 0;
            bool haveVal = false;
            for (int i = 0; i < presetIntList.Count; i++)
            {
                for (int j = 0; j < presets.Count; j++)
                {
                    if (presetIntList[i] == presets[j].preset_num)
                    {
                        haveVal = true;
                    }
                }
                if (haveVal == true)
                {
                    haveVal = false;
                    continue;
                }
                if (haveVal == false)
                {
                    preset = presetIntList[i];
                    goto Line1;
                }
            }
        Line1:
            if (haveVal)
            {
                if (presets.Count > 0)
                {
                    presetMax = presets.Max(p => p.preset_num);

                }
                if (presetMax == 4 || 
                    presetMax == 9 || 
                    presetMax == 14 || 
                    presetMax == 19 || 
                    presetMax == 24 || 
                    presetMax == 29 || 
                    presetMax == 34 || 
                    presetMax == 39 || 
                    presetMax == 44 || 
                    presetMax == 49 || 
                    presetMax == 54 || 
                    presetMax == 59 || 
                    presetMax == 64 || 
                    presetMax == 89 || 
                    presetMax == 91 || 
                    presetMax == 103)
                {
                    preset = presetMax + 2;
                }
                else if(presetMax == 66) {
                    preset = presetMax + 5;
                }
                else if (presetMax == 71 || presetMax == 86 || presetMax == 120)
                {
                    preset = presetMax + 3;
                }
                else
                {
                    preset = presetMax + 1;
                }
            }

            bool isOK = NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)CHCNetSDK.SET_PRESET, (uint)preset);
            if (!isOK)
            {
                new TipsWindow("预置点增加失败", 3, TipsEnum.FAIL).Show();
            }
            else
            {
                new TipsWindow("预置点增加成功", 3, TipsEnum.OK).Show();
                //MainWindow.presetPOJOList[MainWindow.Chosen_device_num].Presets[preset - 1] = "1";//PresetCommentTextBox.Text;
                //Tool.SaveInstanceToFile(MainWindow.presetPOJOList[MainWindow.Chosen_device_num], MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].IP);


                Preset presetObj = new Preset();
                presetObj.preset_num = preset;
                presetObj.notes = NotesText.Text;

                XmlDocument doc = new XmlDocument();

                try
                {
                    //doc.LoadXml("<bookstore></bookstore>");//用这句话,会把以前的数据全部覆盖掉,只有你增加的数据
                    doc.Load(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_presets.xml");
                    XmlNode root = doc.SelectSingleNode("presets");





                    XmlElement xelPreset = doc.CreateElement("preset");


                    XmlAttribute xelPresetNumAttr = doc.CreateAttribute("preset_num");
                    xelPresetNumAttr.InnerText = presetObj.preset_num.ToString();
                    xelPreset.SetAttributeNode(xelPresetNumAttr);


                    XmlElement xelPresetNum = doc.CreateElement("preset_num");
                    xelPresetNum.InnerText = presetObj.preset_num.ToString();
                    xelPreset.AppendChild(xelPresetNum);

                    XmlElement xelNotes = doc.CreateElement("notes");
                    xelNotes.InnerText = presetObj.notes;
                    xelPreset.AppendChild(xelNotes);



                    root.AppendChild(xelPreset);

                    doc.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_presets.xml");
                    presets.Add(presetObj);
                    PresetsDataGrid.Items.Refresh();

                }
                catch (Exception e1)
                {
                    Console.WriteLine(e.ToString());
                }



            }



        }


        /// <summary>
        /// 增加预置点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
   /*     private void addPreset(object sender, RoutedEventArgs e)
        {

            //MessageBox.Show("选中的值是：" + selectedItem.Content.ToString());
                        int presetInt = int.Parse(selectedItem.Content.ToString());
                        bool isOK = NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)CHCNetSDK.SET_PRESET, (uint)presetInt);
                        if (!isOK)
                        {
                            Growl.SuccessGlobal("预置点增加失败");
                        }
                        else
                        {
                            Growl.SuccessGlobal("预置点增加成功");
                            MainWindow.presetPOJOList[MainWindow.Chosen_device_num].Presets[presetInt - 1] = "1";//PresetCommentTextBox.Text;
                            Tool.SaveInstanceToFile(MainWindow.presetPOJOList[MainWindow.Chosen_device_num], MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].IP);
                        }


        }*/

    }
}
