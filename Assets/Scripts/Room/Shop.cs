// -------------------------------- Shop.cs -----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - May 26, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a shop room that inherits from the retail
// class.
// ----------------------------------------------------------------------------
// Notes - Randomly selects an associated Patron interest.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Shop : Retail
{
    // Use this for fast initialization
    void Awake()
    {
        roomSize = Room.Size.Medium;
        capacity = 10;
        constructionCost = 100000;
        maint = 1000;
        rent = 500;
        desc = "Attracts patrons to your tower during business hours. The associated retail interest is randomly selected during construction.";

        #region Randomize a texture to use
        Material newMaterial = null;
        int rand = Mathf.RoundToInt(Random.Range(-0.5f, 6.4f)); //0-6 
        switch (rand)
        {
            case 0:
                newMaterial = Resources.Load("Textures/Room/Shops/Materials/BedBathShop") as Material;
                name = "Bed & Bath Shop"; //Sets the name of the room
                break;
            case 1:
                newMaterial = Resources.Load("Textures/Room/Shops/Materials/ConvenienceStore") as Material;
                name = "Convenience Store"; //Sets the name of the room
                break;
            case 2:
                newMaterial = Resources.Load("Textures/Room/Shops/Materials/FormalShop") as Material;
                name = "Formal Wear Shop"; //Sets the name of the room
                break;
            case 3:
                newMaterial = Resources.Load("Textures/Room/Shops/Materials/FurnitureStore") as Material;
                name = "Furniture Store"; //Sets the name of the room
                break;
            case 4:
                newMaterial = Resources.Load("Textures/Room/Shops/Materials/HipsterShop") as Material;
                name = "Hipster Shop"; //Sets the name of the room
                break;
            case 5:
                newMaterial = Resources.Load("Textures/Room/Shops/Materials/ClothingStore") as Material;
                name = "Clothing Store"; //Sets the name of the room
                break;
            default:
                newMaterial = Resources.Load("Textures/Room/Shops/Materials/Shop") as Material;
                name = "Shop"; //Sets the name of the room
                break;
        }
        if (newMaterial == null)
        {
            newMaterial = Resources.Load("Textures/Room/Shops/Materials/Shop") as Material;
            name = "Shop"; //Sets the name of the room
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
        theInterest = (Patron.Interest)Mathf.RoundToInt(Random.Range(-0.5f, 3.4f));
    }

    // Update is called once per frame
    void Update()
    {
		maintainance ();
		spawner();
    }
}