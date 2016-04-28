using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using log4net;

namespace Devices.MCCB
{
    public class IdentityValue
    {
        #region 公有字段
        #endregion

        #region 事件定义
        /// <summary>
        /// 读寄存器回调事件
        /// </summary>
        public event AsyncCallback GetCallBack;
        public event AsyncCallback SetCallBack;

        /// <summary>
        /// 读取寄存器事件
        /// </summary>
        public event GetSetRegisterAsyncHandler GetRegisterAsync;
        public event GetSetRegisterHandler GetRegister;
        public event GetSetRegisterHandler SetRegister;
        #endregion

        #region 属性
        /// <summary>
        /// 产品编号
        /// </summary>
        public string Device_ProductNumber
        {
            get
            {
                string str = "";
                byte[] asc = new byte[16];
                char[] chars = new char[16];

                for (int i = 0; i < 8; i++)
                {
                    ushort tmp = _cache_identity_code_1.Buf[MBREG.Device_ProductNumber_Start - MBREG.Address_Identity_Code_Start_1 + i];

                    if (tmp == 0)
                    {
                        break;
                    }
                    else
                    {
                        asc[i * 2] = (byte)(tmp & 0x00FF);
                        asc[i * 2 + 1] = (byte)((tmp & 0xFF00) >> 8);
                    }
                }

                Decoder d = Encoding.ASCII.GetDecoder();
                d.GetChars(asc, 0, asc.Length, chars, 0);

                foreach (char ch in chars)
                {
                    str += ch;
                }

                return str;
            }
            set
            {
                byte[] asc = new byte[16];
                char[] chars = new char[16];
                ushort[] buf = new ushort[8];

                Encoder e = Encoding.ASCII.GetEncoder();
                chars = value.ToCharArray();
                e.GetBytes(chars, 0, chars.Length, asc, 0, false);

                for (int i = 0; i < 8; i++)
                {
                    ushort tmp = (ushort)(asc[2 * i + 1] << 8);
                    tmp |= (ushort)asc[2 * i];
                    buf[i] = tmp;
                }

                ushort[] subuf = new ushort[] { 0xD0D0, 0xC0DE };
                pSetRegister((ushort)MBREG.Super_Write, 2, subuf);

                pSetRegister((ushort)MBREG.Device_ProductNumber_Start, (ushort)buf.Length, buf);
            }
        }

        /// <summary>
        /// 产品型号
        /// </summary>
        public string Device_ProductModel
        {
            get
            {
                string str = "";
                byte[] asc = new byte[48];
                char[] chars = new char[48];

                for (int i = 0; i < 24; i++)
                {
                    ushort tmp = _cache_identity_code_1.Buf[MBREG.Device_ProductModel_Start - MBREG.Address_Identity_Code_Start_1 + i];

                    if (tmp == 0)
                    {
                        break;
                    }
                    else
                    {
                        asc[i * 2] = (byte)(tmp & 0x00FF);
                        asc[i * 2 + 1] = (byte)((tmp & 0xFF00) >> 8);
                    }
                }

                Decoder d = Encoding.ASCII.GetDecoder();
                d.GetChars(asc, 0, asc.Length, chars, 0);

                foreach (char ch in chars)
                {
                    str += ch;
                }

                return str;
            }
        }

        /// <summary>
        /// 制造商
        /// </summary>
        public string Device_Manufacture
        {
            get
            {
                string str = "";
                byte[] asc = new byte[64];
                char[] chars = new char[64];

                for (int i = 0; i < 32; i++)
                {
                    ushort tmp = _cache_identity_code_1.Buf[MBREG.Device_Manufacture_Start - MBREG.Address_Identity_Code_Start_1 + i];

                    if (tmp == 0)
                    {
                        break;
                    }
                    else
                    {
                        asc[i * 2] = (byte)(tmp & 0x00FF);
                        asc[i * 2 + 1] = (byte)((tmp & 0xFF00) >> 8);
                    }
                }

                Decoder d = Encoding.Default.GetDecoder();
                d.GetChars(asc, 0, asc.Length, chars, 0);

                foreach (char ch in chars)
                {
                    str += ch;
                }

                return str;
            }
        }

        /// <summary>
        /// 硬件版本号
        /// </summary>
        public string Device_HW_Version
        {
            get
            {
                string str = "";
                byte[] asc = new byte[16];
                char[] chars = new char[16];

                for (int i = 0; i < 8; i++)
                {
                    ushort tmp = _cache_identity_code_2.Buf[MBREG.Device_HardwareVersion_Start - MBREG.Address_Identity_Code_Start_2 + i];

                    if (tmp == 0)
                    {
                        break;
                    }
                    else
                    {
                        asc[i * 2] = (byte)(tmp & 0x00FF);
                        asc[i * 2 + 1] = (byte)((tmp & 0xFF00) >> 8);
                    }
                }

                Decoder d = Encoding.ASCII.GetDecoder();
                d.GetChars(asc, 0, asc.Length, chars, 0);

                foreach (char ch in chars)
                {
                    str += ch;
                }

                return str;
            }
        }

