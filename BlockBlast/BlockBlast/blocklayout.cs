using Microsoft.Xna.Framework;

namespace BlockBlast
{
    public class BlockLayout
    {
        public Vector2 position;
        public Vector2[] squarePositions;
        public bool[,] shape;

        public BlockLayout(Vector2 position, bool[,] shape)
        {
            this.position = position;
            this.shape = shape;
        }
        //position is currently taken from the upper left corner of the shape
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
