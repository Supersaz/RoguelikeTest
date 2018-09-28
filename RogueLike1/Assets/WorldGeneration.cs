using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour {
    
    // Use this for initialization
    void Start () {

        GenerateFeatures(5);
        DisplayWorld();
    }

    private const int WorldMAX_ROW = 50;
    private const int WorldMAX_COL = 70;

    int[,] World = new int[WorldMAX_ROW, WorldMAX_COL];

    public struct Vector2
    {
        public int X, Y;

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    Vector2 currentPosition;

    public void DisplayWorld()
    {
        //for (int row = 0; row < World.GetLength(0); row++)
        //{
        //    for (int col = 0; col < World.GetLength(1); col++)
        //    {
        //        if (World[row, col] == 2)
        //        {
        //            World[row, col] = 1;
        //        }
        //    }
        //}

        for (int row = 0; row < World.GetLength(0); row++)
        {
            for (int col = 0; col < World.GetLength(1); col++)
            {
                if (World[row, col] == 0)
                {
                    Instantiate(Resources.Load("Wall"), new Vector3(row, col, 0), Quaternion.identity);
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
                switch (Random.Range(0, 2))
                {
                    case 0:
                        MakeRoom(currentPosition, Random.Range(-10, 10), Random.Range(-10, 10));
                        break;
                    case 1:
                        //MakeCorridor(currentPosition, direction, Length);
                        break;
                }
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
            currentPosition.X = row - 1;
            currentPosition.Y = col;
            World[row, col] = 2;
        }
        else if (World[row - 1, col] == 1)
        {
            currentPosition.X = row + 1;
            currentPosition.Y = col;
            World[row, col] = 2;
        }
        else if (World[row, col + 1] == 1)
        {
            currentPosition.X = row;
            currentPosition.Y = col - 1;
            World[row, col] = 2;
        }
        else if (World[row, col - 1] == 1)
        {
            currentPosition.X = row;
            currentPosition.Y = col + 1;
            World[row, col] = 2;
        }
    }

    public void MakeRoom(Vector2 startCoord, int LengthX, int LengthY)
    {
        if ((startCoord.X > 0 && startCoord.X + LengthX < World.GetLength(1)) &&
            (startCoord.Y > 0 && startCoord.Y + LengthY < World.GetLength(0)))
        {

            //for (int row = startCoord.Y - 1; row < startCoord.Y + LengthY + 1; row++)
            //{
            //    for (int col = startCoord.X - 1; col < startCoord.X + LengthX + 1; col++)
            //    {
            //        if (World[row,col] == 1)
            //        {
            //            return;
            //        }
            //    }
            //}

            for (int row = startCoord.Y; row < startCoord.Y + LengthY; row++)
            {
                for (int col = startCoord.X; col < startCoord.X + LengthX; col++)
                {
                    World[row, col] = 1;
                }
            }
        }
    }

    public void MakeCorridor(Vector2 startCoord, int LengthX, int LengthY)
    {
        if ((startCoord.X > 0 && startCoord.X + LengthX < World.GetLength(1)) &&
            (startCoord.Y > 0 && startCoord.Y + LengthY < World.GetLength(0)))
        {

            for (int row = startCoord.Y; row < startCoord.Y + LengthY; row++)
            {
                for (int col = startCoord.X; col < startCoord.X + LengthX; col++)
                {
                    World[row, col] = 1;
                }
            }
        }
    }
}
