using System;
using System.Collections.Generic;
using System.Linq;
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
    private const float _PLATERADIUS = 100;
    private const float _PADDING = 5;

    //Board Data
    private bool[,] _board = new bool[8, 8];

    //blocks
    private BlockLayout test = new BlockLayout(new Vector2(0, 0), L_Shape.shape, Color.Red);

    private List<Square> _boardBlocks = new List<Square>();
    private BlockLayout[] _pickBlocks = new BlockLayout[3];

    private Vector2[,] _backroundBlockPositions = new Vector2[8, 8];
    private PrimitiveBatch.Circle[] blockSpawnPlates = new PrimitiveBatch.Circle[3];

    //general control dataa

    private Vector2 _previousMousePosition;
    private bool _isDragging = false;
    private int _draggingBlockIndex = -1;
    Random rnd = new Random();

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
        for (int i = 0; i < blockSpawnPlates.Length; i++)
        {
            blockSpawnPlates[i] = new PrimitiveBatch.Circle(
                new Vector2(
                    _backroundBlockPositions[0, 0].X + _PLATERADIUS + i * (_PLATERADIUS + 30),
                    centreYPad
                        + _backroundBlockPositions.GetLength(1) * (_BLOCKSIZE.Y + padding)
                        + 60
                ),
                _PLATERADIUS,
                Color.SkyBlue
            );
        }
    }

    protected override void Initialize()
    {
        //Board Data
        test.position = new Vector2(0, 0);
        BoardInitilizer(false, _PADDING);
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
        Vector2 mouseDelta = currentMousePosition - _previousMousePosition;

        test.squarePositions = BuildBlock(test.position, L_Shape.shape);

        if (_pickBlocks.All(ns => ns == null) || _pickBlocks.All(ns => !ns.isdragable))
        {
            _pickBlocks[0] = new BlockLayout(new Vector2(0, 0), L_Shape.shape, RandomColour());
            _pickBlocks[1] = new BlockLayout(new Vector2(0, 0), T_Shape.shape, RandomColour());
            _pickBlocks[2] = new BlockLayout(new Vector2(0, 0), i_Shape.shape, RandomColour());

            _pickBlocks[2].shape[2, 0] = false;
            for (int i = 0; i < _pickBlocks.Length; i++)
            {
                _pickBlocks[i].squarePositions = BuildBlock(
                    _pickBlocks[i].position,
                    _pickBlocks[i].shape
                );
            }
            _pickBlocks[1].squarePositions = BuildBlock(
                _pickBlocks[1].position,
                _pickBlocks[1].shape
            );
        }

        for (int i = 0; i < _pickBlocks.Length; i++)
        {
            if (!_pickBlocks[i].isdragable)
            {
                for (int j = 0; j < _pickBlocks[i].squarePositions.Length; j++)
                {
                    _boardBlocks.Add(
                        new Square(_pickBlocks[i].squarePositions[j], _pickBlocks[i].color)
                    );
                }
                // _pickBlocks[i] = new BlockLayout(new Vector2(0, 0), L_Shape.shape, RandomColour());
            }
        }

        if (!_isDragging && mouseState.LeftButton == ButtonState.Pressed)
        {
            for (int i = 0; i < _pickBlocks.Length; i++)
            {
                _pickBlocks[i].squarePositions = BuildBlock(
                    _pickBlocks[i].position,
                    _pickBlocks[i].shape
                );
                for (int j = 0; j < _pickBlocks[i].squarePositions.Length; j++)
                {
                    if (
                        IsmouseOverRectangle(
                            _pickBlocks[i].squarePositions[j],
                            _BLOCKSIZE,
                            mouseState
                        )
                    )
                    {
                        _isDragging = true;
                        _draggingBlockIndex = i;
                        break;
                    }
                }
            }
        }
        else if (
            _isDragging
            && mouseState.LeftButton == ButtonState.Pressed
            && _pickBlocks[_draggingBlockIndex].isdragable
        )
        {
            _pickBlocks[_draggingBlockIndex].position += mouseDelta;
            _pickBlocks[_draggingBlockIndex].squarePositions = BuildBlock(
                _pickBlocks[_draggingBlockIndex].position,
                _pickBlocks[_draggingBlockIndex].shape
            );
        }
        else if (_isDragging && mouseState.LeftButton == ButtonState.Released)
        {
            _isDragging = false;
            LockToGrid(ref _pickBlocks[_draggingBlockIndex]);
        }

        _previousMousePosition = currentMousePosition;

        CheckForFullRow();
        CheckForFullColumn();
        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int j = 0; j < _board.GetLength(1); j++)
            {
                if (!_board[i, j])
                {
                    _boardBlocks.RemoveAll(block =>
                        block.position == _backroundBlockPositions[i, j]
                    );
                }
            }
        }

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    private void CheckForFullRow()
    {
        for (int row = 0; row < _board.GetLength(0); row++)
        {
            bool fullRow = true;
            for (int col = 0; col < _board.GetLength(1); col++)
            {
                if (!_board[row, col])
                {
                    fullRow = false;
                    break;
                }
            }

            if (fullRow)
            {
                // Clear the row in the board
                for (int col = 0; col < _board.GetLength(1); col++)
                {
                    _board[row, col] = false;
                }

                // Remove blocks in this row from _boardBlocks
                _boardBlocks.RemoveAll(block =>
                {
                    for (int i = 0; i < _backroundBlockPositions.GetLength(0); i++)
                    {
                        for (int j = 0; j < _backroundBlockPositions.GetLength(1); j++)
                        {
                            if (_backroundBlockPositions[i, j] == block.position && i == row)
                            {
                                return true; // Mark this block for removal
                            }
                        }
                    }

                    return false;
                });
            }
        }
    }

    private void CheckForFullColumn()
    {
        for (int col = 0; col < _board.GetLength(1); col++)
        {
            bool fullColumn = true;
            for (int row = 0; row < _board.GetLength(0); row++)
            {
                if (!_board[row, col])
                {
                    fullColumn = false;
                    break;
                }
            }

            if (fullColumn)
            {
                // Clear the column in the board
                for (int row = 0; row < _board.GetLength(0); row++)
                {
                    _board[row, col] = false;
                }

                _boardBlocks.RemoveAll(block =>
                {
                    for (int i = 0; i < _backroundBlockPositions.GetLength(0); i++)
                    {
                        for (int j = 0; j < _backroundBlockPositions.GetLength(1); j++)
                        {
                            if (_backroundBlockPositions[i, j] == block.position && j == col)
                            {
                                return true;
                            }
                        }
                    }

                    return false;
                });
            }
        }
    }

    private Color RandomColour()
    {
        return new Color(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
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
                if (
                    Vector2.Distance(square, gridPosition) < 16
                    && !IsPositionOccupied(gridPosition)
                )
                {
                    newsquares[i] = gridPosition;
                    sqrcheck++;
                    break;
                }
            }
        }

        if (sqrcheck == block.squarePositions.Length)
        {
            block.position = newsquares[0];
            block.isdragable = false;
            block.squarePositions = newsquares;

            // Update the board state for this block.
            foreach (var square in block.squarePositions)
            {
                for (int i = 0; i < _backroundBlockPositions.GetLength(0); i++)
                {
                    for (int j = 0; j < _backroundBlockPositions.GetLength(1); j++)
                    {
                        if (_backroundBlockPositions[i, j] == square)
                        {
                            _board[i, j] = true; // Mark as occupied
                            break;
                        }
                    }
                }
            }

            // Add the squares to _boardBlocks only once
            foreach (var square in block.squarePositions)
            {
                // Optionally check if not already present.
                if (!_boardBlocks.Any(s => s.position == square))
                {
                    _boardBlocks.Add(new Square(square, block.color));
                }
            }
        }
    }

    private bool IsPositionOccupied(Vector2 position)
    {
        foreach (Square block in _boardBlocks)
        {
            if (block.position == position)
            {
                return true;
            }
        }

        foreach (var block in _pickBlocks)
        {
            if (block != null && !block.isdragable)
            {
                foreach (var square in block.squarePositions)
                {
                    if (square == position)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void PrintBoolArray(bool[,] array)
    {
        string border = new string('-', array.GetLength(0));
        Console.WriteLine(border);
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                Console.Write(array[i, j] ? "1" : "0");
            }
            Console.WriteLine();
        }
        Console.WriteLine(border);
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

    private void DrawSquares(List<Square> squares)
    {
        foreach (Square square in squares)
        {
            _spriteBatch.Draw(
                _blockTexture,
                new Rectangle(
                    (int)square.position.X,
                    (int)square.position.Y,
                    (int)_BLOCKSIZE.X,
                    (int)_BLOCKSIZE.Y
                ),
                square.color
            );
        }
    }

    private void DrawBlock(Vector2[] block, Color color)
    {
        foreach (Vector2 square in block)
        {
            _spriteBatch.Draw(
                _blockTexture,
                new Rectangle((int)square.X, (int)square.Y, (int)_BLOCKSIZE.X, (int)_BLOCKSIZE.Y),
                color
            );
        }
    }

    private void DrawBackroundBoard()
    {
        Color backgroundBlockColor = new Color(32, 36, 69);
        PrintBoolArray(_board);

        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int j = 0; j < _board.GetLength(1); j++)
            {
                if (_board[i, j])
                {
                    _spriteBatch.Draw(
                        _backgroundBlockTexture,
                        _backroundBlockPositions[i, j],
                        Color.White
                    );
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
        foreach (PrimitiveBatch.Circle circle in blockSpawnPlates)
        {
            circle.Draw(_spriteBatch, _primitiveBatch);
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(55, 75, 134));
        _spriteBatch.Begin();

        DrawBackroundBoard();
        // DrawBlock(test.squarePositions, Color.Red);

        DrawSquares(_boardBlocks);

        foreach (BlockLayout block in _pickBlocks)
        {
            if (block.isdragable)
                DrawBlock(BuildBlock(block.position, block.shape), block.color);
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
