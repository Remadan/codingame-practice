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
 		int turnCount = 0;
		Queue<Card> player1Hand = new Queue<Card>();
        Queue<Card> player2Hand = new Queue<Card>();
        Queue<Card> player1Table = new Queue<Card>();
        Queue<Card> player2Table = new Queue<Card>();
		
		int n = int.Parse(Console.ReadLine()); // the number of cards for player 1
        for (int i = 0; i < n; i++)
        {
			player1Hand.Enqueue(new Card(Console.ReadLine()));
        }
        int m = int.Parse(Console.ReadLine()); // the number of cards for player 2
        for (int i = 0; i < m; i++)
        {
			player2Hand.Enqueue(new Card(Console.ReadLine()));
        }
		
		// simulating game
		while (player1Hand.Count != 0 && player2Hand.Count != 0)
		{
			turnCount++;
			Queue<Card> winnerHand = null;
			
			Card player1card = player1Hand.Dequeue();
			Card player2card = player2Hand.Dequeue();
			player1Table.Enqueue(player1card);
			player2Table.Enqueue(player2card);
			
			if (player1card.Rank > player2card.Rank)
				winnerHand = player1Hand;
			else if (player1card.Rank < player2card.Rank)
				winnerHand = player2Hand;
			else // it's a war
			{
				while (winnerHand == null)
				{
					// do we have enough cards?
					if (player1Hand.Count() < 4 || player2Hand.Count() < 4)
					{
						break;
					}
					else
					{
						for (int i = 0; i < 3; i++)
						{
							player1Table.Enqueue(player1Hand.Dequeue());
							player2Table.Enqueue(player2Hand.Dequeue());
						}
						
						player1card = player1Hand.Dequeue();
						player2card = player2Hand.Dequeue();
						player1Table.Enqueue(player1card);
						player2Table.Enqueue(player2card);
						
						if (player1card.Rank > player2card.Rank)
							winnerHand = player1Hand;
						else if (player1card.Rank < player2card.Rank)
							winnerHand = player2Hand;
						else
							continue;
					}
				}
				
			}
			
			if (winnerHand != null)
			{
				while (player1Table.Count > 0)
					winnerHand.Enqueue(player1Table.Dequeue());
				while (player2Table.Count > 0)
					winnerHand.Enqueue(player2Table.Dequeue());
			}
			else
			{
				Console.WriteLine("PAT");
				break;
			}
			
		}
        
		if (player1Table.Count == 0 && player2Table.Count == 0)
    	{
    		if (player1Hand.Count == 0)
    			Console.WriteLine("2 " + turnCount);
    		else if (player2Hand.Count == 0)
    			Console.WriteLine("1 " + turnCount);
		}
    }
}

class Card
{
	public int Rank { get; }
	public string Suit { get; }
	
	public Card(string cardDescription)  
	{
		string rankString = cardDescription.Substring(0, cardDescription.Length - 1);
		switch(rankString)
		{
			case "J":
				Rank = 11;
				break;
			case "Q":
				Rank = 12;
				break;
			case "K":
				Rank = 13;
				break;
			case "A":
				Rank = 14;
				break;
			default:
				Rank = int.Parse(rankString);
				break;
		}
		Suit = cardDescription.Last().ToString();                 
	}	
}