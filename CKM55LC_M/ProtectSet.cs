using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using log4net;

namespace Devices.MCCB
{
    /// <summary>
    /// 保护参数类
    /// </summary>
    public class ProtectSet
    {
        #region 公有字段

        #endregion

        #region 事件定义
        /// <summary>
        /// 读写寄存器回调事件
        /// </summary>
        public event AsyncCallback GetCallBack;
        public event AsyncCallback SetCallBack;

        /// <summary>
        /// 读写寄存器事件
        /// </summary>
        public event GetSetRegisterAsyncHandler GetRegisterAsync;
        public event GetSetRegisterAsyncHandler SetRegisterAsync;
        public event GetSetRegisterHandler GetRegister;
        #endregion

        #region 属性
        /// <summary>
        /// 保护使能
        /// </summary>
        public List<string> Protect_Enable
        {
            get
            {
                uint tmp;
                tmp = _cache_protect_set.Buf[MBREG.Protect_Enable - MBREG.Address_Protect_Set_Start];

                return Utilities.BitCodeToStringList(Utilities.Str_Protect_Enable, tmp);
            }
            set
            {
                ushort[] buf=new ushort[2];
                buf=Utilities.StringListToBitCode(Utilities.Str_Protect_Enable,value);
                pSetRegisterAsync((ushort)MBREG.Protect_Enable,2,buf);
            }
        }

        /// <summary>
        /// 短路保护方式
        /// </summary>
        public string Protect_ShortCircuit
        {
            get
            {
                uint tmp;
                tmp = _cache_protect_set.Buf[MBREG.Protect_Mode_L - MBREG.Address_Protect_Set_Start];
                tmp &= 0x03;
                return Utilities.Str_Protect_Mode[tmp];
            }
            set
            {
                uint i = Utilities.StringToBitCode(value);

                ushort tmp = _cache_protect_set.Buf[MBREG.Protect_Mode_L - MBREG.Address_Protect_Set_Start];
                tmp &= 0xFFFC;

                ushort[] buf = new ushort[2];

                buf[0] = (ushort)(i | tmp);

                pSetRegisterAsync((ushort)MBREG.Protect_Mode_L, 2, buf);
            }
        }

        /// <summary>
        /// 过电流保护方式
        /// </summary>
        public string Protect_OverCurrent
        {
            get
            {
                uint tmp;
                tmp = _cache_protect_set.Buf[MBREG.Protect_Mode_L - MBREG.Address_Protect_Set_Start];
                tmp = (tmp >> 2) & 0x03;
                return Utilities.Str_Protect_Mode[tmp];
            }
            set
            {
                uint i = Utilities.StringToBitCode(value);

                ushort tmp = _cache_protect_set.Buf[MBREG.Protect_Mode_L - MBREG.Address_Protect_Set_Start];
                tmp &= 0xFFFC;

                ushort[] buf = new ushort[2];

                buf[0] = (ushort)((i << 2) | tmp);

                pSetRegisterAsync((ushort)MBREG.Protect_Mode_L, 2, buf);
            }
        }

        /// <summary>
        /// 过载保护方式
        /// </summary>
        public string Protect_Overload
        {
            get
            {
                uint tmp;
                tmp = _cache_protect_set.Buf[MBREG.Protect_Mode_L - MBREG.Address_Protect_Set_Start];
                tmp = (tmp >> 4) & 0x03;
                return Utilities.Str_Protect_Mode[tmp];
            }
            set
            {
                uint i = Utilities.StringToBitCode(value);

                ushort tmp = _cache_protect_set.Buf[MBREG.Protect_Mode_L - MBREG.Address_Protect_Set_Start];
                tmp &= 0xFFFC;

                ushort[] buf = new ushort[2];

                buf[0] = (ushort)((i << 4) | tmp);

                pSetRegisterAsync((ushort)MBREG.Protect_Mode_L, 2, buf);
            }
        }

        /// <summary>
        /// 剩余电流保护方式
        /// </summary>
        public string Protect_ResidualCurrent
        {
            get
            {
                uint tmp;
                tmp = _cache_protect_set.Buf[MBREG.Protect_Mode_L - MBREG.Address_Protect_Set_Start];
                tmp = (tmp >> 6) & 0x03;
                return Utilities.Str_Protect_Mode[tmp];
            }
            set
            {
                uint i = Utilities.StringToBitCode(value);

                ushort tmp = _cache_protect_set.Buf[MBREG.Protect_Mode_L - MBREG.Address_Protect_Set_Start];
                tmp &= 0xFFFC;

                ushort[] buf = new ushort[2];

                buf[0] = (ushort)((i << 6) | tmp);

                pSetRegisterAsync((ushort)MBREG.Protect_Mode_L, 2, buf);
            }
        }

