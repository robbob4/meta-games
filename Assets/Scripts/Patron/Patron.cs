﻿// ------------------------------- Patron.cs -----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - May 12, 2016
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
    protected float speed = 0.1f;
    protected static GlobalGameManager globalGameManager = null;
    private int happiness = 50; //0-100
    private int money = 1; //0-x
    private int currentFloor = 1;
    private bool movement = true;
    private bool worker = false;
    #endregion
    
    // Use this for initialization
    void Start()
    {
        #region References
        globalGameManager = GameObject.Find("GameManager").GetComponent<GlobalGameManager>();
        if (globalGameManager == null)
            Debug.LogError("GameManager not found.");
        #endregion

        #region Generate interests
        float min = 1.0f;
        float max = 99.0f;
        for (int i = 0; i < interests.Length; i++)
        {
            interests[i] = Mathf.CeilToInt(Random.Range(min, max));
        }
        #endregion

        nextDest = GameObject.Find("Shop").GetComponent<Destination>();
        finalDest = nextDest;
        //TODO: Determine next destination from graph
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
        if (movement == true && nextDest != null && nextDest.transform.position.x - transform.position.x <= nextDest.transform.lossyScale.x / 2)
        {
            Debug.Log("Reached next Dest");
            if (nextDest != finalDest)
            {
                //TODO: update next destination from graph
            }

            //check if it is a room destination or transportation destination
            if (nextDest.Transportation == true)
            {
                //TODO: check if this is transportation we need
                movement = false;
                nextDest.Visit(this);
            }
            else
            {
                //determine if we should visit this room
                if (worker == false && interestCheck(nextDest.TheInterest))
                {
                    //try to visit the room
                    if (nextDest.CurrentCapacity < nextDest.MaxCapacity)
                    {
                        movement = false;
                        nextDest.Visit(this);
                        Debug.Log("Visiting " + nextDest);
                    }
                    else
                    {
                        Happiness -= 10; //happiness deducation
                    }
                }
            }            
        }
        #endregion
    }

    //set next destination
    public void setDestination(Destination next)
    {
        if (next != null)
            finalDest = next;
        else
            Debug.Log(next + " is invalid destination for " + this + ".");
    }

    // randomly determine whether the interest check passed
    private bool interestCheck(Interest check)
    {
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
        get { return currentFloor; }
        set { currentFloor = value; }
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
                Debug.Log(value + " is invalid money value for " + this + ".");
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