using System.Net.Http.Headers;
using Microsoft.Xna.Framework;
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

    //Board Data
    private bool[,] _board = new bool[8, 8];

    private Vector2[,] _backroundBlockPosition = new Vector2[8, 8];

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    private void BoardInitilizer(bool value)
    {
        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int j = 0; j < _board.GetLength(1); j++)
            {
                _board[i, j] = value;
            }
        }
        for (int i = 0; i < _backroundBlockPosition.GetLength(0); i++)
        {
            for (int j = 0; j < _backroundBlockPosition.GetLength(1); j++)
            {
                _backroundBlockPosition[i, j] = new Vector2(i * 50, j * 50);
            }
        }
    }

    protected override void Initialize()
    {
        //Board Data
        BoardInitilizer(false);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _primitiveBatch = new PrimitiveBatch(GraphicsDevice);
        var rectangleTexture = new RectangleTexture();

        //Textures
        // _blockTexture = Content.Load<Texture2D>("block");
        _backgroundBlockTexture = rectangleTexture.CreateTexture(
            new Vector2(50, 50),
            _primitiveBatch
        );
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();

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
                    _spriteBatch.Draw(
                        _blockTexture,
                        new Vector2(i * 50, j * 50),
                        backgroundBlockColor
                    );
                }
                else
                {
                    _spriteBatch.Draw(
                        _backgroundBlockTexture,
                        new Vector2(i * 50, j * 50),
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
