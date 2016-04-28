using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Factory_KRAO_DL
{
    public class Utilities
    {
        #region 寄存器值转换方法
        /// <summary>
        /// BCD码转16进制
        /// </summary>
        public static int bcd_to_dec(byte data)
        {
            int a, b, dec;
            a = data >> 4;
            b = data & 0x0F;
            dec = (a * 10) + b;
            return dec;
        }

        /// <summary>
        /// 10进制转BCD
        /// </summary>
        public static byte[] dec_to_bcd(byte[] data)
        {
            int a, b;
            byte[] bcd = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                a = (data[i] / 10) << 4;
                b = (data[i] % 10) & 0x0F;
                bcd[i] = (byte)(a + b);
            }

            return bcd;
        }

        #endregion

        #region 字符串
        // 以下字符串都有顺序要求
        /// <summary>
        /// 故障类型
        /// </summary>
        public static readonly string[] Str_Fault = new string[]
        {
            "无记录",
            "瞬时故障",
            "短延时故障",
            "长延时故障",
            "剩余电流故障",
            "不平衡电流故障",
            "缺相故障",
            "断零故障",
            "过电压故障",
            "欠电压故障",
            "机械故障",
            "漏电线圈故障",
        };
										
        /// <summary>
        /// SOE类型
        /// </summary>
        public static readonly string[] Str_SOE = new string[]
        {											
            "无记录",
            "合闸",
            "分闸",
            "复位",
            "试验",
            "清零热容",
            "参数修改",
			"预警", 
			"取消预警", 
			"上电自动合闸",
			"漏电保护重合闸",
			"过压保护重合闸",
			"欠压保护重合闸",
        };

        public static readonly string[] Str_LOG = new string[] 
        { 
            "无记录",
        };
										
        public static readonly string[] Str_Protect_Enable = new string[]
        {
            "短路故障",
            "过流故障",
            "过载故障",
            "剩余电流故障",
            "不平衡电流故障",
            "断相故障",
            "%？%",
            "过压故障",
            "欠压故障",
            "机械故障",
            "线圈故障",
            "%？%",
            "%？%",
            "%？%",
            "打开状态预警",
            "过载警告",
        };

        public static readonly string[] Str_Protect_Mode = new string[]
        {
            "关闭",
            "报警",
            "脱扣",
            "报警+脱扣",
        };

        public static readonly string[] Str_Reclosing_Enable = new string[]
        {
            "短路故障",
            "过流故障",
            "过载故障",
            "剩余电流故障",
            "不平衡电流故障",
            "断相故障",
            "NLost",
            "过压故障",
            "欠压故障",
            "%？%",
            "%？%",
            "%？%",
            "%？%",
            "%？%",
            "%？%",
            "上电",
        };
        #endregion
    }
}
