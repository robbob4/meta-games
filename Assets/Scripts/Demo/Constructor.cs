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
    #region Enums
    public enum ConstructionType
    {
        Floor = -1,
        Lobby = 0,
        Shop = 100,
        FastFood = 150,
        Restaurant = 151,
        Office = 200,
        Apartment = 201,
        Hotel = 202
    }

    public enum ToolType
    {
        Inspect,
        Build,
        Destroy
    }
    #endregion

    #region Variables
    //unit sizing
    public const int UNIT_HEIGHT = 10;
    public const int UNIT_WIDTH = 4;
    
    //construction
    [HideInInspector] public GameObject RoomToSpawn = null;
    private GameObject theRoom = null;
    private RoomStats descriptionBox = null;
    private ToolType currentTool = ToolType.Inspect;
    private bool placing = false;

    //placement
    public bool collided = true;
    public Vector3 target = new Vector3(0, 0, 0);
    #endregion

    void Start()
    {
        #region References
        descriptionBox = GameObject.Find("RoomStats").GetComponent<RoomStats>();
        if (descriptionBox == null)
            Debug.LogError("RoomStats not found for " + this + ".");
        #endregion
    }

    // Update is called once per frame
    void Update ()
    {
        #region Construction
        if (currentTool == ToolType.Build)
        {
            //TODO: Update theRoom.transform.position to mouse pos
            calculateTarget();
            theRoom.transform.position = target;

            //LMB down - commit construction?
            if (Input.GetMouseButtonDown(0))
            {
                //check whether the placement is valid
                if (validPlacement())
                {
                    placing = true;
                    //TODO: commit the object
                    //TODO: deduct cost from player funds
                    //theRoom.GetComponent<Room>().ConstructionCost;
                    //TODO: set room's Temp to false if gameplpay is not paused
                    //theRoom.GetComponent<Room>().Temp = false;

                    //prepare a new room to repeat construction
                    theRoom = (GameObject)Instantiate(RoomToSpawn);
                }
            }

            //LMB up - exit construction mode
            if ((Input.GetMouseButtonUp(0) && placing == true) || Input.GetAxis("Cancel") == 1 || Input.GetMouseButtonUp(1))
            {
                ExitConstructionMode();
            }
        }
        #endregion

        #region Deconstruction
        if (currentTool == ToolType.Destroy)
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
            if (Input.GetAxis("Cancel") == 1 || Input.GetMouseButtonUp(1))
            {
                ExitDeconstructionMode();
            }
        }
        #endregion
    }

    #region Construction functions
    //enter construction mode where a room follows the mouse cursor
    public void EnterConstructionMode(ConstructionType type)
    {
        if (currentTool != ToolType.Build)
        {
            //update mode
            if (currentTool == ToolType.Destroy)
                ExitDeconstructionMode();
            currentTool = ToolType.Build;

            //instantiate a temp room
            //RoomToSpawn = Resources.Load(roomType.PrefabLocation) as GameObject;
            //if (RoomToSpawn == null)
            //    Debug.LogError("Prefab not found for " + roomType + "(" + roomType.PrefabLocation + ").");
            //theRoom = (GameObject)Instantiate(RoomToSpawn);

            switch (type)
            {
                case ConstructionType.Shop:
                    RoomToSpawn = Resources.Load("Prefabs/Room/Shop") as GameObject;
                    theRoom = (GameObject)Instantiate(RoomToSpawn, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y)), RoomToSpawn.transform.rotation);
                    descriptionBox.Select("Shop");
                    break;
                case ConstructionType.Office:
                    RoomToSpawn = Resources.Load("Prefabs/Room/Office") as GameObject;
                    theRoom = (GameObject)Instantiate(RoomToSpawn, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y)), RoomToSpawn.transform.rotation);
                    descriptionBox.Select("Office");
                    break;
                default:
                    break;
            }
        }
    }

    //overload for unity onClick() that doesnt support enums
    public void EnterConstructionMode(int type)
    {
        EnterConstructionMode((ConstructionType)type);
    }

    //leave construction mode
    private void ExitConstructionMode()
    {
        placing = false;
        currentTool = ToolType.Inspect;
        RoomToSpawn = null;
        if (theRoom != null)
            Destroy(theRoom);

        theRoom = null;
        descriptionBox.SelectNone();
    }

    private bool validPlacement()
    {
        if (currentTool != ToolType.Build)
            return false;

        //TODO: check the validity of the room's current position
        //check all positions immediately below this room to be either floor or ground
        //and check whether there is space in current position

        // array of the items at the same position as "this" block
        Collider[] hitColliders = Physics.OverlapSphere(theRoom.transform.position, 0.01f);

        if (Input.GetMouseButtonDown(0) && hitColliders.Length == 1)
        {
            //// if block is not following the mouse
            //if (!blockFollowMouse)
            //{

            //    // vector 1 height unit above the block
            //    Vector3 v3 = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);

            //    //if there is nothing at the 1 height unit above the block, allow the block to move
            //    if (!Physics.CheckSphere(v3, 0.01f))
            //    {
            //        //block can move
            //        Debug.LogFormat("we moving!");
            //        blockFollowMouse = !blockFollowMouse;
            //        transform.position = new Vector3(transform.position.x, transform.position.y, -10); //float above other objects
            //    }
            //    // for debug purposes
            //    else
            //    {
            //        Debug.LogFormat("damn there is some fool upstairs!");
            //    }
            //}

            //if the block is following the mouse
            //else if (blockFollowMouse)
            //{
            // vector 1 height unit below the block
            Vector3 v3 = new Vector3(target.x, target.y - 10, target.z);

            //hitColliders = Physics.OverlapSphere(this.transform.position, 1.0f);

            // for debug purposes
            Debug.LogFormat("there are" + hitColliders.Length + " fools here!");
            Debug.Log(Physics.CheckSphere(v3, 0.01f));

            // if there is an item below the block, and there is exactly 1 item in the same spot as the block
            if (Physics.CheckSphere(v3, 0.01f) && hitColliders.Length == 1)
            {
                Debug.LogFormat("yo, we staying here");
            return true;
                //blockFollowMouse = !blockFollowMouse;
                //transform.position = new Vector3(transform.position.x, transform.position.y); //return z to 0
            }
            //}
        }

        return false;
    }

    //Calculates a blocks "target position and sets the center of the blocks position to that vector
    void calculateTarget()
    {
        //current mouse position
        Vector3 currMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        //Debug.Log ("mouse: " + currMousePosition);

        //target x calculation 
        int x_relativeToWidth = Mathf.FloorToInt(currMousePosition.x % 4.0f);
        if (x_relativeToWidth > 4 / 2)
        {
            target.x = Mathf.FloorToInt(currMousePosition.x) - Mathf.FloorToInt(x_relativeToWidth - 4 / 2);
        }
        else if (x_relativeToWidth < 4 / 2)
        {
            //else
            target.x = Mathf.FloorToInt(currMousePosition.x) + Mathf.FloorToInt(4 / 2 - x_relativeToWidth);
        }

        //target y calculation
        int y_relativeToWidth = Mathf.FloorToInt(currMousePosition.y % 10);
        if (x_relativeToWidth > 10 / 2)
        {
            target.y = Mathf.FloorToInt(currMousePosition.y) - Mathf.FloorToInt(y_relativeToWidth - 10 / 2);
        }
        else if (x_relativeToWidth < 10 / 2)
        {
            //else
            target.y = Mathf.FloorToInt(currMousePosition.y) + Mathf.FloorToInt(10 / 2 - y_relativeToWidth);
        }
    }
    #endregion

    #region Deconstruction functions
    //enter deconstruction mode 
    public void EnterDeconstructionMode()
    {
        if (currentTool != ToolType.Destroy)
        {
            //update mode
            if (currentTool == ToolType.Build)
                ExitConstructionMode();
            currentTool = ToolType.Destroy;
        }
    }

    //leave deconstruction mode
    private void ExitDeconstructionMode()
    {
        currentTool = ToolType.Inspect;
    }
    #endregion
}