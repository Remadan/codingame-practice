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

        List<ScrabbleWord> gameDictionary = new List<ScrabbleWord>();
        ScrabbleRuleset ruleset = new ScrabbleRuleset();

        ruleset.AddCharScores("eaionrtlsu", 1);
        ruleset.AddCharScores("dg",         2);
        ruleset.AddCharScores("bcmp",       3);
        ruleset.AddCharScores("fhvwy",      4);
        ruleset.AddCharScores("k",          5);
        ruleset.AddCharScores("jx",         8);
        ruleset.AddCharScores("qz",         10);

        int N = int.Parse(Console.ReadLine());
        for (int i = 0; i < N; i++)
        {
            string W = Console.ReadLine();
            gameDictionary.Add(new ScrabbleWord(W, ruleset));
        }

        ScrabbleWord bestWord = null;
        string gameLetters = Console.ReadLine();
        foreach (ScrabbleWord gameWord in gameDictionary)
        {

            if (gameWord.Usable(gameLetters) && bestWord == null)
                bestWord = gameWord;
            else if(gameWord.Usable(gameLetters) && gameWord.Score > bestWord.Score)
                bestWord = gameWord;
        }

        if (bestWord == null)
            Console.WriteLine("Empty list");
        else
            Console.WriteLine(bestWord.Word);
    }
}


class ScrabbleWord
{
    public string Word { get; }
    public int Score { get; }
    public int Length { get; }

    public ScrabbleWord(string word, ScrabbleRuleset ruleset)
    {
        Word = word;
        Score = ruleset.CalculateWordScore(word);
        Length = word.Length;
    }

    public bool Usable(string gameLetters)
    {
        if (Length > gameLetters.Length)
            return false;

        foreach (char c in Word)
        {
            int charFound = gameLetters.IndexOf(c);
            if (charFound == -1)
                return false;
            else
                gameLetters = gameLetters.Remove(charFound, 1);
        }
        return true;
    }
}

class ScrabbleRuleset
{
    private Dictionary<char, int> charScore;

    public ScrabbleRuleset()
    {
        charScore = new Dictionary<char, int>();
    }

    public void AddCharScores(string characters, int score)
    {
        foreach (char c in characters)
            charScore.Add(c, score);
    }

    public int CalculateWordScore(string word)
    {
        int result = 0;
        foreach (char c in word)
            result += charScore[c];

        return result;
    }
}