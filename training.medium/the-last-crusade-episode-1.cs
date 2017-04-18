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
}

class roomLayout
{
    private Dictionary<int, List<int>> entryPoints;

    public roomLayout()
    {
        entryPoints = new Dictionary<int, List<int>>();
    }

    public List<int> AvaylibleExits(int entryPoint)
    {
        List<>
        if (entryPoints.TryGetValue(entryPoint, out ))
        {

        }
    }

}