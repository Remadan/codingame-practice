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
        int oodsCount = int.Parse(Console.ReadLine());
        int giftPrice = int.Parse(Console.ReadLine());
        List<int> budjet = new List<int>();
        int budjetTotal = 0;

        for (int i = 0; i < oodsCount; i++)
        {
            budjet.Add(int.Parse(Console.ReadLine()));
            budjetTotal += budjet[i];
        }

        if (budjetTotal < giftPrice)
            Console.WriteLine("IMPOSSIBLE");
        else
        {
            budjet.Sort();
            int budjetRemaining = giftPrice;
            for (int i = 0; i < oodsCount; i++)
            {
                int oodContribution = 0;
                int averagebudjet = budjetRemaining / (oodsCount - i);
                if (averagebudjet > budjet[i])
                    oodContribution = budjet[i];
                else if (i == (oodsCount - 1)) // last ood
                    oodContribution = budjetRemaining;
                else
                    oodContribution = averagebudjet;

                budjetRemaining -= oodContribution;
                Console.WriteLine(oodContribution);
            }
        }

    }
}