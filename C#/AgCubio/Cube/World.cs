// Trevor Chapman, Xiaoyun Ding
// PS9
// 12/10/2015
// version 1.0

using System;
using System.Collections.Generic;

namespace AgCubio
{
    /// <summary>
    /// Represents the world and all objects in it for AgCubio
    /// </summary>
    public class World
    {
        /// <summary>
        /// Contains cubes in the world
        /// </summary>
        private Dictionary<int, Cube> cubes;
        /// <summary>
        /// Keeps track of split cubes and their owner
        /// </summary>
        private Dictionary<int, List<Cube>> splitCubes;
        /// <summary>
        /// track the number of foods in the workd
        /// </summary>
        public int foodNum;
        /// <summary>
        /// Width of the world
        /// </summary>
        private readonly int Width;
        /// <summary>
        /// Height of the world
        /// </summary>
        private readonly int Height;
        /// <summary>
        /// Keeps track of how many times to updated
        /// </summary>
        public readonly int Heartbeats;
        /// <summary>
        /// Top movement speed of a cube (when it is smallest
        /// </summary>
        private readonly double TopSpeed;
        /// <summary>
        /// Minimum movement speed of a cube (when largest)
        /// </summary>
        private readonly double LowSpeed;
        /// <summary>
        /// Speed at which cubes lose mass
        /// </summary>
        public readonly double Attrition;
        /// <summary>
        /// Minimum mass before applying attrition
        /// </summary>
        private readonly double AttritionMin;
        /// <summary>
        /// How much mass each food eaten adds
        /// </summary>
        public readonly double FoodValue;
        /// <summary>
        /// Player starting mass
        /// </summary>
        public readonly double PlayerStartMass;
        /// <summary>
        /// Maximum food to keep in the work
        /// </summary>
        public readonly int MaxFood;
        /// <summary>
        /// Minimum mass a cube needs to be before it can split
        /// </summary>
        private readonly double MinSplit;
        /// <summary>
        /// Maximum amount of times a cube can split
        /// </summary>
        private readonly double MaxSplit;
        /// <summary>
        /// Maximum distance a split cube can be launched
        /// </summary>
        private readonly double MaxSplitDistance;
        /// <summary>
        /// Amount a cube needs to overlap before can eat/be eaten
        /// </summary>
        private readonly double AbsorbDelta;
        /// <summary>
        /// The default speed to move a cube
        /// </summary>
        private readonly double moveSpeed;
        /// <summary>
        /// The mass of a virus
        /// </summary>
        public readonly double virusMass;
        /// <summary>
        /// The rate that viruses spawn
        /// </summary>
        public readonly int virusRate;
        /// <summary>
        /// Maximum number of viruses allowed in the world
        /// </summary>
        public readonly int virusMax;
        /// <summary>
        /// Time to wait before merging back together
        /// </summary>
        public readonly double MergeTime;
        /// <summary>
        /// Used to generate random values
        /// </summary>
        private Random rng;
        /// <summary>
        /// Contains a list of colors for picking
        /// </summary>
        private List<Int32> colors;
        /// <summary>
        /// UID Counter to assing to unique cubes
        /// </summary>
        private int uidCounter;

        /// <summary>
        /// Amount of viruses in the world right now
        /// </summary>
        private int virusCounter;

        /// <summary>
        /// Default constructor
        /// </summary>
        public World()
        {
            cubes = new Dictionary<int, Cube>();
            splitCubes = new Dictionary<int, List<Cube>>();
            foodNum = 0;
            rng = new Random();
            GenerateColors();
            uidCounter = 0;

            // World parameters
            Width = 1000;
            Height = 1000;
            Heartbeats = 20;

            AbsorbDelta = 0.02;
            Attrition = .99999;
            AttritionMin = 200;

            FoodValue = 1;
            MaxFood = 5000;
            PlayerStartMass = 1000;

            moveSpeed = 1000;
            TopSpeed = 10;
            LowSpeed = 0.01;

            MinSplit = 100;
            MaxSplit = 10;
            MaxSplitDistance = 100;

            MergeTime = 10000;

            virusMass = 100;
            virusRate = 500;
            virusMax = 5;
        }

