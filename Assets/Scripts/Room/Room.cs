// -------------------------------- Room.cs -----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 2, 2016
// Modified - May 18, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a base room class that inherits from the 
// Destination class. Specifies happiness, spawnchance, and spawnDelayModulo.
// spawnDelayModulo just specifies what minute to attempt to spawn.
// ----------------------------------------------------------------------------
// Notes - Child must call initReferences in start or awake, and spawner if 
// desired in update.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Room : Destination
{
    #region Variables
    //references
    private static GlobalGameManager globalGameManager = null;
    private static GameTime gameTimer = null;
    private static GameObject patronToSpawn = null;

    //live variables
    protected int spawnChance = 5; //0-100%
    protected int spawnDelayModulo = 15; //game minutes modulo between spawn attempts (spawns when mod result is 0)
    protected int lastModulo = -1; //just used to track the last modulo incase a frame skips over the exact result
    protected int happiness = 50; //0-100%
    #endregion

    protected virtual void initReferences()
    {
        if (patronToSpawn == null) //only allow this to be run once
        {
            patronToSpawn = Resources.Load("Prefabs/Patron/Patron") as GameObject;

            globalGameManager = GameObject.Find("GameManager").GetComponent<GlobalGameManager>();
            if (globalGameManager == null)
                Debug.LogError("GameManager not found for " + this + ".");

            gameTimer = globalGameManager.GetComponent<GameTime>();
            if (gameTimer == null)
                Debug.LogError("GameTime not found for " + this + ".");
        }
    }

    protected virtual void spawner()
    {
        if (!Temp && !globalGameManager.Paused)
        {
            if (lastModulo > gameTimer.Min % spawnDelayModulo)
            {
                float prob = Random.Range(0.0f, 100.0f);
                if (prob <= spawnChance)
                {
                    GameObject thePatron = (GameObject)Instantiate(patronToSpawn);
                    thePatron.transform.position = globalGameManager.SpawnPosition;
                    thePatron.GetComponent<Patron>().setDestination(this.GetComponent<Destination>()); //route patron here
                }
                else
                    Debug.Log(prob);
            }

            lastModulo = gameTimer.Min % spawnDelayModulo;
        }
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
