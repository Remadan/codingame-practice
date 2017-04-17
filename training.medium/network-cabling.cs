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
        long buildingCount = long.Parse(Console.ReadLine());
        long[] yCoords = new long[buildingCount];
        long xMin = long.MaxValue;
        long xMax = long.MinValue;
        
        for (long i = 0; i < buildingCount; i++)
        {
            string[] inputs = Console.ReadLine().Split(' ');
            long X = long.Parse(inputs[0]);
            long Y = long.Parse(inputs[1]);
            
            xMin = Math.Min(xMin, X);
            xMax = Math.Max(xMax, X);
            
            yCoords[i] = Y;
        }
        
        // finding mean Y coord
        long meanY;
        Array.Sort(yCoords);
        if (buildingCount % 2 == 0)
            meanY = (yCoords[buildingCount / 2] + yCoords[buildingCount / 2 - 1]) / 2;
        else
            meanY = yCoords[buildingCount / 2];
        
        //Console.Error.WriteLine("meanY: " + meanY);
        //Console.Error.WriteLine("xMin: " + xMin);
        //Console.Error.WriteLine("xMax: " + xMax);
        
        long result = 0;
        for (long i = 0; i < buildingCount; i++)
            result += Math.Abs(yCoords[i] - meanY);
        
        result += xMax - xMin;
        
        Console.WriteLine(result);
    }
}