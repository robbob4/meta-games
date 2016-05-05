// -------------------------------- Node.cs -----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 4, 2016
// Modified - May 5, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a generic Node class and NodeList class for use
// in a graph.
// ----------------------------------------------------------------------------
// Notes - Implementation based on discussion by Scott Mitchell in January 2005
// at https://msdn.microsoft.com/en-us/library/ms379572(v=vs.80).aspx.
// ----------------------------------------------------------------------------

using System.Collections.ObjectModel;

public class Node<T>
{
    // Private member-variables
    private T data;
    private NodeList<T> neighbors = null;
    private bool visited = false; //TODO: remove visited from the node class

    public Node() { }
    public Node(T data) : this(data, null) { }
    public Node(T data, NodeList<T> neighbors)
    {
        this.data = data;
        this.neighbors = neighbors;
    }

    public T Value
    {
        get { return data; }
        set { data = value; }
    }

    public bool Visited
    {
        get { return visited; }
        set { visited = value; }
    }

    protected NodeList<T> Neighbors
    {
        get { return neighbors; }
        set
        { neighbors = value; }
    }
}

public class NodeList<T> : Collection<Node<T>>
{
    public NodeList() : base() { }

    public NodeList(int initialSize)
    {
        // Add the specified number of items
        for (int i = 0; i < initialSize; i++)
            base.Items.Add(default(Node<T>));
    }

    public Node<T> FindByValue(T value)
    {
        // search the list for the value
        foreach (Node<T> node in Items)
            if (node.Value.Equals(value))
                return node;

        // if we reached here, we didn't find a matching node
        return null;
    }
}