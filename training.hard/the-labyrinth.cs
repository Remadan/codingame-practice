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
class Player
{
    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int mazeRows = int.Parse(inputs[0]); // number of rows.
        int mazeColumns = int.Parse(inputs[1]); // number of columns.
        int alarmRounds = int.Parse(inputs[2]); // number of rounds between the time the alarm countdown is activated and the time the alarm goes off.

        Graph <MazeTile> mazeGraph = new Graph<MazeTile>();

        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int currentRow = int.Parse(inputs[0]); // row where Kirk is located.
            int currentColumns = int.Parse(inputs[1]); // column where Kirk is located.
            for (int i = 0; i < mazeRows; i++)
            {
                string inputRow = Console.ReadLine(); // C of the characters in '#.TC?' (i.e. one line of the ASCII maze).
                for (int j = 0; j < mazeColumns; j++)
                {
                    mazeGraph.AddNode(new MazeTile(i, j, inputRow[j]));
                }
            }

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            Console.WriteLine("RIGHT"); // Kirk's next move (UP DOWN LEFT or RIGHT).
        }
    }
}

class MazeTile
{
    public Position Position { get; private set; }
    public char Content { get; set; }

    public MazeTile(int row, int collumn, char content)
    {
        Position = new Position(row, collumn);
        Content = content;
    }

    public override bool Equals(Object obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;
        MazeTile otherTile = (MazeTile)obj;
        return Position.Row == other.Position.Row && Position.Collumn == other.Position.Collumn;
    }

    public override int GetHashCode()
    {
        return Position.Row * 17 + Position.Collumn;
    }
}

struct Position
{
    public int Row { get; set; }
    public int Collumn { get; set; }

    public Position(int row, int collumn)
    {
        Row = collumn;
        Collumn = row;
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
