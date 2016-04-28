using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using log4net;

namespace Devices.MCCB
{
    /// <summary>
    /// 诊断参数类
    /// </summary>
    public class DiagnosisValue
    {
        #region 公有字段
        /// <summary>
        /// 故障历史记录
        /// </summary>
        public struct HistoryFault
        {
            public string Code;           // 故障类型
            public ushort Value;          // 故障值
			public float Current_A;		  // 故障A相电流
			public float Current_B;       // 故障B相电流
			public float Current_C;       // 故障C相电流
			public ushort Current_Residual;       // 故障剩余电流
			public ushort Voltage_A;       // 故障A相电压
			public ushort Voltage_B;       // 故障B相电压
			public ushort Voltage_C;       // 故障C相电压
            public DateTime Time;         // 故障时间
        }

        /// <summary>
        /// SOE历史记录
        /// </summary>
        public struct HistorySOE
        {
            public string Code;           // SOE代码
            public DateTime Time;         // SOE时间
        }
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

        #region  属性
        /// <summary>
        /// 累计故障次数
        /// </summary>
        public ushort Counter_Fault
        {
            get
            {
                return _cache_diagnosis_value.Buf[MBREG.Counter_Fault - MBREG.Address_Diagnosis_Counter_Value_Start];
            }
        }

        /// <summary>
        /// 累计脱扣次数
        /// </summary>
        public ushort Counter_Trip
        {
            get
            {
                return _cache_diagnosis_value.Buf[MBREG.Counter_Trip - MBREG.Address_Diagnosis_Counter_Value_Start];
            }
        }

        /// <summary>
        /// 累计上电重合闸次数
        /// </summary>
        public ushort Counter_PowerupReclosing
        {
            get
            {
                return _cache_diagnosis_value.Buf[MBREG.Counter_PowerupReclosing - MBREG.Address_Diagnosis_Counter_Value_Start];
            }
        }

        /// <summary>
        /// 累计漏电重合闸次数
        /// </summary>
        public ushort Counter_ResidualReclosing
        {
            get
            {
                return _cache_diagnosis_value.Buf[MBREG.Counter_ResidualReclosing - MBREG.Address_Diagnosis_Counter_Value_Start];
            }
        }

        /// <summary>
        /// 累计过压重合闸次数
        /// </summary>
        public ushort Counter_OverVoltageReclosing
        {
            get
            {
                return _cache_diagnosis_value.Buf[MBREG.Counter_OverVoltageReclosing - MBREG.Address_Diagnosis_Counter_Value_Start];
            }
        }

        /// <summary>
        /// 累计欠压重合闸次数
        /// </summary>
        public ushort Counter_UnderVoltageReclosing
        {
            get
            {
                return _cache_diagnosis_value.Buf[MBREG.Counter_UnderVoltageReclosing - MBREG.Address_Diagnosis_Counter_Value_Start];
            }
        }

        /// <summary>
        /// 允许复位倒计时(s)
        /// </summary>
        public ushort Counter_Timer_EnableToReset
        {
            get
            {
                return _cache_diagnosis_value.Buf[MBREG.Timer_EnableToReset - MBREG.Address_Diagnosis_Counter_Value_Start];
            }
        }

        /// <summary>
        /// 自动重合闸倒计时(s)
        /// </summary>
        public ushort Counter_Timer_Reclosing
        {
            get
            {
                ushort tmp = _cache_diagnosis_value.Buf[MBREG.Timer_Reclosing - MBREG.Address_Diagnosis_Counter_Value_Start];
                return (ushort)Math.Round(tmp / 100F, 1);
            }
        }

        /// <summary>
        /// 合闸计时(s)
        /// </summary>
        public ushort Counter_Timer_Closing
        {
            get
            {
                ushort tmp = _cache_diagnosis_value.Buf[MBREG.Timer_Closing - MBREG.Address_Diagnosis_Counter_Value_Start];
                return (ushort)Math.Round(tmp / 100F, 1);
            }
        }

