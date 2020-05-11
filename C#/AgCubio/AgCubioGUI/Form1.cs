// Trevor Chapman, Xiaoyun Ding
// PS7
// 11/17/2015
// version 1.0

using System.Drawing;
using System.Windows.Forms;
using AgCubio;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Sockets;
using System;
using System.Threading.Tasks;

namespace AgCubioGUI
{
    public partial class AgCubio_Form : Form
    {
        /// <summary>
        /// Represents the cubes in the world
        /// </summary>
        private World world;
        /// <summary>
        /// ID of the player cube
        /// </summary>
        private int myID;
        /// <summary>
        /// The player's cube
        /// </summary>
        private Cube myCube;
        /// <summary>
        /// Stopwatch used to calculating FPS
        /// </summary>
        private Stopwatch fpsWatch;
        /// <summary>
        /// Stopwatch to keep track of time user has played
        /// </summary>
        private Stopwatch aliveWatch;
        /// <summary>
        /// Socket used to connect to the server
        /// </summary>
        private double maxMass;
        /// <summary>
        /// Socket used to connect to srever
        /// </summary>
        private Socket socket;
        /// <summary>
        /// Font used to draw string names
        /// </summary>
        private Font myFont;
        /// <summary>
        /// Used to scale window
        /// </summary>
        private const int scaler = 300;

        /// <summary>
        /// Sets up the initial form
        /// </summary>
        public AgCubio_Form()
        {
            // Set UP the form
            InitializeComponent();
            myCube = new Cube();
            myFont = new Font("Aria", 20);
            fpsWatch = new Stopwatch();
            fpsWatch.Start();
            aliveWatch = new Stopwatch();
        }

        /// <summary>
        /// Send a move request to the server
        /// </summary>
        public void MoveRequest()
        {
            SendMessage("move");
        }

        /// <summary>
        /// Send a split command to the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Split(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                SendMessage("split");
        }

        /// <summary>
        /// A private function used to send a command to the server
        /// </summary>
        /// <param name="message">The message to send to the server</param>
        private void SendMessage(string message)
        {
            Point mouse = PointToClient(MousePosition);
            // Send message and mouse position to server
            int dx = (int)(mouse.X * scaler / Width - (scaler / 2) + myCube.loc_x);
            int dy = (int)(mouse.Y * scaler / Height - (scaler / 2) + myCube.loc_y);
            string sendMessage = "(" + message + ", " + dx + ", " + dy + ")\n";

            if (!myCube.IsDead())
            {
                NetworkController.Send(socket, sendMessage, null);
            }
        }

        /// <summary>
        /// Repaint the client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgCubio_Form_Paint(object sender, PaintEventArgs e)
        {
            // Lock on Cube
            lock (world)
            {
                foreach (var pair in world.GetCubes())
                {
                    Cube cube = pair.Value;
                    Color color = Color.FromArgb(cube.argb_color);
                    SolidBrush brush = new SolidBrush(color);

                    // Scale the window
                    double scale = Width / scaler;
                    int width = (int)(cube.Width * scale);
                    int relative_x = (int)((Width - width) / 2 + (cube.loc_x - myCube.loc_x) * scale);
                    int relative_y = (int)((Height - width) / 2 + (cube.loc_y - myCube.loc_y) * scale);

                    // Paint the cube
                    Rectangle rect = new Rectangle(relative_x, relative_y, width, width);
                    e.Graphics.FillRectangle(brush, rect);

                    // Draw name on cube
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString("" + cube.Name, myFont, Brushes.Yellow, rect, stringFormat);
                }

                // Update counters
                massNum_label.Text = "" + myCube.Mass;
                widthNum_label.Text = "" + myCube.Width;
                foodNum_label.Text = "" + world.foodNum;
            }

            // Get FPS
            fpsWatch.Stop();
            long t = fpsWatch.ElapsedMilliseconds;
            if (t > 0)
                FPSNum_label.Text = "" + (int)(10e3 / t);
            else
                FPSNum_label.Text = "0";
            fpsWatch.Restart();
            
            // send move request
            MoveRequest();

            // Repaint
            panel_Game.Invalidate();
        }

        /// <summary>
        /// Called when the user click the start button to begin connecting to the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Start_Click(object sender, EventArgs e)
        {
            start();
        }

