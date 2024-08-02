using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaamGaatNogKomen.Classes.Scripts.Hero;

namespace NaamGaatNogKomen.Classes.Scripts.Hero
{
    enum KnightMovementStates
    {
        Idle,
        Run,
        Jump,
        Fall,
        Attack,
        Taking_Damage,
        Dead,
        Shield_Blocking
    }

    enum KnightMovementDirection
    {
        Right,
        Left
    }

    internal class Knight
    {
        private float scale;
        private bool isJumping;
        private Vector2 position;
        private Vector2 velocity;
        private Texture2D texture;
        private KnightAnimation animation;
        private KnightMovementStates knightMovementStates;
        private KnightMovementDirection knightMovementDirection;


        public Knight()
        {
            velocity.X = 0;
            velocity.Y = 0;

            position.X = 20;
            position.Y = 20;

            knightMovementStates = KnightMovementStates.Idle;
            knightMovementDirection = KnightMovementDirection.Right;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("blackknight");


            animation = new KnightAnimation(texture);
        }

        public void Update(float deltaTime)
        {
            animation.position = position;
            animation.Update(deltaTime, knightMovementStates, knightMovementDirection);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}
