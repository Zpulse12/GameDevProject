using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaamGaatNogKomen.Classes.Interfaces;
using NaamGaatNogKomen.Classes.Scripts;
using NaamGaatNogKomen.Classes.Scripts.Managers;

namespace NaamGaatNogKomen.Classes.Scripts.Enemies
{
    internal abstract class Enemy
    {
        public Hitbox hitbox;

        protected float timer; //this is a timer used for the enemy animation

        protected bool movingLeft;
        protected Vector2 position;
        protected Vector2 displacement;

        protected Vector2 currentFrame; //this is the current frame the animation is using from the sprite sheet
        protected Rectangle sourceRect; //this is used to select a portion of the spritesheet
        protected SpriteEffects spriteEffects; // this can flip the animation horizontally or vertically

        protected int monsterFrameCount;
        protected Vector2 frameSize; // width, height

        protected readonly float animationDuration = 0.2f; //this is the time interval between frames of the animation
        protected readonly Vector2 velocity = new Vector2(25, 20) * GameManager.gameScale;

        public Enemy(Vector2 position, Rectangle hitboxData)
        {
            movingLeft = true;
            this.position = position;
            displacement = Vector2.Zero;

            timer = 0;
            currentFrame = Vector2.Zero;

            hitbox = new Hitbox(hitboxData, Vector2.Zero);
            hitbox.Update(position);
        }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(float deltaTime, Vector2 knightPos);
        public abstract void PlayAnimation(float deltaTime);
    }
}
