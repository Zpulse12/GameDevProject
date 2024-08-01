using Microsoft.Xna.Framework;

namespace NaamGaatNogKomen
{
    internal class Hitbox
    {
        public Rectangle rectangle;
        public Vector2 origin;

        private Rectangle originalRect;

        public Hitbox(Rectangle bounds, Vector2 origin)
        {
            this.origin = origin;
            rectangle = bounds;
            originalRect = rectangle;

            Update(Vector2.Zero);
        }

        public void Update(Vector2 position)
        {
            // Make the hitbox move as the entity moves
            rectangle = new Rectangle((position + (originalRect.Location.ToVector2() - origin)).ToPoint(), originalRect.Size.ToVector2().ToPoint());
        }

        public void MoveLeft(int amount)
        {
            rectangle.X -= amount;
        }

        // Check if two hitboxes intersect
        public bool IsTouching(Hitbox hitbox)
        {
            return rectangle.Intersects(hitbox.rectangle);
        }
    }
}