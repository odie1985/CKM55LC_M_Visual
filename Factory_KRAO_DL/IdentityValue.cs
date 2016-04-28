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

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                for (int i = 0; i < 6; i++)
                {
                    tmp[i] = Utilities.bcd_to_dec(_rcvBuf[10 + i]);
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

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
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

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
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
            byte[] _deviceNumber = CommandMsg.writeData(address, (byte)CommandMsg.ControlCode.WriteData,(byte)CommandMsg.DataFieldLength.WriteData + 6, MBREG.Device_Number.ToString(), data);

            sp.Write(_deviceNumber, 0, _deviceNumber.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                if (_rcvBuf[8] == 0x94)
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
        /// 读资产管理编码
        /// </summary>
        public int[] Read_Device_AssetCode(byte[] address, SerialPort sp)
        {
            int[] tmp = new int[32];
            byte[] _deviceAssetCode = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Device_AssetCode.ToString());

            sp.Write(_deviceAssetCode, 0, _deviceAssetCode.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                for (int i = 0; i < 32; i++)
                {
                    tmp[i] = Utilities.bcd_to_dec(_rcvBuf[14 + i]);
                }
            }
            return tmp;
        }

        /// <summary>
        /// 写资产管理编码
        /// </summary>
        public bool Write_Device_AssetCode(byte[] address, SerialPort sp, byte[] data)
        {
            byte[] _deviceAssetCode = CommandMsg.writeData(address, (byte)CommandMsg.ControlCode.WriteData, (byte)CommandMsg.DataFieldLength.WriteData + 32, MBREG.Device_AssetCode.ToString(), data);

            sp.Write(_deviceAssetCode, 0, _deviceAssetCode.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                if (_rcvBuf[8] == 0x94)
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
        /// 额定电压
        /// </summary>
        public int[] Device_RatedVoltage(byte[] address, SerialPort sp)
        {
            int[] tmp = new int[6];
            byte[] _deviceRatedVoltage = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Device_RatedVoltage.ToString());

            sp.Write(_deviceRatedVoltage, 0, _deviceRatedVoltage.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                for (int i = 0; i < 6; i++)
                {
                    tmp[i] = Utilities.bcd_to_dec(_rcvBuf[14 + i]);
                }
            }
            return tmp;
        }

        /// <summary>
        /// 额定电流
        /// </summary>
        public int[] In(byte[] address, SerialPort sp)
        {
            int[] tmp = new int[6];
            byte[] _In = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.In.ToString());

            sp.Write(_In, 0, _In.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                for (int i = 0; i < 6; i++)
                {
                    tmp[i] = Utilities.bcd_to_dec(_rcvBuf[14 + i]);
                }
            }
            return tmp;
        }

        /// <summary>
        /// 最大电流
        /// </summary>
        public int[] Inm(byte[] address, SerialPort sp)
        {
            int[] tmp = new int[6];
            byte[] _Inm = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Inm.ToString());

            sp.Write(_Inm, 0, _Inm.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                for (int i = 0; i < 6; i++)
                {
                    tmp[i] = Utilities.bcd_to_dec(_rcvBuf[14 + i]);
                }
            }
            return tmp;
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public int[] Device_ProductModel(byte[] address, SerialPort sp)
        {
            int[] tmp = new int[10];
            byte[] _deviceProductModel = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Device_ProductModel.ToString());

            sp.Write(_deviceProductModel, 0, _deviceProductModel.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                for (int i = 0; i < 10; i++)
                {
                    tmp[i] = Utilities.bcd_to_dec(_rcvBuf[14 + i]);
                }
            }
            return tmp;
        }

        /// <summary>
        /// 读生产日期
        /// </summary>
        public int[] Read_Device_ProductDate(byte[] address, SerialPort sp)
        {
            int[] tmp = new int[10];
            byte[] _device_ProductDate = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Device_ProductDate.ToString());

            sp.Write(_device_ProductDate, 0, _device_ProductDate.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                for (int i = 0; i < 10; i++)
                {
                    tmp[i] = Utilities.bcd_to_dec(_rcvBuf[14 + i]);
                }
            }
            return tmp;
        }

        /// <summary>
        /// 写生产日期
        /// </summary>
        public bool Write_Device_ProductDate(byte[] address, SerialPort sp, byte[] data)
        {
            byte[] _device_ProductDate = CommandMsg.writeData(address, (byte)CommandMsg.ControlCode.WriteData, (byte)CommandMsg.DataFieldLength.WriteData + 10, MBREG.Device_ProductDate.ToString(), data);

            sp.Write(_device_ProductDate, 0, _device_ProductDate.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                if (_rcvBuf[8] == 0x94)
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
        /// 读协议版本号
        /// </summary>
        public int[] Read_Device_ProtocolVersion(byte[] address, SerialPort sp)
        {
            int[] tmp = new int[16];
            byte[] _device_ProtocolVersion = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Device_ProtocolVersion.ToString());

            sp.Write(_device_ProtocolVersion, 0, _device_ProtocolVersion.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                for (int i = 0; i < 16; i++)
                {
                    tmp[i] = Utilities.bcd_to_dec(_rcvBuf[14 + i]);
                }
            }
            return tmp;
        }

        /// <summary>
        /// 写协议版本号
        /// </summary>
        public bool Write_Device_ProtocolVersion(byte[] address, SerialPort sp, byte[] data)
        {
            byte[] _device_ProtocolVersion = CommandMsg.writeData(address, (byte)CommandMsg.ControlCode.WriteData, (byte)CommandMsg.DataFieldLength.WriteData + 16, MBREG.Device_ProtocolVersion.ToString(), data);

            sp.Write(_device_ProtocolVersion, 0, _device_ProtocolVersion.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                if (_rcvBuf[8] == 0x94)
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
        /// 工厂代码
        /// </summary>
        public int[] Device_FactoryCode(byte[] address, SerialPort sp)
        {
            int[] tmp = new int[24];
            byte[] _device_FactoryCode = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Device_FactoryCode.ToString());

            sp.Write(_device_FactoryCode, 0, _device_FactoryCode.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                for (int i = 0; i < 24; i++)
                {
                    tmp[i] = Utilities.bcd_to_dec(_rcvBuf[14 + i]);
                }
            }
            return tmp;
        }

        /// <summary>
        /// 固件版本号
        /// </summary>
        public int[] Device_FW_Version(byte[] address, SerialPort sp)
        {
            int[] tmp = new int[32];
            byte[] _device_FW_Version = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Device_FW_Version.ToString());

            sp.Write(_device_FW_Version, 0, _device_FW_Version.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                for (int i = 0; i < 32; i++)
                {
                    tmp[i] = Utilities.bcd_to_dec(_rcvBuf[14 + i]);
                }
            }
            return tmp;
        }

        /// <summary>
        /// 硬件版本号
        /// </summary>
        public int[] Device_HW_Version(byte[] address, SerialPort sp)
        {
            int[] tmp = new int[32];
            byte[] _device_HW_Version = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Device_HW_Version.ToString());

            sp.Write(_device_HW_Version, 0, _device_HW_Version.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                for (int i = 0; i < 32; i++)
                {
                    tmp[i] = Utilities.bcd_to_dec(_rcvBuf[14 + i]);
                }
            }
            return tmp;
        }

        /// <summary>
        /// 额定剩余电流动作值参数组
        /// </summary>
        public int[] Rated_Residual_Parameter_Group(byte[] address, SerialPort sp)
        {
            int[] tmp = new int[8];
            byte[] _ratedResidual = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Rated_Residual_Parameter_Group.ToString());

            sp.Write(_ratedResidual, 0, _ratedResidual.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                for (int i = 0; i < 8; i++)
                {
                    tmp[i] = Utilities.bcd_to_dec(_rcvBuf[14 + i * 2]);
                    tmp[i] = tmp[i] + Utilities.bcd_to_dec(_rcvBuf[15 + i * 2]) * 100;
                }
            }
            return tmp;
        }

        /// <summary>
        /// 通信波特率
        /// </summary>
        public int Device_BaudRate(byte[] address, SerialPort sp)
        {
            int tmp = 3;
            byte[] _baudRate = CommandMsg.readData(address, (byte)CommandMsg.ControlCode.ReadData, MBREG.Device_BaudRate.ToString());

            sp.Write(_baudRate, 0, _baudRate.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                tmp = Utilities.bcd_to_dec(_rcvBuf[10]);
            }
            return tmp;
        }

        /// <summary>
        /// 写通信波特率
        /// </summary>
        public bool Write_Device_BaudRate(byte[] address, SerialPort sp, int baudrate)
        {
            byte tmp = 0;
            if (baudrate == 600)
            {
                tmp = 1;
            }
            else if(baudrate == 1200)
            {
                tmp = 2;
            }
            else if (baudrate == 2400)
            {
                tmp = 3;
            }
            else if (baudrate == 4800)
            {
                tmp = 4;
            }
            else if (baudrate == 9600)
            {
                tmp = 5;
            }
            else if (baudrate == 19200)
            {
                tmp = 6;
            }
            byte[] _baudRate = CommandMsg.changeBaudRate(address, (byte)CommandMsg.ControlCode.ChangeBaudRate, (byte)CommandMsg.DataFieldLength.ChangeBaudRate, tmp);

            sp.Write(_baudRate, 0, _baudRate.Length);

            if (CommandMsg.receive_frame(ref _rcvBuf, sp) == true)
            {
                if (_rcvBuf[8] == 0x97)
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
            Device_ProtocolVersion = 0x0D040004,
            Device_FactoryCode = 0x0E040004,
            Device_FW_Version = 0x0F040004,
            Device_HW_Version = 0x10040004,
            Rated_Residual_Parameter_Group = 0x11040004,
            Device_BaudRate = 0x03070004,
        }

        #endregion

        #endregion

    }
}
