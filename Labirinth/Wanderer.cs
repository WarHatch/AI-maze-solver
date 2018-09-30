using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labirinth
{
    class Wanderer
    {
        private Stack<Point> moveStack;
        private readonly Point[] manuevers = {new Point(0 , 1), new Point(1, 0), new Point(-1, 0), new Point(0, -1)};

        public Wanderer(Board board, Point startingPosition)
        {
            Board = board;
            moveStack = new Stack<Point>(board.xSize * board.ySize / 2);

            //MoveTo
            moveStack.Push(startingPosition);
            Board.PlaceOn(startingPosition, moveStack.Count);
            Console.WriteLine("Pradine padetis: " + CurrentPosition.ToString() + ". L=" + board.Cells[startingPosition.Y, startingPosition.X]);
        }

        public Point CurrentPosition { get => moveStack.Peek(); }
        public Board Board { get; }
        public List<string> MoveLog { get; } = new List<string>();

        private string NewLogMargin(int amount) => new string('-', moveStack.Count);
        private string MoveCounter() => (moveStack.Count + 1).ToString();

        public void MoveTo(Point newPosition, int manueverIndex)
        {
            string appendix = "Laisva. LAB" + newPosition.ToString() + ":=" + MoveCounter();
            MoveLog.Add(CreateLog(newPosition, manueverIndex, appendix));
            Console.WriteLine(MoveLog.Last());

            moveStack.Push(newPosition);
            Board.PlaceOn(newPosition, moveStack.Count);
        }

        public void Backtrack(){
            //MoveLog.Add(MoveLog.Count.ToString().PadRight(7) + NewLogMargin(moveStack.Count) +
            //    "L= " + (moveStack.Count) + " nebeturi tolesniu zingsniu. Backtrack.");
            Console.WriteLine("".PadRight(7) + NewLogMargin(moveStack.Count) +
                "Backtrack from " + CurrentPosition.ToString() + ", L=" + MoveCounter() +
                ". LAB" + CurrentPosition.ToString() + ":= -1.");

            var falsePosition = moveStack.Pop();
            Board.PlaceOn(falsePosition, -1);
        }

        private string CreateLog(Point newPosition, int manueverIndex, string appendix)
        {
            return MoveLog.Count.ToString().PadRight(7)
                + NewLogMargin(moveStack.Count) + "R" + (manueverIndex+1) + ". "
                + newPosition + ". L= " + moveStack.Count  + ". "
                + appendix;
        }

        public Dictionary<int, Point> GoodDestinations()
        {
            Dictionary<int, Point> goodDestinations = new Dictionary<int, Point>();

            for (int manueverIndex = 0; manueverIndex < manuevers.Length; manueverIndex++)
            {
                var potentialPos = CurrentPosition + manuevers[manueverIndex];
                if (Board.Inside(potentialPos.X, potentialPos.Y))
                {
                    if (Board.Cells[potentialPos.Y, potentialPos.X] == 0)
                    {
                        goodDestinations.Add(manueverIndex, potentialPos);
                    }
                    else
                    {
                        if (Board.Cells[potentialPos.Y, potentialPos.X] == 1)
                            FakeMove(potentialPos, manueverIndex, "Siena");
                        else
                            FakeMove(potentialPos, manueverIndex, "Siulas.");
                    }
                }
            }

            return goodDestinations;
        }

        private void FakeMove(Point newPosition, int manueverIndex, string logAppendix)
        {
            MoveLog.Add(
                MoveLog.Count.ToString().PadRight(7)
                + NewLogMargin(moveStack.Count) + "R" + (manueverIndex + 1) + ". "
                + newPosition.ToString() + ". L= " + moveStack.Count + ". "
                + logAppendix
            );
            Console.WriteLine(MoveLog.Last());
        }
    }
}
