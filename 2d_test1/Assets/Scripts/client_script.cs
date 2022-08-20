using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;
using System;

public class client_script : MonoBehaviour
{
    private static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    static string ip_address = "192.168.202.74";

    public static string temp_buffer="";

    public  GameObject d;
    public static bool iscontentAvailable = false;
    public static string temp_content = "";

    public InputMessage inputobject;
    public bool isConnected = false;

    public static byte[] recBuff = new byte[1024];
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 10;
        connect_to_server();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (iscontentAvailable)
        {
            Add_data(temp_content);
            temp_content = "";
            iscontentAvailable = false;
        }
        if ( inputobject.inputMessage.Length!=0)
        {
            send_data();
        }

        
        
    }

    public  void connect_to_server()
    {
        try
        {
            socket.Connect(ip_address, 2021);
        }
        catch (SocketException)
        {
            Add_data("Error occured while connecting");
            return;
        }

        //Console.WriteLine("Connected to server");
        Debug.Log("connected to server");
        Add_data("Connected to server ");
        isConnected = true;

        // create threads for sending and recieving
        //  Thread thread = new Thread(new ThreadStart(() => this.handle(client)));

        //Thread send_thread = new Thread(new ThreadStart(() => send_data()));
        //Thread rec_thread = new Thread(new ThreadStart(() => recieve_data()));

        //send_thread.Start();
        //rec_thread.Start();

        socket.BeginReceive(recBuff, 0, 1024, SocketFlags.None, ReceiveCallback, socket);




    }
    public static void ReceiveCallback(IAsyncResult AR)
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
        Array.Copy(recBuff, recb, received_size);
        string data = Encoding.ASCII.GetString(recb);
        //Console.WriteLine("recieved : " + Text);

        if (data.Length != 0)
        {
            //Add_data(data);
            temp_content = "";
            temp_content = data;
            iscontentAvailable = true;
            Debug.Log("Received data is " + data);
        }

        // Broadcasting code 
        //-------------------------------







        //--------------------
        socket.BeginReceive(recBuff, 0, 1024, SocketFlags.None, ReceiveCallback, socket);

    }

    public  void send_data()
    {
        string data;
       // while (true)
        //{
        data = "";
        data = inputobject.inputMessage;
        //inputobject.inputMessage = "";

        //Debug.Log("to send Data " + data);


        // clear temp_buffer after delivering message  and use for later 

        if (data.Length != 0)
        {

            byte[] buff = Encoding.ASCII.GetBytes(data);
            socket.Send(buff, 0, buff.Length, SocketFlags.None);
            //Add_data("Send data is " + data);
            //temp_content = data;
            Debug.Log("Send data is " + data);
            //temp_buffer = "";
            data = "";
            inputobject.inputMessage = "";
        }
            
        //}
    }

    public void Add_data(string s)
    {
        GameObject temp_g = Instantiate(d, transform);
        temp_g.GetComponentInChildren<Text>().text = s;
        //g.Add(temp_g);

    }

    public  void recieve_data()
    {
        
        while (true)
        {
            byte[] buff = new byte[1024];
            int rec_size = socket.Receive(buff, SocketFlags.None);
            byte[] data_byte = new byte[rec_size];
            Array.Copy(buff, data_byte, rec_size);
            //buff.CopyTo(data_byte,0);

            string data = Encoding.ASCII.GetString(data_byte);
            //Console.WriteLine(data);
            //Debug.Log("Received data is "+data);
            if (data.Length != 0)
            {
                //Add_data(data);
                temp_content = "";
                temp_content = data;
                iscontentAvailable = true;
                Debug.Log("Received data is " + data);
            }
            
            
        }
    }

    public  void get_data_from_client( string s)
    {
        temp_buffer = "";
        temp_buffer=s;
        Debug.Log("temp_buffer is " + s);

        Debug.Log("temp_buffer is "+temp_buffer);
        
    }
    //public static void Add_data1(string s)
    //{
    //    GameObject temp_g = Instantiate(d, transform);
    //    temp_g.GetComponentInChildren<Text>().text = s;
    //    //g.Add(temp_g);

    //}
}
