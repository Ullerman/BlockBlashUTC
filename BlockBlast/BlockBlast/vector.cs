using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VectorGraphics
{
    public class PrimitiveBatch
    {
        public Texture2D WhitePixel;
        public Texture2D WhiteCircle;
        public GraphicsDevice graphicsDevice;

        public PrimitiveBatch(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            CreateTextures();
        }

        public void CreateTextures()
        {
            WhitePixel = new Texture2D(graphicsDevice, 1, 1);
            WhitePixel.SetData(new[] { Color.White });

            int diameter = 1000;
            WhiteCircle = new Texture2D(graphicsDevice, diameter, diameter);
            Color[] colorData = new Color[diameter * diameter];

            float radius = diameter / 2f;
            Vector2 center = new Vector2(radius, radius);

            for (int y = 0; y < diameter; y++)
            {
                for (int x = 0; x < diameter; x++)
                {
                    Vector2 position = new Vector2(x, y);
                    float distance = Vector2.Distance(center, position);

                    if (distance <= radius)
                    {
                        colorData[y * diameter + x] = Color.White;
                    }
                    else
                    {
                        colorData[y * diameter + x] = Color.Transparent;
                    }
                }
            }

            WhiteCircle.SetData(colorData);
        }

        public class Line
        {
            public Vector2 Start;
            public Vector2 End;
            public Color Color;
            public float Width;

            public Line(Vector2 start, Vector2 end, Color color, float width)
            {
                Start = start;
                End = end;
                Color = color;
                Width = width;
            }

            public Line(Vector2 start, float angle, float distance, Color color, float width)
            {
                Start = start;
                End = new Vector2(
                    (float)Math.Cos(angle) * distance,
                    (float)Math.Sin(angle) * distance
                );
                Color = color;
                Width = width;
            }

            public void Draw(SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch)
            {
                Vector2 edge = End - Start;
                float angle = (float)Math.Atan2(edge.Y, edge.X);
                spriteBatch.Draw(
                    primitiveBatch.WhitePixel,
                    new Microsoft.Xna.Framework.Rectangle(
                        (int)(Start.X),
                        (int)(Start.Y),
                        (int)edge.Length(),
                        (int)Width
                    ),
                    null,
                    Color,
                    angle,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0
                );
            }
        }

        public class Circle
        {
            public Vector2 Position;
            public float Radius;
            public Color Color;

            public Circle(Vector2 position, float radius, Color color)
            {
                Position = position;
                Radius = radius;
                Color = color;
            }

            public void Draw(SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch)
            {
                spriteBatch.Draw(
                    primitiveBatch.WhiteCircle,
                    Position,
                    null,
                    Color,
                    0,
                    new Vector2(
                        primitiveBatch.WhiteCircle.Width / 2,
                        primitiveBatch.WhiteCircle.Height / 2
                    ),
                    new Vector2(
                        Radius / primitiveBatch.WhiteCircle.Width,
                        Radius / primitiveBatch.WhiteCircle.Height
                    ),
                    SpriteEffects.None,
                    0
                );
            }
        }

        public class Rectangle
        {
            public Vector2 Position;
            public Vector2 Size;
            public Color Color;

            public Rectangle(Vector2 position, Vector2 size, Color color)
            {
                Position = position;
                Size = size;
                Color = color;
            }

            public Rectangle(Microsoft.Xna.Framework.Rectangle rectangle, Color color)
            {
                Position = new Vector2(rectangle.X, rectangle.Y);
                Size = new Vector2(rectangle.Width, rectangle.Height);
                Color = color;
            }

            public void Draw(SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch)
            {
                spriteBatch.Draw(
                    primitiveBatch.WhitePixel,
                    new Microsoft.Xna.Framework.Rectangle(
                        (int)(Position.X),
                        (int)(Position.Y),
                        (int)Size.X,
                        (int)Size.Y
                    ),
                    Color
                );
            }
        }
    }

    public class RectangleTexture
    {
        public Vector2 size;
        private Texture2D texture;

        public Texture2D CreateTexture(Vector2 size, PrimitiveBatch primitiveBatch)
        {
            this.size = size;

            texture = new Texture2D(primitiveBatch.graphicsDevice, (int)size.X, (int)size.Y);
            Color[] data = new Color[(int)size.X * (int)size.Y];
            for (int i = 0; i < data.Length; ++i)
                data[i] = Color.White;
            texture.SetData(data);
            return texture;
        }
    }

    public class Pixel
    {
        public Vector2 Position;
        public Color Color;

        public Pixel(Vector2 position, Color color)
        {
            Position = position;
            Color = color;
        }

        public void Draw(SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch)
        {
            spriteBatch.Draw(primitiveBatch.WhitePixel, Position, Color);
        }
    }
}
