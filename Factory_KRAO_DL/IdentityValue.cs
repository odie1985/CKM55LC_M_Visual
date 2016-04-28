using System;
using System.IO.Ports;

namespace Factory_KRAO_DL
{
    class IdentityValue
    {
        #region 公有字段
        private byte[] _rcvBuf = new byte[250];
        #endregion

        #region 事件定义
        #endregion

        #region 属性

        /// <summary>
        /// 读通讯地址
        /// </summary>
        public int[] Read_Device_Address(SerialPort sp)
        {
            int[] tmp = new int[6];
            byte[] _readAddr = CommandMsg.readAddress((byte)CommandMsg.ControlCode.ReadAddr,(byte)CommandMsg.DataFieldLength.ReadAddr);

            sp.Write(_readAddr, 0, _readAddr.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf) == true)
            {
                for (int i = 0; i < 6; i++)
                {
                    tmp[6 - 1 - i] = Utilities.bcd_to_dec(_rcvBuf[10 + i]);
                }
            }
            return tmp;
        }

        /// <summary>
        /// 写通讯地址
        /// </summary>
        public bool Write_Device_Address(byte[] address, SerialPort sp)
        {
            byte[] _writeAddr = CommandMsg.writeAddress(Utilities.dec_to_bcd(address), (byte)CommandMsg.ControlCode.WriteAddr,(byte)CommandMsg.DataFieldLength.WriteAddr);
            sp.Write(_writeAddr, 0, _writeAddr.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf) == true)
            {
                if (_rcvBuf[8] == 0x95)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 读设备号
        /// </summary>
        public int[] Read_Device_Number(byte[] address, SerialPort sp)
        {
            int[] tmp = new int[6];
            byte[] _deviceNumber = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Device_Number.ToString());

            sp.Write(_deviceNumber, 0, _deviceNumber.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf) == true)
            {
                for (int i = 0; i < 6; i++)
                {
                    tmp[i] = Utilities.bcd_to_dec(_rcvBuf[14 + i]);
                }
            }
            return tmp;
        }

        /// <summary>
        /// 写设备号
        /// </summary>
        public bool Write_Device_Number(byte[] address, SerialPort sp, byte[] data)
        {
            byte[] _deviceNumber = CommandMsg.writeData(address, (byte)CommandMsg.ControlCode.ReadData,(byte)CommandMsg.DataFieldLength.WriteAddr + 6, MBREG.Device_Number.ToString(), data);

            sp.Write(_deviceNumber, 0, _deviceNumber.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf) == true)
            {
                if (_rcvBuf[8] == 0x95)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region 构造函数
        #endregion

        #region 公有方法
        #endregion

        #region 保护方法

        #endregion

        #region 保护字段

        #region modbus 寄存器定义

        private enum MBREG : int
        {
            Device_Address = 0x01040004,
            Device_Number = 0x02040004,
            Device_AssetCode = 0x03040004,
            Device_RatedVoltage = 0x04040004,
            In = 0x05040004,
            Inm = 0x06040004,
            Device_ProductModel = 0x0B040004,
            Device_ProductDate = 0x0C040004,
            Device_FactoryCode = 0x0E040004,
            Device_FW_Version = 0x0F040004,
            Device_HW_Version = 0x10040004,
            Rated_Residual_Parameter_Group = 0x11040004,
        }

        #endregion

        #endregion

    }
}
