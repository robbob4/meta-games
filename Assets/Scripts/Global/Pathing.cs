// ---------------------------- Pathing.cs -------------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - May 12, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for pathing for patrons and rooms.
// ----------------------------------------------------------------------------
// Notes - None
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pathing : MonoBehaviour
{
    //variables
    Graph<Destination> rooms;

    // Use this for fast initialization
    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        rooms = new Graph<Destination>();
        Destination lobby = GameObject.Find("Lobby").GetComponent<Destination>();
        rooms.AddNode(lobby);

        //testing
        Destination shop = GameObject.Find("Shop").GetComponent<Destination>();
        rooms.AddNode(shop);
        rooms.AddUndirectedEdge(lobby, shop);
        Destination office = GameObject.Find("Office").GetComponent<Destination>();
        rooms.AddNode(office);
        rooms.AddUndirectedEdge(shop, office);
        Destination hotel = GameObject.Find("Hotel").GetComponent<Destination>();
        rooms.AddNode(hotel);
        rooms.AddUndirectedEdge(hotel, office);
    }

    //find the next destiation between two destinations
    public Destination nextDestination(Destination from, Destination to)
    {
        Destination result = null;

        //perform the search
        Stack<Destination> temp = rooms.DepthFirstSearch(to);

        //remove until there are no more results or we reach from point
        while (temp.Count > 0 && temp.Peek() != from)
            result = temp.Pop();

        Debug.Log("from " + from + " to " + to + " -> " + result);
        return result;
    }

    //blind search using strings and name comparison
    public Destination nextDestination(string from, string to)
    {
        Destination result = null;
        

        //perform a blind search
        Stack<Destination> temp = rooms.DepthFirstSearch();

        //pop until destination
        while (temp.Count > 0)
        {
            result = temp.Pop();
            if (result.name == to)
                break;
        }

        //check if we ran out or found it
        if (result.name != to)
            return null;

        //found it, so remove until there are no more research or we reach from point
        while (temp.Count > 0 && temp.Peek().name != from)
            result = temp.Pop();

        return result;
    }

    public bool pathExists(Destination target)
    {
        Stack<Destination> temp = rooms.DepthFirstSearch(target);
        return temp.Pop() == target;
    }
}
