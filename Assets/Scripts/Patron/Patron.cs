﻿// ------------------------------- Patron.cs -----------------------------------
// Author - Robert Griswold CSS 385 
// Author - Samuel Williams CSS 385 
// Created - May 12, 2016
// Modified - May 27, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a base patron.
// ----------------------------------------------------------------------------
// Notes - Contains the patron's traffic, money, and interests. Each interest
// must be a value from 0-100. Should the patron's money or traffic reach 0, 
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
        Stairs = -3,
        Locked = -2,
        None = -1,
        Fashion = 0,
        Gadgets = 1,
        Entertainment = 2,
        Health = 3,
        Spicy = 4,
        Savory = 5,
        Sweet = 6,
		Home = 7,
		Hotel = 8,
        Office = 9
    }
    #endregion

    #region Variables
    protected int[] interests = new int[10]; //array of interests all 1-99
    [SerializeField] protected Destination nextDest = null;
    [SerializeField] protected Destination finalDest = null;
    protected float speed = 0.3f;
    protected static GlobalGameManager globalGameManager = null;
    private static Pathing pather = null;
    private int happiness = 50; //0-100
    [SerializeField] private int money = 1000; //0-x
    [SerializeField] private int floor = 1; //what floor the patron is on
    [SerializeField] private bool movement = true;
    private bool worker = false;
    private bool resident = false;
    [SerializeField] private bool exiting = false;
	protected static GameTime gameTimer = null;
	private bool evicted = false;

    private int despawning = 100;
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

        gameTimer = globalGameManager.GetComponent<GameTime>();
        if (gameTimer == null)
            Debug.LogError("GameTime not found for " + this + ".");
        #endregion

        #region Generate interests
        float min = 1.0f;
        float max = 99.0f;
        for (int i = 0; i < interests.Length; i++)
        {
            interests[i] = Mathf.CeilToInt(Random.Range(min, max));
        }
		interests[7] = interests[8] = interests[9] = 100; //set leased rooms to 100% probability
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        #region Null checking
        if (nextDest == null && despawning % 10 == 0)
        {
            Debug.Log(this + " is unable to find any destination.");
            //try to find a new destination
			setDestination(null, true);
            despawning--;

            if (despawning <= 0)
                Destroy(gameObject);

            return;
        }

        if (nextDest == null || finalDest == null)
        {
            despawning--;
            return;
        }
        #endregion

        #region Daily evict
        if (gameTimer.Hour == 6 && gameTimer.PM && !evicted && movement)
        {
            evicted = true;

            if (!exiting)
            {
                setDestination(null);
                exiting = true;
            }
        }
        #endregion

        //only for debugging
        if (movement && nextDest == finalDest && finalDest == globalGameManager.Lobby && CurrentFloor != finalDest.Floor)
            Debug.Log(this + "shouldnt be here.");

        if (globalGameManager.Paused == false)
        {
            #region Move towards next destination
            if (movement == true && nextDest != null)
            {
                if (transform.position.x <= nextDest.transform.position.x)
                    transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
                else
                    transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
            }
            #endregion

            #region Check for floor
            //only check every unit distance
            Vector3 target = new Vector3(transform.position.x, transform.position.y, 1); 
            int x_relativeToWidth = Mathf.FloorToInt(transform.position.x % Constructor.UNIT_WIDTH);
            if (x_relativeToWidth > Constructor.UNIT_WIDTH / 2)
            {
                target.x = Mathf.FloorToInt(transform.position.x) - Mathf.FloorToInt(x_relativeToWidth - Constructor.UNIT_WIDTH / 2);
            }
            else if (x_relativeToWidth < Constructor.UNIT_WIDTH / 2)
            {
                target.x = Mathf.FloorToInt(transform.position.x) + Mathf.FloorToInt(Constructor.UNIT_WIDTH / 2 - x_relativeToWidth);
            }

            //determine if the floor ends
            if (CurrentFloor != 1 && !Physics.CheckSphere(target, 0.01f))
            {
                //route home
                //exiting = true;
                setDestination();
                return;
            }
            #endregion

            #region Reached next destination
            if (movement == true && Mathf.Abs(nextDest.transform.position.x - transform.position.x) <= nextDest.transform.localScale.x / 4)
            {
                //Debug.Log("Reached " + nextDest);

                //reached lobby exit?
                if (nextDest == finalDest && nextDest == globalGameManager.Lobby)
                {
                    //Debug.Log(this + " exited.");
                    Destroy(gameObject);
                }

                if (nextDest.Transportation == true) // next destination is transporation
                {
                    //check if this transporation gets us closer
                    Stairwell temp = (Stairwell)nextDest; //temp until a real transporation is implemented

                    if (CurrentFloor < finalDest.Floor) //going up
                    {
                        for (int i = finalDest.Floor; i > CurrentFloor; i--)
                        {
                            if (temp.FloorService(i) == true)
                            {
                                if (i != finalDest.Floor)
                                {
                                    //check if this transporation still has a valid route to final destination
                                    if (pather.PathExists(nextDest, finalDest) == false)
                                        break;
                                }
                                movement = false;
                                nextDest.Visit(this);
                                break;
                            }
                        }
                    }
                    else if (CurrentFloor > finalDest.Floor) //going down
                    {
                        for (int i = finalDest.Floor; i < CurrentFloor; i++)
                        {
                            if (temp.FloorService(i) == true)
                            {
                                if (i != finalDest.Floor)
                                {
                                    //check if this transporation has a valid route to final destination
                                    if (pather.PathExists(nextDest, finalDest) == false)
                                        break;
                                }
                                movement = false;
                                nextDest.Visit(this);
                                break;
                            }
                        }
                    } 
                }
                else //next destination is not a transporation
                {
                    //determine if we should visit this room
                    if (!globalGameManager.Paused && !exiting && ((nextDest.WindowShopping && Money >= nextDest.Rent) || nextDest == finalDest) 
                        && !worker  && CurrentFloor == nextDest.Floor && interestCheck(nextDest.TheInterest))
                    {
                        //try to visit the room
                        if (nextDest.Visit(this))
                        {
                            Happiness += 10;
                            Money -= nextDest.Rent;
                            movement = false;
                        }
                        else
                        {
                            globalGameManager.NewStatus(nextDest.name + " is crowded!", true);
                            Happiness -= 20; //traffic deducation
                        }
                    }
                    else if(CurrentFloor != nextDest.Floor)
                    {
                        Debug.Log("Patron trying to visit " + nextDest.name + " that is not on the same floor. (" + CurrentFloor + "->" + nextDest.Floor + ")");
                    }
                }

                //find new destination if it is not our final destination
                if (nextDest != finalDest)
                {
                    setDestination(finalDest);
                }
                else
                {
                    //go home
                    exiting = true;
                    setDestination();
                }
            }
            #endregion

            #region Unhappy check
            if (!exiting && ( (Money <= 0 && !Worker && !Resident) || Happiness <= 0))
            {
                //leave the building
                exiting = true;
                setDestination();

                if (Happiness <= 0)
                    globalGameManager.NewStatus(this.name + " is fed up with this tower!", true);
            }
            #endregion
        }
    }

    //set next destination
    public void setDestination(Destination newFinalDest)
    {
        //Destination oldnextDest = nextDest; //debug
        //Destination oldfinalDest = finalDest; //debug

        if (newFinalDest != null)
            finalDest = newFinalDest;
        else
            finalDest = globalGameManager.Lobby;

		if (nextDest == null)
        {
			//nextDest = pather.ClosestDestination(this.transform.position.x, CurrentFloor);
			nextDest = globalGameManager.Lobby;
		}

        nextDest = pather.NextDestination(nextDest, finalDest);
        //if (nextDest.Floor != finalDest.Floor) Debug.Log("rawr");
    }

    //overloaded setDestination for go to lobby
    public void setDestination()
    {
        setDestination(null);
    }

	public void setDestination(Destination from, Destination newFinalDestination)
	{
		nextDest = from;
		setDestination (newFinalDestination);
	}

	public void setDestination(Destination newFinalDestination, bool findClosest)
	{
		if (findClosest) {
			nextDest = pather.ClosestDestination(this.transform.position.x, CurrentFloor);
		}
		setDestination (newFinalDestination);			
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

    public int GetNextFloor()
    {
        return nextDest.Floor;
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

    public bool Resident
    {
        get { return resident; }
        set { resident = value; }
    }

    public bool Exiting
    {
        get { return exiting; }
        set { exiting = value; }
    }
    #endregion
}