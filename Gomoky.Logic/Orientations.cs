using System;

namespace Gomoku.Logic
{
  public enum Orientations : byte
  {
    None = 0,
    Horizontal = 1 << 0,
    Vertical = 1 << 2,
    Diagonal = 1 << 3,
    SecondDiagonal = 1 << 4,

  }
}