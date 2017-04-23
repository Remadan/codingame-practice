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
        int[,] dungeonMap = new int[dungeonWidth, dungeonHeight];
        for (int i = 0; i < dungeonHeight; i++)
        {
            string[] LINE = Console.ReadLine().Split(' '); // represents a line in the grid and contains W integers. Each integer represents one room of a given type.
            for (int j = 0; j < dungeonWidth; j++)
            {
                dungeonMap[j, i] = int.Parse(LINE[j]);
            }
        }
        int EX = int.Parse(Console.ReadLine()); // the coordinate along the X axis of the exit (not useful for this first mission, but must be read).

        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int XI = int.Parse(inputs[0]);
            int YI = int.Parse(inputs[1]);
            string POS = inputs[2];

            RoomLayout currentRoom = roomTypes[dungeonMap[XI, YI]];
            List<int> newDirections = currentRoom.AvailableExits(POS);

            if (newDirections.Count == 1)
            {
                switch (newDirections[0])
                {
                    case 0:
                        YI -= 1;
                        break;
                    case 1:
                        XI += 1;
                        break;
                    case 2:
                        YI += 1;
                        break;
                    case 3:
                        XI -= 1;
                        break;
                }
            }

            Console.WriteLine("" + XI + " " + YI);
        }
    }

    static Dictionary<int, RoomLayout> DefaultRoomTypes()
    {
        // TOP      - 0
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
    private Dictionary<string, int> directions;

    public RoomLayout()
    {
        entryPoints = new Dictionary<int, List<int>>();

        directions = new Dictionary<string, int>();
        directions.Add("TOP", 0);
        directions.Add("RIGHT", 1);
        directions.Add("BOTTOM", 2);
        directions.Add("LEFT", 3);
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

    public List<int> AvailableExits(string entryPoint)
    {
        return AvailableExits(directions[entryPoint]);
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