        /// <summary>
        /// Generates a list of colors for the cubes in the world to pick from
        /// </summary>
        private void GenerateColors()
        {
            colors = new List<int>();
            colors.Add(0x7FFF0000); // Red
            colors.Add(0x7F0000FF); // Blue
            colors.Add(0x7FE5A470); // Pink
            colors.Add(0x7F581047); // Dark Purprle
            colors.Add(0x7811082D); // Dark Blue
            colors.Add(0x78475810); // Olive Green
            colors.Add(0x789BB7A7); // Light Blue
            colors.Add(0x7856DC2B); // Puke Green
            colors.Add(0x78c1c1c1); // Light Grey
            colors.Add(0x782c2308); // Not quite black, like dark brown or something?
            colors.Add(0x78000000); // Black
            colors.Add(0x78FDA601); // Golden
            colors.Add(0x780F90F0); // Medium Blue
            colors.Add(0x78811611); // Dark Red
        }

        /// <summary>
        /// Adds a cube to the world. If the cube is already in the world it will be updated. If the cube being added contains 
        /// a mass of 0 it will be removed.
        /// </summary>
        /// <param name="cube"></param>
        public void AddCube(Cube cube)
        {
            cubes.Remove(cube.uid);

            // If mass is greater than 0, add to dictionary. Otherwise it has been eaten and does not need to be added
            if (cube.Mass > 0)
                cubes.Add(cube.uid, cube);

            if (cube.food)
            {
                if (cube.Mass == 0)
                    foodNum--;
                else
                    foodNum++;
            }
        }

        /// <summary>
        /// Generates a new cube. If null is passed as the name, creates a new food. If a name is provided, 
        /// a player cube is created.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Cube GenerateNewCube(string name)
        {
            // Create random food
            int x = (int)(rng.NextDouble() * Width);
            int y = (int)(rng.NextDouble() * Height);
            int color = colors[uidCounter % colors.Count];
            Cube cube;
            if (name == null)
            {
                cube = new Cube(x, y, color, uidCounter, true, "", FoodValue, uidCounter++);
                if (uidCounter % virusRate == 0 && virusCounter < virusMax)
                    ChangeToVirus(cube);
            }
            else
            {
                cube = new Cube(x, y, color, uidCounter, false, name, PlayerStartMass, uidCounter++);
                cube.info.lifeTimer.Start();
            }

                AddCube(cube);
            return cube;
        }

        /// <summary>
        /// Change a food cube into a virus
        /// </summary>
        /// <param name="virus"></param>
        private void ChangeToVirus(Cube virus)
        {
            virus.argb_color = 0x7800FF00;
            virus.Mass = virusMass;
            virusCounter++;
        }

        /// <summary>
        /// Returns the dictionary containing the world
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Cube> GetCubes()
        {
            return cubes;
        }

        /// <summary>
        /// Gets a cube
        /// </summary>
        /// <param name="uid">UID of cube to get</param>
        /// <returns></returns>
        public Cube GetCube(int uid)
        {
            Cube cube;
            cubes.TryGetValue(uid, out cube);
            return cube;
        }


        /// <summary>
        /// Handles move requests from a player cube
        /// </summary>
        public void Move(Cube player, double x, double y)
        {
            List<Cube> playerCubes;
            if (splitCubes.TryGetValue(player.uid, out playerCubes))
            {
                List<Cube> copy = playerCubes.GetRange(0, playerCubes.Count);
                //foreach(Cube cube in copy)
                for (int i = 0; i < copy.Count; i++)
                {
                    // Divide by mass for factor
                    double speed = GetSpeed(copy[i], moveSpeed, copy[i].Mass);

                    // don't move if the mouse is at the centre of the player's cube, otherwise update player cube
                    if (x != copy[i].loc_x && y != copy[i].loc_y)
                        playerCubes[i] = CalculateMovement(playerCubes[i], speed, x, y);
                }
                cubes[player.uid] = playerCubes[0];
            }

            else
            {
                // Divide by mass for factor
                double speed = GetSpeed(player, moveSpeed, player.Mass);

                // don't move if the mouse is at the centre of the player's cube, otherwise update player cube
                if (x != player.loc_x || y != player.loc_y)
                    cubes[player.uid] = CalculateMovement(player, speed, x, y);
            }
        }

