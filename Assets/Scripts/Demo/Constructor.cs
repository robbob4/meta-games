// ---------------------------- Constructor.cs --------------------------------
// Author - Robert Griswold CSS 385
// Created - May 2, 2016
// Modified - May 3, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for the script that handles room construction and 
// deconstruction. Calling EnterConstructionMode(Room) attaches a room to the 
// mouse and updates the position of the new room with the mouse snapping to a 
// grid. If the user clicks, this script will check whether it is valid 
// placement, and then place the structure.
// In deconstruction mode, releasing a left click on a room will refund funds
// to the player if it is a temp structure (game is still paused since its 
// construction), call the Evict function if it is not temporary, then destroy
// the game object.
// ----------------------------------------------------------------------------
// Notes - Remain in construction mode if not a valid placement.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Constructor : MonoBehaviour
{
    #region Variables
    //unit sizing
    public const int UNIT_HEIGHT = 10;
    public const int UNIT_WIDTH = 4;
    
    //construction
    [HideInInspector] public GameObject RoomToSpawn = null;
    private GameObject theRoom = null;
    private bool constructionMode = false;

    //deconstruction
    private bool deconstructionMode = false;
    #endregion
	
	// Update is called once per frame
	void Update ()
    {
        #region Construction
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
                    //TODO: deduct cost from player funds
                    //TODO: set room's Temp to false if gameplpay is not paused

                    //prepare a new room to repeat construction
                    theRoom = (GameObject)Instantiate(RoomToSpawn);
                }
            }

            //LMB up - exit construction mode
            if (Input.GetMouseButtonUp(0) || Input.GetAxis("Cancel") == 1)
            {
                ExitConstructionMode();
            }
        }
        #endregion

        #region Deconstruction
        if (deconstructionMode)
        {
            //LMB up - deconstruct?
            if (Input.GetMouseButtonUp(0))
            {
                //check what room the cursor is over
                Room selectedRoom = null; //TODO: change to the selected room

                //check the room's temp bool
                if (selectedRoom.Temp == true)
                {
                    //TODO: refund cost to player funds
                }
                else
                {
                    //call anything the room needs to do on deconstruction
                    selectedRoom.Evict();
                }

                Destroy(selectedRoom);
            }

            //Cancel - exit deconstruction mode
            if (Input.GetAxis("Cancel") == 1)
            {
                ExitDeconstructionMode();
            }
        }
        #endregion
    }

    #region Construction functions
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
    #endregion

    #region Deconstruction functions
    //enter construction mode where a room follows the mouse cursor
    public void EnterDeconstructionMode()
    {
        if (!deconstructionMode)
        {
            //update mode
            deconstructionMode = true;
        }
    }

    //leave construction mode
    public void ExitDeconstructionMode()
    {
        deconstructionMode = false;
    }
    #endregion
}