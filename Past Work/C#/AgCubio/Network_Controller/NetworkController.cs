//Trevor Chapman, Xiaoyun Ding
// PS9
// 12/10/2015
// version 1.0

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AgCubio
{
    /// <summary>
    /// A static class used to send and receive data
    /// </summary>
    public static class NetworkController
    {
        private const int port = 11000;
        private const int webPort = 11100;

        /// <summary>
        /// Method used to attempt to establish connection to a server with a hostname
        /// </summary>
        /// <param name="callback"> The delegate function to call back to once connected</param>
        /// <param name="hostname"> The host to attempt to connect to</param>
        /// <returns>A socket used to connect to the server</returns>
        public static Socket Connect_to_Server(Action<State> callback, string hostname)
        {
            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp); // Create a new object
            State state = new State(callback, socket); // Create a new state with the callback function and created socket

            //Begin connecting using the hostname, port number, callback function, and state object
            socket.BeginConnect(hostname, port, Connected_To_Server, state);
            return socket;
        }

        /// <summary>
        /// A helper function that recieves the data and calls the provided callback function
        /// </summary>
        /// <param name="result">Result from the Connect_To_Server connection</param>
        private static void Connected_To_Server(IAsyncResult result)
        {
            State state = (State)result.AsyncState;
            try
            {
                // End the connection attempt
                state.socket.EndConnect(result);

                // Provide the buffer, length, and method to call
                state.socket.BeginReceive(state.buffer, 0, state.buffer.Length,
                                    SocketFlags.None, ReceiveCallback, state);

                // Access the provided callback function
                state.callback(state);
            }

            // If an exception occured, update the exception message, close the socket and return
            catch (Exception ex)
            {
                HandleException(state, ex.ToString());
            }
        }

        /// <summary>
        /// Function used to ask for more data from the server
        /// </summary>
        /// <param name="state"></param>
        public static void i_want_more_data(State state)
        {
            try
            {
                state.socket.BeginReceive(state.buffer, 0, state.buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), state);
            }

            // If an exception occured, update the exception message, close the socket and return
            catch (Exception ex)
            {
                HandleException(state, ex.ToString());
            }
        }

        /// <summary>
        /// Function called once data is recieved from server, and calls the delegate function
        /// </summary>
        /// <param name="result"></param>
        private static void ReceiveCallback(IAsyncResult result)
        {
            State state = (State)result.AsyncState;
            try
            {
                int bytes = state.socket.EndReceive(result);

                if (bytes == 0)
                // Server has closed connection
                {
                    state.socket.Close();
                }
                else
                {
                    // Decode bytes into string and add to string builder
                    state.Decode();
                    state.callback(state);
                }
            }

            // If an exception occured, update the exception message, close the socket and return
            catch (Exception ex)
            {
                HandleException(state, ex.ToString());
            }
        }

        /// <summary>
        /// Private method to handle exceptions
        /// </summary>
        /// <param name="state">The state object containing data</param>
        /// <param name="exception">The exception message</param>
        private static void HandleException(State state, string exception)
        {
            try
            {
                state.exception = exception.ToString();
                state.socket.Shutdown(SocketShutdown.Both);
                state.socket.Close();
                state.callback(state);
            }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// Function used to send data to a server
        /// </summary>
        /// <param name="socket">The socket used to connect to the server</param>
        /// <param name="data"> The string to send to the server</param>
        /// <param name="callback">Callback function after send has finished sending data</param>
        public static void Send(Socket socket, string data, Action<State> callback)
        {
            State state = new State(callback, socket);

            //Attempt to send the data
            try
            {
                state.buffer = new UTF8Encoding().GetBytes(data);
                byte[] messageBuffer = state.buffer;
                state.builder.Append(data);
                socket.BeginSend(messageBuffer, 0, messageBuffer.Length, SocketFlags.None, new AsyncCallback(SendCallBack), state);
            }
            catch (Exception)
            {
                socket.Close();
            }
        }

        /// <summary>
        /// Helper function for Send. After sending data it checks that theentire message was successfully sent. If only part 
        /// of it was sent, try sending the rest
        /// </summary>
        /// <param name="result"></param>
        private static void SendCallBack(IAsyncResult result)
        {
            State state = (State)result.AsyncState;

            // Attempt to send the data
            try
            {
                Socket socket = state.socket;
                int bytes = socket.EndSend(result);
                byte[] byteData = new UTF8Encoding().GetBytes(state.builder.ToString());

                // If the entire message was not sent, resend the part not sent
                if (bytes < byteData.Length)
                {
                    state.builder.Remove(0, bytes);
                    socket.BeginSend(byteData, bytes, byteData.Length - bytes, SocketFlags.None, new AsyncCallback(SendCallBack), byteData);
                }

                if (state.callback != null)
                {
                    state.callback(state);
                }
            }
            catch (Exception)
            {
                try
                {
                    state.socket.Shutdown(SocketShutdown.Both);
                    state.socket.Close();
                }
                catch (ObjectDisposedException) { }
            }
        }

        /// <summary>
        /// Function used to begin listening for clients for a server
        /// </summary>
        /// <param name="callback"></param>
        public static void ServerAwaitingClientLoop(Action<State> callback)
        {
            Console.WriteLine("Listening for server");
            IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[0];

            TcpListener server = new TcpListener(IPAddress.Any, port);
            TcpListener localHost = new TcpListener(ipAddress, port);

            State state = new State(callback, null);

            state.server = server;
            state.localHostServer = localHost;

            state.server.Start();
            localHost.Start();

            state.server.BeginAcceptSocket(AcceptNewClient, state);
            localHost.BeginAcceptSocket(AcceptNewClient, state);
            Console.WriteLine("Beginning to accept socket");
        }

        /// <summary>
        /// Helper function for when a client connects to the server
        /// </summary>
        /// <param name="result"></param>
        public static void AcceptNewClient(IAsyncResult result)
        {
            Console.WriteLine("Found Client");

            // Get the connected state
            State LoopState = (State)result.AsyncState;
            Socket socket = LoopState.server.EndAcceptSocket(result);

            // Create a new state to send onwards 
            State CallbackState = new State(LoopState.callback, socket);
            CallbackState.callback(CallbackState);

            LoopState.server.BeginAcceptSocket(AcceptNewClient, LoopState);
            LoopState.localHostServer.BeginAcceptSocket(AcceptNewClient, LoopState);
        }

        /// <summary>
        /// Listens for web requests
        /// </summary>
        /// <param name="callback"></param>
        public static void WebServer(Action<State> callback)
        {
            Console.WriteLine("Listening for web client");
            State state = new State(callback, null);
            IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
            TcpListener localWebServer = new TcpListener(ipAddress, webPort);
            TcpListener outsideWebServer = new TcpListener(IPAddress.Any, webPort);

            state.webServer = localWebServer;
            state.webExternalServer = outsideWebServer;

            localWebServer.Start();
            outsideWebServer.Start();

            localWebServer.BeginAcceptSocket(AcceptWebClient, state);
            outsideWebServer.BeginAcceptSocket(AcceptWebClient, state);
        }

        /// <summary>
        /// Accepts web requests
        /// </summary>
        /// <param name="result"></param>
        private static void AcceptWebClient(IAsyncResult result)
        {
            Console.WriteLine("Found Web Client");

            // Get the connected state
            State LoopState = (State)result.AsyncState;
            Socket socket = LoopState.webServer.EndAcceptSocket(result);

            // Create a new state to send onwards 
            State CallbackState = new State(LoopState.callback, socket);
            CallbackState.callback(CallbackState);

            LoopState.webServer.BeginAcceptSocket(AcceptWebClient, LoopState);
            LoopState.webExternalServer.BeginAcceptSocket(AcceptWebClient, LoopState);
        }

    }
}

