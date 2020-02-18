// Trevor Chapman, Xiaoyun Ding
// PS9
// 12/10/2015
// version 1.0

using AgCubio;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Xml;

namespace AgCubioServer
{
    /// <summary>
    /// Class that handles all the server-side mechanics of AgCubio
    /// </summary>
    class Server
    {
        /// <summary>
        /// Keeps track of the state of the world
        /// </summary>
        private World world;
        /// <summary>
        /// Keeps track of all the sockets and uid associated with them
        /// </summary>
        private Dictionary<Socket, int> socketToUID;
        /// <summary>
        /// Keeps track of all players and the cube associated with them
        /// </summary>
        private Dictionary<int, Socket> UIDToSocket;
        private const string errorPagePath = @"..\..\..\Resources\ErrorPage.txt";

        public const string connectionString = "server=atr.eng.utah.edu;database=cs3500_trevorc;uid=cs3500_trevorc;password='P@ssw0rd'";

        static void Main(string[] args)
        {
            new Server();
        }

        /// <summary>
        /// Constructor to set up the server
        /// </summary>
        public Server()
        {
            world = new World();
            socketToUID = new Dictionary<Socket, int>();
            UIDToSocket = new Dictionary<int, Socket>();
            Start();
        }

        /// <summary>
        /// Starts the server by populating the world, starting the heartbeat, and beginning to listen for connections
        /// </summary>
        private void Start()
        {
            // add food
            lock (world)
            {
                for (int i = 0; i < world.MaxFood; i++)
                    world.GenerateNewCube(null);
            }

            // Begin listening for client
            NetworkController.ServerAwaitingClientLoop(HandleNewConnection);
            NetworkController.WebServer(HandleWebConnetion);

            // Start watch used for heartbeats
            Stopwatch watch = new Stopwatch();
            watch.Start();

            // Infinite loop used to calculate when to update the world
            int count = 0;
            while (true)
            {
                if (watch.ElapsedMilliseconds > 1000)
                {
                    watch.Restart();
                    count = 0;
                }
                //while (count < world.Heartbeats)
                while (count * 1000 / world.Heartbeats < watch.ElapsedMilliseconds)
                {
                    Update();
                    count++;
                }
            }
        }

        /// <summary>
        /// Callback function for handling a new connection
        /// </summary>
        /// <param name="state">A state object containing the relevent connection data</param>
        private void HandleNewConnection(State state)
        {
            Console.WriteLine("\nA new client is connecting");
            state.callback = ReceivePlayerName;
            NetworkController.i_want_more_data(state);
        }

        /// <summary>
        /// After receiving the player's name, 
        /// </summary>
        /// <param name="state">A state object containing the relevent connection data</param>
        private void ReceivePlayerName(State state)
        {
            Console.WriteLine("Receiving Play Name");

            // Get the player's name
            int newLIneIndex = state.builder.ToString().IndexOf('\n');
            string name = state.builder.ToString(0, newLIneIndex);
            state.Clear();

            Console.WriteLine("Adding player cube");
            Cube cube;
            lock (world)
            {
                cube = world.GenerateNewCube(name);
                socketToUID.Add(state.socket, cube.uid);
                UIDToSocket.Add(cube.uid, state.socket);
            }

            // Send the player their cube and update to begin handling new data
            String message = JsonConvert.SerializeObject(cube) + '\n';
            foreach (var player in socketToUID)
                NetworkController.Send(player.Key, message, null);

            state.callback = HandleData;

            message = "";
            lock (world)
            {
                Console.WriteLine("Sending Cubes");
                // Send all cubes in the world to the connected player
                foreach (var worldCube in world.GetCubes())
                {
                    message = JsonConvert.SerializeObject(worldCube.Value) + '\n';
                    NetworkController.Send(state.socket, message, null);
                }
            }

            Console.WriteLine("Beginning to receive data from client");
            NetworkController.i_want_more_data(state);
        }

