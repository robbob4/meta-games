// ---------------------------- Destination.cs --------------------------------
// Author - Robert Griswold CSS 385
// Author - Samuel Williams CSS 385 
// Created - May 12, 2016
// Modified - May 26, 2016
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
    protected static Pathing pather = null;

    //construction variables
    protected string desc = "Floors provide structural integitry to your tower. They are automatically placed behind new rooms.";
    protected Size roomSize = Size.Tiny; //width of the room in units
    protected int constructionCost = 100; //0-x
    private bool temp = true; //bool to flag this as a new room
    protected bool transportation = false;
    [SerializeField] private int floor = 0; //what floor this room is on

    //live variables
    protected int maint = 1; //0-x
    protected int rent; //0-x
    [SerializeField] protected Patron.Interest theInterest = Patron.Interest.None; //associated interest
    protected int capacity = 0; //0-x
    protected int currentCapacity = 0; //0-capacity
    protected Patron[] visitors = new Patron[0];
    protected int visits = 0; //# of visits today
    protected int happiness = 50; //percentage, reset daily
    protected int delay = 4; //delay when visiting
	protected bool maintainanceDeducted = true;
    protected bool windowShopping = true;
	protected bool evicted = false;
	protected int evictHour = 6;
	protected bool evictPM = true;
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

        pather = globalGameManager.GetComponent<Pathing>();
        if (pather == null)
            Debug.LogError("Pather not found for " + this + ".");

        patronToSpawn = Resources.Load("Prefabs/Patron/Patron") as GameObject;
        #endregion
    }
		
	void Update()
	{
		maintainance ();
	}

	protected virtual void maintainance()
	{
        if(gameTimer != null)
        {
            if (gameTimer.Hour == 0 && !gameTimer.PM && !maintainanceDeducted)
            {
                globalGameManager.Deduct((float)maint);
                maintainanceDeducted = true;
                Visits = 0;
                Happiness -= 10;
                globalGameManager.GetSoundEffect("maint_s").Play();
            }
            else if (gameTimer.Hour != 0 && maintainanceDeducted)
            {
                maintainanceDeducted = false;
            }

            if (gameTimer.Hour == evictHour && gameTimer.PM == evictPM && !evicted)
            {
                evicted = true;
                Evict();
            }
            else if (gameTimer.Hour != evictHour && evicted)
            {
                evicted = false;
            }
        }
        else
        {
            Debug.LogError("Game Timer is null for " + this);
        }
	}

    //announcment that the entity is about to be destroyed
    public virtual void Evict()
    {
        for (int i = 0; i < capacity; i++)
        {
            if (visitors[i] != null)
            {
				currentCapacity--;
                visitors[i].Movement = true;
                visitors[i].setDestination(this, null); //set patron finalDest to lobby
                visitors[i] = null;
            }
        }
    }

    //returns a bool whether the visit was successful
    public virtual bool Visit(Patron visitor)
    {
        if (currentCapacity < MaxCapacity)
        {
            int i = 0;
            for (; i < visitors.Length; i++)
            {
                if (visitors[i] == null)
                {
                    visitors[i] = visitor;
                    currentCapacity++;
                    break;
                }
            }

            Visits++;
            Happiness += 10;
            globalGameManager.GetSoundEffect("cash_s").Play();
            globalGameManager.Payment(this.rent);
            StartCoroutine(VisitDelay(i));
			return true;
        }
        else
        {
            Happiness -= 20;
            return false;
        }
    }

    //demo ejection after delay (default 4) seconds
    protected virtual IEnumerator VisitDelay(int i)
    {
        yield return new WaitForSeconds(delay);

        if (visitors[i] != null)
        {
            visitors[i].Movement = true;
            visitors[i] = null;
            currentCapacity--;
        }
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
        set
        {
            if (floor == 0) //can only be set once
                floor = value;
            else
                Debug.Log(value + " is invalid floor value for " + this + ".");
        }
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

    public int Happiness
    {
        get { return happiness; }
        set
        {
			if (value >= 0 && value <= 100) {
				happiness = value;
				}
            else if (value <= 0)
                happiness = 0;
            else
                happiness = 100;
        }
    }

    public int Visits
    {
        get { return visits; }
		set { visits = value; }
    }

    public bool WindowShopping
    {
        get { return windowShopping; }
    }
    #endregion
}
