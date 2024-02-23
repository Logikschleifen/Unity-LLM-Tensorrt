using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;
using System.Linq;

public class Subtitles : MonoBehaviour
{
    Thread thread;
    public int connectionPort = 25001;
    TcpListener server;
    TcpClient client;
    bool running;

    private string concatenatedOutput = string.Empty;

    public TextMeshProUGUI text;

    bool start = false;
    private float elapsedTime = 0f;
    private int currentIndex = 0;
    string[] stringArray = null;
    private float totalTime;

    private static Subtitles _instance;

    public static Subtitles Instance { get { return _instance; } }

    private string messageToSend = string.Empty;

    //[SerializeField]
    //private float messageCooldown = 1f;
    //private float messageDelay = 0f;

    private bool canSend = true;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Update()
    {
        //messageDelay += Time.deltaTime;

        if (start)
        {
            elapsedTime = 0;
            start = false;
        }
        if (stringArray != null)
        {
            displayText();
        }
        text.text = concatenatedOutput;
    }

    private void displayText()
    {
        elapsedTime += Time.deltaTime;

        float time_window = (totalTime * .9f) / stringArray.Length;
        if (currentIndex < stringArray.Length && elapsedTime >= time_window)
        {
            concatenatedOutput = concatenatedOutput + stringArray[currentIndex] + " ";
            currentIndex++;

            elapsedTime = 0f;
        }

        if (currentIndex >= stringArray.Length && elapsedTime >= .5)
        {
            canSend = true;
            messageToSend = "";
            currentIndex = 0;
            concatenatedOutput = "";
            stringArray = null;
        }
    }

    void Start()
    {
        //Receive on a separate thread so Unity doesn't freeze waiting for data
        ThreadStart ts = new ThreadStart(GetData);
        thread = new Thread(ts);
        thread.Start();
    }

    public void sendMessage(string message)
    {
        if (canSend)
        {
            canSend = false;
            messageToSend = message;
            //messageDelay = 0;
        }
    }

    void GetData()
    {
        // Create the server
        server = new TcpListener(IPAddress.Any, connectionPort);
        server.Start();

        // Create a client to get the data stream
        client = server.AcceptTcpClient();
        print("here");

        // Start listening
        running = true;
        while (running)
        {

            while (messageToSend == "")
            {
                // Wait for a message to send
            }
            print(messageToSend);

            // Read data from the network stream
            NetworkStream nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];

            buffer = Encoding.UTF8.GetBytes(messageToSend);
            //netStream.Write(sendBytes, 0, sendBytes.Length);

            nwStream.Write(buffer, 0, buffer.Length);

            messageToSend = "";
            Connection();
        }
        server.Stop();
    }

    void Connection()
    {
        // Read data from the network stream
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);


        // Decode the bytes into a string
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        print(dataReceived);

        // Make sure we're not getting an empty string
        if (dataReceived != null && dataReceived != "")
        {

            stringArray = dataReceived.Split(' ');

            if (stringArray.Length >= 2)
            {

                string firstRemoved = stringArray[0];


                stringArray = stringArray.Skip(1).ToArray();


                totalTime = float.Parse(firstRemoved);


                start = true;
                //Debug.Log("Recieved");
            }

        }
    }

}
