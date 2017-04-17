using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Don't let the machines win. You are humanity's last hope...
 **/
class Player
{
    static void Main(string[] args)
    {
        int width = int.Parse(Console.ReadLine()); // the number of cells on the X axis
        int height = int.Parse(Console.ReadLine()); // the number of cells on the Y axis
        int[,] nodesArray = new int[width, height];
		for (int i = 0; i < height; i++)
        {
            string line = Console.ReadLine(); // width characters, each either 0 or .
            Console.Error.WriteLine(line);
			for (int j = 0; j < width; j++)
			{
				nodesArray[j,i] = (line[j] == '0') ? 1:0;
			}
        }
        
        int x2, y2, x3, y3;
		int offset;
        
 		for (int y1 = 0; y1 < height; y1++)
        {
			for (int x1 = 0; x1 < width; x1++)
			{
			    
				// first, check our node
				if (nodesArray[x1,y1] == 0)
				{
					continue;
					//x2 = -1; y2 = -1; 
					//x3 = -1; y3 = -1; 
				}
				else
				{
					// neighbor to the right
					x2 = -1; y2 = -1; 
					for (offset = 1; (x1 + offset) < width; offset++)
					{
						if (nodesArray[x1+offset,y1] != 0)
						{
							x2 = x1+offset; y2 = y1; 
							break;
						}
					}

					// neighbor to the bottom
					x3 = -1; y3 = -1; 
					for (offset = 1; (y1 + offset) < height; offset++)
					{
						if (nodesArray[x1,y1+offset] != 0)
						{
							x3 = x1; y3 = y1+offset; 
							break;
						}
					}
					
				}
				
				// Three coordinates: a node, its right neighbor, its bottom neighbor
		        Console.WriteLine("" + x1 + " " + y1 + " " + x2 + " " + y2 + " " + x3 + " " + y3);

			}
        }
       
    }
}