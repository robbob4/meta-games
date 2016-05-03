// -------------------------------- Room.cs -----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 2, 2016
// Modified - May 3, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a genreic room class.
// ----------------------------------------------------------------------------
// Notes - Child must set _theRoom to load the prefab. Temp can only be 
// changed while it is true. Happiness values are only valid between 0-100.
// CurrentCapacity values only valid between 0-MaxCapacity.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour
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
    protected static int _constructionCost = 0; //0-x
    protected static Size _roomSize = Size.Small; //width of the room
    protected static string _prefabLocation = "";
    private bool _temp = true; //bool to flag this as a new room

    //live variables
    protected int _spawnChance = 50; //0-100%
    protected int _rent = 0; //0-x
    protected int _maint = 0; //0-x
    protected int _happiness = 0; //0-100%
    protected int _capacity = 1; //0-x
    private int _currentCapacity = 0; //0-_capacity
    protected Texture _tex = null;
    #endregion

    // Use this for fast initialization
    void Awake ()
    {
	
	}

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update ()
    {
	
	}

    #region Deconstruction
    //announcment that the room is about to be destroyed
    public virtual void Evict()
    {

    }
    #endregion

    #region Getters and setters
    //construction
    public int ConstructionCost
    {
        get { return _constructionCost; }
    }

    public Size RoomSize
    {
        get { return _roomSize; }
    }

    public string PrefabLocation
    {
        get { return _prefabLocation; }
    }

    public bool Temp
    {
        get { return _temp; }
        set
        {
            if (_temp == true) //can only be temp once
                _temp = value;
            else
                Debug.Log(value + " is invalid temp value for " + this + ".");
        }
    }

    //live
    public float SpawnChance
    {
        get { return _spawnChance; }
    }

    public int Rent
    {
        get { return _rent; }
    }

    public int Maint
    {
        get { return _maint; }
    }

    public int Happiness
    {
        get { return _happiness; }
        set
        {
            if (value >= 0 && value <= 100)
                _happiness = value;
            else
                Debug.Log(value + " is invalid happiness value for " + this + ".");
        }
    }

    public int MaxCapacity
    {
        get { return _capacity; }
    }

    public int CurrentCapacity
    {
        get { return _currentCapacity; }
        set
        {
            if (value >= 0 && value <= _capacity)
                _currentCapacity = value;
            else
                Debug.Log(value + " is invalid currentCapacity value for " + this + ".");
        }
    }
    #endregion
}
