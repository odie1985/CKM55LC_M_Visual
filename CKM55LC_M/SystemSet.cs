using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using log4net;

namespace Devices.MCCB
{
    /// <summary>
    /// 系统参数类
    /// </summary>
    public class SystemSet
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
        /// 键盘锁定密码
        /// </summary>
        public ushort Keyboard_Password
        {
            get
            {
                return _cache_system_set.Buf[MBREG.Keyboard_Password - MBREG.Address_System_Set_Start];
            }
            set
            {
                ushort[] buf = new ushort[] { value };
                pSetRegisterAsync((ushort)MBREG.Keyboard_Password, 1, buf);
            }
        }

        #endregion

        #region 构造函数
        public SystemSet()
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
            if (Log.IsDebugEnabled) { Log.Debug("异步读取系统参数"); }

            if (GetRegisterAsync != null)
            {
                GetRegisterAsync((ushort)MBREG.Address_System_Set_Start, (ushort)_cache_system_set.Buf.Length,
                            _cache_system_set.Buf, timeout, GetCallBack, this);
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
            if (Log.IsDebugEnabled) { Log.Debug("同步读取系统参数"); }

            if (GetRegister != null)
            {
                GetRegister((ushort)MBREG.Address_System_Set_Start, (ushort)_cache_system_set.Buf.Length,
                            _cache_system_set.Buf, timeout);
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
            if (Log.IsDebugEnabled) { Log.DebugFormat("异步写入系统参数({0})=[{1}]", start_ad, buf[0]); }

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
        private enum MBREG : ushort
        {
            Address_System_Set_Start = 0x5C,

            Communication_Keyboard_Enable = 0x5C,
            Keyboard_Password = 0x5D,
            Display_Parameter = 0x5E,
            Now_Time_L = 0x5F,
            Now_Time_M = 0x60,
            Now_Time_H = 0x61,
            System_Operate = 0x62,

            Address_System_Set_End = 0x62,
        }
        #endregion
        /// <summary>
        /// 日志
        /// </summary>
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(SystemSet));

        /// <summary>
        /// 系统参数
        /// </summary>
        private DeviceCache _cache_system_set
            = new DeviceCache(MBREG.Address_System_Set_End - MBREG.Address_System_Set_Start + 1);


        #endregion

    }
}
