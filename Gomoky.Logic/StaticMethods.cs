using Gomoku.Logic.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku.Logic
{
    public static class StaticMethods
    {
        public static (Queue<Tile>, Queue<Tile>, bool) GetLine(Board board, Tile tile, Pieces piece, Directions direction,
            int maxDistance, int tolerance)
        {
            var blankTiles = new Queue<Tile>();
            var blockTiles = new Queue<Tile>();
            var sameTiles = new Queue<Tile>();
            var count = 0;
            var blank = 0;
            var chainBreak = false;

            IterateLine(board, tile, direction, predicate =>
            {
                if (count++ == maxDistance)
                {
                    return false;
                }

                if (predicate.Piece == piece)
                {
                    if (blank > 0)
                    {
                        chainBreak = true;
                    }
                    sameTiles.Enqueue(predicate);
                    return true;
                }
                else if (predicate.Piece.TypeIndex == 0)
                {
                    if (blank++ == tolerance)
                    {
                        return false;
                    }

                    blankTiles.Enqueue(predicate);
                    return true;
                }
                else
                {
                    blockTiles.Enqueue(predicate);
                    return false;
                }
            });
            return (sameTiles, blockTiles, !chainBreak);
        }

        private static void IterateLine(Board board, Tile tile, Directions direction, Predicate<Tile> predicate)
        {
            const int startingOffset = 1;

            switch (direction)
            {
                case Directions.Left:
                    for (int i = tile.X - startingOffset, j = tile.Y;
                         i >= 0 && predicate(board[i, j]);
                         i--)
                    {
                    }

                    break;

                case Directions.Right:
                    for (int i = tile.X + startingOffset, j = tile.Y;
                         i < board.Width && predicate(board[i, j]);
                         i++)
                    {
                    }

                    break;

                case Directions.Up:
                    for (int i = tile.X, j = tile.Y - startingOffset;
                         j >= 0 && predicate(board[i, j]);
                         j--)
                    {
                    }

                    break;

                case Directions.Down:
                    for (int i = tile.X, j = tile.Y + startingOffset;
                         j < board.Height && predicate(board[i, j]);
                         j++)
                    {
                    }

                    break;

                case Directions.UpLeft:
                    for (int i = tile.X - startingOffset, j = tile.Y - startingOffset;
                         i >= 0 && j >= 0 && predicate(board[i, j]);
                         i--, j--)
                    {
                    }

                    break;

                case Directions.DownRight:
                    for (int i = tile.X + startingOffset, j = tile.Y + startingOffset;
                         i < board.Width && j < board.Height && predicate(board[i, j]);
                         i++, j++)
                    {
                        ;
                    }

                    break;

                case Directions.UpRight:
                    for (int i = tile.X + startingOffset, j = tile.Y - startingOffset;
                         i < board.Width && j >= 0 && predicate(board[i, j]);
                         i++, j--)
                    {
                    }

                    break;

                case Directions.DownLeft:
                    for (int i = tile.X - startingOffset, j = tile.Y + startingOffset;
                         i >= 0 && j < board.Height && predicate(board[i, j]);
                         i--, j++)
                    {
                    }

                    break;
                default:
                    throw new ArgumentException("Value is not supported.", nameof(direction));

            };
        }
        public static IEnumerable<Tile> CombineLines(Queue<Tile> firstLine, Queue<Tile> secondLine)
        {
            return firstLine.ToList().Concat(secondLine.ToList());
        }

        public static (IEnumerable<Tile>, IEnumerable<Tile>, bool) CombineLines((Queue<Tile>, Queue<Tile>, bool) firstLine, (Queue<Tile>, Queue<Tile>, bool) secondLine)
        {
            return (firstLine.Item1.Concat(secondLine.Item2), firstLine.Item2.Concat(secondLine.Item2), firstLine.Item3 && secondLine.Item3);
        }

        public static (IEnumerable<Tile>, IEnumerable<Tile>, bool) GetBlockedAndSameTiles(Board board, Tile tile, Piece piece, Orientations orientation, int maxDistance, int tolerance)
        {
            return orientation switch
            {
                Orientations.Horizontal => CombineLines(
                    GetLine(board, tile, piece, Directions.Left, maxDistance, tolerance),
                    GetLine(board, tile, piece, Directions.Right, maxDistance, tolerance)),
                Orientations.Diagonal => CombineLines(
                    GetLine(board, tile, piece, Directions.UpLeft, maxDistance, tolerance),
                    GetLine(board, tile, piece, Directions.DownRight, maxDistance, tolerance)
                ),
                Orientations.Vertical => CombineLines(
                    GetLine(board, tile, piece, Directions.Up, maxDistance, tolerance),
                    GetLine(board, tile, piece, Directions.Down, maxDistance, tolerance)
                ),
                Orientations.SecondDiagonal => CombineLines(
                    GetLine(board, tile, piece, Directions.UpRight, maxDistance, tolerance),
                    GetLine(board, tile, piece, Directions.DownLeft, maxDistance, tolerance)
                ),
                Orientations.None => throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