        /// <summary>
        /// Press Enter key to start game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void player_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                start();
        }

        /// <summary>
        /// Begin trying to start the game
        /// </summary>
        private void start()
        {
            if (player_textBox.Text == "")
                inform_textBox.Text = "Please enter an unempty player name!";

            else
            {
                // Try to connect to the server and start game
                inform_textBox.Text = "Connecting...";
                socket = NetworkController.Connect_to_Server(FirstConnection, Server_textBox.Text);
            }
        }

        /// <summary>
        /// This method is called by the NetworkController after it first connects to the server
        /// </summary>
        private void FirstConnection(State state)
        {
            // If connection could not be made, alert the user and do not proceed
            if (state.exception != null)
            {
                string message = state.exception;
                if (state.exception.Contains("No connection could be made because the target machine actively refused it"))
                    message = "Could not connect to the server. This could be because the server is not available or the Server Name is incorrect. Either try " +
                        "a different server or try again";

                inform_textBox.Invoke(new Action(() => { inform_textBox.Text = message; }));
            }

            // Once connection is established, update variables
            else
            {
                world = new World();
                this.Invoke(new Action(() => { panel_Network.Visible = false; }));

                // Send message to server with player name
                NetworkController.Send(state.socket, player_textBox.Text + "\n", null);

                // Update state to new callback function
                state.callback = Add_Player_Cube;
            }
        }

        /// <summary>
        /// Adds the player cube to the world
        /// </summary>
        /// <param name="state">State object containing networking data</param>
        private void Add_Player_Cube(State state)
        {
            // Lock the world and add all cubes in message to world
            lock (world)
            {
                // Build player cube
                string message = state.builder.ToString();
                myCube = JsonConvert.DeserializeObject<Cube>(message);
                myID = myCube.uid;
                maxMass = myCube.Mass;
                world.AddCube(myCube);

                // Clear the buffer and builder to prepare adding more cubes
                state.Clear();
            }

            // Change state callback to Add_Cubes
            state.callback = Add_Cubes;
            aliveWatch.Restart();
            NetworkController.i_want_more_data(state);
        }

        /// <summary>
        /// Add a cube to the world, and then ask the server for more data
        /// </summary>
        /// <param name="state">State object containing networking data</param>
        private void Add_Cubes(State state)
        {
            // If there has been an exception, show it
            if (state.exception != null)
            {
                string message = state.exception;

                // If the server has closed, inform the user
                if (message.Contains("An existing connection was forcibly closed by the remote host"))
                    message = "Lost connection to the server. This is likely because the server has shut down or disconnected you. Press Start to try again.";

                this.Invoke(new Action(() => { inform_textBox.Text = message; }));
                socket.Close();
                return;
            }

            // Prevent the world from being updated outside of the function
            lock (world)
            {
                string message = state.builder.ToString();

                // Clear the builder and buffer
                state.Clear();

                // Split on \n
                string[] cubeArray = message.Split('\n');
                int endIndex = cubeArray.Length - 1;

                // If the last one is not a complete string, add it back to the builder for next use
                if (!(cubeArray[endIndex].EndsWith("\n")))
                {
                    if (!cubeArray[endIndex].Contains("\0"))
                        state.builder.Append(cubeArray[endIndex]);

                    endIndex--;
                }

                // Add each cube to the world
                for (int i = 0; i <= endIndex; i++)
                {
                    Cube cube = JsonConvert.DeserializeObject<Cube>(cubeArray[i]);
                    world.AddCube(cube);
                    if (cube.uid == myID)
                    {
                        myCube = cube;
                        if (myCube.Mass > maxMass)
                            maxMass = myCube.Mass;

                        CheckDeath();
                    }
                }
            }

            //// Continue if player cube has not died
            if (!myCube.IsDead())
            {
                // Ask for more data
                NetworkController.i_want_more_data(state);
            }
        }

        /// <summary>
        /// Checks if the player has died. If so, display text and close the socket
        /// </summary>
        public void CheckDeath()
        {
            if (myCube.IsDead())
            {
                aliveWatch.Stop();
                this.BeginInvoke(new Action(() => { inform_textBox.Text = DeathText(); }));
                socket.Close();
            }
        }

        /// <summary>
        /// message after player died, including "You Died", "Try again" and stats.
        /// </summary>
        /// <returns></returns>
        public string DeathText()
        {
            string deathMessage = "YOU DIED!" + Environment.NewLine + Environment.NewLine;
            deathMessage += "Player:    " + myCube.Name + Environment.NewLine;
            deathMessage += "Max Mass:  " + maxMass + Environment.NewLine;
            TimeSpan span = TimeSpan.FromMilliseconds(((int)aliveWatch.ElapsedMilliseconds/1000)*1000);
            deathMessage += "Life Time: " + span + Environment.NewLine;
            deathMessage += Environment.NewLine + "Click Start to Reconnect And Try Again!";
            return deathMessage;
        }

        /// <summary>
        /// Whenever the message updates, show the panel_Network.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inform_textBox_TextChanged(object sender, EventArgs e)
        {
            panel_Network.Visible = true;
        }

        /// <summary>
        /// After the Network panel is set to visable, display rest of start menu. 
        /// After the Network panel is set to invisible, display the game panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel_Network_VisibleChanged(object sender, EventArgs e)
        {
            if (panel_Network.Visible)
            {
                panel_Game.Visible = false;
                player_textBox.Focus();
            }

            else
            {
                panel_Game.Visible = true;
                Focus();
            }
        }
    }
}
