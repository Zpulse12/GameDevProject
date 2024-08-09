using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NaamGaatNogKomen.Classes.Scripts.Enemies
{
    internal class Monster3
    {
        public Hitbox hitbox;
        public Projectile projectile;

        private float timer; //this is a timer used for the coins animation

        private bool movingLeft;
        private bool movingDown;
        private Vector2 position;
        private Vector2 displacment;

        private Vector2 currentFrame; //this is the current frame the animation is using from the sprite sheet
        private Rectangle sourceRect; //this is used to select a portion of the spritesheet
        private SpriteEffects spriteEffects; // this can flip the animation horizontally or vertically


        private readonly int maxXDisplacment = (int)(12 * MapGenerator.tileSize * GameManager.gameScale);
        private readonly int maxYDisplacment = (int)(3 * MapGenerator.tileSize * GameManager.gameScale);
        private readonly int MonsterFrameCount = 8;
        private readonly float animationDuration = 0.2f; //this is the time interval between frames of the animation
        private readonly Vector2 frameSize = new Vector2(48, 55); // w, h
        private readonly Vector2 velocity = new Vector2(25 * GameManager.gameScale, 20 * GameManager.gameScale);

        public Monster3(Vector2 position)
        {
            movingLeft = true;
            this.position = position;
            displacment = Vector2.Zero;

            timer = 0;
            currentFrame = Vector2.Zero;

            hitbox = new Hitbox(new Rectangle(0, (int)(7 * GameManager.gameScale), (int)(37 * GameManager.gameScale), (int)(30 * GameManager.gameScale)), Vector2.Zero);
            hitbox.Update(position);

            projectile = new Projectile(position);//(new Vector2(2 * MapGenerator.tileSize, 2 * MapGenerator.tileSize));
        }


        public void Update(float deltaTime)
        {
            // movement handling

            //// Left/Right movement
            //if (movingLeft)
            //{
            //	if (displacment.X <= -maxXDisplacment)
            //		movingLeft = false;
            //	else
            //		displacment.X -= velocity.X * deltaTime;
            //}
            //else
            //{
            //	if (displacment.X >= 0)
            //		movingLeft = true;
            //	else
            //		displacment.X += velocity.X * deltaTime;
            //}
            //hitbox.Update(position + displacment);


            // animation handling
            if (timer >= animationDuration) // time interval between frames
            {
                currentFrame.X = currentFrame.X + 1 >= MonsterFrameCount / 2 ? 0 : currentFrame.X + 1;

                if (currentFrame.X == 0)
                    currentFrame.Y = currentFrame.Y + 1 >= 2 ? 0 : currentFrame.Y + 1;

                timer = 0;
            }
            timer += deltaTime;

            sourceRect = new Rectangle(
                                (int)(1 + currentFrame.X * (frameSize.X + 1)),
                                (int)(1 + currentFrame.Y * (frameSize.Y + 1)),
                                (int)frameSize.X,
                                (int)frameSize.Y);

            if (!movingLeft)
                spriteEffects = SpriteEffects.FlipHorizontally;
            else
                spriteEffects = SpriteEffects.None;

            if (projectile.Disapeared())
                projectile = new Projectile(position + new Vector2(6 * GameManager.gameScale, 21 * GameManager.gameScale));

            projectile.Update(deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Texture2D projectileTexture)
        {
            projectile.Draw(spriteBatch, projectileTexture);
            spriteBatch.Draw(texture, position + displacment, sourceRect, Color.White, 0, Vector2.Zero, 0.75f * GameManager.gameScale, spriteEffects, 0);
        }

        public void MoveLeft(int amount)
        {
            position.X -= amount;
            projectile.MoveLeft(amount);
        }
    }
}
