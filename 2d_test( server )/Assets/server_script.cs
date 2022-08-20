using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;

public class server_script : MonoBehaviour
{
    private static Socket server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    public static List<clients> client_list = new List<clients>();
    public const int port = 2021;
    public static byte[] buffer = new byte[1024];
    public static string temp_data_for_content;
    public static string Ipaddress = "192.168.202.74";

    public GameObject d;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 10;
        setupserver();
        
    }

    

    

    public void setupserver()
    {

        server_socket.Bind(new IPEndPoint(IPAddress.Any, port));
        server_socket.Listen(2);

        server_socket.BeginAccept(AcceptCallback, null);
        //Console.WriteLine("Server started ");
        Add_item_content("Server started");
        //Add_item_content("Server started");




    }


    public void AcceptCallback(IAsyncResult AR)
    {
        int no_of_clients = 0;
        Socket socket;
        try
        {
            socket = server_socket.EndAccept(AR);
        }
        catch
        {
            return;

        }

        clients client = new clients();
        client.Name = "User " + no_of_clients;
        client.socket = socket;
        client_list.Add(client);
        socket.BeginReceive(buffer, 0, 1024, SocketFlags.None, ReceiveCallback, socket);
        //Console.WriteLine("client connected ");
        Add_item_content("Client connected");
        server_socket.BeginAccept(AcceptCallback, null);


    }

    public void ReceiveCallback(IAsyncResult AR)
    {
        Socket socket = (Socket)AR.AsyncState;
        int received_size = 0;
        try
        {
            received_size = socket.EndReceive(AR);


        }
        catch (SocketException)
        {
            socket.Close();
            return;

        }


        byte[] recb = new byte[received_size];
        Array.Copy(buffer, recb, received_size);
        string Text = Encoding.ASCII.GetString(recb);
        //Console.WriteLine("recieved : " + Text);
        Add_item_content(Text);

        // Broadcasting code 
        //-------------------------------

        broadcast(recb);





        //--------------------
        socket.BeginReceive(buffer, 0, 1024, SocketFlags.None, ReceiveCallback, socket);



    }

    public void broadcast(byte[] data_byte)
    {
        if ((data_byte.Length) != 0)
        {
            foreach (clients c in client_list)
            {
                c.socket.Send(data_byte);
            }
        }


    }

    public  void Add_item_content(string s)
    {
        GameObject temp = Instantiate(d, transform);
        temp.GetComponentInChildren<Text>().text = s;

    }


}

public class clients
{
    public string Name = " Guest ";
    public Socket socket;

}
