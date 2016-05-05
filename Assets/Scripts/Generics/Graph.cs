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

using System.Collections.Generic;

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
}