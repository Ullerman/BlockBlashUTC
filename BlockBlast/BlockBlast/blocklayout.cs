using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;

namespace BlockBlast
{
    public class BlockLayout
    {
        public Vector2 position;
        public Vector2[] squarePositions;
        public bool[,] shape;

        public bool isdragable = true;
        public Color color;

        public BlockLayout(Vector2 position, bool[,] shape, Color color)
        {
            this.position = position;
            this.shape = shape;
            this.color = color;
        }
        //position is currently taken from the upper left corner of the shape
    }

    public class Square
    {
        public Vector2 position;
        public Color color;
        public bool isdragable = true;

        public Square(Vector2 position, Color color)
        {
            this.position = position;
            this.color = color;
        }
    }

    public class L_Shape
    {
        public static readonly bool[,] shape = new bool[,]
        {
            { true, false },
            { true, false },
            { true, true },
        };

        public L_Shape() { }
    }

    class T_Shape
    {
        public static readonly bool[,] shape = new bool[,]
        {
            { true, true, true },
            { false, true, false },
            { false, true, false },
        };
    }

    class i_Shape
    {
        public static readonly bool[,] shape = new bool[,]
        {
            { true },
            { true },
            { true },
            { true },
        };
    }

    class O_Shape
    {
        public static readonly bool[,] shape = new bool[,]
        {
            { true, true },
            { true, true },
        };
    }

    class S_Shape
    {
        public static readonly bool[,] shape = new bool[,]
        {
            { false, true, true },
            { true, true, false },
        };
    }
}
