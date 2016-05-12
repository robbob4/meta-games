// -------------------------------- Room.cs -----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 2, 2016
// Modified - May 12, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a base room class that inherits from the 
// Destination class.
// ----------------------------------------------------------------------------
// Notes - Specifies happiness and spawnChance.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Room : Destination
{
    #region Variables
    //live variables
    [SerializeField] protected int spawnChance = 25; //0-100%
    protected int happiness = 50; //0-100%
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

    #region Getters and setters
    //live
    public float SpawnChance
    {
        get { return spawnChance; }
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
    #endregion
}
