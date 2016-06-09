// ------------------------------- Leased.cs ----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - May 18, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a base leased room that inherits from the room
// class.
// ----------------------------------------------------------------------------
// Notes - Reduced spawn chance to 3%.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Leased : Room
{
    #region Variables
    protected int spawnCount = 0;
    protected bool rented = false;
    protected GameObject[] rentors = null;
    #endregion

    protected override void maintainance ()
	{
		if (gameTimer.Hour == 0 && !gameTimer.PM && !maintainanceDeducted) {
			spawnCount = 0;
            if(Traffic <= 20)
                globalGameManager.NewStatus("A " + this.name + " has a traffic of " + Traffic + ".", true);
        }

        if(Visits == capacity)
            rented = true;

        base.maintainance ();
	}

    protected virtual void savedSpawning(PatronType type)
    {
        if (spawnCount < capacity)
        {
            if (!rented)
            {
                randomizePatron(type);
                rentors[spawnCount] = patronToSpawn; //save them
                if (spawnCount == 0 && spawner() == true)
                {
                    spawnCount++;                        
                }
                else
                {
                    if (spawner(true) == true)
                    {
                        spawnCount++;
                    }
                }
            }
            else
            {
                patronToSpawn = rentors[spawnCount]; //load the same looking guy(s)
                if (spawner(true) == true)
                {
                    spawnCount++;
                }
            }
        }
    }

    protected override IEnumerator VisitDelay(int i)
	{
		yield return new WaitForSeconds(delay);
	}

    //getters
    public bool Rented
    {
        get { return rented; }
    }
}
