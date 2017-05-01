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
        // populating network
        Graph<int> adNetwork = new Graph<int>();
        Dictionary<int, GraphNode<int>> nodesMap = new Dictionary<int, GraphNode<int>>();
        int n = int.Parse(Console.ReadLine()); // the number of adjacency relations
        for (int i = 0; i < n; i++)
        {
            string[] inputs = Console.ReadLine().Split(' ');
            int xi = int.Parse(inputs[0]); // the ID of a person which is adjacent to yi
            int yi = int.Parse(inputs[1]); // the ID of a person which is adjacent to xi

            adNetwork.AddNode(xi);
            adNetwork.AddNode(yi);

            adNetwork.AddUndirectedEdge(xi, yi);
        }

        int stepsTaken = 0;
        while(adNetwork.Nodes.Count != 1)
        {
            List<GraphNode<int>> nodesToRemove = new List<GraphNode<int>>();

            if (adNetwork.Nodes.Count == 2)
            {
                nodesToRemove.Add(adNetwork.Nodes.First());
            }
            else
            {
                foreach (GraphNode<int> currentNode in adNetwork.Nodes)
                {
                    if (currentNode.Neighbors.Count == 1) // leaf node
                    {
                        nodesToRemove.Add(currentNode);
                    }
                }
            }

            foreach (GraphNode<int> currentNode in nodesToRemove)
            {
                adNetwork.Remove(currentNode);
            }

            stepsTaken++;
        }

        Console.WriteLine(stepsTaken);
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

    public void AddNode(GraphNode<T> node)
    {
        // adds a node to the graph
        Nodes.Add(node);
    }

    public void AddNode(T value)
    {
        // adds a node to the graph
        if (!this.Contains(value))
            Nodes.Add(new GraphNode<T>(value));
    }

    public void AddUndirectedEdge(T from, T to)
    {
        GraphNode<T> nodeFrom = Nodes.FindByValue(from);
        GraphNode<T> nodeTo = Nodes.FindByValue(to);
        if (nodeFrom != null & nodeTo != null)
        {
            nodeFrom.Neighbors.Add(nodeTo);
            nodeTo.Neighbors.Add(nodeFrom);
        }
    }

    public void AddUndirectedEdge(GraphNode<T> nodeFrom, GraphNode<T> nodeTo)
    {
        nodeFrom.Neighbors.Add(nodeTo);
        nodeTo.Neighbors.Add(nodeFrom);
    }

    public void RemoveUndirectedEdge(GraphNode<T> nodeFrom, GraphNode<T> nodeTo)
    {
        nodeFrom.Neighbors.Remove(nodeTo);
        nodeTo.Neighbors.Remove(nodeFrom);
    }

    public void RemoveUndirectedEdge(T from, T to)
    {
        GraphNode<T> nodeFrom = Nodes.FindByValue(from);
        GraphNode<T> nodeTo = Nodes.FindByValue(to);
        if (nodeFrom != null & nodeTo != null)
        {
            nodeFrom.Neighbors.Remove(nodeTo);
            nodeTo.Neighbors.Remove(nodeFrom);
        }
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

        foreach (GraphNode<T> gnode in nodeToRemove.Neighbors)
        {
            if (gnode.Neighbors.Contains(nodeToRemove))
            {
                gnode.Neighbors.Remove(nodeToRemove);
            }
        }

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
    public HashSet<GraphNode<T>> Neighbors { get; }

    public GraphNode(T data) : this(data, null) { }
    public GraphNode(T data, HashSet<GraphNode<T>> neighbors)
    {
        Value = data;
        if (neighbors == null)
            neighbors = new HashSet<GraphNode<T>>();
        Neighbors = neighbors;
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
