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
    #region Variables
    Graph<Destination> rooms;
    Destination lastDestination = null;
    #endregion


    // Use this for fast initialization
    void Awake()
    {
        rooms = new Graph<Destination>();
        Destination fakeLobby = GameObject.Find("FakeLobby").GetComponent<Destination>();
        if (fakeLobby == null)
            Debug.Log("FakeLobby destination not found for " + this + ".");
        rooms.AddNode(fakeLobby);
        lastDestination = fakeLobby;

        //testing
        //Destination shop = GameObject.Find("Shop").GetComponent<Destination>();
        //GameObject.Find("Shop").GetComponent<Room>().Temp = false;
        //rooms.AddNode(shop);
        //rooms.AddUndirectedEdge(lobby, shop);
        //Destination office = GameObject.Find("Office").GetComponent<Destination>();
        //rooms.AddNode(office);
        //rooms.AddUndirectedEdge(shop, office);
        //Destination shop2 = GameObject.Find("Shop2").GetComponent<Destination>();
        //GameObject.Find("Shop2").GetComponent<Room>().Temp = false;
        //rooms.AddNode(shop2);
        //rooms.AddUndirectedEdge(office, shop2);
        //Destination hotel = GameObject.Find("Hotel").GetComponent<Destination>();
        //rooms.AddNode(hotel);
        //rooms.AddUndirectedEdge(hotel, shop2);
    }

    // Use this for initialization
    void Start()
    {
        
    }

    //find the next destiation between two destinations
    public Destination NextDestination(Destination from, Destination to)
    {
        Destination result = null;

        //perform the search
        //Stack<Destination> temp = rooms.DepthFirstSearch(to);
        Stack<Destination> temp = rooms.BreadthFirstSearch(to, from);

        //remove until there are no more results or we reach from point
        while (temp.Count > 0 && temp.Peek() != from)
            result = temp.Pop();

        Debug.Log("from " + from + " to " + to + " -> " + result);
        return result;
    }

    //blind search using strings and name comparison
    public Destination NextDestination(string from, string to)
    {
        Destination result = null;

        //perform a blind search
        //Stack<Destination> temp = rooms.DepthFirstSearch();
        Stack<Destination> temp = rooms.BreadthFirstSearch();

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

    public bool PathExists(Destination origin, Destination target)
    {
        //todo upgrade dfs for this action
        //Stack<Destination> temp = rooms.DepthFirstSearch(target);
        Stack<Destination> temp = rooms.BreadthFirstSearch(target, origin);
        return temp.Pop() == target;
    }

    //add a new destination
    //TODO: improve the graphing node logic
    public void AddDestination(Destination newDest)
    {
        rooms.AddNode(newDest);
        rooms.AddUndirectedEdge(newDest, lastDestination);
        lastDestination = newDest;
    }
}