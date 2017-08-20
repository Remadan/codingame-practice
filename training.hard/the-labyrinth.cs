using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

class Player
{
    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int mazeRows = int.Parse(inputs[0]); // number of rows.
        int mazeColumns = int.Parse(inputs[1]); // number of columns.
        int alarmRounds = int.Parse(inputs[2]); // number of rounds between the time the alarm countdown is activated and the time the alarm goes off.

        bool alarmTriggered = false;
        KirkMaze KirkMaze = new KirkMaze(mazeRows, mazeColumns);

        // game loop
        while (true)
        {
            string newMove;

            // where are we ?
            inputs = Console.ReadLine().Split(' ');
            Position currentPosition = new Position(int.Parse(inputs[0]), int.Parse(inputs[1]));
            
            // are we screwed?
            if (!alarmTriggered && currentPosition == KirkMaze.ControlRoomPosition)
            {
                alarmTriggered = true;
            }

            // updating maze tiles
            for (int i = 0; i < mazeRows; i++)
            {
                string inputRow = Console.ReadLine(); // C of the characters in '#.TC?' (i.e. one line of the ASCII maze).
                for (int j = 0; j < mazeColumns; j++)
                {
                    KirkMaze.UpdateTile(i, j, inputRow[j]);
                }
            }

            // deciding next move
            if (alarmTriggered)
            {
                newMove = KirkMaze.DirectionToStart(currentPosition);
            }
            else if (KirkMaze.EscapePossible(alarmRounds))
            {
                newMove = KirkMaze.DirectionToControlRoom(currentPosition);
            }
            else
            {
                newMove = KirkMaze.DirectionToScout(currentPosition);
            }

            // To debug: Console.Error.WriteLine("Debug messages...");
            Console.WriteLine(newMove); // Kirk's next move (UP DOWN LEFT or RIGHT).
        }
    }
}

class KirkMaze
{
    private int rows;
    private int columns;
    private Graph<Position> mazeGraph;
    private Dictionary<Position, char> tilesContent;

    public Position StartingPosition { get; private set; }
    public Position ControlRoomPosition { get; private set; }

    public KirkMaze(int newRows, int newColumns)
    {
        rows = newRows;
        columns = newColumns;

        mazeGraph = new Graph<Position>();
        tilesContent = new Dictionary<Position, char>();

        StartingPosition = new Position(rows + 1, columns + 1);
        ControlRoomPosition = new Position(rows + 1, columns + 1);
    }

    public void UpdateTile(int row, int column, char content)
    {
        char tileContet;
        Position newTilePosition = new Position(row, column);

        // do we really need an update?
        if (tilesContent.TryGetValue(newTilePosition, out tileContet))
        {
            if (tileContet == content)
            {
                return;
            }
        }
        else
        {
            mazeGraph.AddNode(newTilePosition);
        }

        tilesContent[newTilePosition] = content;

        bool passableTile = true;
        switch (content)
        {
            case 'T':
                StartingPosition = newTilePosition;
                break;
            case 'C':
                ControlRoomPosition = newTilePosition;
                break;
            case '#':
                passableTile = false;
                break;
        }

        // update 4 posible adjusted positions
        List<Position> tilesToUpdate = new List<Position>();
        tilesToUpdate.Add(new Position(row, column - 1));
        tilesToUpdate.Add(new Position(row - 1, column));
        tilesToUpdate.Add(new Position(row, column + 1));
        tilesToUpdate.Add(new Position(row + 1, column));

        foreach (Position updateTilePosition in tilesToUpdate)
        {
            if (updateTilePosition.Collumn > this.columns || updateTilePosition.Collumn < 0
                || updateTilePosition.Row > this.rows || updateTilePosition.Row < 0)
            {
                continue;
            }

            if (!tilesContent.TryGetValue(updateTilePosition, out tileContet))
            {
                continue;
            }

            if (!passableTile)
            {
                mazeGraph.RemoveUndirectedEdge(newTilePosition, updateTilePosition);
            }
            else if (tileContet != '#')
            {
                mazeGraph.AddUndirectedEdge(newTilePosition, updateTilePosition);
            }
        }
    }

