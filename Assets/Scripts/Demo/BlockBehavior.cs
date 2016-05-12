﻿// ------------------------ ButtonController.cs -------------------------------
// Author - Sam Williams CSS 385
// Created - May 4, 2016
// Modified - May 4, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a block that follows the mouse while adhering 
// to a grid system for the protoype demo.
// ----------------------------------------------------------------------------
// Notes - (Needs comments).
// ----------------------------------------------------------------------------

// 10 hours

using UnityEngine;
using System.Collections;

public class BlockBehavior : MonoBehaviour
{
	public bool blockFollowMouse = true;
	public bool collided = true;
	public Vector3 target = new Vector3 (0, 0, 0);	

	void Start ()
    {
		gameObject.GetComponent<Renderer>().material.color = Color.red;
	}
	
	void Update () 
	{
		if (blockFollowMouse) {
			calculateTarget ();
			this.transform.position = target;
		}
	}

	//Calculates a blocks "target position - as defined by Robert Gridwold," and sets the center of the blocks position to that vector
	void calculateTarget()
	{
		//current mouse position
		Vector3 currMousePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
		//Debug.Log ("mouse: " + currMousePosition);

		//target x calculation 
		int x_relativeToWidth = Mathf.FloorToInt(currMousePosition.x % 4.0f);
		if (x_relativeToWidth > 4 / 2) {
			target.x =  Mathf.FloorToInt(currMousePosition.x) - Mathf.FloorToInt( x_relativeToWidth - 4 / 2);
		}
		else if (x_relativeToWidth < 4 / 2) {
		//else
			target.x = Mathf.FloorToInt(currMousePosition.x) + Mathf.FloorToInt( 4 / 2 - x_relativeToWidth );
		}

		//target y calculation
		int y_relativeToWidth = Mathf.FloorToInt(currMousePosition.y % 10);
		if (x_relativeToWidth > 10 / 2) {
			target.y = Mathf.FloorToInt(currMousePosition.y) - Mathf.FloorToInt( y_relativeToWidth - 10 / 2);
		}
		else if (x_relativeToWidth < 10 / 2) {
		//else
			target.y = Mathf.FloorToInt(currMousePosition.y)+ Mathf.FloorToInt( 10/2 - y_relativeToWidth);
		}
	}

	// to change for larger blocks, loop over the physics checks, incrementing or decrementing the v3
	// need to compensate for height = 0
	// test up -> one large box collider
	// test down -> loop over smallest boxes until width has been covered
	void OnMouseOver()
	{
		// array of the items at the same position as "this" block
		Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 1.0f);

		if (Input.GetMouseButtonDown (0) && hitColliders.Length == 1) 
		{
			// if block is not following the mouse
			if (!blockFollowMouse) { 

				// vector 1 height unit above the block
				Vector3 v3 = new Vector3 (transform.position.x, transform.position.y + 10, transform.position.z);

				//if there is nothing at the 1 height unit above the block, allow the block to move
				if (!Physics.CheckSphere (v3, 1.0f)) 
				{
					//block can move
					Debug.LogFormat ("we moving!");
					blockFollowMouse = !blockFollowMouse;
				} 
				// for debug purposes
				else 
				{
					Debug.LogFormat ("damn there is some fool upstairs!");
				}
			} 

			//if the block is following the mouse
			else if (blockFollowMouse) 
			{ 
				// vector 1 height unit below the block
				Vector3 v3 = new Vector3 (target.x, target.y - 10, target.z);

				//hitColliders = Physics.OverlapSphere(this.transform.position, 1.0f);

				// for debug purposes
				Debug.LogFormat("there are" + hitColliders.Length + " fools here!");

				// if there is an item below the block, and there is exactly 1 item in the same spot as the block
				if (Physics.CheckSphere (v3, 2.0f) && hitColliders.Length == 1) 
				{
					Debug.LogFormat ("yo, we staying here");
					blockFollowMouse = !blockFollowMouse;
				}
			}
		}
	}

}