        /// <summary>
        /// Handles data being sent from a client
        /// </summary>
        /// <param name="state">A state object containing the relevent connection data</param>
        private void HandleData(State state)
        {
            // Console.WriteLine("Receiving data from client");

            if (!socketToUID.ContainsKey(state.socket))
                return;

            // Decode information being sent and determine if correct
            //state.Decode();
            String[] message = state.builder.ToString().Split('\n');
            state.Clear();

            // Process each request
            foreach (string current in message)
            {
                int playerUid;

                // If the socket is not in the list, no more data needs to be sent or received so simply exit out of method
                lock (world)
                {
                    if (!socketToUID.TryGetValue(state.socket, out playerUid))
                        return;
                }
                Cube playerCube = world.GetCube(playerUid);
                double x, y;

                // If it is not a valid message disconnect the user
                if (!TryGetPoint(current, out x, out y))
                {
                    // Messages will occasionally be all \0 due to brief connection issues. If so just continue
                    if (Regex.IsMatch(current, "\\0+"))
                        continue;

                    state.socket.Shutdown(SocketShutdown.Both);
                    state.socket.Close();
                    lock (world)
                    {
                        socketToUID.Remove(state.socket);
                    }
                    return;
                }

                else if (current.StartsWith("(move"))
                {
                    lock (world)
                    {
                        world.Move(playerCube, x, y);
                    }
                }

                else if (current.StartsWith("(split"))
                {
                    lock (world)
                    {
                        world.Split(playerCube, x, y, false);
                    }
                }
            }

            NetworkController.i_want_more_data(state);
        }

        /// <summary>
        /// Attempts to convert a message into a move or split commant
        /// </summary>
        /// <param name="message">The message to parse</param>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">THe y coordinate</param>
        /// <returns>true if it could parse the message, false otherwise</returns>
        private bool TryGetPoint(string message, out double x, out double y)
        {
            x = 0;
            y = 0;

            // Check if valid move or split request
            string request = @"^\((move|split), -{0,1}[0-9]+, -{0,1}[0-9]+\)$";
            if (!Regex.IsMatch(message, request))
                return false;

            string[] split = message.Split(',');

            // Break up message for parsing
            return (double.TryParse(split[1], out x) &&
                double.TryParse(split[2].Substring(0, split[2].Length - 1), out y));
        }


        /// <summary>
        /// Main function of server. This function will create new food, handle players eating food and other players, handle 
        /// cube attrition and growth, and send the state of the world to all players. In addition if a player has since 
        /// disconnected remove them from the appropriate places.
        /// </summary>
        private void Update()
        {
            // TODO: RUNNING INTO ISSUE WHEN TWO PLAYERS EAT AT SAME TIME BOTH ARE KILLED. CANNOT REPRODUCE RIGHT NOW 
            // BUT KEEP AN EYE OUT FOR IT

            List<Cube> modified = new List<Cube>();
            List<Cube> eaten = new List<Cube>();
            string message = "";

            lock (world)
            {
                // Apply player attrition/growth as neeeded
                foreach (var pair in UIDToSocket)
                {
                    Cube cube = world.GetCube(pair.Key);
                    if (cube != null)
                        modified.AddRange(world.Attrit(cube));
                }

                // Add food to world if below max number
                if (world.foodNum < world.MaxFood)
                    modified.Add(world.GenerateNewCube(null));

                // Check if player has eaten other players or eaten other food
                lock (socketToUID)
                {
                    foreach (var pair in UIDToSocket)
                    {
                        Cube player = world.GetCube(pair.Key);
                        if (player != null)
                        {
                            eaten = world.Absorbing(player);
                            modified.AddRange(eaten);
                        }
                    }
                }

                // Serialize all cubes in the list to send
                foreach (Cube cube in modified)
                    if (cube != null)
                        message += JsonConvert.SerializeObject(cube) + "\n";

                // Send state of world to all players. If no longer conencted add to list for clean-up
                HashSet<Socket> socketToRemove = new HashSet<Socket>();
                foreach (var player in socketToUID)
                    if (player.Key.Connected)
                        NetworkController.Send(player.Key, message, null);
                    else
                        socketToRemove.Add(player.Key);
                foreach (Socket socket in socketToRemove)
                    socketToUID.Remove(socket);

                // remove dead player from the lists
                HashSet<int> uidToRemove = new HashSet<int>();
                foreach (var pair in UIDToSocket)
                    if (world.GetCube(pair.Key) == null)
                    {
                        socketToUID.Remove(pair.Value);
                        uidToRemove.Add(pair.Key);
                    }

                foreach (int uid in uidToRemove)
                    UIDToSocket.Remove(uid);

                //if the player die, upate the database to record this game information
                foreach (Cube cube in eaten)
                    if (!cube.food && cube.uid == cube.team_id)
                        UpdateDB(cube);
            }
        }

