// ---------------------------- Destination.cs --------------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - May 18, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a base destination class for use in path 
// finding.
// ----------------------------------------------------------------------------
// Notes - Indicates whether the entity is a transportation entity such that 
// a patron will enter it even if it not their final destination.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Destination : MonoBehaviour
{
    #region Enums
    public enum Size
    {
        Tiny = 1, //1 unit wide
        Small = 3, //3 units wide
        Medium = 5, //5 units wide
        Large = 9 //9 units wide
    }
    #endregion

    #region Variables
    //references
    protected static GlobalGameManager globalGameManager = null;
    protected static GameTime gameTimer = null;
    protected static GameObject patronToSpawn = null;

    //construction variables
    protected string desc = "";
    protected Size roomSize = Size.Small; //width of the room in units
    protected int constructionCost = 100; //0-x
    private bool temp = true; //bool to flag this as a new room
    private bool transportation = false;
    [SerializeField] private int floor; //what floor this room is on

    //live variables
    protected int maint = 10; //0-x
    protected int rent; //0-x
    [SerializeField] protected Patron.Interest theInterest = Patron.Interest.None; //associated interest
    protected int capacity = 0; //0-x
    protected int currentCapacity = 0; //0-capacity
    protected Patron[] visitors = new Patron[0];
    #endregion

    void Awake()
    {
        #region References
        globalGameManager = GameObject.Find("GameManager").GetComponent<GlobalGameManager>();
        if (globalGameManager == null)
            Debug.LogError("GameManager's GlobalGameManager not found for " + this + ".");

        gameTimer = globalGameManager.GetComponent<GameTime>();
        if (gameTimer == null)
            Debug.LogError("GameTime not found for " + this + ".");

        patronToSpawn = Resources.Load("Prefabs/Patron/Patron") as GameObject;
        #endregion
    }

    //announcment that the entity is about to be destroyed
    public virtual void Evict()
    {
        for (int i = 0; i < capacity; i++)
        {
            if (visitors[i] != null)
            {
                visitors[i].Movement = true;
                visitors[i].setDestination(null); //set patron finalDest to lobby
                visitors[i] = null;
            }
        }
    }

    //returns a bool whether the visit was successful
    public bool Visit(Patron visitor)
    {
        if (currentCapacity < MaxCapacity)
        {
            visitors[currentCapacity++] = visitor;
            globalGameManager.GetSoundEffect("cash_s").Play();
            globalGameManager.Payment(this.rent);

            //temp
            StartCoroutine(TempEjection());

            return true;
        }
        else
        {
            return false;
        }
    }

    //demo ejection after 3 seconds
    IEnumerator TempEjection()
    {
        Patron visitor = visitors[currentCapacity - 1];

        yield return new WaitForSeconds(4);

        visitors[--currentCapacity] = null;
        visitor.Movement = true;
    }

    #region Getters and Setters
    //construction variables
    public string Description
    {
        get { return desc; }
    }

    public Size RoomSize
    {
        get { return roomSize; }
    }

    public int ConstructionCost
    {
        get { return constructionCost; }
    }

    public bool Temp
    {
        get { return temp; }
        set
        {
            if (temp == true) //can only be temp once
                temp = value;
            else
                Debug.Log(value + " is invalid temp value for " + this + ".");
        }
    }

    public int Floor
    {
        get { return floor; }
    }

    //live variables
    public int Maint
    {
        get { return maint; }
    }

    public Patron.Interest TheInterest
    {
        get { return theInterest; }
    }

    public bool Transportation
    {
        get { return transportation; }
    }

    public int MaxCapacity
    {
        get { return capacity; }
    }

    public int CurrentCapacity
    {
        get { return currentCapacity; }
    }

    public int Rent
    {
        get { return rent; }
    }
    #endregion
}
