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
    //construction variables
    protected string desc = "";
    protected Size roomSize = Size.Small; //width of the room in units
    protected int constructionCost = 0; //0-x
    private bool temp = true; //bool to flag this as a new room
    private bool transportation = false;
    [SerializeField] private int floor; //what floor this room is on

    //live variables
    protected int maint; //0-x
    protected int rent; //0-x
    [SerializeField] protected Patron.Interest theInterest = Patron.Interest.None; //associated interest
    protected int capacity; //0-x
    protected int currentCapacity = 0; //0-capacity
    protected Patron[] visitors;
    #endregion

    //announcment that the entity is about to be destroyed
    public virtual void Evict()
    {
        for (int i = 0; i < capacity; i++)
        {
            visitors[i].Movement = true;
            visitors[i].setDestination(null);
        }
        //set patron finalDest to lobby if applicable
    }

    public void Visit(Patron visitor)
    {
        if (currentCapacity < MaxCapacity)
        {
            visitors[currentCapacity++] = visitor;

            //temp
            StartCoroutine(TempEjection());
        }
        else
        {
            Debug.Log(visitor + " cannot enter, " + this + " is full. ");
        }
    }

    //demo ejection after 3 seconds
    IEnumerator TempEjection()
    {
        Patron visitor = visitors[currentCapacity - 1];
        visitor.transform.position = new Vector3(visitor.transform.position.x, transform.position.y - 2, visitor.transform.position.z);

        yield return new WaitForSeconds(3);

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
