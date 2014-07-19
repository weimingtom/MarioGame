﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0Game
{
    public class FallingMario : IAnimatedMario
    {
        private Texture2D Texture;
        private int Frame;
        private IMario Mario;
        private SpriteEffects Flip;
        private Color Color = Color.White;
        private Color OriginalColor = Color.White;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public FallingMario(IMario mario, bool rightFacing, MarioPowerLevel powerLevel)
        {
            switch (powerLevel)
            {
                case MarioPowerLevel.Fire:
                case MarioPowerLevel.Metal:
                    this.Texture = SpriteHolder.FireMario;
                    break;
                case MarioPowerLevel.Big:
                    this.Texture = SpriteHolder.BigMario;
                    break;
                case MarioPowerLevel.Small:
                    this.Texture = SpriteHolder.SmallMario;
                    break;
            }

            if (powerLevel == MarioPowerLevel.Metal)
            {
                this.Color = MarioConfig.MetalMarioColor;
                OriginalColor = this.Color;
            }

            this.Mario = mario;
            this.Frame = SpriteHolder.JumpingMarioFrame;

            if (rightFacing)
            {
                this.Flip = SpriteEffects.None;
            }
            else
            {
                this.Flip = SpriteEffects.FlipHorizontally;
            }

            if (MarioPowerLevelGeneralizer.IsBig(powerLevel))
                this.Width = SpriteHolder.BigMarioWidth;
            else
                this.Width = SpriteHolder.SmallMarioWidth;
            this.Height = this.Texture.Height;
        }

        public void Update()
        {
            this.Mario.CurrentPosition = new Vector2(this.Mario.CurrentPosition.X + this.Mario.CurrentVelocity.X, this.Mario.CurrentPosition.Y + this.Mario.CurrentVelocity.Y);
            this.Mario.CurrentVelocity = new Vector2(this.Mario.CurrentVelocity.X, this.Mario.CurrentVelocity.Y + PhysicsConfig.Gravity);

            if (this.Mario.IsStar)
            {
                this.Color = Color.Yellow;
            }
            else
            {
                this.Color = OriginalColor;
            }
            if (this.Mario.IsTransitioningFromDamage)
            {
                this.Flash(this.Mario.TransitionFromDamageTimer);
            }
        }

        public void Flash(int timer)
        {
            if (timer % MarioConfig.FlashStepPeriod == 1)
            {
                this.Color = Color.Black;
            }
            else
            {
                this.Color = OriginalColor;
            }
        }

        public void Draw(SpriteBatch spriteBatch, ICamera camera)
        {
            Rectangle sourceRectangle = new Rectangle(this.Width * this.Frame, 0, this.Width, this.Height);
            Rectangle drawnRectangle = new Rectangle((int)(this.Mario.CurrentPosition.X - camera.CurrentPosition.X),
                (int)this.Mario.CurrentPosition.Y - this.Height, this.Width, this.Height);

            spriteBatch.Draw(this.Texture, drawnRectangle, sourceRectangle, this.Color, 0, Vector2.Zero, this.Flip, 0);   
        
        }

    }
}
