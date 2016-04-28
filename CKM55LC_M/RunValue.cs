using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using log4net;

namespace Devices.MCCB
{
    public class RunValue
    {
        #region 公有字段

        #endregion

        #region 事件定义
        /// <summary>
        /// 读寄存器回调事件
        /// </summary>
        public event AsyncCallback GetCallBack;

        /// <summary>
        /// 读取寄存器事件
        /// </summary>
        public event GetSetRegisterAsyncHandler GetRegisterAsync;
        public event GetSetRegisterHandler GetRegister;
        #endregion

        #region 属性
        /// <summary>
        /// A相电流（A）
        /// </summary>
        public float Current_A
        {
            get
            {
                return _cache_run_value.Buf[MBREG.Current_A - MBREG.Address_Run_Value_Start];                
            }
        }

        /// <summary>
        /// B相电流（A）
        /// </summary>
        public float Current_B
        {
            get
            {
                return _cache_run_value.Buf[MBREG.Current_B - MBREG.Address_Run_Value_Start];
            }
        }

        /// <summary>
        /// C相电流（A）
        /// </summary>
        public float Current_C
        {
            get
            {
                return _cache_run_value.Buf[MBREG.Current_C - MBREG.Address_Run_Value_Start];
            }
        }

        /// <summary>
        /// 剩余电流（A）
        /// </summary>
        public ushort Current_Residual
        {
            get
            {
                return _cache_run_value.Buf[MBREG.Current_Residual - MBREG.Address_Run_Value_Start];
            }
        }

        /// <summary>
        /// 电流不平衡度（%）
        /// </summary>
        public ushort Ratio_Current_Imbalance
        {
            get
            {
                return _cache_run_value.Buf[MBREG.Ratio_Current_Imbalance - MBREG.Address_Run_Value_Start];
            }
        }

        /// <summary>
        /// A相电压（V）
        /// </summary>
        public ushort Voltage_A
        {
            get
            {
                return _cache_run_value.Buf[MBREG.Voltage_A - MBREG.Address_Run_Value_Start];
            }
        }

        /// <summary>
        /// B相电压（V）
        /// </summary>
        public ushort Voltage_B
        {
            get
            {
                return _cache_run_value.Buf[MBREG.Voltage_B - MBREG.Address_Run_Value_Start];
            }
        }

        /// <summary>
        /// C相电压（V）
        /// </summary>
        public ushort Voltage_C
        {
            get
            {
                return _cache_run_value.Buf[MBREG.Voltage_C - MBREG.Address_Run_Value_Start];
            }
        }

        /// <summary>
        /// 热熔比（‰）
        /// </summary>
        public ushort Ratio_Heat
        {
            get
            {
                return _cache_run_value.Buf[MBREG.Ratio_Heat - MBREG.Address_Run_Value_Start];
            }
        }

        /// <summary>
        /// 内部温度（℃）
        /// </summary>
        public float Temperature
        {
            get
            {
                ushort tmp = _cache_run_value.Buf[MBREG.Temperature - MBREG.Address_Run_Value_Start];
                return (float)Math.Round(tmp / 10D, 1);
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
                GetRegisterAsync((ushort)MBREG.Address_Run_Value_Start, (ushort)_cache_run_value.Buf.Length,
                            _cache_run_value.Buf, timeout, GetCallBack, this);
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
                GetRegister((ushort)MBREG.Address_Run_Value_Start, (ushort)_cache_run_value.Buf.Length,
                            _cache_run_value.Buf, timeout);
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

        #endregion

        #region 保护字段

        #region modbus 寄存器定义
        private enum MBREG : ushort
        {
            Address_Run_Value_Start=0x0A,

            Current_A = 0x0A,
            Current_B = 0x0B,
            Current_C = 0x0C,
            Current_Residual = 0x0D,
            Ratio_Current_Imbalance = 0x0E,

            Voltage_A = 0x0F,
            Voltage_B = 0x10,
            Voltage_C = 0x11,

            Ratio_Heat = 0x12,
            Temperature = 0x13,

            Address_Run_Value_End = 0x13,
        }
        #endregion
        /// <summary>
        /// 日志
        /// </summary>
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(RunValue));

        /// <summary>
        /// 测量参数
        /// </summary>
        private DeviceCache _cache_run_value
            = new DeviceCache(MBREG.Address_Run_Value_End - MBREG.Address_Run_Value_Start + 1);
            

        #endregion
    }
}
