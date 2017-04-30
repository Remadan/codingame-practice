using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

class Solution
{
    static void Main(string[] args)
    {
        BenderBot bot;
        TownMap townMap;
        string movesHistory = "";
        bool gameIsRunning = true;

        string[] inputs = Console.ReadLine().Split(' ');
        int lines = int.Parse(inputs[0]);
        int collumns = int.Parse(inputs[1]);

        // populating map
        int startCoordX = 0;
        int startCoordY = 0;
        townMap = new TownMap(collumns, lines);
        for (int i = 0; i < lines; i++)
        {
            string row = Console.ReadLine();
            for (int j = 0; j < collumns; j++)
            {
                char tile = row[j];
                townMap.Tiles[j, i] = tile;
                if (tile == '@')
                {
                    startCoordX = j;
                    startCoordY = i;
                }
            }
        }

        //Console.Error.WriteLine("Bot started: " + startCoordX + " " + startCoordY);
        bot = new BenderBot(startCoordX, startCoordY, townMap);

        // main loop
        while (gameIsRunning)
        {
            string latestMove = bot.Move();
            if (latestMove == "LOOP")
            {
                movesHistory = latestMove;
                gameIsRunning = false;
            }
            else
            {
                movesHistory += latestMove + System.Environment.NewLine;
            }
            gameIsRunning = gameIsRunning && !bot.Dead;
        }

        Console.WriteLine(movesHistory);
    }
}

class TownMap
{
    public char[,] Tiles { get; set; }
    public int Lines { get; }
    public int Collumns { get; }

    public TownMap(int collumns, int lines)
    {
        Lines = lines;
        Collumns = collumns;
        Tiles = new char[collumns, lines];
    }

}

class BenderBot
{
    enum Direction { SOUTH, EAST, NORTH, WEST }

    struct BenderBotState
    {
        public int CoordX { get; }
        public int CoordY { get; }
        public Direction Direction { get; }
        public bool InvertedMode { get; }
        public bool BreakerMode { get; }

        public BenderBotState(BenderBot bot)
        {
            this.CoordX = bot.coordX;
            this.CoordY = bot.coordY;
            this.Direction = bot.direction;
            this.InvertedMode = bot.invertedMode;
            this.BreakerMode = bot.breakerMode;
        }
    }
    private HashSet<BenderBotState> botHistory;

    private int coordX;
    private int coordY;
    private Direction direction;
    private bool invertedMode;
    private bool breakerMode;
    private TownMap townMap;

    public bool Dead { get; private set; }

    public BenderBot(int coordX, int coordY, TownMap townMap)
    {
        this.coordX = coordX;
        this.coordY = coordY;
        this.townMap = townMap;
        direction = Direction.SOUTH;

        invertedMode = false;
        breakerMode = false;
        Dead = false;

        botHistory = new HashSet<BenderBotState>();
    }

    public string Move()
    {
        // long loop detection
        BenderBotState currentState = GetCurrentState();
        if (botHistory.Contains(currentState))
            return "LOOP";
        botHistory.Add(currentState);

        char nextTile = ' ';
        int newDeltaX = 0;
        int newDeltaY = 0;
        string latestMove = "";
        // lets find next tile to handle
        for (int i = 0; i < 4; i++)
        {
            // where to go from here?
            newDeltaX = 0;
            newDeltaY = 0;
            switch (direction)
            {
                case Direction.SOUTH:
                    newDeltaY = 1;
                    break;
                case Direction.NORTH:
                    newDeltaY = -1;
                    break;
                case Direction.EAST:
                    newDeltaX = 1;
                    break;
                case Direction.WEST:
                    newDeltaX = -1;
                    break;
            }
            nextTile = townMap.Tiles[coordX + newDeltaX, coordY + newDeltaY];

            // is it passable tile?
            if (nextTile != '#' && !(nextTile == 'X' && !breakerMode))
                break;
            else
                direction = GetNewDirection(i);

            // 4 turns in a row - LOOP
            if (i == 3)
                return "LOOP";
        }

        // change position
        coordX += newDeltaX;
        coordY += newDeltaY;
        latestMove = direction.ToString("G");

        // tile handling
        switch (nextTile)
        {
            case 'S':
                direction = Direction.SOUTH;
                break;
            case 'E':
                direction = Direction.EAST;
                break;
            case 'N':
                direction = Direction.NORTH;
                break;
            case 'W':
                direction = Direction.WEST;
                break;
            case '$':
                Dead = true;
                break;
            case 'X':
                RemoveCurrentTile();
                break;
            case 'I':
                invertedMode = !invertedMode;
                break;
            case 'B':
                breakerMode = !breakerMode;
                break;
            case 'T':
                Teleport();
                break;
        }

        //Console.Error.WriteLine(latestMove + " " + coordX + " " + coordY);
        return latestMove;
    }

    private void Teleport()
    {
        // find another teleport
        for (int i = 0; i < townMap.Lines; i++)
        {
            for (int j = 0; j < townMap.Collumns; j++)
            {
                char tile = townMap.Tiles[j, i];
                if (tile == 'T' && !(j == coordX && i == coordY))
                {
                    coordX = j;
                    coordY = i;
                    return;
                }
            }
        }
    }

    private void RemoveCurrentTile()
    {
        townMap.Tiles[coordX, coordY] = ' ';
        botHistory.Clear();
    }

    private Direction GetNewDirection(int step)
    {
        if (!invertedMode)
            return (Direction)step;
        else
            return (Direction)3 - step;
    }

    private BenderBotState GetCurrentState()
    {
        return new BenderBotState(this);
    }

}