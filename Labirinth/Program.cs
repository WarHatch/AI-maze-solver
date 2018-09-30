using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Labirinth
{
    class Program
    {
        static readonly Random rnd = new Random();

        static void Solve(ref Wanderer agent, ref Board board, Stopwatch watch)
        {
            if (board.Edge(agent.CurrentPosition.X, agent.CurrentPosition.Y))
                return;

            //// Benchmarking
            //if (watch.IsRunning && knight.MoveLog.Count > 100000)
            //{
            //    watch.Stop();
            //    Console.WriteLine("100000 moves took: " + (watch.ElapsedMilliseconds).ToString());
            //    Console.ReadKey();
            //}

            // Board printing
            string boardView = board.Print();
            Console.WriteLine(boardView);

            Dictionary<int, Point> goodDestinations = agent.GoodDestinations(board);
            while (goodDestinations.Count > 0)
            {
                // Manual Pop() of the next destination
                var nextDestination = goodDestinations.First();
                goodDestinations.Remove(nextDestination.Key);

                // Random nextDestination
                //var chosenPath = rnd.Next(0, goodDestinations.Count);
                //var nextDestination = goodDestinations.ElementAt<Point>(chosenPath);
                //goodDestinations.RemoveAt(chosenPath);

                agent.MoveTo(board, nextDestination.Value, nextDestination.Key);

                Solve(ref agent, ref board, watch);
                if (board.Edge(agent.CurrentPosition.X, agent.CurrentPosition.Y))
                    return;
            }
            if (!board.Edge(agent.CurrentPosition.X, agent.CurrentPosition.Y))
                agent.Backtrack(board);
            return;
        }

        static void Main(string[] args)
        {
            var board = new Board("../../Maze.txt");
            var agent = new Wanderer(ref board, new Point(1, 1));

            Stopwatch watch = Stopwatch.StartNew();
            Solve(ref agent, ref board, watch);
            if (!board.Edge(agent.CurrentPosition.X, agent.CurrentPosition.Y)) Console.WriteLine("[X] Unable to find a solution.");
            else Console.WriteLine(board.Print());
            Console.ReadKey();
        }
    }
}
