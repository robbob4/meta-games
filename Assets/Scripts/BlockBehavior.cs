using UnityEngine;
using System.Collections;

public class BlockBehavior : MonoBehaviour {

	public bool blockFollowMouse = false;
	public bool collided = false;
	public Vector3 target = new Vector3 (0, 0, 0);	

	void Start () {
		gameObject.GetComponent<Renderer> ().material.color = Color.green;
	}
	
	void Update () 
	{
		if (blockFollowMouse) {
			calculateTarget ();
			this.transform.position = target;
		}
	}

	void calculateTarget()
	{
		//current mouse position
		Vector3 currMousePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
		Debug.Log ("mouse: " + currMousePosition);

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

	void OnMouseDown()
	{
		if(!blockFollowMouse) // if not following the block and the block is clicked on
			blockFollowMouse = !blockFollowMouse;
		else if (!collided && blockFollowMouse) //if we are following the block and not currently colliding and the block is clicked on
			blockFollowMouse = !blockFollowMouse;
	}

	void OnMouseDrag()
	{
	}
	/*
	void OnCollisionEnter (Collision col)
	{
		collided = true;
		Debug.LogFormat ("enter");
	}

	void OnCollisionExit (Collision col)
	{
		collided = true;
	}
*/
}
