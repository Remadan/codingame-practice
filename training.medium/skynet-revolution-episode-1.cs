using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;


/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{
    static void Main(string[] args)
    {
		Graph<int> network = new Graph<int>(); // graph to represent our network
		List<GraphNode<int>> exitNodes = new List<GraphNode<int>>(); // list of nodes we need to protect
	
		string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int N = int.Parse(inputs[0]); // the total number of nodes in the level, including the gateways
		int L = int.Parse(inputs[1]); // the number of links
		int E = int.Parse(inputs[2]); // the number of exit gateways
		
  		// populate graph with nodes
        for (int i = 0; i < N; i++)
        {
			network.AddNode(i);
        }
		
		// and add links
        for (int i = 0; i < L; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int N1 = int.Parse(inputs[0]); // N1 and N2 defines a link between these nodes
            int N2 = int.Parse(inputs[1]);
			
			network.AddUndirectedEdge(N1, N2);
        }
		
		// store exit nodes for later
		for (int i = 0; i < E; i++)
        {
            int EI = int.Parse(Console.ReadLine()); // the index of a gateway node
			exitNodes.Add(network.Nodes.FindByValue(EI));
        }

        // game loop
        while (true)
        {
            int SI = int.Parse(Console.ReadLine()); // The index of the node on which the Skynet agent is positioned this turn
			List<GraphNode<int>> priorityPath = null;
			List<GraphNode<int>> curPath;
			
			foreach (GraphNode<int> gatewayNode in exitNodes)
			{
				
				// roadblock strategy
				//curPath = findShortestPath(network.Nodes.FindByValue(SI), gatewayNode);

				// ambush strategy
				curPath = findShortestPath(gatewayNode, network.Nodes.FindByValue(SI));
				
				if (priorityPath == null || (curPath != null && priorityPath.Count > curPath.Count))
				{
					priorityPath = curPath;
				}
				
			}
			
			if (priorityPath != null)
			{
				Console.WriteLine(priorityPath[0].Value.ToString() + " " + priorityPath[1].Value.ToString());
				network.RemoveUndirectedEdge(priorityPath[0], priorityPath[1]);
			}
			else
			{
				Console.WriteLine("0 1");
				
			}
            
        }
    }

	static List<GraphNode<int>> findShortestPath(GraphNode<int> startNode, GraphNode<int> exitNode)
	{
		
		Dictionary<GraphNode<int>, GraphNode<int>> path = new Dictionary<GraphNode<int>, GraphNode<int>>();	// previous node in optimal path
		Dictionary<GraphNode<int>, int> dist = new Dictionary<GraphNode<int>, int>();							// distance to start 
		
		Queue<GraphNode<int>> nodeQueue = new Queue<GraphNode<int>>();			// current nodes to procces
		List<GraphNode<int>> knownNodes = new List<GraphNode<int>>();
		List<GraphNode<int>> shortestPath = new List<GraphNode<int>>();
		
		// lets process start node first
		dist.Add(startNode, 0);
		knownNodes.Add(startNode);
		foreach (GraphNode<int> neighborNode in startNode.Neighbors)
		{
			nodeQueue.Enqueue(neighborNode);
			dist.Add(neighborNode, 1);
			path.Add(neighborNode, startNode);
			knownNodes.Add(neighborNode);
		}	
		
		Console.Error.WriteLine("looking path from  " + startNode.Value.ToString() + " to " + exitNode.Value.ToString());
			
		while (nodeQueue.Count > 0)
		{
			GraphNode<int> curentNode = nodeQueue.Dequeue();
			
			foreach (GraphNode<int> neighborNode in curentNode.Neighbors)
			{
				// what about distance to start node?
				int neighborDistance = dist[curentNode] + 1;
				
				int neighborBestDistance = int.MaxValue;
				if (!dist.TryGetValue(neighborNode, out neighborBestDistance) || neighborDistance < neighborBestDistance)
				{
					// it's the shortest route
					dist.Add(neighborNode, neighborDistance);
					path.Add(neighborNode, curentNode);
				}
				
				// did we been here already?
				if (!knownNodes.Contains(neighborNode))
				{
					nodeQueue.Enqueue(neighborNode);
					knownNodes.Add(neighborNode);
				}
			}
		}
		
		
		if (path.ContainsKey(exitNode))
		{
			Console.Error.WriteLine("Found path.");
			GraphNode<int> curentNode = exitNode;
			shortestPath.Add(exitNode);
			while (curentNode != startNode)
			{
				GraphNode<int> pathNode = path[curentNode];
				shortestPath.Add(pathNode);
				curentNode = pathNode;
			}
			shortestPath.Add(startNode);
		}
		else
		{
			shortestPath = null;
		}
		
		return shortestPath;
	}
	
}



