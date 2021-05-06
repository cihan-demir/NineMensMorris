using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MuehleStein
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            Dictionary<int, List<int>> moves = new Dictionary<int, List<int>>();

            var fromList = new List<int>();
            var toList = new List<int>();
            try {
                while (!game.GameEnded)
                {
                    Console.WriteLine("\r___________________________");
                    Console.WriteLine("\r" + game.GetBoardAsString());
                    moves = game.GetPossibleMoves();
                    Random r = new Random();

                    var from = moves.Keys.ToList()[r.Next(moves.Count - 1)];
                    fromList.Add(from);

                    var to = moves[from][r.Next(moves[from].Count - 1)];
                    toList.Add(to);

                    game.NextMove(from, to);
                    Thread.Sleep(10);
                }

                Console.WriteLine(game.getText());
            } catch 
            {
                foreach (var k in moves.Keys) {
                    Console.Write("[" + k + "]: {");
                    foreach (int i in moves[k]) {
                        Console.Write(i + ", ");
                    }
                    Console.WriteLine("}");
                }

                Console.WriteLine();
                fromList.ForEach(i => Console.Write(i + ", "));
                Console.WriteLine();
                toList.ForEach(i => Console.Write(i + ", "));
            }
        }
    }
}
