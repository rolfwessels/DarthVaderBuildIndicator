using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace BuildIndicatron.Core.Helpers
{
    public static class IpAddressHelper
    {
        public static IEnumerable<string> GetLocalIpAddresses()
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
    }
}