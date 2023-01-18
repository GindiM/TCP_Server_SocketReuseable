using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

// ...

IPAddress localAddr = IPAddress.Any;
int port = 10000;

TcpListener server = new TcpListener(localAddr, port);

// Enable socket reuse
server.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

server.Start();

while (true)
{
    Console.WriteLine("Waiting for a connection...");
    TcpClient client = server.AcceptTcpClient();
    Console.WriteLine("Connected!");
    // Do something with the client here
    new Thread(() => Read(client)).Start();

}

void Read(TcpClient client)
{
    string ip;
    ip = "";

    Stream stream = client.GetStream();
    while (true)
    {
        
        string requestString = "empty Message";

        // Read data from the client
        byte[] request = new byte[1024];
        try
        {
            int bytesRead = stream.Read(request, 0, request.Length);
            requestString = Encoding.UTF8.GetString(request, 0, bytesRead);
            stream.Flush();

            Console.WriteLine("user " + ip + ": " + requestString);
        }
        catch (Exception)
        {
            Console.WriteLine("Reader: client is not available");

            client.Close();

            return;
        }

        // Send a response to the client
        byte[] response = Encoding.UTF8.GetBytes("Server: you said \"" + requestString + "\"");
        stream.Write(response, 0, response.Length);
        stream.Flush();
    }
}
