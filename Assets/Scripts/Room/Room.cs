﻿// -------------------------------- Room.cs -----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 2, 2016
// Modified - May 27, 2016
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
    private int lastModulo = -1; //just used to track the last modulo incase a frame skips over the exact result

	protected int minHour = 7;
	protected int maxHour = 17;

    #endregion

	//hotel spawn 12 - 24
	//other spawn 7 - 17
    protected virtual bool spawner(bool worker)
    {
		bool retVal = false;
		if (gameTimer.Hour24 >= minHour && gameTimer.Hour24 <= maxHour) {
			//make sure this isnt temp, game isnt paused
			if (!Temp && !globalGameManager.Paused) {
				//Delay spawning perodically and only spawn if a path exists
	            
				if (lastModulo > gameTimer.Min % spawnDelayModulo && pather.PathExists (this, globalGameManager.Lobby)) {
					float prob = Random.Range (0.0f, 100.0f);
					if (prob <= spawnChance || worker) {
						GameObject thePatron = (GameObject)Instantiate (patronToSpawn);
						thePatron.transform.position = globalGameManager.SpawnPosition;
						thePatron.GetComponent<Patron> ().setDestination (this.GetComponent<Destination> ()); //route patron here
                        if (worker)
                            thePatron.GetComponent<Patron>().Resident = true;
                        retVal = true;
					}
                    else
                    {
						//Debug.Log(prob);
					}
				}

				lastModulo = gameTimer.Min % spawnDelayModulo;
			}
		}
		return retVal;
    }

    protected virtual bool spawner()
    {
        return spawner(false);
    }

    #region Getters and setters
    //live
    public float SpawnChance
    {
        get { return spawnChance; }
    }
    #endregion
}
