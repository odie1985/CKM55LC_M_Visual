using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Common;
using Modbus_v2;
using Lines.Com;
using log4net;

namespace Devices.MCCB
{
    /// <summary>
    /// 读写寄存器方法委托类型
    /// </summary>
    public delegate void GetSetRegisterAsyncHandler(ushort start_ad, ushort nums, ushort[] buf, int timeout, AsyncCallback callback, object @object);
    public delegate void GetSetRegisterHandler(ushort start_ad, ushort nums, ushort[] buf, int timeout);

    /// <summary>
    /// 设备命令发出后等待总线空闲的超时时间
    /// </summary>
    public enum Timeout
    {
        Never = -1,
        Short = 2000,
        Middle = 5000,
        Long = 10000,
    }

    public class CKM55LC_M:Common.BaseDevice
    {
        #region 公有字段

        #endregion

        #region 事件定义
        /// <summary>
        /// 读写寄存器回调事件
        /// </summary>
        //public event AsyncCallback SetCallBack;
        //public event AsyncCallback GetCallBack;

        /// <summary>
        /// 设备通讯事件
        /// </summary> 
        public event DeviceEventHandler Online;
        public event DeviceEventHandler Offline;

        /// <summary>
        /// 供UI使用，获得Set操作结果
        /// 由于Get操作都为内部操作，故不提供给UI使用
        /// </summary>
        public event DeviceEventHandler SetResult;
        #endregion

        #region 属性
        /// <summary>
        /// 测量参数
        /// </summary>
        public RunValue RunValues
        {
            get
            {
                return _RunValues;
            }
        }

        /// <summary>
        /// 监视参数
        /// </summary>
        public MonitorValue MonitorValues
        {
            get
            {
                return _MonitorValues;
            }
        }

        /// <summary>
        /// 控制参数
        /// </summary>
        public ControlSet ControlSets
        {
            get
            {
                return _ControlSets;
            }
        }

        /// <summary>
        /// 诊断参数
        /// </summary>
        public DiagnosisValue DiagnosisValues
        {
            get
            {
                return _DiagnosisValues;
            }
        }
        /// <summary>
        /// 保护参数
        /// </summary>
        public ProtectSet ProtectSets
        {
            get
            {
                return _ProtectSets;
            }
        }

        /// <summary>
        /// 重合闸参数
        /// </summary>
        public ReclosingSet ReclosingSets
        {
            get
            {
                return _ReclosingSets;
            }
        }

        /// <summary>。
        /// 系统参数
        /// </summary>
        public SystemSet SystemSets
        {
            get
            {
                return _SystemSets;
            }
        }
        
        /// <summary>
        /// 识别参数
        /// </summary>
        public IdentityValue IdentityValues
        {
            get
            {
                return _IdentityValues;
            }
        }

        /// <summary>
        /// 出厂修正参数
        /// </summary>
        public CalibrationValue CalibrationValues
        {
            get
            {
                return _CalibrationValues;
            }
        }

        #endregion

