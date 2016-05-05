// -------------------------------- Graph.cs -----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 4, 2016
// Modified - May 4, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a generic graph class with breadth first search
// and depth first serach.
// ----------------------------------------------------------------------------
// Notes - Implementation based on discussion by Scott Mitchell in January 2005
// at https://msdn.microsoft.com/en-us/library/ms379574(v=vs.80).aspx.
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine; //only for debug

public class GraphNode<T> : Node<T>
{
    private List<int> costs;

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

    public List<int> Costs
    {
        get
        {
            if (costs == null)
                costs = new List<int>();

            return costs;
        }
    }
}

public class Graph<T>
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

    public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
    {
        from.Neighbors.Add(to);
        from.Costs.Add(cost);
    }

    public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
    {
        from.Neighbors.Add(to);
        from.Costs.Add(cost);

        to.Neighbors.Add(from);
        to.Costs.Add(cost);
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
                gnode.Costs.RemoveAt(index);
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

    public Stack<T> DepthFirstSearch(T search)
    {
        Queue<GraphNode<T>> q = new Queue<GraphNode<T>>();
        q.Enqueue(this.root);
        while (q.Count > 0)
        {
            TreeNode n = q.Dequeue();
            Console.WriteLine(n.data);
            if (n.left != null)
                q.Enqueue(n.left);
            if (n.right != null)
                q.Enqueue(n.right);
        }
    }

    //public NodeList<T> DepthFirstSearch(T search)
    //{
    //    int size = Count;
    //    NodeList<T> retVal = new NodeList<T>();

    //    // make all nodes not visisted
    //    for (int i = 1; i <= size; i++)
    //    {
    //        nodeSet[i].Visited = false;
    //    }

    //    // initial output line
    //    Debug.Log("Depth-first ordering until " + search + ":");

    //    // for v = 1 to n
    //    for (int v = 1; v <= size; v++)
    //    {
    //        if (nodeSet[v].Visited == false)
    //            dfsHelper(v, retVal, search);
    //    }

    //    return retVal;
    //}

    ////update the retVal NodeList with any relevant nodes
    //public void dfsHelper(int v, NodeList<T> retVal, T search)
    //{
    //    // mark v vistited
    //    nodeSet[v].Visited = true;

    //    // do output
    //    Debug.Log(nodeSet[v]);

    //    // for each adjacent for v
        
    //    EdgeNode* current = data[v].edgeHead;
    //    while (current != NULL && current->adjGraphNode != 0)
    //    {
    //        int w = current->adjGraphNode;

    //        if (data[w].visited == false)
    //            dfsHelper(w);

    //        // advance
    //        current = current->nextEdge;
    //    }
    //}
}