        /// <summary>
        /// Calculates the speed at which an object should move
        /// </summary>
        /// <param name="startSpeed">The movement speed to apply</param>
        /// <param name="mass">The cube's mass</param>
        /// <param name="cube"></param>
        /// <returns></returns>
        private double GetSpeed(Cube cube, double startSpeed, double mass)
        {
            if (cube.momentumFrames > 0 && startSpeed * cube.splitTimer.ElapsedMilliseconds/1000 < MaxSplitDistance)
                startSpeed += cube.momentumFrames--;

            double speed = startSpeed / mass;

            if (speed < LowSpeed)
                speed = LowSpeed;

            else if (speed > TopSpeed)
                speed = TopSpeed;

            return speed;
        }

        /// <summary>
        /// A private method used to calculate where a cube should be moved to
        /// </summary>
        /// <param name="cube">The cube to move</param>
        /// <param name="movementAmount">The speed to move</param>
        /// <param name="x">The X position to move towards</param>
        /// <param name="y">The Y position to move towards</param>
        /// <returns></returns>
        private Cube CalculateMovement(Cube cube, double movementAmount, double x, double y)
        {
            Cube newCube = new Cube(cube);
            
            if (newCube.momentumFrames > 0)
            {
                movementAmount += newCube.momentumFrames;
                newCube.momentumFrames--;
            }

            double angle = Math.Atan2(x - newCube.loc_x, y - newCube.loc_y);

            // calculate the new position
            x = movementAmount * Math.Sin(angle) + newCube.loc_x;
            y = movementAmount * Math.Cos(angle) + newCube.loc_y;

            // team cubes should not overlap to each other
            List<Cube> playerCubes;
            if (splitCubes.TryGetValue(cube.team_id, out playerCubes))
            {
                for (int i = 0; i < playerCubes.IndexOf(cube); i++)
                {
                    double dx = 0, dy = 0;
                    double aveWidth = AverageWidth(newCube, playerCubes[i], out dx, out dy);

                    if (dx > 0 && dy > 0)
                    {
                        x = newCube.loc_x > playerCubes[i].loc_x ? aveWidth + playerCubes[i].loc_x : playerCubes[i].loc_x - aveWidth;
                        y = newCube.loc_y > playerCubes[i].loc_y ? aveWidth + playerCubes[i].loc_y : playerCubes[i].loc_y - aveWidth;
                    }
                }
            }

            newCube.loc_x = CheckBorders(newCube, x);
            newCube.loc_y = CheckBorders(newCube, y);

            return newCube;
        }

        /// <summary>
        /// Checks if the new cube location is within the bounds of the world
        /// </summary>
        /// <param name="cube">The cube to check</param>
        /// <param name="coordinate">The x or y coordinate to compare against</param>
        /// <returns></returns>
        private double CheckBorders(Cube cube, double coordinate)
        {
            if (coordinate < cube.Width / 2)
                coordinate = cube.Width;
            else if (coordinate > Width - cube.Width / 2)
                coordinate = Width - cube.Width / 2;

            return coordinate;
        }

        /// <summary>
        /// Handles split requests from a player cube
        /// </summary>
        public void Split(Cube cube, double x, double y, bool isVirus)
        {
            // Get the current list
            List<Cube> split;
            if (!splitCubes.TryGetValue(cube.team_id, out split))
                split = new List<Cube>() { cube };

            // If already at maximum splits, return
            if (split.Count == MaxSplit)
                return;

            List<Cube> copy = split.GetRange(0, split.Count); // Copy list over for iterating
            // If it is not a virus, split every cube in the list
            if (!isVirus)
                for (int i = 0; i < copy.Count; i++)
                {
                    if (split.Count >= MaxSplit)
                        break;

                    if (copy[i].Mass < MinSplit)
                        continue;

                    split.Add(Explode(copy[i], x, y));
                }

            // If it is a virus, check if it is above the min mass
            else if (cube.Mass > MinSplit)
            {
                //int index = split.IndexOf(cube);
                split.Add(Explode(cube, x, y));
                if (split.Count < MaxSplit)
                    split.Add(Explode(cube, x, y));
            }

            cube.splitTimer.Restart();
            splitCubes[cube.team_id] = split;
        }

