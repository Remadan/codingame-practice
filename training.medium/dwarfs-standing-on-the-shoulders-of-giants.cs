using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    static void Main(string[] args)
    {
        Dictionary<GraphNode<int>, int> influenceCache = new Dictionary<GraphNode<int>, int>();
        // populating network
        Graph<int> peopleNetwork = new Graph<int>();
        int n = int.Parse(Console.ReadLine()); // the number of relationships of influence
        for (int i = 0; i < n; i++)
        {
            string[] inputs = Console.ReadLine().Split(' ');
            int x = int.Parse(inputs[0]); // a relationship of influence between two people (x influences y)
            int y = int.Parse(inputs[1]);

            GraphNode<int> nodeX = peopleNetwork.AddNode(x);
            GraphNode<int> nodeY = peopleNetwork.AddNode(y);

            peopleNetwork.AddDirectedEdge(nodeX, nodeY);
        }

        int maxInfluence = int.MinValue;
        foreach (GraphNode<int> currentNode in peopleNetwork.Nodes)
        {
            int nodeInfluence = CalculateInfluence(currentNode, influenceCache);
            if (maxInfluence < nodeInfluence)
                maxInfluence = nodeInfluence;
        }
        // To debug: Console.Error.WriteLine("Debug messages...");
        Console.WriteLine(maxInfluence);
    }

    static int CalculateInfluence(GraphNode<int> currentNode, Dictionary<GraphNode<int>, int> influenceCache)
    {
        int nodeInfluence;
        if (!influenceCache.TryGetValue(currentNode, out nodeInfluence))
        {
            nodeInfluence = currentNode.NeighborsOut.Count;
            foreach (GraphNode<int> childNode in currentNode.NeighborsOut)
            {
                nodeInfluence += CalculateInfluence(childNode, influenceCache);
            }
            influenceCache.Add(currentNode, nodeInfluence);
        }
        return nodeInfluence;
    }

}

public class Graph<T>
{
    public GraphNodeList<T> Nodes { get; private set; }

    public Graph() : this(null) { }
    public Graph(GraphNodeList<T> nodeSet)
    {
        if (nodeSet == null)
            this.Nodes = new GraphNodeList<T>();
        else
            this.Nodes = nodeSet;
    }

    public GraphNode<T> AddNode(T value)
    {
        GraphNode<T> newNode = Nodes.FindByValue(value);
        if (newNode == null)
            newNode = new GraphNode<T>(value);

        Nodes.Add(newNode);

        return newNode;
    }

    public GraphNode<T> AddNode(GraphNode<T> newNode)
    {
        // adds a node to the graph
        Nodes.Add(newNode);
        return newNode;
    }

    public void AddDirectedEdge(T from, T to)
    {
        GraphNode<T> nodeFrom = Nodes.FindByValue(from);
        GraphNode<T> nodeTo = Nodes.FindByValue(to);
        if (nodeFrom != null & nodeTo != null)
            AddDirectedEdge(nodeFrom, nodeTo);
    }

    public void AddDirectedEdge(GraphNode<T> nodeFrom, GraphNode<T> nodeTo)
    {
        nodeFrom.NeighborsOut.Add(nodeTo);
        nodeTo.NeighborsIn.Add(nodeFrom);
    }

    public void AddUndirectedEdge(T from, T to)
    {
        GraphNode<T> nodeFrom = Nodes.FindByValue(from);
        GraphNode<T> nodeTo = Nodes.FindByValue(to);
        if (nodeFrom != null & nodeTo != null)
            AddUndirectedEdge(nodeFrom, nodeTo);
    }

    public void AddUndirectedEdge(GraphNode<T> nodeFrom, GraphNode<T> nodeTo)
    {
        nodeFrom.NeighborsOut.Add(nodeTo);
        nodeFrom.NeighborsIn.Add(nodeTo);
        nodeTo.NeighborsOut.Add(nodeFrom);
        nodeTo.NeighborsIn.Add(nodeFrom);
    }

    public void RemoveDirectedEdge(T from, T to)
    {
        GraphNode<T> nodeFrom = Nodes.FindByValue(from);
        GraphNode<T> nodeTo = Nodes.FindByValue(to);
        if (nodeFrom != null & nodeTo != null)
            RemoveDirectedEdge(nodeFrom, nodeTo);
    }

    public void RemoveDirectedEdge(GraphNode<T> nodeFrom, GraphNode<T> nodeTo)
    {
        nodeFrom.NeighborsOut.Remove(nodeTo);
        nodeTo.NeighborsIn.Remove(nodeFrom);
    }

    public void RemoveUndirectedEdge(T from, T to)
    {
        GraphNode<T> nodeFrom = Nodes.FindByValue(from);
        GraphNode<T> nodeTo = Nodes.FindByValue(to);
        if (nodeFrom != null & nodeTo != null)
            RemoveUndirectedEdge(nodeFrom, nodeTo);
    }

    public void RemoveUndirectedEdge(GraphNode<T> nodeFrom, GraphNode<T> nodeTo)
    {
        nodeFrom.NeighborsOut.Remove(nodeTo);
        nodeFrom.NeighborsIn.Remove(nodeTo);
        nodeTo.NeighborsOut.Remove(nodeFrom);
        nodeTo.NeighborsIn.Remove(nodeFrom);
    }

    public bool Contains(T value)
    {
        return Nodes.FindByValue(value) != null;
    }

    public bool Remove(T value)
    {
        // first remove the node from the nodeset
        GraphNode<T> nodeToRemove = Nodes.FindByValue(value);
        if (nodeToRemove == null)
            // node wasn't found
            return false;

        return Remove(nodeToRemove);
    }

    public bool Remove(GraphNode<T> nodeToRemove)
    {
        Nodes.Remove(nodeToRemove);

        foreach (GraphNode<T> gnode in nodeToRemove.NeighborsOut)
            if (gnode.NeighborsIn.Contains(nodeToRemove))
                gnode.NeighborsIn.Remove(nodeToRemove);

        foreach (GraphNode<T> gnode in nodeToRemove.NeighborsIn)
            if (gnode.NeighborsOut.Contains(nodeToRemove))
                gnode.NeighborsOut.Remove(nodeToRemove);

        nodeToRemove.NeighborsIn.Clear();
        nodeToRemove.NeighborsOut.Clear();

        return true;
    }

    public int Count
    {
        get { return Nodes.Count; }
    }

}

public class GraphNode<T>
{
    public T Value { get; }
    public HashSet<GraphNode<T>> NeighborsOut { get; }
    public HashSet<GraphNode<T>> NeighborsIn { get; }

    public GraphNode(T data)
    {
        Value = data;
        NeighborsOut = new HashSet<GraphNode<T>>();
        NeighborsIn = new HashSet<GraphNode<T>>();
    }
}

public class GraphNodeList<T> : HashSet<GraphNode<T>>
{
    private Dictionary<T, GraphNode<T>> valueNodeMap;

    public GraphNodeList()
    {
        valueNodeMap = new Dictionary<T, GraphNode<T>>();
    }

    public new bool Add(GraphNode<T> item)
    {
        bool result = base.Add(item);

        if (result)
            valueNodeMap.Add(item.Value, item);

        return result;
    }

    public new bool Remove(GraphNode<T> item)
    {
        bool result = base.Remove(item);

        if (result)
            valueNodeMap.Remove(item.Value);

        return result;
    }

    public GraphNode<T> FindByValue(T value)
    {
        GraphNode<T> node;
        if (valueNodeMap.TryGetValue(value, out node))
            return node;
        else
            return null;
    }
}
