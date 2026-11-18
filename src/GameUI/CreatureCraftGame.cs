using System.Collections.Generic;
using CraftingCreatureWorld.Core;
using CraftingCreatureWorld.GameUI.Screens;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CraftingCreatureWorld.GameUI
{
    /// <summary>
    /// MonoGame entry point. Owns the screen stack, feeds input to the current
    /// screen each frame and forwards logic-layer toasts to the on-screen queue.
    /// </summary>
    public sealed class CreatureCraftGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch = null!;
        private Renderer _renderer = null!;
        private readonly InputState _input = new();
        private readonly ToastQueue _toasts = new();
        private ScreenContext _ctx = null!;

        private readonly Stack<IScreen> _screens = new();
        private IScreen Current => _screens.Peek();

        public CreatureCraftGame()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 760,
                SynchronizeWithVerticalRetrace = true
            };
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // NOTE: everything this override needs lives in the field
            // initializers / constructor, so build the context BEFORE calling
            // base.Initialize(). XNA calls LoadContent() from within
            // base.Initialize(), and doing our setup first means LoadContent
            // can never observe a partially-initialized game (this previously
            // surfaced as hard-to-diagnose null crashes at startup).
            _ctx = new ScreenContext(new GameLogic("Adventurer"), _input, _toasts);
            _ctx.Navigate = Push;
            _ctx.CloseModal = () => { if (_screens.Count > 1) _screens.Pop(); };

            UiEvents.Toast += (msg, kind) => _toasts.Push(msg, kind);
            GameOverScreen.RequestExit = Exit;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            if (GraphicsDevice is null)
                throw new InvalidOperationException(
                    "The GraphicsDevice is null when LoadContent() runs. This usually " +
                    "means base.Initialize() was skipped in the debugger - restart the " +
                    "debug session without stepping over Initialize()/LoadContent().");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            SpriteFont uiFont, uiBoldFont;
            try
            {
                uiFont = Content.Load<SpriteFont>("Fonts/UI");
                uiBoldFont = Content.Load<SpriteFont>("Fonts/UIBold");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Failed to load UI fonts. Make sure the MonoGame content " +
                    "pipeline ran and Content/Fonts/UI.xnb & UIBold.xnb exist " +
                    $"next to the executable (looked in: '{Content.RootDirectory}').", ex);
            }

            _renderer = new Renderer(_spriteBatch, pixel, uiFont, uiBoldFont);
            _ctx.Renderer = _renderer;

            _screens.Push(new TitleScreen());
        }

        protected override void Update(GameTime gameTime)
        {
            // Esc-to-quit is handled per-screen; the window close button works too.
            _input.Update();

            if (_ctx.Logic.IsGameOver && Current is not GameOverScreen && Current is not TitleScreen)
                _screens.Push(new GameOverScreen(_ctx.Logic.GameOverReason ?? "Game over"));

            Current.Update(_ctx);
            _toasts.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Theme.Background);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);

            Current.Draw(_ctx);
            _toasts.Draw(_renderer);

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>Replaces the current screen with a new one (bounded stack so
        /// repeated navigation can't grow memory forever).</summary>
        private void Push(IScreen screen)
        {
            if (_screens.Count >= 32)
            {
                var rest = new List<IScreen>(_screens);
                _screens.Clear();
                for (int i = 0; i < Math.Min(16, rest.Count); i++)
                    _screens.Push(rest[i]);
            }
            _screens.Push(screen);
        }
    }

    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using var game = new CreatureCraftGame();
            game.Run();
        }
    }
}
