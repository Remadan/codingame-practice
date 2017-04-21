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

        Dictionary<int, RoomLayout> roomTypes = DefaultRoomTypes();





        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int dungeonWidth = int.Parse(inputs[0]); // number of columns.
        int dungeonHeight = int.Parse(inputs[1]); // number of rows.
        for (int i = 0; i < dungeonHeight; i++)
        {
            string LINE = Console.ReadLine(); // represents a line in the grid and contains W integers. Each integer represents one room of a given type.
        }
        int EX = int.Parse(Console.ReadLine()); // the coordinate along the X axis of the exit (not useful for this first mission, but must be read).

        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int XI = int.Parse(inputs[0]);
            int YI = int.Parse(inputs[1]);
            string POS = inputs[2];

            // To debug: Console.Error.WriteLine("Debug messages...");


            // One line containing the X Y coordinates of the room in which you believe Indy will be on the next turn.
            Console.WriteLine("0 0");
        }
    }

    static Dictionary<int, RoomLayout> DefaultRoomTypes()
    {
        // UP       - 0
        // RIGHT    - 1
        // BOTTOM   - 2
        // LEFT     - 3

        Dictionary<int, RoomLayout> roomTypes = new Dictionary<int, RoomLayout>();

        for (int i = 0; i < 14; i++)
            roomTypes.Add(i, new RoomLayout());

        roomTypes[1].AddEntryPoint(0, 2);
        roomTypes[1].AddEntryPoint(1, 2);
        roomTypes[1].AddEntryPoint(3, 2);

        roomTypes[2].AddEntryPoint(1, 3);
        roomTypes[2].AddEntryPoint(3, 1);

        roomTypes[3].AddEntryPoint(0, 2);

        roomTypes[4].AddEntryPoint(0, 3);
        roomTypes[4].AddEntryPoint(1, 2);

        roomTypes[5].AddEntryPoint(0, 1);
        roomTypes[5].AddEntryPoint(3, 2);

        roomTypes[6].AddEntryPoint(1, 3);
        roomTypes[6].AddEntryPoint(3, 1);

        roomTypes[7].AddEntryPoint(0, 2);
        roomTypes[7].AddEntryPoint(1, 2);

        roomTypes[8].AddEntryPoint(1, 2);
        roomTypes[8].AddEntryPoint(3, 2);

        roomTypes[9].AddEntryPoint(0, 2);
        roomTypes[9].AddEntryPoint(3, 2);

        roomTypes[10].AddEntryPoint(0, 3);

        roomTypes[11].AddEntryPoint(0, 1);

        roomTypes[12].AddEntryPoint(1, 2);

        roomTypes[13].AddEntryPoint(3, 2);

        return roomTypes;
    }
}

class RoomLayout
{
    private Dictionary<int, List<int>> entryPoints;

    public RoomLayout()
    {
        entryPoints = new Dictionary<int, List<int>>();
    }

    public List<int> AvailableExits(int entryPoint)
    {
        List<int> result;
        if (!entryPoints.TryGetValue(entryPoint, out result))
        {
            result = null;
        }
        return result;
    }

    public void AddEntryPoint(int entryPoint, List<int> exits)
    {
        entryPoints.Add(entryPoint, exits);
    }

    public void AddEntryPoint(int entryPoint, int onlyExit)
    {
        List<int> exits = new List<int>();
        exits.Add(onlyExit);
        entryPoints.Add(entryPoint, exits);
    }

}