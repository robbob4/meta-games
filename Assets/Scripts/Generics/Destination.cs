// ---------------------------- Destination.cs --------------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - May 12, 2016
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
        Small = 5, //5 units wide
        Medium = 10, //10 units wide
        Large = 20 //20 units wide
    }
    #endregion

    #region Variables
    //construction variables
    [SerializeField] protected static Size roomSize = Size.Small; //width of the room
    private bool temp = true; //bool to flag this as a new room
    protected string prefabLocation = "";
    protected static int constructionCost = 0; //0-x

    //live variables
    [SerializeField] protected Patron.Interest theInterest = Patron.Interest.None; //associated interest
    [SerializeField] private bool transportation;
    [SerializeField] protected int capacity; //0-x
    [SerializeField] private int rent; //0-x
    [SerializeField] private int maint; //0-x
    protected Patron[] visitors;
    protected int currentCapacity = 0; //0-_capacity
    #endregion

    // Use this for fast initialization
    void Awake()
    {
        
    }

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    //announcment that the entity is about to be destroyed
    public virtual void Evict()
    {
        for (int i = 0; i < capacity; i++)
        {
            visitors[i].Movement = true;
        }
        //set patron finalDest to lobby if applicable
    }

    public void Visit(Patron visitor)
    {
        if (currentCapacity < MaxCapacity)
        {
            visitors[currentCapacity++] = visitor;
        }
        else
        {
            Debug.Log(visitor + " cannot enter, " + this + " is full. ");
        }
    }

    #region Getters and Setters
    //construction variables
    public Size RoomSize
    {
        get { return roomSize; }
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

    public string PrefabLocation
    {
        get { return prefabLocation; }
    }    

    public int ConstructionCost
    {
        get { return constructionCost; }
    }

    //live variables
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

    public int Maint
    {
        get { return maint; }
    }
    #endregion
}
