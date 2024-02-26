using System;
using System.Collections.Generic;

public class Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class Player
{
    public string Name { get; set; }
    public Position Position { get; set; }
    public int GemCount { get; set; }

    public Player(string name, Position position)
    {
        Name = name;
        Position = position;
        GemCount = 0;
    }

    public Position Move(string direction, List<Player> players, char[,] board)
    {
        int newX = Position.X;
        int newY = Position.Y;

        switch (direction)
        {
            case "U":
                newY++;
                break;
            case "D":
                newY--;
                break;
            case "L":
                newX--;
                break;
            case "R":
                newX++;
                break;
            default:
                Console.WriteLine("Invalid move, cannot move diagonally.");
                return Position;
        }

        if (newX < 0 || newX >= board.GetLength(0) || newY < 0 || newY >= board.GetLength(1))
        {
            Console.WriteLine("Invalid move, cannot move outside the board.");
            return Position;
        }

        foreach (Player player in players)
        {
            if (player.Position.X == newX && player.Position.Y == newY)
            {
                Console.WriteLine("Invalid move, cannot move onto another player.");
                return Position;
            }
        }

        char newPosition = board[newX, newY];

        if (newPosition == 'O')
        {
            Console.WriteLine("Invalid move, cannot move through obstacles.");
            return Position;
        }
        else if (newPosition == 'G')
        {
            GemCount++;
            board[newX, newY] = '-';
            Console.WriteLine($"{Name} collected a gem!");
        }

        return new Position(newX, newY);
    }
}

public class Game
{
    public int BoardSize { get; set; } = 6;
    public int Turns { get; set; } = 0;
    public Player Winner { get; set; } = null;
    public List<Player> Players { get; set; } = new List<Player>();
    public char[,] Board { get; set; }

    public Game()
    {
        Board = new char[BoardSize, BoardSize];
        InitializeBoard();
        InitializePlayers();
    }

    private void InitializeBoard()
    {
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                Board[i, j] = '-';
            }
        }

        Random random = new Random();
        int numGems = random.Next(1, 10);
        int numObstacles = random.Next(1, 10);

        for (int i = 0; i < numGems; i++)
        {
            int x = random.Next(BoardSize);
            int y = random.Next(BoardSize);
            Board[x, y] = 'G';
        }

        for (int i = 0; i < numObstacles; i++)
        {
            int x = random.Next(BoardSize);
            int y = random.Next(BoardSize);
            Board[x, y] = 'O';
        }
    }

    private void InitializePlayers()
    {
        Players.Add(new Player("P1", new Position(0, 0)));
        Players.Add(new Player("P2", new Position(BoardSize - 1, BoardSize - 1)));
    }

    public void Play()
    {
        while (Turns < 30)
        {
            foreach (Player player in Players)
            {
