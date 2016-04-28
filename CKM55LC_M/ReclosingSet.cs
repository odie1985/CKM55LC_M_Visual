using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Devices.MCCB
{
    public class ReclosingSet
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
        /// 读寄存器事件
        /// </summary>
        public event GetSetRegisterAsyncHandler GetRegisterAsync;
        public event GetSetRegisterHandler GetRegister;
        public event GetSetRegisterAsyncHandler SetRegisterAsync;
        #endregion

        #region 属性
        /// <summary>
        /// 重合闸使能
        /// </summary>
        public List<string> Reclosing_Enable
        {
            get
            {
                uint tmp;
                tmp = _cache_reclosing_set.Buf[MBREG.Reclosing_Enable - MBREG.Address_Reclosing_Set_Start];

                return Utilities.BitCodeToStringList(Utilities.Str_Reclosing_Enable, tmp);
            }
            set
            {
                ushort[] buf = new ushort[2];
                buf = Utilities.StringListToBitCode(Utilities.Str_Reclosing_Enable, value);
                pSetRegisterAsync((ushort)MBREG.Reclosing_Enable, 2, buf);
            }
        }

        /// <summary>
        /// 剩余电流重合闸延时（s）
        /// </summary>
        public ushort Residual_Reclosing_Delay
        {
            get
            {
                return _cache_reclosing_set.Buf[MBREG.Residual_Reclosing_Delay - MBREG.Address_Reclosing_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.Residual_Reclosing_Delay, 1, buf);
            }
        }

        /// <summary>
        /// 剩余电流30min闭锁次数
        /// </summary>
        public ushort Residual_30min_Lock
        {
            get
            {
                return _cache_reclosing_set.Buf[MBREG.Residual_30min_Lock - MBREG.Address_Reclosing_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.Residual_30min_Lock, 1, buf);
            }
        }

        /// <summary>
        /// 过压重合闸电压值（V）
        /// </summary>
        public ushort OverVoltage_Reclosing_Level
        {
            get
            {
                return _cache_reclosing_set.Buf[MBREG.OverVoltage_Reclosing_Level - MBREG.Address_Reclosing_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.OverVoltage_Reclosing_Level, 1, buf);
            }
        }

        /// <summary>
        /// 过压重合闸延时（s）
        /// </summary>
        public ushort OverVoltage_Reclosing_Delay
        {
            get
            {
                return _cache_reclosing_set.Buf[MBREG.OverVoltage_Reclosing_Delay - MBREG.Address_Reclosing_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.OverVoltage_Reclosing_Delay, 1, buf);
            }
        }

        /// <summary>
        /// 欠压重合闸电压值（V）
        /// </summary>
        public ushort UnderVoltage_Reclosing_Level
        {
            get
            {
                return _cache_reclosing_set.Buf[MBREG.UnderVoltage_Reclosing_Level - MBREG.Address_Reclosing_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.UnderVoltage_Reclosing_Level, 1, buf);
            }
        }

        /// <summary>
        /// 欠压重合闸延时（s）
        /// </summary>
        public ushort UnderVoltage_Reclosing_Delay
        {
            get
            {
                return _cache_reclosing_set.Buf[MBREG.UnderVoltage_Reclosing_Delay - MBREG.Address_Reclosing_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.UnderVoltage_Reclosing_Delay, 1, buf);
            }
        }

        #endregion

        #region 构造函数
        #endregion

        #region 公有方法
        /// <summary>
        /// 从设备读取参数
        /// </summary>
        public void GetRegistersAsync(int timeout)
        {
            if (Log.IsDebugEnabled) { Log.Debug("异步读取运行参数"); }

            if (GetRegisterAsync != null)
            {
                GetRegisterAsync((ushort)MBREG.Address_Reclosing_Set_Start, (ushort)_cache_reclosing_set.Buf.Length,
                            _cache_reclosing_set.Buf, timeout, GetCallBack, this);
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
            if (Log.IsDebugEnabled) { Log.Debug("同步读取运行参数"); }

            if (GetRegister != null)
            {
                GetRegister((ushort)MBREG.Address_Reclosing_Set_Start, (ushort)_cache_reclosing_set.Buf.Length,
                            _cache_reclosing_set.Buf, timeout);
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
        private void pSetRegisterAsync(ushort start_ad, ushort nums, ushort[] buf)
        {
            if (Log.IsDebugEnabled)
            {
                Log.DebugFormat("异步写入重合闸参数({0})=({1})", start_ad, buf[0]);
            }

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
        #endregion

        #region 保护字段

        #region modbus 寄存器定义
        private enum MBREG : ushort
        {
            Address_Reclosing_Set_Start = 0x46,

            Reclosing_Enable = 0x46,
            Residual_Reclosing_Delay = 0x47,
            Residual_30min_Lock = 0x48,

            OverVoltage_Reclosing_Level = 0x49,
            OverVoltage_Reclosing_Delay = 0x4A,
            UnderVoltage_Reclosing_Level = 0x4B,
            UnderVoltage_Reclosing_Delay = 0x4C,

            Address_Reclosing_Set_End = 0x4C,
        }
        #endregion
        /// <summary>
        /// 日志
        /// </summary>
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(ReclosingSet));

        /// <summary>
        /// 测量参数
        /// </summary>
        private DeviceCache _cache_reclosing_set
            = new DeviceCache(MBREG.Address_Reclosing_Set_End - MBREG.Address_Reclosing_Set_Start + 1);


        #endregion

    }
}