    public bool EscapePossible(int alarmRounds)
    {
        // A* from ControlRoomPosition to ControlRoomPosition
        return false;
    }

    public string DirectionToStart(Position currentPosition)
    {
        // A* from currentPosition to StartingPosition
        return "RIGHT";
    }

    public string DirectionToControlRoom(Position currentPosition)
    {
        // A* from currentPosition to ControlRoomPosition
        // use result from EscapePossible() ??

        return "RIGHT";
    }

    public string DirectionToScout(Position currentPosition)
    {
        // BFS till we get an unknown tile
        GraphNode<Position> startNode = mazeGraph.Nodes.FindByValue(currentPosition);

        Console.Error.WriteLine("BFS from: " + currentPosition);
        Console.Error.WriteLine("ControlRoomPosition: " + ControlRoomPosition);

        Dictionary<Position, Position> path = new Dictionary<Position, Position>();
        Queue<GraphNode<Position>> nodeQueue = new Queue<GraphNode<Position>>();

        GraphNode<Position> currentNode = null;
        path.Add(currentPosition, currentPosition);
        nodeQueue.Enqueue(startNode);
        while (nodeQueue.Count > 0)
        {
            currentNode = nodeQueue.Dequeue();

            if (tilesContent[currentNode.Value] == '?')
            {
                break;
            }

            foreach (GraphNode<Position> neighborNode in currentNode.NeighborsOut)
            {
                if (tilesContent[neighborNode.Value] == 'C')
                {
                    continue;
                }

                if (!path.ContainsKey(neighborNode.Value))
                {
                    path.Add(neighborNode.Value, currentNode.Value);
                    nodeQueue.Enqueue(neighborNode);
                }
            }
            currentNode = null;
        }

        if (currentNode == null)
            return "PANIC";
        else
            return DirectionFromPath(path, currentPosition, currentNode.Value);
    }

    static string DirectionFromPath(Dictionary<Position, Position> path, Position startPosition, Position targetPosition)
    {
        Position currentPosition = targetPosition;
        Position nextPosition = path[currentPosition];
        while (nextPosition != startPosition)
        {
            currentPosition = nextPosition;
            nextPosition = path[currentPosition];
        }

        int collumnDifference = startPosition.Collumn - currentPosition.Collumn;
        int rowDifference = startPosition.Row - currentPosition.Row;
        if (collumnDifference > 0)
        {
            return "LEFT";
        }
        else if (collumnDifference < 0)
        {
            return "RIGHT";
        }
        else if (rowDifference > 0)
        {
            return "UP";
        }
        else if (rowDifference < 0)
        {
            return "DOWN";
        }
        else
        {
            return "PANIC";
        }
    }
}

struct Position : IEquatable<Position>
{
    public int Row { get; set; }
    public int Collumn { get; set; }

    public Position(int row, int collumn)
    {
        Row = row;
        Collumn = collumn;
    }

    public bool Equals(Position other)
    {
        return Row == other.Row && Collumn == other.Collumn;
    }

    public override bool Equals(object other)
    {
        // https://stackoverflow.com/questions/1502451/what-needs-to-be-overridden-in-a-struct-to-ensure-equality-operates-properly
        return other is Position && this == (Position)other;
    }

    public static bool operator ==(Position x, Position y)
    {
        return x.Row == y.Row && x.Collumn == y.Collumn;
    }

    public static bool operator !=(Position x, Position y)
    {
        return !(x == y);
    }

    public override int GetHashCode()
    {
        // https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + Row.GetHashCode();
            hash = hash * 23 + Collumn.GetHashCode();
            return hash;
        }
    }

    public override string ToString()
    {
        return Row.ToString() + "; " + Collumn.ToString();
    }
}

class Graph<T>
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
        if (nodeFrom != null && nodeTo != null)
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
        if (nodeFrom != null && nodeTo != null)
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
        if (nodeFrom != null && nodeTo != null)
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
        if (nodeFrom != null && nodeTo != null)
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

class GraphNode<T>
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

    public override bool Equals(object other)
    {
        return other != null && Value.Equals(((GraphNode<T>)other).Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}

class GraphNodeList<T> : HashSet<GraphNode<T>>
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
