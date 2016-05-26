// -------------------------------- Stairwell.cs ------------------------------
// Author - Robert Griswold CSS 385
// Created - May 26, 2016
// Modified - May 26, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a stairwell room that inherits from the room
// class.
// ----------------------------------------------------------------------------
// Notes - Transportation type room: Connects nearest floor below to upper
// floor.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Stairwell : Room
{
    // Use this for fast initialization
    void Awake()
    {
        roomSize = Room.Size.Small;
        constructionCost = 1000;
        maint = 10;
        rent = 0;
        capacity = 50;
        transportation = true;
        desc = "Stairwells connect the floor immediately below it allowing patrons to change one floor.";
    }

    // Use this for initialization
    void Start()
    {
        visitors = new Patron[capacity];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool Visit(Patron visitor)
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
            
            visits++;
            Happiness += 10;

            StartCoroutine(VisitDelay(i));

            return true;
        }
        else
        {
            Happiness -= 20;
            return false;
        }
    }

    // ejection after 2 seconds
    // assumes array is not rearranged
    IEnumerator VisitDelay(int i)
    {
        yield return new WaitForSeconds(2);

        // swap floors
        if(visitors[i].CurrentFloor != Floor)
        {
            visitors[i].CurrentFloor = Floor;
            visitors[i].transform.position = new Vector3(visitors[i].transform.position.x, transform.position.y - 2, visitors[i].transform.position.z);
        }
        else
        {
            visitors[i].CurrentFloor = Floor - 1;
            visitors[i].transform.position = new Vector3(visitors[i].transform.position.x, transform.position.y - Constructor.UNIT_HEIGHT - 2, visitors[i].transform.position.z);
        }

        visitors[i].Movement = true;
        currentCapacity--;
        visitors[i] = null;
    }
}
