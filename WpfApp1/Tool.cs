using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using HandyControl.Controls;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WpfApp1
{
    internal class Tool
    {
        #region 4

        /// <summary>
        /// 流->字节数组->字符数组->字符串
        /// </summary>
        /// <param name="stream">接到的流</param>
        /// <returns></returns>
        public static string streamToString(byte[] stream)
        {
            //流->字节数组
            byte[] buffer = stream;

            //字节数组->字符数组
            char[] ch = new ASCIIEncoding().GetChars(buffer);

            //字符数组->字符串
            string str = new string(ch);

            return str.Substring(0, str.IndexOf(".jpg") + 4);
        }

        #endregion 4

        #region 压缩图片

        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片</param>
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="dHeight">高度</param>
        /// <param name="dWidth"></param>
        /// <param name="flag">压缩质量(数字越小压缩率越高) 1-100</param>
        /// <returns></returns>
        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag, string nd, int device_num)
        {
            // dWidth = 1920 * dHeight = 1080

            Image iSource = Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth) //将**改成c#中的或者操作符号
            {
                if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }
            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            var sourceString = "浓度" + nd;                               //水印内容(今天日期)
            var font = new Font("Arial", 53);                                                // 水印字体
            var size = g.MeasureString(sourceString, font);                          //水印的尺寸
            g.DrawString(sourceString, font, System.Drawing.Brushes.Red,             //把我们上面给水印定义好的属性放进去，并给水印添加个红色字体
            new PointF(iSource.Width - size.Width - 260, iSource.Height - size.Height - 200));     //计算好水印的位置（根据宽高做个减法）

            g.Dispose();
            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();

                #region 将图片发送至服务器

                //  TCPTool.sendToServer(dFile,device_num);

                #endregion 将图片发送至服务器
            }
        }

        #endregion 压缩图片

        #region 关闭指定MessageBox

        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public const int WM_CLOSE = 0x10;

        public static void KillMessageBox(string windowsName)
        {
            //按照MessageBox的标题，找到MessageBox的窗口
            IntPtr ptr = FindWindow(null, windowsName);
            if (ptr != IntPtr.Zero)
            {
                //找到则关闭MessageBox窗口
                PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }

        #endregion 关闭指定MessageBox

        #region 窗体处理

        /// <summary>
        /// 功能名称:查看MDI子窗体是否已经被打开
        /// 输入参数:MdiFather,Form,需要判断的父窗体对象
        ///          MdiChild,string,需要判断的子窗体控件名
        /// 返回结果:-1为没有被打开,正数为子窗体集的数组下标
        /// </summary>
        public static int HaveOpened(Form frmMdiFather, string strMdiChild)
        {
            int bReturn = -1;
            for (int i = 0; i < frmMdiFather.MdiChildren.Length; i++)
            {
                if (frmMdiFather.MdiChildren[i].Name == strMdiChild)
                {
                    frmMdiFather.MdiChildren[i].BringToFront();
                    bReturn = i;
                    break;
                }
            }
            return bReturn;
        }

        #endregion 窗体处理

        #region 处理字符

        private static char[] strWall = { 'N', 'M', 'W', 'D', 'E', 'A', 'B', 'C' };
        private static string nmwded = "";

        public static string TakeText(string[] strArray)
        {
            nmwded = "";
            try
            {
                foreach (string str in strArray)
                {
                    #region 判断该字符串是否符合条件

                    if (str.Length == 0)
                    {
                        continue;
                    }
                    if (!str.StartsWith("N"))
                    {
                        continue;
                    }
                    if (str.EndsWith("B"))
                    {
                        continue;
                    }
                    int i = 0;
                    for (i = 0; i < strWall.Length - 1; i++)
                    {
                        if (str.Count(c => c == strWall[i]) != 1)
                        {
                            i = strWall.Length + 1;
                        }
                    }
                    if (i > strWall.Length)
                    {
                        continue;
                    }

                    #endregion 判断该字符串是否符合条件

                    nmwded = str;
                    return str.Remove(0, 1);
                }
                // Console.WriteLine("这轮没有");
                return nmwded;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "");
                return nmwded;
            }
        }

        #endregion 处理字符

        #region 光强

        public static int PD_chart_current(int value)
        {
            if (value >= 5000)
                return 8;
            if (value >= 2000)
                return 7;
            if (value >= 400)
                return 6;
            if (value >= 200)
                return 5;
            if (value > 50)
                return 4;
            if (value > 20)
                return 3;
            if (value > 10)
                return 2;
            if (value > 1)
                return 1;
            return 0;
        }

        #endregion 光强

        #region 读写本地Json

        /// <summary>
        /// 一条空的巡航路径
        /// </summary>
        public static List<CruisePOJO> emptyCruises;

        /// <summary>
        /// 将json保存到本地
        /// </summary>
        /// <param name="ins"></param>
        /// <param name="fullPath"></param>
        public static void SaveInstanceToFile(PresetPOJO ins, string fullPath)
        {
            FileStream file = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + fullPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            using (var stream = new StreamWriter(file))
            {
                string str = JsonConvert.SerializeObject(ins);
                stream.Write(str);
                stream.Flush();
                stream.Close();
            }
        }

        /// <summary>
        /// 读取本地json
        /// </summary>
        /// <param name="ins"></param>
        /// <param name="fullPath"></param>
        public static PresetPOJO LoadFileToInstance(string fullPath)
        {
            try
            {
                var fs = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (var stream = new StreamReader(fs))
                {
                    return JsonConvert.DeserializeObject<PresetPOJO>(stream.ReadToEnd());
                }
            }
            catch (FileNotFoundException)
            {
                PresetPOJO presetPOJO = new PresetPOJO
                {
                    Speed = "15",
                    Presets = new List<string>(),
                    Cruises = new List<List<CruisePOJO>>()
                };
                for (int i = 0; i < 255; i++)
                {
                    presetPOJO.Presets.Add("空");
                }
                for (int i = 0; i < 8; i++)
                {
                    presetPOJO.Cruises.Add(emptyCruises);
                }
                SaveInstanceToFile(presetPOJO, fullPath);

                return presetPOJO;
            }
        }

        #endregion 读写本地Json

        #region Excel

        public static byte[] ToExcelBytes<TEntity>(IList<TEntity> items) where TEntity : class, new()
        {
            var workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("sheet1");
            SetTitle<TEntity>(workbook, sheet);

            int index = 1;
            foreach (TEntity entity in items)
            {
                IRow row = sheet.CreateRow(index);
                PropertyInfo[] values = entity.GetType().GetProperties();
                int col = 0;
                foreach (PropertyInfo property in values)
                {
                    object val = property.GetValue(entity, null);
                    if (val == null)
                        row.CreateCell(col).SetCellValue("");
                    else
                        row.CreateCell(col).SetCellValue(val.ToString());
                    col++;
                }
                index++;
            }
            //写入
            using (var file = new MemoryStream())
            {
                workbook.Write(file);
                return file.GetBuffer();
            }
        }

        private static void SetTitle<T>(HSSFWorkbook workbook, ISheet sheet)
        {
            var titles = new List<string>();
            PropertyInfo[] propertys = typeof(T).GetProperties();
            foreach (PropertyInfo property in propertys)
            {
                object[] excelColumns = property.GetCustomAttributes(typeof(ExcelColumnAttribute), true);
                if (excelColumns.Length > 0)
                {
                    string like = (excelColumns[0] as ExcelColumnAttribute).DisplayNameLike;

                    if (!string.IsNullOrEmpty(like))
                    {
                        titles.Add(like);
                    }
                    else
                    {
                        titles.Add(property.Name);
                    }
                }
            }

            //写入
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < titles.Count; i++)
            {
                row.CreateCell(i).SetCellValue(titles[i]);
            }
        }

        private class EntityColumn
        {
            public string ColumnName { get; set; }

            public int ExcelIndex { get; set; }
        }

        /// <summary>
        /// Excel列属性【添加标签对象】
        /// </summary>
        public class ExcelColumnAttribute : Attribute
        {
            public ExcelColumnAttribute()
            {
                ExcelIndex = -1;
            }

            public ExcelColumnAttribute(int excelIndex)
            {
                ExcelIndex = excelIndex;
            }

            public ExcelColumnAttribute(string displayNameLike)
            {
                DisplayNameLike = displayNameLike;
            }

            public int ExcelIndex { get; set; }

            public string DisplayNameLike { get; set; }
        }

        public static string patch_path;

        public static void saveExcel(List<History_Message> list, string savepath, bool isShowTip)
        {
            try
            {
                var bytes = ToExcelBytes(list);
                using (FileStream fs = new FileStream(savepath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                }
                if (isShowTip)
                {
                    MessageBox.Show("保存成功。文件路径为：" + savepath);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                new TipsWindow("保存失败", 3, TipsEnum.FAIL).Show();
            }
        }
        public static void saveExcel2(List<HistoryMessage2> list, string savepath, bool isShowTip)
        {
            try
            {
                var bytes = ToExcelBytes(list);
                using (FileStream fs = new FileStream(savepath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                }
                if (isShowTip)
                {
                    MessageBox.Show("保存成功。文件路径为：" + savepath);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("save fail");
                Console.WriteLine(ex.Message);
                new TipsWindow("保存失败", 3, TipsEnum.FAIL).Show();
            }
        }
        #endregion Excel
    }
}