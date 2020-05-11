// Trevor Chapman, Xiaoyun Ding
// PS9
// 12/9/2015
// version 1.0

using System;
using System.Net.Sockets;
using System.Text;

namespace AgCubio
{
    /// <summary>
    /// Contains data used for passing between a callback function in the Network_Controller or passing data back to an outside class
    /// </summary>
    public class State
    {
        /// <summary>
        /// Stores the callback function
        /// </summary>
        public Action<State> callback;
        /// <summary>
        /// Stores the socket
        /// </summary>
        public Socket socket;
        /// <summary>
        /// Stores the message
        /// </summary>
        public StringBuilder builder;
        /// <summary>
        /// Buffer for recieving data
        /// </summary>
        public byte[] buffer;
        /// <summary>
        /// Used for encoding the message
        /// </summary>
        public UTF8Encoding encoder;
        /// <summary>
        /// Stores the exception
        /// </summary>
        public string exception;
        /// <summary>
        /// Server for listening to outside conenctions
        /// </summary>
        public TcpListener server;
        /// <summary>
        /// Server to listen for localhost connections
        /// </summary>
        public TcpListener localHostServer;
        /// <summary>
        /// server for web
        /// </summary>
        public TcpListener webServer;
        /// <summary>
        /// Server for listening for outside web connections
        /// </summary>
        public TcpListener webExternalServer;
        /// <summary>
        /// Size of buffer
        /// </summary>
        private const int bufferSize = 1024;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="callback">The function to call once current task is completed</param>
        /// <param name="socket">The socket used to connect to the server</param>
        public State(Action<State> callback, Socket socket)
        {
            this.socket = socket;
            this.callback = callback;
            builder = new StringBuilder();
            buffer = new byte[bufferSize];
            encoder = new UTF8Encoding();
        }

        /// <summary>
        /// Decodes the message and adds it to the StringBuilder
        /// </summary>
        public void Decode()
        {
            builder.Append(encoder.GetString(buffer));
        }

        /// <summary>
        /// Clears the StringBuilder and buffer
        /// </summary>
        public void Clear()
        {
            builder.Clear();
            buffer = new byte[bufferSize];
        }
    }
}
