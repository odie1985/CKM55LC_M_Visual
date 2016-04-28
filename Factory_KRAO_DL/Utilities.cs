using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Factory_KRAO_DL
{
    public class Utilities
    {
        #region �Ĵ���ֵת������
        /// <summary>
        /// BCD��ת16����
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
        /// 10����תBCD
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

        #region �ַ���
        // �����ַ�������˳��Ҫ��
        /// <summary>
        /// ��������
        /// </summary>
        public static readonly string[] Str_Fault = new string[]
        {
            "�޼�¼",
            "˲ʱ����",
            "����ʱ����",
            "����ʱ����",
            "ʣ���������",
            "��ƽ���������",
            "ȱ�����",
            "�������",
            "����ѹ����",
            "Ƿ��ѹ����",
            "��е����",
            "©����Ȧ����",
        };
										
        /// <summary>
        /// SOE����
        /// </summary>
        public static readonly string[] Str_SOE = new string[]
        {											
            "�޼�¼",
            "��բ",
            "��բ",
            "��λ",
            "����",
            "��������",
            "�����޸�",
			"Ԥ��", 
			"ȡ��Ԥ��", 
			"�ϵ��Զ���բ",
			"©�籣���غ�բ",
			"��ѹ�����غ�բ",
			"Ƿѹ�����غ�բ",
        };

        public static readonly string[] Str_LOG = new string[] 
        { 
            "�޼�¼",
        };
										
        public static readonly string[] Str_Protect_Enable = new string[]
        {
            "��·����",
            "��������",
            "���ع���",
            "ʣ���������",
            "��ƽ���������",
            "�������",
            "%��%",
            "��ѹ����",
            "Ƿѹ����",
            "��е����",
            "��Ȧ����",
            "%��%",
            "%��%",
            "%��%",
            "��״̬Ԥ��",
            "���ؾ���",
        };

        public static readonly string[] Str_Protect_Mode = new string[]
        {
            "�ر�",
            "����",
            "�ѿ�",
            "����+�ѿ�",
        };

        public static readonly string[] Str_Reclosing_Enable = new string[]
        {
            "��·����",
            "��������",
            "���ع���",
            "ʣ���������",
            "��ƽ���������",
            "�������",
            "NLost",
            "��ѹ����",
            "Ƿѹ����",
            "%��%",
            "%��%",
            "%��%",
            "%��%",
            "%��%",
            "%��%",
            "�ϵ�",
        };
        #endregion
    }
}
