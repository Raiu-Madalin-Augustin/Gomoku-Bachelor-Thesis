using System;
using Gomoku.Logic.Lines;

namespace Gomoku.Logic.BoardRelated
{
    public class Board : IDeepCloneable<Board>, IShallowCloneable<Board>
    {
        private readonly Tile[,] _tiles;

        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            _tiles = new Tile[Width, Height];
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    _tiles[i, j] = new Tile(i, j)
                    {
                        Piece = (Piece)Pieces.None,
                    };
                }
            }
        }

        private Board(Board b)
        {
            Width = b.Width;
            Height = b.Height;
            _tiles = new Tile[Width, Height];
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    _tiles[i, j] = new Tile(i, j)
                    {
                        Piece = b[i, j].Piece,
                    };
                }
            }
        }

        public int Height { get; }

        public int Width { get; }

        public Tile this[int x, int y] => _tiles[x, y];

        public Board DeepClone()
        {
            return new Board(this);
        }

        public void IterateTiles(
          int x,
          int y,
          Directions direction,
          Predicate<Tile> predicate,
          bool iterateSelf = false)
        {
            var startingOffset = iterateSelf ? 0 : 1;

            switch (direction)
            {
                case Directions.Left:
                    for (var i = x - startingOffset;
                        i >= 0 && predicate(_tiles[i, y]);
                        i--)
                    {
                    }

                    break;

                case Directions.Right:
                    for (var i = x + startingOffset;
                        i < Width && predicate(_tiles[i, y]);
                        i++)
                    {
                    }

                    break;

                case Directions.Up:
                    for (var j = y - startingOffset;
                        j >= 0 && predicate(_tiles[x, j]);
                        j--)
                    {
                    }

                    break;

                case Directions.Down:
                    for (var j = y + startingOffset;
                        j < Height && predicate(_tiles[x, j]);
                        j++)
                    {
                    }

                    break;

                case Directions.UpLeft:
                    for (int i = x - startingOffset, j = y - startingOffset;
                        i >= 0 && j >= 0 && predicate(_tiles[i, j]);
                        i--, j--)
                    {
                    }

                    break;

                case Directions.DownRight:
                    for (int i = x + startingOffset, j = y + startingOffset;
                        i < Width && j < Height && predicate(_tiles[i, j]);
                        i++, j++)
                    {
                    }
                    break;

                case Directions.UpRight:
                    for (int i = x + startingOffset, j = y - startingOffset;
                        i < Width && j >= 0 && predicate(_tiles[i, j]);
                        i++, j--)
                    {
                    }

                    break;

                case Directions.DownLeft:
                    for (int i = x - startingOffset, j = y + startingOffset;
                        i >= 0 && j < Height && predicate(_tiles[i, j]);
                        i--, j++)
                    {
                    }

                    break;

                case Directions.None:
                    break;
                default:
                    throw new ArgumentException("Value is not supported.", nameof(direction));
            }
        }

        object IDeepCloneable.DeepClone()
        {
            return DeepClone();
        }

        object IShallowCloneable.ShallowClone()
        {
            return ShallowClone();
        }
        public Board ShallowClone()
        {
            return (Board)MemberwiseClone();
        }
    }
}