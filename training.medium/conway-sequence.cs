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
        List<int> currentLine = new List<int>();
		List<int> newLine = new List<int>();
        
        int R = int.Parse(Console.ReadLine());
        int L = int.Parse(Console.ReadLine());
        
        currentLine.Add(R);
		//newLine = currentLine.GetRange(0, oldList.Count);
        
		for (int i = 0; i < L-1; i++)
		{
			int curNumberValue = -1;
			int curNumberCount = 0;
			newLine.Clear();
			
			foreach (int lineNumber in currentLine)
			{
				//Console.Error.WriteLine(lineNumber);
				if (lineNumber != curNumberValue)
				{
					if (curNumberValue != -1)
					{
						newLine.Add(curNumberCount);
						newLine.Add(curNumberValue);
					}
					curNumberValue = lineNumber;
					curNumberCount = 1;
				}
				else
				{
					curNumberCount++;
				}
			}
			newLine.Add(curNumberCount);
			newLine.Add(curNumberValue);
			
			currentLine = newLine.GetRange(0, newLine.Count);
		}
		
		string result = "";
		foreach (int lineNumber in currentLine)
		{
		    result += lineNumber + " ";
		}
		Console.WriteLine(result.Trim());
    }
}