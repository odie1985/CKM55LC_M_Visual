using System;
using System.IO.Ports;

namespace Factory_KRAO_DL
{
    public class RunValue
    {
        #region �����ֶ�
        private byte[] _rcvBuf = new byte[250];
        #endregion

        #region �¼�����
        #endregion

        #region ����

        /// <summary>
        /// A���ѹ��V��
        /// </summary>
        public float Voltage_A(byte[] address, SerialPort sp)
        {
            int tmp = 0;
            byte[] _voltage = CommandMsg.readData(address, (byte) CommandMsg.ControlCode.ReadData, MBREG.Voltage_A.ToString());

            sp.Write(_voltage, 0, _voltage.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                tmp = Utilities.bcd_to_dec(_rcvBuf[14]);
                tmp = tmp + Utilities.bcd_to_dec(_rcvBuf[15])*100;
                tmp /= 10;
            }
            return tmp;
        }

        /// <summary>
        /// B���ѹ��V��
        /// </summary>
            public
            float Voltage_B(byte[] address, SerialPort sp)
        {
            int tmp = 0;
            byte[] _voltage = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Voltage_B.ToString());

            sp.Write(_voltage, 0, _voltage.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                tmp = Utilities.bcd_to_dec(_rcvBuf[14]);
                tmp = tmp + Utilities.bcd_to_dec(_rcvBuf[15])*100;
                tmp /= 10;
            }

            return tmp;
        }

        /// <summary>
        /// C���ѹ��V��
        /// </summary>
        public float Voltage_C(byte[] address, SerialPort sp)
        {
            int tmp = 0;
            byte[] _voltage = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData,MBREG.Voltage_C.ToString());

