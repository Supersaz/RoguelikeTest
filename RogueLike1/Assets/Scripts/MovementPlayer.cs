using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayer : MonoBehaviour {

	WorldGeneration WG = new WorldGeneration();
	public Vector2 currentPosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		tryMovement();
	}

	public void findPosition()
	{
		for(int row = 1; row < WG.World.GetLength(0) - 1; row++)
		{
			for(int col = 1; col < WG.World.GetLength(1) - 1; col++)
			{
				if (WG.World[row, col] == 9)
					currentPosition = new Vector2(row, col);
			}
		}
	}

	void tryMovement()
	{
		if(Input.GetKeyDown("a"))
		{
			Debug.Log("GOD");
			if (WG.World[(int)currentPosition.x - 1, (int)currentPosition.y] == 1)
			{
				transform.position = new Vector3(currentPosition.x - 1, 0, currentPosition.y);
			}
		}
	}
}