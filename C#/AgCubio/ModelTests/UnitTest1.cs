// Trevor Chapman, Xiaoyun Ding
// PS8
// 12/3/2015
// version 1.0

using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgCubio;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace ModelTests
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Tests for default Cube constructor
        /// </summary>
        [TestMethod()]
        public void CubeDefaultConstructor()
        {
            Cube cube = new Cube();
            Assert.AreEqual(1000, cube.Mass);
            Assert.AreEqual(50, cube.loc_x);
            Assert.AreEqual(50, cube.loc_y);
        }

        /// <summary>
        /// Test for second Cube constructor
        /// </summary>
        [TestMethod()]
        public void CubeSecondConstructor()
        {
            Cube cube = new Cube(100, 100, 50, 10, false, "Test", 10, 1);
            Assert.AreEqual(100, cube.loc_x);
            Assert.AreEqual(100, cube.loc_y);
            Assert.AreEqual(50, cube.argb_color);
            Assert.AreEqual(10, cube.uid);
            Assert.AreEqual(false, cube.food);
            Assert.AreEqual("Test", cube.Name);
            Assert.AreEqual(10, cube.Mass);
            Assert.AreEqual(1, cube.team_id);
        }

        /// <summary>
        /// Tests for Cube Methods
        /// </summary>
        [TestMethod()]
        public void CubeMethods()
        {
            Cube cube = new Cube(100, 100, 50, 10, false, "Test", 9, 1);
            Assert.AreEqual(3, cube.Width);
            Assert.IsFalse(cube.IsDead());
            cube.Mass = 0;
            Assert.IsTrue(cube.IsDead());
        }

        /// <summary>
        /// Test for World default constructor
        /// </summary>
        [TestMethod()]
        public void WorldDefault()
        {
            World world = new World();
            Dictionary<int, Cube> cubes = world.GetCubes();
            Assert.IsTrue(cubes.Count == 0);
            Assert.AreEqual(0, world.foodNum);
        }

        /// <summary>
        /// Tests for adding and checking cubes in the world
        /// </summary>
        [TestMethod()]
        public void WorldCubes()
        {
            // Check that a Cube is added
            World world = new World();
            Dictionary<int, Cube> cubes;
            world.AddCube(new Cube(100, 100, 50, 10, false, "Test", 9, 1));
            cubes = world.GetCubes();
            Assert.AreEqual(1, cubes.Count);

            // Check that the cube was added
            Cube tempCube;
            cubes.TryGetValue(10, out tempCube);
            Assert.AreEqual(100, tempCube.loc_x);
            Assert.AreEqual(100, tempCube.loc_y);
            Assert.AreEqual(50, tempCube.argb_color);
            Assert.AreEqual(10, tempCube.uid);
            Assert.AreEqual(false, tempCube.food);
            Assert.AreEqual("Test", tempCube.Name);
            Assert.AreEqual(9, tempCube.Mass);
            Assert.AreEqual(1, tempCube.team_id);

            // Check that the cube is replaced
            world.AddCube(new Cube(52, 63, 4, 10, true, "TestReplace", 3.0, 10));
            cubes = world.GetCubes();
            Assert.AreEqual(1, cubes.Count);

            cubes.TryGetValue(10, out tempCube);
            Assert.AreEqual(52, tempCube.loc_x);
            Assert.AreEqual(63, tempCube.loc_y);
            Assert.AreEqual(4, tempCube.argb_color);
            Assert.AreEqual(10, tempCube.uid);
            Assert.AreEqual(true, tempCube.food);
            Assert.AreEqual("TestReplace", tempCube.Name);
            Assert.AreEqual(3.0, tempCube.Mass);
            Assert.AreEqual(10, tempCube.team_id);

            // Add a few more cubes and check they are added correctly
            world.AddCube(new Cube(100, 100, 50, 11, false, "Test", 9, 1));
            world.AddCube(new Cube(50, 10, 723, 12, true, "AnotherTest", 9, 15));
            cubes = world.GetCubes();
            Assert.AreEqual(3, cubes.Count);
        }


        //************************************NEW FOR SERVER BRANCH**********************************************************//

        /// <summary>
        /// Test for moving a cube
        /// </summary>
        [TestMethod()]
        public void WorldMove1()
        {
            // Assumes speed is set to 1000
            World world = new World();

            // Normal test, both greater than
            Cube playerCube = new Cube();
            double x = playerCube.loc_x;
            double y = playerCube.loc_y;
            double movenemt = 1 / Math.Sqrt(2);
            world.AddCube(playerCube);
            world.Move(playerCube, x + 5, y + 5); // move to bottom right
            playerCube = world.GetCube(playerCube.uid);
            Assert.AreEqual(x + movenemt, playerCube.loc_x, 1e-9);
            Assert.AreEqual(y + movenemt, playerCube.loc_y, 1e-9);

            // Normal test, both less than
            playerCube = new Cube();
            world.Move(playerCube, x - 3, y - 3);
            playerCube = world.GetCube(playerCube.uid);
            Assert.AreEqual(x - movenemt, playerCube.loc_x, 1e-9);
            Assert.AreEqual(y - movenemt, playerCube.loc_y, 1e-9);

            // Normal test, X greater than Y less than
            playerCube = new Cube();
            world.Move(playerCube, x + 6, y - 6);
            playerCube = world.GetCube(playerCube.uid);
            Assert.AreEqual(x + movenemt, playerCube.loc_x, 1e-9);
            Assert.AreEqual(y - movenemt, playerCube.loc_y, 1e-9);

            // Normal test, X less than Y greater than
            playerCube = new Cube();
            world.Move(playerCube, x - 7, y + 7);
            playerCube = world.GetCube(playerCube.uid);
            Assert.AreEqual(x - movenemt, playerCube.loc_x, 1e-9);
            Assert.AreEqual(y + movenemt, playerCube.loc_y, 1e-9);
        }

        [TestMethod()]
        public void WorldMove2()
        {
            World world = new World();

            // Should not move if directly in center
            Cube playerCube = new Cube();
            world.AddCube(playerCube);
            world.Move(playerCube, 50, 50);
            double x = playerCube.loc_x;
            double y = playerCube.loc_y;
            int movenemt = 1; // movement = speed/Mass = 1000/1000

            playerCube = world.GetCube(playerCube.uid);
            Assert.AreEqual(x, (int)playerCube.loc_x);
            Assert.AreEqual(y, (int)playerCube.loc_y);

            // If X is the same
            playerCube = new Cube();
            world.Move(playerCube, x, y + 1);
            playerCube = world.GetCube(playerCube.uid);
            Assert.AreEqual(x, (int)playerCube.loc_x);
            Assert.AreEqual(y + movenemt, (int)playerCube.loc_y);

            // If Y is the same
            playerCube = new Cube();
            world.Move(playerCube, x + 1, y);
            playerCube = world.GetCube(playerCube.uid);
            Assert.AreEqual(x + movenemt, (int)playerCube.loc_x);
            Assert.AreEqual(y, (int)playerCube.loc_y);
        }

        /// <summary>
        /// split cubes should move all team cubes
        /// </summary>
        [TestMethod()]
        public void WorldMove3()
        {
            World world = new World();
            Cube playerCube = new Cube();
            world.AddCube(playerCube);

            world.Split(playerCube, 1, 1, false);
            List<Cube> cubes = world.Attrit(playerCube);
            double x0 = cubes[0].loc_x;
            double y0 = cubes[0].loc_y;
            double x1 = cubes[1].loc_x;
            double y1 = cubes[1].loc_y;

            world.Move(playerCube, 1, 1);
            cubes = world.Attrit(playerCube);
            Assert.AreNotEqual(x0, cubes[0].loc_x, 1e-9);
            Assert.AreNotEqual(y0, cubes[0].loc_y, 1e-9);
            Assert.AreNotEqual(x1, cubes[1].loc_x, 1e-9);
            Assert.AreNotEqual(y1, cubes[1].loc_y, 1e-9);
        }

        /// <summary>
        /// if cube is out of world, move should ajust that
        /// </summary>
        [TestMethod()]
        public void WorldMove4()
        {
            World world = new World();
            Cube playerCube = new Cube();
            world.AddCube(playerCube);

            playerCube.loc_x = -50; // left of the world
            world.Move(playerCube, 50, 50);
            playerCube = world.GetCube(playerCube.uid);
            Assert.IsTrue(0 < playerCube.loc_x && playerCube.loc_x < 1000);

            playerCube.loc_x = 10000; // right of the world
            world.Move(playerCube, 50, 50);
            playerCube = world.GetCube(playerCube.uid);
            Assert.IsTrue(0 < playerCube.loc_x && playerCube.loc_x < 1000);

            playerCube.loc_y = -50; // up the world
            world.Move(playerCube, 50, 50);
            playerCube = world.GetCube(playerCube.uid);
            Assert.IsTrue(0 < playerCube.loc_y && playerCube.loc_y < 1000);

            playerCube.loc_x = 10000; // down the world
            world.Move(playerCube, 50, 50);
            playerCube = world.GetCube(playerCube.uid);
            Assert.IsTrue(0 < playerCube.loc_y && playerCube.loc_y < 1000);
        }

        /// <summary>
        /// Tests for generating viruses
        /// </summary>
        [TestMethod()]
        public void VirusTest()
        {
            World world = new World();
            for (int i = 0; i < world.virusRate; i++)
                world.GenerateNewCube(null);

            Cube cube = world.GetCube(world.virusRate - 1);
            Assert.AreEqual(0x7800FF00, cube.argb_color);
            Assert.AreEqual(world.virusMass, cube.Mass);
        }

        /// <summary>
        /// Test for applying attrition to a cube
        /// </summary>
        [TestMethod()]
        public void WorldAttrition1()
        {
            // Assumes Attrition is 0.9
            World world = new World();
            Cube playerCube = new Cube();
            double playerMass = playerCube.Mass;
            world.AddCube(playerCube);
            playerCube = world.Attrit(playerCube)[0];
            //playerCube = world.GetCubes()[playerCube.uid];

            Assert.AreEqual(playerMass * world.Attrition, playerCube.Mass, 1e-9);
        }

        /// <summary>
        /// Test for applying attrition to a cube. if mass is less than AttritionMin, should not attrit
        /// </summary>
        [TestMethod()]
        public void WorldAttrition2()
        {
            // Assumes Attrition is 0.9
            World world = new World();
            Cube playerCube = new Cube();
            playerCube.Mass = 10;
            double playerMass = playerCube.Mass;
            world.AddCube(playerCube);
            playerCube = world.Attrit(playerCube)[0];
            //playerCube = world.GetCubes()[playerCube.uid];

            Assert.AreNotEqual(playerMass * world.Attrition, playerCube.Mass, 1e-9);
        }

        /// <summary>
        /// Test for generating a new player cube
        /// </summary>
        [TestMethod()]
        public void WorldGenerateCube()
        {
            World world = new World();
            Cube cube = world.GenerateNewCube("test");
            Assert.AreEqual(cube.uid, 0);
            Assert.IsNotNull(cube.loc_x);
            Assert.IsNotNull(cube.loc_y);
            Assert.AreEqual(0x7FFF0000, cube.argb_color);
            Assert.AreEqual(0, cube.team_id);
            Assert.IsFalse(cube.food);
            Assert.AreEqual("test", cube.Name);
        }

        /// <summary>
        /// Test for generating random food
        /// </summary>
        [TestMethod()]
        public void WorldGenerateFood()
        {
            World world = new World();
            Cube cube = world.GenerateNewCube(null);
            Assert.IsTrue(cube.food);
            Assert.AreEqual(cube.Mass, world.FoodValue);
            Assert.AreEqual("", cube.Name);
            Assert.AreEqual(0, cube.uid);
            Assert.AreEqual(0, cube.team_id);

            cube = world.GenerateNewCube(null);
            Assert.AreEqual(1, cube.team_id);
            Assert.AreEqual(1, cube.uid);
            Assert.AreEqual(0x7F0000FF, cube.argb_color);
        }

        /// <summary>
        /// Test for absorbing a cube
        /// </summary>
        [TestMethod()]
        public void WorldAbsorb()
        {
            World world = new World();
            Cube playerCube = new Cube();
            double startMass = playerCube.Mass;
            world.AddCube(playerCube);

            Cube foodCube = new Cube(50, 50, 0, 0, true, "food", 1, 0);
            world.AddCube(foodCube);
            world.Absorbing(playerCube);

            Assert.IsFalse(world.GetCubes().ContainsKey(foodCube.uid));
            Assert.AreEqual(startMass + world.FoodValue, world.GetCubes()[playerCube.uid].Mass);
        }

        //*******Add more tests for Absorb once futher along

        /// <summary>
        /// Check that the split method works
        /// </summary>
        [TestMethod()]
        public void WorldSplit()
        {
            World world = new World();
            Cube cube = world.GenerateNewCube("test");
            double startX = cube.loc_x;
            double startY = cube.loc_y;
            world.Split(cube, 100, 100, false);
            Assert.AreEqual(world.PlayerStartMass / 2, cube.Mass);

            // Make sure the initial cube has not moved
            Assert.AreEqual(startX, cube.loc_x);
            Assert.AreEqual(startY, cube.loc_y);
        }

        /// <summary>
        /// Test for merging back together
        /// </summary>
        [TestMethod()]
        public void Merge()
        {
            World world = new World();
            //Cube cube = new Cube(10, 10, 0x78FF0000, 0, false, "test", 100, 0);
            Cube cube = world.GenerateNewCube("test");
            List<Cube> list = new List<Cube>();

            world.Split(cube, 1, 0, false);
            Assert.AreEqual(world.PlayerStartMass / 2, cube.Mass, 1e-9);
            //list = world.Attrit(cube);
            //Assert.AreEqual(2, list.Count);
            //Assert.AreEqual(list[0].loc_x, cube.loc_x, 1e-9);
            //Assert.AreEqual(list[0].loc_y, list[1].loc_y, 1e-9);
            Stopwatch timer = new Stopwatch();
            timer.Start();

            // Wait until should have merged plus a short delay to finish
            while (timer.ElapsedMilliseconds < world.MergeTime + 100 && list.Count<1)
            {
                list = world.Absorbing(cube);
            }

            Assert.AreEqual(1, list.Count);
            Assert.AreNotEqual(cube.uid, list[0].uid);
            cube = world.GetCube(cube.uid);
            Assert.AreEqual(world.PlayerStartMass, cube.Mass);
            Assert.AreEqual(cube.uid, cube.team_id);
            Assert.AreEqual(cube.team_id, list[0].team_id);
        }

        /// <summary>
        /// test normal eating
        /// </summary>
        [TestMethod]
        public void WorldAbsorbing1()
        {
            World world = new World();
            world.GenerateNewCube("first");
            world.GenerateNewCube("second");
            Cube first = world.GetCube(0);
            Cube second = world.GetCube(1);
            List<Cube> list = null;

            first.Mass++;
            list = world.Absorbing(first);
            Assert.AreEqual(0, list.Count);

            first.loc_x = second.loc_x;
            first.loc_y = second.loc_y;
            list = world.Absorbing(first);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(world.PlayerStartMass * 2 + 1, first.Mass, 1e-9);
            Assert.AreEqual(0, second.Mass, 1e-9);

            second = world.GetCube(1);
            Assert.AreEqual(null, second);
        }

        /// <summary>
        /// test split cubes be eaten
        /// </summary>
        [TestMethod]
        public void WorldAbsorbing2()
        {
            World world = new World();
            world.GenerateNewCube("first");
            world.GenerateNewCube("second");
            Cube first = world.GetCube(0);
            Cube second = world.GetCube(1);
            List<Cube> list = null;

            world.Split(second, 0, 0, false);
            list = world.Absorbing(first);
            Assert.AreEqual(0, list.Count);

            first.loc_x = second.loc_x;
            first.loc_y = second.loc_y;
            list = world.Absorbing(first);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(world.PlayerStartMass * 2, first.Mass, 1e-9);
            Assert.AreEqual(0, second.Mass, 1e-9);
        }

        /// <summary>
        /// test split cubes eat others
        /// </summary>
        [TestMethod]
        public void WorldAbsorbing3()
        {
            World world = new World();
            world.GenerateNewCube("first");
            world.GenerateNewCube("second");
            Cube first = world.GetCube(0);
            Cube second = world.GetCube(1);
            List<Cube> list = null;

            first.Mass *= 3;
            world.Split(first, 0, 0, false);

            first.loc_x = second.loc_x;
            first.loc_y = second.loc_y;
            list = world.Absorbing(first);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(world.PlayerStartMass * 2.5, first.Mass, 1e-9);
            Assert.AreEqual(0, second.Mass, 1e-9);
        }
    }
}