        /// <summary>
        /// 历史故障信息
        /// </summary>
        public List<HistoryFault> Fault_0_9
        {
            get
            {
                List<HistoryFault> fl = new List<HistoryFault>();
                HistoryFault hf = new HistoryFault();
                uint tmp;
                ushort y_m;
                ushort d_h;
                ushort m_s;

                // fault0
                tmp = _cache_diagnosis_value.Buf[MBREG.Fault_0_Code - MBREG.Address_Diagnosis_Value_Start];
                tmp <<= 16;
                tmp |= _cache_diagnosis_value.Buf[MBREG.Fault_0_Code - MBREG.Address_Diagnosis_Value_Start];
                hf.Code = Utilities.BitCodeToString(Utilities.Str_Fault, tmp);

                hf.Value = _cache_diagnosis_value.Buf[MBREG.Fault_1_Value - MBREG.Address_Diagnosis_Value_Start];
                hf.Current_A = _cache_diagnosis_value.Buf[MBREG.Fault_0_Current_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_B = _cache_diagnosis_value.Buf[MBREG.Fault_0_Current_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_C = _cache_diagnosis_value.Buf[MBREG.Fault_0_Current_C - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_Residual = _cache_diagnosis_value.Buf[MBREG.Fault_0_Current_Residual - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_A = _cache_diagnosis_value.Buf[MBREG.Fault_0_Voltage_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_B = _cache_diagnosis_value.Buf[MBREG.Fault_0_Voltage_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_C = _cache_diagnosis_value.Buf[MBREG.Fault_0_Voltage_C - MBREG.Address_Diagnosis_Counter_Value_Start];

                y_m = _cache_diagnosis_value.Buf[MBREG.Fault_0_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.Fault_0_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.Fault_0_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hf.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                fl.Add(hf);

                // fault1
                tmp = _cache_diagnosis_value.Buf[MBREG.Fault_1_Code - MBREG.Address_Diagnosis_Value_Start];
                tmp <<= 16;
                tmp |= _cache_diagnosis_value.Buf[MBREG.Fault_1_Code - MBREG.Address_Diagnosis_Value_Start];
                hf.Code = Utilities.BitCodeToString(Utilities.Str_Fault, tmp);

                hf.Value = _cache_diagnosis_value.Buf[MBREG.Fault_1_Value - MBREG.Address_Diagnosis_Value_Start];
                hf.Current_A = _cache_diagnosis_value.Buf[MBREG.Fault_1_Current_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_B = _cache_diagnosis_value.Buf[MBREG.Fault_1_Current_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_C = _cache_diagnosis_value.Buf[MBREG.Fault_1_Current_C - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_Residual = _cache_diagnosis_value.Buf[MBREG.Fault_1_Current_Residual - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_A = _cache_diagnosis_value.Buf[MBREG.Fault_1_Voltage_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_B = _cache_diagnosis_value.Buf[MBREG.Fault_1_Voltage_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_C = _cache_diagnosis_value.Buf[MBREG.Fault_1_Voltage_C - MBREG.Address_Diagnosis_Counter_Value_Start];

                y_m = _cache_diagnosis_value.Buf[MBREG.Fault_1_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.Fault_1_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.Fault_1_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hf.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                // fault2
                tmp = _cache_diagnosis_value.Buf[MBREG.Fault_2_Code - MBREG.Address_Diagnosis_Value_Start];
                tmp <<= 16;
                tmp |= _cache_diagnosis_value.Buf[MBREG.Fault_2_Code - MBREG.Address_Diagnosis_Value_Start];
                hf.Code = Utilities.BitCodeToString(Utilities.Str_Fault, tmp);

                hf.Value = _cache_diagnosis_value.Buf[MBREG.Fault_2_Value - MBREG.Address_Diagnosis_Value_Start];
                hf.Current_A = _cache_diagnosis_value.Buf[MBREG.Fault_2_Current_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_B = _cache_diagnosis_value.Buf[MBREG.Fault_2_Current_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_C = _cache_diagnosis_value.Buf[MBREG.Fault_2_Current_C - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_Residual = _cache_diagnosis_value.Buf[MBREG.Fault_2_Current_Residual - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_A = _cache_diagnosis_value.Buf[MBREG.Fault_2_Voltage_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_B = _cache_diagnosis_value.Buf[MBREG.Fault_2_Voltage_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_C = _cache_diagnosis_value.Buf[MBREG.Fault_2_Voltage_C - MBREG.Address_Diagnosis_Counter_Value_Start];

                y_m = _cache_diagnosis_value.Buf[MBREG.Fault_2_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.Fault_2_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.Fault_2_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hf.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                // fault3
                tmp = _cache_diagnosis_value.Buf[MBREG.Fault_3_Code - MBREG.Address_Diagnosis_Value_Start];
                tmp <<= 16;
                tmp |= _cache_diagnosis_value.Buf[MBREG.Fault_3_Code - MBREG.Address_Diagnosis_Value_Start];
                hf.Code = Utilities.BitCodeToString(Utilities.Str_Fault, tmp);

                hf.Value = _cache_diagnosis_value.Buf[MBREG.Fault_3_Value - MBREG.Address_Diagnosis_Value_Start];
                hf.Current_A = _cache_diagnosis_value.Buf[MBREG.Fault_3_Current_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_B = _cache_diagnosis_value.Buf[MBREG.Fault_3_Current_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_C = _cache_diagnosis_value.Buf[MBREG.Fault_3_Current_C - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_Residual = _cache_diagnosis_value.Buf[MBREG.Fault_3_Current_Residual - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_A = _cache_diagnosis_value.Buf[MBREG.Fault_3_Voltage_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_B = _cache_diagnosis_value.Buf[MBREG.Fault_3_Voltage_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_C = _cache_diagnosis_value.Buf[MBREG.Fault_3_Voltage_C - MBREG.Address_Diagnosis_Counter_Value_Start];

                y_m = _cache_diagnosis_value.Buf[MBREG.Fault_3_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.Fault_3_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.Fault_3_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hf.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                // fault4
                tmp = _cache_diagnosis_value.Buf[MBREG.Fault_4_Code - MBREG.Address_Diagnosis_Value_Start];
                tmp <<= 16;
                tmp |= _cache_diagnosis_value.Buf[MBREG.Fault_4_Code - MBREG.Address_Diagnosis_Value_Start];
                hf.Code = Utilities.BitCodeToString(Utilities.Str_Fault, tmp);

                hf.Value = _cache_diagnosis_value.Buf[MBREG.Fault_4_Value - MBREG.Address_Diagnosis_Value_Start];
                hf.Current_A = _cache_diagnosis_value.Buf[MBREG.Fault_4_Current_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_B = _cache_diagnosis_value.Buf[MBREG.Fault_4_Current_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_C = _cache_diagnosis_value.Buf[MBREG.Fault_4_Current_C - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_Residual = _cache_diagnosis_value.Buf[MBREG.Fault_4_Current_Residual - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_A = _cache_diagnosis_value.Buf[MBREG.Fault_4_Voltage_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_B = _cache_diagnosis_value.Buf[MBREG.Fault_4_Voltage_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_C = _cache_diagnosis_value.Buf[MBREG.Fault_4_Voltage_C - MBREG.Address_Diagnosis_Counter_Value_Start];

                y_m = _cache_diagnosis_value.Buf[MBREG.Fault_4_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.Fault_4_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.Fault_4_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hf.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                // fault5
                tmp = _cache_diagnosis_value.Buf[MBREG.Fault_5_Code - MBREG.Address_Diagnosis_Value_Start];
                tmp <<= 16;
                tmp |= _cache_diagnosis_value.Buf[MBREG.Fault_5_Code - MBREG.Address_Diagnosis_Value_Start];
                hf.Code = Utilities.BitCodeToString(Utilities.Str_Fault, tmp);

                hf.Value = _cache_diagnosis_value.Buf[MBREG.Fault_5_Value - MBREG.Address_Diagnosis_Value_Start];
                hf.Current_A = _cache_diagnosis_value.Buf[MBREG.Fault_5_Current_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_B = _cache_diagnosis_value.Buf[MBREG.Fault_5_Current_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_C = _cache_diagnosis_value.Buf[MBREG.Fault_5_Current_C - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_Residual = _cache_diagnosis_value.Buf[MBREG.Fault_5_Current_Residual - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_A = _cache_diagnosis_value.Buf[MBREG.Fault_5_Voltage_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_B = _cache_diagnosis_value.Buf[MBREG.Fault_5_Voltage_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_C = _cache_diagnosis_value.Buf[MBREG.Fault_5_Voltage_C - MBREG.Address_Diagnosis_Counter_Value_Start];

                y_m = _cache_diagnosis_value.Buf[MBREG.Fault_5_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.Fault_5_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.Fault_5_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hf.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                // fault6
                tmp = _cache_diagnosis_value.Buf[MBREG.Fault_6_Code - MBREG.Address_Diagnosis_Value_Start];
                tmp <<= 16;
                tmp |= _cache_diagnosis_value.Buf[MBREG.Fault_6_Code - MBREG.Address_Diagnosis_Value_Start];
                hf.Code = Utilities.BitCodeToString(Utilities.Str_Fault, tmp);

                hf.Value = _cache_diagnosis_value.Buf[MBREG.Fault_6_Value - MBREG.Address_Diagnosis_Value_Start];
                hf.Current_A = _cache_diagnosis_value.Buf[MBREG.Fault_6_Current_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_B = _cache_diagnosis_value.Buf[MBREG.Fault_6_Current_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_C = _cache_diagnosis_value.Buf[MBREG.Fault_6_Current_C - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_Residual = _cache_diagnosis_value.Buf[MBREG.Fault_6_Current_Residual - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_A = _cache_diagnosis_value.Buf[MBREG.Fault_6_Voltage_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_B = _cache_diagnosis_value.Buf[MBREG.Fault_6_Voltage_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_C = _cache_diagnosis_value.Buf[MBREG.Fault_6_Voltage_C - MBREG.Address_Diagnosis_Counter_Value_Start];

                y_m = _cache_diagnosis_value.Buf[MBREG.Fault_6_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.Fault_6_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.Fault_6_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hf.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                // fault7
                tmp = _cache_diagnosis_value.Buf[MBREG.Fault_7_Code - MBREG.Address_Diagnosis_Value_Start];
                tmp <<= 16;
                tmp |= _cache_diagnosis_value.Buf[MBREG.Fault_7_Code - MBREG.Address_Diagnosis_Value_Start];
                hf.Code = Utilities.BitCodeToString(Utilities.Str_Fault, tmp);

                hf.Value = _cache_diagnosis_value.Buf[MBREG.Fault_7_Value - MBREG.Address_Diagnosis_Value_Start];
                hf.Current_A = _cache_diagnosis_value.Buf[MBREG.Fault_7_Current_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_B = _cache_diagnosis_value.Buf[MBREG.Fault_7_Current_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_C = _cache_diagnosis_value.Buf[MBREG.Fault_7_Current_C - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_Residual = _cache_diagnosis_value.Buf[MBREG.Fault_7_Current_Residual - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_A = _cache_diagnosis_value.Buf[MBREG.Fault_7_Voltage_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_B = _cache_diagnosis_value.Buf[MBREG.Fault_7_Voltage_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_C = _cache_diagnosis_value.Buf[MBREG.Fault_7_Voltage_C - MBREG.Address_Diagnosis_Counter_Value_Start];

                y_m = _cache_diagnosis_value.Buf[MBREG.Fault_7_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.Fault_7_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.Fault_7_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hf.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                // fault8
                tmp = _cache_diagnosis_value.Buf[MBREG.Fault_8_Code - MBREG.Address_Diagnosis_Value_Start];
                tmp <<= 16;
                tmp |= _cache_diagnosis_value.Buf[MBREG.Fault_8_Code - MBREG.Address_Diagnosis_Value_Start];
                hf.Code = Utilities.BitCodeToString(Utilities.Str_Fault, tmp);

                hf.Value = _cache_diagnosis_value.Buf[MBREG.Fault_8_Value - MBREG.Address_Diagnosis_Value_Start];
                hf.Current_A = _cache_diagnosis_value.Buf[MBREG.Fault_8_Current_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_B = _cache_diagnosis_value.Buf[MBREG.Fault_8_Current_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_C = _cache_diagnosis_value.Buf[MBREG.Fault_8_Current_C - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_Residual = _cache_diagnosis_value.Buf[MBREG.Fault_8_Current_Residual - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_A = _cache_diagnosis_value.Buf[MBREG.Fault_8_Voltage_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_B = _cache_diagnosis_value.Buf[MBREG.Fault_8_Voltage_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_C = _cache_diagnosis_value.Buf[MBREG.Fault_8_Voltage_C - MBREG.Address_Diagnosis_Counter_Value_Start];

                y_m = _cache_diagnosis_value.Buf[MBREG.Fault_8_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.Fault_8_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.Fault_8_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hf.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                // fault9
                tmp = _cache_diagnosis_value.Buf[MBREG.Fault_9_Code - MBREG.Address_Diagnosis_Value_Start];
                tmp <<= 16;
                tmp |= _cache_diagnosis_value.Buf[MBREG.Fault_9_Code - MBREG.Address_Diagnosis_Value_Start];
                hf.Code = Utilities.BitCodeToString(Utilities.Str_Fault, tmp);

                hf.Value = _cache_diagnosis_value.Buf[MBREG.Fault_9_Value - MBREG.Address_Diagnosis_Value_Start];
                hf.Current_A = _cache_diagnosis_value.Buf[MBREG.Fault_9_Current_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_B = _cache_diagnosis_value.Buf[MBREG.Fault_9_Current_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_C = _cache_diagnosis_value.Buf[MBREG.Fault_9_Current_C - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Current_Residual = _cache_diagnosis_value.Buf[MBREG.Fault_9_Current_Residual - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_A = _cache_diagnosis_value.Buf[MBREG.Fault_9_Voltage_A - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_B = _cache_diagnosis_value.Buf[MBREG.Fault_9_Voltage_B - MBREG.Address_Diagnosis_Counter_Value_Start];
                hf.Voltage_C = _cache_diagnosis_value.Buf[MBREG.Fault_9_Voltage_C - MBREG.Address_Diagnosis_Counter_Value_Start];

                y_m = _cache_diagnosis_value.Buf[MBREG.Fault_9_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.Fault_9_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.Fault_9_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hf.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                return fl;
            }
        }

                /// <summary>
        /// SOE历史记录
        /// </summary>
        public List<HistorySOE> SOE_0_11
        {
            get
            {
                List<HistorySOE> sl = new List<HistorySOE>();
                HistorySOE hs = new HistorySOE();
                ushort tmp;
                ushort y_m;
                ushort d_h;
                ushort m_s;

                // SOE1
                tmp = _cache_diagnosis_value.Buf[MBREG.SOE_0_Code - MBREG.Address_Diagnosis_Value_Start];
                hs.Code = Utilities.Str_SOE[tmp];

                y_m = _cache_diagnosis_value.Buf[MBREG.SOE_1_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.SOE_1_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.SOE_1_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hs.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                // SOE2
                tmp = _cache_diagnosis_value.Buf[MBREG.SOE_0_Code - MBREG.Address_Diagnosis_Value_Start];
                hs.Code = Utilities.Str_SOE[tmp];

                y_m = _cache_diagnosis_value.Buf[MBREG.SOE_2_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.SOE_2_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.SOE_2_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hs.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                // SOE3
                tmp = _cache_diagnosis_value.Buf[MBREG.SOE_0_Code - MBREG.Address_Diagnosis_Value_Start];
                hs.Code = Utilities.Str_SOE[tmp];

                y_m = _cache_diagnosis_value.Buf[MBREG.SOE_3_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.SOE_3_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.SOE_3_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hs.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                // SOE4
                tmp = _cache_diagnosis_value.Buf[MBREG.SOE_0_Code - MBREG.Address_Diagnosis_Value_Start];
                hs.Code = Utilities.Str_SOE[tmp];

                y_m = _cache_diagnosis_value.Buf[MBREG.SOE_4_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.SOE_4_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.SOE_4_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hs.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                // SOE5
                tmp = _cache_diagnosis_value.Buf[MBREG.SOE_0_Code - MBREG.Address_Diagnosis_Value_Start];
                hs.Code = Utilities.Str_SOE[tmp];

                y_m = _cache_diagnosis_value.Buf[MBREG.SOE_5_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.SOE_5_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.SOE_5_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hs.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                // SOE6
                tmp = _cache_diagnosis_value.Buf[MBREG.SOE_0_Code - MBREG.Address_Diagnosis_Value_Start];
                hs.Code = Utilities.Str_SOE[tmp];

                y_m = _cache_diagnosis_value.Buf[MBREG.SOE_6_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.SOE_6_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.SOE_6_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hs.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                // SOE7
                tmp = _cache_diagnosis_value.Buf[MBREG.SOE_0_Code - MBREG.Address_Diagnosis_Value_Start];
                hs.Code = Utilities.Str_SOE[tmp];

                y_m = _cache_diagnosis_value.Buf[MBREG.SOE_7_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.SOE_7_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.SOE_7_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hs.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                // SOE8
                tmp = _cache_diagnosis_value.Buf[MBREG.SOE_0_Code - MBREG.Address_Diagnosis_Value_Start];
                hs.Code = Utilities.Str_SOE[tmp];

                y_m = _cache_diagnosis_value.Buf[MBREG.SOE_8_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.SOE_8_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.SOE_8_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hs.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                // SOE9
                tmp = _cache_diagnosis_value.Buf[MBREG.SOE_0_Code - MBREG.Address_Diagnosis_Value_Start];
                hs.Code = Utilities.Str_SOE[tmp];

                y_m = _cache_diagnosis_value.Buf[MBREG.SOE_9_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.SOE_9_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.SOE_9_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hs.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                // SOE10
                tmp = _cache_diagnosis_value.Buf[MBREG.SOE_0_Code - MBREG.Address_Diagnosis_Value_Start];
                hs.Code = Utilities.Str_SOE[tmp];

                y_m = _cache_diagnosis_value.Buf[MBREG.SOE_10_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.SOE_10_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.SOE_10_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hs.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);

                // SOE11
                tmp = _cache_diagnosis_value.Buf[MBREG.SOE_0_Code - MBREG.Address_Diagnosis_Value_Start];
                hs.Code = Utilities.Str_SOE[tmp];

                y_m = _cache_diagnosis_value.Buf[MBREG.SOE_11_Time_YM - MBREG.Address_Diagnosis_Value_Start];
                d_h = _cache_diagnosis_value.Buf[MBREG.SOE_11_Time_DH - MBREG.Address_Diagnosis_Value_Start];
                m_s = _cache_diagnosis_value.Buf[MBREG.SOE_11_Time_MS - MBREG.Address_Diagnosis_Value_Start];
                hs.Time = Utilities.BitCodeToDateTime(y_m, d_h, m_s);


                sl.Add(hs);
                return sl;
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
            if (Log.IsDebugEnabled) { Log.Debug("异步读取诊断参数"); }

            if (GetRegisterAsync != null)
            {
                GetRegisterAsync((ushort)MBREG.Address_Diagnosis_Value_Start, (ushort)_cache_diagnosis_value.Buf.Length,
                                 _cache_diagnosis_value.Buf, timeout, GetCallBack, this);
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
            if (Log.IsDebugEnabled) { Log.Debug("同步读取诊断参数"); }

            if (GetRegister != null)
            {
                GetRegister((ushort)MBREG.Address_Diagnosis_Value_Start, (ushort)_cache_diagnosis_value.Buf.Length,
                            _cache_diagnosis_value.Buf, timeout);
            }
            else
            {
                string str = this.ToString() + ".GetRegister has no provider";
                Log.Error(str);
                throw new Exception(str);
            }
        }

        /// <summary>
        /// 从设备读取统计参数
        /// </summary>
        public void GetCounterRegistersAsync(int timeout)
        {
            if (Log.IsDebugEnabled) { Log.Debug("异步读取诊断统计参数"); }

            if (GetRegisterAsync != null)
            {
                GetRegisterAsync((ushort)MBREG.Address_Diagnosis_Counter_Value_Start,
                                 (ushort)(MBREG.Address_Diagnosis_Counter_Value_End - MBREG.Address_Diagnosis_Counter_Value_Start),
                                 _cache_diagnosis_value.Buf, timeout, GetCallBack, this);
            }
            else
            {
                string str = this.ToString() + ".GetRegisterAsync has no provider";
                Log.Error(str);
                throw new Exception(str);
            }
        }

        public void GetCounterRegisters(int timeout)
        {
            if (Log.IsDebugEnabled) { Log.Debug("同步读取诊断统计参数"); }

            if (GetRegister != null)
            {
                GetRegister((ushort)MBREG.Address_Diagnosis_Counter_Value_Start,
                            (ushort)(MBREG.Address_Diagnosis_Counter_Value_End - MBREG.Address_Diagnosis_Counter_Value_Start),
                            _cache_diagnosis_value.Buf, timeout);
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
        /// <summary>
        /// modbus 寄存器定义
        /// </summary>
        private enum MBREG : ushort
        {
            Address_Diagnosis_Value_Start = 0x100,
            Address_Diagnosis_Counter_Value_Start = 0x100,

            Counter_Fault = 0x100,
            Counter_Trip = 0x101,
            Counter_PowerupReclosing = 0x102,
            Counter_ResidualReclosing = 0x103,
            Counter_OverVoltageReclosing = 0x104,
            Counter_UnderVoltageReclosing = 0x105,
            Timer_EnableToReset = 0x106,
            Timer_Reclosing = 0x107,
            Timer_Closing = 0x108,

            Address_Diagnosis_Counter_Value_End = 0x108,

            SOE_0_Code = 0x10E,
            SOE_0_Time_MS = 0x10F,
            SOE_0_Time_DH = 0x110,
            SOE_0_Time_YM = 0x111,

            SOE_1_Code = 0x112,
            SOE_1_Time_MS = 0x113,
            SOE_1_Time_DH = 0x114,
            SOE_1_Time_YM = 0x115,

            SOE_2_Code = 0x1016,
            SOE_2_Time_MS = 0x117,
            SOE_2_Time_DH = 0x118,
            SOE_2_Time_YM = 0x119,

            SOE_3_Code = 0x11A,
            SOE_3_Time_MS = 0x11B,
            SOE_3_Time_DH = 0x11C,
            SOE_3_Time_YM = 0x11D,

            SOE_4_Code = 0x11E,
            SOE_4_Time_MS = 0x11F,
            SOE_4_Time_DH = 0x120,
            SOE_4_Time_YM = 0x121,

            SOE_5_Code = 0x122,
            SOE_5_Time_MS = 0x123,
            SOE_5_Time_DH = 0x124,
            SOE_5_Time_YM = 0x125,

            SOE_6_Code = 0x126,
            SOE_6_Time_MS = 0x127,
            SOE_6_Time_DH = 0x128,
            SOE_6_Time_YM = 0x129,

            SOE_7_Code = 0x12A,
            SOE_7_Time_MS = 0x12B,
            SOE_7_Time_DH = 0x12C,
            SOE_7_Time_YM = 0x12D,

            SOE_8_Code = 0x12E,
            SOE_8_Time_MS = 0x12F,
            SOE_8_Time_DH = 0x130,
            SOE_8_Time_YM = 0x131,

            SOE_9_Code = 0x132,
            SOE_9_Time_MS = 0x133,
            SOE_9_Time_DH = 0x134,
            SOE_9_Time_YM = 0x135,

            SOE_10_Code = 0x136,
            SOE_10_Time_MS = 0x137,
            SOE_10_Time_DH = 0x138,
            SOE_10_Time_YM = 0x139,

            SOE_11_Code = 0x13A,
            SOE_11_Time_MS = 0x13B,
            SOE_11_Time_DH = 0x13C,
            SOE_11_Time_YM = 0x13D,

            Fault_0_Code = 0x13E,
            Fault_0_Value = 0x13F,
            Fault_0_Current_A = 0x140,
            Fault_0_Current_B = 0x141,
            Fault_0_Current_C = 0x142,
            Fault_0_Current_Residual = 0x143,
            Fault_0_Voltage_A = 0x144,
            Fault_0_Voltage_B = 0x145,
            Fault_0_Voltage_C = 0x146,
            Fault_0_Time_MS = 0x147,
            Fault_0_Time_DH = 0x148,
            Fault_0_Time_YM = 0x149,

            Fault_1_Code = 0x14A,
            Fault_1_Value = 0x14B,
            Fault_1_Current_A = 0x14C,
            Fault_1_Current_B = 0x14D,
            Fault_1_Current_C = 0x14E,
            Fault_1_Current_Residual = 0x14F,
            Fault_1_Voltage_A = 0x150,
            Fault_1_Voltage_B = 0x151,
            Fault_1_Voltage_C = 0x152,
            Fault_1_Time_MS = 0x153,
            Fault_1_Time_DH = 0x154,
            Fault_1_Time_YM = 0x155,

            Fault_2_Code = 0x156,
            Fault_2_Value = 0x157,
            Fault_2_Current_A = 0x158,
            Fault_2_Current_B = 0x159,
            Fault_2_Current_C = 0x15A,
            Fault_2_Current_Residual = 0x15B,
            Fault_2_Voltage_A = 0x15C,
            Fault_2_Voltage_B = 0x15D,
            Fault_2_Voltage_C = 0x15E,
            Fault_2_Time_MS = 0x15F,
            Fault_2_Time_DH = 0x160,
            Fault_2_Time_YM = 0x161,

            Fault_3_Code = 0x162,
            Fault_3_Value = 0x163,
            Fault_3_Current_A = 0x164,
            Fault_3_Current_B = 0x165,
            Fault_3_Current_C = 0x166,
            Fault_3_Current_Residual = 0x167,
            Fault_3_Voltage_A = 0x168,
            Fault_3_Voltage_B = 0x169,
            Fault_3_Voltage_C = 0x16A,
            Fault_3_Time_MS = 0x16B,
            Fault_3_Time_DH = 0x16C,
            Fault_3_Time_YM = 0x16D,

            Fault_4_Code = 0x16E,
            Fault_4_Value = 0x16F,
            Fault_4_Current_A = 0x170,
            Fault_4_Current_B = 0x171,
            Fault_4_Current_C = 0x172,
            Fault_4_Current_Residual = 0x173,
            Fault_4_Voltage_A = 0x174,
            Fault_4_Voltage_B = 0x175,
            Fault_4_Voltage_C = 0x176,
            Fault_4_Time_MS = 0x177,
            Fault_4_Time_DH = 0x178,
            Fault_4_Time_YM = 0x179,

            Fault_5_Code = 0x17A,
            Fault_5_Value = 0x17B,
            Fault_5_Current_A = 0x17C,
            Fault_5_Current_B = 0x17D,
            Fault_5_Current_C = 0x17E,
            Fault_5_Current_Residual = 0x17F,
            Fault_5_Voltage_A = 0x180,
            Fault_5_Voltage_B = 0x181,
            Fault_5_Voltage_C = 0x182,
            Fault_5_Time_MS = 0x183,
            Fault_5_Time_DH = 0x184,
            Fault_5_Time_YM = 0x185,

            Fault_6_Code = 0x186,
            Fault_6_Value = 0x187,
            Fault_6_Current_A = 0x188,
            Fault_6_Current_B = 0x189,
            Fault_6_Current_C = 0x18A,
            Fault_6_Current_Residual = 0x18B,
            Fault_6_Voltage_A = 0x18C,
            Fault_6_Voltage_B = 0x18D,
            Fault_6_Voltage_C = 0x18E,
            Fault_6_Time_MS = 0x18F,
            Fault_6_Time_DH = 0x190,
            Fault_6_Time_YM = 0x191,

            Fault_7_Code = 0x192,
            Fault_7_Value = 0x193,
            Fault_7_Current_A = 0x194,
            Fault_7_Current_B = 0x195,
            Fault_7_Current_C = 0x196,
            Fault_7_Current_Residual = 0x197,
            Fault_7_Voltage_A = 0x198,
            Fault_7_Voltage_B = 0x199,
            Fault_7_Voltage_C = 0x19A,
            Fault_7_Time_MS = 0x19B,
            Fault_7_Time_DH = 0x19C,
            Fault_7_Time_YM = 0x19D,

            Fault_8_Code = 0x19E,
            Fault_8_Value = 0x19F,
            Fault_8_Current_A = 0x1A0,
            Fault_8_Current_B = 0x1A1,
            Fault_8_Current_C = 0x1A2,
            Fault_8_Current_Residual = 0x1A3,
            Fault_8_Voltage_A = 0x1A4,
            Fault_8_Voltage_B = 0x1A5,
            Fault_8_Voltage_C = 0x1A6,
            Fault_8_Time_MS = 0x1A7,
            Fault_8_Time_DH = 0x1A8,
            Fault_8_Time_YM = 0x1A9,

            Fault_9_Code = 0x1AA,
            Fault_9_Value = 0x1AB,
            Fault_9_Current_A = 0x1AC,
            Fault_9_Current_B = 0x1AD,
            Fault_9_Current_C = 0x1AE,
            Fault_9_Current_Residual = 0x1AF,
            Fault_9_Voltage_A = 0x1B0,
            Fault_9_Voltage_B = 0x1B1,
            Fault_9_Voltage_C = 0x1B2,
            Fault_9_Time_MS = 0x1B3,
            Fault_9_Time_DH = 0x1B4,
            Fault_9_Time_YM = 0x1B5,

            Current_A_Max_0 = 0x1B6,
            Current_A_Time_MS_0 = 0x1B7,
            Current_A_Time_DH_0 = 0x1B8,
            Current_A_Time_YM_0 = 0x1B9,
            Current_B_Max_0 = 0x1BA,
            Current_B_Time_MS_0 = 0x1BB,
            Current_B_Time_DH_0 = 0x1BC,
            Current_B_Time_YM_0 = 0x1BD,
            Current_C_Max_0 = 0x1BE,
            Current_C_Time_MS_0 = 0x1BF,
            Current_C_Time_DH_0 = 0x1C0,
            Current_C_Time_YM_0 = 0x1C1,
            Current_Residual_Max_0 = 0x1C2,
            Current_Residual_Time_MS_0 = 0x1C3,
            Current_Residual_Time_DH_0 = 0x1C4,
            Current_Residual_Time_YM_0 = 0x1C5,
            Voltage_A_Max_0 = 0x1C6,
            Voltage_A_Time_MS_0 = 0x1C7,
            Voltage_A_Time_DH_0 = 0x1C8,
            Voltage_A_Time_YM_0 = 0x1C9,
            Voltage_B_Max_0 = 0x1CA,
            Voltage_B_Time_MS_0 = 0x1CB,
            Voltage_B_Time_DH_0 = 0x1CC,
            Voltage_B_Time_YM_0 = 0x1CD,
            Voltage_C_Max_0 = 0x1CE,
            Voltage_C_Time_MS_0 = 0x1CF,
            Voltage_C_Time_DH_0 = 0x1D0,
            Voltage_C_Time_YM_0 = 0x1D1,

            Current_A_Max_1 = 0x1D2,
            Current_A_Time_MS_1 = 0x1D3,
            Current_A_Time_DH_1 = 0x1D4,
            Current_A_Time_YM_1 = 0x1D5,
            Current_B_Max_1 = 0x1D6,
            Current_B_Time_MS_1 = 0x1D7,
            Current_B_Time_DH_1 = 0x1D8,
            Current_B_Time_YM_1 = 0x1D9,
            Current_C_Max_1 = 0x1DA,
            Current_C_Time_MS_1 = 0x1DB,
            Current_C_Time_DH_1 = 0x1DC,
            Current_C_Time_YM_1 = 0x1DD,
            Current_Residual_Max_1 = 0x1DE,
            Current_Residual_Time_MS_1 = 0x1DF,
            Current_Residual_Time_DH_1 = 0x1E0,
            Current_Residual_Time_YM_1 = 0x1E1,
            Voltage_A_Max_1 = 0x1E2,
            Voltage_A_Time_MS_1 = 0x1E3,
            Voltage_A_Time_DH_1 = 0x1E4,
            Voltage_A_Time_YM_1 = 0x1E5,
            Voltage_B_Max_1 = 0x1E6,
            Voltage_B_Time_MS_1 = 0x1E7,
            Voltage_B_Time_DH_1 = 0x1E8,
            Voltage_B_Time_YM_1 = 0x1E9,
            Voltage_C_Max_1 = 0x1EA,
            Voltage_C_Time_MS_1 = 0x1EB,
            Voltage_C_Time_DH_1 = 0x1EC,
            Voltage_C_Time_YM_1 = 0x1ED,

            Current_A_Max_2 = 0x1EE,
            Current_A_Time_MS_2 = 0x1EF,
            Current_A_Time_DH_2 = 0x1F0,
            Current_A_Time_YM_2 = 0x1F1,
            Current_B_Max_2 = 0x1F2,
            Current_B_Time_MS_2 = 0x1F3,
            Current_B_Time_DH_2 = 0x1F4,
            Current_B_Time_YM_2 = 0x1F5,
            Current_C_Max_2 = 0x1F6,
            Current_C_Time_MS_2 = 0x1F7,
            Current_C_Time_DH_2 = 0x1F8,
            Current_C_Time_YM_2 = 0x1F9,
            Current_Residual_Max_2 = 0x1FA,
            Current_Residual_Time_MS_2 = 0x1FB,
            Current_Residual_Time_DH_2 = 0x1FC,
            Current_Residual_Time_YM_2 = 0x1FD,
            Voltage_A_Max_2 = 0x1FE,
            Voltage_A_Time_MS_2 = 0x1FF,
            Voltage_A_Time_DH_2 = 0x200,
            Voltage_A_Time_YM_2 = 0x201,
            Voltage_B_Max_2 = 0x202,
            Voltage_B_Time_MS_2 = 0x203,
            Voltage_B_Time_DH_2 = 0x204,
            Voltage_B_Time_YM_2 = 0x205,
            Voltage_C_Max_2 = 0x206,
            Voltage_C_Time_MS_2 = 0x207,
            Voltage_C_Time_DH_2 = 0x208,
            Voltage_C_Time_YM_2 = 0x209,

            Current_A_Max_3 = 0x20A,
            Current_A_Time_MS_3 = 0x20B,
            Current_A_Time_DH_3 = 0x20C,
            Current_A_Time_YM_3 = 0x20D,
            Current_B_Max_3 = 0x20E,
            Current_B_Time_MS_3 = 0x20F,
            Current_B_Time_DH_3 = 0x210,
            Current_B_Time_YM_3 = 0x211,
            Current_C_Max_3 = 0x212,
            Current_C_Time_MS_3 = 0x213,
            Current_C_Time_DH_3 = 0x214,
            Current_C_Time_YM_3 = 0x215,
            Current_Residual_Max_3 = 0x216,
            Current_Residual_Time_MS_3 = 0x217,
            Current_Residual_Time_DH_3 = 0x218,
            Current_Residual_Time_YM_3 = 0x219,
            Voltage_A_Max_3 = 0x21A,
            Voltage_A_Time_MS_3 = 0x21B,
            Voltage_A_Time_DH_3 = 0x21C,
            Voltage_A_Time_YM_3 = 0x21D,
            Voltage_B_Max_3 = 0x21E,
            Voltage_B_Time_MS_3 = 0x21F,
            Voltage_B_Time_DH_3 = 0x220,
            Voltage_B_Time_YM_3 = 0x221,
            Voltage_C_Max_3 = 0x222,
            Voltage_C_Time_MS_3 = 0x223,
            Voltage_C_Time_DH_3 = 0x224,
            Voltage_C_Time_YM_3 = 0x225,

            Current_A_Max_4 = 0x226,
            Current_A_Time_MS_4 = 0x227,
            Current_A_Time_DH_4 = 0x228,
            Current_A_Time_YM_4 = 0x229,
            Current_B_Max_4 = 0x22A,
            Current_B_Time_MS_4 = 0x22B,
            Current_B_Time_DH_4 = 0x22C,
            Current_B_Time_YM_4 = 0x22D,
            Current_C_Max_4 = 0x22E,
            Current_C_Time_MS_4 = 0x22F,
            Current_C_Time_DH_4 = 0x230,
            Current_C_Time_YM_4 = 0x231,
            Current_Residual_Max_4 = 0x232,
            Current_Residual_Time_MS_4 = 0x233,
            Current_Residual_Time_DH_4 = 0x234,
            Current_Residual_Time_YM_4 = 0x235,
            Voltage_A_Max_4 = 0x236,
            Voltage_A_Time_MS_4 = 0x237,
            Voltage_A_Time_DH_4 = 0x238,
            Voltage_A_Time_YM_4 = 0x239,
            Voltage_B_Max_4 = 0x23A,
            Voltage_B_Time_MS_4 = 0x23B,
            Voltage_B_Time_DH_4 = 0x23C,
            Voltage_B_Time_YM_4 = 0x23D,
            Voltage_C_Max_4 = 0x23E,
            Voltage_C_Time_MS_4 = 0x23F,
            Voltage_C_Time_DH_4 = 0x240,
            Voltage_C_Time_YM_4 = 0x241,

            Current_A_Max_5 = 0x242,
            Current_A_Time_MS_5 = 0x243,
            Current_A_Time_DH_5 = 0x244,
            Current_A_Time_YM_5 = 0x245,
            Current_B_Max_5 = 0x246,
            Current_B_Time_MS_5 = 0x247,
            Current_B_Time_DH_5 = 0x248,
            Current_B_Time_YM_5 = 0x249,
            Current_C_Max_5 = 0x24A,
            Current_C_Time_MS_5 = 0x24B,
            Current_C_Time_DH_5 = 0x24C,
            Current_C_Time_YM_5 = 0x24D,
            Current_Residual_Max_5 = 0x24E,
            Current_Residual_Time_MS_5 = 0x24F,
            Current_Residual_Time_DH_5 = 0x250,
            Current_Residual_Time_YM_5 = 0x251,
            Voltage_A_Max_5 = 0x252,
            Voltage_A_Time_MS_5 = 0x253,
            Voltage_A_Time_DH_5 = 0x254,
            Voltage_A_Time_YM_5 = 0x255,
            Voltage_B_Max_5 = 0x256,
            Voltage_B_Time_MS_5 = 0x257,
            Voltage_B_Time_DH_5 = 0x258,
            Voltage_B_Time_YM_5 = 0x259,
            Voltage_C_Max_5 = 0x25A,
            Voltage_C_Time_MS_5 = 0x25B,
            Voltage_C_Time_DH_5 = 0x25C,
            Voltage_C_Time_YM_5 = 0x25D,

            Current_A_Max_6 = 0x25E,
            Current_A_Time_MS_6 = 0x25F,
            Current_A_Time_DH_6 = 0x260,
            Current_A_Time_YM_6 = 0x261,
            Current_B_Max_6 = 0x262,
            Current_B_Time_MS_6 = 0x263,
            Current_B_Time_DH_6 = 0x264,
            Current_B_Time_YM_6 = 0x265,
            Current_C_Max_6 = 0x266,
            Current_C_Time_MS_6 = 0x267,
            Current_C_Time_DH_6 = 0x268,
            Current_C_Time_YM_6 = 0x269,
            Current_Residual_Max_6 = 0x26A,
            Current_Residual_Time_MS_6 = 0x26B,
            Current_Residual_Time_DH_6 = 0x26C,
            Current_Residual_Time_YM_6 = 0x26D,
            Voltage_A_Max_6 = 0x26E,
            Voltage_A_Time_MS_6 = 0x26F,
            Voltage_A_Time_DH_6 = 0x270,
            Voltage_A_Time_YM_6 = 0x271,
            Voltage_B_Max_6 = 0x272,
            Voltage_B_Time_MS_6 = 0x273,
            Voltage_B_Time_DH_6 = 0x274,
            Voltage_B_Time_YM_6 = 0x275,
            Voltage_C_Max_6 = 0x276,
            Voltage_C_Time_MS_6 = 0x277,
            Voltage_C_Time_DH_6 = 0x278,
            Voltage_C_Time_YM_6 = 0x279,

            Current_A_Max_7 = 0x27A,
            Current_A_Time_MS_7 = 0x27B,
            Current_A_Time_DH_7 = 0x27C,
            Current_A_Time_YM_7 = 0x27D,
            Current_B_Max_7 = 0x27E,
            Current_B_Time_MS_7 = 0x27F,
            Current_B_Time_DH_7 = 0x280,
            Current_B_Time_YM_7 = 0x281,
            Current_C_Max_7 = 0x282,
            Current_C_Time_MS_7 = 0x283,
            Current_C_Time_DH_7 = 0x284,
            Current_C_Time_YM_7 = 0x285,
            Current_Residual_Max_7 = 0x286,
            Current_Residual_Time_MS_7 = 0x287,
            Current_Residual_Time_DH_7 = 0x288,
            Current_Residual_Time_YM_7 = 0x289,
            Voltage_A_Max_7 = 0x28A,
            Voltage_A_Time_MS_7 = 0x28B,
            Voltage_A_Time_DH_7 = 0x28C,
            Voltage_A_Time_YM_7 = 0x28D,
            Voltage_B_Max_7 = 0x28E,
            Voltage_B_Time_MS_7 = 0x28F,
            Voltage_B_Time_DH_7 = 0x290,
            Voltage_B_Time_YM_7 = 0x298,
            Voltage_C_Max_7 = 0x292,
            Voltage_C_Time_MS_7 = 0x293,
            Voltage_C_Time_DH_7 = 0x294,
            Voltage_C_Time_YM_7 = 0x295,

            Current_A_Max_8 = 0x296,
            Current_A_Time_MS_8 = 0x297,
            Current_A_Time_DH_8 = 0x298,
            Current_A_Time_YM_8 = 0x299,
            Current_B_Max_8 = 0x29A,
            Current_B_Time_MS_8 = 0x29B,
            Current_B_Time_DH_8 = 0x29C,
            Current_B_Time_YM_8 = 0x29D,
            Current_C_Max_8 = 0x29E,
            Current_C_Time_MS_8 = 0x29F,
            Current_C_Time_DH_8 = 0x2A0,
            Current_C_Time_YM_8 = 0x2A1,
            Current_Residual_Max_8 = 0x2A2,
            Current_Residual_Time_MS_8 = 0x2A3,
            Current_Residual_Time_DH_8 = 0x2A4,
            Current_Residual_Time_YM_8 = 0x2A5,
            Voltage_A_Max_8 = 0x2A6,
            Voltage_A_Time_MS_8 = 0x2A7,
            Voltage_A_Time_DH_8 = 0x2A8,
            Voltage_A_Time_YM_8 = 0x2A9,
            Voltage_B_Max_8 = 0x2AA,
            Voltage_B_Time_MS_8 = 0x2AB,
            Voltage_B_Time_DH_8 = 0x2AC,
            Voltage_B_Time_YM_8 = 0x2AD,
            Voltage_C_Max_8 = 0x2AE,
            Voltage_C_Time_MS_8 = 0x2AF,
            Voltage_C_Time_DH_8 = 0x2B0,
            Voltage_C_Time_YM_8 = 0x2B1,

            Current_A_Max_9 = 0x2B2,
            Current_A_Time_MS_9 = 0x2B3,
            Current_A_Time_DH_9 = 0x2B4,
            Current_A_Time_YM_9 = 0x2B5,
            Current_B_Max_9 = 0x2B6,
            Current_B_Time_MS_9 = 0x2B7,
            Current_B_Time_DH_9 = 0x2B8,
            Current_B_Time_YM_9 = 0x2B9,
            Current_C_Max_9 = 0x2BA,
            Current_C_Time_MS_9 = 0x2BB,
            Current_C_Time_DH_9 = 0x2BC,
            Current_C_Time_YM_9 = 0x2BD,
            Current_Residual_Max_9 = 0x2BE,
            Current_Residual_Time_MS_9 = 0x2BF,
            Current_Residual_Time_DH_9 = 0x2C0,
            Current_Residual_Time_YM_9 = 0x2C1,
            Voltage_A_Max_9 = 0x2C2,
            Voltage_A_Time_MS_9 = 0x2C3,
            Voltage_A_Time_DH_9 = 0x2C4,
            Voltage_A_Time_YM_9 = 0x2C5,
            Voltage_B_Max_9 = 0x2C6,
            Voltage_B_Time_MS_9 = 0x2C7,
            Voltage_B_Time_DH_9 = 0x2C8,
            Voltage_B_Time_YM_9 = 0x2C9,
            Voltage_C_Max_9 = 0x2CA,
            Voltage_C_Time_MS_9 = 0x2CB,
            Voltage_C_Time_DH_9 = 0x2CC,
            Voltage_C_Time_YM_9 = 0x2CD,

            Address_Diagnosis_Value_End = 0x2CD,
        }
        #endregion
        /// <summary>
        /// 日志
        /// </summary>
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(DiagnosisValue));

        /// <summary>
        /// 诊断参数
        /// </summary>
        private DeviceCache _cache_diagnosis_value
                            = new DeviceCache(MBREG.Address_Diagnosis_Value_End - MBREG.Address_Diagnosis_Value_Start + 1);


        #endregion

    }
}
