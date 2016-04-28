using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using Common;
using log4net;
#if CKK65
using Devices.CPS;
#endif
using Devices.MCCB;
using Lines.Com;

namespace LinesDevicesManager
{
    public class ComHelper
    {
        /// <summary>
        /// 串口线路的配置结构体
        /// </summary>
        public struct ComConfig
        {
            public SerialPort ComPort;
            public int Timeout;
            public int RefreshPeriod;
            public int ConnectCheckPeriod;
            public int InterFrameGap;
            public Dictionary<byte, string> Device;
        }

        #region 属性

        #endregion

        #region 公有方法
        /// <summary>
        /// 获取配置文件中串口线路的信息
        /// </summary>
        public static List<ComConfig> GetComConfig()
        {
            try
            {
                List<ComConfig> ls = new List<ComConfig>();

                List<string> strName = StringPlus.GetStrArray(ConfigHelper.GetConfigString("ComName"), ';', true);
                List<string> strBaudRate = StringPlus.GetStrArray(ConfigHelper.GetConfigString("ComBaudRate"), ';', false);
                List<string> strParity = StringPlus.GetStrArray(ConfigHelper.GetConfigString("ComParity"), ';', false);
                List<string> strStopBits = StringPlus.GetStrArray(ConfigHelper.GetConfigString("ComStopBits"), ';', false);
                List<string> strComTimeout = StringPlus.GetStrArray(ConfigHelper.GetConfigString("ComTimeout"), ';', false);
                List<string> strRefreshPeriod = StringPlus.GetStrArray(ConfigHelper.GetConfigString("ComRefreshPeriod"), ';', false);
                List<string> strConnectCheckPeriod = StringPlus.GetStrArray(ConfigHelper.GetConfigString("ComConnectCheckPeriod"), ';', false);
                List<string> strComInterFrameGap = StringPlus.GetStrArray(ConfigHelper.GetConfigString("ComInterFrameGap"), ';', false);
                List<string> strDeviceID = StringPlus.GetStrArray(ConfigHelper.GetConfigString("ComDeviceID"), ';', false);
                List<string> strDeviceType = StringPlus.GetStrArray(ConfigHelper.GetConfigString("ComDeviceType"), ';', true);

                List<int> baudrate = new List<int>();
                foreach (string str in strBaudRate)
                {
                    baudrate.Add(int.Parse(str));
                }

                List<Parity> parity = new List<Parity>();
                foreach (string str in strParity)
                {
                    parity.Add(((Parity)(int.Parse(str))));
                }

                List<StopBits> stop = new List<StopBits>();
                foreach (string str in strStopBits)
                {
                    stop.Add(((StopBits)(int.Parse(str))));
                }

                for (int i = 0; i < strName.Count; i++)
                {
                    ComConfig cc = new ComConfig();

                    cc.ComPort = new SerialPort(strName[i], baudrate[i], parity[i], 8, stop[i]);
                    cc.Timeout = int.Parse(strComTimeout[i]);
                    cc.RefreshPeriod = int.Parse(strRefreshPeriod[i]);
                    cc.ConnectCheckPeriod = int.Parse(strConnectCheckPeriod[i]);
                    cc.InterFrameGap = int.Parse(strComInterFrameGap[i]);

                    Log.InfoFormat("解析串口线路配置信息：{0},{1},{2},{3},刷新周期{4}(ms),连接检测周期{5}(ms),超时{6}(ms),帧间隙{7}(ms)", 
                                   cc.ComPort.PortName, cc.ComPort.BaudRate, cc.ComPort.Parity.ToString(), 
                                   cc.ComPort.StopBits.ToString(), cc.RefreshPeriod, cc.ConnectCheckPeriod,
                                   cc.Timeout, cc.InterFrameGap);

                    cc.Device = new Dictionary<byte, string>();

                    List<string> deviceID = StringPlus.GetStrArray(strDeviceID[i], ',', false);
                    List<string> deviceType = StringPlus.GetStrArray(strDeviceType[i], ',', false);

                    for (int j = 0; j < deviceID.Count; j++)
                    {
                        cc.Device.Add(byte.Parse(deviceID[j]), deviceType[j]);
                        Log.InfoFormat("解析串口线路配置信息：设备通讯地址[{0}],类型[{1}]", deviceID[j], deviceType[j]);
                    }
                    ls.Add(cc);
                }
                return ls;
            }
            catch (System.Exception ex)
            {
                string str = "解析串口线路配置信息出错，程序无法继续执行: " + ex.Message;
                Log.Fatal(str);
                throw new Exception(str);
            }
        }

