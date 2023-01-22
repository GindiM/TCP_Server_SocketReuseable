using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace reuse
{
    internal class ClientDetails
    {
        public TcpClient client { get; set; }
        public string name { get; set; }
        public string id { get;}
        public string ipv4 { get;}
        public string ipv6 { get;}

        public ClientDetails(TcpClient client)
        {
            this.client = client;
            this.name = "name"; //client's decision 
            this.id = client.GetHashCode().ToString(); 
            this.ipv4 = GetClientIpv4(client); //get client's ipv4
            //this.ipv6 = "2001::1"; //get client's ipv6
        }
    
        public string GetClientIpv4(TcpClient client)
        {
            IPEndPoint remoteIpEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
            IPAddress clientIp = remoteIpEndPoint.Address;
            return clientIp.ToString();

        }

    }
}