        /// <summary>
        /// 不平衡电流保护方式
        /// </summary>
        public string Protect_UnbalancedCurrent
        {
            get
            {
                uint tmp;
                tmp = _cache_protect_set.Buf[MBREG.Protect_Mode_L - MBREG.Address_Protect_Set_Start];
                tmp = (tmp >> 8) & 0x03;
                return Utilities.Str_Protect_Mode[tmp];
            }
            set
            {
                uint i = Utilities.StringToBitCode(value);
                ushort tmp = _cache_protect_set.Buf[MBREG.Protect_Mode_L - MBREG.Address_Protect_Set_Start];
                tmp &= 0xFFFC;

                ushort[] buf = new ushort[2];

                buf[0] = (ushort)((i << 8) | tmp);

                pSetRegisterAsync((ushort)MBREG.Protect_Mode_L, 2, buf);
            }
        }

        /// <summary>
        /// 断相保护方式
        /// </summary>
        public string Protect_PhaseLost
        {
            get
            {
                uint tmp;
                tmp = _cache_protect_set.Buf[MBREG.Protect_Mode_L - MBREG.Address_Protect_Set_Start];
                tmp = (tmp >> 10) & 0x03;
                return Utilities.Str_Protect_Mode[tmp];
            }
            set
            {
                uint i = Utilities.StringToBitCode(value);

                ushort tmp = _cache_protect_set.Buf[MBREG.Protect_Mode_L - MBREG.Address_Protect_Set_Start];
                tmp &= 0xFFFC;

                ushort[] buf = new ushort[2];

                buf[0] = (ushort)((i << 10) | tmp);

                pSetRegisterAsync((ushort)MBREG.Protect_Mode_L, 2, buf);
            }
        }

        /// <summary>
        /// 断零保护方式
        /// </summary>
        public string Protect_NLost
        {
            get
            {
                uint tmp;
                tmp = _cache_protect_set.Buf[MBREG.Protect_Mode_L - MBREG.Address_Protect_Set_Start];
                tmp = (tmp >> 12) & 0x03;
                return Utilities.Str_Protect_Mode[tmp];
            }
            set
            {
                uint i = Utilities.StringToBitCode(value);

                ushort tmp = _cache_protect_set.Buf[MBREG.Protect_Mode_L - MBREG.Address_Protect_Set_Start];
                tmp &= 0xFFFC;

                ushort[] buf = new ushort[2];

                buf[0] = (ushort)((i << 12) | tmp);

                pSetRegisterAsync((ushort)MBREG.Protect_Mode_L, 2, buf);
            }
        }

        /// <summary>
        /// 过电压保护方式
        /// </summary>
        public string Protect_OverVoltage
        {
            get
            {
                uint tmp;
                tmp = _cache_protect_set.Buf[MBREG.Protect_Mode_L - MBREG.Address_Protect_Set_Start];
                tmp = (tmp >> 14) & 0x03;
                return Utilities.Str_Protect_Mode[tmp];
            }
            set
            {
                uint i = Utilities.StringToBitCode(value);
                ushort tmp = _cache_protect_set.Buf[MBREG.Protect_Mode_L - MBREG.Address_Protect_Set_Start];
                tmp &= 0xFFFC;

                ushort[] buf = new ushort[2];

                buf[0] = (ushort)((i << 14) | tmp);

                pSetRegisterAsync((ushort)MBREG.Protect_Mode_L, 2, buf);
            }
        }

        /// <summary>
        /// 欠电压保护方式
        /// </summary>
        public string Protect_UnderVoltage
        {
            get
            {
                uint tmp;
                tmp = _cache_protect_set.Buf[MBREG.Protect_Mode_H - MBREG.Address_Protect_Set_Start];
                tmp = tmp & 0x03;
                return Utilities.Str_Protect_Mode[tmp];
            }
            set
            {
                uint i = Utilities.StringToBitCode(value);

                ushort tmp = _cache_protect_set.Buf[MBREG.Protect_Mode_L - MBREG.Address_Protect_Set_Start];
                tmp &= 0xFFFC;

                ushort[] buf = new ushort[2];

                buf[0] = (ushort)(i | tmp);

                pSetRegisterAsync((ushort)MBREG.Protect_Mode_L, 2, buf);
            }
        }

        /// <summary>
        /// 允许复位热容（%）
        /// </summary>
        public ushort Reset_Enable_Heat_Level
        {
            get
            {
                return _cache_protect_set.Buf[MBREG.Reset_Enable_Heat_Level - MBREG.Address_Protect_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.Reset_Enable_Heat_Level, 1, buf);
            }
        }

        /// <summary>
        /// 允许复位延时（s）
        /// </summary>
        public ushort Reset_Enable_Heat_Delay
        {
            get
            {
                return _cache_protect_set.Buf[MBREG.Reset_Enable_Heat_Delay - MBREG.Address_Protect_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.Reset_Enable_Heat_Delay, 1, buf);
            }
        }

