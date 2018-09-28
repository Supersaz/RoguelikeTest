using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueLikeGame1a
{
    class Program
    {
        static void Main(string[] args)
        {
            WorldGenerator WG = new WorldGenerator();

            WG.GenerateFeatures(10);
            WG.SpawnPlayer();
            WG.DisplayWorld();
        }
    }

    public struct Vector2
    {
        public int X, Y;

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class WorldGenerator
    {
        private const int WorldMAX_ROW = 60;
        private const int WorldMAX_COL = 60;

        int[,] World = new int[WorldMAX_ROW, WorldMAX_COL];

        Random R = new Random();
        Vector2 currentPosition;

        public void DisplayWorld()
        {
            for (int row = 0; row < World.GetLength(0); row++)
            {
                for (int col = 0; col < World.GetLength(1); col++)
                {
                    Console.Write(World[row, col]);
                }
                Console.WriteLine();
            }
        }

        public void GenerateFeatures(int featureCount)
        {
            Random R = new Random();
            MakeRoom(new Vector2(WorldMAX_ROW / 2, WorldMAX_COL / 2), 2, 2); // Create inital room // FIXXXXX
            FindWall();

            int BuildTries = 10; // Attempts at placing a feature

            for (int tries = 0; tries < BuildTries; tries++)
            {
                for (int i = 0; i < featureCount; i++)
                {
                    MakeRoom(currentPosition, R.Next(-5, 5), R.Next(-5, 5));
                    FindWall();
                }
            }
        }

        private void FindWall()
        {
            bool isWallFound = false;
            Random R = new Random();

            while (isWallFound == false)
            {
                int tempCol = R.Next(1, WorldMAX_COL - 1);
                int tempRow = R.Next(1, WorldMAX_ROW - 1);

                if (World[tempRow, tempCol] == 0)
                {
                    if (World[tempRow + 1, tempCol] == 1 ||
                        World[tempRow - 1, tempCol] == 1 ||
                        World[tempRow, tempCol + 1] == 1 ||
                        World[tempRow, tempCol - 1] == 1
                        )
                    {
                        FindDirection(tempRow, tempCol);
                        isWallFound = true;
                    }
                }
            }
        }

        public void FindDirection(int row, int col)
        {
            if (World[row + 1, col] == 1)
            {
                currentPosition.Y = row - 1;
                currentPosition.X = col;
                World[row, col] = 2;
            }
            else if (World[row - 1, col] == 1)
            {
                currentPosition.Y = row + 1;
                currentPosition.X = col;
                World[row, col] = 2;
            }
            else if (World[row, col + 1] == 1)
            {
                currentPosition.Y = row;
                currentPosition.X = col - 1;
                World[row, col] = 2;
            }
            else if (World[row, col - 1] == 1)
            {
                currentPosition.Y = row;
                currentPosition.X = col + 1;
                World[row, col] = 2;
            }
        }

        public void MakeRoom(Vector2 startCoord, int LengthX, int LengthY)
        {
            if ((startCoord.X > 0 && startCoord.X + LengthX < World.GetLength(0)) &&
                (startCoord.Y > 0 && startCoord.Y + LengthY < World.GetLength(1)))
            {
                if (LengthX >= 0 && LengthY >= 0)
                {
                    for (int row = startCoord.Y - 1; row < startCoord.Y + LengthY + 1; row++)
                    {
                        for (int col = startCoord.X - 1; col < startCoord.X + LengthX + 1; col++)
                        {
                            if (World[row, col] == 1)
                            {
                                return;
                            }
                        }
                    }

                    for (int row = startCoord.Y; row < startCoord.Y + LengthY; row++)
                    {
                        for (int col = startCoord.X; col < startCoord.X + LengthX; col++)
                        {
                            World[row, col] = 1;
                        }
                    }
                }
                else if (LengthX < 0 && LengthY < 0)
                {
                    for (int row = startCoord.Y + 1; row > startCoord.Y + LengthY - 1; row--)
                    {
                        for (int col = startCoord.X + 1; col > startCoord.X + LengthX - 1; col--)
                        {
                            if (World[row, col] == 1)
                            {
                                return;
                            }
                        }
                    }

                    for (int row = startCoord.Y; row > startCoord.Y + LengthY; row--)
                    {
                        for (int col = startCoord.X; col > startCoord.X + LengthX; col--)
                        {
                            World[row, col] = 1;
                        }
                    }
                }
                else if (LengthX >= 0 && LengthY < 0) // MAYBE FIX
                {
                    for (int row = startCoord.Y + 1; row > startCoord.Y + LengthY - 1; row--)
                    {
                        for (int col = startCoord.X - 1; col < startCoord.X + LengthX + 1; col++)
                        {
                            if (World[row, col] == 1)
                            {
                                return;
                            }
                        }
                    }

                    for (int row = startCoord.Y; row > startCoord.Y + LengthY; row--)
                    {
                        for (int col = startCoord.X; col < startCoord.X + LengthX; col++)
                        {
                            World[row, col] = 1;
                        }
                    }
                }
                else if (LengthX < 0 && LengthY >= 0) // MAYBE FIX
                {
                    for (int row = startCoord.Y - 1; row < startCoord.Y + LengthY + 1; row++)
                    {
                        for (int col = startCoord.X + 1; col > startCoord.X + LengthX - 1; col--)
                        {
                            if (World[row, col] == 1)
                            {
                                return;
                            }
                        }
                    }

                    for (int row = startCoord.Y; row < startCoord.Y + LengthY; row++)
                    {
                        for (int col = startCoord.X; col > startCoord.X + LengthX; col--)
                        {
                            World[row, col] = 1;
                        }
                    }
                }
            }
        }

        public void SpawnPlayer()
        {
            bool isPlayerSpawned = false;

            while(isPlayerSpawned == false)
            {
                Vector2 currentPoint = new Vector2(R.Next(1, WorldMAX_ROW - 1), R.Next(1, WorldMAX_COL - 1));

                if (World[currentPoint.X, currentPoint.Y] == 1)
                {
                    World[currentPoint.X, currentPoint.Y] = 9;
                    isPlayerSpawned = true;
                }
            }
        }
        //public void MakeCorridor(Vector2 startCoord, int LengthX, int LengthY)
        //{
        //    if ((startCoord.X > 0 && startCoord.X + LengthX < World.GetLength(1)) &&
        //        (startCoord.Y > 0 && startCoord.Y + LengthY < World.GetLength(0)))
        //    {

        //        for (int row = startCoord.Y; row < startCoord.Y + LengthY; row++)
        //        {
        //            for (int col = startCoord.X; col < startCoord.X + LengthX; col++)
        //            {
        //                World[row, col] = 1;
        //            }
        //        }
        //    }
        //}
    }
}