            sp.Write(_voltage, 0, _voltage.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                tmp = Utilities.bcd_to_dec(_rcvBuf[14]);
                tmp = tmp + Utilities.bcd_to_dec(_rcvBuf[15]) * 100;
                tmp /= 10;
            }
            return tmp;
        }

        /// <summary>
        /// �����ѹ��V��
        /// </summary>
        public float[] Voltage_All(byte[] address, SerialPort sp)
        {
            float[] tmp = new float[3];
            byte[] _voltage = CommandMsg.readData(address, (byte) CommandMsg.ControlCode.ReadData, MBREG.Voltage_All.ToString());

            sp.Write(_voltage, 0, _voltage.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                tmp[0] = Utilities.bcd_to_dec(_rcvBuf[14]);
                tmp[0] = tmp[0] + Utilities.bcd_to_dec(_rcvBuf[15])*100;
                tmp[0] /= 10;

                tmp[1] = Utilities.bcd_to_dec(_rcvBuf[16]);
                tmp[1] = tmp[1] + Utilities.bcd_to_dec(_rcvBuf[17])*100;
                tmp[1] /= 10;

                tmp[2] = Utilities.bcd_to_dec(_rcvBuf[18]);
                tmp[2] = tmp[2] + Utilities.bcd_to_dec(_rcvBuf[19])*100;
                tmp[2] /= 10;
            }
            return tmp;
        }

        /// <summary>
        /// A�������A��
        /// </summary>
        public float Current_A(byte[] address, SerialPort sp)
        {
            int tmp = 0;
            byte[] _current = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Current_A.ToString());

            sp.Write(_current, 0, _current.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                tmp = Utilities.bcd_to_dec(_rcvBuf[14]);
                tmp = tmp + Utilities.bcd_to_dec(_rcvBuf[15]) * 100;
                tmp = tmp + Utilities.bcd_to_dec(_rcvBuf[16]) * 10000;
                tmp /= 10;
            }
            return tmp;
        }

        /// <summary>
        /// B�������A��
        /// </summary>
        public float Current_B(byte[] address, SerialPort sp)
        {
            int tmp = 0;
            byte[] _current = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Current_B.ToString());

            sp.Write(_current, 0, _current.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                tmp = Utilities.bcd_to_dec(_rcvBuf[14]);
                tmp = tmp + Utilities.bcd_to_dec(_rcvBuf[15]) * 100;
                tmp = tmp + Utilities.bcd_to_dec(_rcvBuf[16]) * 10000;
                tmp /= 10;
            }
            return tmp;
        }

        /// <summary>
        /// C�������A��
        /// </summary>
        public float Current_C(byte[] address, SerialPort sp)
        {
            int tmp = 0;
            byte[] _current = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Current_C.ToString());

            sp.Write(_current, 0, _current.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                tmp = Utilities.bcd_to_dec(_rcvBuf[14]);
                tmp = tmp + Utilities.bcd_to_dec(_rcvBuf[15]) * 100;
                tmp = tmp + Utilities.bcd_to_dec(_rcvBuf[16]) * 10000;
                tmp /= 10;
            }
            return tmp;
        }

        /// <summary>
        /// ���������A��
        /// </summary>
        public float[] Current_All(byte[] address, SerialPort sp)
        {
            float[] tmp = new float[3];
            byte[] _current = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Current_All.ToString());

            sp.Write(_current, 0, _current.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                tmp[0] = Utilities.bcd_to_dec(_rcvBuf[14]);
                tmp[0] = tmp[0] + Utilities.bcd_to_dec(_rcvBuf[15]) * 100;
                tmp[0] = tmp[0] + Utilities.bcd_to_dec(_rcvBuf[16]) * 10000;
                tmp[0] /= 10;

                tmp[1] = Utilities.bcd_to_dec(_rcvBuf[17]);
                tmp[1] = tmp[1] + Utilities.bcd_to_dec(_rcvBuf[18]) * 100;
                tmp[1] = tmp[1] + Utilities.bcd_to_dec(_rcvBuf[19]) * 10000;
                tmp[1] /= 10;

                tmp[2] = Utilities.bcd_to_dec(_rcvBuf[20]);
                tmp[2] = tmp[2] + Utilities.bcd_to_dec(_rcvBuf[21]) * 100;
                tmp[2] = tmp[2] + Utilities.bcd_to_dec(_rcvBuf[22]) * 10000;
                tmp[2] /= 10;
            }
            return tmp;
        }

        /// <summary>
        /// ʣ����������
        /// </summary>
        public float Residual_Phase(byte[] address, SerialPort sp)
        {
            int tmp = 0;
            byte[] _residual = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Residual_Phase.ToString());

            sp.Write(_residual, 0, _residual.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                tmp = _rcvBuf[14];
                
            }
            return tmp;
        }

        /// <summary>
        /// ʣ�������mA��
        /// </summary>
        public int Current_Residual(byte[] address, SerialPort sp)
        {
            int tmp = 0;
            byte[] _residual = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Residual_Current.ToString());

            sp.Write(_residual, 0, _residual.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                tmp = Utilities.bcd_to_dec(_rcvBuf[14]);
                tmp = tmp + Utilities.bcd_to_dec(_rcvBuf[15]) * 100;
            }
            return tmp;
        }

        public int[] Residual_All(byte[] address, SerialPort sp)
        {
            int[] tmp = new int[2];
            byte[] _residual = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Residual_All.ToString());

            sp.Write(_residual, 0, _residual.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                //ʣ����������

                tmp[0] = _rcvBuf[14];

                //ʣ�����ֵ
                tmp[1] = Utilities.bcd_to_dec(_rcvBuf[15]);
                tmp[1] = tmp[1] + Utilities.bcd_to_dec(_rcvBuf[16]) * 100;
            }
            return tmp;
        }

        /// <summary>
        /// �ʣ���������ֵ��mA��
        /// </summary>
        public int Rated_Residual(byte[] address, SerialPort sp)
        {
            int tmp = 0;
            byte[] _residual = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Rated_Residual.ToString());

            sp.Write(_residual, 0, _residual.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                tmp = Utilities.bcd_to_dec(_rcvBuf[14]);
                tmp = tmp + Utilities.bcd_to_dec(_rcvBuf[15]) * 100;
            }
            return tmp;
        }

        /// <summary>
        /// ����޲�����ʱ�䣨ms��
        /// </summary>
        public int Rated_Not_Driving_Time(byte[] address, SerialPort sp)
        {
            int tmp = 0;
            byte[] _residual = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Rated_Not_Driving_Time.ToString());

            sp.Write(_residual, 0, _residual.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                tmp = Utilities.bcd_to_dec(_rcvBuf[14]);
                tmp = tmp + Utilities.bcd_to_dec(_rcvBuf[15]) * 100;
            }
            return tmp;
        }

        public int[] Rated_Residual_All(byte[] address, SerialPort sp)
        {
            int[] tmp = new int[2];
            byte[] _residual = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Rated_Residual_All.ToString());

            sp.Write(_residual, 0, _residual.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                //ʣ����������

                tmp[0] = Utilities.bcd_to_dec(_rcvBuf[14]);
                tmp[0] = tmp[0] + Utilities.bcd_to_dec(_rcvBuf[15]) * 100;

                //ʣ�����ֵ
                tmp[1] = Utilities.bcd_to_dec(_rcvBuf[16]);
                tmp[1] = tmp[1] + Utilities.bcd_to_dec(_rcvBuf[17]) * 100;
            }
            return tmp;
        }

        /// <summary>
        /// ����״̬��1
        /// </summary>
        public string[] Running_Status_1(byte[] address, SerialPort sp)
        {
            string[] tmp = new string[3];
            byte[] _status = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Running_Status1.ToString());

            sp.Write(_status, 0, _status.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                if ((_rcvBuf[14] & 0x80) == 0x80)
                {
                    tmp[0] = "��";
                }
                else
                {
                    tmp[0] = "��";
                }

                if ((_rcvBuf[14] & 0x60) == 0x60)
                {
                    tmp[1] = "��բ";
                }
                else if ((_rcvBuf[14] & 0x60) == 0x00)
                {
                    tmp[1] = "��բ";
                }
                else if ((_rcvBuf[14] & 0x60) == 0x40)
                {
                    tmp[1] = "�غ�բ";
                }
                else
                {
                    tmp[1] = "����";
                }

                if ((_rcvBuf[14] & 0x1F) == 0x00)
                {
                    tmp[2] = "��������";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x02)
                {
                    tmp[2] = "ʣ�����";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x04)
                {
                    tmp[2] = "����";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x05)
                {
                    tmp[2] = "����";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x06)
                {
                    tmp[2] = "��·����ʱ";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x07)
                {
                    tmp[2] = "ȱ��";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x08)
                {
                    tmp[2] = "Ƿѹ";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x09)
                {
                    tmp[2] = "��ѹ";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x0A)
                {
                    tmp[2] = "�ӵ�";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x0B)
                {
                    tmp[2] = "ͣ��";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x0C)
                {
                    tmp[2] = "��ʱ����";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x0D)
                {
                    tmp[2] = "Զ��";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x0E)
                {
                    tmp[2] = "��������";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x0F)
                {
                    tmp[2] = "����";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x12)
                {
                    tmp[2] = "�ֶ�";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x10)
                {
                    tmp[2] = "����������";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x11)
                {
                    tmp[2] = "��բʧ��";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x13)
                {
                    tmp[2] = "���ø���";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x16)
                {
                    tmp[2] = "˲��";
                }
                else if ((_rcvBuf[14] & 0x1F) == 0x17)
                {
                    tmp[2] = "��բʧ��";
                }
                else
                {
                    tmp[2] = "����";
                }
            }
            return tmp;
        }

        /// <summary>
        /// ������4
        /// </summary>
        public string[] Control_Word_4(byte[] address, SerialPort sp)
        {
            string[] tmp = new string[3];
            byte[] _status = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Control_Word4.ToString());

            sp.Write(_status, 0, _status.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                if ((_rcvBuf[14] & 0x80) == 0x80)
                {
                    tmp[0] = "��";
                }
                else
                {
                    tmp[0] = "��";
                }

                //�ʣ���������ֵ
                if ((_rcvBuf[14] & 0xF0) == 0)
                {
                    tmp[0] = "��λ1";
                }
                else if ((_rcvBuf[14] & 0xF0) == 0x10)
                {
                    tmp[0] = "��λ2";
                }
                else if ((_rcvBuf[14] & 0xF0) == 0x20)
                {
                    tmp[0] = "��λ3";
                }
                else if ((_rcvBuf[14] & 0xF0) == 0x30)
                {
                    tmp[0] = "��λ4";
                }
                else if ((_rcvBuf[14] & 0xF0) == 0x40)
                {
                    tmp[0] = "��λ5";
                }
                else if ((_rcvBuf[14] & 0xF0) == 0x50)
                {
                    tmp[0] = "��λ6";
                }
                else if ((_rcvBuf[14] & 0xF0) == 0x60)
                {
                    tmp[0] = "��λ7";
                }
                else if ((_rcvBuf[14] & 0xF0) == 0x70)
                {
                    tmp[0] = "��λ8";
                }
                else if ((_rcvBuf[14] & 0xF0) == 0xF0)
                {
                    tmp[0] = "�����ɵ�";
                }
                else
                {
                    tmp[0] = "����";
                }

                //����޲�����ʱ��
                if ((_rcvBuf[14] & 0x0C) == 0)
                {
                    tmp[1] = "��λ1";
                }
                else if ((_rcvBuf[14] & 0x0C) == 0x04)
                {
                    tmp[1] = "��λ2";
                }
                else if ((_rcvBuf[14] & 0x0C) == 0x08)
                {
                    tmp[1] = "��λ3";
                }
                else
                {
                    tmp[1] = "�����ɵ�";
                }

                //ʣ���������ʱ��
                if ((_rcvBuf[14] & 0x03) == 0)
                {
                    tmp[12] = "�ر�";
                }
                else if ((_rcvBuf[14] & 0x0C) == 0x01)
                {
                    tmp[12] = "����24Сʱ";
                }
                else if ((_rcvBuf[14] & 0x0C) == 0x02)
                {
                    tmp[12] = "��������";
                }
                else
                {
                    tmp[12] = "����";
                }
            }
            return tmp;
        }
        #endregion

        #region ���캯��
        #endregion

        #region ���з���
        #endregion

        #region ��������

        #endregion

        #region �����ֶ�

        #region modbus �Ĵ�������
        private enum MBREG : int
        {
            Voltage_A = 0x00010102,
            Voltage_B = 0x00020102,
            Voltage_C = 0x00030102,
            Voltage_All = 0x00FF0102,

            Current_A = 0x00010202,
            Current_B = 0x00020202,
            Current_C = 0x00030202,
            Current_All = 0x00FF0202,

            Residual_Phase = 0x00009002,
            Residual_Current = 0x00019002,
            Residual_All = 0x00FF9002,

            Rated_Residual = 0x00019102,
            Rated_Not_Driving_Time = 0x00029102,
            Rated_Residual_All = 0x00FF9102,

            Running_Status1 = 0x01050004,
            Control_Word4 = 0x04100004,
        }
        #endregion

        #endregion

    }
}
