using NModbus;
using NModbus.Device;
using System.Net.Sockets;

namespace AnalizorWebApp.Services
{
    public class ModbusService
    {
        private float ToFloat(ushort high, ushort low)
        {
            byte[] bytes =
            {
                (byte)(high >> 8),
                (byte)(high & 0xFF),
                (byte)(low >> 8),
                (byte)(low & 0xFF)
            };

            return BitConverter.ToSingle(bytes.Reverse().ToArray(), 0);
        }

        private float ReadInputFloat(IModbusMaster master, byte unitId, ushort start)
        {
            var r = master.ReadInputRegisters(unitId, start, 2);
            return ToFloat(r[0], r[1]);
        }

        private float ReadHoldingFloat(IModbusMaster master, byte unitId, ushort start)
        {
            var r = master.ReadHoldingRegisters(unitId, start, 4);
            return ToFloat(r[0], r[1]);
        }

        private (IModbusMaster master, TcpClient tcp) CreateMaster(string ip, int port)
        {
            var tcp = new TcpClient
            {
                ReceiveTimeout = 3000,
                SendTimeout = 3000
            };

            tcp.Connect(ip, port);

            var factory = new ModbusFactory();
            var master = factory.CreateMaster(tcp);

            return (master, tcp);
        }

        public (double v1, double v2, double v3,
                double i1, double i2, double i3,
                double p1, double p2, double p3)
            ReadStage1(string ip, int port = 502, byte unitId = 1)
        {
            var (master, tcp) = CreateMaster(ip, port);

            try
            {
                return (
                    ReadInputFloat(master, unitId, 1),
                    ReadInputFloat(master, unitId, 3),
                    ReadInputFloat(master, unitId, 5),

                    ReadInputFloat(master, unitId, 13),
                    ReadInputFloat(master, unitId, 15),
                    ReadInputFloat(master, unitId, 17),

                    ReadInputFloat(master, unitId, 25),
                    ReadInputFloat(master, unitId, 27),
                    ReadInputFloat(master, unitId, 29)
                );
            }
            finally
            {
                tcp.Close();
                tcp.Dispose();
            }
        }

        public double ReadTotalActiveEnergyT1(string ip, int port = 502, byte unitId = 1)
        {
            var (master, tcp) = CreateMaster(ip, port);

            try
            {
                return ReadHoldingFloat(master, unitId, 2801);
            }
            finally
            {
                tcp.Close();
                tcp.Dispose();
            }
        }
    }
}
