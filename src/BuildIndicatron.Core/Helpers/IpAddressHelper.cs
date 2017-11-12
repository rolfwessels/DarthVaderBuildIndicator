using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace BuildIndicatron.Core.Helpers
{
    public static class IpAddressHelper
    {
        public static IEnumerable<string> GetLocalIpAddresses()
        {
            return ViaLookup()
                .Concat(GetIpAddressViaSockets())
                .Distinct()
                .Where(x => x != null);
        }

        private static IEnumerable<string> ViaLookup()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    yield return ip.ToString();
                }
            }
        }

        public static IEnumerable<string> GetIpAddressViaSockets()
        {

            string localIP = null;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                if (endPoint != null) localIP = endPoint.Address.ToString();
            }
            yield return localIP;
        }
    }
}