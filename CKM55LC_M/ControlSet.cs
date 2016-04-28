using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using log4net;

namespace Devices.MCCB
{
    /// <summary>
    /// 控制参数类
    /// </summary>
    public class ControlSet
    {
        #region 公有字段

        #endregion

        #region 事件定义
        /// <summary>
        /// 读寄存器回调事件
        /// </summary>
        public event AsyncCallback SetCallBack;

        /// <summary>
        /// 读寄存器事件
        /// </summary>
        public event GetSetRegisterAsyncHandler SetRegisterAsync;
        #endregion

        #region 属性

        #endregion

        #region 构造函数

        #endregion

        #region 公有方法
        #region 马达控制参数(方法)
        /// <summary>
        /// 合闸
        /// </summary>
        public void Open(AsyncCallback callback)
        {
            ushort[] buf = new ushort[] { 0x01 };
            pSetRegisterAsync((ushort)MBREG.Control_Run, 1, buf,
                callback, this);
        }

        /// <summary>
        /// 分闸
        /// </summary>
        public void Close(AsyncCallback callback)
        {
            ushort[] buf = new ushort[] { 0x02 };
            pSetRegisterAsync((ushort)MBREG.Control_Run, 1, buf,
                callback, this);
        }

        /// <summary>
        /// 复位
        /// </summary>
        public void Reset(AsyncCallback callback)
        {
            ushort[] buf = new ushort[] { 0x04 };
            pSetRegisterAsync((ushort)MBREG.Control_Run, 1, buf,
                callback, this);
        }

        /// <summary>
        /// 试验
        /// </summary>
        public void Test(AsyncCallback callback)
        {
            ushort[] buf = new ushort[] { 0x08 };
            pSetRegisterAsync((ushort)MBREG.Control_Run, 1, buf,
                callback, this);
        }

        /// <summary>
        /// 清除热容
        /// </summary>
        public void ResetQ(AsyncCallback callback)
        {
            ushort[] buf = new ushort[] { 0x10 };
            pSetRegisterAsync((ushort)MBREG.Control_Run, 1, buf,
                callback, this);
        }
        #endregion

        #endregion

        #region 保护方法
        /// <summary>
        /// 写寄存器（异步）
        /// </summary>
        private void pSetRegisterAsync(ushort start_ad, ushort nums, ushort[] buf, AsyncCallback callback, object @object)
        {
            if (Log.IsDebugEnabled) { Log.DebugFormat("异步写入控制参数({0})=[{1}]", start_ad, buf[0]); }

            if (SetRegisterAsync != null)
            {
                SetRegisterAsync(start_ad, nums, buf, -1, callback, @object);
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
        /// <summary>
        /// modbus 寄存器定义
        /// </summary>
        private enum MBREG : ushort
        {
            Address_Control_Value_Start = 0x20,

            Control_Run = 0x20,

            Address_Control_Value_End = 0x20,
        }
        #endregion
        /// <summary>
        /// 日志
        /// </summary>
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(ControlSet));

        /// <summary>
        /// 控制参数
        /// </summary>
        private DeviceCache _cache_control_value
            = new DeviceCache(MBREG.Address_Control_Value_End - MBREG.Address_Control_Value_Start + 1);


        #endregion

    }
}