        /// <summary>
        /// split the cube 1 to 2
        /// </summary>
        /// <param name="cube">the cube waiting for split</param>
        /// <param name="x">x direction of new split cube</param>
        /// <param name="y">y direction of new split cube</param>
        /// <returns></returns>
        private Cube Explode(Cube cube, double x, double y)
        {
            //double speed = GetSpeed(cube, moveSpeed, cube.Mass);
            cube.Mass /= 2;
            Cube newCube = new Cube(cube);
            // Cube newCube = CalculateMovement(cube, speed, x, y); // TODO: FIX SO DOES NOT JUMP
            newCube.uid = uidCounter++;
            newCube.AddMomentum(60);
            //Move(newCube, x, y);
            return newCube;
        }

        /// <summary>
        /// Applys a set amount of attrition to a cube
        /// </summary>
        /// <param name="player">The player to apply attrition to. If the player has split, it will be applied to them as well</param>
        public List<Cube> Attrit(Cube player)
        {
            List<Cube> attrited = new List<Cube>();
            List<Cube> playerCubes;
            if (splitCubes.TryGetValue(player.uid, out playerCubes))
                foreach (Cube cube in playerCubes)
                {
                    if (cube.Mass > AttritionMin)
                        cube.Mass = cube.Mass * Attrition;

                    // Add all player cube in the list, no matter if it is lost mass.
                    attrited.Add(cube);
                }

            else
            {
                if (player.Mass > AttritionMin)
                    player.Mass = player.Mass * Attrition;

                attrited = new List<Cube>() { player };
            }

            return attrited;
        }

        /// <summary>
        /// Checks if the player has absorbed any other cubes
        /// </summary>
        /// <param name="player">The player attempting to eat other cubes</param>
        public List<Cube> Absorbing(Cube player)
        {
            List<Cube> eaten = new List<Cube>();
            List<Cube> playerCubes;
            List<Cube> merge = new List<Cube>();
            List<Cube> eatenSplit = new List<Cube>();
            List<int> list = new List<int>();
            List<Cube> viruses = new List<Cube>();

            if (splitCubes.TryGetValue(player.team_id, out playerCubes))
            {
                // If passed the elasped time, merge back together
                if (player.splitTimer.ElapsedMilliseconds >= MergeTime)
                    Merge(playerCubes, merge);

                // Check if split player cubes eat others' split cubes
                foreach (Cube cube in playerCubes)
                {
                    foreach (var pair in splitCubes)
                    {
                        // If own cube's split list, no need to check
                        if (pair.Key == cube.team_id)
                            continue;

                        List<Cube> split = pair.Value;
                        foreach (Cube c in split)
                            if (ShouldAbsorb(cube, c))
                                Eat(cube, c, eatenSplit, viruses);
                    }

                    // Check if cube has eaten any cubes in world
                    foreach (var pair in cubes)
                    {
                        Cube worldCube = pair.Value;
                        if (ShouldAbsorb(cube, worldCube))
                            Eat(cube, worldCube, eaten, viruses);
                    }
                }
            }

            else
            {
                // whole cube eat split cubes
                foreach (var p in splitCubes)
                {
                    List<Cube> split = p.Value;
                    foreach (Cube c in split)
                        if (ShouldAbsorb(player, c))
                            Eat(player, c, eatenSplit, viruses);
                }

                // Check if cube has eaten any cubes in world
                foreach (var pair in cubes)
                {
                    Cube worldCube = pair.Value;
                    if (ShouldAbsorb(player, worldCube))
                        Eat(player, worldCube, eaten, viruses);
                }
            }

            // Explode cubes that have eaten a virus
            foreach (Cube eating in viruses)
                Split(eating, rng.NextDouble(), rng.NextDouble(), true);

            // Remove eaten cubes from the world
            foreach (Cube cube in eaten)
                cubes.Remove(cube.uid);

            // Remove eaten split cubes
            foreach (Cube c in eatenSplit)
            {
                // If the main cube is to be eaten, swap the IDs of another cube so player is not registered as dead
                if (c.team_id == c.uid && splitCubes[c.team_id].Count > 1)
                    Swap(c);

                // update the split cubes list
                splitCubes[c.team_id].Remove(c);
            }

            // update player
            cubes[player.uid] = player;

            // merge all eaten cubes into eaten
            eaten.AddRange(merge);
            eaten.AddRange(eatenSplit);

            return eaten;
        }