        /// <summary>
        /// Handles incoming web connections for the web server
        /// </summary>
        /// <param name="state"></param>
        private void HandleWebConnetion(State state)
        {
            state.callback = WebRequest;
            NetworkController.i_want_more_data(state);
        }

        /// <summary>
        /// Checks what the request is, and generates HTML for displaying the appropriate web page
        /// </summary>
        /// <param name="state"></param>
        private void WebRequest(State state)
        {
            // Split the entire string and get the needed string
            string request = state.builder.ToString();
            string[] requestSplit = request.Split('\r');
            request = requestSplit[0];
            state.Clear();

            // Generate HTML for web page
            string sendMessage = "HTTP/1.1 200 OK/r/nConnection: close\r\nContent-Type: text/html; charset=UTF-8\r\n";
            sendMessage += "\r\n";
            sendMessage += "<!DOCTYPE html><html lang=\"en\" mlns=\"http://www.w3.org/1999/xhtml\"><head><meta charset=\"utf-8\" /><title>AgCubio Scoreboard</title></head><body>";
            sendMessage += HandleWebRequest(request) + "<p/></body></html>";

            NetworkController.Send(state.socket, sendMessage, (State) => state.socket.Close());

        }

        /// <summary>
        /// Reads the specified file as a string, used to read a custom made error page
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        private string ReadPage(string pathToFile)
        {
            return File.ReadAllText(@pathToFile);
        }

        /// <summary>
        /// Handles web requests and generates the appropriate HTML code
        /// </summary>
        /// <param name="request">The message containining the correct command</param>
        private string HandleWebRequest(string request)
        {
            // check message
            string scoreRegex = @"^GET \/scores HTTP\/1\.1$";
            string playerRegex = @"^GET \/games\?player=.* HTTP\/1\.1$";
            string eatenRegex = @"^GET \/eaten\?id=\d+ HTTP\/1\.1$";

            if (Regex.IsMatch(request, scoreRegex))
            {
                Console.WriteLine("Score request");
                return GetScore();
            }

            else
            {
                // Split the command to get which is being looked at for SQL WHERE command
                string[] splitRequest = request.Split(' ');
                string parameter = splitRequest[1];
                parameter = parameter.Substring(parameter.IndexOf('=') + 1);

                if (Regex.IsMatch(request, playerRegex))
                {
                    Console.WriteLine("Player Request");
                    return GetPlayer(parameter);
                }

                else if (Regex.IsMatch(request, eatenRegex))
                {
                    Console.WriteLine("Eat Request");
                    return GetEaten(parameter);
                }

                else
                {
                    Console.WriteLine("Invalid Request");
                    return ReadPage(errorPagePath);
                }
            }
        }