        /// <summary>
        /// 软件版本号
        /// </summary>
        public string Device_SW_Version
        {
            get
            {
                string str = "";
                byte[] asc = new byte[16];
                char[] chars = new char[16];

                for (int i = 0; i < 8; i++)
                {
                    ushort tmp = _cache_identity_code_2.Buf[MBREG.Device_SoftwareVersion_Start - MBREG.Address_Identity_Code_Start_2 + i];

                    if (tmp == 0)
                    {
                        break;
                    }
                    else
                    {
                        asc[i * 2] = (byte)(tmp & 0x00FF);
                        asc[i * 2 + 1] = (byte)((tmp & 0xFF00) >> 8);
                    }
                }

                Decoder d = Encoding.ASCII.GetDecoder();
                d.GetChars(asc, 0, asc.Length, chars, 0);

                foreach (char ch in chars)
                {
                    str += ch;
                }

                return str;
            }
        }

        /// <summary>
        /// 额定电流规格
        /// </summary>
        public ushort In
        {
            get
            {
                return (ushort)_cache_identity_code_2.Buf[MBREG.In - MBREG.Address_Identity_Code_Start_2];
            }
            set
            {
                ushort[] subuf = new ushort[] { 0xD0D0, 0XC0DE };
                pSetRegister((ushort)MBREG.Super_Write, 2, subuf);
                ushort[] buf = new ushort[] { (ushort)value };
                pSetRegister((ushort)MBREG.In, 1, buf);
            }
        }
        #endregion

        #region 构造函数
        public IdentityValue()
        {
            SetCallBack += new AsyncCallback(pSetCallBack);
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 从设备读取参数
        /// </summary>
        public void GetRegistersAsync(int timeout)
        {
            if (Log.IsDebugEnabled) { Log.Debug("异步读取识别信息"); }

            if (GetRegisterAsync != null)
            {
                GetRegisterAsync((ushort)MBREG.Address_Identity_Code_Start_1, (ushort)_cache_identity_code_1.Buf.Length,
                            _cache_identity_code_1.Buf, timeout, null, this);
                GetRegisterAsync((ushort)MBREG.Address_Identity_Code_Start_2, (ushort)_cache_identity_code_2.Buf.Length,
                            _cache_identity_code_2.Buf, timeout, GetCallBack, this);
            }
            else
            {
                string str = this.ToString() + ".GetRegisterAsync has no provider";
                Log.Error(str);
                throw new Exception(str);
            }
        }

        public void GetRegisters(int timeout)
        {
            if (Log.IsDebugEnabled) { Log.Debug("同步读取识别信息"); }

            if (GetRegister != null)
            {
                GetRegister((ushort)MBREG.Address_Identity_Code_Start_1, (ushort)_cache_identity_code_1.Buf.Length,
                            _cache_identity_code_1.Buf, timeout);
                GetRegister((ushort)MBREG.Address_Identity_Code_Start_2, (ushort)_cache_identity_code_2.Buf.Length,
                            _cache_identity_code_2.Buf, timeout);
            }
            else
            {
                string str = this.ToString() + ".GetRegister has no provider";
                Log.Error(str);
                throw new Exception(str);
            }
        }
        #endregion

        #region 保护方法
        /// <summary>
        /// 写寄存器（同步）
        /// </summary>
        private void pSetRegister(ushort start_ad, ushort nums, ushort[] buf)
        {
            if (Log.IsDebugEnabled) { Log.DebugFormat("同步写入识别参数({0})=[{1}]", start_ad, buf[0]); }

            if (SetRegister != null)
            {
                SetRegister(start_ad, nums, buf, -1);
            }
            else
            {
                string str = this.ToString() + ".SetRegisterAsync has no provider";
                Log.Error(str);
                throw new Exception(str);
            }
        }

        private void pSetCallBack(IAsyncResult ar)
        {
            GetRegisters((int)Timeout.Never);
        }

        #endregion

        #region 保护字段

        #region modbus 寄存器定义
        private enum MBREG : ushort
        {
            Super_Write = 0xFE,
            Super_Command = 0xFF,

            Address_Identity_Code_Start_1 = 0x6E,

            Device_ProductNumber_Start = 0x6E,
            Device_ProductModel_Start = 0x76,
            Device_Manufacture_Start = 0x7E,

            Address_Identity_Code_End_1 = 0x85,
            Address_Identity_Code_Start_2 = 0x86,

            Device_HardwareVersion_Start = 0x86,
            Device_SoftwareVersion_Start = 0x8E,
            In = 0x96,

            Address_Identity_Code_End_2 = 0x96,
        }
        #endregion
        /// <summary>
        /// 日志
        /// </summary>
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(IdentityValue));

        /// <summary>
        /// 识别参数
        /// </summary>
        private DeviceCache _cache_identity_code_1
                            = new DeviceCache(MBREG.Address_Identity_Code_End_1 - MBREG.Address_Identity_Code_Start_1 + 1);

        private DeviceCache _cache_identity_code_2
                            = new DeviceCache(MBREG.Address_Identity_Code_End_2 - MBREG.Address_Identity_Code_Start_2 + 1);


        #endregion

    }
}