public class Graph<T>
{
    private GraphNodeList<T> nodeSet;

    public Graph() : this(null) {}
    public Graph(GraphNodeList<T> nodeSet)
    {
        if (nodeSet == null)
            this.nodeSet = new GraphNodeList<T>();
        else
            this.nodeSet = nodeSet;
    }

    public void AddNode(GraphNode<T> node)
    {
        // adds a node to the graph
        nodeSet.Add(node);
    }

    public void AddNode(T value)
    {
        // adds a node to the graph
		if (!this.Contains(value))
			nodeSet.Add(new GraphNode<T>(value));
    }

    public void AddDirectedEdge(T from, T to)
    {
        GraphNode<T> nodeFrom = nodeSet.FindByValue(from);
        GraphNode<T> nodeTo = nodeSet.FindByValue(to);
		if (nodeFrom != null & nodeTo != null)
			nodeFrom.Neighbors.Add(nodeTo);
    }

    public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to)
    {
        from.Neighbors.Add(to);
    }

    public void AddUndirectedEdge(T from, T to)
    {
        GraphNode<T> nodeFrom = nodeSet.FindByValue(from);
        GraphNode<T> nodeTo = nodeSet.FindByValue(to);
 		if (nodeFrom != null & nodeTo != null)
		{
			nodeFrom.Neighbors.Add(nodeTo);
			nodeTo.Neighbors.Add(nodeFrom);
		}
    }

    public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to)
    {
        from.Neighbors.Add(to);
        to.Neighbors.Add(from);
    }

    public void RemoveUndirectedEdge(GraphNode<T> from, GraphNode<T> to)
    {
        from.Neighbors.Remove(to);
        to.Neighbors.Remove(from);
    }
    public void RemoveUndirectedEdge(T from, T to)
    {
        GraphNode<T> nodeFrom = nodeSet.FindByValue(from);
        GraphNode<T> nodeTo = nodeSet.FindByValue(to);
 		if (nodeFrom != null & nodeTo != null)
		{
			nodeFrom.Neighbors.Remove(nodeTo);
			nodeTo.Neighbors.Remove(nodeFrom);
		}
    }

    public bool Contains(T value)
    {
        return nodeSet.FindByValue(value) != null;
    }

    public bool Remove(T value)
    {
        // first remove the node from the nodeset
        GraphNode<T> nodeToRemove = (GraphNode<T>) nodeSet.FindByValue(value);
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
            }
        }

        return true;
    }

    public GraphNodeList<T> Nodes
    {
        get
        {
            return nodeSet;
        }
    }

    public int Count
    {
        get { return nodeSet.Count; }
    }
    
}

public class GraphNode<T>
{

	private T data;
	private GraphNodeList<T> neighbors = null;

	public GraphNode() {}
	public GraphNode(T data) : this(data, null) {}
	public GraphNode(T data, GraphNodeList<T> neighbors)
	{
		this.data = data;
		this.neighbors = neighbors;
	}

	public T Value
	{
		get
		{
			return data;
		}
		set
		{
			data = value;
		}
	}

    public GraphNodeList<T> Neighbors
    {
        get
        {
            if (this.neighbors == null)
                this.neighbors = new GraphNodeList<T>();

            return this.neighbors;
        }            
    }
	
}

public class GraphNodeList<T> : Collection<GraphNode<T>>
{
    public GraphNodeList() : base() {}

    public GraphNodeList(int initialSize)
    {
        // Add the specified number of items
        for (int i = 0; i < initialSize; i++)
            base.Items.Add(default(GraphNode<T>));
    }

    public GraphNode<T> FindByValue(T value)
    {
        // search the list for the value
        foreach (GraphNode<T> node in Items)
            if (node.Value.Equals(value))
                return node;

        // if we reached here, we didn't find a matching node
        return null;
    }
}

