using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Animation;

namespace AetherAnimationExample
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // AETHER.ANIMATION EXAMPLE CODE
        Model testModel;
        Animations testAnimations;
        Matrix testWorld = Matrix.Identity;
        Matrix testView = Matrix.CreateLookAt(
            new Vector3(0f, 35f, -100f),
            new Vector3(0f, 35f, 0f),
            Vector3.Up
        );
        Matrix testProjection;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // AETHER.ANIMATION EXAMPLE CODE
            float aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            testProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), aspectRatio, 0.01f, 500f);

            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // AETHER.ANIMATION EXAMPLE CODE
            testModel = Content.Load<Model>("animation_example/dude");
            testAnimations = testModel.GetAnimations();

            // you could use multiple clips in one file
            var clip = testAnimations.Clips["Take 001"];
            testAnimations.SetClip(clip);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // AETHER.ANIMATION EXAMPLE CODE
            testAnimations.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // AETHER.ANIMATION EXAMPLE CODE
            foreach (ModelMesh mesh in testModel.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    // for GPU animations
                    SkinnedEffect effect = (SkinnedEffect)part.Effect;
                    // for CPU animations
                    // BasicEffect effect = (BasicEffect)part.Effect;

                    // for GPU animations
                    effect.SetBoneTransforms(testAnimations.AnimationTransforms);
                    // for CPU animations
                    // part.UpdateVertices(testAnimations.AnimationTransforms);

                    effect.World = testWorld;
                    effect.View = testView;
                    effect.Projection = testProjection;
                    effect.SpecularColor = Vector3.Zero;
                    ApplyLighting(effect);
                }

                mesh.Draw();
            }

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        // AETHER.ANIMATION EXAMPLE CODE
        private void ApplyLighting(IEffectLights effect)
        {
            effect.EnableDefaultLighting();
            effect.DirectionalLight0.Direction = Vector3.Backward;
            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight1.Enabled = false;
            effect.DirectionalLight2.Enabled = false;
        }
    }
}
