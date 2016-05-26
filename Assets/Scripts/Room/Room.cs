// -------------------------------- Room.cs -----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 2, 2016
// Modified - May 26, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a base room class that inherits from the 
// Destination class. Specifies happiness, spawnchance, and spawnDelayModulo.
// spawnDelayModulo just specifies what minute to attempt to spawn.
// ----------------------------------------------------------------------------
// Notes - Child must call spawner() if desired in update.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Room : Destination
{
    #region Variables
    //live variables
    protected int spawnChance = 5; //0-100%
    protected int spawnDelayModulo = 15; //game minutes modulo between spawn attempts (spawns when mod result is 0)
    protected int lastModulo = -1; //just used to track the last modulo incase a frame skips over the exact result
    #endregion

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
                {
                    //Debug.Log(prob);
                }
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
    #endregion
}