        #region 构造函数
        public CKM55LC_M(byte index, BaseLine line)
            : base(index, "CKM55LC_M")
        {
            Line = line;
            _lock = new object();

            string str = Line.GetType().ToString();

            if (str == "Lines.Com.ComLine")
            {
                ComLine cl = (ComLine)line;

                _modbus = new Modbus(index);
                _modbus.InterFrameGap = cl.InterFrameGap;
                _modbus.CaptureLineArgs += new CaptureLineArgsEventHandler(cl.CaptureLine);
                _modbus.ReleaseLineArgs += new ReleaseLineArgsEventHandler(cl.ReleaseLine);
                _modbus.DiscardSendBuffer += new CommonEventHandler(cl.DiscardSendBuffer);
                _modbus.DiscardReceiveBuffer += new CommonEventHandler(cl.DiscardReceiveBuffer);
                _modbus.Send += new IOEventHandler(cl.Send);
                _modbus.Receive += new IOEventHandler(cl.Receive);

                if (Log.IsDebugEnabled) { Log.DebugFormat("初始化CKM55LC_M[{0}]绑定ComLine[{1}]", index, cl.Name); }
            }
            else if (str == "Lines.Ethernet.NetLine")
            {
            }
            else
            {
                Log.Error("unsupported Line type:" + str);
                throw new Exception("unsupported Line type:" + str);
            }

            //异步操作Handler对象初始化
            _SetRegisterHandle = new GetSetRegisterHandler(SetRegister);
            _GetRegisterHandle = new GetSetRegisterHandler(GetRegister);

            //各类寄存器对象初始化
            _RunValues = new RunValue();
            _RunValues.GetRegister += new GetSetRegisterHandler(GetRegister);
            _RunValues.GetRegisterAsync += new GetSetRegisterAsyncHandler(GetRegisterAsync);

            _MonitorValues = new MonitorValue();
            _MonitorValues.GetRegister += new GetSetRegisterHandler(GetRegister);
            _MonitorValues.GetRegisterAsync += new GetSetRegisterAsyncHandler(GetRegisterAsync);

            _ControlSets = new ControlSet();
            _ControlSets.SetRegisterAsync += new GetSetRegisterAsyncHandler(SetRegisterAsync);

            _DiagnosisValues = new DiagnosisValue();
            _DiagnosisValues.GetRegister += new GetSetRegisterHandler(GetRegister);
            _DiagnosisValues.GetRegisterAsync += new GetSetRegisterAsyncHandler(GetRegisterAsync);

            _ProtectSets = new ProtectSet();
            _ProtectSets.GetRegister += new GetSetRegisterHandler(GetRegister);
            _ProtectSets.GetRegisterAsync += new GetSetRegisterAsyncHandler(GetRegisterAsync);
            _ProtectSets.SetRegisterAsync += new GetSetRegisterAsyncHandler(SetRegisterAsync);

            _SystemSets = new SystemSet();
            _SystemSets.GetRegister += new GetSetRegisterHandler(GetRegister);
            _SystemSets.GetRegisterAsync += new GetSetRegisterAsyncHandler(GetRegisterAsync);
            _SystemSets.SetRegisterAsync += new GetSetRegisterAsyncHandler(SetRegisterAsync);

            _IdentityValues = new IdentityValue();
            _IdentityValues.GetRegister += new GetSetRegisterHandler(GetRegister);
            _IdentityValues.GetRegisterAsync += new GetSetRegisterAsyncHandler(GetRegisterAsync);
            _IdentityValues.SetRegister += new GetSetRegisterHandler(SetRegister);

            _CalibrationValues = new CalibrationValue();
            _CalibrationValues.GetRegister += new GetSetRegisterHandler(GetRegister);
            _CalibrationValues.GetRegisterAsync += new GetSetRegisterAsyncHandler(GetRegisterAsync);
            _CalibrationValues.SetRegisterAsync += new GetSetRegisterAsyncHandler(SetRegisterAsync);
            _CalibrationValues.SetRegister += new GetSetRegisterHandler(SetRegister);

            GetAllRegistersAsync();
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 异步读取所有寄存器
        /// 默认无等待总线超时时间
        /// </summary>
        public void GetAllRegistersAsync()
        {
            GetAllRegistersAsync((int)Timeout.Never);
        }

        /// <summary>
        /// 异步读取所有寄存器
        /// </summary>
        /// <param name="timeout">等待总线超时时间（ms）</param>
        public void GetAllRegistersAsync(Timeout timeout)
        {
            GetAllRegistersAsync((int)timeout);
        }

        /// <summary>
        /// 同步读取所有寄存器
        /// 默认无等待总线超时时间
        /// </summary>
        public void GetAllRegisters()
        {
            GetAllRegisters((int)Timeout.Never);
        }

        /// <summary>
        /// 同步读取所有寄存器
        /// </summary>
        /// <param name="timeout">等待总线超时时间（ms）</param>
        public void GetAllRegisters(Timeout timeout)
        {
            GetAllRegisters((int)timeout);
        }

        /// <summary>
        /// 提供给线路定时刷新的处理函数
        /// </summary>
        public void PeriodRefresh(object sender, ElapsedEventArgs e)
        {
            // 只实时刷新运行参数和监视参数
            _RunValues.GetRegistersAsync((int)Timeout.Short);
            _MonitorValues.GetRegistersAsync((int)Timeout.Short);
            //_DiagnosisValues.GetCounterRegistersAsync((int)Timeout.Short);
            // 监视参数应当有回调处理各种事件**********************************************

        }

        /// <summary>
        /// 提供给线路定时刷新的处理函数
        /// </summary>
        public void FactoryRefresh(object sender, ElapsedEventArgs e)
        {
            _RunValues.GetRegistersAsync((int)Timeout.Short);
            _MonitorValues.GetRegistersAsync((int)Timeout.Short);
            _IdentityValues.GetRegistersAsync((int)Timeout.Short);
            _CalibrationValues.GetRegistersAsync((int)Timeout.Short);
        }
        #endregion

        #region 保护方法
        /// <summary>
        /// 异步读取所有寄存器
        /// </summary>
        private void GetAllRegistersAsync(int timeout)
        {
            if (Log.IsDebugEnabled) { Log.DebugFormat("异步读取[{0}]所有寄存器,Timeout[{1}]", this.Index, timeout); }

            // 读取所有寄存器（异步方法）
            _RunValues.GetRegistersAsync(timeout);
            _MonitorValues.GetRegistersAsync(timeout);
            _DiagnosisValues.GetRegistersAsync(timeout);
            _ProtectSets.GetRegistersAsync(timeout);
            _SystemSets.GetRegistersAsync(timeout);
            _IdentityValues.GetRegistersAsync(timeout);
            _CalibrationValues.GetRegistersAsync(timeout);
        }

        /// <summary>
        /// 同步读取所有寄存器
        /// </summary>
        private void GetAllRegisters(int timeout)
        {
            if (Log.IsDebugEnabled) { Log.DebugFormat("同步读取[{0}]所有寄存器,Timeout[{1}]", this.Index, timeout); }

            // 读取所有寄存器（同步方法）
            _RunValues.GetRegisters(timeout);
            _MonitorValues.GetRegisters(timeout);
            _DiagnosisValues.GetRegisters(timeout);
            _ProtectSets.GetRegisters(timeout);
            _SystemSets.GetRegisters(timeout);
            _IdentityValues.GetRegisters(timeout);
            _CalibrationValues.GetRegisters(timeout);
        }

        #region 读写寄存器异步方法
        /// <summary>
        /// 设置寄存器
        /// </summary>
        private void SetRegisterAsync(ushort start_ad, ushort nums, ushort[] buf, int timeout, AsyncCallback callback, object @object)
        {
            _SetRegisterHandle.BeginInvoke(start_ad, nums, buf, timeout, callback, @object);
        }

        /// <summary>
        /// 设置寄存器
        /// </summary>
        private void GetRegisterAsync(ushort start_ad, ushort nums, ushort[] buf, int timeout, AsyncCallback callback, object @object)
        {
            _GetRegisterHandle.BeginInvoke(start_ad, nums, buf, timeout, callback, @object);
        }
        #endregion

        #region 读写寄存器同步方法
        /// <summary>
        /// 设置寄存器
        /// </summary>
        private void SetRegister(ushort start_ad, ushort nums, ushort[] buf, int timeout)
        {
            lock (_lock)
            {
                _modbus.StartAddress = start_ad;
                _modbus.Numbers = nums;
                _modbus.Values = buf;

                CountTotalSet++;

                try
                {
                    if (IsOffline == true)
                    {
                        // 重试0次
                        _modbus.WriteRegs(0, timeout);

                        if (IsOffline == true)
                        {
                            IsOffline = false;

                            Status = "SetRegister successful";
                            IsSetSuccess = true;

                            string str = string.Format("Device:[{0}][Online]" + Status, this.Index);

                            Log.Warn(str);

                            if (Online != null)
                            {
                                Online(this, str);
                            }
                        }
                    }
                    else
                    {
                        // 重试0次
                        _modbus.WriteRegs(0, timeout);
                        Status = "SetRegister successful";
                        IsSetSuccess = true;
                    }

                    if (SetResult != null)
                    {
                        SetResult(this, "寄存器设置成功");
                    }
                }
                catch (System.Exception ex)
                {
                    Status = "SetRegister unsuccessful";
                    IsSetSuccess = false;
                    CountErrorSet++;
                    TimeErrorSet = DateTime.Now;

                    if (SetResult != null)
                    {
                        SetResult(this, "寄存器设置失败");
                    }

                    if (Log.IsDebugEnabled) { Log.DebugFormat("Device:[{0}]" + Status + " - " + ex.Message, this.Index); }

                    if (IsOffline == false)
                    {
                        IsOffline = true;
                        TimeOffline = DateTime.Now;

                        string str = string.Format("Device:[{0}][Offline]" + Status + " - " + ex.Message, this.Index);

                        Log.Warn(str);

                        if (Offline != null)
                        {
                            Offline(this, str);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 读取寄存器
        /// </summary>
        private void GetRegister(ushort start_ad, ushort nums, ushort[] buf, int timeout)
        {
            lock (_lock)
            {
                _modbus.StartAddress = start_ad;
                _modbus.Numbers = nums;
                _modbus.Values = buf;

                CountTotalGet++;

                try
                {
                    if (IsOffline == true)
                    {
                        // 重试0次
                        _modbus.ReadRegs(0, timeout);

                        if (IsOffline == true)
                        {
                            IsOffline = false;

                            Status = "GetRegister successful";
                            IsGetSuccess = true;

                            string str = string.Format("Device:[{0}][Online]" + Status, this.Index);

                            Log.Warn(str);

                            if (Online != null)
                            {
                                Online(this, str);
                            }
                        }
                    }
                    else
                    {
                        // 重试0次
                        _modbus.ReadRegs(0, timeout);
                        Status = "GetRegister successful";
                        IsGetSuccess = true;
                    }
                }
                catch (System.Exception ex)
                {
                    Status = "GetRegister unsuccessful";
                    IsGetSuccess = false;
                    CountErrorGet++;
                    TimeErrorGet = DateTime.Now;

                    if (Log.IsDebugEnabled) { Log.DebugFormat("Device:[{0}]" + Status + " - " + ex.Message, this.Index); }

                    if (IsOffline == false)
                    {
                        IsOffline = true;
                        TimeOffline = DateTime.Now;

                        string str = string.Format("Device:[{0}][Offline]" + Status + " - " + ex.Message, this.Index);

                        Log.Warn(str);

                        if (Offline != null)
                        {
                            Offline(this, str);
                        }
                    }
                }
            }
        }
        #endregion

        #endregion

        #region 保护字段
        /// <summary>
        /// 日志
        /// </summary>
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(CKM55LC_M));

        /// <summary>
        /// 读写寄存器方法委托
        /// </summary>
        private GetSetRegisterHandler _SetRegisterHandle;
        private GetSetRegisterHandler _GetRegisterHandle;

        /// <summary>
        /// modbus对象
        /// </summary>
        private Modbus _modbus;

        /// <summary>
        /// 测量参数
        /// </summary>
        private RunValue _RunValues;

        /// <summary>
        /// 监视参数
        /// </summary>
        private MonitorValue _MonitorValues;

        /// <summary>
        /// 控制参数
        /// </summary>
        private ControlSet _ControlSets;

        /// <summary>
        /// 诊断参数
        /// </summary>
        private DiagnosisValue _DiagnosisValues;

        /// <summary>
        /// 保护参数
        /// </summary>
        private ProtectSet _ProtectSets;

        /// <summary>
        /// 重合闸参数
        /// </summary>
        private ReclosingSet _ReclosingSets;
        
        /// <summary>
        /// 系统参数
        /// </summary>
        private SystemSet _SystemSets;

        /// <summary>
        /// 识别信息
        /// </summary>
        private IdentityValue _IdentityValues;

        /// <summary>
        /// 出厂修正参数
        /// </summary>
        private CalibrationValue _CalibrationValues;

        /// <summary>
        /// 排它锁
        /// </summary>
        private object _lock;
        #endregion
    }
}
