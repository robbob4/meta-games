// ------------------------------- Patron.cs -----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - May 18, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a base patron.
// ----------------------------------------------------------------------------
// Notes - Contains the patron's happiness, money, and interests. Each interest
// must be a value from 0-100. Should the patron's money or happiness reach 0, 
// it will route out of the tower.
// Set movement to true and update currentFloor when ejecting a patron.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Patron : MonoBehaviour
{
    #region Enums
    public enum Interest
    {
        None = -1,
        Fashion = 0,
        Gadgets = 1,
        Entertainment = 2,
        Health = 3,
        Spicy = 4,
        Savory = 5,
        Sweet = 6
    }
    #endregion

    #region Variables
    protected int[] interests = new int[7]; //array of interests all 1-99
    protected Destination nextDest = null;
    protected Destination finalDest = null;
    protected float speed = 0.3f;
    protected static GlobalGameManager globalGameManager = null;
    protected static Pathing pather = null;
    private int happiness = 50; //0-100
    private int money = 1000; //0-x
    private int floor = 1; //what floor the patron is on
    private bool movement = true;
    private bool worker = false;
    #endregion

    // Use this for fast initialization
    void Awake()
    {
        #region References
        globalGameManager = GameObject.Find("GameManager").GetComponent<GlobalGameManager>();
        if (globalGameManager == null)
            Debug.LogError("GameManager's GlobalGameManager not found for " + this + ".");

        pather = GameObject.Find("GameManager").GetComponent<Pathing>();
        if (pather == null)
            Debug.LogError("GameManager's Pathing not found for " + this + ".");
        #endregion

        #region Generate interests
        float min = 1.0f;
        float max = 99.0f;
        for (int i = 0; i < interests.Length; i++)
        {
            interests[i] = Mathf.CeilToInt(Random.Range(min, max));
        }
        #endregion
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        #region Move towards next destination
        if (movement == true && globalGameManager.Paused == false && nextDest != null)
        {
            if (transform.position.x <= nextDest.transform.position.x)
                transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
            else
                transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
        }
        #endregion

        #region Reached next destination
        if (movement == true && nextDest != null && Mathf.Abs(nextDest.transform.position.x - transform.position.x) <= nextDest.transform.lossyScale.x / 2)
        {
            Debug.Log("Reached " + nextDest);

            //temp: move up to this floor
            transform.position = new Vector3(transform.position.x, nextDest.transform.position.y - 2, transform.position.z);

            //temp: destroy me when done
            if (nextDest == finalDest && nextDest == globalGameManager.Lobby)
            {
                Destroy(gameObject);
                Debug.Log("finished");
            }

            //check if it is a room destination or transportation destination
            if (nextDest.Transportation == true)
            {
                //TODO: check if this is transportation we need
                movement = false;
                nextDest.Visit(this);
                floor = nextDest.Floor; //temp
            }
            else
            {
                //determine if we should visit this room
                if (worker == false && interestCheck(nextDest.TheInterest))
                {
                    //try to visit the room
                    if (nextDest.Visit(this))
                    {
                        Happiness += 10;
                        Money -= nextDest.Rent;
                        movement = false;
                        floor = nextDest.Floor; //temp
                    }
                    else
                    {
                        //Status message: crowded
                        globalGameManager.NewStatus(nextDest.name + " is crowded!", true);
                        Happiness -= 20; //happiness deducation
                    }
                }
            }

            //find new destination if it is not our final destination
            if (nextDest != finalDest)
            {
                nextDest = pather.NextDestination(nextDest, finalDest);
            }
            else
            {
                //temp - go home
                Destroy(gameObject);
                //nextDest = pather.NextDestination(finalDest, globalGameManager.Lobby);
                //finalDest = globalGameManager.Lobby;
            }
                
        }
        #endregion

        #region Unhappy check
        if (Money <= 0 || Happiness <= 0)
        {
            //leave the building
            nextDest = pather.NextDestination(nextDest, globalGameManager.Lobby);
            finalDest = globalGameManager.Lobby;

            if (Happiness <= 0)
                globalGameManager.NewStatus(this + " is fed up with this tower!", true);
        }
        #endregion
    }

    //set next destination
    public void setDestination(Destination newFinalDest)
    {
        if (newFinalDest != null)
            finalDest = newFinalDest;
        else
            finalDest = globalGameManager.Lobby;
        nextDest = pather.NextDestination(globalGameManager.Lobby, finalDest);
    }

    // randomly determine whether the interest check passed
    private bool interestCheck(Interest check)
    {
        if (check == Interest.None)
            return false;

        float prob = Random.Range(0.0f, 100.0f);
        if (prob <= interests[(int)check])
            return true;
        else
            return false;
    }

    //query an interest value
    public int QueryInterest(Interest query)
    {
        return interests[(int)query];
    }

    #region Getters and Setters
    public int CurrentFloor
    {
        get { return floor; }
        set { floor = value; }
    }

    public int Happiness
    {
        get { return happiness; }
        set
        {
            if (value >= 0 && value <= 100)
                happiness = value;
            else if (value <= 0)
                happiness = 0;
            else
                happiness = 100;
        }
    }

    public int Money
    {
        get { return money; }
        set
        {
            if (value >= 0)
                money = value;
            else
                money = 0;
        }
    }

    public bool Movement
    {
        get { return movement; }
        set { movement = value; }
    }

    public bool Worker
    {
        get { return worker; }
        set { worker = value; }
    }
    #endregion
}