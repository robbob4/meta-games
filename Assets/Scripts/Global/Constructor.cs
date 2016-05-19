// ---------------------------- Constructor.cs --------------------------------
// Author - Robert Griswold CSS 385
// Created - May 2, 2016
// Modified - May 18, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for the script that handles room construction and 
// deconstruction. Calling EnterConstructionMode(Room) attaches a room to the 
// mouse and updates the position of the new room with the mouse snapping to a 
// grid. If the user clicks, this script will check whether it is valid 
// placement, and then place the structure.
// In deconstruction mode, releasing a left click on a room will refund funds
// to the player if it is a temp structure (game is still paused since its 
// construction), call the Evict function if it is not temporary, then destroy
// the game object. Some destruction behavior in BlockBehavior.cs.
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

    //references
    [SerializeField] private Texture2D hammerCursor = null;
    private RoomStats descriptionBox = null;
    private GlobalGameManager globalGameManager = null;
    protected static Pathing pather = null;

    //construction
    private GameObject roomToSpawn = null;
	private GameObject floorToSpawn = null;
    private GameObject theRoom = null;
	private GameObject theFloor = null;
    private ToolType currentTool = ToolType.Inspect;
    private bool placing = false;
    private Vector3 target = new Vector3(0, 0, 0);
    #endregion

    void Start()
    {
        #region References
        descriptionBox = GameObject.Find("RoomStats").GetComponent<RoomStats>();
        if (descriptionBox == null)
            Debug.LogError("RoomStats not found for " + this + ".");

        globalGameManager = GameObject.Find("GameManager").GetComponent<GlobalGameManager>();
        if (globalGameManager == null)
            Debug.LogError("GameManager not found for " + this + ".");

        floorToSpawn = Resources.Load("Prefabs/Room/Floor") as GameObject;
        if (floorToSpawn == null)
            Debug.LogError("floorToSpawn not found for " + this + ".");

        if (hammerCursor == null)
            Debug.LogError("hammerCursor not set for " + this + ".");

        pather = GameObject.Find("GameManager").GetComponent<Pathing>();
        if (pather == null)
            Debug.LogError("GameManager's Pathing not found for " + this + ".");
        #endregion
    }

    // Update is called once per frame
    void Update ()
    {
        #region Construction
        if (currentTool == ToolType.Build && theRoom != null)
        {
            //Update theRoom.transform.position to mouse pos
            calculateTarget();
            theRoom.transform.position = target;

            //LMB held down - commit construction?
            if (Input.GetMouseButton(0))
            {
                int theCost = theRoom.GetComponent<Destination>().ConstructionCost;

                //check whether the placement is valid
                if (validPlacement() && globalGameManager.Money >= theCost)
                {
                    placing = true;
                    globalGameManager.GetSoundEffect("construction_s").Play();
                    globalGameManager.Deduct(theCost); //Deduct cost from player funds

                    //commit anything for the room
                    Room tempRoomComp = theRoom.GetComponent<Room>();
                    tempRoomComp.Floor = Mathf.RoundToInt((theRoom.transform.position.y + UNIT_HEIGHT / 2) / UNIT_HEIGHT);
                    addFloors(); //add floors behind this floor
                    pather.AddDestination(tempRoomComp);
                    //TODO: set room's Temp to false if gameplpay is not paused
                    theRoom.GetComponent<Room>().Temp = false;
                    
                    //prepare a new room to repeat construction
                    theRoom = (GameObject)Instantiate(roomToSpawn);
                    theRoom.transform.position = target;
                }
                else if (globalGameManager.Money < theCost)
                {
                    globalGameManager.NewStatus("Insufficent funds for construction.", false);
                }
            }

            //LMB up or cancel - exit construction mode
            if ((Input.GetMouseButtonUp(0) && placing == true) || Input.GetAxis("Cancel") == 1 || Input.GetMouseButtonUp(1))
            {
                ExitConstructionMode();
            }
        }
        #endregion

        #region Deconstruction
        //Cancel - exit deconstruction mode
        if (Input.GetAxis("Cancel") == 1 || Input.GetMouseButtonUp(1))
        {
            exitDeconstructionMode();
        }
        #endregion
    }

    #region Construction functions
    //enter construction mode where a room follows the mouse cursor
    public void EnterConstructionMode(ConstructionType type)
    {
        //update mode
        if (currentTool == ToolType.Destroy)
            exitDeconstructionMode();
        else if (currentTool == ToolType.Build)
            ExitConstructionMode();
        currentTool = ToolType.Build;

        theRoom = null;

        switch (type)
        {
            case ConstructionType.Shop:
                roomToSpawn = Resources.Load("Prefabs/Room/Shop") as GameObject;
                theRoom = (GameObject)Instantiate(roomToSpawn,
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y)),
                    roomToSpawn.transform.rotation);
                descriptionBox.SetTitle("Shop");
                break;

            case ConstructionType.FastFood:
                roomToSpawn = Resources.Load("Prefabs/Room/FastFood") as GameObject;
                theRoom = (GameObject)Instantiate(roomToSpawn,
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y)),
                    roomToSpawn.transform.rotation);
                descriptionBox.SetTitle("Fast Food");
                break;

            case ConstructionType.Restaurant:
                roomToSpawn = Resources.Load("Prefabs/Room/Restaurant") as GameObject;
                theRoom = (GameObject)Instantiate(roomToSpawn,
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y)),
                    roomToSpawn.transform.rotation);
                descriptionBox.SetTitle("Restaurant");
                break;

            case ConstructionType.Office:
                roomToSpawn = Resources.Load("Prefabs/Room/Office") as GameObject;
                theRoom = (GameObject)Instantiate(roomToSpawn,
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y)),
                    roomToSpawn.transform.rotation);
                descriptionBox.SetTitle("Office");
                break;

            case ConstructionType.Hotel:
                roomToSpawn = Resources.Load("Prefabs/Room/Hotel") as GameObject;
                theRoom = (GameObject)Instantiate(roomToSpawn,
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y)),
                    roomToSpawn.transform.rotation);
                descriptionBox.SetTitle("Hotel");
                break;

            case ConstructionType.Apartment:
                roomToSpawn = Resources.Load("Prefabs/Room/Apartment") as GameObject;
                theRoom = (GameObject)Instantiate(roomToSpawn,
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y)),
                    roomToSpawn.transform.rotation);
                descriptionBox.SetTitle("Apartment");
                break;

            default:
                Debug.Log("Unknown ConstructionType (" + type + ") for " + this + ".");
                break;
        }

        if (theRoom != null)
        {
            Destination temp = theRoom.GetComponent<Destination>();
            descriptionBox.SetCost(temp.ConstructionCost);
            descriptionBox.SetUpkeep(temp.Maint);
            descriptionBox.SetSize(temp.RoomSize);
            descriptionBox.SetDesc(temp.Description);
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
        roomToSpawn = null;
        if (theRoom != null)
            Destroy(theRoom);

        theRoom = null;
        descriptionBox.SelectNone();
    }

    //check the validity of the room's current position
    private bool validPlacement()
    {
        if (currentTool != ToolType.Build)
            return false;

        // array of the items at the same position as "this" block
        Collider[] hitColliders = Physics.OverlapBox(theRoom.transform.position, theRoom.transform.localScale / 2);

        if (hitColliders.Length == 1)
        {
            Vector3 underTarget = new Vector3(target.x, target.y - 10, target.z + 1);
			int size = (int)theRoom.GetComponent<Room>().RoomSize;
			int midSpot = (size / 2) + 1;

			for (int i = 1; i <= size; i++)
            {
				Vector3 underTargetTemp;

				if (i < midSpot)
                {
					underTargetTemp = new Vector3 (underTarget.x - (i * 4), underTarget.y, underTarget.z);
				}
                else if (i == midSpot)
                {
					underTargetTemp = underTarget;
				}
                else // i > midSpot
                { 
					underTargetTemp = new Vector3 (underTarget.x + (i - midSpot)  * 4, underTarget.y, underTarget.z);
				}

				//Debug.LogFormat ("there are" + hitColliders.Length + " fools here!");

				// if there is an item below the block, and there is exactly 1 item in the same spot as the block
				if (!Physics.CheckSphere (underTargetTemp, 0.01f))
                {
					return false;
				}
			}

			return true;
		}

        return false;
    }

    private void addFloors()
    {
        Vector3 underTarget = new Vector3(target.x, target.y, target.z + 1);

        int size = (int)theRoom.GetComponent<Room>().RoomSize;

        int midSpot = (size / 2) + 1;

        for (int i = 1; i <= size; i++)
        {
            Vector3 underTargetTemp;

            if (i < midSpot)
            {
                underTargetTemp = new Vector3(underTarget.x - (i * 4), underTarget.y, underTarget.z);
            }
            else if (i == midSpot)
            {
                underTargetTemp = underTarget;
            }
            else // i > midSpot
            {
                underTargetTemp = new Vector3(underTarget.x + (i - midSpot) * 4, underTarget.y, underTarget.z);
            }

            if (!Physics.CheckSphere(underTargetTemp, 0.01f))
            {
                theFloor = (GameObject)Instantiate(floorToSpawn, underTargetTemp, floorToSpawn.transform.rotation);
                //TODO: Preemtively disallow construction if not enough funds for floors, currently allows negative funds
                globalGameManager.Deduct(theFloor.GetComponent<Destination>().ConstructionCost); //Deduct cost from player funds
                globalGameManager.GetSoundEffect("floor_s").Play();
            }
        }
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
            Cursor.SetCursor(hammerCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    //leave deconstruction mode
    private void exitDeconstructionMode()
    {
        currentTool = ToolType.Inspect;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    //toggle deconstruction mode
    public void ToggleConstructionMode()
    {
        if (currentTool == ToolType.Destroy)
            exitDeconstructionMode();
        else
            EnterDeconstructionMode();
    }
    #endregion

    #region Getters and setters
    public ToolType SelectedTool
    {
        get { return currentTool; }
    }
    #endregion
}