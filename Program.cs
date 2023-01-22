using reuse;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

//Client list
Dictionary<string, ClientDetails> clientList = new Dictionary<string, ClientDetails>(); //hash id, clientDetails obj

IPAddress localAddr = IPAddress.Any;
int port = 10000;

TcpListener server = new TcpListener(localAddr, port);

// Enable socket reuse
server.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

server.Start();

while (true)
{
    Console.WriteLine("Waiting for a connection on port " + port);
    TcpClient client = server.AcceptTcpClient();
    //Console.WriteLine("Connected!");



    //save additional client's data in ClientDetail
    ClientDetails clientDetails = new ClientDetails(client);
    clientList.Add(clientDetails.id, clientDetails);

    //Read Client's stream
    new Thread(() => Read(clientDetails)).Start();

    //write details to console
    Console.WriteLine("Connected: " + clientDetails.id + " " + clientDetails.ipv4 + " " + clientDetails.name);


    Console.WriteLine("\n\nclients connected:");
    foreach (var c in clientList)
    {
        Console.WriteLine(c.Key + " " + c.Value.name);
    }
    Console.WriteLine("\n\n");


}

void Read(ClientDetails clientDetails)
{
    TcpClient client = clientDetails.client;
    string name = clientDetails.name;
    string ip = clientDetails.ipv4;

    Stream stream = client.GetStream();
    while (true)
    {

        string requestString = "Empty Message";

        // Read data from the client
        byte[] request = new byte[1024];
        try
        {
            int bytesRead = stream.Read(request, 0, request.Length);
            requestString = Encoding.UTF8.GetString(request, 0, bytesRead);
            stream.Flush();

            Console.WriteLine("user " + clientDetails.id + ": " + requestString);
            //Console.WriteLine(name + " " + ip + ": " + requestString);
        }
        catch (Exception)
        {
            Console.WriteLine("Reader: client is not available");

            client.Close();

            return;
        }

        // Send a response to the client
        byte[] response = Encoding.UTF8.GetBytes("Server: you said: " + requestString + "\n");
        stream.Write(response, 0, response.Length);
        stream.Flush();
    }
}





//Client side:
//--First message as connected--:
//username|password|name|ipv4|ipv6

//--A command--:
//from id|to id|command


//Command-Queue()                 [ cmd|from|to , cmd|from|to , cmd|from|to ]

//Command-Queue Manager()         adds and pops commands from streams to queue and from queue to command-managaer

//Command-Manager()               received commands are routed to other clients or server's functions



