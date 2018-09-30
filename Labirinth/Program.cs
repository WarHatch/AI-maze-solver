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

        static void Solve(Wanderer agent, Stopwatch watch)
        {
            if (agent.Board.Edge(agent.CurrentPosition.X, agent.CurrentPosition.Y))
                return;

            //// Benchmarking
            //if (watch.IsRunning && knight.MoveLog.Count > 100000)
            //{
            //    watch.Stop();
            //    Console.WriteLine("100000 moves took: " + (watch.ElapsedMilliseconds).ToString());
            //    Console.ReadKey();
            //}

            // Board printing
            string boardView = agent.Board.Print();
            Console.WriteLine(boardView);

            Dictionary<int, Point> goodDestinations = agent.GoodDestinations();
            while (goodDestinations.Count > 0)
            {
                // Manual Pop() of the next destination
                var nextDestination = goodDestinations.First();
                goodDestinations.Remove(nextDestination.Key);

                // Random nextDestination
                //var chosenPath = rnd.Next(0, goodDestinations.Count);
                //var nextDestination = goodDestinations.ElementAt<Point>(chosenPath);
                //goodDestinations.RemoveAt(chosenPath);

                agent.MoveTo(nextDestination.Value, nextDestination.Key);

                Solve(agent, watch);
                if (agent.Board.Edge(agent.CurrentPosition.X, agent.CurrentPosition.Y))
                    return;
            }
            agent.Backtrack();
            return;
        }

        static void Main(string[] args)
        {
            var board = new Board("../../Maze.txt");
            var agent = new Wanderer(board, new Point(1, 1));

            Stopwatch watch = Stopwatch.StartNew();
            Solve(agent, watch);
            if (!agent.Board.Edge(agent.CurrentPosition.X, agent.CurrentPosition.Y)) Console.WriteLine("[X] Unable to find a solution.");
            else Console.WriteLine(agent.Board.Print());
            Console.ReadKey();
        }
    }
}
