using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace Factory_KRAO_DL
{
    class CommandMsg
    {
        /// <summary>
        /// 控制码
        /// </summary>
        internal enum ControlCode : byte
        {
            Broadcasting = 0x08,
            ReadData = 0x11,
            ReadFollowUpData = 0x12,
            ReadAddr = 0x13,
            WriteData = 0x14,
            WriteAddr = 0x15,
            ChangeBaudRate = 0x17,
            ChangePwd = 0x18,
            Alarm = 0x19,
            AlarmResponse = 0x1A,
            EventCleared = 0x1B,
            Control = 0x1C
        }

        /// <summary>
        /// 数据域长度
        /// </summary>
        internal enum DataFieldLength : byte
        {
            ReadData = 0x04,
            ReadAddr = 0x00,
            WriteData = 0x0C,
            WriteAddr = 0x06,
            ChangeBaudRate = 0x01,
            ChangePwd = 0x0C,
            Alarm = 0x00,
            EventCleared = 0x0C,
            Control = 0x0D
        }

        /// <summary>
        /// 波特率特征字
        /// </summary>
        internal enum BaudRate : byte
        {
            bps_600 = 0x01,
            bps_1200 = 0x02,
            bps_2400 = 0x03,
            bps_4800 = 0x04,
            bps_9600 = 0x05,
            bps_19200 = 0x06
        }

        #region Build Message
        public static bool receive_frame(ref byte[] rcv, SerialPort sp)
        {
            int state = 0;
            int j = 0;
            byte tmp = 0;

            // receive byte
            for (int i = 0; i < rcv.Length; i++)
            {
                tmp = (byte)(sp.ReadByte());

                if (tmp == 0x68)
                {
                    state = 1;
                }

                if (state == 1)
                {
                    rcv[j++] = tmp;
                }

                if (tmp == 0x16 && rcv[9] + 9 == j - 3)
                {
                    break;
                }
            }

            // check cs
            tmp = 0;
            for (int i = 0; i < j - 2; i++)
            {
                tmp += rcv[i];
            }

            if (tmp == rcv[j - 2])
            {
                demodulation_frame_data_area(ref rcv);

                return true;
            }
            else
            {
                return false;
            }
        }

        private static void demodulation_frame_data_area(ref byte[] rcv)
        {
            int len = rcv[9];
            int i;

            for (i = 0; i < len; i++)
            {
                rcv[10 + i] -= 0x33;
            }
        }

        /// <summary>
        /// 读数据
        /// </summary>
        public static byte[] readData(byte[] address, byte controlCode, string DID)
        {
            byte[] send = buildMsg(controlCode);

            send[0] = send[7] = 0x68;

            for (int m = 0; m < 6; m++)
            {
                send[1 + m] = address[m];
            }

            send[8] = controlCode;
            send[9] = 0x04;

            send[10] = Convert.ToByte(DID.Substring(2, 2), 16);
            send[10] += 0x33;
            send[11] = Convert.ToByte(DID.Substring(4, 2), 16);
            send[11] += 0x33;
            send[12] = Convert.ToByte(DID.Substring(6, 2), 16);
            send[12] += 0x33;
            send[13] = Convert.ToByte(DID.Substring(8, 2), 16);
            send[13] += 0x33;

            send[send.Length - 2] = 0;
            for (int i = 0; i < send.Length - 2; i++)
            {
                send[send.Length - 2] += send[i];
            }

            send[send.Length - 1] = 0x16;

            return send;
        }

        /// <summary>
        /// 读通信地址
        /// </summary>
        public static byte[] readAddress(byte controlCode, byte dataFieldLength)
        {
            byte[] send = buildMsg(controlCode);

            send[0] = send[7] = 0x68;

            send[1] = send[2] = send[3] = send[4] = send[5] = send[6] = 0xAA;

            send[8] = controlCode;
            send[9] = dataFieldLength;

            send[send.Length - 2] = 0;
            for (int i = 0; i < send.Length - 2; i++)
            {
                send[send.Length - 2] += send[i];
            }

            send[send.Length - 1] = 0x16;

            return send;
        }

        /// <summary>
        /// 写数据
        /// </summary>
        public static byte[] writeData(byte[] address, byte controlCode, byte dataFieldLength, string DID, byte[] data)
        {
            byte[] send = new byte[12 + dataFieldLength];

            send[0] = send[7] = 0x68;

            for (int m = 0; m < 6; m++)
            {
                send[1 + m] = address[m];
            }

            send[8] = controlCode;
            send[9] = dataFieldLength;

            send[10] = Convert.ToByte(DID.Substring(2, 2), 16);
            send[10] += 0x33;
            send[11] = Convert.ToByte(DID.Substring(4, 2), 16);
            send[11] += 0x33;
            send[12] = Convert.ToByte(DID.Substring(6, 2), 16);
            send[12] += 0x33;
            send[13] = Convert.ToByte(DID.Substring(8, 2), 16);
            send[13] += 0x33;

            send[14] = send[18] = 0x4B + 0x33;     //K
            send[15] = send[19] = 0x52 + 0x33;     //A
            send[16] = send[20] = 0x41 + 0x33;     //R
            send[17] = send[21] = 0x4F + 0x33;     //O

            for (int i = 0; i < data.Length; i++)
            {
                send[22 + i] = (byte)(data[data.Length - 1 - i] + 0x33);
            }

            send[send.Length - 2] = 0;
            for (int i = 0; i < send.Length - 2; i++)
            {
                send[send.Length - 2] += send[i];
            }

            send[send.Length - 1] = 0x16;

            return send;
        }

        /// <summary>
        /// 写通信地址
        /// </summary>
        public static byte[] writeAddress(byte[] address, byte controlCode, byte dataFieldLength)
        {
            byte[] send = buildMsg(controlCode);

            send[0] = send[7] = 0x68;

            send[1] = send[2] = send[3] = send[4] = send[5] = send[6] = 0xAA;

            send[8] = controlCode;
            send[9] = dataFieldLength;

            for (int m = 0; m < 6; m++)
            {
                send[10 + m] = address[m];
                send[10 + m] += 0x33;
            }

            send[send.Length - 2] = 0;
            for (int i = 0; i < send.Length - 2; i++)
            {
                send[send.Length - 2] += send[i];
            }

            send[send.Length - 1] = 0x16;

            return send;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        public static byte[] changePassword(byte[] address, byte controlCode, byte dataFieldLength, string DID, byte[] password)
        {
            byte[] send = buildMsg(controlCode);

            send[0] = send[7] = 0x68;

            for (int m = 0; m < 6; m++)
            {
                send[1 + m] = address[m];
            }

            send[8] = controlCode;
            send[9] = dataFieldLength;

            send[10] = Convert.ToByte(DID.Substring(2, 2), 16);
            send[10] += 0x33;
            send[11] = Convert.ToByte(DID.Substring(4, 2), 16);
            send[11] += 0x33;
            send[12] = Convert.ToByte(DID.Substring(6, 2), 16);
            send[12] += 0x33;
            send[13] = Convert.ToByte(DID.Substring(8, 2), 16);
            send[13] += 0x33;

            send[14] = 0x4B + 0x33;
            send[15] = 0x52 + 0x33;
            send[16] = 0x41 + 0x33;
            send[17] = 0x4F + 0x33;

            for (int i = 0; i < password.Length; i++)
            {
                send[18 + i] = password[i];
            }

            send[send.Length - 2] = 0;
            for (int i = 0; i < send.Length - 2; i++)
            {
                send[send.Length - 2] += send[i];
            }

            send[send.Length - 1] = 0x16;

            return send;
        }

        /// <summary>
        /// 更改通信速率
        /// </summary>
        public static byte[] changeBaudRate(byte[] address, byte controlCode, byte dataFieldLength, byte bps)
        {
            byte[] send = buildMsg(controlCode);

            send[0] = send[7] = 0x68;

            for (int m = 0; m < 6; m++)
            {
                send[1 + m] = address[m];
            }

            send[8] = controlCode;
            send[9] = dataFieldLength;

            send[10] = bps;

            send[send.Length - 2] = 0;
            for (int i = 0; i < send.Length - 2; i++)
            {
                send[send.Length - 2] += send[i];
            }

            send[send.Length - 1] = 0x16;

            return send;
        }

        /// <summary>
        /// 控制指令
        /// </summary>
        public static byte[] controlCommand(byte[] address, byte controlCode, byte dataFieldLength, string DID)
        {
            byte[] send = buildMsg(controlCode);

            send[0] = send[7] = 0x68;

            for (int m = 0; m < 6; m++)
            {
                send[1 + m] = address[m];
            }

            send[8] = controlCode;
            send[9] = dataFieldLength;

            send[10] = Convert.ToByte(DID.Substring(2, 2), 16);
            send[10] += 0x33;
            send[11] = Convert.ToByte(DID.Substring(4, 2), 16);
            send[11] += 0x33;
            send[12] = Convert.ToByte(DID.Substring(6, 2), 16);
            send[12] += 0x33;
            send[13] = Convert.ToByte(DID.Substring(8, 2), 16);
            send[13] += 0x33;

            send[14] = send[18] = 0x4B + 0x33;     //K
            send[15] = send[19] = 0x52 + 0x33;     //A
            send[16] = send[20] = 0x41 + 0x33;     //R
            send[17] = send[21] = 0x4F + 0x33;     //O

            send[22] = 0x33;

            send[send.Length - 2] = 0;
            for (int i = 0; i < send.Length - 2; i++)
            {
                send[send.Length - 2] += send[i];
            }

            send[send.Length - 1] = 0x16;

            return send;
        }

        public static byte[] buildMsg(byte controlCode)
        {
            int msgLength = 0;
            switch (controlCode)
            {
                case (byte)ControlCode.ReadData:
                    msgLength = 16;
                    break;
                case (byte)ControlCode.ReadAddr:
                    msgLength = 12;
                    break;
                case (byte)ControlCode.WriteAddr:
                    msgLength = 18;
                    break;
                case (byte)ControlCode.ChangeBaudRate:
                    msgLength = 13;
                    break;
                case (byte)ControlCode.ChangePwd:
                    msgLength = 24;
                    break;
                case (byte)ControlCode.Control:
                    msgLength = 25;
                    break;
            }
            byte[] msg = new byte[msgLength];

            return msg;
        }

        #endregion

    }
}
