using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators
{
    public class RotateAnimator : SpriteAnimator
    {
        private float m_SpinsPerSecond;

        public RotateAnimator(string i_Name, TimeSpan i_AnimationLength, float i_SpinsPerSecond) : base(i_Name, i_AnimationLength)
        {
            m_SpinsPerSecond = i_SpinsPerSecond;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            BoundSprite.Rotation += m_SpinsPerSecond * (float)(i_GameTime.ElapsedGameTime.TotalSeconds * 2 * Math.PI);
        }

        protected override void RevertToOriginal()
        {
            BoundSprite.Rotation = m_OriginalSpriteInfo.Rotation;
        }
    }
}
