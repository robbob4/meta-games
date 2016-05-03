// ---------------------------- Constructor.cs --------------------------------
// Author - Robert Griswold CSS 385
// Created - May 2, 2016
// Modified - May 2, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for the script that handles room construction.
// Calling EnterConstructionMode(Room) attaches a room to the mouse and updates
// the position of the new room with the mouse snapping to a grid. If the user 
// clicks, this script will check whether it is valid placement, and then place
// the structure.
// ----------------------------------------------------------------------------
// Notes - Remain in construction mode if not a valid placement.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Constructor : MonoBehaviour
{
    #region Variables
    [HideInInspector] public GameObject RoomToSpawn = null;

    private GameObject theRoom = null;
    private bool constructionMode = false;
    #endregion
	
	// Update is called once per frame
	void Update ()
    {
        if (constructionMode)
        {
            //TODO: Update theRoom.transform.position to mouse pos

            //LMB down - commit construction?
            if (Input.GetMouseButtonDown(0))
            {
                //check whether the placement is valid
                if (validPlacement())
                {
                    //TODO: commit the object

                    //prepare a new room to repeat construction
                    theRoom = (GameObject)Instantiate(RoomToSpawn);
                }
            }

            //LMB up - exit construction
            if (Input.GetMouseButtonUp(0) || Input.GetAxis("Cancel") == 1)
            {
                ExitConstructionMode();
            }
        }
	}

    //enter construction mode where a room follows the mouse cursor
    public void EnterConstructionMode(Room roomType)
    {
        if (!constructionMode)
        {
            //update mode
            constructionMode = true;

            //instantiate a temp room
            RoomToSpawn = Resources.Load(roomType.PrefabLocation) as GameObject;
            if (RoomToSpawn == null)
                Debug.LogError("Prefab not found for " + roomType + "(" + roomType.PrefabLocation + ").");
            theRoom = (GameObject)Instantiate(RoomToSpawn);
        }
    }

    //leave construction mode
    public void ExitConstructionMode()
    {
        constructionMode = false;
        theRoom = null;
        RoomToSpawn = null;
    }

    private bool validPlacement()
    {
        if (!constructionMode)
            return false;

        //TODO: check the validity of the room's current position
        //check all positions immediately below this room to be either floor or ground
        //and check whether there is space in current position
        return true;
    }
}
