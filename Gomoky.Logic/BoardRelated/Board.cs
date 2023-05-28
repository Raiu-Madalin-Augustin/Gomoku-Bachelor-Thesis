﻿using System;

namespace Gomoku.Logic.BoardRelated
{
    public class Board : IDeepCloneable<Board>, IShallowCloneable<Board>
    {
        private readonly Tile[,] _tiles;

        public Board(int width, int height)
        {
            if (height < 0 || width < 0)
            {
                throw new ArgumentException($"{nameof(Board)} must have non-negative dimensions.");
            }

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

        /// <summary>
        /// Creates a <see cref="Board"/> from a <see cref="string"/> where rows are
        /// semi-colon-separated and columns are comma-separated.
        /// </summary>
        /// <param name="s">the <see cref="string"/> to parse</param>
        /// <returns>a <see cref="Board"/></returns>
        /// <exception cref="FormatException"></exception>
        public static Board Parse(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new FormatException($"{nameof(s)} must not be empty.");
            }

            var heightSplit = s.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (heightSplit.Length == 0)
            {
                throw new FormatException($"{nameof(s)} is not format compliant. (misformatted rows)");
            }

            var widthSplit = heightSplit[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (widthSplit.Length == 0)
            {
                throw new FormatException($"{nameof(s)} is not format compliant. (misformatted columns)");
            }

            var b = new Board(widthSplit.Length, heightSplit.Length);

            for (var j = 0; j < heightSplit.Length; j++)
            {
                widthSplit = heightSplit[j].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (widthSplit.Length != b.Width)
                {
                    throw new FormatException($"{nameof(s)} is not format compliant. (jagged array)");
                }

                for (var i = 0; i < widthSplit.Length; i++)
                {
                    var pieceStr = widthSplit[i];
                    int piece;

                    try
                    {
                        piece = int.Parse(pieceStr);
                    }
                    catch (Exception ex)
                    {
                        throw new FormatException($"{nameof(s)} is not format compliant. (unable to parse '{pieceStr}' to piece)", ex);
                    }

                    b[piece, j].Piece = (Piece)piece;
                }
            }

            return b;
        }

        public Board DeepClone()
        {
            return new Board(this);
        }

        public Tile GetTile(IPositional positional)
        {
            return this[positional.X, positional.Y];
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
                    for (int i = x - startingOffset, j = y;
                        i >= 0 && predicate(_tiles[i, j]);
                        i--)
                    {
                    }

                    break;

                case Directions.Right:
                    for (int i = x + startingOffset, j = y;
                        i < Width && predicate(_tiles[i, j]);
                        i++)
                    {
                    }

                    break;

                case Directions.Up:
                    for (int i = x, j = y - startingOffset;
                        j >= 0 && predicate(_tiles[i, j]);
                        j--)
                    {
                    }

                    break;

                case Directions.Down:
                    for (int i = x, j = y + startingOffset;
                        j < Height && predicate(_tiles[i, j]);
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
                        ;
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

                default:
                    throw new ArgumentException("Value is not supported.", nameof(direction));
            }
        }

        public override string ToString()
        {
            var res = string.Empty;

            for (var j = 0; j < Height; j++)
            {
                for (var i = 0; i < Width; i++)
                {
                    res += _tiles[i, j].Piece.TypeIndex.ToString() + ",";
                }

                res = res[0..^1];
                res += ";";
            }
            res = res[0..^1];

            return res;
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