/*
	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown (0)) {
			
			// create a collider the same dimenstions, 1 height above this blocks height.
			//if (referenceRoom == null)
			//	referenceRoom = Resources.Load ("Prefabs/BuilderHelpers/ProxyRoom 1") as GameObject;
			//Vector3 thisPos = this.transform.position;
			//referenceRoom = (GameObject)Instantiate (referenceRoom, thisPos, this.transform.rotation);

			// if not following the block and the block is clicked on
			if (!blockFollowMouse) { 
				
				Vector3 v3 = new Vector3 (transform.position.x, transform.position.y + 10, transform.position.z);
				//referenceRoom.transform.position = v3;

				//Debug.LogFormat ("" + referenceRoom.transform.position.x + " " + referenceRoom.transform.position.y + " "
				//+ referenceRoom.GetComponent<BlockBehavior> ().collided);
				
				//if (referenceRoom.GetComponent<BlockBehavior> ().returnCollideStatus () == false) {
				//	blockFollowMouse = !blockFollowMouse;
				//}

				//if there is nothing above
				if (!Physics.CheckSphere (v3, 1.0f)) {
					//block can move
					Debug.LogFormat ("we moving!");
					blockFollowMouse = !blockFollowMouse;
				} else {
					Debug.LogFormat ("damn upstairs upstairs!");
				}

				// destroy the referenceRoom block
				//Destroy (referenceRoom);
			} 

			//if the block is following the mouse and not currently colliding and the block is clicked on
			else if (blockFollowMouse) { 

				Vector3 v3 = new Vector3 (target.x, target.y - 10, target.z);
				//referenceRoom.GetComponent<BlockBehavior> ().setPosition (v3);
				//referenceRoom.transform.position = v3;

				// this room space is not occupied and the space below is occupied, 
				// then we can place the block
				//Debug.LogFormat ("" + referenceRoom.transform.position.x + " " + referenceRoom.transform.position.y + " "
				//+ referenceRoom.GetComponent<BlockBehavior> ().collided);
				
				//if (referenceRoom.GetComponent<BlockBehavior> ().returnCollideStatus () == true && !this.collided) {
				//	blockFollowMouse = !blockFollowMouse;
				//}

				// if there is something below
				Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 1.0f);
				Debug.LogFormat("there are" + hitColliders.Length + " fools here!");

				if (Physics.CheckSphere (v3, 2.0f) && hitColliders.Length == 1) {
					Debug.LogFormat ("yo, we staying here");
					blockFollowMouse = !blockFollowMouse;
				}

				// destroy the referenceRoom block
				//Destroy (referenceRoom);
			}
		}
	}
	*/

/*
	void OnTriggerStay(Collider other) {
		this.collided = true;
		//col.collider = true;
		Debug.LogFormat ("Enter, YO");
	}		

	void OnTriggerEnter(Collider other) {
		this.collided = true;
		//col.collider = true;
		Debug.LogFormat ("Enter, YO");
	}		

	void OnTriggerExit(Collider other) {
		this.collided = false;
		//col.collider = true;
		Debug.LogFormat ("Enter, YO");
	}	
	
	void OnCollisionStay(Collision col)
	{
		this.collided = true;
		//col.collider = true;
		Debug.LogFormat ("Enter, YO");
	}

	void OnCollisionEnter(Collision col)
	{
		this.collided = true;
		//col.collider = true;
		Debug.LogFormat ("Enter, YO");
	}

	void OnCollisionExit (Collision col)
	{
		//col.collider = false;
		this.collided = false;
		Debug.LogFormat ("Exit! :)");
	}

	bool returnCollideStatus()
	{
		return this.collided;
	}
	*/

//referenceRoom.GetComponent<BlockBehavior> ().transform.position = v3;
//referenceRoom.transform.position = v3;


//Vector3 spawnPos = v3;
//float radius = 2.0f;

//if (Physics.CheckSphere (spawnPos, radius)) {
//	Debug.LogFormat ("Wut de hell mon, x = " + referenceRoom.GetComponent<BlockBehavior> ().transform.position.x
//		+ "Wut de hell mon, x = " + referenceRoom.GetComponent<BlockBehavior> ().transform.position.y);
//} else {
// spot is empty, we can spawn
//Spawn();
//	blockFollowMouse = !blockFollowMouse;
//}
//referenceRoom.transform.transform.position = v3;