        /// <summary>
        /// Merge cubes together
        /// </summary>
        /// <param name="playerCubes">List of player cubes</param>
        /// <param name="merge">List to add merged cubes to</param>
        private void Merge(List<Cube> playerCubes, List<Cube> merge)
        {
            for (int j = playerCubes.Count-1; j >= 0; j--)
            {
                for (int i = j-1; i >= 0; i--)
                {
                    double dx = 0, dy = 0;
                    AverageWidth(playerCubes[j], playerCubes[i], out dx, out dy);
                    if (dx > -5 && dy > -5)
                        Eat(playerCubes[i], playerCubes[j], merge, null); // When merging cubes they cannot be a virus, so should be fine
                }
            }

            foreach (Cube c in merge)
                playerCubes.Remove(c);
        }

        /// <summary>
        /// Private method to eat a cube
        /// </summary>
        /// <param name="eating">Cube eating</param>
        /// <param name="eaten">Cube being eaten</param>
        /// <param name="eatenCubes">List to add to</param>
        /// <param name="viruses">List to add viruses eaten to</param>
        private void Eat(Cube eating, Cube eaten, List<Cube> eatenCubes, List<Cube> viruses)
        {
            eating.Mass += eaten.Mass;
            eaten.Mass = 0;
            eatenCubes.Add(eaten);

            // Get the total mass of all of the player's cube
            double totalMass = eating.Mass;
            List<Cube> split = null;
            if (splitCubes.TryGetValue(eating.team_id, out split))
            {
                foreach (Cube cube in split)
                    totalMass += cube.Mass;
                totalMass -= eating.Mass;
            }

            if (eaten.food)
            {
                foodNum--;
                // Update stats to reflect cube ate food
                eating.info.Update(totalMass);
                if (eaten.argb_color == 0x7800FF00)
                {
                    viruses.Add(eating);
                    virusCounter--;
                }
            }

            // Update stats if player ate another player
            else if (eating.team_id != eaten.team_id)
                eating.info.Update(totalMass, eaten.Name);
        }

        /// <summary>
        /// privare method help to pass the uid to the next one if the main cube is eaten
        /// </summary>
        /// <param name="c"></param>
        private void Swap(Cube c)
        {
            // swap the first two split cubes
            int temp = splitCubes[c.team_id][1].uid;
            splitCubes[c.team_id][1].uid = c.uid;
            c.uid = temp;
            // set the second split cube to be main cube if the first is eaten
            cubes[c.team_id] = splitCubes[c.team_id][1];
        }

        /// <summary>
        /// Checks if it is within a certain range. If it is, return true. Otherwise return false
        /// </summary>
        /// <param name="player"></param>
        /// <param name="worldCube"></param>
        /// <returns>if the worldCube should be eaten by the player, return true</returns>
        private bool ShouldAbsorb(Cube player, Cube worldCube)
        {
            // should not eat oneself
            if (player.team_id == worldCube.team_id)
                return false;

            // player should not eat cubes bigger
            if (player.Mass <= worldCube.Mass)
                return false;

            double dx = 0, dy = 0;
            AverageWidth(player, worldCube, out dx, out dy);

            // no overlap
            if (dx <= 0 || dy <= 0)
                return false;

            // the overlap size should no more than the worldCube.Width
            dx = Math.Min(dx, worldCube.Width);
            dy = Math.Min(dy, worldCube.Width);

            // check the overlap area
            return dx * dy > worldCube.Mass * AbsorbDelta;
        }

        /// <summary>
        /// help method to get the average width, used to calculate overlap
        /// </summary>
        /// <param name="player"></param>
        /// <param name="worldCube"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private double AverageWidth(Cube player, Cube worldCube, out double x, out double y)
        {
            double aveWidth = (player.Width + worldCube.Width) / 2;
            x = aveWidth - Math.Abs(player.loc_x - worldCube.loc_x);
            y = aveWidth - Math.Abs(player.loc_y - worldCube.loc_y);
            return aveWidth;
        }
    }
}

