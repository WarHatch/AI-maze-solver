using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Labirinth
{
    class Wanderer
    {
        private Stack<Move> moveStack;
        private readonly Point[] manuevers = {new Point(0 , 1), new Point(1, 0), new Point(-1, 0), new Point(0, -1)};

        public Wanderer(ref Board board, Point startingPosition)
        {
            moveStack = new Stack<Move>(board.xSize * board.ySize / 2);

            //MoveTo
            moveStack.Push(new Move(startingPosition, 0));
            board.PlaceOn(startingPosition, moveStack.Count);
            Console.WriteLine("Pradine padetis: " + CurrentPosition.ToString() + ". L=" + MoveCounter());
        }

        public Point CurrentPosition { get => moveStack.Peek().to; }
        public List<string> MoveLog { get; } = new List<string>();

        private string NewLogMargin() => new string('-', moveStack.Count - 1);
        private string MoveCounter() => (moveStack.Count + 1).ToString();

        public void MoveTo(Board board, Point newPosition, int manueverIndex)
        {
            string appendix = "Laisva. LAB" + newPosition.ToString() + ":=" + MoveCounter();
            MoveLog.Add(CreateLog(newPosition, manueverIndex, appendix));
            Console.WriteLine(MoveLog.Last());

            moveStack.Push(new Move(newPosition, manueverIndex));
            board.PlaceOn(newPosition, moveStack.Count);
        }

        public void Backtrack(Board board)
        {
            //MoveLog.Add(MoveLog.Count.ToString().PadRight(7) + NewLogMargin(moveStack.Count) +
            //    "L= " + (moveStack.Count) + " nebeturi tolesniu zingsniu. Backtrack.");
            Console.WriteLine("".PadRight(7) + NewLogMargin() +
                "Backtrack from " + CurrentPosition.ToString() + ", L=" + MoveCounter() +
                ". LAB" + CurrentPosition.ToString() + ":= -1.");

            var falsePosition = moveStack.Pop().to;
            board.PlaceBacktrackToken(falsePosition);
        }

        private string CreateLog(Point newPosition, int manueverIndex, string appendix)
        {
            return MoveLog.Count.ToString().PadRight(7)
                + NewLogMargin() + "R" + manueverIndex + ". "
                + newPosition + ". L:= " + (moveStack.Count + 2) + ". " // TODO: refactor (moveStack.Count + 3)
                + appendix;
        }

        public Dictionary<int, Point> GoodDestinations(Board board)
        {
            Dictionary<int, Point> goodDestinations = new Dictionary<int, Point>();

            for (int manueverIndex = 0; manueverIndex < manuevers.Length; manueverIndex++)
            {
                var potentialPos = CurrentPosition + manuevers[manueverIndex];
                if (board.Inside(potentialPos.X, potentialPos.Y))
                {
                    if (board.Cells[potentialPos.Y, potentialPos.X] == 0)
                    {
                        goodDestinations.Add(manueverIndex, potentialPos);
                    }
                    else
                    {
                        if (board.Cells[potentialPos.Y, potentialPos.X] == 1)
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
                + NewLogMargin() + "R" + manueverIndex + ". "
                + newPosition.ToString() + ". L= " + MoveCounter() + ". "
                + logAppendix
            );
            Console.WriteLine(MoveLog.Last());
        }

        public string printMoves()
        {
            StringBuilder printedMoves = new StringBuilder();

            Move[] moves = new Move[moveStack.Count];
            moveStack.CopyTo(moves, 0);
            for (int i = moveStack.Count - 2; i >= 0; i--)
            {
                printedMoves.Append("R" + moves[i].rule + ", ");
            }
            return printedMoves.ToString();
        }
    }
}
