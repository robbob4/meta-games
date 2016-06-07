// ------------------------------ FastFood.cs ---------------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - May 26, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a fast food room that inherits from the retail
// class.
// ----------------------------------------------------------------------------
// Notes - Randomly selects an associated Patron interest.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class FastFood : Retail
{
    // Use this for fast initialization
    void Awake()
    {
		roomSize = Room.Size.Medium;
        capacity = 10;
        constructionCost = 100000;
        maint = 1000;
        rent = 500;
        desc = "Attracts patrons to your tower during business hours. The associated cuisine interest is randomly selected during construction.";

        #region Randomize a texture to use
        Material newMaterial = null;
        int rand = Mathf.RoundToInt(Random.Range(-0.5f, 2.4f)); //0-2
        switch (rand)
        {
            case 0:
                newMaterial = Resources.Load("Textures/Room/FastFood/Materials/Bakery") as Material;
                name = "Bakery"; //Sets the name of the room
                break;
            case 1:
                newMaterial = Resources.Load("Textures/Room/FastFood/Materials/Chinesefood") as Material;
                name = "Chinese Food"; //Sets the name of the room
                break;
            default:
                newMaterial = Resources.Load("Textures/Room/FastFood/Materials/FastFood") as Material;
                name = "Fast Food"; //Sets the name of the room
                break;
        }
        if (newMaterial == null)
        {
            newMaterial = Resources.Load("Textures/Room/FastFood/Materials/FastFood") as Material;
            name = "Fast Food"; //Sets the name of the room
            Debug.Log("Unable to find material for " + this + ".");
        }
        GetComponent<Renderer>().material = newMaterial;
        #endregion
    }

    // Use this for initialization
    void Start()
    {
        visitors = new Patron[capacity];

        //Generate the interest type
        theInterest = (Patron.Interest)Mathf.RoundToInt(Random.Range(3.5f, 6.5f));
    }

    // Update is called once per frame
    void Update()
    {
		maintainance ();

        spawner();
    }
}
