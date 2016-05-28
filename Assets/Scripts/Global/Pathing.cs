// ---------------------------- Pathing.cs -------------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - May 27, 2016
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
    //references
    GlobalGameManager globalGameManager = null;

    //other
    public const int MAX_FLOORS_HIGH = 40;
    public const int MAX_FLOORS_WIDE = 10;
    Graph<Destination> rooms;
    Destination[,] roomsByFloor = new Destination[MAX_FLOORS_HIGH, MAX_FLOORS_WIDE];
    #endregion

    // Use this for fast initialization
    void Awake()
    {
        globalGameManager = GameObject.Find("GameManager").GetComponent<GlobalGameManager>();
        if (globalGameManager == null)
            Debug.Log("GameManager not found for " + this + ".");

        rooms = new Graph<Destination>();
        rooms.AddNode(globalGameManager.Lobby);
        globalGameManager.Lobby.Floor = 1;
        roomsByFloor[1, 0] = globalGameManager.Lobby;
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
        Stack<Destination> temp = rooms.DepthFirstSearch(to, from);
        //Stack<Destination> temp = rooms.BreadthFirstSearch(to, from);

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
        Stack<Destination> temp = rooms.DepthFirstSearch();
        //Stack<Destination> temp = rooms.BreadthFirstSearch();

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

        Debug.Log("string from " + from + " to " + to + " -> " + result);
        return result;
    }

    public bool PathExists(Destination origin, Destination target)
    {
        Stack<Destination> temp = rooms.DepthFirstSearch(target, origin);
        //Stack<Destination> temp = rooms.BreadthFirstSearch(target, origin);

        if (temp.Count >= 1)
        {
            //Debug.Log("from " + origin + " to " + target + " -> " + temp.Peek());
            return temp.Pop() == target;
        }
        else
        {
            return false;
        }
            
    }

    //add a new destination
    public void AddDestination(Destination newDest)
    {
        // validate 0 < height > MAX_FLOORS_HIGH
        if (newDest.Floor <= 0 || newDest.Floor >= MAX_FLOORS_HIGH)
        {
            Debug.Log("No valid slots with MAX_FLOORS_HIGH == " + MAX_FLOORS_HIGH);
            return;
        }

        // find a free slot 
        for (int i = 0; i < MAX_FLOORS_WIDE; i++)
        {
            if (roomsByFloor[newDest.Floor, i] == null)
            {
                // add it to the roomsbyfloor array
                roomsByFloor[newDest.Floor, i] = newDest;

                // add it to the graph
                rooms.AddNode(newDest);

                // find the left closest
                Destination left = leftClosestNode(newDest);
                if (left != null)
                    rooms.AddUndirectedEdge(newDest, left);

                // find the right closest
                Destination right = rightClosestNode(newDest);
                if (right != null)
                    rooms.AddUndirectedEdge(newDest, right);

                // sever the link between left and right if applicable
                rooms.RemoveUndirectedEdge(left, right);
                
                // if this is transportation, find the below closest (temp)
                if (newDest.Transportation)
                {
                    Destination below = belowClosestNode(newDest);
                    rooms.AddUndirectedEdge(newDest, below);
                }
                
                break;
            }
            else if (i + 1 == MAX_FLOORS_WIDE)
            {
                Debug.Log("No empty slots with MAX_FLOORS_WIDE == " + MAX_FLOORS_WIDE);
                return;
            }
                
        }
    }

    // remove a room from
    public bool RemoveDestination(Destination theRoom)
    {
        //remove from floor array
        for (int i = 0; i < MAX_FLOORS_WIDE; i++)
        {
            if (roomsByFloor[theRoom.Floor,i] == theRoom)
            {
                roomsByFloor[theRoom.Floor, i] = null;
                break;
            }
        }

        //remove from the graph
        return rooms.Remove(theRoom);
    }

    // find the closest destination on the same floor to the left
    private Destination leftClosestNode(Destination search)
    {
        // validate input
        if (search == null)
            return null;

        float distance = 0;
        Destination retVal = null;

        // search each node on the floor
        for (int i = 0; i < MAX_FLOORS_WIDE; i++)
        {
            Destination temp = roomsByFloor[search.Floor, i];
            if (temp != null && temp != search)
            {
                float tempDistance = search.transform.position.x - temp.transform.position.x;
                if (tempDistance > 0 && (distance == 0 || tempDistance < distance))
                {
                    // found a closer distance
                    distance = tempDistance;
                    retVal = roomsByFloor[search.Floor, i];
                } 
            }
        }

        return retVal;
    }

    // find the cloeset destination on the same floor to the right
    private Destination rightClosestNode(Destination search)
    {
        // validate input
        if (search == null)
            return null;

        float distance = 0;
        Destination retVal = null;

        // search each node on the floor
        for (int i = 0; i < MAX_FLOORS_WIDE; i++)
        {
            Destination temp = roomsByFloor[search.Floor, i];
            if (temp != null && temp != search)
            {
                float tempDistance = temp.transform.position.x - search.transform.position.x;
                if (tempDistance > 0 && (distance == 0 || tempDistance < distance))
                {
                    // found a closer distance
                    distance = tempDistance;
                    retVal = roomsByFloor[search.Floor, i];
                }
            }
        }

        return retVal;
    }

    // find the closest destination below the current floor
    private Destination belowClosestNode(Destination search)
    {
        // validate input
        if (search == null)
            return null;

        float distance = 0;
        Destination retVal = null;

        // search each node on the floor below
        for (int i = 0; i < MAX_FLOORS_WIDE; i++)
        {
            Destination temp = roomsByFloor[search.Floor - 1, i];
            if (temp != null && temp != search)
            {
                float tempDistance = Mathf.Abs(search.transform.position.x - temp.transform.position.x);
                if (distance == 0 || tempDistance < distance)
                {
                    // found a closer distance
                    distance = tempDistance;
                    retVal = roomsByFloor[search.Floor - 1, i];
                }
            }
        }

        return retVal;
    }

    // find the closest destination below the current floor
    public Destination ClosestDestination(float x, int floor)
    {
        float distance = 0;
        Destination retVal = null;

        // search each node on the floor
        for (int i = 0; i < MAX_FLOORS_WIDE; i++)
        {
            Destination temp = roomsByFloor[floor, i];
            if (temp != null)
            {
                float tempDistance = Mathf.Abs(x - temp.transform.position.x);
                if (distance == 0 || tempDistance < distance)
                {
                    // found a closer distance
                    distance = tempDistance;
                    retVal = roomsByFloor[floor, i];
                }
            }
        }

        return retVal;
    }
}