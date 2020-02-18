// Trevor Chapman, Xiaoyun Ding
// PS9
// 12/10/2015
// version 1.0

using System;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Generic;

namespace AgCubio
{
    /// <summary>
    /// Representation of a cube object used for AgCubio
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Cube
    {
        /// <summary>
        /// Unique ID
        /// </summary>
        [JsonProperty]
        public int uid;
        /// <summary>
        /// X and Y coordinates
        /// </summary>
        [JsonProperty]
        public double loc_x, loc_y;
        /// <summary>
        /// Color of cube
        /// </summary>
        [JsonProperty]
        public int argb_color;
        /// <summary>
        /// Name of cube
        /// </summary>
        [JsonProperty]
        public string Name;
        /// <summary>
        /// Current mass of the cube
        /// </summary>
        [JsonProperty]
        public double Mass;
        /// <summary>
        /// Boolean whether is food or not
        /// </summary>
        [JsonProperty]
        public bool food;
        /// <summary>
        /// Team ID of cube
        /// </summary>
        [JsonProperty]
        public int team_id;
        /// <summary>
        /// Timer for determining when to re-merge after a split
        /// </summary>
        public Stopwatch splitTimer;
        /// <summary>
        /// When moving, shows how many updates to move at a higher speed for. Used mainly when splitting the cube
        /// </summary>
        public int momentumFrames;
        /// <summary>
        /// Used to keep track of shared information for database
        /// </summary>
        public RecordingInfo info;

        /// <summary>
        /// Calculates the width of the cube
        /// </summary>
        public int Width
        {
            get
            {
                return (int)Math.Sqrt(Mass);
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Cube()
        {
            Mass = 1000;
            loc_x = 50;
            loc_y = 50;
            argb_color = 0x00000;
            uid = 1;
            food = false;
            Name = "Default";
            team_id = 1;
            splitTimer = new Stopwatch();
            info = new RecordingInfo();
        }

        /// <summary>
        /// Constructs the cube
        /// </summary>
        /// <param name="x">The x coordinate of the upper-left corner</param>
        /// <param name="y">The y coordinate of the upper left corner</param>
        /// <param name="color">The color of the cube</param>
        /// <param name="UID">The unique ID of the cube</param>
        /// <param name="Food">Whether it is a food cube or a player cube</param>
        /// <param name="name">The name of the cube</param>
        /// <param name="mass">The current mass of the cube</param>
        /// <param name="Team_ID">The team ID of the cube</param>
        [JsonConstructor]
        public Cube(double x, double y, int color, int UID, bool Food, string name, double mass, int Team_ID)
        {
            Mass = mass;
            loc_x = x;
            loc_y = y;
            argb_color = color;
            uid = UID;
            food = Food;
            Name = name;
            team_id = Team_ID;
            splitTimer = new Stopwatch();
            info = new RecordingInfo();
        }

        /// <summary>
        /// Checks if the cube has been killed/eaten
        /// </summary>
        /// <returns>True if dead (mass = 0), false otherwise</returns>
        public bool IsDead()
        {
            if (Mass == 0)
                return true;

            return false;
        }

        /// <summary>
        /// Create a copy of a cube
        /// </summary>
        /// <param name="cube"></param>
        public Cube(Cube cube)
        {
            loc_x = cube.loc_x;
            loc_y = cube.loc_y;
            argb_color = cube.argb_color;
            uid = cube.uid;
            food = cube.food;
            Name = cube.Name;
            Mass = cube.Mass;
            team_id = cube.team_id;
            splitTimer = cube.splitTimer;
            momentumFrames = cube.momentumFrames;
            info = cube.info;
        }

        /// <summary>
        /// Adds momentum frames to the cube
        /// </summary>
        /// <param name="frames"></param>
        public void AddMomentum(int frames)
        {
            momentumFrames = frames;
        }

        /// <summary>
        /// Inner class for Cubes used to keep track of shared information for writing to the AgCubio database
        /// </summary>
        public class RecordingInfo
        {
            /// <summary>
            /// record the maximum mass the player achieved
            /// </summary>
            public double MaxMass;
            /// <summary>
            /// used to store the number of cubes (food and other players) that a player ate
            /// </summary>
            public int cubesEaten;
            /// <summary>
            /// The timer used to record the length of time a player was alive
            /// </summary>
            public Stopwatch lifeTimer;
            /// <summary>
            /// used to store the name of each player that the player ate
            /// </summary>
            public HashSet<string> playersEaten;

            /// <summary>
            /// Default constructor to initialize variables
            /// </summary>
            public RecordingInfo()
            {
                MaxMass = 0;
                cubesEaten = 0;
                lifeTimer = new Stopwatch();
                playersEaten = new HashSet<string>();
            }

            /// <summary>
            /// Update relevent values. Used for eating food
            /// </summary>
            /// <param name="mass"></param>
            public void Update(double mass)
            {
                if (mass > MaxMass)
                    MaxMass = mass;

                cubesEaten++;
            }

            /// <summary>
            /// Update relevent values. Used for eating players
            /// </summary>
            /// <param name="mass"></param>
            /// <param name="playerName"></param>
            public void Update(double mass, string playerName)
            {
                if (mass > MaxMass)
                    MaxMass = mass;

                cubesEaten++;

                playersEaten.Add(playerName);
            }
        }

    }
}