        /// <summary>
        /// Private method for getting the scores table from the database and generating the appropriate HTML
        /// </summary>
        /// <returns>A string containing HTML</returns>
        private string GetScore()
        {
            string messageToBrowser = "<h1>Scores</h1><p>";
            // Connect to the DB
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    // Create a command
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "SELECT * FROM Scores order by Max_Mass DESC";

                    // Execute the command and cycle through the DataReader object
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        messageToBrowser += "<p>Clicking the Cubes Eaten will show all players eaten by that Cube</p>";
                        messageToBrowser += "<p>Clicking a Player's name will show all games played by that player</p>";

                        messageToBrowser += "<table style=\"width:50%\"><tr><td>Player_Name</td><td>Time_Alive</td>"
                            + "<td> Max_Mass</td><td>Cubes_Eaten</td><td>Rank</td ></tr> ";
                        while (reader.Read())
                        {
                            int id = (int)reader["PlayerID"];
                            int rank = (int)reader["Rank"];
                            string playerName = (string)reader["Player_name"];
                            messageToBrowser += "<tr><td><a href=\"/games?player=" + playerName + "\">" + playerName + "</a></td><td> "
                                + reader["Time_Alive"] + "</td><td> " + reader["Max_Mass"] + "</td><td><a href=\"/eaten?id=" + id + "\">"
                                + reader["Cubes_Eaten"] + "</a></td>";

                            messageToBrowser += AddRank(rank) + "</tr>";
                        }

                        messageToBrowser += "</table>";
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return ReadPage(errorPagePath);
                }
            }
            return messageToBrowser;
        }

        /// <summary>
        /// Adds HTML for displaying the rank if it is between 1-5
        /// </summary>
        /// <param name="rank">The player's rank from SQL Scores table</param>
        /// <returns></returns>
        private string AddRank(int rank)
        {
            // Default value is 0 meaning unranked
            if (rank > 0)
                return "<td>" + rank + "</td></tr>";

            return "<td/></tr>";
        }

        /// <summary>
        /// Handles getting scores from the table and generating HTML code for showing a player's score
        /// </summary>
        /// <param name="player">The name of the player to look up</param>
        /// <returns></returns>
        private string GetPlayer(string player)
        {
            string messageToBrowser = "<h1>Player Score</h1><p>";

            // Connect to the DB
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    // Create a command
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "SELECT * FROM Scores WHERE Player_Name=\'" + player + "\'";
                    messageToBrowser += "<p><a href=\"/scores\">" + "Link to main score table</a></p>";
                    messageToBrowser += "<p>Clicking the Cubes Eaten will show all players eaten by that Cube</p>";
                    messageToBrowser += "<table style=\"width:70%\"> <tr><td> Player_Name</td><td>Time_Alive </td><td>Max_Mass</td>"
                        + "<td>Cubes_Eaten</td><td>Time_Of_Death</td><td>Rank</td></tr>";

                    // Execute the command and generate needed HTML
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id = (int)reader["PlayerID"];
                                int rank = (int)reader["Rank"];
                                // Read row and generate needed HTML
                                messageToBrowser += "<tr><td>" + reader["Player_Name"] + "</td><td> " + reader["Time_Alive"] + "</td><td> "
                                    + reader["Max_Mass"] + "</td><td><a href=\"/eaten?id=" + id + "\">" + reader["Cubes_Eaten"] + "</a></td><td>" + reader["Time_Of_Death"] + "</td>";

                                messageToBrowser += AddRank(rank);
                            }
                            messageToBrowser += "</table>";
                        }
                        else
                            return ReadPage(errorPagePath);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return ReadPage(errorPagePath);
                }
            }
            return messageToBrowser;
        }

        /// <summary>
        /// Merges tables to create a combined one containing all the player's information, and generates the HTML 
        /// code to display it
        /// </summary>
        /// <param name="ID">The ID to lookup</param>
        /// <returns></returns>
        private string GetEaten(string ID)
        {
            string messageToBrowser = "<h1>Players Eaten</h1><p>";

            // Connect to the DB
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    // Create a command
                    MySqlCommand command = conn.CreateCommand();
                    //command.CommandText = "SELECT Scores.*, Players_Eaten.Players_Eaten FROM Scores LEFT JOIN Players_Eaten ON Scores.PlayerID = Players_Eaten.PlayerID" + ID;
                    command.CommandText = "SELECT Scores.*, Players_Eaten.Players_Eaten FROM Scores LEFT JOIN Players_Eaten ON Scores.PlayerID = Players_Eaten.PlayerID WHERE Scores.PlayerID = " + ID;
                    // Execute the command and generate needed HTML
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        messageToBrowser += "<p><a href=\"/scores\">" + "Link to main score table</a></p>";
                        messageToBrowser += "<p>Clicking a Player's name will show all games played by that player</p>";
                        messageToBrowser += "<table style=\"width:70%\"> <tr><td> Player_Name</td><td>Time_Alive </td>"
                            + "<td> Max_Mass</td><td>Cubes_Eaten</td><td>Time_Of_Death</td>"
                            + "<td>Players_Eaten</td><td>Rank</td></tr>";

                        if (reader.Read())
                        {

                            Object playerName = reader["Players_Eaten"];
                            int rank = (int)reader["Rank"];

                            messageToBrowser += "<tr><td>" + reader["Player_Name"] + "</td><td> " + reader["Time_Alive"] + "</td><td> " +
                                reader["Max_Mass"] + "</td><td> " + reader["Cubes_Eaten"] + "</td><td>" + reader["Time_Of_Death"] + "</td>";
                            // With a left join can have DBNull data, check if is not and add player. Otherwise just add empty row
                            if (!(playerName is DBNull))
                            {
                                playerName = (string)playerName;
                                messageToBrowser += "</td><td><a href=\"/games?player=" + playerName + "\">" + playerName + "</a></td>";
                            }
                            else
                                messageToBrowser += "<td/>";

                            messageToBrowser += AddRank(rank);

                            while (reader.Read())
                            {
                                playerName = (string)reader["Players_Eaten"];
                                messageToBrowser += "<td/><td/><td/><td/><td/><td><a href=\"/games?player=" + playerName + "\">" + playerName + "</a></td></tr>";
                            }

                            messageToBrowser += "</table>";
                        }
                        else
                            return ReadPage(errorPagePath);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return ReadPage(errorPagePath);
                }

                return messageToBrowser;
            }
        }

        /// <summary>
        /// update the database after player die
        /// </summary>
        /// <param name="player"></param>
        private void UpdateDB(Cube player)
        {
            // Connect to the DB
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    // Create a command to insert new data into database Scores
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "insert into Scores (Player_Name, Time_Alive, Max_Mass, Cubes_Eaten, Time_Of_Death) values(@Player_Name, @Time_Alive, @Max_Mass, @Cubes_Eaten, @Time_Of_Death);";
                    command.Parameters.AddWithValue("@Player_Name", player.Name);
                    command.Parameters.AddWithValue("@Time_Alive", player.info.lifeTimer.Elapsed);
                    command.Parameters.AddWithValue("@Max_Mass", player.info.MaxMass);
                    command.Parameters.AddWithValue("@Cubes_Eaten", player.info.cubesEaten);
                    command.Parameters.AddWithValue("@Time_Of_Death", DateTime.Now);

                    command.ExecuteNonQuery();

                    // get the primary key of the new data in Scores table
                    command = conn.CreateCommand();
                    command.CommandText = "select max(PlayerID) from Scores";
                    int id = 0;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        id = (int)reader["max(PlayerID)"];
                    }
                    // insert players eaten into database
                    foreach (string name in player.info.playersEaten)
                    {
                        command = conn.CreateCommand();
                        command.CommandText = "insert into Players_Eaten (PlayerID, Players_Eaten) Value (" + id + ", \'" + name + "\');";
                        command.ExecuteNonQuery();
                    }

                    // update rank
                    command = conn.CreateCommand();
                    command.CommandText = "select * from Scores order by Scores.Max_Mass DESC";
                    List<int> rank = new List<int>();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        for (int i = 0; i < 5 && reader.Read(); i++)
                            rank.Add((int)reader["PlayerID"]);
                    }

                    // set all rank to be 0
                    command = conn.CreateCommand();
                    command.CommandText = "update Scores set Rank = 0";
                    command.ExecuteNonQuery();
                    // reset the first five palyers rank
                    int j = 1;
                    foreach (int n in rank)
                    {
                        command = conn.CreateCommand();
                        command.CommandText = "UPDATE Scores SET Scores.Rank = " + j++ + " WHERE PlayerID = " + n;
                        command.ExecuteNonQuery();
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
