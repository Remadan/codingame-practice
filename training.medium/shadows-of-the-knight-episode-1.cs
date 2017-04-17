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
        int W = int.Parse(inputs[0]); // width of the building.
        int H = int.Parse(inputs[1]); // height of the building.
        int N = int.Parse(Console.ReadLine()); // maximum number of turns before game over.
        inputs = Console.ReadLine().Split(' ');
        int X0 = int.Parse(inputs[0]);
        int Y0 = int.Parse(inputs[1]);
		
		int wTop = 0;		int wLeft = 0;
		int wBottom = H;	int wRight = W;
		
        // game loop
        while (true)
        {
			// U, UR, R, DR, D, DL, L or UL
            string bombDir = Console.ReadLine();
			
			// analizing directions
			int dirVer = 0;
			int dirHor = 0;
			
			if (bombDir.Contains("U"))
				dirVer = -1;
			else if (bombDir.Contains("D"))
				dirVer = 1;

			if (bombDir.Contains("L"))
				dirHor = -1;
			else if (bombDir.Contains("R"))
				dirHor = 1;
			
			// calculating new window interval
			if (dirVer == -1) 
				wBottom = Y0 - 1;
			else if (dirVer == 1)
				wTop = Y0 + 1;
			else
				wTop = wBottom = Y0;

			if (dirHor == -1) 
				wRight = X0 - 1;
			else if (dirHor == 1)
				wLeft = X0 + 1;
			else
				wLeft = wRight = X0;
			
			Console.Error.WriteLine("wTop: " + wTop + " wBottom: " + wBottom + " wLeft: " + wLeft + " wRight: " + wRight);
			// new jump
			Y0 = wTop + (wBottom - wTop) / 2;
			X0 = wLeft + (wRight - wLeft) / 2;
			
            // the location of the next window Batman should jump to.
            Console.WriteLine(X0 + " " + Y0);
        }
    }
}