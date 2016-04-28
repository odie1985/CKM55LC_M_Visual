using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace Factory_KRAO_DL
{
    class SerialComm
    {
        private SerialPort sp = new SerialPort();
        public string modbusStatus;

        #region Open / Close Procedures
        public bool Open(string portName, int baudRate, int dataBits, Parity parity, StopBits stopBits)
        {
            //Ensure port isn't already opened:
            if (!sp.IsOpen)
            {
                //Assign desired settings to the serial port:
                sp.PortName = portName;
                sp.BaudRate = baudRate;
                sp.DataBits = dataBits;
                sp.Parity = parity;
                sp.StopBits = stopBits;
                sp.ReadTimeout = 200;
                sp.WriteTimeout = 200;

                try
                {
                    sp.Open();
                }
                catch (Exception err)
                {
                    modbusStatus = "Error opening " + portName + ": " + err.Message;
                    return false;
                }
                modbusStatus = portName + " opened successfully";
                return true;
            }
            else
            {
                modbusStatus = portName + " already opened";
                return false;
            }
        }

        public bool Open(string portName, int baudRate, int dataBits, Parity parity, StopBits stopBits, int timeoutReadms, int timeoutWritems)
        {
            //Ensure port isn't already opened:
            if (!sp.IsOpen)
            {
                //Assign desired settings to the serial port:
                sp.PortName = portName;
                sp.BaudRate = baudRate;
                sp.DataBits = dataBits;
                sp.Parity = parity;
                sp.StopBits = stopBits;
                sp.ReadTimeout = timeoutReadms;//SerialPort.ReadTimeout属性  获取或设置读取操作未完成时发生超时之前的毫秒数
                sp.WriteTimeout = timeoutWritems;//SerialPort.WriteTimeout属性 获取或设置写入操作未完成时发生超时之前的毫秒数

                try
                {
                    sp.Open();
                }
                catch (Exception err)
                {
                    modbusStatus = "Error opening " + portName + ": " + err.Message;
                    return false;
                }
                modbusStatus = portName + " opened successfully";
                return true;
            }
            else
            {
                modbusStatus = portName + " already opened";
                return false;
            }
        }

        public bool Close()
        {
            //Ensure port is opened before attempting to close:
            if (sp.IsOpen)
            {
                try
                {
                    sp.Close();
                }
                catch (Exception err)
                {
                    modbusStatus = "Error closing " + sp.PortName + ": " + err.Message;
                    return false;
                }
                modbusStatus = sp.PortName + " closed successfully";
                return true;
            }
            else
            {
                modbusStatus = sp.PortName + " is not open";
                return false;
            }
        }
        #endregion

    }
}
