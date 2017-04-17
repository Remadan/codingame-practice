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
		long number1; long number2; long result = 0;
		string operation; 
		string[] numerals = new string[20];
		
		// get "numerals" dimensions
		string[] inputs = Console.ReadLine().Split(' ');
        int numeralLength  = int.Parse(inputs[0]);
        int numeralHeight = int.Parse(inputs[1]);
		
 		// populate "numerals" with description
       for (int i = 0; i < numeralHeight; i++)
        {
			string sourceString = Console.ReadLine();
			for (int j = 0; j < 20; j++)
			{
				numerals[j] = "" + numerals[j] + sourceString.Substring(j * numeralLength, numeralLength);
			}
        }

		// dictionary for reverse search
		Dictionary<string, int> numeralsDecription = numerals
													.Select((v, i) => new {Key = v, Value = i})
													.ToDictionary(o => o.Key, o => o.Value);
		
		// decript input numbers
		number1 = decriptInput(numeralHeight, numeralsDecription);
		number2 = decriptInput(numeralHeight, numeralsDecription);
		
		// detect and run operation
		operation = Console.ReadLine();
		switch(operation)
		{
			case "*":
				result = number1 * number2;
				break;
			case "/":
				result = number1 / number2;
				break;
			case "+":
				result = number1 + number2;
				break;
			case "-":
				result = number1 - number2;
				break;
		}
		
        Console.Error.WriteLine("" + number1 + " " + operation + " " + number2 + " = " + result);
		
		// check for zero
		if (result == 0)
		{
            for (int i = 0; i < numeralHeight; i++)
                Console.WriteLine(numerals[0].Substring(numeralLength * i, numeralLength));
		}
		else
		{
            // convert result to mayan
            List<int> mayanResult = new List<int>();
            long quotient = result;
            while(quotient!=0)
            {
                long temp = quotient % 20;
                mayanResult.Add(Convert.ToInt32(temp));
                quotient = quotient / 20;
            }
            mayanResult.Reverse();
            
            // print out result
            foreach (int mayanNumeral in mayanResult)
            {
                for (int i = 0; i < numeralHeight; i++)
                  Console.WriteLine(numerals[mayanNumeral].Substring(numeralLength * i, numeralLength));
            }
		}
		
    }
	
	static long decriptInput(int numeralHeight, Dictionary<string, int> numeralsDecription)
	{
        long result = 0;
		int numberLength = int.Parse(Console.ReadLine()) / numeralHeight; // how many numerals are in next number
		int[] numberNumerals = new int[numberLength];
        for (int i = 0; i < numberLength; i++)
        {
            // collect numeral string
			string currentNumeral = "";
			for (int j = 0; j < numeralHeight; j++)
			{
				currentNumeral += Console.ReadLine();
			}
			
			numberNumerals[i] = numeralsDecription[currentNumeral];
        }
		Array.Reverse(numberNumerals);
		for (int i = 0; i < numberLength; i++)
			result += Convert.ToInt64(numberNumerals[i] * Math.Pow(20, i));
		
		return result;
	}
}