        /// <summary>
        /// 初始化所有ComLines以及其挂接的设备
        /// </summary>
        public static void InitComLinesandDevices()
        {
            string msg = "";

            List<ComConfig> coms = GetComConfig();
            foreach (ComConfig com in coms)
            {
                bool err = false;

                try
                {
                    com.ComPort.Open();
                }
                catch (System.Exception ex)
                {
                    string str = string.Format("初始化[{0}]失败:", com.ComPort.PortName) + ex.Message;
                    Log.Error(str);
                    msg += str + "\n";
                    err = true;
                }

                ComLine cl = new ComLine(com.ComPort);

                cl.ReceiveTimeout = com.Timeout;
                cl.SendTimeout = com.Timeout;
                cl.InterFrameGap = com.InterFrameGap;
                cl.CheckConnect(true, (uint)com.ConnectCheckPeriod);

                foreach (byte id in com.Device.Keys)
                {
                    try
                    {
                        #if CKK65
                        if (com.Device[id] == "CKK65")
                        {
                            CKK65 dev = new CKK65(id, cl);
                            cl.AddDevice(dev);
                            //cl.AddTimer(com.RefreshPeriod, true, dev.PeriodRefresh);                            
                        }
                        else if (com.Device[id] == "CKM55LC-M")
                        #else
                        if (com.Device[id] == "CKM55LC-M")
                        #endif
                        {
                            CKM55LC_M dev = new CKM55LC_M(id, cl);
                            cl.AddDevice(dev);
                        }
                        else
                        {
                            string str = string.Format("初始化[{0}][{1}][{2}]失败:不支持此设备类型,此设备将被忽略",
                                                       com.ComPort.PortName, id, com.Device[id]);
                            Log.Warn(str);
                            msg += str + "\n";
                        }
                    }
                    catch (System.Exception ex)
                    {
                        string str = string.Format("初始化[{0}][{1}][{2}]失败:", com.ComPort.PortName, id, com.Device[id]) + ex.Message;
                        Log.Error(str);
                        msg += str + "\n";
                        err = true;
                    }
                }

                AddComLine(cl);

                string info = "挂接设备";
                foreach (BaseDevice bdev in cl.Devices)
                {
                    info += string.Format("[{0}-{1}],", bdev.Index, bdev.Type);
                }
                if (err == true)
                {
                    Log.ErrorFormat("初始化[{0}]失败，{1}", cl.Name, info);
                }
                else
                {
                    Log.InfoFormat("初始化[{0}]成功，{1}", cl.Name, info);
                }
            }

            if (msg != "")
            {
                Log.Error(msg);
                throw new Exception(msg);
            }
        }

        /// <summary>
        /// 添加一个ComLine对象，当前对象替换同名老对象
        /// </summary>
        public static void AddComLine(ComLine cl)
        {
            if (_dictComLines.ContainsKey(cl.Name) == false)
            {
                _dictComLines.Add(cl.Name, cl);
            }
            else
            {
                if (Log.IsDebugEnabled) { Log.DebugFormat("[{0}]已经存在，替换添加", cl.Name); }
                _dictComLines[cl.Name].Clear();
                _dictComLines.Remove(cl.Name);
                _dictComLines.Add(cl.Name, cl);
            }

            if (Log.IsDebugEnabled) { Log.DebugFormat("[{0}]添加成功", cl.Name); }
        }
        
        /// <summary>
        /// 移除一个ComLine对象，并关闭相应的COM端口
        /// </summary>
        public static void RemoveComLine(string name)
        {
            _dictComLines[name].COM.Close();
            _dictComLines[name].Clear();
            _dictComLines.Remove(name);

            if (Log.IsDebugEnabled) { Log.DebugFormat("[{0}]移除成功", name); }
        }

        /// <summary>
        /// 移除所有ComLine对象
        /// </summary>
        public static void RemoveAll(string name)
        {
            foreach (ComLine cl in _dictComLines.Values)
            {
                cl.COM.Close();
                cl.Clear();
            }

            _dictComLines.Clear();

            if (Log.IsDebugEnabled) { Log.Debug("所有ComLine对象移除成功"); }
        }
        
        /// <summary>
        /// 根据COM名称获取相应的ComLine对象
        /// </summary>
        public static ComLine GetComLine(string name)
        {
            return _dictComLines[name];
        }

        /// <summary>
        /// 获取所有ComLine对象
        /// </summary>
        public static List<ComLine> GetComLines()
        {
            return _dictComLines.Values.ToList();
        }

        /// <summary>
        /// 获取所有Com名称
        /// </summary>
        public static List<string> GetComLineNames()
        {
            return _dictComLines.Keys.ToList();
        }

        /// <summary>
        /// 根据COM名称获取挂接设备对象
        /// </summary>
        public static List<BaseDevice> GetDevices(string name)
        {
            return _dictComLines[name].Devices;
        }
        #endregion

        #region 保护方法

        #endregion

        #region 保护字段
        /// <summary>
        /// 日志
        /// </summary>
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(ComHelper));

        /// <summary>
        /// 保存ComLine对象
        /// </summary>
        private static Dictionary<string, ComLine> _dictComLines = new Dictionary<string, ComLine>();
        #endregion
    }
}

