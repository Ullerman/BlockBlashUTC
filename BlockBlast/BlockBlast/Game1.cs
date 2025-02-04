using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector;

namespace BlockBlast;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private PrimitiveBatch _primitiveBatch;

    //Textures
    private Texture2D _blockTexture;
    private Texture2D _backgroundBlockTexture;

    //Texture Data
    private Vector2 _BLOCKSIZE = new Vector2(50);
    private const float _PADDING = 5;

    //Board Data
    private bool[,] _board = new bool[8, 8];

    //blocks
    private BlockLayout test = new BlockLayout(new Vector2(0, 0), L_Shape.shape);

    private List<BlockLayout> _blocks = new List<BlockLayout>();

    private Vector2[,] _backroundBlockPositions = new Vector2[8, 8];

    //general control dataa

    private Vector2 _previousMousePosition;
    private bool _isDragging = false;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        _graphics.PreferredBackBufferWidth = 600;
        _graphics.PreferredBackBufferHeight = 800;
    }

    private void BoardInitilizer(bool value, float padding)
    {
        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int j = 0; j < _board.GetLength(1); j++)
            {
                _board[i, j] = value;
            }
        }
        float centreXPad =
            Window.ClientBounds.Width / 2
            - (_BLOCKSIZE.X + padding) * _backroundBlockPositions.GetLength(0) / 2;
        float centreYPad = Window.ClientBounds.Height * 0.1f;
        for (int x = 0; x < _backroundBlockPositions.GetLength(0); x++)
        {
            for (int y = 0; y < _backroundBlockPositions.GetLength(1); y++)
            {
                _backroundBlockPositions[y, x] = new Vector2(
                    centreXPad + x * (_BLOCKSIZE.X + padding),
                    centreYPad + y * (_BLOCKSIZE.Y + padding)
                );
            }
        }
    }

    protected override void Initialize()
    {
        //Board Data
        test.position = new Vector2(0, 0);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _primitiveBatch = new PrimitiveBatch(GraphicsDevice);
        var rectangleTexture = new RectangleTexture();

        //Textures
        _blockTexture = Content.Load<Texture2D>("block");
        _backgroundBlockTexture = rectangleTexture.CreateTexture(_BLOCKSIZE, _primitiveBatch);
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();
        MouseState mouseState = Mouse.GetState();
        Vector2 currentMousePosition = new Vector2(mouseState.X, mouseState.Y);

        BoardInitilizer(false, _PADDING);
        test.squarePositions = BuildBlock(test.position, L_Shape.shape);

        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            if (!_isDragging)
            {
                for (int i = 0; i < test.squarePositions.Length; i++)
                {
                    Vector2 square = test.squarePositions[i];
                    if (IsmouseOverRectangle(square, _BLOCKSIZE, mouseState))
                    {
                        _isDragging = true;
                        break;
                    }
                }
            }
            else
            {
                Vector2 mouseDelta = currentMousePosition - _previousMousePosition;
                test.position += mouseDelta;
            }
        }
        else
        {
            _isDragging = false;
        }
        _previousMousePosition = currentMousePosition;
        if (!_isDragging)
        {
            LockToGrid(ref test);
        }

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    private void LockToGrid(ref BlockLayout block)
    {
        Vector2[] newsquares = new Vector2[block.squarePositions.Length];
        byte sqrcheck = 0;
        for (int i = 0; i < block.squarePositions.Length; i++)
        {
            Vector2 square = block.squarePositions[i];
            foreach (Vector2 gridPosition in _backroundBlockPositions)
            {
                if (Vector2.Distance(square, gridPosition) < 16)
                {
                    Console.WriteLine($"Square: {square} Grid: {gridPosition}");
                    newsquares[i] = gridPosition;
                    sqrcheck++;
                    break;
                }
            }
        }
        if (sqrcheck == block.squarePositions.Length)
        {
            block.position = newsquares[0];
            block.squarePositions = newsquares;
        }
    }

    private bool IsmouseOverRectangle(Vector2 position, Vector2 size, MouseState mouseState)
    {
        return (
            mouseState.X > position.X
            && mouseState.X < position.X + size.X
            && mouseState.Y > position.Y
            && mouseState.Y < position.Y + size.Y
        );
    }

    private Vector2[] BuildBlock(Vector2 position, bool[,] shape)
    {
        List<Vector2> block = new List<Vector2>();
        for (int i = 0; i < shape.GetLength(0); i++)
        {
            for (int j = 0; j < shape.GetLength(1); j++)
            {
                if (shape[i, j])
                {
                    block.Add(
                        new Vector2(i * (_BLOCKSIZE.X + _PADDING), j * (_BLOCKSIZE.Y + _PADDING))
                            + position
                    );
                }
            }
        }
        return block.ToArray();
    }

    private void DrawBlock(Vector2[] block, Color color)
    {
        foreach (Vector2 square in block)
        {
            _spriteBatch.Draw(_backgroundBlockTexture, square, color);
        }
    }

    private void DrawBackroundBoard()
    {
        Color backgroundBlockColor = new Color(32, 36, 69);
        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int j = 0; j < _board.GetLength(1); j++)
            {
                if (_board[i, j])
                {
                    _spriteBatch.Draw(_blockTexture, new Vector2(i, j), backgroundBlockColor);
                }
                else
                {
                    _spriteBatch.Draw(
                        _backgroundBlockTexture,
                        _backroundBlockPositions[i, j],
                        backgroundBlockColor
                    );
                }
            }
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(55, 75, 134));
        _spriteBatch.Begin();

        DrawBackroundBoard();
        DrawBlock(test.squarePositions, Color.Red);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
