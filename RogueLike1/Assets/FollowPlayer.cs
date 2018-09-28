using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	public float lerpConst = 10;
	public Vector3 Offset;
	private GameObject playerObj;
	public string LookFor = "Player(Clone)";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!GameObject.Find(LookFor))
			return;

		playerObj = GameObject.Find(LookFor);
		transform.position = Vector3.Lerp(transform.position, playerObj.transform.position + Offset, Time.deltaTime * lerpConst);

	}
}
