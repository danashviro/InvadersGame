using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators
{
    public class ShrinkAnimator : SpriteAnimator
    {
        private float m_ShrinkVelocity;

        public ShrinkAnimator(string i_Name, TimeSpan i_AnimationLength) : base(i_Name, i_AnimationLength)
        {
            m_ShrinkVelocity = 1f / (float)i_AnimationLength.TotalSeconds;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            BoundSprite.Scales -= m_ShrinkVelocity * BoundSprite.Scales * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void RevertToOriginal()
        {
            BoundSprite.Scales = m_OriginalSpriteInfo.Scales;
        }
    }
}
