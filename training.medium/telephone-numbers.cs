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
        Trie phoneBook = new Trie();
        int N = int.Parse(Console.ReadLine());
        for (int i = 0; i < N; i++)
        {
            string telephone = Console.ReadLine();
            phoneBook.AddWord(telephone);
        }

        Console.WriteLine(phoneBook.Count);
    }
}

class Trie
{
    private TrieNode headNode;
    public int Count { get; set; }

    public Trie()
    {
        headNode = new TrieNode(this);
    }

    public void AddWord(string word)
    {
        TrieNode CurrentNode = headNode;
        for (int i = 0; i < word.Length; i++)
            CurrentNode = CurrentNode.GetChild(word[i]);
    }
}

class TrieNode
{
    private Dictionary<char, TrieNode> childrens;
    public char Data { get; }
    private Trie trie;

    public TrieNode(Trie trie, char data = Char.MinValue)
    {
        this.trie = trie;
        this.Data = data;
        childrens = new Dictionary<char, TrieNode>();
    }

    public TrieNode CreateChild(char c)
    {
        var childNode = new TrieNode(trie, c);
        childrens.Add(c, childNode);
        trie.Count++;

        return childNode;
    }

    public TrieNode GetChild(char c)
    {
        TrieNode childNode;
        if (childrens.TryGetValue(c, out childNode))
            return childNode;
        else
            return CreateChild(c);
    }
}