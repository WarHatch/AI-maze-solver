using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Labirinth
{
    public class Board
    {
        public readonly int xSize;
        public readonly int ySize;

        public int[,] Cells { get; }

        public Board(string filepath)
        {
            xSize = 20;
            ySize = 15;
            Cells = new int[ySize, xSize];
            using (TextReader reader = File.OpenText(filepath))
            {
                for (int y = 0; y < ySize; y++)
                {
                    string text = reader.ReadLine();
                    string[] bits = text.Split(' ');
                    for (int x = 0; x < xSize; x++)
                    {
                        Cells[y, x] = int.Parse(bits[x]);
                    }
                }
            }
        }

        public void PlaceOn(Point newPosition, int stackCount)
        {
            Cells[newPosition.Y, newPosition.X] = stackCount + 1;
        }

        // For maze only
        public void PlaceBacktrackToken(Point newPosition)
        {
            Cells[newPosition.Y, newPosition.X] = -1;
        }

        public void ResetCell(Point resetPosition)
        {
            Cells[resetPosition.Y, resetPosition.X] = 0;
        }

        public bool Inside(int x, int y) => (x > -1 && x < xSize && y > -1 && y < ySize);
        public bool Edge(int x, int y) => (x == 0 || x == xSize - 1 || y == 0 || y == ySize - 1);

        // Unused
        public bool AllSpacesTaken()
        {
            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    if (Cells[y, x] == 0)
                        return false;
                }
            }
            return true;
        }

        public string Print()
        {
            StringBuilder board = new StringBuilder();

            for (int y = ySize; y > 0; y--)
            {
                board.AppendFormat(y.ToString().PadLeft(3) + "|");
                for (int x = xSize; x > 0; x--)
                {
                    board.AppendFormat(Cells[y-1, xSize-x].ToString().PadLeft(3));
                }
                board.AppendLine();
            }

            board.Append("Y".PadLeft(3) + "|");
            for (int x = 0; x < xSize; x++)
            {
                board.Append("---");
            }
            board.AppendLine();

            board.Append("X".PadLeft(4));
            for (int x = 1; x <= xSize; x++)
            {
                board.AppendFormat(x.ToString().PadLeft(3));
            }
            return board.ToString();
        }
    }
}
