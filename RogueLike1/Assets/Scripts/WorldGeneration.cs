using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour {
    
    // Use this for initialization
    void Start () {
        CreateWorld(featureCount);
    }
        private const int WorldMAX_ROW = 100;
        private const int WorldMAX_COL = 100;
        int[,] World = new int[WorldMAX_ROW, WorldMAX_COL];
        private Vector2 currentPosition;
        public int featureCount;
        public int RoomSizeMAX = 5;
        public int RoomSizeMIN = -5;

        public void CreateWorld(int Features)
        {
            GenerateFeatures(Features);
            SpawnPlayer();
            for (int row = 0; row < World.GetLength(0); row++)
            {
                for (int col = 0; col < World.GetLength(1); col++)
                {
                    switch(World[row, col])
                    {
                        case 0:
                            Instantiate(Resources.Load("Wall"), new Vector3(row, 0, col), Quaternion.identity);
                            break;
                        case 1:
                            Instantiate(Resources.Load("Floor"), new Vector3(row, -0.1f, col), Quaternion.identity);
                            break;
                        case 2:
                            Instantiate(Resources.Load("Floor"), new Vector3(row, -0.1f, col), Quaternion.identity);
                            break;
                        case 9:
                            Instantiate(Resources.Load("Floor"), new Vector3(row, -0.1f, col), Quaternion.identity);
                            Instantiate(Resources.Load("Player"), new Vector3(row, 0, col), Quaternion.identity);
                            break;
                        default:
                            break;
                    }
                }
            }
            
        }

        public void GenerateFeatures(int featureCount)
        {
            MakeRoom(new Vector2(WorldMAX_ROW / 2, WorldMAX_COL / 2), 2, 2); // Create inital room // FIXXXXX
            FindWall();

            int BuildTries = 10; // Attempts at placing a feature

            for (int tries = 0; tries < BuildTries; tries++)
            {
                for (int i = 0; i < featureCount; i++)
                {
                    MakeRoom(currentPosition, Random.Range(RoomSizeMIN, RoomSizeMAX), Random.Range(RoomSizeMIN, RoomSizeMAX));
                    FindWall();
                }
            }
        }

        private void FindWall()
        {
            bool isWallFound = false;

            while (isWallFound == false)
            {
                int tempCol = Random.Range(1, WorldMAX_COL - 1);
                int tempRow = Random.Range(1, WorldMAX_ROW - 1);

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
                currentPosition.y = row - 1;
                currentPosition.x = col;
                World[row, col] = 2;
            }
            else if (World[row - 1, col] == 1)
            {
                currentPosition.y = row + 1;
                currentPosition.x = col;
                World[row, col] = 2;
            }
            else if (World[row, col + 1] == 1)
            {
                currentPosition.y = row;
                currentPosition.x = col - 1;
                World[row, col] = 2;
            }
            else if (World[row, col - 1] == 1)
            {
                currentPosition.y = row;
                currentPosition.x = col + 1;
                World[row, col] = 2;
            }
        }

        public void MakeRoom(Vector2 startCoord, int LengthX, int LengthY)
        {
            if ((startCoord.x > 0 && startCoord.x + LengthX < World.GetLength(0)) &&
                (startCoord.y > 0 && startCoord.y + LengthY < World.GetLength(1)))
            {
                if (LengthX >= 0 && LengthY >= 0)
                {
                    for (int row = (int)startCoord.y - 1; row < startCoord.y + LengthY + 1; row++)
                    {
                        for (int col = (int)startCoord.x - 1; col < startCoord.x + LengthX + 1; col++)
                        {
                            if (World[row, col] == 1)
                            {
                                return;
                            }
                        }
                    }

                    for (int row = (int)startCoord.y; row < (int)startCoord.y + LengthY; row++)
                    {
                        for (int col = (int)startCoord.x; col < (int)startCoord.x + LengthX; col++)
                        {
                            World[row, col] = 1;
                        }
                    }
                }
                else if (LengthX < 0 && LengthY < 0)
                {
                    for (int row = (int)startCoord.y + 1; row > (int)startCoord.y + LengthY - 1; row--)
                    {
                        for (int col = (int)startCoord.x + 1; col > (int)startCoord.x + LengthX - 1; col--)
                        {
                            if (World[row, col] == 1)
                            {
                                return;
                            }
                        }
                    }

                    for (int row = (int)startCoord.y; row > (int)startCoord.y + LengthY; row--)
                    {
                        for (int col = (int)startCoord.x; col > (int)startCoord.x + LengthX; col--)
                        {
                            World[row, col] = 1;
                        }
                    }
                }
                else if (LengthX >= 0 && LengthY < 0) // MAYBE FIX
                {
                    for (int row = (int)startCoord.y + 1; row > (int)startCoord.y + LengthY - 1; row--)
                    {
                        for (int col = (int)startCoord.x - 1; col < (int)startCoord.x + LengthX + 1; col++)
                        {
                            if (World[row, col] == 1)
                            {
                                return;
                            }
                        }
                    }

                    for (int row = (int)startCoord.y; row > (int)startCoord.y + LengthY; row--)
                    {
                        for (int col = (int)startCoord.x; col < (int)startCoord.x + LengthX; col++)
                        {
                            World[row, col] = 1;
                        }
                    }
                }
                else if (LengthX < 0 && LengthY >= 0) // MAYBE FIX
                {
                    for (int row = (int)startCoord.y - 1; row < (int)startCoord.y + LengthY + 1; row++)
                    {
                        for (int col = (int)startCoord.x + 1; col > (int)startCoord.x + LengthX - 1; col--)
                        {
                            if (World[row, col] == 1)
                            {
                                return;
                            }
                        }
                    }

                    for (int row = (int)startCoord.y; row < (int)startCoord.y + LengthY; row++)
                    {
                        for (int col = (int)startCoord.x; col > (int)startCoord.x + LengthX; col--)
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
                Vector2 currentPoint = new Vector2(Random.Range(1, WorldMAX_ROW - 1), Random.Range(1, WorldMAX_COL - 1));

                if (World[(int)currentPoint.x, (int)currentPoint.y] == 1)
                {
                    World[(int)currentPoint.x, (int)currentPoint.y] = 9;
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
