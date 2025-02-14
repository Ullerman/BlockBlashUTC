using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.Xna.Framework;

namespace BlockBlast
{
    public class BlockLayout
    {
        public Vector2 position;
        public Vector2[] squarePositions;
        public bool[,] shape;

        public bool isdragable = true;
        public bool isPlaced = false;
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

    public class Shapes
    {
        public static Dictionary<string, bool[,]> shapes = new Dictionary<string, bool[,]>
        {
            {
                "L",
                new bool[,]
                {
                    { true, false },
                    { true, false },
                    { true, true },
                }
            },
            {
                "T",
                new bool[,]
                {
                    { true, true, true },
                    { false, true, false },
                    
                }
            },
            {
                "I",
                new bool[,]
                {
                    { true },
                    { true },
                    { true },
                    { true },
                }
            },
            {
                "O",
                new bool[,]
                {
                    { true, true },
                    { true, true },
                }
            },
            {
                "S",
                new bool[,]
                {
                    { false, true, true },
                    { true, true, false },
                }
            },
        };
        public static bool[,] RotateShape(bool[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            bool[,] rotated = new bool[cols, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    rotated[j, rows - 1 - i] = matrix[i, j];
                }
            }
            return rotated;
        }

        public static bool[,] GetRandomShapeandRotation(Random rnd)
        {
            string randomKey = shapes.Keys.ElementAt(rnd.Next(shapes.Count));
            bool[,] shape = shapes[randomKey];
            int rotations = rnd.Next(4);
            for (int i = 0; i < rotations; i++)
            {
                shape = RotateShape(shape);
            }
            return shape;
        }
    }
}
