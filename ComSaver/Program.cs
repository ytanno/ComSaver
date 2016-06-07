using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;

namespace ComSaver
{
	class Program
	{

		static StreamWriter _sw = null;
		static DateTime _beforeTime = DateTime.Now;

		static void Main(string[] args)
		{
			SerialPort mySerialPort = new SerialPort("COM5");

			mySerialPort.BaudRate = 250000;
			mySerialPort.Parity = Parity.None;
			mySerialPort.StopBits = StopBits.One;
			mySerialPort.DataBits = 8;
			mySerialPort.Handshake = Handshake.None;
			mySerialPort.RtsEnable = true;

			mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

			while (true)
			{
				try
				{
					mySerialPort.Open();
					break;
				}
				catch
				{
					System.Threading.Thread.Sleep(1000);
				}
			}


			Console.WriteLine("Press any key to continue...");
			Console.WriteLine();
			Console.ReadKey();
			mySerialPort.Close();
			_sw.Close();
		}

		private static void DataReceivedHandler(
							object sender,
							SerialDataReceivedEventArgs e)
		{
			SerialPort sp = (SerialPort)sender;
			string indata = sp.ReadExisting();


			if(_sw == null)
			{
				var dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\SerialData\";
				if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
				var svFile = dir + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv";
				_sw = new StreamWriter(svFile, false);
			}


			if (indata.Length > 0)
			{
			//	var now = DateTime.Now;
			//	var sub = now.Subtract(_beforeTime);
				_sw.Write(indata);
			//	_beforeTime = now;
			}
		}
	}
}
