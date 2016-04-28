using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Devices.MCCB
{
    public class FactorySet
    {
        #region 公有字段
        #endregion

        #region 事件定义
        /// <summary>
        /// 读寄存器回调事件
        /// </summary>
        public event AsyncCallback GetCallBack;

        /// <summary>
        /// 读寄存器事件
        /// </summary>
        public event GetSetRegisterAsyncHandler GetRegisterAsync;
        public event GetSetRegisterHandler GetRegister;
        #endregion

        #region 属性

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
                //GetRegisterAsync((ushort)MBREG.Address_Run_Value_Start, (ushort)_cache_run_value.Buf.Length,
                //            _cache_run_value.Buf, timeout, GetCallBack, this);
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
                //GetRegister((ushort)MBREG.Address_Run_Value_Start, (ushort)_cache_run_value.Buf.Length,
                //            _cache_run_value.Buf, timeout);
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

        }
        #endregion
        /// <summary>
        /// 日志
        /// </summary>
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(RunValue));

        /// <summary>
        /// 测量参数
        /// </summary>
        //private DeviceCache _cache_run_value
        //    = new DeviceCache(MBREG.Address_Run_Value_End - MBREG.Address_Run_Value_Start + 1);


        #endregion

    }
}
