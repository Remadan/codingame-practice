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
        int maxDrop = 0;
		int currentPeak = 0;
		
		int n = int.Parse(Console.ReadLine());
        string[] inputs = Console.ReadLine().Split(' ');
        for (int i = 0; i < n; i++)
        {
            int currentPrice = int.Parse(inputs[i]);
			int currentDrop = currentPeak - currentPrice;
			
			if (currentPrice >= currentPeak)
			{
				currentPeak = currentPrice;
			}
			else if (currentDrop > maxDrop)
			{
				maxDrop = currentDrop;
			}
        }

        Console.WriteLine(-maxDrop);
    }
}