        /// <summary>
        /// 长延时整定电流Ir1（A）
        /// </summary>
        public ushort LongDelay_Ir1
        {
            get
            {
                return _cache_protect_set.Buf[MBREG.LongDelay_Ir1 - MBREG.Address_Protect_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.LongDelay_Ir1, 1, buf);
            }
        }

        /// <summary>
        /// 长延时整定时间t1（s）
        /// </summary>
        public ushort LongDelay_t1
        {
            get
            {
                return _cache_protect_set.Buf[MBREG.LongDelay_t1 - MBREG.Address_Protect_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { (ushort)value };
                pSetRegisterAsync((ushort)MBREG.LongDelay_t1, 1, buf);
            }
        }

        /// <summary>
        ///  长延时预报警动作值（%）
        /// </summary>
        public ushort LongDelay_Warning_Level
        {
            get
            {
                return _cache_protect_set.Buf[MBREG.LongDelay_Warning_Level - MBREG.Address_Protect_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.LongDelay_Warning_Level, 1, buf);
            }
        }

        /// <summary>
        /// 短延时整定电流Ir2（A）
        /// </summary>
        public ushort ShortDelay_Ir2
        {
            get
            {
                return _cache_protect_set.Buf[MBREG.ShortDelay_Ir2 - MBREG.Address_Protect_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.ShortDelay_Ir2, 1, buf);
            }
        }

        /// <summary>
        /// 短延时整定时间t2（s）
        /// </summary>
        public float ShortDelay_Time_t2
        {
            get
            {
                ushort tmp = _cache_protect_set.Buf[MBREG.ShortDelay_Time_t2 - MBREG.Address_Protect_Set_Start];
                return (float)Math.Round(tmp / 100F, 1);
            }
            set
            {
                ushort[] buf = new ushort[] { (ushort)(value * 100) };
                pSetRegisterAsync((ushort)MBREG.ShortDelay_Time_t2, 1, buf);
            }
        }

        /// <summary>
        /// 瞬时整定电流Ir3（A）
        /// </summary>
        public ushort Instant_Current_Ir3
        {
            get
            {
                return _cache_protect_set.Buf[MBREG.Instant_Current_Ir3 - MBREG.Address_Protect_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.Instant_Current_Ir3, 1, buf);
            }
        }
        
        /// <summary>
        /// 剩余电流动作值（mA）
        /// </summary>
        public ushort Residual_Fault_Level
        {
            get
            {
                return _cache_protect_set.Buf[MBREG.Residual_Fault_Level - MBREG.Address_Protect_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.Residual_Fault_Level, 1, buf);
            }
        }

        /// <summary>
        /// 剩余电流延时时间（s）
        /// </summary>
        public float Residual_Fault_Delay
        {
            get
            {
                ushort tmp = _cache_protect_set.Buf[MBREG.Residual_Fault_Delay - MBREG.Address_Protect_Set_Start];
                return (float)Math.Round(tmp / 100F, 1);
            }
            set
            {
                ushort[] buf = new ushort[] { (ushort)(value * 100) };
                pSetRegisterAsync((ushort)MBREG.Residual_Fault_Delay, 1, buf);
            }
        }

        /// <summary>
        /// 不平衡电流动作值（%）
        /// </summary>
        public ushort Imbalance_Fault_Level
        {
            get
            {
                return _cache_protect_set.Buf[MBREG.Imbalance_Fault_Level - MBREG.Address_Protect_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.Imbalance_Fault_Level, 1, buf);
            }
        }

        /// <summary>
        /// 不平衡电流延时时间（s）
        /// </summary>
        public ushort Imbalance_Fault_Delay
        {
            get
            {
                return _cache_protect_set.Buf[MBREG.Imbalance_Fault_Delay - MBREG.Address_Protect_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.Imbalance_Fault_Delay, 1, buf);
            }
        }

        /// <summary>
        /// 缺相电流动作值（A）
        /// </summary>
        public ushort Phase_Fault_Level
        {
            get
            {
                return _cache_protect_set.Buf[MBREG.Phase_Fault_Level - MBREG.Address_Protect_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.Phase_Fault_Level, 1, buf);
            }
        }

        /// <summary>
        /// 缺相延时时间（s）
        /// </summary>
        public ushort Phase_Fault_Delay
        {
            get
            {
                return _cache_protect_set.Buf[MBREG.Phase_Fault_Delay - MBREG.Address_Protect_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.Phase_Fault_Delay, 1, buf);
            }
        }

        /// <summary>
        /// 断零延时时间（s）
        /// </summary>
        public ushort Zero_Fault_Delay
        {
            get
            {
                return _cache_protect_set.Buf[MBREG.Zero_Fault_Delay - MBREG.Address_Protect_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.Zero_Fault_Delay, 1, buf);
            }
        }

        /// <summary>
        /// 过压动作值（V）
        /// </summary>
        public ushort OverVoltage_Fault_Level
        {
            get
            {
                return _cache_protect_set.Buf[MBREG.OverVoltage_Fault_Level - MBREG.Address_Protect_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.OverVoltage_Fault_Level, 1, buf);
            }
        }

        /// <summary>
        /// 过压延时时间（s）
        /// </summary>
        public ushort OverVoltage_Fault_Delay
        {
            get
            {
                return _cache_protect_set.Buf[MBREG.OverVoltage_Fault_Delay - MBREG.Address_Protect_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.OverVoltage_Fault_Delay, 1, buf);
            }
        }

        /// <summary>
        /// 欠压动作值（V）
        /// </summary>
        public ushort UnderVoltage_Fault_Level
        {
            get
            {
                return _cache_protect_set.Buf[MBREG.UnderVoltage_Fault_Level - MBREG.Address_Protect_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.UnderVoltage_Fault_Level, 1, buf);
            }
        }

        /// <summary>
        /// 欠压延时时间（s）
        /// </summary>
        public ushort UnderVoltage_Fault_Delay
        {
            get
            {
                return _cache_protect_set.Buf[MBREG.UnderVoltage_Fault_Delay - MBREG.Address_Protect_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.UnderVoltage_Fault_Delay, 1, buf);
            }
        }


        #endregion

        #region 构造函数
        public ProtectSet()
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
            if (Log.IsDebugEnabled) { Log.Debug("异步读取保护参数"); }

            if (GetRegisterAsync != null)
            {
                GetRegisterAsync((ushort)MBREG.Address_Protect_Set_Start, (ushort)_cache_protect_set.Buf.Length,
                            _cache_protect_set.Buf, timeout, GetCallBack, this);
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
            if (Log.IsDebugEnabled) { Log.Debug("同步读取保护参数"); }

            if (GetRegister != null)
            {
                GetRegister((ushort)MBREG.Address_Protect_Set_Start, (ushort)_cache_protect_set.Buf.Length,
                            _cache_protect_set.Buf, timeout);
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
        /// 写寄存器（异步）
        /// </summary>
        private void pSetRegisterAsync(ushort start_ad, ushort nums, ushort[] buf)
        {
            if (Log.IsDebugEnabled) { Log.DebugFormat("异步写入保护参数({0})=[{1}]", start_ad, buf[0]); }

            if (SetRegisterAsync != null)
            {
                SetRegisterAsync(start_ad, nums, buf, -1, SetCallBack, this);
            }
            else
            {
                string str = this.ToString() + ".SetRegisterAsync has no provider";
                Log.Error(str);
                throw new Exception(str);
            }
        }

        /// <summary>
        /// 设置寄存器回调函数，用于读回寄存器值
        /// </summary>
        private void pSetCallBack(IAsyncResult ar)
        {
            GetRegisters((int)Timeout.Never);
        }
        #endregion

        #region 保护字段

        #region modbus 寄存器定义
        /// <summary>
        /// modbus 寄存器定义
        /// </summary>
        private enum MBREG : ushort
        {
            Address_Protect_Set_Start = 0x24,

            Protect_Enable = 0x24,
            Protect_Mode_L = 0x25,
            Protect_Mode_H = 0x26,
            Reset_Enable_Heat_Level = 0x27,
            Reset_Enable_Heat_Delay = 0x28,

            LongDelay_Ir1 = 0x29,
            LongDelay_t1 = 0x2A,
            LongDelay_Warning_Level = 0x2B,

            ShortDelay_Ir2 = 0x2C,
            ShortDelay_Time_t2 = 0x2D,
            Instant_Current_Ir3 = 0x2E,

            Residual_Fault_Level = 0x2F,
            Residual_Fault_Delay = 0x30,
            Imbalance_Fault_Level = 0x31,
            Imbalance_Fault_Delay = 0x32,

            Phase_Fault_Level = 0x33,
            Phase_Fault_Delay = 0x34,
            Zero_Fault_Delay = 0x35,

            OverVoltage_Fault_Level = 0x36,
            OverVoltage_Fault_Delay = 0x37,
            UnderVoltage_Fault_Level = 0x38,
            UnderVoltage_Fault_Delay = 0x39,

            Address_Protect_Set_End = 0x39,
        }
        #endregion
        /// <summary>
        /// 日志
        /// </summary>
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(ProtectSet));

        /// <summary>
        /// 保护参数
        /// </summary>
        private DeviceCache _cache_protect_set
            = new DeviceCache(MBREG.Address_Protect_Set_End - MBREG.Address_Protect_Set_Start + 1);


        #endregion

    }
}
