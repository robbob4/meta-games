﻿// -------------------------------- Graph.cs -----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 4, 2016
// Modified - May 5, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a generic graph class with breadth first search
// and depth first serach.
// ----------------------------------------------------------------------------
// Notes - Implementation based on discussion by Scott Mitchell in January 2005
// at https://msdn.microsoft.com/en-us/library/ms379574(v=vs.80).aspx.
// ----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; //only for debug

public class GraphNode<T> : Node<T>
{
    //private List<int> costs;

    public GraphNode() : base() { }
    public GraphNode(T value) : base(value) { }
    public GraphNode(T value, NodeList<T> neighbors) : base(value, neighbors) { }

    new public NodeList<T> Neighbors
    {
        get
        {
            if (base.Neighbors == null)
                base.Neighbors = new NodeList<T>();

            return base.Neighbors;
        }
    }

    //public List<int> Costs
    //{
    //    get
    //    {
    //        if (costs == null)
    //            costs = new List<int>();

    //        return costs;
    //    }
    //}
}

public class Graph<T> : IEnumerable<T>
{
    private NodeList<T> nodeSet;

    public Graph() : this(null) { }
    public Graph(NodeList<T> nodeSet)
    {
        if (nodeSet == null)
            this.nodeSet = new NodeList<T>();
        else
            this.nodeSet = nodeSet;
    }

    // adds a node to the graph
    public void AddNode(GraphNode<T> node)
    {
        nodeSet.Add(node);
    }

    // creates and adds a node to the graph 
    public void AddNode(T value)
    {
        nodeSet.Add(new GraphNode<T>(value));
    }

    //public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
    //{
    //    from.Neighbors.Add(to);
    //    from.Costs.Add(cost);
    //}

    public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to)
    {
        from.Neighbors.Add(to);
    }

    // overload with GraphNode wrapper
    public void AddDirectedEdge(T from, T to)
    {
        GraphNode<T> gnFrom = (GraphNode<T>)nodeSet.FindByValue(from);
        GraphNode<T> gnTo = (GraphNode<T>)nodeSet.FindByValue(to);

        gnFrom.Neighbors.Add(gnTo);
    }

    //public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
    //{
    //    from.Neighbors.Add(to);
    //    from.Costs.Add(cost);

    //    to.Neighbors.Add(from);
    //    to.Costs.Add(cost);
    //}

    public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to)
    {
        from.Neighbors.Add(to);
        to.Neighbors.Add(from);
    }

    // overload with GraphNode wrapper
    public void AddUndirectedEdge(T from, T to)
    {
        GraphNode<T> gnFrom = (GraphNode<T>)nodeSet.FindByValue(from);
        GraphNode<T> gnTo = (GraphNode<T>)nodeSet.FindByValue(to);

        gnFrom.Neighbors.Add(gnTo);
        gnTo.Neighbors.Add(gnFrom);
    }

    public bool Contains(T value)
    {
        return nodeSet.FindByValue(value) != null;
    }

    public bool Remove(T value)
    {
        // first remove the node from the nodeset
        GraphNode<T> nodeToRemove = (GraphNode<T>)nodeSet.FindByValue(value);
        if (nodeToRemove == null)
            // node wasn't found
            return false;

        // otherwise, the node was found
        nodeSet.Remove(nodeToRemove);

        // enumerate through each node in the nodeSet, removing edges to this node
        foreach (GraphNode<T> gnode in nodeSet)
        {
            int index = gnode.Neighbors.IndexOf(nodeToRemove);
            if (index != -1)
            {
                // remove the reference to the node and associated cost
                gnode.Neighbors.RemoveAt(index);
                //gnode.Costs.RemoveAt(index);
            }
        }

        return true;
    }

    public NodeList<T> Nodes
    {
        get { return nodeSet; }
    }

    public int Count
    {
        get { return nodeSet.Count; }
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    //TODO: BFS
    //TODO: Create a seperate entity for visited to allow simultaneous searches?
    //TODO: Ensure nodes are comparable?
    //TODO: hash the nodes so that a node can be searched from for optimization
    //TODO: Search from any node

    public Stack<T> DepthFirstSearch(T search)
    {
        int size = Count;
        Stack<T> retVal = new Stack<T>();

        // make all nodes not visisted
        for (int i = 0; i < size; i++)
        {
            nodeSet[i].Visited = false;
        }

        // initial output line
        //Debug.Log("Depth-first ordering until " + search + ":");

        // for v = 1 to n
        for (int v = 0; v < size; v++)
        {
            if (nodeSet[v].Visited == false)
                dfsHelper(v, nodeSet, retVal, search);

            //skip if another search found the node
            if (retVal.Count >= 1 && retVal.Peek().Equals(search))
            {
                //Debug.Log("Skipping outer loop!");
                break;
            }
        }

        return retVal;
    }

    //overload for a full search
    public Stack<T> DepthFirstSearch()
    {
        return DepthFirstSearch(default(T));
    }

    //update the retVal NodeList with any relevant nodes
    public void dfsHelper(int v, NodeList<T> nodes, Stack<T> retVal, T search)
    {
        // push the data on the stack
        retVal.Push(nodes[v].Value);
        //Debug.Log(nodes[v].Value);

        // found the node we're searching for?
        if (nodes[v].Value.Equals(search))
        {
            //Debug.Log("Found it!");
            return;
        }

        // mark v vistited
        nodes[v].Visited = true;

        // for each w adjacent for v
        int size = ((GraphNode<T>)nodes[v]).Neighbors.Count;
        for (int w = 0; w < size ; w++)
        {
            if (((GraphNode<T>)nodes[v]).Neighbors[w].Visited == false)
                dfsHelper(w, ((GraphNode<T>)nodes[v]).Neighbors, retVal, search);

            //skip if another search found the node
            if (retVal.Count >= 1 && retVal.Peek().Equals(search))
            {
                //Debug.Log("Skipping inner loop!");
                return;
            }
        }
    }
}