using System.Net.Http.Headers;
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
    private Vector2 _blockSize = new Vector2(50);

    //Board Data
    private bool[,] _board = new bool[8, 8];

    private Vector2[,] _backroundBlockPosition = new Vector2[8, 8];

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
    }

    private void BoardInitilizer(bool value, int padding)
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
            - (_blockSize.X + padding) * _backroundBlockPosition.GetLength(0) / 2;
        float centreYPad =
            Window.ClientBounds.Height / 4
            - (_blockSize.Y + padding) * _backroundBlockPosition.GetLength(1) / 2;
        for (int x = 0; x < _backroundBlockPosition.GetLength(0); x++)
        {
            for (int y = 0; y < _backroundBlockPosition.GetLength(1); y++)
            {
                _backroundBlockPosition[y, x] = new Vector2(
                    centreXPad + x * (_blockSize.X + padding),
                    centreYPad + y * (_blockSize.Y + padding)
                );
            }
        }
    }

    protected override void Initialize()
    {
        //Board Data

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _primitiveBatch = new PrimitiveBatch(GraphicsDevice);
        var rectangleTexture = new RectangleTexture();

        //Textures
        // _blockTexture = Content.Load<Texture2D>("block");
        _backgroundBlockTexture = rectangleTexture.CreateTexture(_blockSize, _primitiveBatch);
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();
        
        BoardInitilizer(false, 10);


        // TODO: Add your update logic here

        base.Update(gameTime);
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
                        _backroundBlockPosition[i, j],
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

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
